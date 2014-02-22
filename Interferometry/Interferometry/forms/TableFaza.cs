using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interferometry.math_classes;
using rab1;

//public delegate void ImageUnwrapped(Pi_Class1.ZArrayDescriptor unwrappedImage);

namespace Interferometry.forms
{
    public delegate void Atan_Unwrapped(TableFaza.Res d);

    public partial class TableFaza : Form
    {
        private  int sineNumber1 = 167;
        private  int sineNumber2 = 241;
        //private double[] fz = new double[5];
        private double[] fz = {0,90,180,270, 0};  

        public event Atan_Unwrapped atan_Unwrapped;
        private ZArrayDescriptor[] source;
        private double[] fz1;

        public TableFaza(ZArrayDescriptor[] newSource)
        {
            InitializeComponent();
            //fz[0] = 0;
            //fz[1] = 90;
            //fz[2] = 180;
            //fz[3] = 270;
            //fz[4] = 0;
            textBox1_fz.Text = Convert.ToString(fz[0]);
            textBox2_fz.Text = Convert.ToString(fz[1]);
            textBox3_fz.Text = Convert.ToString(fz[2]);
            textBox4_fz.Text = Convert.ToString(fz[3]);
            textBox5_fz.Text = Convert.ToString(fz[4]);
            sineNumbers1.Text = Convert.ToString(sineNumber1);
            sineNumbers2.Text = Convert.ToString(sineNumber2);
            fz1 = new double[5];
            fz1[0] = fz[0]; fz1[1] = fz[1]; fz1[2] = fz[2]; fz1[3] = fz[3]; fz1[4] = fz[4];
            source = newSource;
        }
        // --------------------------------------------------------------------------------------------
        //                         4-х точечный алгоритм
        // --------------------------------------------------------------------------------------------
        private void okButton_Click(object sender, EventArgs e)   // 1,2,3,4 -> 11  5,6,7,8 ->12
        {
            sineNumber1 = Convert.ToInt32(sineNumbers1.Text);
            sineNumber2 = Convert.ToInt32(sineNumbers2.Text);
            fz[0] = Convert.ToInt32(textBox1_fz.Text);
            fz[1] = Convert.ToInt32(textBox2_fz.Text);
            fz[2] = Convert.ToInt32(textBox3_fz.Text);
            fz[3] = Convert.ToInt32(textBox4_fz.Text);
            

            ZArrayDescriptor[] firstSource = new ZArrayDescriptor[4];
            for (int i = 0; i < 4; i++)  { firstSource[i] = source[i]; }
            Res d = new Res();
            d.result1 = FazaClass.ATAN_1234(firstSource, fz, sineNumber2);

            ZArrayDescriptor[] secondSource = new ZArrayDescriptor[4];
            for (int i = 4; i < 8; i++) { secondSource[i - 4] = source[i]; }
            d.result2 = FazaClass.ATAN_1234(secondSource, fz, sineNumber1);
            
            Close();
            atan_Unwrapped(d);
        }
        // --------------------------------------------------------------------------------------------
        //                         5-х точечный алгоритм
        // --------------------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)  //  1, ... ,5 -> 11   6, ..., 10 -> 12
        {
            sineNumber1 = Convert.ToInt32(sineNumbers1.Text);
            sineNumber2 = Convert.ToInt32(sineNumbers2.Text);
            fz[0] = Convert.ToInt32(textBox1_fz.Text);
            fz[1] = Convert.ToInt32(textBox2_fz.Text);
            fz[2] = Convert.ToInt32(textBox3_fz.Text);
            fz[3] = Convert.ToInt32(textBox4_fz.Text);
            fz[4] = Convert.ToInt32(textBox5_fz.Text);

            ZArrayDescriptor[] firstSource = new ZArrayDescriptor[5];
            for (int i = 0; i < 5; i++) { firstSource[i] = source[i]; }
            Res d = new Res();
            d.result1 = FazaClass.ATAN_1234(firstSource, fz, sineNumber2);

            ZArrayDescriptor[] secondSource = new ZArrayDescriptor[5];
            for (int i = 5; i < 10; i++) { secondSource[i - 5] = source[i]; }
            d.result2 = FazaClass.ATAN_1234(secondSource, fz, sineNumber1);

            Close();
            atan_Unwrapped(d);

        }
        // --------------------------------------------------------------------------------------------
        //                         Carre
        // --------------------------------------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            sineNumber1 = Convert.ToInt32(sineNumbers1.Text);
            sineNumber2 = Convert.ToInt32(sineNumbers2.Text);

            ZArrayDescriptor[] firstSource = new ZArrayDescriptor[4];
            for (int i = 0; i < 4; i++) { firstSource[i] = source[i]; }
            Res d = new Res();
            d.result1 = FazaClass.ATAN_Carre(firstSource,  sineNumber2);

            ZArrayDescriptor[] secondSource = new ZArrayDescriptor[4];
            for (int i = 4; i < 8; i++) { secondSource[i - 4] = source[i]; }
            d.result2 = FazaClass.ATAN_Carre(secondSource,  sineNumber1);

            Close();
            atan_Unwrapped(d);
        }



        public class Res
        {
            public  ZArrayDescriptor result1;
            public  ZArrayDescriptor result2;
        }

       

       
    }
   
}
