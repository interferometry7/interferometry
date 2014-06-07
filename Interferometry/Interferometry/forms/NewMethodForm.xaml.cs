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
using Interferometry.imageCacher;
using Interferometry.math_classes;
using rab1;

namespace Interferometry.forms
{
    /// <summary>
    /// Interaction logic for NewMethodForm.xaml
    /// </summary>
    public partial class NewMethodForm : Window
    {
        private ImageCacheManager cacheManager;
        private int firstSineNumber = 167;
        private int secondSineNumber = 241;

        public NewMethodForm()
        {
            InitializeComponent();
            cacheManager = new ImageCacheManager();

            firstSineNumberTextBox.Text = Convert.ToString(firstSineNumber);
            secondSineNumberTextBox.Text = Convert.ToString(secondSineNumber);
        }

        public void setFileNames(List<String> files, int imageWidth, int imageHeight)
        {
            cacheManager.setFilePathes(files, imageWidth, imageHeight);
        }

        public void setDescriptors(List<ZArrayDescriptor> someDescriptors)
        {
            cacheManager.setZArrayDescriptors(someDescriptors);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double[] fz = new double[cacheManager.getImageNumber()];
            int step = 360/cacheManager.getImageNumber();

            for (int i = 0; i < cacheManager.getImageNumber(); i++)
            {
                fz[i] = step*i;
            }

            for (int i = 0; i < cacheManager.getWidth(); i++)
            {
                for (int j = 0; j < cacheManager.getHeight(); j++)
                {
                    ZArrayDescriptor[] descriptors = cacheManager.getArray(i, j);
                    ZArrayDescriptor firstResult = FazaClass.ATAN_1234(descriptors, fz, firstSineNumber);

                    int a = (int) firstResult.array[0, 0];
                }
            }
        }
    }
}
