using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using rab1;

//public delegate void ImageUnwrapped(Pi_Class1.ZArrayDescriptor unwrappedImage);

namespace Interferometry.forms
{
    public delegate void Atan_Unwrapped(TableFaza.Res d);

    public partial class TableFaza : Form
    {
        private  int sineNumber1 = 167;
        private  int sineNumber2 = 241;
        public event Atan_Unwrapped atan_Unwrapped;
        private Pi_Class1.ZArrayDescriptor[] source1;
        public Res d;

        private double[] fz1;

        public TableFaza(Pi_Class1.ZArrayDescriptor[] source, double[] fz)
        {
            InitializeComponent();

            sineNumbers1.Text = Convert.ToString(sineNumber1);
            sineNumbers2.Text = Convert.ToString(sineNumber2);
            fz1 = new double[4];
            fz1[0] = fz[0];  fz1[1] = fz[1];  fz1[2] = fz[2];  fz1[3] = fz[3];
            source1 = new Pi_Class1.ZArrayDescriptor[8];
          //  for (int i = 0; i < 8; i++) { source1[i] = source[i]; }
           
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            sineNumber1 = Convert.ToInt32(sineNumbers1.Text);
            sineNumber2 = Convert.ToInt32(sineNumbers2.Text);
            
            Pi_Class1.ZArrayDescriptor[] source = new Pi_Class1.ZArrayDescriptor[4];
            for (int i = 0; i < 4; i++) { source[i] = source1[i]; }
            d.result1 = FazaClass.ATAN_1234(source, fz1, sineNumber2);
  
            for (int i = 4; i < 8; i++) { source[i-4] = source1[i]; }
            d.result2 = FazaClass.ATAN_1234(source, fz1, sineNumber1);
            
            Close();
            atan_Unwrapped(d);
        }

        public class Res
        {
            public  Pi_Class1.ZArrayDescriptor result1;
            public  Pi_Class1.ZArrayDescriptor result2;
        }
    }
   
}
