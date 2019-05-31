using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalModel
{
    public class Request
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [ForeignKey("RequestId")]
        public virtual List<MedicationRequest> MedicationRequests { get; set; }
    }
}
