using System.Drawing;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.Core;

namespace CanmanSharp.Presets
{
    public class CrossProcess : FilterBase
    {
        private readonly Channels _channel;
        private readonly Colorize _colorize;
        private readonly Contracts _contract;
        private readonly Curves _curves;
        private readonly Exposure _exposure;
        private readonly Gamma _gamma;
        private readonly Sepia _sepia;

        public CrossProcess(Size ImageSize)
        {
            _exposure = new Exposure(5);
            _colorize = new Colorize(Color.FromArgb(255, 232, 123, 34), 4);
            _sepia = new Sepia(5);
            _channel = new Channels(Color.FromArgb(255, 3, 0, 8));
            _curves = new Curves(new[] {2},
                new[] {new PointF(0, 0), new PointF(100, 150), new PointF(180, 180), new PointF(255, 255)});
            _contract = new Contracts(15);
            _gamma = new Gamma(1.6F);
        }

        public override Color Process(Color layerColor, Point point)
        {
            Color exposure = _exposure.Process(layerColor);
            Color cololize = _colorize.Process(exposure);
            Color sepia = _sepia.Process(cololize);
            Color channel = _channel.Process(sepia);
            Color curves = _curves.Process(channel);
            Color contract = _contract.Process(curves);
            Color gamma = _gamma.Process(contract);

            return gamma;
        }
    }
}