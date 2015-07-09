using System.Collections.Generic;
using System.Drawing;

namespace CanmanSharp.Core
{
    /// <summary>
    ///     Various image analysis methods
    /// </summary>
    public class Analyze
    {
        private List<Color> Colors = new List<Color>();

        /// Calculates the number of occurances of each color value throughout the image.
        /// @return {Object} Hash of RGB channels and the occurance of each value
        public void CalculateLevels(PixelData[] pixelsArray)
        {
        }
    }
}