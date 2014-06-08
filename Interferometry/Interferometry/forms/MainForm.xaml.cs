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
using Interferometry.Forms;
using Interferometry.interfaces;
using Interferometry.math_classes;
using rab1;
using rab1.Forms;
using Application = System.Windows.Application;
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
            tableBuildMode,
            /// <summary>
            /// При клике по изображению строится график по двум первым изображениям
            /// </summary>
            doubleGraph
        };

        private List<ImageContainer> imageContainersList;
        private Grid scrollerContent;
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
            scrollerContent = new Grid();
            scrollerContent.HorizontalAlignment = HorizontalAlignment.Stretch;
            scrollerContent.VerticalAlignment = VerticalAlignment.Stretch;

            for (int i = 0; i < 14; i++)
            {
                addImageContainer(i, scrollerContent);
            }

            imageContainersScroller.Content = scrollerContent;
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
            loadBunchOfImages(8);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Загрузить 10 изображений
        private void loadTenImagesButton_Click(object sender, RoutedEventArgs e)
        {
            loadBunchOfImages(10);
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
                    //imageContainersList[i].setzArrayDescriptor(loadedArrays[i]);
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
            minTextBox.Text = "" + Convert.ToInt64(minSlider.Value);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void redrawButton_Click(object sender, RoutedEventArgs e)
        {
            minSlider.Value = Convert.ToInt64(minTextBox.Text);
            maxSlider.Value = Convert.ToInt64(maxTextBox.Text);
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
            maxTextBox.Text = "" + Convert.ToInt64(maxSlider.Value);

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
        //         Методы из пункта "Восстановление фазы"
        //           Удаление фазовой неоднозначности
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void unwrapClicked(object sender, RoutedEventArgs e)
        {
            if ((imageContainersList[11].getzArrayDescriptor() == null)) { MessageBox.Show("Изображениe 12 пусто"); return; }
            if ((imageContainersList[12].getzArrayDescriptor() == null)) { MessageBox.Show("Изображениe 13 пусто"); return; }

            ZArrayDescriptor[] imagesF = new ZArrayDescriptor[3];

            imagesF[0] = imageContainersList[11].getzArrayDescriptor();
            imagesF[1] = imageContainersList[12].getzArrayDescriptor(); 

            UnwrapForm unwrapForm = new UnwrapForm(imagesF);
            unwrapForm.imageUnwrapped += unwrapFormOnImageUnwrapped;
            unwrapForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void unwrapFormOnImageUnwrapped(ZArrayDescriptor unwrappedPhase)
        {
            imageContainersList[13].setzArrayDescriptor(unwrappedPhase);
        }
        ///////////////////////////////////////////////////////// 
        //           Восстановление полной фазы 
        //           Полная фаза в 14 кадре
        //           1. в 12 кадре -> 10
        //           2. в 11 кадре -> 11
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void unwrap2pi(object sender, RoutedEventArgs e)
        {
            ZArrayDescriptor[] source = new ZArrayDescriptor[3];
            source[0] = imageContainersList[11].getzArrayDescriptor();  // 12 кадр ATAN2
            source[1] = imageContainersList[12].getzArrayDescriptor();  // 13 кадр ATAN2
            source[2] = imageContainersList[13].getzArrayDescriptor();  // 14 кадр 2Pi

            Faza2Pi Faza2Pi = new Faza2Pi(source);
            Faza2Pi.Pi2_Unwrapped += PiFormOnImage;
            Faza2Pi.Show();

        }

        private void PiFormOnImage(Faza2Pi.Res1 d)
        {
            //Pi_Class1.ZArrayDescriptor unwrappedPhaseImage = Pi_Class1.getUnwrappedPhaseImage(unwrappedPhase.array, unwrappedPhase.width, unwrappedPhase.height);
            ZArrayDescriptor unwrappedPhaseImage1 = d.result1;
            imageContainersList[9].setzArrayDescriptor(unwrappedPhaseImage1);
            ZArrayDescriptor unwrappedPhaseImage2 = d.result2;
            imageContainersList[10].setzArrayDescriptor(unwrappedPhaseImage2);
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

            for (int i = 0; i < 8; i++)
            {
                source[i] = imageContainersList[i].getzArrayDescriptor();
            }

            Tabl_Sub Tabl_Sub = new Tabl_Sub(source);
            Tabl_Sub.arraySubbed = ArraySubbed;
            Tabl_Sub.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ArraySubbed(ZArrayDescriptor result, int imageNumber)
        {
            imageContainersList[imageNumber].setzArrayDescriptor(result);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                    ATAN2 (получение фазы по массиву интенсивностей)
        private void createWrappedPhase(object sender, RoutedEventArgs e)
        {
            ZArrayDescriptor[] source = new ZArrayDescriptor[10];

            for (int i = 0; i < 10; i++)
            {
                source[i] = imageContainersList[i].getzArrayDescriptor();
            }

            TableFaza TableFaza = new TableFaza(source);
            TableFaza.atan_Unwrapped += AtanFormOnImage;
            TableFaza.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Environment.Is64BitProcess == false)
            {
                List<String> fileNames = new List<String>();

                for (int i = 0; i < 16; i++)
                {
                    fileNames.Add(imageContainersList[i].getFilePath());
                }


                NewMethodForm newMethodForm = new NewMethodForm();
                newMethodForm.setFileNames(fileNames, imageContainersList[0].getImageWidth(), imageContainersList[0].getImageHeight());
                newMethodForm.imageProcessedWithNewMethod+= NewMethodFormOnImageProcessedWithNewMethod;
                newMethodForm.Show();
            }
            else
            {
                ZArrayDescriptor[] source = new ZArrayDescriptor[10];

                for (int i = 0; i < 10; i++)
                {
                    source[i] = imageContainersList[i].getzArrayDescriptor();
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void NewMethodFormOnImageProcessedWithNewMethod(ZArrayDescriptor firstPartOfResult, ZArrayDescriptor secondPartOfResult)
        {
            addImageContainer(imageContainersList.Count, scrollerContent);
            imageContainersList[imageContainersList.Count - 1].setzArrayDescriptor(firstPartOfResult);

            addImageContainer(imageContainersList.Count, scrollerContent);
            imageContainersList[imageContainersList.Count - 1].setzArrayDescriptor(secondPartOfResult);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void AtanFormOnImage( TableFaza.Res d)
        {
            ZArrayDescriptor unwrappedPhaseImage1 = d.result1; 
            imageContainersList[11].setzArrayDescriptor(unwrappedPhaseImage1);
            ZArrayDescriptor unwrappedPhaseImage2 = d.result2;
            imageContainersList[12].setzArrayDescriptor(unwrappedPhaseImage2);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //              Построить таблицу остатков
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void buildTableClicked(object sender, RoutedEventArgs e)
        {
            ZArrayDescriptor[] imagesForTable = new ZArrayDescriptor[3];

            if ((imageContainersList[11].getzArrayDescriptor() == null) ) { MessageBox.Show("Изображениe 12 пусто");  return; }
            if ((imageContainersList[12].getzArrayDescriptor() == null))  { MessageBox.Show("Изображениe 13 пусто");  return; }
            if ((imageContainersList[10].getzArrayDescriptor() == null))  { MessageBox.Show("Изображениe 11 пусто");  return; }
            
            imagesForTable[0] = imageContainersList[11].getzArrayDescriptor();
            imagesForTable[1] = imageContainersList[12].getzArrayDescriptor();
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
        private void makeManuShots(object sender, RoutedEventArgs e)
        {
            ShotSeriesForm newForm = new ShotSeriesForm();
            newForm.oneShotOfSeries+= NewFormOnOneShotOfSeries;
            newForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void NewFormOnOneShotOfSeries(System.Drawing.Image newImage, int imageNumber)
        {
            ZArrayDescriptor result = Utils.getArrayFromImage((Bitmap)newImage);

            if (imageContainersList.Count < imageNumber)
            {
                addImageContainer(imageContainersList.Count, scrollerContent);
            }

            imageContainersList[imageNumber - 1].setzArrayDescriptor(result);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void oneImageOfSeriesTaken(System.Drawing.Image newImage, int imageNumber)
        {
            ZArrayDescriptor result = Utils.getArrayFromImage((Bitmap) newImage);

            if (imageContainersList.Count < imageNumber)
            {
                addImageContainer(imageContainersList.Count, scrollerContent);
            }

            imageContainersList[imageNumber - 1].setzArrayDescriptor(result);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EightPhotosForm newForm = new EightPhotosForm();
            newForm.oneShotOfSeries += NewFormOnOneShotOfSeries;
            newForm.Show();
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
            if (arrayDescriptor == null)
            {
                return;
            }

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (zArrayDescriptor != null)
            {
                Visualisation.VisualisationWindow visualisationWindow =
                    new Visualisation.VisualisationWindow(zArrayDescriptor, Visualisation.Mesh.ColoringMethod.Grayscale,
                        new Visualisation.BoundCamera(new OpenTK.Vector3(0, 0, 0), 0, 1.47f, 1000.0f));
                visualisationWindow.Run();
            }
        }

        private void button1231_Click(object sender, RoutedEventArgs e)
        {
            if (mainImage.Source == null) { MessageBox.Show("Главное изображение пустое"); return; }
            if (zArrayDescriptor == null) { MessageBox.Show("Z-массив пуст"); return; }
            int w = zArrayDescriptor.width;
            int h = zArrayDescriptor.height;
            Int64 min = Int64.MaxValue;
            Int64 max = Int64.MinValue;
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Int64 k = zArrayDescriptor.array[i, j];
                    if (k > max) max = k; if (k < min) min = k;
                }
            }

            int all = w;
            int done = 0;
            PopupProgressBar.show();
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Int64 k = zArrayDescriptor.array[i, j];
                    if (k > max) max = k; if (k < min) min = k;
                    zArrayDescriptor.array[i, j] = (k - min) * 255 / (max - min);
                }
                done++;
                PopupProgressBar.setProgress(done, all);
            }
            PopupProgressBar.close();
        }

        private void button121_Click(object sender, RoutedEventArgs e)
        {
            // if (mainImage.Source == null) { MessageBox.Show("Главное изображение пустое"); return; }
            // if (zArrayDescriptor == null) { MessageBox.Show("Z-массив пуст"); return; }
            ZArrayDescriptor[] source = new ZArrayDescriptor[8];

            for (int i = 0; i < 8; i++)
            {
                source[i] = imageContainersList[i].getzArrayDescriptor();
            }
            int all = 8;
            int done = 0;
            PopupProgressBar.show();
            for (int ii = 0; ii < 8; ii++)
            {
                int w = source[ii].width;
                int h = source[ii].height;
                Int64 min = Int64.MaxValue;
                Int64 max = Int64.MinValue;
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        Int64 k = source[ii].array[i, j];
                        if (k > max) max = k; if (k < min) min = k;
                    }
                }

                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        Int64 k = source[ii].array[i, j];
                        if (k > max) max = k; if (k < min) min = k;
                        source[ii].array[i, j] = (k - min) * 255 / (max - min);
                    }

                }

                done++;
                PopupProgressBar.setProgress(done, all);
            }
            PopupProgressBar.close();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                 Вырезать 8 кадров по 11 кадру
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            ZArrayDescriptor source = new ZArrayDescriptor();
            if (imageContainersList[10].getzArrayDescriptor() == null) { MessageBox.Show("11 изображение пустое"); return; }
           
            
            source = imageContainersList[10].getzArrayDescriptor();                                 // 11 кадр
            int w = source.width;
            int h = source.height;
            int x_begin = w, x_end = 0;
            int y_begin = h, y_end = 0;
            for (int j = 0; j < h; j++)
            {
                long x = 0;
                for (int i = 0; i < w; i++)
                {
                    x = source.array[i, j];
                    if ( x > 0) { if (x_end < i) x_end = i;}
                }
                x = w - 1;
                for (int i = w-1; i >= 0; i--)
                {
                    x = source.array[i, j];
                    if ( x > 0) { if (x_begin > i) x_begin = i; }
                }

            }
            
          
            for (int i = 0; i < w; i++)
            {
                long x = 0;
                for (int j = 0; j < h; j++)
                {
                    x = source.array[i, j];
                    if (x > 0) { if (y_end < j) y_end = j; }
                }
                x = w - 1;
                for (int j = h - 1; j >= 0; j--)
                {
                    x = source.array[i, j];
                    if (x > 0) { if (y_begin > j) y_begin = j; }
                }

            }
      
            //MessageBox.Show("x0 "+ x_begin+"x1 "+ x_end+"y0 "+ y_begin+"y1 "+y_end);

            ZArrayDescriptor[] zsc = new ZArrayDescriptor[8];
            for (int i = 0; i < 8; i++) { zsc[i] = imageContainersList[i].getzArrayDescriptor(); }  // 8 кадров
           
            int all  = 8;
            int done = 0;
            PopupProgressBar.show();
            for (int ii = 0; ii < 8; ii++)
            {

                ZArrayDescriptor zar = new ZArrayDescriptor();
                zar.array = new long[x_end - x_begin, y_end - y_begin];
                zar.width = x_end - x_begin;
                zar.height = y_end - y_begin;
                
                if (imageContainersList[ii].getzArrayDescriptor() == null) { continue; }
              
               for (int i = 0; i < x_end - x_begin; i++)
                {
                    for (int j = 0; j < y_end - y_begin; j++)
                    {
                        zar.array[i, j] = zsc[ii].array[i + x_begin, j + y_begin];
                    }

                }
              
             
               imageContainersList[ii].setzArrayDescriptor(zar);
              

                done++;
                PopupProgressBar.setProgress(done, all);
                
            }
            PopupProgressBar.close();

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                 Вырезать 14-й кадр по 11 кадру
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Cut14_Click(object sender, RoutedEventArgs e)
        {
            if (imageContainersList[10].getzArrayDescriptor() == null) { MessageBox.Show("11 изображение пустое"); return; }
            if (imageContainersList[13].getzArrayDescriptor() == null) { MessageBox.Show("14 изображение пустое"); return; }
            
            ZArrayDescriptor zsc1 = new ZArrayDescriptor();
            zsc1 = imageContainersList[13].getzArrayDescriptor();   // 14 кадр
            int w = zsc1.width;
            int h = zsc1.height;

            ZArrayDescriptor zsc2 = new ZArrayDescriptor();
            zsc2 = imageContainersList[10].getzArrayDescriptor();   // 14 кадр
           
           ZArrayDescriptor zar = new ZArrayDescriptor();
           zar.array = new long[w, h];
           zar.width = w;
           zar.height = h;

           for (int i = 0; i < w; i++)
             {
               for (int j = 0; j < h; j++)
                 {
                        if (zsc2.array[i, j] <= 0) {zar.array[i, j] = 0; } else { zar.array[i, j] = zsc1.array[i, j];}
                 }

             }
            
            imageContainersList[13].setzArrayDescriptor(zar);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                 СГЛАЖИВАНИЕ
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void smoothButton_Click(object sender, RoutedEventArgs e)
        {
            FiltrationForm filtrationForm = new FiltrationForm(FiltrationForm.FiltrationType.Smoothing, zArrayDescriptor);
            filtrationForm.imageFiltered += FiltrationFormOnImageFiltered;
            filtrationForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void smooth8Images(object sender, RoutedEventArgs e)
        {
            FiltrationForm filtrationForm = new FiltrationForm(FiltrationForm.FiltrationType.Smoothing, null);
            filtrationForm.filterParametersChoosed += FiltrationFormOnFilterParametersChoosed;
            filtrationForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void medianSmooth8Images(object sender, RoutedEventArgs e)
        {
            FiltrationForm filtrationForm = new FiltrationForm(FiltrationForm.FiltrationType.Median, null);
            filtrationForm.filterParametersChoosed += FiltrationFormOnFilterParametersChoosed;
            filtrationForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void FiltrationFormOnFilterParametersChoosed(FiltrationForm.FiltrationType filtrationType, int filtrationOrder)
        {
            for(int i = 0; i < imageContainersList.Count; i++)
            {
                ImageContainer currentContainer = imageContainersList[i];
                currentContainer.filterImage(filtrationType, filtrationOrder);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void FiltrationFormOnImageFiltered(ZArrayDescriptor filtratedImage)
        {
            setZArray(filtratedImage);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                             МЕДИАННАЯ ФИЛЬТРАЦИЯ
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void midianFilterButton_Click(object sender, RoutedEventArgs e)
        {
            FiltrationForm filtrationForm = new FiltrationForm(FiltrationForm.FiltrationType.Median, zArrayDescriptor);
            filtrationForm.imageFiltered += FiltrationFormOnImageFiltered;
            filtrationForm.Show();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                 Фурье-преобразование
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Furie_Click(object sender, RoutedEventArgs e)
        {
            if (mainImage.Source == null) { MessageBox.Show("Главное изображение пустое"); return; }
            if (zArrayDescriptor == null) { MessageBox.Show("Z-массив пуст"); return; }
            setZArray(FurieClass.BPF(zArrayDescriptor));
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                               Фильтрация методом наименьших квадратов
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void MNK_Click(object sender, RoutedEventArgs e)
        {
            if (mainImage.Source == null) { MessageBox.Show("Главное изображение пустое"); return; }
            if (zArrayDescriptor == null) { MessageBox.Show("Z-массив пуст"); return; }
            setZArray(FurieClass.MNK(zArrayDescriptor));

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Построение фигур Лиссажу
        private void lissaguButton_Click(object sender, RoutedEventArgs e)
        {
            LissajousForm firstForm = new LissajousForm();
            ZArrayDescriptor[] parameter = new ZArrayDescriptor[4];
            parameter[0] = imageContainersList[0].getzArrayDescriptor();
            parameter[1] = imageContainersList[1].getzArrayDescriptor();
            parameter[2] = imageContainersList[2].getzArrayDescriptor();
            parameter[3] = imageContainersList[3].getzArrayDescriptor();
            firstForm.setImages(parameter);
            firstForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadBunchOfImages(int numberOfImages)
        {
            ImageSource[] newSources = FilesHelper.loadBunchImages(numberOfImages);

            int all = numberOfImages - 1;
            int done = 0;
            PopupProgressBar.show();

            if (newSources != null)
            {
                for (int i = 0; i < numberOfImages; i++)
                {
                    imageContainersList[i].setImage((BitmapImage) newSources[i]);
                    newSources[i] = null;
                    done++;
                    PopupProgressBar.setProgress(done, all);
                }
            }
            PopupProgressBar.close();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void addImageContainer(int i, Grid newScrollerContent)
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
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadObjClicked(object sender, RoutedEventArgs e)
        {
            String path = FilesHelper.getPathToObjFile();

            if (path != null)
            {
                Visualisation.VisualisationWindow visualisationWindow =
                    new Visualisation.VisualisationWindow(path,
                        new Visualisation.BoundCamera(new OpenTK.Vector3(0, 0, 0), 0, 1.47f, 1000.0f));
                visualisationWindow.Run();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            addImageContainer(imageContainersList.Count, scrollerContent);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
