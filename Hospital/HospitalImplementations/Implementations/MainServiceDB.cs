using HospitalModel;
using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalImplementations.Implementations
{
    public class MainServiceDB : IMainService
    {
        private HospitalDBContext context;

        public MainServiceDB(HospitalDBContext context)
        {
            this.context = context;
        }

        public MainServiceDB()
        {
            this.context = new HospitalDBContext();
        }

        public List<TreatmentViewModel> GetList()
        {
            List<TreatmentViewModel> result = context.Treatments.Select(rec =>
            new TreatmentViewModel
            {
                Id = rec.Id,
                PatientId = rec.PatientId,
                Title = rec.Title,
                TotalCost = rec.TotalCost,
                isReserved = rec.isReserved,
                TreatmentPrescriptions = context.TreatmentPrescriptions
                    .Where(recPC => recPC.TreatmentId == rec.Id)
                    .Select(recPC => new TreatmentPrescriptionViewModel
                    {
                        Id = recPC.Id,
                        TreatmentId = recPC.TreatmentId,
                        PrescriptionId = recPC.PrescriptionId,
                        PrescriptionTitle = recPC.Prescription.Title,
                        Count = recPC.Count,
                    }).ToList()
            }).ToList();
            return result;
        }

        public List<TreatmentViewModel> GetPatientList(int PatientId)
        {
            throw new NotImplementedException();
        }

        public TreatmentViewModel GetTreatment(int id)
        {
            Treatment element = context.Treatments.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new TreatmentViewModel
                {
                    Id = element.Id,
                    PatientId = element.PatientId,
                    Title = element.Title,
                    TotalCost = element.TotalCost,
                    isReserved = element.isReserved,
                    TreatmentPrescriptions = context.TreatmentPrescriptions
                    .Where(recPC => recPC.TreatmentId == element.Id)
                    .Select(recPC => new TreatmentPrescriptionViewModel
                    {
                        Id = recPC.Id,
                        TreatmentId = recPC.TreatmentId,
                        PrescriptionId = recPC.PrescriptionId,
                        PrescriptionTitle = recPC.Prescription.Title,
                        Count = recPC.Count,
                    }).ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void CreateTreatment(TreatmentBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Treatment element = context.Treatments.FirstOrDefault(rec =>
                     rec.Title == model.Title);
                    if (element != null)
                    {
                        throw new Exception("Уже есть лечение с таким названием");
                    }
                    element = new Treatment
                    {
                        PatientId = model.PatientId,
                        Title = model.Title,
                        TotalCost = model.TotalCost,
                    };
                    context.Treatments.Add(element);
                    context.SaveChanges();
                    //// убираем дубли по рецептам 
                    var groupPrescriptions = model.TreatmentPrescriptions
                        .GroupBy(rec => rec.PrescriptionId)
                        .Select(rec => new
                        {
                            PrescriptionId = rec.Key,
                            Count = rec.Sum(r => r.Count)
                        });
                    // запоминаем id и названия рецептов
                    var prescriptionTitle = model.TreatmentPrescriptions.Select(rec => new
                    {
                        PrescriptionId = rec.PrescriptionId,
                        PrescriptionTitle = rec.PrescriptionTitle
                    });
                    // добавляем рецепты  
                    foreach (var groupPrescription in groupPrescriptions)
                    {
                        string Title = null;
                        foreach (var prescription in prescriptionTitle)
                        {
                            if (groupPrescription.PrescriptionId == prescription.PrescriptionId)
                            {
                                Title = prescription.PrescriptionTitle;
                            }
                        }
                        context.TreatmentPrescriptions.Add(new TreatmentPrescription
                        {
                            TreatmentId = element.Id,
                            PrescriptionId = groupPrescription.PrescriptionId,
                            PrescriptionTitle = Title,
                            Count = groupPrescription.Count
                        });
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void TreatmentReservation(TreatmentPrescriptionBindingModel model)
        {
            TreatmentPrescription element = context.TreatmentPrescriptions.FirstOrDefault(rec =>
            rec.PrescriptionId == model.PrescriptionId && rec.TreatmentId == model.TreatmentId);
            if (element != null)
            {
                //element.isReserved = model.isReserved;
            }
            else
            {
                context.TreatmentPrescriptions.Add(new TreatmentPrescription
                {
                    PrescriptionId = model.PrescriptionId,
                    TreatmentId = model.TreatmentId,
                    Count = model.Count,
                    //isReserved = model.isReserved
                });
            }
            context.SaveChanges();
        }

        public void MedicationRefill(RequestMedicationBindingModel model)
        {
            RequestMedication element = context.RequestMedications.FirstOrDefault(rec => rec.RequestId == model.RequestId && rec.MedicationId == model.MedicationId);

            if (element != null)
            {
                element.CountMedications += model.CountMedications;
            }
            else
            {
                context.RequestMedications.Add(new RequestMedication
                {
                    MedicationId = model.MedicationId,
                    RequestId = model.RequestId,
                    CountMedications = model.CountMedications
                });
            }
            context.Medications.FirstOrDefault(res => res.Id == model.MedicationId).Count += model.CountMedications;
            context.SaveChanges();
        }
    }
}
