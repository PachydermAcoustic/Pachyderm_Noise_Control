using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pachyderm_Noise_Control
{
    namespace ASHRAE
    {
        public static class Elbow
        {
            static double C0_mitered_STvanes_40mm = 0.11;
            static double C0_mitered_STvanes_80mm = 0.33;
            static double C0_mitered_DTvanes_60mm = 0.25;
            static double C0_mitered_DTvanes_80mm = 0.41;

            public static void Initialize()
            {
                //CD3 - 9 Elbow, 5 Gore, 90 Degree, r / D = 1.5
                //D, mm 75 150 230 300 380 450 530 600 690 750 1500
                double[] Co_5gore90_rD15 = new double[11] { 0.51, 0.28, 0.21, 0.18, 0.16, 0.15, 0.14, 0.13, 0.12, 0.12, 0.12 };
                //CD3 - 10 Elbow, 7 Gore, 90 Degree, r / D = 2.5
                //D, mm 75 150 230 300 380 450 690 1500
                double[] Co_7gore90_rD25 = new double[8] { 0.16, 0.12, 0.10, 0.08, 0.07, 0.06, 0.05, 0.03 };
                //CD3 - 13 Elbow, 3 Gore, 60 Degree, r / D = 1.5
                //D, mm 75 150 230 300 380 450 530 600 690 750 1500
                double[] Co_3gore60_rD15 = new double[11] { 0.40, 0.21, 0.16, 0.14, 0.12, 0.12, 0.11, 0.10, 0.09, 0.09, 0.09 };
                //CD3 - 14 Elbow, 3 Gore, 45 Degree, r / D = 1.5
                //D, mm 75 150 230 300 380 450 530 600 690 750 1500
                double[] Co_3gore45_rD15 = new double[11] { 0.31, 0.17, 0.13, 0.11, 0.11, 0.09, 0.08, 0.08, 0.07, 0.07, 0.07 };
                //CD3 - 17 Elbow, Mitered, 45 Degree
                //D, mm 75 150 230 300 380 450 530 600 690 1500
                double[] Co_Mitered45 = new double[10] { 0.87, 0.79, 0.74, 0.72, 0.71, 0.70, 0.69, 0.68, 0.68, 0.67 };

                //Rectangular Elbows
                //CR3 - 1 Elbow, Smooth Radius, Without Vanes
                //r / W
                //Cp Values
                //H / W
                //0.5 0.75 1 1.5 2
                //0.25 0.50 0.75 1.0 1.50 2.0 3.0 4.0 5.0 6.0 8.0
                double[][] Co_RadiusedNoVanes = new double[5][]
                {
                    new double[11] { 1.53, 1.38, 1.29, 1.18, 1.06, 1.00, 1.00, 1.06, 1.12, 1.16, 1.18 },
                    new double[11] { 0.57, 0.52, 0.48, 0.44, 0.40, 0.39, 0.39, 0.40, 0.42, 0.43, 0.44 },
                    new double[11] { 0.27, 0.25, 0.23, 0.21, 0.19, 0.18, 0.18, 0.19, 0.20, 0.21, 0.21 },
                    new double[11] { 0.22, 0.20, 0.19, 0.17, 0.15, 0.14, 0.14, 0.15, 0.16, 0.17, 0.17 },
                    new double[11] { 0.20, 0.18, 0.16, 0.15, 0.14, 0.13, 0.13, 0.14, 0.14, 0.15, 0.15 }
                };
                //theta 0 20 30 45 60 75 90 110 130 150 180
                double[] k_RadiusedNoVanes = new double[11] { 0.00, 0.31, 0.45, 0.60, 0.78, 0.90, 1.00, 1.13, 1.20, 1.28, 1.40 };

                //CR3 - 3 Elbow, Smooth Radius, One Splitter Vane
                //r / W
                //Cp Values
                //H / W
                //0.25 0.50 1.0 1.50 2.0 3.0 4.0 5.0 6.0 7.0 8.0
                //0.55-1 (0.05 increments)
                double[][] Co_Radiused1Vane = new double[10][]
                {
                    new double[11] { 0.52, 0.40, 0.43, 0.49, 0.55, 0.66, 0.75, 0.84, 0.93, 1.01, 1.09 },
                    new double[11] { 0.36, 0.27, 0.25, 0.28, 0.30, 0.35, 0.39, 0.42, 0.46, 0.49, 0.52 },
                    new double[11] { 0.28, 0.21, 0.18, 0.19, 0.20, 0.22, 0.25, 0.26, 0.28, 0.30, 0.32 },
                    new double[11] { 0.22, 0.16, 0.14, 0.14, 0.15, 0.16, 0.17, 0.18, 0.19, 0.20, 0.21 },
                    new double[11] { 0.18, 0.13, 0.11, 0.11, 0.11, 0.12, 0.13, 0.14, 0.14, 0.15, 0.15 },
                    new double[11] { 0.15, 0.11, 0.09, 0.09, 0.09, 0.09, 0.10, 0.10, 0.11, 0.11, 0.12 },
                    new double[11] { 0.13, 0.09, 0.08, 0.07, 0.07, 0.08, 0.08, 0.08, 0.08, 0.09, 0.09 },
                    new double[11] { 0.11, 0.08, 0.07, 0.06, 0.06, 0.06, 0.06, 0.07, 0.07, 0.07, 0.07 },
                    new double[11] { 0.10, 0.07, 0.06, 0.05, 0.05, 0.05, 0.05, 0.05, 0.06, 0.06, 0.06 },
                    new double[11] { 0.09, 0.06, 0.05, 0.05, 0.04, 0.04, 0.04, 0.05, 0.05, 0.05, 0.05 }
                };
                //θ 0 30 45 60 90
                double[] k_Radiused1Vane = new double[5] { 0.00, 0.45, 0.60, 0.78, 1.00 };
                //r / W 0.55 0.60 0.65 0.70 0.75 0.80 0.85 0.90 0.95 1.0
                double[] CR_Radiused1Vane = new double[10] { 0.218, 0.302, 0.361, 0.408, 0.447, 0.480, 0.509, 0.535, 0.557, 0.577 };
                //Throat Radius/ Width Ratio(R / W)
                //r / W 0.55 0.60 0.65 0.70 0.75 0.80 0.85 0.90 0.95 1.0
                double[] RW_Radiused1Vane = new double[10] { 0.05, 0.10, 0.15, 0.20, 0.25, 0.30, 0.35, 0.40, 0.45, 0.50 };

                //CR3 - 6 Elbow, Mitered
                //  θ
                //Co Values
                //H / W
                //0.25 0.50 0.75 1.00 1.50 2.0 3.0 4.0 5.0 6.0 8.0
                //Theta 20 30 45 60 75 90
                double[][] Co_Mitered = new double[6][]
                {
                    new double[11] { 0.08, 0.08, 0.08, 0.07, 0.07, 0.07, 0.06, 0.06, 0.05, 0.05, 0.05 },
                    new double[11] { 0.18, 0.17, 0.17, 0.16, 0.15, 0.15, 0.13, 0.13, 0.12, 0.12, 0.11 },
                    new double[11] { 0.38, 0.37, 0.36, 0.34, 0.33, 0.31, 0.28, 0.27, 0.26, 0.25, 0.24 },
                    new double[11] { 0.60, 0.59, 0.57, 0.55, 0.52, 0.49, 0.46, 0.43, 0.41, 0.39, 0.38 },
                    new double[11] { 0.89, 0.87, 0.84, 0.81, 0.77, 0.73, 0.67, 0.63, 0.61, 0.58, 0.57 },
                    new double[11] { 1.30, 1.27, 1.23, 1.18, 1.13, 1.07, 0.98, 0.92, 0.89, 0.85, 0.83 }
                };
                //                    ER3 - 1 Elbow, 90 Degree, Variable Inlet/ Outlet Areas,
                //Exhaust / Return Systems
                //H / Wo
                //0.25, 1, 4, 100
                //Co Values
                //W1 / Wo
                //0.6 0.8 1.0 1.2 1.4 1.6 2.0
                double[][] Co_Return_Mitered_Variable = new double[4][]
                {
                    new double[7] { 1.76, 1.43, 1.24, 1.14, 1.09, 1.06, 1.06 },
                    new double[7] { 1.70, 1.36, 1.15, 1.02, 0.95, 0.90, 0.84 },
                    new double[7] { 1.46, 1.10, 0.90, 0.81, 0.76, 0.72, 0.66 },
                    new double[7] { 1.50, 1.04, 0.79, 0.69, 0.63, 0.60, 0.55 }
                };

                //SR3 - 1 Elbow, 90 Degree, Variable Inlet/ Outlet
                //Areas, Supply Air Systems
                //H / W1
                //Co Values
                //Wo / W1
                //0.6 0.8 1.0 1.2 1.4 1.6 2.0
                double[][] Co_Supply_Mitered_Variable = new double[4][]
                {
                    new double[7] { 0.63, 0.92, 1.24, 1.64, 2.14, 2.71, 4.24 },
                    new double[7] {0.61, 0.87, 1.15, 1.47, 1.86, 2.30, 3.36 },
                    new double[7] {0.53, 0.70, 0.90, 1.17, 1.49, 1.84, 2.64 },
                    new double[7] {0.54, 0.67, 0.79, 0.99, 1.23, 1.54, 2.20 }
                };
            }

            static double[][] Attenuation0 = new double[5][]
                {
                    new double[8]{0, 0, 0, 0, 1, 2, 3, 3}, //5 inches
                    new double[8]{0, 0, 0, 1, 2, 3, 3, 3}, //14 inches
                    new double[8]{0, 0, 1, 2, 3, 3, 3, 3}, //27 inches
                    new double[8]{0, 0, 1, 2, 3, 3, 3, 3}, //44 inches
                    new double[8]{0, 0, 1, 2, 3, 3, 3, 3} //90 inches
                };

            static double[][] Attenuation1_Rect = new double[5][]
                {
                    new double[8]{0, 0, 1, 2, 3, 4, 6, 8}, //5 inches
                    new double[8]{0, 1, 2, 3, 4, 6, 8, 10}, //14 inches
                    new double[8]{1, 2, 3, 4, 5, 6, 8, 10}, //27 inches
                    new double[8]{2, 3, 4, 5, 6, 8, 10, 12}, //44 inches
                    new double[8]{3, 4, 5, 6, 8, 10, 12, 12} //90 inches
                };

            static double[][] Attenuation1_Round = new double[5][]
                {
                    new double[8] { 0, 0, 0, 1, 2, 3, 4, 4 }, //5 inches
                    new double[8] { 0, 0, 1, 2, 3, 4, 4, 5 }, //14 inches
                    new double[8] { 0, 1, 2, 3, 4, 4, 5, 5 }, //27 inches
                    new double[8] { 1, 2, 3, 4, 4, 5, 5, 6 }, //44 inches
                    new double[8] { 2, 3, 4, 4, 5, 5, 6, 6 } //90 inches
                };


            public static State Unlined(State S, double vertical_inches, double horizontal_inches)
            {
                double smallerdim = Math.Min(horizontal_inches, vertical_inches);
                int idx;

                if (smallerdim < 5) idx = 0;
                else if (smallerdim < 14) idx = 1;
                else if (smallerdim < 27) idx = 2;
                else if (smallerdim < 44) idx = 3;
                else idx = 4;

                double velocity = S.Volume / (horizontal_inches * vertical_inches / 144);

                State S_New = (S.Clone() as State) - Attenuation0[idx];

                //calculate regenerated noise...
                double sectionalarea = horizontal_inches * vertical_inches;
                double diameter = Math.Sqrt(sectionalarea * 4 / Math.PI);
                double logub = 50 * Math.Log10(velocity / 60);
                double logsection = 10 * Math.Log10(sectionalarea);
                double logdiameter = 10 * Math.Log10(diameter);

                double[] Regen = new double[8];
                for (int oct = 0; oct < 8; oct++)
                {
                    double f = 31.25 * Math.Pow(2, oct);
                    double st = f * diameter / velocity / 60;
                    Regen[0] = -21.6 + 12.388 * Math.Pow(st, 0.673) - 16.482 * Math.Pow(1, -0.303) * Math.Log10(f * diameter / (velocity / 60)) - 5.047 * Math.Pow(1, -0.254) * Math.Pow(Math.Log10(f * diameter / velocity / 60), 2) + 10 * Math.Log10(f / 63) + logub + logsection + logdiameter + (6.793 - 1.86 * Math.Log10(st));
                }

                return S_New + Regen;
            }

            public static State Circular_Elbow(State S, double diameter_out, double lining_thickness, double Elbow_Radius,  out double[] atten, out double[] regen, double Vane_Chord_Length_in = 0, double no_of_vanes = 0)
            {
                double[] Attenuation = new double[8];
                double[] Regen = new double[8];
                double Area = (Math.PI * diameter_out * diameter_out / 4) / 144;

                if (lining_thickness == 0)
                {
                    for (int oct = 0; oct < 8; oct++)
                    {
                        //ASHRAE
                        double fxw = 0.0625 * Math.Pow(2, oct) * diameter_out;
                        //Insertion Loss of Unlined Elbows            
                        if (fxw < 1.9) Attenuation[oct] = 0;
                        else if (fxw < 3.8) Attenuation[oct] = 1;
                        else if (fxw < 7.5) Attenuation[oct] = 2;
                        else Attenuation[oct] = 3;
                    }
                }
                else
                {
                    if (Elbow_Radius > 0)
                    {
                        double interp2 = Math.Max(lining_thickness - 1, 1), interp1 = Math.Min(2 - lining_thickness, 0);

                        double[][] Attenuation1 = new double[14][] { //Insertion loss with a 1" liner
                        new double[8]{ 0, 1, 4, 8, 14, 18, 18, 14 },
                        new double[8] { 0, 2, 5, 9, 14, 16, 15, 11 },
                        new double[8] { 1, 2, 6, 10, 14, 15, 13, 10 },
                        new double[8] { 1, 3, 7, 11, 14, 14, 11, 9 },
                        new double[8] { 1, 4, 8, 11, 13, 12, 9, 10 },
                        new double[8] { 0, 0, 3, 7, 10, 12, 12, 11 },
                        new double[8] { 0, 1, 4, 8, 11, 12, 12, 10 },
                        new double[8] { 0, 1, 5, 8, 11, 12, 11, 10 },
                        new double[8] { 0, 2, 5, 9, 11, 12, 11, 10 },
                        new double[8] { 0, 3, 6, 9, 11, 11, 10, 10 },
                        new double[8] { 0, 3, 7, 10, 11, 11, 10, 10 },
                        new double[8] { 1, 4, 7, 10, 11, 11, 10, 10 },
                        new double[8] { 1, 4, 8, 10, 11, 11, 9, 10 },
                        new double[8] { 2, 5, 8, 10, 11, 10, 9, 11 } };

                        double[][] Attenuation2 = new double[14][] { //Insertion loss with a 2" liner
                        new double[8] { 1, 2, 6, 13, 22, 28, 28, 22 },
                        new double[8] { 1, 2, 7, 14, 20, 23, 21, 16 },
                        new double[8] { 1, 3, 8, 14, 19, 20, 17, 13 },
                        new double[8] { 1, 4, 8, 14, 18, 18, 14, 12 },
                        new double[8] { 2, 5, 9, 14, 16, 15, 11, 12 },
                        new double[8] { 0, 2, 6, 10, 14, 16, 16, 15 },
                        new double[8] { 0, 3, 6, 11, 14, 16, 15, 14 },
                        new double[8] { 0, 3, 7, 11, 14, 15, 14, 13 },
                        new double[8] { 1, 4, 8, 11, 14, 15, 14, 12 },
                        new double[8] { 1, 4, 8, 12, 14, 14, 13, 12 },
                        new double[8] { 2, 5, 9, 12, 14, 14, 12, 12 },
                        new double[8] { 2, 6, 9, 12, 14, 13, 12, 12 },
                        new double[8] { 3, 6, 10, 12, 13, 13, 11, 12 },
                        new double[8] { 3, 7, 10, 12, 13, 12, 11, 13 } };

                        int dia_choice = 0;
                        if (diameter_out < 6) dia_choice = 0;
                        else if (diameter_out < 8) dia_choice = 1;
                        else if (diameter_out < 10) dia_choice = 2;
                        else if (diameter_out < 12) dia_choice = 3;
                        else if (diameter_out < 16) dia_choice = 4;
                        else if (diameter_out < 20) dia_choice = 5;
                        else if (diameter_out < 24) dia_choice = 6;
                        else if (diameter_out < 28) dia_choice = 7;
                        else if (diameter_out < 32) dia_choice = 8;
                        else if (diameter_out < 36) dia_choice = 9;
                        else if (diameter_out < 42) dia_choice = 10;
                        else if (diameter_out < 48) dia_choice = 12;
                        else if (diameter_out < 54) dia_choice = 11;
                        else dia_choice = 13;

                        for (int oct = 0; oct < 8; oct++) Attenuation[oct] = Attenuation1[dia_choice][oct] * interp1 + Attenuation2[dia_choice][oct] * interp2;
                    }
                    else
                    {
                        for (int oct = 0; oct < 8; oct++)
                        {
                            //ASHRAE
                            double fxw = 0.0625 * Math.Pow(2, oct) * diameter_out;
                            if (fxw < 1.9) Attenuation[oct] = 0;
                            else if (fxw < 3.8) Attenuation[oct] = 1;
                            else if (fxw < 7.5) Attenuation[oct] = 6;
                            else if (fxw < 15) Attenuation[oct] = 11;
                            else if (fxw < 30) Attenuation[oct] = 10;
                            else Attenuation[oct] = 10;
                        }
                    }
                }

                Elbow_Radius += diameter_out / 2 + lining_thickness;

                if (Vane_Chord_Length_in == 0 || no_of_vanes == 0)
                {
                    double Velocity_S = (S.Volume / ((diameter_out * diameter_out) / 60));
                    for (int oct = 0; oct < 8; oct++)
                    {
                        double f = 62.5 * Math.Pow(2, oct);
                        double Strouhal = f * diameter_out / Velocity_S;
                        double Radius_Correction = (1 - (1 - (Elbow_Radius / (12 * diameter_out)) / 0.15) * (6.793 - 1.86 * Math.Log10(Strouhal)));
                        double m = S.Velocity / Velocity_S;
                        double Turbulence_Correction = -1.667 + 1.8 * m - 0.133 * m * m;
                        double Char_Spectrum_Kj = -21.6 * 12.388 * Math.Pow(m, 0.673) - 16.482 * Math.Pow(m, -0.303) * Math.Log10(Strouhal) - 5.047 * Math.Pow(m, -0.254) * Math.Pow(Math.Log10(Strouhal), 2);
                        Regen[oct] = Char_Spectrum_Kj + 10 * Math.Log10(f / 63) + 50 * Math.Log10(Velocity_S) + 10 * Math.Log10(Area) + 10 * Math.Log10(diameter_out) + Radius_Correction + Turbulence_Correction;
                    }
                }
                else
                {
                    double PressureLoss = 0.15;// in.wg.
                    double PaLossCoef = 15.9E6 * PressureLoss / (S.Volume * S.Volume / Math.Pow(Math.PI * diameter_out * diameter_out / 4, 2));
                    double BlockageFactor = (Math.Sqrt(PaLossCoef) - 1) / (PaLossCoef = 1);
                    double Velocity_Adjusted = S.Volume / ((Math.PI * diameter_out * diameter_out /4) * BlockageFactor) / 60;

                    for (int oct = 0; oct < 8; oct++)
                    {
                        double f = 62.5 * Math.Pow(2, oct);
                        double Strouhal = f * diameter_out / 12 / Velocity_Adjusted;
                        double Kt = -47.5 - 7.69 * Math.Pow(Math.Log10(Strouhal), 2.5);
                        Regen[oct] = Kt + 10 * Math.Log10(f / 63) + 50 * Math.Log10(Velocity_Adjusted) + 10 * Math.Log10(Area / 144) + 10 * Math.Log10(Vane_Chord_Length_in) + 10 * Math.Log10(no_of_vanes);
                    }
                }

                State R = S.Clone() as State;
                R -= Attenuation;
                R += Regen;

                R.diameter_last = diameter_out ;
                R.area_last = Area;
                R.Velocity = S.Volume / R.area_last;

                atten = Attenuation;
                regen = Regen;

                return R;
            }

            public static State Rectangular_Elbow(State S, Hare.Geometry.Vector dir, double vertical_inches_out, double horizontal_inches_out, double lining_thickness, double Elbow_Radius, out double[] atten, out double[] regen, double Vane_Chord_Length_in = 0, double no_of_vanes = 0)
            {
                double diameter = Math.Sqrt(4 * (vertical_inches_out * horizontal_inches_out / 144) / Math.PI);
                double[] Attenuation = new double[8];
                double[] Regen = new double[8];
                double Area = horizontal_inches_out * vertical_inches_out / 144;
                if (lining_thickness > 2) lining_thickness = 2;

                double wdot = Math.Abs(Hare.Geometry.Hare_math.Dot(dir, S.Frame[1]));
                double hdot = Math.Abs(Hare.Geometry.Hare_math.Dot(dir, S.Frame[2]));

                double insidedim = (wdot < hdot) ? horizontal_inches_out : vertical_inches_out;
                double dot = Math.Max(hdot, wdot);

                if (lining_thickness == 0)
                {
                    if (Elbow_Radius > 0)
                    {
                        if (no_of_vanes > 0)
                        {
                            //turning vanes should not be present in radiused elbows
                            throw new Exception("Radiused elbows should not have turning vanes...");
                        }

                        for (int oct = 0; oct < 8; oct++)
                        {
                            //ASHRAE
                            double fxw = 0.0625 * Math.Pow(2, oct) * insidedim;
                            //Insertion Loss of Unlined Elbows            
                            if (fxw < 1.9) Attenuation[oct] = 0;
                            else if (fxw < 3.8) Attenuation[oct] = 1;
                            else if (fxw < 7.5) Attenuation[oct] = 2;
                            else Attenuation[oct] = 3;
                        }
                    }
                    else
                    {
                        if (no_of_vanes == 0)
                        {
                            for (int oct = 0; oct < 8; oct++)
                            {
                                //ASHRAE 2007
                                double fxw = 0.0625 * Math.Pow(2, oct) * insidedim;
                                //Insertion Loss of Unlined Elbows   withouot turning vanes          
                                if (fxw < 1.9) Attenuation[oct] = 0;
                                else if (fxw < 3.8) Attenuation[oct] = 1;
                                else if (fxw < 7.5) Attenuation[oct] = 5;
                                else if (fxw < 15) Attenuation[oct] = 8;
                                else if (fxw < 30) Attenuation[oct] = 4;
                                else Attenuation[oct] = 3;
                            }
                        }
                        else
                        {
                            for (int oct = 0; oct < 8; oct++)
                            {
                                //ASHRAE 2007 - check ch 47 table 18 & 19
                                double fxw = 0.0625 * Math.Pow(2, oct) * insidedim;
                                //Insertion Loss of Unlined Elbows with turning vanes
                                if (fxw < 1.9) Attenuation[oct] = 0;
                                else if (fxw < 3.8) Attenuation[oct] = 1;
                                else if (fxw < 7.5) Attenuation[oct] = 4;
                                else if (fxw < 15) Attenuation[oct] = 6;
                                else Attenuation[oct] = 4;
                            }
                        }
                    }
                }
                else
                {
                    if (Elbow_Radius > 0)
                    {
                        if (no_of_vanes > 0)
                        {
                            //turning vanes should not be present in radiused elbows
                            throw new Exception("Radiused elbows should not have turning vanes...");
                        }
                        //lined, rectangular, mitered
                        //Hoover & Keith
                        if (insidedim < 5) Attenuation = new double[8] { 0, 0, 0, 1, 2, 3, 4, 4 };
                        else if (insidedim < 14) Attenuation = new double[8] { 0, 0, 1, 2, 3, 4, 4, 5 };
                        else if (insidedim < 27) Attenuation = new double[8] { 0, 1, 2, 3, 4, 4, 5, 5 };
                        else if (insidedim < 44) Attenuation = new double[8] { 1, 2, 3, 4, 4, 5, 5, 6 };
                        else Attenuation = new double[8] { 2, 3, 4, 4, 5, 5, 6, 6 };
                    }
                    else
                    {
                        if (no_of_vanes == 0)
                        {
                            for (int oct = 0; oct < 8; oct++)
                            {
                                //ASHRAE 2007
                                double fxw = 0.0625 * Math.Pow(2, oct) * insidedim;
                                //Insertion Loss of Lined Mitered Elbows Without Turning Vanes           
                                if (fxw < 1.9) Attenuation[oct] = 0;
                                else if (fxw < 3.8) Attenuation[oct] = 1;
                                else if (fxw < 7.5) Attenuation[oct] = 6;
                                else if (fxw < 15) Attenuation[oct] = 11;
                                else if (fxw < 30) Attenuation[oct] = 10;
                                else Attenuation[oct] = 10;
                            }
                        }
                        else
                        {
                            for (int oct = 0; oct < 8; oct++)
                            {
                                //ASHRAE 2007 - check ch 47 table 18 & 19
                                double fxw = 0.0625 * Math.Pow(2, oct) * insidedim;
                                //Insertion Loss of Lined Mitered Elbows With Turning Vanes           
                                if (fxw < 1.9) Attenuation[oct] = 0;
                                else if (fxw < 3.8) Attenuation[oct] = 1;
                                else if (fxw < 7.5) Attenuation[oct] = 4;
                                else if (fxw < 15) Attenuation[oct] = 7;
                                else Attenuation[oct] = 7;
                            }
                        }
                    }
                }

                //TODO: Correct attenuation for angle
//                for (int oct = 0; oct < 8; oct++) Attenuation[oct] = 10 * Math.Log10(Math.Pow(10, Attenuation[oct] / 10) * dot);

                if (Vane_Chord_Length_in == 0 || no_of_vanes == 0)
                {
                    double Velocity_S = (S.Volume / (horizontal_inches_out * vertical_inches_out/144)) / 60;
                    for (int oct = 0; oct < 8; oct++)
                    {
                        double f = 62.5 * Math.Pow(2, oct);
                        //Attenuation[oct] = (0.485 + 2.094 * Math.Log10(f * diameter) + 3.172 * Math.Pow(Math.Log10(f * diameter), 2) - 1.578 * Math.Pow(Math.Log10(f * diameter), 4) + 0.085 * Math.Pow(Math.Log10(f * diameter), 7)) / (horizontal_inches_out * horizontal_inches_out / (Elbow_Radius * Elbow_Radius));
                        //if (Attenuation[oct] < 0) Attenuation[oct] = 0;
                        double Strouhal = f * diameter / Velocity_S;
                        double Radius_Correction = (1 - ((Elbow_Radius / (12 * diameter)) / 0.15)) * (6.793 - 1.86 * Math.Log10(Strouhal));
                        double m = (S.Velocity/60) / Velocity_S;
                        double Turbulence_Correction = -1.667 + 1.8 * m - 0.133 * m * m;
                        double Char_Spectrum_Kj = -21.6 + 12.388 * Math.Pow(m, 0.673) - 16.482 * Math.Pow(m, -0.303) * Math.Log10(Strouhal) - 5.047 * Math.Pow(m, -0.254) * Math.Pow(Math.Log10(Strouhal), 2);
                        Regen[oct] = Char_Spectrum_Kj + 10 * Math.Log10(f / 63) + 50 * Math.Log10(Velocity_S) + 10 * Math.Log10(Area) + 10 * Math.Log10(diameter);
                        Regen[oct] += Radius_Correction + Turbulence_Correction;
                    }
                    /// Regen w 54 x 18 12 in radius - 42 37 32 25 18 9 0
                }
                else
                {
                    double PressureLoss = 0.15;// in.wg.
                    double PaLossCoef = 15.9E6 * PressureLoss / (S.Volume * S.Volume / (vertical_inches_out * vertical_inches_out * horizontal_inches_out * horizontal_inches_out));
                    double BlockageFactor = (Math.Sqrt(PaLossCoef) - 1) / PaLossCoef;
                    double Velocity_Adjusted = S.Volume / (horizontal_inches_out * vertical_inches_out * BlockageFactor) / 60;

                    for (int oct = 0; oct < 8; oct++)
                    {
                        double f = 62.5 * Math.Pow(2, oct);
                        double Strouhal = f * vertical_inches_out / 12 / Velocity_Adjusted;
                        double Kt = -47.5 - 7.69 * Math.Pow(Math.Log10(Strouhal), 2.5);
                        Regen[oct] = Kt + 10 * Math.Log10(f / 63) + 50 * Math.Log10(Velocity_Adjusted) + 10 * Math.Log10(Area) + 10 * Math.Log10(Vane_Chord_Length_in) + 10 * Math.Log10(no_of_vanes);
                    }
                }

                State R = S.Clone() as State;
                R -= Attenuation;
                R += Regen;

                R.diameter_last = diameter;
                R.Velocity = S.Volume / Area;
                R.area_last = vertical_inches_out * horizontal_inches_out;

                atten = Attenuation;
                regen = Regen;

                dir.Normalize();
                R.Direction = dir;

                if (Math.Abs(dir.dx) + Math.Abs(dir.dy) > 0.01)
                {
                    if (Hare.Geometry.Hare_math.Dot(S.Direction, R.Direction) > 0.95)
                    {
                        List<Hare.Geometry.Point> Pts = new List<Hare.Geometry.Point>();
                        int ct = (int)Math.Ceiling(((Elbow_Radius + S.dimensions[0]) / 12) / Duct_Model.Instance.delta_D);
                        for (int i = 0; i < ct; i++)
                        {
                            Pts.Add(S.Location + R.Direction * i);
                        }
                        Duct_Model.Instance.AddComponent(S.ModelNode, atten, Regen, Duct_Model.NoiseType.Aerodynamic, Pts.ToArray(), horizontal_inches_out, vertical_inches_out, ref R);
                    }
                    else if (Elbow_Radius > 0)
                    {
                        List<Hare.Geometry.Point> Pts = new List<Hare.Geometry.Point>();
                        double radiusCtr = Elbow_Radius + horizontal_inches_out / 2;
                        double dtheta = Duct_Model.Instance.delta_D / (radiusCtr * 0.0254);
                        Hare.Geometry.Vector Perp = Hare.Geometry.Hare_math.Cross(S.Direction, new Hare.Geometry.Vector(0, 0, 1));
                        if (Hare.Geometry.Hare_math.Dot(dir, Perp) > 0)
                        {
                            Perp *= -1;
                        }
                        Hare.Geometry.Point center = S.Location - Perp * (radiusCtr) * 0.0254;
                        double stheta = Math.Atan2(S.Direction.dy, S.Direction.dx);
                        double etheta = Math.Atan2(R.Direction.dy, R.Direction.dx);
                        double diff = (etheta - stheta);
                        //if (diff > Math.PI) diff -= Math.PI;
                        //else if (diff < -Math.PI) diff += Math.PI;
                        int diffdir = diff > 0 ? -1 : 1;
                        int cttheta = (int)(diffdir * Math.Ceiling(1 + Math.Abs(diff) / dtheta));
                        double dz = dir.dz * Duct_Model.Instance.delta_D;
                        for (int i = diffdir; i != cttheta - diffdir; i += diffdir)
                        {
                            Hare.Geometry.Vector v = new Hare.Geometry.Vector(Math.Sin(i * dtheta + etheta - diffdir * Math.PI/2), Math.Cos(i * dtheta + etheta - diffdir * Math.PI/2), dz * i);//(Math.Cos(i * dtheta + etheta - diffdir * Math.PI/2), Math.Sin(i * dtheta + etheta - diffdir * Math.PI/2), dz * i);
                            v.Normalize();
                            Pts.Add(center + (radiusCtr * 0.0254) * v);
                        }
                        //List<Hare.Geometry.Point> Pts = new List<Hare.Geometry.Point>();
                        ////2 * Elbow_Radius * Math.Sin(Math.Acos(Hare.Geometry.Hare_math.Dot(S.Direction, R.Direction))/2);
                        //double radiusCtr = 0.0254 * (Elbow_Radius + horizontal_inches_out / 2);
                        //double dtheta = Duct_Model.Instance.delta_D / (radiusCtr);
                        //Hare.Geometry.Vector Perp = Hare.Geometry.Hare_math.Cross(S.Direction, new Hare.Geometry.Point(0, 0, 1));
                        //Perp *= Hare.Geometry.Hare_math.Dot(dir, Perp) > 0 ? -1 : 1;
                        //Hare.Geometry.Point center = S.Location - Perp * (radiusCtr);
                        //double stheta = Math.Atan2(-Perp.y, -Perp.x);
                        //double etheta = Math.Acos(S.Direction.x * R.Direction.x + S.Direction.y * R.Direction.y);
                        //int cttheta = (int)Math.Ceiling(etheta / dtheta);
                        //dtheta = etheta / cttheta;
                        //double dz = dir.z * Duct_Model.Instance.delta_D;
                        //for(int i = 1; i < cttheta + 1; i++) Pts.Add(center + radiusCtr * new Hare.Geometry.Point(Math.Cos(i*dtheta +stheta), Math.Sin(i * dtheta + stheta), dz * i));
                        Duct_Model.Instance.AddComponent(S.ModelNode, atten, Regen, Duct_Model.NoiseType.Aerodynamic, Pts.ToArray(), horizontal_inches_out, vertical_inches_out, ref R);
                    }
                    else
                    {
                        List<Hare.Geometry.Point> Pts = new List<Hare.Geometry.Point>();
                        double D1 = (horizontal_inches_out / 12) * .3048 / 2;
                        double D2 = (horizontal_inches_out / 12) * .3048 / 2;
                        int D1ct = (int)Math.Ceiling(D1 / Duct_Model.Instance.delta_D);
                        int D2ct = (int)Math.Ceiling(D2 / Duct_Model.Instance.delta_D);
                        double dD1 = D1 / D1ct;
                        double dD2 = D2 / D2ct;
                        for (int i = 1; i < D1ct + 1; i++) Pts.Add(S.Location + dD1 * S.Direction * i);
                        Hare.Geometry.Vector Perp = Hare.Geometry.Hare_math.Cross(S.Direction, new Hare.Geometry.Vector(0, 0, 1));
                        Perp *= Hare.Geometry.Hare_math.Dot(dir, Perp) > 0 ? 1 : -1;
                        Hare.Geometry.Point pt = Pts[Pts.Count - 1];
                        for (int i = 1; i < D2ct + 1; i++) Pts.Add(pt + dD2 * dir * i);
                        Duct_Model.Instance.AddComponent(S.ModelNode, atten, Regen, Duct_Model.NoiseType.Aerodynamic, Pts.ToArray(), horizontal_inches_out, vertical_inches_out, ref R);
                    }
                }
                else
                {
                    if (Hare.Geometry.Hare_math.Dot(S.Direction, R.Direction) > 0.95)
                    {
                        List<Hare.Geometry.Point> Pts = new List<Hare.Geometry.Point>();
                        int ct = (int)Math.Ceiling(((Elbow_Radius + S.dimensions[1]) / 12) / Duct_Model.Instance.delta_D);
                        for (int i = 0; i < ct; i++)
                        {
                            Pts.Add(S.Location + R.Direction * i);
                        }
                        Duct_Model.Instance.AddComponent(S.ModelNode, atten, Regen, Duct_Model.NoiseType.Aerodynamic, Pts.ToArray(), horizontal_inches_out, vertical_inches_out, ref R);
                    }
                    else if (Elbow_Radius > 0)
                    {
                        List<Hare.Geometry.Point> Pts = new List<Hare.Geometry.Point>();
                        double radiusCtr = Elbow_Radius + horizontal_inches_out / 2;
                        double dtheta = Duct_Model.Instance.delta_D / (radiusCtr * 0.0254);
                        Hare.Geometry.Vector Perp = Hare.Geometry.Hare_math.Cross(S.Direction, new Hare.Geometry.Vector(0, 0, 1));
                        if (Hare.Geometry.Hare_math.Dot(dir, Perp) > 0)
                        {
                            Perp *= -1;
                        }
                        Hare.Geometry.Point center = S.Location - Perp * (radiusCtr) * 0.0254;
                        double stheta = Math.Atan2(S.Direction.dy, S.Direction.dx);
                        double etheta = Math.Atan2(R.Direction.dy, R.Direction.dx);
                        double diff = (etheta - stheta);
                        if (diff > Math.PI) diff -= Math.PI;
                        else if (diff < -Math.PI) diff += Math.PI;
                        int diffdir = diff > 0 ? 1 : -1;
                        int cttheta = (int)(diffdir * Math.Ceiling(1 + Math.Abs(diff) / dtheta));
                        double dz = dir.dz * Duct_Model.Instance.delta_D;
                        for (int i = diffdir; i != cttheta; i += diffdir)
                        {
                            Hare.Geometry.Vector v = new Hare.Geometry.Vector(Math.Cos(i * dtheta + stheta), Math.Sin(i * dtheta + stheta), dz * i);
                            v.Normalize();
                            Pts.Add(center + (radiusCtr * 0.0254) * v);
                        }
                        //Duct_Model.Instance.AddComponent(S.ModelNode, ATT[j], Regen_b[j], Duct_Model.NoiseType.Aerodynamic, Pts.ToArray(), ref Outlets[j]);
                        //List<Hare.Geometry.Point> Pts = new List<Hare.Geometry.Point>();
                        ////2 * Elbow_Radius * Math.Sin(Math.Acos(Hare.Geometry.Hare_math.Dot(S.Direction, R.Direction))/2);
                        //double dtheta = 2 * Math.Asin(Duct_Model.Instance.delta_D / (2 * Elbow_Radius));
                        //Hare.Geometry.Vector Lateral = S.Direction + R.Direction / 2;
                        //Hare.Geometry.Vector Axis = Hare.Geometry.Hare_math.Cross(S.Direction, dir);
                        ////Hare.Geometry.Vector Perp = Hare.Geometry.Hare_math.Cross(S.Direction, Axis);
                        //Axis *= Hare.Geometry.Hare_math.Dot(dir, Axis) > 0 ? 1 : -1;
                        //double radiusCtr = Elbow_Radius + vertical_inches_out / 2;
                        //Hare.Geometry.Point center = S.Location -  Axis * (radiusCtr);
                        
                        //Hare.Geometry.Vector diffx;
                        //Hare.Geometry.Vector diffy;
                        ////Check that the ray and the normal are both on the same side...
                        //diffx = new Hare.Geometry.Vector(0, 0, 1);
                        //diffy = Hare.Geometry.Hare_math.Cross(Axis, diffx);
                        //diffx = Hare.Geometry.Hare_math.Cross(diffy, Axis);
                        //diffx.Normalize();
                        //diffy.Normalize();
                        //Axis.Normalize();

                        //double stheta = Math.Atan2(-Axis.y, -Axis.x);
                        //double flatlengthS = Math.Sqrt(S.Direction.x * S.Direction.x + S.Direction.y * S.Direction.y);
                        //double flatlengthR = Math.Sqrt(R.Direction.x * R.Direction.x + R.Direction.y * R.Direction.y);
                        //double etheta = Math.Acos(flatlengthR * flatlengthS + S.Direction.z * R.Direction.z);
                        //int cttheta = (int)Math.Ceiling(etheta / dtheta);
                        //dtheta = etheta / cttheta;
                        //double dflat = dir.z * Duct_Model.Instance.delta_D;
                        //for (int i = 1; i < cttheta + 1; i++)
                        //{
                        //    double x, y;
                        //    Hare.Geometry.Vector vect;
                        //    double theta = i * dtheta + stheta;
                        //    x = Math.Cos(theta) * Math.Sin(theta);
                        //    y = Math.Sin(theta) * Math.Sin(theta);
                        //    vect = (diffx * x) + (diffy * y);
                        //    vect.Normalize();
                        //    Pts.Add(center + radiusCtr * vect);
                        //}
                        Duct_Model.Instance.AddComponent(S.ModelNode, atten, Regen, Duct_Model.NoiseType.Aerodynamic, Pts.ToArray(), horizontal_inches_out, vertical_inches_out, ref R);
                    }
                    else
                    {
                        List<Hare.Geometry.Point> Pts = new List<Hare.Geometry.Point>();
                        double D1 = (vertical_inches_out/12)*.3048 / 2;
                        double D2 = (vertical_inches_out/12)*.3048 / 2;
                        int D1ct = (int)Math.Ceiling(D1 / Duct_Model.Instance.delta_D);
                        int D2ct = (int)Math.Ceiling(D2 / Duct_Model.Instance.delta_D);
                        double dD1 = D1 / D1ct;
                        double dD2 = D2 / D2ct;
                        for (int i = 1; i < D1ct + 1; i++) Pts.Add(S.Location + dD1 * S.Direction * i);
                        Hare.Geometry.Vector Perp = Hare.Geometry.Hare_math.Cross(S.Direction, new Hare.Geometry.Vector(0, 0, 1));
                        Perp *= Hare.Geometry.Hare_math.Dot(dir, Perp) > 0 ? 1 : -1;
                        Hare.Geometry.Point pt = Pts[Pts.Count - 1];
                        for (int i = 1; i < D2ct + 1; i++) Pts.Add(pt + dD2 * dir * i);
                        Duct_Model.Instance.AddComponent(S.ModelNode, atten, Regen, Duct_Model.NoiseType.Aerodynamic, Pts.ToArray(), horizontal_inches_out, vertical_inches_out, ref R);
                    }
                }
                return R;
            }
        }
    }
}