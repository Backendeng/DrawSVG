using System;
using System.IO;
using SkiaSharp;
using Svg.Skia;

class TestSVG
{
    static void Main()
    {
        // Load the existing SVG document using SkiaSharp
        var skSvg = new SKSvg();
        skSvg.Load("input.svg");

        // Ensure that the SKSvg object has a valid picture
        if (skSvg.Picture != null)
        {
            // Create an SKPaint object for styling the text
            var paint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 12,
                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright),
            };

            // Create a new SKBitmap to draw the SVG on
            using (var bitmap = new SKBitmap((int)skSvg.Picture.CullRect.Width, (int)skSvg.Picture.CullRect.Height))
            {
                using (var canvas = new SKCanvas(bitmap))
                {
                    // Draw the SVG on the canvas
                    canvas.DrawPicture(skSvg.Picture);

                    // Draw the text on the canvas
                    canvas.DrawText("Hello, SVG!", 10, 40, paint);
                }

                // Save the modified SKBitmap as an SVG file
                using (var stream = new FileStream("modified.svg", FileMode.Create))
                {
                    using (var svgCanvas = SKSvgCanvas.Create(SKRect.Create(0, 0, bitmap.Width, bitmap.Height), stream))
                    {
                        svgCanvas.DrawPicture(skSvg.Picture);
                        svgCanvas.DrawText("Hello, SVG!", 10, 40, paint);
                    }
                }

                Console.WriteLine("Text added to the existing SVG file successfully.");
            }
        }
        else
        {
            Console.WriteLine("Error: Unable to load SVG file.");
        }
    }
}
