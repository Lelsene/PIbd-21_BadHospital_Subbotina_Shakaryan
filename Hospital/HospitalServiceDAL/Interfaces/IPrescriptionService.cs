using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.ViewModels;
using System.Collections.Generic;

namespace HospitalServiceDAL.Interfaces
{
    public interface IPrescriptionService
    {
        List<PrescriptionViewModel> GetList();

        List<PrescriptionViewModel> GetAvailableList();

        PrescriptionViewModel GetElement(int id);

        void AddElement(PrescriptionBindingModel model);

        void UpdElement(PrescriptionBindingModel model);

        void DelElement(int id);
    }
}
