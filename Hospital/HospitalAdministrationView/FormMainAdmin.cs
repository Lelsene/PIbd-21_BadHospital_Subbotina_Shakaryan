using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace HospitalAdministrationView
{
    public partial class FormMainAdmin : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private IRequestService service;

        public FormMainAdmin(IRequestService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void LoadData()
        {
            try
            {
                List<RequestViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridView.DataSource = list;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void рецептыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void лекарстваToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void отчетToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void buttonCreateRequest_Click(object sender, EventArgs e)
        {

        }

        private void FormMainAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
