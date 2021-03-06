﻿using System;
using System.Windows.Forms;
using Unity;

namespace HospitalAdministrationView
{
    public partial class FormAuthorization : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public FormAuthorization()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxPass.Text == "123")
            {
                var form = Container.Resolve<FormMain>();
                this.Hide();
                form.Show();
            }
            else
            {
                MessageBox.Show("Неправильный пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
