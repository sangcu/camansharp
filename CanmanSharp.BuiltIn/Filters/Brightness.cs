using System;
using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class Brightness : FilterBase
    {
        private readonly int _adjust;

        public Brightness(double Adjust)
        {
            _adjust = (int) Math.Floor(255*(Adjust/100));
        }

        public override Color Process(Color layerColor)
        {
            int r = layerColor.R + _adjust;
            int g = layerColor.G + _adjust;
            int b = layerColor.B + _adjust;
            return Color.FromArgb(layerColor.A, Utils.ClampRGB(r), Utils.ClampRGB(g), Utils.ClampRGB(b));
        }
    }
}