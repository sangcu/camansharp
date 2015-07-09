using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace CanmanSharp.Core
{
    public class Layer
    {
        private readonly Canman _canman;
        private Bitmap _bitmap;

        private BlenderBase _blendingMode;

        private float _opacity = 1.0f;

        public Guid LayerGuid { get; set; }

        public Layer(Canman canman, Bitmap bitmap, BlenderBase blendingMode)
        {
            _canman = canman;
            _blendingMode = blendingMode;
            _bitmap = bitmap;
            LayerGuid=Guid.NewGuid();
        }

        public Bitmap Bitmap
        {
            get { return _bitmap; }
            set { _bitmap = value; }
        }

        public BlenderBase BlendingMode
        {
            get { return _blendingMode; }
            set { _blendingMode = value; }
        }

        public float Opacity
        {
            get { return _opacity; }
            set { _opacity = ((float)value)/100; }
        }

        public void CopyLayer()
        {
        }

        public void FillColor()
        {
        }

        public void OverlayImage(Image image)
        {
        }

        public void ApplyToParent()
        {

            int width = _canman.LayerQueue.Peek().Bitmap.Width;
            int height = _canman.LayerQueue.Peek().Bitmap.Height;
            BitmapData srcpbits = _bitmap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int srcbytes = srcpbits.Stride * height;
            var rgbSrcValues = new byte[srcbytes];
            Marshal.Copy(srcpbits.Scan0, rgbSrcValues, 0, srcbytes);
            _bitmap.UnlockBits(srcpbits);

            BitmapData despbits = _canman.LayerQueue.Peek().Bitmap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            //declare an array to hold the bytes of the bitmap
            
            int desbytes = despbits.Stride * height;
            var rgbDesValues = new byte[desbytes];


            //Copy the RGB values into the array
            Marshal.Copy(despbits.Scan0, rgbDesValues, 0, desbytes);


            for (int i = 0; i < rgbDesValues.Length; i += 4)
            {
                Color parent = Color.FromArgb(rgbDesValues[i + 3], rgbDesValues[i], rgbDesValues[i + 1], rgbDesValues[i+2]);
                Color source = Color.FromArgb(rgbSrcValues[i + 3], rgbSrcValues[i], rgbSrcValues[i + 1], rgbSrcValues[i+2]);
                Color blenColor = _blendingMode.TransformPixel(source, parent);

                rgbDesValues[i + 3] = blenColor.A;
                rgbDesValues[i] = (byte)Utils.ClampRGB(parent.R - ((parent.R - blenColor.R)   * (Opacity * ((float)blenColor.A / 255))));
                rgbDesValues[i + 1] = (byte)Utils.ClampRGB(parent.G - ((parent.G - blenColor.G) * (Opacity * ((float)blenColor.A / 255))));
                rgbDesValues[i + 2] = (byte)Utils.ClampRGB(parent.B - ((parent.B - blenColor.B) * (Opacity * ((float)blenColor.A / 255))));
            }
            Marshal.Copy(rgbDesValues, 0, despbits.Scan0, desbytes);
            _canman.LayerQueue.Peek().Bitmap.UnlockBits(despbits);
        }

        public override string ToString()
        {
            return LayerGuid.ToString();
        }
    }
}