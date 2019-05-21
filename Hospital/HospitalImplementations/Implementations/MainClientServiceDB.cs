using HospitalServiceDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.ViewModels;
using HospitalModel;

namespace HospitalImplementations.Implementations
{
    public class MainClientServiceDB : IMainService
    {
        private HospitalDBContext context;

        public MainClientServiceDB(HospitalDBContext context)
        {
            this.context = context;
        }

        public MainClientServiceDB()
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
                TreatmentPrescriptions = context.TreatmentPrescriptions
                    .Where(recPC => recPC.TreatmentId == rec.Id)
                    .Select(recPC => new TreatmentPrescriptionViewModel
                    {
                        Id = recPC.Id,
                        TreatmentId = recPC.TreatmentId,
                        PrescriptionId = recPC.PrescriptionId,
                        PrescriptionTitle = recPC.Prescription.Title,
                        Count = recPC.Count,
                        isReserved = recPC.isReserved
                    }).ToList()
            }).ToList();
            return result;
        }

        public TreatmentViewModel GetElement(int id)
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
                    TreatmentPrescriptions = context.TreatmentPrescriptions
                    .Where(recPC => recPC.TreatmentId == element.Id)
                    .Select(recPC => new TreatmentPrescriptionViewModel
                    {
                        Id = recPC.Id,
                        TreatmentId = recPC.TreatmentId,
                        PrescriptionId = recPC.PrescriptionId,
                        PrescriptionTitle = recPC.Prescription.Title,
                        Count = recPC.Count,
                        isReserved = recPC.isReserved
                    }).ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(TreatmentBindingModel model)
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
                    //// убираем дубли по компонентам 
                    var groupPrescriptions = model.TreatmentPrescriptions
                        .GroupBy(rec => rec.PrescriptionId)
                        .Select(rec => new
                        {
                            PrescriptionId = rec.Key,
                            Count = rec.Sum(r => r.Count)
                        });
                    // добавляем компоненты  
                    foreach (var groupPrescription in groupPrescriptions)
                    {
                        context.TreatmentPrescriptions.Add(new TreatmentPrescription
                        {
                            Id = element.Id,
                            PrescriptionId = groupPrescription.PrescriptionId,
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

        public void UpdElement(TreatmentBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Treatment element = context.Treatments.FirstOrDefault(rec =>
                    rec.Title == model.Title && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть лечение с таким названием");
                    }
                    element = context.Treatments.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.Title = model.Title;
                    element.TotalCost = model.TotalCost;
                    context.SaveChanges();

                    // обновляем существуюущие компоненты 
                    var compIds = model.TreatmentPrescriptions.Select(rec => rec.PrescriptionId).Distinct();
                    var updatePrescriptions = context.TreatmentPrescriptions.Where(rec =>
                    rec.TreatmentId == model.Id && compIds.Contains(rec.PrescriptionId));
                    foreach (var updatePrescription in updatePrescriptions)
                    {
                        updatePrescription.Count = model.TreatmentPrescriptions.FirstOrDefault(rec =>
                        rec.Id == updatePrescription.Id).Count;
                    }
                    context.SaveChanges();
                    context.TreatmentPrescriptions.RemoveRange(context.TreatmentPrescriptions.Where(rec =>
                    rec.TreatmentId == model.Id && !compIds.Contains(rec.PrescriptionId)));
                    context.SaveChanges();
                    // новые записи  
                    var groupPrescriptions = model.TreatmentPrescriptions.Where(rec =>
                    rec.Id == 0).GroupBy(rec => rec.PrescriptionId).Select(rec => new
                    {
                        PrescriptionId = rec.Key,
                        Count = rec.Sum(r => r.Count)
                    });
                    foreach (var groupPrescription in groupPrescriptions)

                    {
                        TreatmentPrescription elementPC = context.TreatmentPrescriptions.FirstOrDefault(rec =>
                        rec.TreatmentId == model.Id && rec.PrescriptionId == groupPrescription.PrescriptionId);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupPrescription.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.TreatmentPrescriptions.Add(new TreatmentPrescription
                            {
                                TreatmentId = model.Id,
                                PrescriptionId = groupPrescription.PrescriptionId,
                                Count = groupPrescription.Count
                            });
                            context.SaveChanges();
                        }
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
        public void MakeReservation(TreatmentPrescriptionBindingModel model)
        {
            TreatmentPrescription element = context.TreatmentPrescriptions.FirstOrDefault(rec =>
            rec.PrescriptionId == model.PrescriptionId && rec.TreatmentId == model.TreatmentId);
            if (element != null)
            {
                element.isReserved = model.isReserved;
            }
            else
            {
                context.TreatmentPrescriptions.Add(new TreatmentPrescription
                {
                    PrescriptionId = model.PrescriptionId,
                    TreatmentId = model.TreatmentId,
                    Count = model.Count,
                    isReserved = model.isReserved
                });
            }
            context.SaveChanges();
        }
    }
}
