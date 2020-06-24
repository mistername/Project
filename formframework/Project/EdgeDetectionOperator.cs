using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ProjectCsharp
{
    public class EdgeDetectionOperator
    {
        public struct EdgeOperatorData
        {
            public EdgeOperatorData(int input)
            {
                TrueValue = input;
            }
            public int TrueValue;
            public byte Value => (byte)((byte.MaxValue / 2) + TrueValue);
        }

        private static readonly int[,] _xSobolKernel = { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };
        public static readonly EdgeDetectionOperator XSobol = new EdgeDetectionOperator(_xSobolKernel);
        private static readonly int[,] _ySobolKernel = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
        public static readonly EdgeDetectionOperator YSobol = new EdgeDetectionOperator(_ySobolKernel);
        readonly int[,] _kernel;
        int _totalPoints;
        public EdgeDetectionOperator(in int[,] kernel)
        {
            _kernel = kernel;
            generateTotalValue();
        }

        private void generateTotalValue()
        {
            _totalPoints = 0;
            for (int x = 0; x < _kernel.GetLength(0); x++)
            {
                for (int y = 0; y < _kernel.GetLength(0); y++)
                {
                    _totalPoints += Math.Abs(_kernel[x, y]);
                }
            }
        }

        public EdgeOperatorData[,] ApplyKernel(in IChannelImage myImage)
        {
            var appliedImage = new EdgeOperatorData[myImage.Width, myImage.Height];

            for (int x = 0; x < myImage.Width; x++)
            {
                for (int y = 0; y < myImage.Height; y++)
                {
                    int pixelResult = 0;
                    for (int kx = 0; kx < _kernel.GetLength(0); kx++)
                    {
                        for (int ky = 0; ky < _kernel.GetLength(1); ky++)
                        {
                            var tmpx = MyMath.Clamp(x - (_kernel.GetLength(0) / 2) + kx, 0, myImage.Width-1);
                            var tmpy = MyMath.Clamp(y - (_kernel.GetLength(1) / 2) + ky, 0, myImage.Height-1);
                            pixelResult += _kernel[kx, ky] * myImage.GetPixel(tmpx, tmpy);
                        }
                    }

                    appliedImage[x, y] = new EdgeOperatorData(pixelResult / _totalPoints);
                }
            }

            return appliedImage;
        }
    }
}
