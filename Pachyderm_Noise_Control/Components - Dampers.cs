using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pachyderm_Noise_Control
{
    namespace ASHRAE
    {
        public class Dampers
        {
            double C0_TypeC_FireDamper = 0.12;
            double C0_TypeB_FireDamper = 0.19;
            double C0_ParallelOpposed3VBlades_Damper = 0.37;
            double C0_ParallelOpposedAirfoil_Damper = 0.18;
            private static double[] Butterfly_D_C0;
            private static double[][] Butterfly_R_C0;

            public static void Initialize()
            {
                //CD9 - 1 Damper, Butterfly
                //θ 0 10 20 30 40 50 60 70 75 90
                Butterfly_D_C0 = new double[10] { 0.60, 0.85, 1.70, 4.0, 9.4, 24, 67, 215, 400, 9999 };

                //CR9 - 1 Damper, Butterfly
                //H / W 0.1, 0.5, 1, 1.5, 2
                //Co Values
                //θ 0 10 20 30 40 50 60 65 70 90
                Butterfly_R_C0 = new double[5][]
                    {
                        new double[10] { 0.04, 0.30, 1.10, 3.0, 8.0, 23.0, 60.0, 100.0, 190.0, 9999 },
                        new double[10] { 0.04, 0.30, 1.10, 3.0, 8.0, 23.0, 60.0, 100.0, 190.0, 9999 },
                        new double[10] { 0.04, 0.30, 1.10, 3.0, 8.0, 23.0, 60.0, 100.0, 190.0, 9999 },
                        new double[10] { 0.04, 0.35, 1.25, 3.6, 10.0, 29.0, 80.0, 155.0, 230.0, 9999 },
                        new double[10] { 0.04, 0.35, 1.25, 3.6, 10.0, 29.0, 80.0, 155.0, 230.0, 9999 }
                    };

            }

        }
    }
}