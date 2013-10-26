using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.Reflection;
using rab1.Forms;

namespace rab1
{
    public delegate void FunctionPointer(object sender, EventArgs eventArgs);

    public partial class Form1 : Form
    {
        Image[] img = new Image[11];

        Double Gamma = 1.0;                      // Гамма коррекция


        TextBox tb1, tb2, tb3, tb4;   
        
        
        double N_sin  = 167;                          // число синусоид 1
        double N2_sin = 241;                          // число синусоид 2
        int NDiag = 9;
        double N_fz = 0, N_fz2 = 90, N_fz3 = 180, N_fz4 = 270;   // начальная фаза в градусах

        Form newForm;
        Point p;
        
        CheckBox rb3;


        Form f_filt;                             // Для Фильтрации
        TextBox tb1_filt, tb2_filt, tb3_filt;
        int k_filt = 1;

        int x0_end = 0, y0_end = 0;
        int x1_end = 0, y1_end = 0;

       
      
        string string_dialog = "D:\\Студенты\\Эксперимент";       
        int regImage = 0;                            // Номер изображения (0-7)

        int pr_obr = 10;

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
            if (pictureBox01.Image == null)
            {
                return;
            }

            if (cursorMode != 0)
            {
                if (cursorMode == 2)
                {
                     ImageProcessor.floodImage(e.X, e.Y, Color.Black, pictureBox01.Image);
                }

                return;
            }
            else 
            {
                ImageHelper.drawGraph(pictureBox01.Image, e.X, e.Y, currentScaleRatio);
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

            if (img[regImage] != null)
            {
                imageWidth.Text = img[regImage].Width.ToString();
                imageHeight.Text = img[regImage].Height.ToString();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button3_Click(object sender, EventArgs e)
        {
            applyScaleModeToPicturebox();

            /*if (reg_image == 0)
            {
                pictureBox01.Image = new Bitmap(this.pictureBox1.Image);
            }
            if (reg_image == 1)
            {
                pictureBox01.Image = new Bitmap(this.pictureBox2.Image);
            }
            if (reg_image == 2)
            {
                pictureBox01.Image = new Bitmap(this.pictureBox3.Image);
            }
            if (reg_image == 3)
            {
                pictureBox01.Image = new Bitmap(this.pictureBox4.Image);
            }
            if (reg_image == 4)
            {
                pictureBox01.Image = new Bitmap(this.pictureBox5.Image);
            }
            if (reg_image == 5)
            {
                pictureBox01.Image = new Bitmap(this.pictureBox6.Image);
            }
            if (reg_image == 6)
            {
                pictureBox01.Image = new Bitmap(this.pictureBox7.Image);
            }
            if (reg_image == 7)
            {
                pictureBox01.Image = new Bitmap(this.pictureBox8.Image);
            }
            if (reg_image == 8)
            {
                pictureBox01.Image = new Bitmap(this.pictureBox9.Image);
            }
            if (reg_image == 9)
            {
                pictureBox01.Image = new Bitmap(this.pictureBox10.Image);
            }
            if (reg_image == 10)
            {
                pictureBox01.Image = new Bitmap(this.pictureBox11.Image);
            }*/

            if (regImage == 0)
            {
                pictureBox01.Image = this.pictureBox1.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 1)
            {
                pictureBox01.Image = this.pictureBox2.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 2)
            {
                pictureBox01.Image = this.pictureBox3.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 3)
            {
                pictureBox01.Image = this.pictureBox4.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 4)
            {
                pictureBox01.Image = this.pictureBox5.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 5)
            {
                pictureBox01.Image = this.pictureBox6.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 6)
            {
                pictureBox01.Image = this.pictureBox7.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 7)
            {
                pictureBox01.Image = this.pictureBox8.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 8)
            {
                pictureBox01.Image = this.pictureBox9.Image;
                currentScaleRatio = 1;
            }
            if (regImage == 9)
            {
                pictureBox01.Image = this.pictureBox10.Image;
                currentScaleRatio = initialScaleRatio;
            }
            if (regImage == 10)
            {
                pictureBox01.Image = this.pictureBox11.Image;
                currentScaleRatio = afterRemovingScaleRatio;
            }

            applyScaleModeToPicturebox();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button12_Click(object sender, EventArgs e)
        {
            {              
                img[regImage] = pictureBox01.Image;
               
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
            dialog1.InitialDirectory = string_dialog;
            if (dialog1.ShowDialog() == DialogResult.OK)
            {
               // try
                {                    
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;

                    img[i] = pictureBox01.Image;               
                    switch (i)
                    {
                        case 0: pictureBox1.Image = Image.FromFile(dialog1.FileName); img[i] = pictureBox1.Image; break;
                        case 1: pictureBox2.Image = Image.FromFile(dialog1.FileName); img[i] = pictureBox2.Image; break;
                        case 2: pictureBox3.Image = Image.FromFile(dialog1.FileName); img[i] = pictureBox3.Image; break;
                        case 3: pictureBox4.Image = Image.FromFile(dialog1.FileName); img[i] = pictureBox4.Image; break;
                        case 4: pictureBox5.Image = Image.FromFile(dialog1.FileName); img[i] = pictureBox5.Image; break;
                        case 5: pictureBox6.Image = Image.FromFile(dialog1.FileName); img[i] = pictureBox6.Image; break;
                        case 6: pictureBox7.Image = Image.FromFile(dialog1.FileName); img[i] = pictureBox7.Image; break;
                        case 7: pictureBox8.Image = Image.FromFile(dialog1.FileName); img[i] = pictureBox8.Image; break;
                        case 8: pictureBox9.Image = Image.FromFile(dialog1.FileName); img[i] = pictureBox9.Image; break;
                        case 9: pictureBox10.Image = Image.FromFile(dialog1.FileName); img[i] = pictureBox10.Image; break;
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
        private void pictureBox9_MouseClick(object sender, MouseEventArgs e) { ZGR_File(8); }
        private void pictureBox10_MouseClick(object sender, MouseEventArgs e) { ZGR_File(9); }
        private void pictureBox11_MouseClick(object sender, MouseEventArgs e) { ZGR_File(10); }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ZGRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog1 = new OpenFileDialog();
            dialog1.InitialDirectory = string_dialog;

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {                    
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;
                    
                    pictureBox01.Image = Image.FromFile(dialog1.FileName);

                    int w1 = pictureBox01.Image.Width;
                    int h1 = pictureBox01.Image.Height;
                    pictureBox01.Size = new Size(w1, h1);
                    pictureBox01.Show();
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
            var dialog1 = new OpenFileDialog();
            dialog1.InitialDirectory = string_dialog;

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;
                    string str = string_dialog;
                    for (int i = 0; i < 8; i++)
                    {
                        str = SaveString(string_dialog, i + 1);
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



                    GC.Collect();
                    GC.WaitForPendingFinalizers();


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
            dialog1.InitialDirectory = string_dialog;
            dialog1.Filter = "Bitmap(*.bmp)|*.bmp";

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox01.Image.Save(dialog1.FileName);
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;
                                               
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" Ошибка при записи файла " + ex.Message); 
                }
            }                          
        }/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void сохранить8КадровToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog1 = new SaveFileDialog();
            dialog1.InitialDirectory = string_dialog;
            dialog1.Filter = "Bitmap(*.bmp)|*.bmp";

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap newBitmap = new Bitmap(pictureBox1.Image);
                    newBitmap.Save(dialog1.FileName + "1.bmp", System.Drawing.Imaging.ImageFormat.Jpeg);
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;

                    newBitmap = new Bitmap(pictureBox2.Image);
                    newBitmap.Save(dialog1.FileName + "2.bmp", System.Drawing.Imaging.ImageFormat.Jpeg);

                    newBitmap = new Bitmap(pictureBox3.Image);
                    newBitmap.Save(dialog1.FileName + "3.bmp", System.Drawing.Imaging.ImageFormat.Jpeg);

                    newBitmap = new Bitmap(pictureBox4.Image);
                    newBitmap.Save(dialog1.FileName + "4.bmp", System.Drawing.Imaging.ImageFormat.Jpeg);

                    newBitmap = new Bitmap(pictureBox5.Image);
                    newBitmap.Save(dialog1.FileName + "5.bmp", System.Drawing.Imaging.ImageFormat.Jpeg);

                    newBitmap = new Bitmap(pictureBox6.Image);
                    newBitmap.Save(dialog1.FileName + "6.bmp", ImageFormat.Jpeg);

                    newBitmap = new Bitmap(pictureBox7.Image);
                    newBitmap.Save(dialog1.FileName + "7.bmp", ImageFormat.Jpeg);

                    newBitmap = new Bitmap(pictureBox8.Image);
                    newBitmap.Save(dialog1.FileName + "8.bmp", ImageFormat.Jpeg);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch (Exception ex) 
                { 
                    MessageBox.Show(" Ошибка при записи файла " + ex.Message);
                }
            }      
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SumDialog(EventHandler functionPointer, int S, int S1,int S2)
        {
            int max_x = 170, max_y = 100;

            f_filt = new Form();
            f_filt.Size = new Size(max_x, max_y + 36);
            f_filt.StartPosition = FormStartPosition.Manual;
            Point p = this.Location;
            p.Offset(200, 105);
            f_filt.Location = p;
           
             tb1_filt = new TextBox();
             tb1_filt.Location = new System.Drawing.Point(8, 30);
             tb1_filt.Size = new System.Drawing.Size(20, 20);
             tb1_filt.Text = S1.ToString();

            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(35, 30);
            label1.Size = new System.Drawing.Size(20, 20);
            label1.Text = " + ";

            tb2_filt = new TextBox();
            tb2_filt.Location = new System.Drawing.Point(60, 30);
            tb2_filt.Size = new System.Drawing.Size(20, 20);
            tb2_filt.Text = S2.ToString();

            Label label2 = new Label();
            label2.Location = new System.Drawing.Point(85, 30);
            label2.Size = new System.Drawing.Size(30, 20);
            label2.Text = " => ";

            tb3_filt = new TextBox();
            tb3_filt.Location = new System.Drawing.Point(120, 30);
            tb3_filt.Size = new System.Drawing.Size(20, 20);
            tb3_filt.Text = S.ToString();

            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(8, 60);
            b1.Text = "ok";
            b1.Size = new System.Drawing.Size(140, 30);
            b1.Click += new System.EventHandler(functionPointer);

            f_filt.Controls.Add(label1);
            f_filt.Controls.Add(label2);
            f_filt.Controls.Add(tb1_filt);
            f_filt.Controls.Add(tb2_filt);
            f_filt.Controls.Add(tb3_filt);
            f_filt.Controls.Add(b1);

            f_filt.Show();
        }     
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void DIALOG_CHINA(EventHandler functionPointer)
        {
            int max_x = 220, max_y = 260;
            newForm = new Form();
            newForm.Size = new Size(max_x, max_y);
            newForm.StartPosition = FormStartPosition.Manual;
            p = this.Location;                // Глобальный
            p.Offset(100, 105);
            newForm.Location = p;

            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(4, 10);
            label1.Size = new System.Drawing.Size(120, 20);
            label1.Text = "Число синусоид 1:";

            tb1 = new TextBox
                {
                    Location = new Point(126, 10),
                    Size = new Size(80, 8),
                    Text = N_sin.ToString()
                };

            // Фазовый сдвиг
            Label label2 = new Label();
            label2.Location = new Point(4, 50);
            label2.Size = new Size(120, 20);
            label2.Text = "Число синусоид 2:";

            tb2 = new TextBox
                {
                    Location = new Point(126, 50),
                    Size = new Size(80, 8),
                    Text = N2_sin.ToString()
                };

            Label label3 = new Label();
            label3.Location = new Point(4, 90);
            label3.Size = new Size(120, 20);
            label3.Text = "Число периодов:";

            tb3 = new TextBox();
            tb3.Location = new Point(126, 90);
            tb3.Size = new Size(80, 8);
            tb3.Text = NDiag.ToString();

            Label label4 = new Label();
            label4.Location = new Point(4, 130);
            label4.Size = new Size(120, 20);
            label4.Text = "Уровень обрезания (N точек) ";

            tb4 = new TextBox();
            tb4.Location = new System.Drawing.Point(126, 130);
            tb4.Size = new System.Drawing.Size(80, 8);
            tb4.Text = pr_obr.ToString();

            rb3 = new CheckBox();                                                  // ----------   CheckBox rb3;
            rb3.Location = new System.Drawing.Point(22, 160);
            rb3.Size = new System.Drawing.Size(120, 18);
            rb3.Text = "По форме 3 кадра";
            rb3.Checked = true;

            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(8, 190);
            b1.Text = "ok";
            b1.Size = new System.Drawing.Size(160, 30);
            b1.Click += new System.EventHandler(functionPointer);


            newForm.Controls.Add(label1);
            newForm.Controls.Add(label2);
            newForm.Controls.Add(label3);
            newForm.Controls.Add(label4);
            newForm.Controls.Add(tb1);
            newForm.Controls.Add(tb2);
            newForm.Controls.Add(tb3);
            newForm.Controls.Add(tb4);
            newForm.Controls.Add(b1);
            newForm.Controls.Add(rb3);

            newForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void pi2_Click(object sender, EventArgs e)
        {
            img[0] = pictureBox1.Image;
            img[1] = pictureBox2.Image;
            img[2] = pictureBox3.Image;
            img[3] = pictureBox4.Image;

            int rb_int = 0;
            string strN1 = " ", strN2 = " ";
            strN1 = tb1.Text;
            strN2 = tb2.Text;
            if (tb3.Text != "") NDiag = Convert.ToInt32(tb3.Text);
            N_sin = Convert.ToDouble(tb1.Text);
            N2_sin = Convert.ToDouble(tb2.Text);
            pr_obr = Convert.ToInt32(tb4.Text);
            if (rb3.Checked) rb_int = 1; else rb_int = 0;                                                // По форме 3 кадра
            Pi_Class1.pi2_frml(img, pictureBox01, strN1, strN2, NDiag, p, x0_end, x1_end, y0_end, y1_end, rb_int, pr_obr);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void aTAN123ToolStripMenuItem_Click_1(object sender, EventArgs e)         // -------- ATAN2 1,2,3
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



            /*Form firstForm = new Form();
            firsPictureBox = new CustomPictureBox();
            firsPictureBox.BackColor = Color.White;
            firsPictureBox.Location = new System.Drawing.Point(0, 8);
            firsPictureBox.Size = new Size(800, 600);
            firsPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            firsPictureBox.BorderStyle = BorderStyle.Fixed3D;
            firsPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            firstForm.Controls.Add(firsPictureBox);
            firstForm.Show();*/

            img[0] = pictureBox1.Image;
            img[1] = pictureBox2.Image;
            img[2] = pictureBox3.Image;
            img[3] = pictureBox4.Image;

            FazaClass.ATAN_123(img, pictureBox9, n, fz, Gamma);

            /*System.Windows.Forms.SaveFileDialog dialog1 = new System.Windows.Forms.SaveFileDialog();
            dialog1.InitialDirectory = string_dialog;
            dialog1.Filter = "Bitmap(*.bmp)|*.bmp";

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    firsPictureBox.Image.Save(dialog1.FileName);
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(" Ошибка при записи файла " + ex.Message);
                }
            }*/    


            /*Form secondForm = new Form();
            secondPictureBox = new CustomPictureBox();
            secondPictureBox.BackColor = Color.White;
            secondPictureBox.Location = new System.Drawing.Point(0, 8);
            secondPictureBox.Size = new Size(800, 600);
            secondPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            secondPictureBox.BorderStyle = BorderStyle.Fixed3D;
            secondPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            secondForm.Controls.Add(secondPictureBox);
            secondForm.Show();*/

           img[0] = pictureBox5.Image;
           img[1] = pictureBox6.Image;
           img[2] = pictureBox7.Image;
           img[3] = pictureBox8.Image;

           FazaClass.ATAN_123(img, pictureBox10, n, fz, Gamma);    
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void aTANRGBToolStripMenuItem_Click_1(object sender, EventArgs e)        // -------- ATAN2 RGB
        { 
            FazaClass.ATAN_RGB(pictureBox01); 
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
                FazaClass.Graph_ATAN(img, x0_end, x1_end, y0_end, y1_end, n, fz, Gamma);
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
                FazaClass.Graph_ATAN(img, x0_end, x1_end, y0_end, y1_end, n, fz, Gamma);
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
                    FazaClass.Graph_ATAN(img, x0_end, x1_end, y0_end, y1_end, n, fz, Gamma);
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
            OpenGLForm newForm = new OpenGLForm();
            foreach(Point3D currentPoint in newList)
            {
                newForm.addPoint(new Point3D(currentPoint.x, currentPoint.y, currentPoint.z));
            }
            newForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void imageProcessed(Bitmap newImage)
        {
            FazaClass.imageProcessed -= imageProcessed;
            SetControlPropertyThreadSafe(pictureBox01, "Image", newImage);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void captureImage(object sender, EventArgs e)
        {
            ShooterSingleton.imageCaptured += imageTaken;
            ShooterSingleton.getImage();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Cadr8ToolStripMenuItem_Click(object sender, EventArgs e)  
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
            if (pictureBox01.Image == null)
            {
                return;
            }

            if (cursorMode == 1)
            {
                upPoint = new Point(e.X, e.Y);
            }

            Graphics graphics = Graphics.FromImage(pictureBox01.Image);

            Pen p = new Pen(Color.Black, 4);
            graphics.DrawLine(p, downPoint, upPoint);

            pictureBox01.Invalidate();
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
            this.pictureBox01.Size = new Size(this.panel1.Size.Width - 44, this.panel1.Size.Height - 36);
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
                pictureBox01.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            }
            else if (scaleMode == 1)
            {
                pictureBox01.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            }

            relayout();
            pictureBox01.Invalidate();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            Color pixelcolor;
            Color fillColor = Color.Black;

            for(int i = 0; i < pictureBox01.Image.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox01.Image.Size.Height; j++)
                {
                    pixelcolor = ((Bitmap)(pictureBox01.Image)).GetPixel(i, j);
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
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Rectangle neededRect = ImageHelper.determineImageRect((Bitmap)pictureBox01.Image);

            Bitmap target = new Bitmap(neededRect.Width, neededRect.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(pictureBox01.Image, new Rectangle(0, 0, target.Width, target.Height), neededRect, GraphicsUnit.Pixel);
            }
            pictureBox01.Image = target;

            if (pictureBox1.Image != null)
            {
                target = new Bitmap(neededRect.Width, neededRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(pictureBox1.Image, new Rectangle(0, 0, target.Width, target.Height), neededRect, GraphicsUnit.Pixel);
                }
                pictureBox1.Image = target;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (pictureBox2.Image != null)
            {
                target = new Bitmap(neededRect.Width, neededRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(pictureBox2.Image, new Rectangle(0, 0, target.Width, target.Height), neededRect, GraphicsUnit.Pixel);
                }
                pictureBox2.Image = target;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (pictureBox3.Image != null)
            {
                target = new Bitmap(neededRect.Width, neededRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(pictureBox3.Image, new Rectangle(0, 0, target.Width, target.Height), neededRect, GraphicsUnit.Pixel);
                }
                pictureBox3.Image = target;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (pictureBox4.Image != null)
            {
                target = new Bitmap(neededRect.Width, neededRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(pictureBox4.Image, new Rectangle(0, 0, target.Width, target.Height), neededRect, GraphicsUnit.Pixel);
                }
                pictureBox4.Image = target;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

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

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void таблицаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DIALOG_CHINA(this.pi2_Click);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void таблица2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DIALOG_CHINA(this.pi2_Click2);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void pi2_Click2(object sender, EventArgs e)
        {
            img[0] = pictureBox1.Image;
            img[1] = pictureBox2.Image;
            img[2] = pictureBox3.Image;
            img[3] = pictureBox4.Image;

            int rb_int = 0;
            string strN1 = " ", strN2 = " ";
            strN1 = tb1.Text;
            strN2 = tb2.Text;
            if (tb3.Text != "") NDiag = Convert.ToInt32(tb3.Text);
            N_sin = Convert.ToDouble(tb1.Text);
            N2_sin = Convert.ToDouble(tb2.Text);
            pr_obr = Convert.ToInt32(tb4.Text);
            if (rb3.Checked) rb_int = 1; else rb_int = 0;                                                // По форме 3 кадра
            Pi_Class1.pi2_frml2(img, pictureBox01, strN1, strN2, NDiag, p, x0_end, x1_end, y0_end, y1_end, rb_int, pr_obr, pictureBox9, pictureBox10);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button4_Click_1(object sender, EventArgs e)
        {
            List<Point> previousBorderPoints = new List<Point>();

            if ((pictureBox01.Image == null) || (pictureBox1.Image == null))
            {
                MessageBox.Show("Сначала загрузите изображение");
                return;
            }

            Bitmap copyOfImage = new Bitmap((Bitmap)pictureBox01.Image);
            BitmapData imageData = ImageProcessor.getBitmapData(copyOfImage);
            Color previousColor = Color.White;


            List<Point> startPoints = new List<Point>();
            List<Point> endPoints = new List<Point>();

            for (int x = 0; x < copyOfImage.Width; x++)
            {
                List<Point> tempBorders = new List<Point>();

                for (int y = 0; y < copyOfImage.Height; y++)
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

                        if ((Math.Abs(closestPoint.Y - newBorderPoint.Y) > 1) && ((Math.Abs(closestPoint.Y - newBorderPoint.Y) < 20)))
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

            for (int x = 0; x < pictureBox01.Image.Width; x++)
            {
                for (int y = 0; y < pictureBox01.Image.Height; y++)
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
// ------------------------------  Сглаживание
        private void сглаживаниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.filt_Click);
        }
        private void filt_Click(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
            FiltrClass.Filt_121(pictureBox01, progressBar1, k_filt);
            //Razmer(w1, h1);
            f_filt.Close();
        }
// -------------------------------  Медианный
        private void медианныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.filt_median_Click);
        }
        private void filt_median_Click(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
            FiltrClass.Filt_Mediana(pictureBox01, progressBar1, k_filt);
            f_filt.Close();
        }

        public void FiltDialog(EventHandler functionPointer)
        {
            int max_x = 120, max_y = 100;

            f_filt = new Form();
            f_filt.Size = new Size(max_x, max_y + 36);
            f_filt.StartPosition = FormStartPosition.Manual;
            Point p = this.Location;
            p.Offset(40, 165);
            f_filt.Location = p;

            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(8, 10);
            label1.Text = "k = 1,2,3 ... :";

            tb1_filt = new TextBox();
            tb1_filt.Location = new System.Drawing.Point(8, 30);
            tb1_filt.Size = new System.Drawing.Size(60, 20);
            tb1_filt.Text = k_filt.ToString();

            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(8, 60);
            b1.Text = "ok";
            b1.Size = new System.Drawing.Size(100, 30);
            b1.Click += new System.EventHandler(functionPointer);

            f_filt.Controls.Add(label1);
            f_filt.Controls.Add(tb1_filt);
            f_filt.Controls.Add(b1);

            f_filt.Show();

        }

            
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
