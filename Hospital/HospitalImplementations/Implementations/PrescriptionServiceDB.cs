using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalModel;
using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;

namespace HospitalImplementations.Implementations
{
   public  class PrescriptionServiceDB : IPrescriptionService
    {
        private HospitalDBContext context;

        public PrescriptionServiceDB(HospitalDBContext context)
        {
            this.context = context;
        }
        public PrescriptionServiceDB()
        {
            this.context = new HospitalDBContext();
        }
        public List<PrescriptionViewModel> GetList()
        {
            List<PrescriptionViewModel> result = context.Prescriptions.Select(rec => new PrescriptionViewModel
            {
                Id = rec.Id,
                Title = rec.Title,
                Price = rec.Price,
                TreatmentPrescriptions = context.TreatmentPrescriptions
                .Where(recPC => recPC.PrescriptionId == rec.Id)
                .Select(recPC => new TreatmentPrescriptionViewModel
                {
                    Id = recPC.Id,
                    PrescriptionId = recPC.PrescriptionId,
                    TreatmentId = recPC.TreatmentId,
                    Count = recPC.Count
                }).ToList(),

                PrescriptionMedications = context.PrescriptionMedications
                .Where(recPC => recPC.PrescriptionId == rec.Id)
                .Select(recPC => new PrescriptionMedicationViewModel
                {
                    Id = recPC.Id,
                    PrescriptionId = recPC.PrescriptionId,
                    MedicationId = recPC.MedicationId
                }).ToList()
            }).ToList();
            return result;
        }

        public PrescriptionViewModel GetElement(int id)
        {
            Prescription element = context.Prescriptions.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new PrescriptionViewModel
                {
                    Id = element.Id,
                    Title = element.Title,
                    Price = element.Price,
                    TreatmentPrescriptions = context.TreatmentPrescriptions
                .Where(recPC => recPC.PrescriptionId == element.Id)
                .Select(recPC => new TreatmentPrescriptionViewModel
                {
                    Id = recPC.Id,
                    PrescriptionId = recPC.PrescriptionId,
                    TreatmentId = recPC.TreatmentId,
                    Count = recPC.Count
                }).ToList(),

                    PrescriptionMedications = context.PrescriptionMedications
                .Where(recPC => recPC.PrescriptionId == element.Id)
                .Select(recPC => new PrescriptionMedicationViewModel
                {
                    Id = recPC.Id,
                    PrescriptionId = recPC.PrescriptionId,
                    MedicationId = recPC.MedicationId
                }).ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(PrescriptionBindingModel model)
        {
            throw new NotImplementedException();
        }

        public void UpdElement(PrescriptionBindingModel model)
        {
            throw new NotImplementedException();
        }

        public void DelElement(int id)
        {
            throw new NotImplementedException();
        }
    }
}
