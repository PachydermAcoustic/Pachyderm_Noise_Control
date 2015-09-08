using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pachyderm_Noise_Control
{
    namespace ASHRAE
    {
        public class Equipment
        {
            public enum FanType
            {
                Centrifugal_backward,
                Cenrifugal_forward,
                Centrifugal_Radial,
                Vaneaxial,
                Tubeaxial,
                Propeller
            }

            static double[] Centrifugal_Backward_GT30in = new double[9] { 37, 37, 36, 31, 27, 20, 16, 14, 3 }; //Last is BFI
            static double[] Centrifugal_Backward_LT30in = new double[9] { 42, 42, 40, 36, 31, 25, 21, 16, 3 }; //Last is BFI
            static double[] Centrifugal_Forward_allsizes = new double[9] { 50, 50, 40, 33, 33, 28, 23, 18, 2 }; //Last is BFI
            static double[] Centrifugal_Radial_LP_GT40in = new double[9] { 53, 44, 40, 36, 34, 29, 26, 23, 7 }; //Last is BFI
            static double[] Centrifugal_Radial_LP_LT40in = new double[9] { 64, 56, 50, 40, 39, 36, 31, 28, 7 }; //Last is BFI
            static double[] Centrifugal_Radial_MP_GT40in = new double[9] { 55, 51, 42, 39, 35, 30, 26, 23, 8 }; //Last is BFI
            static double[] Centrifugal_Radial_MP_LT40in = new double[9] { 65, 60, 48, 45, 43, 38, 34, 31, 8 }; //Last is BFI
            static double[] Centrifugal_Radial_HP_GT40in = new double[9] { 58, 55, 50, 45, 43, 41, 38, 35, 8 }; //Last is BFI
            static double[] Centrifugal_Radial_HP_LT40in = new double[9] { 68, 64, 56, 51, 51, 49, 46, 43, 8 }; //Last is BFI
            static double[] Vaneaxial_HubRatio_3to4 = new double[9] { 46, 40, 40, 45, 44, 42, 35, 31, 6 }; //Last is BFI
            static double[] Vaneaxial_HubRatio_4to6 = new double[9] { 46, 40, 43, 40, 38, 33, 27, 25, 6 }; //Last is BFI
            static double[] Vaneaxial_HubRatio_6to8 = new double[9] { 56, 49, 48, 48, 46, 44, 40, 37, 6 }; //Last is BFI
            static double[] TubeAxial_GT40in = new double[9] { 48, 43, 44, 46, 44, 43, 36, 34, 7 }; //Last is BFI
            static double[] TubeAxial_LT40in = new double[9] { 45, 44, 46, 50, 49, 48, 40, 37, 7 }; //Last is BFI
            static double[] Propeller = new double[9] { 45, 48, 55, 53, 52, 49, 43, 39, 5 }; //Last is BFI

            public static double[] Fan_Discharge(FanType type, double dimension_inches, double volume, double downstream_pressure_inwg)
            {
                double[] basis = new double[8];
                switch (type)
                {
                    case FanType.Centrifugal_backward:
                        if (dimension_inches < 30) basis = Centrifugal_Backward_GT30in;
                        else basis = Centrifugal_Backward_LT30in;
                        break;
                    case FanType.Cenrifugal_forward:
                        basis = Centrifugal_Forward_allsizes;
                        break;
                    case FanType.Centrifugal_Radial:
                        if (downstream_pressure_inwg < 10)
                        {
                            if (dimension_inches < 40) basis = Centrifugal_Radial_LP_LT40in;
                            else basis = Centrifugal_Radial_LP_GT40in;
                        }
                        else if (downstream_pressure_inwg < 20)
                        {
                            if (dimension_inches < 40) basis = Centrifugal_Radial_MP_LT40in;
                            else basis = Centrifugal_Radial_MP_GT40in;
                        }
                        else
                        {
                            if (dimension_inches < 40) basis = Centrifugal_Radial_HP_LT40in;
                            else basis = Centrifugal_Radial_HP_GT40in;
                        }
                        break;
                    case FanType.Propeller:
                        basis = Propeller;
                        break;
                    case FanType.Tubeaxial:
                        if (dimension_inches > 40) basis = TubeAxial_GT40in;
                        else basis = TubeAxial_LT40in;
                        break;
                    case FanType.Vaneaxial:
                        if (dimension_inches < .4) basis = Vaneaxial_HubRatio_3to4;
                        else if (dimension_inches < .6) basis = Vaneaxial_HubRatio_4to6;
                        else basis = Vaneaxial_HubRatio_6to8;
                        break;
                }

                double[] SWL = new double[8];

                for (int oct = 0; oct < 8; oct++)
                {
                    SWL[oct] = basis[oct] + 10 * Math.Log10(volume) + 20 * Math.Log10(downstream_pressure_inwg);
                }

                return SWL;
            }

            public static double[] PackagedAHU_Radiated(double FanHorsePower)
            {
                double Sum = 87 + 12 * Math.Log10(FanHorsePower);
                double[] SWL = new double[8];
                SWL[0] = Sum - 8;
                SWL[1] = Sum - 7;
                SWL[2] = Sum - 7;
                SWL[3] = Sum - 8;
                SWL[4] = Sum - 10;
                SWL[5] = Sum - 14;
                SWL[6] = Sum - 18;
                SWL[7] = Sum - 24;

                return SWL;
            }

            public static double[] CoolingTower_CentrifugalFan_Radiated(double FanHorsePower)
            {
                double Sum = (FanHorsePower > 80) ? 92 + 7 * Math.Log10(FanHorsePower) : 84 + 11 * Math.Log10(FanHorsePower);
                double[] SWL = new double[8];
                SWL[0] = Sum - 6;
                SWL[1] = Sum - 8;
                SWL[2] = Sum - 10;
                SWL[3] = Sum - 11;
                SWL[4] = Sum - 13;
                SWL[5] = Sum - 12;
                SWL[6] = Sum - 18;
                SWL[7] = Sum - 25;

                return SWL;
            }

            public static double[] CoolingTower_Propellerfan_Radiated(double FanHorsePower)
            {
                double Sum = (FanHorsePower > 100) ? 95 + 10 * Math.Log10(FanHorsePower) : 99 + 10 * Math.Log10(FanHorsePower);
                double[] SWL = new double[8];
                SWL[0] = Sum - 5;
                SWL[1] = Sum - 5;
                SWL[2] = Sum - 8;
                SWL[3] = Sum - 11;
                SWL[4] = Sum - 15;
                SWL[5] = Sum - 18;
                SWL[6] = Sum - 21;
                SWL[7] = Sum - 29;

                return SWL;
            }

            public static double[] Boiler(out double[] SWL_Worst, double HorsePower = 0, double kiloWattage = 0)
            {
                if (HorsePower > 300 || kiloWattage > 2950)
                {
                    SWL_Worst = new double[8] { 102, 101, 99, 96, 93, 90, 87, 84 };
                    return new double[8] { 102, 101, 99, 96, 93, 90, 87, 84 };
                }
                else
                {
                    SWL_Worst = new double[8] { 99, 98, 96, 93, 90, 87, 84, 81 };
                    return new double[8] { 99, 98, 96, 93, 90, 87, 84, 81 };
                }
            }

            public static double[] Pump(double horsepower, double speed_rpm)
            {
                MathNet.Numerics.Interpolation.IInterpolation basis = MathNet.Numerics.Interpolate.CubicSpline(new double[4] { 450d, 1000d, 1600d, 3000d }, new double[4] { 67, 69, 71, 74 }.ToList());

                double Sum = 0;

                Sum = (horsepower < 100) ? basis.Interpolate(speed_rpm) + 14 + 3 * Math.Log10(horsepower) : basis.Interpolate(speed_rpm) + 10 * Math.Log10(horsepower);

                double[] SWL = new double[8];
                SWL[0] = Sum - 12;
                SWL[1] = Sum - 11;
                SWL[2] = Sum - 9;
                SWL[3] = Sum - 9;
                SWL[4] = Sum - 6;
                SWL[5] = Sum - 9;
                SWL[6] = Sum - 13;
                SWL[7] = Sum - 19;

                return SWL;
            }

            public static double[] Transformer(double NEMA_Rating, double Area_of_Sidewalls, out double[] SWL_Worst)
            {
                double basis = NEMA_Rating + 10 * Math.Log10(Area_of_Sidewalls);

                double[] SWL = new double[8];
                SWL[0] = basis - 5;
                SWL[1] = basis - 3;
                SWL[2] = basis - 8;
                SWL[3] = basis - 8;
                SWL[4] = basis - 14;
                SWL[5] = basis - 19;
                SWL[6] = basis - 24;
                SWL[7] = basis - 31;

                SWL_Worst = new double[8];
                SWL_Worst[0] = basis - 2;
                SWL_Worst[1] = basis + 3;
                SWL_Worst[2] = basis + 2;
                SWL_Worst[3] = basis + 2;
                SWL_Worst[4] = basis - 4;
                SWL_Worst[5] = basis - 9;
                SWL_Worst[6] = basis - 14;
                SWL_Worst[7] = basis - 21;

                return SWL;
            }

            public static double[] Generator_radiated(double Wattage_MW, double speed_rpm)
            {
                MathNet.Numerics.Interpolation.IInterpolation speed_term = MathNet.Numerics.Interpolate.CubicSpline(new double[6] { 600, 1200, 1800, 2400, 3600, 4800 }, new double[6] { 0, 2, 3, 4, 5, 6 });
                MathNet.Numerics.Interpolation.IInterpolation wattage_term = MathNet.Numerics.Interpolate.CubicSpline(new double[8] { .2, .5, 1, 2, 5, 10, 20, 50 }, new double[8] { 95, 99, 102, 105, 109, 112, 115, 119 });

                double basis = wattage_term.Interpolate(Wattage_MW) + speed_term.Interpolate(speed_rpm);

                // Adjustments for each octave band (example values, should be based on empirical data)
                double[] SWL = new double[8];
                SWL[0] = basis - 6;
                SWL[1] = basis - 8;
                SWL[2] = basis - 10;
                SWL[3] = basis - 11;
                SWL[4] = basis - 13;
                SWL[5] = basis - 12;
                SWL[6] = basis - 18;
                SWL[7] = basis - 25;

                return SWL;
            }

            public enum ChillerType
            {
                Chiller_Compressor,
                Chiller_Screw,
                Chiller_InternallyGeared,
                Chiller_DirectDrive,
            }

            public static double[] Chiller_radiated(ChillerType type, double Wattage_kW, double volume_T)
            {
                double[] SWL = new double[8];

                switch (type)
                {
                    case ChillerType.Chiller_Compressor:
                        SWL = new double[8] { 91, 92, 93, 94, 93, 90, 86, 81 };
                        if (Wattage_kW > 109)
                        {
                            double mod = 1;
                            if (Wattage_kW < 285) mod = (Wattage_kW - 108) / (283 - 108);
                            SWL[0] += 2 * mod;
                            SWL[1] += 2 * mod;
                            SWL[2] += 3 * mod;
                            SWL[3] += 3 * mod;
                            SWL[4] += 3 * mod;
                            SWL[5] += 3 * mod;
                            SWL[6] += 3 * mod;
                            SWL[7] += 3 * mod;
                        }
                        if (Wattage_kW > 285)
                        {
                            double mod = 1;
                            if (Wattage_kW < 1055) mod = (Wattage_kW - 284) / (1055 - 284);
                            SWL[0] += 2 * mod;
                            SWL[1] += 2 * mod;
                            SWL[2] += 3 * mod;
                            SWL[3] += 3 * mod;
                            SWL[4] += 3 * mod;
                            SWL[5] += 3 * mod;
                            SWL[6] += 3 * mod;
                            SWL[7] += 3 * mod;
                        }
                        break;
                    case ChillerType.Chiller_Screw:
                        SWL = new double[8] { 84, 88, 100, 97, 93, 88, 83, 81 };
                        break;
                    case ChillerType.Chiller_InternallyGeared:
                        if (volume_T > 1000)
                        {
                            double basic = 93 + 6 * Math.Log10(volume_T);
                            SWL[0] = basic - 11;
                            SWL[1] = basic - 10;
                            SWL[2] = basic - 9;
                            SWL[3] = basic - 11;
                            SWL[4] = basic - 6;
                            SWL[5] = basic - 6;
                            SWL[6] = basic - 11;
                            SWL[7] = basic - 17;
                        }
                        else
                        {
                            double basic = 83 + 9 * Math.Log10(volume_T);
                            SWL[0] = basic - 9;
                            SWL[1] = basic - 8;
                            SWL[2] = basic - 7;
                            SWL[3] = basic - 10;
                            SWL[4] = basic - 8;
                            SWL[5] = basic - 8;
                            SWL[6] = basic - 12;
                            SWL[7] = basic - 19;
                        }
                        break;
                    case ChillerType.Chiller_DirectDrive:
                        if (volume_T > 1000)
                        {
                            double basic = 93 + 6 * Math.Log10(volume_T);
                            SWL[0] = basic - 11;
                            SWL[1] = basic - 10;
                            SWL[2] = basic - 9;
                            SWL[3] = basic - 11;
                            SWL[4] = basic - 6;
                            SWL[5] = basic - 6;
                            SWL[6] = basic - 11;
                            SWL[7] = basic - 17;
                        }
                        else
                        {
                            double basic = 68 + 14 * Math.Log10(volume_T);
                            SWL[0] = basic - 8;
                            SWL[1] = basic - 7;
                            SWL[2] = basic - 6;
                            SWL[3] = basic - 10;
                            SWL[4] = basic - 10;
                            SWL[5] = basic - 10;
                            SWL[6] = basic - 14;
                            SWL[7] = basic - 21;
                        }
                        break;
                }
                return SWL;
            }
        }
    }
}