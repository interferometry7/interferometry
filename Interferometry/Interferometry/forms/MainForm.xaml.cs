using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
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
using Interferometry.interfaces;
using rab1;
using rab1.Forms;
using Image = System.Drawing.Image;

namespace Interferometry.forms
{
    /// <summary>
    /// Логика взаимодействия для MainForm.xaml
    /// </summary>
    public partial class MainForm : Window, ImageContainerDelegate
    {
        private List<ImageContainer> imageContainersList;

        //Life Cycle
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public MainForm()
        {
            InitializeComponent();

            imageContainersList = new List<ImageContainer>();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Grid newScrollerContent = new Grid();
            newScrollerContent.HorizontalAlignment = HorizontalAlignment.Stretch;
            newScrollerContent.VerticalAlignment = VerticalAlignment.Stretch;

            for (int i = 0; i < 11; i++)
            {
                ImageContainer newImageContainer = new ImageContainer();
                newImageContainer.myDelegate = this;
                newImageContainer.HorizontalAlignment = HorizontalAlignment.Stretch;
                newImageContainer.VerticalAlignment = VerticalAlignment.Stretch;
                newImageContainer.Width = Double.NaN;
                newImageContainer.Height = Double.NaN;

                RowDefinition newRowDefinition = new RowDefinition();
                newScrollerContent.RowDefinitions.Add(newRowDefinition);
                newScrollerContent.Children.Add(newImageContainer);
                Grid.SetRow(newImageContainer, i);
                imageContainersList.Add(newImageContainer);
            }

            imageContainersScroller.Content = newScrollerContent;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      
        
        
        //Методы из пункта "Работа с файлами"
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ImageSource newSource = FilesHelper.loadImege();

            if (newSource != null)
            {
                mainImage.Source = newSource;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Методы из пункта "Расшифровка"
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void unwrapClicked(object sender, RoutedEventArgs e)
        {
            if ((imageContainersList[8].getImage() == null) || (imageContainersList[9].getImage() == null))
            {
                MessageBox.Show("Изображения пустые");
                return;
            }

            Image[] imagesF = new Image[3];

            imagesF[0] = FilesHelper.bitmapImageToBitmap(imageContainersList[8].getImage());     // 1 фаза
            imagesF[1] = FilesHelper.bitmapImageToBitmap(imageContainersList[9].getImage());    // 2 фаза
            imagesF[2] = FilesHelper.bitmapImageToBitmap(imageContainersList[10].getImage());    // 3 ограничение по контуру

            UnwrapForm unwrapForm = new UnwrapForm(imagesF);
            unwrapForm.imageUnwrapped += unwrapFormOnImageUnwrapped;
            unwrapForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void unwrapFormOnImageUnwrapped(Pi_Class1.UnwrapResult unwrappedPhase)
        {
            Bitmap unwrappedPhaseImage = Pi_Class1.Z_bmp(unwrappedPhase.unwrappedPhase, unwrappedPhase.width, unwrappedPhase.height);
            imageContainersList[7].setImage(unwrappedPhaseImage);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      



        //ImageContainerDelegate Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void exportImage(ImageContainer imageContainer, ImageSource bitmapImage)
        {
            mainImage.Source = bitmapImage;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ImageSource getImageToLoad(ImageContainer imageContainer)
        {
            return mainImage.Source;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
