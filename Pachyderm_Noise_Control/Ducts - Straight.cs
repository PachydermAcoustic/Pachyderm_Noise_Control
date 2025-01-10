using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pachyderm_Noise_Control
{
    namespace ASHRAE
    {
        public static class Round_Duct
        {
            private static Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation> DP_transition_R_SQ2R;
            private static Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation> DP_transition_R_SQ2SQ;
            private static Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation> DP_transition_R_R2R;
            private static Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation> DP_transition_R_R2SQ;
            private static Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation> DP_transition_S_SQ2R;
            private static Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation> DP_transition_S_R2R;
            private static Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation> DP_transition_S_SQ2SQ;
            private static Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation> DP_transition_S_R2SQ;

            public static void Initialize()
            {
                double[] A0_A1 = new double[10] { 0.063, 01, 0.167, 0.25, 0.5, 1, 2, 4, 6, 10 };

                //ER4-1
                DP_transition_R_SQ2SQ = new Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation>();
                DP_transition_R_SQ2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
                DP_transition_R_SQ2SQ.Add(3, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.44, 0.41, 0.34, 0.26, 0.16, 0, 0.3, 1.66, 4.05, 12.01 }));
                DP_transition_R_SQ2SQ.Add(5, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.27, 0.27, 0.28, 0.29, 0.24, 0, 0.38, 1.25, 3.14, 9.39 }));
                DP_transition_R_SQ2SQ.Add(10, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.25, 0.23, 0.21, 0.17, 0.14, 0, 0.25, 0.77, 1.76, 5.33 }));
                DP_transition_R_SQ2SQ.Add(15, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.27, 0.25, 0.23, 0.19, 0.13, 0, 0.17, 0.7, 1.58, 5 }));
                DP_transition_R_SQ2SQ.Add(20, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.36, 0.34, 0.3, 0.25, 0.15, 0, 0.17, 0.7, 1.58, 5 }));
                DP_transition_R_SQ2SQ.Add(30, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.56, 0.53, 0.48, 0.42, 0.24, 0, 0.17, 0.7, 1.58, 5 }));
                DP_transition_R_SQ2SQ.Add(45, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.71, 0.69, 0.65, 0.6, 0.35, 0, 0.23, 0.9, 2.12, 6.45 }));
                DP_transition_R_SQ2SQ.Add(60, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.86, 0.83, 0.76, 0.68, 0.37, 0, 0.29, 1.09, 2.66, 7.93 }));
                DP_transition_R_SQ2SQ.Add(90, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.99, 0.94, 0.83, 0.7, 0.38, 0, 0.49, 2.84, 6.71, 19.1 }));
                DP_transition_R_SQ2SQ.Add(120, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.99, 0.94, 0.83, 0.7, 0.37, 0, 0.66, 4.36, 10.11, 28.6 }));
                DP_transition_R_SQ2SQ.Add(150, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.98, 0.92, 0.82, 0.68, 0.36, 0, 0.81, 5.69, 13.13, 36.79 }));
                DP_transition_R_SQ2SQ.Add(180, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.98, 0.91, 0.8, 0.66, 0.35, 0, 0.88, 6.57, 15.2, 42.79 }));

                //ER4-3
                DP_transition_R_SQ2R = new Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation>();
                DP_transition_R_SQ2R.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
                DP_transition_R_SQ2R.Add(3, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.17, 0.17, 0.18, 0.16, 0.14, 0, 0.3, 1.6, 3.89, 11.8 }));
                DP_transition_R_SQ2R.Add(5, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.19, 0.19, 0.19, 0.18, 0.14, 0, 0.27, 1.14, 3.04, 9.31 }));
                DP_transition_R_SQ2R.Add(10, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.3, 0.3, 0.3, 0.25, 0.15, 0, 0.26, 0.84, 1.84, 5.4 }));
                DP_transition_R_SQ2R.Add(15, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.46, 0.45, 0.44, 0.36, 0.22, 0, 0.28, 0.85, 1.77, 5.18 }));
                DP_transition_R_SQ2R.Add(20, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.53, 0.53, 0.53, 0.45, 0.25, 0, 0.25, 0.86, 1.78, 5.15 }));
                DP_transition_R_SQ2R.Add(30, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.64, 0.64, 0.63, 0.52, 0.3, 0, 0.19, 0.76, 1.73, 5.05 }));
                DP_transition_R_SQ2R.Add(45, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.77, 0.75, 0.72, 0.58, 0.33, 0, 0.23, 0.9, 2.18, 6.44 }));
                DP_transition_R_SQ2R.Add(60, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.88, 0.84, 0.78, 0.62, 0.33, 0, 0.27, 1.09, 2.67, 7.94 }));
                DP_transition_R_SQ2R.Add(90, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.95, 0.89, 0.79, 0.64, 0.33, 0, 0.52, 2.78, 6.67, 19.06 }));
                DP_transition_R_SQ2R.Add(120, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.95, 0.89, 0.79, 0.64, 0.32, 0, 0.75, 4.3, 10.07, 28.55 }));
                DP_transition_R_SQ2R.Add(150, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.94, 0.89, 0.79, 0.64, 0.31, 0, 0.91, 5.65, 13.09, 36.75 }));
                DP_transition_R_SQ2R.Add(180, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.93, 0.88, 0.79, 0.64, 0.3, 0, 0.95, 6.55, 15.18, 42.75 }));


                //ED4-1
                DP_transition_R_R2R = new Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation>();
                DP_transition_R_R2R.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
                DP_transition_R_R2R.Add(3, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.18, 0.2, 0.18, 0.2, 0.15, 0, 0.3, 1.6, 3.89, 11.8 }));
                DP_transition_R_R2R.Add(5, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.18, 0.18, 0.17, 0.17, 0.13, 0, 0.26, 1.14, 3.02, 9.3 }));
                DP_transition_R_R2R.Add(10, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.2, 0.2, 0.18, 0.16, 0.11, 0, 0.21, 0.75, 1.73, 5.3 }));
                DP_transition_R_R2R.Add(15, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.29, 0.27, 0.25, 0.21, 0.13, 0, 0.19, 0.7, 1.58, 5 }));
                DP_transition_R_R2R.Add(20, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.38, 0.38, 0.33, 0.30, 0.19, 0, 0.19, 0.7, 1.58, 5 }));
                DP_transition_R_R2R.Add(30, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.6, 0.59, 0.48, 0.46, 0.32, 0, 0.19, 0.7, 1.58, 5 }));
                DP_transition_R_R2R.Add(45, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.84, 0.76, 0.66, 0.61, 0.33, 0, 0.23, 0.9, 2.12, 6.45 }));
                DP_transition_R_R2R.Add(60, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.88, 0.8, 0.77, 0.68, 0.33, 0, 0.27, 1.09, 2.66, 7.9 }));
                DP_transition_R_R2R.Add(90, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.88, 0.83, 0.74, 0.64, 0.32, 0, 0.51, 2.78, 6.62, 19 }));
                DP_transition_R_R2R.Add(120, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.88, 0.84, 0.73, 0.63, 0.31, 0, 0.73, 4.29, 10.01, 28.5 }));
                DP_transition_R_R2R.Add(150, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.88, 0.83, 0.73, 0.62, 0.3, 0, 0.9, 5.63, 13.03, 36.7 }));
                DP_transition_R_R2R.Add(180, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.88, 0.83, 0.72, 0.62, 0.3, 0, 0.95, 6.53, 15.12, 42.7 }));

                //ED4-2
                DP_transition_R_R2SQ = new Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation>();
                DP_transition_R_R2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
                DP_transition_R_R2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.17, 0.17, 0.18, 0.16, 0.14, 0, 0.3, 1.6, 3.89, 11.8 }));
                DP_transition_R_R2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.19, 0.19, 0.19, 0.18, 0.14, 0, 0.27, 1.14, 3.04, 9.31 }));
                DP_transition_R_R2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.3, 0.3, 0.3, 0.25, 0.15, 0, 0.26, 0.84, 1.84, 5.4 }));
                DP_transition_R_R2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.46, 0.45, 0.44, 0.36, 0.22, 0, 0.28, 0.85, 1.77, 5.18 }));
                DP_transition_R_R2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.53, 0.53, 0.53, 0.45, 0.25, 0, 0.25, 0.86, 1.78, 5.15 }));
                DP_transition_R_R2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.64, 0.64, 0.63, 0.52, 0.3, 0, 0.19, 0.76, 1.73, 5.05 }));
                DP_transition_R_R2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.77, 0.75, 0.72, 0.58, 0.33, 0, 0.23, 0.9, 2.18, 6.44 }));
                DP_transition_R_R2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.88, 0.84, 0.78, 0.62, 0.33, 0, 0.27, 1.09, 2.67, 7.94 }));
                DP_transition_R_R2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.95, 0.89, 0.79, 0.64, 0.33, 0, 0.52, 2.78, 6.67, 19.06 }));
                DP_transition_R_R2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.95, 0.89, 0.79, 0.64, 0.32, 0, 0.75, 4.3, 10.07, 28.55 }));
                DP_transition_R_R2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.94, 0.89, 0.79, 0.64, 0.31, 0, 0.91, 5.65, 13.09, 36.75 }));
                DP_transition_R_R2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.93, 0.88, 0.79, 0.64, 0.3, 0, 0.95, 6.55, 15.18, 42.75 }));

                //SD4-2
                A0_A1 = new double[10] { 01, 0.167, 0.25, 0.5, 1, 2, 4, 6, 10, 16 };
                DP_transition_S_SQ2R = new Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation>();
                DP_transition_S_SQ2R.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
                DP_transition_S_SQ2R.Add(3, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.12, 0.11, 0.1, 0.08, 0, 0.57, 2.6, 6.57, 17.25, 42.75 }));
                DP_transition_S_SQ2R.Add(5, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.09, 0.08, 0.07, 0.07, 0, 0.55, 2.84, 6.75, 18.75, 48.13 }));
                DP_transition_S_SQ2R.Add(10, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.05, 0.05, 0.05, 0.06, 0, 0.61, 3.92, 10.62, 30, 77.57 }));
                DP_transition_S_SQ2R.Add(15, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.05, 0.05, 0.05, 0.07, 0, 0.87, 5.72, 15.84, 45, 116.74 }));
                DP_transition_S_SQ2R.Add(20, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.05, 0.05, 0.05, 0.06, 0, 1, 7.2, 18.9, 53, 136.45 }));
                DP_transition_S_SQ2R.Add(30, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.05, 0.05, 0.05, 0.05, 0, 1.2, 8.32, 22.5, 63.5, 164.1 }));
                DP_transition_S_SQ2R.Add(45, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.06, 0.06, 0.06, 0.06, 0, 1.3, 9.28, 25.74, 75, 196.86 }));
                DP_transition_S_SQ2R.Add(60, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.08, 0.07, 0.07, 0.07, 0, 1.3, 9.92, 27.9, 84, 224.26 }));
                DP_transition_S_SQ2R.Add(90, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.19, 0.19, 0.17, 0.13, 0, 1.3, 10.24, 28.44, 89, 241.92 }));
                DP_transition_S_SQ2R.Add(120, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.29, 0.28, 0.27, 0.19, 0, 1.28, 10.24, 28.44, 89, 241.92 }));
                DP_transition_S_SQ2R.Add(120, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.37, 0.37, 0.35, 0.23, 0, 1.24, 10.24, 28.35, 88.5, 240.38 }));
                DP_transition_S_SQ2R.Add(120, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.43, 0.42, 0.41, 0.24, 0, 1.2, 10.24, 28.26, 88, 238.59 }));

                //SD4-1
                A0_A1 = new double[12] { 01, 0.167, 0.25, 0.39, 0.5, 0.64, 1, 2, 4, 6, 10, 16 };
                DP_transition_S_R2R = new Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation>();
                DP_transition_S_R2R.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
                DP_transition_S_R2R.Add(3, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[12] { 0.12, 0.11, 0.1, 0.1, 0.07, 0.07, 0, 0.59, 3.15, 6.55, 19.5, 45.82 }));
                DP_transition_S_R2R.Add(5, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[12] { 0.09, 0.08, 0.07, 0.07, 0.06, 0.07, 0, 0.51, 2.78, 6.08, 18.25, 44.8 }));
                DP_transition_S_R2R.Add(10, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[12] { 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0, 0.41, 2.51, 6.44, 20, 50.18 }));
                DP_transition_S_R2R.Add(15, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[12] { 0.05, 0.04, 0.04, 0.05, 0.05, 0.04, 0, 0.52, 3.38, 9.14, 27.3, 73.73 }));
                DP_transition_S_R2R.Add(20, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[12] { 0.05, 0.04, 0.04, 0.05, 0.05, 0.04, 0, 0.76, 4.77, 11.92, 38, 96.77 }));
                DP_transition_S_R2R.Add(30, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[12] { 0.05, 0.04, 0.04, 0.05, 0.05, 0.04, 0, 1.26, 7.38, 17.35, 58.5, 153.6 }));
                DP_transition_S_R2R.Add(45, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[12] { 0.06, 0.06, 0.06, 0.06, 0.06, 0.05, 0, 1.32, 9.7, 23.58, 76, 215.04 }));
                DP_transition_S_R2R.Add(60, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[12] { 0.08, 0.07, 0.07, 0.06, 0.07, 0.06, 0, 1.3, 10.88, 27.58, 80, 225.28 }));
                DP_transition_S_R2R.Add(90, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[12] { 0.019, 0.18, 0.17, 0.16, 0.13, 0.09, 0, 1.26, 10.29, 26.71, 83.4, 225.28 }));
                DP_transition_S_R2R.Add(120, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[12] { 0.29, 0.28, 0.27, 0.25, 0.18, 0.13, 0, 1.23, 10.08, 26.32, 84, 225.28 }));
                DP_transition_S_R2R.Add(150, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[12] { 0.37, 0.36, 0.35, 0.32, 0.23, 0.17, 0, 1.21, 9.96, 26.15, 83.35, 225.28 }));
                DP_transition_S_R2R.Add(180, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[12] { 0.43, 0.42, 0.41, 0.36, 0.24, 0.19, 0, 1.19, 9.84, 25.99, 82.7, 225.28 }));

                //SR4-1
                A0_A1 = new double[10] { 0.1, 0.167, 0.25, 0.5, 1, 2, 4, 6, 10, 16 };
                DP_transition_S_SQ2SQ = new Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation>();
                DP_transition_S_R2R.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
                DP_transition_S_R2R.Add(3, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.12, 0.11, 0.1, 0.08, 0, 0.64, 4.16, 12.24, 40.5, 112.64 }));
                DP_transition_S_R2R.Add(5, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.09, 0.09, 0.08, 0.09, 0, 0.96, 4.64, 10.08, 27.2, 68.35 }));
                DP_transition_S_R2R.Add(10, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.05, 0.05, 0.05, 0.06, 0, 0.54, 2.72, 7.38, 23.3, 63.74 }));
                DP_transition_S_R2R.Add(15, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.05, 0.04, 0.04, 0.04, 0, 0.52, 3.09, 8.1, 25.1, 67.84 }));
                DP_transition_S_R2R.Add(20, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.05, 0.04, 0.04, 0.04, 0, 0.62, 4, 10.8, 34, 92.93 }));
                DP_transition_S_R2R.Add(30, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.05, 0.04, 0.04, 0.04, 0, 0.94, 6.72, 17.28, 52.84, 142.13 }));
                DP_transition_S_R2R.Add(45, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.06, 0.06, 0.06, 0.06, 0, 1.4, 9.6, 23.4, 69, 182.53 }));
                DP_transition_S_R2R.Add(60, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.08, 0.07, 0.07, 0.07, 0, 1.48, 10.88, 27.36, 82.5, 220.16 }));
                DP_transition_S_R2R.Add(90, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.19, 0.19, 0.18, 0.12, 0, 1.52, 11.2, 29.88, 93.5, 254.21 }));
                DP_transition_S_R2R.Add(120, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.29, 0.28, 0.27, 0.17, 0, 1.48, 11.2, 29.88, 93.5, 254.21 }));
                DP_transition_S_R2R.Add(150, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.37, 0.36, 0.36, 0.2, 0, 1.44, 10.88, 29.34, 92.4, 251.9 }));
                DP_transition_S_R2R.Add(180, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.43, 0.42, 0.41, 0.27, 1, 1.4, 10.56, 28.8, 91.3, 249.6 }));

                //SR4-3
                DP_transition_S_R2SQ = new Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation>();
                DP_transition_S_R2SQ.Add(0, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
                DP_transition_S_R2SQ.Add(3, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.12, 0.11, 0.1, 0.08, 0, 0.57, 2.6, 6.57, 17.25, 42.75 }));
                DP_transition_S_R2SQ.Add(5, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.09, 0.08, 0.07, 0.07, 0, 0.55, 2.84, 6.75, 18.75, 48.13 }));
                DP_transition_S_R2SQ.Add(10, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.05, 0.05, 0.05, 0.06, 0, 0.61, 3.92, 10.62, 30, 77.57 }));
                DP_transition_S_R2SQ.Add(15, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.05, 0.05, 0.05, 0.07, 0, 0.87, 5.72, 15.84, 45, 116.74 }));
                DP_transition_S_R2SQ.Add(20, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.05, 0.05, 0.05, 0.06, 0, 1, 7.2, 18.9, 53, 136.45 }));
                DP_transition_S_R2SQ.Add(30, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.05, 0.05, 0.05, 0.05, 0, 1.2, 8.32, 22.5, 63.5, 164.1 }));
                DP_transition_S_R2SQ.Add(45, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.06, 0.06, 0.06, 0.06, 0, 1.3, 9.28, 25.74, 75, 196.86 }));
                DP_transition_S_R2SQ.Add(60, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.08, 0.07, 0.07, 0.07, 0, 1.3, 9.92, 27.9, 84, 224.26 }));
                DP_transition_S_R2SQ.Add(90, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.19, 0.19, 0.17, 0.13, 0, 1.3, 10.24, 28.44, 89, 241.92 }));
                DP_transition_S_R2SQ.Add(120, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.29, 0.28, 0.27, 0.19, 0, 1.28, 10.24, 28.44, 89, 241.92 }));
                DP_transition_S_R2SQ.Add(150, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.37, 0.37, 0.35, 0.23, 0, 1.24, 10.24, 28.35, 88.5, 240.38 }));
                DP_transition_S_R2SQ.Add(180, MathNet.Numerics.Interpolate.CubicSpline(A0_A1, new double[10] { 0.43, 0.42, 0.41, 0.24, 0, 1.2, 10.24, 28.26, 88, 238.59 }));
            }

            //Constants for unlined ducts...
            static double[][] attenuation0 = new double[5][]
            {
                new double[8]{.03, .03, .05, .05, .1, .1, .1, .1 }, // 4 inches
                new double[8]{.03, .03, .05, .05, .1, .1, .1, .1 }, //7 inches
                new double[8]{.03, .03, .03, .05, .07, .07, .07, .07 }, //15 inches
                new double[8]{.02, .02, .02, .03, .05, .05, .05, .05 }, //30 inches
                new double[8]{.01, .01, .02, .02, .02, .02, .02, .02 }, //60 inches
            };

            //Constants for 1" lined ducts...
            static double[][] attenuation1 = new double[28][]
            {
                new double[8]{ 0.38, 0.59, 0.93, 1.53, 2.17, 2.31, 2.04, 1.26 },//6
                new double[8]{ 0.32, 0.54, 0.89, 1.50, 2.19, 2.17, 1.83, 1.18 },//8
                new double[8]{ 0.27, 0.50, 0.85, 1.48, 2.20, 2.04, 1.64, 1.12 },//10
                new double[8]{ 0.23, 0.46, 0.81, 1.45, 2.18, 1.91, 1.48, 1.05 },//12
                new double[8]{ 0.19, 0.42, 0.77, 1.43, 2.14, 1.79, 1.34, 1.00 },//14
                new double[8]{ 0.16, 0.38, 0.73, 1.40, 2.08, 1.67, 1.21, 0.95 },//16
                new double[8]{ 0.13, 0.35, 0.69, 1.37 ,2.01, 1.56, 1.10, 0.90 },//18
                new double[8]{ 0.11, 0.31, 0.65, 1.34, 1.92, 1.45, 1.00, 0.87 },//20
                new double[8]{ 0.08, 0.28, 0.61 ,1.31 ,1.82, 1.34, 0.92, 0.83},//22
                new double[8]{ 0.07, 0.25, 0.57 ,1.28 ,1.71, 1.24, 0.85, 0.80},//24
                new double[8]{ 0.05, 0.22, 0.53, 1.24, 1.59, 1.14, 0.79, 0.77},//26
                new double[8]{ 0.03, 0.19, 0.49, 1.20, 1.46, 1.04, 0.74, 0.74},//28
                new double[8]{ 0.02, 0.16, 0.45, 1.16, 1.33, 0.95, 0.69, 0.71},//30
                new double[8]{ 0.01, 0.14, 0.42, 1.12, 1.20, 0.87, 0.66, 0.69},//32
                new double[8]{ 0, 0.11, 0.38, 1.07, 1.07, 0.79, 0.63, 0.66},//34
                new double[8]{0, 0.08, 0.35, 1.02, 0.93, 0.71, 0.60, 0.64},//36
                new double[8]{0, 0.06, 0.31, 0.96, 0.80, 0.64, 0.58, 0.61},//38
                new double[8]{0, 0.03, 0.28, 0.91, 0.68, 0.57, 0.55, 0.58},//40
                new double[8]{0, 0.01, 0.25, 0.84, 0.56, 0.50, 0.53, 0.55},//42
                new double[8]{0, 0, 0.23, 0.78, 0.45,0.44, 0.51, 0.52},//44
                new double[8]{0, 0, 0.20, 0.71, 0.35, 0.39, 0.48, 0.48},//46
                new double[8]{0, 0, 0.18, 0.63, 0.26, 0.34, 0.45, 0.44},//48
                new double[8]{0, 0, 0.15, 0.55, 0.19, 0.29, 0.41, 0.40},//50
                new double[8]{0, 0, 0.14, 0.46, 0.13, 0.25, 0.37, 0.34},//52
                new double[8]{0, 0, 0.12, 0.37, 0.09, 0.22, 0.31, 0.29},//54
                new double[8]{0, 0, 0.10, 0.28, 0.08, 0.18, 0.25, 0.22},//56
                new double[8]{0, 0, 0.09, 0.17, 0.08,0.16, 0.18, 0.15},//58
                new double[8]{0, 0, 0.08, 0.06, 0.10, 0.14, 0.09, 0.07}//60
            };

            //Constants for unlined ducts...
            static double[][] attenuation2 = new double[28][]{
                new double[8]{ 0.56, 0.80, 1.37, 2.25, 2.17, 2.31, 2.04, 1.26},//6
                new double[8]{ 0.51, 0.75, 1.33, 2.23, 2.19, 2.17, 1.83, 1.18},//8
                new double[8]{ 0.46, 0.71, 1.29, 2.20, 2.20, 2.04, 1.64, 1.12},//10
                new double[8]{ 0.42, 0.67, 1.25, 2.18, 2.18, 1.91, 1.48, 1.05},//12
                new double[8]{ 0.38, 0.63, 1.21, 2.15, 2.14, 1.79, 1.34, 1.00},//14
                new double[8]{ 0.35, 0.59, 1.17, 2.12, 2.08, 1.67, 1.21, 0.95},//16
                new double[8]{ 0.32, 0.56, 1.13, 2.10, 2.01, 1.56, 1.10, 0.90},//18
                new double[8]{ 0.29, 0.52, 1.09, 2.07, 1.92, 1.45, 1.00, 0.87},//20
                new double[8]{ 0.27, 0.49, 1.05, 2.03, 1.82, 1.34, 0.92, 0.83},//22
                new double[8]{ 0.25, 0.46, 1.01, 2.00, 1.71, 1.24, 0.85, 0.80},//24
                new double[8]{ 0.24, 0.43, 0.97, 1.96, 1.59, 1.14, 0.79, 0.77},//26
                new double[8]{ 0.22, 0.40, 0.93, 1.93, 1.46, 1.04, 0.74, 0.74},//28
                new double[8]{ 0.21, 0.37, 0.90, 1.88, 1.33, 0.95, 0.69, 0.71},//30
                new double[8]{ 0.20, 0.34, 0.86, 1.84, 1.20, 0.87, 0.66, 0.69},//32
                new double[8]{ 0.19, 0.32, 0.82, 1.79, 1.07, 0.79, 0.63, 0.66},//34
                new double[8]{ 0.18, 0.29, 0.79, 1.74, 0.93, 0.71, 0.60, 0.64},//36
                new double[8]{ 0.17, 0.27, 0.76, 1.69, 0.80, 0.64, 0.58, 0.61},//38
                new double[8]{ 0.16, 0.24, 0.73, 1.63, 0.68, 0.57, 0.55, 0.58},//40
                new double[8]{ 0.15, 0.22, 0.70, 1.57, 0.56, 0.50, 0.53, 0.55},//42
                new double[8]{ 0.13, 0.20, 0.67, 1.50, 0.45, 0.44, 0.51, 0.52},//44
                new double[8]{ 0.12, 0.17, 0.64, 1.43, 0.35, 0.39, 0.48, 0.48},//46
                new double[8]{ 0.11, 0.15, 0.62, 1.36, 0.26, 0.34, 0.45, 0.44},//48
                new double[8]{ 0.09, 0.12, 0.60, 1.28, 0.19, 0.29, 0.41, 0.40},//50
                new double[8]{ 0.07, 0.10, 0.58, 1.19, 0.13, 0.25, 0.37, 0.34},//52
                new double[8]{ 0.05, 0.08, 0.56, 1.10, 0.09, 0.22, 0.31, 0.29},//54
                new double[8]{ 0.02, 0.05, 0.55, 1.00, 0.08, 0.18, 0.25, 0.22},//56
                new double[8]{ 0.00, 0.03, 0.53, 0.90, 0.08, 0.16, 0.18, 0.15},//58
                new double[8]{ 0.00, 0.00, 0.53, 0.79, 0.10, 0.14, 0.09, 0.07}//60
            };


            public static State Straight_Duct(State S, double diameter, double length, double Lining_thickness, out double[] atten)
            {
                double[] a_ft1;
                double[] a_ft2 = new double[8];

                double interp1, interp2;

                if (Lining_thickness <= 0)
                {
                    interp1 = 1;
                    interp2 = 0;
                }
                else if (Lining_thickness < 1)
                {
                    interp1 = 1 - Lining_thickness;
                    interp2 = Lining_thickness;
                }
                else if (Lining_thickness < 2)
                {
                    interp1 = 2 - Lining_thickness;
                    interp2 = Lining_thickness - 1;
                }
                else
                {
                    interp1 = 0;
                    interp2 = 1;
                }

                if (Lining_thickness < 1)
                {
                    //Ashrae
                    if (diameter < 4) a_ft1 = attenuation0[0];
                    else if (diameter < 7) a_ft1 = attenuation0[1];
                    else if (diameter < 15) a_ft1 = attenuation0[2];
                    else if (diameter < 30) a_ft1 = attenuation0[3];
                    else a_ft1 = attenuation0[4];

                    int idx = (int)Math.Max(0, Math.Ceiling((diameter - 6) / 2));
                    a_ft2 = attenuation1[Math.Min(idx, attenuation1.Length - 1)];
                }
                else
                {
                    int idx = (int)Math.Max(0, Math.Ceiling((diameter - 6) / 2));
                    a_ft1 = attenuation1[Math.Min(idx, attenuation1.Length - 1)];
                    a_ft2 = attenuation2[Math.Min(idx, attenuation2.Length - 1)];
                }

                double[] totalAtten = new double[16];
                for (int oct = 0; oct < 8; oct++)
                {
                    totalAtten[oct] = (a_ft1[oct] * interp1 + a_ft2[oct] * interp2) * length;
                }

                atten = totalAtten;

                State S_new = S.Clone() as State - totalAtten;
                S_new.Resize(diameter);
                S_new.lining_thickness = Lining_thickness;

                List<Hare.Geometry.Point> Pts = new List<Hare.Geometry.Point>();
                int ptct = (int)Math.Ceiling(length * 0.3048 / Duct_Model.Instance.delta_D);
                double delta = length * 0.3048 / ptct;
                for (int i = 1; i < ptct + 1; i++) Pts.Add(S.Location + S.Direction * delta * i);
                Duct_Model.Instance.AddComponent(S.ModelNode, atten, Pts.ToArray(), ref S_new);

                return S_new;
            }

            //public static State Unlined(State S, double diameter, double length)
            //{
            //    double[] a_ft1;
            //    double[] a_ft2 = new double[8];

            //    //Ashrae
            //    if (diameter < 4)
            //    {
            //        a_ft1 = attenuation0[0];
            //    }
            //    else if (diameter < 7)
            //    {
            //        a_ft1 = attenuation0[1];
            //    }
            //    else if (diameter < 15)
            //    {
            //        a_ft1 = attenuation0[2];
            //    }
            //    else if (diameter < 30)
            //    {
            //        a_ft1 = attenuation0[3];
            //    }
            //    else
            //    {
            //        a_ft1 = attenuation0[4];
            //    }

            //    //Algorithms for HVAC Acoustics Manual
            //    double p_a = Math.PI * diameter / (Math.PI * diameter * diameter / 4);
            //    if (p_a < 3)
            //    {
            //        for (int oct = 0; oct < 8; oct++)
            //        {
            //            a_ft2[oct] = 1.64 * (Math.Pow(p_a, 0.73)) * Math.Pow(62.5 * Math.Pow(2, oct), -0.58);
            //        }
            //    }
            //    else
            //    {
            //        for (int oct = 0; oct < 8; oct++)
            //        {
            //            a_ft2[oct] = 17 * (Math.Pow(p_a, -0.25)) * Math.Pow(62.5 * Math.Pow(2, oct), -0.85);
            //        }
            //    }

            //    //Take best and worst...
            //    double[] totalAtten = new double[16];
            //    for (int oct = 0; oct < 8; oct++)
            //    {
            //        double atten1 = length * a_ft1[oct];
            //        double atten2 = length * a_ft2[oct];
            //        totalAtten[oct] = Math.Max(atten1, atten2);
            //        totalAtten[oct + 8] = Math.Min(atten1, atten2);
            //    }
            //    return S - totalAtten;
            //}

            //public static State Lined_OneInch(State S, double diameter, double length)
            //{
            //    int idx = (int)Math.Max(0, Math.Ceiling((diameter - 6) / 2));
            //    double[] totalAtten = new double[8];
            //    for (int oct = 0; oct < 8; oct++) totalAtten[oct] = length * attenuation1[idx][oct];
            //    return S - totalAtten;
            //}

            //public static State Lined_TwoInch(State S, double diameter, double length)
            //{
            //    int idx = (int)Math.Max(0, Math.Ceiling((diameter - 6) / 2));
            //    double[] totalAtten = new double[8];
            //    for (int oct = 0; oct < 8; oct++) totalAtten[oct] = length * attenuation2[idx][oct];
            //    return S - totalAtten;
            //}
        }

        public static class Rectangular_Duct
        {
            //Constants for unlined ducts...
            static double[][] attenuation = new double[6][]{
                new double[8]{.1, .1, .05, .02, .02, .02, .02, .02 }, // P/A = 0.7 1/ft
                new double[8]{.15, .1, .07, .02, .02, .02, .02, .02 }, // P/A = 1 1/ft
                new double[8]{.25, .2, .1, .03, .03, .03, .03, .03 }, // P/A = 2 1/ft
                new double[8]{.4, .2, .1, .05, .05, .05, .05, .05 }, // P/A = 3 1/ft
                new double[8]{.35, .2, .1, .06, .06, .06, .06, .06 }, // P/A = 4 1/ft
                new double[8]{.3, .2, .1, .1, .1, .1, .1, .1 }, // P/A = 8 1/ft
                };

            public static double[] Unlined_Atten(double dim1, double dim2)
            {
                double P_A = 2 * (dim1 + dim2) / (dim1 * dim2);
                double[] atten = new double[8];
                double[] bounds = new double[6] { 0.7, 1, 2, 3, 4, 8 };

                if (P_A < bounds[0]) for (int oct = 0; oct < 8; oct++) atten[oct] = attenuation[0][oct] * (P_A / 0.7);
                else if (P_A <= bounds[bounds.Length - 1])
                {
                    for (int i = 1; i < bounds.Length; i++)
                    {
                        if (P_A < bounds[i])
                        {
                            double width = bounds[i] - bounds[i - 1];
                            for (int oct = 0; oct < 8; oct++) atten[oct] = attenuation[i][oct] * (Math.Abs(P_A - bounds[i - 1])) + attenuation[i - 1][oct] * (Math.Abs(P_A - bounds[i]));
                            break;
                        }
                    }
                }
                else
                {
                    atten = attenuation[attenuation.Length - 1].Clone() as double[];
                }
                return atten;
            }

            //Constants for 1" lined ducts...
            //static double[][][] attenuation1 = new double[7][][]
            //{
            //    new double[4][]{ new double[8]{ 0.5, 0.5, 1.2, 2.3, 5.0, 5.8, 3.6, 3.6 }, new double[8]{ 0.4, 0.4, 1.0, 2.1, 4.5, 4.9, 3.2, 3.2 }, new double[8]{ 0.4, 0.4, 0.9, 2.0, 4.3, 4.5, 3.0, 3.0 }, new double[8]{ 0.4, 0.4, 0.8, 1.9, 4.0, 4.1, 2.8, 2.8 }},
            //    new double[4][]{ new double[8]{ 0.4, 0.4, 0.8, 1.9, 4, 4.1, 2.8, 2.8 }, new double[8]{ 0.3, 0.3, 0.7, 1.7, 3.7, 3.5, 2.5, 2.5 }, new double[8]{ 0.3, 0.3, 0.6, 1.7, 3.5, 3.2, 2.3, 2.3 }, new double[8]{ 0.3, 0.3, 0.6, 1.6, 3.3, 2.9, 2.2, 2.2 }},
            //    new double[4][]{ new double[8]{ 0.3, 0.3, 0.6, 1.6, 3.3, 2.9, 2.2, 2.2 }, new double[8]{ 0.2, 0.2, 0.5, 1.4, 3.0, 2.4, 1.9, 1.9 }, new double[8]{ 0.2, 0.2, 0.5, 1.4, 2.8, 2.2, 1.8, 1.8 }, new double[8]{ 0.2, 0.2, 0.4, 1.3, 2.7, 2.0, 1.7, 1.7 }},
            //    new double[4][]{ new double[8]{0.2, 0.2, 0.5, 1.4, 2.8, 2.2, 1.8, 1.8}, new double[8]{0.2, 0.2, 0.4, 1.2, 2.6, 1.9, 1.6, 1.6}, new double[8]{0.2, 0.2, 0.4, 1.1, 2.4, 1.7, 1.5, 1.5}, new double[8]{0.2, 0.2, 0.3, 1.1, 2.3, 1.6, 1.4, 1.4}},
            //    new double[4][]{ new double[8]{0.2, 0.2, 0.4, 1.2, 2.5, 1.8, 1.6, 1.6}, new double[8]{0.2, 0.2, 0.3, 1.1, 2.3, 1.6, 1.4, 1.4}, new double[8]{0.2, 0.2, 0.3, 1.1, 2.2, 1.4, 1.3, 1.3}, new double[8]{0.1, 0.1, 0.3, 1.0, 2.1, 1.3, 1.2, 1.2}},
            //    new double[4][]{ new double[8]{0.2, 0.2, 0.3, 1.0, 2.1, 1.4, 1.3, 1.3}, new double[8]{0.1, 0.1, 0.2, 0.9, 1.9, 1.2, 1.1, 1.1}, new double[8]{0.1, 0.1, 0.2, 0.8, 1.8, 1.1, 1.1, 1.1}, new double[8]{0.1, 0.1, 0.2, 0.8, 1.7, 1.0, 1.0, 1.0}},
            //    new double[4][]{ new double[8]{0.1, 0.1, 0.9, 1.0, 2.0, 1.2, 1.2, 1.2}, new double[8]{0.1, 0.1, 0.2, 0.9, 1.8, 1.0, 1.0, 1.0}, new double[8]{0.1, 0.1, 0.2, 0.8, 1.7, 1.0, 1.0, 1.0}, new double[8]{0.1, 0.1, 0.2, 0.8, 1.6, 0.9, 0.9, 0.9}}
            //};

            static double[][] bounds = new double[5][] { new double[11]{ 6, 8, 10, 12, 15, 18, 24, 30, 36, 42, 48 }, new double[11]{ 10, 12, 16, 18, 22, 28, 36, 45, 54, 64, 72 }, new double[11]{ 12, 16, 20, 24, 30, 36, 48, 60, 72, 84, 96 }, new double[11]{ 18, 24, 30, 36, 45, 54, 72, 90, 108, 126, 144 }, new double[11] { 18, 24, 30, 36, 45, 54, 72, 90, 108, 126, 144 } };

            static double[][][] attenuation1 = new double[11][][]
            {
                new double[4][]{ new double[8]{ .6, .6, 1.5, 2.7, 5.8, 7.4, 4.3, 4.3 }, new double[8]{0.5, 0.5 ,1.2 ,2.4 ,5.1 ,6.1 ,3.7, 3.7 }, new double[8]{0.5,0.5 ,1.2 ,2.3 ,5.0 ,5.8 ,3.6, 3.6 }, new double[8]{0.5,0.5 ,1.0 ,2.2 ,4.7 ,5.2 ,3.3, 3.3 } },
                new double[4][]{ new double[8]{0.5, 0.5 ,1.2 ,2.3 , 5.0 ,5.8 ,3.6, 3.6 }, new double[8]{0.4, 0.4 ,1.0 ,2.1 ,4.5 ,4.9 ,3.2, 3.2 }, new double[8]{0.4, 0.4 ,0.9 ,2.0 ,4.3 ,4.5 ,3.0, 3.0 }, new double[8]{0.4, 0.4 ,0.8 ,1.9 ,4.0 ,4.1 ,2.8, 2.8 } },
                new double[4][]{ new double[8]{0.4, 0.4 ,1.0 ,2.1 , 4.4 ,4.7 ,3.1, 3.1 }, new double[8]{0.4, 0.4 ,0.8 ,1.9 ,4.0 ,4.0 ,2.7, 2.7 }, new double[8]{0.3, 0.3 ,0.8 ,1.8 ,3.8 ,3.7 ,2.6, 2.6 }, new double[8]{0.3, 0.3 ,0.7 ,1.7 ,3.6 ,3.3 ,2.4, 2.4 } },
                new double[4][]{ new double[8]{0.4, 0.4 ,0.8 ,1.9 ,4.0 ,4.1 ,2.8, 2.8 }, new double[8]{0.3, 0.3 ,0.7 ,1.7 ,3.7 ,3.5 ,2.5, 2.5 }, new double[8]{0.3, 0.3 ,0.6 ,1.7 ,3.5 ,3.2 ,2.3, 2.3 }, new double[8]{0.3, 0.3 ,0.6 ,1.6 ,3.3 ,2.9 ,2.2, 2.2} },
                new double[4][]{ new double[8]{0.3, 0.3 ,0.7 ,1.7 ,3.6 ,3.3 ,2.4, 2.4 }, new double[8]{0.3,0.3 ,0.6 ,1.6 ,3.3 ,2.9 ,2.2, 2.2 }, new double[8]{0.3 ,0.3 ,0.5 ,1.5 ,3.1 ,2.6 ,2.0, 2.0 }, new double[8]{0.2 ,0.2 ,0.5 ,1.4 ,2.9 ,2.4 ,1.9, 1.9 } },
                new double[4][]{ new double[8]{0.3, 0.3 ,0.6 ,1.6 ,3.3 ,2.9 ,2.2, 2.2 }, new double[8]{0.2 ,0.2 ,0.5 ,1.4 ,3.0 ,2.4 ,1.9, 1.9 }, new double[8]{0.2 ,0.2 ,0.5 ,1.4 ,2.8 ,2.2 ,1.8, 1.8 }, new double[8]{0.2 ,0.2 ,0.4 ,1.3 ,2.7 ,2.0 ,1.7, 1.7 } },
                new double[4][]{ new double[8]{0.2, 0.2 ,0.5 ,1.4 ,2.8 ,2.2 ,1.8, 1.8 }, new double[8]{0.2 ,0.2 ,0.4 ,1.2 ,2.6 ,1.9 ,1.6, 1.6 }, new double[8]{0.2 ,0.2 ,0.4 ,1.2 ,2.4 ,1.7 ,1.5, 1.5 }, new double[8]{0.2 ,0.2 ,0.3 ,1.1 ,2.3 ,1.6 ,1.4, 1.4 } },
                new double[4][]{ new double[8]{0.2, 0.2 ,0.4 ,1.2 ,2.5 ,1.8 ,1.6, 1.6 }, new double[8]{0.2 ,0.2 ,0.3 ,1.1 ,2.3 ,1.6 ,1.4, 1.4 }, new double[8]{0.2 ,0.2 ,0.3 ,1.1 ,2.2 ,1.4 ,1.3, 1.3 }, new double[8]{0.2 ,0.1 ,0.3 ,1.0 ,2.1 ,1.3 ,1.2, 1.2} },
                new double[4][]{ new double[8]{0.2, 0.2 ,0.3 ,1.1 ,2.3 ,1.6 ,1.4, 1.4 }, new double[8]{0.1 ,0.1 ,0.3 ,1.0 ,2.1 ,1.3 ,1.2, 1.2 }, new double[8]{0.1 ,0.1 ,0.3 ,1.0 ,2.0 ,1.2 ,1.2, 1.2 }, new double[8]{0.1 ,0.1 ,0.2 ,0.9 ,1.9 ,1.1 ,1.1, 1.1 } },
                new double[4][]{ new double[8]{0.2, 0.2 ,0.3 ,1.0 ,2.1 ,1.4 ,1.3, 1.3 }, new double[8]{0.1 ,0.1 ,0.3 ,0.9 ,1.9 ,1.2 ,1.1, 1.1 }, new double[8]{0.1 ,0.1 ,0.2 ,0.9 ,1.8 ,1.1 ,1.1, 1.1 }, new double[8]{0.1 ,0.1 ,0.2 ,0.9 ,1.7 ,1.0 ,1.0, 1.0 } },
                new double[4][]{ new double[8]{0.1, 0.1 ,0.3 ,1.0 ,2.0 ,1.2 ,1.2, 1.2 }, new double[8]{0.1 ,0.1 ,0.2 ,0.9 ,1.8 ,1.0 ,1.0, 1.0 }, new double[8]{ 0.1 ,0.1 ,0.2 ,0.8 ,1.7 ,1.0 ,1.0, 1.0 }, new double[8]{ 0.1 ,0.1 ,0.2 ,0.8 ,1.6 ,0.9 ,0.9, 0.9 } }
            };

            public static double[] Lined_atten(double dim1, double dim2, double[][][] attenuation_function)
            {
                double smallerdim = Math.Min(dim1, dim2);
                double largerdim = Math.Min(dim1, dim2);
                double[] atten = new double[8];
                int idx1, idx1_, idx2a, idx2b, idx2a_, idx2b_;

                for (idx1 = 0; idx1 < bounds[0].Length; idx1++)
                {
                    if (smallerdim <= bounds[0][idx1]) break;
                }

                for (idx2a = 0; idx2a < bounds.Length; idx2a++)
                {
                    if (largerdim <= bounds[idx2a][idx1]) break;
                }
                if (idx2a == 0 || idx2a == 5)
                {
                    idx2a_ = idx2a;
                }
                else idx2a_ = idx2a - 1;


                //First find interpolation of two larger dim indices
                double wla1;
                double wlb1;
                double ws1;
                if (idx1 == 0 || idx1 == 5)
                {
                    idx1_ = idx1;
                    ws1 = 0;
                    idx2b = idx2a;
                    idx2b_ = idx2a_;
                }
                else
                {
                    ws1 = (bounds[0][idx1] - smallerdim) / (bounds[0][idx1] - bounds[0][idx1 - 1]);
                    for (idx2b = 0; idx2b < bounds.Length; idx2b++)
                    {
                        if (largerdim <= bounds[idx2b][idx1 - 1]) break;
                    }
                    if (idx2b == 0 || idx2b == 5)
                    {
                        idx2b_ = idx2b;
                    }
                    else idx2b_ = idx2b - 1;
                }

                if (idx2a == 0 || idx2a == 5)
                {
                    idx2a_ = idx2a;
                    wla1 = 0;
                    wlb1 = 0;
                }
                else
                {
                    wla1 = (bounds[idx2a][idx1] - largerdim) / (bounds[idx2a][idx1] - bounds[idx2a][idx1 - 1]);
                    wlb1 = (bounds[idx2b][idx1] - largerdim) / (bounds[idx2b][idx1] - bounds[idx2b][idx1 - 1]);
                }

                for (int oct = 0; oct < 8; oct++)
                {
                    atten[oct] = (attenuation_function[idx1 - 1][idx2a_][oct] * wla1 + attenuation_function[idx1 - 1][idx2a][oct] * (1 - wla1)) * ws1 + (attenuation_function[idx1][idx2a_][oct] * wla1 + attenuation_function[idx1][idx2a][oct] * (1 - wla1)) * (1 - ws1);
                }

                return atten;
            }

            static double[][][] attenuation2 = new double[11][][]
            {
                new double[4][]{ new double[8]{ 0.8, 0.8, 2.9, 4.9, 7.2, 7.4, 4.3, 4.3 }, new double[8]{ 0.7, 0.7, 2.4, 4.4, 6.4, 6.1, 3.7, 3.7 }, new double[8]{ 0.6, 0.6, 2.3, 4.2, 6.2, 5.8, 3.6, 3.6 }, new double[8]{ 0.6, 0.6, 2.1, 4.0, 5.8, 5.2, 3.3, 3.3 }}, //6
                new double[4][]{ new double[8]{ 0.6, 0.6 ,2.3 ,4.2 ,6.2 ,5.8 ,3.6, 3.6 }, new double[8]{ 0.6 ,0.6 ,1.9 ,3.9 ,5.6 ,4.9 ,3.2, 3.2 }, new double[8]{ 0.5, 0.5 ,1.8 ,3.7 ,5.4 ,4.5 ,3.0, 3.0 }, new double[8]{ 0.5 ,0.5 ,1.6 ,3.5 ,5.0 ,4.1 ,2.8, 2.8 }}, //8
                new double[4][]{ new double[8]{ 0.6 ,0.6 ,1.9 ,3.8 ,5.5 ,4.7 ,3.1, 3.1 }, new double[8]{ 0.5 ,0.5 ,1.6 ,3.4 ,5.0 ,4.0 ,2.7, 2.7 }, new double[8]{ 0.4, 0.4 ,1.5 ,3.3 ,4.8 ,3.7 ,2.6, 2.6 }, new double[8]{ 0.4 ,0.4 ,1.3 ,3.1 ,4.5 ,3.3 ,2.4, 2.4 }}, //10
                new double[4][]{ new double[8]{ 0.5, 0.5, 1.6, 3.5, 5.0, 4.1, 2.8, 2.8 }, new double[8]{ 0.4, 0.4, 1.4, 3.2, 4.6, 3.5, 2.5, 2.5 }, new double[8]{ 0.4, 0.4, 1.3, 3.0, 4.3, 3.2, 2.3, 2.3 }, new double[8]{ 0.4 ,0.4, 1.2, 2.9, 4.1, 2.9, 2.2, 2.2 }}, //12
                new double[4][]{ new double[8]{ 0.4, 0.4, 1.3, 3.1, 4.5, 3.3, 2.4, 2.4 }, new double[8]{ 0.4, 0.4, 1.2, 2.9, 4.1, 2.9, 2.2, 2.2 }, new double[8]{ 0.3, 0.3, 1.1, 2.7, 3.9, 2.6, 2.0, 2.0 }, new double[8]{ 0.3, 0.3, 1.0, 2.6, 3.6, 2.4, 1.9, 1.9 }}, //15
                new double[4][]{ new double[8]{ 0.4, 0.4, 1.2, 2.9, 4.1 ,2.9 ,2.2, 2.2 }, new double[8]{ 0.3, 0.3 ,1.0 ,2.6 ,3.7 ,2.4 ,1.9, 1.9 }, new double[8]{ 0.3, 0.3 ,0.9 ,2.5 ,3.5 ,2.2 ,1.8, 1.8 }, new double[8]{ 0.3, 0.3 ,0.8 ,2.3 ,3.3 ,2.0 ,1.7, 1.7 }}, //18
                new double[4][]{ new double[8]{ 0.3, 0.3, 0.9, 2.5, 3.5, 2.2, 1.8, 1.8 }, new double[8]{ 0.3 ,0.3 ,0.8 ,2.3 ,3.2 ,1.9 ,1.6, 1.6 }, new double[8]{ 0.2, 0.2 ,0.7 ,2.2 ,3.0 ,1.7 ,1.5, 1.5 }, new double[8]{ 0.2 ,0.2 ,0.7 ,2.0 ,2.9 ,1.6 ,1.4, 1.4 }}, //24
                new double[4][]{ new double[8]{ 0.2, 0.2 ,0.8 ,2.2 ,3.1 ,1.8 ,1.6, 1.6 }, new double[8]{ 0.2 ,0.2 ,0.7 ,2.0 ,2.9 ,1.6 ,1.4, 1.4 }, new double[8]{ 0.2, 0.2 ,0.6 ,1.9 ,2.7 ,1.4 ,1.3, 1.3 }, new double[8]{ 0.2 ,0.2 ,0.5 ,1.8 ,2.6 ,1.3 ,1.2, 1.2 }}, //30
                new double[4][]{ new double[8]{ 0.2, 0.2 ,0.7 ,2.0 ,2.9 ,1.6 ,1.4, 1.4 }, new double[8]{ 0.2 ,0.2 ,0.6 ,1.9 ,2.6 ,1.3 ,1.2, 1.2 }, new double[8]{ 0.2, 0.2 ,0.5 ,1.8 ,2.5 ,1.2 ,1.2, 1.2 }, new double[8]{ 0.2 ,0.2 ,0.5 ,1.7 ,2.3 ,1.1 ,1.1, 1.1 }}, //36
                new double[4][]{ new double[8]{ 0.2, 0.2 ,0.6 ,1.9 ,2.6 ,1.4 ,1.3, 1.3 }, new double[8]{ 0.2 ,0.2 ,0.5 ,1.7 ,2.4 ,1.2 ,1.1, 1.1 }, new double[8]{ 0.2, 0.2 ,0.5 ,1.6 ,2.3 ,1.1 ,1.1, 1.1 }, new double[8]{ 0.1 ,0.1 ,0.4 ,1.6 ,2.2 ,1.0 ,1.0, 1.0 }}, //42
                new double[4][]{ new double[8]{ 0.2, 0.2 ,0.5 ,1.8 ,2.5 ,1.2 ,1.2, 1.2 }, new double[8]{ 0.2, 0.2 ,0.4 ,1.6 ,2.3 ,1.0 ,1.0, 1.0 }, new double[8]{ 0.1, 0.1 ,0.4 ,1.5 ,2.1 ,1.0 ,1.0, 1.0 }, new double[8]{ 0.1 ,0.1 ,0.4 ,1.5 ,2.0 ,0.9 ,0.9, 0.9 }} //48
            };

            public static State Straight_Duct(State S, double horizontal_inches, double vertical_inches, double length, double Lining_thickness, out double[] atten)
            {
                double smallerdim = Math.Min(vertical_inches, horizontal_inches);
                double largerdim = Math.Max(horizontal_inches, vertical_inches);

                double[] a_ft1 = new double[8];
                double[] a_ft2 = new double[8];

                double interp1, interp2;

                if (Lining_thickness <= 0)
                {
                    interp1 = 1;
                    interp2 = 0;
                    a_ft1 = Unlined_Atten(smallerdim, largerdim);
                    ///Set a_ft2
                    a_ft2 = Lined_atten(smallerdim, largerdim, attenuation1);
                }
                else if (Lining_thickness < 1)
                {
                    interp1 = 1 - Lining_thickness;
                    interp2 = Lining_thickness;
                    ///Set a_ft1
                    double P_A = 2 * (smallerdim + largerdim) / (smallerdim * largerdim);
                    a_ft1 = Unlined_Atten(smallerdim, largerdim);
                    ///Set a_ft2
                    a_ft2 = Lined_atten(smallerdim, largerdim, attenuation1);
                }
                else if (Lining_thickness < 2)
                {
                    interp1 = 2 - Lining_thickness;
                    interp2 = Lining_thickness - 1;
                    ///Set a_ft1
                    a_ft1 = Lined_atten(smallerdim, largerdim, attenuation1);
                    ///Set a_ft2
                    a_ft2 = Lined_atten(smallerdim, largerdim, attenuation2);
                }
                else
                {
                    interp1 = 0;
                    interp2 = 1;
                    ///Set a_ft2
                    a_ft2 = Lined_atten(smallerdim, largerdim, attenuation2);
                    a_ft1 = a_ft2;
                }

                double[] totalAtten = new double[16];
                for (int oct = 0; oct < 8; oct++)
                {
                    totalAtten[oct] = (a_ft1[oct] * interp1 + a_ft2[oct] * interp2) * length;
                }

                atten = totalAtten;

                State S_new = S.Clone() as State - totalAtten;
                S_new.Resize(horizontal_inches,vertical_inches);
                S_new.lining_thickness = Lining_thickness;

                List<Hare.Geometry.Point> Pts = new List<Hare.Geometry.Point>();
                int ptct = (int)Math.Ceiling(length * 0.3048 / Duct_Model.Instance.delta_D);
                double delta = length * 0.3048 / ptct;
                for (int i = 1; i < ptct + 1; i++) Pts.Add(S.Location + S.Direction * delta * i);
                Duct_Model.Instance.AddComponent(S.ModelNode, atten, Pts.ToArray(), ref S_new);

                return S_new;
            }
        }
    }
}