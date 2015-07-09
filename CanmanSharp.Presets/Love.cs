using System.Drawing;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.Core;

namespace CanmanSharp.Presets
{
    public class Love : FilterBase
    {
        private readonly Brightness _brightness;
        private readonly Colorize _colorize;
        private readonly Contracts _contract;
        private readonly Exposure _exposure;
        private readonly Gamma _gamma;
        private readonly Vibrance _vibrance;

        public Love(Size ImageSize)
        {
            _brightness = new Brightness(5);

            _exposure = new Exposure(8);

            _contract = new Contracts(4);

            _vibrance = new Vibrance(50);

            _colorize = new Colorize(Color.FromArgb(255, 196, 32, 7), 30);

            _gamma = new Gamma(1.3F);
        }

        public override Color Process(Color layerColor, Point point)
        {
            Color br = _brightness.Process(layerColor);

            Color ex = _exposure.Process(br);
            Color con = _contract.Process(ex);
            Color co = _colorize.Process(con);
            Color vig = _vibrance.Process(co, point);
            Color gam = _gamma.Process(vig);

            return gam;
        }
    }
}