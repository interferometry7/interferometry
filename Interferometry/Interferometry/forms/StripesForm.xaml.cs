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
using System.Windows.Threading;

namespace Interferometry.forms
{
    /// <summary>
    /// Interaction logic for StripesForm.xaml
    /// </summary>
    public partial class StripesForm : Window
    {
        public StripesForm()
        {
            InitializeComponent();
        }

        public void setImage(Bitmap someImage)
        {
            stripesImage.Source = FilesHelper.bitmapToBitmapImage(someImage);
        }
    }
}
