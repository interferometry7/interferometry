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
using Interferometry.math_classes;
using rab1;
using rab1.Forms;
using Color = System.Drawing.Color;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Image = System.Windows.Controls.Image;
using ListBox = System.Windows.Forms.ListBox;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace Interferometry.forms
{
    /// <summary>
    /// Логика взаимодействия для MainForm.xaml
    /// </summary>
    public partial class MainForm : ImageContainerDelegate
    {
        private enum CursorMode
        {
            /// <summary>
            /// Режим по умолчанию. При клике по изображению ничего не происходит
            /// </summary>
            defaultMode,
            /// <summary>
            /// При клике по изображению строится график
            /// </summary>
            graphBuildMode,
            /// <summary>
            /// При клике по изображению строится таблица
            /// </summary>
            tableBuildMode
            ,
            /// <summary>
            /// При клике по изображению строится график по двум первым изображениям
            /// </summary>
            doubleGraph
        };

        private List<ImageContainer> imageContainersList;
        private ZArrayDescriptor zArrayDescriptor;

        private bool needPointsCapture = false;
        private Point3D firstClick;
        private Point3D secondClick;

        private CursorMode currentCursorMode = CursorMode.defaultMode;

        /// <summary>
        /// Значение косинуса для удаления плоскости по двум точкам
        /// </summary>
        private double cosinusValue = 0;

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

            for (int i = 0; i < 14; i++)
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
        private void loadImageClicked(object sender, RoutedEventArgs e)
        {
           ImageSource newSource = FilesHelper.loadImage();

            if (newSource != null)
            {
                zArrayDescriptor = Utils.getArrayFromImage((BitmapSource) newSource);
                mainImage.Source = Utils.getImageFromArray(zArrayDescriptor);

                adjustSliders();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Загрузить 8 изображений
        private void loadEightImages(object sender, RoutedEventArgs e)
        {
            ImageSource[] newSources = FilesHelper.loadEightImages();

            int all = 7;
            int done = 0;
            PopupProgressBar.show();

            if (newSources != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    imageContainersList[i].setzArrayDescriptorWithoutImageGenerating(Utils.getArrayFromImage((BitmapSource)newSources[i]));
                    imageContainersList[i].setImageWithoutArrayGenerating(newSources[i]);
                    done++;
                    PopupProgressBar.setProgress(done, all);
                }
            }
            PopupProgressBar.close();
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
        /// Сохранить массив ZArray
        private void saveArrayClicked(object sender, RoutedEventArgs e)
        {
            if (zArrayDescriptor != null)
            {
                FilesHelper.saveZArray(zArrayDescriptor);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadArrayClicked(object sender, RoutedEventArgs e)
        {
            setZArray(FilesHelper.loadZArray());
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void saveArraysClicked(object sender, RoutedEventArgs e)
        {
            ZArrayDescriptor[] arraysToSave = new ZArrayDescriptor[8];

            for(int i = 0; i < 8; i++)
            {
                ImageContainer currentContainer = imageContainersList[i];
                ZArrayDescriptor currentArray = currentContainer.getzArrayDescriptor();

                if (currentArray != null)
                {
                    arraysToSave[i] = currentArray;
                }
            }

            FilesHelper.saveZArrays(arraysToSave);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadArraysClicked(object sender, RoutedEventArgs e)
        {
            int all = 7;
            int done = 0;
            PopupProgressBar.show();

            ZArrayDescriptor[] loadedArrays = FilesHelper.loadArrays();

            if (loadedArrays != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    imageContainersList[i].setzArrayDescriptor(loadedArrays[i]);
                    done++;
                    PopupProgressBar.setProgress(done, all);
                }
            }
            PopupProgressBar.close();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void saveImagesClicked(object sender, RoutedEventArgs e)
        {
            ImageSource[] imagesToSave = new ImageSource[8];

            for (int i = 0; i < 8; i++)
            {
                ImageContainer currentContainer = imageContainersList[i];
                ImageSource currentArray = currentContainer.getImage();

                if (currentArray != null)
                {
                    imagesToSave[i] = currentArray;
                }
            }

            FilesHelper.saveImages(imagesToSave);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Методы для обработки слайдеров и кнопок управления построением изображения
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void minSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            minTextBox.Text = "" + Convert.ToInt32(minSlider.Value);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void redrawButton_Click(object sender, RoutedEventArgs e)
        {
            minSlider.Value = Convert.ToInt32(minTextBox.Text);
            maxSlider.Value = Convert.ToInt32(maxTextBox.Text);
            mainImage.Source = Utils.getImageFromArray(zArrayDescriptor, (long)minSlider.Value, (long)maxSlider.Value);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void changeArrayClicked(object sender, RoutedEventArgs e)
        {
            zArrayDescriptor = Utils.cutZArray(zArrayDescriptor, (int)minSlider.Value, (int)maxSlider.Value);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void maxSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            maxTextBox.Text = "" + Convert.ToInt32(maxSlider.Value);

            if (minSlider.Value > maxSlider.Value)
            {
                minSlider.Value = maxSlider.Value;
            }

            minSlider.Maximum = maxSlider.Value;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void maxTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                redrawButton_Click(null, null);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void minTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                redrawButton_Click(null, null);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        


        //Методы из пункта "Восстановление фазы"
        //Удаление фазовой неоднозначности
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void unwrapClicked(object sender, RoutedEventArgs e)
        {
           
            if ((imageContainersList[8].getzArrayDescriptor() == null)) { MessageBox.Show("Изображениe 9 пусто"); return; }
            if ((imageContainersList[9].getzArrayDescriptor() == null)) { MessageBox.Show("Изображениe 10 пусто"); return; }
           // if ((imageContainersList[10].getzArrayDescriptor() == null)) { MessageBox.Show("Изображениe 11 пусто"); return; }

            ZArrayDescriptor[] imagesF = new ZArrayDescriptor[3];

            imagesF[0] = imageContainersList[8].getzArrayDescriptor();
            imagesF[1] = imageContainersList[9].getzArrayDescriptor();
           // imagesF[2] = imageContainersList[10].getzArrayDescriptor();  

            UnwrapForm unwrapForm = new UnwrapForm(imagesF);
            unwrapForm.imageUnwrapped += unwrapFormOnImageUnwrapped;
            unwrapForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void unwrapFormOnImageUnwrapped(ZArrayDescriptor unwrappedPhase)
        {
            imageContainersList[10].setzArrayDescriptor(unwrappedPhase);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                    Вычитание опорной плоскости
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void choosePointsClicked(object sender, RoutedEventArgs e)
        {
            currentCursorMode = CursorMode.defaultMode;

            if (mainImage.Source == null)  { MessageBox.Show("Главное изображение пустое");  return;  }

            if (zArrayDescriptor == null)  { MessageBox.Show("Z-массив пуст");               return;  }

            needPointsCapture = true;
            firstClick = null;
            secondClick = null;

            PointsChooseForm pointsChooseForm = new PointsChooseForm();
            pointsChooseForm.cosinusChoosed+= PointsChooseFormOnCosinusChoosed;
            pointsChooseForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void PointsChooseFormOnCosinusChoosed(double newCosinusValue)
        {
            cosinusValue = newCosinusValue;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                    Вычитание двух массивов
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ZArrayDescriptor[] source = new ZArrayDescriptor[8];
            for (int i = 0; i < 8; i++) source[i] = imageContainersList[i].getzArrayDescriptor();
            Tabl_Sub Tabl_Sub = new Tabl_Sub(source);
            Tabl_Sub.Show();
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                    ATAN2
        private void createWrappedPhase(object sender, RoutedEventArgs e)
        {
            
            ZArrayDescriptor[] source = new ZArrayDescriptor[8];
            for (int i = 0; i < 8; i++) source[i] = imageContainersList[i].getzArrayDescriptor();

            TableFaza TableFaza = new TableFaza(source);
            TableFaza.atan_Unwrapped += AtanFormOnImage;
            TableFaza.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void AtanFormOnImage( TableFaza.Res d)
        {
            //Pi_Class1.ZArrayDescriptor unwrappedPhaseImage = Pi_Class1.getUnwrappedPhaseImage(unwrappedPhase.array, unwrappedPhase.width, unwrappedPhase.height);
            ZArrayDescriptor unwrappedPhaseImage1 = d.result1; 
            imageContainersList[8].setzArrayDescriptor(unwrappedPhaseImage1);
            ZArrayDescriptor unwrappedPhaseImage2 = d.result2;
            imageContainersList[9].setzArrayDescriptor(unwrappedPhaseImage2);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //              Построить таблицу остатков
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void buildTableClicked(object sender, RoutedEventArgs e)
        {
            ZArrayDescriptor[] imagesForTable = new ZArrayDescriptor[3];

            if ((imageContainersList[8].getzArrayDescriptor() == null) ) { MessageBox.Show("Изображениe 9 пусто");  return; }
            if ((imageContainersList[9].getzArrayDescriptor() == null))  { MessageBox.Show("Изображениe 10 пусто"); return; }
            if ((imageContainersList[10].getzArrayDescriptor() == null)) { MessageBox.Show("Изображениe 11 пусто"); return; }
            
            imagesForTable[0] = imageContainersList[8].getzArrayDescriptor();
            imagesForTable[1] = imageContainersList[9].getzArrayDescriptor();
            imagesForTable[2] = imageContainersList[10].getzArrayDescriptor();


            TableGenerateForm tableGenerateForm = new TableGenerateForm(imagesForTable);
            tableGenerateForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Методы из пункта "Информация"
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void defaultCursorModeButton_Checked(object sender, RoutedEventArgs e)
        {
            currentCursorMode = CursorMode.defaultMode;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void graphCursorModeButton_Checked(object sender, RoutedEventArgs e)
        {
            currentCursorMode = CursorMode.graphBuildMode;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            currentCursorMode = CursorMode.tableBuildMode;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            currentCursorMode = CursorMode.doubleGraph;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Методы раздела Камера
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void makePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            ImageGetter.sharedInstance().imageReceived += OnImageReceived;
            ImageGetter.sharedInstance().getImage();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void OnImageReceived(System.Drawing.Image newImage)
        {
            ImageGetter.sharedInstance().imageReceived -= OnImageReceived;
            imageContainersList[0].setzArrayDescriptor(Utils.getArrayFromImage((Bitmap) newImage));
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void makePhotoSeriesButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundImagesGeneratorForm newForm = new BackgroundImagesGeneratorForm();
            newForm.oneImageOfSeries += oneImageOfSeriesTaken;
            newForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void oneImageOfSeriesTaken(System.Drawing.Image newImage, int imageNumber)
        {
            ZArrayDescriptor result = Utils.getArrayFromImage((Bitmap) newImage);
            imageContainersList[imageNumber - 1].setzArrayDescriptor(result);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Private Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void adjustSliders()
        {
            long max = Utils.getMax(zArrayDescriptor);
            maxTextBox.Text = "" + max;
            maxSlider.Minimum = 0;
            maxSlider.Maximum = max;
            maxSlider.Value = max;

            long min = Utils.getMin(zArrayDescriptor);
            minTextBox.Text = "" + min;
            minSlider.Minimum = min;
            minSlider.Maximum = max;
            minSlider.Value = min;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void setZArray(ZArrayDescriptor arrayDescriptor)
        {
            zArrayDescriptor = new ZArrayDescriptor(arrayDescriptor);
            mainImage.Source = Utils.getImageFromArray(zArrayDescriptor);

            adjustSliders();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        


        //ImageContainerDelegate Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void exportImage(ImageContainer imageContainer, ZArrayDescriptor arrayDescriptor)
        {
            setZArray(arrayDescriptor);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ZArrayDescriptor getImageToLoad(ImageContainer imageContainer)
        {
            return zArrayDescriptor;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        
        
        //Mouse Events
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void mainImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (currentCursorMode == CursorMode.defaultMode)
            {
                if (needPointsCapture == false)
                {
                    return;
                }

                if (firstClick == null)
                {
                    int x = (int) e.GetPosition(mainImage).X;
                    int y = (int) e.GetPosition(mainImage).Y;
                    int z = (int) zArrayDescriptor.array[x, y];

                    firstClick = new Point3D(x, y, z);
                    return;
                }
                else
                {
                    int x = (int) e.GetPosition(mainImage).X;
                    int y = (int) e.GetPosition(mainImage).Y;
                    int z = (int) zArrayDescriptor.array[x, y];

                    secondClick = new Point3D(x, y, z);
                }

                ZArrayDescriptor result = Pi_Class1.Z_sub(firstClick.x, firstClick.y, secondClick.x,
                    secondClick.y, zArrayDescriptor, cosinusValue);

                zArrayDescriptor = result;
                mainImage.Source = Utils.getImageFromArray(zArrayDescriptor);

                adjustSliders();

                needPointsCapture = false;
            }
            else if (currentCursorMode == CursorMode.graphBuildMode)
            {
                int x = (int)e.GetPosition(mainImage).X;
                int y = (int)e.GetPosition(mainImage).Y;
                Graphic graphic = new Graphic(zArrayDescriptor, x, y);  // График новый
               
                graphic.Show();   
            }
            else if (currentCursorMode == CursorMode.tableBuildMode)
            {
                if (imageContainersList[8].getzArrayDescriptor() == null)
                {
                    MessageBox.Show("9 изображение пустое!");
                    return;
                }

                if (imageContainersList[9].getzArrayDescriptor() == null)
                {
                    MessageBox.Show("10 изображение пустое!");
                    return;
                }

                if (imageContainersList[10].getzArrayDescriptor() == null)
                {
                    MessageBox.Show("11 изображение пустое!");
                    return;
                }

                ZArrayDescriptor[] imagesForTable = new ZArrayDescriptor[3];

                imagesForTable[0] = imageContainersList[8].getzArrayDescriptor();
                imagesForTable[1] = imageContainersList[9].getzArrayDescriptor();
                imagesForTable[2] = imageContainersList[10].getzArrayDescriptor();


                TableGenerateForm tableGenerateForm = new TableGenerateForm(imagesForTable);
                tableGenerateForm.setX((int)e.GetPosition(mainImage).X);
                tableGenerateForm.setY((int)e.GetPosition(mainImage).Y);
                tableGenerateForm.Show();
            }
            else if (currentCursorMode == CursorMode.doubleGraph)
            {
                if (imageContainersList[0].getzArrayDescriptor() == null)
                {
                    MessageBox.Show("1-e изображение пустое!");
                    return;
                }

                int x = (int)e.GetPosition(mainImage).X;
                int y = (int)e.GetPosition(mainImage).Y;
                Graphic graphic = new Graphic(zArrayDescriptor, imageContainersList[0].getzArrayDescriptor(), x, y);

                graphic.Show();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void mainImage_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (zArrayDescriptor != null)
                {
                    int redComponent = (int) zArrayDescriptor.array[(int)e.GetPosition(mainImage).X, (int)e.GetPosition(mainImage).Y];
                    int greenComponent = (int)zArrayDescriptor.array[(int)e.GetPosition(mainImage).X, (int)e.GetPosition(mainImage).Y];
                    int blueComponent = (int)zArrayDescriptor.array[(int)e.GetPosition(mainImage).X, (int)e.GetPosition(mainImage).Y];

                    redComponentLabel.Content = Convert.ToString(redComponent);
                    greenComponentLabel.Content = Convert.ToString(greenComponent);
                    blueComponentLabel.Content = Convert.ToString(blueComponent);

                    int xPosition = (int) e.GetPosition(mainImage).X;
                    int yPositon = (int) e.GetPosition(mainImage).Y;

                    xLabel.Content = Convert.ToString(xPosition);
                    yLabel.Content = Convert.ToString(yPositon);
                }
            }catch (Exception ex){}
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
