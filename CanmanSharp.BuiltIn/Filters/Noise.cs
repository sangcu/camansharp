using System;
using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class Noise : FilterBase
    {
        private readonly double _adjust;
        private readonly Random random;

        public Noise(double adjust)
        {
            _adjust = Math.Abs(adjust)*2.55f;
            random = new Random();
        }

        public override Color Process(Color layerColor)
        {
            double r = layerColor.R, g = layerColor.G, b = layerColor.B;
            int rand = random.Next((int) _adjust*-1, (int) _adjust);
            r = r + rand;
            g = g + rand;
            b = b + rand;
            return Color.FromArgb(layerColor.A, (int) Utils.ClampRGB(r), (int) Utils.ClampRGB(g),
                (int) Utils.ClampRGB(b));
        }
    }
}