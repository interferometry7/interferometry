using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using rab1;

namespace Interferometry
{
    class Utils
    {
        /// <summary>
        ///  Метод для получения изображения из массива
        /// </summary>
        public static BitmapSource getImageFromArray(Pi_Class1.ZArrayDescriptor newDescriptor)
        {
            if (newDescriptor == null)
            {
                return null;
            }

            double max = 0;
            double min = Double.MaxValue;

            for (int i = 0; i < newDescriptor.width; i++)
            {
                for (int j = 0; j < newDescriptor.height; j++)
                {
                    max = Math.Max(max, newDescriptor.array[i, j]);
                    min = Math.Min(min, newDescriptor.array[i, j]);
                }
            }

            double multiplier = 255 / (max - min);

            Bitmap newBitmap = new Bitmap(newDescriptor.width, newDescriptor.height);
            BitmapData data = ImageProcessor.getBitmapData(newBitmap);

            for (int i = 0; i < newDescriptor.width; i++)
            {
                for (int j = 0; j < newDescriptor.height; j++)
                {
                    int colorComponent = (int)(newDescriptor.array[i, j] * multiplier);
                    ImageProcessor.setPixel(data, i, j, Color.FromArgb(colorComponent, colorComponent, colorComponent));
                }
            }

            newBitmap.UnlockBits(data);

            BitmapSource result = FilesHelper.bitmapToBitmapImage(newBitmap);
            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Метод для получения массива из Bitmap. В массив записывается интенсивность (I = 1/3(R + G + B))
        /// </summary>
        public static Pi_Class1.ZArrayDescriptor getArrayFromImage(BitmapSource someImage)
        {
            if (someImage == null)
            {
                MessageBox.Show("someImage == null!!!!");
                return null;
            }

            Bitmap newBitmap = FilesHelper.bitmapImageToBitmap((BitmapImage) someImage);
            Pi_Class1.ZArrayDescriptor result = getArrayFromImage(newBitmap);

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Pi_Class1.ZArrayDescriptor getArrayFromImage(Bitmap newBitmap)
        {
            if (newBitmap == null)
            {
                MessageBox.Show("newBitmap == null!!!!");
                return null;
            }

            Pi_Class1.ZArrayDescriptor result = new Pi_Class1.ZArrayDescriptor();
            result.array = new long[newBitmap.Width, newBitmap.Height];
            result.width = newBitmap.Width;
            result.height = newBitmap.Height;

            BitmapData data = ImageProcessor.getBitmapData(newBitmap);

            for (int i = 0; i < newBitmap.Width; i++)
            {
                for (int j = 0; j < newBitmap.Height; j++)
                {
                    Color currentColor = ImageProcessor.getPixel(i, j, data);
                    result.array[i, j] = (currentColor.R + currentColor.G + currentColor.B) / 3;
                }
            }

            newBitmap.UnlockBits(data);

            return result;
        }
    }
}
