namespace HospitalServiceDAL.ViewModels
{
   public class TreatmentPrescriptionViewModel
    {
        public int Id { get; set; }

        public int TreatmentId { get; set; }

        public int PrescriptionId { get; set; }

        public string PrescriptionTitle { get; set; }

        public bool isReserved { get; set; }

        public int Count { get; set; }
    }
}
