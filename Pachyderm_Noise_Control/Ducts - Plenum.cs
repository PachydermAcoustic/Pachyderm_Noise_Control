using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pachyderm_Noise_Control
{
    class Ducts_Plenum
    {
        private static double[][] Plenum_conical_in_LDo_Return_Co;
        private static double[][] Plenum_conical_in_angle_Return_Co;
        private static double[] Plenum_bellmouth_out_Supply_Co;
        private static double[] Plenum_bellmouth_out_Supply_C1;
        private static double[][] Plenum_bellmouth2Rect_out_Supply_C0;
        public static void Initialize()
        {
            //ED2-1 Conical Diffuser, Round to Plenum, Exhaust/Return Systems
            //A1/Ao : 1.5 2 2.5 3 4 6 8 10 14 20 1000
            //Co Values
            //L /Do : 0.5 1.0 2.0 3.0 4.0 5.0 6.0 8.0 10.0 12.0 14.0
            Plenum_conical_in_LDo_Return_Co = new double[12][]
            {
                new double[11] {0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00 ,0.00 },
                new double[11] {0.03, 0.02, 0.03, 0.03, 0.04, 0.05, 0.06, 0.08, 0.10, 0.11, 0.13 },
                new double[11] {0.08, 0.06, 0.04, 0.04, 0.04, 0.05, 0.05, 0.06, 0.08, 0.09, 0.10 },
                new double[11] {0.13, 0.09, 0.06, 0.06, 0.06, 0.06, 0.06, 0.06, 0.07, 0.08, 0.09 },
                new double[11] {0.17, 0.12, 0.09, 0.07, 0.07, 0.06, 0.06, 0.07, 0.07, 0.08, 0.08 },
                new double[11] {0.23, 0.17, 0.12, 0.10, 0.09, 0.08, 0.08, 0.08, 0.08, 0.08, 0.08 },
                new double[11] {0.30, 0.22, 0.16, 0.13, 0.12, 0.10, 0.10, 0.09, 0.09, 0.09, 0.08 },
                new double[11] {0.34, 0.26, 0.18, 0.15, 0.13, 0.12, 0.11, 0.10, 0.09, 0.09, 0.09 },
                new double[11] {0.36, 0.28, 0.20, 0.16, 0.14, 0.13, 0.12, 0.11, 0.10, 0.09, 0.09 },
                new double[11] {0.39, 0.30, 0.22, 0.18, 0.16, 0.14, 0.13, 0.12, 0.10, 0.10, 0.10 },
                new double[11] {0.41, 0.32, 0.24, 0.20, 0.17, 0.15, 0.14, 0.12, 0.11, 0.11, 0.10 },
                new double[11] {0.41, 0.32, 0.24, 0.20, 0.17, 0.15, 0.14, 0.12, 0.11, 0.11, 0.10 }
            };
            //A1/Ao
            //Optimum Angle θ, degrees
            //0.5 1.0 2.0 3.0 4.0 5.0 6.0 8.0 10.0 12.0 14.0
            Plenum_conical_in_angle_Return_Co = new double[12][]
            {
            new double[11] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new double[11] {17, 10, 6.5, 4.5, 3.5, 2.8, 2.2, 1.7, 1.2, 1.0, 0.8 },
            new double[11] {21, 14, 8.5, 6.2, 5.0, 4.3, 3.8, 3.0, 2.3, 2.0, 1.6 },
            new double[11] {25, 16, 10, 7.4, 6.0, 5.4, 4.8, 4.0, 3.5, 3.0, 2.5 },
            new double[11] {27, 17, 11, 8.5, 7.0, 6.1, 5.6, 4.8, 4.2, 3.8, 3.2 },
            new double[11] {29, 20, 13, 9.8, 8.0, 7.2, 6.6, 5.8, 5.2, 4.8, 4.4 },
            new double[11] {31, 21, 14, 11, 9.4, 8.2, 7.4, 6.2, 5.6, 5.2, 4.7 },
            new double[11] {32, 22, 15, 12, 10, 8.8, 8.0, 6.6, 5.8, 5.4, 5.0 },
            new double[11] {33, 23, 15, 12, 11, 9.4, 8.4, 7.0, 6.2, 5.5, 5.2 },
            new double[11] {33, 24, 16, 13, 11, 9.6, 8.7, 7.3, 6.3, 5.6, 5.4 },
            new double[11] {34, 24, 16, 13, 11, 9.8, 9.0, 7.5, 6.5, 6.0, 5.6 },
            new double[11] {34, 24, 16, 13, 11, 9.8, 9.0, 7.5, 6.5, 6.0, 5.6 }
            };


            //SD1-1 Bellmouth, Plenum to Round, Supply Air Systems
            //r/Do 0.0 0.01 0.02 0.03 0.04 0.05 0.06 0.08 0.10 0.12 0.16 0.20 10.0
            Plenum_bellmouth_out_Supply_Co = new double[13] { 0.50, 0.44, 0.37, 0.31, 0.26, 0.22, 0.20, 0.15, 0.12, 0.09, 0.06, 0.03, 0.03 };

            //SD1 - 2 Conical Bellmouth/ Sudden Contraction,
            //Plenum to Round, Supply Air Systems
            //L / Do
            //Co Values
            //0 10 20 30 45 60 100 140 180
            Plenum_conical_in_angle_Return_Co = new double[7][]
            {
                new double[10] { 0.00, 0.50, 0.50, 0.50, 0.50, 0.50, 0.50, 0.50, 0.50, 0.50 },
                new double[10] { 0.025, 0.50, 0.47, 0.45, 0.43, 0.41, 0.40, 0.42, 0.45, 0.50 },
                new double[10] { 0.05, 0.50, 0.45, 0.41, 0.36, 0.33, 0.30, 0.35, 0.42, 0.50 },
                new double[10] { 0.075, 0.50, 0.42, 0.35, 0.30, 0.26, 0.23, 0.30, 0.40, 0.50 },
                new double[10] { 0.10, 0.50, 0.39, 0.32, 0.25, 0.22, 0.18, 0.27, 0.38, 0.50 },
                new double[10] { 0.15, 0.50, 0.37, 0.27, 0.20, 0.16, 0.15, 0.25, 0.37, 0.50 },
                new double[10] { 0.60, 0.50, 0.27, 0.18, 0.13, 0.11, 0.12, 0.23, 0.36, 0.50 }
            };

            //ER2 - 1 Bellmouth, Plenum to Round, Exhaust / Return Systems
            //r / D1, 0.0, 0.01, 0.02, 0.03, 0.04, 0.05, 0.06, 0.08, 0.10, 0.12, 0.16, 0.20, 10.0
            Plenum_bellmouth_out_Supply_C1 = new double[13] { 0.50, 0.44, 0.37, 0.31, 0.26, 0.22, 0.20, 0.15, 0.12, 0.09, 0.06, 0.03, 0.03 };
            //C0 = C1 * A_Plenum * A_Pleum / (ADuct * ADuct)


            //SR1 - 1 Conical Bellmouth/ Sudden Contraction, Plenum to Rectangular,
            //Supply Air Systems
            //L / Dh: 0, 0.025, 0.05, 0.075, 0.1, 0.15, 0.6
            //Co Values
            //0 10 20 30 40 60 100 140 180
            Plenum_bellmouth2Rect_out_Supply_C0 = new double[7][]
            {
                    new double[9] { 0.50, 0.50, 0.50, 0.50, 0.50, 0.50, 0.50, 0.50, 0.50 },
                    new double[9] { 0.50, 0.47, 0.45, 0.43, 0.41, 0.40, 0.42, 0.45, 0.50 },
                    new double[9] { 0.50, 0.45, 0.41, 0.36, 0.33, 0.30, 0.35, 0.42, 0.50 },
                    new double[9] { 0.50, 0.42, 0.35, 0.30, 0.26, 0.23, 0.30, 0.40, 0.50 },
                    new double[9] { 0.50, 0.39, 0.32, 0.25, 0.22, 0.18, 0.27, 0.38, 0.50 },
                    new double[9] { 0.50, 0.37, 0.27, 0.20, 0.16, 0.15, 0.25, 0.37, 0.50 },
                    new double[9] { 0.50, 0.27, 0.18, 0.13, 0.11, 0.12, 0.23, 0.36, 0.50 }
            };
        }
    }
}
