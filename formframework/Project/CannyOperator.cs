using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCsharp
{
    /// <summary>
    /// class for getting edges out of images
    /// </summary>
    class CannyOperator
    {
        public CannyOperator(byte highThreshold, byte lowThreshold)
        {
            _highThreshhold = highThreshold;
            _lowThreshhold = lowThreshold;
        }

        readonly byte _highThreshhold;
        readonly byte _lowThreshhold;
        public SobolOutput filter(SobolOutput sobolOutput)
        {
            var edgefiltered = FilterNeighbors(sobolOutput);
            return CheckThreshold(edgefiltered);
        }

        private SobolOutput CheckThreshold(SobolOutput input)
        {
            var output = new SobolOutput(input.Width, input.Height);
            for (int x = 0; x < output.Width; x++)
            {
                for (int y = 0; y < output.Height; y++)
                {
                    var pixel = input.GetPixel(x, y);
                    if (pixel.WeightStrength >= _highThreshhold)
                    {
                        output.SetPixel(x, y, pixel);

                        CheckWeakNeighbors(x, y, input, output);
                    }
                }
            }

            //for (int x = 0; x < output.Width; x++)
            //{
            //    for (int y = 0; y < output.Height; y++)
            //    {
            //        if (output.GetPixel(x,y).Strength != 0)
            //        {
            //            output.SetPixel(x, y, new SobolOutput.pixelSobolData(byte.MaxValue, byte.MaxValue));
            //        }
            //    }
            //}

            return output;
        }

        private void CheckWeakNeighbors(int x, int y, SobolOutput input, SobolOutput output)
        {
            for (int xt = x-1; xt <= 1; xt++)
            {
                for (int yt = y-1; yt <= 1; yt++)
                {
                    if (xt == x && yt == y)
                        continue;

                    if (xt < 0 || xt >= output.Width || yt < 0 || yt >= output.Height)
                        continue;

                    var currentPixel = output.GetPixel(xt, yt);
                    if (currentPixel.X != 0.0 || currentPixel.Y != 0.0)
                        continue;

                    var pixel = input.GetPixel(xt, yt);
                    if (pixel.WeightStrength >= _lowThreshhold && pixel.WeightStrength < _highThreshhold)
                    {
                        output.SetPixel(xt, yt, pixel);
                        CheckWeakNeighbors(xt, yt, input, output);
                    }
                }
            }
        }

        private SobolOutput FilterNeighbors(SobolOutput input)
        {
            var output = new SobolOutput(input.Width, input.Height);

            for (int x = 0; x < input.Width; x++)
            {
                for (int y = 0; y < input.Height; y++)
                {
                    var pixel = input.GetPixel(x, y);
                    var angle = pixel.Angle;
                    (SobolOutput.pixelSobolData, SobolOutput.pixelSobolData) neighbors = default;

                    //should do interpolation
                    if (angle >= 67.5 || angle < -67.5)
                    {
                        neighbors = getUpDown(input, x, y);
                    }
                    else if (angle >= 22.5)
                    {
                        neighbors = getRightUp(input, x, y);
                    }
                    else if (angle >= -22.5)
                    {
                        neighbors = getRight(input, x, y);

                    }
                    else if (angle >= -67.5)
                    {
                        neighbors = GetRightDown(input, x, y);
                    }

                    if (neighbors.Item1.WeightStrength > pixel.WeightStrength && neighbors.Item2.WeightStrength > pixel.WeightStrength)
                    {
                        continue;
                    }

                    output.SetPixel(x, y, pixel);
                }
            }

            return output;
        }

        private (SobolOutput.pixelSobolData, SobolOutput.pixelSobolData) getRightUp(SobolOutput input, int x, int y)
        {
            var upright = input.GetPixel(MyMath.Clamp(x + 1, 0, input.Width - 1), MyMath.Clamp(y - 1, 0, input.Height - 1));
            var rightleft = input.GetPixel(MyMath.Clamp(x - 1, 0, input.Width - 1), MyMath.Clamp(y + 1, 0, input.Height - 1));

            return (upright, rightleft);
        }

        private (SobolOutput.pixelSobolData, SobolOutput.pixelSobolData) getRight(SobolOutput input, int x, int y)
        {
            var right = input.GetPixel(MyMath.Clamp(x + 1, 0, input.Width - 1), y);
            var left = input.GetPixel(MyMath.Clamp(x - 1, 0, input.Width - 1), y);
            return (right, left);
        }

        private (SobolOutput.pixelSobolData, SobolOutput.pixelSobolData) GetRightDown(SobolOutput input, int x, int y)
        {
            var right = input.GetPixel(MyMath.Clamp(x + 1, 0, input.Width - 1), MyMath.Clamp(y + 1, 0, input.Height - 1));
            var left = input.GetPixel(MyMath.Clamp(x - 1, 0, input.Width - 1), MyMath.Clamp(y - 1, 0, input.Height - 1));

            return (right, left);
        }

        private (SobolOutput.pixelSobolData, SobolOutput.pixelSobolData) getUpDown(SobolOutput input, int x, int y)
        {
            var up = input.GetPixel(x, MyMath.Clamp(y + 1, 0, input.Height - 1));
            var down = input.GetPixel(x, MyMath.Clamp(y - 1, 0, input.Height - 1));
            return (up, down);
        }
    }
}
