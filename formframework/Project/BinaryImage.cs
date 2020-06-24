using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ProjectCsharp
{
    public class BinaryImage
    {
        readonly bool[,] _points;

        public BinaryImage(int width, int height)
        {
            _points = new bool[width, height];
        }

        public BinaryImage(IChannelImage image)
        {
            _points = new bool[image.Width, image.Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _points[x, y] = image.GetPixel(x, y) != 0;
                }
            }
        }

        public BinaryImage Invert()
        {
            var copy = new BinaryImage(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    copy.SetPixel(x, y, !this.GetPixel(x, y));
                }
            }

            return copy;
        }

        public bool GetPixel(int x, int y) => _points[x, y];
        public void SetPixel(int x, int y, bool value) => _points[x, y] = value;

        public int Width => _points.GetLength(0);
        public int Height => _points.GetLength(1);

        public BinaryImage Copy()
        {
            var copy = new BinaryImage(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    copy.SetPixel(x, y, this.GetPixel(x, y));
                }
            }

            return copy;
        }

        public Bitmap GetBitmap()
        {
            var btmp = new Bitmap(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (GetPixel(x,y))
                    {
                        btmp.SetPixel(x, y, Color.White);
                    }
                }
            }

            return btmp;
        }


        public BinaryImage FindShapes()
        {
            var output = new BinaryImage(Width, Height);
            
            Queue<(int, int)> queue = new Queue<(int, int)>();

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    while (queue.Count != 0)
                    {
                        var cell = queue.Dequeue();
                        CheckCell(output,cell.Item1, cell.Item2, queue);
                    }

                    CheckCell(output,x, y, queue);
                }
            }

            return output;
        }

        private void CheckCell(BinaryImage output,int x, int y, Queue<(int, int)> queue)
        {
            if (!this.GetPixel(x, y))
            {
                if (!output.GetPixel(x, y))
                {
                    if (NeighborsEmpty(x, y))
                    {
                        output.SetPixel(x, y, true);
                        if (x != 0)
                        {
                            queue.Enqueue((x - 1, y));
                        }
                        if (x != output.Width - 1)
                        {
                            queue.Enqueue((x + 1, y));
                        }
                        if (y != 0)
                        {
                            queue.Enqueue((x, y - 1));
                        }
                        if (y != output.Height - 1)
                        {
                            queue.Enqueue((x, y + 1));
                        }

                        return;
                    }
                }
            }

            return;
        }

        private bool NeighborsEmpty(int x, int y, int range = 2)
        {
            for (int xt = x - range; xt <= x + range; xt++)
            {
                for (int yt = y - range; yt <= y + range; yt++)
                {
                    var xd = MyMath.Clamp(xt, 0, Width - 1);
                    var yd = MyMath.Clamp(yt, 0, Height - 1);

                    if (this.GetPixel(xd, yd))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
