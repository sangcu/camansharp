using System;
using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class Contracts : FilterBase
    {
        private readonly double _adjust;

        public Contracts(double Adjust)
        {
            _adjust = Math.Pow((Adjust + 100)/100, 2);
        }

        public override Color Process(Color layerColor)
        {
            double r = layerColor.R, g = layerColor.G, b = layerColor.B;
            // Red channel
            r /= 255;
            r -= 0.5;
            r *= _adjust;
            r += 0.5;
            r *= 255;

            // Green channel
            g /= 255;
            g -= 0.5;
            g *= _adjust;
            g += 0.5;
            g *= 255;

            // Blue channel
            b /= 255;
            b -= 0.5;
            b *= _adjust;
            b += 0.5;
            b *= 255;
            return Color.FromArgb(layerColor.A, (int) Utils.ClampRGB(r), (int) Utils.ClampRGB(g),
                (int) Utils.ClampRGB(b));
        }
    }
}