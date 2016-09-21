using System;
using Microsoft.Practices.Unity;
using Service;
using AutoMapper;
using Model.Tasks.ViewModel;
using Model.Tasks.Models;
using Data.Infrastructure;
using Components.Logging;
using Data.Models;
using Components.ExceptionHandlers;
using Model.DatabaseViews;
using Models.Login.Models;

namespace TaskManagement.App_Start
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

            //container.RegisterType<AccountController>(new InjectionConstructor());
            //container.RegisterType<ApplicationDbContext, ApplicationDbContext>("New");

            container.RegisterType<IDataContext, LoginApplicationDbContext>(new PerRequestLifetimeManager());
            //container.RegisterType<IUnitOfWorkForService, UnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager());




            container.RegisterType<IComboHelpListService, ComboHelpListService>(new PerRequestLifetimeManager());
            container.RegisterType<IDARService, DARService>(new PerRequestLifetimeManager());
            container.RegisterType<IProjectService, ProjectService>(new PerRequestLifetimeManager());
            container.RegisterType<ITasksService, TasksService>(new PerRequestLifetimeManager());
            container.RegisterType<IUserTeamService, UserTeamService>(new PerRequestLifetimeManager());



            container.RegisterType<IRepository<Project>, Repository<Project>>(new PerRequestLifetimeManager());
            container.RegisterType<IRepository<Tasks>, Repository<Tasks>>(new PerRequestLifetimeManager());
            container.RegisterType<IRepository<DAR>, Repository<DAR>>(new PerRequestLifetimeManager());
            container.RegisterType<ILogger, LogActivity>(new PerRequestLifetimeManager());
            container.RegisterType<IRepository<UserTeam>, Repository<UserTeam>>(new PerRequestLifetimeManager());
            container.RegisterType<IRepository<_Users>, Repository<_Users>>(new PerRequestLifetimeManager());
            container.RegisterType<IModificationCheck, ModificationCheck>(new PerRequestLifetimeManager());
            container.RegisterType<IExceptionHandler, ExceptionHandler>(new PerRequestLifetimeManager());


            Mapper.CreateMap<TasksViewModel, Tasks>();
            Mapper.CreateMap<Tasks, TasksViewModel>();

            Mapper.CreateMap<Tasks, Tasks>();
            Mapper.CreateMap<Project, Project>();
            Mapper.CreateMap<ProjectViewModel, Project>();
            Mapper.CreateMap<Project, ProjectViewModel>();
            Mapper.CreateMap<DAR, DAR>();

            Mapper.CreateMap<DAR, DARViewModel>();
            Mapper.CreateMap<DARViewModel, DAR>();

            Mapper.CreateMap<UserTeam, UserTeamViewModel>();
            Mapper.CreateMap<UserTeamViewModel, UserTeam>();
        }
    }
}
