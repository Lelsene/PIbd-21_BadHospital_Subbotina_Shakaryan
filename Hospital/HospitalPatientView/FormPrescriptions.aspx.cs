using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;
using HospitalServiceDAL.Interfaces;
using HospitalImplementations.Implementations;
using HospitalServiceDAL.ViewModels;

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
                list = service.GetList();
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