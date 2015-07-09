using System.Drawing;
using CanmanSharp.BuiltIn.Blurs;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.BuiltIn.Filters.Vignettes;
using CanmanSharp.Core;

namespace CanmanSharp.Presets
{
    public class Clarity : FilterBase
    {
        private readonly Curves _curves;
        private readonly Saturation _satutation;
        private readonly Vignette _vignette;
        private Sharpen _sharpen;

        public Clarity(Canman canman, Size ImageSize)
        {
            _curves = new Curves(new[] {0, 1, 2},
                new[] {new PointF(5, 0), new PointF(130, 150), new PointF(190, 220), new PointF(250, 255)});

            _sharpen = new Sharpen(50);
            _vignette = new Vignette(ImageSize, 60, 35);
            _satutation = new Saturation(0);
            canman.Add(_sharpen);
        }

        public override Color Process(Color layerColor, Point point)
        {
            Color curves = _curves.Process(layerColor);

            Color vignette = _vignette.Process(curves, point);
            return _satutation.Process(vignette);
        }
    }
}