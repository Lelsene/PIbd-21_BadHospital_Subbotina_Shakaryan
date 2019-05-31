using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using HospitalServiceDAL.Interfaces;
using HospitalImplementations;
using HospitalImplementations.Implementations;
using System.Data.Entity;
using Unity.Lifetime;

namespace HospitalPatientView
{
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<DbContext, HospitalDBContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IPatientService, PatientServiceDB>(new HierarchicalLifetimeManager());
            container.RegisterType<IMainService, MainServiceDB>(new HierarchicalLifetimeManager());

        }
    }
}