using System.Drawing;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.BuiltIn.Filters.Vignettes;
using CanmanSharp.Core;

namespace CanmanSharp.Presets
{
    public class Lomo : FilterBase
    {
        private readonly Brightness _brightness5;
        private readonly Brightness _brightness15;
        private readonly Exposure _exposure;
        private readonly Gamma _gamma;
        private readonly Saturation _saturation;
        private Curves _curves;
        private Vignette _vignette;

        public Lomo(SizeF imageSizeF)
        {
            _brightness15 = new Brightness(15);
            _exposure = new Exposure(15);

            _curves = new Curves(new[] {0, 1, 2},
                new[] {new PointF(0, 0), new PointF(200, 0), new PointF(155, 255), new PointF(255, 255)});
            
            _saturation = new Saturation(-20);
            _gamma = new Gamma(1.8F);
            _vignette = new Vignette(imageSizeF,50,60);
            _brightness5 = new Brightness(5);
            
        }

        public override Color Process(Color layerColor, Point point)
        {
            var br15 = _brightness15.Process(layerColor);
            var ex = _exposure.Process(br15);
            var cur = _curves.Process(ex);
            var satu = _saturation.Process(cur);
            var gam = _gamma.Process(satu);
            var vignet = _vignette.Process(gam, point);
            var br5 =_brightness5.Process(vignet);
            return br5;
        }
    }
}