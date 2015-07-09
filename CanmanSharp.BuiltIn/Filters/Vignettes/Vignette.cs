using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters.Vignettes
{
    public class Vignette : FilterBase
    {
        private readonly Dictionary<float, double> _bezier;
        private readonly PointF _center;
        private readonly double _end;
        private readonly double _size;
        private readonly double _strength;
        private SizeF _dimension;
        private double _start;

        public Vignette(SizeF ImageSize, float SizePercent, float Strength)
        {
            if (ImageSize.Height > ImageSize.Width)
                _size = ImageSize.Width*(SizePercent/100);
            else
                _size = ImageSize.Height*(SizePercent/100);

            _dimension = ImageSize;
            _strength = Strength/100;
            _center = new PointF(_dimension.Width/2, _dimension.Height/2);
            _start = Math.Sqrt(Math.Pow(_center.X, 2) + Math.Pow(_center.Y, 2));

            _end = _start - _size;

            _bezier = Calculate.Bezier(new PointF(0, 1), new PointF(30, 30), new PointF(70, 60), new PointF(100, 80));
        }

        public override Color Process(Color LayerColor, Point point)
        {
            Point loc = point;

            double dist = Calculate.Distance(loc, _center);
            double div = 0.0;
            double r = LayerColor.R, g = LayerColor.G, b = LayerColor.B;
            if (dist > _end)
            {
                var key = (int) Math.Ceiling(((dist - _end)/_size)*100);
                if (_bezier.ContainsKey(key))
                {
                    div = Math.Max(1, ((_bezier[key]/10)*_strength));

                    r = Math.Pow((float) LayerColor.R/255, div)*255;
                    g = Math.Pow((float) LayerColor.G/255, div)*255;
                    b = Math.Pow((float) LayerColor.B/255, div)*255;
                }
                else
                {
                    Debug.WriteLine("{0},{1}", point.X, point.Y);
                }
            }
            return Color.FromArgb(LayerColor.A, (int) r, (int) g, (int) b);
        }
    }
}