using System.Drawing;
using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Filters.Vignettes
{
    public class RectangularVignette : FilterBase
    {
        private readonly float _coordsBottom;
        private readonly float _coordsLeft;
        private readonly float _coordsRight;
        private readonly float _coordsTop;

        private readonly PointF[] _corners = new PointF[4];
        private readonly double _maxDist;
        private readonly VignetteOption _options;
        private Size _dimension;

        public RectangularVignette(Size ImageSize, VignetteOption Options)
        {
            _dimension = ImageSize;
            _options = Options;
            int percent = Options.Size.Width/100;

            _options.Size = new Size(_dimension.Width*percent, _dimension.Height*percent);

            _options.Strength /= 100;
            _coordsLeft = (_dimension.Width - Options.Size.Width)/2;
            _coordsRight = _dimension.Width - _coordsLeft;
            _coordsBottom = (_dimension.Height - Options.Size.Height)/2;
            _coordsTop = (_dimension.Height - _coordsBottom);

            _corners[0] = new PointF(_coordsLeft + Options.CornorRadius, _coordsTop - Options.CornorRadius);
            _corners[1] = new PointF(_coordsRight + Options.CornorRadius, _coordsTop - Options.CornorRadius);
            _corners[2] = new PointF(_coordsRight + Options.CornorRadius, _coordsBottom - Options.CornorRadius);
            _corners[3] = new PointF(_coordsLeft + Options.CornorRadius, _coordsBottom - Options.CornorRadius);

            _maxDist = Calculate.Distance(new Point(0, 0), new Point((int) _corners[3].X, (int) _corners[3].Y)) -
                       Options.CornorRadius;
        }

        public override Color Process(Color layerColor, Point point)
        {
            Point loc = point;
            // Trivial rejects
            if ((loc.X > _corners[0].X && loc.X < _corners[1].X) && (loc.Y > _coordsBottom && loc.Y < _coordsTop))
                return layerColor;
            if ((loc.X > _coordsLeft && loc.X < _coordsRight) && (loc.Y > _corners[3].Y && loc.Y < _corners[2].Y))
                return layerColor;

            double amt = 0;
            double radialDist = 0;
            // Need to figure out which section we're in. First, the easy ones, then the harder ones.
            if (loc.X > _corners[0].X && loc.X < _corners[1].X && loc.Y > _coordsTop)
                // top-middle section
                amt = (loc.Y - _coordsTop)/_maxDist;
            else if (loc.Y > _corners[2].Y && loc.Y < _corners[1].Y && loc.X > _coordsRight)
                // right-middle section
                amt = (loc.X - _coordsRight)/_maxDist;
            else if (loc.X > _corners[0].X && loc.X < _corners[1].X && loc.Y < _coordsBottom)
                // bottom-middle section
                amt = (_coordsBottom - loc.Y)/_maxDist;
            else if (loc.Y > _corners[2].Y && loc.Y < _corners[1].Y && loc.X < _coordsLeft)
                // left-middle section
                amt = (_coordsLeft - loc.X)/_maxDist;
            else if (loc.X <= _corners[0].X && loc.Y >= _corners[0].Y)
            {
                // top-left corner
                radialDist = Calculate.Distance(loc, new Point((int) _corners[0].X, (int) _corners[0].Y));
                amt = (radialDist - _options.CornorRadius)/_maxDist;
            }
            else if (loc.X >= _corners[1].X && loc.Y >= _corners[1].Y)
            {
                // top-right corner
                radialDist = Calculate.Distance(loc, new Point((int) _corners[1].X, (int) _corners[1].Y));
                amt = (radialDist - _options.CornorRadius)/_maxDist;
            }
            else if (loc.X >= _corners[2].X && loc.Y <= _corners[2].Y)
            {
                // bottom-right corner
                radialDist = Calculate.Distance(loc, new Point((int) _corners[2].X, (int) _corners[2].Y));
                amt = (radialDist - _options.CornorRadius)/_maxDist;
            }
            else if (loc.X <= _corners[3].X && loc.Y <= _corners[3].Y)
            {
                // bottom-left corner
                radialDist = Calculate.Distance(loc, new Point((int) _corners[3].X, (int) _corners[3].Y));
                amt = (radialDist - _options.CornorRadius)/_maxDist;
            }
            if (amt < 0)
                return layerColor;
            switch (_options.VignetteFilter)
            {
                case VignetteFilterEnum.Brightness:
                    return VignetterFilters.Brightness(layerColor, (float) amt, _options.Strength);

                case VignetteFilterEnum.Gamma:
                    return VignetterFilters.Gamma(layerColor, (float) amt, _options.Strength);

                case VignetteFilterEnum.Colorize:
                    return VignetterFilters.Gamma(layerColor, (float) amt, _options.Strength);
                default:
                    return VignetterFilters.Brightness(layerColor, (float) amt, _options.Strength);
            }
        }
    }
}