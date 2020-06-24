using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ProjectCsharp
{
    public enum ColorChannel
    {
        R,
        G,
        B
    }
    /// <summary>
    /// class for getting a channel image out of RGB image
    /// </summary>
    class ColorChannelImage : IChannelImage
    {
        readonly RGBImage _backingImage;
        readonly ColorChannel _channel;
        public ColorChannelImage(RGBImage image, ColorChannel channel)
        {
            _backingImage = image;
            _channel = channel;
        }

        public int Height => _backingImage.Height;
        public int Width => _backingImage.Width;

        public IChannelImage CreateBlankCopy()
        {
            return new GrayImage(this.Width, this.Height);
        }

        public void SetPixel(int x, int y, byte value)
        {
            var pixel = _backingImage.GetPixel(x, y);
            switch (_channel)
            {
                case ColorChannel.R:
                    pixel = Color.FromArgb(value, pixel.G, pixel.B);
                    break;
                case ColorChannel.G:
                    pixel = Color.FromArgb(pixel.R, value, pixel.B);
                    break;
                case ColorChannel.B:
                    pixel = Color.FromArgb(pixel.R, pixel.G, value);
                    break;
                default:
                    break;
            }
            _backingImage.SetPixel(x, y, pixel);
        }

        public byte GetPixel(int x, int y)
        {
            var pixel = _backingImage.GetPixel(x, y);
            return _channel switch
            {
                ColorChannel.R => pixel.R,
                ColorChannel.G => pixel.G,
                ColorChannel.B => pixel.B,
                _ => 0,
            };
        }
    }
}
