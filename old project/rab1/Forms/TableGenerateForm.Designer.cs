namespace rab1.Forms
{
    partial class TableGenerateForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.sineNumberTextBox1 = new System.Windows.Forms.TextBox();
            this.sineNumberTextBox2 = new System.Windows.Forms.TextBox();
            this.periodsNumberTextBox = new System.Windows.Forms.TextBox();
            this.cutLevelTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(134, 227);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Построить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buildClicked);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(110, 180);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(125, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "По форме 11 кадра";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Число синусоид 1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Число синусоид 2:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Число периодов:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Уровень обрезания (N точек)";
            // 
            // sineNumberTextBox1
            // 
            this.sineNumberTextBox1.Location = new System.Drawing.Point(229, 49);
            this.sineNumberTextBox1.Name = "sineNumberTextBox1";
            this.sineNumberTextBox1.Size = new System.Drawing.Size(100, 20);
            this.sineNumberTextBox1.TabIndex = 6;
            // 
            // sineNumberTextBox2
            // 
            this.sineNumberTextBox2.Location = new System.Drawing.Point(229, 76);
            this.sineNumberTextBox2.Name = "sineNumberTextBox2";
            this.sineNumberTextBox2.Size = new System.Drawing.Size(100, 20);
            this.sineNumberTextBox2.TabIndex = 7;
            // 
            // periodsNumberTextBox
            // 
            this.periodsNumberTextBox.Location = new System.Drawing.Point(229, 103);
            this.periodsNumberTextBox.Name = "periodsNumberTextBox";
            this.periodsNumberTextBox.Size = new System.Drawing.Size(100, 20);
            this.periodsNumberTextBox.TabIndex = 8;
            // 
            // cutLevelTextBox
            // 
            this.cutLevelTextBox.Location = new System.Drawing.Point(229, 130);
            this.cutLevelTextBox.Name = "cutLevelTextBox";
            this.cutLevelTextBox.Size = new System.Drawing.Size(100, 20);
            this.cutLevelTextBox.TabIndex = 9;
            // 
            // TableGenerateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 276);
            this.Controls.Add(this.cutLevelTextBox);
            this.Controls.Add(this.periodsNumberTextBox);
            this.Controls.Add(this.sineNumberTextBox2);
            this.Controls.Add(this.sineNumberTextBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "TableGenerateForm";
            this.Text = "Укажите параметры";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox sineNumberTextBox1;
        private System.Windows.Forms.TextBox sineNumberTextBox2;
        private System.Windows.Forms.TextBox periodsNumberTextBox;
        private System.Windows.Forms.TextBox cutLevelTextBox;
    }
}