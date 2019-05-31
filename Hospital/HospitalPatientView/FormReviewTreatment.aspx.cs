using HospitalImplementations.Implementations;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using Unity;

namespace HospitalPatientView
{
    public partial class FormReviewTreatment : System.Web.UI.Page
    {
        private readonly IMainService service = UnityConfig.Container.Resolve<MainServiceDB>();

        private List<TreatmentPrescriptionViewModel> TreatmentPrescriptions;

        private int id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Int32.TryParse((string)Session["id"], out id))
            {
                try
                {
                    TreatmentViewModel view = service.GetTreatment(id);
                    if (view != null)
                    {
                        if (!Page.IsPostBack)
                        {
                            textBoxName.Text = view.Title;
                            textBoxPrice.Text = view.TotalCost.ToString();
                        }
                        this.TreatmentPrescriptions = view.TreatmentPrescriptions;
                        LoadData();
                    }

                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        private void LoadData()
        {
            try
            {
                if (TreatmentPrescriptions != null)
                {
                    dataGridView.DataBind();
                    dataGridView.DataSource = TreatmentPrescriptions;
                    dataGridView.DataBind();
                    dataGridView.ShowHeaderWhenEmpty = true;
                    dataGridView.SelectedRowStyle.BackColor = Color.Silver;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormMain.aspx");
        }
    }
}