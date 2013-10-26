using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System.Drawing;
using System.Windows.Forms;


namespace rab1
{
    public  class SumClass
    {
        public static void Sum_Color(Image[] img, int k1, int k2, int k3)
        {
            int w1 = img[k1-1].Width;
            int h1 = img[k1-1].Height;
            Bitmap bmp1 = new Bitmap(img[k1-1], w1, h1);
            

            int w2 = img[k2 - 1].Width;
            int h2 = img[k2 - 1].Height;
            Bitmap bmp2 = new Bitmap(img[k2 - 1], w2, h2);
           

            w1 = (int)Math.Min(w1, w2);
            h1 = (int)Math.Min(h1, h2);
           
            Bitmap bmp3 = new Bitmap(img[k3 - 1], w1, h1);

            Color c1,c2;
            int r1, r2, rs , rg, rb;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c1 = bmp1.GetPixel(i, j);
                    c2 = bmp2.GetPixel(i, j);
                    r1 = c1.R; r2 = c2.R; rs = r1 + r2; if (rs > 255) rs = 255;
                    r1 = c1.G; r2 = c2.G; rg = r1 + r2; if (rg > 255) rg = 255;
                    r1 = c1.B; r2 = c2.B; rb = r1 + r2; if (rb > 255) rb = 255;
                    bmp3.SetPixel(i, j, Color.FromArgb(rs, rg, rb));
                }
            }
            img[k3 - 1] = bmp3;
           
        }


        public static void Sub_Color(Image[] img, int k1, int k2, int k3, double N1, double N2)
        {
            int w1 = img[k1 - 1].Width;
            int h1 = img[k1 - 1].Height;
            Bitmap bmp1 = new Bitmap(img[k1 - 1], w1, h1);


            int w2 = img[k2 - 1].Width;
            int h2 = img[k2 - 1].Height;
            Bitmap bmp2 = new Bitmap(img[k2 - 1], w2, h2);


            w1 = (int)Math.Min(w1, w2);
            h1 = (int)Math.Min(h1, h2);

            Bitmap bmp3 = new Bitmap(img[k3 - 1], w1, h1);

            Color c1, c2;
            int r1, r2, rs;
            int max=-32000, min=32000;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c1 = bmp1.GetPixel(i, j);  r1 = (int) (c1.R * N1);
                    c2 = bmp2.GetPixel(i, j);  r2 = (int) (c2.R * N2);
                    rs = (r1 - r2);
                    min = Math.Min(rs, min);
                    max = Math.Max(rs, max);
                }
            }
// ---------------------------------------------------------------------------------------------------
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c1 = bmp1.GetPixel(i, j); r1 = (int) (c1.R * N1);
                    c2 = bmp2.GetPixel(i, j); r2 = (int) (c2.R * N2);
                    rs = (r1 - r2);
                    rs = (rs - min) * 255 / (max - min);
                    bmp3.SetPixel(i, j, Color.FromArgb(rs, rs, rs));
                }
            }
            img[k3 - 1] = bmp3;

        }
    }
}
