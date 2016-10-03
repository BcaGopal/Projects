using System;
using Microsoft.Practices.Unity;
using Infrastructure.IO;
using Data;
using Service;
using AutoMapper;
using Models.Company.Models;
using Models.Company.ViewModels;
using AdminSetup.Models.Models;
using AdminSetup.Models.ViewModels;
using Components.Logging;

namespace Module.App_Start
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

            //container.RegisterType<ApplicationDbContext, ApplicationDbContext>("New");

            container.RegisterType<IDataContext, ApplicationDbContext>(new PerRequestLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager());
           // container.RegisterType<ILogger, LogActivity>(new PerRequestLifetimeManager());

            container.RegisterType<IModuleService, ModuleService>(new PerRequestLifetimeManager());
            container.RegisterType<ISubModuleService, SubModuleService>(new PerRequestLifetimeManager());
            container.RegisterType<IMenuService, MenuService>(new PerRequestLifetimeManager());
            container.RegisterType<IUserBookMarkService, UserBookMarkService>(new PerRequestLifetimeManager());
            container.RegisterType<INotificationService, NotificationService>(new PerRequestLifetimeManager());
            container.RegisterType<ISiteSelectionService, SiteSelectionService>(new PerRequestLifetimeManager());
            container.RegisterType<IUserRolesService, UserRolesService>(new PerRequestLifetimeManager());
            container.RegisterType<IRolesControllerActionService, RolesControllerActionService>(new PerRequestLifetimeManager());
            container.RegisterType<ILogger, LogActivity>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<MenuModule>, Repository<MenuModule>>();
            container.RegisterType<IRepository<ActivityLog>, Repository<ActivityLog>>();
            container.RegisterType<IRepository<MenuSubModule>, Repository<MenuSubModule>>();
            container.RegisterType<IRepository<Menu>, Repository<Menu>>();
            container.RegisterType<IRepository<UserBookMark>, Repository<UserBookMark>>();
            container.RegisterType<IRepository<RolesMenu>, Repository<RolesMenu>>();
            container.RegisterType<IRepository<ControllerAction>, Repository<ControllerAction>>();
            container.RegisterType<IRepository<RolesControllerAction>, Repository<RolesControllerAction>>();


            Mapper.Initialize(cfg => { cfg.CreateMap<Site, SiteViewModel>();
            cfg.CreateMap<MvcController, MvcControllerViewModel>();
            cfg.CreateMap<MvcControllerViewModel, MvcController>();
            cfg.CreateMap<DocumentType, DocumentTypeViewModel>();
            cfg.CreateMap<RolesDivisionViewModel, RolesDivision>();
            cfg.CreateMap<RolesDivision, RolesDivisionViewModel>();
            cfg.CreateMap<RolesSiteViewModel, RolesSite>();
            cfg.CreateMap<RolesSite, RolesSiteViewModel>();
            });
            //Mapper.Initialize(cfg => cfg.CreateMap<MvcController, MvcControllerViewModel>());
            //Mapper.Initialize(cfg => cfg.CreateMap<MvcControllerViewModel, MvcController>());
            //Mapper.Initialize(cfg => cfg.CreateMap<DocumentType, DocumentTypeViewModel>());
            //Mapper.Initialize(cfg => cfg.CreateMap<RolesDivisionViewModel, RolesDivision>());
            //Mapper.Initialize(cfg => cfg.CreateMap<RolesDivision, RolesDivisionViewModel>());
            //Mapper.Initialize(cfg => cfg.CreateMap<RolesSiteViewModel, RolesSite>());
            //Mapper.Initialize(cfg => cfg.CreateMap<RolesSite, RolesSiteViewModel>());

        }
    }
}
