using System;
using System.Data.SqlTypes;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Interferometry.forms;
using Interferometry.math_classes;
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
        static int NMAX=4800;
        static int[] glbl_faze  = new int[NMAX];               // Номера для прослеживания полос (номера линий)
        static int[] glbl_faze1 = new int[NMAX];               // Количество добавлений b1 (Для расшифровки)
        static int[] glbl_faze2 = new int[NMAX];               // Адрес точной дигонали (Для расшифровки)

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

        private static int China(int sN1, int sN2)
        {
            int n;
            n1 = sN1;
            n2 = sN2;
            Int32 NOD = Evklid(n1, n2); // Если NOD == 1 числа взаимно просты
            if (NOD != 1) 
                { MessageBox.Show("Числа не взаимно просты. Наибольший общий делитель равен - "+ NOD);
                  n1 = sN1 / NOD;
                  n2 = sN2 / NOD;
                }

            M1 = n2;
            M2 = n1;
            N1 = -1;
            N2 = -1;
            for (int i = 0; i < M1; i++) { n = (M1 * i) % n1; if (n == 1) { N1 = i; break; } } if (N1 < 0) N1 = N1 + n1;
            for (int i = 0; i < M2; i++) { n = (M2 * i) % n2; if (n == 1) { N2 = i; break; } } if (N2 < 0) N2 = N2 + n2;
            return NOD;
        }
// --------------------------------------------------------------------------------------------------------------------------- Рисование таблицы  (параметры) (b2, b1)
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //
        //           Построение таблицы
        //
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void Graph_China2(ZArrayDescriptor[] img, int NOD, int Diag, bool rb, int pr_obr, int sdvg_x, int X, int Y)
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

            string s = n2.ToString(); 
            grBack.DrawString("b2 " + s, font, new SolidBrush(Color.Black), w1 * scale + x0 + 8, y0 - 8, drawFormat);
            s = n1.ToString();
            grBack.DrawString("b1 " + s, font, new SolidBrush(Color.Black), x0, hh * scale + 20 + 10 * scale, drawFormat);
            // ----------------------------------------------------------------------------------------------------------------
            GLBL_FAZE(n1, n2, Diag);                                     //  Заполнение glbl_faze[]  и glbl_faze1[] - допустимые границы диапазона
            // -----------------------------------------------------------------------------------------------Отрисовка диагоналей
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
            int count = bmp_2pi(img, NOD, bmp_r, Diag, pr_obr, sdvg_x);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Рисование точек в таблице по диагоналям
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            int b = 0, ib2 = 0, ib1 = 0;
            for (ib2 = 0; ib2 < n2 - 1; ib2++)
            {
                for (ib1 = 0; ib1 < n1 - 1; ib1++)
                {
                    b = bmp_r[ib2, ib1];                            
                    if (b > pr_obr) { grBack.DrawRectangle(new Pen(Color.FromArgb(146, 24, 47)), x0 + ib2 * scale, y0 + ib1 * scale, 1, 1); }
                }
            }
            // -------------------------------------------------------------------------------------Рисование одной точки X, Y
            
            int is1;
            int is2;

            is1 = (int)img[1].array[X, Y]; is1 += sdvg_x; if (is1 > n1) is1 -= n1;
            is2 = (int)img[0].array[X, Y];  
            grBack.DrawRectangle(new Pen(Color.FromArgb(0, 255, 0)), x0 + is2 * scale, y0 + is1 * scale, 25, 25);
            //MessageBox.Show(" x = " + is2 + " Y =  " + is1);
            //--------------------------------------------------------------------------------------------------------------------
            pc1.Refresh();
            f_sin.Show();
            bmp_gstgr(bmp_r);     // гистограмма
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //     Заполнение   массива  гистограмм
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void bmp_gstgr(int[,] bmp_r)
        {
            int n = n1 + n2 + 3;
            Int64[] buf = new Int64[n];
            int[] buf1 = new int[n];
            for (int i = 0; i < n1; i++)
            {
                for (int j = 0; j < n2; j++)
                {
                    buf[j + (n1 - i)] += bmp_r[j, i];
                }
            }

            for (int i = 0; i < n; i++) buf1[i] = glbl_faze[i];
            Graphic graphic = new Graphic(n, 0, buf, buf1);
            graphic.Show();

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //     Заполнение  массива bmp_r
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static int bmp_2pi(ZArrayDescriptor[] img, int NOD, int[,] bmp_r, int Diag, int pr_obr, int sdvg_x)
        {
            int r;
            int g;
            int w = img[0].width;
            int h = img[0].height;

            int[] ims1 = new int[h];
            int[] ims2 = new int[h];
            int[] ims3 = new int[h];
     
            //double fn1 = (double)(n1 - 1) / 255;
            //double fn2 = (double)(n2 - 1) / 255;


            int all = w;
            int done = 0;
            PopupProgressBar.show();

            int count = 0;
            //int max = 0;
            // ------------------------------------------------------------------------- По фигуре из 3 квадрата
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j <h; j++)
                {
                    if (img[2].array[i, j] == 0)
                    {
                        ims3[j] = 0;
                    }
                    else
                    {
                        ims3[j] = 1;                    
                        ims1[j] = (int)(img[1].array[i, j] / NOD);
                        ims2[j] = (int)(img[0].array[i, j] / NOD);
                    }

                }

                for (int j = 0; j < h; j++)
                {
                    if (ims3[j] != 0)
                    {
                        r = ims1[j];    r = r + sdvg_x; if (r > n1) r -= n1;
                        g = ims2[j];
                        bmp_r[g, r]++; 
                        count++;
                    }

                }
                done++;
                PopupProgressBar.setProgress(done, all);
               
            }

           // MessageBox.Show(" Max = " + max.ToString());
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
        public static void pi2_frml2(ZArrayDescriptor[] img, int sN1, int sN2, int Diag, bool rb, int pr_obr, int sdvg_x, int X, int Y)
        {
            int NOD = China(sN1, sN2);
            MessageBox.Show(" M1 = " + M1 + " N1 =  " + N1 + " M2 =  " + M2 + " N2 =  " + N2 + " M1*N1 =  " + M1*N1 + " M2*N2 =  " + M2*N2);
            Graph_China2(img, NOD, Diag, rb, pr_obr, sdvg_x, X, Y);            // Вычисление формулы => n1, n2, NOD
                                      // Построение таблицы
        }

//-----------------------------------------------------------------------------------------------------------------------------------
        
//-----------------------------------------------------------------------------------------------------------------------------------
        public static ZArrayDescriptor pi2_rshfr(ZArrayDescriptor[] img, int sN1, int sN2, int Diag,  int pr_obr, int sdvg_x) // Расшифровка
        {

            int w = img[0].width;
            int h = img[0].height;  
            

            int NOD=China(sN1, sN2);                                     // Проверка на взаимную простоту
                                                                         // Вычисление формулы sN1, sN2 -> в глобальные n1, n2
           // int[,] bmp_r = new int[sN2 + 3, sN1 + 3];                  // Массив точек в таблице 2pi
           // bmp_2pi(img, bmp_r, Diag, pr_obr, sdvg_x);                 // Заполнение массива bmp_r

             

            Z = new Int64[w, h];

            GLBL_FAZE(n1, n2, Diag);                                     // Заполнение массива glbl_faze[] (Все -1 кроме номеров полос) 
                                                                         // glbl_faze1[] расширяется значениям номеров полос
                                                                         //  РАСШИФРОВКА (Заполнение Z[,])
            rash_2pi(img[1], img[0], img[2], NOD, sdvg_x, n1, n2, Diag, Z);

            ZArrayDescriptor result = new ZArrayDescriptor();
            result.array = new long[w, h];
            result.width = w;
            result.height = h;
            for (int i = 0; i < w; i++)                                                                   //  Отображение точек на pictureBox01
            {
                for (int j = 0; j < h; j++)
                {
                    result.array[i, j] = Z[i, j];
                }
            }
            //Z_bmp(result, Z);                                           //  Z -> bmp с масштабированием (bmp3 - маска)

          return result;
        }
        // -----------------------------------------------------------------------------------------------------------------------------------           
        // -----------------------------------       Вычитание наклона  -> в вещественный массив Z             -------------------------------          
        // -----------------------------------------------------------------------------------------------------------------------------------  
        public static ZArrayDescriptor Z_sub(int x1, int y1, int x2, int y2, ZArrayDescriptor descriptor, double cosinusDegrees)
        {
            double cos = Math.Cos(cosinusDegrees * Math.PI / 180);
            y2 = y1;
            long z1 = descriptor.array[x1, y1];
            long z2 = descriptor.array[x2, y2];
            //MessageBox.Show(" X1 = " + x1 + " X2 = " + x2 + " y1 = " + y1 + " y2 = " + y2);
            double tt = (z2 - z1)*cos / (double)(x2 - x1);

            long[] s = new long[descriptor.width];

            for (int i = 0; i < descriptor.width; i++)
            {
                s[i] = (Int64)(tt * (i - x1)) + (z1);
            }

            ZArrayDescriptor result = new ZArrayDescriptor();
            result.width = descriptor.width;
            result.height = descriptor.height;
            result.array = new long[result.width, result.height];


            for (int i = 0; i < descriptor.width; i++)
            {
                for (int j = 0; j < descriptor.height; j++)
                {
                    result.array[i, j] = descriptor.array[i, j] - s[i];
                }
            }

            return result;
        }

        // -----------------------------------------------------------------------------------------------------------------------------------           
        // -----------------------------------       Сама расшифровка   -> в вещественный массив Z             -------------------------------          
        // -----------------------------------------------------------------------------------------------------------------------------------  

       // private static void rash_2pi(ZArrayDescriptor bmp1, ZArrayDescriptor bmp2, ZArrayDescriptor bmp3, int[,] bmp_r, int pr_obr, int sdvg_x, int n1, int n2, int Diag, Int64[,] Z)
        private static void rash_2pi(ZArrayDescriptor bmp1, ZArrayDescriptor bmp2, ZArrayDescriptor bmp3, int NOD, int sdvg_x, int n1, int n2, int Diag, Int64[,] Z)
        {
            //MessageBox.Show(" sdvg = " + sdvg_x + " n1 = " + n1 + " n2 = " + n2);
           
            int w = bmp1.width;
            int h = bmp1.height;

            int all = w; int done = 0; PopupProgressBar.show();

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {      
                   int i1 = (int)(bmp1.array[i, j]);
                   int i2 = (int)(bmp2.array[i, j]);
                   Z[i, j] = GLBL_R(n1, n2, i1, i2, sdvg_x, NOD);
                }
                //MessageBox.Show(" i = " + i);
                done++; 
                PopupProgressBar.setProgress(done, all);
            }

            PopupProgressBar.close();
        }
        //  ib1, ib2 - координаты в таблице
        //  i1, i2   - истинные координаты (по числу разрядов)
        //
        //   ------------------>   ib2
        //   |  
        //   |  
        //
        //   ib1
        private static long GLBL_R(int n1, int n2, int i1, int i2, int sdvg_x, int NOD)
        {
            double s2 = Math.Sqrt(2);
            int b1 = i1 / NOD; 
            int ib1 = b1 + sdvg_x;                    while (ib1 >= n1) { ib1 -= n1; };            // Сдвиг значений к нулевой диагонали
                           

            int ib2 = i2 / NOD;
            int i0 = ib2 + (n1 - ib1);           // Индекс в массиве

            i1 = i1 + (sdvg_x * NOD);               while (i1 >= (n1 * NOD)) { i1 -= (n1 * NOD); };
            int i00 = i2 + (n1 * NOD - i1);
            //int ll = glbl_faze2[i0]-i0;                                 
            int l = (glbl_faze2[i0]*NOD - i00);
            //l = 0;
            l = (int)((double)l / s2);
                                                 //if ((l < 0) && (glbl_faze2[i0]!=0)) MessageBox.Show(" i0 = " + i0 + " glbl_faze2[i0] = " + glbl_faze2[i0]);
            int ib = i1 - l ;               // Уточнение ---------------------------------------------------------------
           
            int b0 = glbl_faze1[i0];             // Значение ближайшей диагонали
            long z = (n1 * NOD) * b0 + ib;
           
            return z;
        }
           
        // ----------------------------------------------------------------------------------------------------------------           
        // ----------------        Заполнение массива для расшифровки  glbl_faze      -1 -1 2 -1 -1 
        // ----------------                                            glbl_faze1      22222222233333333
        // ----------------                                            glbl_faze2      адрес точной диагонали
        // ----------------------------------------------------------------------------------------------------------------          
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
                    int bb = glbl_faze[i]; 
                    if (bb >= 0 && bb != mnx) { mxx = bb; mxx_x = i; break; }
                }
                
                if (cntr >= n1 + n2 - 1) break;
                //MessageBox.Show(" mnx =  " + mnx.ToString() + " mxx =  " + mxx.ToString());                    
                int m = (mxx_x - mnx_x) / 2;                                                                                                                                                                                                                                                                                                                                                                                                                               
                for (int j = mnx_x; j < mnx_x + m; j++) { glbl_faze1[j] = mnx; glbl_faze2[j] = mnx_x; }
                for (int j = mnx_x + m; j < mxx_x; j++) { glbl_faze1[j] = mxx; glbl_faze2[j] = mxx_x; } 
                mnx_x = mxx_x;
                mnx = mxx;
            }

            
         //   for (int i = 0; i < n1 + n2; i++) { pf = glbl_faze1[i]; MessageBox.Show(" i =  " + i + "  glbl_faze1[i] " + glbl_faze1[i] + "  glbl_faze[i] " + glbl_faze[i]); }    

        }
        // -----------------------------------------------------------------------------------------------------------------------------------           
        // -----------------------------------        Z -> bmp с масштабированием                              -------------------------------          
        // -----------------------------------------------------------------------------------------------------------------------------------  
        /*       static void Z_bmp(ZArrayDescriptor bmp, Int64[,] Z)               // -------------------------- Z -> BMP
               {
            
                   int w = bmp.width; ;
                   int h = bmp.height;

                   for (int i = 0; i < w; i++)                                                                   //  Отображение точек на pictureBox01
                   {
                       for (int j = 0; j < h; j++)
                       {


                           bmp.array[i, j] = Z[i, j];
                       }
                   }

            
                   Int64 b2_min = 1000000, b2_max = -1000000;
                   int b2;

                   for (int i = 0; i < w; i++)
                       for (int j = 0; j < h; j++)
                       {

                           { b2_max = Math.Max(b2_max, Z[i, j]); b2_min = Math.Min(b2_min, Z[i, j]); }

                       }

                   double max = 255 / (double)(b2_max - b2_min);


                   int all = w; int done = 0; PopupProgressBar.show();
                   for (int i = 0; i < w; i++)                                                                   //  Отображение точек на pictureBox01
                   {
                       for (int j = 0; j < h; j++)
                       {
                           b2 = (int)((Z[i, j] - b2_min) * max);
                           if (b2 < 0) b2 = 0;
                           if (b2 > 255) b2 = 255;
                    
                           bmp.array[i, j] = b2;
                       }



                       done++;
                       PopupProgressBar.setProgress(done, all);
                   }

                   PopupProgressBar.close();
 
               }
       */
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

            getUnwrappedPhaseImage(bmp, Z);                                                                          //  Z -> bmp с масштабированием
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
                getUnwrappedPhaseImage(bmp, Z);                                                                          //  Z -> bmp с масштабированием
                pictureBox01.Size = new System.Drawing.Size(w, h);
                pictureBox01.Image = bmp;
        }

*/


    }
}
