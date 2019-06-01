using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.ViewModels;
using System.Collections.Generic;

namespace HospitalServiceDAL.Interfaces
{
    public interface IReportService
    {
        List<PatientTreatmentsViewModel> GetPatientsTreatments(ReportBindingModel model);

        List<PatientTreatmentsViewModel> GetPatientTreatments(ReportBindingModel model, int PatientId);

        List<RequestLoadViewModel> GetRequestLoad(ReportBindingModel model);

        void SaveRequestLoad(ReportBindingModel model);

        void SavePatientTreatments(ReportBindingModel model, int PatientId);

        void SavePatientsTreatments(ReportBindingModel model);

        void SavePatientAllTreatments(ReportBindingModel model, int PatientId);
    }
}
