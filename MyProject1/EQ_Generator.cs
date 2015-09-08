using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Pachyderm_Noise_GH
{
    public class EQ_Generator : GH_Component
    {
        Duct_Model_Manager DMM = Duct_Model_Manager.Instance;

        /// <summary>
        /// Initializes a new instance of the Generator class.
        /// </summary>
        public EQ_Generator()
          : base("Generator", "Gnrtr",
              "Get the SWL of a generator.",
              "Noise", "Source")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Wattage in Megawatts", "MW", "Wattage in Megawatts", GH_ParamAccess.item);
            pManager.AddNumberParameter("Speed in RPM", "RPM", "speed in revolutions per minute (rpm)", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Radiated_Sound_Power_Level", "SWL", "The radated sound power of the Chiller", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // First, we need to retrieve all data from the input parameters.
            // We'll start by declaring variables and assigning them starting values.
            double MW = 0;
            double RPM = 0;

            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            if (!DA.GetData(0, ref MW)) return;
            if (!DA.GetData(0, ref RPM)) return;

            // We're set to create the spiral now. To keep the size of the SolveInstance() method small, 
            // The actual functionality will be in a different method:
            double[] SWL = Pachyderm_Noise_Control.ASHRAE.Equipment.Generator_radiated(MW, RPM);

            // Finally assign the spiral to the output parameter.
            DA.SetData(0, SWL.ToList());

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
            get { return new Guid("6f185296-8941-4fa9-9883-5716fa73f16b"); }
        }
    }
}