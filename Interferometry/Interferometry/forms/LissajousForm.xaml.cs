using System;
using System.Collections.Generic;
using System.ComponentModel;
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
public delegate void LissajousImageBuilded(ZArrayDescriptor firstPartOfResult, ZArrayDescriptor secondPartOfResult);

namespace Interferometry.forms
{
    /// <summary>
    /// Interaction logic for LissajousForm.xaml
    /// </summary>
    public partial class LissajousForm : Window
    {
        public event LissajousImageBuilded lissajousImageBuilded;

        private List<String> firstBunch;
        private List<String> secondBunch;

        private int imagesWidth;
        private int imagesHeight;

        private ZArrayDescriptor firstResult;
        private ZArrayDescriptor secondResult;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public LissajousForm(List<String> files, int imageWidth, int imageHeight)
        {
            InitializeComponent();

            imagesWidth = imageWidth;
            imagesHeight = imageHeight;

            firstBunch = new List<string>(files.Count / 2);

            for (int i = 0; i < files.Count / 2; i++)
            {
                firstBunch.Add(files[i]);
            }

            secondBunch = new List<string>(files.Count / 2);

            for (int i = files.Count / 2; i < files.Count; i++)
            {
                secondBunch.Add(files[i]);
            }

            LissajousImageBuilder lissajousImageBuilder = new LissajousImageBuilder(firstBunch, imagesWidth, imagesHeight);
            lissajousImageBuilder.RunWorkerCompleted += LissajousImageBuilderOnRunWorkerCompleted;
            lissajousImageBuilder.RunWorkerAsync();

            LissajousImageBuilder secondBuilder = new LissajousImageBuilder(secondBunch, imagesWidth, imagesHeight);
            secondBuilder.RunWorkerCompleted += SecondBuilderOnRunWorkerCompleted;
            secondBuilder.RunWorkerAsync();
        }
        
        private void LissajousImageBuilderOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            firstResult = (ZArrayDescriptor)runWorkerCompletedEventArgs.Result;
            checkResult();
        }

        private void SecondBuilderOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            secondResult = (ZArrayDescriptor)runWorkerCompletedEventArgs.Result;
            checkResult();
        }

        private void checkResult()
        {
            if ((firstResult != null) && (secondResult != null))
            {
                lissajousImageBuilded(firstResult, secondResult);
                Close();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*public void setImages(ZArrayDescriptor[] someImages)
        {
            double[] phaseShifts = new double[someImages.Length];
            double step = 360.0 / someImages.Length;

            for (int i = 0; i < phaseShifts.Count(); i++)
            {
                phaseShifts[i] = step * i;
            }

            mainImage.Source = FilesHelper.bitmapToBitmapImage(FazaClass.Graph_ATAN(someImages, phaseShifts));
        }*/
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
