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
        private static Random random = new Random((new DateTime()).Millisecond);

        const double FIRST_PHASE = 167.0;
        const double SECOND_PHASE = 241.0;
        const int NUMBER_OF_IMAGES_IN_SERIES = 4;

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

            List<ZArrayDescriptor> sinesArray = new List<ZArrayDescriptor>();

            for (int imageNumber = 0; imageNumber < NUMBER_OF_IMAGES_IN_SERIES; imageNumber++)
            {
                ZArrayDescriptor newSineImage = createSineImage(imageNumber, FIRST_PHASE, NUMBER_OF_IMAGES_IN_SERIES, amplitudePercent, phasePercent);
                sinesArray.Add(newSineImage);
            }

            for (int imageNumber = 0; imageNumber < NUMBER_OF_IMAGES_IN_SERIES; imageNumber++)
            {
                ZArrayDescriptor newSineImage = createSineImage(imageNumber, SECOND_PHASE, NUMBER_OF_IMAGES_IN_SERIES, amplitudePercent, phasePercent);
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
            double phaseDistortionPercent)
        {
            double phaseShiftInDegrees = (360.0 / totalImages) * imageNumber;
            double phaseShiftInRadians = (phaseShiftInDegrees * Math.PI) / 180.0;

            ZArrayDescriptor newSineImage = new ZArrayDescriptor(1024, 1024);

            for (int i = 0; i < newSineImage.width; i++)
            {
                double x = i / (phase / 10.0);

                const float multiplier = 100.0f;
                const float INITIAL_VALUE = 1.0f;
                int percent = random.Next((int)(-(amplitudeDistortionPercent * multiplier) / 2.0f), (int)((amplitudeDistortionPercent * multiplier) / 2.0f));
                float randomValue = percent * INITIAL_VALUE / multiplier / 100.0f;

                double resultValue = Math.Sin(x + phaseShiftInRadians + 2 * Math.PI * phaseDistortionPercent / 100.0);
                resultValue = (resultValue + 1.0) / 2.0 + randomValue + ((amplitudeDistortionPercent) / 2.0f / 100.0f) * INITIAL_VALUE;

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
