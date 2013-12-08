using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Interferometry.interfaces;
using rab1;
using rab1.Forms;
using Color = System.Drawing.Color;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Image = System.Windows.Controls.Image;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace Interferometry.forms
{
    /// <summary>
    /// Логика взаимодействия для MainForm.xaml
    /// </summary>
    public partial class MainForm : ImageContainerDelegate
    {
        private List<ImageContainer> imageContainersList;
        private Pi_Class1.ZArrayDescriptor zArrayDescriptor;

        private bool needPointsCapture = false;
        private Point3D firstClick;
        private Point3D secondClick;

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
                newImageContainer.setImageNumberLabel(i + 1);

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
        private void saveButtonClicked(object sender, RoutedEventArgs e)
        {
            if (mainImage.Source != null)
            {
                FilesHelper.saveImage(mainImage.Source);
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

            Bitmap[] imagesF = new Bitmap[3];

            imagesF[0] = FilesHelper.bitmapImageToBitmap(imageContainersList[8].getImage());     // 1 фаза
            imagesF[1] = FilesHelper.bitmapImageToBitmap(imageContainersList[9].getImage());    // 2 фаза
            imagesF[2] = FilesHelper.bitmapImageToBitmap(imageContainersList[10].getImage());    // 3 ограничение по контуру

            UnwrapForm unwrapForm = new UnwrapForm(imagesF);
            unwrapForm.imageUnwrapped += unwrapFormOnImageUnwrapped;
            unwrapForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void unwrapFormOnImageUnwrapped(Pi_Class1.ZArrayDescriptor unwrappedPhase)
        {
            Bitmap unwrappedPhaseImage = Pi_Class1.getUnwrappedPhaseImage(unwrappedPhase.unwrappedPhase, unwrappedPhase.width, unwrappedPhase.height);
            imageContainersList[7].setImage(unwrappedPhaseImage);
            imageContainersList[7].zArrayDescriptor = unwrappedPhase;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void choosePointsClicked(object sender, RoutedEventArgs e)
        {
            if (mainImage.Source == null)
            {
                MessageBox.Show("Главное изображение пустое");
                return;
            }

            if (zArrayDescriptor == null)
            {
                MessageBox.Show("Z-массив пуст");
                return;
            }

            needPointsCapture = true;
            firstClick = null;
            secondClick = null;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      

        
        //ImageContainerDelegate Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void exportImage(ImageContainer imageContainer, ImageSource bitmapImage)
        {
            mainImage.Source = bitmapImage;
            zArrayDescriptor = imageContainer.zArrayDescriptor;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ImageSource getImageToLoad(ImageContainer imageContainer)
        {
            imageContainer.zArrayDescriptor = zArrayDescriptor;
            return mainImage.Source;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Mouse Events
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void mainImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (needPointsCapture == false)
            {
                return;
            }

            if (firstClick == null)
            {
                int x = (int)e.GetPosition(mainImage).X;
                int y = (int)e.GetPosition(mainImage).Y;
                int z = (int)zArrayDescriptor.unwrappedPhase[x, y];

                firstClick = new Point3D(x, y, z);
                return;
            }
            else
            {
                int x = (int)e.GetPosition(mainImage).X;
                int y = (int)e.GetPosition(mainImage).Y;
                int z = (int)zArrayDescriptor.unwrappedPhase[x, y];

                secondClick = new Point3D(x, y, z);
            }

            MessageBox.Show("Первая точка - X = " + firstClick.z + " Y = " + firstClick.y + " Z = " + firstClick.z
                + " Вторая точка -" + secondClick.x + " Y = " + secondClick.y + " Z = " + secondClick.z);

            needPointsCapture = false;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void mainImage_MouseMove(object sender, MouseEventArgs e)
        {
            /*Image image = (Image)sender;

            if (image.Source != null)
            {
                Bitmap currentBitmap = FilesHelper.bitmapImageToBitmap((BitmapImage) image.Source);

                Color currentColor;

                try
                {
                    currentColor = currentBitmap.GetPixel((int)e.GetPosition(mainImage).X, (int)e.GetPosition(mainImage).Y);
                }
                catch (Exception)
                {
                    return;
                }


                int redComponent = currentColor.R;
                int greenComponent = currentColor.G;
                int blueComponent = currentColor.B;

                redComponentLabel.Content = Convert.ToString(redComponent);
                greenComponentLabel.Content = Convert.ToString(greenComponent);
                blueComponentLabel.Content = Convert.ToString(blueComponent);

                int xPosition = (int)e.GetPosition(mainImage).X;
                int yPositon = (int)e.GetPosition(mainImage).Y;

                xLabel.Content = Convert.ToString(xPosition);
                yLabel.Content = Convert.ToString(yPositon);
            }*/
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
         
        

        //Additional Classes
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public class Point3D
        {
            public int x;
            public int y;
            public int z;
            
            public Point3D(int newX, int newY, int newZ)
            {
                x = newX;
                y = newY;
                z = newZ;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
