using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CanmanSharp.BuiltIn.Blurs;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.Core;

namespace CanmanSharp.Presets
{
    public class Jarques : FilterBase
    {
        private readonly List<FilterBase> _effectList = new List<FilterBase>();
        public Jarques()
        {
            _effectList.Add(new Saturation(-35));
            _effectList.Add(new Curves(new[] {2},
                new[] {new PointF(20, 0), new PointF(90, 120), new PointF(186, 144), new PointF(255, 230)}));
            _effectList.Add(new Curves(new[] {0},
                new[] {new PointF(0, 0), new PointF(144, 90), new PointF(138, 120), new PointF(255, 255)}));
            _effectList.Add(new Curves(new[] {1},
                new[] {new PointF(10, 0), new PointF(115, 105), new PointF(148, 100), new PointF(255, 248)}));
            _effectList.Add(new Curves(new[] {2},
                new[] {new PointF(0, 0), new PointF(120, 100), new PointF(128, 140), new PointF(255, 255)}));
            
        }

        public override Color Process(Color layerColor, Point point)
        {
            return _effectList.Aggregate(layerColor, (color, filter) => filter.Process(color, point));

        }
    }
}