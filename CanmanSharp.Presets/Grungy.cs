using System.Drawing;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.BuiltIn.Filters.Vignettes;
using CanmanSharp.Core;

namespace CanmanSharp.Presets
{
    public class Grungy : FilterBase
    {
        private readonly Clip _clip;
        private readonly Contracts _contract;
        private readonly Gamma _gamma;
        private readonly Noise _noise;
        private readonly Saturation _saturation;
        private readonly Vignette _vignette;

        public Grungy(Size ImageSize)
        {
            _gamma = new Gamma(1.5F);
            _clip = new Clip(25);
            _saturation = new Saturation(-60);
            _contract = new Contracts(5);
            _noise = new Noise(5);
            _vignette = new Vignette(ImageSize, 50, 30);
        }

        public override Color Process(Color layerColor, Point point)
        {
            Color gam = _gamma.Process(layerColor);
            Color cl = _clip.Process(gam);
            Color sa = _saturation.Process(cl);
            Color con = _contract.Process(sa);
            Color noi = _noise.Process(con);
            Color vig = _vignette.Process(noi, point);
            return vig;
        }
    }
}