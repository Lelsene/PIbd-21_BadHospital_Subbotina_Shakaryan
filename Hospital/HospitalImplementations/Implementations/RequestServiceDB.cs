using HospitalModel;
using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
                MedicationRequests = context.RequestMedications
                    .Where(recPC => recPC.RequestId == rec.Id)
                    .Select(recPC => new RequestMedicationViewModel
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
                    MedicationRequests = context.RequestMedications
                    .Where(recPC => recPC.RequestId == element.Id)
                    .Select(recPC => new RequestMedicationViewModel
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

        public int AddElement(RequestBindingModel model)
        {
            int id = -1;
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
                    var Rec = context.Requests.OrderByDescending(p => p.Id).Take(1);
                    foreach (var req in Rec)
                    {
                        id = req.Id;
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return id;
        }
    }
}
