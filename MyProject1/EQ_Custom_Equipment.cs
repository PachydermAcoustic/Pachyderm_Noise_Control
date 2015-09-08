using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Pachyderm_Noise_Control;

namespace Pachyderm_Noise_GH
{
    public class UserEquipment : GH_Component, IHVACComponent
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        
        Duct_Model_Manager DMM = Duct_Model_Manager.Instance;

        public UserEquipment()
          : base("User Equipment", "U-EQ",
              "Equipment/Noise Source from user input",
              "Noise", "Source")
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
            pManager.AddNumberParameter("Discharge Sound Power", "DSWL", "The discharge sound power of the unit in decibels, 63 hz. to 8khz.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Intake Sound Power", "ISWL", "The intake sound power of the unit in decibels, 63 hz. to 8khz.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Radiated Sound Power", "RadSWL", "The radiated sound power of the unit in decibels, 63 hz. to 8khz.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Volume", "V", "the volume of air served by this device.", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Supply Frame", "SF", "Provide a frame describing the plane of the outlet section.", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Return Frame", "RF", "Provide a frame describing the plane of the inlet section.", GH_ParamAccess.item);
            // If you want to change properties of certain parameters, 
            // you can use the pManager instance to access them by index:
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // Use the pManager object to register your output parameters.
            // Output parameters do not have default values, but they too must have the correct access type.
            pManager.AddNumberParameter("Radiated_Sound_Power_Level", "SWL", "The radated sound power of the Air Handler Unit", GH_ParamAccess.list);
            pManager.AddGenericParameter("Ducted Discharge", "Supply", "Sound Power at the in-duct discharge of the Air Handler Unit", GH_ParamAccess.item);
            pManager.AddGenericParameter("Ducted Return", "Return", "Sound Power at the in-duct return of the Air Handler Unit", GH_ParamAccess.item);
            // Sometimes you want to hide a specific parameter from the Rhino preview.
            // You can use the HideParameter() method as a quick way:
            //pManager.HideParameter(0);
        }

        public double Pressure_Drop()
        {
            return 0;
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // First, we need to retrieve all data from the input parameters.
            // We'll start by declaring variables and assigning them starting values.
            List<double> Discharge = new List<double>();
            List<double> Intake = new List<double>();
            List<double> Radiated = new List<double>();
            double volume = 0;
            Plane SupplyFrame = new Plane();
            Plane ReturnFrame = new Plane();

            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            if (DA.GetDataList(2, Radiated)) DA.SetDataList(2, Radiated);

            DA.GetDataList(0, Discharge);
            DA.GetDataList(1, Intake);
            DA.GetData(3, ref volume);
            DA.GetData(4, ref SupplyFrame);
            DA.GetData(5, ref ReturnFrame);

            // We're set to create the spiral now. To keep the size of the SolveInstance() method small, 
            // The actual functionality will be in a different method:
            State supplystate = null;
            State returnstate = null;

            if (Discharge != null && Discharge.Count == 8)
            {
                supplystate = new State(volume, double.Epsilon, double.Epsilon, Discharge.ToArray(), Discharge.ToArray(), new Hare.Geometry.Vector(SupplyFrame.Normal.X, SupplyFrame.Normal.Y, SupplyFrame.Normal.Z), new Hare.Geometry.Vector(SupplyFrame.XAxis.X, SupplyFrame.XAxis.Y, SupplyFrame.XAxis.Z), new Hare.Geometry.Vector(SupplyFrame.YAxis.X, SupplyFrame.YAxis.Y, SupplyFrame.YAxis.Z));
                DA.SetData(1, supplystate);
            }
            if (Intake != null && Intake.Count == 8)
            {
                returnstate = new State(volume, double.Epsilon, double.Epsilon, Intake.ToArray(), Intake.ToArray(), new Hare.Geometry.Vector(ReturnFrame.Normal.X, ReturnFrame.Normal.Y, ReturnFrame.Normal.Z), new Hare.Geometry.Vector(ReturnFrame.XAxis.X, ReturnFrame.XAxis.Y, ReturnFrame.XAxis.Z), new Hare.Geometry.Vector(ReturnFrame.YAxis.X, ReturnFrame.YAxis.Y, ReturnFrame.YAxis.Z));
                DA.SetData(2, returnstate);
            }

            supplystate.PathId = Duct_Model.Instance.Intiate_Path(new Hare.Geometry.Point(SupplyFrame.OriginX, SupplyFrame.OriginY, SupplyFrame.OriginZ), supplystate.Noise_Best, Duct_Model.NoiseType.Equipment);
            returnstate.PathId = Duct_Model.Instance.Intiate_Path(new Hare.Geometry.Point(ReturnFrame.OriginX, ReturnFrame.OriginY, ReturnFrame.OriginZ), supplystate.Noise_Best, Duct_Model.NoiseType.Equipment);
        }

        /// <summary>
        /// The Exposure property controls where in the panel a component icon 
        /// will appear. There are seven possible locations (primary to septenary), 
        /// each of which can be combined with the GH_Exposure.obscure flag, which 
        /// ensures the component will only be visible on panel dropdowns.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override void BeforeSolveInstance()
        {
            Rhino.RhinoApp.WriteLine("UserEquipment Before");
        }

        protected override void AfterSolveInstance()
        {
            Rhino.RhinoApp.WriteLine("UserEquipment After");
        }

        ///// <summary>
        ///// Provides an Icon for every component that will be visible in the User Interface.
        ///// Icons need to be 24x24 pixels.
        ///// </summary>
        //protected override System.Drawing.Bitmap Icon
        //{
        //    get
        //    {
        //        // You can add image files to your project resources and access them like this:
        //        //return Resources.IconForThisComponent;
        //        return null;
        //    }
        //}

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("5089FA6A-30D5-4742-ADBE-606AAAF56851"); }
        }
    }
}
