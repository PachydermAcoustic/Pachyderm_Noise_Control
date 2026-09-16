using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Pachyderm_Noise_GH
{
    public class Elbow_Rectangular : GH_Component, IHVACComponent
    {
        Duct_Model_Manager DMM = Duct_Model_Manager.Instance;

        /// <summary>
        /// Initializes a new instance of the Duct_Straight class.
        /// </summary>
        public Elbow_Rectangular()
          : base("Rectangular Elbow", "Rect-Elb",
              "Rectangular elbow duct, lined as specified...",
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
            pManager.AddNumberParameter("Width(inches)", "W", "Horizontal dimension of the duct.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Height(inches)", "H", "Vertical dimension of the duct", GH_ParamAccess.item);
            pManager.AddNumberParameter("Radius(inches)", "R", "The radius of the inside of the elbow", GH_ParamAccess.item);
            pManager.AddNumberParameter("Lining thickness (inches)", "Lng", "The thicknes of acoustical lining in inches", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Turning Vane Chord Length", "VL", "The length of turning vanes. (Leave 0 if none)", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Turning Vane Count", "VC", "The number of turning vanes. (Leave 0 if none)", GH_ParamAccess.item, 0);
            pManager.AddVectorParameter("Direction", "D", "Vector indicating the direction each duct travels from the elbow.", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            pManager[6].Optional = true;
        }

        public double Pressure_Drop()
        {
            return 0;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("State out", "OUT", "Connect to the next component here...", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Pachyderm_Noise_Control.State S = new Pachyderm_Noise_Control.State(0, 0, 0, new double[8], new double[8], new Hare.Geometry.Vector(), new Hare.Geometry.Vector(), new Hare.Geometry.Vector());
            double W = 0, H = 0, R = 0, Lng = 0, VL = 0, VC = 0;
            Rhino.Geometry.Vector3d dir = new Vector3d();
            if (!DA.GetData<Pachyderm_Noise_Control.State>
                (0, ref S)) return;

            if (!DA.GetData(1, ref W)) W = S.dimensions[0];
            Grasshopper.Kernel.Parameters.Param_Number paramW = (Params.Input[1] as Grasshopper.Kernel.Parameters.Param_Number);
            if (paramW != null) paramW.NickName = "W: " + Math.Round(W) + " in.";

            if (!DA.GetData(2, ref H)) H = S.dimensions[1];
            Grasshopper.Kernel.Parameters.Param_Number paramH = (Params.Input[2] as Grasshopper.Kernel.Parameters.Param_Number);
            if (paramH != null) paramH.NickName = "H: " + Math.Round(H) + " in.";

            if (!DA.GetData(3, ref R)) R = 0;
            Grasshopper.Kernel.Parameters.Param_Number paramR = (Params.Input[3] as Grasshopper.Kernel.Parameters.Param_Number);
            if (paramR != null) paramR.NickName = "R: " + Math.Round(R) + " in.";

            if (!DA.GetData(4, ref Lng)) Lng = S.lining_thickness;
            Grasshopper.Kernel.Parameters.Param_Number paramLng = (Params.Input[4] as Grasshopper.Kernel.Parameters.Param_Number);
            if (paramLng != null) paramLng.NickName = "Lng: " + Math.Round(Lng) + " in.";

            DA.GetData(5, ref VL);
            DA.GetData(6, ref VC);

            if (!DA.GetData(7, ref dir))
            {
                Hare.Geometry.Vector sDir = S.Direction;
                dir = new Vector3d(sDir.dx, sDir.dy, sDir.dz);
            }

            if (dir.IsZero) dir = Vector3d.XAxis;
            double[] atten, regen;
            DA.SetData(0, Pachyderm_Noise_Control.ASHRAE.Elbow.Rectangular_Elbow(S, new Hare.Geometry.Vector(dir.X, dir.Y, dir.Z), H, W, Lng, R, out atten, out regen, VL, VC));
            this.Message = "F      63  125  250  500   1k   2k   4k   8k" + "\n" + "Atten: " + Math.Round(atten[0], 2) + " " + Math.Round(atten[1], 2) + " " + Math.Round(atten[2], 2) + " " + Math.Round(atten[3], 2) + " " + Math.Round(atten[4], 2) + " " + Math.Round(atten[5], 2) + " " + Math.Round(atten[6], 2) + " " + Math.Round(atten[7], 2) + " " + "\n" + "Regen: " + Math.Round(regen[0], 2) + " " + Math.Round(regen[1], 2) + " " + Math.Round(regen[2], 2) + " " + Math.Round(regen[3], 2) + " " + Math.Round(regen[4], 2) + " " + Math.Round(regen[5], 2) + " " + Math.Round(regen[6], 2) + " " + Math.Round(regen[7], 2) + " ";
            DMM.Display_Needed = true;
        }

        ///// <summary>
        ///// Provides an Icon for the component.
        ///// </summary>
        //protected override System.Drawing.Bitmap Icon
        //{
        //    get
        //    {
        //        //You can add image files to your project resources and access them like this:
        //        // return Resources.IconForThisComponent;
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("892D698A-DA0D-4EBE-B543-AD97481F755D"); }
        }
    }
}