using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pachyderm_Noise_Control
{
    public class Duct_Model
    {
        List<List<node>> Nodes = new List<List<node>>();
        List<Noise_Source> Noise_Sources = new List<Noise_Source>();
        public bool Connected = true;
        public double delta_D = 0.3048; //Meters
        private static Duct_Model instance = new Duct_Model();

        private Duct_Model()
        {
        }

        public static Duct_Model Instance
        {
            get
            {
                if (instance == null) instance = new Duct_Model();
                return instance;
            }
        }

        public void reset()
        {
            Nodes = new List<List<node>>();
            Noise_Sources = new List<Noise_Source>();
        }

        public int Intiate_Path(Hare.Geometry.Point Origin, double[] Noise, NoiseType type)
        {
            Nodes.Add(new List<node>());
            int path_id = Nodes.Count - 1;
            Nodes[path_id].Add(new node(new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 }, Origin, Nodes[path_id].Count));
            Noise_Sources.Add(new Noise_Source(Noise, Nodes[path_id][Nodes[path_id].Count - 1], type));
            return path_id;
        }

        public int AddComponent(int StartingNode, double[] Total_Attenuation, Hare.Geometry.Point[] Points, ref State S)
        {
            double[] atten = new double[8];
            for (int i = 0; i < 8; i++) atten[i] = Total_Attenuation[i] / Points.Length;
            Nodes[S.PathId].Add(new node(atten, Points[0], Nodes[S.PathId].Count, Nodes[S.PathId][StartingNode]));

            for (int i = 1; i < Points.Length; i++)
            {
                Nodes[S.PathId].Add(new node(atten, Points[i], Nodes[S.PathId].Count, Nodes[S.PathId][Nodes[S.PathId].Count - 1]));
            }

            S.ModelNode = Nodes[S.PathId].Count - 1;

            return Nodes[S.PathId].Count - 1;
        }

        public int AddComponent(int StartingNode, double[] Total_Attenuation, double[] Noise, NoiseType Type, Hare.Geometry.Point[] Points, ref State S)
        {
            int id = AddComponent(StartingNode, Total_Attenuation, Points, ref S);
            Noise_Sources.Add(new Noise_Source(Noise, Nodes[S.PathId][id], Type));
            return id;
        }

        public int[] AddComponent(int StartingNode, double[][] Total_Attenuation, double[][] Noise, NoiseType Type, Hare.Geometry.Point[][] Points, ref State[] S)
        {
            if (Total_Attenuation.Length != Noise.Length || Noise.Length != Points.Length || Points.Length != S.Length) throw new Exception("Number of attenuation, regen and point paths must match.");
            int[] ids = new int[Total_Attenuation.Length];
            for (int i = 0; i < Total_Attenuation.Length; i++) ids[i] = AddComponent(StartingNode, Total_Attenuation[i], Noise[i], Type, Points[i], ref S[i]);
            return ids;
        }

        public void Propagate_Noise_Sources()
        {
            foreach (Noise_Source N in Noise_Sources)
            {
                N.Propagate_Sound();
            }
        }

        public void WireDiagram(out List<Hare.Geometry.Point[]> WF, out List<double[]> SPL, NoiseType type)
        {
            WF = new List<Hare.Geometry.Point[]>();
            SPL = new List<double[]>();

            for (int i = 0; i < Nodes.Count; i++)
            {
                foreach (node start in Nodes[i])
                {
                    for (int j = 1; j < start.Neighbors.Count; j++)//(node end in start.Neighbors)
                    {
                        WF.Add(new Hare.Geometry.Point[2] { start.loc, start.Neighbors[j].loc });
                        SPL.Add(Sound_Pressure_Level(type, start));
                    }
                }
             }
        }

        public double[] Sound_Pressure_Level(NoiseType type, int PathId, int Node_index)
        {
            //Upon display, sometimes resets.
            return Sound_Pressure_Level(type, Nodes[PathId][Node_index]);
        }

        private double[] Sound_Pressure_Level(NoiseType type, node N)
        {
            double[] spl = new double[8];
            switch (type)
            {
                case NoiseType.Equipment:
                    spl = N.SPL_Equipment;
                    break;
                case NoiseType.Aerodynamic:
                    spl = N.SPL_Aerodynamic;
                    break;
                default:
                    for (int oct = 0; oct < 8; oct++) spl[oct] = 10 * Math.Log10(Math.Pow(10f, N.SPL_Aerodynamic[oct] / 10) + Math.Pow(10f, N.SPL_Equipment[oct] / 10));
                    break;
            }
            return spl;
        }

        public Hare.Geometry.Point Location(int nodeindex, int Pathid)
        {
            return Nodes[Pathid][nodeindex].loc;
        }

        public void Add_Attenuation(int node_id, int path_id, double[] Atten)
        {
            double[] atten = new double[8];
            for (int i = 0; i < 8; i++) atten[i] = Nodes[path_id][node_id].Atten[i] + Math.Abs(Atten[i]);
            Nodes[path_id][node_id].Atten = atten;
        }

        class node
        {
            public double[] Atten;
            public int node_id;
            public double[] SPL_Equipment = new double[8];
            public double[] SPL_Aerodynamic = new double[8];
            public Hare.Geometry.Point loc;
            public List<node> Neighbors = new List<node>();

            public node(double[] Attenuation, Hare.Geometry.Point p, int id)
            {
                node_id = id;
                Atten = Attenuation;
                loc = p;
            }

            public node(double[] Attenuation, Hare.Geometry.Point p, int id, node neighbor)
                : this(Attenuation, p, id)
            {
                connect(neighbor);
            }

            public void connect(node neighbor)
            {
                Neighbors.Add(neighbor);
                neighbor.Neighbors.Add(this);
            }

            public void propagate(double[] noise, NoiseType type, int last_id)
            {
                double[] new_noise = new double[8];
                for (int oct = 0; oct < 8; oct++) new_noise[oct] = noise[oct] - Atten[oct];

                if (type == NoiseType.Equipment)
                {
                    for (int oct = 0; oct < 8; oct++) SPL_Equipment[oct] = 10 * Math.Log10(Math.Pow(10f, SPL_Equipment[oct] / 10) + Math.Pow(10f, (new_noise[oct]) / 10));
                }
                else if (type == NoiseType.Aerodynamic)
                {
                    for (int oct = 0; oct < 8; oct++) SPL_Aerodynamic[oct] = 10 * Math.Log10(Math.Pow(10f, SPL_Aerodynamic[oct] / 10) + Math.Pow(10f, (new_noise[oct]) / 10));
                }

                foreach (node n in Neighbors)
                {
                    if (n.node_id != last_id) n.propagate(new_noise, type, this.node_id);
                }//TODO: Add system to prevent sound from rolling back on itself.
            }
        }

        class Noise_Source
        {
            node origin;
            double[] SWL;
            NoiseType Type;

            public Noise_Source(double[] Noise, node Origin, NoiseType type)
            {
                SWL = Noise;
                origin = Origin;
                Type = type;
            }

            public void Propagate_Sound()
            {
                if (Type == NoiseType.Equipment)
                {
                    for (int oct = 0; oct < 8; oct++) origin.SPL_Equipment[oct] = 10 * Math.Log10(Math.Pow(10f, origin.SPL_Equipment[oct] / 10) + Math.Pow(10f, (SWL[oct]) / 10));
                }
                else if (Type == NoiseType.Aerodynamic)
                {
                    for (int oct = 0; oct < 8; oct++) origin.SPL_Aerodynamic[oct] = 10 * Math.Log10(Math.Pow(10f, origin.SPL_Aerodynamic[oct] / 10) + Math.Pow(10f, (SWL[oct]) / 10));
                }

                foreach (node n in origin.Neighbors)
                {
                    n.propagate(SWL, Type, origin.node_id);
                }
            }
        }

        public enum NoiseType
        {
            Equipment,
            Aerodynamic,
            Total
        }
    }
}