using System;
using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class Saturation : FilterBase
    {
        private readonly double _adjust;

        public Saturation(double Adjust)
        {
            _adjust = Adjust*-0.01;
        }

        public override Color Process(Color layerColor)
        {
            byte max = Math.Max(layerColor.R, Math.Max(layerColor.G, layerColor.B));
            double r = layerColor.R, g = layerColor.G, b = layerColor.B;

            if (r != max)
                r += (max - r)*_adjust;
            if (g != max)
                g += (max - g)*_adjust;
            if (b != max)
                b += (max - b)*_adjust;
            return Color.FromArgb(layerColor.A, Utils.ClampRGB((int) r), Utils.ClampRGB((int) g),
                Utils.ClampRGB((int) b));
        }
    }
}