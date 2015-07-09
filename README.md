##The port of Camanjs on .NET

##How to use it
###Vignette Effect
```c#
string input = @"C:\in.jpg";
string output = @"C:\out.jpg";

Image image = Image.FromFile(input);

var canman = new Canman(image as Bitmap, output, new Normal());
canman.Add(new Vignette(image.Size, 40, 30));

canman.Render();
```
###Multiple Layer Effects
```c#
string input = @"C:\in.jpg";
string output = @"C:\out.jpg";

Image image = Image.FromFile(input);

var canman = new CamanEffects(image as Bitmap,output, new Normal());
canman.Add(new Sharpen(40));
canman.RenderBitmap();
canman.Add(EffectsEnum.OrangePeel);
canman.CurrentLayer.BlendingMode = new Multiply();
canman.NewLayer(new ConcentrateOverlay(), new Multiply(), 80);
canman.ExecuteLayer(new Layer(canman, canman.SourceImage, new Multiply()));
canman.RenderBitmap();
canman.Add(new ConcentrateOverlay());
canman.Add(new Sharpen(5));
canman.RenderBitmap();
canman.Add(new Brightness(10));
canman.Render();
```
