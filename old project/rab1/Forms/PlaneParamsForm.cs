using System;
using System.Drawing;
using System.Windows.Forms;
using rab1;

public delegate void PlaneParamsChoosed(Pi_Class1.Plane somePlane);

namespace rab1.Forms
{
    public partial class PlaneParamsForm : Form
    {
        private Pi_Class1.Plane somePlane;

        public event PlaneParamsChoosed planeParamsChoosed;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public PlaneParamsForm(Pi_Class1.Plane somePlane)
        {
            InitializeComponent();
            this.somePlane = somePlane;

            textBox1.Text = "" + Convert.ToString(somePlane.a);
            textBox2.Text = "" + Convert.ToString(somePlane.b);
            textBox3.Text = "" + Convert.ToString(somePlane.c);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void buildClicked(object sender, EventArgs e)
        {
            double a = Convert.ToDouble(textBox1.Text);
            double b = Convert.ToDouble(textBox2.Text);
            double c = Convert.ToDouble(textBox3.Text);

            somePlane.a = a;
            somePlane.b = b;
            somePlane.c = c;

            if (planeParamsChoosed != null)
            {
                planeParamsChoosed(somePlane);
            }

            Close();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
