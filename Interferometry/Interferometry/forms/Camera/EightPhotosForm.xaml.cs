using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Interferometry.math_classes;
using Image = System.Drawing.Image;

namespace Interferometry.forms.Camera
{
    /// <summary>
    /// Interaction logic for EightPhotosForm.xaml
    /// </summary>
    public partial class EightPhotosForm : Window
    {
        private const int imageWidth = 800;
        private const int imageHeight = 600;

        private double firstSinNumber = 167;
        private double secondSinNumber = 241;

        private int shotNumbers = 8;
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
            firstSinNumber = Convert.ToDouble(firstSineNumberTextBox.Text);
            secondSinNumber = Convert.ToDouble(secondSineNumberTextBox.Text);

            Bitmap result = SinClass1.drawSine(firstSinNumber / 10.0, 0, imageWidth, imageHeight, 0);
            formForStripes.setImage(result);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void startShooting(object sender, RoutedEventArgs e)
        {
            shotNumbers = Convert.ToInt32(shotsNumberTextBox.Text) * 2;

            imageNumber = 0;
            updateInitialImage();

            Console.WriteLine("shift = " + 0);

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
            
            Bitmap result;
            double shift;

            if (imageNumber < shotNumbers/2)
            {
                shift = (360.0 / (shotNumbers / 2)) * (imageNumber);
                result = SinClass1.drawSine(firstSinNumber / 10.0, shift, imageWidth, imageHeight, 0);                
            }
            else
            {
                shift = (360.0 / (shotNumbers / 2)) * (imageNumber - shotNumbers / 2);
                result = SinClass1.drawSine(secondSinNumber / 10.0, shift, imageWidth, imageHeight, 0);
            }

            Console.WriteLine("shift = " + shift);


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
