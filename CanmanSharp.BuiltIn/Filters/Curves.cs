using System;
using System.Collections.Generic;
using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class Curves : FilterBase
    {
        private readonly Dictionary<float, double> _bezier;
        private readonly int[] _chans;

        /// <summary>
        /// </summary>
        /// <param name="ColorsOrders">Array of Colors Order: 0,1,2</param>
        /// <param name="Cps"></param>
        public Curves(int[] ColorsOrders, PointF[] Cps)
        {
            if (Cps.Length < 4)
                throw new ArgumentException("Cps have to 4 points");

            _bezier = Calculate.Bezier(Cps[0], Cps[1], Cps[2], Cps[3], 0, 255);
            _chans = ColorsOrders;
            // If the curve starts after x = 0, initialize it with a flat line
            // until the curve begins.
            PointF start = Cps[0];
            if (start.X > 0)
            {
                for (int i = 0; i < start.X; i++)
                {
                    _bezier[i] = start.Y;
                }
            }
            //... and the same with the end point
            PointF end = Cps[Cps.Length - 1];
            if (end.X < 255)
            {
                for (var i = (int) end.X; i <= 255; i++)
                {
                    _bezier[i] = end.Y;
                }
            }
        }


        public override Color Process(Color layerColor)
        {
            /*
             * # Now that we have the bezier curve, we do a basic hashmap lookup
             * # to find and replace color values.
             */
            var rbga = new int[] {layerColor.R, layerColor.G, layerColor.B, layerColor.A};

            for (int i = 0; i < _chans.Length; i++)
            {
                rbga[_chans[i]] = (int) _bezier[rbga[_chans[i]]];
            }
            return Color.FromArgb(rbga[3], rbga[0], rbga[1], rbga[2]);
        }
    }
}