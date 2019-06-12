using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.ViewModels;
using System.Collections.Generic;

namespace HospitalServiceDAL.Interfaces
{
    public interface IReportService
    {
        List<ReportViewModel> GetReport(ReportBindingModel model);

        List<ReportViewModel> GetRequests(ReportBindingModel model);

        List<ReportViewModel> GetTreatments(ReportBindingModel model, int PatientId);

        void SaveReport(ReportBindingModel model);        

        void SavePatientTreatments(ReportBindingModel model, int PatientId);       

        void SaveRequestLoad(ReportBindingModel model);
    }
}
