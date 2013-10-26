using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rab1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            double N = 167;                                 // Период синусомд в первой серии
            double N2 = 241;
            double N_fz = 0, N_fz2 = 90, N_fz3 = 180, N_fz4 = 270;   // начальная фаза в градусах
            double N_sin = 167;                          // число синусоид 1

            int XY = 1;                                    // Направление полос

            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(4, 10);
            label1.Size = new System.Drawing.Size(100, 20);
            label1.Text = "Число синусоид:";

            TextBox tb1 = new TextBox();
            tb1.Location = new System.Drawing.Point(106, 10);
            tb1.Size = new System.Drawing.Size(40, 8);
            tb1.Text = N.ToString();

            TextBox tb12 = new TextBox();
            tb12.Location = new System.Drawing.Point(156, 10);
            tb12.Size = new System.Drawing.Size(40, 8);
            tb12.Text = N2.ToString();


            // ---------------------------------------------------------     Фазовый сдвиг
            Label label2 = new Label();
            label2.Location = new System.Drawing.Point(4, 50);
            label1.Size = new System.Drawing.Size(100, 20);
            label2.Text = "Фазовый сдвиг";

            TextBox tb2 = new TextBox();
            tb2.Location = new System.Drawing.Point(106, 50);
            tb2.Size = new System.Drawing.Size(40, 8);
            tb2.Text = N_fz.ToString();
            TextBox tb3 = new TextBox();
            tb3.Location = new System.Drawing.Point(156, 50);
            tb3.Size = new System.Drawing.Size(40, 8);
            tb3.Text = N_fz2.ToString();
            TextBox tb4 = new TextBox();
            tb4.Location = new System.Drawing.Point(206, 50);
            tb4.Size = new System.Drawing.Size(40, 8);
            tb4.Text = N_fz3.ToString();
            TextBox tb5 = new TextBox();
            tb5.Location = new System.Drawing.Point(256, 50);
            tb5.Size = new System.Drawing.Size(40, 8);
            tb5.Text = N_fz4.ToString();

            // ---------------------------------------------------------------------  Ориентация синусоид (вдоль или поперек оси X)
            GroupBox groupbx1 = new GroupBox();
            RadioButton rb1 = new RadioButton();
            RadioButton rb2 = new RadioButton();

            rb1.Size = new System.Drawing.Size(40, 20);
            rb2.Size = new System.Drawing.Size(40, 20);

            groupbx1.Location = new System.Drawing.Point(86, 80);
            groupbx1.Size = new System.Drawing.Size(100, 55);
            rb1.Location = new System.Drawing.Point(10, 10);
            rb2.Location = new System.Drawing.Point(10, 30);

            rb1.Text = "X";
            //rb1.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
            rb1.Checked = true;
            rb2.Text = "Y";
           // rb2.CheckedChanged += new EventHandler(radioButton_CheckedChanged);

            groupbx1.Controls.Add(rb1);
            groupbx1.Controls.Add(rb2);

            //-------------------------------------------------------------------  Проекция синусоиды или по битам
            GroupBox groupbx2 = new GroupBox();
            RadioButton rb11 = new RadioButton();
            RadioButton rb22 = new RadioButton();

            rb11.Size = new System.Drawing.Size(40, 20);
            rb22.Size = new System.Drawing.Size(40, 20);

            groupbx2.Location = new System.Drawing.Point(206, 80);
            groupbx2.Size = new System.Drawing.Size(100, 55);
            rb11.Location = new System.Drawing.Point(10, 10);
            rb22.Location = new System.Drawing.Point(10, 30);

            rb11.Text = "Sin";
           // rb11.CheckedChanged += new EventHandler(radioButton_CheckedChanged11);
            rb11.Checked = true;
            rb22.Text = "Bit";
            //rb22.CheckedChanged += new EventHandler(radioButton_CheckedChanged11);

            groupbx2.Controls.Add(rb11);
            groupbx2.Controls.Add(rb22);

            
            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(8, 140);
            b1.Text = "ok";
            b1.Size = new System.Drawing.Size(160, 30);
            //b1.Click += new System.EventHandler(makeSeriesOfImages);



            this.Controls.Add(label1);
            this.Controls.Add(label2);
            this.Controls.Add(tb1);
            this.Controls.Add(tb12);
            this.Controls.Add(tb2);
            this.Controls.Add(tb3);
            this.Controls.Add(tb4);
            this.Controls.Add(tb5);
            this.Controls.Add(b1);
            this.Controls.Add(groupbx1);
            this.Controls.Add(groupbx2);
            this.Show();

            Form f_sin = new Form();
            f_sin.Size = new Size(800 + 8, 600 + 8);
            f_sin.StartPosition = FormStartPosition.Manual;

            PictureBox pc1 = new PictureBox();
            pc1.BackColor = Color.White;
            pc1.Location = new System.Drawing.Point(0, 8);
            pc1.Size = new Size(800, 600);
            pc1.SizeMode = PictureBoxSizeMode.StretchImage;
            pc1.BorderStyle = BorderStyle.Fixed3D;

            f_sin.Controls.Add(pc1);

            SinClass1.sin_f(N_sin / 10, N_fz, 800, 600, XY, pc1);     //---------Первая серия--------------1 sin()
            pc1.Refresh();
            f_sin.Show();
        }




       /* void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb == rb1)
            {
                XY = 1;
            }
            else if (rb == rb2)
            {
                XY = 0;
            }
        }*/
    }
}
