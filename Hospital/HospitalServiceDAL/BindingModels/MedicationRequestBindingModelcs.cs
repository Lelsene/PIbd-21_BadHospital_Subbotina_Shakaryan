namespace HospitalServiceDAL.BindingModels
{
    public class MedicationRequestBindingModelcs
    {
        public int Id { get; set; }

        public int MedicationId { get; set; }

        public int RequestId { get; set; }

        public int CountMedications { get; set; }
    }
}
