﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Interferometry.forms;
using Interferometry.math_classes;
using rab1;
using MessageBox = System.Windows.MessageBox;

namespace Interferometry
{
    class Utils
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public enum RGBColor
        {
            Red,
            Green, 
            Blue,
            Gray
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Метод для получения изображения из массива
        /// </summary>
        public static BitmapSource getImageFromArray(ZArrayDescriptor newDescriptor, RGBColor color)
        {
            if (newDescriptor == null)
            {
                return null;
            }

            long max = getMax(newDescriptor);
            long min = getMin(newDescriptor);

            if (max - min == 0)
            {
                max = 1;
                min = 0;
            }

            return getImageFromArray(newDescriptor, min, max, color);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Метод для получения изображения из массива
        /// </summary>
        public static BitmapSource getImageFromArray(ZArrayDescriptor newDescriptor)
        {
            return getImageFromArray(newDescriptor, RGBColor.Gray);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Метод для получения изображения из массива
        /// </summary>
        public static BitmapSource getImageFromArray(ZArrayDescriptor newDescriptor, long min, long max)
        {
            return getImageFromArray(newDescriptor, min, max, RGBColor.Gray);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Метод для получения изображения из массива
        /// </summary>
        public static BitmapSource getImageFromArray(ZArrayDescriptor newDescriptor, long min, long max, RGBColor color)
        {
            if (newDescriptor == null)
            {
                return null;
            }

            if (max - min == 0)
            {
                max = 1;
                min = 0;
            }

            double multiplier = (255 / (double)(max - min));

            Bitmap newBitmap = new Bitmap(newDescriptor.width, newDescriptor.height);
            BitmapData data = ImageProcessor.getBitmapData(newBitmap);

            for (int i = 0; i < newDescriptor.width; i++)
            {
                for (int j = 0; j < newDescriptor.height; j++)
                {
                    int colorComponent = (int)((newDescriptor.array[i][j] - min) * multiplier);

                    if (colorComponent > 255) { colorComponent = 255; }
                    if (colorComponent < 0) { colorComponent = 0; }

                    if (color == RGBColor.Red)
                    {
                        ImageProcessor.setPixel(data, i, j, Color.FromArgb(colorComponent, 0, 0));
                    }
                    else if (color == RGBColor.Green)
                    {
                        ImageProcessor.setPixel(data, i, j, Color.FromArgb(0, colorComponent, 0));
                    }
                    else if (color == RGBColor.Blue)
                    {
                        ImageProcessor.setPixel(data, i, j, Color.FromArgb(0, 0, colorComponent));
                    }
                    else
                    {
                        ImageProcessor.setPixel(data, i, j, Color.FromArgb(colorComponent, colorComponent, colorComponent));
                    }
                }
            }

            newBitmap.UnlockBits(data);

            BitmapSource result = FilesHelper.bitmapToBitmapImage(newBitmap);
            result.Freeze();
            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Метод для получения массива из Bitmap. В массив записывается интенсивность (I = 1/3(R + G + B))
        /// </summary>
        public static ZArrayDescriptor getArrayFromImage(BitmapSource someImage)
        {
            if (someImage == null)
            {
                MessageBox.Show("someImage == null!!!!");
                return null;
            }

            Bitmap newBitmap = FilesHelper.bitmapImageToBitmap((BitmapImage) someImage);
            ZArrayDescriptor result = getArrayFromImage(newBitmap);

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static ZArrayDescriptor getArrayFromImage(Bitmap newBitmap)
        {
            if (newBitmap == null)
            {
                MessageBox.Show("newBitmap == null!!!!");
                return null;
            }

            ZArrayDescriptor result = new ZArrayDescriptor();
            result.array = new long[newBitmap.Width][];

            for (int i = 0; i < newBitmap.Width; i++)
            {
                result.array[i] = new long[newBitmap.Height];
            }

            result.width = newBitmap.Width;
            result.height = newBitmap.Height;

            BitmapData data = ImageProcessor.getBitmapData(newBitmap);

            for (int i = 0; i < newBitmap.Width; i++)
            {
                for (int j = 0; j < newBitmap.Height; j++)
                {
                    Color currentColor = ImageProcessor.getPixel(i, j, data);
                    result.array[i][j] = (currentColor.R + currentColor.G + currentColor.B) / 3;
                }
            }

            newBitmap.UnlockBits(data);

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Метод для получения максимума из массива
        /// </summary>
        public static long getMax(ZArrayDescriptor newDescriptor)
        {
            if (newDescriptor == null)
            {
                return 0;
            }

            long max = long.MinValue; ;

            for (int i = 0; i < newDescriptor.width; i++)
            {
                for (int j = 0; j < newDescriptor.height; j++)
                {
                    max = Math.Max(max, newDescriptor.array[i][j]);
                }
            }

            return max;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Метод для получения минимума из массива
        /// </summary>
        public static long getMin(ZArrayDescriptor newDescriptor)
        {
            if (newDescriptor == null)
            {
                return 0;
            }

            long min = long.MaxValue;

            for (int i = 0; i < newDescriptor.width; i++)
            {
                for (int j = 0; j < newDescriptor.height; j++)
                {
                    min = Math.Min(min, newDescriptor.array[i][j]);
                }
            }

            return min;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static ZArrayDescriptor cutZArray(ZArrayDescriptor someDescriptor, int min, int max)
        {
            for (int i = 0; i < someDescriptor.width; i++)
            {
                for (int j = 0; j < someDescriptor.height; j++)
                {
                    if (someDescriptor.array[i][j] > max)
                    {
                        someDescriptor.array[i][j] = max;
                    }

                    if (someDescriptor.array[i][j] < min)
                    {
                        someDescriptor.array[i][j] = min;
                    }
                }
            }

            return someDescriptor;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static long[][] createLongArray(int width, int height)
        {
             long[][] array = new long[width][];

            for (int i = 0; i < width; i++)
            {
                array[i] = new long[height];
            }

            return array;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static double[][] createDoubleArray(int width, int height)
        {
            double[][] array = new double[width][];

            for (int i = 0; i < width; i++)
            {
                array[i] = new double[height];
            }

            return array;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static int[][] createIntArray(int width, int height)
        {
            int[][] array = new int[width][];

            for (int i = 0; i < width; i++)
            {
                array[i] = new int[height];
            }

            return array;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static int maxInt(int[][] someArray, int width, int height)
        {
            int result = int.MinValue;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    result = Math.Max(result, someArray[i][j]);
                }
            }

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static long maxLong(long[][] someArray, int width, int height)
        {
            long result = long.MinValue;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    result = Math.Max(result, someArray[i][j]);
                }
            }

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static long maxFromArray(ZArrayDescriptor someArray)
        {
            long result = long.MinValue;

            for (int i = 0; i < someArray.width; i++)
            {
                for (int j = 0; j < someArray.height; j++)
                {
                    result = Math.Max(result, someArray.array[i][j]);
                }
            }

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Bitmap mergeBitmaps(Bitmap firstBitmap, Bitmap secondBitmap)
        {
            BitmapData firstData = ImageProcessor.getBitmapData(firstBitmap);
            BitmapData secondData = ImageProcessor.getBitmapData(secondBitmap);

            int minWidth = Math.Min(firstBitmap.Width, secondBitmap.Width);
            int minHeight = Math.Min(firstBitmap.Height, secondBitmap.Height);

            Bitmap result = new Bitmap(minWidth, minHeight);
            BitmapData resultData = ImageProcessor.getBitmapData(result);

            for (int x = 0; x < minWidth; x++)
            {
                for (int y = 0; y < minHeight; y++)
                {
                    Color firstColor = ImageProcessor.getPixel(x, y, firstData);
                    Color secondColor = ImageProcessor.getPixel(x, y, secondData);

                    int redComponent = Math.Min(firstColor.R + secondColor.R, 255);
                    int greenComponent = Math.Min(firstColor.G + secondColor.G, 255);
                    int blueComponent = Math.Min(firstColor.B + secondColor.B, 255);

                    ImageProcessor.setPixel(resultData, x, y, Color.FromArgb(redComponent, greenComponent, blueComponent));
                }
            }

            firstBitmap.UnlockBits(firstData);
            secondBitmap.UnlockBits(secondData);
            result.UnlockBits(resultData);

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static double degreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static double radianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        } 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
