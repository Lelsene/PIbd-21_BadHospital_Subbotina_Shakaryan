using System.Collections.Generic;

namespace HospitalServiceDAL.ViewModels
{
    public class PatientTreatmentsViewModel
    {
        public string FIO { get; set; }

        public string Date { get; set; }

        public string prescriptionListStr { get; set; }

        public string medicationListStr { get; set; }

        public bool isReserved { get; set; }

        public List<PrescriptionViewModel> prescriptionList { get; set; }

        public List<MedicationViewModel> medicationList { get; set; }
    }
}
