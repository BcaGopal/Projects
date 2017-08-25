using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Store.Controllers;
using Data.Models;
using Data.Infrastructure;
using Service;
using Model.Models;
using AutoMapper;
using Model.ViewModel;
using Model.ViewModels;

namespace Store.App_Start
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
            
            container.RegisterType<IExceptionHandlingService, ExceptionHandlingService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<SalesTaxProductCode>, Repository<SalesTaxProductCode>>();
            container.RegisterType<ISalesTaxProductCodeService, SalesTaxProductCodeService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ImportHeader>, Repository<ImportHeader>>();
            container.RegisterType<IImportHeaderService, ImportHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ImportLine>, Repository<ImportLine>>();
            container.RegisterType<IImportLineService, ImportLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Godown>, Repository<Godown>>();
            container.RegisterType<IGodownService, GodownService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ChargeGroupSettings>, Repository<ChargeGroupSettings>>();
            container.RegisterType<IChargeGroupSettingsService, ChargeGroupSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<StockHeaderSettings>, Repository<StockHeaderSettings>>();
            container.RegisterType<IStockHeaderSettingsService, StockHeaderSettingsService>(new PerRequestLifetimeManager());


            container.RegisterType<IRepository<MaterialRequestSettings>, Repository<MaterialRequestSettings>>();
            container.RegisterType<IMaterialRequestSettingsService, MaterialRequestSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Person>, Repository<Person>>();
            container.RegisterType<IPersonService, PersonService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<BusinessEntity>, Repository<BusinessEntity>>();
            container.RegisterType<IBusinessEntityService, BusinessEntityService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<LedgerAccount>, Repository<LedgerAccount>>();
            container.RegisterType<IAccountService, AccountService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonProcess>, Repository<PersonProcess>>();
            container.RegisterType<IPersonProcessService, PersonProcessService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonRegistration>, Repository<PersonRegistration>>();
            container.RegisterType<IPersonRegistrationService, PersonRegistrationService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonAddress>, Repository<PersonAddress>>();
            container.RegisterType<IPersonAddressService, PersonAddressService>(new PerRequestLifetimeManager());

            container.RegisterType<IPersonOpeningService, PersonOpeningService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonContact>, Repository<PersonContact>>();
            container.RegisterType<IPersonContactService, PersonContactService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonRole>, Repository<PersonRole>>();
            container.RegisterType<IPersonRoleService, PersonRoleService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonBankAccount>, Repository<PersonBankAccount>>();
            container.RegisterType<IPersonBankAccountService, PersonBankAccountService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonSettings>, Repository<PersonSettings>>();
            container.RegisterType<IPersonSettingsService, PersonSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<StockHeader>, Repository<StockHeader>>();
            container.RegisterType<IStockHeaderService, StockHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<StockLine>, Repository<StockLine>>();
            container.RegisterType<IStockLineService, StockLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Stock>, Repository<Stock>>();
            container.RegisterType<IStockService, StockService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<StockUid>, Repository<StockUid>>();
            container.RegisterType<IStockUidService, StockUidService>(new PerRequestLifetimeManager());            

            container.RegisterType<IRepository<MaterialReceiveSettings>, Repository<MaterialReceiveSettings>>();
            container.RegisterType<IMaterialReceiveSettingsService, MaterialReceiveSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Dimension1>, Repository<Dimension1>>();
            container.RegisterType<IDimension1Service, Dimension1Service>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Dimension2>, Repository<Dimension2>>();
            container.RegisterType<IDimension2Service, Dimension2Service>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Dimension3>, Repository<Dimension3>>();
            container.RegisterType<IDimension3Service, Dimension3Service>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Dimension4>, Repository<Dimension4>>();
            container.RegisterType<IDimension4Service, Dimension4Service>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Godown>, Repository<Godown>>();
            container.RegisterType<IGodownService, GodownService>(new PerRequestLifetimeManager());

            container.RegisterType<IGateService, GateService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<RequisitionHeader>, Repository<RequisitionHeader>>();
            container.RegisterType<IRequisitionHeaderService, RequisitionHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<RequisitionLine>, Repository<RequisitionLine>>();
            container.RegisterType<IRequisitionLineService, RequisitionLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Process>, Repository<Process>>();
            container.RegisterType<IProcessService, ProcessService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProcessSequenceHeader>, Repository<ProcessSequenceHeader>>();
            container.RegisterType<IProcessSequenceHeaderService, ProcessSequenceHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProcessSequenceLine>, Repository<ProcessSequenceLine>>();
            container.RegisterType<IProcessSequenceLineService, ProcessSequenceLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonRateGroup>, Repository<PersonRateGroup>>();
            container.RegisterType<IPersonRateGroupService, PersonRateGroupService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Product>, Repository<Product>>();
            container.RegisterType<IProductService, ProductService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<BomDetail>, Repository<BomDetail>>();
            container.RegisterType<IBomDetailService, BomDetailService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductProcess>, Repository<ProductProcess>>();
            container.RegisterType<IProductProcessService, ProductProcessService>(new PerRequestLifetimeManager());


            container.RegisterType<IRepository<FinishedProduct>, Repository<FinishedProduct>>();
            container.RegisterType<IFinishedProductService, FinishedProductService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductCustomGroupHeader>, Repository<ProductCustomGroupHeader>>();
            container.RegisterType<IProductCustomGroupHeaderService, ProductCustomGroupHeaderService>(new PerRequestLifetimeManager());





            container.RegisterType<IRepository<ProductCustomGroupLine>, Repository<ProductCustomGroupLine>>();
            container.RegisterType<IProductCustomGroupLineService, ProductCustomGroupLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductGroupSettings>, Repository<ProductGroupSettings>>();
            container.RegisterType<IProductGroupSettingsService, ProductGroupSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductGroupProcessSettings>, Repository<ProductGroupProcessSettings>>();
            container.RegisterType<IProductGroupProcessSettingsService, ProductGroupProcessSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductGroup>, Repository<ProductGroup>>();
            container.RegisterType<IProductGroupService, ProductGroupService>(new PerRequestLifetimeManager());


            container.RegisterType<IRepository<ProductCategorySettings>, Repository<ProductCategorySettings>>();
            container.RegisterType<IProductCategorySettingsService, ProductCategorySettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductCategoryProcessSettings>, Repository<ProductCategoryProcessSettings>>();
            container.RegisterType<IProductCategoryProcessSettingsService, ProductCategoryProcessSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductCategory>, Repository<ProductCategory>>();
            container.RegisterType<IProductCategoryService, ProductCategoryService>(new PerRequestLifetimeManager());


            container.RegisterType<IRepository<ProductTypeAttribute>, Repository<ProductTypeAttribute>>();
            container.RegisterType<IProductTypeAttributeService, ProductTypeAttributeService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductType>, Repository<ProductType>>();
            container.RegisterType<IProductTypeService, ProductTypeService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductTypeSettings>, Repository<ProductTypeSettings>>();
            container.RegisterType<IProductTypeSettingsService, ProductTypeSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Unit>, Repository<Unit>>();
            container.RegisterType<IUnitService, UnitService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<UnitConversion>, Repository<UnitConversion>>();
            container.RegisterType<IUnitConversionService, UnitConversionService>(new PerRequestLifetimeManager());

            container.RegisterType<IReportLineService, ReportLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IActivityLogService, ActivityLogService>(new PerRequestLifetimeManager());

            container.RegisterType<IStockInHandService, StockInHandService>(new PerRequestLifetimeManager());

            container.RegisterType<IStockProcessDisplayService, StockProcessDisplayService>(new PerRequestLifetimeManager());

            container.RegisterType<IStockInHandSettingService, StockInHandSettingService>(new PerRequestLifetimeManager());

            container.RegisterType<IRequisitionSettingService, RequisitionSettingService>(new PerRequestLifetimeManager());

            container.RegisterType<IRequisitionCancelHeaderService, RequisitionCancelHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRequisitionCancelLineService, RequisitionCancelLineService>(new PerRequestLifetimeManager());


            container.RegisterType<IDuplicateDocumentCheckService, DuplicateDocumentCheckService>(new PerRequestLifetimeManager());
            container.RegisterType<IProductUidService, ProductUidService>(new PerRequestLifetimeManager());

            container.RegisterType<IProductUidHeaderService, ProductUidHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IProductRateGroupService, ProductRateGroupService>(new PerRequestLifetimeManager());

            container.RegisterType<IRateListHeaderService, RateListHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRateListPersonRateGroupService, RateListPersonRateGroupService>(new PerRequestLifetimeManager());

            container.RegisterType<IRateListProductRateGroupService, RateListProductRateGroupService>(new PerRequestLifetimeManager());

            container.RegisterType<IRateListLineService, RateListLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRateListLineHistoryService, RateListLineHistoryService>(new PerRequestLifetimeManager());

            container.RegisterType<IExcessMaterialHeaderService, ExcessMaterialHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IExcessMaterialLineService, ExcessMaterialLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IExcessMaterialSettingsService, ExcessMaterialSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<ILedgerSettingService, LedgerSettingService>(new PerRequestLifetimeManager());


            //Registering Mappers:



            //new Gopal
            container.RegisterType<IRepository<GatePassHeader>, Repository<GatePassHeader>>();
            container.RegisterType<IGatePassHeaderService, GatePassHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<GatePassLine>, Repository<GatePassLine>>();
            container.RegisterType<IGatePassLineService, GatePassLineService>(new PerRequestLifetimeManager());


            container.RegisterType<IRepository<QAGroup>, Repository<QAGroup>>();
            container.RegisterType<IQAGroupService, QAGroupService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<QAGroupLine>, Repository<QAGroupLine>>();
            container.RegisterType<IQAGroupLineService, QAGroupLineService>(new PerRequestLifetimeManager());

            Mapper.CreateMap<Person, PersonViewModel>();
            Mapper.CreateMap<PersonViewModel, Person>();

            Mapper.CreateMap<BusinessEntity, PersonViewModel>();
            Mapper.CreateMap<PersonViewModel, BusinessEntity>();

            Mapper.CreateMap<LedgerAccount, PersonViewModel>();
            Mapper.CreateMap<PersonViewModel, LedgerAccount>();

            Mapper.CreateMap<PersonAddress, PersonViewModel>();
            Mapper.CreateMap<PersonViewModel, PersonAddress>();

            Mapper.CreateMap<PersonContact, PersonContactViewModel>();
            Mapper.CreateMap<PersonContactViewModel, PersonContact>();

            Mapper.CreateMap<PersonRole, PersonRoleViewModel>();
            Mapper.CreateMap<PersonRoleViewModel, PersonRole>();

            Mapper.CreateMap<PersonSettings, PersonSettingsViewModel>();
            Mapper.CreateMap<PersonSettingsViewModel, PersonSettings>();

            Mapper.CreateMap<GatePassHeader, GatePassHeaderViewModel>();
            Mapper.CreateMap<GatePassHeaderViewModel, GatePassHeader>();

            Mapper.CreateMap<GatePassLine, GatePassLineViewModel>();
            Mapper.CreateMap<GatePassLineViewModel, GatePassLine>();

            Mapper.CreateMap<QAGroup, QAGroupViewModel>();
            Mapper.CreateMap<QAGroupViewModel, QAGroup>();

            Mapper.CreateMap<QAGroupLine, QAGroupLineViewModel>();
            Mapper.CreateMap<QAGroupLineViewModel, QAGroupLine>();

            //////
            Mapper.CreateMap<ProcessSequenceHeader, ProcessSequenceHeaderIndexViewModel>();
            Mapper.CreateMap<ProcessSequenceHeaderIndexViewModel, ProcessSequenceHeader>();

            Mapper.CreateMap<ProcessSequenceHeaderIndexViewModelForEdit, ProcessSequenceHeaderIndexViewModel>();
            Mapper.CreateMap<ProcessSequenceHeaderIndexViewModel, ProcessSequenceHeaderIndexViewModelForEdit>();


            Mapper.CreateMap<ProcessSequenceLine, ProcessSequenceLineViewModel>();
            Mapper.CreateMap<ProcessSequenceLineViewModel, ProcessSequenceLine>();

            Mapper.CreateMap<ProductGroupSettings, ProductGroupSettingsViewModel>();
            Mapper.CreateMap<ProductGroupSettingsViewModel, ProductGroupSettings>();

            Mapper.CreateMap<ProductCategorySettings, ProductCategorySettingsViewModel>();
            Mapper.CreateMap<ProductCategorySettingsViewModel, ProductCategorySettings>();

            Mapper.CreateMap<ProductTypeSettings, ProductTypeSettingsViewModel>();
            Mapper.CreateMap<ProductTypeSettingsViewModel, ProductTypeSettings>();

            Mapper.CreateMap<StockHeaderSettings, StockHeaderSettingsViewModel>();
            Mapper.CreateMap<StockHeaderSettingsViewModel, StockHeaderSettings>();

            Mapper.CreateMap<MaterialRequestSettings, MaterialRequestSettingsViewModel>();
            Mapper.CreateMap<MaterialRequestSettingsViewModel, MaterialRequestSettings>();

            Mapper.CreateMap<Stock, StockViewModel>();
            Mapper.CreateMap<StockViewModel, Stock>();

            Mapper.CreateMap<StockHeader, StockHeaderViewModel>();
            Mapper.CreateMap<StockHeaderViewModel, StockHeader>();

            Mapper.CreateMap<StockLine, StockLineViewModel>();
            Mapper.CreateMap<StockLineViewModel, StockLine>();            

            Mapper.CreateMap<RequisitionHeaderViewModel, RequisitionHeader>();
            Mapper.CreateMap<RequisitionHeader, RequisitionHeaderViewModel>();

            Mapper.CreateMap<RequisitionLineViewModel, RequisitionLine>();
            Mapper.CreateMap<RequisitionLine, RequisitionLineViewModel>();

            Mapper.CreateMap<RequisitionSetting, RequisitionSettingsViewModel>();
            Mapper.CreateMap<RequisitionSettingsViewModel, RequisitionSetting>();

            Mapper.CreateMap<RequisitionCancelHeader, RequisitionCancelHeaderViewModel>();
            Mapper.CreateMap<RequisitionCancelHeaderViewModel, RequisitionCancelHeader>();

            Mapper.CreateMap<RequisitionCancelLine, RequisitionCancelLineViewModel>();
            Mapper.CreateMap<RequisitionCancelLineViewModel, RequisitionCancelLine>();

            Mapper.CreateMap<RateListHeader, RateListHeaderViewModel>();
            Mapper.CreateMap<RateListHeaderViewModel, RateListHeader>();

            Mapper.CreateMap<ExcessMaterialHeader, ExcessMaterialHeaderViewModel>();
            Mapper.CreateMap<ExcessMaterialHeaderViewModel, ExcessMaterialHeader>();

            Mapper.CreateMap<ExcessMaterialLine, ExcessMaterialLineViewModel>();
            Mapper.CreateMap<ExcessMaterialLineViewModel, ExcessMaterialLine>();

            Mapper.CreateMap<ExcessMaterialSettings, ExcessMaterialSettingsViewModel>();
            Mapper.CreateMap<ExcessMaterialSettingsViewModel, ExcessMaterialSettings>();

            Mapper.CreateMap<StockHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<StockHeader, DocumentUniqueId>();

            Mapper.CreateMap<RequisitionCancelHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<RequisitionCancelHeader, DocumentUniqueId>();

            Mapper.CreateMap<RequisitionHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<RequisitionHeader, DocumentUniqueId>();

            Mapper.CreateMap<ExcessMaterialHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<ExcessMaterialHeader, DocumentUniqueId>();

            Mapper.CreateMap<ExcessMaterialLineViewModel, DocumentUniqueId>();
            Mapper.CreateMap<ExcessMaterialLine, DocumentUniqueId>();

            Mapper.CreateMap<ProductGroupProcessSettings, ProductGroupProcessSettingsViewModel>();
            Mapper.CreateMap<ProductGroupProcessSettingsViewModel, ProductGroupProcessSettings>();

            Mapper.CreateMap<LedgerSetting, LedgerSettingViewModel>();
            Mapper.CreateMap<LedgerSettingViewModel, LedgerSetting>();


            Mapper.CreateMap<Dimension1, Dimension1ViewModel>();
            Mapper.CreateMap<Dimension1ViewModel, Dimension1>();

            Mapper.CreateMap<Dimension2, Dimension2ViewModel>();
            Mapper.CreateMap<Dimension2ViewModel, Dimension2>();

            Mapper.CreateMap<Dimension3, Dimension3ViewModel>();
            Mapper.CreateMap<Dimension3ViewModel, Dimension3>();

            Mapper.CreateMap<Dimension4, Dimension4ViewModel>();
            Mapper.CreateMap<Dimension4ViewModel, Dimension4>();


            Mapper.CreateMap<RequisitionCancelHeader, RequisitionCancelHeader>();
            Mapper.CreateMap<RequisitionCancelLineViewModel, RequisitionCancelLine>();
            Mapper.CreateMap<StockHeader, StockHeader>();
            Mapper.CreateMap<StockLine, StockLine>();
            Mapper.CreateMap<RequisitionHeader, RequisitionHeader>();
            Mapper.CreateMap<RequisitionLineViewModel, RequisitionLine>();
            Mapper.CreateMap<StockHeader, StockHeader>();
            Mapper.CreateMap<StockLine, StockLine>();
            Mapper.CreateMap<RequisitionLine, RequisitionLine>();
            Mapper.CreateMap<RequisitionCancelLine, RequisitionCancelLine>();
            Mapper.CreateMap<PersonRateGroup, PersonRateGroup>();
            Mapper.CreateMap<Dimension1, Dimension1>();
            Mapper.CreateMap<Dimension2, Dimension2>();
            Mapper.CreateMap<Godown, Godown>();
            Mapper.CreateMap<ChargeGroupSettings, ChargeGroupSettings>();
            Mapper.CreateMap<Process, Process>();
            Mapper.CreateMap<ProcessSequenceHeader, ProcessSequenceHeader>();
            Mapper.CreateMap<ProcessSequenceLine, ProcessSequenceLine>();
            Mapper.CreateMap<FinishedProduct, FinishedProduct>();
            Mapper.CreateMap<Product, Product>();
            Mapper.CreateMap<ProductCustomGroupHeader, ProductCustomGroupHeader>();
            Mapper.CreateMap<ProductCustomGroupLine, ProductCustomGroupLine>();
            Mapper.CreateMap<ProductGroup, ProductGroup>();
            Mapper.CreateMap<ProductCategory, ProductCategory>();
            Mapper.CreateMap<ProductTypeAttribute, ProductTypeAttribute>();
            Mapper.CreateMap<ProductType, ProductType>();
            Mapper.CreateMap<Unit, Unit>();
            Mapper.CreateMap<Gate, Gate>();
            Mapper.CreateMap<ProductRateGroup, ProductRateGroup>();
            Mapper.CreateMap<RateListLine, RateListLine>();
            Mapper.CreateMap<RateListHeader, RateListHeader>();
            Mapper.CreateMap<UnitConversion, UnitConversion>();
            Mapper.CreateMap<ExcessMaterialSettings, ExcessMaterialSettings>();
            Mapper.CreateMap<MaterialRequestSettings, MaterialRequestSettings>();
            Mapper.CreateMap<RequisitionSetting, RequisitionSetting>();
            Mapper.CreateMap<StockHeaderSettings, StockHeaderSettings>();

            //new
            Mapper.CreateMap<GatePassHeader, GatePassHeader>();
            Mapper.CreateMap<GatePassLineViewModel, GatePassLine>();
            Mapper.CreateMap<GatePassHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<GatePassHeader, DocumentUniqueId>();

            Mapper.CreateMap<QAGroup, QAGroup>();
            Mapper.CreateMap<QAGroupLineViewModel, QAGroupLine>();
            Mapper.CreateMap<QAGroupViewModel, DocumentUniqueId>();
            Mapper.CreateMap<QAGroup, DocumentUniqueId>();

            Mapper.CreateMap<PersonRegistration, PersonRegistration>();
            Mapper.CreateMap<LedgerAccount, LedgerAccount>();
            Mapper.CreateMap<PersonAddress, PersonAddress>();
            Mapper.CreateMap<Person, Person>();
            Mapper.CreateMap<BusinessEntity, BusinessEntity>();
            Mapper.CreateMap<ProductProcess, ProductProcess>();

            Mapper.CreateMap<ImportHeader, ImportHeaderViewModel>();
            Mapper.CreateMap<ImportHeaderViewModel, ImportHeader>();

            Mapper.CreateMap<ImportLine, ImportLineViewModel>();
            Mapper.CreateMap<ImportLineViewModel, ImportLine>();

        }
    }
}
