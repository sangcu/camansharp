using System.Drawing;

namespace CanmanSharp.Core
{
    public abstract class BlenderBase : JobFactory
    {
        public abstract Color TransformPixel(Color layer, Color parent);
    }
}