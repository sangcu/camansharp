using System;
using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class Sepia : FilterBase
    {
        private readonly double _adjust;

        public Sepia(double Adjust)
        {
            _adjust = Adjust/100;
        }

        public override Color Process(Color layerColor)
        {
            double r = layerColor.R, g = layerColor.G, b = layerColor.B;
            r = Math.Min(255, (r*(1 - (0.607*_adjust))) + (g*(0.769*_adjust)) + (b*(0.189*_adjust)));
            g = Math.Min(255, (r*(0.349*_adjust)) + (g*(1 - (0.314*_adjust))) + (b*(0.168*_adjust)));
            b = Math.Min(255, (r*(0.272*_adjust)) + (g*(0.534*_adjust)) + (b*(1 - (0.869*_adjust))));
            return Color.FromArgb(layerColor.A, (int) Utils.ClampRGB(r), (int) Utils.ClampRGB(g),
                (int) Utils.ClampRGB(b));
        }
    }
}