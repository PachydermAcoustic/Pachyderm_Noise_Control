using Pachyderm_Noise_Control.ASHRAE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Pachyderm_Noise_Control
{
    public class Duct_Model
    {
        List<List<node>> Nodes = new List<List<node>>();
        List<List<int[]>> DisplayEdges = new List<List<int[]>>();
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
            DisplayEdges = new List<List<int[]>>();
            Noise_Sources = new List<Noise_Source>();
        }

        public int Intiate_Path(Hare.Geometry.Point Origin, double[] Noise, NoiseType type, double width = 0, double height = 0)
        {
            Nodes.Add(new List<node>());
            DisplayEdges.Add(new List<int[]>());
            int path_id = Nodes.Count - 1;
            Nodes[path_id].Add(new node(new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 }, Origin, Nodes[path_id].Count, width, height));
            Noise_Sources.Add(new Noise_Source(Noise, Nodes[path_id][Nodes[path_id].Count - 1], type));
            return path_id;
        }

        public int AddComponent(int StartingNode, double[] Total_Attenuation, Hare.Geometry.Point[] Points, double width, double height, ref State S)
        {
            if (Points == null || Points.Length == 0)
            {
                S.ModelNode = StartingNode;
                return StartingNode;
            }

            double[] atten = new double[8];
            for (int i = 0; i < 8; i++) atten[i] = Total_Attenuation[i] / Points.Length;

            if (DisplayEdges.Count <= S.PathId)
            {
                while (DisplayEdges.Count <= S.PathId) DisplayEdges.Add(new List<int[]>());
            }

            int newNodeId = Nodes[S.PathId].Count;
            Nodes[S.PathId].Add(new node(atten, Points[0], newNodeId, Nodes[S.PathId][StartingNode], width, height));
            DisplayEdges[S.PathId].Add(new int[2] { StartingNode, newNodeId });

            for (int i = 1; i < Points.Length; i++)
            {
                int previousNodeId = Nodes[S.PathId].Count - 1;
                newNodeId = Nodes[S.PathId].Count;
                Nodes[S.PathId].Add(new node(atten, Points[i], newNodeId, Nodes[S.PathId][previousNodeId], width, height));
                DisplayEdges[S.PathId].Add(new int[2] { previousNodeId, newNodeId });
            }

            S.ModelNode = Nodes[S.PathId].Count - 1;

            return Nodes[S.PathId].Count - 1;
        }

        public int AddComponent(int StartingNode, double[] Total_Attenuation, double[] Noise, NoiseType Type, Hare.Geometry.Point[] Points, double width, double height, ref State S)
        {
            int id = AddComponent(StartingNode, Total_Attenuation, Points, width, height, ref S);
            Noise_Sources.Add(new Noise_Source(Noise, Nodes[S.PathId][id], Type));
            return id;
        }

        public int[] AddComponent(int StartingNode, double[][] Total_Attenuation, double[][] Noise, NoiseType Type, Hare.Geometry.Point[][] Points, double width, double height, ref State[] S)
        {
            if (Total_Attenuation.Length != Noise.Length || Noise.Length != Points.Length || Points.Length != S.Length) throw new Exception("Number of attenuation, regen and point paths must match.");
            int[] ids = new int[Total_Attenuation.Length];
            for (int i = 0; i < Total_Attenuation.Length; i++) ids[i] = AddComponent(StartingNode, Total_Attenuation[i], Noise[i], Type, Points[i], width, height, ref S[i]);
            return ids;
        }

        public void Propagate_Noise_Sources()
        {
            foreach (Noise_Source N in Noise_Sources)
            {
                N.Propagate_Sound();
            }
        }

        public void WireDiagram(out List<Hare.Geometry.Point> DuctCenters, out List<List<Hare.Geometry.Point[]>> DuctWallcollection, out List<double[]> TSPL, out List<double[]> ESPL, out List<double[]> ASPL, out List<double[]> Breakout)
        {
            List<bool> Transition;
            WireDiagram(out DuctCenters, out DuctWallcollection, out TSPL, out ESPL, out ASPL, out Breakout, out Transition);
        }

        public void WireDiagram(out List<Hare.Geometry.Point> DuctCenters, out List<List<Hare.Geometry.Point[]>> DuctWallcollection, out List<double[]> TSPL, out List<double[]> ESPL, out List<double[]> ASPL, out List<double[]> Breakout, out List<bool> Transition)
        {
            DuctCenters = new List<Hare.Geometry.Point>();
            TSPL = new List<double[]>();
            ESPL = new List<double[]>();
            ASPL = new List<double[]>();
            Breakout = new List<double[]>();
            DuctWallcollection = new List<List<Hare.Geometry.Point[]>>();
            Transition = new List<bool>();

            int numSegments = 16;
            const double epsilon = 1e-9;
            const double inch = 0.0254;

            for (int i = 0; i < Nodes.Count; i++)
            {
                List<node> pathNodes = Nodes[i];
                if (pathNodes.Count < 2) continue;

                // Older models may not have had DisplayEdges. Fall back to the original sequential behavior,
                // but new/normal models use explicit creation edges so junction branches do not cross-connect.
                List<int[]> edges;
                if (DisplayEdges.Count > i && DisplayEdges[i].Count > 0)
                {
                    edges = DisplayEdges[i];
                }
                else
                {
                    edges = new List<int[]>();
                    for (int j = 1; j < pathNodes.Count; j++) edges.Add(new int[2] { j - 1, j });
                }

                for (int j = 0; j < edges.Count; j++)
                {
                    int start_id = edges[j][0];
                    int end_id = edges[j][1];
                    if (start_id < 0 || end_id < 0 || start_id >= pathNodes.Count || end_id >= pathNodes.Count) continue;

                    node prevNode = pathNodes[start_id];
                    node currentNode = pathNodes[end_id];

                    Hare.Geometry.Vector segmentDirection = currentNode.loc - prevNode.loc;
                    double segLen2 = Hare.Geometry.Hare_math.Dot(segmentDirection, segmentDirection);
                    if (segLen2 < epsilon) continue;
                    double segLen = Math.Sqrt(segLen2);
                    segmentDirection.Normalize();

                    Hare.Geometry.Vector right, up;
                    up = new Hare.Geometry.Vector(0, 0, 1);
                    if (Math.Abs(Hare.Geometry.Hare_math.Dot(segmentDirection, up)) > 0.99) up = new Hare.Geometry.Vector(1, 0, 0);
                    right = Hare.Geometry.Hare_math.Cross(segmentDirection, up);
                    up = Hare.Geometry.Hare_math.Cross(right, segmentDirection);
                    right.Normalize();
                    up.Normalize();

                    List<Hare.Geometry.Point[]> DuctWalls = new List<Hare.Geometry.Point[]>();

                    double w1 = prevNode.Width * inch;
                    double h1 = prevNode.Height * inch;
                    double w2 = currentNode.Width * inch;
                    double h2 = currentNode.Height * inch;

                    // Equipment/source nodes often have no size. For display only, borrow the adjacent duct size.
                    // Do not write this back to the node, or junction states will be changed.
                    if (w1 == 0 && h1 == 0 && (w2 > 0 || h2 > 0))
                    {
                        w1 = w2;
                        h1 = h2;
                    }
                    if (w2 == 0 && h2 == 0 && (w1 > 0 || h1 > 0))
                    {
                        w2 = w1;
                        h2 = h1;
                    }

                    bool prevIsRectangular = w1 > 0 && h1 > 0;
                    bool currentIsRectangular = w2 > 0 && h2 > 0;
                    bool prevIsCircular = (w1 > 0 && h1 == 0) || (w1 == 0 && h1 > 0);
                    bool currentIsCircular = (w2 > 0 && h2 == 0) || (w2 == 0 && h2 > 0);
                    bool isTransition = false;

                    // Junctions create very short bookkeeping pieces close to the branch intersection.
                    // Mesh-piping full rectangular/round profiles on those tiny legs is what creates the
                    // fan/pyramid artifacts. Keep those as a simple centerline display only.
                    bool prevLikelyJunction = prevNode.Neighbors.Count > 2;
                    bool currentLikelyJunction = currentNode.Neighbors.Count > 2;
                    bool prevTouchesJunction = false;
                    bool currentTouchesJunction = false;

                    for (int n = 0; n < prevNode.Neighbors.Count; n++)
                    {
                        if (prevNode.Neighbors[n].Neighbors.Count > 2) prevTouchesJunction = true;
                    }

                    for (int n = 0; n < currentNode.Neighbors.Count; n++)
                    {
                        if (currentNode.Neighbors[n].Neighbors.Count > 2) currentTouchesJunction = true;
                    }

                    double maxDim = Math.Max(Math.Max(w1, h1), Math.Max(w2, h2));
                    bool shortJunctionLeg = (prevLikelyJunction || currentLikelyJunction || prevTouchesJunction || currentTouchesJunction) && segLen < Math.Max(Duct_Model.Instance.delta_D * 1.1, maxDim * 1.1);

                    if (shortJunctionLeg)
                    {
                        DuctWalls.Add(new Hare.Geometry.Point[2] { prevNode.loc, currentNode.loc });
                    }
                    else if ((prevIsRectangular && currentIsRectangular) || (prevIsCircular && currentIsCircular))
                    {
                        if (prevIsRectangular)
                        {
                            if (Math.Abs(w1 - w2) > epsilon || Math.Abs(h1 - h2) > epsilon) isTransition = true;

                            Hare.Geometry.Point[] startCorners = GetRectangleCorners(prevNode.loc, right, up, w1 / 2, h1 / 2);
                            Hare.Geometry.Point[] endCorners = GetRectangleCorners(currentNode.loc, right, up, w2 / 2, h2 / 2);
                            AddSegmentWalls(DuctWalls, startCorners, endCorners, true, 0, false, false);
                        }
                        else
                        {
                            double r1 = (w1 > 0) ? w1 / 2 : h1 / 2;
                            double r2 = (w2 > 0) ? w2 / 2 : h2 / 2;
                            if (Math.Abs(r1 - r2) > epsilon) isTransition = true;

                            Hare.Geometry.Point[] startCircle = GetCirclePoints(prevNode.loc, right, up, r1, numSegments);
                            Hare.Geometry.Point[] endCircle = GetCirclePoints(currentNode.loc, right, up, r2, numSegments);
                            AddSegmentWalls(DuctWalls, startCircle, endCircle, false, numSegments, false, false);
                        }
                    }
                    else if ((prevIsRectangular && currentIsCircular) || (prevIsCircular && currentIsRectangular))
                    {
                        isTransition = true;
                        Hare.Geometry.Point[][] profiles = new Hare.Geometry.Point[2][];
                        double[] widths = new double[2] { w1, w2 };
                        double[] heights = new double[2] { h1, h2 };
                        Hare.Geometry.Point[] centers = new Hare.Geometry.Point[2] { prevNode.loc, currentNode.loc };

                        for (int k = 0; k < 2; k++)
                        {
                            if (widths[k] > 0 && heights[k] > 0)
                            {
                                Hare.Geometry.Point[] corners = GetRectangleCorners(centers[k], right, up, widths[k] / 2, heights[k] / 2);
                                profiles[k] = new Hare.Geometry.Point[numSegments];
                                for (int h = 0; h < numSegments; h++)
                                {
                                    double t = (double)h / numSegments;
                                    int edge = (int)(t * 4.0);
                                    double edgeT = (t * 4.0) - edge;
                                    Hare.Geometry.Point a = corners[edge % 4];
                                    Hare.Geometry.Point b = corners[(edge + 1) % 4];
                                    profiles[k][h] = a + (b - a) * edgeT;
                                }
                            }
                            else
                            {
                                double r = (widths[k] > 0) ? widths[k] / 2 : heights[k] / 2;
                                profiles[k] = GetCirclePoints(centers[k], right, up, r, numSegments);
                            }
                        }

                        AddSegmentWalls(DuctWalls, profiles[0], profiles[1], false, numSegments, false, false);
                    }
                    else
                    {
                        DuctWalls.Add(new Hare.Geometry.Point[2] { prevNode.loc, currentNode.loc });
                    }

                    TSPL.Add(Sound_Pressure_Level(NoiseType.Total, currentNode));
                    ESPL.Add(Sound_Pressure_Level(NoiseType.Equipment, currentNode));
                    ASPL.Add(Sound_Pressure_Level(NoiseType.Aerodynamic, currentNode));
                    Breakout.Add(SPL_Breakout(currentNode));
                    DuctCenters.Add(prevNode.loc + (currentNode.loc - prevNode.loc) * 0.5);
                    DuctWallcollection.Add(DuctWalls);
                    Transition.Add(isTransition);
                }
            }
        }

        private static Hare.Geometry.Vector RotateVector(Hare.Geometry.Vector v, Hare.Geometry.Vector axis, double cosA, double sinA)
        {
            Hare.Geometry.Vector cross = Hare.Geometry.Hare_math.Cross(axis, v);
            double dot = Hare.Geometry.Hare_math.Dot(axis, v);
            return new Hare.Geometry.Vector(
                v.dx * cosA + cross.dx * sinA + axis.dx * dot * (1 - cosA),
                v.dy * cosA + cross.dy * sinA + axis.dy * dot * (1 - cosA),
                v.dz * cosA + cross.dz * sinA + axis.dz * dot * (1 - cosA));
        }

        private static void AddSegmentWalls(List<Hare.Geometry.Point[]> ductWalls, Hare.Geometry.Point[] startProfile, Hare.Geometry.Point[] endProfile, bool isRect, int numSegments, bool drawStart, bool drawEnd)
        {
            if (isRect)
            {
                for (int k = 0; k < 4; k++)
                {
                    ductWalls.Add(new[] { startProfile[k], endProfile[k] });
                }

                if (drawStart)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        ductWalls.Add(new[] { startProfile[k], startProfile[(k + 1) % 4] });
                    }
                }

                if (drawEnd)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        ductWalls.Add(new[] { endProfile[k], endProfile[(k + 1) % 4] });
                    }
                }
            }
            else
            {
                for (int k = 0; k < numSegments; k++)
                {
                    ductWalls.Add(new[] { startProfile[k], endProfile[k] });
                }

                if (drawStart)
                {
                    for (int k = 0; k < numSegments; k++)
                    {
                        ductWalls.Add(new[] { startProfile[k], startProfile[(k + 1) % numSegments] });
                    }
                }

                if (drawEnd)
                {
                    for (int k = 0; k < numSegments; k++)
                    {
                        ductWalls.Add(new[] { endProfile[k], endProfile[(k + 1) % numSegments] });
                    }
                }
            }
        }

        private Hare.Geometry.Point[] GetRectangleCorners(Hare.Geometry.Point center, Hare.Geometry.Vector right, Hare.Geometry.Vector up, double halfWidth, double halfHeight)
        {
            return new Hare.Geometry.Point[]
            {
                center + (right * halfWidth) + (up * halfHeight),
                center + (right * halfWidth) - (up * halfHeight),
                center - (right * halfWidth) - (up * halfHeight),
                center - (right * halfWidth) + (up * halfHeight)
            };
        }

        private Hare.Geometry.Point[] GetCirclePoints(Hare.Geometry.Point center, Hare.Geometry.Vector right, Hare.Geometry.Vector up, double radius, int numSegments)
        {
            Hare.Geometry.Point[] points = new Hare.Geometry.Point[numSegments];

            for (int i = 0; i < numSegments; i++)
            {
                double angle = (2 * Math.PI * i) / numSegments;
                Hare.Geometry.Vector offset = (right * Math.Cos(angle) + up * Math.Sin(angle)) * radius;
                points[i] = center + offset;
            }

            return points;
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

        private double[] SPL_Breakout(node N)
        {
            double[] DuctBreakout = new double[8];
            for (int oct = 0; oct < 8; oct++) DuctBreakout[oct] = 10 * Math.Log10(Math.Pow(10f, N.SPL_Aerodynamic[oct] / 10) + Math.Pow(10f, N.SPL_Equipment[oct] / 10)) - N.DuctTL[oct] - N.DuctLagging[oct];
            return DuctBreakout;
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
            public double Width;
            public double Height;
            public double[] DuctTL = new double[8];
            public double[] DuctLagging = new double[8];
            public double[] DuctBreakout = new double[8];

            public node(double[] Attenuation, Hare.Geometry.Point p, int id, double width, double height)
            {
                node_id = id;
                Atten = Attenuation;
                loc = p;
                Width = width;
                Height = height;
                DuctTL = (width != 0 && height != 0) ? Breakout.Breakout_Rect : Breakout.Breakout_Round_Longseam;
                DuctLagging = new double[8] {0,0,0,0,0,0,0,0};
            }

            public node(double[] Attenuation, Hare.Geometry.Point p, int id, node neighbor, double width, double height)
                : this(Attenuation, p, id, width, height)
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
                    for (int oct = 0; oct < 8; oct++)
                    {
                        SPL_Equipment[oct] = 10 * Math.Log10(Math.Pow(10f, SPL_Equipment[oct] / 10) + Math.Pow(10f, (new_noise[oct]) / 10));
                    }
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