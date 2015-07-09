using System.Drawing;

namespace CanmanSharp.BuiltIn.Filters.Vignettes
{
    public class VignetteOption
    {
        public float Strength { get; set; }
        public float CornorRadius { get; set; }
        public VignetteFilterEnum VignetteFilter { get; set; }
        public Color Color { get; set; }
        public Size Size { get; set; }
    }
}