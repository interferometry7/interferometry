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
using Interferometry.imageCacher;
using Interferometry.math_classes;
using rab1;

public delegate void ImageProcessedWithNewMethod(ZArrayDescriptor firstPartOfResult, ZArrayDescriptor secondPartOfResult);

namespace Interferometry.forms
{
    /// <summary>
    /// Interaction logic for NewMethodForm.xaml
    /// </summary>
    public partial class NewMethodForm : Window
    {
        private const int firstSineNumber = 167;
        private const int secondSineNumber = 241;

        public event ImageProcessedWithNewMethod imageProcessedWithNewMethod;

        private List<String> firstBunch;
        private List<String> secondBunch;

        private int imagesWidth;
        private int imagesHeight;

        private ZArrayDescriptor firstResult;
        private ZArrayDescriptor secondResult;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public NewMethodForm()
        {
            InitializeComponent();

            firstSineNumberTextBox.Text = Convert.ToString(firstSineNumber);
            secondSineNumberTextBox.Text = Convert.ToString(secondSineNumber);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void setFileNames(List<String> files, int imageWidth, int imageHeight)
        {
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
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WrappedPhaseGetter wrappedPhaseGetter = new WrappedPhaseGetter(firstBunch, imagesWidth, imagesHeight);
            wrappedPhaseGetter.RunWorkerCompleted+=WrappedPhaseGetterOnRunWorkerCompleted;
            wrappedPhaseGetter.RunWorkerAsync();

            WrappedPhaseGetter secondWrappedPhaseGetter = new WrappedPhaseGetter(secondBunch, imagesWidth, imagesHeight);
            secondWrappedPhaseGetter.RunWorkerCompleted += SecondWrappedPhaseGetterOnRunWorkerCompleted;
            secondWrappedPhaseGetter.RunWorkerAsync();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SecondWrappedPhaseGetterOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            secondResult = (ZArrayDescriptor)runWorkerCompletedEventArgs.Result;
            checkResult();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void WrappedPhaseGetterOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            firstResult = (ZArrayDescriptor) runWorkerCompletedEventArgs.Result;
            checkResult();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void checkResult()
        {
            if ((firstResult != null) && (secondResult != null))
            {
                imageProcessedWithNewMethod(firstResult, secondResult);
                Close();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
