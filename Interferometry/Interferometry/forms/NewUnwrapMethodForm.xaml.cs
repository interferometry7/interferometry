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

public delegate void ImagesUnwrappedWithNewMethod(ZArrayDescriptor result);

namespace Interferometry.forms
{
    /// <summary>
    /// Логика взаимодействия для NewUnwrapMethodForm.xaml
    /// </summary>
    public partial class NewUnwrapMethodForm : Window
    {
        private List<ZArrayDescriptor> someImages;
        private List<int> sineNumbers = new List<int>();

        public event ImagesUnwrappedWithNewMethod imagesUnwrappedWithNewMethod;

        public NewUnwrapMethodForm(List<ZArrayDescriptor> someImages)
        {
            InitializeComponent();

            this.someImages = someImages;

            sineNumbers.Add(167);
            sineNumbers.Add(241);
        }

        private void startClicked(object sender, RoutedEventArgs e)
        {
            NewMethodUnwrapper lissajousImageBuilder = new NewMethodUnwrapper(someImages, sineNumbers);
            lissajousImageBuilder.RunWorkerCompleted += imagesUnwrapped;
            lissajousImageBuilder.RunWorkerAsync();
        }

        private void imagesUnwrapped(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            if (imagesUnwrappedWithNewMethod != null)
            {
                ZArrayDescriptor result = (ZArrayDescriptor) runWorkerCompletedEventArgs.Result;
                imagesUnwrappedWithNewMethod(result);
            }
        }
    }
}
