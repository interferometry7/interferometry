using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace rab1
{
    public partial class GraphForm : Form
    {
        public bool DrawUnwrup = false;
        private double vscmin = -1;
        private double vscmax = -1;
        private double hscmin = -1;
        private double hscmax = -1;
        private int[] rx;
        private int[] gx;
        private int[] bx;
        private int[] ry;
        private int[] gy;
        private int[] by;
        private int wd;
        private int ht;
        private int pos_x;
        private int pos_y;
        
        public GraphForm()
        {
            InitializeComponent();
            EnableSelection();
        }

        public GraphForm(int x1, int y1,int w1, int h1, int[] buf, int[] bufy, int[] buf1, int[] bufy1, int[] buf2, int[] bufy2)
        {
            InitializeComponent();
            EnableSelection();
            rx = buf;
            ry = bufy;
            gx = buf1;
            gy = bufy1;
            bx = buf2;
            by = bufy2;
            wd = w1;
            ht = h1;
            cb.SelectedIndex = 0;
            pos_x = x1;
            pos_y = y1;
            draw_chart(w1, h1, buf, bufy);
        }

        private void draw_chart(int w1, int h1, int[] buf, int[] bufy)
         {
            chart.Series.Clear();
            vc.Series.Clear();
            Series ser = new Series();
            ser.Color = Color.FromArgb(255, 0, 0);
            ser.ChartType = SeriesChartType.Line;

            Series ser_p = new Series();
            ser_p.ChartType = SeriesChartType.Point;
            ser_p.MarkerSize = 8;
            ser_p.MarkerStyle = MarkerStyle.Circle; 
            ser_p.Color = Color.Blue;


            Series ser_unw = new Series();
            ser_unw.Color = Color.FromArgb(0, 0, 255);
            ser_unw.ChartType = SeriesChartType.Line;

            int currentValue, add = 0, add1 = 0;

            for (int i = 1; i < w1; ++i)
            {

                currentValue = buf[i];
                ser.Points.AddXY(i, currentValue);
                currentValue += add;
                add1 = buf[i - 1] - currentValue + add;

                if (Math.Abs(add1) > 128)
                {
                    add += add1;
                    currentValue += add1;
                }

                ser_unw.Points.AddXY(i, currentValue);

                if (i == pos_x)
                {
                    ser_p.Points.AddXY(i, currentValue);
                }  
            }
      
            chart.Series.Add(ser);
            chart.Series.Add(ser_p);

            if (DrawUnwrup)
            {
                chart.Series.Add(ser_unw);
            }


            //////////////////vert
            Series ser1 = new Series();
            ser1.Color = Color.FromArgb(255, 0, 0);
            ser1.ChartType = SeriesChartType.Line;

            Series ser_unw1 = new Series();
            ser_unw1.Color = Color.FromArgb(0, 0, 255);
            ser_unw1.ChartType = SeriesChartType.Line;

            ser_p = new Series();
            ser_p.ChartType = SeriesChartType.Point;
            ser_p.MarkerSize = 8;
            ser_p.MarkerStyle = MarkerStyle.Circle;
            ser_p.Color = Color.Blue;

            currentValue = 0; add = 0; add1 = 0;
            for (int x = 1; x < h1; ++x)
            {

                currentValue = bufy[x];
                ser1.Points.AddXY(x, currentValue);
                currentValue += add;
                add1 = bufy[x - 1] - currentValue + add;
                if (Math.Abs(add1) > 128)
                {
                    add += add1;
                    currentValue += add1;
                }
                ser_unw1.Points.AddXY(x, currentValue);
                if (x == pos_y) ser_p.Points.AddXY(x, currentValue);  
            }

            vc.Series.Add(ser1);
            vc.Series.Add(ser_p);

            if (DrawUnwrup)
            {
                vc.Series.Add(ser_unw1);
            }      
        }

        private void GraphForm_Resize(object sender, EventArgs e)
        {
            chart.Top = 3;
            vc.Top = Height / 2 - 25;
            vc.Height = Height / 2 - 25;
            chart.Height = Height / 2 - 25;
        }


        private void EnableSelection()
        {
            this.chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            this.chart.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
            this.chart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            this.chart.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = false;
            this.vc.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            this.vc.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
            this.vc.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            this.vc.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = false;

            this.chart.AxisViewChanged += this._AxisViewChanged;
            this.vc.AxisViewChanged += this._AxisViewChanged;
        }

        private void _AxisViewChanged(object sender, ViewEventArgs e)
        {
            if (sender is Chart)
            {
                double mx, mn, interv;
                if(((Chart)sender).Name == "chart")
                {
                    mx = chart.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
                    mn = chart.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
                    if (Math.Abs(mx - hscmax)> 15 && Math.Abs(mn - hscmin)>15)
                    {
                        hscmax = mx;
                        hscmin = mn;
                        mn = (int)mn - (int)mn % 5;
                        mx = (int)mx - (int)mx % 5;
                        if (mx - mn < 5) mx = mn + 5;                     
                        interv = (int)Math.Abs(mx - mn) / 5;
                        interv = (int)interv - (int)interv % 5;
                        chart.ChartAreas[0].AxisX.ScaleView.Zoom(mn+1, mx+1);
                        chart.ChartAreas[0].AxisX.Interval = interv;
                    }
                
                }
                if (((Chart)sender).Name == "vc")
                {
                    mx = vc.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
                    mn = vc.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
                    if (Math.Abs(mx - vscmax) > 15 && Math.Abs(mn - vscmin) > 15)
                    {
                        vscmax = mx;
                        vscmin = mn;
                        mn = (int)mn - (int)mn % 5;
                        mx = (int)mx - (int)mx % 5;
                        if (mx - mn < 5) mx = mn + 5;
                        interv = (int)Math.Abs(mx - mn) / 5;
                        interv = (int)interv - (int)interv % 5;
                        vc.ChartAreas[0].AxisX.ScaleView.Zoom(mn + 1, mx + 1);
                        vc.ChartAreas[0].AxisX.Interval = interv;
                    }
                }
            }
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb.SelectedIndex == 0) draw_chart(wd, ht, rx, ry);
            if (cb.SelectedIndex == 1) draw_chart(wd, ht, gx, gy);
            if (cb.SelectedIndex == 2) draw_chart(wd, ht, bx, by);
        }
    }
}
