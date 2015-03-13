using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Drawing.Imaging;

namespace Tagger.Utils
{
    public static class Util
    {
        private static MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

        // Creates MD5 Hash from file.
        public static string Md5FromFile(string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Open);
            byte[] hash = md5.ComputeHash(file);
            file.Close();
            StringBuilder sb = new StringBuilder();
            foreach (byte hex in hash)
                sb.Append(hex.ToString("x2"));
            return sb.ToString();
        }

        // Creates MD5 Hash from byte array.
        public static string Md5FromBytes(byte[] bytes)
        {
            byte[] hash = md5.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte hex in hash)
                sb.Append(hex.ToString("x2"));
            return sb.ToString();
        }

        // Creates a high quality thumbnail respecting image ratio.
        public static Bitmap CreateThumbnail(Image image, int width, int height)
        {
            Bitmap newImage = null;
            double ratio = Math.Max(image.Width / (double)width, image.Height / (double)height);
            int newWidth = Math.Max(1, (int)(image.Width / ratio));
            int newHeight = Math.Max(1, (int)(image.Height / ratio));

            newImage = new Bitmap(newWidth, newHeight);

            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight));
            }
            return newImage;
        }

        public static Bitmap MakeGrayscale(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][] 
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
    }
}
