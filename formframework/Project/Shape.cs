using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ProjectCsharp
{
    public class Shape
    {
        public static List<Shape> ShapeFromBinary(in BinaryImage image)
        {
            return ShapeToContour(image);
        }

        public static void OrderShapes(List<Shape> shapes)
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                var shape = shapes[i];
                if (shape.Length < 3)
                {
                    continue;
                }

                for (int j = 0; j < shapes.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var testshape = shapes[j];
                    if(shape.IsShapeInShape(testshape))
                    {
                        shape.AddShape(testshape);
                        shapes[j] = new Shape();
                        OrderShapes(shape._insideShapes);
                    }
                }
            }

            for (int i = 0; i < shapes.Count; i++)
            {
                if (shapes[i].Length < 3)
                {
                    shapes.RemoveAt(i--);
                }
            }
        }

        private bool IsShapeInShape(Shape testshape)
        {
            for (int h = 0; h < testshape.Length; h++)
            {
                if (!this.IsPointInShape(testshape.GetPoint(h)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if the given point is inside the polygon
        /// </summary>
        /// <param name="polygon">the vertices of polygon</param>
        /// <param name="testPoint">the given point</param>
        /// <returns>true if the point is inside the polygon; otherwise, false</returns>
        private bool IsPointInShape(Point testPoint)
        {
            bool result = false;
            int j = this.Length - 1;
            for (int i = 0; i < this.Length; i++)
            {
                var pointi = (PointF)this.GetPoint(i);
                var pointj = (PointF)this.GetPoint(j);
                if (pointi.Y < testPoint.Y && pointj.Y >= testPoint.Y || pointj.Y < testPoint.Y && pointi.Y >= testPoint.Y)
                {
                    if (pointi.X + (testPoint.Y - pointi.Y) / (pointj.Y - pointi.Y) * (pointj.X - pointi.X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        Point[] _points;
        readonly List<Shape> _insideShapes = new List<Shape>();
        public List<Shape> GetShapes()
        {
            return new List<Shape>(_insideShapes);
        }

        public void AddShape(Shape shape)
        {
            _insideShapes.Add(shape);
        }


        public Shape(params Point[] points)
        {
            _points = points;
        }

        public Shape(IEnumerable<Point> points)
        {
            _points = points.ToArray();
        }

        public int Length => _points.Length;
        public Point GetPoint(in int index) => _points[index];
        public form GetForm()
        {
            switch (_points.Length)
            {
                case (int)form.Triangle:
                    return form.Triangle;
                case (int)form.Square:
                    return form.Square;
                case (int)form.Pentagon:
                    return form.Pentagon;
                default:
                    if (_points.Length < (int)form.Triangle)
                    {
                        return form.Invalid;
                    }
                    return form.Circle;
            }

        }

        static double GetLineDistance(Point line1, Point line2, Point point)
        {
            var top = Math.Abs((line2.Y - line1.Y) * point.X - (line2.X - line1.X) * point.Y + line2.X * line1.Y - line2.Y * line1.X);
            var bottom = Math.Sqrt(Math.Pow((line2.Y - line1.Y), 2) + Math.Pow(line2.X - line1.X, 2));
            return top / bottom;
        }

        public void Reduce(double epsilon, bool recursive = false)
        {
            if (recursive)
            {
                foreach (var item in _insideShapes)
                {
                    item.Reduce(epsilon);
                }
            }

            if (Length < 3)
            {
                _points = new Point[0];
                return;
            }

            var tmp = reduce_loop(epsilon, 0, Length - 1);

            //remove the neighbour last one
            var startp = GetPoint(0);
            var endp = GetPoint(Length - 1);
            if ((Math.Abs(startp.X - endp.X) <= 1 && Math.Abs(startp.Y - endp.Y) <= 1))
            {
                tmp.RemoveAt(tmp.Count - 1);
            }

            if (tmp.Count < 3)
            {
                _points = new Point[0];
                return;
            }

            _points = tmp.ToArray();
        }

        private List<Point> reduce_loop(double epsilon, int start = 0, int end = 0)
        {
            double dmax = 0;
            var index = 0;

            for (int i = start + 1; i < (end - 1); i++)
            {
                var d = GetLineDistance(GetPoint(start), GetPoint(end), GetPoint(i));
                if (d > dmax)
                {
                    index = i;
                    dmax = d;
                }
            }

            var returnlist = new List<Point>();

            if (dmax > epsilon)
            {
                var list1 = reduce_loop(epsilon, start, index);
                var list2 = reduce_loop(epsilon, index, end);

                list1.RemoveAt(list1.Count - 1);

                returnlist.AddRange(list1);
                returnlist.AddRange(list2);
            }
            else
            {
                returnlist.Add(GetPoint(start));
                returnlist.Add(GetPoint(end));
            }

            return returnlist;
        }

        public enum form
        {
            Invalid,
            Triangle = 3,
            Square = 4, 
            Pentagon = 5,
            Circle
        }

        public static Shape FindFormInForm(List<Shape> shapes, form parent, form child)
        {
            foreach (var shape in shapes)
            {
                if (shape.Length < 3)
                    continue;

                if (shape.Length == (int)parent || (parent == form.Circle && shape.Length >= (int)form.Circle))
                {
                    foreach (var childShape in shape.GetShapes())
                    {
                        if (childShape.Length == (int)child || (child == form.Circle && childShape.Length >= (int)form.Circle))
                        {
                            return childShape;
                        }
                    }
                }
            }

            return null;
        }

        enum direction
        {
            N,
            NE,
            E,
            SE,
            S,
            SW,
            W,
            NW
        }

        private static List<Shape> ShapeToContour(in BinaryImage image)
        {
            var output = new List<Shape>();
            var copy = image.Copy();

            for (int startX = 0; startX < copy.Width; startX++)
            {
                for (int startY = 0; startY < copy.Height; startY++)
                {
                    var pixelValue = copy.GetPixel(startX, startY);
                    if (!pixelValue)
                    {
                        continue;
                    }

                    List<Point> list = GetContour(image, startX, startY);

                    //last one is equal to first one so remove
                    list.RemoveAt(list.Count - 1);
                    output.Add(new Shape(list));
                    ClearShape(copy, startX, startY);
                }
            }


            return output;
        }

        private static List<Point> GetContour(BinaryImage image, int startX, int startY)
        {
            var list = new List<Point>
            {
                new Point(startX, startY)
            };

            var dir = direction.N;

            int currentX = startX;
            int currentY = startY;

            do
            {
                //always test left (leftforward if possible) corner from direction perspective
                var testdir = GetLeftForward(dir);

                var attempts = 0;
                while (attempts < 9)
                {
                    if (attempts++ != 0)
                        testdir = GetNextClockwise(testdir);

                    (var testx, var testy) = Getcoordinate(currentX, currentY, testdir);
                    if (TestPixel(image, testx, testy))
                    {
                        list.Add(new Point(testx, testy));
                        currentX = testx;
                        currentY = testy;
                        break;
                    }
                }

                dir = testdir;

                //if there is no way out (lonely pixel)
                if (attempts >= 9)
                    break;
            } while (currentX != startX || currentY != startY);

            

            return list;
        }

        private static bool TestPixel(BinaryImage image, int x, int y)
        {
            if (x < 0 || x >= image.Width || y < 0 || y >= image.Height)
            {
                return false;
            }

            return image.GetPixel(x, y) && AnyNeighborsEmpty(image, x, y);
        }

        /// <summary>
        /// clear shape from binary image
        /// </summary>
        private static void ClearShape(BinaryImage image, int x, int y)
        {
            var queue = new Queue<(int, int)>();
            queue.Enqueue((x, y));

            while (queue.Count != 0)
            {
                var item = queue.Dequeue();
                if (image.GetPixel(item.Item1, item.Item2))
                {
                    image.SetPixel(item.Item1, item.Item2, false);
                    if (item.Item1 != 0)
                    {
                        queue.Enqueue((item.Item1 - 1, item.Item2));
                    }
                    if (item.Item2 != 0)
                    {
                        queue.Enqueue((item.Item1, item.Item2 - 1));
                    }
                    if (item.Item1 != image.Width - 1)
                    {
                        queue.Enqueue((item.Item1 + 1, item.Item2));
                    }
                    if (item.Item2 != image.Height - 1)
                    {
                        queue.Enqueue((item.Item1, item.Item2 + 1));
                    }
                }
            }
        }

        /// <summary>
        /// returns true if any neighbor in range is empty, returns true on out of bound neighbor
        /// </summary>
        private static bool AnyNeighborsEmpty(BinaryImage image, int x, int y, int range = 1)
        {
            //treat out of bounds as empty
            if (x - range < 0 || x + range >= image.Width || y - range < 0 || y + range >= image.Height)
                return true;

            for (int xt = x - range; xt <= x + range; xt++)
            {
                for (int yt = y - range; yt <= y + range; yt++)
                {
                    if (xt == x || yt == y)
                        continue;

                    if (!image.GetPixel(xt, yt))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        static direction GetNextClockwise(in direction dir)
        {
            return dir switch
            {
                direction.N     => direction.NE,
                direction.NE    => direction.E,
                direction.E     => direction.SE,
                direction.SE    => direction.S,
                direction.S     => direction.SW,
                direction.SW    => direction.W,
                direction.W     => direction.NW,
                direction.NW    => direction.N,
                _               => throw new NotImplementedException()
            };
        }

        static direction GetLeftForward(in direction dir)
        {
            return dir switch
            {
                direction.N     => direction.NW,
                direction.NE    => direction.NW,
                direction.E     => direction.NE,
                direction.SE    => direction.NE,
                direction.S     => direction.SE,
                direction.SW    => direction.SE,
                direction.W     => direction.SW,
                direction.NW    => direction.SW,
                _               => throw new NotImplementedException(),
            };
        }

        static (int, int) Getcoordinate(in int x, in int y, in direction dir)
        {
            return dir switch
            {
                direction.N     => (x   ,y-1    ),
                direction.NE    => (x+1 ,y-1    ),
                direction.E     => (x+1 ,y      ),
                direction.SE    => (x+1 ,y+1    ),
                direction.S     => (x   ,y+1    ),
                direction.SW    => (x-1 ,y+1    ),
                direction.W     => (x-1 ,y      ),
                direction.NW    => (x-1 ,y-1    ),
                _               => throw new NotImplementedException(),
            };
        }
    }
}
