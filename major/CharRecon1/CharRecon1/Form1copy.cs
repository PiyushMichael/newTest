using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CharRecon1
{
    public partial class Form1 : Form
    {
        Image i1, i2;
        Bitmap b1, b2;
        shape ab, bc;
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
                for(int j=0;j<height;j++)
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
                    for (int i = 0; i < width; i++) match_matrix[i,j] = 0;
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
            total_pixels = matched_pixels = 0;
            s1.measure();
            s2.measure();
            s2.scale(s2.diag_length / s1.diag_length);
            s2.measure();
            s1.fit();
            s2.fit();
            s1.measure();
            s2.measure();

            /*richTextBox1.AppendText(Environment.NewLine);
            for (int i = 0; i < s1.pixels; i++)
            {
                richTextBox1.AppendText(Convert.ToString(s1.x[i]));
                richTextBox1.AppendText(",");
                richTextBox1.AppendText(Convert.ToString(s1.y[i]));
                richTextBox1.AppendText("   ");
            }
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s1.diag_length));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s1.diag_slope));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s1.up));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s1.down));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s1.left));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s1.right));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s1.width));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s1.height));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s1.pixels));
            richTextBox1.AppendText(Environment.NewLine);
            for (int j = 0; j < s1.height-1; j++)
            {
                for (int i = 0; i < s1.width-1; i++)
                {
                    if (s1.match_matrix[i, j] == 1) richTextBox1.AppendText("--");
                    else richTextBox1.AppendText("//");
                }
                richTextBox1.AppendText(Environment.NewLine);
            }

            
            richTextBox1.AppendText(Environment.NewLine);
            for (int i = 0; i < s2.pixels; i++)
            {
                richTextBox1.AppendText(Convert.ToString(s2.x[i]));
                richTextBox1.AppendText(",");
                richTextBox1.AppendText(Convert.ToString(s2.y[i]));
                richTextBox1.AppendText("   ");
            }
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s2.diag_length));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s2.diag_slope));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s2.up));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s2.down));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s2.left));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s2.right));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s2.width));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s2.height));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(s2.pixels));
            richTextBox1.AppendText(Environment.NewLine);
            for (int j = 0; j < s2.height-1; j++)
            {
                for (int i = 0; i < s2.width-1; i++)
                {
                    if (s2.match_matrix[i, j] == 1) richTextBox1.AppendText("--");
                    else richTextBox1.AppendText("//");
                }
                richTextBox1.AppendText(Environment.NewLine);
            }*/


            int w = s1.width; if (s2.width < w) w = s2.width;
            int h = s1.height; if (s2.height < h) w = s2.height;
            m = new int[w, h];
            for (int j = 0; j < h - 1; j++)
            {
                for (int i = 0; i < w - 1; i++)
                {
                    m[i, j] = s1.match_matrix[i, j] * s2.match_matrix[i, j];
                }
                //richTextBox1.AppendText(Environment.NewLine);
            }
            /*for (int j = 0; j < h - 1; j++)
            {
                for (int i = 0; i < w - 1; i++)
                {
                    if (m[i, j] == 1) richTextBox1.AppendText("--");
                    else richTextBox1.AppendText("//");
                }
                richTextBox1.AppendText(Environment.NewLine);
            }*/
            total_pixels = w * h;
            for (int j = 0; j < h - 1; j++)
            {
                for (int i = 0; i < w - 1; i++)
                {
                    matched_pixels += m[i, j];
                }
                //richTextBox1.AppendText(Environment.NewLine);
            }
            float match = (float)matched_pixels / (float)s1.pixels * 100;
            /*richTextBox1.AppendText(Convert.ToString(matched_pixels)); richTextBox1.AppendText(" ");
            richTextBox1.AppendText(Convert.ToString(total_pixels)); richTextBox1.AppendText(" ");
            richTextBox1.AppendText(Convert.ToString(match));*/
            return match;
            //return 0;
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            pictureBox2.SizeMode = PictureBoxSizeMode.Normal;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.SafeFileName;
            b1 = new Bitmap(openFileDialog1.FileName);
            i1 = b1;
            pictureBox1.Image = i1;
            //gen_map(b1, im1);
            ab = new shape(b1);           
            ab.measure();
            //ab.fit();
            //ab.scale(2f);
            //ab.fit();
            //ab.measure();
            /*for (int i = 0; i < ab.pixels; i++)
            {
                richTextBox1.AppendText(Convert.ToString(ab.x[i]));
                richTextBox1.AppendText(",");
                richTextBox1.AppendText(Convert.ToString(ab.y[i]));
                richTextBox1.AppendText("   ");
            }
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(ab.diag_length));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(ab.diag_slope));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(ab.up));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(ab.down));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(ab.left));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(ab.right));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(ab.width));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(ab.height));
            */
            /*richTextBox1.AppendText(Environment.NewLine);
            for (int j = 0; j < ab.height-1; j++)
            {
                for (int i = 0; i < ab.width-1; i++)
                {
                    if (ab.match_matrix[i, j] == 1) richTextBox1.AppendText("--");
                    else richTextBox1.AppendText("//");
                }
                richTextBox1.AppendText(Environment.NewLine);
            }*/
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
            textBox2.Text = openFileDialog2.SafeFileName;
            b2 = new Bitmap(openFileDialog2.FileName);
            i2 = b2;
            pictureBox2.Image = i2;
            bc = new shape(b2);
            bc.measure();

            /*richTextBox1.AppendText(Environment.NewLine);
            for (int i = 0; i < bc.pixels; i++)
            {
                richTextBox1.AppendText(Convert.ToString(bc.x[i]));
                richTextBox1.AppendText(",");
                richTextBox1.AppendText(Convert.ToString(bc.y[i]));
                richTextBox1.AppendText("   ");
            }
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(bc.diag_length));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(bc.diag_slope));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(bc.up));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(bc.down));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(bc.left));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(bc.right));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(bc.width));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(bc.height));
            richTextBox1.AppendText("   ");
            richTextBox1.AppendText(Convert.ToString(bc.pixels));
            richTextBox1.AppendText(Environment.NewLine);*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            float x=match_shape(ab, bc);
            //richTextBox1.AppendText(Environment.NewLine);
            textBox3.Text = Convert.ToString(x);
            //richTextBox1.AppendText(Convert.ToString(x));
        }

        //private point_map im1 { get; set; }
    }
}

/*
Expanding on Chris and Migol`s answer with a code sample.

Using an array
 
 int[,,] buttons = new int[4,5,3];

 
Student[] array = new Student[2];
array[0] = new Student("bob");
array[1] = new Student("joe");
Using a generic list. Under the hood the List<T> class uses an array for storage but does so in a fashion that allows it to grow effeciently.

List<Student> list = new List<Student>();
list.Add(new Student("bob"));
list.Add(new Student("joe"));
Student joe = list[1];
*/