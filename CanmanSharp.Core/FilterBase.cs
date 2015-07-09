using System.Drawing;

namespace CanmanSharp.Core
{
    public abstract class FilterBase : JobFactory
    {
        public FilterBase()
        {
            Type = FilterTypes.Single;
        }

        public virtual Color Process(Color layerColor)
        {
            return layerColor;
        }

        public virtual Color Process(Color layerColor, Point point)
        {
            return Process(layerColor);
        }
    }
}