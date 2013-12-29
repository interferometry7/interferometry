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

public delegate void CosinusChoosed(double newCosinusValue);

namespace Interferometry.forms
{
    /// <summary>
    /// Interaction logic for PointsChooseForm.xaml
    /// </summary>
    
    
    public partial class PointsChooseForm
    {
        private double degrees = 30;
        public event CosinusChoosed cosinusChoosed;

        public PointsChooseForm()
        {
            InitializeComponent();
            degreesTextBox.Text = Convert.ToString(degrees);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (cosinusChoosed != null)
            {
                degrees = Convert.ToDouble(degreesTextBox.Text);
                cosinusChoosed(degrees);
            }

            Close();
        }
    }
}
