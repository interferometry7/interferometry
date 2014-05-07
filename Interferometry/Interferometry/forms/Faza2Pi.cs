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
    public delegate void Pi2_Unwrapped(Faza2Pi.Res1 d);

    public partial class Faza2Pi : Form
    {
        private  int sineNumber1 = 167;
        private  int sineNumber2 = 241;
       
        public event Pi2_Unwrapped Pi2_Unwrapped;
        private ZArrayDescriptor[] source;
       

        public Faza2Pi(ZArrayDescriptor[] newSource)
        {
            InitializeComponent();         
         
            sineNumbers1.Text = Convert.ToString(sineNumber1);
            sineNumbers2.Text = Convert.ToString(sineNumber2);
            source = newSource;
        }
        // --------------------------------------------------------------------------------------------
        //    Восстановление полной фазы по уже известной востановленной целочисленным методом
        // --------------------------------------------------------------------------------------------
        private void okButton_Click(object sender, EventArgs e)     // 12 -> 10  13 -> 11
        {
            sineNumber1 = Convert.ToInt32(sineNumbers1.Text);
            sineNumber2 = Convert.ToInt32(sineNumbers2.Text);
                      
            Res1 d = new Res1();
            d.result1 = FazaClass.Pi2_V(source[0], source[2], sineNumber2, 4);          
            d.result2 = FazaClass.Pi2_V(source[1], source[2], sineNumber1, 0);
            
            Close();
            Pi2_Unwrapped(d);
        }       

        public class Res1
        {
            public  ZArrayDescriptor result1;
            public  ZArrayDescriptor result2;
        }
       
       
    }
   
}
