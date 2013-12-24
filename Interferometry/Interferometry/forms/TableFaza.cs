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
        private  int sineNumber1 = 167;
        private  int sineNumber2 = 241;
        private Pi_Class1.ZArrayDescriptor[] images1;
        private Pi_Class1.ZArrayDescriptor[] images2;
        private Pi_Class1.ZArrayDescriptor result1;
        private Pi_Class1.ZArrayDescriptor result2;
        private double[] fz;

        public TableFaza()
        {
            InitializeComponent();

            sineNumbers1.Text = Convert.ToString(sineNumber1);
            sineNumbers2.Text = Convert.ToString(sineNumber2);        
           
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            sineNumber1 = Convert.ToInt32(sineNumbers1.Text);
            sineNumber2 = Convert.ToInt32(sineNumbers2.Text);
        }
        public int get_1()
        {
            return (sineNumber1);
        }
        public int get_2()
        {
            return (sineNumber2);
        }
    }
}
