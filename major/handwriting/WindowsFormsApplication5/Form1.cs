using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Form1 : Form
    {
        public class delta
        {
            List<int> x, y;                                             //the list of coordinates
            int count;                                                  //the number of points
            int seg;                                                    //the number of segments to be taken                                         //approximation factor of slopes
            List<float> segments;                                       //list of slopes
            internal List<int> deltas;                                           //list of deltas
            public delta(List<int> arg_x, List<int> arg_y)              //initializes coordinate list
            {
                x = new List<int>();
                y = new List<int>();
                for (int i = 0; i < arg_x.Count; i++)
                {
                    x.Add(arg_x[i]);
                    y.Add(arg_y[i]);
                }
                count = arg_x.Count;
            }
            public void segmentor(int seg_count)                        //extracts segment based on segment count provided
            {
                seg = seg_count;
                int length = count / seg;
                segments = new List<float>();
                int il = 0;
                float sl;
                for (int i = length; i < x.Count; i += length)
                {
                    sl = (float)(y[i] - y[il]) / (float)(x[i] - x[il]);       //possible type conflict
                    sl = (float)System.Math.Atan(sl);
                    sl = (float)(sl * 180 / System.Math.PI);
                    if ((x[i] - x[il]) < 0)
                    {
                        sl += 180;
                    }
                    else if ((y[i] - y[il]) < 0)
                    {
                        sl = 360 + sl;
                    }
                    segments.Add(sl);
                    il = i;
                }
            }
            public void stringer()                              //extracts delta of the slopes of segments as approximated valuea
            {
                int dl;
                deltas = new List<int>();
                for (int i = 1; i < seg - 1; i++)                            //possible index overflow
                {
                    dl = (int)(segments[i] - segments[i - 1]);            //multiply or divide optional
                    deltas.Add(dl);
                }
            }
        }
        Graphics g1;
        Graphics g2;
        Point point,p2,p3;
        SolidBrush brush;
        Pen penka;
        List<int> x1, y1, x2, y2;
        delta d1,d2;
        
        public Form1()
        {
            InitializeComponent();
            g1 = panel1.CreateGraphics();
            g2 = panel2.CreateGraphics();           
            brush = new SolidBrush(Color.Black);
            p3.X=p2.X = -1;
            p3.Y=p2.Y = -1;
            penka = new Pen(brush);
            x1 = new List<int>();
            y1 = new List<int>();
            x2 = new List<int>();
            y2 = new List<int>();            
        }
       
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //MessageBox.Show("sonu");
            g1.Clear(Color.White); 
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            //MessageBox.Show("monu");
            g2.Clear(Color.White);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                point = panel1.PointToClient(Cursor.Position);
                //MessageBox.Show(point.ToString());
                richTextBox1.AppendText(point.ToString());
                //g1 = panel1.CreateGraphics();
                //brush = new SolidBrush(Color.Black);
                //g1.FillRectangle(brush, point.X, point.Y, 1, 1);
                if (p2.X != -1)
                {
                    g1.DrawLine(penka, point, p2);
                    x1.Add(point.X);
                    y1.Add(point.Y);
                }
                p2.X = point.X;
                p2.Y = point.Y;
            }
            else
            {
                p2.X = -1;
                p2.Y = -1;
            }
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                point = panel2.PointToClient(Cursor.Position);
                //MessageBox.Show(point.ToString());
                richTextBox2.AppendText(point.ToString());
                //g1 = panel1.CreateGraphics();
                //brush = new SolidBrush(Color.Black);
                //g2.FillRectangle(brush, point.X, point.Y, 1, 1);
                if (p3.X != -1)
                {
                    g2.DrawLine(penka, point, p3);
                    x2.Add(point.X);
                    y2.Add(point.Y);
                }
                p3.X = point.X;
                p3.Y = point.Y;
            }
            else
            {
                p3.X = -1;
                p3.Y = -1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            g1.Clear(Color.White);
            x1.Clear(); y1.Clear();
            p2.X = -1;
            p2.Y = -1;
            richTextBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            g2.Clear(Color.White);
            x2.Clear(); y2.Clear();
            p3.X = -1;
            p3.Y = -1;
            richTextBox2.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<float> Match_list = new List<float>();
            float match_value = 0;
            d1 = new delta(x1, y1);
            d1.segmentor(30);
            d1.stringer();
            d2 = new delta(x2, y2);
            d2.segmentor(30);
            d2.stringer();
            for (int i = 0; i < d1.deltas.Count; i++)
            {
                Match_list.Add(d1.deltas[i] - d2.deltas[i]);
                //MessageBox.Show(Match_list[i].ToString());
            }
            for (int i = 0; i < Match_list.Count; i++)
            {
                match_value += Match_list[i];
            }
            match_value /= Match_list.Count;
            match_value = System.Math.Abs(match_value);
            match_value = 100 - (match_value * 7);
            if (match_value < 0)
            {
                match_value = 0;
            }
            textBox1.Text = match_value.ToString() + "%";
        }


        
    }
}
