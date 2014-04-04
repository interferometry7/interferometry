using System;
using System.Numerics;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using Interferometry.forms;
using Interferometry.math_classes;
using rab1;

//public delegate void ImageProcessed(Bitmap resultBitmap);

namespace rab1
{

    public class FurieClass
    {
      public static ZArrayDescriptor BPF(ZArrayDescriptor img)
        {

            int w1 = img.width;
            int h1 = img.height;
            //MessageBox.Show(" w1 =  " + w1 + " h1 =  " + h1); 
            ZArrayDescriptor Spectr = new ZArrayDescriptor();
            Spectr.width = w1;
            Spectr.height = h1;
            Spectr.array = new long[w1, h1];

            Complex[] array = new Complex[w1];
            Complex[] ar    = new Complex[w1];
            Complex[] ar1 = new Complex[w1];
                            
                                      int all = h1;
                                      int done = 0;
                                      PopupProgressBar.show();
                                      for (int j=0; j<h1; j++)
                                        { 
                                          for (int i = 0; i < w1; i++)
                                           {
                                            double r = img.array[i, j];
                                            Complex s = new Complex(r, 0);
                                            array[i] = s;
                
                               
                                           }
                                          ar = Furie(array);
                                          Complex nc = new Complex(0, 0);
                                            for (int i = 30;   i < 42;   i++) ar[i] = nc;
                                            for (int i = 2006; i < 2018; i++) ar[i] = nc;
                                            ar1 = Furie_invers(ar);
                                          //MessageBox.Show(" j =  " + j ); 
                                          for (int i = 0; i < w1; i++)
                                          {
                                            Spectr.array[i, j] = Convert.ToInt64(Math.Sqrt(ar1[i].Real*ar1[i].Real + ar1[i].Imaginary*ar1[i].Imaginary));
                  
                                          }
                                          done++;
                                          PopupProgressBar.setProgress(done, all);
                                        }

 /* 
             for (int j = 100; j < 200; j++)
                 {
                  for (int i = 0; i < w1; i++)
                  {
                     Spectr.array[i, j] = img.array[i, j];
                  }
                 }
  
             double af = Math.PI * 2 * 60 / w1;
             for (int j = 0; j < h1; j++)
                 {
                  for (int i = 0; i < w1; i++)
                  {
                     Spectr.array[i, j] = (int)((Math.Sin(af * i + Math.PI) + 1) * 255.0 / 2.0);
                  }
                }
  */
            PopupProgressBar.close();
          return Spectr;
        
        
        }


        public static Complex[] Furie(Complex[] array)
        {

            int m = Convert.ToInt32(Math.Ceiling(Math.Log(array.Length) / Math.Log(2))); // n=2**m
            int n = Convert.ToInt32( Math.Pow( 2.0, m ) );
            //MessageBox.Show(" n =  " + n + " m =  " + m); 

            Complex[] a = new Complex[n];
            array.CopyTo(a, 0);

            Complex u, w, t;
            int i, j, ip, l;

            int n1 = n >> 1;
            int k = n1;
            for (i = 0, j = 0; i < n - 1; i++, j = j + k)
            {
                if (i < j)
                {
                    t = a[j];
                    a[j] = a[i];
                    a[i] = t;
                }
                k = n1;
                while (k <= j)
                {
                    j = j - k;
                    k = k >> 1;
                }
            }
            for (l = 1; l <= m; l++)
            {
                int ll = Convert.ToInt32( Math.Pow( 2.0, l ) );
                int ll1 = ll >> 1;
                u = new Complex(1.0, 0.0);
                w = new Complex(Math.Cos(Math.PI / ll1), Math.Sin(Math.PI / ll1));
                for (j = 1; j <= ll1; j++)
                {
                    for (i = j - 1; i < n; i = i + ll)
                    {
                        ip = i + ll1;
                        t = a[ip]*u;
                        a[ip] = a[i] - t;
                        a[i] = a[i] + t;
                    }
                    u = u*w;
                }
            }      
           
            for (i = 0; i < array.Length; i++)
            {
                array[i] = a[i] / Math.Sqrt(n);
              
            }
          
            return array;
        }

        public static Complex[] Furie_invers(Complex[] array)
        {

            int m = Convert.ToInt32(Math.Ceiling(Math.Log(array.Length) / Math.Log(2))); // n=2**m
            int n = Convert.ToInt32(Math.Pow(2.0, m));
            //MessageBox.Show(" n =  " + n + " m =  " + m); 

            Complex[] a = new Complex[n];
            array.CopyTo(a, 0);

            Complex u, w, t;
            int i, j, ip, l;

            int n1 = n >> 1;
            int k = n1;
            for (i = 0, j = 0; i < n - 1; i++, j = j + k)
            {
                if (i < j)
                {
                    t = a[j];
                    a[j] = a[i];
                    a[i] = t;
                }
                k = n1;
                while (k <= j)
                {
                    j = j - k;
                    k = k >> 1;
                }
            }
            for (l = 1; l <= m; l++)
            {
                int ll = Convert.ToInt32(Math.Pow(2.0, l));
                int ll1 = ll >> 1;
                u = new Complex(1.0, 0.0);
                w = new Complex(Math.Cos(Math.PI / ll1), Math.Sin(-Math.PI / ll1));
                for (j = 1; j <= ll1; j++)
                {
                    for (i = j - 1; i < n; i = i + ll)
                    {
                        ip = i + ll1;
                        t = a[ip] * u;
                        a[ip] = a[i] - t;
                        a[i] = a[i] + t;
                    }
                    u = u * w;
                }
            }

            for (i = 0; i < array.Length; i++)
            {
                array[i] = a[i] / Math.Sqrt(n);

            }

            return array;
        }
        //-----------------------------------------------------------------------------------
        //     Фильтрация методом наименьших квадратов
        //-----------------------------------------------------------------------------------
        public static ZArrayDescriptor MNK(ZArrayDescriptor img)
        {

            int w1 = img.width;
            int h1 = img.height;
            //MessageBox.Show(" w1 =  " + w1 + " h1 =  " + h1); 
            ZArrayDescriptor Spectr = new ZArrayDescriptor();
            Spectr.width = w1;
            Spectr.height = h1;
            Spectr.array = new long[w1, h1];

            long[] array = new long[w1];
            long[] ar    = new long[w1];
           

          
            int all = h1;
            int done = 0;
            PopupProgressBar.show();
            for (int j = 0; j < h1; j++)
            {
                for (int i = 0; i < w1; i++) { array[i] = Convert.ToInt64(img.array[i, j]); }
                ar = MNK_line(array);
                for (int i = 0; i < w1; i++) { Spectr.array[i, j] = ar[i];       }
                done++;
                PopupProgressBar.setProgress(done, all);
            }

            
            PopupProgressBar.close();
            return Spectr;

        }

        public static Int64[] MNK_line( long[] array)
        {
            int N = array.Length;
            double[] S = new double[4];
            double[] A = new double[4];
            for (int i = 0; i < N; i++) { S[i] = 0; A[i] = 0; }
            for (int i = 0; i < N; i++)
            {
                double y = array[i];
                S[0] = S[0] + y;
                S[1] = S[1] + y*i;
                S[2] = S[2] + y * i * i;
                S[3] = S[3] + y * i * i * i;
            }
            double[,] m = new double[4, 4];
            m = m_MNK4(N);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    A[i] = A[i] + m[i, j]*S[j];
                }
            }

            for (int i = 0; i < N; i++)
            {
                array[i] = Convert.ToInt64(A[0] + A[1] * i + A[2] * i * i + A[3] * i * i * i);
            }



            return array;
        }

        private static double[,] m_MNK4(int N)
        {  
            double[,] m = new double[4,4];
            double N2 = N * N;
            double N3 = N2 * N;
            double N4 = N3 * N;
            double N5 = N4 * N;
            double N6 = N5 * N;
            double N7 = N6 * N;
            double B1 = N4 + 6 * N3 + 11 * N2 + 6 * N;
            double B2 = 14 * N5 - N7 - 49 * N3 + 36 * N2;
            double B3 = N6 + N5 - 13 * N4 - 13 * N3 + 36 * N2 + 36 * N;

            double B03 = B3 / 4200;
            double B13 = B3 * (15 * N - 15) / 4200;
            double B33 = B3 * (3 * N - 3) / 4200;

            m[0, 0] = (16 * N3 - 24 * N2 + 56 * N - 24) / B1; m[0, 1] = -(120 * N2 - 120 * N + 100) / B1;  m[0, 2] = (240 * N - 120) / B1;  m[0, 3] = -(140) / B1;

            m[1, 0] = m[0,1];                                 m[1, 1] = -(1200*N4 -5400*N3 +8400 * N2 - 6000 * N + 2200) / B2; 
            m[1, 2] = -(2700*N2-6300*N+3000) / B3;            m[1, 3] = -(1680*N2-4200 * N +3080) / B2; 

            m[2, 0] = m[0,2];   m[2, 1] = m[1, 2];   m[2, 2] = -(6480*N2-12600 *N+4680) / B2;    m[2, 3] = -(4200) / B3;

            m[3, 0] = (N2 - 5 * N + 6) / (B03*30); m[3, 1] = (6 * N2 - 15 * N + 11) / B13; m[3, 2] = -1 / B03; m[3, 3] = 2 / B33; 
            return m;
        }
    }

}