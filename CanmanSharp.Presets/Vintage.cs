using System.Drawing;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.BuiltIn.Filters.Vignettes;
using CanmanSharp.Core;

namespace CanmanSharp.Presets
{
    public class Vintage : FilterBase
    {
        private readonly Sepia _Sepia;
        private readonly Channels _channel;
        private readonly Contracts _contract;
        private readonly Gamma _gamma;
        private readonly GreyScale _greyScale;
        private readonly Noise _noice;
        private readonly Vignette _vignette;

        public Vintage(Size ImageSize)
        {
            _greyScale = new GreyScale();
            _contract = new Contracts(5);
            _noice = new Noise(3);
            _Sepia = new Sepia(100);
            _channel = new Channels(Color.FromArgb(8, 4, 2));
            _gamma = new Gamma(0.87F);

            _vignette = new Vignette(ImageSize, 40, 30);
        }

        public override Color Process(Color layerColor, Point point)
        {
            Color grey = _greyScale.Process(layerColor);
            Color contract = _contract.Process(grey);
            Color noise = _noice.Process(contract);
            Color sepia = _Sepia.Process(noise);
            Color channel = _channel.Process(sepia);
            Color gamma = _gamma.Process(channel);
            Color vintage = _vignette.Process(gamma, point);

            return vintage;
        }
    }
}