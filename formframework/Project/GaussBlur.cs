using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCsharp
{
    public static class MyMath
    {
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }
    }

    /// <summary>
    /// class for guassbluring
    /// </summary>
    public static class GaussBlur
    {
        private static readonly byte[,] kernel = { { 2, 4, 5, 4, 2 }, { 4, 9, 12, 9, 4 }, { 5, 12, 15, 12, 5 }, { 4, 9, 12, 9, 4 }, { 2, 4, 5, 4, 2 } };
        private static int sumKernel;

        private static int GetSumKernel()
        {
            if (sumKernel != default)
            {
                return sumKernel;
            }

            for (int x = 0; x < kernel.GetLength(0); x++)
            {
                for (int y = 0; y < kernel.GetLength(1); y++)
                {
                    sumKernel += kernel[x, y];
                }
            }

            return sumKernel;
        }

        public static IChannelImage Filter<T>(T image) where T : IChannelImage
        {
            var kernelxmin = -(kernel.GetLength(0) - 1)/ 2;
            var kernelxmax = (kernel.GetLength(0) / 2);
            var kernelymin = -(kernel.GetLength(1) - 1) / 2;
            var kernelymax = (kernel.GetLength(1) / 2);
            var output = image.CreateBlankCopy();

            for (int x = 0; x < output.Width; x++)
            {
                for (int y = 0; y < output.Height; y++)
                {
                    var total = 0;
                    for (int xt = kernelxmin; xt <= kernelxmax; xt++)
                    {
                        for (int yt = kernelymin; yt <= kernelymax; yt++)
                        {
                            var xtk = MyMath.Clamp(xt+x, 0, output.Width - 1);
                            var ytk = MyMath.Clamp(yt+y, 0, output.Height - 1);

                            var pixel = image.GetPixel(xtk, ytk);
                            total += kernel[xt-kernelxmin, yt-kernelymin] * pixel;
                        }
                    }

                    var average = total / GetSumKernel();

                    output.SetPixel(x, y, (byte)average);
                }
            }

            return output;
        }
    }
}
