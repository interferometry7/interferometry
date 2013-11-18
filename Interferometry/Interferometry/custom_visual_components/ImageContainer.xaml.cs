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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Interferometry.interfaces;

namespace Interferometry
{
    /// <summary>
    /// Логика взаимодействия для ImageContainer.xaml
    /// </summary>
    public partial class ImageContainer : UserControl
    {
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



        //Private Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void onImageClick(object sender, MouseButtonEventArgs e)
        {
            image.Source = FilesHelper.loadImege();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void exportImageButton_Click(object sender, RoutedEventArgs e)
        {
            if ((image.Source != null) && (myDelegate != null))
            {
                myDelegate.exportImage(this, (BitmapImage) image.Source);
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
