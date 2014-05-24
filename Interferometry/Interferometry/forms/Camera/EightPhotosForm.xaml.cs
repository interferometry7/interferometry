using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using rab1;
using Image = System.Drawing.Image;

namespace Interferometry.forms
{
    /// <summary>
    /// Interaction logic for EightPhotosForm.xaml
    /// </summary>
    public partial class EightPhotosForm : Window
    {
        private int imageWidth = 800;
        private int imageHeight = 600;

        private double shotNumbers = 8;
        private int imageNumber;

        private BackkgroundStripesForm formForStripes;

        public event OneShotOfSeries oneShotOfSeries;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public EightPhotosForm()
        {
            InitializeComponent();

            formForStripes = new BackkgroundStripesForm();
            updateInitialImage();
            formForStripes.Show();

            shotsNumberTextBox.Text = Convert.ToString(shotNumbers);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void updateInitialImage()
        {
            Bitmap result = SinClass1.drawSine(167 / 10, 0, imageWidth, imageHeight, 0);
            formForStripes.setImage(result);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void startShooting(object sender, RoutedEventArgs e)
        {
            shotNumbers = Convert.ToInt32(shotsNumberTextBox.Text);

            imageNumber = 0;
            updateInitialImage();

            ImageGetter.sharedInstance().imageReceived += imageTaken;
            ImageGetter.sharedInstance().getImage();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Window_Closed(object sender, EventArgs e)
        {
            formForStripes.Close();
            ImageGetter.sharedInstance().imageReceived -= imageTaken;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void imageTaken(Image newImage)
        {
            imageNumber++;

            if (oneShotOfSeries != null)
            {
                oneShotOfSeries(newImage, imageNumber);
            }

            if (imageNumber >= shotNumbers)
            {
                ImageGetter.sharedInstance().imageReceived -= imageTaken;
                return;
            }

            double shift = (360.0 / shotNumbers) * imageNumber;

            Bitmap result = SinClass1.drawSine(167 / 10, shift, imageWidth, imageHeight, 0);
            formForStripes.setImage(result);
            ImageGetter.sharedInstance().getImage();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void shotsNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]");
            return !regex.IsMatch(text);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
