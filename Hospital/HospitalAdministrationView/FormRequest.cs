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

        public FormRequest(IMainService service, IRequestService serviceR, IMedicationService serviceM)
        {
            InitializeComponent();
            this.service = service;
            this.serviceR = serviceR;
            this.serviceM = serviceM;
        }

        private void FormRequest_Load(object sender, EventArgs e)
        {
            try
            {
                List<MedicationViewModel> listM = serviceM.GetList();
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
                int Requestid = serviceR.AddElement(new RequestBindingModel
                {
                    RequestName = textBoxRequest.Text,
                    Date = DateTime.Now
                });

                service.MedicationRefill(new RequestMedicationBindingModel
                {
                    MedicationId = Convert.ToInt32(comboBoxMedication.SelectedValue),
                    RequestId = Requestid,
                    CountMedications = Convert.ToInt32(textBoxCount.Text)
                });

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
