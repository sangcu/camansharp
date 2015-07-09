using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CanmanSharp.BuiltIn.Blurs;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.BuiltIn.Filters.Vignettes;
using CanmanSharp.Core;

namespace CanmanSharp.Presets
{
    public class OrangePeel : FilterBase
    {
        private readonly List<FilterBase> _filterBases=new List<FilterBase>();

        public OrangePeel()
        {
            _filterBases.Add(new Curves(new[] {0, 1, 2},
                new[] {new PointF(0, 0), new PointF(100, 50), new PointF(140, 200), new PointF(255, 255)}));

            _filterBases.Add(new Vibrance(-30));
            
            _filterBases.Add(new Saturation(-30));

            
            _filterBases.Add(new Colorize(Color.FromArgb(255, 255, 144, 0), 30));
            _filterBases.Add(new Contracts(-5));
            _filterBases.Add(new Gamma(1.4F));
        }

        public override Color Process(Color layerColor, Point point)
        {
            return _filterBases.Aggregate(layerColor, (color, filter) => filter.Process(color, point));
        }
    }
}