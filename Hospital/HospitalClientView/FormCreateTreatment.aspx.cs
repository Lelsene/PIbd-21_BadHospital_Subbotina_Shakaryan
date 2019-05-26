using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HospitalClientView
{
    public partial class FormCreateTreatment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormPrescription.aspx");
        }

        protected void ButtonChange_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonDelete_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonUpd_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
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