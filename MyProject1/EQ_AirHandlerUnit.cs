using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Pachyderm_Noise_GH
{
    public class EQ_AirHandlerUnit : GH_Component, IHVACComponent
    {
        Duct_Model_Manager DMM = Duct_Model_Manager.Instance;
        Plane Supply;
        Plane Return;

        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public EQ_AirHandlerUnit()
          : base("Air Handler Unit", "AHU",
              "Get the SWL of an Air Handler Unit.",
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
            pManager.AddNumberParameter("Horsepower", "HP", "The power rating for the motor", GH_ParamAccess.item);
            pManager.AddNumberParameter("Fansize(in)", "FS", "Characteristic dimension of the fan in inchehs (usually the diameter)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Volume", "V", "Volume of air pushed by the fan", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Discharge Direction", "SD", "Vectors indicating the direction each duct.", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Return Direction", "RD", "Vectors indicating the direction the duct travels from the unit in.", GH_ParamAccess.item);
            // If you want to change properties of certain parameters, 
            // you can use the pManager instance to access them by index:
            //pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // Use the pManager object to register your output parameters.
            // Output parameters do not have default values, but they too must have the correct access type.
            pManager.AddNumberParameter("Radiated_Sound_Power_Level", "SWL", "The radated sound power of the Air Handler Unit", GH_ParamAccess.list);
            pManager.AddGenericParameter("Ducted Discharge", "Supply", "Sound Power at the in-duct discharge of the Air Handler Unit", GH_ParamAccess.list);
            pManager.AddGenericParameter("Ducted Return", "Return", "Sound Power at the in-duct return of the Air Handler Unit", GH_ParamAccess.list);
            // Sometimes you want to hide a specific parameter from the Rhino preview.
            // You can use the HideParameter() method as a quick way:
            //pManager.HideParameter(0);
        }

        //public override bool AppendMenuItems(ToolStripDropDown menu)
        //{
        //    Menu_AppendItem(menu, "Centrifugal - Backward Inclined", CB_Click, true, Centrifugal_Backward);
        //    Menu_AppendItem(menu, "Centrifugal - Forward Inclined", CF_Click, true, Centrifugal_Forward);
        //    Menu_AppendItem(menu, "Centrifugal - Radial", CR_Click, true, Centrifugal_Radial);
        //    Menu_AppendItem(menu, "Propeller", Propeller_Click, true, Propeller);
        //    Menu_AppendItem(menu, "Vaneaxial", Vaneaxial_Click, true, Vaneaxial);
        //    Menu_AppendItem(menu, "Tubeaxial", Tubeaxial_Click, true, Tubeaxial);
        //    return base.AppendMenuItems(menu);
        //}

        public double Pressure_Drop()
        {
            return 0;
        }

        bool Centrifugal_Backward = true;
        bool Centrifugal_Forward = false;
        bool Centrifugal_Radial = false;
        bool Propeller = false;
        bool Vaneaxial = false;
        bool Tubeaxial = false;

        private void CB_Click(Object sender, EventArgs e)
        {
            Centrifugal_Backward = true;
            Centrifugal_Forward = false;
            Centrifugal_Radial = false;
            Propeller = false;
            Vaneaxial = false;
            Tubeaxial = false;
            ExpireSolution(true);
        }
        private void CF_Click(Object sender, EventArgs e)
        {
            Centrifugal_Backward = false;
            Centrifugal_Forward = true;
            Centrifugal_Radial = false;
            Propeller = false;
            Vaneaxial = false;
            Tubeaxial = false;
            ExpireSolution(true);
        }
        private void CR_Click(Object sender, EventArgs e)
        {
            Centrifugal_Backward = false;
            Centrifugal_Forward = false;
            Centrifugal_Radial = true;
            Propeller = false;
            Vaneaxial = false;
            Tubeaxial = false;
            ExpireSolution(true);
        }
        private void Propeller_Click(Object sender, EventArgs e)
        {
            Centrifugal_Backward = false;
            Centrifugal_Forward = false;
            Centrifugal_Radial = false;
            Propeller = true;
            Vaneaxial = false;
            Tubeaxial = false;
            ExpireSolution(true);
        }
        private void Vaneaxial_Click(Object sender, EventArgs e)
        {
            Centrifugal_Backward = false;
            Centrifugal_Forward = false;
            Centrifugal_Radial = false;
            Propeller = false;
            Vaneaxial = true;
            Tubeaxial = false;
            ExpireSolution(true);
        }
        private void Tubeaxial_Click(Object sender, EventArgs e)
        {
            Centrifugal_Backward = false;
            Centrifugal_Forward = false;
            Centrifugal_Radial = false;
            Propeller = false;
            Vaneaxial = false;
            Tubeaxial = true;
            ExpireSolution(true);
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
            double HP = 0;
            double size = 0;
            double volume = 0;
            //Can we make this check downstream for static pressure?
            double staticpressure = 1;
            Plane Splane = new Plane();
            Plane Rplane = new Plane();

            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            if (!DA.GetData(0, ref HP)) return;
            if (!DA.GetData(1, ref size)) return;
            if (!DA.GetData(2, ref volume)) return;
            if (!DA.GetData(3, ref Splane)) return;
            if (!DA.GetData(3, ref Rplane)) return;

            Pachyderm_Noise_Control.ASHRAE.Equipment.FanType ftype;
            if (Centrifugal_Backward) ftype = Pachyderm_Noise_Control.ASHRAE.Equipment.FanType.Centrifugal_backward;
            else if (Centrifugal_Forward) ftype = Pachyderm_Noise_Control.ASHRAE.Equipment.FanType.Cenrifugal_forward;
            else if (Centrifugal_Radial) ftype = Pachyderm_Noise_Control.ASHRAE.Equipment.FanType.Centrifugal_Radial;
            else if (Propeller) ftype = Pachyderm_Noise_Control.ASHRAE.Equipment.FanType.Propeller;
            else if (Vaneaxial) ftype = Pachyderm_Noise_Control.ASHRAE.Equipment.FanType.Vaneaxial;
            else ftype = Pachyderm_Noise_Control.ASHRAE.Equipment.FanType.Tubeaxial;

            // We're set to create the spiral now. To keep the size of the SolveInstance() method small, 
            // The actual functionality will be in a different method:
            List<double> SWL = new List<double>( Pachyderm_Noise_Control.ASHRAE.Equipment.PackagedAHU_Radiated(HP));
            double[] Supply = Pachyderm_Noise_Control.ASHRAE.Equipment.Fan_Discharge(ftype, size, volume, staticpressure);

            Pachyderm_Noise_Control.State supplystate = new Pachyderm_Noise_Control.State(volume, double.Epsilon, double.Epsilon, Supply, Supply, new Hare.Geometry.Vector(Splane.Normal.X, Splane.Normal.Y, Splane.Normal.Z), new Hare.Geometry.Vector(Splane.XAxis.X, Splane.XAxis.Y, Splane.XAxis.Z), new Hare.Geometry.Vector(Splane.YAxis.X, Splane.YAxis.Y, Splane.YAxis.Z));
            Pachyderm_Noise_Control.State returnstate = new Pachyderm_Noise_Control.State(volume, double.Epsilon, double.Epsilon, Supply, Supply, new Hare.Geometry.Vector(Rplane.Normal.X, Rplane.Normal.Y, Rplane.Normal.Z), new Hare.Geometry.Vector(Rplane.XAxis.X, Rplane.XAxis.Y, Rplane.XAxis.Z), new Hare.Geometry.Vector(Rplane.YAxis.X, Rplane.YAxis.Y, Rplane.YAxis.Z));

            supplystate.PathId = Pachyderm_Noise_Control.Duct_Model.Instance.Intiate_Path(new Hare.Geometry.Point(Splane.OriginX, Splane.OriginY, Splane.OriginZ), supplystate.Noise_Best, Pachyderm_Noise_Control.Duct_Model.NoiseType.Equipment);
            returnstate.PathId = Pachyderm_Noise_Control.Duct_Model.Instance.Intiate_Path(new Hare.Geometry.Point(Rplane.OriginX, Rplane.OriginY, Rplane.OriginZ), supplystate.Noise_Best, Pachyderm_Noise_Control.Duct_Model.NoiseType.Equipment);

            DA.SetDataList(0, SWL);
            DA.SetData(1, supplystate);
            DA.SetData(2, returnstate);
        }

        public override void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            if (Supply == null || Return == null) return;

            double minx, miny, minz, maxx, maxy, maxz;

            if (Supply.Origin.X < Return.Origin.X)
            {
                minx = Supply.Origin.X;
                maxx = Return.Origin.X;
            }
            else
            {
                maxx = Supply.Origin.X;
                minx = Return.Origin.X;
            }
            if (Supply.Origin.Y < Return.Origin.Y)
            {
                miny = Supply.Origin.Y;
                maxy = Return.Origin.Y;
            }
            else
            {
                maxy = Supply.Origin.Y;
                miny = Return.Origin.Y;
            }
            if (Supply.Origin.Z < Return.Origin.Z)
            {
                minz = Supply.Origin.Z;
                maxz = Return.Origin.Z;
            }
            else
            {
                maxz = Supply.Origin.Z;
                minz = Return.Origin.Z;
            }

            List<Line> arrows = new List<Line>();
            arrows.Add(new Line(Supply.Origin, Supply.Origin + Supply.Normal));
            arrows.Add(new Line(Return.Origin, Return.Origin + Return.Normal));

            args.Display.DrawArrows(arrows, System.Drawing.Color.Red);
            args.Display.DrawBox(new BoundingBox(minx, miny, minz, maxx, maxy, maxz), System.Drawing.Color.Red); 
            base.DrawViewportMeshes(args);
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
            get { return new Guid("40cd025a-8507-4ca9-9d5f-382a3206a50a"); }
        }
    }
}
