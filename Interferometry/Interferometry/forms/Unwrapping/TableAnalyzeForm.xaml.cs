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

            List<ZArrayDescriptor> diagonals = new List<ZArrayDescriptor>();
            Point startPoint = new Point(0, 0);
            Bitmap resultBitmap = null;

            for (int i = 0; i < 10; i++)
            {
                ZArrayDescriptor nextDiag = getDiagonal(descriptorToAnalyze, startPoint);

                Bitmap nextBitmap = FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(nextDiag, Utils.RGBColor.Red));

                if (resultBitmap == null)
                {
                    resultBitmap = nextBitmap;
                }
                else
                {
                    resultBitmap = Utils.mergeBitmaps(resultBitmap, nextBitmap);
                }

                Point bottomPoint = new Point(0, 0);

                for (int x = 0; x < nextDiag.width; x++)
                {
                    for (int y = 0; y < nextDiag.height; y++)
                    {
                        if (nextDiag.array[x][y] == 0)
                        {
                            continue;
                        }

                        if (y > bottomPoint.Y)
                        {
                            bottomPoint = new Point(x, y);
                        }
                    }
                }

                int distanceToRightBorder = (int)(descriptorToAnalyze.width - bottomPoint.X);
                int distanceToBottomBorder = (int)(descriptorToAnalyze.height - bottomPoint.Y);

                if (distanceToRightBorder < distanceToBottomBorder)
                {
                    startPoint = new Point(0, bottomPoint.Y);
                }
                else
                {
                    startPoint = new Point(bottomPoint.X, 0);
                }
            }

            imageView.Source = FilesHelper.bitmapToBitmapImage(resultBitmap);

            /*ZArrayDescriptor firstDiag = getDiagonal(descriptorToAnalyze, new Point(0, 0));
            Bitmap firstDiagBitmap = FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(firstDiag, Utils.RGBColor.Red));

            Point bottomPoint = new Point(0, 0);

            for (int x = 0; x < firstDiag.width; x++)
            {
                for (int y = 0; y < firstDiag.height; y++)
                {
                    if (firstDiag.array[x][y] == 0)
                    {
                        continue;
                    }

                    if (y > bottomPoint.Y)
                    {
                        bottomPoint = new Point(x, y);
                    }
                }
            }

            Point secondPoint;
            int distanceToRightBorder = (int) (descriptorToAnalyze.width - bottomPoint.X);
            int distanceToBottomBorder = (int) (descriptorToAnalyze.height - bottomPoint.Y);

            if (distanceToRightBorder < distanceToBottomBorder)
            {
                secondPoint = new Point(0, bottomPoint.Y);
            }
            else
            {
                secondPoint = new Point(bottomPoint.X, 0);
            }

            ZArrayDescriptor secondDiag = getDiagonal(descriptorToAnalyze, secondPoint);
            Bitmap secondDiagBitmap = FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(secondDiag, Utils.RGBColor.Blue));

            Bitmap result = Utils.mergeBitmaps(firstDiagBitmap, secondDiagBitmap);

            imageView.Source = FilesHelper.bitmapToBitmapImage(result);*/
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private ZArrayDescriptor getDiagonal(ZArrayDescriptor someDescriptor, Point startPoint)
        {
            ZArrayDescriptor copyDescriptor = new ZArrayDescriptor(someDescriptor);

            ZArrayDescriptor result = new ZArrayDescriptor(copyDescriptor.width, copyDescriptor.height);
            startPoint = getNearestPointInDirection(startPoint, copyDescriptor, Direction.RightBottom);

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

            List<Point> arrayForRemove = new List<Point>();
            List<Point> arrayForAdd = new List<Point>();

            while (arrayForCheck.Count > 0)
            {
                foreach (Point currentPoint in arrayForCheck)
                {
                    int x = (int) currentPoint.X;
                    int y = (int) currentPoint.Y;

                    if (x + 1 < someArray.width)
                    {
                        arrayForAdd.Add(new Point(x + 1, y));
                    }

                    if (y + 1 < someArray.height)
                    {
                        arrayForAdd.Add(new Point(x, y + 1));
                    }

                    if ((x + 1 < someArray.width) && (y + 1 < someArray.height))
                    {
                        arrayForAdd.Add(new Point(x + 1, y + 1));
                    }

                    if (someArray.array[x][y] == 0)
                    {
                        arrayForRemove.Add(currentPoint);
                    }
                    else
                    {
                        return currentPoint;
                    }
                }

                arrayForCheck.AddRange(arrayForAdd);

                foreach (Point currentPoint in arrayForRemove)
                {
                    arrayForCheck.Remove(currentPoint);
                }
                
                arrayForAdd.Clear();
                arrayForRemove.Clear();
            }

            return startPoint;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
