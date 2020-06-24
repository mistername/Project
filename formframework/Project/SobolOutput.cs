using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCsharp
{
    class SobolOutput
    {
        readonly pixelSobolData[,] _PixelData;

        public struct pixelSobolData
        {
            public int X { get; }
            public int Y { get; }

            public double Angle => Math.Atan((double)Y/ X)*(180/Math.PI);
            public byte Strength => (byte)(Math.Sqrt(X * X + Y * Y)/Math.Sqrt(2));

            private byte WeightX
            {
                get
                {
                    return (byte)((byte.MaxValue / 2) + Math.Abs(X));
                }
            }

            private byte WeightY
            {
                get
                {
                    return (byte)((byte.MaxValue / 2) + Math.Abs(Y));
                }
            }

            public byte WeightStrength => (byte)(Math.Sqrt(WeightX * WeightX + WeightY * WeightY) / Math.Sqrt(2));

            public pixelSobolData(in int sobolXoutput, in int sobolYoutput)
            {
                X = sobolXoutput;
                Y = sobolYoutput;
            }
        }

        public SobolOutput(in IChannelImage channelImage)
        {
            var ximage = EdgeDetectionOperator.XSobol.ApplyKernel(channelImage);
            var yimage = EdgeDetectionOperator.YSobol.ApplyKernel(channelImage);

            _PixelData = new pixelSobolData[channelImage.Width, channelImage.Height];

            for (int x = 0; x < channelImage.Width; x++)
            {
                for (int y = 0; y < channelImage.Height; y++)
                {
                    SetPixel(x,y,ximage[x,y], yimage[x,y]);
                }
            }
        }

        public SobolOutput(in int width, in int height)
        {
            _PixelData = new pixelSobolData[width, height];
        }

        public void SetPixel(in int x, in int y, in EdgeDetectionOperator.EdgeOperatorData Xvalue, in EdgeDetectionOperator.EdgeOperatorData Yvalue)
        {
            _PixelData[x, y] = new pixelSobolData(Xvalue.TrueValue, Yvalue.TrueValue);
        }

        public void SaveInImage(IChannelImage image)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    image.SetPixel(x, y, this.GetPixel(x, y).Strength);
                }
            }
        }

        public void SetPixel(in int x, in int y, in pixelSobolData data)
        {
            _PixelData[x, y] = data;
        }

        public pixelSobolData GetPixel(in int x, in int y) => _PixelData[x, y];

        public int Width => _PixelData.GetLength(0);
        public int Height => _PixelData.GetLength(1);

        public SobolOutput(in IChannelImage ximage, in IChannelImage yimage)
        {
            if (ximage.Width != yimage.Width || ximage.Height != yimage.Height)
            {
                throw new ArgumentException($"Dimensions not equal, {nameof(ximage)}:{ximage.Width}x{ximage.Height} vs {nameof(yimage)}:{yimage.Width}x{yimage.Height}");
            }

            for (int x = 0; x < ximage.Width; x++)
            {
                for (int y = 0; y < ximage.Height; y++)
                {
                    _PixelData[x, y] = new pixelSobolData(ximage.GetPixel(x, y), yimage.GetPixel(x, y));
                }
            }
        }
    }
}
