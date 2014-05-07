namespace Interferometry.forms
{
    partial class Faza2Pi
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sineNumbers1 = new System.Windows.Forms.TextBox();
            this.sineNumbers2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Число периодов синусоид 1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Число периодов синусоид 2:";
            // 
            // sineNumbers1
            // 
            this.sineNumbers1.Location = new System.Drawing.Point(180, 12);
            this.sineNumbers1.Name = "sineNumbers1";
            this.sineNumbers1.Size = new System.Drawing.Size(100, 20);
            this.sineNumbers1.TabIndex = 4;
            // 
            // sineNumbers2
            // 
            this.sineNumbers2.Location = new System.Drawing.Point(180, 49);
            this.sineNumbers2.Name = "sineNumbers2";
            this.sineNumbers2.Size = new System.Drawing.Size(100, 20);
            this.sineNumbers2.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(432, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 13);
            this.label10.TabIndex = 22;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(12, 91);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(268, 23);
            this.okButton.TabIndex = 23;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // TableFaza
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 126);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.sineNumbers2);
            this.Controls.Add(this.sineNumbers1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "TableFaza";
            this.Text = "Восстановление полной фазы";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox sineNumbers1;
        private System.Windows.Forms.TextBox sineNumbers2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button okButton;
    }
}