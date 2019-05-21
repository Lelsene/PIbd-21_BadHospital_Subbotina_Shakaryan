using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.ViewModels;
using System.Collections.Generic;

namespace HospitalServiceDAL.Interfaces
{
    public interface IRequestService
    {
        List<RequestViewModel> GetList();

        RequestViewModel GetElement(int id);

        void AddElement(RequestBindingModel model);

        void UpdElement(RequestBindingModel model);

        void DelElement(int id);
    }
}
