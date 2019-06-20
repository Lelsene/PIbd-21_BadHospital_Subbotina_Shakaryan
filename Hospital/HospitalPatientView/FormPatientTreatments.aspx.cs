using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using Microsoft.Reporting.WebForms;
using System;
using System.Web.UI;
using Unity;

namespace HospitalPatientView
{
    public partial class FormPatientTreatments : System.Web.UI.Page
    {
        private readonly IReportService serviceR = UnityConfig.Container.Resolve<IReportService>();

        private readonly IMainService service = UnityConfig.Container.Resolve<IMainService>();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonMake_Click(object sender, EventArgs e)
        {
            if (Calendar1.SelectedDate >= Calendar2.SelectedDate)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ScriptAllertDate", "<script>alert('Дата начала должна быть меньше даты окончания');</script>");
                return;
            }
            try
            {
                ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                            "c " + Calendar1.SelectedDate.ToShortDateString() +
                                            " по " + Calendar2.SelectedDate.ToShortDateString());
                ReportViewer.LocalReport.SetParameters(parameter);

                var dataSource = serviceR.GetTreatments(new ReportBindingModel
                {
                    DateFrom = Calendar1.SelectedDate,
                    DateTo = Calendar2.SelectedDate
                }, Convert.ToInt32(Session["PatientId"]));

                ReportDataSource source = new ReportDataSource("DataSet", dataSource);
                ReportViewer.LocalReport.DataSources.Add(source);
                ReportViewer.DataBind();

                string path = "C:\\Users\\Шонова\\Desktop\\Treatments.pdf";
                serviceR.SaveTreatments(new ReportBindingModel
                {
                    FileName = path,
                    DateFrom = Calendar1.SelectedDate,
                    DateTo = Calendar2.SelectedDate
                }, Convert.ToInt32(Session["PatientId"]));
                service.SendEmail(Session["PatientEmail"].ToString(), "Лечения пациента", "", path);
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ScriptAllert", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormMain.aspx");
        }
    }
}