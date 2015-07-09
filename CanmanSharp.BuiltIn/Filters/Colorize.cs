using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class Colorize : FilterBase
    {
        private readonly float _level;
        private Color _color;

        public Colorize(Color color, float level)
        {
            _color = color;
            _level = level;
        }

        public override Color Process(Color layerColor)
        {
            float r = layerColor.R - (layerColor.R - _color.R)*(_level/100);
            float g = layerColor.G - (layerColor.G - _color.G)*(_level/100);
            float b = layerColor.B - (layerColor.B - _color.B)*(_level/100);
            return Color.FromArgb(layerColor.A, (int) r, (int) g, (int) b);
        }
    }
}