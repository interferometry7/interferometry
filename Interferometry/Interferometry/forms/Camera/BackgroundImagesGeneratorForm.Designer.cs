﻿namespace rab1
{
    partial class BackgroundImagesGeneratorForm
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
            this.numberOfSin1 = new System.Windows.Forms.TextBox();
            this.numberOfSin2 = new System.Windows.Forms.TextBox();
            this.phaseShift1 = new System.Windows.Forms.TextBox();
            this.phaseShift2 = new System.Windows.Forms.TextBox();
            this.phaseShift3 = new System.Windows.Forms.TextBox();
            this.phaseShift4 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sineParameterSaveButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.phaseShift5 = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.okButton = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Число синусоид";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Фазовый сдвиг";
            // 
            // numberOfSin1
            // 
            this.numberOfSin1.Location = new System.Drawing.Point(134, 12);
            this.numberOfSin1.Name = "numberOfSin1";
            this.numberOfSin1.Size = new System.Drawing.Size(55, 20);
            this.numberOfSin1.TabIndex = 2;
            this.numberOfSin1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // numberOfSin2
            // 
            this.numberOfSin2.Location = new System.Drawing.Point(222, 12);
            this.numberOfSin2.Name = "numberOfSin2";
            this.numberOfSin2.Size = new System.Drawing.Size(55, 20);
            this.numberOfSin2.TabIndex = 3;
            this.numberOfSin2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // phaseShift1
            // 
            this.phaseShift1.Location = new System.Drawing.Point(121, 14);
            this.phaseShift1.Name = "phaseShift1";
            this.phaseShift1.Size = new System.Drawing.Size(55, 20);
            this.phaseShift1.TabIndex = 4;
            this.phaseShift1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // phaseShift2
            // 
            this.phaseShift2.Location = new System.Drawing.Point(182, 14);
            this.phaseShift2.Name = "phaseShift2";
            this.phaseShift2.Size = new System.Drawing.Size(55, 20);
            this.phaseShift2.TabIndex = 5;
            this.phaseShift2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // phaseShift3
            // 
            this.phaseShift3.Location = new System.Drawing.Point(243, 14);
            this.phaseShift3.Name = "phaseShift3";
            this.phaseShift3.Size = new System.Drawing.Size(55, 20);
            this.phaseShift3.TabIndex = 6;
            this.phaseShift3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // phaseShift4
            // 
            this.phaseShift4.Location = new System.Drawing.Point(304, 14);
            this.phaseShift4.Name = "phaseShift4";
            this.phaseShift4.Size = new System.Drawing.Size(55, 20);
            this.phaseShift4.TabIndex = 7;
            this.phaseShift4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.sineParameterSaveButton);
            this.panel1.Controls.Add(this.numberOfSin2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.numberOfSin1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(433, 45);
            this.panel1.TabIndex = 8;
            // 
            // sineParameterSaveButton
            // 
            this.sineParameterSaveButton.Location = new System.Drawing.Point(306, 6);
            this.sineParameterSaveButton.Name = "sineParameterSaveButton";
            this.sineParameterSaveButton.Size = new System.Drawing.Size(116, 32);
            this.sineParameterSaveButton.TabIndex = 4;
            this.sineParameterSaveButton.Text = "сохранить";
            this.sineParameterSaveButton.UseVisualStyleBackColor = true;
            this.sineParameterSaveButton.Click += new System.EventHandler(this.sineParameterSaveButton_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.phaseShift5);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.phaseShift1);
            this.panel2.Controls.Add(this.phaseShift4);
            this.panel2.Controls.Add(this.phaseShift2);
            this.panel2.Controls.Add(this.phaseShift3);
            this.panel2.Location = new System.Drawing.Point(12, 75);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(433, 54);
            this.panel2.TabIndex = 9;
            // 
            // phaseShift5
            // 
            this.phaseShift5.Location = new System.Drawing.Point(365, 14);
            this.phaseShift5.Name = "phaseShift5";
            this.phaseShift5.Size = new System.Drawing.Size(55, 20);
            this.phaseShift5.TabIndex = 8;
            this.phaseShift5.Visible = false;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.radioButton2);
            this.panel3.Controls.Add(this.radioButton1);
            this.panel3.Location = new System.Drawing.Point(12, 144);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(127, 100);
            this.panel3.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Ориентация полос";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(14, 68);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(32, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Y";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(14, 40);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(32, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "X";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(14, 41);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(40, 17);
            this.radioButton3.TabIndex = 11;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Sin";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.radioButton9);
            this.panel4.Controls.Add(this.radioButton6);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.radioButton5);
            this.panel4.Controls.Add(this.radioButton3);
            this.panel4.Location = new System.Drawing.Point(157, 144);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(155, 142);
            this.panel4.TabIndex = 13;
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(14, 89);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(72, 17);
            this.radioButton6.TabIndex = 15;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "дизеринг";
            this.radioButton6.UseVisualStyleBackColor = true;
            this.radioButton6.CheckedChanged += new System.EventHandler(this.radioButton6_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Тип полос";
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(14, 64);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(130, 17);
            this.radioButton5.TabIndex = 13;
            this.radioButton5.Text = "черно белые полосы";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(464, 254);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(145, 32);
            this.okButton.TabIndex = 14;
            this.okButton.Text = "Ок";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okClicked);
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.radioButton8);
            this.panel5.Controls.Add(this.radioButton7);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Location = new System.Drawing.Point(464, 12);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(145, 117);
            this.panel5.TabIndex = 15;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(6, 82);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(31, 17);
            this.radioButton8.TabIndex = 2;
            this.radioButton8.Text = "5";
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.CheckedChanged += new System.EventHandler(this.radioButton8_CheckedChanged);
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Checked = true;
            this.radioButton7.Location = new System.Drawing.Point(6, 58);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(31, 17);
            this.radioButton7.TabIndex = 1;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "4";
            this.radioButton7.UseVisualStyleBackColor = true;
            this.radioButton7.CheckedChanged += new System.EventHandler(this.radioButton7_CheckedChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(135, 38);
            this.label5.TabIndex = 0;
            this.label5.Text = "Количество снимков для каждого числа синусоид";
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Location = new System.Drawing.Point(14, 112);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(139, 17);
            this.radioButton9.TabIndex = 16;
            this.radioButton9.TabStop = true;
            this.radioButton9.Text = "битовые изображения";
            this.radioButton9.UseVisualStyleBackColor = true;
            this.radioButton9.CheckedChanged += new System.EventHandler(this.radioButton9_CheckedChanged);
            // 
            // BackgroundImagesGeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 295);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BackgroundImagesGeneratorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Генерация фона";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BackgroundImagesGeneratorForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox numberOfSin1;
        private System.Windows.Forms.TextBox numberOfSin2;
        private System.Windows.Forms.TextBox phaseShift1;
        private System.Windows.Forms.TextBox phaseShift2;
        private System.Windows.Forms.TextBox phaseShift3;
        private System.Windows.Forms.TextBox phaseShift4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.Button sineParameterSaveButton;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox phaseShift5;
        private System.Windows.Forms.RadioButton radioButton9;
    }
}