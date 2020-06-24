#define PARALLEL

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using formframework.Project;
using ProjectCsharp;

namespace formframework
{

    public partial class Form1 : Form
    {
        static void ApplyEdgeDetection(IChannelImage image)
        {
            var cannyoperator = new CannyOperator(50, 100);
            //var image2 = (GrayImage) GaussBlur.Filter(image);
            //image2.GetBitmap().Save("output/guass.png");
            var simage = new SobolOutput(image);
            var cimage = cannyoperator.filter(simage);
            cimage.SaveInImage(image);
        }

        ShapeMap Test()
        {
            var rgbimage = new RGBImage(currentbtmp);

            var red = new ColorChannelImage(rgbimage, ColorChannel.R);
            var green = new ColorChannelImage(rgbimage, ColorChannel.G);
            var blue = new ColorChannelImage(rgbimage, ColorChannel.B);

            //GrayImage donegreen = null;
            //GrayImage donered = null;
            //GrayImage doneblue = null;
#if PARALLEL
            var task1 = Task.Factory.StartNew(() => ApplyEdgeDetection(green));
            var task2 = Task.Factory.StartNew(() => ApplyEdgeDetection(red));
            var task3 = Task.Factory.StartNew(() => ApplyEdgeDetection(blue));

            Task.WaitAll(task1, task2, task3);
#else
            ApplyEdgeDetection(green);
            ApplyEdgeDetection(red);
            ApplyEdgeDetection(blue);
#endif
            green = null;
            blue = null;
            red = null;

            var CombinedEdgeImage = rgbimage.GetBinaryImage();

            Stage1.Image = CombinedEdgeImage.GetBitmap();

            var blobs = CombinedEdgeImage.FindShapes();

            Stage2.Image = blobs.GetBitmap();
            var shapes = Shape.ShapeFromBinary(blobs);

            var bitmp2 = new Bitmap(300, 300, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            for (int i = 0; i < shapes.Count; i++)
            {
                for (int j = 0; j < shapes[i].Length; j++)
                {
                    var point = shapes[i].GetPoint(j);
                    bitmp2.SetPixel(point.X, point.Y, Color.White);
                }
            }

            Stage3.Image = bitmp2;

            for (int i = 0; i < shapes.Count; i++)
            {
                shapes[i].Reduce(5);
            }

            var bitmp = new Bitmap(300, 300, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            for (int i = 0; i < shapes.Count; i++)
            {
                for (int j = 0; j < shapes[i].Length; j++)
                {
                    var point = shapes[i].GetPoint(j);
                    bitmp.SetPixel(point.X, point.Y, Color.White);
                }
            }

            Stage4.Image = bitmp;

            //var targetShapes = shapes;
            Shape.OrderShapes(shapes);

            //Shape targetShapes = null;
            //for (int i = 0; i < shapes.Count; i++)
            //{
            //    if (shapes[i].Length >= (int)Shape.form.Circle)
            //    {
            //        targetShapes = shapes[i];
            //    }
            //}

            var targetTriangle = Shape.FindFormInForm(shapes, Shape.form.Circle, Shape.form.Triangle);
            var targetSquare = Shape.FindFormInForm(shapes, Shape.form.Circle, Shape.form.Square);
            var targetCircle = Shape.FindFormInForm(shapes, Shape.form.Circle, Shape.form.Circle);

            var targetShapes = new List<Shape> { targetTriangle, targetSquare, targetCircle };

            var map = new ShapeMap(targetShapes);

            return map;
        }

        public Form1()
        {
            InitializeComponent();
        }

        int MapGenerate = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            Rotate();

            if (rotating)
            {
                GenerateImage();
                if (MapGenerate++ > 10)
                {
                    generatingmap = false;
                    MapGenerate = 0;
                    currentmap = Test();

                    if (Return.Checked)
                    {
                        gotoangle = currentmap.RotateMap(startmap);
                    }

                    if (Triangle.Checked)
                    {
                        gotoangle = currentmap.RotateShape(Shape.form.Triangle, currentbtmp.Width / 2, currentbtmp.Height);
                    }

                    if (Square.Checked)
                    {
                        gotoangle = currentmap.RotateShape(Shape.form.Square, currentbtmp.Width / 2, currentbtmp.Height);
                    }

                    if (Circle.Checked)
                    {
                        gotoangle = currentmap.RotateShape(Shape.form.Circle, currentbtmp.Width / 2, currentbtmp.Height);
                    }

                    if (gotoangle > 180)
                    {
                        gotoangle = gotoangle - 360;
                    }
                }
            }
        }

        bool rotating = true;
        bool generatingmap = true;
        private void Rotate()
        {
            if (gotoangle > 0)
            {
                gotoangle--;
                angle--;
            }

            if (gotoangle < 0)
            {
                gotoangle++;
                angle++;
            }

            if (gotoangle == 0 && generatingmap == false)
            {
                rotating = false;
                Status.Text = "done";
            }
        }

        ShapeMap startmap;
        ShapeMap currentmap;

        int gotoangle = 0;
        int angle = 0;
        Bitmap btmp = null;
        Bitmap currentbtmp = null;
        private void GenerateImage()
        {
            if (btmp == null)
            {
                btmp = new Bitmap("images/Untitled.png");
                if (btmp.Width != 300 || btmp.Height != 300)
                {
                    btmp = new Bitmap(btmp, 300, 300);
                }
            }
            DisplayImage.Image = currentbtmp = RotateBitmap(btmp, angle);
        }

        public static Bitmap RotateBitmap(Bitmap bmp, float angle)
        {
            var newBitmap = new Bitmap(bmp.Width, bmp.Height);

            var graphics = Graphics.FromImage(newBitmap);
            graphics.Clear(Color.Black);
            graphics.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
            graphics.RotateTransform(angle);
            graphics.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);
            graphics.DrawImage(bmp, new Point(0, 0));

            return newBitmap;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerateImage();
            startmap = Test();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Return_CheckedChanged(object sender, EventArgs e)
        {
            rotating = true;
            generatingmap = true;
            Status.Text = "rotating";
        }
    }
}
