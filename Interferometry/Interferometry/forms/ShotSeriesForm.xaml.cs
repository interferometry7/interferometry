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
using rab1;
using Image = System.Drawing.Image;

public delegate void OneShotOfSeries(Image newImage, int imageNumber);

namespace Interferometry.forms
{
    /// <summary>
    /// Interaction logic for ShotSeriesForm.xaml
    /// </summary>
    public partial class ShotSeriesForm : Window
    {
        private int phaseShift1Value;
        private int phaseShift2Value;
        private int phaseShift3Value;
        private int phaseShift4Value;
        private int phaseShift5Value;

        private int imageWidth = 800;
        private int imageHeight = 600;

        private int shotNumbers;
        private int imageNumber;

        private BackkgroundStripesForm formForStripes;

        public event OneShotOfSeries oneShotOfSeries;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ShotSeriesForm()
        {
            InitializeComponent();

            formForStripes = new BackkgroundStripesForm();
            updateInitialImage();
            formForStripes.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void updateInitialImage()
        {
            Bitmap result = SinClass1.drawSine(167/10, 0, imageWidth, imageHeight, 0);
            formForStripes.setImage(result);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            shotNumbers = Convert.ToInt32(shotNumbersLabel.Text);

            imageNumber = 0;
            updateInitialImage();

            ImageGetter.sharedInstance().imageReceived += imageTaken;
            ImageGetter.sharedInstance().getImage();
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

            Bitmap result = SinClass1.drawSine(167/10, (360 / shotNumbers) * imageNumber, imageWidth, imageHeight, 0);
            formForStripes.setImage(result);
            ImageGetter.sharedInstance().getImage();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
