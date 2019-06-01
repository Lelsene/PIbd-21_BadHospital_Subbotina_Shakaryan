using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.ViewModels;
using System.Collections.Generic;

namespace HospitalServiceDAL.Interfaces
{
    public interface IReportService
    {
        List<PatientTreatmentsViewModel> GetPatientTreatments(ReportBindingModel model);

        List<RequestLoadViewModel> GetRequestLoad(ReportBindingModel model);

        void SaveRequestLoad(ReportBindingModel model);

        void SavePatientTreatments(ReportBindingModel model);
    }
}
