using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalModel
{
    public class Treatment
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int PatientId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int TotalCost { get; set; }

        public bool isReserved { get; set; }

        public virtual Patient Patient { get; set; }

        [ForeignKey("TreatmentId")]
        public virtual List<TreatmentPrescription> TreatmentPrescriptions { get; set; }
    }
}
