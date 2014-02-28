using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace rab1
{
    public class FazaClass
    {
        public static void ATAN_N(Image[] img, int k1, int k2, int k3, int k4, ProgressBar progressBar1, double Gamma)
        {
            int w1 = img[0].Width;
            int h1 = img[0].Height;

            int n_sdv = 3;                                                       // Число фазовых сдвигов

            double[] i_sdv = new double[n_sdv];
            double[] v_sdv = new double[n_sdv];                                  // Вектор коэффициентов
            double[] k_sin = new double[n_sdv];
            double[] k_cos = new double[n_sdv];
            double pi = Math.PI;
            //  Сдвиги фаз (3 сдвига - 0, 120, 240 градусов)
            k_sin[0] = Math.Sin(0); k_sin[1] = Math.Sin(2 * pi / 3); k_sin[2] = Math.Sin(4 * pi / 3);
            k_cos[0] = Math.Cos(0); k_cos[1] = Math.Cos(2 * pi / 3); k_cos[2] = Math.Cos(4 * pi / 3);

            Color c;
            int r, g, b;
            double fz, fz1, fz2;
            Bitmap bmp1 = new Bitmap(img[k1], w1, h1);
            Bitmap bmp2 = new Bitmap(img[k2], w1, h1);
            Bitmap bmp3 = new Bitmap(img[k3], w1, h1);
            Bitmap bmp4 = new Bitmap( w1, h1);

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = w1;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

            int[] ims1 = new int[h1];
            int[] ims2 = new int[h1];
            int[] ims3 = new int[h1];

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c = bmp1.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; ims1[j] = (int)Math.Pow(r, Gamma);
                    c = bmp2.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; ims2[j] = (int)Math.Pow(r, Gamma);
                    c = bmp3.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; ims3[j] = (int)Math.Pow(r, Gamma);
                }
                for (int j = 0; j < h1; j++)
                {
                    r = ims1[j]; g = ims2[j]; b = ims3[j];
                    // ------                                     Формула расшифровки
                    i_sdv[0] = r; i_sdv[1] = g; i_sdv[2] = b;
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int ii = 1; ii < n_sdv - 1; ii++) v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1];
                    fz1 = fz2 = 0; for (int ii = 0; ii < n_sdv; ii++) { fz1 += v_sdv[ii] * k_sin[ii]; fz2 += v_sdv[ii] * k_cos[ii]; }

                    fz = Math.Atan2(fz1, fz2) + Math.PI;
                    r = (int)((fz * 255) / (2 * Math.PI));

                    bmp4.SetPixel(i, j, Color.FromArgb(r, r, r));
                }
                progressBar1.PerformStep();
            }

           //img[k4].Size = bmp4.Size;
           img[k4] = bmp4;
        }

        public static void ATAN_123(Image[] img, PictureBox pictureBox01, ProgressBar progressBar1, int n, double[] fzz, double Gamma)
        {
            int w1 = img[0].Width;
            int h1 = img[0].Height;

            int n_sdv = n;                                                       // Число фазовых сдвигов

            double[] i_sdv = new double[4];
            double[] v_sdv = new double[4];                                  // Вектор коэффициентов
            double[] k_sin = new double[4];
            double[] k_cos = new double[4];
            double pi = Math.PI;
          
            for (int i = 0; i < n_sdv; i++) { k_sin[i] = Math.Sin(fzz[i] * pi / 180); k_cos[i] = Math.Cos(fzz[i] * pi / 180); }  //  Сдвиги фаз 
           

            Color c;
            int r;
            double fz, fz1, fz2;
            Bitmap bmp1 = new Bitmap(img[0], w1, h1);
            Bitmap bmp2 = new Bitmap(img[1], w1, h1);
            Bitmap bmp3 = new Bitmap(img[2], w1, h1);
            Bitmap bmp4 = new Bitmap(img[3], w1, h1);
            Bitmap bmp5 = new Bitmap(w1, h1);

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = w1;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

          
          
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c = bmp1.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; i_sdv[0] = (int)Math.Pow(r, Gamma);
                    c = bmp2.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; i_sdv[1] = (int)Math.Pow(r, Gamma);
                    c = bmp3.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; i_sdv[2] = (int)Math.Pow(r, Gamma);
                    c = bmp4.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; i_sdv[3] = (int)Math.Pow(r, Gamma);
               
                  
                    // ------                                     Формула расшифровки
                    
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int ii = 1; ii < n_sdv - 1; ii++) v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1];
                    fz1 = fz2 = 0; for (int ii = 0; ii < n_sdv; ii++) { fz1 += v_sdv[ii] * k_sin[ii]; fz2 += v_sdv[ii] * k_cos[ii]; }

                    fz = Math.Atan2(fz1, fz2) + Math.PI;
                    r = (int)((fz * 255) / (2 * Math.PI));

                    bmp5.SetPixel(i, j, Color.FromArgb(r, r, r));
                }
                progressBar1.PerformStep();
            }
            pictureBox01.Size = bmp4.Size;
            pictureBox01.Image = bmp5;
        }





        public static void ATAN_RGB(PictureBox pictureBox01, ProgressBar progressBar1)
        {

            int w1 = pictureBox01.Image.Width;
            int h1 = pictureBox01.Image.Height;
                    


            int n_sdv = 3;                                                       // Число фазовых сдвигов

            double[] i_sdv = new double[n_sdv];
            double[] v_sdv = new double[n_sdv];                                  // Вектор коэффициентов
            double[] k_sin = new double[n_sdv];
            double[] k_cos = new double[n_sdv];
            double pi = Math.PI;                                                //  Сдвиги фаз
            k_sin[0] = Math.Sin(0); k_sin[1] = Math.Sin(2 * pi / 3); k_sin[2] = Math.Sin(4 * pi / 3);
            k_cos[0] = Math.Cos(0); k_cos[1] = Math.Cos(2 * pi / 3); k_cos[2] = Math.Cos(4 * pi / 3);

            Color c;
            int r, g, b;
            double fz, fz1, fz2, sqrt3 = Math.Sqrt(3);
            Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);
            Bitmap bmp2 = new Bitmap(w1, h1);

           

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = w1;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

            int[] ims1 = new int[h1];
            int[] ims2 = new int[h1];
            int[] ims3 = new int[h1];

            for (int i = 0; i < w1; i++)
            {
               
                for (int j = 0; j < h1; j++)
                {
                    c = bmp1.GetPixel(i, j);


                    r = c.R; g = c.G; b = c.B;
                    // ------                                     Формула расшифровки
                    i_sdv[0] = r; i_sdv[1] = g; i_sdv[2] = b;
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int ii = 1; ii < n_sdv - 1; ii++) v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1];
                    fz1 = fz2 = 0; for (int ii = 0; ii < n_sdv; ii++) { fz1 += v_sdv[ii] * k_sin[ii]; fz2 += v_sdv[ii] * k_cos[ii]; }

                    fz = Math.Atan2(fz1, fz2) + Math.PI;
                    r = (int)((fz * 255) / (2 * Math.PI));

                    bmp2.SetPixel(i, j, Color.FromArgb(r, r, r));
                }
                progressBar1.PerformStep();
            }
            pictureBox01.Image = bmp2;
        }


// ------------------------------------------------------------------------------------------------------------------ Эллипс
        public static void Graph_ATAN(Image[] img, PictureBox pictureBox01, ProgressBar progressBar1, Point p, int x0_end, int x1_end, int y0_end, int y1_end, int n, double[] fzz, double Gamma)
        {
            int w1 = img[0].Width;
            int h1 = img[0].Height;
           
            int n_sdv = n;                                                       // Число фазовых сдвигов

            double[] i_sdv = new double[4];
            double[] v_sdv = new double[4];                                  // Вектор коэффициентов
            double[] k_sin = new double[4];
            double[] k_cos = new double[4];
            double pi = Math.PI;
            for (int i = 0; i < n_sdv; i++) { k_sin[i] = Math.Sin(fzz[i] * pi / 180); k_cos[i] = Math.Cos(fzz[i] * pi / 180); }  //  Сдвиги фаз 

            Color c;
            
          
            Bitmap bmp1 = new Bitmap(img[0], w1, h1);
            Bitmap bmp2 = new Bitmap(img[1], w1, h1);
            Bitmap bmp3 = new Bitmap(img[2], w1, h1);
            Bitmap bmp4 = new Bitmap(img[3], w1, h1);
           

           

            int x0=0, x1=w1, y0=0, y1=h1;
            if (x0_end !=0 || x1_end !=0 || y0_end !=0 || y1_end !=0 ) { x0=x0_end; x1=x1_end;  y0=y0_end; y1=y1_end;}

            int r;
            double fz1, fz2;
 //------------------------------------------------------------------------------------------------------------------------------  Поиск max и min     
            double min1 = 35000, max1 = -35000;
           
            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = x1-x0;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

            for (int i = x0; i < x1; i++)
            {
                for (int j = y0; j < y1; j++)
                {
                    c = bmp1.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; i_sdv[0] = (int)Math.Pow(r, Gamma);
                    c = bmp2.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; i_sdv[1] = (int)Math.Pow(r, Gamma);
                    c = bmp3.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; i_sdv[2] = (int)Math.Pow(r, Gamma);
                    c = bmp4.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; i_sdv[3] = (int)Math.Pow(r, Gamma);

               
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int ii = 1; ii < n_sdv - 1; ii++) v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1];
                    fz1 = fz2 = 0; for (int ii = 0; ii < n_sdv; ii++) { fz2 += v_sdv[ii] * k_sin[ii]; fz1 += v_sdv[ii] * k_cos[ii]; }
                    //----------------------------------------------------------------------------------------
                    max1 = Math.Max(fz1, max1); max1 = Math.Max(fz2, max1);
                    min1 = Math.Min(fz1, min1); min1 = Math.Min(fz2, min1);
                                                                        
                }
                progressBar1.PerformStep();
            }
            int w11 = 600;
            int h11 = 600;
            int x, y;
            double fw = (int)w11;
            max1 = max1 - min1;
            Bitmap bmp5 = new Bitmap(w11, h11);
// ------------------------------------------------------------------------------------------------------------------------ 
            progressBar1.Value = 1;
            for (int i = x0; i < x1; i++)
            {
                for (int j = y0; j < y1; j++)
                {
                    c = bmp1.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; i_sdv[0] = (int)Math.Pow(r, Gamma);
                    c = bmp2.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; i_sdv[1] = (int)Math.Pow(r, Gamma);
                    c = bmp3.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; i_sdv[2] = (int)Math.Pow(r, Gamma);
                    c = bmp4.GetPixel(i, j); r = (c.R + c.G + c.B) / 3; i_sdv[3] = (int)Math.Pow(r, Gamma);
               
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int ii = 1; ii < n_sdv - 1; ii++) v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1];
                    fz1 = fz2 = 0; for (int ii = 0; ii < n_sdv; ii++) { fz2 += v_sdv[ii] * k_sin[ii]; fz1 += v_sdv[ii] * k_cos[ii]; }
                    //-------------------------------------------------------------------------------------------

                    x = (int)((fz1-min1) * fw / max1); y = (int)((fz2-min1) * fw / max1);

                    if ((x < w11 && x >= 0) && (y < h11 && y >= 0))
                    {
                        c = bmp4.GetPixel(x, y); r = c.R; r++;
                        if (r > 255) r = 255;
                        bmp5.SetPixel(x, y, Color.FromArgb(250, 0, 0));
                    }
                }
                progressBar1.PerformStep();
            }

            pictureBox01.Image = bmp5;
            pictureBox01.Size = bmp4.Size;
        }

    }
}
