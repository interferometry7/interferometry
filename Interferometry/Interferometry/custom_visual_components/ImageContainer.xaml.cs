using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Interferometry.interfaces;
using Interferometry.math_classes;
using rab1;
using rab1.Forms;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
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
        private BackgroundWorker worker;
        private int imageWidth;
        private int imageHeight;

        public ImageContainerDelegate myDelegate;

        //Interface Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ImageContainer()
        {
            InitializeComponent();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ~ImageContainer()
        {
            File.Delete(getFilePath());
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

            if (zArrayDescriptor != null)
            {
                imageWidth = zArrayDescriptor.width;
                imageHeight = zArrayDescriptor.height;
            }
            else
            {
                imageWidth = 0;
                imageHeight = 0;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void setImage(BitmapImage bitmapImage)
        {
            if (bitmapImage != null)
            {
                progressBar.Visibility = Visibility.Visible;

                if (worker != null)
                {
                    worker.CancelAsync();
                }

                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += loadImageAsync;
                worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
                worker.ProgressChanged += WorkerOnProgressChanged;
                worker.RunWorkerAsync(bitmapImage);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void setBitmap(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                progressBar.Visibility = Visibility.Visible;

                if (worker != null)
                {
                    worker.CancelAsync();
                }

                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += loadBitmapAsync;
                worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
                worker.ProgressChanged += WorkerOnProgressChanged;
                worker.RunWorkerAsync(bitmap);
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
            Bitmap resizedImage;

            if ((imageContainer.ActualWidth != 0) && (imageContainer.ActualHeight != 0))
            {
                Size imageContainerSize = new Size(imageContainer.ActualWidth, imageContainer.ActualHeight);
                resizedImage = ResizeImage(FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(zArrayDescriptor)), imageContainerSize);
            }
            else
            {
                resizedImage = FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(zArrayDescriptor));
            }


            image.Source = FilesHelper.bitmapToBitmapImage(resizedImage);

            if (zArrayDescriptor != null)
            {
                imageWidth = zArrayDescriptor.width;
                imageHeight = zArrayDescriptor.height;

                if (Environment.Is64BitProcess == false)
                {
                    FilesHelper.saveDescriptorWithName(zArrayDescriptor, getFilePath());
                    zArrayDescriptor = null;
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ZArrayDescriptor getzArrayDescriptor()
        {
            if (zArrayDescriptor == null)
            {
                zArrayDescriptor = FilesHelper.readDescriptorWithName(getFilePath());
            }

            return zArrayDescriptor;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ImageSource getImage()
        {
            return image.Source;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void filterImage(FiltrationForm.FiltrationType filtrationType, int order)
        {
            if (zArrayDescriptor == null)
            {
                return;
            }

            ZArrayDescriptor result = null;

            if (filtrationType == FiltrationForm.FiltrationType.Smoothing)
            {
                result = FiltrClass.Filt_121(zArrayDescriptor, order);
            }
            else if (filtrationType == FiltrationForm.FiltrationType.Median)
            {
                result = FiltrClass.Filt_Mediana(zArrayDescriptor, order);
            }

            setzArrayDescriptor(result);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public String getFilePath()
        {
            return imageNumber + "_image";
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int getImageWidth()
        {
            return imageWidth;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int getImageHeight()
        {
            return imageHeight;
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
                progressBar.IsIndeterminate = true;
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
            if ((imageWidth == 0) && (imageHeight == 0))
            {
                return;
            }

            popupText.Text = "" + imageWidth + "x" + imageHeight;

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
        private static Bitmap ResizeImage(Bitmap imgToResize, Size size)
        {
            try
            {
                Bitmap b = new Bitmap((int)size.Width, (int)size.Height);
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(imgToResize, 0, 0, (float)size.Width, (float)size.Height);
                }
                return b;
            }
            catch
            {
                return null;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadBitmapAsync(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            Bitmap newBitmap = (Bitmap)doWorkEventArgs.Argument;

            if (newBitmap == null)
            {
                return;
            }

            zArrayDescriptor = Utils.getArrayFromImage(newBitmap);

            Bitmap resizedImage;

            if ((imageContainer.ActualWidth != 0) && (imageContainer.ActualHeight != 0))
            {
                Size imageContainerSize = new Size(imageContainer.ActualWidth, imageContainer.ActualHeight);
                resizedImage = ResizeImage(FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(zArrayDescriptor)), imageContainerSize);
            }
            else
            {
                resizedImage = FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(zArrayDescriptor));
            }

            if (zArrayDescriptor != null)
            {
                imageWidth = zArrayDescriptor.width;
                imageHeight = zArrayDescriptor.height;

                if (Environment.Is64BitProcess == false)
                {
                    FilesHelper.saveDescriptorWithName(zArrayDescriptor, getFilePath());
                    zArrayDescriptor = null;
                }
            }

            doWorkEventArgs.Result = FilesHelper.bitmapToBitmapImage(resizedImage);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadImageAsync(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            BitmapImage newSource = (BitmapImage)doWorkEventArgs.Argument;

            if (newSource == null)
            {
                return;
            }

            zArrayDescriptor = Utils.getArrayFromImage(newSource);

            Bitmap resizedImage;

            if ((imageContainer.ActualWidth != 0) && (imageContainer.ActualHeight != 0))
            {
                Size imageContainerSize = new Size(imageContainer.ActualWidth, imageContainer.ActualHeight);
                resizedImage = ResizeImage(FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(zArrayDescriptor)), imageContainerSize);
            }
            else
            {
                resizedImage = FilesHelper.bitmapSourceToBitmap(Utils.getImageFromArray(zArrayDescriptor));
            }

            if (zArrayDescriptor != null)
            {
                imageWidth = zArrayDescriptor.width;
                imageHeight = zArrayDescriptor.height;

                if (Environment.Is64BitProcess == false)
                {
                    FilesHelper.saveDescriptorWithName(zArrayDescriptor, getFilePath());
                    zArrayDescriptor = null;
                }
            }

            doWorkEventArgs.Result = FilesHelper.bitmapToBitmapImage(resizedImage);
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
