using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;

namespace HospitalAdministrationView
{
    public partial class FormPrescriptionMedication : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public PrescriptionMedicationViewModel Model
        {
            set { model = value; }
            get
            {
                return model;
            }
        }

        public int Price
        {
            set { price = value; }
            get
            {
                return price;
            }
        }

        private readonly IMedicationService service;

        private PrescriptionMedicationViewModel model;

        private List<MedicationViewModel> list;

        private int price;

        public FormPrescriptionMedication(IMedicationService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormPrescriptionMedication_Load(object sender, EventArgs e)
        {
            try
            {
                list = service.GetList();
                if (list != null)
                {
                    comboBoxMedication.DisplayMember = "Name";
                    comboBoxMedication.ValueMember = "Id";
                    comboBoxMedication.DataSource = list;
                    comboBoxMedication.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
            if (model != null)
            {
                comboBoxMedication.Enabled = false;
                comboBoxMedication.SelectedValue = model.MedicationId;
                textBoxCount.Text = model.CountMedications.ToString();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxMedication.SelectedValue == null)
            {
                MessageBox.Show("Выберите лекарство", "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new PrescriptionMedicationViewModel
                    {
                        MedicationId = Convert.ToInt32(comboBoxMedication.SelectedValue),
                        MedicationName = comboBoxMedication.Text,
                        CountMedications = Convert.ToInt32(textBoxCount.Text)
                    };
                    foreach (var Medication in list)
                    {
                        if (Medication.Id == model.MedicationId)
                        {
                            price = Medication.Price;
                        }
                    }
                }
                else
                {
                    model.CountMedications = Convert.ToInt32(textBoxCount.Text);
                }
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
