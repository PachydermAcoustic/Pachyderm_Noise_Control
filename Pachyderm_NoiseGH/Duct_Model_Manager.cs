using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

namespace Pachyderm_Noise_GH
{
    public class Duct_Model_Manager
    {
        private static Duct_Model_Manager instance = new Duct_Model_Manager();
        //public bool Complete = false;
        public List<Ctrl_Display> displays = new List<Ctrl_Display>();
        public bool Display_Needed = false;
        public bool Displaying = false;

        private Duct_Model_Manager()
        {                            
        }

        public static void InitializeDuctModelManager(GH_DocumentServer ds, GH_Document doc)
        {
            if (doc != null)
            {
                doc.SolutionStart -= Duct_Model_Manager.Reset_Model;
                doc.SolutionEnd -= Duct_Model_Manager.Propagate_Noise;
                doc.SolutionStart += Duct_Model_Manager.Reset_Model;
                doc.SolutionEnd += Duct_Model_Manager.Propagate_Noise;
            }
        }

        public static void Reset_Model(object O, GH_SolutionEventArgs X)
        {
            if (!Duct_Model_Manager.Instance.Displaying)
            {
//                Duct_Model_Manager.Instance.Display_needed = false;
                Pachyderm_Noise_Control.Duct_Model.Instance.reset();
            }
        }

        public static void Propagate_Noise(object O, GH_SolutionEventArgs X)
        {
            if (Duct_Model_Manager.Instance.Display_Needed)
            {
                Duct_Model_Manager.Instance.Display_Needed = false;
                Pachyderm_Noise_Control.Duct_Model.Instance.Propagate_Noise_Sources();
                Duct_Model_Manager.instance.Show();
            }
        }

        public void Show()
        {
            foreach (Ctrl_Display d in displays) d.Display();
        }

        public static Duct_Model_Manager Instance
        {
            get
            {
                if (instance == null) instance = new Duct_Model_Manager();
                return instance;
            }
        }
    }
}
