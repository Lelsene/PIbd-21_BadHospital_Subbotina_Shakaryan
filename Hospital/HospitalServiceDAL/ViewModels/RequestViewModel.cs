using System;
using System.Collections.Generic;

namespace HospitalServiceDAL.ViewModels
{
    public class RequestViewModel
    {
        public int Id { get; set; }

        public string RequestName { get; set; }

        public DateTime Date { get; set; }

        public List<MedicationRequestViewModel> MedicationRequests { get; set; }
    }
}
