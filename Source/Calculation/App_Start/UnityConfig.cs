using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Service;
using Data.Models;
using Calculations.Controllers;
using Data.Infrastructure;
using AutoMapper;
using Model.ViewModel;
using Model.Models;

namespace Calculations.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<IReportLineService, ReportLineService>(new PerRequestLifetimeManager());
            container.RegisterType<IEmployeeService, EmployeeService>(new PerRequestLifetimeManager());

            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ApplicationDbContext, ApplicationDbContext>("New");

            container.RegisterType<IDataContext, ApplicationDbContext>(new PerRequestLifetimeManager());
            container.RegisterType<IUnitOfWorkForService, UnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager());

            container.RegisterType<IComboHelpListService, ComboHelpListService>(new PerRequestLifetimeManager());
            container.RegisterType<IExceptionHandlingService, ExceptionHandlingService>(new PerRequestLifetimeManager());

            container.RegisterType<ICalculationService, CalculationService>(new PerRequestLifetimeManager());
            container.RegisterType<ICalculationFooterService, CalculationFooterService>(new PerRequestLifetimeManager());
            container.RegisterType<ICalculationProductService, CalculationProductService>(new PerRequestLifetimeManager());
            container.RegisterType<IChargeService, ChargeService>(new PerRequestLifetimeManager());
            container.RegisterType<IChargeGroupPersonService, ChargeGroupPersonService>(new PerRequestLifetimeManager());
            container.RegisterType<IChargeGroupProductService, ChargeGroupProductService>(new PerRequestLifetimeManager());
            container.RegisterType<IChargeService, ChargeService>(new PerRequestLifetimeManager());

            container.RegisterType<ICalculationHeaderLedgerAccountService, CalculationHeaderLedgerAccountService>(new PerRequestLifetimeManager());
            container.RegisterType<ICalculationLineLedgerAccountService, CalculationLineLedgerAccountService>(new PerRequestLifetimeManager());

            container.RegisterType<IDuplicateDocumentCheckService, DuplicateDocumentCheckService>(new PerRequestLifetimeManager());


            //Registering Mappers::::

            Mapper.CreateMap<CalculationFooterViewModel, CalculationFooter>();
            Mapper.CreateMap<CalculationFooter, CalculationFooterViewModel>();

            Mapper.CreateMap<CalculationProductViewModel, CalculationProduct>();
            Mapper.CreateMap<CalculationProduct, CalculationProductViewModel>();

            Mapper.CreateMap<CalculationHeaderLedgerAccount, CalculationHeaderLedgerAccountViewModel>();
            Mapper.CreateMap<CalculationHeaderLedgerAccountViewModel, CalculationHeaderLedgerAccount>();

            Mapper.CreateMap<CalculationLineLedgerAccount, CalculationLineLedgerAccountViewModel>();
            Mapper.CreateMap<CalculationLineLedgerAccountViewModel, CalculationLineLedgerAccount>();

            Mapper.CreateMap<HeaderChargeViewModel, HeaderChargeViewModel>();
            Mapper.CreateMap<LineChargeViewModel, LineChargeViewModel>();


            Mapper.CreateMap<Calculation, Calculation>();
            Mapper.CreateMap<CalculationFooter, CalculationFooter>();
            Mapper.CreateMap<CalculationProduct, CalculationProduct>();
            Mapper.CreateMap<CalculationHeaderLedgerAccount, CalculationHeaderLedgerAccount>();
            Mapper.CreateMap<CalculationLineLedgerAccount, CalculationLineLedgerAccount>();
            Mapper.CreateMap<Charge, Charge>();
            Mapper.CreateMap<ChargeGroupPerson, ChargeGroupPerson>();
            Mapper.CreateMap<ChargeGroupProduct, ChargeGroupProduct>();
            Mapper.CreateMap<ChargeType, ChargeType>();
        }
    }
}
