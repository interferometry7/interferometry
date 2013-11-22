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


        public static void grfk(int w1, int x, int y,  Int64[,] Z)
        {
            int hh = 511;   //260;
            int [] buf=new int[w1];
          
            int maxx = 0, minx = 0, b=0; 
            for (int i = 0; i < w1; i++){  b =  (int) Z[i, y]; if (b < minx) minx = b; if (b > maxx) maxx = b; buf[i] = b;}
            
            for (int i = 0; i < w1; i++) { buf[i] = (buf[i] - minx) * hh / (maxx - minx); }
           

            


            Font font = new Font("Arial", 12, FontStyle.Regular); //, GraphicsUnit.Pixel);
            StringFormat drawFormat = new StringFormat(StringFormatFlags.NoClip);
            string sx = " minx =  " + minx + "  maxx =  " + maxx;
            
           
            Form f2 = new Form();
            f2.Size = new Size(w1 + 38,  hh + 92);

            PictureBox pc1 = new PictureBox();
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
            grBack.DrawString(sx, font, new SolidBrush(Color.Black), 32, hh + 24, drawFormat);
            
                //  Ось y
            grBack.DrawLine(p1, 8, 8, 8, hh +8);
            for (int i = 8; i < hh + 8; i += 8) grBack.DrawLine(p1, 8, i, 12, i);
            
            grBack.DrawLine(p3, x + 8, 0, x + 8, hh + 9);       // Значение координаты
           
           


            for (int i = 0; i < w1 - 1; i++) grBack.DrawLine(p2, i + 8, hh - buf[i] + 8, i + 1 + 8, hh - buf[i + 1] + 8);
           
            
         

            pc1.Refresh();
          
            f2.Controls.Add(pc1);
         
            f2.Show();

        }

    }
}
