using HospitalModel;
using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalImplementations.Implementations
{
    public class RequestServiceDB : IRequestService
    {
        private HospitalDBContext context;

        public RequestServiceDB()
        {
            this.context = new HospitalDBContext();
        }

        public RequestServiceDB(HospitalDBContext context)
        {
            this.context = context;
        }

        public List<RequestViewModel> GetList()
        {
            List<RequestViewModel> result = context.Requests.Select(rec =>
            new RequestViewModel
            {
                Id = rec.Id,
                RequestName = rec.RequestName,
                Date = rec.Date,
                MedicationRequests = context.MedicationsRequest
                    .Where(recPC => recPC.RequestId == rec.Id)
                    .Select(recPC => new MedicationRequestViewModel
                    {
                        Id = recPC.Id,
                        MedicationId = recPC.MedicationId,
                        RequestId = recPC.RequestId,
                        MedicationName = recPC.MedicationName,
                        CountMedications = recPC.CountMedications
                    }).ToList()
            }).ToList();
            return result;
        }

        public RequestViewModel GetElement(int id)
        {
            Request element = context.Requests.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new RequestViewModel
                {
                    Id = element.Id,
                    RequestName = element.RequestName,
                    Date = element.Date,
                    MedicationRequests = context.MedicationsRequest
                    .Where(recPC => recPC.RequestId == element.Id)
                    .Select(recPC => new MedicationRequestViewModel
                    {
                        Id = recPC.Id,
                        MedicationId = recPC.MedicationId,
                        RequestId = recPC.RequestId,
                        MedicationName = recPC.MedicationName,
                        CountMedications = recPC.CountMedications
                    }).ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(RequestBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Request element = context.Requests.FirstOrDefault(rec =>
                     rec.RequestName == model.RequestName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть запрос с таким названием");
                    }
                    element = new Request
                    {
                        RequestName = model.RequestName,
                        Date = model.Date,
                    };
                    context.Requests.Add(element);
                    context.SaveChanges();
                    //// убираем дубли по компонентам 
                    var groupMedications = model.MedicationRequests
                        .GroupBy(rec => rec.MedicationId)
                        .Select(rec => new
                        {
                            MedicationId = rec.Key,
                            CountMedications = rec.Sum(r => r.CountMedications)
                        });
                    // добавляем компоненты  
                    foreach (var groupMedication in groupMedications)
                    {
                        context.MedicationsRequest.Add(new MedicationRequest
                        {
                            Id = element.Id,
                            MedicationId = groupMedication.MedicationId,
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

        public void UpdElement(RequestBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Request element = context.Requests.FirstOrDefault(rec =>
                    rec.RequestName == model.RequestName && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть запрос с таким названием");
                    }
                    element = context.Requests.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.RequestName = model.RequestName;
                    element.Date = model.Date;
                    context.SaveChanges();

                    // обновляем существуюущие компоненты 
                    var compIds = model.MedicationRequests.Select(rec => rec.MedicationId).Distinct();
                    var updateMedications = context.MedicationsRequest.Where(rec =>
                    rec.RequestId == model.Id && compIds.Contains(rec.MedicationId));
                    foreach (var updateMedication in updateMedications)
                    {
                        updateMedication.CountMedications = model.MedicationRequests.FirstOrDefault(rec =>
                        rec.Id == updateMedication.Id).CountMedications;
                    }
                    context.SaveChanges();
                    context.MedicationsRequest.RemoveRange(context.MedicationsRequest.Where(rec =>
                    rec.RequestId == model.Id && !compIds.Contains(rec.MedicationId)));
                    context.SaveChanges();
                    // новые записи  
                    var groupMedications = model.MedicationRequests.Where(rec =>
                    rec.Id == 0).GroupBy(rec => rec.MedicationId).Select(rec => new
                    {
                        MedicationId = rec.Key,
                        CountMedications = rec.Sum(r => r.CountMedications)
                    });
                    foreach (var groupMedication in groupMedications)

                    {
                        MedicationRequest elementPC = context.MedicationsRequest.FirstOrDefault(rec =>
                        rec.RequestId == model.Id && rec.MedicationId == groupMedication.MedicationId);
                        if (elementPC != null)
                        {
                            elementPC.CountMedications += groupMedication.CountMedications;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.MedicationsRequest.Add(new MedicationRequest
                            {
                                RequestId = model.Id,
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

        void IRequestService.DelElement(int id)
        {
            throw new NotImplementedException();
        }
    }
}
