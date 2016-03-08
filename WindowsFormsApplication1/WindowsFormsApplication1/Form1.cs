using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Function separateFunction = null;
        List<Point>[] points = new List<Point>[2];
        double step;
        Graphics graphics;
        Random rand = new Random();

        public Form1()
        {
            points[0] = new List<Point>();
            points[1] = new List<Point>();
            InitializeComponent();
            graphics = pictureBox1.CreateGraphics();
            random();
        }
        private void random()
        {
            x11.Value = rand.Next(-10, 10);
            x12.Value = rand.Next(-10, 10);
            x21.Value = rand.Next(-10, 10);
            x22.Value = rand.Next(-10, 10);
            y11.Value = rand.Next(-10, 10);
            y12.Value = rand.Next(-10, 10);
            y21.Value = rand.Next(-10, 10);
            y22.Value = rand.Next(-10, 10);
            testX.Value = rand.Next(-10, 10);
            testY.Value = rand.Next(-10, 10);
        }
        private void randomPoints(object sender, EventArgs e)
        {
            x11.Value = rand.Next(-10, 10);
            x12.Value = rand.Next(-10, 10);
            x21.Value = rand.Next(-10, 10);
            x22.Value = rand.Next(-10, 10);
            y11.Value = rand.Next(-10, 10);
            y12.Value = rand.Next(-10, 10);
            y21.Value = rand.Next(-10, 10);
            y22.Value = rand.Next(-10, 10);
        }
        private void randomTest(object sender, EventArgs e)
        {
            testX.Value = rand.Next(-10, 10);
            testY.Value = rand.Next(-10, 10);
        } 

        private void button1_Click(object sender, EventArgs e)
        {         
            step = pictureBox1.Height / 20;
            var potintials = new Potentials();
            var teaching = new Point[2][];

            teaching[0] = new Point[2];
            teaching[0][0] = new Point((int)x11.Value, (int)y11.Value);
            teaching[0][1] = new Point((int)x12.Value, (int)y12.Value);
            points[0].Add(teaching[0][0]);
            points[0].Add(teaching[0][1]);
                                                                   
            teaching[1] = new Point[2];                                                  
            teaching[1][0] = new Point((int)x21.Value, (int)y21.Value);
            teaching[1][1] = new Point((int)x22.Value, (int)y22.Value);
            points[1].Add(teaching[1][0]);
            points[1].Add(teaching[1][1]);

            separateFunction = potintials.GetFunction(teaching);
            label14.Text = separateFunction.ToString();

            graphics.Clear(Color.Black);
            graphics.DrawLine(Pens.White, 0, pictureBox1.Height / 2, pictureBox1.Width, pictureBox1.Height / 2);
            graphics.DrawLine(Pens.White, pictureBox1.Width / 2, 0, pictureBox1.Width / 2, pictureBox1.Height);
            DrawGraph(graphics);
            DrawPoints(graphics, teaching);
        }

        private void DrawPoints(Graphics graphics, Point[][] teaching)
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    if (i == 0)
                        graphics.FillEllipse(new SolidBrush(Color.Lime),
                            (int)(pictureBox1.Width / 2 + teaching[i][j].X * step - 4),
                            (int)(pictureBox1.Height / 2 - teaching[i][j].Y * step - 4), 9, 9);
                    else
                        graphics.FillEllipse(new SolidBrush(Color.LimeGreen),
                            (int)(pictureBox1.Width / 2 + teaching[i][j].X * step - 4),
                            (int)(pictureBox1.Height / 2 - teaching[i][j].Y * step - 4), 9, 9);
        }

        private void DrawGraph(Graphics graphics)
        {
            Pen graphPen = new Pen(Color.White,4);
            var prevPoint = new Point(pictureBox1.Width/2 + (int) (-pictureBox1.Width/2*step),
                pictureBox1.Height/2 - (int) (separateFunction.GetY(-pictureBox1.Width/2/step)*step));
            
            for (double x = -pictureBox1.Width/2; x < pictureBox1.Width/2; x += 0.1)
            {
                double y = separateFunction.GetY(x/step);
                var nextPoint = new Point((int)(pictureBox1.Width/2 + x),
                    (int)(pictureBox1.Height/2 - y*step));

                if (Math.Abs(nextPoint.Y - prevPoint.Y) < pictureBox1.Height)
                    graphics.DrawLine((Pen) graphPen, prevPoint, nextPoint);

                prevPoint = nextPoint;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var testPoint = new Point((int)testX.Value, (int)testY.Value);
            int classNumber;

            if (separateFunction.GetValue(testPoint) >= 0)
                classNumber = 0;
            else 
                classNumber = 1;

            points[classNumber].Add(testPoint);
            MessageBox.Show("Класс " + (classNumber + 1));

            if (classNumber == 0)
                graphics.FillEllipse(new SolidBrush(Color.Red),
                    (int)(pictureBox1.Width / 2 + testPoint.X * step - 4),
                    (int)(pictureBox1.Height / 2 - testPoint.Y * step - 4), 9, 9);
            else
                graphics.FillEllipse(new SolidBrush(Color.Purple),
                    (int)(pictureBox1.Width / 2 + testPoint.X * step - 4),
                    (int)(pictureBox1.Height / 2 - testPoint.Y * step - 4), 9, 9);
        
        }    
    }
}
