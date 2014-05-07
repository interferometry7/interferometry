using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using Interferometry.forms;
using Interferometry.math_classes;
using rab1;

public delegate void ImageProcessed(Bitmap resultBitmap);

namespace Interferometry
{
    public class FazaClass
    {
        //----------------------------------------------------------------------------------------------
        // Восстановление полной фазы по известной востановленной целочисленным методом
        //----------------------------------------------------------------------------------------------
        public static ZArrayDescriptor Pi2_V(ZArrayDescriptor f1, ZArrayDescriptor fp, int sineNumber, int sdvg)
        {
            int w1 = f1.width;
            int h1 = f1.height;
            long[,] result = new long[w1, h1];

            int all = w1;
            int done = 0;
            PopupProgressBar.show();

            for (int i = 0; i < w1-4; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    long a = f1.array[i, j]; a += sdvg; if (a > sineNumber) a -= sineNumber;
                    long b = fp.array[i, j];

                    while (Math.Abs(a - b) >= sineNumber)
                    {
                        if (a < b) a += sineNumber;
                        if (a > b) a -= sineNumber;
                    }
                    if (Math.Abs(a - b) >= 80)
                    {
                        if (a < b) a += sineNumber;
                        if (a > b) a -= sineNumber;

                    } 
                    result[i, j] = a;

                }
                done++;
                PopupProgressBar.setProgress(done, all);
            }

            PopupProgressBar.close();
            ZArrayDescriptor wrappedPhase = new ZArrayDescriptor();
            wrappedPhase.array = result;
            wrappedPhase.width = w1;
            wrappedPhase.height = h1;

            return wrappedPhase;
        }
        
        public static ZArrayDescriptor ATAN_1234(ZArrayDescriptor[] img, double[] fzz, int sineNumber)
        {
            
            int w1 = img[0].width;
            int h1 = img[0].height;
            long[,] result = new long[w1, h1];                        // массив для значений фаз

            int n_sdv = img.Length;                                   // Число фазовых сдвигов   
            double[] i_sdv = new double[n_sdv];
            double[] v_sdv = new double[n_sdv];                                  // Вектор коэффициентов
            double[] k_sin = new double[n_sdv];
            double[] k_cos = new double[n_sdv];

            double pi = Math.PI;
            double pi2 = sineNumber / (Math.PI * 2);

            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fzz[i] * pi / 180);                          // Перевод в радианы
                k_cos[i] = Math.Cos(fzz[i] * pi / 180);
            }

            //int Gamma = 1;

            int all = w1; 
            int done = 0; 
            PopupProgressBar.show();

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                   // i_sdv[0] = (int)Math.Pow(img[0].array[i, j], Gamma);
                   // i_sdv[1] = (int)Math.Pow(img[1].array[i, j], Gamma);
                   // i_sdv[2] = (int)Math.Pow(img[2].array[i, j], Gamma);
                   // i_sdv[3] = (int)Math.Pow(img[3].array[i, j], Gamma);
                    for (int ii = 0; ii < n_sdv; ii++)
                    {
                        i_sdv[ii] = img[ii].array[i, j];
                    }
                   // i_sdv[0] = img[0].array[i, j];
                   // i_sdv[1] = img[1].array[i, j];
                   // i_sdv[2] = img[2].array[i, j];
                   // i_sdv[3] = img[3].array[i, j];

                    // ------                                     Формула расшифровки

                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];

                    for (int ii = 1; ii < n_sdv - 1; ii++)
                    {
                        v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1];
                    }

                    double fz2;
                    double fz1 = fz2 = 0;

                    for (int ii = 0; ii < n_sdv; ii++)
                    {
                        fz1 += v_sdv[ii] * k_sin[ii];
                        fz2 += v_sdv[ii] * k_cos[ii];
                    }

                    result[i, j] = (long) ((Math.Atan2(fz1, fz2) + pi) * pi2);
                }

                done++;  
                PopupProgressBar.setProgress(done, all);
            }

            PopupProgressBar.close();

            ZArrayDescriptor wrappedPhase = new ZArrayDescriptor();
            wrappedPhase.array = result;
            wrappedPhase.width = w1;
            wrappedPhase.height = h1;

            return wrappedPhase;
        }


        public static ZArrayDescriptor ATAN_Carre(ZArrayDescriptor[] img, int sineNumber)
        {

            int w1 = img[0].width;
            int h1 = img[0].height;
            long[,] result = new long[w1, h1];                        // массив для значений фаз

                                            // Число фазовых сдвигов  
           
            double pi = Math.PI;
            double pi2 = sineNumber / (Math.PI * 2);
            double tg;
            double max = -99999;
            double min = 99999;
            //                     x>0  y>0         atan(y/x)
            //                     x<0  y>=0        atan(y/x) + pi
            // atan2(y,x) =        x<0  y<0         atan(y/x) - pi
            //                     x=0  y>0          pi/2
            //                     x=0  y<0          -pi/2
            //                     x=0  y=0          undefined
            int all = w1;
            int done = 0;
            PopupProgressBar.show();

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    long i1 = img[0].array[i, j];       // ------         Формула расшифровки
                    long i2 =  img[1].array[i, j];
                    long i3 =  img[2].array[i, j];
                    long i4 =  img[3].array[i, j];
                    double ay = (i1 - i4) + (i2 - i3);
                    double ax = (i2 + i3) - (i1 + i4);
                    double c = 3 * (i2 - i3) - (i1 - i4);

                    double  r = 0;
                    if ( (ax == 0) && (ay > 0) )  { r = (-pi/2 + pi + pi/4); }
                    if ((ax == 0) && (ay < 0))    { r = (+pi / 2 + pi + pi / 4); }
                    if ((ax >= 0) && (ay == 0))   { r = 0 + pi + pi / 4; }
                    if ((ax < 0) && (ay == 0))    { r = (-pi + pi + pi / 4); }

                    double x = Math.Abs(ax);
                    double y = Math.Abs(ay * c);
                    double b = Math.Sqrt(y) /x;
                    tg = Math.Atan(b); 
                   // if (tg == Double.NaN) MessageBox.Show(" Double.NaN  ");
                   // if (tg == Double.NegativeInfinity) MessageBox.Show(" NegativeInfinity  ");
                   // if (tg == Double.PositiveInfinity) MessageBox.Show(" PositiveInfinity  "); 
                                       
                   
                   if (ax > 0) 
                   {
                     if (ay > 0) { r = (-tg +   pi +  pi/4); }
                     if (ay < 0) { r = ( tg +   pi +  pi/4); }
                   }
                   
                    if (ax < 0)
                    {
                        if (ay > 0) { r = (tg  + pi / 4); }
                        if (ay < 0) { r = (-tg + pi / 4); if (r < 0) r += 2*pi; }
                    }

                    //long r1 = (long)((r - pi34) * pi2);
                    //if (r1 < 0) r1 += (sineNumber+1);


                    if (r > max) max = r;
                    if (r < min) min = r;
                    long r1 = (long)((r ) * pi2);
                    //if (r1 < 0) r1 += (sineNumber+1);
                    result[i, j] = r1;
                }

                done++;
                PopupProgressBar.setProgress(done, all);
            }

            PopupProgressBar.close();
            //MessageBox.Show(" max =  " + max + " min =  " + min);
            ZArrayDescriptor wrappedPhase = new ZArrayDescriptor();
            wrappedPhase.array = result;
            wrappedPhase.width = w1;
            wrappedPhase.height = h1;

            return wrappedPhase;
        }

        public static ZArrayDescriptor ATAN_CarreA(ZArrayDescriptor[] img, int sineNumber, double a)
        {

            int w1 = img[0].width;
            int h1 = img[0].height;
            long[,] result = new long[w1, h1];                        // массив для значений фаз  

            double pi = Math.PI;
            double pi2 = sineNumber / (Math.PI * 2);
           
            int all = w1;
            int done = 0;
            PopupProgressBar.show();

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    long i1 = img[0].array[i, j];       // ------         Формула расшифровки
                    long i2 = img[1].array[i, j];
                    long i3 = img[2].array[i, j];
                    long i4 = img[3].array[i, j];
                    double ay = (i1 - i4) + (i2 - i3);
                    ay = ay * Math.Atan(a);
                    double ax = (i2 + i3) - (i1 + i4);


                    result[i, j] = (long)((Math.Atan2(ax, ay) + pi) * pi2);
                    
                }

                done++;
                PopupProgressBar.setProgress(done, all);
            }

            PopupProgressBar.close();
           
            ZArrayDescriptor wrappedPhase = new ZArrayDescriptor();
            wrappedPhase.array = result;
            wrappedPhase.width = w1;
            wrappedPhase.height = h1;

            return wrappedPhase;
        }

        public static ZArrayDescriptor ATAN_CarreAlpha(ZArrayDescriptor[] img)
        {

            int w1 = img[0].width;
            int h1 = img[0].height;
            long[,] result = new long[w1, h1];                        // массив для значений фаз

            // Число фазовых сдвигов
            //MessageBox.Show(" sineNumber =  " + sineNumber + " w1 =  " + w1 + " h1 =  " + h1);   

            double t=360;
            double pi2 =t/(2* Math.PI);
            long r;
            long max = -900;
            long min = 9999;

            int all = w1;
            int done = 0;
            PopupProgressBar.show();

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    long i1 = img[0].array[i, j];       // ------         Формула расшифровки
                    long i2 = img[1].array[i, j];
                    long i3 = img[2].array[i, j];
                    long i4 = img[3].array[i, j];
                    double y = Math.Abs(3 * (i2 - i3) - (i1 - i4));
                    double x = Math.Abs((i1 - i4) + (i2 - i3));
                    double b;
                    if (x != 0) b = Math.Sqrt(y / x); else b = 0;
                    double tg = Math.Atan(b);
                    r = (long)(tg * pi2);
                    if (r > max) max = r;      if (r < min) min = r;
                    result[i, j] =(long) (tg*pi2);

                }

                done++;
                PopupProgressBar.setProgress(done, all);
            }

            PopupProgressBar.close();
            MessageBox.Show(" max =  " + max + " min =  " + min);
            ZArrayDescriptor wrappedPhase = new ZArrayDescriptor();
            wrappedPhase.array = result;
            wrappedPhase.width = w1;
            wrappedPhase.height = h1;

            return wrappedPhase;
        }

        public static Bitmap Graph_ATAN(ZArrayDescriptor[] descriptors, double[] fzz)
        {
            int w1 = descriptors[0].width;
            int h1 = descriptors[0].height;

            int n_sdv = 4;                                                       // Число фазовых сдвигов

            int[] i_sdv = new int[4];
            int[] v_sdv = new int[4];

            int x0 = 0, x1 = w1, y0 = 0, y1 = h1;

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

            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fzz[i] * pi / 180); 
                k_cos[i] = Math.Cos(fzz[i] * pi / 180);
            }

            for (int ii = 0; ii < n_sdv; ii++)
            {
                for (int i = 0; i < 512; i++)
                {
                    vvs[ii, i] = i*k_sin[ii];
                    vvc[ii, i] = i*k_cos[ii];
                }
            }

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
                    r = (int) descriptors[0].array[i, j];
                    i_sdv[0] = r;
                    buffer1[i, j] = i_sdv[0];

                    r = (int)descriptors[1].array[i, j];
                    i_sdv[1] = r;
                    buffer2[i, j] = i_sdv[1];

                    r = (int)descriptors[2].array[i, j];
                    i_sdv[2] = r;
                    buffer3[i, j] = i_sdv[2];

                    r = (int)descriptors[3].array[i, j];
                    i_sdv[3] = r;
                    buffer4[i, j] = i_sdv[3];

                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1] + 255;
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2] + 255;

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
            double fw = w11;
            fw = fw / (max1 - min1);

            Bitmap result = new Bitmap(w11, h11);
            BitmapData data5 = ImageProcessor.getBitmapData(result);
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

                    x = (int)((fz1 - min1) * fw);
                    y = (int)((fz2 - min1) * fw);

                    if ((x < w11 && x >= 0) && (y < h11 && y >= 0))
                    {
                        r = buffer5[x, y]; r++;
                        if (r > 255) { r = 255; }
                        buffer5[x, y] = r;
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
            result.UnlockBits(data5);

            return result;
        }
    }
}
