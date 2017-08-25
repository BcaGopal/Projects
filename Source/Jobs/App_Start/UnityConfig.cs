using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Jobs.Controllers;
using Data.Models;
using Data.Infrastructure;
using Service;
using Model.Models;
using AutoMapper;
using Model.ViewModel;
using Model.ViewModels;

namespace Jobs.App_Start
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

            container.RegisterType<IRepository<SaleOrderHeader>, Repository<SaleOrderHeader>>();
            container.RegisterType<Service.ISaleOrderHeaderService, Service.SaleOrderHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleOrderLine>, Repository<SaleOrderLine>>();
            container.RegisterType<Service.ISaleOrderLineService, Service.SaleOrderLineService>(new PerRequestLifetimeManager());




            container.RegisterType<IRepository<SaleInvoiceHeaderCharge>, Repository<SaleInvoiceHeaderCharge>>();
            container.RegisterType<ISaleInvoiceHeaderChargeService, SaleInvoiceHeaderChargeService>(new PerRequestLifetimeManager());
            
            container.RegisterType<AccountController>(new InjectionConstructor());
            //container.RegisterType<ApplicationDbContext, ApplicationDbContext>("New");

            container.RegisterType<IDataContext, ApplicationDbContext>(new PerRequestLifetimeManager());
            container.RegisterType<IUnitOfWorkForService, UnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<BusinessEntity>, Repository<BusinessEntity>>();
            container.RegisterType<IBusinessEntityService, BusinessEntityService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<LedgerAccount>, Repository<LedgerAccount>>();
            container.RegisterType<IAccountService, AccountService>(new PerRequestLifetimeManager());

            container.RegisterType<IExceptionHandlingService, ExceptionHandlingService>(new PerRequestLifetimeManager());           

            container.RegisterType<IRepository<JobOrderHeader>, Repository<JobOrderHeader>>();
            container.RegisterType<IJobOrderHeaderService, JobOrderHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobOrderLine>, Repository<JobOrderLine>>();
            container.RegisterType<IJobOrderLineService, JobOrderLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobOrderSettings>, Repository<JobOrderSettings>>();
            container.RegisterType<IJobOrderSettingsService, JobOrderSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobInvoiceSettings>, Repository<JobInvoiceSettings>>();
            container.RegisterType<IJobInvoiceSettingsService, JobInvoiceSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobConsumptionSettings>, Repository<JobConsumptionSettings>>();
            container.RegisterType<IJobConsumptionSettingsService, JobConsumptionSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobReceiveSettings>, Repository<JobReceiveSettings>>();
            container.RegisterType<IJobReceiveSettingsService, JobReceiveSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobReceiveHeader>, Repository<JobReceiveHeader>>();
            container.RegisterType<IJobReceiveHeaderService, JobReceiveHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobReceiveLine>, Repository<JobReceiveLine>>();
            container.RegisterType<IJobReceiveLineService, JobReceiveLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobReceiveQAAttribute>, Repository<JobReceiveQAAttribute>>();
            container.RegisterType<IJobReceiveQAAttributeService, JobReceiveQAAttributeService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobReceiveQAPenalty>, Repository<JobReceiveQAPenalty>>();
            container.RegisterType<IJobReceiveQAPenaltyService, JobReceiveQAPenaltyService>(new PerRequestLifetimeManager());


            container.RegisterType<IRepository<JobReturnHeader>, Repository<JobReturnHeader>>();
            container.RegisterType<IJobReturnHeaderService, JobReturnHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobReturnLine>, Repository<JobReturnLine>>();
            container.RegisterType<IJobReturnLineService, JobReturnLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobReceiveBom>, Repository<JobReceiveBom>>();
            container.RegisterType<IJobReceiveBomService, JobReceiveBomService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobReceiveByProduct>, Repository<JobReceiveByProduct>>();
            container.RegisterType<IJobReceiveByProductService, JobReceiveByProductService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobOrderBom>, Repository<JobOrderBom>>();
            container.RegisterType<IJobOrderBomService, JobOrderBomService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobInvoiceHeader>, Repository<JobInvoiceHeader>>();
            container.RegisterType<IJobInvoiceHeaderService, JobInvoiceHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobInvoiceLine>, Repository<JobInvoiceLine>>();
            container.RegisterType<IJobInvoiceLineService, JobInvoiceLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobOrderCancelHeader>, Repository<JobOrderCancelHeader>>();
            container.RegisterType<IJobOrderCancelHeaderService, JobOrderCancelHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobOrderCancelLine>, Repository<JobOrderCancelLine>>();
            container.RegisterType<IJobOrderCancelLineService, JobOrderCancelLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobOrderAmendmentHeader>, Repository<JobOrderAmendmentHeader>>();
            container.RegisterType<IJobOrderAmendmentHeaderService, JobOrderAmendmentHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobOrderRateAmendmentLine>, Repository<JobOrderRateAmendmentLine>>();
            container.RegisterType<IJobOrderRateAmendmentLineService, JobOrderRateAmendmentLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobInvoiceAmendmentHeader>, Repository<JobInvoiceAmendmentHeader>>();
            container.RegisterType<IJobInvoiceAmendmentHeaderService, JobInvoiceAmendmentHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobInvoiceRateAmendmentLine>, Repository<JobInvoiceRateAmendmentLine>>();
            container.RegisterType<IJobInvoiceRateAmendmentLineService, JobInvoiceRateAmendmentLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobWorker>, Repository<JobWorker>>();
            container.RegisterType<IJobWorkerService, JobWorkerService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonAddress>, Repository<PersonAddress>>();
            container.RegisterType<IPersonAddressService, PersonAddressService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonProcess>, Repository<PersonProcess>>();
            container.RegisterType<IPersonProcessService, PersonProcessService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonRegistration>, Repository<PersonRegistration>>();
            container.RegisterType<IPersonRegistrationService, PersonRegistrationService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Person>, Repository<Person>>();
            container.RegisterType<IPersonService, PersonService>(new PerRequestLifetimeManager()); //

            container.RegisterType<IRepository<ProductUid>, Repository<ProductUid>>();
            container.RegisterType<IProductUidService, ProductUidService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<RateConversionSettings>, Repository<RateConversionSettings>>();
            container.RegisterType<IRateConversionSettingsService, RateConversionSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<StockHeader>, Repository<StockHeader>>();
            container.RegisterType<IStockHeaderService, StockHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<StockLine>, Repository<StockLine>>();
            container.RegisterType<IStockLineService, StockLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IReportLineService, ReportLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IActivityLogService, ActivityLogService>(new PerRequestLifetimeManager());

            container.RegisterType<IJobOrderHeaderChargeService, JobOrderHeaderChargeService>(new PerRequestLifetimeManager());

            container.RegisterType<IJobOrderLineChargeService, JobOrderLineChargeService>(new PerRequestLifetimeManager());

            container.RegisterType<IChargesCalculationService, ChargesCalculationService>(new PerRequestLifetimeManager());            

            container.RegisterType<IJobInvoiceHeaderChargeService, JobInvoiceHeaderChargeService>(new PerRequestLifetimeManager());

            container.RegisterType<IJobInvoiceLineChargeService, JobInvoiceLineChargeService>(new PerRequestLifetimeManager());

            container.RegisterType<IDuplicateDocumentCheckService, DuplicateDocumentCheckService>(new PerRequestLifetimeManager());

            container.RegisterType<IPersonContactService, PersonContactService>(new PerRequestLifetimeManager());

            container.RegisterType<IPersonBankAccountService, PersonBankAccountService>(new PerRequestLifetimeManager());

            container.RegisterType<IRateListService, RateListService>(new PerRequestLifetimeManager());

            container.RegisterType<IExcessJobReviewService, ExcessJobReviewService>(new PerRequestLifetimeManager());
            container.RegisterType<IUpdateJobExpiryService, UpdateJobExpiryService>(new PerRequestLifetimeManager());

            container.RegisterType<IJobOrderInspectionRequestHeaderService, JobOrderInspectionRequestHeaderService>(new PerRequestLifetimeManager());
            container.RegisterType<IJobOrderInspectionRequestLineService, JobOrderInspectionRequestLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IJobOrderInspectionRequestSettingsService, JobOrderInspectionRequestSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IJobOrderInspectionSettingsService, JobOrderInspectionSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IJobOrderInspectionRequestCancelHeaderService, JobOrderInspectionRequestCancelHeaderService>(new PerRequestLifetimeManager());
            container.RegisterType<IJobOrderInspectionRequestCancelLineService, JobOrderInspectionRequestCancelLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IJobOrderInspectionHeaderService, JobOrderInspectionHeaderService>(new PerRequestLifetimeManager());
            container.RegisterType<IJobOrderInspectionLineService, JobOrderInspectionLineService>(new PerRequestLifetimeManager());


            //Registering Mappers:
            Mapper.CreateMap<BusinessEntity, AgentViewModel>();
            Mapper.CreateMap<AgentViewModel, BusinessEntity>();

            Mapper.CreateMap<BusinessEntity, EmployeeViewModel>();
            Mapper.CreateMap<EmployeeViewModel, BusinessEntity>();

            Mapper.CreateMap<BusinessEntity, SupplierViewModel>();
            Mapper.CreateMap<SupplierViewModel, BusinessEntity>();

            Mapper.CreateMap<BusinessEntity, JobWorkerViewModel>();
            Mapper.CreateMap<JobWorkerViewModel, BusinessEntity>();

            Mapper.CreateMap<Person, JobWorkerViewModel>();
            Mapper.CreateMap<JobWorkerViewModel, Person >();

            Mapper.CreateMap<JobWorker, JobWorkerViewModel>();
            Mapper.CreateMap<JobWorkerViewModel, JobWorker>();


            Mapper.CreateMap<PersonAddress, JobWorkerViewModel>();
            Mapper.CreateMap<JobWorkerViewModel, PersonAddress>();


            Mapper.CreateMap<LedgerAccount, JobWorkerViewModel>();
            Mapper.CreateMap<JobWorkerViewModel, LedgerAccount>();

            Mapper.CreateMap<BusinessEntity, ManufacturerViewModel>();
            Mapper.CreateMap<ManufacturerViewModel, BusinessEntity>();

            Mapper.CreateMap<BusinessEntity, CourierViewModel>();
            Mapper.CreateMap<CourierViewModel, BusinessEntity>();

            Mapper.CreateMap<BusinessEntity, TransporterViewModel>();
            Mapper.CreateMap<TransporterViewModel, BusinessEntity>();

            Mapper.CreateMap<BusinessEntity, BuyerViewModel>();
            Mapper.CreateMap<BuyerViewModel, BusinessEntity>();

            Mapper.CreateMap<JobOrderHeader, JobOrderHeaderViewModel>();
            Mapper.CreateMap<JobOrderHeaderViewModel, JobOrderHeader>();

            Mapper.CreateMap<JobOrderLine, JobOrderLineViewModel>();
            Mapper.CreateMap<JobOrderLineViewModel, JobOrderLine>();

            Mapper.CreateMap<JobOrderSettings, JobOrderSettingsViewModel>();
            Mapper.CreateMap<JobOrderSettingsViewModel, JobOrderSettings>();

            Mapper.CreateMap<JobConsumptionSettings, JobConsumptionSettingsViewModel>();
            Mapper.CreateMap<JobConsumptionSettingsViewModel, JobConsumptionSettings>();

            Mapper.CreateMap<JobReceiveSettings, JobReceiveSettingsViewModel>();
            Mapper.CreateMap<JobReceiveSettingsViewModel, JobReceiveSettings>();

            Mapper.CreateMap<JobOrderPerk, PerkViewModel>();
            Mapper.CreateMap<PerkViewModel, JobOrderPerk>().ForMember(m => m.CreatedDate, x => x.Ignore()).ForMember(m => m.CreatedBy, x => x.Ignore())
                .ForMember(m => m.ModifiedDate, x => x.Ignore())
                .ForMember(m => m.ModifiedBy, x => x.Ignore());

            Mapper.CreateMap<Perk, PerkViewModel>();
            Mapper.CreateMap<PerkViewModel, Perk>();

            Mapper.CreateMap<LineChargeViewModel, CalculationProduct>().ForMember(m => m.CalculationProductId , x => x.Ignore());
            Mapper.CreateMap<CalculationProduct, LineChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<LineChargeViewModel, JobOrderLineCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<JobOrderLineCharge, LineChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<HeaderChargeViewModel, JobOrderHeaderCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<JobOrderHeaderCharge, HeaderChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());           

            Mapper.CreateMap<LineChargeViewModel, JobInvoiceLineCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<JobInvoiceLineCharge, LineChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());
            
            Mapper.CreateMap<HeaderChargeViewModel, JobInvoiceHeaderCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<JobInvoiceHeaderCharge, HeaderChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<LineChargeViewModel, JobInvoiceReturnLineCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<JobInvoiceReturnLineCharge, LineChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<HeaderChargeViewModel, JobInvoiceReturnHeaderCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<JobInvoiceReturnHeaderCharge, HeaderChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());


            Mapper.CreateMap<JobReceiveHeader, JobReceiveHeaderViewModel>();
            Mapper.CreateMap<JobReceiveHeaderViewModel, JobReceiveHeader>();

            Mapper.CreateMap<JobReceiveLine, JobReceiveLineViewModel>();
            Mapper.CreateMap<JobReceiveLineViewModel, JobReceiveLine>();

            Mapper.CreateMap<JobReturnHeader, JobReturnHeaderViewModel>();
            Mapper.CreateMap<JobReturnHeaderViewModel, JobReturnHeader>();

            Mapper.CreateMap<JobReturnLine, JobReturnLineViewModel>();
            Mapper.CreateMap<JobReturnLineViewModel, JobReturnLine>();


            Mapper.CreateMap<JobInvoiceHeader, JobInvoiceHeaderViewModel>();
            Mapper.CreateMap<JobInvoiceHeaderViewModel, JobInvoiceHeader>();

            Mapper.CreateMap<JobInvoiceLine, JobInvoiceLineViewModel>();
            Mapper.CreateMap<JobInvoiceLineViewModel, JobInvoiceLine>();

            Mapper.CreateMap<JobOrderCancelHeader, JobOrderCancelHeaderViewModel>();
            Mapper.CreateMap<JobOrderCancelHeaderViewModel, JobOrderCancelHeader>();

            Mapper.CreateMap<JobOrderCancelLine, JobOrderCancelLineViewModel>();
            Mapper.CreateMap<JobOrderCancelLineViewModel, JobOrderCancelLine>();

            Mapper.CreateMap<JobOrderAmendmentHeader, JobOrderAmendmentHeaderViewModel>();
            Mapper.CreateMap<JobOrderAmendmentHeaderViewModel, JobOrderAmendmentHeader>();

            Mapper.CreateMap<JobOrderRateAmendmentLine, JobOrderRateAmendmentLineViewModel>();
            Mapper.CreateMap<JobOrderRateAmendmentLineViewModel, JobOrderRateAmendmentLine>();

            Mapper.CreateMap<JobInvoiceAmendmentHeader, JobInvoiceAmendmentHeaderViewModel>();
            Mapper.CreateMap<JobInvoiceAmendmentHeaderViewModel, JobInvoiceAmendmentHeader>();

            Mapper.CreateMap<JobInvoiceRateAmendmentLine, JobInvoiceRateAmendmentLineViewModel>();
            Mapper.CreateMap<JobInvoiceRateAmendmentLineViewModel, JobInvoiceRateAmendmentLine>();

            Mapper.CreateMap<JobReceiveByProduct, JobReceiveByProductViewModel>();
            Mapper.CreateMap<JobReceiveByProductViewModel, JobReceiveByProduct>();

            Mapper.CreateMap<JobReceiveBom, JobReceiveBomViewModel>();
            Mapper.CreateMap<JobReceiveBomViewModel, JobReceiveBom>();

            Mapper.CreateMap<JobInvoiceSettings, JobInvoiceSettingsViewModel>();
            Mapper.CreateMap<JobInvoiceSettingsViewModel, JobInvoiceSettings>();

            Mapper.CreateMap<CalculationFooterViewModel, HeaderChargeViewModel>();
            Mapper.CreateMap<CalculationProductViewModel, LineChargeViewModel>();

            Mapper.CreateMap<RateConversionSettings, RateConversionSettingsViewModel>();
            Mapper.CreateMap<RateConversionSettingsViewModel, RateConversionSettings>();

            Mapper.CreateMap<StockHeader, StockHeaderViewModel>();
            Mapper.CreateMap<StockHeaderViewModel, StockHeader>();

            Mapper.CreateMap<StockLineViewModel, StockLine>();
            Mapper.CreateMap<StockLine, StockLineViewModel>();

            Mapper.CreateMap<PersonContact, PersonContactViewModel>();
            Mapper.CreateMap<PersonContactViewModel, PersonContact>();

            Mapper.CreateMap<JobInvoiceHeaderViewModel, JobReceiveHeader>();
            Mapper.CreateMap<JobReceiveHeader, JobInvoiceHeaderViewModel>();

            Mapper.CreateMap<JobInvoiceLineViewModel, JobReceiveLine>();
            Mapper.CreateMap<JobReceiveLine, JobInvoiceLineViewModel>();

            Mapper.CreateMap<RateListViewModel, RateList>();
            Mapper.CreateMap<RateList, RateListViewModel>();

            Mapper.CreateMap<RateList, RateListHistory>();
            Mapper.CreateMap<RateListHistory, RateList>();

            Mapper.CreateMap<HeaderChargeViewModel, HeaderChargeViewModel>();
            Mapper.CreateMap<LineChargeViewModel, LineChargeViewModel>();

            Mapper.CreateMap<JobInvoiceReturnHeader, JobInvoiceReturnHeaderViewModel>();
            Mapper.CreateMap<JobInvoiceReturnHeaderViewModel, JobInvoiceReturnHeader>();

            Mapper.CreateMap<JobInvoiceReturnHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<JobInvoiceReturnHeader, DocumentUniqueId>();


            Mapper.CreateMap<JobOrderSettings, JobOrderSettings>();
            Mapper.CreateMap<JobInvoiceSettings, JobInvoiceSettings>();
            Mapper.CreateMap<JobOrderInspectionRequestSettings, JobOrderInspectionRequestSettings>();
            Mapper.CreateMap<JobOrderInspectionSettings, JobOrderInspectionSettings>();
            Mapper.CreateMap<JobReceiveQASettings, JobReceiveQASettings>();
            Mapper.CreateMap<JobReceiveSettings, JobReceiveSettings>();
            Mapper.CreateMap<JobOrderLine, JobOrderLine>();
            Mapper.CreateMap<JobReceiveLine, JobReceiveLine>();
            Mapper.CreateMap<JobOrderHeaderCharge, JobOrderHeaderCharge>();
            Mapper.CreateMap<JobOrderLineCharge, JobOrderLineCharge>();
            Mapper.CreateMap<JobOrderCancelLine, JobOrderCancelLine>();
            Mapper.CreateMap<JobInvoiceLine, JobInvoiceLine>();
            Mapper.CreateMap<JobInvoiceLineCharge, JobInvoiceLineCharge>();
            Mapper.CreateMap<JobInvoiceHeaderCharge, JobInvoiceHeaderCharge>();
            Mapper.CreateMap<JobReturnLine, JobReturnLine>();
            Mapper.CreateMap<PersonRegistration, PersonRegistration>();
            Mapper.CreateMap<LedgerAccount, LedgerAccount>();
            Mapper.CreateMap<PersonAddress, PersonAddress>();
            Mapper.CreateMap<Person, Person>();
            Mapper.CreateMap<BusinessEntity, BusinessEntity>();
            Mapper.CreateMap<JobWorker, JobWorker>();
            Mapper.CreateMap<StockLine, StockLine>();
            Mapper.CreateMap<JobOrderRateAmendmentLine, JobOrderRateAmendmentLine>();
            Mapper.CreateMap<JobInvoiceRateAmendmentLine, JobInvoiceRateAmendmentLine>();
            Mapper.CreateMap<StockHeader, StockHeader>();
            Mapper.CreateMap<JobInvoiceHeader, JobInvoiceHeader>();
            Mapper.CreateMap<JobReceiveHeader, JobReceiveHeader>();
            Mapper.CreateMap<JobInvoiceLineIndexViewModel,JobInvoiceLine>();
            Mapper.CreateMap<JobReceiveLineViewModel, JobReceiveLine>();
            Mapper.CreateMap<JobOrderCancelHeader, JobOrderCancelHeader>();
            Mapper.CreateMap<JobOrderCancelLineViewModel, JobOrderCancelLine>();
            Mapper.CreateMap<JobOrderAmendmentHeader, JobOrderAmendmentHeader>();
            Mapper.CreateMap<JobOrderRateAmendmentLineViewModel, JobOrderRateAmendmentLine>();
            Mapper.CreateMap<JobInvoiceAmendmentHeader, JobInvoiceAmendmentHeader>();
            Mapper.CreateMap<JobInvoiceRateAmendmentLineViewModel, JobInvoiceRateAmendmentLine>();
            Mapper.CreateMap<JobReceiveLineViewModel, JobReceiveLine>();
            Mapper.CreateMap<JobReturnLineIndexViewModel, JobReturnLine>();
            Mapper.CreateMap<JobOrderHeader, JobOrderHeader>();
            Mapper.CreateMap<JobOrderLineViewModel, JobOrderLine>();
            Mapper.CreateMap<JobOrderHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<JobOrderHeader, DocumentUniqueId>();

            Mapper.CreateMap<JobInvoiceHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<JobInvoiceHeader, DocumentUniqueId>();

            Mapper.CreateMap<JobInvoiceAmendmentHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<JobInvoiceAmendmentHeader, DocumentUniqueId>();

            Mapper.CreateMap<JobOrderAmendmentHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<JobOrderAmendmentHeader, DocumentUniqueId>();

            Mapper.CreateMap<JobOrderCancelHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<JobOrderCancelHeader, DocumentUniqueId>();

            Mapper.CreateMap<JobOrderInspectionHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<JobOrderInspectionHeader, DocumentUniqueId>();

            Mapper.CreateMap<JobReceiveQAHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<JobReceiveQAHeader, DocumentUniqueId>();
            Mapper.CreateMap<JobReceiveQAHeader, JobReceiveQAAttributeViewModel>();
            Mapper.CreateMap<JobReceiveQAAttributeViewModel, JobReceiveQAHeader>();

            Mapper.CreateMap<JobReceiveQAPenalty, JobReceiveQAPenaltyViewModel>();
            Mapper.CreateMap<JobReceiveQAPenaltyViewModel, JobReceiveQAPenalty>();


            Mapper.CreateMap<JobOrderInspectionRequestCancelHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<JobOrderInspectionRequestCancelHeader, DocumentUniqueId>();

            Mapper.CreateMap<JobOrderInspectionRequestHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<JobOrderInspectionRequestHeader, DocumentUniqueId>();

            Mapper.CreateMap<JobReceiveHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<JobReceiveHeader, DocumentUniqueId>();

            Mapper.CreateMap<JobReturnHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<JobReturnHeader, DocumentUniqueId>();

            Mapper.CreateMap<JobOrderInspectionRequestHeader, JobOrderInspectionRequestHeaderViewModel>();
            Mapper.CreateMap<JobOrderInspectionRequestHeaderViewModel, JobOrderInspectionRequestHeader>();

            Mapper.CreateMap<JobOrderInspectionRequestLine, JobOrderInspectionRequestLineViewModel>();
            Mapper.CreateMap<JobOrderInspectionRequestLineViewModel, JobOrderInspectionRequestLine>();

            Mapper.CreateMap<JobOrderInspectionRequestSettings, JobOrderInspectionRequestSettingsViewModel>();
            Mapper.CreateMap<JobOrderInspectionRequestSettingsViewModel, JobOrderInspectionRequestSettings>();

            Mapper.CreateMap<JobOrderInspectionSettings, JobOrderInspectionSettingsViewModel>();
            Mapper.CreateMap<JobOrderInspectionSettingsViewModel, JobOrderInspectionSettings>();

            Mapper.CreateMap<JobOrderInspectionRequestHeader, JobOrderInspectionRequestHeader>();
            Mapper.CreateMap<JobOrderInspectionRequestLine, JobOrderInspectionRequestLine>();

            Mapper.CreateMap<JobOrderInspectionRequestCancelHeader, JobOrderInspectionRequestCancelHeaderViewModel>();
            Mapper.CreateMap<JobOrderInspectionRequestCancelHeaderViewModel, JobOrderInspectionRequestCancelHeader>();

            Mapper.CreateMap<JobOrderInspectionRequestCancelLine, JobOrderInspectionRequestCancelLineViewModel>();
            Mapper.CreateMap<JobOrderInspectionRequestCancelLineViewModel, JobOrderInspectionRequestCancelLine>();

            Mapper.CreateMap<JobOrderInspectionRequestCancelHeader, JobOrderInspectionRequestCancelHeader>();
            Mapper.CreateMap<JobOrderInspectionRequestCancelLine, JobOrderInspectionRequestCancelLine>();

            Mapper.CreateMap<JobOrderInspectionHeader, JobOrderInspectionHeaderViewModel>();
            Mapper.CreateMap<JobOrderInspectionHeaderViewModel, JobOrderInspectionHeader>();

            Mapper.CreateMap<JobOrderInspectionLine, JobOrderInspectionLineViewModel>();
            Mapper.CreateMap<JobOrderInspectionLineViewModel, JobOrderInspectionLine>();

            Mapper.CreateMap<JobOrderInspectionHeader, JobOrderInspectionHeader>();
            Mapper.CreateMap<JobOrderInspectionLine, JobOrderInspectionLine>();

            Mapper.CreateMap<JobReceiveQAHeader, JobReceiveQAHeaderViewModel>();
            Mapper.CreateMap<JobReceiveQAHeaderViewModel, JobReceiveQAHeader>();

            Mapper.CreateMap<JobReceiveQALine, JobReceiveQALineViewModel>();
            Mapper.CreateMap<JobReceiveQALineViewModel, JobReceiveQALine>();
            Mapper.CreateMap<JobReceiveQALine, JobReceiveQAAttributeViewModel>();
            Mapper.CreateMap<JobReceiveQAAttributeViewModel, JobReceiveQALine>();

            Mapper.CreateMap<JobReceiveQAHeader, JobReceiveQAHeader>();
            Mapper.CreateMap<JobReceiveQALine, JobReceiveQALine>();

            Mapper.CreateMap<JobReceiveQASettings, JobReceiveQASettingsViewModel>();
            Mapper.CreateMap<JobReceiveQASettingsViewModel, JobReceiveQASettings>();

            Mapper.CreateMap<JobInvoiceReturnHeader, JobInvoiceReturnHeader>();
            Mapper.CreateMap<JobInvoiceReturnLine, JobInvoiceReturnLine>();

            Mapper.CreateMap<JobInvoiceReturnHeaderViewModel, JobReturnHeader>();

            Mapper.CreateMap<JobInvoiceReturnLineViewModel, JobInvoiceReturnLine>();
            Mapper.CreateMap<JobInvoiceReturnLine, JobInvoiceReturnLineViewModel>();

            Mapper.CreateMap<JobInvoiceReturnLineViewModel, JobReturnLine>();
            Mapper.CreateMap<JobReturnLine, JobInvoiceReturnLineViewModel>();

            Mapper.CreateMap<JobInvoiceReturnLine, JobReturnLine>();
            
        }
    }
}
