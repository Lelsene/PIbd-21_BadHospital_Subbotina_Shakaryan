namespace HospitalAdministrationView
{
    partial class FormMainAdmin
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
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.buttonRef = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.рецептыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.лекарстваToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отчетToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonCreateRequest = new System.Windows.Forms.Button();
            this.buttonUpdRequest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(12, 27);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(587, 326);
            this.dataGridView.TabIndex = 2;
            // 
            // buttonRef
            // 
            this.buttonRef.Location = new System.Drawing.Point(612, 103);
            this.buttonRef.Name = "buttonRef";
            this.buttonRef.Size = new System.Drawing.Size(131, 32);
            this.buttonRef.TabIndex = 7;
            this.buttonRef.Text = "Обновить список";
            this.buttonRef.UseVisualStyleBackColor = true;
            this.buttonRef.Click += new System.EventHandler(this.buttonRef_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.рецептыToolStripMenuItem,
            this.лекарстваToolStripMenuItem,
            this.отчетToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(755, 24);
            this.menuStrip.TabIndex = 8;
            this.menuStrip.Text = "menuStrip";
            // 
            // рецептыToolStripMenuItem
            // 
            this.рецептыToolStripMenuItem.Name = "рецептыToolStripMenuItem";
            this.рецептыToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.рецептыToolStripMenuItem.Text = "Рецепты";
            this.рецептыToolStripMenuItem.Click += new System.EventHandler(this.рецептыToolStripMenuItem_Click);
            // 
            // лекарстваToolStripMenuItem
            // 
            this.лекарстваToolStripMenuItem.Name = "лекарстваToolStripMenuItem";
            this.лекарстваToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.лекарстваToolStripMenuItem.Text = "Лекарства";
            this.лекарстваToolStripMenuItem.Click += new System.EventHandler(this.лекарстваToolStripMenuItem_Click);
            // 
            // отчетToolStripMenuItem
            // 
            this.отчетToolStripMenuItem.Name = "отчетToolStripMenuItem";
            this.отчетToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.отчетToolStripMenuItem.Text = "Отчет";
            this.отчетToolStripMenuItem.Click += new System.EventHandler(this.отчетToolStripMenuItem_Click);
            // 
            // buttonCreateRequest
            // 
            this.buttonCreateRequest.Location = new System.Drawing.Point(612, 27);
            this.buttonCreateRequest.Name = "buttonCreateRequest";
            this.buttonCreateRequest.Size = new System.Drawing.Size(131, 32);
            this.buttonCreateRequest.TabIndex = 9;
            this.buttonCreateRequest.Text = "Создать заявку";
            this.buttonCreateRequest.UseVisualStyleBackColor = true;
            this.buttonCreateRequest.Click += new System.EventHandler(this.buttonCreateRequest_Click);
            // 
            // buttonUpdRequest
            // 
            this.buttonUpdRequest.Location = new System.Drawing.Point(612, 65);
            this.buttonUpdRequest.Name = "buttonUpdRequest";
            this.buttonUpdRequest.Size = new System.Drawing.Size(131, 32);
            this.buttonUpdRequest.TabIndex = 10;
            this.buttonUpdRequest.Text = "Изменить заявку";
            this.buttonUpdRequest.UseVisualStyleBackColor = true;
            // 
            // FormMainAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 369);
            this.Controls.Add(this.buttonUpdRequest);
            this.Controls.Add(this.buttonCreateRequest);
            this.Controls.Add(this.buttonRef);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormMainAdmin";
            this.Text = "Больница";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMainAdmin_FormClosed);
            this.Load += new System.EventHandler(this.buttonRef_Click);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button buttonRef;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem рецептыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem лекарстваToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отчетToolStripMenuItem;
        private System.Windows.Forms.Button buttonCreateRequest;
        private System.Windows.Forms.Button buttonUpdRequest;
    }
}