using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace major
{
    public partial class Form1 : Form
    {
        Image i1, i2;
        Bitmap b1, b2;
        shape ab, bc;
        String[] lst;
        float[] matches;
        float max=0;
        int index;
        public class shape
        {
            public int pixels, size, width, height;
            public int left, right, up, down;
            public int first_x, first_y;
            public int last_x, last_y;
            public float diag_slope, diag_length;
            public List<int> x, y;
            public int[,] match_matrix;
            private System.Drawing.Bitmap b1;

            public shape(Bitmap bm)
            {
                Color cp;
                pixels = 0;
                width = bm.Width - 1;
                height = bm.Height - 1;
                size = bm.Height * bm.Width - 1;
                x = new List<int>();
                y = new List<int>();
                for (int j = 0; j < height; j++)
                    for (int i = 0; i < width; i++)
                    {
                        cp = bm.GetPixel(i, j);
                        if (cp.R * cp.G * cp.B == 0)
                        {
                            pixels++;
                            x.Add(i);
                            y.Add(j);
                        }
                    }
            }
            public void measure()
            {
                first_x = x.First();
                first_y = y.First();
                last_x = x.Last();
                last_y = y.Last();
                match_matrix = new int[width, height];
                diag_slope = (float)(last_y - first_y) / (float)(last_x - first_x);
                diag_length = (last_y - first_y) * (last_y - first_y) - (last_x - first_x) * (last_x - first_x);
                diag_length = (float)Math.Sqrt(diag_length);
                left = x[0]; right = x[0]; up = y[0]; down = y[0];
                for (int i = 1; i < pixels; i++)
                {
                    if (x[i] < left) left = x[i];
                    if (x[i] > right) right = x[i];
                    if (y[i] < up) up = y[i];
                    if (y[i] > down) down = y[i];
                }

                for (int j = 0; j < height; j++)
                    for (int i = 0; i < width; i++) match_matrix[i, j] = 0;
                for (int i = 0; i < pixels; i++) if (x[i] < width && y[i] < height) match_matrix[x[i], y[i]] = 1;
            }
            public void translate(int a, int b)
            {
                for (int i = 0; i < pixels; i++)
                {
                    x[i] -= a;
                    y[i] -= b;
                }
            }
            public void fit()
            {
                int x_trans, y_trans;
                x_trans = left - 5;
                y_trans = up - 5;
                translate(x_trans, y_trans);
                down -= up - 5;
                first_y -= up - 5;
                last_y -= up - 5;
                right -= left - 5;
                first_x -= left - 5;
                last_x -= left - 5;
                up = left = 5;
                width = right + 5;
                height = down + 5;
            }
            public void scale(float num)
            {
                /*List<int> x1, y1;
                x1 = new List<int>();
                y1 = new List<int>();
                left = (int)((float)left / num);
                right = (int)((float)right / num);
                up = (int)((float)up / num);
                down = (int)((float)down / num);
                first_x = (int)((float)first_x / num);
                first_y = (int)((float)first_y / num);
                last_x = (int)((float)last_x / num);
                last_y = (int)((float)last_y / num);
                diag_length /= num;
                x1.Add((int)((float)x[0] / num));
                y1.Add((int)((float)y[0] / num));*/
                for (int i = 0; i < pixels; i++)
                {
                    x[i] = (int)((float)x[i] / num);
                    y[i] = (int)((float)y[i] / num);
                    /*if (x1.Last() != x[i] && y1.Last() != y[i])
                    {
                        x1.Add(x[i]);
                        y1.Add(y[i]);
                    }*/
                }
                //x = x1; y = y1;
                //pixels = x1.Count();
            }
        }
         public float match_shape(shape s1,shape s2)
        {
            //the last function
            int[,] m;
            int total_pixels, matched_pixels;
            float match;
            total_pixels = matched_pixels = 0;
            s1.measure();
            s2.measure();
            s2.scale(s2.diag_length / s1.diag_length);
            s2.measure();
            s1.fit();
            s2.fit();
            s1.measure();
            s2.measure();


            int w = s1.width; if (s2.width < w) w = s2.width;
            int h = s1.height; if (s2.height < h) h = s2.height;
            m = new int[w, h];
            for (int j = 0; j < h - 1; j++)
            {
                for (int i = 0; i < w - 1; i++)
                {
                    try
                    {
                        m[i, j] = s1.match_matrix[i, j] * s2.match_matrix[i, j];
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        return 0;
                    }
                }
            }
                total_pixels = w * h;
                for (int j = 0; j < h - 1; j++)
                {
                    for (int i = 0; i < w - 1; i++)
                    {
                        matched_pixels += m[i, j];
                    }
                }
                match = (float)matched_pixels / (float)s1.pixels * 100;
            return match;
            //return 0;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            b1 = new Bitmap(openFileDialog1.FileName);
            i1 = b1;
            ab = new shape(b1);
            ab.measure();
            textBox2.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //openFileDialog1.ShowDialog();
            //b2 = new Bitmap(openFileDialog1.FileName);
            //i2 = b2;
            //bc = new shape(b2);
            //bc.measure();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //float x = match_shape(ab, bc);
            ////richTextBox1.AppendText(Environment.NewLine);
            //textBox1.Text = Convert.ToString(x);
            for (int i = 0; i < lst.Count(); i++)
            {
                b2 = new Bitmap(lst[i]);
                i2 = b2;
                bc = new shape(b2);
                bc.measure();
                matches[i] = match_shape(ab, bc);
                richTextBox1.AppendText(matches[i].ToString());
                richTextBox1.AppendText(Environment.NewLine);
            }
            for (int i = 0; i < lst.Count(); i++)
            {
                if (matches[i] > max)
                {
                    max = matches[i];
                    index = i;
                }
            }
            textBox1.Text = lst[index];

        }

        private void button4_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            String address;
            address = folderBrowserDialog1.SelectedPath.ToString();
            //textBox1.Text = address;
            lst = System.IO.Directory.GetFiles(address);
            matches = new float[lst.Count()];
            for (int i = 0; i < lst.Count(); i++)
            {
                richTextBox1.AppendText(lst[i]);
                richTextBox1.AppendText(Environment.NewLine);
            }
            textBox3.Text = address;
        }
    }
}
