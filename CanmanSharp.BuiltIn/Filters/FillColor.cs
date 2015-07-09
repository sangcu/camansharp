using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class FillColor : FilterBase
    {
        private Color _fillColor;

        public FillColor(Color FillColor)
        {
            _fillColor = FillColor;
        }

        public override Color Process(Color layerColor)
        {
            return Color.FromArgb(layerColor.A, _fillColor.R, _fillColor.G, _fillColor.B);
        }
    }
}