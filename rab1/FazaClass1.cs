using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using rab1;

public delegate void ImageProcessed(Bitmap resultBitmap);
public delegate void ImageProcessedForOpenGL(List<Point3D> points);
//  ATAN
namespace rab1
{
    public class FazaClass
    {
        public static event ImageProcessed imageProcessed;
        public static event ImageProcessedForOpenGL imageProcessedForOpenGL;

        public static void ATAN_N(Image[] img, int k1, int k2, int k3, int k4, double Gamma)
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
            }
           img[k4] = bmp4;
        }

        public static void ATAN_123(Image[] img, PictureBox pictureBox01, int n, double[] fzz, double Gamma)
        {
            int w1 = img[0].Width;
            int h1 = img[0].Height;

            int n_sdv = n;                                                       // Число фазовых сдвигов

            double[] i_sdv = new double[4];
            double[] v_sdv = new double[4];                                  // Вектор коэффициентов
            double[] k_sin = new double[4];
            double[] k_cos = new double[4];
            double pi = Math.PI;
            double pi2 = Math.PI*2;
          
            for (int i = 0; i < n_sdv; i++) 
            { 
                k_sin[i] = Math.Sin(fzz[i] * pi / 180);
                k_cos[i] = Math.Cos(fzz[i] * pi / 180); 
            }


            BitmapData data1 = ImageProcessor.getBitmapData((Bitmap)img[0]);
            BitmapData data2 = ImageProcessor.getBitmapData((Bitmap)img[1]);
            BitmapData data3 = ImageProcessor.getBitmapData((Bitmap)img[2]);
            BitmapData data4 = ImageProcessor.getBitmapData((Bitmap)img[3]);

            Color c;
            int r;
            double fz, fz1, fz2;
            Bitmap bmp5 = new Bitmap(w1, h1);

            BitmapData data5 = ImageProcessor.getBitmapData(bmp5);
          
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c = ImageProcessor.getPixel(i, j, data1);  r = (c.R + c.G + c.B) / 3;  i_sdv[0] = (int)Math.Pow(r, Gamma);
                    c = ImageProcessor.getPixel(i, j, data2);  r = (c.R + c.G + c.B) / 3;  i_sdv[1] = (int)Math.Pow(r, Gamma);
                    c = ImageProcessor.getPixel(i, j, data3);  r = (c.R + c.G + c.B) / 3;  i_sdv[2] = (int)Math.Pow(r, Gamma);
                    c = ImageProcessor.getPixel(i, j, data4);  r = (c.R + c.G + c.B) / 3;  i_sdv[3] = (int)Math.Pow(r, Gamma);
                                 
                    // ------                                     Формула расшифровки
                    
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];

                    for (int ii = 1; ii < n_sdv - 1; ii++)
                    {
                        v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1];
                    }

                    fz1 = fz2 = 0; 
                    
                    for (int ii = 0; ii < n_sdv; ii++)
                    { 
                        fz1 += v_sdv[ii] * k_sin[ii]; 
                        fz2 += v_sdv[ii] * k_cos[ii];
                    }

                    fz = Math.Atan2(fz1, fz2) + pi;
                    r = (int)((fz * 255) / pi2);

                    //bmp5.SetPixel(i, j, Color.FromArgb(r, r, r));
                    ImageProcessor.setPixel(data5, i, j, Color.FromArgb(r, r, r)); 
                }
            }

            ((Bitmap)img[0]).UnlockBits(data1);
            ((Bitmap)img[1]).UnlockBits(data2);
            ((Bitmap)img[2]).UnlockBits(data3);
            ((Bitmap)img[3]).UnlockBits(data4);
            bmp5.UnlockBits(data5);

            //pictureBox01.Size = bmp5.Size;
            pictureBox01.Image = bmp5;
        }
//-----------------------------------------------------------------------------------------------------------------------------------------
        public static void ATAN_1234(ref double [,] Float_Image, Image[] img, PictureBox pictureBox01, int n, double[] fzz, double Gamma)
        {
            int w1 = img[0].Width;
            int h1 = img[0].Height;
            double[,] Float_Tmp = new double[w1, h1];                        // массив для значений фаз

            int n_sdv = n;                                                   // Число фазовых сдвигов

            double[] i_sdv = new double[4];
            double[] v_sdv = new double[4];                                  // Вектор коэффициентов
            double[] k_sin = new double[4];
            double[] k_cos = new double[4];
            double pi = Math.PI;
            double pi2 = 255/(Math.PI * 2);

            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fzz[i] * pi / 180);
                k_cos[i] = Math.Cos(fzz[i] * pi / 180);
            }


            BitmapData data1 = ImageProcessor.getBitmapData((Bitmap)img[0]);
            BitmapData data2 = ImageProcessor.getBitmapData((Bitmap)img[1]);
            BitmapData data3 = ImageProcessor.getBitmapData((Bitmap)img[2]);
            BitmapData data4 = ImageProcessor.getBitmapData((Bitmap)img[3]);

            Color c;
            int r;
            double fz, fz1, fz2;
            Bitmap bmp5 = new Bitmap(w1, h1);

            BitmapData data5 = ImageProcessor.getBitmapData(bmp5);

            int all = w1; 
            int done = 0; 
            PopupProgressBar.show();

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c = ImageProcessor.getPixel(i, j, data1); r = (c.R + c.G + c.B) / 3; i_sdv[0] = (int)Math.Pow(r, Gamma);
                    c = ImageProcessor.getPixel(i, j, data2); r = (c.R + c.G + c.B) / 3; i_sdv[1] = (int)Math.Pow(r, Gamma);
                    c = ImageProcessor.getPixel(i, j, data3); r = (c.R + c.G + c.B) / 3; i_sdv[2] = (int)Math.Pow(r, Gamma);
                    c = ImageProcessor.getPixel(i, j, data4); r = (c.R + c.G + c.B) / 3; i_sdv[3] = (int)Math.Pow(r, Gamma);

                    // ------                                     Формула расшифровки

                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];

                    for (int ii = 1; ii < n_sdv - 1; ii++)
                    {
                        v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1];
                    }

                    fz1 = fz2 = 0;

                    for (int ii = 0; ii < n_sdv; ii++)
                    {
                        fz1 += v_sdv[ii] * k_sin[ii];
                        fz2 += v_sdv[ii] * k_cos[ii];
                    }

                    fz = (Math.Atan2(fz1, fz2) + pi)*pi2;

                    Float_Tmp[i, j] = fz ;
                    r = (int)(fz);

                    //bmp5.SetPixel(i, j, Color.FromArgb(r, r, r));
                    ImageProcessor.setPixel(data5, i, j, Color.FromArgb(r, r, r));
                }
                done++;  PopupProgressBar.setProgress(done, all);
            }

            ((Bitmap)img[0]).UnlockBits(data1);
            ((Bitmap)img[1]).UnlockBits(data2);
            ((Bitmap)img[2]).UnlockBits(data3);
            ((Bitmap)img[3]).UnlockBits(data4);
            bmp5.UnlockBits(data5);

            //pictureBox01.Size = bmp5.Size;
            pictureBox01.Image = bmp5;
            PopupProgressBar.close();
            
            Float_Image = Float_Tmp;
           // Array.Copy(Float_Image, Float_Tmp, Float_Tmp.Length);
           
        }


        public static void ATAN_RGB(PictureBox pictureBox01)
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
            }
            pictureBox01.Image = bmp2;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void Graph_ATAN(Image[] img, int x0_end, int x1_end, int y0_end, int y1_end, int n, double[] fzz, double Gamma)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            int w1 = img[0].Width;
            int h1 = img[0].Height;
           
            int n_sdv = n;                                                       // Число фазовых сдвигов

            int[] i_sdv = new int[4];
            int [] v_sdv = new int[4];
           
            Color c;

            Bitmap bmp1 = (Bitmap)img[0];
            Bitmap bmp2 = (Bitmap)img[1];
            Bitmap bmp3 = (Bitmap)img[2];
            Bitmap bmp4 = (Bitmap)img[3];


            BitmapData data1 = ImageProcessor.getBitmapData(bmp1);
            BitmapData data2 = ImageProcessor.getBitmapData(bmp2);
            BitmapData data3 = ImageProcessor.getBitmapData(bmp3);
            BitmapData data4 = ImageProcessor.getBitmapData(bmp4);

            int x0=0, x1=w1, y0=0, y1=h1;
            if (x0_end !=0 || x1_end !=0 || y0_end !=0 || y1_end !=0 ) 
            { 
                x0=x0_end; x1=x1_end;  y0=y0_end; y1=y1_end;
            }

            int r;
            double fz1, fz2;

            int[,] buffer1 = new int[x1, y1];
            int[,] buffer2 = new int[x1, y1];
            int[,] buffer3 = new int[x1, y1];
            int[,] buffer4 = new int[x1, y1];

            double[] k_sin = new double[4];
            double[] k_cos = new double[4];
            double[,] vvs = new double[4, 512];
            double[,] vvc = new double[4, 512];
            double pi = Math.PI;
            for (int i = 0; i < n_sdv; i++) { k_sin[i] = Math.Sin(fzz[i] * pi / 180); k_cos[i] = Math.Cos(fzz[i] * pi / 180); }  //  Сдвиги фаз 
            for (int ii = 0; ii < n_sdv; ii++) for (int i = 0; i < 512; i++) { vvs[ii, i] = i * k_sin[ii]; vvc[ii, i] = i * k_cos[ii]; } 
  
            double min1 = 35000, max1 = -35000;

            int w11 = 600;
            int h11 = 600;

            int all = x1 * y1 * 2 + w11 + h11;
            int done = 0;
            
            for (int i = x0; i < x1; i++)
            {
                done = i * x1;

                PopupProgressBar.setProgress(done, all);

                for (int j = y0; j < y1; j++)
                {
                    c = ImageProcessor.getPixel(i, j, data1);
                    r = (c.R + c.G + c.B) / 3;      
                    i_sdv[0] = (int)Math.Pow(r, Gamma);     
                    buffer1[i, j] = i_sdv[0];

                    c = ImageProcessor.getPixel(i, j, data2);
                    r = (c.R + c.G + c.B) / 3;   
                    i_sdv[1] = (int)Math.Pow(r, Gamma);  
                    buffer2[i, j] = i_sdv[1];

                    c = ImageProcessor.getPixel(i, j, data3);
                    r = (c.R + c.G + c.B) / 3;   
                    i_sdv[2] = (int)Math.Pow(r, Gamma); 
                    buffer3[i, j] = i_sdv[2];

                    c = ImageProcessor.getPixel(i, j, data4);
                    r = (c.R + c.G + c.B) / 3;   
                    i_sdv[3] = (int)Math.Pow(r, Gamma);   
                    buffer4[i, j] = i_sdv[3];
               
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1] + 255;
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2] +255;

                    for (int ii = 1; ii < n_sdv - 1; ii++)
                    {  
                        v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1] + 255; 
                    }

                    fz1 = fz2 = 0; 

                    for (int ii = 0; ii < n_sdv; ii++) 
                    { 
                       fz2 += vvs[ii, v_sdv[ii]];
                       fz1 += vvc[ii, v_sdv[ii]];
                    }

                    max1 = Math.Max(fz1, max1); 
                    max1 = Math.Max(fz2, max1);
                    min1 = Math.Min(fz1, min1); 
                    min1 = Math.Min(fz2, min1);                              
                }
            }
            
            int x, y;
            double fw = (int)w11;
            fw = fw / (max1-min1);
            Bitmap bmp5 = new Bitmap(w11, h11);
            BitmapData data5 = ImageProcessor.getBitmapData(bmp5);
            int[,] buffer5 = new int[w11, h11];

            
            for (int i = x0; i < x1; i++)
            {
                done += x1;
                PopupProgressBar.setProgress(done, all);

                for (int j = y0; j < y1; j++)
                {
                    i_sdv[0] = buffer1[i, j]; 
                    i_sdv[1] = buffer2[i, j]; 
                    i_sdv[2] = buffer3[i, j]; 
                    i_sdv[3] = buffer4[i, j];

                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1] + 255;
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2] + 255;
                    for (int ii = 1; ii < n_sdv - 1; ii++) { v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1] + 255; }


                    fz1 = fz2 = 0;
                    for (int ii = 0; ii < n_sdv; ii++)
                    {
                        fz2 += vvs[ii, v_sdv[ii]];
                        fz1 += vvc[ii, v_sdv[ii]];
                    }
                   
                    //-------------------------------------------------------------------------------------------

                    x = (int)((fz1-min1) * fw);
                    y = (int)((fz2-min1) * fw) ;

                    if ((x < w11 && x >= 0) && (y < h11 && y >= 0))
                    {
                        r = buffer5[x, y]; r++;
                        if (r > 255) { r = 255;  }
                        buffer5[x, y]=r;
                    }
                }
            }
            for (int i = 0; i < w11; i++)
            {
                done += w11;

                PopupProgressBar.setProgress(done, all);

                for (int j = 0; j < h11; j++)
                {
                    ImageProcessor.setPixel(data5, i, j, Color.FromArgb(buffer5[i, j], 0, 0));
                }
            }

            PopupProgressBar.setProgress(all, all);

            bmp1.UnlockBits(data1);
            bmp2.UnlockBits(data2);
            bmp3.UnlockBits(data3);
            bmp4.UnlockBits(data4);
            bmp5.UnlockBits(data5);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (imageProcessed != null)
            {
                imageProcessed(bmp5);
            }

            if (imageProcessedForOpenGL != null)
            {
                List<Point3D> newList = new List<Point3D>();

                for (int i = x0; i < x1; i++)
                {
                    for (int j = y0; j < y1; j++)
                    {
                        newList.Add(new Point3D(buffer1[i, j], buffer2[i, j], buffer3[i, j])); 
                    }                    
                }

                imageProcessedForOpenGL(newList);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
