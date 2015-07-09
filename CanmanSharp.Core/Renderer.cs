using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace CanmanSharp.Core
{
    public delegate void RendererCompleted(object sender);

    public class Renderer
    {
        private readonly Canman _canman;
        private readonly Queue<JobFactory> _renderQueue = new Queue<JobFactory>();
        private JobFactory _currentJob;

        public Renderer(Canman Canman)
        {
            _canman = Canman;
        }

        public event RendererCompleted RendererCompletedEvent = null;

        public void Add(JobFactory job)
        {
            if (job == null)
                throw new ArgumentNullException();
            _renderQueue.Enqueue(job);
        }

        private void ProcessNext()
        {
            if (_renderQueue.Count == 0)
            {
                if (RendererCompletedEvent != null)
                    RendererCompletedEvent(this);
                return;
            }

            _currentJob = _renderQueue.Dequeue();
            switch (_currentJob.Type)
            {
                case FilterTypes.LayerDequeue:
                    Layer layer = _canman.LayerQueue.Dequeue();
                    _canman.ExecuteLayer(layer);
                    ProcessNext();
                    break;
                case FilterTypes.LoadOverlay:
                    LoadOverlay(_canman.CurrentLayer.Bitmap, _currentJob.Src);
                    break;
                case FilterTypes.LayerFinished:
                    _canman.ApplyCurrentLayer();
                    _canman.PopContext();
                    ProcessNext();
                    break;
                case FilterTypes.Plugin:
                    ExecutePlugin();
                    break;
                default:
                    ExecutedFilter();
                    break;
            }
        }

        private void ExecutePlugin()
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            ProcessNext();
        }

        private void ExecutedFilter()
        {
            if (_currentJob.Type == FilterTypes.Single)
                RenderFilter();
            else
                RenderKenel();

            ProcessNext();
        }

        /// <summary>
        ///     Renders a single block of the canvas with the current filter function
        /// </summary>
        private void RenderFilter()
        {
            int width = _canman.CurrentLayer.Bitmap.Width;
            int height = _canman.CurrentLayer.Bitmap.Height;

            BitmapData pbits = _canman.CurrentLayer.Bitmap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            //declare an array to hold the bytes of the bitmap
            int bytes = pbits.Stride*height;
            var rgbValues = new byte[bytes];

            //Copy the RGB values into the array
            Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

            for (int i = 0; i < rgbValues.Length; i += 4)
            {
                var filter = _currentJob as FilterBase;
                Color color = Color.FromArgb(rgbValues[i + 3], rgbValues[i + 2], rgbValues[i + 1], rgbValues[i]);
                if (filter != null)
                {
                    int x = (i/4)%width;

                    int y = ((i/4) - x)/width;
                    Color result = filter.Process(color, new Point(x, y));
                    rgbValues[i + 3] = result.A;
                    rgbValues[i + 2] = result.R;
                    rgbValues[i + 1] = result.G;
                    rgbValues[i] = result.B;
                }
            }

            //for (int i = 0; i < width; i++)
            //{
            //    for (int j = 0; j < height; j++)
            //    {

            //        FilterBase filter=_currentJob as FilterBase;

            //        rgb = j * pbits.Stride + 3 * i;

            //        var color = Color.FromArgb(255, rgbValues[rgb + 2], rgbValues[rgb + 1], rgbValues[rgb]);
            //        var result = filter.Process(color, new Point(i, j));
            //        //rgbValues[rgb + 3] = result.A;
            //        rgbValues[rgb + 2] = result.R;
            //        rgbValues[rgb + 1] = result.G;
            //        rgbValues[rgb] = result.B;
            //    }
            //}
            // Copy the RGB values back to the bitmap.
            Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);
            // Release image bits.
            _canman.CurrentLayer.Bitmap.UnlockBits(pbits);
        }

        private void RenderKenel()
        {
            string name = _currentJob.Name;

            double[,] adjust = _currentJob.Adjust;
            double bias = _currentJob.Bias;
            double divisor = _currentJob.Divisor;

            int width = _canman.CurrentLayer.Bitmap.Width;
            int height = _canman.CurrentLayer.Bitmap.Height;
            double adjustSize = Math.Sqrt(adjust.Length);

            var result = new Color[width, height];
            //http://stackoverflow.com/questions/903632/sharpen-on-a-bitmap-using-c-sharp

            //Lock image bits for read/write
            BitmapData pbits = _canman.CurrentLayer.Bitmap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            //declare an array to hold the bytes of the bitmap
            int bytes = pbits.Stride*height;
            var rgbValues = new byte[bytes];

            //Copy the RGB values into the array
            Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

            int rgb;
            int s = (int) adjustSize/2;
            //Fill the color array with the new sharpened color values.
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    rgb = y*pbits.Stride + 3*x;
                    result[x, y] = Color.FromArgb(rgbValues[rgb + 2], rgbValues[rgb + 1], rgbValues[rgb]);
                    double red = 0, green = 0, blue = 0;
                    for (int filterX = 0; filterX < adjustSize; filterX++)
                    {
                        for (int filterY = 0; filterY < adjustSize; filterY++)
                        {
                            int imageX = (x - s + filterX + width)%width;
                            int imageY = (y - s + filterY + height)%height;

                            rgb = imageY*pbits.Stride + 3*imageX;

                            red += rgbValues[rgb + 2]*adjust[filterX, filterY];
                            green += rgbValues[rgb + 1]*adjust[filterX, filterY];
                            blue += rgbValues[rgb + 0]*adjust[filterX, filterY];
                        }
                    }
                    int r = Utils.ClampRGB((int) Math.Floor((divisor*red + bias)));
                    int g = Utils.ClampRGB((int) Math.Floor((divisor*green + bias)));
                    int b = Utils.ClampRGB((int) Math.Floor((divisor*blue + bias)));

                    result[x, y] = Color.FromArgb(r, g, b);
                }
            }

            // Update the image with the sharpened pixels.
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    rgb = y*pbits.Stride + 3*x;

                    rgbValues[rgb + 2] = result[x, y].R;
                    rgbValues[rgb + 1] = result[x, y].G;
                    rgbValues[rgb + 0] = result[x, y].B;
                }
            }

            // Copy the RGB values back to the bitmap.
            Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);
            // Release image bits.
            _canman.CurrentLayer.Bitmap.UnlockBits(pbits);
        }

        public void LoadOverlay(Bitmap img, string src)
        {
            
        }
    }
}