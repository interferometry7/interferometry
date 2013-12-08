using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Interferometry.interfaces;
using rab1;
using Color = System.Drawing.Color;

namespace Interferometry
{
    /// <summary>
    /// Логика взаимодействия для ImageContainer.xaml
    /// </summary>
    public partial class ImageContainer : UserControl
    {
        private int imageNumber;
        private Pi_Class1.ZArrayDescriptor zArrayDescriptor;

        public ImageContainerDelegate myDelegate;

        //Interface Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ImageContainer()
        {
            InitializeComponent();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public BitmapImage getImage()
        {
            return (BitmapImage) image.Source;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void setImage(Bitmap bitmap)
        {
            image.Source = FilesHelper.bitmapToBitmapImage(bitmap);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void setImageNumberLabel(int newImageNumber)
        {
            imageNumber = newImageNumber;
            imageNumberLabel.Content = imageNumber;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void setzArrayDescriptor(Pi_Class1.ZArrayDescriptor newDescriptor)
        {
            zArrayDescriptor = newDescriptor;

            if (zArrayDescriptor == null)
            {
                return;
            }

            double max = 0;
            double min = Double.MaxValue;

            for (int i = 0; i < zArrayDescriptor.width; i++)
            {
                for (int j = 0; j < zArrayDescriptor.height; j++)
                {
                    max = Math.Max(max, zArrayDescriptor.array[i, j]);
                    min = Math.Min(min, zArrayDescriptor.array[i, j]);
                }
            }

            double multiplier = 255/(max - min);

            Bitmap newBitmap = new Bitmap(zArrayDescriptor.width, zArrayDescriptor.height);
            BitmapData data = ImageProcessor.getBitmapData(newBitmap);

            for (int i = 0; i < zArrayDescriptor.width; i++)
            {
                for (int j = 0; j < zArrayDescriptor.height; j++)
                {
                    int colorComponent = (int) (zArrayDescriptor.array[i, j]*multiplier);
                    ImageProcessor.setPixel(data, i, j, Color.FromArgb(colorComponent, colorComponent, colorComponent));
                }
            }

            newBitmap.UnlockBits(data);
            setImage(newBitmap);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Pi_Class1.ZArrayDescriptor getzArrayDescriptor()
        {
            return zArrayDescriptor;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Private Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void onImageClick(object sender, MouseButtonEventArgs e)
        {
            ImageSource newSource = FilesHelper.loadImege();

            if (newSource != null)
            {
                image.Source = newSource;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void exportImageButton_Click(object sender, RoutedEventArgs e)
        {
            if ((image.Source != null) && (myDelegate != null))
            {
                myDelegate.exportImage(this, image.Source);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadImageClicked(object sender, RoutedEventArgs e)
        {
            if (myDelegate != null)
            {
                image.Source = myDelegate.getImageToLoad(this);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
