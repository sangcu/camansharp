using System;
using System.Drawing;

namespace CanmanSharp.Core
{
    public class Pixel
    {
        private readonly Canman _canman;
        public Color Color;
        private SizeF _dimension;

        public Pixel(Color Color, Canman Canman)
        {
            this.Color = Color;
            _canman = Canman;
            _dimension = _canman.SourceImage.Size;
        }

        public double Loc { get; set; }

        public double CoordinatesToLocation(int x, int y, int width)
        {
            return (y*width + x)*4;
        }

        public PointF LocationToCoordinates(double loc, int width)
        {
            double x = (loc%(width*4))/4;
            double y = Math.Floor(loc/(width*4));
            return new PointF((float) x, (float) y);
        }

        public Pixel GetPixelRelative(double horiz, double vert)
        {
            double newLoc = Loc + (_dimension.Width*4*(vert*-1) + (4*horiz));
            if (newLoc > _dimension.Width*_dimension.Height || newLoc < 0)
                return new Pixel(Color.FromArgb(255, 0, 0, 0), _canman);
            return PixelAtLocation(newLoc);
        }

        private Pixel PixelAtLocation(double newLoc)
        {
            double x = newLoc/_dimension.Height;
            double y = newLoc%_dimension.Height;
            Color pixel =
                _canman.CurrentLayer.Bitmap.GetPixel(
                    (int) Math.Min(Math.Floor(x), _canman.CurrentLayer.Bitmap.Width - 1),
                    (int) Math.Min(Math.Floor(y), _canman.CurrentLayer.Bitmap.Height - 1));

            return new Pixel(pixel, _canman);
        }
    }
}