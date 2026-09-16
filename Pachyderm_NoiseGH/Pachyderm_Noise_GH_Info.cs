using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Pachyderm_Noise_GH
{
    public class PachNoiseGHInfo : GH_AssemblyInfo
    {
        public PachNoiseGHInfo()
            :base()
        {
            Grasshopper.Instances.DocumentServer.DocumentAdded += Duct_Model_Manager.InitializeDuctModelManager;
        }

        public override string Name
        {
            get
            {
                return "PachNoiseGH";
            }
        }
        //public override Bitmap Icon
        //{
        //    get
        //    {
        //        //Return a 24x24 pixel bitmap to represent this GHA library.
        //        return null;
        //    }
        //}
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "Common noise control algorithms for the purpose of simulatiing noise in built space with Pachyderm Acoustic.";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("e3a01449-332b-4a24-abad-4232e5e7431d");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Pachyderm Acoustic Simulation";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "Arthur.vanderharten@gmail.com";
            }
        }
    }
}
