using System;
using System.Drawing;
using System.Windows.Forms;

public delegate void ImageUnwrapped(Bitmap unwrappedImage);

namespace rab1.Forms
{
    public partial class UnwrapForm : Form
    {
        private Image[] images;

        public event ImageUnwrapped imageUnwrapped;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public UnwrapForm(Image[] images)
        {
            InitializeComponent();

            sineNumbers1.Text = "167";
            sineNumbers2.Text = "241";
            periodsNumber.Text = "9";

            this.images = images;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void okButtonClicked(object sender, EventArgs e)
        {
            if (imageUnwrapped != null)
            {
                int firstSineNumber = Convert.ToInt32(sineNumbers1.Text);
                int secondSineNumber = Convert.ToInt32(sineNumbers2.Text);
                int poriodsNumber = Convert.ToInt32(periodsNumber.Text);

                Bitmap result = Pi_Class1.pi2_rshfr(images, firstSineNumber, secondSineNumber, poriodsNumber);

                imageUnwrapped(result);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
