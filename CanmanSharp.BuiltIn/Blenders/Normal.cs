using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Blenders
{
    public class Normal : BlenderBase
    {
        public override Color TransformPixel(Color layer, Color parent)
        {
            return layer;
        }
    }
}