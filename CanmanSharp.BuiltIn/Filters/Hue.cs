using System;
using System.Drawing;
using CanmanSharp.Core;
using Convert = CanmanSharp.Core.Convert;

namespace CanmanSharp.BuiltIn.Filters
{
    public class Hue : FilterBase
    {
        private readonly double _adjust;

        public Hue(double Adjust)
        {
            _adjust = Adjust;
        }

        public override Color Process(Color layerColor)
        {
            // var hsv = CanmanSharp.Core.Convert.RgbToHsv(Color.FromArgb(1, 0, 255, 0));

            HSVColor hsv = Convert.RgbToHsv(layerColor);

            double h = hsv.H*100;
            h += Math.Abs(_adjust);
            h = h%100;
            h /= 100;
            hsv.H = h;
            Color rgb = Convert.HsvToRgb(hsv.H, hsv.S, hsv.V);

            return Color.FromArgb(layerColor.A, rgb.R, rgb.G, rgb.B);
        }
    }
}