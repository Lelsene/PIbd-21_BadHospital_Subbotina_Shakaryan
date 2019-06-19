using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;

namespace HospitalAdministrationView
{
    public partial class FormRequest : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IMainService service;

        private readonly IRequestService serviceR;

        private readonly IMedicationService serviceM;

        private readonly IReportService serviceRep;

        public FormRequest(IMainService service, IRequestService serviceR, IMedicationService serviceM, IReportService serviceRep)
        {
            InitializeComponent();
            this.service = service;
            this.serviceR = serviceR;
            this.serviceM = serviceM;
            this.serviceRep = serviceRep;
        }

        private void FormRequest_Load(object sender, EventArgs e)
        {
            try
            {
                List<MedicationViewModel> listM = serviceM.GetMostList();
                if (listM != null)
                {
                    comboBoxMedication.DisplayMember = "Name";
                    comboBoxMedication.ValueMember = "Id";
                    comboBoxMedication.DataSource = listM;
                    comboBoxMedication.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(textBoxRequest.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBoxMedication.SelectedValue == null)
            {
                MessageBox.Show("Выберите лекарство", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DateTime date = DateTime.Now;
                string path = "C:\\Users\\Шонова\\Desktop\\RequestLoad.xls";
                int Requestid = serviceR.AddElement(new RequestBindingModel
                {
                    RequestName = textBoxRequest.Text,
                    Date = DateTime.Now
                });
                service.MedicationRefill(new RequestMedicationBindingModel
                {
                    MedicationId = Convert.ToInt32(comboBoxMedication.SelectedValue),
                    MedicationName = comboBoxMedication.Text,
                    RequestId = Requestid,
                    CountMedications = Convert.ToInt32(textBoxCount.Text)
                });
                serviceRep.SaveLoad(new ReportBindingModel
                {
                    FileName = path,
                    DateFrom = date,
                    DateTo = DateTime.Now
                }, -1);
                //service.SendEmail("lelsene@mail.ru", "Оповещение по заявке", "Заявка выполнена", path);
                MessageBox.Show("Сохранение прошло успешно", "Сообщение",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
