using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rab1.Forms
{
    public partial class Graphic : Form
    {
        public Graphic(int w1, int x, Int64[] buf)
        {
            InitializeComponent();


            int hh = 511;   //260;

            Int64 maxx = buf[0], minx = buf[0], b;
            for (int i = 0; i < w1; i++) { b = buf[i]; if (b < minx) minx = b; if (b > maxx) maxx = b; }

            for (int i = 0; i < w1; i++) { buf[i] = (buf[i] - minx) * hh / (maxx - minx); }


            Font font = new Font("Courier", 12, FontStyle.Regular); //, GraphicsUnit.Pixel)Regular;
            StringFormat drawFormat = new StringFormat(StringFormatFlags.NoClip); //   .  NoClip);
            string sx = " minx =  " + minx + "  maxx =  " + maxx;


            pc1.BackColor = Color.White;
            pc1.Location = new System.Drawing.Point(0, 8);
            pc1.Size = new Size(w1 + 16, hh + 64);
            pc1.SizeMode = PictureBoxSizeMode.StretchImage;
            pc1.BorderStyle = BorderStyle.Fixed3D;
            Bitmap btmBack = new Bitmap(w1 + 16, hh + 64);      //изображение
            Bitmap btmFront = new Bitmap(w1 + 16, hh + 64);     //фон
            Graphics grBack = Graphics.FromImage(btmBack);
            //Graphics grFront = Graphics.FromImage(btmFront);  //лучше объявить заранее глобально.
            pc1.Image = btmFront;
            pc1.BackgroundImage = btmBack;




            Pen p1 = new Pen(Color.Black, 1);
            Pen p2 = new Pen(Color.Red, 1);
            Pen p3 = new Pen(Color.Green, 1);
            // ------------------------------------------------------------------------------------------------------------График по x   

            //  Ось x
            grBack.DrawLine(p1, 8, hh + 9, w1 + 8, hh + 9);
            for (int i = 0; i < w1; i += 8) grBack.DrawLine(p1, i + 8, hh + 1, i + 8, hh + 9);
            grBack.DrawString(sx, font, new SolidBrush(Color.Black), 48, hh + 25, drawFormat);

            //  Ось y
            grBack.DrawLine(p1, 8, 8, 8, hh + 8);
            for (int i = 8; i < hh + 8; i += 8) grBack.DrawLine(p1, 8, i, 12, i);

            grBack.DrawLine(p3, x + 8, 0, x + 8, hh + 9);       // Значение координаты




            for (int i = 0; i < w1 - 1; i++) grBack.DrawLine(p2, i + 8, hh - buf[i] + 8, i + 1 + 8, hh - buf[i + 1] + 8);




            pc1.Refresh();

            Controls.Add(pc1);
        }


        //  Перегруженный конструктор
        public Graphic(int w1, int x, Int64[] buf, int[] buf1)
        {
            InitializeComponent();


            int hh = 511;   //260;

            Int64 maxx = buf[0], minx = buf[0], b;
            for (int i = 0; i < w1; i++) { b = buf[i]; if (b < minx) minx = b; if (b > maxx) maxx = b; }

            for (int i = 0; i < w1; i++) { buf[i] = (buf[i] - minx) * hh / (maxx - minx); }


            Font font = new Font("Courier", 12, FontStyle.Regular); //, GraphicsUnit.Pixel)Regular;
            StringFormat drawFormat = new StringFormat(StringFormatFlags.NoClip); //   .  NoClip);
            string sx = " minx =  " + minx + "  maxx =  " + maxx;


            pc1.BackColor = Color.White;
            pc1.Location = new System.Drawing.Point(0, 8);
            pc1.Size = new Size(w1 + 16, hh + 64);
            pc1.SizeMode = PictureBoxSizeMode.StretchImage;
            pc1.BorderStyle = BorderStyle.Fixed3D;
            Bitmap btmBack = new Bitmap(w1 + 16, hh + 64);      //изображение
            Bitmap btmFront = new Bitmap(w1 + 16, hh + 64);     //фон
            Graphics grBack = Graphics.FromImage(btmBack);
            //Graphics grFront = Graphics.FromImage(btmFront);  //лучше объявить заранее глобально.
            pc1.Image = btmFront;
            pc1.BackgroundImage = btmBack;




            Pen p1 = new Pen(Color.Black, 1);
            Pen p2 = new Pen(Color.Red, 1);
            Pen p3 = new Pen(Color.Green, 1);
            Pen p4 = new Pen(Color.Blue, 1);
            // ------------------------------------------------------------------------------------------------------------График по x   

            //  Ось x
            grBack.DrawLine(p1, 8, hh + 9, w1 + 8, hh + 9);
            for (int i = 0; i < w1; i += 8) grBack.DrawLine(p1, i + 8, hh + 1, i + 8, hh + 9);
            grBack.DrawString(sx, font, new SolidBrush(Color.Black), 48, hh + 25, drawFormat);

            //  Ось y
            grBack.DrawLine(p1, 8, 8, 8, hh + 8);
            for (int i = 8; i < hh + 8; i += 8) grBack.DrawLine(p1, 8, i, 12, i);

            grBack.DrawLine(p3, x + 8, 0, x + 8, hh + 9);       // Значение координаты




            for (int i = 0; i < w1 - 1; i++) grBack.DrawLine(p2, i + 8, hh - buf[i] + 8, i + 1 + 8, hh - buf[i + 1] + 8);

            for (int i = 0; i < w1 - 1; i++)
            {
                //string sx1 = " i =  " + i + "  buf1[i] =  " + buf1[i];

                if (buf1[i] >= 0)
                {
                    //MessageBox.Show(sx1);
                    grBack.DrawLine(p4, i + 8, 8, i + 8, hh + 8);
                }
                
            }



            pc1.Refresh();

            Controls.Add(pc1);
        }
    }
}
