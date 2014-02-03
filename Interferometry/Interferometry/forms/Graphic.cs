using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interferometry.math_classes;

namespace rab1.Forms
{
    public partial class Graphic : Form
    {
        private Int64[] buf_gl;
        private Int64[] buf1_gl;
        private int w;
        private int h;
        private int x0 = 40;
        private int hh = 256;
        private Int64 maxx = -4000000, minx = 4000000;  

        public Graphic(ZArrayDescriptor ZZ, int x, int y)
        {
            InitializeComponent();
            init(ZZ, x, y, Color.Red);
        }

        public Graphic(ZArrayDescriptor ZZ, ZArrayDescriptor secondArray, int x, int y)  // Двойной график
        {
            InitializeComponent();
            init1(ZZ, secondArray, x, y);
        }
        private void init1(ZArrayDescriptor ZZ1, ZArrayDescriptor ZZ2, int x, int y)
        {
            int w1 = ZZ1.width;  w = w1;
            int h1 = ZZ1.height; h = h1;

            Int64[] buf1 = new Int64[Math.Max(w1, h1)];
            Int64[] buf2 = new Int64[Math.Max(w1, h1)];

            buf_gl = new Int64[Math.Max(w1, h1)];
            buf1_gl = new Int64[Math.Max(w1, h1)];


            for (int i = 0; i < w1; i++) { buf1[i] = ZZ1.array[i, y]; buf_gl[i]  = buf1[i]; }
            for (int i = 0; i < w1; i++) { buf2[i] = ZZ2.array[i, y]; buf1_gl[i] = buf2[i]; }
            MaxMin(buf1, buf2, w1);

            pc1.BackColor = Color.White;
            pc1.Size = new Size(w1 + 16, hh + 32);
            pc1.BorderStyle = BorderStyle.Fixed3D;

            if (pc1.BackgroundImage == null)
            {
                Bitmap btmBack = new Bitmap(w1 + 16, hh + 32); //изображение
                pc1.BackgroundImage = btmBack;
            }

            if (pc1.Image == null)
            {
                Bitmap btmFront = new Bitmap(w1 + 16, hh + 32); //фон
                pc1.Image = btmFront;
            }


            Graphics grBack = Graphics.FromImage(pc1.BackgroundImage);


            Graph(buf1, w1, x, grBack, Color.Red,1);                                       // 1 График по x
            Graph(buf2, w1, x, grBack, Color.Blue,1);                                      // 2 График по x


            for (int i = 0; i < h1; i++) { buf1[i] = ZZ1.array[x, i]; buf_gl[i] = buf1[i]; }
            for (int i = 0; i < h1; i++) { buf2[i] = ZZ2.array[x, i]; buf1_gl[i] = buf2[i]; }
            MaxMin(buf1, buf2, h1);
           
            pictureBox1.BackColor = Color.White;
            pictureBox1.Size = new Size(w1 + 16, hh + 32);
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;


            if (pictureBox1.Image == null)
            {
                Bitmap btmFront = new Bitmap(w1 + 16, hh + 32); //фон
                pictureBox1.Image = btmFront;
            }

            if (pictureBox1.BackgroundImage == null)
            {
                Bitmap btmBack = new Bitmap(w1 + 16, hh + 32); //изображение
                pictureBox1.BackgroundImage = btmBack;
            }

            grBack = Graphics.FromImage(pictureBox1.BackgroundImage);

            Graph(buf1, h1, y, grBack, Color.Red,1);                                          // 1 График по y
            Graph(buf2, h1, y, grBack, Color.Blue,1);                                         // 2 График по y
        }


        private void init(ZArrayDescriptor ZZ, int x, int y, Color drawColor)
        {
            int w1 = ZZ.width; w = w1;
            int h1 = ZZ.height; h = h1;
            Int64[] buf = new Int64[Math.Max(w1, h1)];
            buf_gl = new Int64[Math.Max(w1, h1)];
            buf1_gl = new Int64[Math.Max(w1, h1)];


            for (int i = 0; i < w1; i++) { buf[i] = ZZ.array[i, y]; buf_gl[i] = buf[i]; }
            pc1.BackColor = Color.White;
            pc1.Size = new Size(w1 + 16, hh + 32);
            pc1.BorderStyle = BorderStyle.Fixed3D;

            if (pc1.BackgroundImage == null)
            {
                Bitmap btmBack = new Bitmap(w1 + 16, hh + 32); //изображение
                pc1.BackgroundImage = btmBack;
            }

            if (pc1.Image == null)
            {
                Bitmap btmFront = new Bitmap(w1 + 16, hh + 32); //фон
                pc1.Image = btmFront;
            }


            Graphics grBack = Graphics.FromImage(pc1.BackgroundImage);
            
            

            Graph(buf, w1, x, grBack, drawColor);

            for (int i = 0; i < h1; i++) { buf[i] = ZZ.array[x, i]; buf1_gl[i] = buf[i]; }
            pictureBox1.BackColor = Color.White;
            pictureBox1.Size = new Size(w1 + 16, hh + 32);
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            

            if (pictureBox1.Image == null)
            {
                Bitmap btmFront = new Bitmap(w1 + 16, hh + 32); //фон
                pictureBox1.Image = btmFront;
            }

            if (pictureBox1.BackgroundImage == null)
            {
                Bitmap btmBack = new Bitmap(w1 + 16, hh + 32); //изображение
                pictureBox1.BackgroundImage = btmBack;
            }

            grBack = Graphics.FromImage(pictureBox1.BackgroundImage);

            Graph(buf, h1, y, grBack, drawColor);
        }


        //  Перегруженный конструктор для графика таблицы
   
        public Graphic(int w1, int x, Int64[] buf, int[] buf1)
                {
                    InitializeComponent();

   
                    int hh = 256;   //260;
                    w = w1;
                    buf_gl  = new Int64[w1];
                    buf1_gl = new Int64[w1];
                    Int64 maxx = buf[0], minx = buf[0], b;
                    for (int i = 0; i < w1; i++) { b = buf[i]; if (b < minx) minx = b; if (b > maxx) maxx = b; buf_gl[i] = b; } //buf_gl[i] = b; }
                    for (int i = 0; i < w1; i++) { buf[i] = (buf[i] - minx) * hh / (maxx - minx); }
       
                    pc1.BackColor = Color.White;
                    pc1.Size = new Size(w1 + 16, hh + 32);
                    pc1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pc1.BorderStyle = BorderStyle.Fixed3D;
                    Bitmap btmBack = new Bitmap(w1 + 16, hh + 64);      //изображение
                    Bitmap btmFront = new Bitmap(w1 + 16, hh + 64);     //фон
                    Graphics grBack = Graphics.FromImage(btmBack);
                    pc1.Image = btmFront;
                    pc1.BackgroundImage = btmBack;



                    Graph(buf, w1, x, grBack, Color.Red);


          //-------------------------------------------------------------------------------------------------------  Истинные диагонали
            Pen p4 = new Pen(Color.Blue, 1);
            for (int i = 0; i < w1 - 1; i++)
            {
                buf1_gl[i] = buf1[i];
                if (buf1[i] >= 0)
                {
                   grBack.DrawLine(p4, i + x0, 8, i + x0, hh + 8);
                }
            }
        }


    
        private void pc1_MouseMove(object sender, MouseEventArgs e)
        {
                       
                int xPosition = e.X - x0;
                int с_buf1 = 0;
                int с_buf2 = 0; 
              
            if (xPosition >= 0 && xPosition < w)
              {
                с_buf1   = (int) buf_gl[xPosition];
                с_buf2   = (int)buf1_gl[xPosition];
                label4.Text = Convert.ToString(xPosition);
                label5.Text = Convert.ToString(с_buf1);
                label11.Text = Convert.ToString(с_buf2);
              }
        }
        private void pc2_MouseMove(object sender, MouseEventArgs e)
        {

            int xPosition = e.X - x0;
            int с_buf1 = 0;

            if (xPosition >= 0 && xPosition < h)
            {
                с_buf1 = (int)buf1_gl[xPosition];
                label8.Text = Convert.ToString(xPosition);
                label10.Text = Convert.ToString(с_buf1);
            }
        }

        private void Graph(Int64[] buf, int w1, int x, Graphics grBack, Color drawColor, int maxmin=0)
        {
            Int64 b;
//            if ((maxx == 0) && (minx == 0))
            if (maxmin == 0) 
            {
                for (int i = 0; i < w1; i++)
                {
                    b = buf[i];  if (b < minx) minx = b;  if (b > maxx) maxx = b;
                }
                for (int i = 0; i < w1; i++)
                {
                    buf[i] = (buf[i] - minx)*hh/(maxx - minx);
                }
            }

            Pen p1 = new Pen(Color.Black, 1);
            Pen p2 = new Pen(drawColor, 1);
            Pen p3 = new Pen(Color.Green, 1);
            // ------------------------------------------------------------------------------------------------------------График 
            Font drawFont = new Font("Arial", 8);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            //  -----------------------------------------------------------------------------------------------------Ось x
            grBack.DrawLine(p1, x0, hh, w1 + x0, hh);
            for (int i = 0; i < w1; i += 8) grBack.DrawLine(p1, i + x0, hh, i + x0, hh + 8);
            for (int i = 0; i <= w1; i += 64)
            {
                string sx = i.ToString();
                grBack.DrawString(sx, drawFont, drawBrush, i+x0, hh + 11);
            }

            //  -----------------------------------------------------------------------------------------------------Ось y
            grBack.DrawLine(p1, x0, 8, x0, hh + 8);
            for (int i = 8; i < hh + 8; i += 8) grBack.DrawLine(p1, x0, i, x0 + 4, i);      

            double k = (hh) / 32;
            double kx = (maxx - minx) / k;
            double nf = minx;
            long kf;
            for (int i = 0; i <= hh; i += 32)
            {
                kf = (long)nf;
                string sx = kf.ToString();
                grBack.DrawString(sx, drawFont, drawBrush, 2, hh - i);
                nf += kx;
                grBack.DrawLine(p1, x0, i, x0 + w1, i);
            }


            grBack.DrawLine(p3, x + x0, 0, x + x0, hh + 9);                                                                     // Значение координаты


            for (int i = 0; i < w1 - 1; i++)
            {
                grBack.DrawLine(p2, i + x0, hh - buf[i], i + 1 + x0, hh - buf[i + 1]);
            }
        }


        private void MaxMin(Int64[] buf1, Int64[] buf2, int w1)
        {

            Int64 b, max = -4000000, min = 4000000;

            for (int i = 0; i < w1; i++)
            {
                b = buf1[i]; if (b < min) min = b; if (b > max) max = b;
                b = buf2[i]; if (b < min) min = b; if (b > max) max = b;
            }
            for (int i = 0; i < w1; i++)
            {
                buf1[i] = (buf1[i] - min) * hh / (max - min);
                buf2[i] = (buf2[i] - min) * hh / (max - min);
            }
            maxx = max;
            minx = min;

        }
       
    }
}

