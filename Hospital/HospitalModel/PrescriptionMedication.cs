using System.ComponentModel.DataAnnotations;

namespace HospitalModel
{
    public class PrescriptionMedication
    {
        public int Id { get; set; }

        public int PrescriptionId { get; set; }

        public int MedicationId { get; set; }

        [Required]
        public int CountMedications { get; set; }

        public virtual Medication Medication { get; set; }

        public virtual Prescription Prescription { get; set; }
    }
}
