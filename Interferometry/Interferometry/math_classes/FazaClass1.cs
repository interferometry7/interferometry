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
        public static Pi_Class1.ZArrayDescriptor ATAN_1234(Bitmap[] img, double[] fzz)
        {
            int w1 = img[0].Width;
            int h1 = img[0].Height;
            Int64[,] result = new Int64[w1, h1];                        // массив для значений фаз

            int n_sdv = img.Length;                                                   // Число фазовых сдвигов

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

            BitmapData data1 = ImageProcessor.getBitmapData(img[0]);
            BitmapData data2 = ImageProcessor.getBitmapData(img[1]);
            BitmapData data3 = ImageProcessor.getBitmapData(img[2]);
            BitmapData data4 = ImageProcessor.getBitmapData(img[3]);

            Color c;
            int r;
            double fz, fz1, fz2;
            Bitmap bmp5 = new Bitmap(w1, h1);
            int Gamma = 1;

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

                    result[i, j] = (long) (Math.Atan2(fz1, fz2) + pi);
                    fz = (Math.Atan2(fz1, fz2) + pi)*pi2;

                    //Float_Tmp[i, j] = fz ;
                    r = (int)(fz);

                    ImageProcessor.setPixel(data5, i, j, Color.FromArgb(r, r, r));
                }
                done++;  PopupProgressBar.setProgress(done, all);
            }

            img[0].UnlockBits(data1);
            img[1].UnlockBits(data2);
            img[2].UnlockBits(data3);
            img[3].UnlockBits(data4);
            bmp5.UnlockBits(data5);

            //pictureBox01.Size = bmp5.Size;
            //pictureBox01.Image = bmp5;
            PopupProgressBar.close();

            Pi_Class1.ZArrayDescriptor wrappedPhase = new Pi_Class1.ZArrayDescriptor();
            wrappedPhase.array = result;
            wrappedPhase.width = w1;
            wrappedPhase.height = h1;

            return wrappedPhase;
        }

        //Класс для хранения и передачи карты фазы (нерасшифрованной)
        /*public class WrappedPhase
        {
            public double[,] phaseArray;
            public int width;
            public int height;
        }*/
    }
}
