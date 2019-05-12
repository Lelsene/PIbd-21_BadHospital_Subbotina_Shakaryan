using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.ViewModels;
using System.Collections.Generic;

namespace HospitalServiceDAL.Interfaces
{
    public interface IMainService
    {
        List<RequestViewModel> GetList();

        void CreateRequest(RequestBindingModel model);
    }
}
