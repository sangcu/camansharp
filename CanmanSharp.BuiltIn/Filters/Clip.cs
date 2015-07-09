using System;
using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class Clip : FilterBase
    {
        private readonly double _adjust;

        public Clip(double Adjust)
        {
            _adjust = Math.Abs(Adjust)*2.55;
        }

        public override Color Process(Color layerColor)
        {
            double r = layerColor.R, g = layerColor.G, b = layerColor.B;
            if (r > (255 - _adjust))
                r = 255;
            else if (r < _adjust)
                r = 0;
            if (g > 255 - _adjust)
                g = 255;
            else if (g < _adjust)
                g = 0;
            if (b > 255 - _adjust)
                b = 255;
            else if (b < _adjust)
                b = 0;
            return Color.FromArgb(layerColor.A, (int) r, (int) g, (int) b);
        }
    }
}