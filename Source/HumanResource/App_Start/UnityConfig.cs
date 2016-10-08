using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Service;
using HumanResource.Controllers;
using Data.Models;
using Data.Infrastructure;
using AutoMapper;
using Model.Models;
using Model.ViewModels;
using Model.ViewModel;

namespace HumanResource.App_Start
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

            container.RegisterType<IDepartmentService, DepartmentService>(new PerRequestLifetimeManager());
            container.RegisterType<IDesignationService, DesignationService>(new PerRequestLifetimeManager());
            container.RegisterType<IBusinessEntityService, BusinessEntityService>(new PerRequestLifetimeManager());

            container.RegisterType<IPersonService, PersonService>(new PerRequestLifetimeManager());
            container.RegisterType<IPersonAddressService, PersonAddressService>(new PerRequestLifetimeManager());

            container.RegisterType<IAccountService, AccountService>(new PerRequestLifetimeManager());
            container.RegisterType<IPersonProcessService, PersonProcessService>(new PerRequestLifetimeManager());

            container.RegisterType<IPersonRegistrationService, PersonRegistrationService>(new PerRequestLifetimeManager());
            container.RegisterType<IActivityLogService, ActivityLogService>(new PerRequestLifetimeManager());

            container.RegisterType<IPersonBankAccountService, PersonBankAccountService>(new PerRequestLifetimeManager());

            container.RegisterType<IPersonContactService, PersonContactService>(new PerRequestLifetimeManager());
            container.RegisterType<IDuplicateDocumentCheckService, DuplicateDocumentCheckService>(new PerRequestLifetimeManager());

            container.RegisterType<ILeaveTypeServices, LeaveTypeServices>(new PerRequestLifetimeManager());
            container.RegisterType<IAttendanceHeaderService, AttendanceHeaderService>(new PerRequestLifetimeManager());
            container.RegisterType<IOverTimeApplicationHeaderService, OverTimeApplicationHeaderService>(new PerRequestLifetimeManager());

            //Registering Mappers::


            Mapper.CreateMap<Person, EmployeeViewModel>();
            Mapper.CreateMap<EmployeeViewModel, Person>();

            Mapper.CreateMap<BusinessEntity, EmployeeViewModel>();
            Mapper.CreateMap<EmployeeViewModel, BusinessEntity>();

            Mapper.CreateMap<Employee, EmployeeViewModel>();
            Mapper.CreateMap<EmployeeViewModel, Employee>();

            Mapper.CreateMap<PersonAddress, EmployeeViewModel>();
            Mapper.CreateMap<EmployeeViewModel, PersonAddress>();

            Mapper.CreateMap<LedgerAccount, EmployeeViewModel>();
            Mapper.CreateMap<EmployeeViewModel, LedgerAccount>();

            Mapper.CreateMap<PurchaseInvoiceHeader, PurchaseInvoiceHeaderViewModel>();
            Mapper.CreateMap<PurchaseInvoiceHeaderViewModel, PurchaseInvoiceHeader>();

            Mapper.CreateMap<PersonContactViewModel, PersonContact>();
            Mapper.CreateMap<PersonContact, PersonContactViewModel>();

            Mapper.CreateMap<HeaderChargeViewModel, HeaderChargeViewModel>();
            Mapper.CreateMap<LineChargeViewModel, LineChargeViewModel>();

            Mapper.CreateMap<Department, Department>();
            Mapper.CreateMap<Designation, Designation>();
            Mapper.CreateMap<LeaveType, LeaveType>();
            Mapper.CreateMap<Person, Person>();
            Mapper.CreateMap<PersonContact, PersonContact>();


            Mapper.CreateMap<AttendanceHeaderViewModel, AttendanceHeader>();

            Mapper.CreateMap<AttendanceHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<AttendanceHeader, DocumentUniqueId>();
            Mapper.CreateMap<AttendanceHeader, AttendanceHeader>();

            Mapper.CreateMap<OverTimeApplicationHeaderViewModel, OverTimeApplicationHeader>();
            Mapper.CreateMap<OverTimeApplicationHeader, DocumentUniqueId>();
            Mapper.CreateMap<OverTimeApplicationHeaderViewModel, DocumentUniqueId>();
            Mapper.CreateMap<OverTimeApplicationHeader, OverTimeApplicationHeader>();
        }
    }
}
