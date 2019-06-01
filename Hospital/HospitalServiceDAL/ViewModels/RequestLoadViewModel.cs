using System;
using System.Collections.Generic;

namespace HospitalServiceDAL.ViewModels
{
    public class RequestLoadViewModel
    {
        public string RequestName { get; set; }

        public IEnumerable<Tuple<string, int>> Medications { get; set; }
    }
}
