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

public delegate void ImageProcessedWithNewMethod(ZArrayDescriptor firstPartOfResult, ZArrayDescriptor secondPartOfResult);

namespace Interferometry.forms
{
    /// <summary>
    /// Interaction logic for NewMethodForm.xaml
    /// </summary>
    public partial class NewMethodForm : Window
    {
        private ImageCacheManager firstCacheManager;
        private ImageCacheManager secondCacheManager;
        private int firstSineNumber = 167;
        private int secondSineNumber = 241;

        public event ImageProcessedWithNewMethod imageProcessedWithNewMethod;



        private List<String> firstBunch;
        private List<String> secondBunch;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public NewMethodForm()
        {
            InitializeComponent();
            firstCacheManager = new ImageCacheManager();
            secondCacheManager = new ImageCacheManager();

            firstSineNumberTextBox.Text = Convert.ToString(firstSineNumber);
            secondSineNumberTextBox.Text = Convert.ToString(secondSineNumber);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void setFileNames(List<String> files, int imageWidth, int imageHeight)
        {
            /*List<String> firstBunch = new List<string>(files.Count / 2);

            for (int i = 0; i < files.Count/2; i++)
            {
                firstBunch.Add(files[i]);
            }

            firstCacheManager.setFilePathes(firstBunch, imageWidth, imageHeight);
            List<String> secondBunch = new List<string>(files.Count / 2);
            

            for (int i = files.Count / 2; i < files.Count; i++)
            {
                secondBunch.Add(files[i]);
            }

            secondCacheManager.setFilePathes(secondBunch, imageWidth, imageHeight);*/

            firstBunch = new List<string>(files.Count / 2);

            for (int i = 0; i < files.Count / 2; i++)
            {
                firstBunch.Add(files[i]);
            }

            
            secondBunch = new List<string>(files.Count / 2);


            for (int i = files.Count / 2; i < files.Count; i++)
            {
                secondBunch.Add(files[i]);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void setDescriptors(List<ZArrayDescriptor> someDescriptors)
        {
            //cacheManager.setZArrayDescriptors(someDescriptors);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double[] fz = new double[firstBunch.Count];
            double step = 360.0 / firstBunch.Count;

            for (int i = 0; i < fz.Count(); i++)
            {
                fz[i] = step * i;
            }

            WrappedPhaseGetter wrappedPhaseGetter = new WrappedPhaseGetter(firstBunch, fz);
            wrappedPhaseGetter.RunWorkerAsync();

            /*if (imageProcessedWithNewMethod == null)
            {
                return;
            }

            ZArrayDescriptor firstResult = null;

            double[] fz = new double[firstCacheManager.getImageNumber()];
            int step = 360 / firstCacheManager.getImageNumber();

            for (int i = 0; i < firstCacheManager.getImageNumber(); i++)
            {
                fz[i] = step*i;
            }

            for (int i = 0; i < firstCacheManager.getWidth(); i++)
            {
                for (int j = 0; j < firstCacheManager.getHeight(); j++)
                {
                    ZArrayDescriptor[] descriptors = firstCacheManager.getArray(i, j);

                    if (firstResult == null)
                    {
                        firstResult = new ZArrayDescriptor(FazaClass.ATAN_1234(descriptors, fz, firstSineNumber));
                    }
                    else
                    {
                        firstResult.add(FazaClass.ATAN_1234(descriptors, fz, firstSineNumber));
                    }

                }
            }



            ZArrayDescriptor secondResult = null;

            for (int i = 0; i < secondCacheManager.getWidth(); i++)
            {
                for (int j = 0; j < secondCacheManager.getHeight(); j++)
                {
                    ZArrayDescriptor[] descriptors = secondCacheManager.getArray(i, j);

                    if (secondResult == null)
                    {
                        secondResult = new ZArrayDescriptor(FazaClass.ATAN_1234(descriptors, fz, firstSineNumber));
                    }
                    else
                    {
                        secondResult.add(FazaClass.ATAN_1234(descriptors, fz, firstSineNumber));
                    }

                }
            }

            imageProcessedWithNewMethod(firstResult, secondResult);*/
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
