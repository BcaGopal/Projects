using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using AutoMapper;
using Model.Models;
using Data.Infrastructure;
using Service;
using Data.Models;
using Purchase.Controllers;
using Model.ViewModels;
using Model.ViewModel;

namespace Purchase.App_Start
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

            container.RegisterType<IPurchaseOrderHeaderService, PurchaseOrderHeaderService>(new PerRequestLifetimeManager());
            container.RegisterType<IPurchaseOrderLineService, PurchaseOrderLineService>(new PerRequestLifetimeManager());
            container.RegisterType<IExceptionHandlingService, ExceptionHandlingService>(new PerRequestLifetimeManager());
            container.RegisterType<IModuleService, ModuleService>(new PerRequestLifetimeManager());
            container.RegisterType<ISubModuleService, SubModuleService>(new PerRequestLifetimeManager());
            container.RegisterType<IPurchaseOrderSettingService, PurchaseOrderSettingService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseIndentHeader>, Repository<PurchaseIndentHeader>>();
            container.RegisterType<IPurchaseIndentHeaderService, PurchaseIndentHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseIndentLine>, Repository<PurchaseIndentLine>>();
            container.RegisterType<IPurchaseIndentLineService, PurchaseIndentLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseOrderCancelHeader>, Repository<PurchaseOrderCancelHeader>>();
            container.RegisterType<IPurchaseOrderCancelHeaderService, PurchaseOrderCancelHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseOrderCancelLine>, Repository<PurchaseOrderCancelLine>>();
            container.RegisterType<IPurchaseOrderCancelLineService, PurchaseOrderCancelLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseIndentCancelHeader>, Repository<PurchaseIndentCancelHeader>>();
            container.RegisterType<IPurchaseIndentCancelHeaderService, PurchaseIndentCancelHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseIndentCancelLine>, Repository<PurchaseIndentCancelLine>>();
            container.RegisterType<IPurchaseIndentCancelLineService, PurchaseIndentCancelLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseInvoiceReturnHeader>, Repository<PurchaseInvoiceReturnHeader>>();
            container.RegisterType<IPurchaseInvoiceReturnHeaderService, PurchaseInvoiceReturnHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseInvoiceReturnLine>, Repository<PurchaseInvoiceReturnLine>>();
            container.RegisterType<IPurchaseInvoiceReturnLineService, PurchaseInvoiceReturnLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseGoodsReturnHeader>, Repository<PurchaseGoodsReturnHeader>>();
            container.RegisterType<IPurchaseGoodsReturnHeaderService, PurchaseGoodsReturnHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseGoodsReturnLine>, Repository<PurchaseGoodsReturnLine>>();
            container.RegisterType<IPurchaseGoodsReturnLineService, PurchaseGoodsReturnLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseGoodsReceiptHeader>, Repository<PurchaseGoodsReceiptHeader>>();
            container.RegisterType<IPurchaseGoodsReceiptHeaderService, PurchaseGoodsReceiptHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseGoodsReceiptLine>, Repository<PurchaseGoodsReceiptLine>>();
            container.RegisterType<IPurchaseGoodsReceiptLineService, PurchaseGoodsReceiptLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProductUid>, Repository<ProductUid>>();
            container.RegisterType<IProductUidService, ProductUidService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseInvoiceHeader>, Repository<PurchaseInvoiceHeader>>();
            container.RegisterType<IPurchaseInvoiceHeaderService, PurchaseInvoiceHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IDuplicateDocumentCheckService, DuplicateDocumentCheckService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseInvoiceLine>, Repository<PurchaseInvoiceLine>>();
            container.RegisterType<IPurchaseInvoiceLineService, PurchaseInvoiceLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Supplier>, Repository<Supplier>>();
            container.RegisterType<ISupplierService, SupplierService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<BusinessEntity>, Repository<BusinessEntity>>();
            container.RegisterType<IBusinessEntityService, BusinessEntityService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Person>, Repository<Person>>();
            container.RegisterType<IPersonService, PersonService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonAddress>, Repository<PersonAddress>>();
            container.RegisterType<IPersonAddressService, PersonAddressService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<LedgerAccount>, Repository<LedgerAccount>>();
            container.RegisterType<IAccountService, AccountService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonProcess>, Repository<PersonProcess>>();
            container.RegisterType<IPersonProcessService, PersonProcessService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonRegistration>, Repository<PersonRegistration>>();
            container.RegisterType<IPersonRegistrationService, PersonRegistrationService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseWaybill>, Repository<PurchaseWaybill>>();
            container.RegisterType<IPurchaseWaybillService, PurchaseWaybillService>(new PerRequestLifetimeManager());


            container.RegisterType<IPurchaseGoodsReceiptSettingService, PurchaseGoodsReceiptSettingService>(new PerRequestLifetimeManager());

            container.RegisterType<IPurchaseInvoiceSettingService, PurchaseInvoiceSettingService>(new PerRequestLifetimeManager());

            container.RegisterType<IReportLineService, ReportLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IPurchaseIndentSettingService, PurchaseIndentSettingService>(new PerRequestLifetimeManager());

            container.RegisterType<IChargesCalculationService, ChargesCalculationService>(new PerRequestLifetimeManager());

            container.RegisterType<IPersonContactService, PersonContactService>(new PerRequestLifetimeManager());

            container.RegisterType<IPersonBankAccountService, PersonBankAccountService>(new PerRequestLifetimeManager());


            container.RegisterType<IDocEmailContentService, DocEmailContentService>(new PerRequestLifetimeManager());

            container.RegisterType<IDocNotificationContentService, DocNotificationContentService>(new PerRequestLifetimeManager());

            container.RegisterType<IUpdatePurchaseExpiryService, UpdatePurchaseExpiryService>(new PerRequestLifetimeManager());           

            container.RegisterType<IPurchaseOrderRateAmendmentLineService, PurchaseOrderRateAmendmentLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IPurchaseQuotationHeaderService, PurchaseQuotationHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IPurchaseQuotationLineService, PurchaseQuotationLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IPurchaseQuotationSettingService, PurchaseQuotationSettingService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseInvoiceHeader>, Repository<PurchaseInvoiceHeader>>();
            container.RegisterType<IPurchaseInvoiceHeaderService, PurchaseInvoiceHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PurchaseInvoiceLine>, Repository<PurchaseInvoiceLine>>();
            container.RegisterType<IPurchaseInvoiceLineService, PurchaseInvoiceLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ActivityLog>, Repository<ActivityLog>>();
            container.RegisterType<IActivityLogService, ActivityLogService>(new PerRequestLifetimeManager());

            //Registering Mappers:

            Mapper.CreateMap<Person, SupplierViewModel>();
            Mapper.CreateMap<SupplierViewModel, Person>();

            Mapper.CreateMap<BusinessEntity, SupplierViewModel>();
            Mapper.CreateMap<SupplierViewModel, BusinessEntity>();

            Mapper.CreateMap<Supplier, SupplierViewModel>();
            Mapper.CreateMap<SupplierViewModel, Supplier>();

            Mapper.CreateMap<PersonAddress, SupplierViewModel>();
            Mapper.CreateMap<SupplierViewModel, PersonAddress>();

            Mapper.CreateMap<LedgerAccount, SupplierViewModel>();
            Mapper.CreateMap<SupplierViewModel, LedgerAccount>();


           
            Mapper.CreateMap<PersonContact, PersonContactViewModel>();
            Mapper.CreateMap<PersonContactViewModel, PersonContact>();

            Mapper.CreateMap<PurchaseInvoiceHeader, PurchaseInvoiceHeaderViewModel>();
            Mapper.CreateMap<PurchaseInvoiceHeaderViewModel, PurchaseInvoiceHeader>();

            Mapper.CreateMap<PurchaseInvoiceLine, PurchaseInvoiceLineViewModel>();
            Mapper.CreateMap<PurchaseInvoiceLineViewModel, PurchaseInvoiceLine>();

            Mapper.CreateMap<PurchaseQuotationLine, PurchaseQuotationLineViewModel>();
            Mapper.CreateMap<PurchaseQuotationLineViewModel, PurchaseQuotationLine>();

            Mapper.CreateMap<PurchaseQuotationHeader, PurchaseQuotationHeaderViewModel>();
            Mapper.CreateMap<PurchaseQuotationHeaderViewModel, PurchaseQuotationHeader>();

            Mapper.CreateMap<PurchaseIndentHeader, PurchaseIndentHeaderViewModel>();
            Mapper.CreateMap<PurchaseIndentHeaderViewModel, PurchaseIndentHeader>();

            Mapper.CreateMap<PurchaseIndentLine, PurchaseIndentLineViewModel>();
            Mapper.CreateMap<PurchaseIndentLineViewModel, PurchaseIndentLine>();

            Mapper.CreateMap<PurchaseOrderHeader, PurchaseOrderHeaderViewModel>();
            Mapper.CreateMap<PurchaseOrderHeaderViewModel, PurchaseOrderHeader>();

            Mapper.CreateMap<PurchaseOrderLineViewModel, PurchaseOrderLine>();
            Mapper.CreateMap<PurchaseOrderLine, PurchaseOrderLineViewModel>();

            Mapper.CreateMap<PurchaseOrderSetting, PurchaseOrderSettingsViewModel>();
            Mapper.CreateMap<PurchaseOrderSettingsViewModel, PurchaseOrderSetting>();

            Mapper.CreateMap<LineChargeViewModel, PurchaseOrderLineCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<PurchaseOrderLineCharge, LineChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<HeaderChargeViewModel, PurchaseOrderHeaderCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<PurchaseOrderHeaderCharge, HeaderChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<PurchaseGoodsReceiptHeader, PurchaseGoodsReceiptHeaderViewModel>();
            Mapper.CreateMap<PurchaseGoodsReceiptHeaderViewModel, PurchaseGoodsReceiptHeader>();

            Mapper.CreateMap<PurchaseGoodsReceiptLine, PurchaseGoodsReceiptLineViewModel>();
            Mapper.CreateMap<PurchaseGoodsReceiptLineViewModel, PurchaseGoodsReceiptLine>();            

            Mapper.CreateMap<PurchaseOrderCancelHeaderViewModel, PurchaseOrderCancelHeader>();
            Mapper.CreateMap<PurchaseOrderCancelHeader, PurchaseOrderCancelHeaderViewModel>();

            Mapper.CreateMap<PurchaseGoodsReceiptHeader, PurchaseGoodsReceiptHeaderViewModel>();
            Mapper.CreateMap<PurchaseGoodsReceiptHeaderViewModel, PurchaseGoodsReceiptHeader>();

            Mapper.CreateMap<PurchaseGoodsReceiptLine, PurchaseGoodsReceiptLineViewModel>();
            Mapper.CreateMap<PurchaseGoodsReceiptLineViewModel, PurchaseGoodsReceiptLine>();

            Mapper.CreateMap<PurchaseGoodsReceiptHeaderViewModel, PurchaseWaybill>();
            Mapper.CreateMap<PurchaseWaybill, PurchaseGoodsReceiptHeaderViewModel>();

            Mapper.CreateMap<PurchaseInvoiceHeader, PurchaseInvoiceHeaderViewModel>().ForMember(m => m.CreatedBy, x => x.Ignore())
               .ForMember(m => m.CreatedDate, x => x.Ignore())
               .ForMember(m => m.ModifiedBy, x => x.Ignore())
               .ForMember(m => m.ModifiedDate, x => x.Ignore())
               .ForMember(m => m.DivisionId, x => x.Ignore())
               .ForMember(m => m.SiteId, x => x.Ignore());
            Mapper.CreateMap<PurchaseInvoiceHeaderViewModel, PurchaseInvoiceHeader>().ForMember(m => m.CreatedBy, x => x.Ignore())
                .ForMember(m => m.CreatedDate, x => x.Ignore())
                .ForMember(m => m.ModifiedBy, x => x.Ignore())
                .ForMember(m => m.ModifiedDate, x => x.Ignore())
                .ForMember(m => m.DivisionId, x => x.Ignore())
                .ForMember(m => m.SiteId, x => x.Ignore());

            Mapper.CreateMap<PurchaseIndentCancelHeader, PurchaseIndentCancelHeaderViewModel>();
            Mapper.CreateMap<PurchaseIndentCancelHeaderViewModel, PurchaseIndentCancelHeader>();

            Mapper.CreateMap<PurchaseIndentCancelLine, PurchaseIndentCancelLineViewModel>();
            Mapper.CreateMap<PurchaseIndentCancelLineViewModel, PurchaseIndentCancelLine>();

            Mapper.CreateMap<PurchaseInvoiceReturnHeader, PurchaseInvoiceReturnHeaderViewModel>();
            Mapper.CreateMap<PurchaseInvoiceReturnHeaderViewModel, PurchaseInvoiceReturnHeader>();

            Mapper.CreateMap<PurchaseInvoiceReturnLine, PurchaseInvoiceReturnLineViewModel>();
            Mapper.CreateMap<PurchaseInvoiceReturnLineViewModel, PurchaseInvoiceReturnLine>();

            Mapper.CreateMap<PurchaseGoodsReturnHeader, PurchaseGoodsReturnHeaderViewModel>();
            Mapper.CreateMap<PurchaseGoodsReturnHeaderViewModel, PurchaseGoodsReturnHeader>();

            Mapper.CreateMap<PurchaseGoodsReturnLine, PurchaseGoodsReturnLineViewModel>();
            Mapper.CreateMap<PurchaseGoodsReturnLineViewModel, PurchaseGoodsReturnLine>();

            Mapper.CreateMap<LineChargeViewModel, PurchaseOrderLineCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<PurchaseOrderLineCharge, LineChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<LineChargeViewModel, PurchaseOrderRateAmendmentLineCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<PurchaseOrderRateAmendmentLineCharge, LineChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<HeaderChargeViewModel, PurchaseOrderAmendmentHeaderCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<PurchaseOrderAmendmentHeaderCharge, HeaderChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<PurchaseWaybillViewModel, PurchaseWaybill>();
            Mapper.CreateMap<PurchaseWaybill, PurchaseWaybillViewModel>();

            Mapper.CreateMap<PurchaseOrderAmendmentHeader, PurchaseOrderAmendmentHeaderViewModel>();
            Mapper.CreateMap<PurchaseOrderAmendmentHeaderViewModel, PurchaseOrderAmendmentHeader>();

            Mapper.CreateMap<PurchaseOrderRateAmendmentLine, PurchaseOrderRateAmendmentLineViewModel>();
            Mapper.CreateMap<PurchaseOrderRateAmendmentLineViewModel, PurchaseOrderRateAmendmentLine>();         

            Mapper.CreateMap<PurchaseGoodsReceiptSetting, PurchaseGoodsReceiptSettingsViewModel>();
            Mapper.CreateMap<PurchaseGoodsReceiptSettingsViewModel, PurchaseGoodsReceiptSetting>();

            Mapper.CreateMap<PurchaseInvoiceSetting, PurchaseInvoiceSettingsViewModel>();
            Mapper.CreateMap<PurchaseInvoiceSettingsViewModel, PurchaseInvoiceSetting>();

            Mapper.CreateMap<PurchaseQuotationSetting, PurchaseQuotationSettingsViewModel>();
            Mapper.CreateMap<PurchaseQuotationSettingsViewModel, PurchaseQuotationSetting>();

            Mapper.CreateMap<CalculationFooterViewModel, HeaderChargeViewModel>();
            Mapper.CreateMap<CalculationProductViewModel, LineChargeViewModel>();

            Mapper.CreateMap<HeaderChargeViewModel, PurchaseInvoiceHeaderCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<PurchaseInvoiceHeaderCharge, HeaderChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<LineChargeViewModel, PurchaseInvoiceLineCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<PurchaseInvoiceLineCharge, LineChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<HeaderChargeViewModel, PurchaseQuotationHeaderCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<PurchaseQuotationHeaderCharge, HeaderChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<LineChargeViewModel, PurchaseQuotationLineCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<PurchaseQuotationLineCharge, LineChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<LineChargeViewModel, PurchaseInvoiceReturnLineCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<PurchaseInvoiceReturnLineCharge, LineChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<HeaderChargeViewModel, PurchaseInvoiceReturnHeaderCharge>().ForMember(m => m.Id, x => x.Ignore());
            Mapper.CreateMap<PurchaseInvoiceReturnHeaderCharge, HeaderChargeViewModel>().ForMember(m => m.Id, x => x.Ignore());

            Mapper.CreateMap<PurchaseIndentSetting, PurchaseIndentSettingsViewModel>();
            Mapper.CreateMap<PurchaseIndentSettingsViewModel, PurchaseIndentSetting>();

            Mapper.CreateMap<PurchaseInvoiceReturnHeaderViewModel, PurchaseGoodsReturnHeader>().ForMember(m => m.PurchaseGoodsReturnHeaderId, x => x.Ignore());
            Mapper.CreateMap<PurchaseGoodsReturnHeader, PurchaseInvoiceReturnHeaderViewModel>();

            Mapper.CreateMap<PurchaseInvoiceReturnLineViewModel, PurchaseGoodsReturnLine>();
            Mapper.CreateMap<PurchaseGoodsReturnLine, PurchaseInvoiceReturnLineViewModel>();

            Mapper.CreateMap<PurchaseInvoiceReturnHeader, PurchaseGoodsReturnHeader>().ForMember(m => m.PurchaseGoodsReturnHeaderId, x => x.Ignore());
            Mapper.CreateMap<PurchaseGoodsReturnHeader, PurchaseInvoiceReturnHeader>();

            Mapper.CreateMap<PurchaseInvoiceReturnLine, PurchaseGoodsReturnLine>();
            Mapper.CreateMap<PurchaseGoodsReturnLine, PurchaseInvoiceReturnLine>();

            Mapper.CreateMap<PurchaseInvoiceHeaderViewModel, PurchaseGoodsReceiptHeader>();
            Mapper.CreateMap<PurchaseGoodsReceiptHeader, PurchaseInvoiceHeaderViewModel>();

            Mapper.CreateMap<PurchaseInvoiceLineViewModel, PurchaseGoodsReceiptLine>();
            Mapper.CreateMap<PurchaseGoodsReceiptLine, PurchaseInvoiceLineViewModel>();

            Mapper.CreateMap<HeaderChargeViewModel, HeaderChargeViewModel>();
            Mapper.CreateMap<LineChargeViewModel, LineChargeViewModel>();
        
            Mapper.CreateMap<PurchaseInvoiceHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<PurchaseInvoiceHeader, DocumentUniqueId>();

            Mapper.CreateMap<PurchaseQuotationHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<PurchaseQuotationHeader, DocumentUniqueId>();

            Mapper.CreateMap<PurchaseGoodsReceiptHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<PurchaseGoodsReceiptHeader, DocumentUniqueId>();

            Mapper.CreateMap<PurchaseGoodsReturnHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<PurchaseGoodsReturnHeader, DocumentUniqueId>();

            Mapper.CreateMap<PurchaseIndentCancelHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<PurchaseIndentCancelHeader, DocumentUniqueId>();

            Mapper.CreateMap<PurchaseIndentHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<PurchaseIndentHeader, DocumentUniqueId>();
           
            Mapper.CreateMap<PurchaseInvoiceReturnHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<PurchaseInvoiceReturnHeader, DocumentUniqueId>();

            Mapper.CreateMap<PurchaseOrderCancelHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<PurchaseOrderCancelHeader, DocumentUniqueId>();

            Mapper.CreateMap<PurchaseOrderHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<PurchaseOrderHeader, DocumentUniqueId>();

            Mapper.CreateMap<PurchaseOrderAmendmentHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<PurchaseOrderAmendmentHeader, DocumentUniqueId>();
            

            Mapper.CreateMap<PurchaseInvoiceHeader, PurchaseInvoiceHeader>();
            Mapper.CreateMap<PurchaseQuotationHeader, PurchaseQuotationHeader>();
            Mapper.CreateMap<PurchaseGoodsReceiptHeader, PurchaseGoodsReceiptHeader>();
            Mapper.CreateMap<PurchaseInvoiceLineIndexViewModel, PurchaseInvoiceLine>();
            Mapper.CreateMap<PurchaseGoodsReceiptLineViewModel, PurchaseGoodsReceiptLine>();
            Mapper.CreateMap<PurchaseIndentHeader, PurchaseIndentHeader>();
            Mapper.CreateMap<PurchaseIndentLineViewModel, PurchaseIndentLine>();
            Mapper.CreateMap<PurchaseGoodsReturnHeader, PurchaseGoodsReturnHeader>();
            Mapper.CreateMap<PurchaseGoodsReturnLineIndexViewModel, PurchaseGoodsReturnLine>();
            Mapper.CreateMap<PurchaseOrderCancelHeader, PurchaseOrderCancelHeader>();
            Mapper.CreateMap<PurchaseOrderCancelLineViewModel, PurchaseOrderCancelLine>();
            Mapper.CreateMap<PurchaseInvoiceReturnHeader, PurchaseInvoiceReturnHeader>();
            Mapper.CreateMap<PurchaseInvoiceReturnLineIndexViewModel, PurchaseInvoiceReturnLine>();
            Mapper.CreateMap<PurchaseIndentCancelHeader, PurchaseIndentCancelHeader>();
            Mapper.CreateMap<PurchaseIndentCancelLineViewModel, PurchaseIndentCancelLine>();
            Mapper.CreateMap<PurchaseOrderHeader, PurchaseOrderHeader>();
            Mapper.CreateMap<PurchaseOrderLineViewModel, PurchaseOrderLine>();
            Mapper.CreateMap<PurchaseIndentCancelLine, PurchaseIndentCancelLine>();
            Mapper.CreateMap<PurchaseIndentLine, PurchaseIndentLine>();
            Mapper.CreateMap<PurchaseOrderCancelLine, PurchaseOrderCancelLine>();
            Mapper.CreateMap<PurchaseInvoiceLine, PurchaseInvoiceLine>();
            Mapper.CreateMap<PurchaseInvoiceLineCharge, PurchaseInvoiceLineCharge>();
            Mapper.CreateMap<PurchaseInvoiceHeaderCharge, PurchaseInvoiceHeaderCharge>();
            Mapper.CreateMap<PurchaseQuotationLine, PurchaseQuotationLine>();
            Mapper.CreateMap<PurchaseQuotationLineCharge, PurchaseQuotationLineCharge>();
            Mapper.CreateMap<PurchaseQuotationHeaderCharge, PurchaseQuotationHeaderCharge>();
            Mapper.CreateMap<PurchaseGoodsReceiptLine, PurchaseGoodsReceiptLine>();
            Mapper.CreateMap<PurchaseGoodsReturnLine, PurchaseGoodsReturnLine>();
            Mapper.CreateMap<PurchaseInvoiceReturnLine, PurchaseInvoiceReturnLine>();
            Mapper.CreateMap<PurchaseInvoiceReturnLineCharge, PurchaseInvoiceReturnLineCharge>();
            Mapper.CreateMap<PurchaseInvoiceReturnHeaderCharge, PurchaseInvoiceReturnHeaderCharge>();
            Mapper.CreateMap<PurchaseOrderLine, PurchaseOrderLine>();
            Mapper.CreateMap<PurchaseOrderLineCharge, PurchaseOrderLineCharge>();
            Mapper.CreateMap<PurchaseOrderHeaderCharge, PurchaseOrderHeaderCharge>();
            Mapper.CreateMap<PurchaseOrderRateAmendmentLineCharge, PurchaseOrderRateAmendmentLineCharge>();
            Mapper.CreateMap<PurchaseOrderAmendmentHeaderCharge, PurchaseOrderAmendmentHeaderCharge>();
            Mapper.CreateMap<PurchaseGoodsReceiptSetting, PurchaseGoodsReceiptSetting>();
            Mapper.CreateMap<PurchaseIndentSetting, PurchaseIndentSetting>();
            Mapper.CreateMap<PurchaseInvoiceSetting, PurchaseInvoiceSetting>();
            Mapper.CreateMap<PurchaseQuotationSetting, PurchaseQuotationSetting>();
            Mapper.CreateMap<PurchaseOrderSetting, PurchaseOrderSetting>();

        }
    }
}
