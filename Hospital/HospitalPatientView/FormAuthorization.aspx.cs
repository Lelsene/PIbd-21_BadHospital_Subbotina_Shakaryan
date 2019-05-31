using HospitalImplementations.Implementations;
using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.UI;
using Unity;

namespace HospitalPatientView
{
    public partial class AuthorizationForm : System.Web.UI.Page
    {
        private readonly IPatientService service = UnityConfig.Container.Resolve<PatientServiceDB>();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RegistrationButton_Click(object sender, EventArgs e)
        {
            try
            {
                String fio = textBoxFIO.Text;
                String email = textBoxEmail.Text;
                String password = textBoxPassword.Text;

                if (!string.IsNullOrEmpty(fio) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    service.AddElement(new PatientBindingModel
                    {
                        FIO = fio,
                        Email = email,
                        Password = password
                    });
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Регистрация прошла успешно');</script>");

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните все поля');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void SignInButton_Click(object sender, EventArgs e)
        {
            String email = textBoxEmail.Text;
            String password = textBoxPassword.Text;

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                List<PatientViewModel> patients = service.GetList();
                foreach (PatientViewModel patient in patients)
                {
                    if (patient.Email.Equals(email) && patient.Password.Equals(password))
                    {
                        Session["PatientId"] = patient.Id.ToString();
                        Response.Redirect("FormMain.aspx");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Нет такого пользователя');</script>");
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните все поля');</script>");
            }
        }
    }
}