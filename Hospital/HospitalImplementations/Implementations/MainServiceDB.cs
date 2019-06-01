using HospitalModel;
using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Mail;

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
                Date = SqlFunctions.DateName("dd", rec.Date) + " " +
                       SqlFunctions.DateName("mm", rec.Date) + " " +
                       SqlFunctions.DateName("yyyy", rec.Date),
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
            List<TreatmentViewModel> result = context.Treatments.
                Where(rec => rec.PatientId == PatientId).
                Select(rec => new TreatmentViewModel
                {
                    Id = rec.Id,
                    Date = SqlFunctions.DateName("dd", rec.Date) + " " +
                           SqlFunctions.DateName("mm", rec.Date) + " " +
                           SqlFunctions.DateName("yyyy", rec.Date),
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

        public TreatmentViewModel GetTreatment(int id)
        {
            Treatment element = context.Treatments.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new TreatmentViewModel
                {
                    Id = element.Id,
                    Date = element.Date.ToShortDateString(),
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
                        Date = DateTime.Now,
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

        public void UpdTreatment(TreatmentBindingModel model)
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

                    // обновляем существуюущие рецепты 
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

        public void DelTreatment(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Treatment element = context.Treatments.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        // удаяем записи по рецептам при удалении лечения
                        context.TreatmentPrescriptions.RemoveRange(context.TreatmentPrescriptions.Where(rec => rec.TreatmentId == id));
                        context.Treatments.Remove(element);
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

        public void TreatmentReservation(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Treatment element = context.Treatments.FirstOrDefault(rec => rec.Id == id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    if (element.isReserved)
                    {
                        throw new Exception("Лекарства по этому лечению уже зарезервированы");
                    }
                    else
                    {
                        element.isReserved = true;
                    }
                    List<PrescriptionMedicationViewModel> prepmed = new List<PrescriptionMedicationViewModel>();
                    var treatmentPrescriptions = context.TreatmentPrescriptions.Where(rec => rec.TreatmentId == element.Id).Select(rec => new TreatmentPrescriptionViewModel
                    {
                        PrescriptionId = rec.PrescriptionId,
                        Count = rec.Count
                    });
                    foreach (var pres in treatmentPrescriptions)
                    {
                        var prescriptionMedications = context.PrescriptionMedications.Where(rec => rec.PrescriptionId == pres.PrescriptionId).Select(rec => new PrescriptionMedicationViewModel
                        {
                            MedicationId = rec.MedicationId,
                            CountMedications = rec.CountMedications
                        });
                        foreach (var med in prescriptionMedications)
                        {
                            bool flag = false;
                            for (int i = 0; i < prepmed.Count(); i++)
                            {
                                if (prepmed[i].MedicationId == med.MedicationId)
                                {
                                    prepmed[i].CountMedications += med.CountMedications;
                                    flag = true;
                                }
                            }
                            if (!flag)
                            {
                                prepmed.Add(med);
                                prepmed.Last().CountMedications = med.CountMedications * pres.Count;
                            }
                        }
                    }
                    var Medications = context.Medications.Select(rec => new MedicationViewModel
                    {
                        Id = rec.Id,
                        Count = rec.Count
                    }).ToList();

                    for (int i = 0; i < prepmed.Count(); i++)
                    {
                        var index = prepmed[i].MedicationId;
                        var Med = context.Medications.Where(rec => rec.Id == index);
                        foreach (var med in Med)
                        {
                            if (prepmed[i].CountMedications <= med.Count)
                            {
                                med.Count -= prepmed[i].CountMedications;
                                context.SaveChanges();
                            }
                            else
                            {
                                throw new Exception("Это лечение пока не доступно для бронирования, попробуйте позже");
                            }
                        }
                    }
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
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
                    MedicationName = model.MedicationName,
                    RequestId = model.RequestId,
                    CountMedications = model.CountMedications
                });
            }
            context.Medications.FirstOrDefault(res => res.Id == model.MedicationId).Count += model.CountMedications;
            context.SaveChanges();            
        }

        public void SendEmail(string mailAddress, string subject, string text, string path)
        {
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpClient = null;
            try
            {
                objMailMessage.From = new MailAddress(ConfigurationManager.AppSettings["MailLogin"]);
                objMailMessage.To.Add(new MailAddress(mailAddress));
                objMailMessage.Subject = subject;
                objMailMessage.Body = text;
                objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                objMailMessage.Attachments.Add(new Attachment(path));

                objSmtpClient = new SmtpClient("smtp.gmail.com", 587);
                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new
                NetworkCredential(ConfigurationManager.AppSettings["MailLogin"],
                ConfigurationManager.AppSettings["MailPassword"]);

                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpClient = null;
            }
        }
    }
}
