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

            //int n_sdv = img.Length;                                   // Число фазовых сдвигов
            //MessageBox.Show(" n_sdv =  " + n_sdv );   
           
            double pi = Math.PI;
            double pi2 = sineNumber / (Math.PI * 2);
            double tg;
            //                     x>0              atan(y/x)
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
                    long i1 =  img[0].array[i, j];
                    long i2 =  img[1].array[i, j];
                    long i3 =  img[2].array[i, j];
                    long i4 =  img[3].array[i, j];
                    double ay = (i1 - i4) + (i2 - i3);
                    double ax = (i2 + i3) - (i1 + i4);
                    double c = 3 * (i2 - i3) - (i1 - i4);

                    if (ax == 0 && ay > 0)  { result[i, j] = (long)((pi / 2 + pi/2) * pi2);  continue; }
                    if (ax == 0 && ay < 0)  { result[i, j] = (long)((-pi / 2 + pi/2) * pi2); continue; }
                    if (ax == 0 && ay == 0) { result[i, j] = 0;                              continue; }


                    tg = Math.Atan(Math.Sqrt(ay * c) / (ax));
                    //if (ax > 0)            { result[i, j] = (long)((tg        + pi/2) * pi2);  continue;}
                    if (ax < 0 && ay>=0)   { result[i, j] = (long)((tg   + pi + pi/2) * pi2);  continue;}
                    //if (ax < 0 && ay<0)    { result[i, j] = (long)((tg   - pi + pi/2) * pi2);  continue;}
                   
                    
                    // ------                                     Формула расшифровки

                   
                   
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




    }
}
