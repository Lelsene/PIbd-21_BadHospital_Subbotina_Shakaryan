using System.Collections.Generic;

namespace HospitalServiceDAL.ViewModels
{
    public class PrescriptionViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Price { get; set; }

        public List<PrescriptionMedicationViewModel> PrescriptionMedications { get; set; }
    }
}
