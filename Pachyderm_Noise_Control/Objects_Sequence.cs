using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hare.Geometry;

namespace Pachyderm_Noise_Control
{
    public enum frequency
    {
        f63 = 0,
        f125 = 1,
        f250 = 2,
        f500 = 3,
        f1000 = 4,
        f2000 = 5,
        f4000 = 6,
        f8000 = 7
    }

    public static class Conversions
    {
        public static double[] A_Weights = new double[8] { -26.2, -16.1, -8.6, -3.2, 0, 1.2, 1, -1.1 };
        public static double feet2meters = 0.3048;
        public static double meters2feet = 3.2808399;
    }

    public class State:ICloneable
    {
        public double diameter_last;
        public double area_last;
        public double Volume; //CFM
        public double Velocity; //FPM
        public double[] Noise_Best;
        public double[] Noise_Worst;
        public double[] dimensions = new double[2];
        public double lining_thickness = 0;
        public Vector[] Frame = new Vector[3]; //N(Z),W(X),H(Y)
        public int ModelNode;
        public double[] ModelPower;
        public int PathId = -1;

        public State(double _Volume, double _horizontal, double _vertical, double[] _Noise_Best, double[] _Noise_Worst, Vector Dir, Vector W_dir, Vector H_dir, double lining = 0)
        {
            lining_thickness = lining;
            dimensions[0] = _horizontal;
            dimensions[1] = _vertical;
            area_last = _horizontal * _vertical / 144;
            diameter_last = Math.Sqrt(4 * area_last / Math.PI);
            Volume = _Volume;
            Velocity = _Volume / area_last;
            if (_Noise_Best.Length != 8 || _Noise_Worst.Length != 8) throw new Exception("Input to State type should include all 8 octave bands from 63 Hz. to 8 kHz.");
            Noise_Best = _Noise_Best;
            Noise_Worst = _Noise_Worst;
            Frame[0] = Dir;
            Frame[1] = W_dir;
            Frame[2] = H_dir;
            for (int i = 0; i < 3; i++) Frame[i].Normalize();
        }

        public void Resize(double _horizontal, double _vertical)
        {
            dimensions[0] = _horizontal;
            dimensions[1] = _vertical;
            area_last = _horizontal * _vertical/144;
            diameter_last = Math.Sqrt(4 * area_last / Math.PI);
            Velocity = Volume / area_last;
        }

        public void Resize(double _Diameter)
        {
            dimensions[0] = _Diameter;
            area_last = (Math.PI * _Diameter * _Diameter / 4) / 144;
            diameter_last = _Diameter;
            Velocity = Volume / area_last;
        }

        public object Clone()
        {
            double[] NB = new double[8];
            double[] NW = new double[8];
            for (int oct = 0; oct < 8; oct++)
            {
                NB[oct] = Noise_Best[oct];
                NW[oct] = Noise_Worst[oct];
            }

            State S = new State(this.Volume, this.dimensions[0], this.dimensions[1], NB, NW, new Vector(Direction.dx, Direction.dy, Direction.dz), new Vector(Frame[1].dx, Frame[1].dy, Frame[1].dz), new Vector(Frame[2].dx, Frame[2].dy, Frame[2].dz), this.lining_thickness);
            S.PathId = PathId;

            return S;
        }

        public double SPL_A(bool worst = false)
        {
            double SPLA = 0;
            double[] SPL = worst? Noise_Worst : Noise_Best ;

            for(int oct = 0; oct < 8; oct++) SPLA += Math.Pow(10, (SPL[oct] + Conversions.A_Weights[oct]) / 10);

            return 10 * Math.Log10(SPLA);
        }

        public static State operator + (State S, double[] noise)
        {
            State S_New = S.Clone() as State;

            if (noise.Length == 8)
            {
                for (int i = 0; i < 8; i++)
                {
                    S_New.Noise_Best[i] = 10 * Math.Log10(Math.Pow(10, S.Noise_Best[i] / 10) + Math.Pow(10, noise[i]/10));
                    S_New.Noise_Worst[i] = 10 * Math.Log10(Math.Pow(10, S.Noise_Worst[i] / 10) + Math.Pow(10, (noise[i] + 5)/10));
                }
            }
            else if (noise.Length == 16)
            {
                for (int i = 0; i < 8; i++)
                {
                    S_New.Noise_Best[i] = 10 * Math.Log10(Math.Pow(10, S.Noise_Best[i] / 10) + Math.Pow(10, noise[i] / 10));
                }
                for (int i = 8; i < 16; i++)
                {
                    S_New.Noise_Worst[i - 8] = 10 * Math.Log10(Math.Pow(10, S.Noise_Worst[i - 8] / 10) + Math.Pow(10, noise[i] / 10));
                }
            }
            else
            {
                throw new InvalidOperationException();
            }

            return S_New;
        }

        public static State operator -(State S, double[] atten)
        {
            State S_New = S.Clone() as State;

            if (atten.Length == 8)
            {
                for (int i = 0; i < 8; i++)
                {
                    S_New.Noise_Best[i] = S.Noise_Best[i] - atten[i];
                    S_New.Noise_Worst[i] = S.Noise_Worst[i] - atten[i];
                }
            }
            else if (atten.Length == 16)
            {
                for (int i = 0; i < 8; i++)
                {
                    S_New.Noise_Best[i] = S.Noise_Best[i] - atten[i];
                }
                for (int i = 8; i < 16; i++)
                {
                    S_New.Noise_Worst[i - 8] = S.Noise_Worst[i - 8] - atten[i];
                }
            }
            else
            {
                throw new InvalidOperationException();
            }

            return S_New;
        }

        public Hare.Geometry.Vector Direction
        {
            set
            {
                //all vectors must be normalized.
                Frame[0] = new Vector(value.dx, value.dy, value.dz);
                double dAlt = Math.Asin(value.dz);
                double dAzi = Math.Atan(value.dy / value.dx);

                Frame[1] = new Vector(Math.Cos(dAzi + Math.PI/2) * Math.Sin(dAlt), Math.Sin(dAzi + Math.PI/2) * Math.Sin(dAlt), 0);
                Frame[2] = new Vector(Math.Cos(dAzi) * Math.Cos(dAlt + Math.PI/2), Math.Sin(dAzi) * Math.Sin(dAlt + Math.PI/2), Math.Sin(dAlt + Math.PI/2));
            }

            get
            {
                return Frame[0];
            }
        }

        public Hare.Geometry.Point Location
        {
            get
            {
                return Duct_Model.Instance.Location(ModelNode, PathId);
            }
        }

        public override string ToString()
        {
            if (Duct_Model.Instance.Connected)
            {
                double[] SPL = Duct_Model.Instance.Sound_Pressure_Level(Duct_Model.NoiseType.Total, PathId, ModelNode);
                return "F      63  125  250  500   1k   2k   4k   8k" + "\n" + "SWL   " + Math.Round(SPL[0], 1) +" " + Math.Round(SPL[1], 1) + " " + Math.Round(SPL[2], 1) + " " + Math.Round(SPL[3], 1) + " " + Math.Round(SPL[4], 1) + " " + Math.Round(SPL[5], 1) + " " + Math.Round(SPL[6], 1) + " " + Math.Round(SPL[7], 1) + "\n" + "Volume   " + Volume + " CFM" + "   SPL = " + this.SPL_A() + " dBA";
            }
            else return "F      63  125  250  500   1k   2k   4k   8k" + "\n" + "SWL   " + Math.Round(Noise_Best[0], 1) + " " + Math.Round(Noise_Best[1], 1) + " " + Math.Round(Noise_Best[2], 1) + " " + Math.Round(Noise_Best[3], 1) + " " + Math.Round(Noise_Best[4], 1) + " " + Math.Round(Noise_Best[5], 1) + " " + Math.Round(Noise_Best[6], 1) + " " + Math.Round(Noise_Best[7], 1) + "\n" + "Volume   " + Volume + " CFM" + "   SPL = " + this.SPL_A() + " dBA";

        }
    }
}
