using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;

namespace HospitalServiceDAL.Interfaces
{
    public interface IMainService
    {
        List<TreatmentViewModel> GetList();

        List<TreatmentViewModel> GetPatientList(int PatientId);

        TreatmentViewModel GetTreatment(int id);

        void CreateTreatment(TreatmentBindingModel model);

        void UpdTreatment(TreatmentBindingModel model);

        void DelTreatment(int id);

        DateTime TreatmentReservation(int id);

        void MedicationRefill(RequestMedicationBindingModel model);

        void SendEmail(string mailAddress, string subject, string text, string path);
    }
}
