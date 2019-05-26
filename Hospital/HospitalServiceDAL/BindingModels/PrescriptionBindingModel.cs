using System.Collections.Generic;

namespace HospitalServiceDAL.BindingModels
{
    public class PrescriptionBindingModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Price { get; set; }

        public List<PrescriptionMedicationBindingModel> PrescriptionMedications { get; set; }
    }
}
