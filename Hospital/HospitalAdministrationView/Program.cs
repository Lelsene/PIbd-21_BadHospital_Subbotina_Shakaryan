using HospitalImplementations;
using HospitalImplementations.Implementations;
using HospitalServiceDAL.Interfaces;
using System;
using System.Data.Entity;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace HospitalAdministrationView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormAuthorization>());
        }
        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<DbContext, HospitalDBContext>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPatientService, PatientServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IRequestService, RequestServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMedicationService, MedicationServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPrescriptionService, PrescriptionServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceDB>(new HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}
