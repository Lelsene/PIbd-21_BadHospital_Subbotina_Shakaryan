using System.Runtime.Serialization;

namespace HospitalModel
{
    [DataContract]
    public class TreatmentPrescription
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int TreatmentId { get; set; }

        [DataMember]
        public int PrescriptionId { get; set; }

        [DataMember]
        public string PrescriptionTitle { get; set; }

        [DataMember]
        public int Count { get; set; }

        public virtual Treatment Treatment { get; set; }

        public virtual Prescription Prescription { get; set; }
    }
}
