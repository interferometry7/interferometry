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
        private enum Direction
        {
            RightBottom
        }

        private const int MAX_DISTANCE = 10;
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
            if (descriptorToAnalyze == null)
            {
                MessageBox.Show("Дескриптор не передан");
                return;
            }

            ZArrayDescriptor firstDiag = getDiagonal(descriptorToAnalyze);

            //Bitmap firstBitmap = FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(descriptorToAnalyze));
            Bitmap secondBitmap = FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(firstDiag, Utils.RGBColor.Red));
            //Bitmap result = Utils.mergeBitmaps(firstBitmap, secondBitmap);

            imageView.Source = FilesHelper.bitmapToBitmapImage(secondBitmap);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private ZArrayDescriptor getDiagonal(ZArrayDescriptor someDescriptor)
        {
            ZArrayDescriptor copyDescriptor = new ZArrayDescriptor(someDescriptor);

            ZArrayDescriptor result = new ZArrayDescriptor(copyDescriptor.width, copyDescriptor.height);
            Point startPoint = new Point(-1, -1);

            for (int x = 0; x < copyDescriptor.width; x++)
            {
                for (int y = 0; y < copyDescriptor.height; y++)
                {
                    if (copyDescriptor.array[x][y] != 0)
                    {
                        startPoint = new Point(x, y);
                        goto ExitOfLoops;
                    }
                }
            }

            ExitOfLoops:

            if ((startPoint.X == -1) && (startPoint.Y == -1))
            {
                return null;
            }

            int maxNumberOfPoints = copyDescriptor.width * copyDescriptor.height;

            for (int i = 0; i < maxNumberOfPoints; i++)
            {
                Point nearestPoint = getNearestPoint(copyDescriptor, startPoint.X, startPoint.Y);
                double currentDistance = Math.Sqrt(Math.Pow((startPoint.X - nearestPoint.X), 2) + Math.Pow((startPoint.Y - nearestPoint.Y), 2));

                if (currentDistance > MAX_DISTANCE)
                {
                    break;
                }

                copyDescriptor.array[(int)nearestPoint.X][(int)nearestPoint.Y] = 0;
                copyDescriptor.array[(int)nearestPoint.X][(int)nearestPoint.Y] = 0;
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
        private Point getNearestPointInDirection(Point startPoint, ZArrayDescriptor someArray, Direction direction)
        {
            List<Point> arrayForCheck = new List<Point>();
            arrayForCheck.Add(startPoint);

            foreach (Point currentPoint in arrayForCheck)
            {
                int x = (int) currentPoint.X;
                int y = (int) currentPoint.Y;

                Console.WriteLine("x = " + x + "y = " + y);

                if (x + 1 < someArray.width)
                {
                    arrayForCheck.Add(new Point(x + 1, y));
                }

                if (y + 1 < someArray.height)
                {
                    arrayForCheck.Add(new Point(x, y + 1));
                }

                if ((x + 1 < someArray.width) && (y + 1 < someArray.height))
                {
                    arrayForCheck.Add(new Point(x + 1, y + 1));
                }

                if (someArray.array[x][y] == 0)
                {
                    arrayForCheck.Remove(currentPoint);
                }
                else
                {
                    return currentPoint;
                }
            }

            return startPoint;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
