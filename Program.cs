using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

class Program
{
    static void Main()
    {
        // Load the JPG file into a Bitmap object
        using (Bitmap originalBmp = (Bitmap)Image.FromFile("input.svg"))
        {
            // Create a new Bitmap with a non-indexed pixel format
            Bitmap bmp = new Bitmap(originalBmp.Width, originalBmp.Height, PixelFormat.Format32bppArgb);

            // Create a Graphics object for the new Bitmap
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Draw the original image onto the new Bitmap
                g.DrawImage(originalBmp, 0, 0, originalBmp.Width, originalBmp.Height);

                // Create a circular mask
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(20, 20, 300, 300); // Adjust the position and size as needed
                Region region = new Region(path);
                g.SetClip(region, CombineMode.Replace);

                // Fill the ring-shaped region with a background color (white in this example)
                FillRing(g, Brushes.White, 170, 170, 150, 100); // Adjust the inner radius as needed

                // Set the Font property to the desired font
                Font font = new Font("Arial", 30, FontStyle.Regular);

                // Set the center and radius of the circle
                float centerX = 170; // Adjust the X-coordinate of the center
                float centerY = 170; // Adjust the Y-coordinate of the center
                float radius = 150; // Adjust the radius

                // Iterate through each letter and draw it facing towards the top
                for (int i = 0; i < "Hello, Wor ld!".Length; i++)
                {
                    // Calculate the position for the current letter
                    float angle = i * (360f / "Hello, World!".Length);
                    float x = centerX + radius * (float)Math.Cos(angle * Math.PI / 180);
                    float y = centerY + radius * (float)Math.Sin(angle * Math.PI / 180);

                    // Calculate the rotation angle to face towards the top
                    float rotationAngle = angle + 90;

                    // Draw the new rotated text on the Bitmap
                    g.TranslateTransform(x, y);
                    g.RotateTransform(rotationAngle);
                    g.DrawString("Hello, Wor ld!"[i].ToString(), font, Brushes.Black, -font.Size / 2, 0);
                    g.ResetTransform(); // Reset the transformation for the next letter
                }

                // Save the modified Bitmap
                bmp.Save("output.jpg", ImageFormat.Jpeg);
            }
        }
    }

    // Function to fill a ring-shaped region with a background color
    static void FillRing(Graphics g, Brush brush, float x, float y, float outerRadius, float innerRadius)
    {
        RectangleF outerRect = new RectangleF(x - outerRadius, y - outerRadius, 2 * outerRadius, 2 * outerRadius);
        RectangleF innerRect = new RectangleF(x - innerRadius, y - innerRadius, 2 * innerRadius, 2 * innerRadius);

        GraphicsPath path = new GraphicsPath();
        path.AddEllipse(outerRect);
        path.AddEllipse(innerRect);

        Region region = new Region(path);
        g.FillRegion(brush, region);
    }
}
