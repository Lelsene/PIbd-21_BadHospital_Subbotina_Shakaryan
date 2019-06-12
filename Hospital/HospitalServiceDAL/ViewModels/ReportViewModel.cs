using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalServiceDAL.ViewModels
{
    public class ReportViewModel
    {
        public string FIO { get; set; }

        public string Date { get; set; }

        public string Title { get; set; }

        public string MedicationName { get; set; }

        public int MedicationCount { get; set; }
    }
}
