using System;
using System.Drawing;
using CanmanSharp.BuiltIn.Blurs;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.BuiltIn.Filters.Vignettes;
using CanmanSharp.Core;

namespace CanmanSharp.Presets
{
    public class Concentrate : FilterBase
    {
        //private readonly Sharpen _sharpen;
        private readonly Saturation _satutation;
        private readonly Channels _channels;

        public Concentrate()
        {
            //_sharpen=new Sharpen(60);
            _satutation = new Saturation(-50);
            _channels=new Channels(Color.FromArgb(3,0,0));
        }

        public override Color Process(Color layerColor)
        {
            var satu = _satutation.Process(layerColor);
            var channel = _channels.Process(satu);

            return channel;
        }
    }
}