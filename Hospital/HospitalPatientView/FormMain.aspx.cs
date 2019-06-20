using HospitalImplementations.Implementations;
using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using Unity;

namespace HospitalPatientView
{
    public partial class FormMain : System.Web.UI.Page
    {
        private readonly IMainService service = UnityConfig.Container.Resolve<MainServiceDB>();

        private readonly IReportService serviceR = UnityConfig.Container.Resolve<ReportServiceDB>();

        private readonly IBackUpService serviceB = UnityConfig.Container.Resolve<BackUpServiceDB>();

        List<TreatmentViewModel> list;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                list = service.GetPatientList(Convert.ToInt32(Session["PatientId"]));
                dataGridView.DataSource = list;
                dataGridView.DataBind();
                dataGridView.ShowHeaderWhenEmpty = true;
                dataGridView.SelectedRowStyle.BackColor = Color.Silver;
                dataGridView.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCreateTreatment_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormCreateTreatment.aspx");
        }

        protected void ButtonReviewTreatment_Click(object sender, EventArgs e)
        {
            try
            {
                string index = list[dataGridView.SelectedIndex].Id.ToString();
                Session["id"] = index;
                Response.Redirect("FormReviewTreatment.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonDeleteTreatment_Click(object sender, EventArgs e)
        {
            try
            {
                service.DelTreatment(list[dataGridView.SelectedIndex].Id);
                LoadData();
                Response.Redirect("FormMain.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonTreatmentReservation_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date = service.TreatmentReservation(list[dataGridView.SelectedIndex].Id);
                string path = "C:\\Users\\Шонова\\Desktop\\PatientTreatment.xls";
                serviceR.SaveLoad(new ReportBindingModel
                {
                    FileName = path,
                    DateFrom = date,
                    DateTo = date.AddMilliseconds(100)
                }, Convert.ToInt32(Session["PatientId"]));
                service.SendEmail(Session["PatientEmail"].ToString(), "Оповещение по резервированию", "Резервирование выполнено", path);
                LoadData();
                Response.Redirect("FormMain.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonRef_Click(object sender, EventArgs e)
        {
            LoadData();
            Response.Redirect("FormMain.aspx");
        }

        protected void ButtonBackUpXML_Click(object sender, EventArgs e)
        {
            serviceB.PatientBackUpXML(Convert.ToInt32(Session["PatientId"]));
        }

        protected void ButtonBackUpJSON_Click(object sender, EventArgs e)
        {
            serviceB.PatientBackUpJSON(Convert.ToInt32(Session["PatientId"]));
        }
    }
}