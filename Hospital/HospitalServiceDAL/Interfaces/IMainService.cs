﻿using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.ViewModels;
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

        void TreatmentReservation(TreatmentPrescriptionBindingModel model);

        void MedicationRefill(RequestMedicationBindingModel model);
    }
}
