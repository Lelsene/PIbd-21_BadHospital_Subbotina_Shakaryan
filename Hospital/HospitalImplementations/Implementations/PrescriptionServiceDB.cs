using HospitalModel;
using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HospitalImplementations.Implementations
{
    public class PrescriptionServiceDB : IPrescriptionService
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
                PrescriptionMedications = context.PrescriptionMedications
                    .Where(recPM => recPM.PrescriptionId == rec.Id)
                    .Select(recPM => new PrescriptionMedicationViewModel
                    {
                        Id = recPM.Id,
                        PrescriptionId = recPM.PrescriptionId,
                        MedicationId = recPM.MedicationId,
                        MedicationName = recPM.MedicationName,
                        CountMedications = recPM.CountMedications
                    }).ToList()
            }).ToList();
            return result;
        }

        public List<PrescriptionViewModel> GetClientList(int PatientId)
        {
            var groupPrescriptons = context.TreatmentPrescriptions
                                    .Include(rec => rec.Prescription)
                                    .Include(rec => rec.Treatment)
                                    .Where(rec => rec.Treatment.PatientId == PatientId)
                                    .Select(rec => new PrescriptionViewModel
                                    {
                                        Id = rec.PrescriptionId,
                                        Title = rec.PrescriptionTitle,
                                        Price = rec.Count
                                    })
                                    .GroupBy(rec => rec.Id)
                                    .Select(rec => new
                                    {
                                        PrescriptionId = rec.Key,
                                        Count = rec.Sum(r => r.Price)
                                    })
                                    .OrderByDescending(rec => rec.Count)
                                    .ToList();

            List<PrescriptionViewModel> result = new List<PrescriptionViewModel>();
            foreach (var pre in groupPrescriptons)
            {
                var pres = context.Prescriptions.FirstOrDefault(rec => rec.Id == pre.PrescriptionId);
                result.Add(new PrescriptionViewModel
                {
                    Id = pres.Id,
                    Title = pres.Title,
                    Price = pres.Price,
                    PrescriptionMedications = context.PrescriptionMedications
                                              .Where(recPM => recPM.PrescriptionId == pres.Id)
                                              .Select(recPM => new PrescriptionMedicationViewModel
                                              {
                                                  Id = recPM.Id,
                                                  PrescriptionId = recPM.PrescriptionId,
                                                  MedicationId = recPM.MedicationId,
                                                  MedicationName = recPM.MedicationName,
                                                  CountMedications = recPM.CountMedications
                                              }).ToList()
                });
            }

            foreach (var el in context.Prescriptions)
            {
                bool flag = false;
                foreach (var pre in result)
                {
                    if (el.Id == pre.Id)
                    {
                        flag = true;
                    }
                }
                if (!flag)
                {
                    result.Add(new PrescriptionViewModel
                    {
                        Id = el.Id,
                        Title = el.Title,
                        Price = el.Price,
                        PrescriptionMedications = context.PrescriptionMedications
                                              .Where(recPM => recPM.PrescriptionId == el.Id)
                                              .Select(recPM => new PrescriptionMedicationViewModel
                                              {
                                                  Id = recPM.Id,
                                                  PrescriptionId = recPM.PrescriptionId,
                                                  MedicationId = recPM.MedicationId,
                                                  MedicationName = recPM.MedicationName,
                                                  CountMedications = recPM.CountMedications
                                              }).ToList()
                    });
                }
            }

            return result;
        }

        public List<PrescriptionViewModel> GetAvailableList()
        {
            List<PrescriptionViewModel> result = context.Prescriptions.Select(rec => new PrescriptionViewModel
            {
                Id = rec.Id,
                Title = rec.Title,
                Price = rec.Price,
                PrescriptionMedications = context.PrescriptionMedications
                    .Where(recPM => recPM.PrescriptionId == rec.Id)
                    .Select(recPM => new PrescriptionMedicationViewModel
                    {
                        Id = recPM.Id,
                        PrescriptionId = recPM.PrescriptionId,
                        MedicationId = recPM.MedicationId,
                        MedicationName = recPM.MedicationName,
                        CountMedications = recPM.CountMedications
                    }).ToList()
            }).ToList();

            List<PrescriptionViewModel> neww = new List<PrescriptionViewModel>();
            foreach (var pr in result)
            {
                bool add = false;
                foreach (var m in pr.PrescriptionMedications)
                {
                    if (m.CountMedications <= context.Medications.FirstOrDefault(rec => rec.Id == m.MedicationId).Count)
                    {
                        add = true;
                    }
                }
                if (add)
                {
                    neww.Add(pr);
                }
            }
            return neww;
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
                    PrescriptionMedications = context.PrescriptionMedications
                        .Where(recPM => recPM.PrescriptionId == element.Id)
                        .Select(recPM => new PrescriptionMedicationViewModel
                        {
                            Id = recPM.Id,
                            PrescriptionId = recPM.PrescriptionId,
                            MedicationId = recPM.MedicationId,
                            MedicationName = recPM.MedicationName,
                            CountMedications = recPM.CountMedications
                        }).ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(PrescriptionBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Prescription element = context.Prescriptions.FirstOrDefault(rec =>
                    rec.Title == model.Title);
                    if (element != null)
                    {
                        throw new Exception("Уже есть рецепт с таким названием");
                    }
                    element = new Prescription
                    {
                        Title = model.Title,
                        Price = model.Price
                    };
                    context.Prescriptions.Add(element);
                    context.SaveChanges();
                    // убираем дубли по лекарствам
                    var groupMedications = model.PrescriptionMedications
                                                .GroupBy(rec => rec.MedicationId)
                                                .Select(rec => new
                                                {
                                                    MedicationId = rec.Key,
                                                    CountMedications = rec.Sum(r => r.CountMedications)
                                                });
                    // запоминаем id и названия лекарств
                    var medicationName = model.PrescriptionMedications.Select(rec => new
                    {
                        MedicationId = rec.MedicationId,
                        MedicationName = rec.MedicationName
                    });
                    // добавляем лекарства
                    foreach (var groupMedication in groupMedications)
                    {
                        string Name = null;
                        foreach (var medication in medicationName)
                        {
                            if (groupMedication.MedicationId == medication.MedicationId)
                            {
                                Name = medication.MedicationName;
                            }
                        }
                        context.PrescriptionMedications.Add(new PrescriptionMedication
                        {
                            PrescriptionId = element.Id,
                            MedicationId = groupMedication.MedicationId,
                            MedicationName = Name,
                            CountMedications = groupMedication.CountMedications
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

        public void UpdElement(PrescriptionBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Prescription element = context.Prescriptions.FirstOrDefault(rec =>
                    rec.Title == model.Title && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть рецепт с таким названием");
                    }
                    element = context.Prescriptions.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.Title = model.Title;
                    element.Price = model.Price;
                    context.SaveChanges();
                    // обновляем существуюущие лекарства
                    var presIds = model.PrescriptionMedications.Select(rec =>
                    rec.MedicationId).Distinct();
                    var updateMedications = context.PrescriptionMedications.Where(rec =>
                    rec.PrescriptionId == model.Id && presIds.Contains(rec.MedicationId));
                    foreach (var updateMedication in updateMedications)
                    {
                        updateMedication.CountMedications =
                        model.PrescriptionMedications.FirstOrDefault(rec => rec.Id == updateMedication.Id).CountMedications;
                    }
                    context.SaveChanges();
                    context.PrescriptionMedications.RemoveRange(context.PrescriptionMedications.Where(rec =>
                    rec.PrescriptionId == model.Id && !presIds.Contains(rec.MedicationId)));
                    context.SaveChanges();
                    // новые записи
                    var groupMedications = model.PrescriptionMedications
                                                .Where(rec => rec.Id == 0)
                                                .GroupBy(rec => rec.MedicationId)
                                                .Select(rec => new
                                                {
                                                    MedicationId = rec.Key,
                                                    CountMedications = rec.Sum(r => r.CountMedications)
                                                });
                    foreach (var groupMedication in groupMedications)
                    {
                        PrescriptionMedication elementPC =
                        context.PrescriptionMedications.FirstOrDefault(rec => rec.PrescriptionId == model.Id &&
                        rec.MedicationId == groupMedication.MedicationId);
                        if (elementPC != null)
                        {
                            elementPC.CountMedications += groupMedication.CountMedications;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.PrescriptionMedications.Add(new PrescriptionMedication
                            {
                                PrescriptionId = model.Id,
                                MedicationId = groupMedication.MedicationId,
                                CountMedications = groupMedication.CountMedications
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

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Prescription element = context.Prescriptions.FirstOrDefault(rec => rec.Id ==
                    id);
                    if (element != null)
                    {
                        // удаяем записи по лекарствам при удалении рецепта
                        context.PrescriptionMedications.RemoveRange(context.PrescriptionMedications.Where(rec =>
                        rec.PrescriptionId == id));
                        context.Prescriptions.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
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
    }
}
