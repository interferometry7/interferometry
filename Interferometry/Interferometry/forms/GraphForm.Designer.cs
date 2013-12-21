namespace Interferometry.forms
{
    partial class GraphForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.cb = new System.Windows.Forms.ComboBox();
            this.vc = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.vc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // cb
            // 
            this.cb.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cb.DisplayMember = "0";
            this.cb.FormattingEnabled = true;
            this.cb.Items.AddRange(new object[] {
            "Красный канал",
            "Зеленый канал",
            "Синий канал"});
            this.cb.Location = new System.Drawing.Point(361, 641);
            this.cb.Name = "cb";
            this.cb.Size = new System.Drawing.Size(219, 21);
            this.cb.TabIndex = 5;
            this.cb.ValueMember = "0";
            // 
            // vc
            // 
            this.vc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.vc.ChartAreas.Add(chartArea1);
            this.vc.Location = new System.Drawing.Point(44, 318);
            this.vc.Name = "vc";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.vc.Series.Add(series1);
            this.vc.Size = new System.Drawing.Size(864, 300);
            this.vc.TabIndex = 4;
            this.vc.Text = "chart2";
            // 
            // chart
            // 
            this.chart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea2);
            this.chart.Location = new System.Drawing.Point(44, 12);
            this.chart.Name = "chart";
            series2.ChartArea = "ChartArea1";
            series2.Name = "Series1";
            this.chart.Series.Add(series2);
            this.chart.Size = new System.Drawing.Size(864, 300);
            this.chart.TabIndex = 3;
            this.chart.Text = "chart1";
            // 
            // GraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 675);
            this.Controls.Add(this.cb);
            this.Controls.Add(this.vc);
            this.Controls.Add(this.chart);
            this.Name = "GraphForm";
            this.Text = "GraphForm";
            ((System.ComponentModel.ISupportInitialize)(this.vc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cb;
        private System.Windows.Forms.DataVisualization.Charting.Chart vc;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;

    }
}