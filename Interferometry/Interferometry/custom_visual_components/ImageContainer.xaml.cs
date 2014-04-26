using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public void setImage(BitmapImage bitmapImage)
        {
            if (bitmapImage != null)
            {
                progressBar.Visibility = Visibility.Visible;
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += loadImageAsync;
                worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
                worker.ProgressChanged += WorkerOnProgressChanged;
                worker.RunWorkerAsync(bitmapImage);
            }
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



        //GUI Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void onImageClick(object sender, MouseButtonEventArgs e)
        {
            BitmapImage newSource = FilesHelper.loadImage();

            if (newSource != null)
            {
                progressBar.Visibility = Visibility.Visible;
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork+=loadImageAsync;
                worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
                worker.RunWorkerAsync(newSource);
            }
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



        //Private Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadImageAsync(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            BitmapImage newSource = (BitmapImage)doWorkEventArgs.Argument;

            if (newSource == null)
            {
                return;
            }

            zArrayDescriptor = Utils.getArrayFromImage(newSource);
            ImageSource imageSource = Utils.getImageFromArray(zArrayDescriptor);
            doWorkEventArgs.Result = imageSource;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void WorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            ImageSource result = (ImageSource)runWorkerCompletedEventArgs.Result;
            image.Source = result;
            progressBar.Visibility = Visibility.Hidden;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void WorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
