using System.Drawing;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.Core;

namespace CanmanSharp.Presets
{
    public class Toaster : FilterBase
    {
        private readonly Colorize _colorize;
        private readonly Exposure _exposure;

        public Toaster(Size ImageSize)
        {
            _colorize = new Colorize(Color.Red, 20);

            _exposure = new Exposure(26F);
        }

        public override Color Process(Color layerColor, Point point)
        {
            Color co = _colorize.Process(layerColor);
            Color ex = _exposure.Process(co, point);
            return ex;
        }
    }
}