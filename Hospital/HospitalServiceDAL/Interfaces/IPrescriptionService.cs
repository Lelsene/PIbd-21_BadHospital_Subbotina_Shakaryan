using HospitalServiceDAL.BindingModels;
using System.Collections.Generic;
using HospitalServiceDAL.ViewModels;

namespace HospitalServiceDAL.Interfaces
{
    public interface IPrescriptionService
    {
        List<PrescriptionViewModel> GetList();

        PrescriptionViewModel GetElement(int id);

        void AddElement(PrescriptionBindingModel model);

        void UpdElement(PrescriptionBindingModel model);

        void DelElement(int id);
    }
}
