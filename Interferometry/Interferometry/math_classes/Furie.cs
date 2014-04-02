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
          
                        int all = 5;
                        int done = 0;
                        PopupProgressBar.show();
                        for (int j=0; j<5; j++)
                          { 
                            for (int i = 0; i < w1; i++)
                             {
                              double r = img.array[i, j];
                              Complex s = new Complex(r, 0);
                              array[i] = s;
                
                               ar = Furie(array);
                             }
                
                            //MessageBox.Show(" j =  " + j ); 
                            for (int i = 0; i < w1; i++)
                            {
                              Spectr.array[i, j] = Convert.ToInt64(Math.Sqrt(ar[i].Real*ar[i].Real + ar[i].Imaginary*ar[i].Imaginary));
                  
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

                         double af = Math.PI * 2 * 80 / w1;
                        for (int j = 0; j < h1; j++)
                        {
                            for (int i = 0; i < w1; i++)
                            {
                                Spectr.array[i, j] = (int)((Math.Sin(af * i + Math.PI) + 1) * 255.0 / 2.0);
                      
                            }

                        }
             *  */
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
      
           // int nr = (n - array.Length) / 2;
            //if (nr < 0) nr = 0;
           
            for (i = 0; i < array.Length; i++)
            {
                array[i] = a[i] / Math.Sqrt(n);
              
            }
          
            return array;
        }

        public static Complex[] Furie_Inverse(Complex[] array)
        {

            int m = Convert.ToInt32(Math.Ceiling(Math.Log(array.Length) / Math.Log(2))); // n=2**m
            int n = Convert.ToInt32(Math.Pow(2.0, m));

            Complex[] a = new Complex[array.Length]; array.CopyTo(a, 0);

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
            int nr = n/2 - array.Length/2;
            if (nr < 0) nr = 0;
            for (i = nr; i < array.Length; i++) a[i] = a[i] / Math.Sqrt(n);
            return a;
        }

    }

}