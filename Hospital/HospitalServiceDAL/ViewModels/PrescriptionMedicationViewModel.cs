namespace HospitalServiceDAL.ViewModels
{
    public class PrescriptionMedicationViewModel
    {
        public int Id { get; set; }

        public int PrescriptionId { get; set; }

        public int MedicationId { get; set; }

        public string MedicationName { get; set; }

        public int CountMedications { get; set; }
    }
}
