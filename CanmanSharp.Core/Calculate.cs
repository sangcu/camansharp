using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CanmanSharp.Core
{
    public static class Calculate
    {
        public static double Distance(Point A, Point B)
        {
            return Math.Sqrt(Math.Pow(B.X - A.X, 2) + Math.Pow(B.Y - A.Y, 2));
        }

        /*
        Generates a pseudorandom number that lies within the max - mix range. The number can be either 
        an integer or a float depending on what the user specifies.
        @param [Number] min The lower bound (inclusive).
        @param [Number] max The upper bound (inclusive).
        @param [Boolean] getFloat Return a Float or a rounded Integer?
        @return [Number] The pseudorandom number, either as a float or integer.
        */

        public static int RandomRange(int min, int max, int seed)
        {
            var rand = new Random(seed);
            return rand.Next(min, max);
        }

        /*
         * Calculates the luminance of a single pixel using a special weighted sum.
         * @param [Object] rgba RGBA object describing a single pixel.
         * @return [Number] The luminance value of the pixel.
         */

        public static double Luminance(Color rgba)
        {
            return (0.299*rgba.R) + (0.587*rgba.G) + (0.114*rgba.B);
        }

        public static float Lerp(float a, float b, float t)
        {
            return a*(1 - t) + b*t;
        }

        public static double Clamp(double a, int min, int max)
        {
            return Math.Min(Math.Max(a, min), max);
        }

        public static double Clamp(double a, float min, float max)
        {
            return Math.Min(Math.Max(a, min), max);
        }

        /*
          # Generates a bezier curve given a start and end point, with control points in between.
          # Can also optionally bound the y values between a low and high bound.
          #
          # This is different than most bezier curve functions because it attempts to construct it in such 
          # a way that we can use it more like a simple input -> output system, or a one-to-one function. 
          # In other words we can provide an input color value, and immediately receive an output modified 
          # color value.
          #
          # Note that, by design, this does not force X values to be in the range [0..255]. This is to
          # generalize the function a bit more. If you give it a starting X value that isn't 0, and/or a
          # ending X value that isn't 255, you may run into problems with your filter!
          #
          # @param [Array] 2-item arrays describing the x, y coordinates of the control points. Minimum two.
          # @param [Number] lowBound (optional) Minimum possible value for any y-value in the curve.
          # @param [Number] highBound (optional) Maximum posisble value for any y-value in the curve.
          # @return [Array] Array whose index represents every x-value between start and end, and value
          #   represents the corresponding y-value.
         */

        public static Dictionary<float, double> Bezier(PointF start, PointF p1, PointF p2, PointF end, int lowBound = 0,
            int highBound = 255)
        {
            var controlPoints = new List<PointF>();
            float endX = 0;
            controlPoints.Add(start);
            controlPoints.Add(p1);
            controlPoints.Add(p2);
            controlPoints.Add(end);
            float Cx = (int) (3*(p1.X - start.X));

            float Bx = 3*(p2.X - p1.X) - Cx;

            float Ax = end.X - start.X - Cx - Bx;
            float Cy = 3*(p1.Y - start.Y);
            float By = 3*(p2.Y - p1.Y) - Cy;
            float Ay = end.Y - start.Y - Cy - By;


            if (controlPoints.Count < 2)
                throw new ArgumentException("Invalid number of arguments to bezier");

            var beziers = new Dictionary<float, double>();
            for (float i = 0; i < 1000; i++)
            {
                float t = i/1000;
                double curveX = Math.Round((Ax*Math.Pow(t, 3)) + (Bx*Math.Pow(t, 2)) + (Cx*t) + start.X);
                double curveY = Math.Round((Ay*Math.Pow(t, 3)) + (By*Math.Pow(t, 2)) + (Cy*t) + start.Y);
                if (curveY < lowBound)
                {
                    curveY = lowBound;
                }
                else if (curveY > highBound)
                {
                    curveY = highBound;
                }
                beziers[(int) curveX] = curveY;
            }
            endX = controlPoints[controlPoints.Count - 1].X;
            beziers = MissingValues(beziers, endX);
            //Edge case
            if (beziers[endX] == 0)
                beziers[endX] = beziers[endX - 1];
            return beziers;
        }

        public static PointF Add(PointF a, PointF b, PointF c, PointF d)
        {
            return new PointF(a.X + b.X + c.X + d.X, a.Y + b.Y + c.Y + d.Y);
        }

        public static Point Mul(Point a, Point b)
        {
            return new Point(a.X*b.X, a.Y*b.Y);
        }

        public static PointF Mul(PointF a, PointF b)
        {
            return new PointF(a.X*b.X, a.Y*b.Y);
        }

        public static PointF Sub(PointF a, PointF b)
        {
            return new PointF(a.X - b.X, a.Y - b.Y);
        }

        public static void Hermite(PointF[] controlPoints, int lowBound, int highBound)
        {
            if (controlPoints.Length < 2)
                throw new ArgumentException("Invalid number of arguments to bezier");

            var ret = new Dictionary<float, double>();

            int count = 0;
            for (int i = 0; i < controlPoints.Length - 2; i++)
            {
                PointF p0 = controlPoints[i];
                PointF p1 = controlPoints[i + 1];
                float pointsPerSegment = p1.X - p0.X;
                float pointsPerStep = 1/pointsPerSegment;
                //the last point of the last segment should reach p1
                if (i == controlPoints.Length - 2)
                    pointsPerStep = 1/(pointsPerSegment - 1);
                PointF p = i > 0 ? controlPoints[i - 1] : p0;
                PointF m0 = Mul(Sub(p1, p), new PointF(0.5f, 0.5f));

                p = i > controlPoints.Length - 2 ? controlPoints[i + 2] : p1;
                PointF m1 = Mul(Sub(p, p0), new PointF(0.5f, 0.5f));

                for (int j = 0; j < pointsPerSegment; j++)
                {
                    float t = j*pointsPerStep;
                    var fac0 = (float) (2.0*t*t*t - 3.0*t*t + 1.0);
                    var fac1 = (float) (t*t*t - 2.0*t*t + t);
                    var fac2 = (float) (-2.0*t*t*t + 3.0*t*t);
                    float fac3 = t*t*t - t*t;

                    PointF pos = Add(Mul(p0, new PointF(fac0, fac0)), Mul(m0, new PointF(fac1, fac1)),
                        Mul(p1, new PointF(fac2, fac2)), Mul(m1, new PointF(fac3, fac3)));

                    ret[pos.X] = Math.Round(Clamp(pos.Y, lowBound, highBound));

                    count += 1;
                }
            }
            float endX = controlPoints[controlPoints.Length - 1].X;
            ret = MissingValues(ret, endX);
        }

        /*
         # Calculates possible missing values from a given value array. Note that this returns a copy
          # and does not mutate the original. In case no values are missing the original array is
          # returned as that is convenient.
          #
          # @param [Array] 2-item arrays describing the x, y coordinates of the control points.
          # @param [Number] end x value of the array (maximum)
          # @return [Array] Array whose index represents every x-value between start and end, and value
          #   represents the corresponding y-value.
         */

        public static Dictionary<float, double> MissingValues(Dictionary<float, double> values, float EndX)
        {
            /*
             Do a search for missing values in the bezier array and use linear
             interpolation to approximate their values
             */
            PointF rightCoord = PointF.Empty, leftCoord = PointF.Empty;
            if (values.Keys.Count < EndX + 1)
            {
                var ret = new Dictionary<float, double>();
                for (int i = 0; i <= EndX; i++)
                {
                    if (values.Keys.Contains(i))
                        ret[i] = values[i];
                    else if (ret.Count > 0)
                    {
                        leftCoord = new PointF(i - 1, (float) ret[i - 1]);
                        /*
                         *  # Find the first value to the right. Ideally this loop will break
                            # very quickly.
                         */
                        for (int j = i; j <= EndX; j++)
                        {
                            if (values.Keys.Contains(j))
                            {
                                rightCoord = new PointF(j, (float) values[j]);
                                break;
                            }
                        }
                        ret[i] = leftCoord.Y +
                                 ((rightCoord.Y - leftCoord.Y)/(rightCoord.X - leftCoord.X))*(i - leftCoord.X);
                    }
                }
                return ret;
            }
            return values;
        }

        public static double Distance(PointF A, PointF B)
        {
            return Math.Sqrt(Math.Pow(B.X - A.X, 2) + Math.Pow(B.Y - A.Y, 2));
        }
    }
}