using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using HospitalImplementations.Implementations;
using Unity;

namespace HospitalClientView
{
    public partial class FormMainClient : System.Web.UI.Page
    {
        private readonly IMainService service = UnityConfig.Container.Resolve<MainServiceDB>();

        List<TreatmentViewModel> list;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                list = service.GetList();
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonPrescription_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormPrescriptions.aspx");
        }

        protected void ButtonReport_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonCreateTreatment_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormCreateTreatment.aspx");
        }

        protected void ButtonMakeReservation_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonUpd_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonUpdateTreatment_Click(object sender, EventArgs e)
        {

        }
    }
}