using System.Drawing;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.Core;

namespace CanmanSharp.Presets
{
    public class SinCity : FilterBase
    {
        private readonly Brightness _brightness;
        private readonly Clip _clip;
        private readonly Contracts _contract;
        private readonly Exposure _exposure;
        private readonly GreyScale _greyScale;

        public SinCity(Size ImageSize)
        {
            _contract = new Contracts(100);
            _brightness = new Brightness(15);
            _exposure = new Exposure(10);
            _clip = new Clip(30);
            _greyScale = new GreyScale();
        }

        public override Color Process(Color layerColor, Point point)
        {
            Color contract = _contract.Process(layerColor);
            Color exposure = _exposure.Process(contract);
            Color brightness = _brightness.Process(exposure);
            Color clip = _clip.Process(brightness);
            Color grey = _greyScale.Process(clip);

            return grey;
        }
    }
}