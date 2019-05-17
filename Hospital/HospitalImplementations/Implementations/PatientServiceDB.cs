using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalModel;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.ViewModels;
using HospitalServiceDAL;

namespace HospitalImplementations.Implementations
{
    public class PatientServiceDB : IPatientService
    {
        private HospitalDBContext context;

        public PatientServiceDB(HospitalDBContext context)
        {
            this.context = context;
        }

        public List<PatientViewModel> GetList()
        {
            List<PatientViewModel> result = context.Patients.Select(rec =>
            new PatientViewModel
            {
                Id = rec.Id,
                FIO = rec.FIO,
                Email = rec.Email,
                Password = rec.Password
            })
            .ToList();
            return result;
        }

        public PatientViewModel GetElement(string email, string password)
        {
            Patient element = context.Patients.FirstOrDefault(rec => rec.Email == email && rec.Password == password);

            if (element != null)
            {
                return new PatientViewModel
                {
                    Id = element.Id,
                    FIO = element.FIO,
                    Email = element.Email,
                    Password = element.Password
                };
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(PatientBindingModel model)
        {
            Patient element = context.Patients.FirstOrDefault(rec => rec.Email == model.Email);

            if (element != null)
            {
                throw new Exception("Уже есть пациент с такой почтой");
            }
            context.Patients.Add(new Patient
            {
                FIO = model.FIO,
                Email = model.Email,
                Password = model.Password
            });
            context.SaveChanges();
        }
    }
}
