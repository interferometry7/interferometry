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
        double[] fz = new double[4];
           

        public event Atan_Unwrapped atan_Unwrapped;
        private ZArrayDescriptor[] source;
        private double[] fz1;

        public TableFaza(ZArrayDescriptor[] newSource)
        {
            InitializeComponent();
            fz[0] = 0;
            fz[1] = 90;
            fz[2] = 180;
            fz[3] = 270;
            textBox1_fz.Text = Convert.ToString(fz[0]);
            textBox2_fz.Text = Convert.ToString(fz[1]);
            textBox3_fz.Text = Convert.ToString(fz[2]);
            textBox4_fz.Text = Convert.ToString(fz[3]);
            sineNumbers1.Text = Convert.ToString(sineNumber1);
            sineNumbers2.Text = Convert.ToString(sineNumber2);
            fz1 = new double[4];
            fz1[0] = fz[0];  fz1[1] = fz[1];  fz1[2] = fz[2];  fz1[3] = fz[3];
            source = newSource;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            sineNumber1 = Convert.ToInt32(sineNumbers1.Text);
            sineNumber2 = Convert.ToInt32(sineNumbers2.Text);

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

        public class Res
        {
            public  ZArrayDescriptor result1;
            public  ZArrayDescriptor result2;
        }
    }
   
}
