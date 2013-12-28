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

public delegate void CosinusChoosed(int newCosinusValue);

namespace Interferometry.forms
{
    /// <summary>
    /// Interaction logic for PointsChooseForm.xaml
    /// </summary>
    public partial class PointsChooseForm
    {
        public event CosinusChoosed cosinusChoosed;

        public PointsChooseForm()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (cosinusChoosed != null)
            {
                int degrees = Convert.ToInt32(degreesTextBox.Text);
                cosinusChoosed(degrees);
            }

            Close();
        }
    }
}
