using HospitalImplementations.Implementations;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.UI;
using Unity;

namespace HospitalPatientView
{
    public partial class FormTreatmentPrescription : System.Web.UI.Page
    {
        private readonly IPrescriptionService serviceS = UnityConfig.Container.Resolve<PrescriptionServiceDB>();

        private readonly IMainService serviceM = UnityConfig.Container.Resolve<MainServiceDB>();

        private TreatmentPrescriptionViewModel model;

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
            if (string.IsNullOrEmpty(TextBoxCount.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните поле Количество');</script>");
                return;
            }
            if (DropDownListPrescription.SelectedValue == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Выберите ингредиент');</script>");
                return;
            }
            try
            {
                if (Session["SEId"] == null)
                {
                    model = new TreatmentPrescriptionViewModel
                    {
                        PrescriptionId = Convert.ToInt32(DropDownListPrescription.SelectedValue),
                        PrescriptionTitle = DropDownListPrescription.SelectedItem.Text,
                        Count = Convert.ToInt32(TextBoxCount.Text)
                    };
                    Session["SEId"] = model.Id;
                    Session["SETreatmentId"] = model.TreatmentId;
                    Session["SEPrescriptionId"] = model.PrescriptionId;
                    Session["SEPrescriptionTitle"] = model.PrescriptionTitle;
                    Session["SECount"] = model.Count;
                }
                else
                {
                    model.Count = Convert.ToInt32(TextBoxCount.Text);
                    Session["SEId"] = model.Id;
                    Session["SETreatmentId"] = model.TreatmentId;
                    Session["SEPrescriptionId"] = model.PrescriptionId;
                    Session["SEPrescriptionTitle"] = model.PrescriptionTitle;
                    Session["SECount"] = model.Count;
                    Session["Change"] = "1";
                }
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                Server.Transfer("FormCreateTreatment.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
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