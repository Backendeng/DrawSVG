using System;
using System.Drawing.Printing;
using System.IO;
using SkiaSharp;
using Svg.Skia;

class TestSVG
{
    static void Main()
    {
        Console.Write("Enter your top text: ");
        string topTxt = Console.ReadLine();

        Console.WriteLine($"You entered: {topTxt}");

        Console.Write("Enter your bottom text: ");
        string bottomTxt = Console.ReadLine();

        Console.WriteLine($"You entered: {bottomTxt}");

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
                Typeface = SKTypeface.FromFamilyName("Verdana", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright),
            };

            // Create a new SKBitmap to draw the SVG on
            using (var bitmap = new SKBitmap((int)skSvg.Picture.CullRect.Width, (int)skSvg.Picture.CullRect.Height))
            {
                using (var canvas = new SKCanvas(bitmap))
                {
                    // Draw the SVG on the canvas
                    canvas.DrawPicture(skSvg.Picture);

                    // Add semicircular text at the top
                    var textPath = new SKPath();
                    textPath.AddArc(new SKRect(50, 50, 150, 150), -180, 180); // Adjust the circle parameters as needed

                    canvas.DrawTextOnPath("Semicircular Text", textPath, 0, 0, paint);
                }

                // Save the modified SKBitmap as an SVG file
                using (var stream = new FileStream("modified.svg", FileMode.Create))
                {
                    using (var svgCanvas = SKSvgCanvas.Create(SKRect.Create(0, 0, bitmap.Width, bitmap.Height), stream))
                    {
                        svgCanvas.DrawPicture(skSvg.Picture);

                        // Draw semicircular text on the SVG canvas at the top
                        var semicircularTextPath = new SKPath();
                        semicircularTextPath.AddArc(new SKRect(32.5f, 82.5f, 228.5f, 280), -180, 180); // Adjust the circle parameters as needed
                        
                        paint.TextSize = 20;


                        // Calculate the width of the text
                        float semicircularTextWidth = paint.MeasureText(topTxt);
                        // Calculate the offset to center the text
                        float semicircularTextOffsetX = 78 - semicircularTextWidth / 3.8f ;

                        svgCanvas.DrawTextOnPath(topTxt, semicircularTextPath, semicircularTextOffsetX, 0, paint);

                        // Draw semicircular text on the SVG canvas at the top
                        var semicircularTextPath1 = new SKPath();
                        semicircularTextPath1.AddArc(new SKRect(21, 80, 242, 292), 180, -180); // Adjust the circle parameters as needed

                        paint.TextSize = 20;

                        // Calculate the width of the text
                        semicircularTextWidth = paint.MeasureText(bottomTxt);

                        // Calculate the offset to center the text
                        semicircularTextOffsetX = 84 - semicircularTextWidth / 4.15f;
                        Console.WriteLine(semicircularTextWidth);

                        // Adjust the offset (0, 0) based on your desired position
                        svgCanvas.DrawTextOnPath(bottomTxt, semicircularTextPath1, semicircularTextOffsetX, 0, paint);


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
