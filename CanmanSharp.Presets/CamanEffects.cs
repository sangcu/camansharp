using System.Drawing;
using CanmanSharp.BuiltIn.Blenders;
using CanmanSharp.BuiltIn.Blurs;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.Core;

namespace CanmanSharp.Presets
{
    public sealed class CamanEffects : Canman
    {
        public CamanEffects(string ImagePath) : base(ImagePath)
        {
        }

        public CamanEffects(Bitmap SourceImage)
            : base(SourceImage, string.Empty, new Normal())
        {
        }

        public CamanEffects(Bitmap SourceImage, string Output, BlenderBase Blender)
            : base(SourceImage, Output, Blender)
        {
        }

        public void Add(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Vintage:
                    Add(new Vintage(SourceImage.Size));
                    break;
                case EffectsEnum.Clarity:
                    Add(new Clarity(this, SourceImage.Size));
                    break;
                case EffectsEnum.Concentrate:
                    Add(new Sharpen(40));
                    Add(new Concentrate());
                    NewLayer(new ConcentrateOverlay(),new Multiply(),80);
                    Add(new Sharpen(5));
                    Add(new Brightness(10));
                    break;
                case EffectsEnum.CrossProcess:
                    Add(new CrossProcess(SourceImage.Size));
                    break;
                case EffectsEnum.OrangePeel:
                    Add(new OrangePeel());
                    break;
                case EffectsEnum.Lomo:
                    Add(new Lomo(SourceImage.Size));
                    break;
                case EffectsEnum.SinCity:
                    Add(new SinCity(SourceImage.Size));
                    break;
                case EffectsEnum.Sunrise:
                    Add(new Sunrise(SourceImage.Size));
                    break;
                case EffectsEnum.Love:
                    Add(new Love(SourceImage.Size));
                    break;
                case EffectsEnum.Grungy:
                    Add(new Grungy(SourceImage.Size));
                    break;
                case EffectsEnum.Jarques:
                    Add(new Jarques());
                    Add(new Sharpen(20));
                    break;
            }
        }
    }
}