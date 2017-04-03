using System;
using Microsoft.Practices.Unity;
using Customise.Controllers;
using Data.Models;
using Data.Infrastructure;
using Service;
using Model.Models;
using AutoMapper;
using Model.ViewModel;
using Model.ViewModels;
using Models.Login.Models;

namespace Customise.App_Start
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
            container.RegisterType<IActivityLogService, ActivityLogService>(new PerRequestLifetimeManager());

            container.RegisterType<IDataContext, ApplicationDbContext>(new PerRequestLifetimeManager());
            container.RegisterType<IUnitOfWorkForService, UnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager());

            container.RegisterType<IExceptionHandlingService, ExceptionHandlingService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<LedgerAccount>, Repository<LedgerAccount>>();
            container.RegisterType<IAccountService, AccountService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<BomDetail>, Repository<BomDetail>>();
            container.RegisterType<IBomDetailService, BomDetailService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<BusinessEntity>, Repository<BusinessEntity>>();
            container.RegisterType<IBusinessEntityService, BusinessEntityService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Buyer>, Repository<Buyer>>();
            container.RegisterType<IBuyerService, BuyerService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Colour>, Repository<Colour>>();
            container.RegisterType<IColourService, ColourService>(new PerRequestLifetimeManager());

            Mapper.CreateMap<JobReceiveBom, JobReceiveBomViewModel>();
            Mapper.CreateMap<JobReceiveBomViewModel, JobReceiveBom>();

            container.RegisterType<IRepository<Currency>, Repository<Currency>>();
            container.RegisterType<ICurrencyService, CurrencyService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<CustomDetail>, Repository<CustomDetail>>();
            container.RegisterType<ICustomDetailService, CustomDetailService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<DeliveryTerms>, Repository<DeliveryTerms>>();
            container.RegisterType<IDeliveryTermsService, DeliveryTermsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<DescriptionOfGoods>, Repository<DescriptionOfGoods>>();
            container.RegisterType<IDescriptionOfGoodsService, DescriptionOfGoodsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Dimension2>, Repository<Dimension2>>();
            container.RegisterType<IDimension2Service, Dimension2Service>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Notification>, Repository<Notification>>();
            container.RegisterType<INotificationService, NotificationService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<NotificationSubject>, Repository<NotificationSubject>>();
            container.RegisterType<INotificationSubjectService, NotificationSubjectService>(new PerRequestLifetimeManager());           

            container.RegisterType<IDuplicateDocumentCheckService, DuplicateDocumentCheckService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<FinishedProduct>, Repository<FinishedProduct>>();
            container.RegisterType<IFinishedProductService, FinishedProductService>(new PerRequestLifetimeManager());          

            container.RegisterType<IRepository<PackingHeader>, Repository<PackingHeader>>();
            container.RegisterType<Service.IPackingHeaderService, Service.PackingHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PackingLine>, Repository<PackingLine>>();
            container.RegisterType<Service.IPackingLineService, Service.PackingLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonAddress>, Repository<PersonAddress>>();
            container.RegisterType<IPersonAddressService, PersonAddressService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonProcess>, Repository<PersonProcess>>();
            container.RegisterType<IPersonProcessService, PersonProcessService>(new PerRequestLifetimeManager());



            container.RegisterType<IRepository<PersonRegistration>, Repository<PersonRegistration>>();
            container.RegisterType<IPersonRegistrationService, PersonRegistrationService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Person>, Repository<Person>>();
            container.RegisterType<IPersonService, PersonService>(new PerRequestLifetimeManager()); //

            container.RegisterType<IRepository<ProductCategory>, Repository<ProductCategory>>();
            container.RegisterType<IProductCategoryService, ProductCategoryService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductCategorySettings>, Repository<ProductCategorySettings>>();
            container.RegisterType<IProductCategorySettingsService, ProductCategorySettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductCategoryProcessSettings>, Repository<ProductCategoryProcessSettings>>();
            container.RegisterType<IProductCategoryProcessSettingsService, ProductCategoryProcessSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductCollection>, Repository<ProductCollection>>();
            container.RegisterType<IProductCollectionService, ProductCollectionService>(new PerRequestLifetimeManager());

            container.RegisterType<IProductConstructionService, ProductConstructionService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductContentHeader>, Repository<ProductContentHeader>>();
            container.RegisterType<IProductContentHeaderService, ProductContentHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductContentLine>, Repository<ProductContentLine>>();
            container.RegisterType<IProductContentLineService, ProductContentLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductDesignPattern>, Repository<ProductDesignPattern>>();
            container.RegisterType<IProductDesignPatternService, ProductDesignPatternService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductDesign>, Repository<ProductDesign>>();
            container.RegisterType<IProductDesignService, ProductDesignService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductGroup>, Repository<ProductGroup>>();
            container.RegisterType<IProductGroupService, ProductGroupService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductInvoiceGroup>, Repository<ProductInvoiceGroup>>();
            container.RegisterType<IProductInvoiceGroupService, ProductInvoiceGroupService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductStyle>, Repository<ProductStyle>>();
            container.RegisterType<IProductManufacturingStyleService, ProductManufacturingStyleService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductNature>, Repository<ProductNature>>();
            container.RegisterType<IProductNatureService, ProductNatureService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductQuality>, Repository<ProductQuality>>();
            container.RegisterType<IProductQualityService, ProductQualityService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Product>, Repository<Product>>();
            container.RegisterType<IProductService, ProductService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductShape>, Repository<ProductShape>>();
            container.RegisterType<IProductShapeService, ProductShapeService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductSize>, Repository<ProductSize>>();
            container.RegisterType<IProductSizeService, ProductSizeService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductSizeType>, Repository<ProductSizeType>>();
            container.RegisterType<IProductSizeTypeService, ProductSizeTypeService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductStyle>, Repository<ProductStyle>>();
            container.RegisterType<IProductStyleService, ProductStyleService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductUid>, Repository<ProductUid>>();
            container.RegisterType<IProductUidService, ProductUidService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ReportHeader>, Repository<ReportHeader>>();
            container.RegisterType<IReportHeaderService, ReportHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ReportLine>, Repository<ReportLine>>();
            container.RegisterType<IReportLineService, ReportLineService>(new PerRequestLifetimeManager());           

            container.RegisterType<IRepository<RugStencil>, Repository<RugStencil>>();
            container.RegisterType<IRugStencilService, RugStencilService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleDispatchHeader>, Repository<SaleDispatchHeader>>();
            container.RegisterType<Service.ISaleDispatchHeaderService, Service.SaleDispatchHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleDispatchLine>, Repository<SaleDispatchLine>>();
            container.RegisterType<Service.ISaleDispatchLineService, Service.SaleDispatchLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleInvoiceHeader>, Repository<SaleInvoiceHeader>>();
            container.RegisterType<Service.ISaleInvoiceHeaderService, Service.SaleInvoiceHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleInvoiceLine>, Repository<SaleInvoiceLine>>();
            container.RegisterType<Service.ISaleInvoiceLineService, Service.SaleInvoiceLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleOrderHeader>, Repository<SaleOrderHeader>>();
            container.RegisterType<Service.ISaleOrderHeaderService, Service.SaleOrderHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleOrderLine>, Repository<SaleOrderLine>>();
            container.RegisterType<Service.ISaleOrderLineService, Service.SaleOrderLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleEnquiryHeader>, Repository<SaleEnquiryHeader>>();
            container.RegisterType<Service.ISaleEnquiryHeaderService, Service.SaleEnquiryHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SaleEnquiryLine>, Repository<SaleEnquiryLine>>();
            container.RegisterType<Service.ISaleEnquiryLineService, Service.SaleEnquiryLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<FeetConversionToCms>, Repository<FeetConversionToCms>>();
            container.RegisterType<IFeetConversionToCmsService, FeetConversionToCmsService>(new PerRequestLifetimeManager());           

            container.RegisterType<IRepository<SalesTaxGroupProduct>, Repository<SalesTaxGroupProduct>>();
            container.RegisterType<ISalesTaxGroupProductService, SalesTaxGroupProductService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SalesTaxGroupParty>, Repository<SalesTaxGroupParty>>();
            container.RegisterType<ISalesTaxGroupPartyService, SalesTaxGroupPartyService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ServiceTaxCategory>, Repository<ServiceTaxCategory>>();
            container.RegisterType<IServiceTaxCategoryService, ServiceTaxCategoryService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ShipMethod>, Repository<ShipMethod>>();
            container.RegisterType<IShipMethodService, ShipMethodService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Size>, Repository<Size>>();
            container.RegisterType<ISizeService, SizeService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Stock>, Repository<Stock>>();
            container.RegisterType<IStockService, StockService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductBuyer>, Repository<ProductBuyer>>();
            container.RegisterType<IProductBuyerService, ProductBuyerService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<UnitConversion>, Repository<UnitConversion>>();
            container.RegisterType<IUnitConversionService, UnitConversionService>(new PerRequestLifetimeManager());

            container.RegisterType<IReportLineService, ReportLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IJobOrderHeaderService, JobOrderHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<ITrialBalanceService, TrialBalanceService>(new PerRequestLifetimeManager());

            container.RegisterType<ICarpetSkuSettingsService, CarpetSkuSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IProductBuyerSettingsService, ProductBuyerSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleEnquirySettingsService, SaleEnquirySettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<ITrialBalanceSettingService, TrialBalanceSettingService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleInvoiceSettingService, SaleInvoiceSettingService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleInvoiceHeaderChargeService, SaleInvoiceHeaderChargeService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleInvoiceLineChargeService, SaleInvoiceLineChargeService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobInvoiceHeader>, Repository<JobInvoiceHeader>>();
            container.RegisterType<IJobInvoiceHeaderService, JobInvoiceHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobInvoiceLine>, Repository<JobInvoiceLine>>();
            container.RegisterType<IJobInvoiceLineService, JobInvoiceLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobReceiveLine>, Repository<JobReceiveLine>>();
            container.RegisterType<IJobReceiveLineService, JobReceiveLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobReceiveSettings>, Repository<JobReceiveSettings>>();
            container.RegisterType<IJobReceiveSettingsService, JobReceiveSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobReceiveQASettings>, Repository<JobReceiveQASettings>>();
            container.RegisterType<IJobReceiveQASettingsService, JobReceiveQASettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<JobReceiveQAPenalty>, Repository<JobReceiveQAPenalty>>();
            container.RegisterType<IJobReceiveQAPenaltyService, JobReceiveQAPenaltyService>(new PerRequestLifetimeManager());



            container.RegisterType<IRepository<JobReceiveHeader>, Repository<JobReceiveHeader>>();
            container.RegisterType<IJobReceiveHeaderService, JobReceiveHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleInvoiceReturnHeaderService, SaleInvoiceReturnHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleInvoiceReturnLineService, SaleInvoiceReturnLineService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleDispatchReturnHeaderService, SaleDispatchReturnHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<ISaleDispatchReturnLineService,SaleDispatchReturnLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IProcessSequenceLineService, ProcessSequenceLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IDocumentTypeTimeExtensionService, DocumentTypeTimeExtensionService>(new PerRequestLifetimeManager());

            container.RegisterType<IJobOrderLineExtendedService, JobOrderLineExtendedService>(new PerRequestLifetimeManager());


            //Registering Mappers:

            Mapper.CreateMap<UnitConversion, UnitConversionViewModel>();
            Mapper.CreateMap<UnitConversionViewModel, UnitConversion>();

            Mapper.CreateMap<ProductBuyer, ProductBuyerViewModel>();
            Mapper.CreateMap<ProductBuyerViewModel, ProductBuyer>();

            Mapper.CreateMap<ProcessSequenceLine, ProcessSequenceLineViewModel>();
            Mapper.CreateMap<ProcessSequenceLineViewModel, ProcessSequenceLine>();

            Mapper.CreateMap<ProcessSequenceLine, ProcessSequenceLine>();

            Mapper.CreateMap<Stock, StockViewModel>();
            Mapper.CreateMap<StockViewModel, Stock>();

            Mapper.CreateMap<SaleOrderLine, SaleOrderLineViewModel>();
            Mapper.CreateMap<SaleOrderLineViewModel, SaleOrderLine>();

            Mapper.CreateMap<SaleOrderHeader, SaleOrderHeaderIndexViewModel>();
            Mapper.CreateMap<SaleOrderHeaderIndexViewModel, SaleOrderHeader>();

            Mapper.CreateMap<SaleOrderHeaderIndexViewModelForEdit, SaleOrderHeaderIndexViewModel>();
            Mapper.CreateMap<SaleOrderHeaderIndexViewModel, SaleOrderHeaderIndexViewModelForEdit>();


            Mapper.CreateMap<SaleEnquiryLine, SaleEnquiryLineViewModel>();
            Mapper.CreateMap<SaleEnquiryLineViewModel, SaleEnquiryLine>();

            Mapper.CreateMap<SaleEnquiryHeader, SaleEnquiryHeaderIndexViewModel>();
            Mapper.CreateMap<SaleEnquiryHeaderIndexViewModel, SaleEnquiryHeader>();

            Mapper.CreateMap<SaleEnquiryHeaderIndexViewModelForEdit, SaleEnquiryHeaderIndexViewModel>();
            Mapper.CreateMap<SaleEnquiryHeaderIndexViewModel, SaleEnquiryHeaderIndexViewModelForEdit>();

            Mapper.CreateMap<SaleEnquiryHeader, DocumentUniqueId>();
            Mapper.CreateMap<SaleEnquiryHeaderIndexViewModel, DocumentUniqueId>();


            Mapper.CreateMap<DirectSaleInvoiceHeaderViewModel, SaleInvoiceHeader>();
            Mapper.CreateMap<SaleInvoiceHeader, DirectSaleInvoiceHeaderViewModel>();

            Mapper.CreateMap<DirectSaleInvoiceHeaderViewModel, PackingHeader>();
            Mapper.CreateMap<PackingHeader, DirectSaleInvoiceHeaderViewModel>();

            Mapper.CreateMap<DirectSaleInvoiceHeaderViewModel, SaleDispatchHeader>();
            Mapper.CreateMap<SaleDispatchHeader, DirectSaleInvoiceHeaderViewModel>();

            Mapper.CreateMap<DirectSaleInvoiceLineViewModel, SaleInvoiceLine>();
            Mapper.CreateMap<SaleInvoiceLine, DirectSaleInvoiceLineViewModel>();

            Mapper.CreateMap<DirectSaleInvoiceLineViewModel, PackingLine>();
            Mapper.CreateMap<PackingLine, DirectSaleInvoiceLineViewModel>();

            Mapper.CreateMap<DirectSaleInvoiceLineViewModel, SaleDispatchLine>();
            Mapper.CreateMap<SaleDispatchLine, DirectSaleInvoiceLineViewModel>();

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

            Mapper.CreateMap<PersonAddress, AgentViewModel>();
            Mapper.CreateMap<AgentViewModel, PersonAddress>();

            Mapper.CreateMap<PersonAddress, EmployeeViewModel>();
            Mapper.CreateMap<EmployeeViewModel, PersonAddress>();

            Mapper.CreateMap<PersonAddress, SupplierViewModel>();
            Mapper.CreateMap<SupplierViewModel, PersonAddress>();

            Mapper.CreateMap<PersonAddress, TransporterViewModel>();
            Mapper.CreateMap<TransporterViewModel, PersonAddress>();

            Mapper.CreateMap<PackingHeader, PackingHeaderViewModel>();
            Mapper.CreateMap<PackingHeaderViewModel, PackingHeader>();

            Mapper.CreateMap<PackingHeaderViewModelWithLog, PackingHeaderViewModel>();
            Mapper.CreateMap<PackingHeaderViewModel, PackingHeaderViewModelWithLog>();

            Mapper.CreateMap<PackingLine, PackingLineViewModel>();
            Mapper.CreateMap<PackingLineViewModel, PackingLine>();

            Mapper.CreateMap<JobReceiveSettings, JobReceiveSettingsViewModel>();
            Mapper.CreateMap<JobReceiveSettingsViewModel, JobReceiveSettings>();

            Mapper.CreateMap<JobReceiveQASettings, JobReceiveQASettingsViewModel>();
            Mapper.CreateMap<JobReceiveQASettingsViewModel, JobReceiveQASettings>();

            Mapper.CreateMap<JobReceiveHeader, JobReceiveHeaderViewModel>();
            Mapper.CreateMap<JobReceiveHeaderViewModel, JobReceiveHeader>();

            Mapper.CreateMap<JobReceiveQAPenalty, JobReceiveQAPenaltyViewModel>();
            Mapper.CreateMap<JobReceiveQAPenaltyViewModel, JobReceiveQAPenalty>();



            Mapper.CreateMap<FinishedProductViewModel, FinishedProduct>().ForMember(m => m.CreatedDate, x => x.Ignore())
              .ForMember(m => m.CreatedBy, x => x.Ignore())
              .ForMember(m => m.ModifiedBy, x => x.Ignore())
              .ForMember(m => m.ModifiedDate, x => x.Ignore())
              .ForMember(m => m.ImageFileName, x => x.Ignore())
              .ForMember(m => m.ImageFolderName, x => x.Ignore())
              .ForMember(m => m.IsSystemDefine, x => x.Ignore())
              .ForMember(m => m.SalesTaxGroupProductId, x => x.Ignore())
              .ForMember(m => m.UnitId, x => x.Ignore());
            Mapper.CreateMap<FinishedProduct, FinishedProductViewModel>();

            Mapper.CreateMap<CustomDetail, CustomDetailViewModel>();
            Mapper.CreateMap<CustomDetailViewModel, CustomDetail>();

            Mapper.CreateMap<CustomDetailViewModelWithLog, CustomDetailViewModel>();
            Mapper.CreateMap<CustomDetailViewModel, CustomDetailViewModelWithLog>();          

            Mapper.CreateMap<Buyer, BuyerViewModel>();
            Mapper.CreateMap<BuyerViewModel, Buyer>();

            Mapper.CreateMap<Person, BuyerViewModel>();
            Mapper.CreateMap<BuyerViewModel, Person>();

            Mapper.CreateMap<PersonAddress, BuyerViewModel>();
            Mapper.CreateMap<BuyerViewModel, PersonAddress>();

            Mapper.CreateMap<LedgerAccount, BuyerViewModel>();
            Mapper.CreateMap<BuyerViewModel, LedgerAccount>();

            Mapper.CreateMap<BusinessEntity, BuyerViewModel>();
            Mapper.CreateMap<BuyerViewModel, BusinessEntity>();

            Mapper.CreateMap<BusinessEntity, TransporterViewModel>();
            Mapper.CreateMap<TransporterViewModel, BusinessEntity>();
           
            Mapper.CreateMap<BusinessEntity, JobWorkerViewModel>();
            Mapper.CreateMap<JobWorkerViewModel, BusinessEntity>();

            Mapper.CreateMap<BusinessEntity, SupplierViewModel>();
            Mapper.CreateMap<SupplierViewModel, BusinessEntity>();

            Mapper.CreateMap<BusinessEntity, EmployeeViewModel>();
            Mapper.CreateMap<EmployeeViewModel, BusinessEntity>();

            Mapper.CreateMap<BusinessEntity, AgentViewModel>();
            Mapper.CreateMap<AgentViewModel, BusinessEntity>();

            Mapper.CreateMap<JobOrderSettings, JobOrderSettingsViewModel>();
            Mapper.CreateMap<JobOrderSettingsViewModel, JobOrderSettings>();

            Mapper.CreateMap<SaleEnquirySettings, SaleEnquirySettingsViewModel>();
            Mapper.CreateMap<SaleEnquirySettingsViewModel, SaleEnquirySettings>();

            Mapper.CreateMap<CarpetSkuSettings, CarpetSkuSettingsViewModel>();
            Mapper.CreateMap<CarpetSkuSettingsViewModel, CarpetSkuSettings>();

            Mapper.CreateMap<ProductBuyerSettings, ProductBuyerSettingsViewModel>();
            Mapper.CreateMap<ProductBuyerSettingsViewModel, ProductBuyerSettings>();

            Mapper.CreateMap<JobOrderHeader, JobOrderHeaderViewModel>();
            Mapper.CreateMap<JobOrderHeaderViewModel, JobOrderHeader>();

            Mapper.CreateMap<JobOrderPerk, PerkViewModel>();
            Mapper.CreateMap<PerkViewModel, JobOrderPerk>().ForMember(m => m.CreatedDate, x => x.Ignore()).ForMember(m => m.CreatedBy, x => x.Ignore())
                .ForMember(m => m.ModifiedDate, x => x.Ignore())
                .ForMember(m => m.ModifiedBy, x => x.Ignore());

            Mapper.CreateMap<LineChargeViewModel, JobOrderLineCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<JobOrderLineCharge, LineChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<HeaderChargeViewModel, JobOrderHeaderCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<JobOrderHeaderCharge, HeaderChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<CalculationFooterViewModel, HeaderChargeViewModel>();
            Mapper.CreateMap<CalculationProductViewModel, LineChargeViewModel>();

            Mapper.CreateMap<SaleInvoiceSetting, SaleInvoiceSettingsViewModel>();
            Mapper.CreateMap<SaleInvoiceSettingsViewModel, SaleInvoiceSetting>();

            Mapper.CreateMap<LineChargeViewModel, SaleInvoiceLineCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<SaleInvoiceLineCharge, LineChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<HeaderChargeViewModel, SaleInvoiceHeaderCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<SaleInvoiceHeaderCharge, HeaderChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<LineChargeViewModel, SaleInvoiceReturnLineCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<SaleInvoiceReturnLineCharge, LineChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<HeaderChargeViewModel, SaleInvoiceReturnHeaderCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<SaleInvoiceReturnHeaderCharge, HeaderChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<HeaderChargeViewModel, HeaderChargeViewModel>();
            Mapper.CreateMap<LineChargeViewModel, LineChargeViewModel>();

            Mapper.CreateMap<JobOrderPerk, PerkViewModel>();
            Mapper.CreateMap<PerkViewModel, JobOrderPerk>().ForMember(m => m.CreatedDate, x => x.Ignore()).ForMember(m => m.CreatedBy, x => x.Ignore())
                .ForMember(m => m.ModifiedDate, x => x.Ignore())
                .ForMember(m => m.ModifiedBy, x => x.Ignore());

            Mapper.CreateMap<Perk, PerkViewModel>();
            Mapper.CreateMap<PerkViewModel, Perk>();

            Mapper.CreateMap<SaleInvoiceReturnHeaderViewModel, SaleInvoiceReturnHeader>();
            Mapper.CreateMap<SaleInvoiceReturnHeader, SaleInvoiceReturnHeaderViewModel>();

            Mapper.CreateMap<SaleInvoiceReturnHeaderViewModel, SaleDispatchReturnHeader>();
            Mapper.CreateMap<SaleDispatchReturnHeader, SaleInvoiceReturnHeaderViewModel>();

            Mapper.CreateMap<SaleDispatchReturnLine, SaleInvoiceReturnLine>();
            Mapper.CreateMap<SaleInvoiceReturnLine, SaleDispatchReturnLine>();

            Mapper.CreateMap<SaleInvoiceReturnLine, SaleInvoiceReturnLineViewModel>();
            Mapper.CreateMap<SaleInvoiceReturnLineViewModel, SaleInvoiceReturnLine>();

            Mapper.CreateMap<DirectSaleInvoiceHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<SaleInvoiceHeader, DocumentUniqueId>();
            Mapper.CreateMap<SaleInvoiceHeaderIndexViewModel, DocumentUniqueId>();
            Mapper.CreateMap<SaleInvoiceHeaderDetail, DocumentUniqueId>();

            Mapper.CreateMap<JobOrderHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<JobOrderHeader, DocumentUniqueId>();

            Mapper.CreateMap<JobReceiveHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<JobReceiveHeader, DocumentUniqueId>();

            Mapper.CreateMap<PackingHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<PackingHeader, DocumentUniqueId>();

            Mapper.CreateMap<SaleInvoiceReturnHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<SaleInvoiceReturnHeader, DocumentUniqueId>();

            Mapper.CreateMap<JobReceiveLine, JobReceiveLineViewModel>();
            Mapper.CreateMap<JobReceiveLineViewModel, JobReceiveLine>();

            Mapper.CreateMap<ProductCategorySettings, ProductCategorySettingsViewModel>();
            Mapper.CreateMap<ProductCategorySettingsViewModel, ProductCategorySettings>();

            Mapper.CreateMap<ProductCategoryProcessSettings, ProductCategoryProcessSettingsViewModel>();
            Mapper.CreateMap<ProductCategoryProcessSettingsViewModel, ProductCategoryProcessSettings>();



            Mapper.CreateMap<SaleInvoiceReturnHeader, SaleInvoiceReturnHeader>();
            Mapper.CreateMap<SaleDispatchReturnHeader, SaleDispatchReturnHeader>();
            Mapper.CreateMap<SaleInvoiceReturnLineIndexViewModel, SaleInvoiceReturnLine>();
            Mapper.CreateMap<SaleDispatchReturnLineIndexViewModel, SaleDispatchReturnLine>();
            Mapper.CreateMap<SaleInvoiceReturnLine, SaleInvoiceReturnLine>();
            Mapper.CreateMap<SaleDispatchReturnLine, SaleDispatchReturnLine>();
            Mapper.CreateMap<SaleInvoiceReturnLineCharge, SaleInvoiceReturnLineCharge>();
            Mapper.CreateMap<SaleInvoiceReturnHeaderCharge, SaleInvoiceReturnHeaderCharge>();
            Mapper.CreateMap<PackingLine, PackingLine>();
            Mapper.CreateMap<ProductBuyer, ProductBuyer>();
            Mapper.CreateMap<SaleDispatchLine, SaleDispatchLine>();
            Mapper.CreateMap<SaleInvoiceLine, SaleInvoiceLine>();
            Mapper.CreateMap<SaleInvoiceLineCharge, SaleInvoiceLineCharge>();
            Mapper.CreateMap<SaleInvoiceHeaderCharge, SaleInvoiceHeaderCharge>();
            Mapper.CreateMap<SaleInvoiceHeaderDetail, SaleInvoiceHeaderDetail>();
            Mapper.CreateMap<SaleDispatchHeader, SaleDispatchHeader>();
            Mapper.CreateMap<SaleInvoiceLine, SaleInvoiceLine>();
            Mapper.CreateMap<SaleDispatchLine, SaleDispatchLine>();
            Mapper.CreateMap<SaleInvoiceHeader, SaleInvoiceHeader>();
            Mapper.CreateMap<PackingHeader, PackingHeader>();
            Mapper.CreateMap<CostCenter, CostCenter>();
            Mapper.CreateMap<Colour, Colour>();
            Mapper.CreateMap<CustomDetail, CustomDetail>();
            Mapper.CreateMap<DeliveryTerms, DeliveryTerms>();
            Mapper.CreateMap<DescriptionOfGoods, DescriptionOfGoods>();
            Mapper.CreateMap<Product, Product>();
            Mapper.CreateMap<BomDetail, BomDetail>();
            
            Mapper.CreateMap<ProductCategory, ProductCategory>();
            Mapper.CreateMap<ProductCollection, ProductCollection>();
            Mapper.CreateMap<ProductCategory, ProductCategory>();
            Mapper.CreateMap<ProductContentHeader, ProductContentHeader>();
            Mapper.CreateMap<ProductContentLine, ProductContentLine>();
            Mapper.CreateMap<ProductDesign, ProductDesign>();
            Mapper.CreateMap<ProductDesignPattern, ProductDesignPattern>();
            Mapper.CreateMap<ProductInvoiceGroup, ProductInvoiceGroup>();
            Mapper.CreateMap<ProductNature, ProductNature>();
            Mapper.CreateMap<ProductQuality, ProductQuality>();
            Mapper.CreateMap<ProductShape, ProductShape>();
            Mapper.CreateMap<ProductSizeType, ProductSizeType>();
            Mapper.CreateMap<ProductStyle, ProductStyle>();
            Mapper.CreateMap<ReportLine, ReportLine>();           
            Mapper.CreateMap<SalesTaxGroupParty, SalesTaxGroupParty>();
            Mapper.CreateMap<SalesTaxGroupProduct, SalesTaxGroupProduct>();
            Mapper.CreateMap<ShipMethod, ShipMethod>();
            Mapper.CreateMap<Size, Size>();
            Mapper.CreateMap<JobReceiveLine, JobReceiveLine>();
            Mapper.CreateMap<RateListLine, RateListLine>();
            Mapper.CreateMap<RateListHeader, RateListHeader>();
            Mapper.CreateMap<DocumentTypeTimeExtension, DocumentTypeTimeExtensionViewModel>();
            Mapper.CreateMap<DocumentTypeTimeExtensionViewModel, DocumentTypeTimeExtension>();
            Mapper.CreateMap<DocumentTypeTimeExtension, DocumentTypeTimeExtension>();
            Mapper.CreateMap<ProductProcess, ProductProcess>();
            Mapper.CreateMap<SaleInvoiceSetting, SaleInvoiceSetting>();

        }
    }
}
