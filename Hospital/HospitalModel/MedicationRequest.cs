using System.ComponentModel.DataAnnotations;

namespace HospitalModel
{
    public class MedicationRequest
    {
        public int Id { get; set; }

        public int MedicationId { get; set; }

        public int RequestId { get; set; }

        public string MedicationName { get; set; }

        [Required]
        public int CountMedications { get; set; }

        public virtual Medication Medication { get; set; }

        public virtual Request Request { get; set; }
    }
}
