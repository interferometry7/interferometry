using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace rab1
{
    public class FiltrClass
    {
        // -------------------------------------------------------------------------------------------- Фильтрация 121
        public static void Filt_121(PictureBox pictureBox01,  ProgressBar progressBar1, int k_filt)
        {            
            int r1 = 0;
            int k = k_filt;
            int k_cntr = 1;

            int w1 = pictureBox01.Image.Width;
            int h1 = pictureBox01.Image.Height;

            int max = w1; if (h1 > max) max = h1;

            int[] k_x = new int[max];
            int[] k_x1 = new int[max];

            //Bitmap bmp1 = new Bitmap(Form1.SelfRef.pictureBox01.Image, w1, h1);
            Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);
            Bitmap bmp2 = new Bitmap(w1, h1);

            Color c;
            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = w1;
            progressBar1.Value = 1;
            progressBar1.Step = 1;
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++) { c = bmp1.GetPixel(i, j); k_x[j] = (int)((double)(c.R + c.G + c.B) / 3); }
                k_cntr = 1;
                for (int ik = k; ik > 0; ik /= 2)
                {
                    if (k_cntr == 1) { for (int j = ik; j < h1 - ik; j++)   k_x1[j] = (k_x[j - ik] + (k_x[j] << 1) + k_x[j + ik]) >> 2; k_cntr = 2; }
                    else { for (int j = ik; j < h1 - ik; j++)   k_x[j] = (k_x1[j - ik] + (k_x1[j] << 1) + k_x1[j + ik]) >> 2; k_cntr = 1; }
                }
                for (int j = 0; j < h1; j++)
                {
                    if (k_cntr == 1) r1 = k_x[j]; else r1 = k_x1[j];
                    bmp2.SetPixel(i, j, Color.FromArgb(r1, r1, r1));
                }
                progressBar1.PerformStep();
            }
            progressBar1.Maximum = h1;
           progressBar1.Value = 1;
            for (int j = 0; j < h1; j++)
            {
                for (int i = 0; i < w1; i++) { c = bmp2.GetPixel(i, j); k_x[i] = (int)((double)(c.R + c.G + c.B) / 3); }
                k_cntr = 1;
                for (int ik = k; ik > 0; ik /= 2)
                {
                    if (k_cntr == 1) { for (int i = ik; i < w1 - ik; i++)   k_x1[i] = (k_x[i - ik] + (k_x[i] << 1) + k_x[i + ik]) >> 2; k_cntr = 2; }
                    else { for (int i = ik; i < w1 - ik; i++)   k_x[i] = (k_x1[i - ik] + (k_x1[i] << 1) + k_x1[i + ik]) >> 2; k_cntr = 1; }
                }
                for (int i = 0; i < w1; i++)
                {
                    if (k_cntr == 1) r1 = k_x[i]; else r1 = k_x1[i];
                    bmp1.SetPixel(i, j, Color.FromArgb(r1, r1, r1));
                }
                 progressBar1.PerformStep();
            }           
            pictureBox01.Size = new System.Drawing.Size(w1, h1);
            pictureBox01.Image = bmp1;
             progressBar1.Value = 1;
        }
 // -------------------------------------------------------------------------------------------------- Медианная фильтрация
       static int[] f_x = new int[100];

       private static int filt_median(int k)
        {
             
             int min = f_x[0], i_min1, i_min2, s, k2=k/2;
             for (int i = 0; i < k; i++)
             {
                 min = f_x[i]; i_min1 = i; i_min2 = i;
                 for (int j = i; j < k;  j++) if (f_x[j] < min) { min = f_x[j]; i_min2 = j; }
                 if (i_min1 != i_min2) {  s = f_x[i_min1]; f_x[i_min1] = f_x[i_min2]; f_x[i_min2] = s; }
             }    
            if (k%2 != 0) s= f_x[k/2]; else s= (f_x[k2]+ f_x[k2-1])/2;
            return s;
         }

       public static void Filt_Mediana(PictureBox pictureBox01, ProgressBar progressBar1, int k_filt)
       {          
           int s = 0;
           int k = k_filt;
           int k2 = k / 2;

           int w1 = pictureBox01.Image.Width;
           int h1 = pictureBox01.Image.Height;

           Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);
           Bitmap bmp2 = new Bitmap(w1, h1);

           Color c;
           progressBar1.Visible = true;
           progressBar1.Minimum = 1;
           progressBar1.Maximum = w1;
           progressBar1.Value = 1;
           progressBar1.Step = 1;

           int max = w1; if (h1 > max) max = h1;
           int[] k_x1 = new int[max];
           int[] k_x2 = new int[max];

           for (int i = 0; i < w1; i++)
           {
               for (int j = 0; j < h1; j++) { c = bmp1.GetPixel(i, j); k_x1[j] = (c.R + c.G + c.B) / 3; }
               for (int j = 0; j < h1 - k; j++)
               {
                   for (int m = 0; m < k; m++) { f_x[m] = k_x1[j + m]; }
                   k_x2[j] = filt_median(k);
               }
               for (int j = 0; j < h1 - k; j++) { s = k_x2[j]; bmp2.SetPixel(i, j + k2, Color.FromArgb(s, s, s)); }
               progressBar1.PerformStep();
           }

           progressBar1.Value = 1;
           progressBar1.Maximum = h1;
           for (int j = 0; j < h1; j++)
           {
               for (int i = 0; i < w1; i++) { c = bmp1.GetPixel(i, j); k_x1[i] = (c.R + c.G + c.B) / 3; }
               for (int i = 0; i < w1 - k; i++)
               {
                   for (int m = 0; m < k; m++) { f_x[m] = k_x1[i + m]; }
                   k_x2[i] = filt_median(k);
               }
               for (int i = 0; i < w1 - k; i++) { s = k_x2[i]; bmp2.SetPixel(i + k2, j, Color.FromArgb(s, s, s)); }
               progressBar1.PerformStep();
           }
           pictureBox01.Size = new System.Drawing.Size(w1, h1);
           pictureBox01.Image = bmp2;
       }


       public static void Filt_Sobel(PictureBox pictureBox01, int k_filt)
       {
           //if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
           int k = k_filt;

           int w1 = pictureBox01.Image.Width;
           int h1 = pictureBox01.Image.Height;

           //int max = w1; if (h1 > max) max = h1;


           Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);
           Bitmap bmp2 = new Bitmap(w1, h1);

           Color c, c1, c2, c3;
           int r0, r1, r2, r3;

           int max = -32000, min = 32000;

           for (int i = 0; i < w1; i++)
           {
               for (int j = k; j < h1 - k; j++)
               {
                   c1 = bmp1.GetPixel(i, j - k); c2 = bmp1.GetPixel(i, j); c3 = bmp1.GetPixel(i, j + k);
                   r1 = (c1.R + c1.G + c1.B) / 3; r2 = (c2.R + c2.G + c2.B) / 3; r3 = (c3.R + c3.G + c3.B) / 3;
                   r0 = Math.Abs(r1 - 2 * r2 + r3) / 2;
                   if (r0 > max) max = r0; if (r0 < min) min = r0;
                   bmp2.SetPixel(i, j, Color.FromArgb(r0, 0, 0));
               }          
           }

           MessageBox.Show("Max =" + max + "Min =" + min);

           for (int j = 0; j < h1; j++)
           {
               for (int i = k; i < w1 - k; i++)
               {
                   c1 = bmp1.GetPixel(i - k, j); c2 = bmp1.GetPixel(i, j); c3 = bmp1.GetPixel(i + k, j);
                   r1 = (c1.R + c1.G + c1.B) / 3; r2 = (c2.R + c2.G + c2.B) / 3; r3 = (c3.R + c3.G + c3.B) / 3;
                   r0 = Math.Abs(r1 - 2 * r2 + r3) / 2;

                   c = bmp2.GetPixel(i, j);
                   if (r0 < c.R) r0 = c.R;
                   if (r0 > max) max = r0; if (r0 < min) min = r0;
                   //if (r0 > 255) r0 = 255;
                   bmp2.SetPixel(i, j, Color.FromArgb(r0, r0, r0));
               }
           }

           for (int j = 0; j < h1; j++)
           {
               for (int i = 0; i < w1; i++)
               {
                   c = bmp2.GetPixel(i, j);
                   r0 = c.R * 255 / max;
                   if (r0 > 100) r0 = 255;
                   bmp2.SetPixel(i, j, Color.FromArgb(r0, r0, r0));
               }
           }

           pictureBox01.Size = new System.Drawing.Size(w1, h1);
           pictureBox01.Image = bmp2;
           MessageBox.Show("Max =" + max + "Min =" + min);
       }

       public static void Filt_BW(PictureBox pictureBox01, int k_filt)
       {
           int k = k_filt;

           int w1 = pictureBox01.Image.Width;
           int h1 = pictureBox01.Image.Height;
           Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);
           Bitmap bmp2 = new Bitmap(w1, h1);

           Color c1;
           int r, r1;

           for (int j = 0; j < h1; j++)
           {
               for (int i = 0; i < w1; i++)
               {
                   c1 = bmp1.GetPixel(i, j);
                   r1 = (c1.R + c1.G + c1.B) / 3;
                   if (r1 > k) r = 255; else r = 0;
                   bmp2.SetPixel(i, j, Color.FromArgb(r, r, r));
               }
           }
           pictureBox01.Image = bmp2;
       }

    }
}
