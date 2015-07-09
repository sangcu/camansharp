using System;
using System.Diagnostics;
using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class Vibrance : FilterBase
    {
        private readonly float _adjust;

        public Vibrance(float Adjust)
        {
            _adjust = Adjust*(-1);
        }

        public override Color Process(Color layerColor)
        {
            float max = Math.Max(Math.Max(layerColor.R, layerColor.G), layerColor.B);
            float avg = (float) (layerColor.R + layerColor.G + layerColor.B)/3;
            float amt = ((Math.Abs(max - avg)*2/255)*_adjust)/100;

            float r = layerColor.R, g = layerColor.G, b = layerColor.B;
            if (Math.Abs(r - max)>0)
                r = r + (max - r)*amt;
            if (Math.Abs(g-max)>0)
                g = g + (max - g)*amt;
            if (Math.Abs(b - max)>0)
                b = b + (max - b)*amt;
            return Color.FromArgb(layerColor.A, (int)Math.Ceiling(Utils.ClampRGB(r)), (int)Math.Ceiling(Utils.ClampRGB(g)),
                (int)Math.Ceiling(Utils.ClampRGB(b)));
        }
    }
}