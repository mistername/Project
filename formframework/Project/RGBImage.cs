using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace ProjectCsharp
{
    class RGBImage
    {
        readonly Color[,] _imageData;
        public RGBImage(Bitmap bitmap)
        {
            _imageData = new Color[bitmap.Width, bitmap.Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _imageData[x, y] = bitmap.GetPixel(x, y);
                }
            }
        }

        public Color GetPixel(int x, int y)
        {
            return _imageData[x, y];
        }

        public void SetPixel(int x, int y, Color pixel)
        {
            _imageData[x, y] = pixel;
        }

        public int Width => _imageData.GetLength(0);
        public int Height => _imageData.GetLength(1);

        public GrayImage GetGrayImage()
        {
            var output = new GrayImage(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    output.SetPixel(x, y, ColorToByte(_imageData[x, y]));
                }
            }

            return output;
        }

        public BinaryImage GetBinaryImage()
        {
            var output = new BinaryImage(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    output.SetPixel(x, y, ColorToByte(_imageData[x, y]) != 0);
                }
            }

            return output;
        }

        private static byte ColorToByte(Color color)
        {
            return (byte)((color.R + color.G + color.B) / 3);
        }
    }
}
