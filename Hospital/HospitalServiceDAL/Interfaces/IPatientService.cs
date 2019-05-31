using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.ViewModels;
using System.Collections.Generic;

namespace HospitalServiceDAL.Interfaces
{
   public interface IPatientService
    {
        List<PatientViewModel> GetList();

        PatientViewModel GetElement(string Email, string Password);

        void AddElement(PatientBindingModel model);
    }
}
