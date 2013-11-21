using System;
using System.Data.SqlTypes;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using System.Drawing.Imaging;
using rab1.Forms;

namespace rab1
{
    public class Pi_Class1
    {
        //MessageBox.Show(" n1= " + N1.ToString() + " n2= " + N2.ToString());

        static Int32 M1 = -1;
        static Int32 M2 = -1;
        static Int32 N1 = -1;
        static Int32 N2 = -1;
        static int NMAX=1600;
        static int[] glbl_faze  = new int[NMAX];               // Номера для прослеживания полос (номера линий)
        static int[] glbl_faze1 = new int[NMAX];               // Количество добавлений b1 (Для расшифровки)
        static int[] number_2pi = new int[200];                // Максимум 200 полос (пока) ------------------------------
        static Form  f_sin;
        static PictureBox pc1;
        static int scale = 4;
        static int x0 = 46, y0 = 16;
        static double A=0, B=0, C=0, D=0;

        static Int32 n1;
        static Int32 n2;

        static Int64[,] Z;                                     // Глобальный массив результирующих фаз (Размер задается при расшифровке)

        /*      
        Назначение: Нахождение наибольшего общего делителя двух чисел N и M по рекуррентному соотношению
        N0 = max(|N|, |M|) N1 = min(|N|, |M|)
        N k = N k-2 - INT(N k-2 / N k-1)*N                   k-1 k=2,3 ...
       
        Если Nk = 0 => НОД = N k-1
        (N=23345 M=9135 => 1015 N=238 M=347 => 34)
        */
        private static Int32 Evklid(Int32 N1, Int32 N2)
        {
           Int32 n0 = Math.Max(N1, N2);
           Int32 n1 = Math.Min(N1, N2);
          
           do { Int32 n = n0 - (n0 / n1) * n1; n0 = n1; n1 = n; } while (n1 != 0);
          
           return n0;
        }

        private static void China(int sN1, int sN2)
        {
            int n;
            n1 = sN1;
            n2 = sN2;
            Int32 NOD = Evklid(n1, n2); // Если NOD == 1 числа взаимно просты
            if (NOD != 1) { MessageBox.Show("Числа не взаимно просты"); return; }

            M1 = n2;
            M2 = n1;
            N1 = -1;
            N2 = -1;
            for (int i = 0; i < M1; i++) { n = (M1 * i) % n1; if (n == 1) { N1 = i; break; } } if (N1 < 0) N1 = N1 + n1;
            for (int i = 0; i < M2; i++) { n = (M2 * i) % n2; if (n == 1) { N2 = i; break; } } if (N2 < 0) N2 = N2 + n2;
        }
// --------------------------------------------------------------------------------------------------------------------------- Рисование таблицы  (параметры) (b2, b1)
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //
        //           Построение таблицы
        //
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void Graph_China2(Image[] img,  int Diag, bool rb, int pr_obr)
        {
            int max_x = (n1 + n2) * scale, max_y = 800;
            int w1 = n2, hh = n1;

           

            f_sin = new Form();
            f_sin.Size = new Size(max_x + 8, max_y + 8);
            f_sin.StartPosition = FormStartPosition.Manual;

            pc1 = new PictureBox();
            pc1.BackColor = Color.White;
            pc1.Location = new Point(0, 8);
            pc1.Size = new Size(max_x, max_y);
            pc1.SizeMode = PictureBoxSizeMode.StretchImage;
            pc1.BorderStyle = BorderStyle.Fixed3D;

            Bitmap btmBack = new Bitmap(max_x + 8, max_y + 8);      //изображение          
            Graphics grBack = Graphics.FromImage(btmBack);

            pc1.BackgroundImage = btmBack;


            f_sin.Controls.Add(pc1);

            Pen p1 = new Pen(Color.Black, 1);
            Pen p2 = new Pen(Color.Red, 1);
            Pen p3 = new Pen(Color.Blue, 1);
            Pen p4 = new Pen(Color.Gold, 1);
            Font font = new Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel);

            grBack.DrawLine(p1, x0, y0, x0, hh * scale + y0);
            grBack.DrawLine(p1, x0, hh * scale + y0, 2 * w1 * scale + x0, hh * scale + y0);

            grBack.DrawLine(p1, w1 * scale + x0, hh * scale + y0, w1 * scale + x0, y0);
            grBack.DrawLine(p1, w1 * scale + x0, y0, x0, y0);

            StringFormat drawFormat = new StringFormat(StringFormatFlags.NoClip);

            string s = n2.ToString(); grBack.DrawString("b2 " + s, font, new SolidBrush(Color.Black), w1 * scale + x0 + 8, y0 - 8, drawFormat);
                   s = n1.ToString(); grBack.DrawString("b1 " + s, font, new SolidBrush(Color.Black), x0, hh * scale + 20 + 10 * scale, drawFormat);
            // ----------------------------------------------------------------------------------------------------------------
           
                   GLBL_FAZE(n1, n2, Diag);                                                         //  Заполнение glbl_faze[]  и glbl_faze1[] - допустимые границы диапазона
// Отрисовка диагоналей
            Int32 A = Diag * Math.Max(n1, n2);
            Int32 pf;
            for (int b2 = 0; b2 < n2; b2++)                                                                    // Диагонали   
            {
                pf = M2 * N2 * b2 % (n1 * n2);
                if (pf < A)
                {
                    grBack.DrawLine(p2, x0 + b2 * scale, y0, x0 + b2 * scale + n1 * scale, hh * scale + y0);
                    pf = pf / n1;
                //    glbl_faze[n1 + b2] = pf;
                    s = pf.ToString(); grBack.DrawString(s, font, new SolidBrush(Color.Black), x0 + b2 * scale, y0 - 4 * scale, drawFormat);
                }
            }
            for (int b1 = 0; b1 < n1; b1++)
            {
                pf = M1 * N1 * b1 % (n1 * n2);
                if (pf < A)
                {
                    grBack.DrawLine(p3, x0, y0 + b1 * scale, x0 + n1 * scale - b1 * scale, hh * scale + y0);
                    pf = pf / n1;
                //    glbl_faze[n1 - b1] = pf;
                    s = pf.ToString(); grBack.DrawString(s, font, new SolidBrush(Color.Black), x0 - 10 * scale, y0 + b1 * scale, drawFormat);
                }
            }
// Нумерация внизу таблицы
            for (int i = 0; i < n1 + n2; i++)
            {
                int bb = glbl_faze[i];
                if (bb >= 0) { s = bb.ToString(); grBack.DrawString(s, font, new SolidBrush(Color.Black), x0 + i * scale, y0 + n1 * scale + 8 * scale, drawFormat); }
            }

            
            
//            int mxx = 0, mxx_x = 0, mnx_x = 0, cntr = 0;
            int mnx = 0;

// Отрисовка границ допустимого диапазона(Gold)
            mnx = glbl_faze1[0];
            for (int i = 0; i < n1 + n2; i++)
            {
                int bb = glbl_faze1[i];
                if (bb != mnx)
                {
                    mnx = bb;
                    grBack.DrawLine(p4, x0 + i * scale, y0 + hh * scale, x0, y0 + hh * scale - i * scale);
                }
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //     Заполнение  массива bmp_r
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            
            int[,] bmp_r = new int[n2 + 3, n1 + 3];
            int count=bmp_2pi(img, bmp_r, Diag, pr_obr);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Рисование точек в таблице по диагоналям
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            int b = 0, ib2 = 0, ib1 = 0, max_count = 0;
            for (ib2 = 0; ib2 < n2 - 1; ib2++)
            {
                for (ib1 = 0; ib1 < n1 - 1; ib1++)
                {
                    b = bmp_r[ib2, ib1];
                    max_count = Math.Max(b, max_count);              
                    if (b > pr_obr) { grBack.DrawRectangle(new Pen(Color.FromArgb(146, 24, 47)), x0 + ib2 * scale, y0 + ib1 * scale, 1, 1);  }
                }
            }
             
            MessageBox.Show(" count =  " + count + "  max =  " + max_count  );
           

            pc1.Refresh();
            f_sin.Show();

            //HystogrammForm hystogrammForm = new HystogrammForm(bmp_r, n2 + 3, n1 + 3);
            //hystogrammForm.Show();

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //     Заполнение  массива bmp_r
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static int bmp_2pi(Image[] img, int[,] bmp_r , int Diag, int pr_obr)
        {
           int r,g;
           int w = img[0].Width;
           int h = img[0].Height;
           Bitmap bmp1 = new Bitmap(img[1], w, h);
           Bitmap bmp2 = new Bitmap(img[0], w, h);
           Bitmap bmp3 = new Bitmap(img[2], w, h);


           int[] ims1 = new int[h];
           int[] ims2 = new int[h];
           int[] ims3 = new int[h];

           Color c, c1;

           int xx0 = 0, yy0 = 0;
           int xx1 = w, yy1 = h;
           double fn1 = (double) (n1 - 1)/255;
           double fn2 = (double) (n2 - 1)/255;

   
           int all = xx1 - xx0;
           int done = 0;
           PopupProgressBar.show();

          int count = 0;
          // ------------------------------------------------------------------------- По фигуре из 3 квадрата
            for (int i = xx0; i < xx1; i++)
            {
                for (int j = yy0; j < yy1; j++)
                {
                    c1 = bmp3.GetPixel(i, j);
                    if (c1.R == 0)
                    {
                        ims3[j] = 0;
                    }
                    else // ims1[j] = (int)((double)(r * (n1 - 1)) / 255); 
                    {
                        ims3[j] = 1;
                        c = bmp1.GetPixel(i, j); r = c.R; ims1[j] = (int) ((double) (r)*fn1); //(b2)
                        c = bmp2.GetPixel(i, j); r = c.R; ims2[j] = (int) ((double) (r)*fn2); //(b1) 
                    }

                }

                for (int j = yy0; j < yy1; j++)
                {
                    if (ims3[j] != 0)
                    {
                        r = ims1[j]; g = ims2[j]; bmp_r[g, r]++; count++;
                    }

                }
                done++;
                PopupProgressBar.setProgress(done, all);
            }
       

        PopupProgressBar.close();
            return (count);
        }

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// ----------------------------------------------------------------------------------------------------------------------------------------------------------------          
// ---------------------------------------------         Определение коэффициентов плоскости  z[i,j] = A*i + B*j +C   методом наименьших квадратов     
// ----------------------------------------------------------------------------------------------------------------------------------------------------------------  

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public class Plane
        {
            public double a;
            public double b;
            public double c;
            public double d;
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public Plane()
            {
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public Plane(double newA, double newB, double newC)
            {
                a = newA;
                b = newB;
                c = newC;
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public Plane(double newA, double newB, double newC, double newD)
            {
                a = newA;
                b = newB;
                c = newC;
                d = newD;
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Plane getPlaneParams(Int64[,] Z, int xx0, int xx1, int yy0, int yy1)                                                  // По строке
        {
            double width = xx1 - xx0;
            double height = yy1 - yy0;

            double a1 = (width * (height - 1) * (2 * (height - 1) * (height - 1) + 3 * height - 2)) / 6;
            double b1 = (width * (width - 1) * height * (height - 1)) / 4;
            double b2 = (height * (width - 1) * (2 * (width - 1) * (width - 1) + 3 * width - 2)) / 6;

            double c1 = (width * height * (height - 1)) / 2;
            double c2 = (width * height * (width - 1)) / 2;
            double c3 = width * height;

            double d1 = 0, d2 = 0, d3 = 0;
            for (int j = yy0; j < yy1; j++)
            {
                for (int i = xx0; i < xx1; i++)
                {
                    d1 += (j * Z[i, j]);
                    d2 += (i * Z[i, j]);
                    d3 += Z[i, j];
                }
            }

            double k1 = -b1 / a1;
            double k2 = -c1 / a1;
            double k3 = -(c2 + k2 * b1) / (b2 + k1 * b1);

            Plane result = new Plane();

            result.c = (d3 + k2 * d1 + k3 * (d2 + k1 * d1)) / (c3 + k2 * c1 + k3 * (c2 + k1 * c1));
            result.b = (d2 + k1 * d1 - (c2 + k1 * c1) * C) / (b2 + k1 * b1);
            result.a = (d1 - b1 * B - c1 * C) / a1;

            //result.b = (d3 + k2 * d1 + k3 * (d2 + k1 * d1)) / (c3 + k2 * c1 + k3 * (c2 + k1 * c1));
            //result.a = (d2 + k1 * d1 - (c2 + k1 * c1) * C) / (b2 + k1 * b1);
            //result.c = (d1 - b1 * B - c1 * C) / a1;

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void ABC(Int64 [,] Z,  int xx0, int xx1, int yy0, int yy1)                                                  // По строке
        {
            double M = xx1-xx0;
            double N = yy1-yy0;
            //MessageBox.Show(" N = " + N.ToString() + " M =  " + M.ToString() );
            double a1 = (M * (N - 1)*(2 * (N - 1) * (N - 1) + 3 * N - 2)) / 6;
            double b1 = (M * (M - 1) * N * (N - 1)) / 4;
            double b2 = (N*(M-1)*(2*(M-1)*(M-1)+3*M-2) )/6;
           
            double c1 = (M * N * (N - 1)) / 2;
            double c2 = (M * N * (M - 1)) / 2;
            double c3 = M * N;
            //MessageBox.Show(" c1 = " + c1.ToString() + " c2 =  " + c2.ToString() + " c3 =  " + c3.ToString() + " b2 =  " + b2.ToString());
            double d1 = 0, d2 = 0, d3 = 0;
            for (int j = yy0; j < yy1; j++) for (int i = xx0; i < xx1; i++) { d1 += (j * Z[i, j]); d2 += (i * Z[i, j]); d3 += Z[i, j]; }
            //MessageBox.Show(" d1 = " + d1.ToString() + " d2 =  " + d2.ToString() + " d3 =  " + d3.ToString());
            double k1 = - b1 / a1;
            double k2 = - c1 / a1;
            double k3 = - (c2+k2*b1 )/ ( b2+k1*b1);
            //MessageBox.Show(" k1 = " + k1.ToString() + " k2 =  " + k2.ToString() + " k3 =  " + k3.ToString());
            C = (d3+k2*d1+k3*(d2+k1*d1) )/(c3+k2*c1+k3*(c2+k1*c1) );
            B = (d2+k1*d1-(c2+k1*c1)*C)/(b2+k1*b1);
            A = (d1 - b1 * B - c1 * C) / a1;
            
        }
          
   
// --------------------------------------------------------------------------------------------------------------------------------        
// --------------------------------------------------------------------------------------------------------------------------------        
// --------------------------------------------------------------------------------------------------------------------------------                
// --------------------------------------------------------------------------------------------------------------------------------                
// --------------------------------------------------------------------------------------------------------------------------------                
// --------------------------------------------------------------------------------------------------------------------------------
        public static void pi2_frml2(Image[] img, int sN1, int sN2, int Diag, bool rb, int pr_obr)
        {
            China(sN1, sN2);           // Вычисление формулы
            Graph_China2(img, Diag, rb, pr_obr);                          // Построение таблицы
        }

//-----------------------------------------------------------------------------------------------------------------------------------
        
//-----------------------------------------------------------------------------------------------------------------------------------
        public static Bitmap pi2_rshfr(Image[] img, int sN1, int sN2, int Diag, bool rd, int pr_obr) // Расшифровка
        {
            China(sN1, sN2);                                          // Вычисление формулы 
                                                   
            int[,] bmp_r = new int[sN2 + 3, sN1 + 3];                 // Массив точек в таблице 2pi
            int count = bmp_2pi(img, bmp_r, Diag, pr_obr);

            int w = img[0].Width;
            int h = img[0].Height;
            Bitmap bmp1 = new Bitmap(img[1], w, h);     // 1 фаза
            Bitmap bmp2 = new Bitmap(img[0], w, h);     // 2 фаза
            Bitmap bmp3 = new Bitmap(img[2], w, h);     // Маска
        
            Bitmap bmp  = new Bitmap( w, h);            // Результат

            bool rd1 = false;
            Z = new Int64[w, h];
            rash_2pi(bmp1, bmp2, bmp3, bmp_r, pr_obr, sN1, sN2, Diag, Z, rd1);    //  РАСШИФРОВКА (Заполнение Z[,])
            int x1 = 15, x2 = 1403, y1 = 50;
            Z_sub(x1,   x2, y1, Z, w, h);
            Z_bmp(bmp, bmp3, Z, rd);                              //  Z -> bmp с масштабированием

            return bmp;
        }
        // -----------------------------------------------------------------------------------------------------------------------------------           
        // -----------------------------------       Вычитание наклона  -> в вещественный массив Z             -------------------------------          
        // -----------------------------------------------------------------------------------------------------------------------------------  
        private static void Z_sub(int x1,  int x2, int y1, Int64[,] Z, int w, int h)
        {
            Int64 z1 = Z[x1, y1];
            Int64 z2 = Z[x2, y1];
            double tt = (double) (z2 - z1)/(double) (x2 - x1);
            Int64 [] s = new Int64[w];
            for (int j = x1; j < x2; j++)
            {
                s[j] = (Int64)(tt * (j - x1)) + (z1);
            }
            for (int i =0; i < 200; i++)
            {
                for (int j = x1; j < x2; j++)
                {
                    Z[j, i] =  s[j];

                }
            }
           
            for (int i = 500; i < h; i++)
            {
                for (int j = x1; j < x2; j++)
                {
                    Z[j, i] =Z[j, i] - s[j];
                    
                }
            }
        }

        // -----------------------------------------------------------------------------------------------------------------------------------           
        // -----------------------------------       Сама расшифровка   -> в вещественный массив Z             -------------------------------          
        // -----------------------------------------------------------------------------------------------------------------------------------  
       
       private static void rash_2pi(Bitmap bmp1, Bitmap bmp2, Bitmap bmp3, int[,] bmp_r, int pr_obr, int n1, int n2, int Diag, Int64[,] Z, bool rd)
        {
            GLBL_FAZE(n1, n2, Diag);                         // Заполнение массива glbl_faze[] для расшифровки
            int b1, b2, b3, ib1, ib2;
            int w = bmp1.Width;
            int h = bmp1.Height;

            BitmapData data1 = ImageProcessor.getBitmapData(bmp1);
            BitmapData data2 = ImageProcessor.getBitmapData(bmp2);
            BitmapData data3 = ImageProcessor.getBitmapData(bmp3);  // Маска

            Color c;
            double fn1 = (double)(n1 - 1) / 255;
            double fn2 = (double)(n2 - 1) / 255;


           

            int all = w;  int done = 0;   PopupProgressBar.show();   
                       
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    c = ImageProcessor.getPixel(i, j, data1);
                    b1 = c.R;
                    ib1 = (int) (fn1*b1); // c = bmp1.GetPixel(i, j);                   
                    c = ImageProcessor.getPixel(i, j, data2);
                    b2 = c.R;
                    ib2 = (int) (fn2*b2); // c = bmp2.GetPixel(i, j);
                   
                    if (rd)
                    {
                        c = ImageProcessor.getPixel(i, j, data3);
                        if (bmp_r[ib2, ib1] >= pr_obr && c.R != 0)
                        {
                            b3 = glbl_faze1[ib2 + (n1 - ib1)];
                            Z[i, j] = (n1)*b3 + ib1; // glbl_faze1[ib2 + (n1 - ib1)];
                        }
                    }
                    else
                    {
                        if (bmp_r[ib2, ib1] >= pr_obr)
                        {
                            b3 = glbl_faze1[ib2 + (n1 - ib1)];
                            Z[i, j] = (n1) * b3 + ib1; // glbl_faze1[ib2 + (n1 - ib1)];
                        }
                    }
                }
                done++; PopupProgressBar.setProgress(done, all);
            }
            //int d1 = 0, d2 = 0, id1 = 0, id2 = 0, ib3 = 0, x = 0;
            //int ii = 1300; int jj = 536;
            //int ii = 19 * (n1 - 1) / 255; int jj = 237 * (n2 - 1) / 255;
            //int ii = 1294; int jj = 546;
            //Color c1 = ImageProcessor.getPixel(ii, jj, data1);
            //Color c2 = ImageProcessor.getPixel(jj, ii, data2);
            //int d1 = c1.R;
            //int d2 = c2.R;
          //  int dd1 = (int)Z[ii, jj];
          //  int dd2 = (int)Z[jj, ii];
          //  MessageBox.Show(" (" + ii + "," + jj + ")= " + " cx= " + d1 + " cy = " + d2 + " ib1= " + id1 + " ib2 = " + id2 + " ib3 = " + ib3 + " x = " + x);

            bmp1.UnlockBits(data1);
            bmp2.UnlockBits(data2);
            bmp3.UnlockBits(data3);
            PopupProgressBar.close();
         }
           
        // -----------------------------------------------------------------------------------------------------------------------------------           
        // -----------------------------------        Заполнение массива для расшифровки  glbl_faze ,    glbl_faze1   ------------------------          
        // -----------------------------------------------------------------------------------------------------------------------------------          
        private static void GLBL_FAZE(int n1, int n2, int Diag)
        {

            for (int i = 0; i < n1 + n2; i++) { glbl_faze[i] = -1; glbl_faze1[i] = -1; }                       // Массив для расшифровки

            int A = Diag * Math.Max(n1, n2);
            int pf;
            for (int b2 = 0; b2 < n2; b2++)                                                                    // Диагонали   
            {
                pf = M2 * N2 * b2 % (n1 * n2);
                if (pf < A) { pf = pf / n1; glbl_faze[n1 + b2] = pf; }
            }
            for (int b1 = 0; b1 < n1; b1++)
            {
                pf = M1 * N1 * b1 % (n1 * n2);
                if (pf < A) { pf = pf / n1; glbl_faze[n1 - b1] = pf; }
            }
            int mxx = 0, mxx_x = 0, mnx = 0, mnx_x = 0, cntr = 0;
            for (; ; )
            {
                for (int i = mnx_x; i < n1 + n2; i++)
                {
                    cntr = i;
                    int bb = glbl_faze[i]; if (bb >= 0 && bb != mnx) { mxx = bb; mxx_x = i; break; }
                }
                if (cntr >= n1 + n2 - 1) break;
                //MessageBox.Show(" mnx =  " + mnx.ToString() + " mxx =  " + mxx.ToString());                    
                int m = (mxx_x - mnx_x) / 2;
                for (int j = mnx_x; j < mnx_x + m; j++) glbl_faze1[j] = mnx;
                for (int j = mnx_x + m; j < mxx_x; j++) glbl_faze1[j] = mxx;
                mnx_x = mxx_x;
                mnx = mxx;
            }

         
           // for (int i = 0; i < n1 + n2; i++) { pf = glbl_faze1[i]; MessageBox.Show(" i =  " + i.ToString() + "  =  " + pf.ToString()); }    

        }
        // -----------------------------------------------------------------------------------------------------------------------------------           
        // -----------------------------------        Z -> bmp с масштабированием                              -------------------------------          
        // -----------------------------------------------------------------------------------------------------------------------------------  
        static void Z_bmp(Bitmap bmp, Bitmap bmp3, Int64[,] Z, bool rd)               // -------------------------- Z -> BMP
        {
            Int64 b2_min = Z[0, 0], b2_max = Z[0, 0];
            int w = bmp.Width; ;
            int h = bmp.Height;
            int b2;
            Color c;

            BitmapData data = ImageProcessor.getBitmapData(bmp);
            BitmapData data3 = ImageProcessor.getBitmapData(bmp3);

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    if (rd)
                    {
                        c = ImageProcessor.getPixel(i, j, data3);
                        if (c.R != 0)
                        {
                            b2_max = Math.Max(b2_max, Z[i, j]);
                            b2_min = Math.Min(b2_min, Z[i, j]);
                        }
                    }
                    else
                    {
                        b2_max = Math.Max(b2_max, Z[i, j]);
                        b2_min = Math.Min(b2_min, Z[i, j]);
                    }
                }
            MessageBox.Show(" Max = " + b2_max + " Min =  " + b2_min);
            b2_min = 0;
            b2_max = 255*4;
            double max = (double)255 / (double)(b2_max - b2_min);
          
           
            int all = w;             int done = 0;             PopupProgressBar.show();
            for (int i = 0; i < w; i++)                                                                   //  Отображение точек на pictureBox01
            {
                for (int j = 0; j < h; j++)
                {
                    if (rd)
                    {
                        c = ImageProcessor.getPixel(i, j, data3);
                        if (c.R != 0)
                        {
                            b2 = (int) ((Z[i, j] - b2_min)*max);
                        }
                        else b2 = 0;
                    }
                    else
                    {
                        b2 = (int) ((Z[i, j] - b2_min)*max);
                    }
                    if (b2 < 0 || b2 > 255) b2 = 0;
                    ImageProcessor.setPixel(data, i, j, Color.FromArgb(b2, b2, b2));       
                    //bmp.SetPixel(i, j, Color.FromArgb(b2, b2, b2));                 
                }
                   
                   
               
                done++; 
                PopupProgressBar.setProgress(done, all);
            }

            PopupProgressBar.close();
            bmp.UnlockBits(data);
            bmp3.UnlockBits(data3);
          
        }
//-----------------------------------------------------------------------------------------------------------------------------------
/*
        public static void pi2_ABC( PictureBox pictureBox01, int xx0, int xx1, int yy0, int yy1) // Устранение тренда по методу наименьших квадратов
        {

            int w = pictureBox01.Width;
            int h = pictureBox01.Height;
            Z = new Int64[w, h];                //------------------------------- УБРАТЬ
            Color c;
            Bitmap bmp = new Bitmap(pictureBox01.Image, w, h);
            for (int i = 0; i < w; i++) for (int j = 0; j < h; j++) { c = bmp.GetPixel(i, j);  Z[i, j] = c.R; }
            ABC(Z, xx0, xx1, yy0, yy1);
            MessageBox.Show(" A "+ A + " B " + B + " C " + C);
            for (int i = 0; i < w; i++) for (int j = 0; j < h; j++) Z[i, j] = Z[i, j] - Convert.ToInt32(A * i + B * j + C);           

            Z_bmp(bmp, Z);                                                                          //  Z -> bmp с масштабированием
            pictureBox01.Size = new Size(w, h);
            pictureBox01.Image = bmp;
        }
 */
 // ------------------------------------------------------------------------------------------------------------------- Вычитание плоскости, проходящей через 3 точки
 /*       public static void NKL(PictureBox pictureBox01, int x1, int y1, int x2, int y2, int x3, int y3)
        {   
            
            double ax=x2-x1, ay=y2-y1, az=Z[x2,y2]-Z[x1,y1];
            double bx=x3-x1, by=y3-y1, bz=Z[x3,y3]-Z[x1,y1];
                A = ay*bz-az*by;
                B = -(ax*bz-bx*az);
                C = ax*by-ay*bx;
                D = -(A*x1 + B*y1 + C*Z[x1,y1]);               
                A = A / C; B = B / C; D = D / C;
                MessageBox.Show(" A " + A.ToString() + " B " + B.ToString() + " C " + C.ToString() + " D " + D.ToString());
                int w = pictureBox01.Width;
                int h = pictureBox01.Height;

                for (int i = 0; i < w; i++) for (int j = 0; j < h; j++) Z[i, j] =Z[i, j] + Convert.ToInt32(A * i + B * j + D);
                Bitmap bmp = new Bitmap(pictureBox01.Image, w, h);
                Z_bmp(bmp, Z);                                                                          //  Z -> bmp с масштабированием
                pictureBox01.Size = new System.Drawing.Size(w, h);
                pictureBox01.Image = bmp;
        }

*/


    }
}
