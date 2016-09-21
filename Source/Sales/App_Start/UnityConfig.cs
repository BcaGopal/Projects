using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Sales.Controllers;
using Data.Models;
using Data.Infrastructure;
using Service;
using Model.Models;
using AutoMapper;
using Model.ViewModel;
using Model.ViewModels;

namespace Sales.App_Start
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

            container.RegisterType<IDataContext, ApplicationDbContext>(new PerRequestLifetimeManager());
            container.RegisterType<IUnitOfWorkForService, UnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType<IActivityLogService, ActivityLogService>(new PerRequestLifetimeManager());

            container.RegisterType<IDuplicateDocumentCheckService, DuplicateDocumentCheckService>(new PerRequestLifetimeManager());
          
            container.RegisterType<IExceptionHandlingService, ExceptionHandlingService>(new PerRequestLifetimeManager());
            container.RegisterType<IModuleService, ModuleService>(new PerRequestLifetimeManager());
            container.RegisterType<ISubModuleService, SubModuleService>(new PerRequestLifetimeManager());
            container.RegisterType<IRepository<SaleOrderQtyAmendmentLine>, Repository<SaleOrderQtyAmendmentLine>>();
            container.RegisterType<ISaleOrderQtyAmendmentLineService, SaleOrderQtyAmendmentLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleOrderSettings>, Repository<SaleOrderSettings>>();
            container.RegisterType<ISaleOrderSettingsService, SaleOrderSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleOrderAmendmentHeader>, Repository<SaleOrderAmendmentHeader>>();
            container.RegisterType<ISaleOrderAmendmentHeaderService, SaleOrderAmendmentHeaderService>(new PerRequestLifetimeManager());


            container.RegisterType<IRepository<SaleOrderCancelHeader>, Repository<SaleOrderCancelHeader>>();
            container.RegisterType<ISaleOrderCancelHeaderService, SaleOrderCancelHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleOrderCancelLine>, Repository<SaleOrderCancelLine>>();
            container.RegisterType<ISaleOrderCancelLineService, SaleOrderCancelLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleOrderHeader>, Repository<SaleOrderHeader>>();
            container.RegisterType<Service.ISaleOrderHeaderService, Service.SaleOrderHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleOrderLine>, Repository<SaleOrderLine>>();
            container.RegisterType<Service.ISaleOrderLineService, Service.SaleOrderLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<DispatchWaybillHeader>, Repository<DispatchWaybillHeader>>();
            container.RegisterType<Service.IDispatchWaybillHeaderService, Service.DispatchWaybillHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ChargeGroupPerson>, Repository<ChargeGroupPerson>>();
            container.RegisterType<IChargeGroupPersonService, ChargeGroupPersonService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<DispatchWaybillLine>, Repository<DispatchWaybillLine>>();
            container.RegisterType<Service.IDispatchWaybillLineService, Service.DispatchWaybillLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductBuyer>, Repository<ProductBuyer>>();
            container.RegisterType<IProductBuyerService, ProductBuyerService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductUid>, Repository<ProductUid>>();
            container.RegisterType<IProductUidService, ProductUidService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Person>, Repository<Person>>();
            container.RegisterType<IPersonService, PersonService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Buyer>, Repository<Buyer>>();
            container.RegisterType<IBuyerService, BuyerService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Currency>, Repository<Currency>>();
            container.RegisterType<ICurrencyService, CurrencyService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ShipMethod>, Repository<ShipMethod>>();
            container.RegisterType<IShipMethodService, ShipMethodService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<DeliveryTerms>, Repository<DeliveryTerms>>();
            container.RegisterType<IDeliveryTermsService, DeliveryTermsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Stock>, Repository<Stock>>();
            container.RegisterType<IStockService, StockService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<BusinessEntity>, Repository<BusinessEntity>>();
            container.RegisterType<IBusinessEntityService, BusinessEntityService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<LedgerAccount>, Repository<LedgerAccount>>();
            container.RegisterType<IAccountService, AccountService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonAddress>, Repository<PersonAddress>>();
            container.RegisterType<IPersonAddressService, PersonAddressService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonProcess>, Repository<PersonProcess>>();
            container.RegisterType<IPersonProcessService, PersonProcessService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonRegistration>, Repository<PersonRegistration>>();
            container.RegisterType<IPersonRegistrationService, PersonRegistrationService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonContact>, Repository<PersonContact>>();
            container.RegisterType<IPersonContactService, PersonContactService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonBankAccount>, Repository<PersonBankAccount>>();
            container.RegisterType<IPersonBankAccountService, PersonBankAccountService>(new PerRequestLifetimeManager());

            container.RegisterType<IReportLineService, ReportLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IProductService, ProductService>(new PerRequestLifetimeManager());

            container.RegisterType<IPromoCodeService, PromoCodeService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleDeliveryOrderHeaderService, SaleDeliveryOrderHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleDeliveryOrderLineService, SaleDeliveryOrderLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IUpdateSaleExpiryService, UpdateSaleExpiryService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleDeliveryOrderSettingsService, SaleDeliveryOrderSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleDeliveryOrderCancelHeaderService, SaleDeliveryOrderCancelHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleDeliveryOrderCancelLineService, SaleDeliveryOrderCancelLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleDispatchHeader>, Repository<SaleDispatchHeader>>();
            container.RegisterType<Service.ISaleDispatchHeaderService, Service.SaleDispatchHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleDispatchLine>, Repository<SaleDispatchLine>>();
            container.RegisterType<Service.ISaleDispatchLineService, Service.SaleDispatchLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleInvoiceHeader>, Repository<SaleInvoiceHeader>>();
            container.RegisterType<Service.ISaleInvoiceHeaderService, Service.SaleInvoiceHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleInvoiceLine>, Repository<SaleInvoiceLine>>();
            container.RegisterType<Service.ISaleInvoiceLineService, Service.SaleInvoiceLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PackingHeader>, Repository<PackingHeader>>();
            container.RegisterType<Service.IPackingHeaderService, Service.PackingHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PackingLine>, Repository<PackingLine>>();
            container.RegisterType<Service.IPackingLineService, Service.PackingLineService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleInvoiceSettingService, SaleInvoiceSettingService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleInvoiceReturnHeaderService, SaleInvoiceReturnHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleInvoiceReturnLineService, SaleInvoiceReturnLineService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleDispatchReturnHeaderService, SaleDispatchReturnHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleDispatchReturnLineService, SaleDispatchReturnLineService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleDispatchSettingService, SaleDispatchSettingService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Manufacturer>, Repository<Manufacturer>>();
            container.RegisterType<IManufacturerService, ManufacturerService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonDocument>, Repository<PersonDocument>>();
            container.RegisterType<IPersonDocumentService, PersonDocumentService>(new PerRequestLifetimeManager());

            container.RegisterType<IAgentService, AgentService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<DrawBackTariffHead>, Repository<DrawBackTariffHead>>();
            container.RegisterType<IDrawBackTariffHeadService, DrawBackTariffHeadService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Route>, Repository<Route>>();
            container.RegisterType<IRouteService, RouteService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<RouteLine>, Repository<RouteLine>>();
            container.RegisterType<IRouteLineService, RouteLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Courier>, Repository<Courier>>();
            container.RegisterType<ICourierService, CourierService>(new PerRequestLifetimeManager());

            //Registering Mappers:
            Mapper.CreateMap<ProductBuyer, ProductBuyerViewModel>();
            Mapper.CreateMap<ProductBuyerViewModel, ProductBuyer>();

            Mapper.CreateMap<BuyerViewModel, Person>();
            Mapper.CreateMap<Person, BuyerViewModel>();

            Mapper.CreateMap<BuyerViewModel, BusinessEntity>();
            Mapper.CreateMap<BusinessEntity, BuyerViewModel>();

            Mapper.CreateMap<BuyerViewModel, Buyer>();
            Mapper.CreateMap<Buyer, BuyerViewModel>();

            Mapper.CreateMap<BuyerViewModel, PersonAddress>();
            Mapper.CreateMap<PersonAddress, BuyerViewModel>();

            Mapper.CreateMap<BuyerViewModel, LedgerAccount>();
            Mapper.CreateMap<LedgerAccount, BuyerViewModel>();

            Mapper.CreateMap<SaleOrderLine, SaleOrderLineViewModel>();
            Mapper.CreateMap<SaleOrderLineViewModel, SaleOrderLine>();

            Mapper.CreateMap<SaleOrderHeader, SaleOrderHeaderIndexViewModel>();
            Mapper.CreateMap<SaleOrderHeaderIndexViewModel, SaleOrderHeader>();

            Mapper.CreateMap<SaleOrderHeaderIndexViewModelForEdit, SaleOrderHeaderIndexViewModel>();
            Mapper.CreateMap<SaleOrderHeaderIndexViewModel, SaleOrderHeaderIndexViewModelForEdit>();


            Mapper.CreateMap<SaleOrderCancelHeaderIndexViewModel, SaleOrderCancelHeader>();
            Mapper.CreateMap<SaleOrderCancelHeader, SaleOrderCancelHeaderIndexViewModel>();

            Mapper.CreateMap<SaleOrderAmendmentHeader, SaleOrderAmendmentHeaderViewModel>();
            Mapper.CreateMap<SaleOrderAmendmentHeaderViewModel, SaleOrderAmendmentHeader>();

            Mapper.CreateMap<DispatchWaybillHeader, DispatchWaybillHeaderViewModel>();
            Mapper.CreateMap<DispatchWaybillHeaderViewModel, DispatchWaybillHeader>();

            Mapper.CreateMap<DispatchWaybillHeaderViewModelWithLog, DispatchWaybillHeaderViewModel>();
            Mapper.CreateMap<DispatchWaybillHeaderViewModel, DispatchWaybillHeaderViewModelWithLog>();

            Mapper.CreateMap<DispatchWaybillLine, DispatchWaybillLineViewModel>();
            Mapper.CreateMap<DispatchWaybillLineViewModel, DispatchWaybillLine>();


            Mapper.CreateMap<PersonContact, PersonContactViewModel>();
            Mapper.CreateMap<PersonContactViewModel, PersonContact>();

            Mapper.CreateMap<SaleDeliveryOrderHeaderViewModel, SaleDeliveryOrderHeader>();
            Mapper.CreateMap<SaleDeliveryOrderHeader, SaleDeliveryOrderHeaderViewModel>();

            Mapper.CreateMap<SaleDeliveryOrderLine, SaleDeliveryOrderLineViewModel>();
            Mapper.CreateMap<SaleDeliveryOrderLineViewModel, SaleDeliveryOrderLine>();


            Mapper.CreateMap<HeaderChargeViewModel, HeaderChargeViewModel>();
            Mapper.CreateMap<LineChargeViewModel, LineChargeViewModel>();

            Mapper.CreateMap<DirectSaleInvoiceHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<SaleInvoiceHeader, DocumentUniqueId>();
            Mapper.CreateMap<SaleInvoiceHeaderIndexViewModel, DocumentUniqueId>();
            Mapper.CreateMap<SaleInvoiceHeaderDetail, DocumentUniqueId>();

            Mapper.CreateMap<SaleDispatchHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<SaleDispatchHeader, DocumentUniqueId>();
            Mapper.CreateMap<SaleDispatchHeaderIndexViewModel, DocumentUniqueId>();

            Mapper.CreateMap<DispatchWaybillHeader, DocumentUniqueId>();
            Mapper.CreateMap<DispatchWaybillHeaderViewModel, DocumentUniqueId>();

            Mapper.CreateMap<SaleDeliveryOrderCancelHeader, DocumentUniqueId>();
            Mapper.CreateMap<SaleDeliveryOrderCancelHeaderViewModel, DocumentUniqueId>();

            Mapper.CreateMap<SaleDeliveryOrderHeader, DocumentUniqueId>();
            Mapper.CreateMap<SaleDeliveryOrderHeaderViewModel, DocumentUniqueId>();

            Mapper.CreateMap<SaleOrderAmendmentHeader, DocumentUniqueId>();
            Mapper.CreateMap<SaleOrderAmendmentHeaderViewModel, DocumentUniqueId>();

            Mapper.CreateMap<SaleOrderCancelHeader, DocumentUniqueId>();
            Mapper.CreateMap<SaleOrderCancelHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<SaleOrderCancelHeaderIndexViewModel, DocumentUniqueId>();

            Mapper.CreateMap<SaleOrderHeader, DocumentUniqueId>();
            Mapper.CreateMap<SaleOrderHeaderIndexViewModel, DocumentUniqueId>();

            Mapper.CreateMap<ProductBuyer, ProductBuyer>();
            Mapper.CreateMap<SaleOrderAmendmentHeader, SaleOrderAmendmentHeader>();
            Mapper.CreateMap<SaleOrderQtyAmendmentLineViewModel, SaleOrderQtyAmendmentLine>();
            Mapper.CreateMap<SaleDeliveryOrderHeader, SaleDeliveryOrderHeader>();
            Mapper.CreateMap<SaleDeliveryOrderLineViewModel, SaleDeliveryOrderLine>();
            Mapper.CreateMap<SaleDeliveryOrderLine, SaleDeliveryOrderLine>();
            Mapper.CreateMap<SaleOrderCancelHeader, SaleOrderCancelHeader>();
            Mapper.CreateMap<SaleOrderQtyAmendmentLine, SaleOrderQtyAmendmentLine>();
            Mapper.CreateMap<SaleOrderHeader, SaleOrderHeader>();
            Mapper.CreateMap<SaleOrderLineIndexViewModel, SaleOrderLine>();
            Mapper.CreateMap<SaleOrderLine, SaleOrderLine>();
            Mapper.CreateMap<SaleOrderCancelLine, SaleOrderCancelLine>();
            Mapper.CreateMap<SaleOrderCancelLineViewModel, SaleOrderCancelLine>();
            Mapper.CreateMap<SaleDeliveryOrderSettings, SaleDeliveryOrderSettingsViewModel>();
            Mapper.CreateMap<SaleDeliveryOrderSettingsViewModel, SaleDeliveryOrderSettings>();
            Mapper.CreateMap<SaleDeliveryOrderCancelHeader, SaleDeliveryOrderCancelHeader>();
            Mapper.CreateMap<SaleDeliveryOrderCancelLine, SaleDeliveryOrderCancelLine>();
            Mapper.CreateMap<DispatchWaybillLine, DispatchWaybillLine>();
            Mapper.CreateMap<SaleDeliveryOrderSettings, SaleDeliveryOrderSettings>();

            Mapper.CreateMap<SaleDeliveryOrderCancelHeader, SaleDeliveryOrderCancelHeaderViewModel>();
            Mapper.CreateMap<SaleDeliveryOrderCancelHeaderViewModel, SaleDeliveryOrderCancelHeader>();

            Mapper.CreateMap<SaleDeliveryOrderCancelLine, SaleDeliveryOrderCancelLineViewModel>();
            Mapper.CreateMap<SaleDeliveryOrderCancelLineViewModel, SaleDeliveryOrderCancelLine>();

            Mapper.CreateMap<LineChargeViewModel, SaleInvoiceReturnLineCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<SaleInvoiceReturnLineCharge, LineChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<HeaderChargeViewModel, SaleInvoiceReturnHeaderCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<SaleInvoiceReturnHeaderCharge, HeaderChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<CalculationFooterViewModel, HeaderChargeViewModel>();
            Mapper.CreateMap<CalculationProductViewModel, LineChargeViewModel>();

            Mapper.CreateMap<SaleInvoiceReturnHeaderViewModel, SaleInvoiceReturnHeader>();
            Mapper.CreateMap<SaleInvoiceReturnHeader, SaleInvoiceReturnHeaderViewModel>();

            Mapper.CreateMap<SaleInvoiceReturnHeaderViewModel, SaleDispatchReturnHeader>();
            Mapper.CreateMap<SaleDispatchReturnHeader, SaleInvoiceReturnHeaderViewModel>();

            Mapper.CreateMap<SaleDispatchReturnLine, SaleInvoiceReturnLine>();
            Mapper.CreateMap<SaleInvoiceReturnLine, SaleDispatchReturnLine>();

            Mapper.CreateMap<SaleInvoiceReturnLine, SaleInvoiceReturnLineViewModel>();
            Mapper.CreateMap<SaleInvoiceReturnLineViewModel, SaleInvoiceReturnLine>();

            Mapper.CreateMap<SaleInvoiceReturnHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<SaleInvoiceReturnHeader, DocumentUniqueId>();

            Mapper.CreateMap<SaleDispatchReturnHeader, DocumentUniqueId>();
            Mapper.CreateMap<SaleDispatchReturnHeaderViewModel, DocumentUniqueId>();

            Mapper.CreateMap<SaleInvoiceReturnHeader, SaleInvoiceReturnHeader>();
            Mapper.CreateMap<SaleDispatchReturnHeader, SaleDispatchReturnHeader>();
            Mapper.CreateMap<SaleInvoiceReturnLineIndexViewModel, SaleInvoiceReturnLine>();
            Mapper.CreateMap<SaleDispatchReturnLineIndexViewModel, SaleDispatchReturnLine>();
            Mapper.CreateMap<SaleInvoiceReturnLine, SaleInvoiceReturnLine>();
            Mapper.CreateMap<SaleDispatchReturnLine, SaleDispatchReturnLine>();
            Mapper.CreateMap<SaleInvoiceReturnLineCharge, SaleInvoiceReturnLineCharge>();
            Mapper.CreateMap<SaleInvoiceReturnHeaderCharge, SaleInvoiceReturnHeaderCharge>();





            Mapper.CreateMap<SaleOrderSettings, SaleOrderSettingsViewModel>();
            Mapper.CreateMap<SaleOrderSettingsViewModel, SaleOrderSettings>();

            Mapper.CreateMap<DirectSaleInvoiceHeaderViewModel, SaleInvoiceHeader>();
            Mapper.CreateMap<SaleInvoiceHeader, DirectSaleInvoiceHeaderViewModel>();

            Mapper.CreateMap<DirectSaleInvoiceHeaderViewModel, PackingHeader>();
            Mapper.CreateMap<PackingHeader, DirectSaleInvoiceHeaderViewModel>();

            Mapper.CreateMap<DirectSaleInvoiceHeaderViewModel, SaleDispatchHeader>();
            Mapper.CreateMap<SaleDispatchHeader, DirectSaleInvoiceHeaderViewModel>();



            Mapper.CreateMap<SaleDispatchHeaderViewModel, PackingHeader>();
            Mapper.CreateMap<PackingHeader, SaleDispatchHeaderViewModel>();

            Mapper.CreateMap<SaleDispatchHeaderViewModel, SaleDispatchHeader>();
            Mapper.CreateMap<SaleDispatchHeader, SaleDispatchHeaderViewModel>();


            Mapper.CreateMap<DirectSaleInvoiceLineViewModel, SaleInvoiceLine>();
            Mapper.CreateMap<SaleInvoiceLine, DirectSaleInvoiceLineViewModel>();

            Mapper.CreateMap<DirectSaleInvoiceLineViewModel, PackingLine>();
            Mapper.CreateMap<PackingLine, DirectSaleInvoiceLineViewModel>();

            Mapper.CreateMap<DirectSaleInvoiceLineViewModel, SaleDispatchLine>();
            Mapper.CreateMap<SaleDispatchLine, DirectSaleInvoiceLineViewModel>();



            Mapper.CreateMap<SaleDispatchLineViewModel, PackingLine>();
            Mapper.CreateMap<PackingLine, SaleDispatchLineViewModel>();

            Mapper.CreateMap<SaleDispatchLineViewModel, SaleDispatchLine>();
            Mapper.CreateMap<SaleDispatchLine, SaleDispatchLineViewModel>();


            Mapper.CreateMap<SaleInvoiceHeader, SaleInvoiceHeaderIndexViewModel>();
            Mapper.CreateMap<SaleInvoiceHeaderIndexViewModel, SaleInvoiceHeader>();

            Mapper.CreateMap<SaleInvoiceHeaderDetail, SaleInvoiceHeaderIndexViewModel>();
            Mapper.CreateMap<SaleInvoiceHeaderIndexViewModel, SaleInvoiceHeaderDetail>();

            Mapper.CreateMap<SaleInvoiceHeaderIndexViewModelForEdit, SaleInvoiceHeaderIndexViewModel>();
            Mapper.CreateMap<SaleInvoiceHeaderIndexViewModel, SaleInvoiceHeaderIndexViewModelForEdit>();

            Mapper.CreateMap<SaleInvoiceLine, SaleInvoiceLineViewModel>();
            Mapper.CreateMap<SaleInvoiceLineViewModel, SaleInvoiceLine>();


            Mapper.CreateMap<SaleDispatchHeader, SaleInvoiceHeaderIndexViewModel>();
            Mapper.CreateMap<SaleInvoiceHeaderIndexViewModel, SaleDispatchHeader>();

            Mapper.CreateMap<SaleDispatchHeader, SaleInvoiceHeaderIndexViewModel>();
            Mapper.CreateMap<SaleInvoiceHeaderIndexViewModel, SaleDispatchHeader>();

            Mapper.CreateMap<SaleInvoiceSetting, SaleInvoiceSettingsViewModel>();
            Mapper.CreateMap<SaleInvoiceSettingsViewModel, SaleInvoiceSetting>();

            Mapper.CreateMap<SaleDispatchSetting, SaleDispatchSettingsViewModel>();
            Mapper.CreateMap<SaleDispatchSettingsViewModel, SaleDispatchSetting>();

            Mapper.CreateMap<SaleDispatchReturnHeader, SaleDispatchReturnHeaderViewModel>();
            Mapper.CreateMap<SaleDispatchReturnHeaderViewModel, SaleDispatchReturnHeader>();

            Mapper.CreateMap<SaleDispatchReturnHeader, SaleDispatchReturnHeader>();

            Mapper.CreateMap<SaleDispatchReturnLine, SaleDispatchReturnLineViewModel>();
            Mapper.CreateMap<SaleDispatchReturnLineViewModel, SaleDispatchReturnLine>();

            Mapper.CreateMap<SaleDispatchReturnLine, SaleDispatchReturnLine>();


            Mapper.CreateMap<Manufacturer, ManufacturerViewModel>();
            Mapper.CreateMap<ManufacturerViewModel, Manufacturer>();

            Mapper.CreateMap<Person, ManufacturerViewModel>();
            Mapper.CreateMap<ManufacturerViewModel, Person>();

            Mapper.CreateMap<PersonAddress, ManufacturerViewModel>();
            Mapper.CreateMap<ManufacturerViewModel, PersonAddress>();

            Mapper.CreateMap<LedgerAccount, ManufacturerViewModel>();
            Mapper.CreateMap<ManufacturerViewModel, LedgerAccount>();

            Mapper.CreateMap<BusinessEntity, ManufacturerViewModel>();
            Mapper.CreateMap<ManufacturerViewModel, BusinessEntity>();

            Mapper.CreateMap<PersonDocument, PersonDocument>();

            Mapper.CreateMap<Person, AgentViewModel>();
            Mapper.CreateMap<AgentViewModel, Person>();

            Mapper.CreateMap<BusinessEntity, AgentViewModel>();
            Mapper.CreateMap<AgentViewModel, BusinessEntity>();

            Mapper.CreateMap<Agent, AgentViewModel>();
            Mapper.CreateMap<AgentViewModel, Agent>();

            Mapper.CreateMap<PersonAddress, AgentViewModel>();
            Mapper.CreateMap<AgentViewModel, PersonAddress>();

            Mapper.CreateMap<LedgerAccount, AgentViewModel>();
            Mapper.CreateMap<AgentViewModel, LedgerAccount>();

            Mapper.CreateMap<DrawBackTariffHead, DrawBackTariffHead>();

            Mapper.CreateMap<RouteLine, RouteLineViewModel>();
            Mapper.CreateMap<RouteLineViewModel, RouteLine>();

            Mapper.CreateMap<Route, Route>();
            Mapper.CreateMap<RouteLine, RouteLine>();

            Mapper.CreateMap<Courier, CourierViewModel>();
            Mapper.CreateMap<CourierViewModel, Courier>();

            Mapper.CreateMap<Person, CourierViewModel>();
            Mapper.CreateMap<CourierViewModel, Person>();

            Mapper.CreateMap<PersonAddress, CourierViewModel>();
            Mapper.CreateMap<CourierViewModel, PersonAddress>();

            Mapper.CreateMap<LedgerAccount, CourierViewModel>();
            Mapper.CreateMap<CourierViewModel, LedgerAccount>();

            Mapper.CreateMap<BusinessEntity, CourierViewModel>();
            Mapper.CreateMap<CourierViewModel, BusinessEntity>();

        }
    }
}
