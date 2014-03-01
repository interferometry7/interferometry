namespace Interferometry.forms
{
    partial class BackkgroundStripesForm
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
            this.stripesImage = new Interferometry.CustomPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.stripesImage)).BeginInit();
            this.SuspendLayout();
            // 
            // stripesImage
            // 
            this.stripesImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stripesImage.Location = new System.Drawing.Point(0, 0);
            this.stripesImage.Margin = new System.Windows.Forms.Padding(0);
            this.stripesImage.Name = "stripesImage";
            this.stripesImage.Size = new System.Drawing.Size(593, 434);
            this.stripesImage.TabIndex = 0;
            this.stripesImage.TabStop = false;
            // 
            // BackkgroundStripesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 435);
            this.Controls.Add(this.stripesImage);
            this.Name = "BackkgroundStripesForm";
            this.Text = "BackkgroundStripesForm";
            ((System.ComponentModel.ISupportInitialize)(this.stripesImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomPictureBox stripesImage;
    }
}