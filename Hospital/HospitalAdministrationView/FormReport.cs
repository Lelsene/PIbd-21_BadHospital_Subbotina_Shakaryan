using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using Microsoft.Reporting.WinForms;
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
                string path = "C:\\Users\\Шонова\\Desktop\\AdminReport.pdf";
                serviceRep.SaveReport(new ReportBindingModel
                {
                    FileName = path,
                    DateFrom = dateTimePickerFrom.Value,
                    DateTo = dateTimePickerTo.Value
                });
                service.SendEmail("lelsene@mail.ru", "Отчет по заявкам и лечениям", "", "C:\\Users\\Шонова\\Desktop\\AdminReport.pdf");
                MessageBox.Show("Отчет отправлен", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void buttonReport_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var list = serviceRep.GetReport(new ReportBindingModel
            {
                FileName = "",
                DateFrom = dateTimePickerFrom.Value,
                DateTo = dateTimePickerTo.Value
            });

            ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                        "c " + dateTimePickerFrom.Value.ToShortDateString() +
                                        " по " + dateTimePickerTo.Value.ToShortDateString());

            reportViewer.LocalReport.SetParameters(parameter);
            reportViewer.LocalReport.DataSources.Clear();
            ReportDataSource source = new ReportDataSource("DataSet", list);
            reportViewer.LocalReport.DataSources.Add(source);
            reportViewer.RefreshReport();
        }
    }
}
