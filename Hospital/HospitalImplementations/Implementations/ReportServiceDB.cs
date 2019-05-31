using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HospitalImplementations.Implementations
{
    public class ReportServiceDB : IReportService
    {
        private HospitalDBContext context;

        public ReportServiceDB(HospitalDBContext context)
        {
            this.context = context;
        }

        public ReportServiceDB()
        {
            this.context = new HospitalDBContext();
        }
    }
}
