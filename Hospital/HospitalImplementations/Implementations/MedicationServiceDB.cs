using HospitalModel;
using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalImplementations.Implementations
{
    public class MedicationServiceDB : IMedicationService
    {
        private HospitalDBContext context;

        public MedicationServiceDB(HospitalDBContext context)
        {
            this.context = context;
        }

        public MedicationServiceDB()
        {
            this.context = new HospitalDBContext();
        }

        public List<MedicationViewModel> GetList()
        {
            List<MedicationViewModel> result = context.Medications.Select(rec => new
            MedicationViewModel
            {
                Id = rec.Id,
                Name = rec.Name,
                Price = rec.Price,
                Count = rec.Count
            })
            .ToList();
            return result;
        }

        public MedicationViewModel GetElement(int id)
        {
            Medication element = context.Medications.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new MedicationViewModel
                {
                    Id = element.Id,
                    Name = element.Name,
                    Price = element.Price,
                    Count = element.Count
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(MedicationBindingModel model)
        {
            Medication element = context.Medications.FirstOrDefault(rec => rec.Name ==
            model.Name);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            context.Medications.Add(new Medication
            {
                Name = model.Name,
                Price = model.Price,
                Count = model.Count
            });
            context.SaveChanges();
        }

        public void UpdElement(MedicationBindingModel model)
        {
            Medication element = context.Medications.FirstOrDefault(rec => rec.Name ==
            model.Name && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = context.Medications.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Name = model.Name;
            element.Price = model.Price;
            element.Count = model.Count;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Medication element = context.Medications.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Medications.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
