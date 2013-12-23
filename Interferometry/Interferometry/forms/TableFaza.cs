using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using rab1;

namespace Interferometry.forms
{
    public partial class TableFaza : Form
    {
        private const int sineNumber1 = 167;
        private const int sineNumber2 = 241;
        private Pi_Class1.ZArrayDescriptor[] images1;
        private Pi_Class1.ZArrayDescriptor[] images2;
        private Pi_Class1.ZArrayDescriptor result1;
        private Pi_Class1.ZArrayDescriptor result2;
        private double[] fz;

        public TableFaza(Pi_Class1.ZArrayDescriptor[] imagesToProcess1, Pi_Class1.ZArrayDescriptor[] imagesToProcess2, double[] fzToProcess)
        {
            InitializeComponent();

            sineNumbers1.Text = Convert.ToString(sineNumber1);
            sineNumbers2.Text = Convert.ToString(sineNumber2);

            images1 = imagesToProcess1;
            images2 = imagesToProcess2;
            fz = fzToProcess;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            result1 = FazaClass.ATAN_1234(images1, fz);
            result2 = FazaClass.ATAN_1234(images2, fz);
        }
        public Pi_Class1.ZArrayDescriptor get1()
        {
            result1 = FazaClass.ATAN_1234(images1, fz);
            return (result1);
        }
        public Pi_Class1.ZArrayDescriptor get2()
        {
            result2 = FazaClass.ATAN_1234(images1, fz);
            return (result2);
        }
    }
}
