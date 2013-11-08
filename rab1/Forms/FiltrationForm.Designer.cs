namespace rab1.Forms
{
    partial class FiltrationForm
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
            this.applyButton = new System.Windows.Forms.Button();
            this.orderTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.filtrationTypeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(232, 82);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 0;
            this.applyButton.Text = "Применить";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // orderTextBox
            // 
            this.orderTextBox.Location = new System.Drawing.Point(33, 83);
            this.orderTextBox.Name = "orderTextBox";
            this.orderTextBox.Size = new System.Drawing.Size(100, 20);
            this.orderTextBox.TabIndex = 1;
            this.orderTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.orderTextBox_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Порядок (целое число)";
            // 
            // filtrationTypeLabel
            // 
            this.filtrationTypeLabel.AutoSize = true;
            this.filtrationTypeLabel.Location = new System.Drawing.Point(140, 21);
            this.filtrationTypeLabel.Name = "filtrationTypeLabel";
            this.filtrationTypeLabel.Size = new System.Drawing.Size(90, 13);
            this.filtrationTypeLabel.TabIndex = 3;
            this.filtrationTypeLabel.Text = "Тип фильтрации";
            // 
            // FiltrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 137);
            this.Controls.Add(this.filtrationTypeLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.orderTextBox);
            this.Controls.Add(this.applyButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FiltrationForm";
            this.Text = "Фильтрация";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.TextBox orderTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label filtrationTypeLabel;
    }
}