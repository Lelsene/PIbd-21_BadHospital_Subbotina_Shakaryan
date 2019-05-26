﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;
using HospitalServiceDAL.Interfaces;
using HospitalImplementations.Implementations;
using HospitalServiceDAL.ViewModels;

namespace HospitalClientView
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
                dataGridView.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonUpd_Click(object sender, EventArgs e)
        {
            LoadData();
            Server.Transfer("FormPrescriptions.aspx");
        }

        protected void ButtonBack_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormMainClient.aspx");
        }
    }
}