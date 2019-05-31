using System;
using System.Collections.Generic;

namespace HospitalServiceDAL.BindingModels
{
    public class RequestBindingModel
    {
        public int Id { get; set; }

        public string RequestName { get; set; }

        public DateTime Date { get; set; }

        public List<RequestMedicationBindingModel> RequestMedications { get; set; }
    }
}
