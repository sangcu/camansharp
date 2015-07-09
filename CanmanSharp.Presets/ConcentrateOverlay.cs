using System.Drawing;
using CanmanSharp.BuiltIn.Blurs;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.Core;

namespace CanmanSharp.Presets
{
    public class ConcentrateOverlay : FilterBase
    {
        //private readonly Sharpen _sharpen;
        private readonly Channels _channels;
        private readonly Contracts _contrast;
        private readonly Exposure _explore;

        public ConcentrateOverlay()
        {
            _contrast = new Contracts(50);
            _explore = new Exposure(10);
            _channels = new Channels(Color.FromArgb(0, 0, 5));
        }

        public override Color Process(Color layerColor)
        {
            var contrast = _contrast.Process(layerColor);
            var explore = _explore.Process(contrast);
            var channel = _channels.Process(explore);
            return channel;
        }
    }
}