using System;
using System.Drawing;
using System.Windows.Forms;
using Interferometry.math_classes;

public delegate void ImageUnwrapped(ZArrayDescriptor unwrappedImage);

namespace rab1.Forms
{
    public partial class UnwrapForm : Form
    {
       
        private int firstSineNumber= 167;
        private int secondSineNumber= 241;
        private int poriodsNumber= 9;
        private int cutLevel=0;
        private int sdvg_x;
       
        // private bool unknownParameter;
        // private bool SUB_RD;
        private ZArrayDescriptor[] images;
        public event ImageUnwrapped imageUnwrapped;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public UnwrapForm(ZArrayDescriptor[] images)
        {
            InitializeComponent();

            sineNumbers1.Text  = Convert.ToString(firstSineNumber);
            sineNumbers2.Text  = Convert.ToString(secondSineNumber);
            periodsNumber.Text = Convert.ToString(poriodsNumber);
            //cutLevelTextBox.Text = Convert.ToString(0);
            textBox1.Text = Convert.ToString(0);

            this.images = images;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void okButtonClicked(object sender, EventArgs e)
        {
            if (imageUnwrapped != null)
            {
                firstSineNumber = Convert.ToInt32(sineNumbers1.Text);
                secondSineNumber = Convert.ToInt32(sineNumbers2.Text);
                poriodsNumber = Convert.ToInt32(periodsNumber.Text);
                //cutLevel = Convert.ToInt32(cutLevelTextBox.Text);
                sdvg_x = Convert.ToInt32(textBox1.Text);
                //unknownParameter = checkBox1.Checked;
                //SUB_RD = checkBox2.Checked;
                //Pi_Class1.pi2_rshfr(images, firstSineNumber, secondSineNumber, poriodsNumber, unknownParameter, SUB_RD, cutLevel, sdvg_x);
                //Pi_Class1.ZArrayDescriptor result = Pi_Class1.pi2_rshfr(images, firstSineNumber, secondSineNumber, poriodsNumber, unknownParameter, SUB_RD, cutLevel, sdvg_x);
                ZArrayDescriptor result = Pi_Class1.pi2_rshfr(images, firstSineNumber, secondSineNumber, poriodsNumber,  cutLevel, sdvg_x);
               imageUnwrapped(result);
            }
        }
      }
  }
 

