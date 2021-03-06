﻿using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Interferometry;
using Interferometry.forms;
using Interferometry.math_classes;

public delegate void OneImageOfSeries(Image newImage, int imageNumber);

namespace rab1
{
    public partial class BackgroundImagesGeneratorForm : Form
    {
        private enum StripeType
        {
            /// <summary>
            /// Режим по умолчанию. Синусоидальные полосы
            /// </summary>
            sine,
            /// <summary>
            /// Просто линии
            /// </summary>
            lines,
            /// <summary>
            /// Полосы с дизерингом
            /// </summary>
            dithered,
            /// <summary>
            /// Битовые полосы
            /// </summary>
            bits
        };

        public enum BitImageType
        {
            /// <summary>
            /// Режим по умолчанию. Неинвертированные полосы
            /// </summary>
            simple,
            /// <summary>
            /// Инвертированные полосы
            /// </summary>
            inverted
        };

        private int stripOrientation = 0;
        private StripeType stripeType = StripeType.sine;
        private int imageWidth = 800;
        private int imageHeight = 600;
        private int imageNumber = 0;

        private double numberOfSin1Value;
        private double numberOfSin2Value;

        private int phaseShift1Value;
        private int phaseShift2Value;
        private int phaseShift3Value;
        private int phaseShift4Value;
        private int phaseShift5Value;

        private BackkgroundStripesForm formForStripes;
        private int numberOfImageInSeries = 8;

        /// <summary>
        /// Массив для хранения битовых изображений
        /// </summary>
        private Image[] imagesContainer = new Image[8];
        private int currentBitImageNumber = 0;

        public event OneImageOfSeries oneImageOfSeries;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public BackgroundImagesGeneratorForm()
        {
            InitializeComponent();

            numberOfSin1.Text = "167";
            numberOfSin2.Text = "241";

            phaseShift1.Text = "0";
            phaseShift2.Text = "90";
            phaseShift3.Text = "180";
            phaseShift4.Text = "270";
            phaseShift5.Text = "0";

            convertValues();

            formForStripes = new BackkgroundStripesForm();
            updateInitialImage();
            formForStripes.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void updateInitialImage()
        {
            convertValues();
            Bitmap result = null;

            if (stripeType == StripeType.sine)
            {
                result = SinClass1.drawSine(numberOfSin1Value / 10.0, phaseShift1Value, imageWidth, imageHeight, stripOrientation);
            }
            else if (stripeType == StripeType.lines)
            {
                result = SinClass1.drawLines(numberOfSin1Value / 10.0, phaseShift1Value, imageWidth, imageHeight, stripOrientation);
            }
            else if (stripeType == StripeType.dithered)
            {
                result = SinClass1.drawDitheredLines(numberOfSin1Value / 10.0, phaseShift1Value, imageWidth, imageHeight, stripOrientation);
            }
            else if (stripeType == StripeType.bits)
            {
                result = SinClass1.drawBitImage(numberOfSin1Value / 10.0, phaseShift1Value, imageWidth, imageHeight, stripOrientation, 1, BitImageType.simple);
            }

            formForStripes.setImage(result);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void convertValues()
        {
            numberOfSin1Value = Convert.ToDouble(numberOfSin1.Text);
            numberOfSin2Value = Convert.ToDouble(numberOfSin2.Text);

            phaseShift1Value = Convert.ToInt16(phaseShift1.Text);
            phaseShift2Value = Convert.ToInt16(phaseShift2.Text);
            phaseShift3Value = Convert.ToInt16(phaseShift3.Text);
            phaseShift4Value = Convert.ToInt16(phaseShift4.Text);
            phaseShift5Value = Convert.ToInt16(phaseShift5.Text);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void okClicked(object sender, EventArgs e)
        {
            imageNumber = 0;
            currentBitImageNumber = 0;
            imagesContainer = new Image[8];
            updateInitialImage();

            ImageGetter.sharedInstance().imageReceived += imageTaken;
            ImageGetter.sharedInstance().getImage();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void imageTaken(Image newImage)
        {
            if (stripeType != StripeType.bits)
            {
                imageNumber++;

                if (oneImageOfSeries != null)
                {
                    oneImageOfSeries(newImage, imageNumber);
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    if (imagesContainer[i] == null)
                    {
                        imagesContainer[i] = newImage;
                        break;
                    }
                }

                currentBitImageNumber++;

                if (imagesContainer[7] == null)
                {
                    
                }
                else
                {
                    Image result = SinClass1.bit8sin(imagesContainer);
                    imageNumber++;

                    if (oneImageOfSeries != null)
                    {
                        oneImageOfSeries(result, imageNumber);
                    }

                    currentBitImageNumber = 0;
                    imagesContainer = new Image[8];
                }
            }           

            if (imageNumber >= numberOfImageInSeries)
            {
                ImageGetter.sharedInstance().imageReceived -= imageTaken;
            }
            else
            {
                if (numberOfImageInSeries == 8)
                {
                    if (imageNumber == 0)
                    {
                        drawLines(numberOfSin1Value / 10, phaseShift1Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 1)
                    {
                        drawLines(numberOfSin1Value/10, phaseShift2Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 2)
                    {
                        drawLines(numberOfSin1Value/10, phaseShift3Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 3)
                    {
                        drawLines(numberOfSin1Value/10, phaseShift4Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 4)
                    {
                        drawLines(numberOfSin2Value/10, phaseShift1Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 5)
                    {
                        drawLines(numberOfSin2Value/10, phaseShift2Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 6)
                    {
                        drawLines(numberOfSin2Value/10, phaseShift3Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 7)
                    {
                        drawLines(numberOfSin2Value/10, phaseShift4Value, imageWidth, imageHeight, stripOrientation);
                    }
                }
                else if (numberOfImageInSeries == 10)
                {
                    if (imageNumber == 1)
                    {
                        drawLines(numberOfSin1Value / 10, phaseShift2Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 2)
                    {
                        drawLines(numberOfSin1Value / 10, phaseShift3Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 3)
                    {
                        drawLines(numberOfSin1Value / 10, phaseShift4Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 4)
                    {
                        drawLines(numberOfSin1Value / 10, phaseShift5Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 5)
                    {
                        drawLines(numberOfSin2Value / 10, phaseShift1Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 6)
                    {
                        drawLines(numberOfSin2Value / 10, phaseShift2Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 7)
                    {
                        drawLines(numberOfSin2Value / 10, phaseShift3Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 8)
                    {
                        drawLines(numberOfSin2Value / 10, phaseShift4Value, imageWidth, imageHeight, stripOrientation);
                    }
                    else if (imageNumber == 9)
                    {
                        drawLines(numberOfSin2Value / 10, phaseShift5Value, imageWidth, imageHeight, stripOrientation);
                    }
                }

                ImageGetter.sharedInstance().getImage();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void drawLines(double N_sin, double phaseShift, int max_x, int max_y, int XY)
        {
            Bitmap result = null;

            if (stripeType == StripeType.sine)
            {
                result = SinClass1.drawSine(N_sin, phaseShift, max_x, max_y, XY);
            }
            else if (stripeType == StripeType.lines)
            {
                result = SinClass1.drawLines(N_sin, phaseShift, max_x, max_y, XY);
            }
            else if (stripeType == StripeType.dithered)
            {
                result = SinClass1.drawDitheredLines(N_sin, phaseShift, max_x, max_y, XY);
            }
            else if (stripeType == StripeType.bits)
            {
                result = SinClass1.drawBitImage(N_sin, phaseShift, max_x, max_y, XY, currentBitImageNumber + 1, BitImageType.simple);
            }

            formForStripes.setImage(result);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                stripOrientation = 0;
                updateInitialImage();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                stripOrientation = 1;
                updateInitialImage();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                stripeType = 0;
                updateInitialImage();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                stripeType = StripeType.lines;
                updateInitialImage();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BackgroundImagesGeneratorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            formForStripes.Close();
            ImageGetter.sharedInstance().imageReceived -= imageTaken;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                stripeType = StripeType.dithered;
                updateInitialImage();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void sineParameterSaveButton_Click(object sender, EventArgs e)
        {
            updateInitialImage();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            phaseShift5.Visible = true;
            numberOfImageInSeries = 10;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            phaseShift5.Visible = false;
            numberOfImageInSeries = 8;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                stripeType = StripeType.bits;
                updateInitialImage();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
