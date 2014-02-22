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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Interferometry.interfaces;
using Interferometry.math_classes;
using rab1;
using Color = System.Drawing.Color;
using Size = System.Windows.Size;

namespace Interferometry
{
    /// <summary>
    /// Логика взаимодействия для ImageContainer.xaml
    /// </summary>
    public partial class ImageContainer : UserControl
    {
        private int imageNumber;
        private ZArrayDescriptor zArrayDescriptor;

        public ImageContainerDelegate myDelegate;

        //Interface Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ImageContainer()
        {
            InitializeComponent();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void setImageNumberLabel(int newImageNumber)
        {
            imageNumber = newImageNumber;
            imageNumberLabel.Content = imageNumber;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void setzArrayDescriptorWithoutImageGenerating(ZArrayDescriptor newDescriptor)
        {
            zArrayDescriptor = newDescriptor;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void setImageWithoutArrayGenerating(ImageSource imageSource)
        {
            image.Source = imageSource;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void setzArrayDescriptor(ZArrayDescriptor newDescriptor)
        {
            zArrayDescriptor = newDescriptor;
            image.Source = Utils.getImageFromArray(zArrayDescriptor);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ZArrayDescriptor getzArrayDescriptor()
        {
            return zArrayDescriptor;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ImageSource getImage()
        {
            return image.Source;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Private Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void onImageClick(object sender, MouseButtonEventArgs e)
        {
            BitmapImage newSource = FilesHelper.loadImage();

            if (newSource != null)
            {
                setImage(FilesHelper.bitmapImageToBitmap(newSource));
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void setImage(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return;
            }

            zArrayDescriptor = Utils.getArrayFromImage(bitmap);
            setzArrayDescriptor(zArrayDescriptor);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void exportImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (myDelegate != null)
            {
                myDelegate.exportImage(this, getzArrayDescriptor());
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadImageClicked(object sender, RoutedEventArgs e)
        {
            if (myDelegate != null)
            {
                setzArrayDescriptor(myDelegate.getImageToLoad(this));
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void grid1_MouseEnter(object sender, MouseEventArgs e)
        {
            if (zArrayDescriptor == null)
            {
                return;
            }

            popupText.Text = "" + zArrayDescriptor.width + "x" + zArrayDescriptor.height;

            if (popup.IsOpen == false)
            {
                popup.IsOpen = true;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void grid1_MouseLeave(object sender, MouseEventArgs e)
        {
            if (popup.IsOpen == true)
            {
                popup.IsOpen = false;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
