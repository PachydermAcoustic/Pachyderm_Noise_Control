using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Pachyderm_Noise_Control;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace Pachyderm_Noise_GH
{
    public class Ctrl_Display : GH_Component
    {
        Duct_Model_Manager DMM = Duct_Model_Manager.Instance;

        /// <summary>
        /// Initializes a new instance of the Duct_Straight class.
        /// </summary>
        public Ctrl_Display()
          : base("Display Ductwork", "Dspl-Duct",
              "Display the duct model in Rhino.",
              "Noise", "Control")
        {
            Duct_Model_Manager.Instance.displays.Add(this);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "Pts", "Points at a semi-regular spacing along duct run.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Total Sound Pressure Level", "TSPL", "Total Sound pressure level at each point in Pts.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Equipment Sound Pressure Level", "ESPL", "Sound pressure level at each point in Pts due to equipment alone.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Aerodynamic Sound Pressure Level", "ASPL", "Sound pressure level at each point in due to aerodynamic regenerated noise alone.", GH_ParamAccess.list);
            pManager.AddCurveParameter("Ducts", "D", "Curves representing the ductwork.", GH_ParamAccess.tree);
            pManager.AddNumberParameter("Breakout SPL", "BkSPL", "Sound pressure level at each breakout point.", GH_ParamAccess.list);
        }

        //protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        //{
        //    Menu_AppendItem(menu, "Total SWL", TotalSWL_Click, true, TSWL);
        //    Menu_AppendItem(menu, "Equipment SWL", EquSWL_Click, true, ESWL);
        //    Menu_AppendItem(menu, "Aerodynamic SWL", AeroSWL_Click, true, ASWL);
        //    base.AppendAdditionalComponentMenuItems(menu);
        //}

        //private void TotalSWL_Click(object sender, EventArgs e)
        //{
        //    TSWL = true;
        //    ESWL = false;
        //    ASWL = false;
        //    OnPingDocument().NewSolution(true);
        //}
        //private void EquSWL_Click(object sender, EventArgs e)
        //{
        //    TSWL = false;
        //    ESWL = true;
        //    ASWL = false;
        //    OnPingDocument().NewSolution(true);
        //}
        //private void AeroSWL_Click(object sender, EventArgs e)
        //{
        //    TSWL = false;
        //    ESWL = false;
        //    ASWL = true;
        //    OnPingDocument().NewSolution(true);
        //}
        //public bool TSWL = true;
        //public bool ESWL = false;
        //public bool ASWL = false;

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<List<Hare.Geometry.Point[]>> pts;
            List<Hare.Geometry.Point> ctrs;
            List<double[]> tspl;
            List<double[]> espl;
            List<double[]> aspl;
            List<double[]> spl_breakout;
            //if (TSWL) Duct_Model.Instance.WireDiagram(out ctrs, out pts, out spl, out spl_breakout, Duct_Model.NoiseType.Total);
            //else if (ESWL) Duct_Model.Instance.WireDiagram(out ctrs, out pts, out spl, out spl_breakout, Duct_Model.NoiseType.Equipment);
            //else Duct_Model.Instance.WireDiagram(out ctrs, out pts, out spl, out spl_breakout, Duct_Model.NoiseType.Aerodynamic);
            Duct_Model.Instance.WireDiagram(out ctrs, out pts, out tspl, out espl, out aspl, out spl_breakout);
            List<Point3d> PTS = new List<Point3d>();
            List<double> SPL = new List<double>();
            List<double> Breakout = new List<double>();
            DataTree<Curve> ductTree = new DataTree<Curve>();

            for (int i = 0; i < pts.Count; i++)
            {
                PTS.Add(new Point3d(ctrs[i].x, ctrs[i].y, ctrs[i].z));
                SPL.Add(tspl[i][0]);

                GH_Path path = new GH_Path(i);
                for (int j = 0; j < pts[i].Count; j++)
                {
                    Point3d a = new Point3d(pts[i][j][0].x, pts[i][j][0].y, pts[i][j][0].z);
                    Point3d b = new Point3d(pts[i][j][1].x, pts[i][j][1].y, pts[i][j][1].z);
                    PolylineCurve segment = new PolylineCurve(new[] { a, b });
                    ductTree.Add(segment, path);
                }

                Breakout.Add(spl_breakout[i][0]);
            }

            DA.SetDataList(0, PTS);
            DA.SetDataList(1, SPL);
            DA.SetDataList(2, espl);
            DA.SetDataList(3, aspl);
            DA.SetDataTree(4, ductTree);
            DA.SetDataList(5, spl_breakout);
        }

        public void Display()
        { 
            DMM.Displaying = true;
            this.ExpireSolution(true);
            DMM.Displaying = false;
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
            get { return new Guid("A502B04D-65AF-4958-AF48-67D8CF8E2CCC"); }
        }
    }
}