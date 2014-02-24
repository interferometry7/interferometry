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
        public static ZArrayDescriptor ATAN_1234(ZArrayDescriptor[] img, double[] fzz, int sineNumber)
        {
            
            int w1 = img[0].width;
            int h1 = img[0].height;
           // MessageBox.Show(" sineNumber =  " + sineNumber + " w1 =  " + w1 + " h1 =  " + h1); 
            long[,] result = new long[w1, h1];                        // массив для значений фаз

            int n_sdv = img.Length;                                   // Число фазовых сдвигов
            //MessageBox.Show(" n_sdv =  " + n_sdv );   
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
            //MessageBox.Show(" sineNumber =  " + sineNumber + " w1 =  " + w1 + " h1 =  " + h1);   
           
            double pi = Math.PI;
            double pi34 = 3 * pi / 4;
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
            MessageBox.Show(" max =  " + max + " min =  " + min);
            ZArrayDescriptor wrappedPhase = new ZArrayDescriptor();
            wrappedPhase.array = result;
            wrappedPhase.width = w1;
            wrappedPhase.height = h1;

            return wrappedPhase;
        }




    }
}
