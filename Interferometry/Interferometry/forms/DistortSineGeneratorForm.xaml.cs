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

public delegate void DistortedImageCreated(List<ZArrayDescriptor> sinesArray);

namespace Interferometry.forms
{
    /// <summary>
    /// Interaction logic for DistortSineGeneratorForm.xaml
    /// </summary>
    public partial class DistortSineGeneratorForm : Window
    {
        private static readonly Random random = new Random((new DateTime()).Millisecond);

        private const double FIRST_PHASE = 167.0;
        private const double SECOND_PHASE = 241.0;
        private const int NUMBER_OF_IMAGES_IN_SERIES = 4;
        private const int SINE_IMAGE_WIDTH = 300;
        private const int SINE_IMAGE_HEIGHT = 300;

        public DistortedImageCreated distortedImageCreated;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public DistortSineGeneratorForm()
        {
            InitializeComponent();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void generateImages(object sender, RoutedEventArgs e)
        {
            double amplitudePercent = Convert.ToDouble(amplitudePercentField.Text);
            double phasePercent = Convert.ToDouble(phasePercentField.Text);
            double gammaCorrection = Convert.ToDouble(gammaCorrectionField.Text);

            List<ZArrayDescriptor> sinesArray = new List<ZArrayDescriptor>();

            for (int imageNumber = 0; imageNumber < NUMBER_OF_IMAGES_IN_SERIES; imageNumber++)
            {
                ZArrayDescriptor newSineImage = createSineImage(imageNumber, FIRST_PHASE, NUMBER_OF_IMAGES_IN_SERIES, amplitudePercent, phasePercent, gammaCorrection);
                sinesArray.Add(newSineImage);
            }

            for (int imageNumber = 0; imageNumber < NUMBER_OF_IMAGES_IN_SERIES; imageNumber++)
            {
                ZArrayDescriptor newSineImage = createSineImage(imageNumber, SECOND_PHASE, NUMBER_OF_IMAGES_IN_SERIES, amplitudePercent, phasePercent, gammaCorrection);
                sinesArray.Add(newSineImage);
            }

            if (distortedImageCreated != null)
            {
                distortedImageCreated(sinesArray);
            }

            Close();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static ZArrayDescriptor createSineImage(int imageNumber, 
            double phase, 
            int totalImages,
            double amplitudeDistortionPercent,
            double phaseDistortionPercent,
            double gammaCorrection)
        {
            double phaseShiftInDegrees = (360.0 / totalImages) * imageNumber;
            double phaseShiftInRadians = Utils.degreeToRadian(phaseShiftInDegrees);

            ZArrayDescriptor newSineImage = new ZArrayDescriptor(SINE_IMAGE_WIDTH, SINE_IMAGE_HEIGHT);

            for (int i = 0; i < newSineImage.width; i++)
            {
                double x = i / (phase / 10.0);

                const double multiplier = 100.0;

                const double AMPLITUDE_INITIAL_VALUE = 1.0;
                int amplitudeRandomPercent = random.Next((int)(-(amplitudeDistortionPercent * multiplier) / 2.0f), (int)((amplitudeDistortionPercent * multiplier) / 2.0f));
                double amplitudeRandomValue = amplitudeRandomPercent * AMPLITUDE_INITIAL_VALUE / multiplier / 100.0f;

                const double PHASE_INITIAL_VALUE = 360.0;
                int phaseRandomPercent = random.Next((int)(-(phaseDistortionPercent * multiplier) / 2.0), (int)((phaseDistortionPercent * multiplier) / 2.0));
                double phaseRandomValueInDegrees = phaseRandomPercent * PHASE_INITIAL_VALUE / multiplier / 100.0;
                double phaseRandomValueInRadians = Utils.degreeToRadian(phaseRandomValueInDegrees);

                double resultValue = Math.Sin(x + phaseShiftInRadians + phaseRandomValueInRadians);
                resultValue = (resultValue + 1.0) / 2.0 + amplitudeRandomValue + ((amplitudeDistortionPercent) / 2.0 / 100.0) * AMPLITUDE_INITIAL_VALUE;

                resultValue = Math.Pow(resultValue, gammaCorrection);
                resultValue *= 255.0;

                for (int j = 0; j < newSineImage.height; j++)
                {
                    newSineImage.array[i][j] = (long)resultValue;
                }
            }
            return newSineImage;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
