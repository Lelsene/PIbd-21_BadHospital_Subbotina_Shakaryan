using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalModel
{
    public class Prescription
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Price { get; set; }

        [ForeignKey("PrescriptionId")]
        public virtual List<TreatmentPrescription> TreatmentPrescriptions { get; set; }
    }
}
