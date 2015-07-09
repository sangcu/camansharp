using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Blenders
{
    public class Multiply : BlenderBase
    {
        public override Color TransformPixel(Color layer, Color parent)
        {
            var r = ((float)layer.R * parent.R) / 255;
            var g = ((float)layer.G * parent.G) / 255;
            var b = ((float)layer.B * parent.B) / 255;
            return Color.FromArgb(layer.A, (int) Utils.ClampRGB(r), (int) Utils.ClampRGB(g), (int) Utils.ClampRGB(b));
        }
    }
}