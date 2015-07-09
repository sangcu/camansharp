using System;
using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class Gamma : FilterBase
    {
        private readonly double _adjust;

        public Gamma(double Adjust)
        {
            _adjust = Adjust;
        }

        public override Color Process(Color layerColor)
        {
            double r = layerColor.R, g = layerColor.G, b = layerColor.B;
            r = Math.Pow((float) layerColor.R/255, _adjust)*255;
            g = Math.Pow((float) layerColor.G/255, _adjust)*255;
            b = Math.Pow((float) layerColor.B/255, _adjust)*255;
            return Color.FromArgb(layerColor.A, (int) r, (int) g, (int) b);
        }
    }
}