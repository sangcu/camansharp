using System;
using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters
{
    public class Channels : FilterBase
    {
        private readonly float _blue;
        private readonly float _green;
        private readonly float _red;

        public Channels(Color Option)
        {
            if (Option.R != 0)
                _red = (float) Option.R/100;
            if (Option.G != 0)
                _green = (float) Option.G/100;
            if (Option.B != 0)
                _blue = (float) Option.B/100;
        }

        public override Color Process(Color layerColor)
        {
            double r = layerColor.R, g = layerColor.G, b = layerColor.B;

            if (_red > 0)
            {
                r += (255 - layerColor.R)*_red;
            }
            else
            {
                r -= layerColor.R*Math.Abs(_red);
            }

            if (_green > 0)
            {
                g += (255 - layerColor.G)*_green;
            }
            else
            {
                g -= layerColor.G*Math.Abs(_green);
            }

            if (_blue > 0)
            {
                b += (255 - layerColor.B)*_blue;
            }
            else
            {
                b -= layerColor.B*Math.Abs(_blue);
            }

            return Color.FromArgb((int) r, (int) g, (int) b);
        }
    }
}