using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalModel
{
    public class Medication
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int Count { get; set; }

        [ForeignKey("MedicationId")]
        public virtual List<PrescriptionMedication> PrescriptionMedications { get; set; }

        [ForeignKey("MedicationId")]
        public virtual List<RequestMedication> RequestMedications { get; set; }
    }
}
