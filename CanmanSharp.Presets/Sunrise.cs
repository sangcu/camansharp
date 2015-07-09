using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.BuiltIn.Filters.Vignettes;
using CanmanSharp.Core;

namespace CanmanSharp.Presets
{
    public class Sunrise : FilterBase
    {
        private readonly List<FilterBase> _effectsList=new List<FilterBase>();

        public Sunrise(Size imageSize)
        {
            _effectsList.Add(new Exposure(3.5F));
            _effectsList.Add(new Saturation(-5));
            _effectsList.Add(new Vibrance(50));
            _effectsList.Add(new Sepia(60));
            _effectsList.Add(new Colorize(Color.FromArgb(232, 123, 34), 10));
            _effectsList.Add(new Channels(Color.FromArgb(8, 0, 8)));
            _effectsList.Add(new Contracts(5));
            _effectsList.Add(new Gamma(1.2F));
            _effectsList.Add(new Vignette(imageSize, 55, 25));
        }

        public override Color Process(Color layerColor, Point point)
        {
            return _effectsList.Aggregate(layerColor, (current, filter) => filter.Process(current, point));
        }
    }
}