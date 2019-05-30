using System.Collections.Generic;

namespace HospitalServiceDAL.BindingModels
{
    public class TreatmentBindingModel
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public string Title { get; set; }

        public int TotalCost { get; set; }

        public bool isReserved { get; set; }

        public List<TreatmentPrescriptionBindingModel> TreatmentPrescriptions { get; set; }
    }
}
