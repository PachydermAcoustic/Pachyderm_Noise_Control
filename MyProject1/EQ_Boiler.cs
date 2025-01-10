using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Pachyderm_Noise_GH
{
    public class EQ_Boiler : GH_Component
    {
        Duct_Model_Manager DMM = Duct_Model_Manager.Instance;

        /// <summary>
        /// Initializes a new instance of the Boiler class.
        /// </summary>
        public EQ_Boiler()
          : base("Boiler", "Blr", "Get the SWL of a boiler.", "Noise", "Source")
        {
        }

        /// <summary>
        /// RegiC:\Users\Arthu\Desktop\DEV\Pachyderm_Noise_Control\MyProject1\EQ_Boiler.cssters all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Horsepower", "HP", "The power rating for the motor", GH_ParamAccess.item,0);
            pManager.AddNumberParameter("Wattage(kW)", "W", "Wattage in kilowatts", GH_ParamAccess.item,0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Radiated_Sound_Power_Level", "SWL", "The radated sound power of the Air Handler Unit", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // First, we need to retrieve all data from the input parameters.
            // We'll start by declaring variables and assigning them starting values.
            double HP = 0;
            double kWattage = 0;

            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            if (DA.GetData(0, ref HP) && DA.GetData(1, ref kWattage)) return;
            if (HP + kWattage == 0) return;

            // We're set to create the spiral now. To keep the size of the SolveInstance() method small, 
            // The actual functionality will be in a different method:
            double[] SWL_Worst;
            double[] SWL = Pachyderm_Noise_Control.ASHRAE.Equipment.Boiler(out SWL_Worst, HP, kWattage);

            // Finally assign the spiral to the output parameter.
            DA.SetData(0, SWL.ToList());
            DA.SetData(1, SWL_Worst.ToList());
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

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("71bc1757-9743-47ae-aee5-f412f1d118a8"); }
        }
    }
}