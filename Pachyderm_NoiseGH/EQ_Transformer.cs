using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Pachyderm_Noise_GH
{
    public class EQ_Transformer : GH_Component
    {
        Duct_Model_Manager DMM = Duct_Model_Manager.Instance;

        /// <summary>
        /// Initializes a new instance of the Transformer class.
        /// </summary>
        public EQ_Transformer()
          : base("Transformer", "XFMR",
              "Get the SWL of a Transformer.",
              "Noise", "Source")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("NEMA_Rating", "NEMA", "the NEMA rating of the transformer.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Area_of_Sidewalls", "A", "Area of all Transformer Sidewalls.", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Radiated Sound Power Level - best case", "SWL Best", "The best-case radated sound power of the Air Handler Unit", GH_ParamAccess.list);
            pManager.AddNumberParameter("Radiated Sound Power Level - worst case", "SWL Worst", "The worst-case radated sound power of the Air Handler Unit", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double NEMA_Rating = 0;
            double Area_of_Sidewalls = 0;
            if (!DA.GetData(0, ref NEMA_Rating)) return;
            if (!DA.GetData(1, ref Area_of_Sidewalls)) return;

            double[] SWL, SWL_Worst;
            SWL = Pachyderm_Noise_Control.ASHRAE.Equipment.Transformer(NEMA_Rating, Area_of_Sidewalls, out SWL_Worst);

            DA.SetData(0, SWL.ToList());
            DA.SetData(0, SWL_Worst.ToList());
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
            get { return new Guid("f2192ec0-ad0c-4efe-b526-8699d269d7e3"); }
        }
    }
}