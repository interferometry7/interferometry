using System;
using System.Drawing;
using System.Windows.Forms;

public delegate void ImageFiltered(Image filtratedImage);

namespace rab1.Forms
{
    public partial class FiltrationForm : Form
    {
        public enum FiltrationType
        {
            Smoothing, 
            Median
        };

        private readonly FiltrationType filtrationType;
        private readonly Image image;

        public event ImageFiltered imageFiltered;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public FiltrationForm(FiltrationType filtrationType, Image image)
        {
            InitializeComponent();

            this.filtrationType = filtrationType;
            this.image = image;

            if (filtrationType == FiltrationType.Smoothing)
            {
                filtrationTypeLabel.Text = "Сглаживание";
            }
            else if (filtrationType == FiltrationType.Median)
            {
                filtrationTypeLabel.Text = "Медианный";
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void applyButton_Click(object sender, EventArgs e)
        {
            int filtrationOrder;

            if (orderTextBox.Text != "")
            {
                filtrationOrder = Convert.ToInt32(orderTextBox.Text);
            }
            else
            {
                MessageBox.Show("Введите порядок фильтрации");
                return;
            }

            Image result;

            if (filtrationType == FiltrationType.Smoothing)
            {
                result = FiltrClass.Filt_121(image, filtrationOrder);
            }
            else if (filtrationType == FiltrationType.Median)
            {
                result = FiltrClass.Filt_Mediana(image, filtrationOrder);
            }
            else
            {
                MessageBox.Show("Неизвестный тип фильтрации");
                return;
            }

            if ((result != null) && (imageFiltered != null))
            {
                imageFiltered(result);
            }

            Close();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void orderTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
