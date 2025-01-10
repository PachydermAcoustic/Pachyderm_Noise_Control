using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pachyderm_Noise_Control
{
    namespace ASHRAE
    {
        public class Terminations
        {
            private static double[] Screen_C0;
            private static double[] Abrupt_Laminar_C0;
            private static double Abrupt_Turbulent_C0 = 1;
            private static double[] PyrDiff_inwall_C0;
            private static double[][][] PlainDiff_Free_C0;
            private static double[][][] PyrDiff_Free_C0;

            public static void Initialize()
            {
                ///Screen by open area:
                ///0.30 0.35 0.40 0.45 0.50 0.55 0.60 0.65 0.70 0.75 0.80 0.90 1.00
                Screen_C0 = new double[13] { 6.20, 4.10, 3.00, 2.20, 1.65, 1.26, 0.97, 0.75, 0.58, 0.44, 0.32, 0.14, 0.0 };

                //    SR2 - 1 Abrupt Exit
                //Laminar Flow
                //H / W 0.1 0.2 0.9 0.999 1.0 1.001 1.1 4.0 5.0 10.0
                Abrupt_Laminar_C0 = new double[10] { 1.55, 1.55, 1.55, 1.55, 2.00, 1.555, 1.55, 1.55, 1.55, 1.55 };
                //Turbulent Flow
                //    Co = 1.0


                //                    SR2 - 3 Plain Diffuser (Two Sides Parallel), Free Discharge
                //A1 / Ao : 1, 2, 4, 6
                //Re / 1000: 50, 100, 200, 400, 2000
                //θ : 4 8 10 14 20 30 45 60 90 120
                PlainDiff_Free_C0 = new double[4][][]
                    {
                        new double[5][]
                        {
                            new double[10] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                            new double[10] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                            new double[10] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                            new double[10] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                            new double[10] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }
                        },
                        new double[5][]
                        {
                            new double[10] { 0.51, 0.50, 0.51, 0.56, 0.63, 0.80, 0.96, 1.04, 1.09, 1.09 },
                            new double[10] { 0.48, 0.48, 0.50, 0.56, 0.63, 0.80, 0.96, 1.04, 1.09, 1.09 },
                            new double[10] { 0.42, 0.44, 0.47, 0.53, 0.63, 0.74, 0.93, 1.02, 1.08, 1.08 },
                            new double[10] { 0.38, 0.40, 0.42, 0.50, 0.62, 0.74, 0.93, 1.02, 1.08, 1.08 },
                            new double[10] { 0.38, 0.40, 0.42, 0.50, 0.62, 0.74, 0.93, 1.02, 1.08, 1.08 }
                        },
                        new double[5][]
                        {
                            new double[10] { 0.35, 0.34, 0.38, 0.48, 0.63, 0.76, 0.91, 1.03, 1.07, 1.07 },
                            new double[10] { 0.31, 0.31, 0.36, 0.45, 0.59, 0.72, 0.88, 1.02, 1.07, 1.07 },
                            new double[10] { 0.27, 0.26, 0.31, 0.41, 0.53, 0.67, 0.83, 0.96, 1.06, 1.06 },
                            new double[10] { 0.21, 0.22, 0.27, 0.39, 0.53, 0.67, 0.83, 0.96, 1.06, 1.06 },
                            new double[10] { 0.21, 0.22, 0.27, 0.39, 0.53, 0.67, 0.83, 0.96, 1.06, 1.06 }
                        },
                        new double[5][]
                        {
                            new double[10] { 0.36, 0.32, 0.34, 0.41, 0.56, 0.70, 0.84, 0.96, 1.08, 1.08 },
                            new double[10] { 0.32, 0.27, 0.30, 0.41, 0.56, 0.70, 0.84, 0.96, 1.08, 1.08 },
                            new double[10] { 0.26, 0.24, 0.27, 0.36, 0.52, 0.67, 0.81, 0.94, 1.06, 1.06 },
                            new double[10] { 0.21, 0.20, 0.24, 0.36, 0.52, 0.67, 0.81, 0.94, 1.06, 1.06 },
                            new double[10] { 9.21, 0.18, 0.24, 0.34, 0.50, 0.67, 0.81, 0.94, 1.05, 1.05 }
                        }
                    };
                //                    SR2 - 5 Pyramidal Diffuser, Free Discharge
                //A1 / Ao : 1, 2, 4, 6, 10
                //Re / 1000: 50, 100, 200, 400, 2000
                //θ : 4 8 10 14 20 30 45 60 90 120

                PyrDiff_Free_C0 = new double[5][][]
                {
                        new double[5][]
                        {
                            new double[10] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 },
                            new double[10] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 },
                            new double[10] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 },
                            new double[10] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 },
                            new double[10] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 }
                        },
                        new double[5][]
                        {
                            new double[10] { 0.55, 0.65, 0.68, 0.74, 0.82, 0.92, 1.05, 1.10, 1.08, 1.08 },
                            new double[10] { 0.51, 0.61, 0.66, 0.73, 0.81, 0.90, 1.04, 1.09, 1.08, 1.08 },
                            new double[10] { 0.47, 0.57, 0.61, 0.70, 0.79, 0.89, 1.04, 1.09, 1.08, 1.08 },
                            new double[10] { 0.42, 0.50, 0.56, 0.64, 0.76, 0.88, 1.02, 1.07, 1.08, 1.08 },
                            new double[10] { 0.42, 0.50, 0.56, 0.64, 0.76, 0.88, 1.02, 1.07, 1.08, 1.08 }
                        },
                        new double[5][]
                        {
                            new double[10] { 0.38, 0.53, 0.60, 0.69, 0.78, 0.90, 1.02, 1.07, 1.09, 1.09 },
                            new double[10] { 0.33, 0.49, 0.55, 0.66, 0.78, 0.90, 1.02, 1.07, 1.09, 1.09 },
                            new double[10] { 0.27, 0.42, 0.50, 0.62, 0.74, 0.87, 1.00, 1.06, 1.08, 1.08 },
                            new double[10] { 0.22, 0.36, 0.44, 0.56, 0.70, 0.84, 0.99, 1.06, 1.08, 1.08 },
                            new double[10] { 0.22, 0.36, 0.44, 0.56, 0.70, 0.84, 0.99, 1.06, 1.08, 1.08 }
                        },
                        new double[5][]
                        {
                            new double[10] { 0.34, 0.50, 0.57, 0.66, 0.77, 0.91, 1.02, 1.07, 1.08, 1.08 },
                            new double[10] { 0.30, 0.47, 0.54, 0.63, 0.76, 0.98, 1.02, 1.07, 1.08, 1.08 },
                            new double[10] { 0.24, 0.42, 0.48, 0.60, 0.73, 0.88, 1.00, 1.06, 1.08, 1.08 },
                            new double[10] { 0.18, 0.34, 0.44, 0.56, 0.73, 0.86, 0.98, 1.06, 1.08, 1.08 },
                            new double[10] { 0.18, 0.34, 0.44, 0.56, 0.73, 0.86, 0.98, 1.06, 1.08, 1.08 }
                        },
                        new double[5][]
                        {
                            new double[10] { 0.30, 0.45, 0.53, 0.64, 0.74, 0.85, 0.97, 1.10, 1.12, 1.12 },
                            new double[10] { 0.25, 0.40, 0.48, 0.62, 0.73, 0.85, 0.97, 1.10, 1.12, 1.12 },
                            new double[10] { 0.20, 0.34, 0.44, 0.56, 0.69, 0.82, 0.95, 1.10, 1.11, 1.11 },
                            new double[10] { 0.16, 0.28, 0.40, 0.55, 0.67, 0.80, 0.93, 1.09, 1.11, 1.11 },
                            new double[10] { 0.16, 0.28, 0.40, 0.55, 0.67, 0.80, 0.93, 1.09, 1.11, 1.11 }
                        }
                };
                    //SR2 - 6 Pyramidal Diffuser, with Wall
                    //L / Dh 0.5 1.0 2.0 3.0 4.0 5.0 6.0 8.0 10.0 12.0 14.0
                PyrDiff_inwall_C0 = new double[] { 0.49, 0.40, 0.30, 0.26, 0.23, 0.21, 0.19, 0.17, 0.16, 0.15, 0.14 };
//θ 26 19 13 11 9 8 7 6 6 5 5
//θ is the optimum angle.
             }


            public static State Open_Ended_NoWall(State S, double speed_of_sound, out double[] delta)
            {
                delta = new double[8];
                delta[0] = 10 * Math.Log10(1 + (Math.Pow(speed_of_sound / (Math.PI * 62.5 * S.diameter_last), 1.88)));
                delta[1] = 10 * Math.Log10(1 + (Math.Pow(speed_of_sound / (Math.PI * 125 * S.diameter_last), 1.88)));
                delta[2] = 10 * Math.Log10(1 + (Math.Pow(speed_of_sound / (Math.PI * 250 * S.diameter_last), 1.88)));
                delta[3] = 10 * Math.Log10(1 + (Math.Pow(speed_of_sound / (Math.PI * 500 * S.diameter_last), 1.88)));
                delta[4] = 10 * Math.Log10(1 + (Math.Pow(speed_of_sound / (Math.PI * 1000 * S.diameter_last), 1.88)));
                delta[5] = 10 * Math.Log10(1 + (Math.Pow(speed_of_sound / (Math.PI * 2000 * S.diameter_last), 1.88)));
                delta[6] = 10 * Math.Log10(1 + (Math.Pow(speed_of_sound / (Math.PI * 4000 * S.diameter_last), 1.88)));
                delta[7] = 10 * Math.Log10(1 + (Math.Pow(speed_of_sound / (Math.PI * 8000 * S.diameter_last), 1.88)));

                Duct_Model.Instance.Add_Attenuation(S.ModelNode, S.PathId, delta);

                return S.Clone() as State - delta;
            }

            public static State Open_Ended_AtWall(State S, double speed_of_sound, out double[] delta)
            {
                delta = new double[8];
                delta[0] = 10 * Math.Log10(1 + (Math.Pow(0.8 * speed_of_sound / (Math.PI * 62.5 * S.diameter_last), 1.88)));
                delta[1] = 10 * Math.Log10(1 + (Math.Pow(0.8 * speed_of_sound / (Math.PI * 125 * S.diameter_last), 1.88)));
                delta[2] = 10 * Math.Log10(1 + (Math.Pow(0.8 * speed_of_sound / (Math.PI * 250 * S.diameter_last), 1.88)));
                delta[3] = 10 * Math.Log10(1 + (Math.Pow(0.8 * speed_of_sound / (Math.PI * 500 * S.diameter_last), 1.88)));
                delta[4] = 10 * Math.Log10(1 + (Math.Pow(0.8 * speed_of_sound / (Math.PI * 1000 * S.diameter_last), 1.88)));
                delta[5] = 10 * Math.Log10(1 + (Math.Pow(0.8 * speed_of_sound / (Math.PI * 2000 * S.diameter_last), 1.88)));
                delta[6] = 10 * Math.Log10(1 + (Math.Pow(0.8 * speed_of_sound / (Math.PI * 4000 * S.diameter_last), 1.88)));
                delta[7] = 10 * Math.Log10(1 + (Math.Pow(0.8 * speed_of_sound / (Math.PI * 8000 * S.diameter_last), 1.88)));

                Duct_Model.Instance.Add_Attenuation(S.ModelNode, S.PathId, delta);

                return S.Clone() as State - delta;
            }
        }
    }
}