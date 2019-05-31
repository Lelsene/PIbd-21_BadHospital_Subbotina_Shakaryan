﻿using HospitalImplementations.Implementations;
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
                service.TreatmentReservation(list[dataGridView.SelectedIndex].Id);
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
    }
}