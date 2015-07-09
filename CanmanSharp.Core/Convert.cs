using System;
using System.Drawing;
using System.Globalization;

namespace CanmanSharp.Core
{
    public class Convert
    {
        /// <summary>
        ///     # Converts the hex representation of a color to RGB values.
        ///     # Hex value can optionally start with the hash (#).
        ///     #
        ///     # @param  [String] hex  The colors hex value
        ///     # @return [Array]       The RGB representation
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Color HexToRGB(string hex)
        {
            if (hex.StartsWith("#"))
            {
                hex = hex.Substring(1);
            }
            short r = Int16.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            short g = Int16.Parse(hex.Substring(2, 4), NumberStyles.HexNumber);
            short b = Int16.Parse(hex.Substring(4, 6), NumberStyles.HexNumber);

            return Color.FromArgb(1, r, g, b);
        }

        /*
         * # Converts an RGB color to HSL.
          # Assumes r, g, and b are in the set [0, 255] and
          # returns h, s, and l in the set [0, 1].
          #
          # @overload rgbToHSL(r, g, b)
          #   @param   [Number]  r   Red channel
          #   @param   [Number]  g   Green channel
          #   @param   [Number]  b   Blue channel
          #
          # @overload rgbToHSL(rgb)
          #   @param [Object] rgb The RGB object.
          #   @option rgb [Number] r The red channel.
          #   @option rgb [Number] g The green channel.
          #   @option rgb [Number] b The blue channel.
          #
          # @return  [Array]       The HSL representation
         */

        public static Color RgbToHSL(int r, int g, int b)
        {
            r /= 255;
            g /= 255;
            b /= 255;

            int max = Math.Max(Math.Max(r, g), b);
            int min = Math.Min(Math.Min(r, g), b);
            int l = (max + min)/2;
            int h = 0, s = 0, d = 0;
            if (max == min)
                h = s = 0;
            else
            {
                d = max - min;
                if (l > 0.5)
                    s = d/(2 - max - min);
                else
                    s = d/(max + min);

                if (max == r)
                    h = (g - b)/d + g < b ? 6 : 0;
                else if (max == g)
                    h = (b - r)/d + 2;
                else if (max == b)
                    h = (r - g)/d + 4;

                h /= 6;
            }
            return Color.FromArgb(1, h, s, l);
        }

        public static Color HslToRgb(Color hsl)
        {
            int h = hsl.R, s = hsl.G, l = hsl.B;
            int r = 0, g = 0, b = 0;
            if (s == 0)
                r = g = b = l;
            else
            {
                int q = 0;
                if (l < 0.5)
                {
                    q = l*(1 + s);
                }
                else
                {
                    q = (l + s) - (l*s);
                }

                int p = 2*l - q;

                r = HueToRgb(p, q, h + 1/3);
                g = HueToRgb(p, q, h);
                b = HueToRgb(p, q, h - 1/3);
            }
            return Color.FromArgb(1, r*255, g*255, b*255);
        }

        /*
         * # Converts from the hue color space back to RGB.
          #
          # @param [Number] p
          # @param [Number] q
          # @param [Number] t
          # @return [Number] RGB value
         */

        public static int HueToRgb(int p, int q, int t)
        {
            if (t < 0)
                t += 1;
            if (t > 1)
                t -= 1;
            if (t < 1/6)
                return p + (q - p)*6*t;
            if (t < 1/2)
                return q;
            if (t < 2/3)
                return p + (q - p)*(2/3 - t)*6;
            return p;
        }

        /*
         * # Converts an RGB color value to HSV. Conversion formula
          # adapted from {http://en.wikipedia.org/wiki/HSV_color_space}.
          # Assumes r, g, and b are contained in the set [0, 255] and
          # returns h, s, and v in the set [0, 1].
          #
          # @param   [Number]  r       The red color value
          # @param   [Number]  g       The green color value
          # @param   [Number]  b       The blue color value
          # @return  [Array]           The HSV representation
         */

        public static HSVColor RgbToHsv(Color color)
        {
            float r = (float) color.R/255, g = (float) color.G/255, b = (float) color.B/255;
            float h = 0, s = 0, v = 0, d = 0;
            float max = Math.Max(Math.Max(r, g), b);
            float min = Math.Min(Math.Min(r, g), b);
            v = max;
            d = max - min;
            s = max == 0 ? 0 : d/max;
            if (max == min)
                h = 0;
            else
            {
                if (max == r)
                    h = (g - b)/d + (g < b ? 6 : 0);
                else if (max == g)
                    h = (b - r)/d + 2;
                else if (max == b)
                    h = (r - g)/d + 4;
                h /= 6;
            }
            return new HSVColor {H = h, S = s, V = v};
        }

        public static Color HsvToRgb(double h, double s, double v)
        {
            double b = 0, f, g = 0, i, p, q, r = 0, t;
            i = Math.Floor(h*6);
            f = h*6 - i;
            p = v*(1 - s);
            q = v*(1 - f*s);
            t = v*(1 - (1 - f)*s);
            switch ((int) i%6)
            {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                case 5:
                    r = v;
                    g = p;
                    b = q;
                    break;
            }
            return Color.FromArgb(1, (int) Math.Floor(r*255), (int) Math.Floor(g*255), (int) Math.Floor(b*255));
        }

        /*
         * # Converts a RGB color value to the XYZ color space. Formulas
          # are based on http://en.wikipedia.org/wiki/SRGB assuming that
          # RGB values are sRGB.
          #
          # Assumes r, g, and b are contained in the set [0, 255] and
          # returns x, y, and z.
          #
          # @param   [Number]  r       The red color value
          # @param   [Number]  g       The green color value
          # @param   [Number]  b       The blue color value
          # @return  [Array]           The XYZ representation
         */

        public static Color RgbToXyz(Color color)
        {
            double r = color.R/255, g = color.G/255, b = color.B/255;

            if (r > 0.04045)
                r = Math.Pow((r + 0.055)/1.055, 2.4);
            else
                r /= 12.92;

            if (g > 0.04045)
                g = Math.Pow((g + 0.055)/1.055, 2.4);
            else
                g /= 12.92;

            if (b > 0.04045)
                b = Math.Pow((b + 0.055)/1.055, 2.4);
            else
                b /= 12.92;
            double x = r*0.4124 + g*0.3576 + b*0.1805;
            double y = r*0.2126 + g*0.7152 + b*0.0722;
            double z = r*0.0193 + g*0.1192 + b*0.9505;
            return Color.FromArgb(1, (int) x*100, (int) y*100, (int) z*100);
        }

        /*
         * # Converts a XYZ color value to the sRGB color space. Formulas
          # are based on http://en.wikipedia.org/wiki/SRGB and the resulting
          # RGB value will be in the sRGB color space.
          # Assumes x, y and z values are whatever they are and returns
          # r, g and b in the set [0, 255].
          #
          # @param   [Number]  x       The X value
          # @param   [Number]  y       The Y value
          # @param   [Number]  z       The Z value
          # @return  [Array]           The RGB representation
         */

        public static Color XyzToRgb(Color xyz)
        {
            double x = xyz.R/100, y = xyz.G/100, z = xyz.B/100;


            double r = (3.2406*x) + (-1.5372*y) + (-0.4986*z);
            double g = (-0.9689*x) + (1.8758*y) + (0.0415*z);
            double b = (0.0557*x) + (-0.2040*y) + (1.0570*z);

            if (r > 0.0031308)
                r = (1.055*Math.Pow(r, 0.4166666667)) - 0.055;
            else
                r *= 12.92;

            if (g > 0.0031308)
                g = (1.055*Math.Pow(g, 0.4166666667)) - 0.055;
            else
                g *= 12.92;

            if (b > 0.0031308)
                b = (1.055*Math.Pow(b, 0.4166666667)) - 0.055;
            else
                b *= 12.92;

            return Color.FromArgb(1, (int) r*255, (int) g*255, (int) b*255);
        }

        /*
         # Converts a XYZ color value to the CIELAB color space. Formulas
              # are based on http://en.wikipedia.org/wiki/Lab_color_space
              # The reference white point used in the conversion is D65.
              # Assumes x, y and z values are whatever they are and returns
              # L*, a* and b* values
              #
              # @overload xyzToLab(x, y, z)
              #   @param   [Number]  x       The X value
              #   @param   [Number]  y       The Y value
              #   @param   [Number]  z       The Z value
              #
              # @overload xyzToLab(xyz)
              #   @param [Object] xyz The XYZ object.
              #   @option xyz [Number] x The X value.
              #   @option xyz [Number] y The Y value.
              #   @option xyz [Number] z The z value.
              #
              # @return [Array] The Lab representation
                     */

        public static Color XyzToLab(Color xyz)
        {
            double x = xyz.R, y = xyz.G, z = xyz.B;

            double whiteX = 95.047;
            double whiteY = 100.0;
            double whiteZ = 108.883;

            x /= whiteX;
            y /= whiteY;
            z /= whiteZ;

            if (x > 0.008856451679)
                x = Math.Pow(x, 0.3333333333);
            else
                x = (7.787037037*x) + 0.1379310345;

            if (y > 0.008856451679)
                y = Math.Pow(y, 0.3333333333);
            else
                y = (7.787037037*y) + 0.1379310345;

            if (z > 0.008856451679)
                z = Math.Pow(z, 0.3333333333);
            else
                z = (7.787037037*z) + 0.1379310345;

            double l = 116*y - 16;
            double a = 500*(x - y);
            double b = 200*(y - z);
            return Color.FromArgb(1, (int) l, (int) a, (int) b);
        }

        /*
         * # Converts a L*, a*, b* color values from the CIELAB color space
          # to the XYZ color space. Formulas are based on
          # http://en.wikipedia.org/wiki/Lab_color_space
          #
          # The reference white point used in the conversion is D65.
          # Assumes L*, a* and b* values are whatever they are and returns
          # x, y and z values.
          #
          # @overload labToXYZ(l, a, b)
          #   @param   [Number]  l       The L* value
          #   @param   [Number]  a       The a* value
          #   @param   [Number]  b       The b* value
          #
          # @overload labToXYZ(lab)
          #   @param [Object] lab The LAB values
          #   @option lab [Number] l The L* value.
          #   @option lab [Number] a The a* value.
          #   @option lab [Number] b The b* value.
          #
          # @return  [Array]           The XYZ representation
         */

        public static Color LabToXyz(Color lab)
        {
            double a = lab.R, b = lab.G, l = lab.B;
            double y = (l + 16)/116;
            double x = y + (a/500);
            double z = y - (b/200);

            if (x > 0.2068965517)
                x = x*x*x;
            else
                x = 0.1284185493*(x - 0.1379310345);

            if (y > 0.2068965517)
                y = y*y*y;
            else
                y = 0.1284185493*(y - 0.1379310345);

            if (z > 0.2068965517)
                z = z*z*z;
            else
                z = 0.1284185493*(z - 0.1379310345);

            //# D65 reference white point

            return Color.FromArgb(1, (int) (x*95.047), (int) (y*100.0), (int) (z*108.883));
        }

        /*
         * # Converts L*, a*, b* back to RGB values.
            #
            # @see Convert.rgbToXYZ
            # @see Convert.xyzToLab
         */

        public static Color RgbToLab(Color color)
        {
            double r = color.R, g = color.G, b = color.B;

            Color xyz = RgbToXyz(Color.FromArgb(1, (int) r, (int) g, (int) b));
            return XyzToLab(xyz);
        }

        public static Color LabToRgb(Color color)
        {
            throw new NotImplementedException();
        }
    }
}