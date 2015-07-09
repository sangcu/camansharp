using System;
using System.Drawing;

namespace CanmanSharp.BuiltIn.Filters.Vignettes
{
    internal class VignetterFilters
    {
        public static Color Brightness(Color Rgb, float Amt, float OptsLength)
        {
            float r = Rgb.R - (Rgb.R*Amt*OptsLength);
            float g = Rgb.G - (Rgb.G*Amt*OptsLength);
            float b = Rgb.B - (Rgb.B*Amt*OptsLength);
            return Color.FromArgb(Rgb.A, (int) r, (int) g, (int) b);
        }

        public static Color Gamma(Color Rgb, float Amt, float OptsLength)
        {
            double r = Math.Pow(Rgb.R/255, Math.Max(10*Amt*OptsLength, 1))*255;
            double g = Math.Pow(Rgb.G/255, Math.Max(10*Amt*OptsLength, 1))*255;
            double b = Math.Pow(Rgb.B/255, Math.Max(10*Amt*OptsLength, 1))*255;
            return Color.FromArgb(Rgb.A, (int) r, (int) g, (int) b);
        }

        public static Color Colorize(Color Rgb, float Amt, Color Opts)
        {
            float r = Rgb.R - (Rgb.R - Opts.R)*Amt;
            float g = Rgb.G - (Rgb.G - Opts.G)*Amt;
            float b = Rgb.B - (Rgb.B - Opts.B)*Amt;
            return Color.FromArgb(Rgb.A, (int) r, (int) g, (int) b);
        }
    }
}