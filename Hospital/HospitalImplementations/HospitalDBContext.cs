using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using HospitalModel;

namespace HospitalImplementations
{
    public class HospitalDBContext : DbContext
    {
        public HospitalDBContext() : base("HospitalDatabase")
        {
            //настройки конфигурации для entity            
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
        public virtual DbSet<Patient> Patients { get; set; }

        public virtual DbSet<Request> Requests { get; set; }

        public virtual DbSet<Treatment> Treatments { get; set; }

        public virtual DbSet<Medication> Medications { get; set; }

        public virtual DbSet<Prescription> Prescriptions { get; set; }

        public virtual DbSet<PrescriptionMedication> PrescriptionMedications { get; set; }

        public virtual DbSet<TreatmentPrescription> TreatmentPrescriptions { get; set; }

        public virtual DbSet<MedicationRequest> MedicationsRequest { get; set; }
    }
}
