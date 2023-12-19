﻿using System;
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
                        semicircularTextPath.AddArc(new SKRect(42, 83, 217, 220), -165, 180); // Adjust the circle parameters as needed
                        
                        paint.TextSize = 20;

                        svgCanvas.DrawTextOnPath("MELISSA CARDINALS", semicircularTextPath, 0, 0, paint);


                        // Draw semicircular text on the SVG canvas at the top
                        var semicircularTextPath1 = new SKPath();
                        semicircularTextPath1.AddArc(new SKRect(22, 100, 240, 292), 175, -180); // Adjust the circle parameters as needed

                        paint.TextSize = 20;

                        // Adjust the offset (0, 0) based on your desired position
                        svgCanvas.DrawTextOnPath("2023 4A D1 STATE CHAMPIONS", semicircularTextPath1, 0, 0, paint);


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
