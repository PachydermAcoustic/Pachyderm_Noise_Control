using System;
using System.Collections.Generic;
using Pachyderm_Acoustic.Utilities;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;

namespace Pachyderm_Noise_GH
{
    public class Junction_Rectangular : GH_Component, IHVACComponent, IGH_VariableParameterComponent
    {
        Duct_Model_Manager DMM = Duct_Model_Manager.Instance;

        /// <summary>
        /// Initializes a new instance of the Duct_Straight class.
        /// </summary>
        public Junction_Rectangular()
          : base("Rectangular Junction", "Rect-Jct",
              "Rectangular junction duct",
              "Noise", "Ducts")
        {
            SolutionExpired += Expire_Train;
        }

        private void Expire_Train(Object o, GH_SolutionExpiredEventArgs X)
        {
            GH_Document doc = OnPingDocument();
            if (doc != null) doc.ExpireSolution();
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("State in", "IN", "Connect the previous component here...", GH_ParamAccess.item);
            pManager.AddNumberParameter("Width/Diameter(inches)", "W/D", "Horizontal dimension of ducts leaving this junction. The first will be the continuation, and should have the largest area.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Height(inches)", "H", "Vertical dimension of ducts leaving this junction. The first will be the continuation, and should have the largest area.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Radius(inches)", "R", "The radius of the inside of the elbow", GH_ParamAccess.list);
            pManager.AddNumberParameter("Volume", "V", "Specify the volume for each branch (if omitted, volume will be calcuated according to cross sectional area.", GH_ParamAccess.list);
            pManager.AddVectorParameter("Direction", "D", "Vectors indicating the direction each duct travels from the junciton in. The first will be the continuation.", GH_ParamAccess.list);
            pManager[4].Optional = true;
            pManager[5].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("State out", "OUT", "Connect to the next component here. Use a ____ to get all branches individually.", GH_ParamAccess.item);
            //Grasshopper.Kernel.Components.GH_ExplodeTreeComponent ET = new Grasshopper.Kernel.Components.GH_ExplodeTreeComponent();
            //this.OnPingDocument().AddObject(ET, false);
            //ET.Params.Input[0].AddSource(pManager[0]);
        }

        public double Pressure_Drop()
        {
            return 0;
        }

        int ct;

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Pachyderm_Noise_Control.State S = new Pachyderm_Noise_Control.State(0, 0, 0, new double[8], new double[8], new Hare.Geometry.Vector(), new Hare.Geometry.Vector(), new Hare.Geometry.Vector());
            List<double> R = new List<double>();
            List<double> V = new List<double>();
            List<Vector3d> Dir = new List<Vector3d>();
            List<double> W = new List<double>();
            List<double> H = new List<double>();
            if (!DA.GetData<Pachyderm_Noise_Control.State>(0, ref S)) return;
            DA.GetDataList(1, W);
            DA.GetDataList(2, H);
            if (W.Count == 0 && H.Count == 0) return;
            if (W.Count < 2 && H.Count < 2) throw new Exception("Junctions require at least 2 branches.");
            if (W == null || W.Count < 1 || W.Sum() == 0) for (int i = 0; i < H.Count; i++) W.Add(0);
            if (H == null || H.Count < 1 || H.Sum() == 0) for (int i = 0; i < W.Count; i++) H.Add(0);
            if (W.Count != H.Count) throw new Exception("For each branch, make sure to specify both a height and a width, unless all branches are round.");
            if (!DA.GetDataList(3, R)) for (int i = 0; i < W.Count; i++) R.Add(0);
            DA.GetDataList(4, V);
            if (!DA.GetDataList(5, Dir)) return;

            bool HasThroughDuct = false;
            List<double> angles = new List<double>();
            Hare.Geometry.Vector[] dir = new Hare.Geometry.Vector[Dir.Count];

            for (int i = 0; i < Dir.Count; i++)
            {
                dir[i] = Pachyderm_Acoustic.Utilities.RC_PachTools.RPttoHPt(Dir[i]);
                dir[i].Normalize();
                angles.Add(Hare.Geometry.Hare_math.Dot(S.Frame[2] , dir[i]));
                if (angles[i] > 0.6)
                {
                    HasThroughDuct = true;
                    continue;
                }
            }

            Pachyderm_Noise_Control.ASHRAE.JCT.JunctionType T;

            if (HasThroughDuct)
            {
                if (angles.Count == 2) T = Pachyderm_Noise_Control.ASHRAE.JCT.JunctionType.Takeoff_90;
                else T = Pachyderm_Noise_Control.ASHRAE.JCT.JunctionType.X_Junction;
            }
            else
            {
                if (angles.Count == 2) T = Pachyderm_Noise_Control.ASHRAE.JCT.JunctionType.T_Junction;
                else T = Pachyderm_Noise_Control.ASHRAE.JCT.JunctionType.X_Junction;
            }

            double[] atten;
            double[][] regen;

            Pachyderm_Noise_Control.State[] outlets; 
            Pachyderm_Noise_Control.ASHRAE.JCT.Junction(S, T, H.ToArray(), W.ToArray(), dir, out outlets, R, out atten, out regen, V.ToArray());
            Pachyderm_Noise_Control.State[] St = new Pachyderm_Noise_Control.State[outlets.Length];

            ct = outlets.Length;

            for (int i = 0; i < Params.Output.Count; i++)
            {
                if (outlets.Length - 1 < i)
                {
                    break;
                }
                outlets[i].Direction = dir[i];
                DA.SetData(i, outlets[i]);
            }

            //DA.SetDataList(0, St);
            this.Message = "F      63  125  250  500   1k   2k   4k   8k";
            for(int i = 0; i < regen.Length; i++)this.Message += "\n" + "Atten: " + Math.Round(atten[i], 2) + " " + Math.Round(atten[i], 2) + " " + Math.Round(atten[i], 2) + " " + Math.Round(atten[i], 2) + " " + Math.Round(atten[i], 2) + " " + Math.Round(atten[i], 2) + " " + Math.Round(atten[i], 2) + " " + Math.Round(atten[i], 2) + " " + "\n" + "Regen: " + Math.Round(regen[i][0], 2) + " " + Math.Round(regen[i][1], 2) + " " + Math.Round(regen[i][2], 2) + " " + Math.Round(regen[i][3], 2) + " " + Math.Round(regen[i][4], 2) + " " + Math.Round(regen[i][5], 2) + " " + Math.Round(regen[i][6], 2) + " " + Math.Round(regen[i][7], 2) + " ";
            DMM.Display_Needed = true;
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        protected override void AfterSolveInstance()
        {
            base.AfterSolveInstance();
            if (Params.Output.Count == ct) return;

            for (int i = 0; i < ct; i++)
            {
                if (Params.Output.Count - 1 < i)
                {
                    Grasshopper.Kernel.Parameters.Param_GenericObject GO = new Grasshopper.Kernel.Parameters.Param_GenericObject();
                    GO.Name = "State out";
                    GO.NickName = "OUT";
                    GO.Description = "Connect to the next component here. Use a ____ to get all branches individually.";
                    GO.Access = GH_ParamAccess.item;
                    Params.RegisterOutputParam(GO);
                }
            }

            int pct = Params.Output.Count;

            for (int i = ct; i < pct; i++)
            {
                Params.UnregisterOutputParameter(Params.Output[ct]);
            }

            Params.OnParametersChanged();
            this.ExpireSolution(true);
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("97f4ebed-bc5b-4dce-b3c6-4c719d9fa55e"); }
        }

        public void VariableParameterMaintenance(){ }
        public bool DestroyParameter(GH_ParameterSide ps, int arg) { return false; }
        public IGH_Param CreateParameter(GH_ParameterSide ps, int arg) { return null; }
        public bool CanRemoveParameter(GH_ParameterSide ps, int arg) { return false; }
        public bool CanInsertParameter(GH_ParameterSide ps, int arg) { return false; }
    }
}