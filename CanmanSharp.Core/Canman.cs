using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CanmanSharp.Core
{
    public class Canman
    {
        private readonly Analyze _analyzer;
        private readonly Renderer _renderer;
        private string _output;
        private Bitmap _sourceImage;

        public Canman(Bitmap SourceImage, string Output, BlenderBase DefaultBlending)
        {
            _analyzer = new Analyze();
            _renderer = new Renderer(this);
            Initial(SourceImage, Output, DefaultBlending);
        }

        public Canman(Bitmap SourceImage, BlenderBase DefaultBlending)
        {
            Initial(SourceImage, string.Empty, DefaultBlending);
        }


        public Canman(string ImagePath)
        {
                _sourceImage = Image.FromFile(ImagePath) as Bitmap;
        }

        public Bitmap SourceImage
        {
            get { return _sourceImage; }
            set { _sourceImage = value; }
        }

        public Layer CurrentLayer { get; set; }
        public Queue<Layer> LayerQueue { get; set; }

        private void Initial(Bitmap SourceImage, string Output, BlenderBase DefaultBlending)
        {
            _sourceImage = SourceImage;
            _output = Output;
            CurrentLayer = new Layer(this, SourceImage, DefaultBlending);
        }

        private void _renderer_RendererCompletedEvent(object sender)
        {
            var encoderParameters = new EncoderParameters(1);
            ImageCodecInfo jgpEncoder = Utils.GetEncoder(ImageFormat.Jpeg);

            var originalEncoderParameter = new EncoderParameter(Encoder.Quality,
                100L);
            encoderParameters.Param[0] = originalEncoderParameter;
            if (!string.IsNullOrEmpty(_output))
                CurrentLayer.Bitmap.Save(_output, jgpEncoder, encoderParameters);
        }

        public void Add(JobFactory job)
        {
            if (job == null)
                throw new ArgumentNullException();
            if (job.Type == FilterTypes.Kernel)
            {
                if (job.Divisor == 0.0f)
                {
                    job.Divisor = 0;
                    int maxX = job.Adjust.GetUpperBound(0);
                    int maxY = job.Adjust.GetUpperBound(1);
                    for (int i = 0; i <= maxX; i++)
                    {
                        for (int j = 0; j <= maxY; j++)
                        {
                            job.Divisor += job.Adjust[i, j];
                        }
                    }
                }
            }
            _renderer.Add(job);
        }

        public void Render()
        {
            _renderer.RendererCompletedEvent += _renderer_RendererCompletedEvent;
            _renderer.Execute();
        }

        public Bitmap RenderBitmap()
        {
            _renderer.Execute();
            return _sourceImage;
        }

        public void ExecuteLayer(Layer layer)
        {
            PushContext(layer);
        }

        private void PushContext(Layer layer)
        {
            LayerQueue.Enqueue(CurrentLayer);
            CurrentLayer = layer;
        }

        public void NewLayer(FilterBase filters,BlenderBase blender,int opacity)
        {
            //RenderBitmap();
            if (LayerQueue == null)
                LayerQueue = new Queue<Layer>();
            var layer = new Layer(this, Utils.CopyBitmap(this.CurrentLayer.Bitmap), blender) {Opacity = opacity};
            LayerQueue.Enqueue(layer);
            Add(new LayerJob() {Type = FilterTypes.LayerDequeue});
            Add(filters);
            Add(new LayerJob() {Type = FilterTypes.LayerFinished});
        }

        public void ApplyCurrentLayer()
        {
            CurrentLayer.ApplyToParent();
        }

        internal void PopContext()
        {
            CurrentLayer = LayerQueue.Dequeue();
        }
    }
}