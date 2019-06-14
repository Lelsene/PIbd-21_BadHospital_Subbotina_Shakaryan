using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace HospitalModel
{
    [DataContract]
    public class Prescription
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string Title { get; set; }

        [DataMember]
        [Required]
        public int Price { get; set; }

        [ForeignKey("PrescriptionId")]
        public virtual List<TreatmentPrescription> TreatmentPrescriptions { get; set; }

        [ForeignKey("PrescriptionId")]
        public virtual List<PrescriptionMedication> PrescriptionMedications { get; set; }
    }
}
