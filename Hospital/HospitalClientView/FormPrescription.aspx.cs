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
    public partial class FormPrescription : System.Web.UI.Page
    {
        private readonly IPrescriptionService serviceS = UnityConfig.Container.Resolve<PrescriptionServiceDB>();

        private readonly IMainService serviceM = UnityConfig.Container.Resolve<MainClientServiceDB>();

        private List<TreatmentPrescriptionViewModel> TreatmentPrescriptions;

        private int id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    List<PrescriptionViewModel> listP = serviceS.GetList();
                    if (listP != null)
                    {
                        DropDownListPrescription.DataSource = listP;
                        DropDownListPrescription.DataBind();
                        DropDownListPrescription.DataTextField = "Title";
                        DropDownListPrescription.DataValueField = "Id";
                    }
                    Page.DataBind();

                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormCreateTreatment.aspx");
        }
        protected void TextBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }
        private void CalcSum()
        {
            if (DropDownListPrescription.SelectedValue != null && !string.IsNullOrEmpty(TextBoxCount.Text))
            {
                try
                {
                    int id = Convert.ToInt32(DropDownListPrescription.SelectedValue);
                    PrescriptionViewModel prescription = serviceS.GetElement(id);
                    int count = Convert.ToInt32(TextBoxCount.Text);
                    TextBoxSum.Text = (count * prescription.Price).ToString();
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
        }
    }
}