using System.Collections.Generic;

namespace HospitalServiceDAL.ViewModels
{
    public class TreatmentViewModel
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public string Title { get; set; }

        public int TotalCost { get; set; }

        public List<TreatmentPrescriptionViewModel> TreatmentPrescriptions { get; set; }
    }
}
