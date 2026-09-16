//using Pachyderm_Noise_Control.ASHRAE;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.NetworkInformation;
//using System.Security.Principal;
//using System.Text;
//using System.Threading.Tasks;

//namespace Pachyderm_Noise_Control
//{
//    public class Duct_Model
//    {
//        List<List<node>> Nodes = new List<List<node>>();
//        List<Noise_Source> Noise_Sources = new List<Noise_Source>();
//        public bool Connected = true;
//        public double delta_D = 0.3048; //Meters
//        private static Duct_Model instance = new Duct_Model();


//        private Duct_Model()
//        {
//        } 

//        public static Duct_Model Instance
//        {
//            get
//            {
//                if (instance == null) instance = new Duct_Model();
//                return instance;
//            }
//        }

//        public void reset()
//        {
//            Nodes = new List<List<node>>();
//            Noise_Sources = new List<Noise_Source>();
//        }

//        public int Intiate_Path(Hare.Geometry.Point Origin, double[] Noise, NoiseType type, double width = 0, double height = 0)
//        {
//            Nodes.Add(new List<node>());
//            int path_id = Nodes.Count - 1;
//            Nodes[path_id].Add(new node(new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 }, Origin, Nodes[path_id].Count, width, height));
//            Noise_Sources.Add(new Noise_Source(Noise, Nodes[path_id][Nodes[path_id].Count - 1], type));
//            return path_id;
//        }

//        public int AddComponent(int StartingNode, double[] Total_Attenuation, Hare.Geometry.Point[] Points, double width, double height, ref State S)
//        {
//            double[] atten = new double[8];
//            for (int i = 0; i < 8; i++) atten[i] = Total_Attenuation[i] / Points.Length;
//            Nodes[S.PathId].Add(new node(atten, Points[0], Nodes[S.PathId].Count, Nodes[S.PathId][StartingNode], width, height));

//            for (int i = 1; i < Points.Length; i++)
//            {
//                Nodes[S.PathId].Add(new node(atten, Points[i], Nodes[S.PathId].Count, Nodes[S.PathId][Nodes[S.PathId].Count - 1], width, height));
//            }

//            S.ModelNode = Nodes[S.PathId].Count - 1;

//            return Nodes[S.PathId].Count - 1;
//        }

//        public int AddComponent(int StartingNode, double[] Total_Attenuation, double[] Noise, NoiseType Type, Hare.Geometry.Point[] Points, double width, double height, ref State S)
//        {
//            int id = AddComponent(StartingNode, Total_Attenuation, Points, width, height, ref S);
//            Noise_Sources.Add(new Noise_Source(Noise, Nodes[S.PathId][id], Type));
//            return id;
//        }

//        public int[] AddComponent(int StartingNode, double[][] Total_Attenuation, double[][] Noise, NoiseType Type, Hare.Geometry.Point[][] Points, double width, double height, ref State[] S)
//        {
//            if (Total_Attenuation.Length != Noise.Length || Noise.Length != Points.Length || Points.Length != S.Length) throw new Exception("Number of attenuation, regen and point paths must match.");
//            int[] ids = new int[Total_Attenuation.Length];
//            for (int i = 0; i < Total_Attenuation.Length; i++) ids[i] = AddComponent(StartingNode, Total_Attenuation[i], Noise[i], Type, Points[i], width, height, ref S[i]);
//            return ids;
//        }

//        public void Propagate_Noise_Sources()
//        {
//            foreach (Noise_Source N in Noise_Sources)
//            {
//                N.Propagate_Sound();
//            }
//        }

//        //public void WireDiagram(out List<Hare.Geometry.Point[]> WF,  out List<double[]> SPL, NoiseType type)
//        //{
//        //    WF = new List<Hare.Geometry.Point[]>();
//        //    SPL = new List<double[]>();

//        //    for (int i = 0; i < Nodes.Count; i++)
//        //    {
//        //        foreach (node start in Nodes[i])
//        //        {
//        //            for (int j = 1; j < start.Neighbors.Count; j++)//(node end in start.Neighbors)
//        //            {
//        //                WF.Add(new Hare.Geometry.Point[2] { start.loc, start.Neighbors[j].loc });
//        //                SPL.Add(Sound_Pressure_Level(type, start));
//        //            }
//        //        }
//        //    }
//        //}

//        //public void WireDiagram(out List<Hare.Geometry.Point> DuctCenters, out List<List<Hare.Geometry.Point[]>> DuctWallcollection, out List<double[]> TSPL, out List<double[]> ESPL, out List<double[]> ASPL, out List<double[]> Breakout)
//        //{
//        //    DuctCenters = new List<Hare.Geometry.Point>();
//        //    TSPL = new List<double[]>();
//        //    ESPL = new List<double[]>();
//        //    ASPL = new List<double[]>();
//        //    Breakout = new List<double[]>();
//        //    DuctWallcollection = new List<List<Hare.Geometry.Point[]>>();

//        //    const int numSegmentsRound = 16;   // polygon approximation for round ducts
//        //    const int numLongitudinalRound = 8; // longitudinal lines for round ducts (less visual clutter)
//        //    const double epsilon = 1e-9;

//        //    for (int i = 0; i < Nodes.Count; i++)
//        //    {
//        //        List<node> pathNodes = Nodes[i];
//        //        if (pathNodes.Count < 2) continue;

//        //        // Pre-compute a consistent orthonormal frame per node so that
//        //        // adjacent segments share the same cross-section orientation.
//        //        // We propagate the frame along the path using parallel transport.
//        //        Hare.Geometry.Vector[] tangents = new Hare.Geometry.Vector[pathNodes.Count - 1];
//        //        Hare.Geometry.Vector[] rights = new Hare.Geometry.Vector[pathNodes.Count];
//        //        Hare.Geometry.Vector[] ups = new Hare.Geometry.Vector[pathNodes.Count];

//        //        // Compute tangents for each segment
//        //        for (int j = 0; j < pathNodes.Count - 1; j++)
//        //        {
//        //            tangents[j] = pathNodes[j + 1].loc - pathNodes[j].loc;
//        //            double len2 = Hare.Geometry.Hare_math.Dot(tangents[j], tangents[j]);
//        //            if (len2 < epsilon)
//        //                tangents[j] = new Hare.Geometry.Vector(1, 0, 0); // degenerate fallback
//        //            else
//        //                tangents[j].Normalize();
//        //        }

//        //        // Seed the frame at the first node
//        //        Hare.Geometry.Vector seedUp = new Hare.Geometry.Vector(0, 0, 1);
//        //        if (Math.Abs(Hare.Geometry.Hare_math.Dot(tangents[0], seedUp)) > 0.99)
//        //            seedUp = new Hare.Geometry.Vector(0, 1, 0);

//        //        rights[0] = Hare.Geometry.Hare_math.Cross(tangents[0], seedUp);
//        //        rights[0].Normalize();
//        //        ups[0] = Hare.Geometry.Hare_math.Cross(rights[0], tangents[0]);
//        //        ups[0].Normalize();

//        //        // Parallel-transport the frame along the path
//        //        for (int j = 1; j < pathNodes.Count; j++)
//        //        {
//        //            Hare.Geometry.Vector t0 = tangents[Math.Min(j - 1, tangents.Length - 1)];
//        //            Hare.Geometry.Vector t1 = tangents[Math.Min(j, tangents.Length - 1)];

//        //            // Rotate previous right/up from t0 to t1
//        //            Hare.Geometry.Vector axis = Hare.Geometry.Hare_math.Cross(t0, t1);
//        //            double sinA = Math.Sqrt(Hare.Geometry.Hare_math.Dot(axis, axis));
//        //            double cosA = Hare.Geometry.Hare_math.Dot(t0, t1);

//        //            if (sinA < epsilon)
//        //            {
//        //                // Tangents are (anti-)parallel — keep previous frame
//        //                rights[j] = rights[j - 1];
//        //                ups[j] = ups[j - 1];
//        //            }
//        //            else
//        //            {
//        //                axis.Normalize();
//        //                // Rodrigues' rotation formula
//        //                rights[j] = RotateVector(rights[j - 1], axis, cosA, sinA);
//        //                ups[j] = RotateVector(ups[j - 1], axis, cosA, sinA);
//        //                rights[j].Normalize();
//        //                ups[j].Normalize();
//        //            }
//        //        }

//        //        // Pre-compute profiles at each node (so adjacent segments share the same ring)
//        //        Hare.Geometry.Point[][] profiles = new Hare.Geometry.Point[pathNodes.Count][];
//        //        bool[] isRect = new bool[pathNodes.Count];
//        //        bool[] isCirc = new bool[pathNodes.Count];

//        //        for (int j = 0; j < pathNodes.Count; j++)
//        //        {
//        //            node n = pathNodes[j];
//        //            isRect[j] = n.Width > 0 && n.Height > 0;
//        //            isCirc[j] = (n.Width > 0) ^ (n.Height > 0);

//        //            if (isRect[j])
//        //            {
//        //                double hw = n.Width / 2.0;
//        //                double hh = n.Height / 2.0;
//        //                profiles[j] = GetRectangleCorners(n.loc, rights[j], ups[j], hw, hh);
//        //            }
//        //            else if (isCirc[j])
//        //            {
//        //                double r = (n.Width > 0 ? n.Width : n.Height) / 2.0;
//        //                profiles[j] = GetCirclePoints(n.loc, rights[j], ups[j], r, numSegmentsRound);
//        //            }
//        //            else
//        //            {
//        //                // Node has no cross-section (e.g. origin node) — inherit from next/prev node shape
//        //                profiles[j] = null; // resolved below
//        //            }
//        //        }

//        //        // Resolve null profiles: inherit dimensions from the nearest neighbour that has a shape
//        //        for (int j = 0; j < pathNodes.Count; j++)
//        //        {
//        //            if (profiles[j] != null) continue;

//        //            // Look forward first, then backward
//        //            node donor = null;
//        //            for (int k = j + 1; k < pathNodes.Count; k++) { if (profiles[k] != null) { donor = pathNodes[k]; isRect[j] = isRect[k]; isCirc[j] = isCirc[k]; break; } }
//        //            if (donor == null) for (int k = j - 1; k >= 0; k--) { if (profiles[k] != null) { donor = pathNodes[k]; isRect[j] = isRect[k]; isCirc[j] = isCirc[k]; break; } }

//        //            if (donor != null)
//        //            {
//        //                if (isRect[j])
//        //                {
//        //                    double hw = donor.Width / 2.0;
//        //                    double hh = donor.Height / 2.0;
//        //                    profiles[j] = GetRectangleCorners(pathNodes[j].loc, rights[j], ups[j], hw, hh);
//        //                }
//        //                else if (isCirc[j])
//        //                {
//        //                    double r = (donor.Width > 0 ? donor.Width : donor.Height) / 2.0;
//        //                    profiles[j] = GetCirclePoints(pathNodes[j].loc, rights[j], ups[j], r, numSegmentsRound);
//        //                }
//        //            }
//        //            else
//        //            {
//        //                // Absolute fallback: degenerate point
//        //                profiles[j] = new Hare.Geometry.Point[] { pathNodes[j].loc };
//        //            }
//        //        }

//        //        // Now draw each segment
//        //        for (int j = 1; j < pathNodes.Count; j++)
//        //        {
//        //            node prevNode = pathNodes[j - 1];
//        //            node currentNode = pathNodes[j];

//        //            // Skip zero-length segments
//        //            Hare.Geometry.Vector seg = currentNode.loc - prevNode.loc;
//        //            if (Hare.Geometry.Hare_math.Dot(seg, seg) < epsilon) continue;

//        //            List<Hare.Geometry.Point[]> DuctWalls = new List<Hare.Geometry.Point[]>();

//        //            Hare.Geometry.Point[] startProfile = profiles[j - 1];
//        //            Hare.Geometry.Point[] endProfile = profiles[j];
//        //            bool startRect = isRect[j - 1];
//        //            bool endRect = isRect[j];

//        //            if (startRect && endRect)
//        //            {
//        //                // Rectangular to Rectangular — both profiles have 4 corners
//        //                for (int k = 0; k < 4; k++)
//        //                    DuctWalls.Add(new[] { startProfile[k], endProfile[k] });

//        //                // Start perimeter (only draw for first segment or when size changes)
//        //                if (j == 1)
//        //                    for (int k = 0; k < 4; k++)
//        //                        DuctWalls.Add(new[] { startProfile[k], startProfile[(k + 1) % 4] });

//        //                // End perimeter
//        //                for (int k = 0; k < 4; k++)
//        //                    DuctWalls.Add(new[] { endProfile[k], endProfile[(k + 1) % 4] });
//        //            }
//        //            else if (!startRect && !endRect && startProfile.Length == numSegmentsRound && endProfile.Length == numSegmentsRound)
//        //            {
//        //                // Round to Round
//        //                int step = numSegmentsRound / numLongitudinalRound;
//        //                for (int k = 0; k < numSegmentsRound; k += step)
//        //                    DuctWalls.Add(new[] { startProfile[k], endProfile[k] });

//        //                // Start circumference (only for first segment)
//        //                if (j == 1)
//        //                    for (int k = 0; k < numSegmentsRound; k++)
//        //                        DuctWalls.Add(new[] { startProfile[k], startProfile[(k + 1) % numSegmentsRound] });

//        //                // End circumference
//        //                for (int k = 0; k < numSegmentsRound; k++)
//        //                    DuctWalls.Add(new[] { endProfile[k], endProfile[(k + 1) % numSegmentsRound] });
//        //            }
//        //            else
//        //            {
//        //                // Transition (rect↔round) or mismatched profile sizes
//        //                // Resample both profiles to numSegmentsRound so we can connect them 1-to-1
//        //                Hare.Geometry.Point[] startResampled = ResampleProfile(startProfile, rights[j - 1], ups[j - 1], prevNode.loc, numSegmentsRound);
//        //                Hare.Geometry.Point[] endResampled = ResampleProfile(endProfile, rights[j], ups[j], currentNode.loc, numSegmentsRound);

//        //                int step = numSegmentsRound / numLongitudinalRound;
//        //                for (int k = 0; k < numSegmentsRound; k += step)
//        //                    DuctWalls.Add(new[] { startResampled[k], endResampled[k] });

//        //                if (j == 1)
//        //                    for (int k = 0; k < numSegmentsRound; k++)
//        //                        DuctWalls.Add(new[] { startResampled[k], startResampled[(k + 1) % numSegmentsRound] });

//        //                for (int k = 0; k < numSegmentsRound; k++)
//        //                    DuctWalls.Add(new[] { endResampled[k], endResampled[(k + 1) % numSegmentsRound] });
//        //            }

//        //            TSPL.Add(Sound_Pressure_Level(NoiseType.Total, prevNode));
//        //            ESPL.Add(Sound_Pressure_Level(NoiseType.Equipment, prevNode));
//        //            ASPL.Add(Sound_Pressure_Level(NoiseType.Aerodynamic, prevNode));
//        //            Breakout.Add(SPL_Breakout(prevNode));
//        //            DuctCenters.Add(prevNode.loc + (currentNode.loc - prevNode.loc) * 0.5);
//        //            DuctWallcollection.Add(DuctWalls);
//        //        }
//        //    }
//        //}

//        ///// <summary>
//        ///// Rodrigues' rotation: rotates vector v around unit axis by angle defined by (cosA, sinA).
//        ///// </summary>
//        //private static Hare.Geometry.Vector RotateVector(Hare.Geometry.Vector v, Hare.Geometry.Vector axis, double cosA, double sinA)
//        //{
//        //    // v_rot = v*cos(a) + (axis x v)*sin(a) + axis*(axis·v)*(1-cos(a))
//        //    Hare.Geometry.Vector cross = Hare.Geometry.Hare_math.Cross(axis, v);
//        //    double dot = Hare.Geometry.Hare_math.Dot(axis, v);
//        //    return new Hare.Geometry.Vector(
//        //        v.dx * cosA + cross.dx * sinA + axis.dx * dot * (1 - cosA),
//        //        v.dy * cosA + cross.dy * sinA + axis.dy * dot * (1 - cosA),
//        //        v.dz * cosA + cross.dz * sinA + axis.dz * dot * (1 - cosA));
//        //}

//        ///// <summary>
//        ///// Resamples a profile polygon to the requested number of evenly-spaced points
//        ///// by walking its perimeter. Used to match different cross-section types for transitions.
//        ///// </summary>
//        //private static Hare.Geometry.Point[] ResampleProfile(Hare.Geometry.Point[] profile, Hare.Geometry.Vector right, Hare.Geometry.Vector up, Hare.Geometry.Point center, int targetCount)
//        //{
//        //    if (profile.Length == targetCount) return profile;
//        //    if (profile.Length < 2)
//        //    {
//        //        // Degenerate — return copies
//        //        Hare.Geometry.Point[] result = new Hare.Geometry.Point[targetCount];
//        //        for (int i = 0; i < targetCount; i++) result[i] = profile[0];
//        //        return result;
//        //    }

//        //    // Compute cumulative arc-length around the closed polygon
//        //    int n = profile.Length;
//        //    double[] cumLen = new double[n + 1];
//        //    cumLen[0] = 0;
//        //    for (int k = 0; k < n; k++)
//        //    {
//        //        Hare.Geometry.Vector d = profile[(k + 1) % n] - profile[k];
//        //        cumLen[k + 1] = cumLen[k] + Math.Sqrt(Hare.Geometry.Hare_math.Dot(d, d));
//        //    }
//        //    double totalLen = cumLen[n];

//        //    Hare.Geometry.Point[] resampled = new Hare.Geometry.Point[targetCount];
//        //    for (int i = 0; i < targetCount; i++)
//        //    {
//        //        double targetDist = totalLen * i / targetCount;
//        //        // Find which edge this falls on
//        //        int edge = 0;
//        //        for (int k = 0; k < n; k++) { if (cumLen[k + 1] >= targetDist) { edge = k; break; } }
//        //        double edgeLen = cumLen[edge + 1] - cumLen[edge];
//        //        double t = edgeLen > 0 ? (targetDist - cumLen[edge]) / edgeLen : 0;
//        //        Hare.Geometry.Point a = profile[edge];
//        //        Hare.Geometry.Point b = profile[(edge + 1) % n];
//        //        resampled[i] = a + (b - a) * t;
//        //    }
//        //    return resampled;
//        //}

//        //public void WireDiagram(out List<Hare.Geometry.Point> DuctCenters, out List<List<Hare.Geometry.Point[]>> DuctWallcollection, out List<double[]> TSPL, out List<double[]> ESPL, out List<double[]> ASPL, out List<double[]> Breakout)
//        //{
//        //    DuctCenters = new List<Hare.Geometry.Point>();
//        //    TSPL = new List<double[]>();
//        //    ESPL = new List<double[]>();
//        //    ASPL = new List<double[]>();
//        //    Breakout = new List<double[]>();
//        //    DuctWallcollection = new List<List<Hare.Geometry.Point[]>>();

//        //    const int numSegments = 16; // polygon approximation segments for round ducts and transitions
//        //    const double epsilon = 1e-9;

//        //    for (int i = 0; i < Nodes.Count; i++)
//        //    {
//        //        List<node> pathNodes = Nodes[i];

//        //        for (int j = 1; j < pathNodes.Count; j++)
//        //        {
//        //            List<Hare.Geometry.Point[]> DuctWalls = new List<Hare.Geometry.Point[]>();

//        //            node prevNode = pathNodes[j - 1];
//        //            node currentNode = pathNodes[j];

//        //            // Skip zero-length segments to avoid bad frames
//        //            Hare.Geometry.Vector segmentDirection = (currentNode.loc - prevNode.loc);
//        //            double segLen2 = Hare.Geometry.Hare_math.Dot(segmentDirection, segmentDirection);
//        //            if (segLen2 < epsilon) continue;

//        //            // Build orthonormal frame (right, up) perpendicular to segmentDirection
//        //            segmentDirection.Normalize();
//        //            Hare.Geometry.Vector up = new Hare.Geometry.Vector(0, 0, 1);
//        //            if (Math.Abs(Hare.Geometry.Hare_math.Dot(segmentDirection, up)) > 0.99)
//        //                up = new Hare.Geometry.Vector(1, 0, 0);

//        //            Hare.Geometry.Vector right = Hare.Geometry.Hare_math.Cross(segmentDirection, up);
//        //            up = Hare.Geometry.Hare_math.Cross(right, segmentDirection);
//        //            right.Normalize();
//        //            up.Normalize();

//        //            bool prevRect = prevNode.Width > 0 && prevNode.Height > 0;
//        //            bool currRect = currentNode.Width > 0 && currentNode.Height > 0;
//        //            // Circular if exactly one dimension is specified (>0) and the other is 0
//        //            bool prevCirc = (prevNode.Width > 0) ^ (prevNode.Height > 0);
//        //            bool currCirc = (currentNode.Width > 0) ^ (currentNode.Height > 0);

//        //            // Same cross-section type on both ends
//        //            if (prevRect && currRect)
//        //            {
//        //                // Rectangular-to-Rectangular (allow different sizes)
//        //                double prevHalfWidth = prevNode.Width / 2.0;
//        //                double prevHalfHeight = prevNode.Height / 2.0;
//        //                double currHalfWidth = currentNode.Width / 2.0;
//        //                double currHalfHeight = currentNode.Height / 2.0;

//        //                Hare.Geometry.Point[] startCorners = GetRectangleCorners(prevNode.loc, right, up, prevHalfWidth, prevHalfHeight);
//        //                Hare.Geometry.Point[] endCorners = GetRectangleCorners(currentNode.loc, right, up, currHalfWidth, currHalfHeight);

//        //                // Connect corresponding edges
//        //                for (int k = 0; k < 4; k++)
//        //                    DuctWalls.Add(new[] { startCorners[k], endCorners[k] });

//        //                // Perimeters at start and end
//        //                for (int k = 0; k < 4; k++)
//        //                {
//        //                    DuctWalls.Add(new[] { startCorners[k], startCorners[(k + 1) % 4] });
//        //                    DuctWalls.Add(new[] { endCorners[k], endCorners[(k + 1) % 4] });
//        //                }
//        //            }
//        //            else if (prevCirc && currCirc)
//        //            {
//        //                // Round-to-Round (allow different diameters)
//        //                double prevRadius = (prevNode.Width > 0 ? prevNode.Width : prevNode.Height) / 2.0;
//        //                double currRadius = (currentNode.Width > 0 ? currentNode.Width : currentNode.Height) / 2.0;

//        //                Hare.Geometry.Point[] startCircle = GetCirclePoints(prevNode.loc, right, up, prevRadius, numSegments);
//        //                Hare.Geometry.Point[] endCircle = GetCirclePoints(currentNode.loc, right, up, currRadius, numSegments);

//        //                for (int k = 0; k < numSegments; k++)
//        //                    DuctWalls.Add(new[] { startCircle[k], endCircle[k] });

//        //                for (int k = 0; k < numSegments; k++)
//        //                {
//        //                    DuctWalls.Add(new[] { startCircle[k], startCircle[(k + 1) % numSegments] });
//        //                    DuctWalls.Add(new[] { endCircle[k], endCircle[(k + 1) % numSegments] });
//        //                }
//        //            }
//        //            else if ((prevRect && currCirc) || (prevCirc && currRect))
//        //            {
//        //                // Transition between Rectangular and Round
//        //                Hare.Geometry.Point[][] profiles = new Hare.Geometry.Point[2][];

//        //                // Helper: generate polygonal profile for a node
//        //                Func<node, Hare.Geometry.Point[]> buildProfile = n =>
//        //                {
//        //                    if (n.Width > 0 && n.Height > 0)
//        //                    {
//        //                        // Rectangular profile as polygon around perimeter
//        //                        double hw = n.Width / 2.0;
//        //                        double hh = n.Height / 2.0;
//        //                        Hare.Geometry.Point[] corners = GetRectangleCorners(n.loc, right, up, hw, hh);
//        //                        Hare.Geometry.Point[] poly = new Hare.Geometry.Point[numSegments];

//        //                        for (int h = 0; h < numSegments; h++)
//        //                        {
//        //                            double t = (double)h / numSegments;
//        //                            int edge = (int)(t * 4.0);
//        //                            double edgeT = (t * 4.0) - edge;

//        //                            Hare.Geometry.Point a = corners[edge % 4];
//        //                            Hare.Geometry.Point b = corners[(edge + 1) % 4];
//        //                            poly[h] = a + (b - a) * edgeT;
//        //                        }
//        //                        return poly;
//        //                    }
//        //                    else if ((n.Width > 0) ^ (n.Height > 0))
//        //                    {
//        //                        double r = (n.Width > 0 ? n.Width : n.Height) / 2.0;
//        //                        return GetCirclePoints(n.loc, right, up, r, numSegments);
//        //                    }
//        //                    else
//        //                    {
//        //                        // Unknown cross-section: fallback to a degenerate "point" profile
//        //                        Hare.Geometry.Point[] poly = new Hare.Geometry.Point[numSegments];
//        //                        for (int h = 0; h < numSegments; h++) poly[h] = n.loc;
//        //                        return poly;
//        //                    }
//        //                };

//        //                profiles[0] = buildProfile(prevNode);
//        //                profiles[1] = buildProfile(currentNode);

//        //                // Connect corresponding profile points
//        //                for (int k = 0; k < numSegments; k++)
//        //                    DuctWalls.Add(new[] { profiles[0][k], profiles[1][k] });

//        //                // Perimeters at start and end
//        //                for (int k = 0; k < numSegments; k++)
//        //                {
//        //                    DuctWalls.Add(new[] { profiles[0][k], profiles[0][(k + 1) % numSegments] });
//        //                    DuctWalls.Add(new[] { profiles[1][k], profiles[1][(k + 1) % numSegments] });
//        //                }
//        //            }
//        //            else
//        //            {
//        //                // Fallback: unknown/unsupported shape state, draw centerline so the segment is at least visible
//        //                DuctWalls.Add(new[] { prevNode.loc, currentNode.loc });
//        //            }

//        //            // Accumulate acoustic data for the segment (use start node)
//        //            TSPL.Add(Sound_Pressure_Level(NoiseType.Total, prevNode));
//        //            ESPL.Add(Sound_Pressure_Level(NoiseType.Equipment, prevNode));c) * 0.5);
//        //            DuctWallcollection.Add(DuctWalls);
//        //        }
//        //    }
//        //}
//        public void WireDiagram(out List<Hare.Geometry.Point> DuctCenters, out List<List<Hare.Geometry.Point[]>> DuctWallcollection, out List<double[]> TSPL, out List<double[]> ESPL, out List<double[]> ASPL, out List<double[]> Breakout)
//        {
//            List<bool> Transition;
//            WireDiagram(out DuctCenters, out DuctWallcollection, out TSPL, out ESPL, out ASPL, out Breakout, out Transition);
//        }

//        public void WireDiagram(out List<Hare.Geometry.Point> DuctCenters, out List<List<Hare.Geometry.Point[]>> DuctWallcollection, out List<double[]> TSPL, out List<double[]> ESPL, out List<double[]> ASPL, out List<double[]> Breakout, out List<bool> Transition)
//        {
//            DuctCenters = new List<Hare.Geometry.Point>();
//            TSPL = new List<double[]>();
//            ESPL = new List<double[]>();
//            ASPL = new List<double[]>();
//            Breakout = new List<double[]>();
//            DuctWallcollection = new List<List<Hare.Geometry.Point[]>>();
//            Transition = new List<bool>();

//            //segments in circle
//            int numSegments = 16;
//            const double epsilon = 1e-9;
//            const double inch = 0.0254;

//            for (int i = 0; i < Nodes.Count; i++)
//            {
//                List<node> pathNodes = Nodes[i];

//                for (int j = 0; j < pathNodes.Count; j++)
//                {
//                    node prevNode = pathNodes[j];

//                    foreach (node currentNode in prevNode.Neighbors)
//                    {
//                        //Connections are reciprocal. Draw each actual model edge once.
//                        if (currentNode.node_id <= prevNode.node_id) continue;

//                        List<Hare.Geometry.Point[]> DuctWalls = new List<Hare.Geometry.Point[]>();

//                        Hare.Geometry.Vector segmentDirection = (currentNode.loc - prevNode.loc);
//                        double segLen2 = Hare.Geometry.Hare_math.Dot(segmentDirection, segmentDirection);

//                        //Coincident nodes occur at junctions. They are a bookkeeping connection,
//                        //not a duct piece to draw.
//                        if (segLen2 < epsilon) continue;

//                        segmentDirection.Normalize();

//                        Hare.Geometry.Vector right, up;
//                        up = new Hare.Geometry.Vector(0, 0, 1);

//                        if (Math.Abs(Hare.Geometry.Hare_math.Dot(segmentDirection, up)) > 0.99)
//                        {
//                            up = new Hare.Geometry.Vector(1, 0, 0);
//                        }

//                        right = Hare.Geometry.Hare_math.Cross(segmentDirection, up);
//                        up = Hare.Geometry.Hare_math.Cross(right, segmentDirection);
//                        right.Normalize();
//                        up.Normalize();

//                        bool prevIsJunction = prevNode.Neighbors.Count > 2;
//                        bool currentIsJunction = currentNode.Neighbors.Count > 2;

//                        for (int k = 0; k < prevNode.Neighbors.Count; k++)
//                        {
//                            Hare.Geometry.Vector v = prevNode.Neighbors[k].loc - prevNode.loc;
//                            if (Hare.Geometry.Hare_math.Dot(v, v) < epsilon) prevIsJunction = true;
//                        }

//                        for (int k = 0; k < currentNode.Neighbors.Count; k++)
//                        {
//                            Hare.Geometry.Vector v = currentNode.Neighbors[k].loc - currentNode.loc;
//                            if (Hare.Geometry.Hare_math.Dot(v, v) < epsilon) currentIsJunction = true;
//                        }

//                        double prevWidth = prevNode.Width * inch;
//                        double prevHeight = prevNode.Height * inch;
//                        double currentWidth = currentNode.Width * inch;
//                        double currentHeight = currentNode.Height * inch;

//                        //At junctions, do not interpolate from the trunk size to the branch size.
//                        //The connected duct member should carry its own section into the junction.
//                        if (prevIsJunction && (currentWidth > 0 || currentHeight > 0))
//                        {
//                            prevWidth = currentWidth;
//                            prevHeight = currentHeight;
//                        }

//                        if (currentIsJunction && (prevWidth > 0 || prevHeight > 0))
//                        {
//                            currentWidth = prevWidth;
//                            currentHeight = prevHeight;
//                        }

//                        //Equipment/source nodes may have no size. Let the adjoining duct set the size
//                        //for that display segment, but only for display.
//                        if (prevWidth == 0 && prevHeight == 0 && (currentWidth > 0 || currentHeight > 0))
//                        {
//                            prevWidth = currentWidth;
//                            prevHeight = currentHeight;
//                        }

//                        if (currentWidth == 0 && currentHeight == 0 && (prevWidth > 0 || prevHeight > 0))
//                        {
//                            currentWidth = prevWidth;
//                            currentHeight = prevHeight;
//                        }

//                        bool prevIsRectangular = prevWidth > 0 && prevHeight > 0;
//                        bool currentIsRectangular = currentWidth > 0 && currentHeight > 0;
//                        bool prevIsCircular = (prevWidth > 0 && prevHeight == 0) || (prevWidth == 0 && prevHeight > 0);
//                        bool currentIsCircular = (currentWidth > 0 && currentHeight == 0) || (currentWidth == 0 && currentHeight > 0);

//                        bool drawStart = !prevIsJunction;
//                        bool drawEnd = !currentIsJunction;
//                        bool isTransition = false;

//                        if ((prevIsRectangular && currentIsRectangular) || (prevIsCircular && currentIsCircular))
//                        {
//                            //Same cross-section type.
//                            if (prevIsRectangular)
//                            {
//                                double prevHalfWidth = prevWidth / 2;
//                                double prevHalfHeight = prevHeight / 2;
//                                double currentHalfWidth = currentWidth / 2;
//                                double currentHalfHeight = currentHeight / 2;

//                                if (Math.Abs(prevWidth - currentWidth) > epsilon || Math.Abs(prevHeight - currentHeight) > epsilon) isTransition = true;

//                                Hare.Geometry.Point[] startCorners = GetRectangleCorners(prevNode.loc, right, up, prevHalfWidth, prevHalfHeight);
//                                Hare.Geometry.Point[] endCorners = GetRectangleCorners(currentNode.loc, right, up, currentHalfWidth, currentHalfHeight);

//                                for (int k = 0; k < 4; k++)
//                                {
//                                    DuctWalls.Add(new Hare.Geometry.Point[] { startCorners[k], endCorners[k] });
//                                }

//                                if (drawStart)
//                                {
//                                    for (int k = 0; k < 4; k++)
//                                    {
//                                        DuctWalls.Add(new Hare.Geometry.Point[] { startCorners[k], startCorners[(k + 1) % 4] });
//                                    }
//                                }

//                                if (drawEnd)
//                                {
//                                    for (int k = 0; k < 4; k++)
//                                    {
//                                        DuctWalls.Add(new Hare.Geometry.Point[] { endCorners[k], endCorners[(k + 1) % 4] });
//                                    }
//                                }
//                            }
//                            else if (prevIsCircular)
//                            {
//                                double prevRadius = (prevWidth > 0) ? prevWidth / 2 : prevHeight / 2;
//                                double currentRadius = (currentWidth > 0) ? currentWidth / 2 : currentHeight / 2;

//                                if (Math.Abs(prevRadius - currentRadius) > epsilon) isTransition = true;

//                                Hare.Geometry.Point[] startCircle = GetCirclePoints(prevNode.loc, right, up, prevRadius, numSegments);
//                                Hare.Geometry.Point[] endCircle = GetCirclePoints(currentNode.loc, right, up, currentRadius, numSegments);

//                                for (int k = 0; k < numSegments; k++)
//                                {
//                                    DuctWalls.Add(new Hare.Geometry.Point[] { startCircle[k], endCircle[k] });
//                                }

//                                if (drawStart)
//                                {
//                                    for (int k = 0; k < numSegments; k++)
//                                    {
//                                        DuctWalls.Add(new Hare.Geometry.Point[] { startCircle[k], startCircle[(k + 1) % numSegments] });
//                                    }
//                                }

//                                if (drawEnd)
//                                {
//                                    for (int k = 0; k < numSegments; k++)
//                                    {
//                                        DuctWalls.Add(new Hare.Geometry.Point[] { endCircle[k], endCircle[(k + 1) % numSegments] });
//                                    }
//                                }
//                            }
//                        }
//                        else if ((prevIsRectangular && currentIsCircular) || (prevIsCircular && currentIsRectangular))
//                        {
//                            //Transition between different cross-sections. This should happen at a real transition,
//                            //not merely because a branch is attached to a larger trunk.
//                            isTransition = true;

//                            node[] nodes = new node[2] { prevNode, currentNode };
//                            double[] widths = new double[2] { prevWidth, currentWidth };
//                            double[] heights = new double[2] { prevHeight, currentHeight };
//                            Hare.Geometry.Point[][] Profiles = new Hare.Geometry.Point[2][];

//                            for (int k = 0; k < 2; k++)
//                            {
//                                if (widths[k] > 0 && heights[k] > 0)
//                                {
//                                    double halfWidth = widths[k] / 2;
//                                    double halfHeight = heights[k] / 2;
//                                    Profiles[k] = new Hare.Geometry.Point[numSegments];

//                                    Hare.Geometry.Point[] corners = GetRectangleCorners(nodes[k].loc, right, up, halfWidth, halfHeight);

//                                    for (int h = 0; h < numSegments; h++)
//                                    {
//                                        double t = (double)h / numSegments;
//                                        int edge = (int)(t * 4);
//                                        double edgeT = (t * 4) - edge;

//                                        Hare.Geometry.Point start = corners[edge % 4];
//                                        Hare.Geometry.Point end = corners[(edge + 1) % 4];
//                                        Profiles[k][h] = start + (end - start) * edgeT;
//                                    }
//                                }
//                                else if ((widths[k] > 0 && heights[k] == 0) || (widths[k] == 0 && heights[k] > 0))
//                                {
//                                    double radius = (widths[k] > 0) ? widths[k] / 2 : heights[k] / 2;
//                                    Profiles[k] = GetCirclePoints(nodes[k].loc, right, up, radius, numSegments);
//                                }
//                                else
//                                {
//                                    Profiles[k] = new Hare.Geometry.Point[numSegments];
//                                    for (int h = 0; h < numSegments; h++) Profiles[k][h] = nodes[k].loc;
//                                }
//                            }

//                            for (int k = 0; k < numSegments; k++)
//                            {
//                                DuctWalls.Add(new Hare.Geometry.Point[] { Profiles[0][k], Profiles[1][k] });
//                            }

//                            if (drawStart)
//                            {
//                                for (int k = 0; k < numSegments; k++)
//                                {
//                                    DuctWalls.Add(new Hare.Geometry.Point[] { Profiles[0][k], Profiles[0][(k + 1) % numSegments] });
//                                }
//                            }

//                            if (drawEnd)
//                            {
//                                for (int k = 0; k < numSegments; k++)
//                                {
//                                    DuctWalls.Add(new Hare.Geometry.Point[] { Profiles[1][k], Profiles[1][(k + 1) % numSegments] });
//                                }
//                            }
//                        }
//                        else
//                        {
//                            //Last-resort centerline, so the display remains usable even if a node has incomplete dimensions.
//                            DuctWalls.Add(new Hare.Geometry.Point[] { prevNode.loc, currentNode.loc });
//                        }

//                        TSPL.Add(Sound_Pressure_Level(NoiseType.Total, currentNode));
//                        ESPL.Add(Sound_Pressure_Level(NoiseType.Equipment, currentNode));
//                        ASPL.Add(Sound_Pressure_Level(NoiseType.Aerodynamic, currentNode));
//                        Breakout.Add(SPL_Breakout(currentNode));
//                        DuctCenters.Add(prevNode.loc + (currentNode.loc - prevNode.loc) * 0.5);
//                        DuctWallcollection.Add(DuctWalls);
//                        Transition.Add(isTransition);
//                    }
//                }
//            }
//        }

//        private static Hare.Geometry.Vector RotateVector(Hare.Geometry.Vector v, Hare.Geometry.Vector axis, double cosA, double sinA)
//        {
//            Hare.Geometry.Vector cross = Hare.Geometry.Hare_math.Cross(axis, v);
//            double dot = Hare.Geometry.Hare_math.Dot(axis, v);
//            return new Hare.Geometry.Vector(
//                v.dx * cosA + cross.dx * sinA + axis.dx * dot * (1 - cosA),
//                v.dy * cosA + cross.dy * sinA + axis.dy * dot * (1 - cosA),
//                v.dz * cosA + cross.dz * sinA + axis.dz * dot * (1 - cosA));
//        }

//        private static void AddSegmentWalls(List<Hare.Geometry.Point[]> ductWalls, Hare.Geometry.Point[] startProfile, Hare.Geometry.Point[] endProfile, bool isRect, int numSegments, bool drawStart, bool drawEnd)
//        {
//            if (isRect)
//            {
//                for (int k = 0; k < 4; k++)
//                {
//                    ductWalls.Add(new[] { startProfile[k], endProfile[k] });
//                }

//                if (drawStart)
//                {
//                    for (int k = 0; k < 4; k++)
//                    {
//                        ductWalls.Add(new[] { startProfile[k], startProfile[(k + 1) % 4] });
//                    }
//                }

//                if (drawEnd)
//                {
//                    for (int k = 0; k < 4; k++)
//                    {
//                        ductWalls.Add(new[] { endProfile[k], endProfile[(k + 1) % 4] });
//                    }
//                }
//            }
//            else
//            {
//                for (int k = 0; k < numSegments; k++)
//                {
//                    ductWalls.Add(new[] { startProfile[k], endProfile[k] });
//                }

//                if (drawStart)
//                {
//                    for (int k = 0; k < numSegments; k++)
//                    {
//                        ductWalls.Add(new[] { startProfile[k], startProfile[(k + 1) % numSegments] });
//                    }
//                }

//                if (drawEnd)
//                {
//                    for (int k = 0; k < numSegments; k++)
//                    {
//                        ductWalls.Add(new[] { endProfile[k], endProfile[(k + 1) % numSegments] });
//                    }
//                }
//            }
//        }


//        //public void WireDiagram(out List<Hare.Geometry.Point> DuctCenters, out List<List<Hare.Geometry.Point[]>> DuctWallcollection, out List<double[]> TSPL, out List<double[]> ESPL, out List<double[]> ASPL, out List<double[]> Breakout)
//        //{
//        //    DuctCenters = new List<Hare.Geometry.Point>();
//        //    TSPL = new List<double[]>();
//        //    ESPL = new List<double[]>();
//        //    ASPL = new List<double[]>();
//        //    Breakout = new List<double[]>();
//        //    DuctWallcollection = new List<List<Hare.Geometry.Point[]>>();

//        //    for (int i = 0; i < Nodes.Count; i++)
//        //    {
//        //        List<node> pathNodes = Nodes[i];

//        //        for (int j = 1; j < pathNodes.Count; j++)
//        //        {
//        //            List<Hare.Geometry.Point[]> DuctWalls = new List<Hare.Geometry.Point[]>();

//        //            node prevNode = pathNodes[j - 1];
//        //            node currentNode = pathNodes[j];

//        //            // Calculate the segment direction
//        //            Hare.Geometry.Vector segmentDirection = (currentNode.loc - prevNode.loc);
//        //            segmentDirection.Normalize();
//        //            Hare.Geometry.Vector right, up;
//        //            up = new Hare.Geometry.Vector(0, 0, 1);

//        //            if (Math.Abs(Hare.Geometry.Hare_math.Dot(segmentDirection, up)) > 0.99)
//        //            {
//        //                up = new Hare.Geometry.Vector(1, 0, 0);
//        //            }
//        //            right = Hare.Geometry.Hare_math.Cross(segmentDirection, up);
//        //            up = Hare.Geometry.Hare_math.Cross(right, segmentDirection);
//        //            right.Normalize();
//        //            up.Normalize();

//        //            bool prevIsRectangular = prevNode.Width * 0.0254 > 0 && prevNode.Height * 0.0254 > 0;
//        //            bool currentIsRectangular = currentNode.Width * 0.0254 > 0 && currentNode.Height * 0.0254 > 0;
//        //            bool prevIsCircular = (prevNode.Width * 0.0254 > 0 && prevNode.Height * 0.0254 == 0) || (prevNode.Width * 0.0254 == 0 && prevNode.Height * 0.0254 > 0);
//        //            bool currentIsCircular = (currentNode.Width * 0.0254 > 0 && currentNode.Height * 0.0254 == 0) || (currentNode.Width * 0.0254 == 0 && currentNode.Height * 0.0254 > 0);

//        //            // Draw the duct segment
//        //            if ((prevIsRectangular && currentIsRectangular) || (prevIsCircular && currentIsCircular))
//        //            {
//        //                // Same cross-section type
//        //                if (prevIsRectangular)
//        //                {
//        //                    // Rectangular duct
//        //                    double prevHalfWidth = prevNode.Width * 0.0254 / 2;
//        //                    double prevHalfHeight = prevNode.Height * 0.0254 / 2;
//        //                    double currentHalfWidth = currentNode.Width * 0.0254 / 2;
//        //                    double currentHalfHeight = currentNode.Height * 0.0254 / 2;

//        //                    // Calculate corners at start and end nodes
//        //                    Hare.Geometry.Point[] startCorners = GetRectangleCorners(prevNode.loc, right, up, prevHalfWidth, prevHalfHeight);
//        //                    Hare.Geometry.Point[] endCorners = GetRectangleCorners(currentNode.loc, right, up, currentHalfWidth, currentHalfHeight);

//        //                    // Add lines between corresponding corners
//        //                    for (int k = 0; k < 4; k++)
//        //                    {
//        //                        DuctWalls.Add(new Hare.Geometry.Point[] { startCorners[k], endCorners[k] });
//        //                    }

//        //                    // Add perimeter lines at start and end
//        //                    for (int k = 0; k < 4; k++)
//        //                    {
//        //                        DuctWalls.Add(new Hare.Geometry.Point[] { startCorners[k], startCorners[(k + 1) % 4] });
//        //                        DuctWalls.Add(new Hare.Geometry.Point[] { endCorners[k], endCorners[(k + 1) % 4] });
//        //                    }
//        //                }
//        //                else if (prevIsCircular)
//        //                {
//        //                    // Circular duct
//        //                    double prevRadius = (prevNode.Width * 0.0254 > 0) ? prevNode.Width * 0.0254 / 2 : prevNode.Height * 0.0254 / 2;
//        //                    double currentRadius = (currentNode.Width * 0.0254 > 0) ? currentNode.Width * 0.0254 / 2 : currentNode.Height * 0.0254 / 2;

//        //                    int numSegments = 16; // Approximation segments for the circle
//        //                    Hare.Geometry.Point[] startCircle = GetCirclePoints(prevNode.loc, right, up, prevRadius, numSegments);
//        //                    Hare.Geometry.Point[] endCircle = GetCirclePoints(currentNode.loc, right, up, currentRadius, numSegments);

//        //                    // Add lines between corresponding points
//        //                    for (int k = 0; k < numSegments; k++)
//        //                    {
//        //                        DuctWalls.Add(new Hare.Geometry.Point[] { startCircle[k], endCircle[k] });
//        //                    }

//        //                    // Add circumference lines at start and end
//        //                    for (int k = 0; k < numSegments; k++)
//        //                    {
//        //                        DuctWalls.Add(new Hare.Geometry.Point[] { startCircle[k], startCircle[(k + 1) % numSegments] });
//        //                        DuctWalls.Add(new Hare.Geometry.Point[] { endCircle[k], endCircle[(k + 1) % numSegments] });
//        //                    }
//        //                }
//        //            }
//        //            else if ((prevIsRectangular && currentIsCircular) || (prevIsCircular && currentIsRectangular))
//        //            {
//        //                // Transition between different cross-sections
//        //                int numSegments = 16;

//        //                node[] nodes = new node[2] { prevNode, currentNode };
//        //                Hare.Geometry.Point[][] Profiles = new Hare.Geometry.Point[2][];

//        //                for (int k = 0; k < 2; k++)
//        //                {
//        //                    if (nodes[k].Width * 0.0254 > 0 && nodes[k].Height * 0.0254 > 0)
//        //                    {
//        //                        // Rectangular profile
//        //                        double halfWidth = nodes[k].Width * 0.0254 / 2;
//        //                        double halfHeight = nodes[k].Height * 0.0254 / 2;
//        //                        Profiles[k] = new Hare.Geometry.Point[numSegments];

//        //                        // Define the four corners of the rectangle
//        //                        Hare.Geometry.Point[] corners = GetRectangleCorners(nodes[k].loc, right, up, halfWidth, halfHeight);

//        //                        // Approximate the rectangle with a polygon
//        //                        for (int h = 0; h < numSegments; h++)
//        //                        {
//        //                            double t = (double)h / numSegments;
//        //                            int edge = (int)(t * 4);
//        //                            double edgeT = (t * 4) - edge;

//        //                            Hare.Geometry.Point start = corners[edge % 4];
//        //                            Hare.Geometry.Point end = corners[(edge + 1) % 4];
//        //                            Profiles[k][h] = start + (end - start) * edgeT;
//        //                        }
//        //                    }
//        //                    else if ((nodes[k].Width * 0.0254 > 0 && nodes[k].Height * 0.0254 == 0) || (nodes[k].Width * 0.0254 == 0 && nodes[k].Height * 0.0254 > 0))
//        //                    {
//        //                        // Circular profile
//        //                        double radius = (nodes[k].Width > 0) ? nodes[k].Width * 0.0254 / 2 : nodes[k].Height * 0.0254 / 2;
//        //                        Profiles[k] = GetCirclePoints(nodes[k].loc, right, up, radius, numSegments);
//        //                    }
//        //                    else
//        //                    {
//        //                        // Invalid configuration - create empty profile
//        //                        Profiles[k] = new Hare.Geometry.Point[numSegments];
//        //                        for (int h = 0; h < numSegments; h++)
//        //                        {
//        //                            Profiles[k][h] = nodes[k].loc;
//        //                        }
//        //                    }
//        //                }

//        //                // Add a taper segment in between if needed
//        //                double segLen = Math.Sqrt(Hare.Geometry.Hare_math.Dot(currentNode.loc - prevNode.loc, currentNode.loc - prevNode.loc));
//        //                const double maxTransitionAngleRad = 15.0 * Math.PI / 180.0;
//        //                double maxTransitionTan = Math.Tan(maxTransitionAngleRad);

//        //                // Circular if exactly one dimension is specified (>0) and the other is 0
//        //                double w1 = prevNode.Width * 0.0254;
//        //                double h1 = prevNode.Height * 0.0254;
//        //                double w2 = currentNode.Width * 0.0254;
//        //                double h2 = currentNode.Height * 0.0254;
//        //                double d1 = (prevNode.Width * 0.0254 > 0) ? prevNode.Width * 0.0254 : prevNode.Height * 0.0254;
//        //                double d2 = (currentNode.Width * 0.0254 > 0) ? currentNode.Width * 0.0254 : currentNode.Height * 0.0254;

//        //                bool needsTaper = false;
//        //                if (prevIsRectangular && !currentIsRectangular)
//        //                {
//        //                    // Rectangular to Circular transition
//        //                    double requiredLength = Math.Abs(d2 - Math.Min(w1, h1)) / 2 / maxTransitionTan;
//        //                    needsTaper = segLen > requiredLength;
//        //                }
//        //                else if (!prevIsRectangular && currentIsRectangular)
//        //                {
//        //                    // Circular to Rectangular transition
//        //                    double requiredLength = Math.Abs(w2 - d1) / 2 / maxTransitionTan;
//        //                    needsTaper = segLen > requiredLength;
//        //                }

//        //                if (needsTaper)
//        //                {
//        //                    // Add a segment in the middle with interpolated shape
//        //                    int numTaperSegments = 4;
//        //                    for (int t = 1; t <= numTaperSegments; t++)
//        //                    {
//        //                        double tFactor = (double)t / numTaperSegments;
//        //                        Hare.Geometry.Point[] interpolatedProfile = new Hare.Geometry.Point[numSegments];

//        //                        for (int k = 0; k < numSegments; k++)
//        //                        {
//        //                            Hare.Geometry.Point p1 = profiles[0][k];
//        //                            Hare.Geometry.Point p2 = profiles[1][k];
//        //                            interpolatedProfile[k] = p1 + (p2 - p1) * tFactor;
//        //                        }

//        //                        if (t == 1)
//        //                            AddSegmentWalls(DuctWalls, profiles[0], interpolatedProfile, prevIsRectangular, numSegments, false, false);
//        //                        else if (t == numTaperSegments)
//        //                            AddSegmentWalls(DuctWalls, interpolatedProfile, profiles[1], prevIsRectangular, numSegments, false, false);
//        //                        else
//        //                            AddSegmentWalls(DuctWalls, interpolatedProfile, interpolatedProfile, prevIsRectangular, numSegments, false, false);
//        //                    }
//        //                }

//        //                // Connect the profiles directly
//        //                for (int k = 0; k < numSegments; k++)
//        //                {
//        //                    DuctWalls.Add(new[] { profiles[0][k], profiles[1][k] });
//        //                }

//        //                // Add perimeter lines at start and end profiles
//        //                for (int k = 0; k < numSegments; k++)
//        //                {
//        //                    DuctWalls.Add(new[] { profiles[0][k], profiles[0][(k + 1) % numSegments] });
//        //                    DuctWalls.Add(new[] { profiles[1][k], profiles[1][(k + 1) % numSegments] });
//        //                }
//        //            }

//        //            TSPL.Add(Sound_Pressure_Level(NoiseType.Total, prevNode));
//        //            ESPL.Add(Sound_Pressure_Level(NoiseType.Equipment, prevNode));
//        //            ASPL.Add(Sound_Pressure_Level(NoiseType.Aerodynamic, prevNode));
//        //            Breakout.Add(SPL_Breakout(prevNode));
//        //            DuctCenters.Add(prevNode.loc);
//        //            DuctWallcollection.Add(DuctWalls);
//        //        }
//        //    }
//        //}

//        private Hare.Geometry.Point[] GetRectangleCorners(Hare.Geometry.Point center, Hare.Geometry.Vector right, Hare.Geometry.Vector up, double halfWidth, double halfHeight)
//        {
//            return new Hare.Geometry.Point[]
//            {
//                center + (right * halfWidth) + (up * halfHeight),
//                center + (right * halfWidth) - (up * halfHeight),
//                center - (right * halfWidth) - (up * halfHeight),
//                center - (right * halfWidth) + (up * halfHeight)
//            };
//        }

//        private Hare.Geometry.Point[] GetCirclePoints(Hare.Geometry.Point center, Hare.Geometry.Vector right, Hare.Geometry.Vector up, double radius, int numSegments)
//        {
//            Hare.Geometry.Point[] points = new Hare.Geometry.Point[numSegments];

//            for (int i = 0; i < numSegments; i++)
//            {
//                double angle = (2 * Math.PI * i) / numSegments;
//                Hare.Geometry.Vector offset = (right * Math.Cos(angle) + up * Math.Sin(angle)) * radius;
//                points[i] = center + offset;
//            }

//            return points;
//        }

//        public double[] Sound_Pressure_Level(NoiseType type, int PathId, int Node_index)
//        {
//            //Upon display, sometimes resets.
//            return Sound_Pressure_Level(type, Nodes[PathId][Node_index]);
//        }

//        private double[] Sound_Pressure_Level(NoiseType type, node N)
//        {
//            double[] spl = new double[8];
//            switch (type)
//            {
//                case NoiseType.Equipment:
//                    spl = N.SPL_Equipment;
//                    break;
//                case NoiseType.Aerodynamic:
//                    spl = N.SPL_Aerodynamic;
//                    break;
//                default:
//                    for (int oct = 0; oct < 8; oct++) spl[oct] = 10 * Math.Log10(Math.Pow(10f, N.SPL_Aerodynamic[oct] / 10) + Math.Pow(10f, N.SPL_Equipment[oct] / 10));
//                    break;
//            }
//            return spl;
//        }

//        private double[] SPL_Breakout(node N)
//        {
//            double[] DuctBreakout = new double[8];
//            for (int oct = 0; oct < 8; oct++) DuctBreakout[oct] = 10 * Math.Log10(Math.Pow(10f, N.SPL_Aerodynamic[oct] / 10) + Math.Pow(10f, N.SPL_Equipment[oct] / 10)) - N.DuctTL[oct] - N.DuctLagging[oct];
//            return DuctBreakout;
//        }

//        public Hare.Geometry.Point Location(int nodeindex, int Pathid)
//        {
//            return Nodes[Pathid][nodeindex].loc;
//        }

//        public void Add_Attenuation(int node_id, int path_id, double[] Atten)
//        {
//            double[] atten = new double[8];
//            for (int i = 0; i < 8; i++) atten[i] = Nodes[path_id][node_id].Atten[i] + Math.Abs(Atten[i]);
//            Nodes[path_id][node_id].Atten = atten;
//        }

//        class node
//        {
//            public double[] Atten;
//            public int node_id;
//            public double[] SPL_Equipment = new double[8];
//            public double[] SPL_Aerodynamic = new double[8];
//            public Hare.Geometry.Point loc;
//            public List<node> Neighbors = new List<node>();
//            public double Width;
//            public double Height;
//            public double[] DuctTL = new double[8];
//            public double[] DuctLagging = new double[8];
//            public double[] DuctBreakout = new double[8];

//            public node(double[] Attenuation, Hare.Geometry.Point p, int id, double width, double height)
//            {
//                node_id = id;
//                Atten = Attenuation;
//                loc = p;
//                Width = width;
//                Height = height;
//                DuctTL = (width != 0 && height != 0) ? Breakout.Breakout_Rect : Breakout.Breakout_Round_Longseam;
//                DuctLagging = new double[8] {0,0,0,0,0,0,0,0};
//            }

//            public node(double[] Attenuation, Hare.Geometry.Point p, int id, node neighbor, double width, double height)
//                : this(Attenuation, p, id, width, height)
//            {
//                connect(neighbor);
//            }

//            public void connect(node neighbor)
//            {
//                Neighbors.Add(neighbor);
//                neighbor.Neighbors.Add(this);
//            }

//            public void propagate(double[] noise, NoiseType type, int last_id)
//            {
//                double[] new_noise = new double[8];
//                for (int oct = 0; oct < 8; oct++) new_noise[oct] = noise[oct] - Atten[oct];

//                if (type == NoiseType.Equipment)
//                {
//                    for (int oct = 0; oct < 8; oct++)
//                    {
//                        SPL_Equipment[oct] = 10 * Math.Log10(Math.Pow(10f, SPL_Equipment[oct] / 10) + Math.Pow(10f, (new_noise[oct]) / 10));
//                    }
//                }
//                else if (type == NoiseType.Aerodynamic)
//                {
//                    for (int oct = 0; oct < 8; oct++) SPL_Aerodynamic[oct] = 10 * Math.Log10(Math.Pow(10f, SPL_Aerodynamic[oct] / 10) + Math.Pow(10f, (new_noise[oct]) / 10));
//                }

//                foreach (node n in Neighbors)
//                {
//                    if (n.node_id != last_id) n.propagate(new_noise, type, this.node_id);
//                }//TODO: Add system to prevent sound from rolling back on itself.
//            }
//        }

//        class Noise_Source
//        {
//            node origin;
//            double[] SWL;
//            NoiseType Type;

//            public Noise_Source(double[] Noise, node Origin, NoiseType type)
//            {
//                SWL = Noise;
//                origin = Origin;
//                Type = type;
//            }

//            public void Propagate_Sound()
//            {
//                if (Type == NoiseType.Equipment)
//                {
//                    for (int oct = 0; oct < 8; oct++) origin.SPL_Equipment[oct] = 10 * Math.Log10(Math.Pow(10f, origin.SPL_Equipment[oct] / 10) + Math.Pow(10f, (SWL[oct]) / 10));
//                }
//                else if (Type == NoiseType.Aerodynamic)
//                {
//                    for (int oct = 0; oct < 8; oct++) origin.SPL_Aerodynamic[oct] = 10 * Math.Log10(Math.Pow(10f, origin.SPL_Aerodynamic[oct] / 10) + Math.Pow(10f, (SWL[oct]) / 10));
//                }

//                foreach (node n in origin.Neighbors)
//                {
//                    n.propagate(SWL, Type, origin.node_id);
//                }
//            }
//        }

//        public enum NoiseType
//        {
//            Equipment,
//            Aerodynamic,
//            Total
//        }
//    }
//}