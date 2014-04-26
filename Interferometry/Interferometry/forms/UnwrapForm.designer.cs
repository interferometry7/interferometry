namespace Interferometry.Forms
{
    partial class UnwrapForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.sineNumbers1 = new System.Windows.Forms.TextBox();
            this.sineNumbers2 = new System.Windows.Forms.TextBox();
            this.periodsNumber = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Число периодов синусоид 1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Число периодов синусоид 2:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Число периодов:";
            // 
            // sineNumbers1
            // 
            this.sineNumbers1.Location = new System.Drawing.Point(236, 29);
            this.sineNumbers1.Name = "sineNumbers1";
            this.sineNumbers1.Size = new System.Drawing.Size(100, 20);
            this.sineNumbers1.TabIndex = 3;
            // 
            // sineNumbers2
            // 
            this.sineNumbers2.Location = new System.Drawing.Point(236, 55);
            this.sineNumbers2.Name = "sineNumbers2";
            this.sineNumbers2.Size = new System.Drawing.Size(100, 20);
            this.sineNumbers2.TabIndex = 4;
            // 
            // periodsNumber
            // 
            this.periodsNumber.Location = new System.Drawing.Point(236, 81);
            this.periodsNumber.Name = "periodsNumber";
            this.periodsNumber.Size = new System.Drawing.Size(100, 20);
            this.periodsNumber.TabIndex = 5;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(152, 179);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "Применить";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButtonClicked);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(236, 133);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Сдвиг по первой координате";
            // 
            // UnwrapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 217);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.periodsNumber);
            this.Controls.Add(this.sineNumbers2);
            this.Controls.Add(this.sineNumbers1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "UnwrapForm";
            this.Text = "Расшифровка";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox sineNumbers1;
        private System.Windows.Forms.TextBox sineNumbers2;
        private System.Windows.Forms.TextBox periodsNumber;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
    }
}