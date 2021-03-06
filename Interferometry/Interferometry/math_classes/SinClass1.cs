﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using rab1;

namespace Interferometry.math_classes
{
    public static class SinClass1
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        public static Bitmap drawSine(double waveNumbers, double phaseShift, int width, int height, int XY)    // sin b/w
        {
            int i, j;
            const double PI = Math.PI;
            double af = PI * 2 * waveNumbers / width;
            Bitmap result = new Bitmap(width, height);
            BitmapData bitmapData = ImageProcessor.getBitmapData(result);

            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    int colorComponent;
                    if (XY == 0)
                    {
                        colorComponent = (int) ((Math.Sin(af*i + PI*phaseShift/180) + 1)*255.0/2.0);
                    }
                    else
                    {
                        colorComponent = (int)((Math.Sin(af * j + PI * phaseShift / 180) + 1) * 255.0 / 2.0);
                    }

                    ImageProcessor.setPixel(bitmapData, i, j, Color.FromArgb(colorComponent, colorComponent, colorComponent));
                }
            }

            result.UnlockBits(bitmapData);
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
        public static Bitmap drawBitImage(double N_sin, double f1, int width, int height, int XY, int MASK, BackgroundImagesGeneratorForm.BitImageType type)    // bit b/w
        {
            int i, j;
            byte r;
            double nx = width + 1;
            double pi = Math.PI;
            double af = pi * 2 * N_sin / nx;
            Bitmap result = new Bitmap(width, height);
            BitmapData bitmapData = ImageProcessor.getBitmapData(result);

            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    if (XY == 0)
                    {
                        r = (byte)((Math.Sin(af * i + pi * f1 / 180) + 1) * 127);
                    }
                    else
                    {
                        r = (byte)((Math.Sin(af * j + pi * f1 / 180) + 1) * 127);
                    }

                    if (r > 255)
                    {
                        r = 0;
                    }

                    r = BITMASK(r, MASK);

                    ImageProcessor.setPixel(bitmapData, i, j, Color.FromArgb(r, r, r));
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

            for (int i = 0; i < width; i++)
            {
                Color c;

                for (int j = 0; j < height; j++)
                {
                    c = ImageProcessor.getPixel(i, j, bitmapData); 
                    cr[j] = c.R; 
                    cg[j] = c.G; 
                    cb[j] = c.B;
                }

                int kk = 8;

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
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
