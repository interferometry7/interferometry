using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using Interferometry.forms;
using rab1;

public delegate void ImageProcessed(Bitmap resultBitmap);

namespace Interferometry
{
    public class FazaClass
    {
        public static Pi_Class1.ZArrayDescriptor ATAN_1234(Pi_Class1.ZArrayDescriptor[] img, double[] fzz)
        {
            int w1 = img[0].width;
            int h1 = img[0].height;
            long[,] result = new long[w1, h1];                        // массив для значений фаз

            int n_sdv = img.Length;                                                   // Число фазовых сдвигов

            double[] i_sdv = new double[4];
            double[] v_sdv = new double[4];                                  // Вектор коэффициентов
            double[] k_sin = new double[4];
            double[] k_cos = new double[4];
            double pi = Math.PI;
            double pi2 = 512/(Math.PI * 2);

            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fzz[i] * pi / 180);
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
                    i_sdv[0] = img[0].array[i, j];
                    i_sdv[1] = img[1].array[i, j];
                    i_sdv[2] = img[2].array[i, j];
                    i_sdv[3] = img[3].array[i, j];

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

            Pi_Class1.ZArrayDescriptor wrappedPhase = new Pi_Class1.ZArrayDescriptor();
            wrappedPhase.array = result;
            wrappedPhase.width = w1;
            wrappedPhase.height = h1;

            return wrappedPhase;
        }
    }
}
