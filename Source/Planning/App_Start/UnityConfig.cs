using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Data.Infrastructure;
using Model.Models;
using Service;
using AutoMapper;
using Model.ViewModel;
using Planning.Controllers;
using Data.Models;

namespace Planning.App_Start
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

            container.RegisterType<IRepository<MaterialPlanSettings>, Repository<MaterialPlanSettings>>();
            container.RegisterType<IMaterialPlanSettingsService, MaterialPlanSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<MaterialPlanHeader>, Repository<MaterialPlanHeader>>();
            container.RegisterType<IMaterialPlanHeaderService, MaterialPlanHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<MaterialPlanForSaleOrderLine>, Repository<MaterialPlanForSaleOrderLine>>();
            container.RegisterType<IMaterialPlanForSaleOrderLineService, MaterialPlanForSaleOrderLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<MaterialPlanForProdOrder>, Repository<MaterialPlanForProdOrder>>();
            container.RegisterType<IMaterialPlanForProdOrderService, MaterialPlanForProdOrderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<MaterialPlanCancelForProdOrder>, Repository<MaterialPlanCancelForProdOrder>>();
            container.RegisterType<IMaterialPlanCancelForProdOrderService, MaterialPlanCancelForProdOrderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<MaterialPlanLine>, Repository<MaterialPlanLine>>();
            container.RegisterType<IMaterialPlanLineService, MaterialPlanLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<MaterialPlanCancelHeader>, Repository<MaterialPlanCancelHeader>>();
            container.RegisterType<IMaterialPlanCancelHeaderService, MaterialPlanCancelHeaderService>(new PerRequestLifetimeManager());
           
            container.RegisterType<IRepository<MaterialPlanCancelForProdOrderLine>, Repository<MaterialPlanCancelForProdOrderLine>>();
            container.RegisterType<IMaterialPlanCancelForProdOrderLineService, MaterialPlanCancelForProdOrderLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<MaterialPlanCancelLine>, Repository<MaterialPlanCancelLine>>();
            container.RegisterType<IMaterialPlanCancelLineService, MaterialPlanCancelLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IModuleService, ModuleService>(new PerRequestLifetimeManager());

            container.RegisterType<ISubModuleService, SubModuleService>(new PerRequestLifetimeManager());

            container.RegisterType<IExceptionHandlingService, ExceptionHandlingService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProdOrderHeader>, Repository<ProdOrderHeader>>();
            container.RegisterType<IProdOrderHeaderService, ProdOrderHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProdOrderLine>, Repository<ProdOrderLine>>();
            container.RegisterType<IProdOrderLineService, ProdOrderLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProdOrderCancelHeader>, Repository<ProdOrderCancelHeader>>();
            container.RegisterType<IProdOrderCancelHeaderService, ProdOrderCancelHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<ProdOrderCancelLine>, Repository<ProdOrderCancelLine>>();
            container.RegisterType<IProdOrderCancelLineService, ProdOrderCancelLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IActivityLogService, ActivityLogService>(new PerRequestLifetimeManager());

            container.RegisterType<IDuplicateDocumentCheckService, DuplicateDocumentCheckService>(new PerRequestLifetimeManager());

            container.RegisterType<IReportLineService, ReportLineService>(new PerRequestLifetimeManager());

            container.RegisterType<IProdOrderSettingsService, ProdOrderSettingsService>(new PerRequestLifetimeManager());

            container.RegisterType<IMaterialPlanCancelForSaleOrderService, MaterialPlanCancelForSaleOrderService>(new PerRequestLifetimeManager());

            Mapper.CreateMap<ProdOrderSettingsViewModel, ProdOrderSettings>();
            Mapper.CreateMap<ProdOrderSettings, ProdOrderSettingsViewModel>();

            Mapper.CreateMap<MaterialPlanHeader, MaterialPlanHeaderViewModel>();
            Mapper.CreateMap<MaterialPlanHeaderViewModel, MaterialPlanHeader>();

            Mapper.CreateMap<MaterialPlanCancelHeader, MaterialPlanCancelHeaderViewModel>();
            Mapper.CreateMap<MaterialPlanCancelHeaderViewModel, MaterialPlanCancelHeader>();

            Mapper.CreateMap<MaterialPlanLineViewModel, MaterialPlanLine>();
            Mapper.CreateMap<MaterialPlanLine, MaterialPlanLineViewModel>();

            Mapper.CreateMap<MaterialPlanSettings, MaterialPlanSettingsViewModel>();
            Mapper.CreateMap<MaterialPlanSettingsViewModel, MaterialPlanSettings>();

            Mapper.CreateMap<ProdOrderLine, ProdOrderLineViewModel>();
            Mapper.CreateMap<ProdOrderLineViewModel, ProdOrderLine>();

            Mapper.CreateMap<ProdOrderHeader, ProdOrderHeaderViewModel>();
            Mapper.CreateMap<ProdOrderHeaderViewModel, ProdOrderHeader>();

            Mapper.CreateMap<ProdOrderCancelHeader, ProdOrderCancelHeaderViewModel>();
            Mapper.CreateMap<ProdOrderCancelHeaderViewModel, ProdOrderCancelHeader>();

            Mapper.CreateMap<HeaderChargeViewModel, HeaderChargeViewModel>();
            Mapper.CreateMap<LineChargeViewModel, LineChargeViewModel>();

            Mapper.CreateMap<MaterialPlanHeader, DocumentUniqueId>();
            Mapper.CreateMap<MaterialPlanHeaderViewModel, DocumentUniqueId>();

            Mapper.CreateMap<MaterialPlanCancelHeader, DocumentUniqueId>();
            Mapper.CreateMap<MaterialPlanCancelHeaderViewModel, DocumentUniqueId>();

            Mapper.CreateMap<ProdOrderCancelHeader, DocumentUniqueId>();
            Mapper.CreateMap<ProdOrderCancelHeaderViewModel, DocumentUniqueId>();

            Mapper.CreateMap<ProdOrderHeader, DocumentUniqueId>();
            Mapper.CreateMap<ProdOrderHeaderViewModel, DocumentUniqueId>();

            Mapper.CreateMap<ProdOrderHeader, ProdOrderHeader>();
            Mapper.CreateMap<ProdOrderLineViewModel, ProdOrderLine>();
            Mapper.CreateMap<ProdOrderCancelHeader, ProdOrderCancelHeader>();
            Mapper.CreateMap<ProdOrderCancelLineViewModel, ProdOrderCancelLine>();
            Mapper.CreateMap<ProdOrderCancelLine, ProdOrderCancelLine>();
            Mapper.CreateMap<MaterialPlanHeader, MaterialPlanHeader>();
            Mapper.CreateMap<MaterialPlanCancelHeader, MaterialPlanCancelHeader>();
            Mapper.CreateMap<MaterialPlanLine, MaterialPlanLine>();
            Mapper.CreateMap<ProdOrderLine, ProdOrderLine>();
            Mapper.CreateMap<PurchaseIndentLine, PurchaseIndentLine>();
            Mapper.CreateMap<MaterialPlanForSaleOrder, MaterialPlanForSaleOrder>();
            Mapper.CreateMap<MaterialPlanForProdOrderLine, MaterialPlanForProdOrderLine>();
            Mapper.CreateMap<PurchaseIndentHeader, PurchaseIndentHeader>();
            Mapper.CreateMap<MaterialPlanForProdOrder, MaterialPlanForProdOrder>();
            Mapper.CreateMap<MaterialPlanSettings, MaterialPlanSettings>();
            Mapper.CreateMap<ProdOrderSettings,ProdOrderSettings>();

        }
    }
}
