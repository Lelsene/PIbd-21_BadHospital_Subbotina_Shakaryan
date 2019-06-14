using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HospitalModel
{
    [DataContract]
    public class PrescriptionMedication
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int PrescriptionId { get; set; }

        [DataMember]
        public int MedicationId { get; set; }

        [DataMember]
        public string MedicationName { get; set; }

        [DataMember]
        [Required]
        public int CountMedications { get; set; }

        public virtual Medication Medication { get; set; }

        public virtual Prescription Prescription { get; set; }
    }
}
