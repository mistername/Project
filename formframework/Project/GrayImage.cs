using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCsharp
{
    class GrayImage : IChannelImage
    {
        readonly byte[,] _imageData;
        public GrayImage(Bitmap bitmap)
        {
            _imageData = new byte[bitmap.Width, bitmap.Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    var graypixel = (pixel.R + pixel.G + pixel.B) / 3;
                    _imageData[x, y] = (byte)MyMath.Clamp(graypixel, byte.MinValue, byte.MaxValue);
                }
            }
        }

        public GrayImage(params IChannelImage[] images)
        {
            if (images.Length == 0)
            {
                throw new ArgumentException("Requires at least one image");
            }

            if (images.Length != 1)
            {
                var checkimage = images[0];
                if (images.Any((image) => image.Height != checkimage.Height || image.Width != checkimage.Width))
                {
                    throw new ArgumentException("images must be of same height and width");
                }
            }

            _imageData = new byte[images[0].Width, images[0].Height];

            for (int x = 0; x < images[0].Width; x++)
            {
                for (int y = 0; y < images[0].Height; y++)
                {
                    _imageData[x,y] = (byte)images.Average((image) => (double)image.GetPixel(x, y));
                }
            }
        }

        public GrayImage(int width, int height)
        {
            _imageData = new byte[width, height];
        }

        public int Width
        {
            get
            {
               return _imageData.GetLength(0);
            }
        }

        public int Height
        {
            get
            {
                return _imageData.GetLength(1);
            }
        }

        public Bitmap GetBitmap()
        {
            Bitmap bitmap = new Bitmap(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var gray = _imageData[x, y];
                    bitmap.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }

            return bitmap;
        }

        public byte GetPixel(int x, int y)
        {
            return _imageData[x, y];
        }

        public void SetPixel(int x, int y, byte value)
        {
            _imageData[x, y] = value;
        }

        public IChannelImage CreateBlankCopy()
        {
            return new GrayImage(Width, Height);
        }

        byte IChannelImage.GetPixel(int x, int y)
        {
            return GetPixel(x, y);
        }

        void IChannelImage.SetPixel(int x, int y, byte value)
        {
            SetPixel(x, y, value);
        }
    }
}
