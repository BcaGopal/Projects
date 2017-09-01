using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Finance.Controllers;
using Data.Models;
using Data.Infrastructure;
using Service;
using Model.Models;
using AutoMapper;
using Model.ViewModel;
using Model.ViewModels;

namespace Finance.App_Start
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

            
            container.RegisterType<AccountController>(new InjectionConstructor());
            //container.RegisterType<ApplicationDbContext, ApplicationDbContext>("New");
            container.RegisterType<IDuplicateDocumentCheckService, DuplicateDocumentCheckService>(new PerRequestLifetimeManager());
            container.RegisterType<IDataContext, ApplicationDbContext>(new PerRequestLifetimeManager());
            container.RegisterType<IUnitOfWorkForService, UnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager());

            container.RegisterType<IExceptionHandlingService, ExceptionHandlingService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<City>, Repository<City>>();
            container.RegisterType<ICityService, CityService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<CostCenter>, Repository<CostCenter>>();
            container.RegisterType<ICostCenterService, CostCenterService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Country>, Repository<Country>>();
            container.RegisterType<ICountryService, CountryService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Currency>, Repository<Currency>>();
            container.RegisterType<ICurrencyService, CurrencyService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Division>, Repository<Division>>();
            container.RegisterType<IDivisionService, DivisionService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<DocumentCategory>, Repository<DocumentCategory>>();
            container.RegisterType<IDocumentCategoryService, DocumentCategoryService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<DocumentType>, Repository<DocumentType>>();
            container.RegisterType<IDocumentTypeService, DocumentTypeService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<LedgerAccount>, Repository<LedgerAccount>>();
            container.RegisterType<IAccountService, AccountService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<LedgerAccount>, Repository<LedgerAccount>>();
            container.RegisterType<ILedgerAccountService, LedgerAccountService>(new PerRequestLifetimeManager());

            container.RegisterType<ILedgerAccountOpeningService, LedgerAccountOpeningService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<LedgerAccountGroup>, Repository<LedgerAccountGroup>>();
            container.RegisterType<ILedgerAccountGroupService, LedgerAccountGroupService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Ledger>, Repository<Ledger>>();
            container.RegisterType<ILedgerService, LedgerService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<LedgerHeader>, Repository<LedgerHeader>>();
            container.RegisterType<ILedgerHeaderService, LedgerHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Narration>, Repository<Narration>>();
            container.RegisterType<INarrationService, NarrationService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonAddress>, Repository<PersonAddress>>();
            container.RegisterType<IPersonAddressService, PersonAddressService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonBankAccount>, Repository<PersonBankAccount>>();
            container.RegisterType<IPersonBankAccountService, PersonBankAccountService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Person>, Repository<Person>>();
            container.RegisterType<IPersonService, PersonService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonContact>, Repository<PersonContact>>();
            container.RegisterType<IPersonContactService, PersonContactService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonContactType>, Repository<PersonContactType>>();
            container.RegisterType<IPersonContactTypeService, PersonContactTypeService>(new PerRequestLifetimeManager());            

            container.RegisterType<IRepository<Reason>, Repository<Reason>>();
            container.RegisterType<IReasonService, ReasonService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Site>, Repository<Site>>();
            container.RegisterType<ISiteService, SiteService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<State>, Repository<State>>();
            container.RegisterType<IStateService, StateService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<TdsCategory>, Repository<TdsCategory>>();
            container.RegisterType<ITdsCategoryService, TdsCategoryService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<TdsGroup>, Repository<TdsGroup>>();
            container.RegisterType<ITdsGroupService, TdsGroupService>(new PerRequestLifetimeManager());

            container.RegisterType<IReportLineService, ReportLineService>(new PerRequestLifetimeManager());

            container.RegisterType<ITrialBalanceService, TrialBalanceService>(new PerRequestLifetimeManager());

            container.RegisterType<ITrialBalanceSettingService, TrialBalanceSettingService>(new PerRequestLifetimeManager());

            container.RegisterType<IActivityLogService, ActivityLogService>(new PerRequestLifetimeManager());

            container.RegisterType<ILedgerSettingService, LedgerSettingService>(new PerRequestLifetimeManager());

            container.RegisterType<ILedgerLineRefValueService, LedgerLineRefValueService>(new PerRequestLifetimeManager());
            //Registering Mappers:


            Mapper.CreateMap<LedgerAccount, LedgerAccountViewModel>();
            Mapper.CreateMap<LedgerAccountViewModel, LedgerAccount>();

            Mapper.CreateMap<Person, PersonContactViewModel>();
            Mapper.CreateMap<PersonContactViewModel, Person>();

            Mapper.CreateMap<PersonContact, PersonContactViewModel>();
            Mapper.CreateMap<PersonContactViewModel, PersonContact>();

            Mapper.CreateMap<PersonAddress, AgentViewModel>();
            Mapper.CreateMap<AgentViewModel, PersonAddress>();

            Mapper.CreateMap<PersonAddress, EmployeeViewModel>();
            Mapper.CreateMap<EmployeeViewModel, PersonAddress>();

            Mapper.CreateMap<PersonAddress, SupplierViewModel>();
            Mapper.CreateMap<SupplierViewModel, PersonAddress>();

            Mapper.CreateMap<PersonAddress, JobWorkerViewModel>();
            Mapper.CreateMap<JobWorkerViewModel, PersonAddress>();

            Mapper.CreateMap<PersonAddress, ManufacturerViewModel>();
            Mapper.CreateMap<ManufacturerViewModel, PersonAddress>();

            Mapper.CreateMap<PersonAddress, CourierViewModel>();
            Mapper.CreateMap<CourierViewModel, PersonAddress>();

            Mapper.CreateMap<PersonAddress, TransporterViewModel>();
            Mapper.CreateMap<TransporterViewModel, PersonAddress>();

            Mapper.CreateMap<PersonAddress, BuyerViewModel>();
            Mapper.CreateMap<BuyerViewModel, PersonAddress>();

            Mapper.CreateMap<LedgerHeader, LedgerHeaderViewModel>();
            Mapper.CreateMap<LedgerHeaderViewModel, LedgerHeader>();

            Mapper.CreateMap<CityViewModel, City>();
            Mapper.CreateMap<City, CityViewModel>();

            Mapper.CreateMap<LedgerAccount, BuyerViewModel>();
            Mapper.CreateMap<BuyerViewModel, LedgerAccount>();

            Mapper.CreateMap<LedgerAccount, TransporterViewModel>();
            Mapper.CreateMap<TransporterViewModel, LedgerAccount>();

            Mapper.CreateMap<LedgerAccount, CourierViewModel>();
            Mapper.CreateMap<CourierViewModel, LedgerAccount>();

            Mapper.CreateMap<LedgerAccount, ManufacturerViewModel>();
            Mapper.CreateMap<ManufacturerViewModel, LedgerAccount>();

            Mapper.CreateMap<LedgerAccount, JobWorkerViewModel>();
            Mapper.CreateMap<JobWorkerViewModel, LedgerAccount>();

            Mapper.CreateMap<LedgerAccount, SupplierViewModel>();
            Mapper.CreateMap<SupplierViewModel, LedgerAccount>();

            Mapper.CreateMap<LedgerAccount, EmployeeViewModel>();
            Mapper.CreateMap<EmployeeViewModel, LedgerAccount>();

            Mapper.CreateMap<LedgerAccount, AgentViewModel>();
            Mapper.CreateMap<AgentViewModel, LedgerAccount>();

            Mapper.CreateMap<LedgersViewModel, Ledger>();
            Mapper.CreateMap<Ledger, LedgersViewModel>();

            Mapper.CreateMap<HeaderChargeViewModel, HeaderChargeViewModel>();
            Mapper.CreateMap<LineChargeViewModel, LineChargeViewModel>();

            Mapper.CreateMap<LedgerSetting, LedgerSettingViewModel>();
            Mapper.CreateMap<LedgerSettingViewModel, LedgerSetting>();

            Mapper.CreateMap<LedgerHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<LedgerHeader, DocumentUniqueId>();


            Mapper.CreateMap<City, City>();
            Mapper.CreateMap<CostCenter, CostCenter>();
            Mapper.CreateMap<Country, Country>();
            Mapper.CreateMap<Currency, Currency>();
            Mapper.CreateMap<Division, Division>();
            Mapper.CreateMap<DocumentCategory, DocumentCategory>();
            Mapper.CreateMap<DocumentType, DocumentType>();
            Mapper.CreateMap<LedgerAccount, LedgerAccount>();
            Mapper.CreateMap<LedgerAccountGroup, LedgerAccountGroup>();
            Mapper.CreateMap<LedgerHeader, LedgerHeader>();
            Mapper.CreateMap<LedgerLine, LedgerLine>();
            Mapper.CreateMap<Narration, Narration>();
            Mapper.CreateMap<Person, Person>();
            Mapper.CreateMap<PersonContact, PersonContact>();
            Mapper.CreateMap<PersonContactType, PersonContactType>();
            Mapper.CreateMap<PersonDocument, PersonDocument>();
            Mapper.CreateMap<Reason, Reason>();
            Mapper.CreateMap<Site, Site>();
            Mapper.CreateMap<State, State>();
            Mapper.CreateMap<TdsCategory, TdsCategory>();
            Mapper.CreateMap<TdsGroup, TdsGroup>();
        }
    }
}
