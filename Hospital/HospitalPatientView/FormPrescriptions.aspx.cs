using HospitalImplementations.Implementations;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.UI;
using Unity;

namespace HospitalPatientView
{
    public partial class FormPrescriptions : System.Web.UI.Page
    {
        private readonly IPrescriptionService service = UnityConfig.Container.Resolve<PrescriptionServiceDB>();

        List<PrescriptionViewModel> list;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                list = service.GetAvailableList();
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonRef_Click(object sender, EventArgs e)
        {
            LoadData();
            Response.Redirect("FormPrescriptions.aspx");
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormMain.aspx");
        }
    }
}