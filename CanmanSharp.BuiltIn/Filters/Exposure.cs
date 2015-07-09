using System;
using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class Exposure : FilterBase
    {
        private readonly Curves _curves;

        public Exposure(float Adjust)
        {
            float p = Math.Abs(Adjust)/100;
            var ctr1 = new PointF(0.0F, 255.0F*p);
            var ctr2 = new PointF(255 - (255*p), 255);

            if (Adjust < 0)
            {
                ctr1 = new PointF(ctr1.Y, ctr1.X);
                ctr2 = new PointF(ctr2.Y, ctr2.X);
            }
            _curves = new Curves(new[] {0, 1, 2}, new[] {new PointF(0, 0), ctr1, ctr2, new PointF(255, 255)});
        }

        public override Color Process(Color layerColor)
        {
            return _curves.Process(layerColor);
        }
    }
}