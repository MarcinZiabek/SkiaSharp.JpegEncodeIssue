using System.IO;
using SkiaSharp;

void Example1()
{
    using var image = SKImage.FromEncodedData("input1.png");
    using var data = image.Encode(SKEncodedImageFormat.Jpeg, 90);
    File.WriteAllBytes("output1.jpg", data.ToArray());
}

void Example2()
{
    using var image = SKImage.FromEncodedData("input2.jpg");
    using var data = image.Encode(SKEncodedImageFormat.Png, 100);
    File.WriteAllBytes("output2.png", data.ToArray());
}

void Example3(SKEncodedImageFormat format)
{
    var imageInfo = new SKImageInfo(400, 300);
    using var surface = SKSurface.Create(imageInfo);
    surface.Canvas.Clear(SKColors.White);
        
    using var shaderPaint = new SKPaint
    {
        Shader = SKShader.CreateRadialGradient(
            new SKPoint(200, 150), 300,
            new[] { SKColors.DeepSkyBlue, SKColors.Turquoise }, new[] {0, 1f},
            SKShaderTileMode.Decal)
    };
            
    surface.Canvas.DrawRect(0, 0, imageInfo.Width, imageInfo.Height, shaderPaint);
            
    // return result as an image
    using var image = surface.Snapshot();
    using var data = image.Encode(format, 90);
    File.WriteAllBytes($"output3.{format.ToString().ToLower()}", data.ToArray());
}

// IMPORTANT!
// on windows 11 everything works correctly
// problems (image artifacts when encoding SkImage as JPEG file) are present on MacOS Sonoma 14.0.0, dotnet 7.0.401

// broken: loading image as JPEG, saving as PNG
Example1();

// works: loading image as PNG, saving as JPEG
Example2();

// broken: generating image in runtime and saving as JPEG
Example3(SKEncodedImageFormat.Jpeg);

// works: generating image in runtime and saving as PNG
Example3(SKEncodedImageFormat.Png);

