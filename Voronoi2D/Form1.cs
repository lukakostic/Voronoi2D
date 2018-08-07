using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Voronoi2D
{
    public partial class Form1 : Form
    {
        public List<ButtonPoint> points;
        System.Random r;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            r = new System.Random();
            points = new List<ButtonPoint>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MakePoint(5, 5);
        }

        private void button2_Click(object sender, EventArgs e)//draw voronoi
        {
            if (points.Count == 0) return;
            int size = pictureBox1.Size.Width;
            var img = new Bitmap(size, size);
            
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    // Find closest point and color the pixel that color
                    int closest = 0;
                    float dist = float.MaxValue;
                    float minDist = float.MaxValue;
                    for (int i = 0; i < points.Count; i++)
                    {
                        int xx = points[i].b.Location.X-x;
                        int yy = points[i].b.Location.Y-y;
                        dist = (float)Math.Sqrt((double)(xx * xx + yy * yy));
                        if (dist < minDist)
                        {
                            closest = i;
                            minDist = dist;
                        }
                    }
                    img.SetPixel(x, y, points[closest].c);
                }
            }
            pictureBox1.Image = img;
            pictureBox1.Update();
            pictureBox1.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e) //Move selected button/point every 1ms
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].mDown)
                {
                    if (points[i].b.Location.X - 1 > pictureBox1.Size.Width || points[i].b.Location.X<0)
                    {
                        RemovePoint(i);
                        return;
                    }
                    if (points[i].b.Location.Y - 1 > pictureBox1.Size.Height || points[i].b.Location.Y < 0)
                    {
                        RemovePoint(i);
                        return;
                    }

                    points[i].b.Location = new Point(Cursor.Position.X-this.Location.X-10, Cursor.Position.Y - this.Location.Y -35);
                }
            }
        }

        public void RemovePoint(int i)
        {
            points[i].b.Dispose();
            points.RemoveAt(i);
        }

        public void MakePoint(int x,int y)
        {
            var b = new Button();

            b.BackColor = button3.BackColor;
            b.Cursor = button3.Cursor;
            b.Size = button3.Size;
            b.FlatStyle = button3.FlatStyle;

            b.Tag = points.Count.ToString();
            b.MouseDown += (s, ev) => { points[int.Parse((string)((Button)s).Tag)].mDown = true; }; //Get point index from tag
            b.MouseUp += (s, ev) => { points[int.Parse((string)((Button)s).Tag)].mDown = false; }; //Get point index from tag

            var c = Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));

            b.Show();
            pictureBox1.Controls.Add(b);
            b.Location = new Point(x, y);
            points.Add(new ButtonPoint(c, b));
        }

        private void button4_Click(object sender, EventArgs e) // Add random
        {
            MakePoint(r.Next(pictureBox1.Size.Width), r.Next(pictureBox1.Size.Height));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            while (points.Count > 0) RemovePoint(0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);
            }
        }
    }

    public class ButtonPoint
    {
        public Color c;
        public Button b;
        public bool mDown = false;

        public ButtonPoint(Color cc,Button bb)
        {
            this.c = cc;
            this.b = bb;
        }
    }
}

