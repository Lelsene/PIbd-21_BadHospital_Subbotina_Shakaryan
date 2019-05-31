namespace HospitalAdministrationView
{
    partial class FormRequest
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
            this.textBoxRequest = new System.Windows.Forms.TextBox();
            this.labelRequest = new System.Windows.Forms.Label();
            this.labelCount = new System.Windows.Forms.Label();
            this.comboBoxMedication = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxCount = new System.Windows.Forms.TextBox();
            this.labelMedication = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxRequest
            // 
            this.textBoxRequest.Location = new System.Drawing.Point(78, 12);
            this.textBoxRequest.Name = "textBoxRequest";
            this.textBoxRequest.Size = new System.Drawing.Size(217, 20);
            this.textBoxRequest.TabIndex = 24;
            // 
            // labelRequest
            // 
            this.labelRequest.AutoSize = true;
            this.labelRequest.Location = new System.Drawing.Point(15, 15);
            this.labelRequest.Name = "labelRequest";
            this.labelRequest.Size = new System.Drawing.Size(60, 13);
            this.labelRequest.TabIndex = 17;
            this.labelRequest.Text = "Название:";
            // 
            // labelCount
            // 
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(6, 74);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(69, 13);
            this.labelCount.TabIndex = 20;
            this.labelCount.Text = "Количество:";
            // 
            // comboBoxMedication
            // 
            this.comboBoxMedication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMedication.FormattingEnabled = true;
            this.comboBoxMedication.Location = new System.Drawing.Point(78, 41);
            this.comboBoxMedication.Name = "comboBoxMedication";
            this.comboBoxMedication.Size = new System.Drawing.Size(217, 21);
            this.comboBoxMedication.TabIndex = 19;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(220, 97);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 23;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(139, 97);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 22;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxCount
            // 
            this.textBoxCount.Location = new System.Drawing.Point(78, 71);
            this.textBoxCount.Name = "textBoxCount";
            this.textBoxCount.Size = new System.Drawing.Size(217, 20);
            this.textBoxCount.TabIndex = 21;
            // 
            // labelMedication
            // 
            this.labelMedication.AutoSize = true;
            this.labelMedication.Location = new System.Drawing.Point(9, 44);
            this.labelMedication.Name = "labelMedication";
            this.labelMedication.Size = new System.Drawing.Size(65, 13);
            this.labelMedication.TabIndex = 18;
            this.labelMedication.Text = "Лекарство:";
            // 
            // FormRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 128);
            this.Controls.Add(this.textBoxRequest);
            this.Controls.Add(this.labelRequest);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.comboBoxMedication);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxCount);
            this.Controls.Add(this.labelMedication);
            this.Name = "FormRequest";
            this.Text = "Заявка";
            this.Load += new System.EventHandler(this.FormRequest_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxRequest;
        private System.Windows.Forms.Label labelRequest;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.ComboBox comboBoxMedication;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxCount;
        private System.Windows.Forms.Label labelMedication;
    }
}