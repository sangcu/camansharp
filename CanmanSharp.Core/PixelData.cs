using System.Collections.Generic;
using System.Drawing;

namespace CanmanSharp.Core
{
    public class PixelData
    {
        private List<Color> ColorData;

        public PixelData(List<Color> colors)
        {
            ColorData = colors;
        }
    }
}