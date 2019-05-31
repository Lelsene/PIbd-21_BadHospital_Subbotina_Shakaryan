namespace HospitalServiceDAL.BindingModels
{
    public class PrescriptionMedicationBindingModel
    {
        public int Id { get; set; }

        public int PrescriptionId { get; set; }

        public int MedicationId { get; set; }

        public int CountMedications { get; set; }
    }
}
