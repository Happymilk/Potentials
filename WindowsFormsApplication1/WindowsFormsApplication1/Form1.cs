using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private ToolTip tooltip = new ToolTip();
        public Function separetFunction = null;
        private List<Point>[] points = new List<Point>[2];
        private double step;

        public Form1()
        {
            points[0] = new List<Point>();
            points[1] = new List<Point>();
            InitializeComponent();      
        }

        private void teachingButton_Click(object sender, EventArgs e)
        {         
            step = pictureBox1.Height / 20;
            var potintials = new Potintials();
            var teaching = new Point[2][];

            teaching[0] = new Point[2];
            teaching[0][0] = new Point((int)x11NumericUpDown.Value, (int)y11NumericUpDown.Value);
            teaching[0][1] = new Point((int)x12NumericUpDown.Value, (int)y12NumericUpDown.Value);
            points[0].Add(teaching[0][0]);
            points[0].Add(teaching[0][1]);
                                                                   
            teaching[1] = new Point[2];                                                  
            teaching[1][0] = new Point((int)x21NumericUpDown.Value, (int)y21NumericUpDown.Value);
            teaching[1][1] = new Point((int)x22NumericUpDown.Value, (int)y22NumericUpDown.Value);
            points[1].Add(teaching[1][0]);
            points[1].Add(teaching[1][1]);

            separetFunction = potintials.GetFunction(teaching);
            MessageBox.Show("Разделяющая функция: " + separetFunction.ToString());

            var bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap) )
            {
                graphics.Clear(Color.White);
                DrawGraph(graphics);
                DrawEdges(graphics);
                DrawPoints(graphics, teaching);
            }
            pictureBox1.Image = bitmap;
        }

        private void DrawPoints(Graphics graphics, Point[][] teaching)
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    DrawPoint(graphics, teaching[i][j], i);
        }

        private void DrawEdges(Graphics graphics)
        {
            graphics.DrawLine(Pens.Black, 0, pictureBox1.Height/2, pictureBox1.Width, pictureBox1.Height/2);
            graphics.DrawLine(Pens.Black, pictureBox1.Width/2, 0, pictureBox1.Width/2, pictureBox1.Height);
        }

        private void DrawGraph(Graphics graphics)
        {
            Pen graphPen = new Pen(Color.Red,2);

            var prevPoint = new Point(pictureBox1.Width/2 + (int) (-pictureBox1.Width/2*step),
                pictureBox1.Height/2 - (int) (separetFunction.GetY(-pictureBox1.Width/2/step)*step));
            for (double x = - pictureBox1.Width/2; x < pictureBox1.Width/2; x += 0.1)
            {
                double y = separetFunction.GetY(x/step);
                var nextPoint = new Point((int) (pictureBox1.Width/2 + x),
                    (int) (pictureBox1.Height/2 - y*step));
                try
                {
                    if (Math.Abs(nextPoint.Y - prevPoint.Y) < pictureBox1.Height)
                    {
                        graphics.DrawLine((Pen) graphPen, prevPoint, nextPoint);
                    }
                }
                catch (OverflowException)
                {
                    ;
                }
                prevPoint = nextPoint;
            }
        }

        private void DrawPoint(Graphics graphics, Point point, int classNumber)
        {
            graphics.FillEllipse(new SolidBrush(classNumber == 0 ? Color.ForestGreen: Color.Blue),
                (int) (pictureBox1.Width/2 + point.X*step - 4),
                (int) (pictureBox1.Height/2 - point.Y*step - 4), 9, 9);
        }

        private bool showen = false;
        private void Move_MouseMove(object sender, MouseEventArgs e)
        {
            if (DrawToolTip(e, points[0], 1)) return;
            if (DrawToolTip(e, points[1], 2)) return;
            tooltip.Hide(pictureBox1);
            showen = false;

        }

        private bool DrawToolTip(MouseEventArgs e, IEnumerable<Point> list, int classNumber)
        {
            foreach (Point point in list)
            {
                Point position = e.Location;
                if (Math.Abs(point.X * step + pictureBox1.Width / 2 - position.X) < 10 &&
                    Math.Abs(pictureBox1.Height / 2 - point.Y * step - position.Y) < 10)
                {
                    if (!showen)
                    {
                        tooltip.Show(
                            String.Format("Класс:{0}\r\n({1};{2})", classNumber, point.X, point.Y),
                            pictureBox1, e.Location + new Size(10,10));
                        showen = true;
                       
                    }
                    return true;
                    
                }
            }
            return false;
        }
        private void testButton_Click(object sender, EventArgs e)
        {
            var testPoint = new Point((int)testXNumericUpDown.Value, (int)testYNumericUpDown.Value);
            int classNumber = separetFunction.GetValue(testPoint) >= 0 ? 0 : 1;
            points[classNumber].Add(testPoint);
            MessageBox.Show("Класс " + (classNumber + 1));
            var bitmap = new Bitmap(pictureBox1.Image);
            pictureBox1.Image.Dispose();
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                DrawPoint(graphics,testPoint,classNumber);
            }
            pictureBox1.Image = bitmap;
        }
    }
}
