using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace rab1.Forms
{
    public delegate void FunctionPointer(object sender, EventArgs eventArgs);

    public partial class Form1 : Form
    {
        Image[] img = new Image[11];
        double[,] Float_Image1 = new double[512, 512];
        double[,] Float_Image2 = new double[512, 512];  
        
        double N_fz = 0, N_fz2 = 90, N_fz3 = 180, N_fz4 = 270;   // начальная фаза в градусах
    
        int regImage = 0;                            // Номер изображения (0-7)

        int cursorMode = 0;
        Point downPoint;
        Point upPoint;
        int scaleMode = 0;

        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);
        private delegate void StringParameterDelegate(List<Point3D> newList);

        CustomPictureBox firsPictureBox;
        CustomPictureBox secondPictureBox;

        int batchProcessingFlag = 0;

        private double currentScaleRatio = 1;
        private double initialScaleRatio = 1;
        private double afterRemovingScaleRatio = 1;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Form1()                                 
        {
            InitializeComponent();

            ShooterSingleton.init();

            relayout();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void imageTaken(Image newImage)
        {
            //изображение получено
            pictureBox1.Image = newImage;
            ShooterSingleton.imageCaptured -= imageTaken;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {
            if (mainPictureBox.Image == null)
            {
                return;
            }

            if (cursorMode != 0)
            {
                if (cursorMode == 2)
                {
                     ImageProcessor.floodImage(e.X, e.Y, Color.Black, mainPictureBox.Image);
                }

                return;
            }
            else 
            {
                ImageHelper.drawGraph(mainPictureBox.Image, e.X, e.Y, currentScaleRatio);
            }

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            regImage = 0;
          
            RadioButton rb = sender as RadioButton;

            if (rb == radioButton1) { regImage = 0;  }
            if (rb == radioButton2) { regImage = 1; }
            if (rb == radioButton3) { regImage = 2; }
            if (rb == radioButton4) { regImage = 3; }
            if (rb == radioButton5) { regImage = 4; }
            if (rb == radioButton6) { regImage = 5; }
            if (rb == radioButton7) { regImage = 6; }
            if (rb == radioButton8) { regImage = 7; }
            if (rb == radioButton9) { regImage = 8; }
            if (rb == radioButton10) { regImage = 9; }
            if (rb == radioButton14) { regImage = 10; }
            if (rb == radioButton18) { regImage = 12; }
            if (rb == radioButton19) { regImage = 13; }

            if (regImage < 12)
            {
                if (img[regImage] != null)
                {
                    imageWidth.Text = img[regImage].Width.ToString();
                    imageHeight.Text = img[regImage].Height.ToString();
                }
            }
            else
            {
                imageWidth.Text  =  Float_Image1.GetLength(0).ToString();
                imageHeight.Text = Float_Image1.GetLength(1).ToString();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button3_Click(object sender, EventArgs e)
        {
            applyScaleModeToPicturebox();

           if (regImage == 0)
            {
                mainPictureBox.Image = this.pictureBox1.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 1)
            {
                mainPictureBox.Image = this.pictureBox2.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 2)
            {
                mainPictureBox.Image = this.pictureBox3.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 3)
            {
                mainPictureBox.Image = this.pictureBox4.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 4)
            {
                mainPictureBox.Image = this.pictureBox5.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 5)
            {
                mainPictureBox.Image = this.pictureBox6.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 6)
            {
                mainPictureBox.Image = this.pictureBox7.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 7)
            {
                mainPictureBox.Image = this.pictureBox8.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 8)
            {
                mainPictureBox.Image = this.pictureBox9.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 9)
            {
                mainPictureBox.Image = this.pictureBox10.Image;
                currentScaleRatio = initialScaleRatio;
            }
            if (regImage == 10)
            {
                mainPictureBox.Image = this.pictureBox11.Image;
                currentScaleRatio = afterRemovingScaleRatio;
            }

            applyScaleModeToPicturebox();
        }

//----------------------------------------------------------------  <- Double

        private void button6_Click(object sender, EventArgs e)
        {
            applyScaleModeToPicturebox();

            if (regImage == 12)
            {
                mainPictureBox.Image = this.pictureBox12.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 13)
            {
                mainPictureBox.Image = this.pictureBox13.Image;
                currentScaleRatio = 1;
            }
           
            applyScaleModeToPicturebox();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button12_Click(object sender, EventArgs e)
        {
            {              
                img[regImage] = mainPictureBox.Image;
               
                switch (regImage)
                {
                    case 0: pictureBox1.Image = img[regImage]; break;
                    case 1: pictureBox2.Image = img[regImage]; break;
                    case 2: pictureBox3.Image = img[regImage]; break;
                    case 3: pictureBox4.Image = img[regImage]; break;
                    case 4: pictureBox5.Image = img[regImage]; break;
                    case 5: pictureBox6.Image = img[regImage]; break;
                    case 6: pictureBox7.Image = img[regImage]; break;
                    case 7: pictureBox8.Image = img[regImage]; break;
                    case 8: pictureBox9.Image = img[regImage]; break;
                    case 9: pictureBox10.Image = img[regImage]; break;
                    case 10: pictureBox11.Image = img[regImage]; break;
                }           
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ZGR_File(int i)
        {
            OpenFileDialog dialog1 = new OpenFileDialog();
            if (dialog1.ShowDialog() == DialogResult.OK)
            {
               // try
                {                    
                    dialog1.InitialDirectory = dialog1.FileName;

                    img[i] = mainPictureBox.Image;               
                    switch (i)
                    {
                        case 0:  pictureBox1.Image  = Image.FromFile(dialog1.FileName); img[i] = pictureBox1.Image;  break;
                        case 1:  pictureBox2.Image  = Image.FromFile(dialog1.FileName); img[i] = pictureBox2.Image;  break;
                        case 2:  pictureBox3.Image  = Image.FromFile(dialog1.FileName); img[i] = pictureBox3.Image;  break;
                        case 3:  pictureBox4.Image  = Image.FromFile(dialog1.FileName); img[i] = pictureBox4.Image;  break;
                        case 4:  pictureBox5.Image  = Image.FromFile(dialog1.FileName); img[i] = pictureBox5.Image;  break;
                        case 5:  pictureBox6.Image  = Image.FromFile(dialog1.FileName); img[i] = pictureBox6.Image;  break;
                        case 6:  pictureBox7.Image  = Image.FromFile(dialog1.FileName); img[i] = pictureBox7.Image;  break;
                        case 7:  pictureBox8.Image  = Image.FromFile(dialog1.FileName); img[i] = pictureBox8.Image;  break;
                        case 8:  pictureBox9.Image  = Image.FromFile(dialog1.FileName); img[i] = pictureBox9.Image;  break;
                        case 9:  pictureBox10.Image = Image.FromFile(dialog1.FileName); img[i] = pictureBox10.Image; break;
                        case 10: pictureBox11.Image = Image.FromFile(dialog1.FileName); img[i] = pictureBox11.Image; break;
                    }
                                
                                                                                       
                }
                //catch (Exception ex) { MessageBox.Show(" Ошибка " + ex.Message); }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(0); }
        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(1); }
        private void pictureBox3_MouseClick_1(object sender, MouseEventArgs e) { ZGR_File(2); }
        private void pictureBox4_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(3); }
        private void pictureBox5_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(4); }
        private void pictureBox6_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(5); }
        private void pictureBox7_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(6); }
        private void pictureBox8_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(7); }
        private void pictureBox9_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(8); }
        private void pictureBox10_MouseClick(object sender, MouseEventArgs e)  { ZGR_File(9); }
        private void pictureBox11_MouseClick(object sender, MouseEventArgs e)  { ZGR_File(10);}


        private void pictureBox12_Click(object sender, EventArgs e) { ZGR_File_Double(0); }

        private void pictureBox13_Click(object sender, EventArgs e) { ZGR_File_Double(1); }
//--------------------------------------------------------------------------------------------------------------------------------------------------      
        private void ZGR_File_Double(int i)   
        {
            int w1 = Float_Image1.GetLength(0);
            int h1 = Float_Image1.GetLength(1);
            int c=0;

            Bitmap bmp2 = new Bitmap(w1, h1);
            for (int x = 0; x < w1; x++)
            {
               for (int y = 0; y < h1; y++)
                {
                    switch (i)
                    {
                        case 0: c = (int)(Float_Image1[x, y]); break;
                        case 1: c = (int)(Float_Image2[x, y]); break;
                    }             
                  
                    bmp2.SetPixel(x, y, Color.FromArgb(c, c, c));
                }
            }
            switch (i)
            {
                case 0:  pictureBox12.Image = bmp2;  break;
                case 1:  pictureBox13.Image = bmp2;  break;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ZGRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog1 = new OpenFileDialog();

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {                    
                    dialog1.InitialDirectory = dialog1.FileName;
                    
                    mainPictureBox.Image = Image.FromFile(dialog1.FileName);

                    int w1 = mainPictureBox.Image.Width;
                    int h1 = mainPictureBox.Image.Height;
                    mainPictureBox.Size = new Size(w1, h1);
                    mainPictureBox.Show();
                                                                        // Вывод размера
                }
                catch (Exception ex) { MessageBox.Show("Ошибка " + ex.Message); }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private string SaveString(string string_dialog, int k)
        {
                string strk = k.ToString();

                string string_rab = string_dialog;
                if (string_dialog.Contains("1.")) { string_rab = string_dialog.Replace("1.", strk+"."); }
               

                return string_rab;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Save8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog1 = new OpenFileDialog();

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    for (int i = 0; i < 8; i++)
                    {
                        String str = SaveString(dialog1.FileName, i + 1);

                        switch (i)
                        {
                            case 0: pictureBox1.Image = Image.FromFile(str); pictureBox1.Invalidate(); img[0] = pictureBox1.Image; break;
                            case 1: pictureBox2.Image = Image.FromFile(str); pictureBox2.Invalidate(); img[1] = pictureBox2.Image; break;
                            case 2: pictureBox3.Image = Image.FromFile(str); pictureBox3.Invalidate(); img[2] = pictureBox3.Image; break;
                            case 3: pictureBox4.Image = Image.FromFile(str); pictureBox4.Invalidate(); img[3] = pictureBox4.Image; break;
                            case 4: pictureBox5.Image = Image.FromFile(str); pictureBox5.Invalidate(); img[4] = pictureBox5.Image; break;
                            case 5: pictureBox6.Image = Image.FromFile(str); pictureBox6.Invalidate(); img[5] = pictureBox6.Image; break;
                            case 6: pictureBox7.Image = Image.FromFile(str); pictureBox7.Invalidate(); img[6] = pictureBox7.Image; break;
                            case 7: pictureBox8.Image = Image.FromFile(str); pictureBox8.Invalidate(); img[7] = pictureBox8.Image; break;
                        }
                    }

                    if (img[regImage] != null)
                    {
                        imageWidth.Text = img[regImage].Width.ToString();
                        imageHeight.Text = img[regImage].Height.ToString();
                    }

                    if ((pictureBox1.Image.Size.Equals(pictureBox2.Image.Size))
                        && (pictureBox1.Image.Size.Equals(pictureBox3.Image.Size))
                        && (pictureBox1.Image.Size.Equals(pictureBox4.Image.Size))
                        && (pictureBox1.Image.Size.Equals(pictureBox5.Image.Size))
                        && (pictureBox1.Image.Size.Equals(pictureBox6.Image.Size))
                        && (pictureBox1.Image.Size.Equals(pictureBox7.Image.Size))
                        && (pictureBox1.Image.Size.Equals(pictureBox8.Image.Size)))
                    {
                        StretchImageForm newForm = new StretchImageForm();
                        newForm.initialSize = pictureBox1.Image.Size;
                        newForm.userChoosedSize += userChoosedSize;
                        newForm.Show();
                    }
                }
                catch (Exception ex) { MessageBox.Show(" Ошибка " + ex.Message); }
            }            
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void userChoosedSize(Size newSize)
        {
            for (int i = 0; i < 8; i++)
            {
                img[i] = ImageProcessor.ResizeBitmap((Bitmap)img[i], newSize.Width, newSize.Height);

                switch (i)
                {
                    case 0: pictureBox1.Image = img[i]; pictureBox1.Invalidate(); break;
                    case 1: pictureBox2.Image = img[i]; pictureBox2.Invalidate(); break;
                    case 2: pictureBox3.Image = img[i]; pictureBox3.Invalidate(); break;
                    case 3: pictureBox4.Image = img[i]; pictureBox4.Invalidate(); break;
                    case 4: pictureBox5.Image = img[i]; pictureBox5.Invalidate(); break;
                    case 5: pictureBox6.Image = img[i]; pictureBox6.Invalidate(); break;
                    case 6: pictureBox7.Image = img[i]; pictureBox7.Invalidate(); break;
                    case 7: pictureBox8.Image = img[i]; pictureBox8.Invalidate(); break;
                }
            }

            if (img[regImage] != null)
            {
                imageWidth.Text = img[regImage].Width.ToString();
                imageHeight.Text = img[regImage].Height.ToString();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SAVEToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            SaveFileDialog dialog1 = new SaveFileDialog();
            dialog1.Filter = "Bitmap(*.bmp)|*.bmp";

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    mainPictureBox.Image.Save(dialog1.FileName);
                    dialog1.InitialDirectory = dialog1.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" Ошибка при записи файла " + ex.Message); 
                }
            }                          
        }/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void сохранить8КадровToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog1 = new SaveFileDialog();
            dialog1.Filter = "Bitmap(*.bmp)|*.bmp";

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap newBitmap = new Bitmap(pictureBox1.Image);
                    newBitmap.Save(dialog1.FileName + "1.bmp", ImageFormat.Bmp);
                    dialog1.InitialDirectory = dialog1.FileName;

                    newBitmap = new Bitmap(pictureBox2.Image);
                    newBitmap.Save(dialog1.FileName + "2.bmp", ImageFormat.Bmp);

                    newBitmap = new Bitmap(pictureBox3.Image);
                    newBitmap.Save(dialog1.FileName + "3.bmp", ImageFormat.Bmp);

                    newBitmap = new Bitmap(pictureBox4.Image);
                    newBitmap.Save(dialog1.FileName + "4.bmp", ImageFormat.Bmp);

                    newBitmap = new Bitmap(pictureBox5.Image);
                    newBitmap.Save(dialog1.FileName + "5.bmp", ImageFormat.Bmp);

                    newBitmap = new Bitmap(pictureBox6.Image);
                    newBitmap.Save(dialog1.FileName + "6.bmp", ImageFormat.Bmp);

                    newBitmap = new Bitmap(pictureBox7.Image);
                    newBitmap.Save(dialog1.FileName + "7.bmp", ImageFormat.Bmp);

                    newBitmap = new Bitmap(pictureBox8.Image);
                    newBitmap.Save(dialog1.FileName + "8.bmp", ImageFormat.Bmp);
                }
                catch (Exception ex) 
                { 
                    MessageBox.Show(" Ошибка при записи файла " + ex.Message);
                }
            }      
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void aTAN123ToolStripMenuItem_Click_1(object sender, EventArgs e)         // -------- ATAN2 1,2,3,4
        {
            double[] fz = new double[4];
            fz[0] = N_fz; 
            fz[1] = N_fz2;
            fz[2] = N_fz3;
            fz[3] = N_fz4;
            int n = 3;  if (fz[3] != 0) { n = 4; }

            img[0] = pictureBox1.Image;
            img[1] = pictureBox2.Image;
            img[2] = pictureBox3.Image;
            img[3] = pictureBox4.Image;

            FazaClass.ATAN_123(img, pictureBox9, n, fz, 1);

            img[0] = pictureBox5.Image;
            img[1] = pictureBox6.Image;
            img[2] = pictureBox7.Image;
            img[3] = pictureBox8.Image;

            FazaClass.ATAN_123(img, pictureBox10, n, fz, 1);    
        }
//--------------------------------------------------------------------------------------------------------------  ATAN2 double
        private void aTAN2123412567813ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[] fz = new double[4];
            fz[0] = N_fz;
            fz[1] = N_fz2;
            fz[2] = N_fz3;
            fz[3] = N_fz4;
            int n = 3; if (fz[3] != 0) { n = 4; }
                         
            img[0] = pictureBox1.Image;
            img[1] = pictureBox2.Image;
            img[2] = pictureBox3.Image;
            img[3] = pictureBox4.Image;

            FazaClass.ATAN_1234(ref Float_Image1, img, pictureBox12, n, fz, 1);

            img[0] = pictureBox5.Image;
            img[1] = pictureBox6.Image;
            img[2] = pictureBox7.Image;
            img[3] = pictureBox8.Image;

            FazaClass.ATAN_1234(ref Float_Image2, img, pictureBox13, n, fz, 1);  
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void aTANRGBToolStripMenuItem_Click_1(object sender, EventArgs e)        // -------- ATAN2 RGB
        { 
            FazaClass.ATAN_RGB(mainPictureBox); 
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void aTAN2123GraphToolStripMenuItem_Click(object sender, EventArgs e)    // -------- ATAN2 эллипс
        {
            double[] fz = new double[4];
            fz[0] = N_fz; 
            fz[1] = N_fz2; 
            fz[2] = N_fz3; 
            fz[3] = N_fz4;
            int n = 3;

            if (fz[3] != 0)
            {
                n = 4;
            }

            img[0] = pictureBox1.Image;
            img[1] = pictureBox2.Image;
            img[2] = pictureBox3.Image;
            img[3] = pictureBox4.Image;
            img[4] = pictureBox5.Image;
            img[5] = pictureBox6.Image;
            img[6] = pictureBox7.Image;
            img[7] = pictureBox8.Image;

            FazaClass.imageProcessed -= imageProcessed;
            FazaClass.imageProcessedForOpenGL -= imageProcessedForOpenGLDelegate;
            FazaClass.imageProcessed += imageProcessed;
            FazaClass.imageProcessedForOpenGL += imageProcessedForOpenGLDelegate;

            PopupProgressBar.show();

            ThreadPool.QueueUserWorkItem((obj) =>
            {
                FazaClass.Graph_ATAN(img, n, fz, 1);
            });
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void aTAN2123456ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form firstForm = new Form();
            firsPictureBox = new CustomPictureBox();
            firsPictureBox.BackColor = Color.White;
            firsPictureBox.Location = new Point(0, 8);
            firsPictureBox.Size = new Size(800, 600);
            firsPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            firsPictureBox.BorderStyle = BorderStyle.Fixed3D;
            firstForm.Controls.Add(firsPictureBox);
            firstForm.Show();

            Form secondForm = new Form();
            secondPictureBox = new CustomPictureBox();
            secondPictureBox.BackColor = Color.White;
            secondPictureBox.Location = new Point(0, 8);
            secondPictureBox.Size = new Size(800, 600);
            secondPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            secondPictureBox.BorderStyle = BorderStyle.Fixed3D;
            secondForm.Controls.Add(secondPictureBox);
            secondForm.Show();

            double[] fz = new double[4];
            fz[0] = N_fz;
            fz[1] = N_fz2;
            fz[2] = N_fz3;
            fz[3] = N_fz4;
            int n = 3;

            if (fz[3] != 0)
            {
                n = 4;
            }

            img[0] = pictureBox1.Image;
            img[1] = pictureBox2.Image;
            img[2] = pictureBox3.Image;
            img[3] = pictureBox4.Image;

            FazaClass.imageProcessed -= imageProcessedForSecondMethod;
            FazaClass.imageProcessed += imageProcessedForSecondMethod;

            batchProcessingFlag = 0;

            PopupProgressBar.show();

            ThreadPool.QueueUserWorkItem((obj) =>
            {
                FazaClass.Graph_ATAN(img, n, fz, 1);
            });   
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void imageProcessedForSecondMethod(Bitmap newImage)
        {
            FazaClass.imageProcessed -= imageProcessedForSecondMethod;

            if (batchProcessingFlag == 0)
            {
                SetControlPropertyThreadSafe(firsPictureBox, "Image", newImage);
                batchProcessingFlag = 1;

                double[] fz = new double[4];
                fz[0] = N_fz;
                fz[1] = N_fz2;
                fz[2] = N_fz3;
                fz[3] = N_fz4;
                int n = 3;

                if (fz[3] != 0)
                {
                    n = 4;
                }

                img[0] = pictureBox5.Image;
                img[1] = pictureBox6.Image;
                img[2] = pictureBox7.Image;
                img[3] = pictureBox8.Image;

                FazaClass.imageProcessed -= imageProcessedForSecondMethod;
                FazaClass.imageProcessed += imageProcessedForSecondMethod;

                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    FazaClass.Graph_ATAN(img, n, fz, 1);
                }); 
            }
            else if (batchProcessingFlag == 1)
            {
                SetControlPropertyThreadSafe(secondPictureBox, "Image", newImage);
                batchProcessingFlag = 0;
            }        
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void imageProcessedForOpenGLDelegate(List<Point3D> newList)
        {
            FazaClass.imageProcessedForOpenGL -= imageProcessedForOpenGLDelegate;
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterDelegate(imageProcessedForOpenGLDelegate), new object[] { newList });
                return;
            }
            // Must be on the UI thread if we've got this far
            //
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void imageProcessed(Bitmap newImage)
        {
            FazaClass.imageProcessed -= imageProcessed;
            SetControlPropertyThreadSafe(mainPictureBox, "Image", newImage);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new[] { propertyValue });
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void captureImage(object sender, EventArgs e)
        {
            ShooterSingleton.imageCaptured += imageTaken;
            ShooterSingleton.getImage();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void getSeriesOfPhotos(object sender, EventArgs e)  
        {
            BackgroundImagesGeneratorForm newForm = new BackgroundImagesGeneratorForm();
            newForm.oneImageOfSeries += oneImageCaptured;
            newForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void oneImageCaptured(Image newImage, int imageNumber)
        {
            if (imageNumber == 1)
            {
                pictureBox1.Image = newImage;
            }
            else if (imageNumber == 2)
            {
                pictureBox2.Image = newImage;
            }
            else if (imageNumber == 3)
            {
                pictureBox3.Image = newImage;
            }
            else if (imageNumber == 4)
            {
                pictureBox4.Image = newImage;
            }
            else if (imageNumber == 5)
            {
                pictureBox5.Image = newImage;
            }
            else if (imageNumber == 6)
            {
                pictureBox6.Image = newImage;
            }
            else if (imageNumber == 7)
            {
                pictureBox7.Image = newImage;
            }
            else if (imageNumber == 8)
            {
                pictureBox8.Image = newImage;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton913.Checked == true)
            {
                cursorMode = 0;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            scaleMode = 0;
            radioButton12.Checked = true;
            radioButton13.Checked = false;
            applyScaleModeToPicturebox();

            if (radioButton103.Checked == true)
            {
                cursorMode = 1;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton11.Checked == true)
            {
                cursorMode = 2;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void pictureBox01_MouseDown(object sender, MouseEventArgs e)
        {
            if (cursorMode == 1)
            {
                downPoint = new Point(e.X, e.Y);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void pictureBox01_MouseUp(object sender, MouseEventArgs e)
        {
            if (mainPictureBox.Image == null)
            {
                return;
            }

            if (cursorMode == 1)
            {
                upPoint = new Point(e.X, e.Y);
            }

            Graphics graphics = Graphics.FromImage(mainPictureBox.Image);

            Pen p = new Pen(Color.Black, 4);
            graphics.DrawLine(p, downPoint, upPoint);

            mainPictureBox.Invalidate();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            relayout();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            relayout();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void relayout()
        {
            this.panel1.Size = new Size(this.Size.Width - 160, this.Size.Height - 150);
            this.mainPictureBox.Size = new Size(this.panel1.Size.Width - 120, this.panel1.Size.Height - 36);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            scaleMode = 0;
            applyScaleModeToPicturebox();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            scaleMode = 1;
            applyScaleModeToPicturebox();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void applyScaleModeToPicturebox()
        {
            if (scaleMode == 0)
            {
                mainPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            }
            else if (scaleMode == 1)
            {
                mainPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }

            relayout();
            mainPictureBox.Invalidate();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            Color fillColor = Color.Black;

            for(int i = 0; i < mainPictureBox.Image.Size.Width; i++)
            {
                for (int j = 0; j < mainPictureBox.Image.Size.Height; j++)
                {
                    Color pixelcolor = ((Bitmap)(mainPictureBox.Image)).GetPixel(i, j);
                    if (pixelcolor.ToArgb() == fillColor.ToArgb())
                    {
                        ((Bitmap)(pictureBox1.Image)).SetPixel(i, j, fillColor);
                        ((Bitmap)(pictureBox2.Image)).SetPixel(i, j, fillColor);
                        ((Bitmap)(pictureBox3.Image)).SetPixel(i, j, fillColor);
                        ((Bitmap)(pictureBox4.Image)).SetPixel(i, j, fillColor);
                        ((Bitmap)(pictureBox5.Image)).SetPixel(i, j, fillColor);
                        ((Bitmap)(pictureBox6.Image)).SetPixel(i, j, fillColor);
                        ((Bitmap)(pictureBox7.Image)).SetPixel(i, j, fillColor);
                        ((Bitmap)(pictureBox8.Image)).SetPixel(i, j, fillColor);
                    }
                }
            }

            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
            pictureBox3.Invalidate();
            pictureBox4.Invalidate();
            pictureBox5.Invalidate();
            pictureBox6.Invalidate();
            pictureBox7.Invalidate();
            pictureBox8.Invalidate();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button2_Click(object sender, EventArgs e)
        {
            Rectangle neededRect = ImageHelper.determineImageRect((Bitmap)mainPictureBox.Image);

            Bitmap target = new Bitmap(neededRect.Width, neededRect.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(mainPictureBox.Image, new Rectangle(0, 0, target.Width, target.Height), neededRect, GraphicsUnit.Pixel);
            }
            mainPictureBox.Image = target;

            if (pictureBox1.Image != null)
            {
                target = new Bitmap(neededRect.Width, neededRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(pictureBox1.Image, new Rectangle(0, 0, target.Width, target.Height), neededRect, GraphicsUnit.Pixel);
                }
                pictureBox1.Image = target;
            }

            if (pictureBox2.Image != null)
            {
                target = new Bitmap(neededRect.Width, neededRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(pictureBox2.Image, new Rectangle(0, 0, target.Width, target.Height), neededRect, GraphicsUnit.Pixel);
                }
                pictureBox2.Image = target;
            }

            if (pictureBox3.Image != null)
            {
                target = new Bitmap(neededRect.Width, neededRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(pictureBox3.Image, new Rectangle(0, 0, target.Width, target.Height), neededRect, GraphicsUnit.Pixel);
                }
                pictureBox3.Image = target;
            }

            if (pictureBox4.Image != null)
            {
                target = new Bitmap(neededRect.Width, neededRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(pictureBox4.Image, new Rectangle(0, 0, target.Width, target.Height), neededRect, GraphicsUnit.Pixel);
                }
                pictureBox4.Image = target;
            }

            if (pictureBox5.Image != null)
            {
                target = new Bitmap(neededRect.Width, neededRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(pictureBox5.Image, new Rectangle(0, 0, target.Width, target.Height), neededRect, GraphicsUnit.Pixel);
                }
                pictureBox5.Image = target;
            }

            if (pictureBox6.Image != null)
            {
                target = new Bitmap(neededRect.Width, neededRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(pictureBox6.Image, new Rectangle(0, 0, target.Width, target.Height), neededRect, GraphicsUnit.Pixel);
                }
                pictureBox6.Image = target;
            }

            if (pictureBox7.Image != null)
            {
                target = new Bitmap(neededRect.Width, neededRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(pictureBox7.Image, new Rectangle(0, 0, target.Width, target.Height), neededRect, GraphicsUnit.Pixel);
                }
                pictureBox7.Image = target;
            }

            if (pictureBox8.Image != null)
            {
                target = new Bitmap(neededRect.Width, neededRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(pictureBox8.Image, new Rectangle(0, 0, target.Width, target.Height), neededRect, GraphicsUnit.Pixel);
                }
                pictureBox8.Image = target;
            }

            imageWidth.Text = mainPictureBox.Width.ToString();
            imageHeight.Text = mainPictureBox.Height.ToString();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void таблица2ToolStripMenuItem_Click(object sender, EventArgs e)                                // Построение таблицы
        {
            Image[] imagesForTable = new Image[3];

            imagesForTable[0] = pictureBox9.Image;     // 1 фаза
            imagesForTable[1] = pictureBox10.Image;    // 2 фаза
            imagesForTable[2] = pictureBox11.Image;    // 3 ограничение по контуру
           

            TableGenerateForm tableGenerateForm = new TableGenerateForm(imagesForTable);
            tableGenerateForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button4_Click_1(object sender, EventArgs e)
        {
            List<Point> previousBorderPoints = new List<Point>();

            if ((mainPictureBox.Image == null) || (pictureBox1.Image == null))
            {
                MessageBox.Show("Сначала загрузите изображение");
                return;
            }

            Bitmap copyOfImage = new Bitmap((Bitmap)mainPictureBox.Image);
            BitmapData imageData = ImageProcessor.getBitmapData(copyOfImage);
            Color previousColor = Color.White;


            List<Point> startPoints = new List<Point>();
            List<Point> endPoints = new List<Point>();

            //for (int x = 0; x < copyOfImage.Width; x++)
            for (int y = 0; y < copyOfImage.Height; y++)
            {
                List<Point> tempBorders = new List<Point>();

                //for (int y = 0; y < copyOfImage.Height; y++)
                for (int x = 0; x < copyOfImage.Width; x++)
                {
                    Color currentColor = ImageProcessor.getPixel(x, y, imageData);

                    if (Math.Abs(previousColor.R - currentColor.R) > 120)
                    {
                        ImageProcessor.setPixel(imageData, x, y, Color.Red);
                        Point newBorderPoint = new Point(x, y);
                        tempBorders.Add(newBorderPoint);

                        Point closestPoint = new Point(-100, -100);
                        foreach(Point currentPoint in previousBorderPoints)
                        {
                            if (Math.Abs(currentPoint.Y - newBorderPoint.Y) < Math.Abs(closestPoint.Y - newBorderPoint.Y))
                            {
                                closestPoint = currentPoint;
                            }
                        }

                        if ((Math.Abs(closestPoint.Y - newBorderPoint.Y) >= 0) && ((Math.Abs(closestPoint.X - newBorderPoint.X) < 20)))
                        {
                            startPoints.Add(closestPoint);
                            endPoints.Add(newBorderPoint);
                        }
                    }
                    previousColor = currentColor;
                }
                previousBorderPoints = tempBorders;
            }

            BitmapData imageData2 = ImageProcessor.getBitmapData((Bitmap)pictureBox1.Image);

            for (int x = 0; x < mainPictureBox.Image.Width; x++)
            {
                for (int y = 0; y < mainPictureBox.Image.Height; y++)
                {
                    Color currentColor = ImageProcessor.getPixel(x, y, imageData2);

                    if (currentColor.ToArgb() == Color.Black.ToArgb())
                    {
                        ImageProcessor.setPixel(imageData, x, y, Color.Red);
                    }
                }
            }

            copyOfImage.UnlockBits(imageData);
            ((Bitmap)pictureBox1.Image).UnlockBits(imageData2);


            Graphics graphics = Graphics.FromImage(copyOfImage);
            for (int i = 0; i < startPoints.Count; i++)
            {
                Point start = startPoints[i];
                Point end = endPoints[i];

                Pen p = new Pen(Color.Red, 1);
                graphics.DrawLine(p, start, end);
            }

            RestoreForm restoreForm = new RestoreForm();
            restoreForm.imageRestored += restoderImageReceived;
            restoreForm.phaseMapBuilded += phaseImageReceived;
            restoreForm.imageToEdit = copyOfImage;
            restoreForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void restoderImageReceived(Bitmap newBitmap, double ratio)
        {
            pictureBox10.Image = newBitmap;
            pictureBox10.Invalidate();
            pictureBox10.Update();

            afterRemovingScaleRatio = ratio; 
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void phaseImageReceived(Bitmap newBitmap, double ratio)
        {
            pictureBox9.Image = newBitmap;
            pictureBox9.Invalidate();
            pictureBox9.Update();

            initialScaleRatio = ratio;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //   Сглаживание
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void сглаживаниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FiltrationForm filtrationForm = new FiltrationForm(FiltrationForm.FiltrationType.Smoothing, mainPictureBox.Image);
            filtrationForm.imageFiltered+= filtrationFormOnImageFiltered;
            filtrationForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void filtrationFormOnImageFiltered(Image filtratedImage)
        {
            mainPictureBox.Size = new Size(filtratedImage.Width, filtratedImage.Height);
            mainPictureBox.Image = filtratedImage;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //  медианный фильтр
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void медианныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FiltrationForm filtrationForm = new FiltrationForm(FiltrationForm.FiltrationType.Median, mainPictureBox.Image);
            filtrationForm.imageFiltered += filtrationFormOnImageFiltered;
            filtrationForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //  Транспонирование
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void транспонированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FiltrClass.Transp(mainPictureBox);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button5_Click(object sender, EventArgs e)
        {
            Image targetImage = pictureBox9.Image;
            List<Point3D> pointsList = new List<Point3D>();

            for (int i = 0; i < targetImage.Width; i++)
            {
                for (int j = 0; j < targetImage.Height; j++)
                {
                    Color currentColor = ((Bitmap)(targetImage)).GetPixel(i, j);
                    int currentIntencity = currentColor.R;
                    Point3D newPoint = new Point3D(i, j, currentIntencity);

                    if (newPoint.z != 0)
                    {
                        pointsList.Add(newPoint);
                    }
                }
            }

            Pi_Class1.Plane somePlane = RestoreForm.getPlaneParams(pointsList);
            
            subPlane(somePlane);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void subPlane(Pi_Class1.Plane somePlane)
        {
            Image targetImage = pictureBox9.Image;
            List<Point3D> pointsList = new List<Point3D>();

            for (int i = 0; i < targetImage.Width; i++)
            {
                for (int j = 0; j < targetImage.Height; j++)
                {
                    Color currentColor = ((Bitmap)(targetImage)).GetPixel(i, j);
                    int currentIntencity = currentColor.R;
                    Point3D newPoint = new Point3D(i, j, currentIntencity);

                    if (newPoint.z != 0)
                    {
                        pointsList.Add(newPoint);
                    }
                }
            }

            OpenGLForm newForm = new OpenGLForm();

            foreach (Point3D currentPoint in pointsList)
            {
                double planeZ = (somePlane.a * currentPoint.x) + (somePlane.b * currentPoint.y) + somePlane.c;
                //newForm.addPoint(new Point3D(currentPoint.x, currentPoint.y, (int)(currentPoint.z - planeZ), Color.RoyalBlue));
                newForm.addPoint(new Point3D(currentPoint.x, currentPoint.y, (int)planeZ, Color.Red));
                newForm.addPoint(new Point3D(currentPoint.x, currentPoint.y, currentPoint.z, Color.Green));

                Color planeColor;

                if ((int)(currentPoint.z - planeZ) > 255)
                {
                    planeColor = Color.White;
                }
                else if ((int)(currentPoint.z - planeZ) < 0)
                {
                    planeColor = Color.Black;
                }
                else
                {
                    planeColor = Color.FromArgb((int)(currentPoint.z - planeZ), (int)(currentPoint.z - planeZ),
                    (int)(currentPoint.z - planeZ));
                }

                ((Bitmap)(mainPictureBox.Image)).SetPixel(currentPoint.x, currentPoint.y, planeColor);
            }

            mainPictureBox.Invalidate();

            newForm.Show();



            PlaneParamsForm planeParamsForm = new PlaneParamsForm(somePlane);
            planeParamsForm.planeParamsChoosed+=PlaneParamsFormOnPlaneParamsChoosed;
            planeParamsForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void PlaneParamsFormOnPlaneParamsChoosed(Pi_Class1.Plane somePlane)
        {
            subPlane(somePlane);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void определениеПлоскостиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button5_Click(sender, e);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                  Расшифровка
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void расшифровка2piToolStripMenuItem_Click(object sender, EventArgs e)
        {

        
           // this.DIALOG_CHINA(this.RSHFRT);
            int sN1=167;
            int sN2=241;
            int Diag=9;
            int pr = 30;                       // Отсечение
            Image[] imagesF = new Image[4];

            imagesF[0] = pictureBox9.Image;     // 1 фаза
            imagesF[1] = pictureBox10.Image;    // 2 фаза
            imagesF[2] = pictureBox11.Image;    // 3 ограничение по контуру
            //imagesF[3] = pictureBox8.Image; 
           //Pi_Class1.pi2_rshfr(Image[] img, PictureBox pictureBox01,  sN1,  sN2, int Diag)
            Pi_Class1.pi2_rshfr(imagesF,  pictureBox8, sN1, sN2, Diag);
        }
/*
        private void RSHFRT(object sender, EventArgs e)
        {
            string strN1 = " ", strN2 = " ";
            strN1 = tb1.Text;
            strN2 = tb2.Text;
            if (tb3.Text != "") NDiag = Convert.ToInt32(tb3.Text);
            Pi_Class1.pi2_rshfr(img, pictureBox01, strN1, strN2, NDiag);
        }
 */
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
