using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class Invert : FilterBase
    {
        public override Color Process(Color layerColor)
        {
            int r = 255 - layerColor.R;
            int g = 255 - layerColor.G;
            int b = 255 - layerColor.B;
            return Color.FromArgb(layerColor.A, r, g, b);
        }
    }
}