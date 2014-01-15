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
        private Pi_Class1.ZArrayDescriptor zArrayDescriptor;

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
        public void setzArrayDescriptor(Pi_Class1.ZArrayDescriptor newDescriptor)
        {
            zArrayDescriptor = newDescriptor;
            image.Source = Utils.getImageFromArray(zArrayDescriptor);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Pi_Class1.ZArrayDescriptor getzArrayDescriptor()
        {
            return zArrayDescriptor;
        }
        public Size GetElementPixelSize(UIElement element)
        {
            Matrix transformToDevice;
            var source = PresentationSource.FromVisual(element);
            if (source != null)
                transformToDevice = source.CompositionTarget.TransformToDevice;
            else
                using (var source2 = new HwndSource(new HwndSourceParameters()))
                    transformToDevice = source2.CompositionTarget.TransformToDevice;

            if (element.DesiredSize == new Size())
                element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            return (Size)transformToDevice.Transform((Vector)element.DesiredSize);
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
    }
}
