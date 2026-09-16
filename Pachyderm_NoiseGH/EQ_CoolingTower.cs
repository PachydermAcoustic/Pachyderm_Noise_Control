using System;
using Grasshopper.Kernel;
using System.Linq;

namespace Pachyderm_Noise_GH
{
    public class EQ_CoolingTower : GH_Component
    {
        Duct_Model_Manager DMM = Duct_Model_Manager.Instance;

        /// <summary>
        /// Initializes a new instance of the CoolingTower class.
        /// </summary>
        public EQ_CoolingTower()
          : base("CoolingTower", "CT",
              "Get the SWL of a Cooling Tower.",
              "Noise", "Source")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("HorsePower", "HP", "Fan power in horsepower", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Radiated_Sound_Power_Level", "SWL", "The radated sound power of the Chiller", GH_ParamAccess.list);
        }

        //public override bool AppendMenuItems(ToolStripDropDown menu)
        //{
        //    Menu_AppendItem(menu, "Centrifugal", Centrifugal_Click, true, Centrifugal);
        //    Menu_AppendItem(menu, "Commpressor Type", Propeller_Click, true, Propeller);
        //    return base.AppendMenuItems(menu);
        //}

        bool Centrifugal = true;
        bool Propeller = false;
        private void Centrifugal_Click(Object sender, EventArgs e)
        {
            Centrifugal = true;
            Propeller = false;
            ExpireSolution(true);
        }

        private void Propeller_Click(Object sender, EventArgs e)
        {
            Centrifugal = false;
            Propeller = true;
            ExpireSolution(true);
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

            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            if (!DA.GetData(0, ref HP)) return;

            // We're set to create the spiral now. To keep the size of the SolveInstance() method small, 
            // The actual functionality will be in a different method:
            double[] SWL = (Centrifugal) ? Pachyderm_Noise_Control.ASHRAE.Equipment.CoolingTower_CentrifugalFan_Radiated(HP) : Pachyderm_Noise_Control.ASHRAE.Equipment.CoolingTower_Propellerfan_Radiated(HP);
                        
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
            get { return new Guid("9032ce39-f504-4043-ba38-f65b3c1270d0"); }
        }
    }
}