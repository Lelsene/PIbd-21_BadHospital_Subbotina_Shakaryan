using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using System;
using System.Windows.Forms;

namespace HospitalAdministrationView
{
    public partial class FormReport : Form
    {
        private readonly IReportService serviceRep;

        private readonly IMainService service;

        public FormReport(IReportService serviceRep, IMainService service)
        {
            InitializeComponent();
            this.serviceRep = serviceRep;
            this.service = service;
        }

        private void buttonMake_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string path = "C:\\Users\\Шонова\\Desktop\\PatientTreatments.pdf";
                serviceRep.SavePatientsTreatments(new ReportBindingModel
                {
                    FileName = path,
                    DateFrom = dateTimePickerFrom.Value,
                    DateTo = dateTimePickerTo.Value
                });
                service.SendEmail("lelsene@mail.ru", "Отчет по заявкам и лечениям", "", "C:\\Users\\Шонова\\Desktop\\PatientTreatments.pdf");
                MessageBox.Show("Отчет отправлен", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }
    }
}
