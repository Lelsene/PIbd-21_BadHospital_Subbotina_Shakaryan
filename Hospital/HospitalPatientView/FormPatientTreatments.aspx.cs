using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using System;
using System.Web.UI;
using Unity;

namespace HospitalPatientView
{
    public partial class FormPatientTreatments : System.Web.UI.Page
    {
        private readonly IMainService service = UnityConfig.Container.Resolve<IMainService>();

        private readonly IReportService serviceR = UnityConfig.Container.Resolve<IReportService>();

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
                string path = "C:\\Users\\Шонова\\Desktop\\PatientAllTreatment.pdf";
                serviceR.SavePatientAllTreatments(new ReportBindingModel
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