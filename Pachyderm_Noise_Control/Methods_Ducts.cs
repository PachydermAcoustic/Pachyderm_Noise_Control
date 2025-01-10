//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Pachyderm_Noise_Control
//{
//    namespace ASHRAE_Components
//    {
//        public class Noise_Source : component
//        {
//            string Source_Name;

//            public Noise_Source(string _name, double[] _Noise, double _volume_in_cfm)
//            {
//                Source_Name = _name;
//                Noise = _Noise;
//                volume = _volume_in_cfm;
//            }

//            public override string name()
//            {
//                return Source_Name;
//            }
//        }

//        public class Round_Duct_Unlined : component
//        {
//            //Constants for unlined ducts...
//            static double[][] attenuation = new double[5][]
//            {
//                new double[8]{.03, .03, .05, .05, .1, .1, .1, .1 }, // 4 inches
//                new double[8]{.03, .03, .05, .05, .1, .1, .1, .1 }, //7 inches
//                new double[8]{.03, .03, .03, .05, .07, .07, .07, .07 }, //15 inches
//                new double[8]{.02, .02, .02, .03, .05, .05, .05, .05 }, //30 inches
//                new double[8]{.01, .01, .02, .02, .02, .02, .02, .02 }, //60 inches
//            };

//            public override string name() { return "Unlined round duct"; }

//            public Round_Duct_Unlined(Stack<component> duct_train, double vertical_inches, double horizontal_inches, double _length)
//            {
//                this.length = _length;
//                section = new double[2] { horizontal_inches, vertical_inches };

//                double smallerdim = section.Min();
//                double[] a_ft;

//                if (smallerdim < 4 / 12)
//                {
//                    a_ft = attenuation[0];
//                }
//                else if (smallerdim < 7 / 12)
//                {
//                    a_ft = attenuation[1];
//                }
//                else if (smallerdim < 15 / 12)
//                {
//                    a_ft = attenuation[2];
//                }
//                else if (smallerdim < 30 / 12)
//                {
//                    a_ft = attenuation[3];
//                }
//                else
//                {
//                    a_ft = attenuation[4];
//                }

//                volume = duct_train.Peek().volume;
//                velocity = duct_train.Peek().volume;

//                Noise = new double[8];
//                double[] levels = duct_train.Peek().Total_Level;
//                for (int oct = 0; oct < 8; oct++) Noise[oct] = levels[oct] + length * a_ft[oct];
//            }
//        }

//        public class Round_Duct_1inchlined : component
//        {
//            //Constants for unlined ducts...
//            static double[][] attenuation = new double[28][]
//            {
//                new double[8]{ 0.38, 0.59, 0.93, 1.53, 2.17, 2.31, 2.04, 1.26 },//6
//                new double[8]{ 0.32, 0.54, 0.89, 1.50, 2.19, 2.17, 1.83, 1.18 },//8
//                new double[8]{ 0.27, 0.50, 0.85, 1.48, 2.20, 2.04, 1.64, 1.12 },//10
//                new double[8]{ 0.23, 0.46, 0.81, 1.45, 2.18, 1.91, 1.48, 1.05 },//12
//                new double[8]{ 0.19, 0.42, 0.77, 1.43, 2.14, 1.79, 1.34, 1.00 },//14
//                new double[8]{ 0.16, 0.38, 0.73, 1.40, 2.08, 1.67, 1.21, 0.95 },//16
//                new double[8]{ 0.13, 0.35, 0.69, 1.37 ,2.01, 1.56, 1.10, 0.90 },//18
//                new double[8]{ 0.11, 0.31, 0.65, 1.34, 1.92, 1.45, 1.00, 0.87 },//20
//                new double[8]{ 0.08, 0.28, 0.61 ,1.31 ,1.82, 1.34, 0.92, 0.83},//22
//                new double[8]{ 0.07, 0.25, 0.57 ,1.28 ,1.71, 1.24, 0.85, 0.80},//24
//                new double[8]{ 0.05, 0.22, 0.53, 1.24, 1.59, 1.14, 0.79, 0.77},//26
//                new double[8]{ 0.03, 0.19, 0.49, 1.20, 1.46, 1.04, 0.74, 0.74},//28
//                new double[8]{ 0.02, 0.16, 0.45, 1.16, 1.33, 0.95, 0.69, 0.71},//30
//                new double[8]{ 0.01, 0.14, 0.42, 1.12, 1.20, 0.87, 0.66, 0.69},//32
//                new double[8]{ 0, 0.11, 0.38, 1.07, 1.07, 0.79, 0.63, 0.66},//34
//                new double[8]{0, 0.08, 0.35, 1.02, 0.93, 0.71, 0.60, 0.64},//36
//                new double[8]{0, 0.06, 0.31, 0.96, 0.80, 0.64, 0.58, 0.61},//38
//                new double[8]{0, 0.03, 0.28, 0.91, 0.68, 0.57, 0.55, 0.58},//40
//                new double[8]{0, 0.01, 0.25, 0.84, 0.56, 0.50, 0.53, 0.55},//42
//                new double[8]{0, 0, 0.23, 0.78, 0.45,0.44, 0.51, 0.52},//44
//                new double[8]{0, 0, 0.20, 0.71, 0.35, 0.39, 0.48, 0.48},//46
//                new double[8]{0, 0, 0.18, 0.63, 0.26, 0.34, 0.45, 0.44},//48
//                new double[8]{0, 0, 0.15, 0.55, 0.19, 0.29, 0.41, 0.40},//50
//                new double[8]{0, 0, 0.14, 0.46, 0.13, 0.25, 0.37, 0.34},//52
//                new double[8]{0, 0, 0.12, 0.37, 0.09, 0.22, 0.31, 0.29},//54
//                new double[8]{0, 0, 0.10, 0.28, 0.08, 0.18, 0.25, 0.22},//56
//                new double[8]{0, 0, 0.09, 0.17, 0.08,0.16, 0.18, 0.15},//58
//                new double[8]{0, 0, 0.08, 0.06, 0.10, 0.14, 0.09, 0.07}//60
//            };

//            public override string name() { return "1-inch lined round duct"; }

//            public Round_Duct_1inchlined(Stack<component> duct_train, double diameter, double _length)
//            {
//                this.length = _length;
//                section = new double[2] { diameter, 0 };

//                double smallerdim = section.Min();
//                int idx = (int)Math.Max(0, Math.Ceiling((diameter - 6) / 2));

//                volume = duct_train.Peek().volume;
//                velocity = duct_train.Peek().volume;

//                Noise = new double[8];
//                double[] levels = duct_train.Peek().Total_Level;
//                for (int oct = 0; oct < 8; oct++) Noise[oct] = levels[oct] + length * attenuation[idx][oct];
//            }


//        }

//        public class Round_Duct_2inchlined : component
//        {
//            //Constants for unlined ducts...
//            static double[][] attenuation = new double[28][]{
//                new double[8]{ 0.56, 0.80, 1.37, 2.25, 2.17, 2.31, 2.04, 1.26},//6
//                new double[8]{ 0.51, 0.75, 1.33, 2.23, 2.19, 2.17, 1.83, 1.18},//8
//                new double[8]{ 0.46, 0.71, 1.29, 2.20, 2.20, 2.04, 1.64, 1.12},//10
//                new double[8]{ 0.42, 0.67, 1.25, 2.18, 2.18, 1.91, 1.48, 1.05},//12
//                new double[8]{ 0.38, 0.63, 1.21, 2.15, 2.14, 1.79, 1.34, 1.00},//14
//                new double[8]{ 0.35, 0.59, 1.17, 2.12, 2.08, 1.67, 1.21, 0.95},//16
//                new double[8]{ 0.32, 0.56, 1.13, 2.10, 2.01, 1.56, 1.10, 0.90},//18
//                new double[8]{ 0.29, 0.52, 1.09, 2.07, 1.92, 1.45, 1.00, 0.87},//20
//                new double[8]{ 0.27, 0.49, 1.05, 2.03, 1.82, 1.34, 0.92, 0.83},//22
//                new double[8]{ 0.25, 0.46, 1.01, 2.00, 1.71, 1.24, 0.85, 0.80},//24
//                new double[8]{ 0.24, 0.43, 0.97, 1.96, 1.59, 1.14, 0.79, 0.77},//26
//                new double[8]{ 0.22, 0.40, 0.93, 1.93, 1.46, 1.04, 0.74, 0.74},//28
//                new double[8]{ 0.21, 0.37, 0.90, 1.88, 1.33, 0.95, 0.69, 0.71},//30
//                new double[8]{ 0.20, 0.34, 0.86, 1.84, 1.20, 0.87, 0.66, 0.69},//32
//                new double[8]{ 0.19, 0.32, 0.82, 1.79, 1.07, 0.79, 0.63, 0.66},//34
//                new double[8]{ 0.18, 0.29, 0.79, 1.74, 0.93, 0.71, 0.60, 0.64},//36
//                new double[8]{ 0.17, 0.27, 0.76, 1.69, 0.80, 0.64, 0.58, 0.61},//38
//                new double[8]{ 0.16, 0.24, 0.73, 1.63, 0.68, 0.57, 0.55, 0.58},//40
//                new double[8]{ 0.15, 0.22, 0.70, 1.57, 0.56, 0.50, 0.53, 0.55},//42
//                new double[8]{ 0.13, 0.20, 0.67, 1.50, 0.45, 0.44, 0.51, 0.52},//44
//                new double[8]{ 0.12, 0.17, 0.64, 1.43, 0.35, 0.39, 0.48, 0.48},//46
//                new double[8]{ 0.11, 0.15, 0.62, 1.36, 0.26, 0.34, 0.45, 0.44},//48
//                new double[8]{ 0.09, 0.12, 0.60, 1.28, 0.19, 0.29, 0.41, 0.40},//50
//                new double[8]{ 0.07, 0.10, 0.58, 1.19, 0.13, 0.25, 0.37, 0.34},//52
//                new double[8]{ 0.05, 0.08, 0.56, 1.10, 0.09, 0.22, 0.31, 0.29},//54
//                new double[8]{ 0.02, 0.05, 0.55, 1.00, 0.08, 0.18, 0.25, 0.22},//56
//                new double[8]{ 0.00, 0.03, 0.53, 0.90, 0.08, 0.16, 0.18, 0.15},//58
//                new double[8]{ 0.00, 0.00, 0.53, 0.79, 0.10, 0.14, 0.09, 0.07}//60
//            };

//            public override string name() { return "2-inch lined rectangular duct"; }

//            public Round_Duct_2inchlined(Stack<component> ductrain, double diameter, double _length)
//            {
//                this.length = _length;
//                section = new double[2] { diameter, 0 };

//                double smallerdim = section.Min();
//                int idx = (int)Math.Max(0, Math.Ceiling((diameter - 6) / 2));

//                volume = duct_train.Peek().volume;
//                velocity = duct_train.Peek().volume;

//                Noise = new double[8];
//                double[] levels = duct_train.Peek().Total_Level;
//                for (int oct = 0; oct < 8; oct++) Noise[oct] = levels[oct] + length * attenuation[idx][oct];
//            }
//        }

//        public class Rectangular_Duct_Unlined : component
//        {
//            //Constants for unlined ducts...
//            static double[][] attenuation = new double[12][]{
//                new double[8]{.4, .4, .25, .15, .1, .1, .1, .1 }, // 4 inches
//                new double[8]{.4, .4, .25, .15, .1, .1, .1, .1 }, //10 inches
//                new double[8]{.4, .3, .15, .15, .1, .1, .1, .1 }, //16 inches
//                new double[8]{.4, .3, .15, .15, .1, .1, .1, .1 }, //22 inches
//                new double[8]{.4, .3, .15, .15, .1, .1, .1, .1 }, //28 inches
//                new double[8]{.4, .3, .15, .15, .1, .1, .1, .1 }, //34 inches
//                new double[8]{.4, .3, .15, .15, .1, .1, .1, .1 }, //40 inches
//                new double[8]{.3, .25, .15, .15, .1, .1, .1, .1 }, //46 inches
//                new double[8]{.3, .25, .15, .15, .1, .1, .1, .1 }, //55 inches
//                new double[8]{.3, .25, .15, .15, .1, .1, .1, .1 }, //67 inches
//                new double[8]{.3, .25, .15, .15, .1, .1, .1, .1 }, //79 inches
//                new double[8]{.3, .25, .15, .15, .1, .1, .1, .1 } //over 90 inches
//                };

//            public override string name() { return "Unlined rectangular duct"; }

//            public Rectangular_Duct_Unlined(Stack<component> duct_train, double vertical_inches, double horizontal_inches, double _length)
//            {
//                this.length = _length;
//                section = new double[2] { horizontal_inches, vertical_inches };

//                double smallerdim = section.Min();
//                double[] a_ft;

//                if (smallerdim < 10 / 12)
//                {
//                    a_ft = attenuation[0];
//                }
//                else if (smallerdim < 16 / 12)
//                {
//                    a_ft = attenuation[1];
//                }
//                else if (smallerdim < 22 / 12)
//                {
//                    a_ft = attenuation[2];
//                }
//                else if (smallerdim < 28 / 12)
//                {
//                    a_ft = attenuation[3];
//                }
//                else if (smallerdim < 34 / 12)
//                {
//                    a_ft = attenuation[4];
//                }
//                else if (smallerdim < 40 / 12)
//                {
//                    a_ft = attenuation[5];
//                }
//                else if (smallerdim < 46 / 12)
//                {
//                    a_ft = attenuation[6];
//                }
//                else if (smallerdim < 55 / 12)
//                {
//                    a_ft = attenuation[7];
//                }
//                else if (smallerdim < 67 / 12)
//                {
//                    a_ft = attenuation[8];
//                }
//                else if (smallerdim < 79 / 12)
//                {
//                    a_ft = attenuation[9];
//                }
//                else if (smallerdim < 90 / 12)
//                {
//                    a_ft = attenuation[10];
//                }
//                else
//                {
//                    a_ft = attenuation[10];
//                }

//                volume = duct_train.Peek().volume;
//                velocity = duct_train.Peek().volume;

//                Noise = new double[8];
//                double[] levels = duct_train.Peek().Total_Level;
//                for (int oct = 0; oct < 8; oct++) Noise[oct] = levels[oct] + length * a_ft[oct];
//            }
//        }

//        public class Rectangular_Duct_1inchlined : component
//        {
//            //Constants for unlined ducts...
//            static double[][][] attenuation = new double[7][][]
//            {
//                new double[4][]{ new double[8]{ 0.5, 0.5, 1.2, 2.3, 5.0, 5.8, 3.6, 3.6 }, new double[8]{ 0.4, 0.4, 1.0, 2.1, 4.5, 4.9, 3.2, 3.2 }, new double[8]{ 0.4, 0.4, 0.9, 2.0, 4.3, 4.5, 3.0, 3.0 }, new double[8]{ 0.4, 0.4, 0.8, 1.9, 4.0, 4.1, 2.8, 2.8 }},
//                new double[4][]{ new double[8]{ 0.4, 0.4, 0.8, 1.9, 4, 4.1, 2.8, 2.8 }, new double[8]{ 0.3, 0.3, 0.7, 1.7, 3.7, 3.5, 2.5, 2.5 }, new double[8]{ 0.3, 0.3, 0.6, 1.7, 3.5, 3.2, 2.3, 2.3 }, new double[8]{ 0.3, 0.3, 0.6, 1.6, 3.3, 2.9, 2.2, 2.2 }},
//                new double[4][]{ new double[8]{ 0.3, 0.3, 0.6, 1.6, 3.3, 2.9, 2.2, 2.2 }, new double[8]{ 0.2, 0.2, 0.5, 1.4, 3.0, 2.4, 1.9, 1.9 }, new double[8]{ 0.2, 0.2, 0.5, 1.4, 2.8, 2.2, 1.8, 1.8 }, new double[8]{ 0.2, 0.2, 0.4, 1.3, 2.7, 2.0, 1.7, 1.7 }},
//                new double[4][]{ new double[8]{0.2, 0.2, 0.5, 1.4, 2.8, 2.2, 1.8, 1.8}, new double[8]{0.2, 0.2, 0.4, 1.2, 2.6, 1.9, 1.6, 1.6}, new double[8]{0.2, 0.2, 0.4, 1.1, 2.4, 1.7, 1.5, 1.5}, new double[8]{0.2, 0.2, 0.3, 1.1, 2.3, 1.6, 1.4, 1.4}},
//                new double[4][]{ new double[8]{0.2, 0.2, 0.4, 1.2, 2.5, 1.8, 1.6, 1.6}, new double[8]{0.2, 0.2, 0.3, 1.1, 2.3, 1.6, 1.4, 1.4}, new double[8]{0.2, 0.2, 0.3, 1.1, 2.2, 1.4, 1.3, 1.3}, new double[8]{0.1, 0.1, 0.3, 1.0, 2.1, 1.3, 1.2, 1.2}},
//                new double[4][]{ new double[8]{0.2, 0.2, 0.3, 1.0, 2.1, 1.4, 1.3, 1.3}, new double[8]{0.1, 0.1, 0.2, 0.9, 1.9, 1.2, 1.1, 1.1}, new double[8]{0.1, 0.1, 0.2, 0.8, 1.8, 1.1, 1.1, 1.1}, new double[8]{0.1, 0.1, 0.2, 0.8, 1.7, 1.0, 1.0, 1.0}},
//                new double[4][]{ new double[8]{0.1, 0.1, 0.9, 1.0, 2.0, 1.2, 1.2, 1.2}, new double[8]{0.1, 0.1, 0.2, 0.9, 1.8, 1.0, 1.0, 1.0}, new double[8]{0.1, 0.1, 0.2, 0.8, 1.7, 1.0, 1.0, 1.0}, new double[8]{0.1, 0.1, 0.2, 0.8, 1.6, 0.9, 0.9, 0.9}}
//            };

//            public override string name() { return "1-inch lined rectangular duct"; }

//            public Rectangular_Duct_1inchlined(Stack<component> duct_train, double vertical_inches, double horizontal_inches, double _length)
//            {
//                this.length = _length;
//                section = new double[2] { horizontal_inches, vertical_inches };

//                double smallerdim = section.Min();
//                double largerdim = section.Max();
//                double[] a_ft;

//                if (smallerdim < 8 / 12)
//                {
//                    if (largerdim < 12 / 12) a_ft = attenuation[0][0];
//                    else if (largerdim < 16 / 12) a_ft = attenuation[0][1];
//                    else if (largerdim < 24 / 12) a_ft = attenuation[0][2];
//                    else a_ft = attenuation[0][3];
//                }
//                else if (smallerdim < 12 / 12)
//                {
//                    if (largerdim < 18 / 12) a_ft = attenuation[1][0];
//                    else if (largerdim < 24 / 12) a_ft = attenuation[1][1];
//                    else if (largerdim < 36 / 12) a_ft = attenuation[1][2];
//                    else a_ft = attenuation[1][3];
//                }
//                else if (smallerdim < 18 / 12)
//                {
//                    if (largerdim < 28 / 12) a_ft = attenuation[2][0];
//                    else if (largerdim < 36 / 12) a_ft = attenuation[2][1];
//                    else if (largerdim < 54 / 12) a_ft = attenuation[2][2];
//                    else a_ft = attenuation[2][3];
//                }
//                else if (smallerdim < 24 / 12)
//                {
//                    if (largerdim < 36 / 12) a_ft = attenuation[3][0];
//                    else if (largerdim < 48 / 12) a_ft = attenuation[3][1];
//                    else if (largerdim < 72 / 12) a_ft = attenuation[3][2];
//                    else a_ft = attenuation[3][3];
//                }
//                else if (smallerdim < 30 / 12)
//                {
//                    if (largerdim < 45 / 12) a_ft = attenuation[4][0];
//                    else if (largerdim < 60 / 12) a_ft = attenuation[4][1];
//                    else if (largerdim < 90 / 12) a_ft = attenuation[4][2];
//                    else a_ft = attenuation[4][3];
//                }
//                else if (smallerdim < 42 / 12)
//                {
//                    if (largerdim < 64 / 12) a_ft = attenuation[5][0];
//                    else if (largerdim < 84 / 12) a_ft = attenuation[5][1];
//                    else if (largerdim < 126 / 12) a_ft = attenuation[5][2];
//                    else a_ft = attenuation[5][3];
//                }
//                else
//                {
//                    if (largerdim < 72 / 12) a_ft = attenuation[6][0];
//                    else if (largerdim < 96 / 12) a_ft = attenuation[6][1];
//                    else if (largerdim < 144 / 12) a_ft = attenuation[6][2];
//                    else a_ft = attenuation[6][3];
//                }

//                volume = duct_train.Peek().volume;
//                velocity = duct_train.Peek().volume;

//                Noise = new double[8];
//                double[] levels = duct_train.Peek().Total_Level;
//                for (int oct = 0; oct < 8; oct++) Noise[oct] = levels[oct] + length * a_ft[oct];
//            }
//        }

//        public class Rectangular_Duct_2inchlined : component
//        {
//            //Constants for unlined ducts...
//            static double[][][] attenuation = new double[7][][]
//            {
//                new double[4][]{ new double[8] { .6, .6, 2.3, 4.2, 6.2, 5.8, 3.6, 3.6 }, new double[8] { .6, .6, 1.9, 3.9, 5.6, 4.9, 3.2, 3.2 }, new double[8] { .5, .5, 1.8, 3.7, 5.4, 4.5, 3.0, 3.0 }, new double[8] { .5, .5, 1.6, 3.5, 5.0, 4.1, 2.8, 2.8 } }, // 8 inches
//                new double[4][]{ new double[8] { .5, .5, 1.6, 3.5, 5, 4.1, 2.8, 2.8 }, new double[8] { .4, .4, 1.4, 3.2, 4.6, 3.5, 2.5, 2.5 }, new double[8] { .4, .4, 1.3, 3, 4.3, 3.2, 2.3, 2.3 }, new double[8] { .4, .4, 1.2, 2.9, 4.1, 2.9, 2.2, 2.2 } }, // 12 inches
//                new double[4][]{ new double[8] { .4, .4, 1.2, 2.9, 4.1, 2.9, 2.2, 2.2 }, new double[8] { .3, .3, 1, 2.6, 3.7, 2.4, 1.9, 1.9 }, new double[8] { .3, .3, .9, 2.5, 3.5, 2.2, 1.8, 1.8 }, new double[8] { .3, .3, .8, 2.3, 3.3, 2, 1.7 , 1.7 } }, // 18 inches
//                new double[4][]{ new double[8] { .3, .3, .9, 2.5, 3.5, 2.2, 1.8, 1.8 }, new double[8] { .3, .3, .8, 2.3, 3.2, 1.9, 1.6, 1.6 }, new double[8] { .2, .2, .7, 2.2, 3, 1.7, 1.5, 1.5 }, new double[8] { .2, .2, .7, 2, 2.9, 1.6, 1.4, 1.4 } }, // 24 inches
//                new double[4][]{ new double[8] { .2, .2, .8, 2.2, 3.1, 1.8, 1.6, 1.6 }, new double[8] { .2, .2, .7, 2, 2.9, 1.6, 1.4, 1.4 }, new double[8] { .2, .2, .6, 1.9, 2.7, 1.4, 1.3, 1.3 }, new double[8] { .2, .2, .5, 1.8, 2.6, 1.3, 1.2, 1.2 } }, // 30 inches
//                new double[4][]{ new double[8] { .2, .2, .6, 1.9, 2.6, 1.4, 1.3, 1.3 }, new double[8] { .2, .2, .5, 1.7, 2.4, 1.2, 1.1, 1.1 }, new double[8] { .2, .2, .5, 1.6, 2.3, 1.1, 1.1, 1.1 }, new double[8] { .1, .1, .4, 1.6, 2.2, 1, 1, 1 } }, // 42 inches
//                new double[4][]{ new double[8] { .2, .2, .5, 1.8, 2.5, 1.2, 1.2, 1.2 }, new double[8] { .2, .2, .4, 1.6, 2.3, 1, 1, 1 }, new double[8] { .1, .1, .4, 1.5, 2.1, 1, 1, 1 }, new double[8] { .1, .1, .4, 1.5, 2, .9, .9, .9 } }, // 48 inches
//            };

//            public override string name() { return "2-inch lined rectangular duct"; }

//            public Rectangular_Duct_2inchlined(Stack<component> duct_train, double vertical_inches, double horizontal_inches, double _length)
//            {
//                this.length = _length;
//                section = new double[2] { horizontal_inches, vertical_inches };

//                double smallerdim = section.Min();
//                double largerdim = section.Max();
//                double[] a_ft;

//                if (smallerdim < 8 / 12)
//                {
//                    if (largerdim < 12 / 12) a_ft = attenuation[0][0];
//                    else if (largerdim < 16 / 12) a_ft = attenuation[0][1];
//                    else if (largerdim < 24 / 12) a_ft = attenuation[0][2];
//                    else a_ft = attenuation[0][3];
//                }
//                else if (smallerdim < 12 / 12)
//                {
//                    if (largerdim < 18 / 12) a_ft = attenuation[1][0];
//                    else if (largerdim < 24 / 12) a_ft = attenuation[1][1];
//                    else if (largerdim < 36 / 12) a_ft = attenuation[1][2];
//                    else a_ft = attenuation[1][3];
//                }
//                else if (smallerdim < 18 / 12)
//                {
//                    if (largerdim < 28 / 12) a_ft = attenuation[2][0];
//                    else if (largerdim < 36 / 12) a_ft = attenuation[2][1];
//                    else if (largerdim < 54 / 12) a_ft = attenuation[2][2];
//                    else a_ft = attenuation[2][3];
//                }
//                else if (smallerdim < 24 / 12)
//                {
//                    if (largerdim < 36 / 12) a_ft = attenuation[3][0];
//                    else if (largerdim < 48 / 12) a_ft = attenuation[3][1];
//                    else if (largerdim < 72 / 12) a_ft = attenuation[3][2];
//                    else a_ft = attenuation[3][3];
//                }
//                else if (smallerdim < 30 / 12)
//                {
//                    if (largerdim < 45 / 12) a_ft = attenuation[4][0];
//                    else if (largerdim < 60 / 12) a_ft = attenuation[4][1];
//                    else if (largerdim < 90 / 12) a_ft = attenuation[4][2];
//                    else a_ft = attenuation[4][3];
//                }
//                else if (smallerdim < 42 / 12)
//                {
//                    if (largerdim < 64 / 12) a_ft = attenuation[5][0];
//                    else if (largerdim < 84 / 12) a_ft = attenuation[5][1];
//                    else if (largerdim < 126 / 12) a_ft = attenuation[5][2];
//                    else a_ft = attenuation[5][3];
//                }
//                else
//                {
//                    if (largerdim < 72 / 12) a_ft = attenuation[6][0];
//                    else if (largerdim < 96 / 12) a_ft = attenuation[6][1];
//                    else if (largerdim < 144 / 12) a_ft = attenuation[6][2];
//                    else a_ft = attenuation[6][3];
//                }

//                volume = duct_train.Peek().volume;
//                velocity = duct_train.Peek().volume;

//                Noise = new double[8];
//                double[] levels = duct_train.Peek().Total_Level;
//                for (int oct = 0; oct < 8; oct++) Noise[oct] = levels[oct] + length * a_ft[oct];
//            }
//        }

//        public abstract class Elbow : component
//        {
//            public double[][] Attenuation;

//            public Elbow(Stack<component> duct_train, double vertical_inches, double horizontal_inches, double[][] _Attenuation)
//            {
//                Attenuation = _Attenuation;
//                section = new double[2] { horizontal_inches, vertical_inches };

//                double smallerdim = section.Min();
//                int idx;

//                if (smallerdim < 5 / 12)
//                {
//                    idx = 0;
//                }
//                else if (smallerdim < 14 / 12)
//                {
//                    idx = 1;
//                }
//                else if (smallerdim < 27 / 12)
//                {
//                    idx = 2;
//                }
//                else if (smallerdim < 44 / 12)
//                {
//                    idx = 3;
//                }
//                else
//                {
//                    idx = 4;
//                }

//                volume = duct_train.Peek().volume;
//                velocity = duct_train.Peek().volume;

//                Noise = new double[8];
//                double[] levels = duct_train.Peek().Total_Level;
//                for (int oct = 0; oct < 8; oct++) Noise[oct] = levels[oct] + Attenuation[idx][oct];

//                //calculate regenerated noise...
//                double sectionalarea = section[0] * section[1];
//                double diameter = Math.Sqrt(sectionalarea * 4 / Math.PI);
//                double logub = 50 * Math.Log10(velocity / 60);
//                double logsection = 10 * Math.Log10(sectionalarea);
//                double logdiameter = 10 * Math.Log10(diameter);

//                for (int oct = 0; oct < 8; oct++)
//                {
//                    double f = 31.25 * Math.Pow(2, oct);
//                    double st = f * diameter / velocity / 60;
//                    Regen[0] = -21.6 + 12.388 * Math.Pow(st, 0.673) - 16.482 * Math.Pow(1, -0.303) * Math.Log10(f * diameter / (velocity / 60)) - 5.047 * Math.Pow(1, -0.254) * Math.Pow(Math.Log10(f * diameter / velocity / 60), 2) + 10 * Math.Log10(f / 63) + logub + logsection + logdiameter + (6.793 - 1.86 * Math.Log10(st));
//                }
//                //=MAX(0,-21.6+12.388*$P51^0.673-16.482*$P51^-0.303*LOG(63*$M51/$N51)-5.047*$P51^-0.254*LOG(63*$M51/$N51)^2+10*LOG(63/63)+$Z51+$AA51+$AB51+(1-$Q51/0.15)*(6.793-1.86*LOG($R51))+$AC51)
//            }
//        }

//        public class Elbow_Unlined : Elbow
//        {

//            public Elbow_Unlined(Stack<component> duct_train, double vertical_inches, double horizontal_inches)
//                : base(duct_train, vertical_inches, horizontal_inches, new double[5][]
//                    {
//                        new double[8]{0, 0, 0, 0, 1, 2, 3, 3}, //5 inches
//                        new double[8]{0, 0, 0, 1, 2, 3, 3, 3}, //14 inches
//                        new double[8]{0, 0, 1, 2, 3, 3, 3, 3}, //27 inches
//                        new double[8]{0, 0, 1, 2, 3, 3, 3, 3}, //44 inches
//                        new double[8]{0, 0, 1, 2, 3, 3, 3, 3} //90 inches
//                    })
//            {
//            }

//            public override string name() { return "Unlined elbow"; }
//        }

//        public class Elbow_squarelined : Elbow
//        {

//            public Elbow_squarelined(Stack<component> duct_train, double vertical_inches, double horizontal_inches)
//                : base(duct_train, vertical_inches, horizontal_inches, new double[5][]
//                    {
//                        new double[8]{0, 0, 1, 2, 3, 4, 6, 8}, //5 inches
//                        new double[8]{0, 1, 2, 3, 4, 6, 8, 10}, //14 inches
//                        new double[8]{1, 2, 3, 4, 5, 6, 8, 10}, //27 inches
//                        new double[8]{2, 3, 4, 5, 6, 8, 10, 12}, //44 inches
//                        new double[8]{3, 4, 5, 6, 8, 10, 12, 12} //90 inches
//                    })
//            {
//            }

//            public override string name() { return "Unlined elbow"; }
//        }

//        public class Elbow_roundlined : Elbow
//        {

//            public Elbow_roundlined(Stack<component> duct_train, double vertical_inches, double horizontal_inches)
//                : base(duct_train, vertical_inches, horizontal_inches, new double[5][]
//                    {
//                    new double[8] { 0, 0, 0, 1, 2, 3, 4, 4 }, //5 inches
//                        new double[8] { 0, 0, 1, 2, 3, 4, 4, 5 }, //14 inches
//                        new double[8] { 0, 1, 2, 3, 4, 4, 5, 5 }, //27 inches
//                        new double[8] { 1, 2, 3, 4, 4, 5, 5, 6 }, //44 inches
//                        new double[8] { 2, 3, 4, 4, 5, 5, 6, 6 } //90 inches
//                    })
//            {
//            }

//            public override string name() { return "Unlined elbow"; }
//        }

//        public class Junction : component
//        {
//            double attenuation;

//            public Junction(Stack<component> duct_train, double vertical_inches_main, double horizontal_inches_main, double[] Vertical_inches_takeoff, double[] Horizontal_inches_takeoff)
//            {
//                section = new double[2] { horizontal_inches_main, vertical_inches_main };

//                volume = duct_train.Peek().volume;
//                velocity = duct_train.Peek().volume;

//                Noise = new double[8];
//                double[] levels = duct_train.Peek().Total_Level;
//                double[] start_section = duct_train.Peek().section_imperial;

//                double areamain = vertical_inches_main * horizontal_inches_main;
//                double area_other = 0;
//                for (int i = 0; i < Horizontal_inches_takeoff.Length; i++)
//                {
//                    area_other += Horizontal_inches_takeoff[i] * Vertical_inches_takeoff[i];
//                }

//                double prev_volume = duct_train.Peek().volume;
//                volume = prev_volume * areamain / (areamain + area_other);
//                attenuation = 10 * Math.Log10(volume / prev_volume);
//                for (int oct = 0; oct < 8; oct++)
//                {
//                    levels[oct] += attenuation;
//                }

//                //calculate regenerated noise...
//                double sectionalarea = section[0] * section[1];
//                velocity /= sectionalarea;
//                double diameter = Math.Sqrt(sectionalarea * 4 / Math.PI);
//                double logub = 50 * Math.Log10(velocity / 60);
//                double logsection = 10 * Math.Log10(sectionalarea);
//                double logdiameter = 10 * Math.Log10(diameter);

//                for (int oct = 0; oct < 8; oct++)
//                {
//                    double f = 31.25 * Math.Pow(2, oct);
//                    double st = f * diameter / velocity / 60;
//                    Regen[0] = -21.6 + 12.388 * Math.Pow(st, 0.673) - 16.482 * Math.Pow(1, -0.303) * Math.Log10(f * diameter / (velocity / 60)) - 5.047 * Math.Pow(1, -0.254) * Math.Pow(Math.Log10(f * diameter / velocity / 60), 2) + 10 * Math.Log10(f / 63) + logub + logsection + logdiameter + (6.793 - 1.86 * Math.Log10(st));
//                }
//                //=MAX(0,-21.6+12.388*$P51^0.673-16.482*$P51^-0.303*LOG(63*$M51/$N51)-5.047*$P51^-0.254*LOG(63*$M51/$N51)^2+10*LOG(63/63)+$Z51+$AA51+$AB51+(1-$Q51/0.15)*(6.793-1.86*LOG($R51))+$AC51)
//            }
//        }
//    }
//}