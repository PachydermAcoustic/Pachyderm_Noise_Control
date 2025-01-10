using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pachyderm_Noise_Control
{
    public static class VAV
    {
        public class Terminal
        {
            string name;
            string PressureSystem;
            List<double> Static_Pressure = new List<double>();
            System.Collections.Generic.Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation>[,] RadiatedByV; //[oct, static_Pa]
            System.Collections.Generic.Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation>[,] DischargeByV; //[oct, static_Pa]

            public Terminal(string path)
            {
                System.IO.StreamReader File = System.IO.File.OpenText(path);
                name = File.ReadLine();

                int bandmin = 2, bandmax = 7;

                while (!File.EndOfStream)
                {
                    string[] line = File.ReadLine().Split(' ');
                    int size;
                    int start;
                    List<double> volume;
                    string l;
                    switch (line[0])
                    {
                        case "dP":
                            PressureSystem = line[1];
                            for (int i = 2; i < line.Length; i++) Static_Pressure.Add(int.Parse(line[i]));
                            break;
                        case "band":
                            bandmin = int.Parse(line[1]);
                            bandmax = int.Parse(line[2]);
                            break;
                        case "Radiated":
                            l = File.ReadLine();
                            List<double>[][] SPLbyVolume;
                            RadiatedByV = new Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation>[8, PressureSystem.Length];
                            SPLbyVolume = new List<double>[PressureSystem.Length][];
                            while (l != "")
                            {
                                size = int.Parse(l);
                                volume = new List<double>();
                                line = File.ReadLine().Split(' ');

                                while (line.Length > 1)
                                {
                                    volume.Add(double.Parse(line[0]));
                                    start = 1;

                                    for (int j = 0; j < PressureSystem.Length; j++) SPLbyVolume[j] = new List<double>[8];

                                    for (int j = 0; j < PressureSystem.Length; j++)
                                    {
                                        int max = start + bandmax - bandmin;
                                        SPLbyVolume[j][0].Add(double.Parse(line[start]));
                                        if (bandmin == 2) SPLbyVolume[j][1].Add(double.Parse(line[start]));
                                        for (int i = start + 1, oct = bandmin + 1; i < max; i++, oct++)
                                        {
                                            SPLbyVolume[i][oct].Add(double.Parse(line[i]));
                                        }
                                        start += bandmax - bandmin;
                                        if (bandmin == 7) SPLbyVolume[j][8].Add(double.Parse(line[start - 1]));
                                    }
                                }
                                for (int j = 0; j < PressureSystem.Length; j++)
                                {
                                    for (int oct = 0; oct < 8; oct++)
                                    {
                                        RadiatedByV[oct, j].Add(size, MathNet.Numerics.Interpolate.CubicSpline(volume.ToArray(), SPLbyVolume[j][oct].ToArray()));
                                    }
                                }
                                //Done reading line
                                l = File.ReadLine();
                            }
                            break;
                        case "Discharge":
                            l = File.ReadLine();
                            List<double>[][] SPLDbyVolume;
                            while (l != "")
                            {
                                DischargeByV = new Dictionary<int, MathNet.Numerics.Interpolation.IInterpolation>[8, PressureSystem.Length];
                                size = int.Parse(l);
                                volume = new List<double>();
                                line = File.ReadLine().Split(' ');
                                SPLDbyVolume = new List<double>[PressureSystem.Length][];

                                while (line.Length > 1)
                                {
                                    volume.Add(double.Parse(line[0]));
                                    start = 1;

                                    for (int j = 0; j < PressureSystem.Length; j++) SPLDbyVolume[j] = new List<double>[8];

                                    for (int j = 0; j < PressureSystem.Length; j++)
                                    {
                                        int max = start + bandmax - bandmin;
                                        SPLDbyVolume[j][0].Add(double.Parse(line[start]));
                                        if (bandmin == 2) SPLDbyVolume[j][1].Add(double.Parse(line[start]));
                                        for (int i = start + 1, oct = bandmin + 1; i < max; i++, oct++)
                                        {
                                            SPLDbyVolume[i][oct].Add(double.Parse(line[i]));
                                        }
                                        start += bandmax - bandmin;
                                        if (bandmin == 7) SPLDbyVolume[j][8].Add(double.Parse(line[start - 1]));
                                    }
                                }
                                for (int j = 0; j < PressureSystem.Length; j++)
                                {
                                    for (int oct = 0; oct < 8; oct++)
                                    {
                                        DischargeByV[oct, j].Add(size, MathNet.Numerics.Interpolate.CubicSpline(volume, SPLDbyVolume[j][oct]));
                                    }
                                }
                                //Done reading line
                                l = File.ReadLine();
                               
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            public void lookup(int size, double volume_CFM, double DownstreamPressure, out double[] Radiated, out double[] Discharge)
            {
                Radiated = new double[8];
                Discharge = new double[8];

                int SP_index = 0;
                for (SP_index = 0; SP_index < Static_Pressure.Count; SP_index++)
                {
                    if (Static_Pressure[SP_index] > DownstreamPressure) break;
                }

                for (int oct = 0; oct < 8; oct++)
                {
                    Radiated[oct] = RadiatedByV[oct, SP_index][size].Interpolate(volume_CFM);
                    Discharge[oct] = DischargeByV[oct, SP_index][size].Interpolate(volume_CFM);
                }

                if (SP_index == 0 || DownstreamPressure > Static_Pressure[SP_index]) return;

                double fract1 = Static_Pressure[SP_index - 1] / (Static_Pressure[SP_index] - Static_Pressure[SP_index - 1]);

                for (int oct = 0; oct < 8; oct++)
                {
                    Radiated[oct] = RadiatedByV[oct, SP_index-1][size].Interpolate(volume_CFM) * fract1 + Radiated[oct] * (1-fract1);
                    Discharge[oct] = DischargeByV[oct, SP_index-1][size].Interpolate(volume_CFM) * fract1 + Discharge[oct] * (1-fract1);
                }
            }
        }
    }
}