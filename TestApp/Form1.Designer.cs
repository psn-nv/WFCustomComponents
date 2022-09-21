namespace TestApp
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.simpleCheckListComboBox1 = new WFSimpleCheckListComboBox.SimpleCheckListComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // simpleCheckListComboBox1
            // 
            this.simpleCheckListComboBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.simpleCheckListComboBox1.DisplayMember = "";
            this.simpleCheckListComboBox1.Location = new System.Drawing.Point(12, 12);
            this.simpleCheckListComboBox1.MaxDropDownItems = 5;
            this.simpleCheckListComboBox1.Name = "simpleCheckListComboBox1";
            this.simpleCheckListComboBox1.Size = new System.Drawing.Size(427, 23);
            this.simpleCheckListComboBox1.TabIndex = 2;
            this.simpleCheckListComboBox1.ValuesSeparator = ", ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 127);
            this.Controls.Add(this.simpleCheckListComboBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Click += new System.EventHandler(this.Form1_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private WFSimpleCheckListComboBox.SimpleCheckListComboBox simpleCheckListComboBox1;
    }
}

