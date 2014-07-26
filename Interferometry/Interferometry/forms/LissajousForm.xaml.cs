using System;
using System.Collections.Generic;
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

namespace Interferometry.forms
{
    /// <summary>
    /// Interaction logic for LissajousForm.xaml
    /// </summary>
    public partial class LissajousForm : Window
    {
        public LissajousForm()
        {
            InitializeComponent();
        }

        public void setImages(ZArrayDescriptor[] someImages)
        {
            double[] phaseShifts = new double[someImages.Length];
            double step = 360.0 / someImages.Length;

            for (int i = 0; i < phaseShifts.Count(); i++)
            {
                phaseShifts[i] = step * i;
            }

            mainImage.Source = FilesHelper.bitmapToBitmapImage(FazaClass.Graph_ATAN(someImages, phaseShifts));
        }
    }
}
