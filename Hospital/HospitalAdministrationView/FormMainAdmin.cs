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

        private readonly IMainService serviceT;

        private readonly IMedicationService serviceM;

        public FormMainAdmin(IMainService serviceT, IMedicationService serviceM)
        {
            InitializeComponent();
            this.serviceT = serviceT;
            this.serviceM = serviceM;
        }

        private void LoadData()
        {
            try
            {
                List<TreatmentViewModel> listT = serviceT.GetList();
                if (listT != null)
                {
                    dataGridViewT.DataSource = listT;
                    dataGridViewT.Columns[0].Visible = false;
                    dataGridViewT.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                List<MedicationViewModel> listM = serviceM.GetList();
                if (listM != null)
                {
                    dataGridViewM.DataSource = listM;
                    dataGridViewM.Columns[0].Visible = false;
                    dataGridViewM.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
            var form = Container.Resolve<FormPrescriptions>();
            form.ShowDialog();
        }

        private void лекарстваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormMedications>();
            form.ShowDialog();
        }

        private void отчетToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void buttonCreateRequest_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormRequest>();
            form.ShowDialog();
        }

        private void FormMainAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
