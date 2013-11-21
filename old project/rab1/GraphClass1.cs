using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace rab1
{
    public class GraphClass1
    {


        public static void grfk(int w1, int h1, int x, int y, int[] buf, int[] bufy, Pen p2)
        {
            int hh = 260;

            Form f2 = new Form();
            f2.Size = new Size(w1 + 38, 2 * hh + 48);

            PictureBox pc1 = new PictureBox();
            pc1.BackColor = Color.White;
            pc1.Location = new System.Drawing.Point(0, 8);
            pc1.Size = new Size(w1 + 16, hh + 16);
            pc1.SizeMode = PictureBoxSizeMode.StretchImage;
            pc1.BorderStyle = BorderStyle.Fixed3D;
            Bitmap btmBack = new Bitmap(w1 + 16, hh + 16);      //изображение
            Bitmap btmFront = new Bitmap(w1 + 16, hh + 16);     //фон
            Graphics grBack = Graphics.FromImage(btmBack);
            Graphics grFront = Graphics.FromImage(btmFront);  //лучше объявить заранее глобально.
            pc1.Image = btmFront;
            pc1.BackgroundImage = btmBack;


            PictureBox pc2 = new PictureBox();
            pc2.BackColor = Color.White;
            pc2.Location = new System.Drawing.Point(0, hh + 24);
            pc2.Size = new Size(w1 + 16, hh + 16);
            pc2.SizeMode = PictureBoxSizeMode.StretchImage;
            pc2.BorderStyle = BorderStyle.Fixed3D;
            Bitmap btmBack2 = new Bitmap(w1 + 16, hh + 16);      //изображение
            Bitmap btmFront2 = new Bitmap(w1 + 16, hh + 16);     //фон
            Graphics grBack2 = Graphics.FromImage(btmBack2);
            Graphics grFront2 = Graphics.FromImage(btmFront2);  //лучше объявить заранее глобально.
            pc2.Image = btmFront2;
            pc2.BackgroundImage = btmBack2;
            // График по x
            Pen p1 = new Pen(Color.Black, 1);
            //Pen p2 = new Pen(Color.Red, 1);
            Pen p3 = new Pen(Color.Green, 1);

            grBack.DrawLine(p1, 8, 0, 8, hh - 8);
            grBack.DrawLine(p1, 8, hh - 8, w1 + 8, hh - 8);
            grBack.DrawLine(p3, x + 8, 0, x + 8, hh - 8);
            for (int i = 0; i < w1; i += 8) grBack.DrawLine(p1, i + 8, hh - 12, i + 8, hh - 8);
            for (int i = 0; i < 255; i += 8) grBack.DrawLine(p1, 8, i, 12, i);


            for (int i = 0; i < w1 - 1; i++) grBack.DrawLine(p2, i + 8, 255 - buf[i], i + 1 + 8, 255 - buf[i + 1]);
            // График по y                
            grBack2.DrawLine(p1, 8, 0, 8, hh - 8);
            grBack2.DrawLine(p1, 8, hh - 8, w1 + 8, hh - 8);
            grBack2.DrawLine(p3, y + 8, 0, y + 8, hh - 8);
            for (int i = 0; i < w1; i += 8) grBack2.DrawLine(p1, i + 8, hh - 12, i + 8, hh - 8);
            for (int i = 0; i < 255; i += 8) grBack2.DrawLine(p1, 8, i, 12, i);


            for (int i = 0; i < h1 - 1; i++) grBack2.DrawLine(p2, i + 8, 255 - bufy[i], i + 1 + 8, 255 - bufy[i + 1]);


           
            f2.Show();

        }

    }
}
