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

namespace Interferometry.forms
{
    public partial class Tabl_Sub : Form
    {
        private int m1 = 1;
        private int m2 = 2;
        private int m3 = 3;
        private ZArrayDescriptor[] source;
        
        public Tabl_Sub(ZArrayDescriptor[] newSource)
        {
            InitializeComponent();
            textBox1_sub.Text = Convert.ToString(m1);
            textBox2_sub.Text = Convert.ToString(m2);
            textBox3_sub.Text = Convert.ToString(m3);
            source = newSource;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            m1 = Convert.ToInt32(textBox1_sub.Text);
            m2 = Convert.ToInt32(textBox2_sub.Text);
            m3 = Convert.ToInt32(textBox3_sub.Text);
            source[m3] = FiltrClass.Sub(source, m1, m2);
            Close();
        }
    }
}
