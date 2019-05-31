using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.ViewModels;
using System.Collections.Generic;

namespace HospitalServiceDAL.Interfaces
{
    public interface IMedicationService
    {
        List<MedicationViewModel> GetList();

        MedicationViewModel GetElement(int id);

        void AddElement(MedicationBindingModel model);

        void UpdElement(MedicationBindingModel model);

        void DelElement(int id);
    }
}
