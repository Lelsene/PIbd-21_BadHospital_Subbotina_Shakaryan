using HospitalImplementations.Implementations;
using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using Unity;

namespace HospitalClientView
{
    public partial class FormCreateTreatment : System.Web.UI.Page
    {
        private readonly IMainService service = UnityConfig.Container.Resolve<MainClientServiceDB>();

        private int id;

        private List<TreatmentPrescriptionViewModel> TreatmentPrescriptions;

        private TreatmentPrescriptionViewModel model;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Int32.TryParse((string)Session["id"], out id))
            {
                try
                {
                    TreatmentViewModel view = service.GetElement(id);
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
            else
            {
                this.TreatmentPrescriptions = new List<TreatmentPrescriptionViewModel>();

            }
            if (Session["SEId"] != null)
            {
                if ((Session["SEIs"] != null) && (Session["Change"].ToString() != "0"))
                {
                    model = new TreatmentPrescriptionViewModel
                    {
                        Id = (int)Session["SEId"],
                        TreatmentId = (int)Session["SETreatmentId"],
                        PrescriptionId = (int)Session["SEPrescriptionId"],
                        PrescriptionTitle = (string)Session["SEPrescriptionTitle"],
                        isReserved = (bool)Session["SEIsReserved"],
                        Count = (int)Session["SECount"]
                    };
                    this.TreatmentPrescriptions[(int)Session["SEIs"]] = model;
                    Session["Change"] = "0";
                }
                else
                {
                    model = new TreatmentPrescriptionViewModel
                    {
                        TreatmentId = (int)Session["SETreatmentId"],
                        PrescriptionId = (int)Session["SEPrescriptionId"],
                        PrescriptionTitle = (string)Session["SEPrescriptionTitle"],
                        isReserved = (bool)Session["SEIsReserved"],
                        Count = (int)Session["SECount"]
                    };
                    this.TreatmentPrescriptions.Add(model);
                }
                Session["SEId"] = null;
                Session["SETreatmentId"] = null;
                Session["SEPrescriptionId"] = null;
                Session["SEPrescriptionTitle"] = null;
                Session["SEIsReserved"] = null;
                Session["SECount"] = null;
                Session["SEIs"] = null;
            }
            List<TreatmentPrescriptionBindingModel> TreatmentPrescriptionBM = new List<TreatmentPrescriptionBindingModel>();
            for (int i = 0; i < this.TreatmentPrescriptions.Count; ++i)
            {
                TreatmentPrescriptionBM.Add(new TreatmentPrescriptionBindingModel
                {
                    Id = this.TreatmentPrescriptions[i].Id,
                    TreatmentId = this.TreatmentPrescriptions[i].TreatmentId,
                    PrescriptionId = this.TreatmentPrescriptions[i].PrescriptionId,
                    PrescriptionTitle = this.TreatmentPrescriptions[i].PrescriptionTitle,
                    isReserved = this.TreatmentPrescriptions[i].isReserved,
                    Count = this.TreatmentPrescriptions[i].Count
                });
            }
            if (TreatmentPrescriptionBM.Count != 0)
            {
                if (Int32.TryParse((string)Session["id"], out id))
                {
                    service.UpdElement(new TreatmentBindingModel
                    {
                        Id = id,
                        PatientId = Int32.Parse(Session["PatientId"].ToString()),
                        Title = "Введите название",
                        TotalCost = 0,
                        TreatmentPrescriptions = TreatmentPrescriptionBM
                    });
                }
                else
                {
                    service.AddElement(new TreatmentBindingModel
                    {
                        PatientId = Int32.Parse(Session["PatientId"].ToString()),
                        Title = "Введите название",
                        TotalCost = 0,
                        TreatmentPrescriptions = TreatmentPrescriptionBM
                    });
                    Session["id"] = service.GetList().Last().Id.ToString();
                    Session["Change"] = "0";
                }
            }
            LoadData();
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
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormPrescription.aspx");
        }

        protected void ButtonChange_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedIndex >= 0)
            {
                model = service.GetElement(id).TreatmentPrescriptions[dataGridView.SelectedIndex];
                Session["SEId"] = model.Id;
                Session["SETreatmentId"] = model.TreatmentId;
                Session["SEPrescriptionId"] = model.PrescriptionId;
                Session["SEPrescriptionTitle"] = model.PrescriptionTitle;
                Session["SEIsReserved"] = model.isReserved;
                Session["SECount"] = model.Count;
                Session["SEIs"] = dataGridView.SelectedIndex;
                Session["Change"] = "0";
                Server.Transfer("FormTreatmentPrescription.aspx");
            }
        }

        protected void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedIndex >= 0)
            {
                try
                {
                    TreatmentPrescriptions.RemoveAt(dataGridView.SelectedIndex);
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
                LoadData();
            }
        }

        protected void ButtonUpd_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните название');</script>");
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните цену');</script>");
                return;
            }
            if (TreatmentPrescriptions == null || TreatmentPrescriptions.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните ингредиенты');</script>");
                return;
            }
            try
            {
                List<TreatmentPrescriptionBindingModel> TreatmentPrescriptionBM = new List<TreatmentPrescriptionBindingModel>();
                for (int i = 0; i < TreatmentPrescriptions.Count; ++i)
                {
                    TreatmentPrescriptionBM.Add(new TreatmentPrescriptionBindingModel
                    {
                        Id = TreatmentPrescriptions[i].Id,
                        TreatmentId = TreatmentPrescriptions[i].TreatmentId,
                        PrescriptionId = TreatmentPrescriptions[i].PrescriptionId,
                        PrescriptionTitle = TreatmentPrescriptions[i].PrescriptionTitle,
                        isReserved = TreatmentPrescriptions[i].isReserved,
                        Count = TreatmentPrescriptions[i].Count
                    });
                }
                if (Int32.TryParse((string)Session["id"], out id))
                {
                    service.UpdElement(new TreatmentBindingModel
                    {
                        Id = id,
                        PatientId = Int32.Parse(Session["PatientId"].ToString()),
                        Title = textBoxName.Text,
                        TotalCost = Convert.ToInt32(textBoxPrice.Text),
                        TreatmentPrescriptions = TreatmentPrescriptionBM
                    });
                }
                else
                {
                    service.AddElement(new TreatmentBindingModel
                    {
                        PatientId = Int32.Parse(Session["PatientId"].ToString()),
                        Title = textBoxName.Text,
                        TotalCost = Convert.ToInt32(textBoxPrice.Text),
                        TreatmentPrescriptions = TreatmentPrescriptionBM
                    });
                }
                Session["id"] = null;
                Session["Change"] = null;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                Server.Transfer("FormCocktails.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {

            //if (service.GetList().Count != 0 && service.GetList().Last().CocktailName == null)
            //{
            //    service.DelElement(service.GetList().Last().Id);
            //}
            //if (!String.Equals(Session["Change"], null))
            //{
            //    service.DelElement(id);
            //}
            Session["id"] = null;
            Session["Change"] = null;
            Server.Transfer("FormMainClient.aspx");
        }
        protected void dataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //e.Row.Cells[1].Visible = false;
            //e.Row.Cells[2].Visible = false;
            //e.Row.Cells[3].Visible = false;
        }
    }
}