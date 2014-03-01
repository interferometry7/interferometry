using System;

using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Interferometry.forms;

namespace rab1
{
    public class SinClass1
    {

       // 
        public static Bitmap drawSine(double waveNumbers, double phaseShift, int width, int height, int XY)    // sin b/w
        {
            int i, j;
            int colorComponent;
            const double PI = Math.PI;
            double af = PI * 2 * waveNumbers / width;
            Bitmap result = new Bitmap(width, height);

            if (XY == 0) // Полосы ориентированы перпендикулярно оси X
            {
                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        colorComponent = (int) ((Math.Sin(af*i + PI*phaseShift/180) + 1) * 255.0/2.0);

                        if (XY == 0)
                        {
                            result.SetPixel(i, j, Color.FromArgb(colorComponent, colorComponent, colorComponent));
                        }
                        else
                        {
                            result.SetPixel(j, i, Color.FromArgb(colorComponent, colorComponent, colorComponent));
                        }
                    }
                }
            }

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Bitmap drawLines(double N_sin, double f1, int max_x, int max_y, int XY)    // sin b/w
        {
            Bitmap result = new Bitmap(max_x, max_y);
            BitmapData bitmapData = ImageProcessor.getBitmapData(result);
            byte r;
            double nx = max_x + 1;
            double pi = Math.PI;
            double af = pi * 2 * N_sin / nx;

            if (XY == 0)  // Полосы ориентированы перпендикулярно оси X
            {
                for (int i = 0; i < max_x; i++)
                {
                    for (int j = 0; j < max_y; j++)
                    {
                        r = (byte)((Math.Sin(af * i + 1 + pi * f1 / 180) + 1) * 127);
                        if (r > 255)
                        {
                            r = 0;
                        }

                        if (r > 125)
                        {
                            r = 255;
                        }
                        else
                        {
                            r = 0;
                        }

                        ImageProcessor.setPixel(bitmapData, i, j, Color.FromArgb(r, r, r));
                    }
                }
            }

            if (XY == 1) // Полосы ориентированы перпендикулярно оси y
            {
                for (int i = 0; i < max_y; i++)
                {
                    for (int j = 0; j < max_x; j++)
                    {
                        r = (byte)((Math.Sin(af * i + 1 + pi * f1 / 180) + 1) * 127);
                        if (r > 255)
                        {
                            r = 0;
                        }

                        if (r > 125)
                        {
                            r = 255;
                        }
                        else
                        {
                            r = 0;
                        }

                        ImageProcessor.setPixel(bitmapData, j, i, Color.FromArgb(r, r, r));
                    }
                }
            }

            result.UnlockBits(bitmapData);
            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Bitmap drawDitheredLines(double N_sin, double f1, int max_x, int max_y, int XY)
        {
            Bitmap result = new Bitmap(max_x, max_y);
            BitmapData bitmapData = ImageProcessor.getBitmapData(result);
            byte r;
            double nx = max_x + 1;
            double pi = Math.PI;
            double af = pi * 2 * N_sin / nx;

            if (XY == 0)  // Полосы ориентированы перпендикулярно оси X
            {
                for (int i = 0; i < max_x; i++)
                {
                    for (int j = 0; j < max_y; j++)
                    {
                        r = (byte)((Math.Sin(af * i + 1 + pi * f1 / 180) + 1) * 127);
                        if (r > 255)
                        {
                            r = 0;
                        }
                        ImageProcessor.setPixel(bitmapData, i, j, Color.FromArgb(r, r, r));
                    }
                }
            }
            else if (XY == 1) // Полосы ориентированы перпендикулярно оси y
            {
                for (int i = 0; i < max_y; i++)
                {
                    for (int j = 0; j < max_x; j++)
                    {
                        r = (byte)((Math.Sin(af * i + 1 + pi * f1 / 180) + 1) * 127);
                        if (r > 255)
                        {
                            r = 0;
                        }

                        ImageProcessor.setPixel(bitmapData, j, i, Color.FromArgb(r, r, r));
                    }
                }
            }


            Color currentColor;
            int averageColor;
            Color newColor;
            double quantError;

            for (int y = 0; y < max_y; y++)
            {
                for (int x = 0; x < max_x; x++)
                {
                    currentColor = ImageProcessor.getPixel(x, y, bitmapData);
                    averageColor = currentColor.R;

                    if (averageColor >= 128)
                    {
                        newColor = Color.White;
                    }
                    else
                    {
                        newColor = Color.Black;
                    }

                    quantError = averageColor - newColor.R;

                    ImageProcessor.setPixel(bitmapData, x, y, newColor);

                    if (x < max_x - 1)
                    {
                        currentColor = ImageProcessor.getPixel(x + 1, y, bitmapData);
                        double a = 7.0 / 16.0 * quantError;
                        averageColor = currentColor.R + (int)a;

                        if (averageColor > 255)
                        {
                            averageColor = 255;
                        }

                        if (averageColor < 0)
                        {
                            averageColor = 0;
                        }

                        ImageProcessor.setPixel(bitmapData, x + 1, y, Color.FromArgb(averageColor, averageColor, averageColor));
                    }

                    if ((y < max_y - 1) && (x > 0))
                    {
                        currentColor = ImageProcessor.getPixel(x - 1, y + 1, bitmapData);
                        double a = (3.0 / 16.0) * quantError;
                        averageColor = currentColor.R + (int)a;

                        if (averageColor > 255)
                        {
                            averageColor = 255;
                        }

                        if (averageColor < 0)
                        {
                            averageColor = 0;
                        }

                        ImageProcessor.setPixel(bitmapData, x - 1, y + 1, Color.FromArgb(averageColor, averageColor, averageColor));
                    }

                    if (y < max_y - 1)
                    {
                        currentColor = ImageProcessor.getPixel(x, y + 1, bitmapData);
                        double a = (5.0 / 16.0) * quantError;
                        averageColor = currentColor.R + (int)a;

                        if (averageColor > 255)
                        {
                            averageColor = 255;
                        }

                        if (averageColor < 0)
                        {
                            averageColor = 0;
                        }

                        ImageProcessor.setPixel(bitmapData, x, y + 1, Color.FromArgb(averageColor, averageColor, averageColor));
                    }

                    if ((y < max_y - 1) && (x < max_x - 1))
                    {
                        currentColor = ImageProcessor.getPixel(x + 1, y + 1, bitmapData);
                        double a = (1.0 / 16.0) * quantError;
                        averageColor = currentColor.R + (int)a;

                        if (averageColor > 255)
                        {
                            averageColor = 255;
                        }

                        if (averageColor < 0)
                        {
                            averageColor = 0;
                        }

                        ImageProcessor.setPixel(bitmapData, x + 1, y + 1, Color.FromArgb(averageColor, averageColor, averageColor));
                    }
                }
            }

            int black = 0;
            int white = 0;

            for (int y = 0; y < max_y; y++)
            {
                for (int x = 0; x < max_x; x++)
                {
                    currentColor = ImageProcessor.getPixel(x, y, bitmapData);

                    if ((currentColor.ToArgb() != Color.White.ToArgb()) && (currentColor.ToArgb() != Color.Black.ToArgb()))
                    {
                        if (currentColor.R >= 128)
                        {
                            newColor = Color.White;
                        }
                        else
                        {
                            newColor = Color.Black;
                        }
                        ImageProcessor.setPixel(bitmapData, x, y, newColor);
                    }

                    if (currentColor.ToArgb() == Color.White.ToArgb())
                    {
                        white++;
                    }
                    else
                    {
                        black++;
                    }
                }
            }

            result.UnlockBits(bitmapData);
            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Bitmap drawBitImage(double N_sin, double f1, int width, int height, int XY, int MASK)    // bit b/w
        {
            int i, j;
            byte r;
            double nx = width + 1;
            double pi = Math.PI;
            double af = pi * 2 * N_sin / nx;
            Bitmap result = new Bitmap(width, height);
            BitmapData bitmapData = ImageProcessor.getBitmapData(result);

            if (XY == 0)
            {
                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        r = (byte) ((Math.Sin(af*i + pi*f1/180) + 1)*127);
                        if (r > 255) r = 0;
                        r = BITMASK(r, MASK);
                        ImageProcessor.setPixel(bitmapData, i, j, Color.FromArgb(r, r, r));
                    }
                }
            }

            if (XY == 1)
            {
                for (i = 0; i < height; i++)
                {
                    for (j = 0; j < width; j++)
                    {
                        r = (byte) ((Math.Sin(af*i + pi*f1/180) + 1)*127);
                        if (r > 255) r = 0;
                        r = BITMASK(r, MASK);
                        ImageProcessor.setPixel(bitmapData, j, i, Color.FromArgb(r, r, r));
                    }
                }
            }

            result.UnlockBits(bitmapData);
            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static byte BITMASK(byte k, int MASK)
        {  
            switch (MASK)
            {
                case 1: k &= 1; k <<= 7;  if (k == 128) k = 255; else k = 0; break;
                case 2: k &= 2; k <<= 6;  if (k == 128) k = 255; else k = 0; break;
                case 3: k &= 4; k <<= 5;  if (k == 128) k = 255; else k = 0; break;
                case 4: k &= 8; k <<= 4;  if (k == 128) k = 255; else k = 0; break;
                case 5: k &= 16; k <<= 3; if (k == 128) k = 255; else k = 0; break;
                case 6: k &= 32; k <<= 2; if (k == 128) k = 255; else k = 0; break;
                case 7: k &= 64; k <<= 1; if (k == 128) k = 255; else k = 0; break;
                case 8: k &= 128; if (k == 128) k = 255; else k = 0; break;
            }          
            return k;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  

// Кнопка ok
        public static void b1_RGB(double N_sin, double N_fz, int XY, int rb, int MASK, int max_x, int  max_y,  PictureBox pc1, int wrt)
        {
            
            int i, j;
            double N =  N_sin;                         // число синусоид
            double N1 = max_x / N;  // Число точек в одном периоде
            double N2 = N1 / 255;
            double pi = Math.PI;         

            byte r, g, b;
            double nx = max_x + 1;

            double af = pi * 2 * N / nx;
            Bitmap bmp_sin = new Bitmap(max_x, max_y);

            if (rb == 1 && MASK == 0)      //  Sinus (Цветное Все биты)
            {
                if (XY == 1)  // Полосы ориентированы перпендикулярно оси X
                    for (i = 0; i < max_x; i++)
                        for (j = 0; j < max_y; j++)
                        {
                            r = (byte)((Math.Sin(af * i) + 1) * 127); if (r > 255) r = 0;
                            g = (byte)((Math.Sin(af * i + 2 * pi / 3) + 1) * 127); if (g > 255) g = 0;
                            b = (byte)((Math.Sin(af * i + 4 * pi / 3) + 1) * 127); if (b > 255) b = 0;
                            bmp_sin.SetPixel(i, j, Color.FromArgb(r, g, b)); 
                            
                        }
                if (XY == 0) // Полосы ориентированы перпендикулярно оси y
                    for (i = 0; i < max_y; i++)
                        for (j = 0; j < max_x; j++)
                        {
                            r = (byte)((Math.Sin(af * i) + 1) * 127); if (r > 255) r = 0;
                            g = (byte)((Math.Sin(af * i + 2 * pi / 3) + 1) * 127); if (g > 255) g = 0;
                            b = (byte)((Math.Sin(af * i + 4 * pi / 3) + 1) * 127); if (b > 255) b = 0;
                            bmp_sin.SetPixel(j, i, Color.FromArgb(r, g, b)); 
                            
                        }
            }


            if (rb != 1 && MASK == 0)      //  Sinus ( ч/б)
            {
                if (XY == 1)  // Полосы ориентированы перпендикулярно оси X
                    for (i = 0; i < max_x; i++)
                        for (j = 0; j < max_y; j++)
                        {
                            r = (byte)((Math.Sin(af * i + pi * N_fz / 180) + 1) * 127); if (r > 255) r = 0;                           
                            bmp_sin.SetPixel(i, j, Color.FromArgb(r, r, r));

                        }
                if (XY == 0) // Полосы ориентированы перпендикулярно оси y
                    for (i = 0; i < max_y; i++)
                        for (j = 0; j < max_x; j++)
                        {
                            r = (byte)((Math.Sin(af * i + pi * N_fz / 180) + 1) * 127); if (r > 255) r = 0;                           
                            bmp_sin.SetPixel(j, i, Color.FromArgb(r, r, r));

                        }
            }

            if (rb == 1 && MASK != 0)            // По битам (Три цвета)
            {
                if (XY == 1)
                    for (i = 0; i < max_x; i++)
                        for (j = 0; j < max_y; j++)
                        {
                            r = (byte)((Math.Sin(af * i + pi * N_fz / 180) + 1) * 127); if (r > 255) r = 0;
                            g = (byte)((Math.Sin(af * i + 2 * pi / 3) + 1) * 127);      if (g > 255) g = 0;
                            b = (byte)((Math.Sin(af * i + 4 * pi / 3) + 1) * 127);      if (b > 255) b = 0;

                            if (MASK != 0) { r = BITMASK(r, MASK); b = BITMASK(b, MASK); g = BITMASK(g, MASK); }
                            bmp_sin.SetPixel(i, j, Color.FromArgb(r, g, b));
                        }
                if (XY == 0)
                    for (i = 0; i < max_y; i++)
                        for (j = 0; j < max_x; j++)
                        {
                            r = (byte)((Math.Sin(af * i + pi * N_fz / 180) + 1) * 127); if (r > 255) r = 0;
                            if (MASK != 0) r = BITMASK(r, MASK);
                            g = (byte)((Math.Sin(af * i + 2 * pi / 3) + 1) * 127); if (g > 255) g = 0;
                            if (MASK != 0) g = BITMASK(g, MASK);
                            b = (byte)((Math.Sin(af * i + 4 * pi / 3) + 1) * 127); if (b > 255) b = 0;
                            if (MASK != 0) b = BITMASK(b, MASK);


                            bmp_sin.SetPixel(j, i, Color.FromArgb(r, g, b));
                        }
            }
            if (rb != 1 && MASK != 0)            // По битам ч/б
            {
                if (XY == 1)
                    for (i = 0; i < max_x; i++)
                        for (j = 0; j < max_y; j++)
                        {
                            r = (byte)((Math.Sin(af * i + pi * N_fz / 180) + 1) * 127); if (r > 255) r = 0;                       
                            r = BITMASK(r, MASK);                             
                            bmp_sin.SetPixel(i, j, Color.FromArgb(r, r, 0));
                        }
                if (XY == 0)
                    for (i = 0; i < max_y; i++)
                        for (j = 0; j < max_x; j++)
                        {
                            r = (byte)((Math.Sin(af * i + pi * N_fz / 180) + 1) * 127); if (r > 255) r = 0;
                            r = BITMASK(r, MASK);                          
                            bmp_sin.SetPixel(j, i, Color.FromArgb(r, r, 0));
                        }
            }
            pc1.Image = bmp_sin;
            if (wrt == 1)
            {
                string string_dialog = "D:\\Студенты\\Эксперимент";
                System.Windows.Forms.SaveFileDialog dialog1 = new System.Windows.Forms.SaveFileDialog();
                dialog1.InitialDirectory = string_dialog;
                dialog1.Filter = "JPEG (*.jpg)|*.jpg|Bitmap(*.bmp)|*.bmp";

                if (dialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pc1.Image.Save(dialog1.FileName);
                        dialog1.InitialDirectory = dialog1.FileName;
                        //string_dialog = dialog1.FileName;
                    }
                    catch (Exception ex) { MessageBox.Show(" Ошибка при записи файла " + ex.Message); }
                }                          

                
            }
    
        }
        // --------------------------------------------------------------------------------------------------- Объединение 8 битов
        public static Bitmap bit8(Image[] img)
         {            
            int width = img[7].Width;
            int height = img[7].Height;
            Bitmap result = new Bitmap(width, height);

            Color  c;
            int r1, g1, b1;
            int[] cr = new int[height];
            int[] cb = new int[height];
            int[] cg = new int[height];

            int[] cr1 = new int[height];
            int[] cb1 = new int[height];
            int[] cg1 = new int[height];                                                  

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++) 
                { 
                    c = result.GetPixel(i, j); 
                    cr[j] = c.R; 
                    cg[j] = c.G; 
                    cb[j] = c.B; 
                }

                for (int k = 0; k < 8; k++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        c = ((Bitmap)img[k]).GetPixel(i, j); 
                        cr1[j] = c.R; 
                        cg1[j] = c.G; 
                        cb1[j] = c.B;
                    }

                    if (k != 0)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            r1 = ((cr1[j] > 127) ? 1 : 0) << k;
                            g1 = ((cg1[j] > 127) ? 1 : 0) << k;
                            b1 = ((cb1[j] > 127) ? 1 : 0) << k;
                            cr[j] |= r1;
                            cg[j] |= g1;
                            cb[j] |= b1;
                        }
                    }
                }

                for (int j = 0; j < height; j++)
                {
                    result.SetPixel(i, j, Color.FromArgb(cr[j], cg[j], cb[j]));
                }
            }

            return result;
         }

        // ------------------------------------------------------------------------------------------------------ 8 bit Cуммирование
        public static Bitmap bit8sin(Image[] img)
        {
            int width = img[7].Width;
            int height = img[7].Height;
            Bitmap result = new Bitmap(width, height);
            BitmapData bitmapData = ImageProcessor.getBitmapData(result);

            BitmapData[] imagesData = new BitmapData[8];

            for (int i = 0; i < 8; i++)
            {
                imagesData[i] = ImageProcessor.getBitmapData((Bitmap)img[i]);
            }

            int[] cr = new int[height];
            int[] cb = new int[height];
            int[] cg = new int[height];

            int[] cr1 = new int[height];
            int[] cb1 = new int[height];
            int[] cg1 = new int[height];
                                                           
            int kk;
            Color c;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    c = ImageProcessor.getPixel(i, j, bitmapData); 
                    cr[j] = c.R; 
                    cg[j] = c.G; 
                    cb[j] = c.B;
                }

                kk = 8;

                for (int k = 0; k < 8; k++, kk--)
                {
                    for (int j = 0; j < height; j++)
                    {
                        c = ImageProcessor.getPixel(i, j, imagesData[k]); 
                        cr1[j] = c.R; 
                        cg1[j] = c.G; 
                        cb1[j] = c.B;
                    } 

                    for (int j = 0; j < height; j++)
                    {
                        cr[j] += cr1[j] >> kk; 
                        cg[j] += cg1[j] >> kk; 
                        cb[j] += cb1[j] >> kk;  
                    }
                }

                for (int j = 0; j < height; j++)
                { 
                    if (cr[j] > 255) cr[j] = 255; 
                    if (cg[j] > 255) cg[j] = 255; 
                    if (cb[j] > 255) cb[j] = 255;

                    ImageProcessor.setPixel(bitmapData, i, j, Color.FromArgb(cr[j], cb[j], cg[j])); 
                }
            }

            for (int i = 0; i < 8; i++)
            {
                ((Bitmap)img[i]).UnlockBits(imagesData[i]); ;
            }

            result.UnlockBits(bitmapData);
            return result;
        }
    }
}
