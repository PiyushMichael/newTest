using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Border
{
    public partial class Form1 : Form
    {
        int extension_flag = 0;
        Bitmap pic;
        Image im;
        Color pix;
        Graphics gfx;
        SolidBrush brush,brush1;
        Pen penka,penka1;
        Point p;
        int threshold = 50, Difference_value = 0;
        int width, height;
        int[,] matrix_red,matrix_green,matrix_blue, border_matrix,fill_matrix1,fill_matrix2;
        float x, y;
        public Form1()
        {
            InitializeComponent();
        }

        public void bucketfill(int x, int y)
        {
            if (fill_matrix1[x, y] == 0) fill_matrix1[x, y] = 2;
            else return;
            if (x < width - 2) bucketfill(x + 1, y);
            if (y < height - 2) bucketfill(x, y + 1);
            if (x > 0) bucketfill(x - 1, y);
            //if (y > 0) bucketfill(x, y - 1);
        }
        public void bucketfill_twin(int x, int y)
        {
            if (fill_matrix2[x, y] == 0) fill_matrix2[x, y] = 2;
            else return;
            if (x < width - 2) bucketfill_twin(x + 1, y);
            if (y < height - 2) bucketfill_twin(x, y + 1);
            if (x > 0) bucketfill_twin(x - 1, y);
            if (y > 0) bucketfill_twin(x, y - 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string name, extension;
            name = openFileDialog1.SafeFileName.ToString();
            int i = name.Length - 1;
            while (!name[i].Equals('.'))
            {
                i--;
            }
            extension = name.Substring(i);
            richTextBox1.Text = extension;
            if ((extension.Equals(".jpeg")) || (extension.Equals(".png")) || (extension.Equals(".jpg")) || (extension.Equals(".bmp")) || (extension.Equals(".gif")))
            {
                extension_flag = 1;
            }
            if (extension_flag == 0)
            {
                MessageBox.Show("Invalid File Format");
            }
            else
            {
                pic = new Bitmap(openFileDialog1.FileName);
                im = pic;
                pictureBox1.Image = im;
                gfx = panel1.CreateGraphics();
                width = pic.Width;
                height = pic.Height;
                matrix_red = new int[width, height];
                matrix_green = new int[width, height];
                matrix_blue = new int[width, height];
                border_matrix = new int[width - 1, height - 1];
                fill_matrix1 = new int[width - 1, height - 1];
                fill_matrix2 = new int[width - 1, height - 1];
                brush = new SolidBrush(Color.Black);
                brush1 = new SolidBrush(Color.Blue);
                penka = new Pen(brush);
                penka1 = new Pen(brush1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (extension_flag == 1)
            {
                for (int j = 0; j < height; j++)                       //Loop to make color matrices.
                {
                    for (int i = 0; i < width; i++)
                    {
                        pix = pic.GetPixel(i, j);
                        matrix_red[i, j] = pix.R;
                        matrix_green[i, j] = pix.G;
                        matrix_blue[i, j] = pix.B;
                    }
                }

                for (int j = 0; j < height - 1; j++)                      //matching here
                {
                    for (int i = 0; i < width - 1; i++)
                    {
                        Difference_value = 0;
                        Difference_value += Math.Abs(matrix_red[i, j] - matrix_red[i + 1, j]);
                        Difference_value += Math.Abs(matrix_green[i, j] - matrix_green[i + 1, j]);
                        Difference_value += Math.Abs(matrix_blue[i, j] - matrix_blue[i + 1, j]);

                        if (Difference_value >= threshold)
                        {
                            border_matrix[i, j] = 1;
                        }
                        else
                        {
                            border_matrix[i, j] = 0;
                        }
                    }
                }
                for (int i = 0; i < width - 1; i++)
                {
                    for (int j = 0; j < height - 1; j++)
                    {
                        Difference_value = 0;
                        Difference_value += Math.Abs(matrix_red[i, j] - matrix_red[i, j + 1]);
                        Difference_value += Math.Abs(matrix_green[i, j] - matrix_green[i, j + 1]);
                        Difference_value += Math.Abs(matrix_blue[i, j] - matrix_blue[i, j + 1]);

                        if (Difference_value >= threshold)
                        {
                            border_matrix[i, j] = 1;
                        }
                    }
                }

                for (int j = 0; j < height - 1; j++)
                {
                    for (int i = 0; i < width - 1; i++)
                    {

                       fill_matrix1[i, j] = border_matrix[i, j];
                    }
                }
                
                gfx.Clear(Color.White);


                for (int j = 0; j < height - 1; j++)
                {
                    for (int i = 0; i < width - 1; i++)
                    {

                        if (border_matrix[i, j] == 1)
                        {
                            x = (float)i;
                            y = (float)j;
                            gfx.DrawRectangle(penka, x, y, 0.1f, 0.1f);
                        }
                    }
                }
            }
        }
        private void update(object sender, EventArgs e)
        {
            threshold = trackBar1.Value;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            p = panel1.PointToClient(Cursor.Position);
            //string str;
            //str = p.X.ToString();
            //str += ',';
            //str += p.Y.ToString();
            //MessageBox.Show(str);
            bucketfill(p.X, p.Y);


            for (int j = 0; j < height - 1; j++)
            {
                for (int i = 0; i < width - 1; i++)
                {

                    if (fill_matrix1[i, j] == 2)
                    {
                        x = (float)i;
                        y = (float)j;
                        gfx.DrawRectangle(penka1, x, y, 0.1f, 0.1f);
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            p = pictureBox1.PointToClient(Cursor.Position);
            for (int j = 0; j < height - 1; j++)
            {
                for (int i = 0; i < width - 1; i++)
                {
                    fill_matrix2[i, j] = border_matrix[i, j];
                }
            }
            
            bucketfill_twin(p.X, p.Y);

            gfx.Clear(Color.White);
            for (int j = 0; j < height - 1; j++)
            {
                for (int i = 0; i < width - 1; i++)
                {

                    if (fill_matrix2[i, j] == 2)
                    {
                        x = (float)i;
                        y = (float)j;
                        gfx.DrawRectangle(penka1, x, y, 0.1f, 0.1f);
                    }
                }
            }
        }
    }
}
