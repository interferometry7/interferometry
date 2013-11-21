using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace rab1.Forms
{
    public partial class HystogrammForm : Form
    {
        public HystogrammForm(int[,] someArray, int width, int height)
        {
            InitializeComponent();
            graphChart.Palette = ChartColorPalette.Grayscale;
            graphChart.Titles.Add("Гистограмма");

            List<object> labels = new List<object>(width);

            int maxValue = 0;
            int minValue = int.MaxValue;

            for (int i = 0; i < width; i++)
            {             
                for (int j = 0; j < height; j++)
                {
                    int currentValue = someArray[i, j];

                    if (maxValue < currentValue)
                    {
                        maxValue = currentValue;
                    }

                    if (minValue > currentValue)
                    {
                        minValue = currentValue;
                    }
                }
            }

            int [] result = new int[maxValue + 1];

          
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int currentValue = someArray[i, j];

                    if (currentValue != 0)
                    {
                        result[currentValue]++;
                    }
                }
            }

            graphChart.Series.Clear();

            for (int i = 0; i < 10/*result.Length*/; i++)
            {
                Series series = new Series(Convert.ToString(i), 5);
                series.Points.AddXY(i, result[i]);
                series.MarkerStyle = MarkerStyle.Square;
                series.ChartType = SeriesChartType.Line;
                graphChart.Series.Add(series);
            }
        }
    }
}
