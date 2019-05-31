namespace HospitalAdministrationView
{
    partial class FormPrescriptionMedication
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
            this.comboBoxMedication = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxCount = new System.Windows.Forms.TextBox();
            this.labelCount = new System.Windows.Forms.Label();
            this.labelMedication = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxMedication
            // 
            this.comboBoxMedication.DisplayMember = "IngredientName";
            this.comboBoxMedication.FormattingEnabled = true;
            this.comboBoxMedication.Location = new System.Drawing.Point(84, 12);
            this.comboBoxMedication.Name = "comboBoxMedication";
            this.comboBoxMedication.Size = new System.Drawing.Size(267, 21);
            this.comboBoxMedication.TabIndex = 18;
            this.comboBoxMedication.ValueMember = "Id";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(268, 69);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(83, 22);
            this.buttonCancel.TabIndex = 17;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(179, 69);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(83, 22);
            this.buttonSave.TabIndex = 16;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxCount
            // 
            this.textBoxCount.Location = new System.Drawing.Point(84, 43);
            this.textBoxCount.Name = "textBoxCount";
            this.textBoxCount.Size = new System.Drawing.Size(267, 20);
            this.textBoxCount.TabIndex = 15;
            // 
            // labelCount
            // 
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(9, 46);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(69, 13);
            this.labelCount.TabIndex = 14;
            this.labelCount.Text = "Количество:";
            // 
            // labelMedication
            // 
            this.labelMedication.AutoSize = true;
            this.labelMedication.Location = new System.Drawing.Point(13, 15);
            this.labelMedication.Name = "labelMedication";
            this.labelMedication.Size = new System.Drawing.Size(65, 13);
            this.labelMedication.TabIndex = 13;
            this.labelMedication.Text = "Лекарство:";
            // 
            // FormPrescriptionMedication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 97);
            this.Controls.Add(this.comboBoxMedication);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxCount);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.labelMedication);
            this.Name = "FormPrescriptionMedication";
            this.Text = "Лекарство рецепта";
            this.Load += new System.EventHandler(this.FormPrescriptionMedication_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxMedication;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxCount;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Label labelMedication;
    }
}