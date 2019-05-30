namespace HospitalServiceDAL.BindingModels
{
    public class TreatmentPrescriptionBindingModel
    {
        public int Id { get; set; }

        public int TreatmentId { get; set; }

        public int PrescriptionId { get; set; }

        public string PrescriptionTitle { get; set; }

        public int Count { get; set; }
    }
}
