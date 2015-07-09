using System.Drawing;
using CanmanSharp.BuiltIn.Blenders;
using CanmanSharp.BuiltIn.Blurs;
using CanmanSharp.BuiltIn.Filters;
using CanmanSharp.BuiltIn.Filters.Vignettes;
using CanmanSharp.Core;
using CanmanSharp.Presets;

namespace CanmanSharpSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string input = @"C:\in.jpg";

            string output = @"C:\out.jpg";

            Image image = Image.FromFile(input);

            //var canman = new Canman(image as Bitmap, output, new Normal());
            //canman.Add(new Vignette(image.Size, 40, 30));
            //canman.Render();


            var canman = new CamanEffects(image as Bitmap,output, new Normal());
            canman.Add(new Sharpen(40));
            canman.RenderBitmap();

            canman.Add(EffectsEnum.OrangePeel);

            canman.CurrentLayer.BlendingMode = new Multiply();
            canman.NewLayer(new ConcentrateOverlay(), new Multiply(), 80);
            canman.ExecuteLayer(new Layer(canman, canman.SourceImage, new Multiply()));

            canman.RenderBitmap();
            canman.NewLayer(new ConcentrateOverlay(), new Multiply(), 80);
            canman.Add(new ConcentrateOverlay());
            canman.Add(new Sharpen(5));
            canman.RenderBitmap();
            canman.Add(new Brightness(10));

            canman.Render();


            ////canmane = new CamanEffects(image as Bitmap, @"D:\out2.jpg");
            ////canmane.Add(EffectsEnum.OrancePeel);
            ////canmane.Render();
            //canmane = new CamanEffects(image as Bitmap, @"D:\out3.jpg");
            //canmane.Add(EffectsEnum.Toaster);
            //canmane.Render();
            // create Perlin noise function
        }
    }
}