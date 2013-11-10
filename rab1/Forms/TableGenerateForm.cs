using System;
using System.Drawing;
using System.Windows.Forms;

namespace rab1.Forms
{
    public partial class TableGenerateForm : Form
    {
        private const int sineNumber1 = 167;
        private const int sineNumber2 = 241;
        private const int periodsNumber = 9;
        private const int cutLevel = 10;
        private Image[] images;

        public TableGenerateForm(Image[] imagesToProcess)
        {
            InitializeComponent();

            sineNumberTextBox1.Text = Convert.ToString(sineNumber1);
            sineNumberTextBox2.Text = Convert.ToString(sineNumber2);
            periodsNumberTextBox.Text = Convert.ToString(periodsNumber);
            cutLevelTextBox.Text = Convert.ToString(cutLevel);

            images = imagesToProcess;
        }

        private void buildClicked(object sender, EventArgs e)
        {
            int firstSineNumber = Convert.ToInt32(sineNumberTextBox1.Text);
            int secondSineNumber = Convert.ToInt32(sineNumberTextBox2.Text);
            int poriodsNumber = Convert.ToInt32(periodsNumberTextBox.Text);
            int cutLevel = Convert.ToInt32(cutLevelTextBox.Text);
            bool unknownParameter = checkBox1.Checked;

            Pi_Class1.pi2_frml2(images, firstSineNumber, secondSineNumber, poriodsNumber, unknownParameter, cutLevel);
        }
    }
}
