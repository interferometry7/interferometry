using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using Interferometry.forms;
using Interferometry.math_classes;

namespace rab1
{
    class ImageHelper
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void drawGraph(ZArrayDescriptor someImage, int pointX, int pointY)
        {
            drawGraph(someImage, pointX, pointY, 1);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
        public static void drawGraph(ZArrayDescriptor someImage, int pointX, int pointY, double ratio)
        {
            int w1 = someImage.width;
            int h1 = someImage.height;
            int[] bufr = new int[w1];
            int[] bufyr = new int[h1];
            int[] bufg = new int[w1];
            int[] bufyg = new int[h1];
            int[] bufb = new int[w1];
            int[] bufyb = new int[h1];

            if (pointX >= w1 || pointY >= h1)
            {
                return;
            }

            for (int i = 0; i < w1; i++)
            {
                bufr[i] = (int) (someImage.array[i][pointY] * ratio);
                bufg[i] = (int)(someImage.array[i][pointY] * ratio);
                bufb[i] = (int)(someImage.array[i][pointY] * ratio);
            }

            for (int i = 0; i < h1; i++)
            {
                bufyr[i] = (int)(someImage.array[pointX][i] * ratio);
                bufyg[i] = (int)(someImage.array[pointX][i] * ratio);
                bufyb[i] = (int)(someImage.array[pointX][i] * ratio);
            }

            Form fo = new GraphForm(pointX, pointY, w1, h1, bufr, bufyr, bufg, bufyg, bufb, bufyb);
            fo.Show();
            fo.StartPosition = FormStartPosition.Manual;
            fo.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Rectangle determineImageRect(Bitmap someImage)
        {
            BitmapData bitmapData = ImageProcessor.getBitmapData(someImage);

            bool flag = false;
            Color pixelColor;
            Color blackColor = Color.Black;

            int top = 0;
            int left = 0;
            int right = 0;
            int bottom = 0;
            
            for (int i = 0; i < someImage.Height; i++)
            {
                for (int j = 0; j < someImage.Width; j++)
                {
                    pixelColor = ImageProcessor.getPixel(j, i, bitmapData);
                    if (pixelColor.ToArgb() != blackColor.ToArgb())
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag == true)
                {
                    top = i;
                    break;
                }
            }

            flag = false;
            for (int i = 0; i < someImage.Width; i++)
            {
                for (int j = 0; j < someImage.Height; j++)
                {
                    pixelColor = ImageProcessor.getPixel(i, j, bitmapData);
                    if (pixelColor.ToArgb() != blackColor.ToArgb())
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag == true)
                {
                    left = i;
                    break;
                }
            }

            flag = false;
            for (int i = someImage.Width - 1; i >= 0; i--)
            {
                for (int j = 0; j < someImage.Height; j++)
                {
                    pixelColor = ImageProcessor.getPixel(i, j, bitmapData);
                    if (pixelColor.ToArgb() != blackColor.ToArgb())
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag == true)
                {
                    right = i;
                    break;
                }
            }

            flag = false;
            for (int i = someImage.Height - 1; i >= 0; i--)
            {
                for (int j = 0; j < someImage.Width; j++)
                {
                    pixelColor = ImageProcessor.getPixel(j, i, bitmapData);
                    if (pixelColor.ToArgb() != blackColor.ToArgb())
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag == true)
                {
                    bottom = i;
                    break;
                }
            }


            someImage.UnlockBits(bitmapData);

            Rectangle result = new Rectangle(left, top, right - left + 1, bottom - top + 1);
            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
