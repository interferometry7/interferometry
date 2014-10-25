using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Interferometry.math_classes;
using rab1;
using Point = System.Windows.Point;

namespace Interferometry.forms.Unwrapping
{
    /// <summary>
    /// Interaction logic for TableAnalyzeForm.xaml
    /// </summary>
    public partial class TableAnalyzeForm : Window
    {
        private ZArrayDescriptor descriptorToAnalyze;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public TableAnalyzeForm(ZArrayDescriptor zArrayDescriptor)
        {
            InitializeComponent();
            descriptorToAnalyze = zArrayDescriptor;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void analyzeClicked(object sender, RoutedEventArgs e)
        {
            ZArrayDescriptor firstDiag = getDiagonal(descriptorToAnalyze);

            //Bitmap firstBitmap = FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(descriptorToAnalyze));
            Bitmap secondBitmap = FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(firstDiag, Utils.RGBColor.Red));
            //Bitmap result = Utils.mergeBitmaps(firstBitmap, secondBitmap);

            imageView.Source = FilesHelper.bitmapToBitmapImage(secondBitmap);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private ZArrayDescriptor getDiagonal(ZArrayDescriptor someDescriptor)
        {
            ZArrayDescriptor result = new ZArrayDescriptor(someDescriptor.width, someDescriptor.height);
            Point startPoint = new Point(-1, -1);

            for (int x = 0; x < someDescriptor.width; x++)
            {
                for (int y = 0; y < someDescriptor.height; y++)
                {
                    if (someDescriptor.array[x][y] != 0)
                    {
                        startPoint = new Point(x, y);
                    }
                }
            }

            if (startPoint.X == -1)
            {
                return null;
            }

            int maxNumberOfPoints = someDescriptor.width*someDescriptor.height;

            for (int i = 0; i < maxNumberOfPoints; i++)
            {
                Point nearestPoint = getNearestPoint(someDescriptor, startPoint.X, startPoint.Y);

                double currentDistance = Math.Sqrt(Math.Pow((startPoint.X - nearestPoint.X), 2) + Math.Pow((startPoint.Y - nearestPoint.Y), 2));

                if (currentDistance > 10)
                {
                    break;
                }

                someDescriptor.array[(int)nearestPoint.X][(int)nearestPoint.Y] = 0;
                someDescriptor.array[(int)nearestPoint.X][(int)nearestPoint.Y] = 0;
                result.array[(int) nearestPoint.X][(int) nearestPoint.Y] = 1;
                result.array[(int) startPoint.X][(int) startPoint.Y] = 1;
                startPoint = nearestPoint;
            }

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private Point getNearestPoint(ZArrayDescriptor someArray, double a, double b)
        {
            Point result = new Point();
            double distance = Double.MaxValue;

            for (int x = 0; x < someArray.width; x++)
            {
                for (int y = 0; y < someArray.height; y++)
                {
                    long currentValue = someArray.array[x][y];

                    if (currentValue == 0)
                    {
                        continue;
                    }

                    double currentDistance = Math.Sqrt(Math.Pow((a - x), 2) + Math.Pow((b - y), 2));

                    if (currentDistance < distance)
                    {
                        distance = currentDistance;
                        result.X = x;
                        result.Y = y;
                    }
                }
            }

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
