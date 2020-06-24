using ProjectCsharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace formframework.Project
{
    class ShapeMap
    {
        private struct ShapeData
        {
            public ShapeData(int xcenter, int ycenter, Shape.form form)
            {
                centerx = xcenter;
                centery = ycenter;
                this.form = form;
            }
            Shape.form form;
            public Shape.form Form => form;
            public (int, int) Center => (centerx, centery);
            public int RotateTo(int x, int y, int imageWidth,int imageHeight)
            {
                double mindiv = double.MaxValue;
                int bestangle = int.MinValue;

                var cx = centerx - (imageWidth / 2);
                var cy = (imageHeight / 2) - centery;

                for (int angle = 0; angle < 360; angle++)
                {
                    var xnew = cx * Math.Cos(angle * (Math.PI / 180.0)) - cy * Math.Sin(angle * (Math.PI / 180.0));
                    var ynew = cx * Math.Sin(angle * (Math.PI / 180.0)) + cy * Math.Cos(angle * (Math.PI / 180.0));

                    var xdiv = xnew - (x - (imageWidth / 2));
                    var ydiv = ynew - ((imageHeight / 2) - y);
                    var totaldiv = Math.Sqrt(xdiv * xdiv + ydiv * ydiv);

                    if (totaldiv < mindiv)
                    {
                        mindiv = totaldiv;
                        bestangle = angle;
                    }
                }

                return bestangle;
            }

            int centerx;
            int centery;
        }

        List<ShapeData> shapeDatas = new List<ShapeData>();

        public int Count => shapeDatas.Count;

        public ShapeMap(List<Shape> shapes)
        {
            foreach (var shape in shapes)
            {
                if (shape == null)
                    continue;
                var totalx = 0;
                var totaly = 0;
                for (int i = 0; i < shape.Length; i++)
                {
                    var point = shape.GetPoint(i);
                    totalx += point.X;
                    totaly += point.Y;
                }

                shapeDatas.Add(new ShapeData(totalx / shape.Length, totaly / shape.Length, shape.GetForm()));
            }
        }

        public int RotateMap(ShapeMap shapeMap, int imageWidth = 300, int imageHeight = 300)
        {
            var totalangle = 0;

            var triangles1 = shapeMap.shapeDatas.Where(s => s.Form == Shape.form.Triangle).ToArray();
            var triangles2 = this.shapeDatas.Where(s => s.Form == Shape.form.Triangle).ToArray();
            var squares1 = shapeMap.shapeDatas.Where(s => s.Form == Shape.form.Square).ToArray();
            var squares2 = this.shapeDatas.Where(s => s.Form == Shape.form.Square).ToArray(); 
            var circle1 = shapeMap.shapeDatas.Where(s => s.Form == Shape.form.Circle).ToArray();
            var circle2 = this.shapeDatas.Where(s => s.Form == Shape.form.Circle).ToArray();

            for (int i = 0; i < triangles1.Length && i < triangles2.Length; i++)
            {
                (var x, var y) = triangles1[i].Center;

                totalangle += triangles2[i].RotateTo(x, y, imageWidth, imageHeight);
            }

            for (int i = 0; i < squares1.Length && i < squares2.Length; i++)
            {
                (var x, var y) = squares1[i].Center;

                totalangle += squares2[i].RotateTo(x, y, imageWidth, imageHeight);
            }

            for (int i = 0; i < circle1.Length && i < circle2.Length; i++)
            {
                (var x, var y) = circle1[i].Center;

                totalangle += circle2[i].RotateTo(x, y, imageWidth, imageHeight);
            }


            var totalcount = Math.Min(triangles1.Length, triangles2.Length) + Math.Min(squares1.Length, squares2.Length) + Math.Min(circle1.Length, circle2.Length);

            if (totalcount == 0)
            {
                return 0;
            }
            return totalangle / totalcount;
        }

        public int RotateShape(Shape.form form, int x, int y, int imageWidth = 300, int imageHeight = 300)
        {
            var angle = int.MaxValue;
            foreach (var item in shapeDatas)
            {
                if (item.Form != form)
                {
                    continue;
                }

                var currentangle = item.RotateTo(x, y, imageWidth, imageHeight);
                if (Math.Abs(currentangle) < Math.Abs(angle))
                {
                    angle = currentangle;
                }
            }

            return angle;
        }
    }
}
