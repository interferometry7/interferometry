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

namespace Interferometry.forms
{
    /// <summary>
    /// Interaction logic for CompareForm.xaml
    /// </summary>
    public partial class CompareForm : Window
    {
        private ZArrayDescriptor firstDescriptor;
        private ZArrayDescriptor secondDescriptor;

        public CompareForm()
        {
            InitializeComponent();
        }

        private void loadFirstArray(object sender, RoutedEventArgs e)
        {
            firstDescriptor = FilesHelper.loadZArray();
            ImageSource newImage = Utils.getImageFromArray(firstDescriptor);
            imageToComparation.Source = newImage;
        }

        private void loadSecondArray(object sender, RoutedEventArgs e)
        {
            if (firstDescriptor == null)
            {
                return;
            }

            secondDescriptor = FilesHelper.loadZArray();

            Bitmap firstBitmap = FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(firstDescriptor));
            Bitmap secondBitmap = FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(secondDescriptor, Utils.RGBColor.Red));
            Bitmap result = Utils.mergeBitmaps(firstBitmap, secondBitmap);

            imageToComparation.Source = FilesHelper.bitmapToBitmapImage(result);
        }
    }
}
