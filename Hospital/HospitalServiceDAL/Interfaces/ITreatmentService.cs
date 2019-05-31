using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.ViewModels;
using System.Collections.Generic;

namespace HospitalServiceDAL.Interfaces
{
    public interface ITreatmentService
    {
        List<TreatmentViewModel> GetList();

        TreatmentViewModel GetElement(int id);

        void AddElement(TreatmentBindingModel model);

        void UpdElement(TreatmentBindingModel model);

        void DelElement(int id);
    }
}
