using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



using System.Threading;

namespace rab1
{

    public delegate void FunctionPointer(object sender, EventArgs eventArgs);

    public partial class Form1 : Form
    {
        

       

        int camera_height = 600;
        int camera_width = 800;

        bool seriesOfPhotosInProgress = false;
        int numberOgPhoto = 0;

       

        
        int[] h = new int[8];
        int[] w = new int[8];
        Image[] img = new Image[8];

        int x_Scroll = 0;                       // Scrol = 0
        int y_Scroll = 0;

        int count_scale = 0;                     // Текущий масштаб

        Double Gamma = 1.0;                      // Гамма коррекция


        TextBox tb1, tb2, tb3, tb4, tb5, tb12;             // Для ввода периода синусоиды ----------------------------------------------
        
        
        double N_sin  = 167;                          // число синусоид 1
        double N2_sin = 241;                          // число синусоид 2
        int NDiag = 9;
        double N_fz = 0, N_fz2 = 90, N_fz3 = 180, N_fz4 = 270;   // начальная фаза в градусах

        int XY=1;                                    // Направление полос
        int MASK;                                    // Маска для синусоиды
        Form f2, f_sin;
        PictureBox pc1;
        Point p;
        
        GroupBox groupbx1, groupbx2;
        RadioButton rb1, rb2, rb11, rb22;
        RadioButton rbf0, rbf1, rbf2, rbf3, rbf4, rbf5, rbf6, rbf7, rbf8;
        //RadioButton rb;                            // ----------------------------------------------------------------------------
        CheckBox rb3;


        Form f_filt;                             // Для Фильтрации
        TextBox tb1_filt, tb2_filt, tb3_filt;
        int k_filt = 1;
       

        Form f_8bit;                             // Для 8 bit
        TextBox tb1_8bit;
        int k_8bit=8;

                                                     
        int Cntr_end = 1;                               // Ограничение выключено
        int reggrf = -1;                                // 0 - график не рисуется  1 - график рисуется -1 - маркер

        int x0_end = 0, y0_end = 0;
        int x1_end = 0, y1_end = 0;
        Form f_end;
        TextBox tb0_end, tb1_end, tb2_end, tb3_end, tb4_end, tb5_end;

       
      
        string string_dialog = "D:\\Студенты\\Эксперимент";       
        int reg_image = 0;                            // Номер изображения (0-7)

        int index_img = 0;
      
/*
        public static Form1 SelfRef
        {
            get;
            set;
        }
*/

        ImageGetter imageGetter;
   // -------------------------------------------------------------------------------------------------------------------
        public Form1()                                 // Задание начальных значений
        {
            InitializeComponent();

            int w1 = 800, h1 = 600;
            Color c=Color.FromArgb(0, 0, 0);
            Bitmap bmp2 = new Bitmap(w1, h1);
            for (int i = 0; i < w1; i++)    for (int j = 0; j < h1; j++) {   bmp2.SetPixel(i, j, c); }

            for (int i = 0; i < 8; i++) { img[i] = bmp2;        }                            // Image.FromFile("D:\\34.jpg"); }
            for (int i = 0; i < 8; i++) { h[i] = h1; w[i] = w1; }


           // _manager = new FrameworkManager();
            //_manager.CameraAdded += this.HandleCameraAdded;
            pictureBox01.Image = bmp2;


            imageGetter = new ImageGetter();
            imageGetter.imageReceived += imageTaken;

        }

        private void imageTaken(Image newImage)
        {
            if (seriesOfPhotosInProgress == false)
            {
                pictureBox1.Image = newImage;
            }
            else
            {
                numberOgPhoto++;

                switch (numberOgPhoto)
                {
                    case 1: 
                        pictureBox1.Image = newImage;
                        imageGetter.getImage();
                        break;
                    case 2: 
                        pictureBox2.Image = newImage;
                        imageGetter.getImage();
                        break;
                    case 3: 
                        pictureBox3.Image = newImage;
                        imageGetter.getImage();
                        break;
                    case 4: 
                        pictureBox4.Image = newImage; 


                        imageGetter.getImage();
                        break;
                    case 5: 
                        pictureBox5.Image = newImage; 
                        imageGetter.getImage();
                        break;
                    case 6: 
                        pictureBox6.Image = newImage;
                        imageGetter.getImage();
                        break;
                    case 7: 
                        pictureBox7.Image = newImage;
                        imageGetter.getImage();
                        break;
                    case 8: 
                        pictureBox8.Image = img[index_img];
                        imageGetter.getImage();
                        seriesOfPhotosInProgress = false;
                        numberOgPhoto = 0;
                        break;
                }

            }
        }

        // ------------------------------------------------------------------------------------------------ 
        public void Razmer(int w1, int h1)       { label3.Text = "Размер: " + w1 + " X " + h1; }
        // ------------------------------------------------------------------------------------------------ panel1_Scroll
        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {            
            x_Scroll = this.panel1.AutoScrollPosition.X;
            y_Scroll = this.panel1.AutoScrollPosition.Y;
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        private int X_MouseMove(MouseEventArgs e) { return (e.X);}   // + x_Scroll); }
        private int Y_MouseMove(MouseEventArgs e) { return (e.Y); }  // + y_Scroll); }
        
        // ------------------------------------------------------------------------------------------------------------------------------------------ Движение мыши
      
        private void pictureBox01_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics line_cursor;
            

            int x,y;
            int s = (int)Math.Pow(2, count_scale);
            x = X_MouseMove(e); x *= s;
            y = Y_MouseMove(e); y *= s;
            label2.Text = "X:" + x.ToString();
            label4.Text = "Y:" + y.ToString();
            label15.Text = "S:" + count_scale.ToString();
            Pen line_pen = new Pen(Color.Blue, 1);

            line_cursor = Graphics.FromHwndInternal(pictureBox01.Handle);

            if (reggrf == 1)    // Если рисование графика
            {
                pictureBox01.Refresh();          
                line_cursor.DrawLine(line_pen, 0, e.Y, pictureBox01.Width, e.Y);
                line_cursor.DrawLine(line_pen, e.X, 0, e.X, pictureBox01.Height);
            }
            else
            {
               // line_pen.Close();
               // line_cursor.hide();
               // pictureBox01.Refresh();     
            }

                line_pen.Dispose();
                line_cursor.Dispose();
                
        }
        // -------------------------------------------------------------------------------------------------------------------------- Режим график
        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            reggrf = 1; Cntr_end = 1;
        }
        // -------------------------------------------------------------------------------------------------------------------------- Задание области вырезания
        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            reggrf = 0; Cntr_end = 0; 
        }
        // --------------------------------------------------------------------------------------------------------------------------- Режим маркер
        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            reggrf = -1; Cntr_end = 1;
        }
        // --------------------------------------------------------------------------------------------------------------------------- Выход мышки за пределы pictureBox01
        private void pictureBox01_MouseLeave(object sender, EventArgs e)
        {
            pictureBox01.Refresh();    

        }
        // --------------------------------------------------------------------------------------------------------------------------- Двойной щелчок мыши
        private void pictureBox01_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            reggrf = 0; Cntr_end = 0; x0_end = 0; y0_end = 0; x1_end = 0; y1_end = 0;
            pictureBox01.Refresh();    
        }
        // --------------------------------------------------------------------------------------------------------------------------- Рисование графика по клику на мышку

        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {

            int w1 = pictureBox01.Image.Width;
            int h1 = pictureBox01.Image.Height;
            int[] bufr = new int[w1];
            int[] bufyr = new int[h1];
            int[] bufg = new int[w1];
            int[] bufyg = new int[h1];
            int[] bufb = new int[w1];
            int[] bufyb = new int[h1];
            Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);
           

            Color c;

            int ii = 0;
            int s = (int)Math.Pow(2, count_scale);
            int x1 = X_MouseMove(e);
            int y1 = Y_MouseMove(e);
            int x = x1 * s;
            int y = y1 * s;
            label2.Text = "X:" + x.ToString();
            label4.Text = "Y:" + y.ToString();
            //MessageBox.Show(x.ToString() + " : " + y.ToString());      
            if (x1 >= w1 || y1 >= h1) { return; }
           
           // Point pnt = new Point(e.X, e.Y);

            if (reggrf == -1)                                                          //  Маркер
            {
                
                c = bmp1.GetPixel(x1, y1);
                ii = c.R; label1.Text = "R:" + ii.ToString();
                ii = c.G; label5.Text = "G:" + ii.ToString();
                ii = c.B; label6.Text = "B:" + ii.ToString();
            }
           
            if (reggrf == 1)                                                           // рисование графика
            {
                c = bmp1.GetPixel(x1, y1);
                ii = c.R; label1.Text = "R:" + ii.ToString();
                ii = c.G; label5.Text = "G:" + ii.ToString();
                ii = c.B; label6.Text = "B:" + ii.ToString();

                for (int i = 0; i < w1; i++) { c = bmp1.GetPixel(i, y1); bufr[i] = c.R; bufg[i] = c.G; bufb[i] = c.B; }
                for (int i = 0; i < h1; i++) { c = bmp1.GetPixel(x1, i); bufyr[i] = c.R; bufyg[i] = c.G; bufyb[i] = c.B; }
                //Pen p = new Pen(Color.Red, 1);
                // GraphClass1.grfk(w1, h1, x, y, buf, bufy, p);
                Form fo = new GraphForm(x1, y1, w1, h1, bufr, bufyr, bufg, bufyg, bufb, bufyb);
                fo.Show();
               fo.StartPosition = FormStartPosition.Manual;
               Point p = this.Location;
               p.Offset(840, 105);
               fo.Location = p;
               fo.Show();

               // System.Windows.Forms.Cursor.Position = pnt;
               // Cursor.Show();
            }
            Graphics line_cursor;      
            Pen line_pen = new Pen(Color.Blue, 1);
            line_cursor = Graphics.FromHwndInternal(pictureBox01.Handle);

            if (reggrf == 0 && Cntr_end == 0)                                                                                // Если включена кнопка область
            {
                if (x0_end != 0 || x1_end != 0 || y0_end != 0 || y1_end != 0) { pictureBox01.Refresh(); line_cursor.DrawRectangle(line_pen, x0_end / s, y0_end / s, (x1_end - x0_end) / s, (y1_end - y0_end) / s); }
                     else
                            {
                              x0_end = x; y0_end = y; Cntr_end = -1; MessageBox.Show(" Левый верхний угол:" + x0_end.ToString() + " : " + y0_end.ToString());
                            }
                 return;
            }     
            if (reggrf == 0 && Cntr_end != 0)       
                           {
                                  x1_end = x; y1_end = y; Cntr_end = 0; MessageBox.Show(" Правый верхний угол:" + x1_end.ToString() + " : " + y1_end.ToString());

                                  pictureBox01.Refresh();
                                  line_cursor.DrawRectangle(line_pen, x0_end/s, y0_end/s, (x1_end - x0_end)/s, (y1_end - y0_end)/s);
                                  //line_cursor.DrawLine(line_pen, e.X, 0, e.X, pictureBox01.Height);                     

                           }
                    
            
            line_pen.Dispose();
            line_cursor.Dispose();
        }


// R -------------------------------------------------------------------------------------- R
        private void button2_Click(object sender, EventArgs e)
        {
            Color c;
            int r1;
            int w1 = pictureBox01.Image.Width ;
            int h1 = pictureBox01.Image.Height;
          
            Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);
           
            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = w1;
            progressBar1.Value = 1;
            progressBar1.Step = 1;
         
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c = bmp1.GetPixel(i, j);
                    r1 = c.R;
                    bmp1.SetPixel(i, j, Color.FromArgb(r1, 0, 0));
                }
                progressBar1.PerformStep();
            }
            pictureBox01.Image = bmp1;
        }
// G -------------------------------------------------------------------------------------- G
       private void button4_Click(object sender, EventArgs e)
        {
            Color c;
            int r1;
            int w1 = pictureBox01.Image.Width;
            int h1 = pictureBox01.Image.Height;
           
            Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = w1;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c = bmp1.GetPixel(i, j);
                    r1 = c.G;
                    bmp1.SetPixel(i, j, Color.FromArgb(0, r1, 0));
                }
                progressBar1.PerformStep();
            }
            pictureBox01.Image = bmp1;
        }
// B
        private void button5_Click(object sender, EventArgs e)
        {
            Color c;
            int r1;
            int w1 = pictureBox01.Image.Width;
            int h1 = pictureBox01.Image.Height;
          
            Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = w1;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c = bmp1.GetPixel(i, j);
                    r1 = c.B;
                    bmp1.SetPixel(i, j, Color.FromArgb(0, 0, r1));
                }
                progressBar1.PerformStep();
            }
            pictureBox01.Image = bmp1;
        }
// W
        private void button6_Click(object sender, EventArgs e)
        {
            Color c;
            int r1;
            int r, g, b;
         
            int w1 = pictureBox01.Image.Width;
            int h1 = pictureBox01.Image.Height;
           
            Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = w1;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c = bmp1.GetPixel(i, j);
                    r = c.R; g = c.G; b = c.B;
                    r1 = (r + b + g)/3;                                   
                    bmp1.SetPixel(i, j, Color.FromArgb(r1, r1, r1));
                }
                progressBar1.PerformStep();
            }
            pictureBox01.Image = bmp1;
            
        }


 // ------------------------------------------------------------------------------------ Радио кнопки (номер изображения)
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            reg_image = 0;
          
            RadioButton rb = sender as RadioButton;

            if (rb == radioButton1) { reg_image = 0; }
            if (rb == radioButton2) { reg_image = 1;  }
            if (rb == radioButton3) { reg_image = 2; }
            if (rb == radioButton4) { reg_image = 3; }
            if (rb == radioButton5) { reg_image = 4; }
            if (rb == radioButton6) { reg_image = 5; }
            if (rb == radioButton7) { reg_image = 6; }
            if (rb == radioButton8) { reg_image = 7; }

            //label2.Text = "R:" + reg_image.ToString();
            
            
            int w1 = w[reg_image];
            int h1 = h[reg_image];
            Bitmap bmp1 = new Bitmap(img[reg_image], w1, h1);
           

            switch (reg_image)
            {
                case 0: pictureBox1.Image = bmp1; break;
                case 1: pictureBox2.Image = bmp1; break;
                case 2: pictureBox3.Image = bmp1; break;
                case 3: pictureBox4.Image = bmp1; break;
                case 4: pictureBox5.Image = bmp1; break;
                case 5: pictureBox6.Image = bmp1; break;
                case 6: pictureBox7.Image = bmp1; break;
                case 7: pictureBox8.Image = bmp1; break;
            }

            Razmer(w1, h1);
        }

        // ---------------------------------------------------------------------------------- Записать из мини окон в 1 окно  ->
        private void button3_Click(object sender, EventArgs e)
        {
            count_scale = 0;    // Масштаб
            label15.Text = "S:" + count_scale.ToString();

            int w1 = w[reg_image];
            int h1 = h[reg_image];
            Razmer(w1, h1); 
            Bitmap bmp1 = new Bitmap(img[reg_image], w1, h1);
            pictureBox01.Size = new System.Drawing.Size(w1, h1);
            pictureBox01.Image = bmp1;

        }

     
        // -------------------------------------------------------------------------------- Из 1 в мини окно   ->
        private void button12_Click(object sender, EventArgs e)
        {
            {
                w[reg_image] = pictureBox01.Image.Width;
                h[reg_image] = pictureBox01.Image.Height;       
                           
                img[reg_image] = pictureBox01.Image;
               
                switch (reg_image)
                {
                    case 0: pictureBox1.Image = img[reg_image]; break;
                    case 1: pictureBox2.Image = img[reg_image]; break;
                    case 2: pictureBox3.Image = img[reg_image]; break;
                    case 3: pictureBox4.Image = img[reg_image]; break;
                    case 4: pictureBox5.Image = img[reg_image]; break;
                    case 5: pictureBox6.Image = img[reg_image]; break;
                    case 6: pictureBox7.Image = img[reg_image]; break;
                    case 7: pictureBox8.Image = img[reg_image]; break;
                }
                Razmer(w[reg_image], h[reg_image]); 
                
            }
        }



        // -------------------------------------------------------------------------------------------------------- Загрузить в  pictureBox1  . . . 8   
        private void ZGR_File(int i)
        {
            System.Windows.Forms.OpenFileDialog dialog1 = new System.Windows.Forms.OpenFileDialog();
            dialog1.InitialDirectory = string_dialog;
            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {                    
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;

                    img[i] = pictureBox01.Image;               
                    switch (i)
                    {
                        case 0: pictureBox1.Image = Image.FromFile(dialog1.FileName); w[i] = pictureBox1.Image.Width; h[i] = pictureBox1.Image.Height; img[i] = pictureBox1.Image; break;
                        case 1: pictureBox2.Image = Image.FromFile(dialog1.FileName); w[i] = pictureBox2.Image.Width; h[i] = pictureBox1.Image.Height; img[i] = pictureBox2.Image; break;
                        case 2: pictureBox3.Image = Image.FromFile(dialog1.FileName); w[i] = pictureBox3.Image.Width; h[i] = pictureBox1.Image.Height; img[i] = pictureBox3.Image; break;
                        case 3: pictureBox4.Image = Image.FromFile(dialog1.FileName); w[i] = pictureBox4.Image.Width; h[i] = pictureBox1.Image.Height; img[i] = pictureBox4.Image; break;
                        case 4: pictureBox5.Image = Image.FromFile(dialog1.FileName); w[i] = pictureBox5.Image.Width; h[i] = pictureBox1.Image.Height; img[i] = pictureBox5.Image; break;
                        case 5: pictureBox6.Image = Image.FromFile(dialog1.FileName); w[i] = pictureBox6.Image.Width; h[i] = pictureBox1.Image.Height; img[i] = pictureBox6.Image; break;
                        case 6: pictureBox7.Image = Image.FromFile(dialog1.FileName); w[i] = pictureBox7.Image.Width; h[i] = pictureBox1.Image.Height; img[i] = pictureBox7.Image; break;
                        case 7: pictureBox8.Image = Image.FromFile(dialog1.FileName); w[i] = pictureBox8.Image.Width; h[i] = pictureBox1.Image.Height; img[i] = pictureBox8.Image; break;
                    }
                    Razmer(w[i], h[i]);     // Вывод размера                
                                                                                       
                }
                catch (Exception ex) { MessageBox.Show(" Ошибка " + ex.Message); }
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(0); }
        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(1); }
        private void pictureBox3_MouseClick_1(object sender, MouseEventArgs e) { ZGR_File(2); }
        private void pictureBox4_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(3); }
        private void pictureBox5_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(4); }
        private void pictureBox6_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(5); }
        private void pictureBox7_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(6); }
        private void pictureBox8_MouseClick(object sender, MouseEventArgs e)   { ZGR_File(7); }

        // -------------------------------------------------------------------------------------------------------- Загрузить в  pictureBox01       
        private void ZGRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog1 = new System.Windows.Forms.OpenFileDialog();
            dialog1.InitialDirectory = string_dialog;
            //dialog1.Filter = "*.jpg";
            count_scale = 0;

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {                    
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;
                    
                    pictureBox01.Image = Image.FromFile(dialog1.FileName);

                    int w1 = pictureBox01.Image.Width;
                    int h1 = pictureBox01.Image.Height;
                    pictureBox01.Size = new System.Drawing.Size(w1, h1);
                    pictureBox01.Show();
                    Razmer(w1, h1);                                                        // Вывод размера
                }
                catch (Exception ex) { MessageBox.Show(" Ошибка " + ex.Message); }
            }
        }






        private string SaveString(string string_dialog, int k)
        {
               
                string strk = k.ToString();

                string string_rab = string_dialog;
                if (string_dialog.Contains("1.")) { string_rab = string_dialog.Replace("1.", strk+"."); }
               

                return string_rab;

        }
        private string SaveString8(string string_dialog, int k)
        {

            string strk = k.ToString();

            string string_rab = string_dialog;
            if (string_dialog.Contains(".")) { string_rab = string_dialog.Replace(".",strk+"."); }
           

            return string_rab;

        }
        // ----------------------------------------------------------------------------------------------------------------------- Загрузить 8 файлов
        private void Save8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog1 = new System.Windows.Forms.OpenFileDialog();
            dialog1.InitialDirectory = string_dialog;
            //dialog1.Filter = "*.jpg";
            count_scale = 0;

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
                        //MessageBox.Show(" Новое имя " + str);

                        img[i] = Image.FromFile(str);
                        switch (i)
                        {
                            case 0: pictureBox1.Image = img[i]; pictureBox1.Show(); break;
                            case 1: pictureBox2.Image = img[i]; pictureBox2.Show(); break;
                            case 2: pictureBox3.Image = img[i]; pictureBox3.Show(); break;
                            case 3: pictureBox4.Image = img[i]; pictureBox4.Show(); break;
                            case 4: pictureBox5.Image = img[i]; pictureBox5.Show(); break;
                            case 5: pictureBox6.Image = img[i]; pictureBox6.Show(); break;
                            case 6: pictureBox7.Image = img[i]; pictureBox7.Show(); break;
                            case 7: pictureBox8.Image = img[i]; pictureBox8.Show(); break;
                        }

                    }                                                  
                }
                catch (Exception ex) { MessageBox.Show(" Ошибка " + ex.Message); }
            }
        }

// ----------------------------------------------------------------------------------------------------- Сохранить
        private void SAVEToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            System.Windows.Forms.SaveFileDialog dialog1 = new System.Windows.Forms.SaveFileDialog();
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
                catch (Exception ex) { MessageBox.Show(" Ошибка при записи файла " + ex.Message); }
            }                          

        }
        private void сохранить8КадровToolStripMenuItem_Click(object sender, EventArgs e) // ------------ Сохранить 8 кадров
        {
            
            System.Windows.Forms.SaveFileDialog dialog1 = new System.Windows.Forms.SaveFileDialog();
            dialog1.InitialDirectory = string_dialog;
            dialog1.Filter = "Bitmap(*.bmp)|*.bmp";

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    
                    //pictureBox01.Image.Save(dialog1.FileName);
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;
                    string str = string_dialog;
                    for (int i = 0; i < 8; i++)
                    {
                        str = SaveString8(string_dialog, i + 1);
                        img[i].Save(str, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                  
                }
                catch (Exception ex) { MessageBox.Show(" Ошибка при записи файла " + ex.Message); }
            }       

        }


        //-------------------------------------------------------------------------------------------------------- Очистить
        private void CLOSEToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Color c = Color.FromArgb(0, 0, 0);
            int w1 = 800;
            int h1 = 600;
            Bitmap bmp2 = new Bitmap(w1, h1);

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = w1;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++) { bmp2.SetPixel(i, j, c); }
                progressBar1.PerformStep();
            }

            //img[reg_image] = bmp2;
            pictureBox01.Image = bmp2;
        }

       
        // --------------------------------------------------------------------------------------------------------------- Фильтрация

        private void F1ToolStripMenuItem_Click(object sender, EventArgs e)                    // -------------- Сглаживание 121
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
        // --------------------------------------------------------------------------------------------------------------- Медианный 
        private void MEDIANAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.filt_median_Click);
        }
        private void filt_median_Click(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);           
            FiltrClass.Filt_Mediana(pictureBox01, progressBar1, k_filt);
            f_filt.Close();
        }
        // ------------------------------------------------------------------------------------------------------------- 2 производная (Собель)       
        private void SobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.filt_Sobel_Click);
        }

        private void filt_Sobel_Click(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
            FiltrClass.Filt_Sobel(pictureBox01, progressBar1, k_filt);
            f_filt.Close();
        }
        // ----------------------------------------------------------------------------------------------------------- Выделение перепадов
        private void bWToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.BW);
        }


        private void BW(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
            FiltrClass.Filt_BW(pictureBox01, progressBar1, k_filt);
            f_filt.Close();
        }
        // --------------------------------------------------------------------------------------------

        public void FiltDialog(EventHandler functionPointer)
        {
            int max_x = 120, max_y = 100;

            f_filt = new Form();
            f_filt.Size = new Size(max_x, max_y + 36);
            f_filt.StartPosition = FormStartPosition.Manual;
            Point p = this.Location;
            p.Offset(40, 105);
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
    


// ---------------------------------------------------------------------------------------Вырезать область
        private void VRZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int max_x = 120, max_y = 140;
            f_end = new Form();
            f_end.StartPosition = FormStartPosition.Manual;
            f_end.Location = new System.Drawing.Point(585, 250);
            f_end.Size = new Size(max_x, max_y + 36);


            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(8, 10);
            label1.Size = new System.Drawing.Size(40, 20);
            label1.Text = "x0:";

            tb0_end = new TextBox();
            tb0_end.Location = new System.Drawing.Point(48, 10);
            tb0_end.Size = new System.Drawing.Size(60, 20);
            tb0_end.Text = x0_end.ToString();

            Label label2 = new Label();
            label2.Location = new System.Drawing.Point(8, 30);
            label2.Size = new System.Drawing.Size(40, 20);
            label2.Text = "y0:";

            tb1_end = new TextBox();
            tb1_end.Location = new System.Drawing.Point(48, 30);
            tb1_end.Size = new System.Drawing.Size(60, 20);
            tb1_end.Text = y0_end.ToString();

            Label label3 = new Label();
            label3.Location = new System.Drawing.Point(8, 50);
            label3.Size = new System.Drawing.Size(40, 20);
            label3.Text = "x1:";

            tb2_end = new TextBox();
            tb2_end.Location = new System.Drawing.Point(48, 50);
            tb2_end.Size = new System.Drawing.Size(60, 20);
            tb2_end.Text = x1_end.ToString();

            Label label4 = new Label();
            label4.Location = new System.Drawing.Point(8, 70);
            label4.Size = new System.Drawing.Size(40, 20);
            label4.Text = "y1:";

            tb3_end = new TextBox();
            tb3_end.Location = new System.Drawing.Point(48, 70);
            tb3_end.Size = new System.Drawing.Size(60, 20);
            tb3_end.Text = y1_end.ToString();


            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(8, 110);
            b1.Text = "ok";
            b1.Size = new System.Drawing.Size(100, 30);
            b1.Click += new System.EventHandler(end_Click);

            f_end.Controls.Add(label1);
            f_end.Controls.Add(label2);
            f_end.Controls.Add(label3);
            f_end.Controls.Add(label4);

            f_end.Controls.Add(tb0_end);
            f_end.Controls.Add(tb1_end);
            f_end.Controls.Add(tb2_end);
            f_end.Controls.Add(tb3_end);

            f_end.Controls.Add(b1);

            f_end.Show();
        }

        private void end_Click(object sender, EventArgs e)    // ОК для вырезания
        {
            if (tb0_end.Text != "") x0_end = Convert.ToInt32(tb0_end.Text);
            if (tb1_end.Text != "") y0_end = Convert.ToInt32(tb1_end.Text);
            if (tb2_end.Text != "") x1_end = Convert.ToInt32(tb2_end.Text);
            if (tb3_end.Text != "") y1_end = Convert.ToInt32(tb3_end.Text);

            int w1 = pictureBox01.Image.Width;
            int h1 = pictureBox01.Image.Height;

            Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);
            Bitmap bmp2 = new Bitmap(x1_end - x0_end, y1_end - y0_end);
            Color c;
            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = x1_end - x0_end;
            progressBar1.Value = 1;
            progressBar1.Step = 1;
            MessageBox.Show(" Левый верхний угол:" + x0_end.ToString() + " : " + y0_end.ToString() + " Правый верхний угол:" + x1_end.ToString() + " : " + y1_end.ToString());
            int s = (int)Math.Pow(2, count_scale);
            int x0 = x0_end / s;
            int y0 = y0_end / s;
            int x1 = x1_end / s;
            int y1 = y1_end / s;


            int x = x1 * s;
            int y = y1 * s;
            
            for (int i = x0, i1 = 0; i < x1; i++, i1++)
            {
                for (int j = y0, j1 = 0; j < y1; j++, j1++) { c = bmp1.GetPixel(i, j); bmp2.SetPixel(i1, j1, c); }
                progressBar1.PerformStep();
            }
            w1 = x1 - x0; //w[reg_image] = w1;
            h1 = y1 - y0; //h[reg_image] = h1;

            Razmer(w1, h1);
            pictureBox01.Size = new System.Drawing.Size(w1, h1);
            pictureBox01.Image = bmp2;
            f_end.Close();
        }
//-------------------------------------------------------------------------------------- По трем точкам строится плоскость и вычитается из массива Z[i,j] ---------------------------------------
        int x0_n = 1256,  y0_n = 480;  // 1 точка
        int x1_n =  104,  y1_n = 318;  // 1 точка
        int x2_n =  102,  y2_n = 710;  // 1 точка
        
        private void убратьНаклонToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int max_x = 240, max_y = 110, dx=110;
            f_end = new Form();
            f_end.StartPosition = FormStartPosition.Manual;
            f_end.Location = new System.Drawing.Point(300, 300);
            f_end.Size = new Size(max_x, max_y + 36);


            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(8, 10);
            label1.Size = new System.Drawing.Size(40, 20);
            label1.Text = "x0:";

            tb0_end = new TextBox();
            tb0_end.Location = new System.Drawing.Point(48, 10);
            tb0_end.Size = new System.Drawing.Size(60, 20);
            tb0_end.Text = x0_n.ToString();

            Label label2 = new Label();
            label2.Location = new System.Drawing.Point(8+dx, 10);
            label2.Size = new System.Drawing.Size(40, 20);
            label2.Text = "y0:";

            tb1_end = new TextBox();
            tb1_end.Location = new System.Drawing.Point(48+dx, 10);
            tb1_end.Size = new System.Drawing.Size(60, 20);
            tb1_end.Text = y0_n.ToString();

            Label label3 = new Label();
            label3.Location = new System.Drawing.Point(8, 30);
            label3.Size = new System.Drawing.Size(40, 20);
            label3.Text = "x1:";

            tb2_end = new TextBox();
            tb2_end.Location = new System.Drawing.Point(48, 30);
            tb2_end.Size = new System.Drawing.Size(60, 20);
            tb2_end.Text = x1_n.ToString();

            Label label4 = new Label();
            label4.Location = new System.Drawing.Point(8+dx, 30);
            label4.Size = new System.Drawing.Size(40, 20);
            label4.Text = "y1:";

            tb3_end = new TextBox();
            tb3_end.Location = new System.Drawing.Point(48+dx, 30);
            tb3_end.Size = new System.Drawing.Size(60, 20);
            tb3_end.Text = y1_n.ToString();

            Label label5 = new Label();
            label5.Location = new System.Drawing.Point(8, 50);
            label5.Size = new System.Drawing.Size(40, 20);
            label5.Text = "x2:";

            tb4_end = new TextBox();
            tb4_end.Location = new System.Drawing.Point(48, 50);
            tb4_end.Size = new System.Drawing.Size(60, 20);
            tb4_end.Text = x2_n.ToString();

            Label label6 = new Label();
            label6.Location = new System.Drawing.Point(8 + dx, 50);
            label6.Size = new System.Drawing.Size(40, 20);
            label6.Text = "y2:";

            tb5_end = new TextBox();
            tb5_end.Location = new System.Drawing.Point(48 + dx, 50);
            tb5_end.Size = new System.Drawing.Size(60, 20);
            tb5_end.Text = y2_n.ToString();


            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(28, 80);
            b1.Text = "ok";
            b1.Size = new System.Drawing.Size(200, 30);
            b1.Click += new System.EventHandler(nkl_Click);

            f_end.Controls.Add(label1);
            f_end.Controls.Add(label2);
            f_end.Controls.Add(label3);
            f_end.Controls.Add(label4);
            f_end.Controls.Add(label5);
            f_end.Controls.Add(label6);


            f_end.Controls.Add(tb0_end);
            f_end.Controls.Add(tb1_end);
            f_end.Controls.Add(tb2_end);
            f_end.Controls.Add(tb3_end);
            f_end.Controls.Add(tb4_end);
            f_end.Controls.Add(tb5_end);


            f_end.Controls.Add(b1);

            f_end.Show();
        }

        private void nkl_Click(object sender, EventArgs e)    // ОК для вырезания
        {
            if (tb0_end.Text != "") x0_n = Convert.ToInt32(tb0_end.Text);
            if (tb1_end.Text != "") y0_n = Convert.ToInt32(tb1_end.Text);
            if (tb2_end.Text != "") x1_n = Convert.ToInt32(tb2_end.Text);
            if (tb3_end.Text != "") y1_n = Convert.ToInt32(tb3_end.Text);
            if (tb4_end.Text != "") x2_n = Convert.ToInt32(tb4_end.Text);
            if (tb5_end.Text != "") y2_n = Convert.ToInt32(tb5_end.Text);
            Pi_Class1.NKL(pictureBox01, progressBar1, x0_n, y0_n, x1_n, y1_n, x2_n, y2_n);
            
            f_end.Close();
        }
//-------------------------------------------------------------------------------------- Вырезать по контуру, который хранится в 3 кадре
        private void вырезатьПо3КадруToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int w = pictureBox01.Image.Width;
            int h = pictureBox01.Image.Height;

            Bitmap bmp1 = new Bitmap(pictureBox01.Image, w, h);
            Bitmap bmp3 = new Bitmap(pictureBox3.Image);

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = w;
            progressBar1.Value = 1;
            progressBar1.Step = 1;
            Color c;
            Color c0 = Color.FromArgb(0, 0, 0);
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++) { c = bmp3.GetPixel(i, j); if (c.R == 0)  bmp1.SetPixel(i, j, c0); }
                progressBar1.PerformStep();
            }
           
            pictureBox01.Image = bmp1;
        }     

// ------------------------------------------------------------------------------------ Объединение 8 битов
        private void bit8_Click(object sender, EventArgs e)
        {
            if (tb1_8bit.Text != "") k_8bit = Convert.ToInt32(tb1_8bit.Text);                    
            //Razmer(w1, h1);
            SinClass1.bit8(k_8bit, img, pictureBox01, progressBar1);
            f_8bit.Close();
        }

// -------------------------------------------------------------------------------------- Из 8 бит -> sin
        private void Bit8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int max_x = 160, max_y = 130;

            f_8bit = new Form();
            f_8bit.Size = new Size(max_x, max_y);
            f_8bit.StartPosition = FormStartPosition.Manual;
            Point p = this.Location;
            p.Offset(120, 105);
            f_8bit.Location = p;




            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(8, 10);
            label1.Size = new System.Drawing.Size(140, 20);
            label1.Text = "Число старших разрядов:";

            tb1_8bit = new TextBox();
            tb1_8bit.Location = new System.Drawing.Point(8, 30);
            tb1_8bit.Size = new System.Drawing.Size(140, 20);
            tb1_8bit.Text = k_8bit.ToString();

            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(8, 60);
            b1.Text = "ok";
            b1.Size = new System.Drawing.Size(140, 30);
            b1.Click += new System.EventHandler(bit8_Click);   // ===> bit8_Click

            f_8bit.Controls.Add(label1);
            f_8bit.Controls.Add(tb1_8bit);
            f_8bit.Controls.Add(b1);

            f_8bit.Show();
        }
        // -------------------------------------------------------------------------------------- Из 8 бит -> sin Суммирование
        private void bitSummToolStripMenuItem_Click(object sender, EventArgs e)
        {


            int max_x = 160, max_y = 130;

            f_8bit = new Form();
            f_8bit.Size = new Size(max_x, max_y);
            f_8bit.StartPosition = FormStartPosition.Manual;
            Point p = this.Location;
            p.Offset(120, 105);
            f_8bit.Location = p;

            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(8, 10);
            label1.Size = new System.Drawing.Size(140, 20);
            label1.Text = "Число старших разрядов:";

            tb1_8bit = new TextBox();
            tb1_8bit.Location = new System.Drawing.Point(8, 30);
            tb1_8bit.Size = new System.Drawing.Size(140, 20);
            tb1_8bit.Text = k_8bit.ToString();

            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(8, 60);
            b1.Text = "ok";
            b1.Size = new System.Drawing.Size(140, 30);
            b1.Click += new System.EventHandler(bit8sin_Click);                                  // ===> bit8sin_Click

            f_8bit.Controls.Add(label1);
            f_8bit.Controls.Add(tb1_8bit);
            f_8bit.Controls.Add(b1);

            f_8bit.Show();
            //f_8bit.Close();
        }

// ------------------------------------------------------------------------------------------------------ 8 bit Cуммирование
        private void bit8sin_Click(object sender, EventArgs e)
        {
            if (tb1_8bit.Text != "") k_8bit = Convert.ToInt32(tb1_8bit.Text);
            SinClass1.bit8sin(k_8bit, img, pictureBox01, progressBar1);
            f_8bit.Close();
        }

      

        private void pictureBox01_Click(object sender, EventArgs e)
        {

        }
// --------------------------------------------------------------------------------------------------------- Масштаб

        // -------------------------------------------------------------------------------------------------------- Отображение масштаба
       
        private void ScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int w1 = pictureBox01.Image.Width;
            int h1 = pictureBox01.Image.Height;
            Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);

            count_scale++;
            label15.Text = "S:" + count_scale.ToString();
            if (count_scale == 0)
            {
                pictureBox01.Image = bmp1;
                //pictureBox01.Image.Size = bmp1.Size;
            }
            else   //Math.Pow(2, count_scale)
            {
                Size nSize = new Size((int)(bmp1.Width / 2), (int)(bmp1.Height / 2));
               Image gdi = new Bitmap(nSize.Width, nSize.Height);
                Graphics ZoomInGraphics = Graphics.FromImage(gdi);

                ZoomInGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                ZoomInGraphics.DrawImage(bmp1, new Rectangle(new Point(0, 0), nSize), new Rectangle(new Point(0, 0), bmp1.Size), GraphicsUnit.Pixel);
                ZoomInGraphics.Dispose();// 
                              pictureBox01.Image = gdi;
                              pictureBox01.Size = gdi.Size;
           }
        }
// -------------------------------------------------------------------------------------------------------- Объединение по цветам
        private void SUM_ColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SumDialog(this.Sum_Color_Click, 1, 2, 3);
        }

        private void SubToolStripMenuItem_Click(object sender, EventArgs e)  //----------------------------- Вычитание
        {
            this.SumDialog(this.Sub_Color_Click, 3, 1, 2);
        } 
        
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
        
        private void Sum_Color_Click(object sender, EventArgs e)
        {
            int Sum_s=1;
            int Sum_1 = 2;
            int Sum_2 = 3;
            if (tb1_filt.Text != "") Sum_1 = Convert.ToInt32(tb1_filt.Text);
            if (tb2_filt.Text != "") Sum_2 = Convert.ToInt32(tb2_filt.Text);
            if (tb3_filt.Text != "") Sum_s = Convert.ToInt32(tb3_filt.Text);

            switch (Sum_1-1)
            {
                case 0: img[0] = pictureBox1.Image; break;
                case 1: img[1] = pictureBox2.Image; break;
                case 2: img[2] = pictureBox3.Image; break;
                case 3: img[3] = pictureBox4.Image; break;
                case 4: img[4] = pictureBox5.Image; break;
                case 5: img[5] = pictureBox6.Image; break;
                case 6: img[6] = pictureBox7.Image; break;
                case 7: img[7] = pictureBox8.Image; break;
            }
            switch (Sum_2-1)
            {
                case 0: img[0] = pictureBox1.Image; break;
                case 1: img[1] = pictureBox2.Image; break;
                case 2: img[2] = pictureBox3.Image; break;
                case 3: img[3] = pictureBox4.Image; break;
                case 4: img[4] = pictureBox5.Image; break;
                case 5: img[5] = pictureBox6.Image; break;
                case 6: img[6] = pictureBox7.Image; break;
                case 7: img[7] = pictureBox8.Image; break;
            }
            

            SumClass.Sum_Color(img, Sum_1, Sum_2, Sum_s, progressBar1);  //(pictureBox01, progressBar1, k_filt);

            h[Sum_s - 1] = img[Sum_s - 1].Height;
            w[Sum_s - 1] = img[Sum_s - 1].Width;
           
            switch (Sum_s-1)
            {
                case 0: pictureBox1.Image = img[0];  break;
                case 1: pictureBox2.Image = img[1];  break; 
                case 2: pictureBox3.Image = img[2];  break; 
                case 3: pictureBox4.Image = img[3];  break; 
                case 4: pictureBox5.Image = img[4];  break;
                case 5: pictureBox6.Image = img[5];  break; 
                case 6: pictureBox7.Image = img[6];  break; 
                case 7: pictureBox8.Image = img[7];  break; 
            }


            f_filt.Close();
        }

        private void Sub_Color_Click(object sender, EventArgs e)
        {
            int Sum_s = 1;
            int Sum_1 = 2;
            int Sum_2 = 3;
            if (tb1_filt.Text != "") Sum_1 = Convert.ToInt32(tb1_filt.Text);
            if (tb2_filt.Text != "") Sum_2 = Convert.ToInt32(tb2_filt.Text);
            if (tb3_filt.Text != "") Sum_s = Convert.ToInt32(tb3_filt.Text);

            switch (Sum_1 - 1)
            {
                case 0: img[0] = pictureBox1.Image; break;
                case 1: img[1] = pictureBox2.Image; break;
                case 2: img[2] = pictureBox3.Image; break;
                case 3: img[3] = pictureBox4.Image; break;
                case 4: img[4] = pictureBox5.Image; break;
                case 5: img[5] = pictureBox6.Image; break;
                case 6: img[6] = pictureBox7.Image; break;
                case 7: img[7] = pictureBox8.Image; break;
            }
            switch (Sum_2 - 1)
            {
                case 0: img[0] = pictureBox1.Image; break;
                case 1: img[1] = pictureBox2.Image; break;
                case 2: img[2] = pictureBox3.Image; break;
                case 3: img[3] = pictureBox4.Image; break;
                case 4: img[4] = pictureBox5.Image; break;
                case 5: img[5] = pictureBox6.Image; break;
                case 6: img[6] = pictureBox7.Image; break;
                case 7: img[7] = pictureBox8.Image; break;
            }


            SumClass.Sub_Color(img, Sum_1, Sum_2, Sum_s, progressBar1, N_sin, N2_sin);  //(pictureBox01, progressBar1, k_filt);

            h[Sum_s - 1] = img[Sum_s - 1].Height;
            w[Sum_s - 1] = img[Sum_s - 1].Width;

            switch (Sum_s - 1)
            {
                case 0: pictureBox1.Image = img[0]; break;
                case 1: pictureBox2.Image = img[1]; break;
                case 2: pictureBox3.Image = img[2]; break;
                case 3: pictureBox4.Image = img[3]; break;
                case 4: pictureBox5.Image = img[4]; break;
                case 5: pictureBox6.Image = img[5]; break;
                case 6: pictureBox7.Image = img[6]; break;
                case 7: pictureBox8.Image = img[7]; break;
            }


            f_filt.Close();
        }

//----------------------------------------------------------------------------------------------------------------------------------     
        private void SinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int max_x = 10, max_y = 600;
            //int  i, j;
            double N = N_sin;                         // число синусоид
            // double pi =  Math.PI; 

            f2 = new Form();
            f2.Size = new Size(max_x + 146, max_y + 36);
            f2.StartPosition = FormStartPosition.Manual;
            Point p = this.Location;
            p.Offset(100, 105);
            f2.Location = p;
            //---------
            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(max_x + 8, 30);
            label1.Text = "Число синусоид:";

            tb1 = new TextBox();
            tb1.Location = new System.Drawing.Point(max_x + 8, 50);
            tb1.Size = new System.Drawing.Size(90, 8);
            tb1.Text = N.ToString();
            //----------
            Label label2 = new Label();
            label2.Location = new System.Drawing.Point(max_x + 8, 80);
            label2.Text = "Фазовый сдвиг";

            tb2 = new TextBox();
            tb2.Location = new System.Drawing.Point(max_x + 8, 100);
            tb2.Size = new System.Drawing.Size(90, 8);
            tb2.Text = N_fz.ToString();

            //----------
            groupbx1 = new GroupBox();
            rb1 = new RadioButton();
            rb2 = new RadioButton();

            rb1.Size = new System.Drawing.Size(40, 20);
            rb2.Size = new System.Drawing.Size(40, 20);

            groupbx1.Location = new System.Drawing.Point(max_x + 8, 130);
            groupbx1.Size = new System.Drawing.Size(100, 55);
            rb1.Location = new System.Drawing.Point(10, 10);
            rb2.Location = new System.Drawing.Point(10, 30);

            rb1.Text = "X";
            rb1.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
            rb1.Checked = true;
            rb2.Text = "Y";
            rb2.CheckedChanged += new EventHandler(radioButton_CheckedChanged);

            groupbx1.Controls.Add(rb1);
            groupbx1.Controls.Add(rb2);
            //-----------
            groupbx2 = new GroupBox();
            rb3 = new CheckBox();
            rb3.Location = new System.Drawing.Point(max_x + 12, 195);
            rb3.Text = "Color";
            rb3.Checked = true;
            //---------
            groupbx2 = new GroupBox();
            rbf0 = new RadioButton();
            rbf1 = new RadioButton();
            rbf2 = new RadioButton();
            rbf3 = new RadioButton();
            rbf4 = new RadioButton();
            rbf5 = new RadioButton();
            rbf6 = new RadioButton();
            rbf7 = new RadioButton();
            rbf8 = new RadioButton();

            groupbx2.Location = new System.Drawing.Point(max_x + 8, 220);
            groupbx2.Size = new System.Drawing.Size(118, 200);
            rbf0.Location = new System.Drawing.Point(10, 10);
            rbf1.Location = new System.Drawing.Point(10, 30);
            rbf2.Location = new System.Drawing.Point(10, 50);
            rbf3.Location = new System.Drawing.Point(10, 70);
            rbf4.Location = new System.Drawing.Point(10, 90);
            rbf5.Location = new System.Drawing.Point(10, 110);
            rbf6.Location = new System.Drawing.Point(10, 130);
            rbf7.Location = new System.Drawing.Point(10, 150);
            rbf8.Location = new System.Drawing.Point(10, 170);

            rbf0.Text = "Все разряды"; rbf0.Checked = true; rbf0.CheckedChanged += new EventHandler(radioButton_f_CheckedChanged);
            rbf1.Text = "0 - разряд"; rbf1.CheckedChanged += new EventHandler(radioButton_f_CheckedChanged);
            rbf2.Text = "1 - разряд"; rbf2.CheckedChanged += new EventHandler(radioButton_f_CheckedChanged);
            rbf3.Text = "2 - разряд"; rbf3.CheckedChanged += new EventHandler(radioButton_f_CheckedChanged);
            rbf4.Text = "3 - разряд"; rbf4.CheckedChanged += new EventHandler(radioButton_f_CheckedChanged);
            rbf5.Text = "4 - разряд"; rbf5.CheckedChanged += new EventHandler(radioButton_f_CheckedChanged);
            rbf6.Text = "5 - разряд"; rbf6.CheckedChanged += new EventHandler(radioButton_f_CheckedChanged);
            rbf7.Text = "6 - разряд"; rbf7.CheckedChanged += new EventHandler(radioButton_f_CheckedChanged);
            rbf8.Text = "7 - разряд"; rbf8.CheckedChanged += new EventHandler(radioButton_f_CheckedChanged);

            groupbx2.Controls.Add(rbf0);
            groupbx2.Controls.Add(rbf1);
            groupbx2.Controls.Add(rbf2);
            groupbx2.Controls.Add(rbf3);
            groupbx2.Controls.Add(rbf4);
            groupbx2.Controls.Add(rbf5);
            groupbx2.Controls.Add(rbf6);
            groupbx2.Controls.Add(rbf7);
            groupbx2.Controls.Add(rbf8);
            //---------     
            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(max_x + 8, 445);
            b1.Text = "ok";
            b1.Size = new System.Drawing.Size(100, 30);
            b1.Click += new System.EventHandler(b1_Click);
            //---------     
            Button b2 = new Button();
            b2.Location = new System.Drawing.Point(max_x + 8, 495);
            b2.Text = "Save 8 bit";
            b2.Size = new System.Drawing.Size(100, 30);
            b2.Click += new System.EventHandler(b2_Click);


            f2.Controls.Add(tb1);
            f2.Controls.Add(label1);
            f2.Controls.Add(tb2);
            f2.Controls.Add(label2);

            f2.Controls.Add(b1);
            f2.Controls.Add(b2);
            f2.Controls.Add(groupbx1);
            f2.Controls.Add(groupbx2);
            f2.Controls.Add(rb3);
          
            f2.Show();


            f_sin = new Form();
            f_sin.Size = new Size(1280 + 8, 1024 + 8);
            f_sin.StartPosition = FormStartPosition.Manual;
            p.Offset(160, 0);
            f_sin.Location = p;

            pc1 = new PictureBox();
            pc1.BackColor = Color.White;
            pc1.Location = new System.Drawing.Point(0, 8);
            pc1.Size = new Size(1280,1024);
            pc1.SizeMode = PictureBoxSizeMode.StretchImage;
            pc1.BorderStyle = BorderStyle.Fixed3D;
            f_sin.Controls.Add(pc1);
           
            //pc1.Refresh();
            f_sin.Show();

        }

        // --------------------------------------------------------------------------------------------- "OK" в синусоиде

        private void b1_Click(object sender, EventArgs e)
        {
            if (tb1.Text != "") N_sin = Convert.ToDouble(tb1.Text);
            if (tb2.Text != "") N_fz = Convert.ToDouble(tb2.Text);

            int rb3_int = 1;
            if (rb3.Checked) rb3_int = 1; else rb3_int = 0;


            int max_x = 1024, max_y = 768;
         
            SinClass1.b1_RGB(N_sin, N_fz, XY, rb3_int, MASK, max_x, max_y, pc1, 0);
            pc1.Refresh();
           
        }

        //--------------------------------------------------------------------------------------Кнопка Save 8 bit в Синусоиде
        private void b2_Click(object sender, EventArgs e)
        {
            if (tb1.Text != "") N_sin = Convert.ToDouble(tb1.Text);
            if (tb2.Text != "") N_fz = Convert.ToDouble(tb2.Text);

            int rb3_int = 1;
            if (rb3.Checked) rb3_int = 1; else rb3_int = 0;


            int max_x = 1024, max_y = 768;

            SinClass1.b1_RGB(N_sin, N_fz, XY, rb3_int, MASK, max_x, max_y, pc1, 1);
            pc1.Refresh();
            
        }
        // ------------------------------------------------------------------------ Радиокнопки X Y в Синусоиде
        void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb == rb1) { XY = 1; } else if (rb == rb2) { XY = 0; }

        }
        //--------------------------------------------------------------------------- Номер разряда
        void radioButton_f_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb == rbf0) { MASK = 0; }
            if (rb == rbf1) { MASK = 1; }
            if (rb == rbf2) { MASK = 2; }
            if (rb == rbf3) { MASK = 3; }
            if (rb == rbf4) { MASK = 4; }
            if (rb == rbf5) { MASK = 5; }
            if (rb == rbf6) { MASK = 6; }
            if (rb == rbf7) { MASK = 7; }
            if (rb == rbf8) { MASK = 8; }
            label2.Text = "M:" + MASK.ToString();

        }

 
      
//--------------------------------------------------------------------------------------------------- Диалог для расшифровки и построения таблиц
        //CheckBox rb;
        int pr_obr= 10;
        
        private void DIALOG_CHINA(EventHandler functionPointer)
        {
            int max_x = 220, max_y = 260;
            f2 = new Form();
            f2.Size = new Size(max_x, max_y);
            f2.StartPosition = FormStartPosition.Manual;
            p = this.Location;                // Глобальный
            p.Offset(100, 105);
            f2.Location = p;

            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(4, 10);
            label1.Size = new System.Drawing.Size(120, 20);
            label1.Text = "Число синусоид 1:";

            tb1 = new TextBox();
            tb1.Location = new System.Drawing.Point(126, 10);
            tb1.Size = new System.Drawing.Size(80, 8);
            tb1.Text = N_sin.ToString();

            // Фазовый сдвиг
            Label label2 = new Label();
            label2.Location = new System.Drawing.Point(4, 50);
            label2.Size = new System.Drawing.Size(120, 20);
            label2.Text = "Число синусоид 2:";

            tb2 = new TextBox();
            tb2.Location = new System.Drawing.Point(126, 50);
            tb2.Size = new System.Drawing.Size(80, 8);
            tb2.Text = N2_sin.ToString();

            Label label3 = new Label();
            label3.Location = new System.Drawing.Point(4, 90);
            label3.Size = new System.Drawing.Size(120, 20);
            label3.Text = "Число периодов:";

            tb3 = new TextBox();
            tb3.Location = new System.Drawing.Point(126, 90);
            tb3.Size = new System.Drawing.Size(80, 8);
            tb3.Text = NDiag.ToString();

            Label label4 = new Label();
            label4.Location = new System.Drawing.Point(4, 130);
            label4.Size = new System.Drawing.Size(120, 20);
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


            f2.Controls.Add(label1);
            f2.Controls.Add(label2);
            f2.Controls.Add(label3);
            f2.Controls.Add(label4);
            f2.Controls.Add(tb1);
            f2.Controls.Add(tb2);
            f2.Controls.Add(tb3);
            f2.Controls.Add(tb4);
            f2.Controls.Add(b1);
            f2.Controls.Add(rb3);

            f2.Show();
        }
        // ---------------------------------------------------------------------------------------------------------------------- Построение распределения в таблице для расшифровки
      
        private void FRMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DIALOG_CHINA(this.pi2_Click);
        }
        private void pi2_Click(object sender, EventArgs e)
        {
            int rb_int = 0;
            string strN1 = " ", strN2 = " ";
            strN1 = tb1.Text;
            strN2 = tb2.Text;
           
            N_sin                     = Convert.ToDouble(tb1.Text);
            N2_sin                    = Convert.ToDouble(tb2.Text);
            if (tb3.Text != "") NDiag = Convert.ToInt32(tb3.Text);
            pr_obr                    = Convert.ToInt32(tb4.Text);

            if (rb3.Checked) rb_int = 1; else rb_int = 0;                                                // По форме 3 кадра
            Pi_Class1.pi2_frml(img, pictureBox01, progressBar1, strN1, strN2, NDiag, p, x0_end, x1_end, y0_end, y1_end, rb_int, pr_obr);
           // f2.Close();

        }
        // ----------------------------------------------------------------------------------------------------------------------- Построение идеальной таблицы для расшифровки
        private void PRM_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DIALOG_CHINA(this.pi2_prm_Click);
        }
        private void pi2_prm_Click(object sender, EventArgs e)
        {
            string strN1 = " ", strN2 = " ";
            strN1 = tb1.Text;
            strN2 = tb2.Text;
            if (tb3.Text != "") NDiag = Convert.ToInt32(tb3.Text);
            N_sin = Convert.ToDouble(tb1.Text);
            N2_sin = Convert.ToDouble(tb2.Text);
            Pi_Class1.pi2_prmtr(img, strN1, strN2, NDiag, p);      
            f2.Close();
        }

// -------------------------------------------------------------------------------------------------------------------- Расшифровка
        private void RSHFRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DIALOG_CHINA(this.RSHFRT);
           
        }

        private void RSHFRT(object sender, EventArgs e)
        {
            string strN1 = " ", strN2 = " ";
            strN1 = tb1.Text;
            strN2 = tb2.Text;
            if (tb3.Text != "") NDiag = Convert.ToInt32(tb3.Text);
            Pi_Class1.pi2_rshfr(img, pictureBox01, progressBar1, x0_end, x1_end, y0_end, y1_end, strN1, strN2, NDiag);
        }

// -------------------------------------------------------------------------------------------------------------------- Устранение тренда
        private void устранениеТрендаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pi_Class1.pi2_ABC(pictureBox01, progressBar1, x0_end, x1_end, y0_end, y1_end);
        }

        // --------------------------------------------------------------------------------------  Фаза
        private void aTAN123ToolStripMenuItem_Click_1(object sender, EventArgs e)         // -------- ATAN2 1,2,3
        {
            double[] fz = new double[4];
            fz[0] = N_fz; fz[1] = N_fz2; fz[2] = N_fz3; fz[3] = N_fz4;
            int n = 3; if (fz[3] != 0) n = 4;
            FazaClass.ATAN_123(img, pictureBox01, progressBar1, n, fz, Gamma);
        
        }

        private void aTANRGBToolStripMenuItem_Click_1(object sender, EventArgs e)        // -------- ATAN2 RGB
        { FazaClass.ATAN_RGB(pictureBox01, progressBar1); }

        private void aTAN2123GraphToolStripMenuItem_Click(object sender, EventArgs e)    // -------- ATAN2 эллипс
        {
            double[] fz = new double[4];
            fz[0] = N_fz; fz[1] = N_fz2; fz[2] = N_fz3; fz[3] = N_fz4;
            int n = 3; if (fz[3] != 0) n = 4;
            FazaClass.Graph_ATAN(img, pictureBox01, progressBar1, p, x0_end, x1_end, y0_end, y1_end, n, fz, Gamma);

        }
        private void PRMTRToolStripMenuItem_Click(object sender, EventArgs e)            // Задание начальных параметров для получения ATAN 
        {
            int max_x = 320, max_y = 200;
            f2 = new Form();
            f2.Size = new Size(max_x, max_y);
            f2.StartPosition = FormStartPosition.Manual;
            p = this.Location;                // Глобальный
            p.Offset(100, 105);
            f2.Location = p;

            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(4, 10);
            label1.Size = new System.Drawing.Size(120, 20);
            label1.Text = "Gamma:";

            tb1 = new TextBox();
            tb1.Location = new System.Drawing.Point(126, 10);
            tb1.Size = new System.Drawing.Size(80, 8);
            tb1.Text = Gamma.ToString();


            Label label2 = new Label();
            label2.Location = new System.Drawing.Point(4, 50);
            label1.Size = new System.Drawing.Size(100, 20);
            label2.Text = "Фазовый сдвиг";

            tb2 = new TextBox();
            tb2.Location = new System.Drawing.Point(106, 50);
            tb2.Size = new System.Drawing.Size(40, 8);
            tb2.Text = N_fz.ToString();
            tb3 = new TextBox();
            tb3.Location = new System.Drawing.Point(156, 50);
            tb3.Size = new System.Drawing.Size(40, 8);
            tb3.Text = N_fz2.ToString();
            tb4 = new TextBox();
            tb4.Location = new System.Drawing.Point(206, 50);
            tb4.Size = new System.Drawing.Size(40, 8);
            tb4.Text = N_fz3.ToString();
            tb5 = new TextBox();
            tb5.Location = new System.Drawing.Point(256, 50);
            tb5.Size = new System.Drawing.Size(40, 8);
            tb5.Text = N_fz4.ToString();

            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(8, 130);
            b1.Text = "ok";
            b1.Size = new System.Drawing.Size(160, 30);
            b1.Click += new System.EventHandler(pi2_prmtr_Click);


            f2.Controls.Add(label1);
            f2.Controls.Add(label2);   
            f2.Controls.Add(tb1);
            f2.Controls.Add(tb2);
            f2.Controls.Add(tb3);
            f2.Controls.Add(tb4);
            f2.Controls.Add(tb5); 

            f2.Controls.Add(b1);

            f2.Show();
        }

        private void pi2_prmtr_Click(object sender, EventArgs e)
        {
            
            if (tb1.Text != "") Gamma = Convert.ToDouble(tb1.Text);
            if (tb2.Text != "") N_fz = Convert.ToDouble(tb2.Text);
            if (tb3.Text != "") N_fz2 = Convert.ToDouble(tb3.Text);
            if (tb4.Text != "") N_fz3 = Convert.ToDouble(tb4.Text);
            if (tb5.Text != "") N_fz4 = Convert.ToDouble(tb5.Text);
          
            f2.Close();
        }

 // ----------------------------------------------------------------------------------------------------------------------------------------------- Включить камеру      
      



//------------------------------------------------------------------------------------------------- Canon 550
        int Canon_Height = 1200;
        int Canon_Width  = 1600;
        int sleep = 2500;

        

        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.SafeCall(() => {  reg_image = 0;  if (camera != null)  camera.TakePicture(); }, ex => MessageBox.Show(ex.ToString()));

            imageGetter.getImage();
        }

            
      

      /*  private void UpdatePicture(Image image)
        {
            w[index_img] = Canon_Width;
            h[index_img] = Canon_Height;  
            
            img[index_img] = image;
            switch (index_img)
            {
                case 0: pictureBox1.Image = img[index_img]; break;
                case 1: pictureBox2.Image = img[index_img]; break;
                case 2: pictureBox3.Image = img[index_img]; break;
                case 3: pictureBox4.Image = img[index_img]; break;
                case 4: pictureBox5.Image = img[index_img]; break;
                case 5: pictureBox6.Image = img[index_img]; break;
                case 6: pictureBox7.Image = img[index_img]; break;
                case 7: pictureBox8.Image = img[index_img]; break;
            }
        }*/

       /* private void SafeCall(Action action, Action<Exception> exceptionHandler)
        {
            try { action(); }
            catch (Exception ex)  { if (this.InvokeRequired) this.Invoke(exceptionHandler, ex);  else exceptionHandler(ex); }
        }*/

       

        private void SizeToolStripMenuItem_Click(object sender, EventArgs e)  // ----------------------------------------- Изменить размер кадра
        {
      
            int max_x = 200, max_y = 150;
            f2 = new Form();
            f2.Size = new Size(max_x, max_y);
            f2.StartPosition = FormStartPosition.Manual;
            Point p = this.Location;
            p.Offset(100, 105);
            f2.Location = p;

            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(4, 10);
            label1.Size = new System.Drawing.Size(100, 20);
            label1.Text = "Размер по X:";

            tb1 = new TextBox();
            tb1.Location = new System.Drawing.Point(106, 10);
            tb1.Size = new System.Drawing.Size(40, 40);
            tb1.Text = Canon_Width.ToString();

            Label label2 = new Label();
            label2.Location = new System.Drawing.Point(4, 50);
            label2.Size = new System.Drawing.Size(100, 20);
            label2.Text = "Размер по Y:";

            tb2 = new TextBox();
            tb2.Location = new System.Drawing.Point(106, 50);
            tb2.Size = new System.Drawing.Size(40, 40);
            tb2.Text = Canon_Height.ToString();

            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(8, 90);
            b1.Text = "ok";
            b1.Size = new System.Drawing.Size(150, 30);
            b1.Click += new System.EventHandler(Cadr_Ini_Click);

            f2.Controls.Add(label1);
            f2.Controls.Add(label2);
            f2.Controls.Add(tb1);
            f2.Controls.Add(tb2);
            f2.Controls.Add(b1);

            f2.Show();
            
        }


        private void Cadr_Ini_Click(object sender, EventArgs e)
        {
            if (tb1.Text != "") Canon_Width = Convert.ToInt32(tb1.Text);
            if (tb2.Text != "") Canon_Height = Convert.ToInt32(tb2.Text);
           
        }

        private void серия8КадровToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageGetter.getImage();
            seriesOfPhotosInProgress = true;
        }

//--------------------------------------------------------------------------------------------------- Регистрация двух серий синусоид
    

        int cntr = 0;       
        private void Cadr8ToolStripMenuItem_Click(object sender, EventArgs e)  
        {
            double N = N_sin;                                 // Период синусомд в первой серии
            double N2 = N2_sin;                               // Период синусоид во второй серии

            int max_x = 356, max_y = 200;
            f2 = new Form();
            f2.Size = new Size(max_x, max_y);
            f2.StartPosition = FormStartPosition.Manual;
            Point p = this.Location;
            p.Offset(100, 105);
            f2.Location = p;

            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(4, 10);
            label1.Size = new System.Drawing.Size(100, 20);
            label1.Text = "Число синусоид:";

            tb1 = new TextBox();
            tb1.Location = new System.Drawing.Point(106, 10);
            tb1.Size = new System.Drawing.Size(40, 8);
            tb1.Text = N.ToString();

            tb12 = new TextBox();
            tb12.Location = new System.Drawing.Point(156, 10);
            tb12.Size = new System.Drawing.Size(40, 8);
            tb12.Text = N2.ToString();


            // ---------------------------------------------------------     Фазовый сдвиг
            Label label2 = new Label();
            label2.Location = new System.Drawing.Point(4, 50);
            label1.Size = new System.Drawing.Size(100, 20);
            label2.Text = "Фазовый сдвиг";

            tb2 = new TextBox();
            tb2.Location = new System.Drawing.Point(106, 50);
            tb2.Size = new System.Drawing.Size(40, 8);
            tb2.Text = N_fz.ToString();
            tb3 = new TextBox();
            tb3.Location = new System.Drawing.Point(156, 50);
            tb3.Size = new System.Drawing.Size(40, 8);
            tb3.Text = N_fz2.ToString();
            tb4 = new TextBox();
            tb4.Location = new System.Drawing.Point(206, 50);
            tb4.Size = new System.Drawing.Size(40, 8);
            tb4.Text = N_fz3.ToString();
            tb5 = new TextBox();
            tb5.Location = new System.Drawing.Point(256, 50);
            tb5.Size = new System.Drawing.Size(40, 8);
            tb5.Text = N_fz4.ToString();

            // ---------------------------------------------------------------------  Ориентация синусоид (вдоль или поперек оси X)
            groupbx1 = new GroupBox();
            rb1 = new RadioButton();
            rb2 = new RadioButton();

            rb1.Size = new System.Drawing.Size(40, 20);
            rb2.Size = new System.Drawing.Size(40, 20);

            groupbx1.Location = new System.Drawing.Point(86, 80);
            groupbx1.Size = new System.Drawing.Size(100, 55);
            rb1.Location = new System.Drawing.Point(10, 10);
            rb2.Location = new System.Drawing.Point(10, 30);
           
            rb1.Text = "X";
            rb1.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
            rb1.Checked = true;
            rb2.Text = "Y";
            rb2.CheckedChanged += new EventHandler(radioButton_CheckedChanged);

            groupbx1.Controls.Add(rb1);
            groupbx1.Controls.Add(rb2);

            //-------------------------------------------------------------------  Проекция синусоиды или по битам
            groupbx2 = new GroupBox();
            rb11 = new RadioButton();
            rb22 = new RadioButton();

            rb11.Size = new System.Drawing.Size(40, 20);
            rb22.Size = new System.Drawing.Size(40, 20);

            groupbx2.Location = new System.Drawing.Point(206, 80);
            groupbx2.Size = new System.Drawing.Size(100, 55);
            rb11.Location = new System.Drawing.Point(10, 10);
            rb22.Location = new System.Drawing.Point(10, 30);

            rb11.Text = "Sin";
            rb11.CheckedChanged += new EventHandler(radioButton_CheckedChanged11);
            rb11.Checked = true;
            rb22.Text = "Bit";
            rb22.CheckedChanged += new EventHandler(radioButton_CheckedChanged11);

            groupbx2.Controls.Add(rb11);
            groupbx2.Controls.Add(rb22);

            //-----------------------------------------------------------------------------------------------------------------------
            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(8, 140);
            b1.Text = "ok";
            b1.Size = new System.Drawing.Size(160, 30);
            b1.Click += new System.EventHandler(Cadr8_Click);

                    

            f2.Controls.Add(label1);
            f2.Controls.Add(label2);
            f2.Controls.Add(tb1);
            f2.Controls.Add(tb12);
            //f2.Controls.Add(tb22);
            f2.Controls.Add(tb2);   f2.Controls.Add(tb3);   f2.Controls.Add(tb4);   f2.Controls.Add(tb5);
            f2.Controls.Add(b1); 
            f2.Controls.Add(groupbx1);
            f2.Controls.Add(groupbx2);
            f2.Show();

            f_sin = new Form();
            f_sin.Size = new Size(800 + 8, 600 + 8);
            f_sin.StartPosition = FormStartPosition.Manual;
           // Point p = this.Location;
            p.Offset(200, 0);
            f_sin.Location = p;

            pc1 = new PictureBox();
            pc1.BackColor = Color.White;
            pc1.Location = new System.Drawing.Point(0, 8);
            pc1.Size = new Size(800, 600);
            pc1.SizeMode = PictureBoxSizeMode.StretchImage;
            pc1.BorderStyle = BorderStyle.Fixed3D;

            f_sin.Controls.Add(pc1);
            cntr = 1;


        }

        int SINUS =  1;
        void radioButton_CheckedChanged11(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb == rb11) { SINUS = 1; } else if (rb == rb22) { SINUS = 0; }

        }
         
        private void Cadr8_Click(object sender, EventArgs e)
        {
           /* if (tb1.Text  != "")  N_sin  = Convert.ToDouble(tb1.Text);
            if (tb12.Text != "")  N2_sin = Convert.ToDouble(tb12.Text);
            if (tb2.Text  != "")  N_fz   = Convert.ToDouble(tb2.Text);
            if (tb3.Text  != "")  N_fz2  = Convert.ToDouble(tb3.Text);
            if (tb4.Text  != "")  N_fz3  = Convert.ToDouble(tb4.Text);
            if (tb5.Text  != "")  N_fz4  = Convert.ToDouble(tb5.Text);

            int max_x = 800, max_y = 600;                                   //  Размер для проектора

            if (SINUS == 1)
            {
                SinClass1.sin_f(N_sin / 10, N_fz, max_x, max_y, XY, pc1);
                pc1.Refresh();
                f_sin.Show();

                index_img = 0;                                                //     Начать запись с pictureBox1
                if (cntr == 1) { cntr = 0; }
                else
                {
                    SinClass1.sin_f(N_sin / 10, N_fz, max_x, max_y, XY, pc1);     //---------Первая серия--------------1 sin()
                    pc1.Refresh();
                    f_sin.Show();
                    if (camera != null) camera.TakePicture();
                    Thread.Sleep(sleep);


                    SinClass1.sin_f(N_sin / 10, N_fz2, max_x, max_y, XY, pc1);    //----------------------------------2 sin()
                    pc1.Refresh();
                    f_sin.Show();
                    if (camera != null) camera.TakePicture();
                    Thread.Sleep(sleep);

                    SinClass1.sin_f(N_sin / 10, N_fz3, max_x, max_y, XY, pc1);    //----------------------------------3 sin()
                    pc1.Refresh();
                    f_sin.Show();
                    if (camera != null) camera.TakePicture();
                    Thread.Sleep(sleep);

                    if (N_fz4 != 0)
                    {
                        SinClass1.sin_f(N_sin / 10, N_fz4, max_x, max_y, XY, pc1);    //----------------------------------4 sin()
                        pc1.Refresh();
                        f_sin.Show();
                        if (camera != null) camera.TakePicture();
                        Thread.Sleep(sleep);
                    }




                    SinClass1.sin_f(N2_sin / 10, N_fz, max_x, max_y, XY, pc1);     //-------------Вторая серия---------11 sin()
                    pc1.Refresh();
                    f_sin.Show();
                    if (camera != null) camera.TakePicture();
                    Thread.Sleep(sleep);


                    SinClass1.sin_f(N2_sin / 10, N_fz2, max_x, max_y, XY, pc1);    //----------------------------------21 sin()
                    pc1.Refresh();
                    f_sin.Show();
                    if (camera != null) camera.TakePicture();
                    Thread.Sleep(sleep);

                    SinClass1.sin_f(N2_sin / 10, N_fz3, max_x, max_y, XY, pc1);    //----------------------------------31 sin()
                    pc1.Refresh();
                    f_sin.Show();
                    if (camera != null) camera.TakePicture();
                    Thread.Sleep(sleep);

                    if (N_fz4 != 0)
                    {
                        SinClass1.sin_f(N2_sin / 10, N_fz4, max_x, max_y, XY, pc1);    //----------------------------------4 sin()
                        pc1.Refresh();
                        f_sin.Show();
                        if (camera != null) camera.TakePicture();
                        Thread.Sleep(sleep);
                    }

                }
                // f2.Close();
            }// end SINUS
            else  // ------------------------------------------------------------------------------------------ По битам 8 кадров
            {
                SinClass1.bit_f(N_sin / 10, N_fz, max_x, max_y, XY, 8, pc1);
                pc1.Refresh();
                f_sin.Show();
                index_img = 0;
                if (cntr == 1) { cntr = 0; }
                else
                {
                    for (int i=0; i<8; i++)
                    {
                      SinClass1.bit_f(N_sin / 10, N_fz, max_x, max_y, XY, i+1, pc1);     //---------Первая серия--------------1 bit() -  8 кадр
                      pc1.Refresh();
                      f_sin.Show();
                      if (camera != null) camera.TakePicture();
                      Thread.Sleep(sleep);
                    }
                }
                
            }*/
        }

        private void aTAN2123456ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FazaClass.ATAN_N(img, 0, 1, 2,3, progressBar1, Gamma);          // Вычисление фазы из   1, 2, 3, 4
            h[6] = h[0]; w[6] = h[0];
            pictureBox7.Image = img[6];
            Razmer(w[6], h[6]);
            FazaClass.ATAN_N(img, 3, 4, 5, 7, progressBar1, Gamma);          // Вычисление фазы  из 4, 5, 6
            h[7] = h[0]; w[7] = h[0];
            pictureBox8.Image = img[7];
            Razmer(w[7], h[7]);
        }

        

       

       

        

      
       





        //MessageBox.Show(" Продолжить ? ");
        //if (MessageBox.Show("Закончить нумерацию?", "Нумерация полос", MessageBoxButtons.OKCancel) == DialogResult.OK) return;
    }
}
