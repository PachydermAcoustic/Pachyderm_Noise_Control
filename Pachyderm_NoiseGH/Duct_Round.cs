using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Pachyderm_Noise_Control;
using Rhino.Geometry;

namespace Pachyderm_Noise_GH
{
    public class Duct_Round : GH_Component, IHVACComponent
    {
        Duct_Model_Manager DMM = Duct_Model_Manager.Instance;

        /// <summary>
        /// Initializes a new instance of the Duct_Straight class.
        /// </summary>
        public Duct_Round()
          : base("Round Duct", "Rnd-Duct",
              "Round Duct, with specified acoustical liner.",
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
            pManager.AddNumberParameter("Diameter(inches)", "D", "Cross-duct dimension of the duct", GH_ParamAccess.item);
            pManager.AddNumberParameter("Length(feet)", "L", "the volume of air served by this device.", GH_ParamAccess.item, 3);
            pManager.AddNumberParameter("Lining thickness (inches)", "Lng", "The thicknes of acoustical lining in inches", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("State out", "OUT", "Connect to the next component here...", GH_ParamAccess.item);
        }

        public double Pressure_Drop()
        {
            State S = (Params.Input[0].VolatileData.AllData(true).GetEnumerator().Current as Grasshopper.Kernel.Types.GH_Goo<State>).Value as State;
            double D = (Params.Input[1].VolatileData.AllData(true).GetEnumerator().Current as Grasshopper.Kernel.Types.GH_Number).Value;
            double L = (Params.Input[2].VolatileData.AllData(true).GetEnumerator().Current as Grasshopper.Kernel.Types.GH_Number).Value;
            double Lng = (Params.Input[3].VolatileData.AllData(true).GetEnumerator().Current as Grasshopper.Kernel.Types.GH_Number).Value;
            double dz = S.Direction.dz * L;
            double density_in_duct = .075;
            double Area = Math.PI * D * D / 4;
            double Velocity = S.Volume / (Area);
            double Velocity_Pressure = 0.602 * Velocity * Velocity;
            double Dh = 4 * Area * Math.PI * D;

            double Reynolds = 8.5 * Dh * Velocity;
            double roughness = 0;
            ///Getting Friction Factor (ASHRAE Fundamentals & Other)
            double friction_factor = 0;
            if (Reynolds < 2320)
            {
                //Laminar Flow
                friction_factor = 64 / Reynolds;
            }
            else if (Reynolds < 10000)
            {
                //Transition flow (from 2320 - 4000
                //Turbulent Flow - Coleman White - Goudar-Sonnad eequation
                double a = 2 / Math.Log(10);
                double b = (roughness / Dh) / 3.7;
                double d = Math.Log(10) * Reynolds / 5.02;
                double s = b * d + Math.Log(d);
                double q = Math.Pow(s, s / (s + 1));
                double g = b * d + Math.Log(d / q);
                double z = Math.Log(q / g);
                double Dla = z * g / (g + 1);
                double Dcfa = Dla * (1 + (z / 2) / ((g + 1) * (g + 1) + (z / 3) * (2 * g - 1)));
                friction_factor = Math.Pow(a * (Math.Log(d / q) + Dcfa), -2);
            }

            double dpf = (12 * friction_factor * L / Dh) * density_in_duct * Velocity * Velocity / 1203409;
            double dpse = 0.192 * (0.075 - density_in_duct) * dz;

            return dpf + dpse;
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Pachyderm_Noise_Control.State S = new Pachyderm_Noise_Control.State(0, 0, 0, new double[8], new double[8], new Hare.Geometry.Vector(), new Hare.Geometry.Vector(), new Hare.Geometry.Vector());
            double D = 0, L = 0, Lng = 0;
            if (!DA.GetData<Pachyderm_Noise_Control.State> (0, ref S)) return;
            if (!DA.GetData(1, ref D)) D = S.dimensions[0];
            Grasshopper.Kernel.Parameters.Param_Number paramD = (Params.Input[1] as Grasshopper.Kernel.Parameters.Param_Number);
            if (paramD != null) paramD.NickName = "D: " + Math.Round(D) + " in.";
            if (!DA.GetData(2, ref L)) return;
            Grasshopper.Kernel.Parameters.Param_Number paramL = (Params.Input[2] as Grasshopper.Kernel.Parameters.Param_Number);
            if (paramL != null) paramL.NickName = "L: " + Math.Round(L) + " ft.";
            if (!DA.GetData(3, ref Lng)) Lng = S.lining_thickness;
            Grasshopper.Kernel.Parameters.Param_Number paramLng = (Params.Input[3] as Grasshopper.Kernel.Parameters.Param_Number);
            if (paramLng != null) paramLng.NickName = "Lng: " + Math.Round(Lng) + " in.";

            //if (Lng == 0) DA.SetData(0, Pachyderm_Noise_Control.ASHRAE.Round_Duct.Unlined(S, D, L));
            //else if (Lng == 1) DA.SetData(0, Pachyderm_Noise_Control.ASHRAE.Round_Duct.Lined_OneInch(S, D, L));
            //else if (Lng == 2) DA.SetData(0, Pachyderm_Noise_Control.ASHRAE.Round_Duct.Lined_TwoInch(S, D, L));
            //else return;
            double[] atten;
            State S_new = Pachyderm_Noise_Control.ASHRAE.Round_Duct.Straight_Duct(S, D, L, Lng, out atten);
            DA.SetData(0, S_new);
            this.Message = "F      63  125  250  500   1k   2k   4k   8k" + "\n" + "Atten: " + Math.Round(atten[0], 2) + " " + Math.Round(atten[1], 2) + " " + Math.Round(atten[2], 2) + " " + Math.Round(atten[3], 2) + " " + Math.Round(atten[4], 2) + " " + Math.Round(atten[5], 2) + " " + Math.Round(atten[6], 2) + " " + Math.Round(atten[7], 2) + " ";
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
            get { return new Guid("B5F5B522-2D91-4285-8C95-5572F9FAD0F5"); }
        }
    }
}