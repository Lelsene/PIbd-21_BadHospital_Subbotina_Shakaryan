using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.ViewModels;
using System.Collections.Generic;

namespace HospitalServiceDAL.Interfaces
{
    public interface IMainService
    {
        List<TreatmentViewModel> GetList();

        TreatmentViewModel GetElement(int id);

        void AddElement(TreatmentBindingModel model);

        void UpdElement(TreatmentBindingModel model);

       // void CreateRequest(RequestBindingModel model);
    }
}
