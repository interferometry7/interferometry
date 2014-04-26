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
        private double phase1 = 0;
        private double phase2 = 90;
        private double phase3 = 180;
        private double phase4 = 270;

        public LissajousForm()
        {
            InitializeComponent();
        }

        public void setImages(ZArrayDescriptor[] someImages)
        {
            double[] phases = new double[4];
            phases[0] = phase1;
            phases[1] = phase2;
            phases[2] = phase3; 
            phases[3] = phase4;
            mainImage.Source = FilesHelper.bitmapToBitmapImage(FazaClass.Graph_ATAN(someImages, phases));
        }
    }
}
