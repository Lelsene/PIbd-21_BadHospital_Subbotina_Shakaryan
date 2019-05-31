using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Windows.Forms;
using Unity;

namespace HospitalAdministrationView
{
    public partial class FormMedication : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IMedicationService service;

        private int? id;

        public FormMedication(IMedicationService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormMedication_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    MedicationViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.Name;
                        textBoxPrice.Text = view.Price.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните Название", "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new MedicationBindingModel
                    {
                        Id = id.Value,
                        Name = textBoxName.Text,
                        Price = Int32.Parse(textBoxPrice.Text),
                        Count = 0
                    });
                }
                else
                {
                    service.AddElement(new MedicationBindingModel
                    {
                        Name = textBoxName.Text,
                        Price = Int32.Parse(textBoxPrice.Text),
                        Count = 0
                    });
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
