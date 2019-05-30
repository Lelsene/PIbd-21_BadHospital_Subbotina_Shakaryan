using System.ComponentModel.DataAnnotations;


namespace HospitalModel
{
    public class Treatment
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int TotalCost { get; set; }

        public bool isReserved { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
