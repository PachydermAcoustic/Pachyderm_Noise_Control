using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;

namespace Pachyderm_Noise_GH
{
    public class EQ_Chiller : GH_Component
    {
        Duct_Model_Manager DMM = Duct_Model_Manager.Instance;

        /// <summary>
        /// Initializes a new instance of the Chiller class.
        /// </summary>
        public EQ_Chiller()
          : base("Chiller", "Chlr",
              "Get the SWL of a Chiller.",
              "Noise", "Source")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Wattage(kw)", "W", "Wattage in kilowatts", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Volume(tons)", "V", "Volume in tons", GH_ParamAccess.item, 0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Radiated_Sound_Power_Level", "SWL", "The radated sound power of the Chiller", GH_ParamAccess.list);
        }

        public override bool AppendMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Screw Type", Screw_Click, true, Screw);
            Menu_AppendItem(menu, "Commpressor Type", Compressor_Click, true, Compressor);
            Menu_AppendItem(menu, "Internally-geared Type", IG_Click, true, Internallygeared);
            Menu_AppendItem(menu, "Direct Drive Type", DD_Click, true, DirectDrive);
            return base.AppendMenuItems(menu);
        }

        bool Screw = true;
        bool Compressor = false;
        bool Internallygeared = false;
        bool DirectDrive = false;

        private void Screw_Click(Object sender, EventArgs e)
        {
            Screw = true;
            Compressor = false;
            Internallygeared = false;
            DirectDrive = false;
            ExpireSolution(true);
        }
        private void Compressor_Click(Object sender, EventArgs e)
        {
            Screw = false;
            Compressor = true;
            Internallygeared = false;
            DirectDrive = false;
            ExpireSolution(true);
        }
        private void IG_Click(Object sender, EventArgs e)
        {
            Screw = false;
            Compressor = false;
            Internallygeared = true;
            DirectDrive = false;
            ExpireSolution(true);
        }
        private void DD_Click(Object sender, EventArgs e)
        {
            Screw = false;
            Compressor = false;
            Internallygeared = false;
            DirectDrive = true;
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
            double kWattage = 0;
            double Volume_T = 0;

            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            if (DA.GetData(0, ref kWattage) && DA.GetData(1, ref Volume_T)) return;
            if (kWattage + Volume_T == 0) return;

            Pachyderm_Noise_Control.ASHRAE.Equipment.ChillerType ctype = Pachyderm_Noise_Control.ASHRAE.Equipment.ChillerType.Chiller_DirectDrive;

            if (Screw) ctype = Pachyderm_Noise_Control.ASHRAE.Equipment.ChillerType.Chiller_Screw;
            else if (Compressor) ctype = Pachyderm_Noise_Control.ASHRAE.Equipment.ChillerType.Chiller_Compressor;
            else if (Internallygeared) ctype = Pachyderm_Noise_Control.ASHRAE.Equipment.ChillerType.Chiller_InternallyGeared;

            // We're set to create the spiral now. To keep the size of the SolveInstance() method small, 
            // The actual functionality will be in a different method:
            double[] SWL = Pachyderm_Noise_Control.ASHRAE.Equipment.Chiller_radiated(ctype, kWattage, Volume_T);

            // Finally assign the spiral to the output parameter.
            DA.SetData(0, SWL.ToList());
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
            get { return new Guid("28987da2-da00-48e8-9276-21cf9c7014a0"); }
        }
    }
}