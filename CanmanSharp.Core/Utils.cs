using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace CanmanSharp.Core
{
    public class Utils
    {
        public static int ClampRGB(int value)
        {
            return Math.Min(255, Math.Max(0, value));
        }

        public static double ClampRGB(double value)
        {
            return Math.Min(255.0, Math.Max(0.0, value));
        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static Bitmap CopyBitmap(Bitmap source)
        {
            BitmapData srcpbits = source.LockBits(new Rectangle(0, 0, source.Width, source.Height),
               ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int srcbytes = srcpbits.Stride * source.Height;
            var rgbSrcValues = new byte[srcbytes];
            Marshal.Copy(srcpbits.Scan0, rgbSrcValues, 0, srcbytes);

            var bitmap = new Bitmap(source.Width, source.Height);
            var desData= bitmap.LockBits(new Rectangle(0, 0, source.Width, source.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            Marshal.Copy(rgbSrcValues,0,desData.Scan0, srcbytes);

            bitmap.UnlockBits(desData);

            source.UnlockBits(srcpbits);

            return bitmap;
        }
    }
}