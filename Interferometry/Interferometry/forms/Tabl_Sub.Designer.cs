namespace Interferometry.forms
{
    partial class Tabl_Sub
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
            this.textBox1_sub = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2_sub = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3_sub = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1_sub
            // 
            this.textBox1_sub.Location = new System.Drawing.Point(12, 12);
            this.textBox1_sub.Name = "textBox1_sub";
            this.textBox1_sub.Size = new System.Drawing.Size(51, 20);
            this.textBox1_sub.TabIndex = 13;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(111, 64);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 14;
            this.okButton.Text = "Вычесть";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "-";
            // 
            // textBox2_sub
            // 
            this.textBox2_sub.Location = new System.Drawing.Point(111, 12);
            this.textBox2_sub.Name = "textBox2_sub";
            this.textBox2_sub.Size = new System.Drawing.Size(46, 20);
            this.textBox2_sub.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(176, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "=";
            // 
            // textBox3_sub
            // 
            this.textBox3_sub.Location = new System.Drawing.Point(206, 12);
            this.textBox3_sub.Name = "textBox3_sub";
            this.textBox3_sub.Size = new System.Drawing.Size(46, 20);
            this.textBox3_sub.TabIndex = 18;
            // 
            // Tabl_Sub
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 99);
            this.Controls.Add(this.textBox3_sub);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2_sub);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.textBox1_sub);
            this.Name = "Tabl_Sub";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1_sub;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2_sub;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox3_sub;
    }
}