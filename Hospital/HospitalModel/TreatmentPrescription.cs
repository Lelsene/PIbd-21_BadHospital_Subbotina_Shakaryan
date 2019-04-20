
namespace HospitalModel
{
    public class TreatmentPrescription
    {
        public int Id { get; set; }

        public int TreatmentId { get; set; }

        public int PrescriptionId { get; set; }

        public virtual Treatment Treatment { get; set; }

        public virtual Prescription Prescription { get; set; }
    }
}
