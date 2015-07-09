using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class GreyScale : FilterBase
    {
        public override Color Process(Color layerColor)
        {
            double r = layerColor.R, g = layerColor.G, b = layerColor.B;

            double avg = Calculate.Luminance(layerColor);

            r = avg;
            g = avg;
            b = avg;
            return Color.FromArgb(layerColor.A, Utils.ClampRGB((int) r), Utils.ClampRGB((int) g),
                Utils.ClampRGB((int) b));
        }
    }
}