using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Model.Models;
using Data.Models;
using Service;
using Data.Infrastructure;
using Presentation.ViewModels;
using Presentation;
using Core.Common;
using Model.ViewModel;
using AutoMapper;
using Presentation.Helper;
using System.Configuration;
using System.Xml.Linq;
using DocumentEvents;
using CustomEventArgs;
using JobReceiveQADocumentEvents;
using Reports.Reports;
using Model.ViewModels;
using Reports.Controllers;

namespace Web
{
    [Authorize]
    public class JobReceiveQAAttributeController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        List<string> UserRoles = new List<string>();
        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        private bool EventException = false;

        bool TimePlanValidation = true;
        string ExceptionMsg = "";
        bool Continue = true;

        IUnitOfWork _unitOfWork;
        IJobReceiveQAAttributeService _JobReceiveQAAttributeService;
        IExceptionHandlingService _exception;
        public JobReceiveQAAttributeController(IExceptionHandlingService exec, IUnitOfWork uow)
        {
            _unitOfWork = uow;
            _JobReceiveQAAttributeService = new JobReceiveQAAttributeService(db);
            _exception = exec;
            if (!JobReceiveQAEvents.Initialized)
            {
                JobReceiveQAEvents Obj = new JobReceiveQAEvents();
            }

            UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;
        }


        public void PrepareViewBag(int id)
        {
            ViewBag.DeliveryUnitList = new UnitService(_unitOfWork).GetUnitList().ToList();

            ViewBag.Name = db.DocumentType.Find(id).DocumentTypeName;
        }

        // GET: /JobReceiveQAAttributeMaster/

        public ActionResult DocumentTypeIndex(int id)//DocumentCategoryId
        {
            var p = new JobReceiveQAHeaderService(db).FindByDocumentCategory(id).ToList();

            if (p != null)
            {
                if (p.Count == 1)
                    return RedirectToAction("Index", new { id = p.FirstOrDefault().DocumentTypeId });
            }

            return View("DocumentTypeList", p);
        }

        public ActionResult Index(int id)//DocumentTypeId
        {
            var JobReceiveQAAttribute = _JobReceiveQAAttributeService.GetJobReceiveQAAttributeList(id, User.Identity.Name);
            ViewBag.Name = db.DocumentType.Find(id).DocumentTypeName;
            ViewBag.id = id;
            return View(JobReceiveQAAttribute);
        }


        public ActionResult Create(int id, int DocTypeId)//JobReceiveLineId
        {
            JobReceiveQAAttributeViewModel vm = new JobReceiveQAAttributeViewModel();

            vm.DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            vm.SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            vm.CreatedDate = DateTime.Now;

            //Getting Settings
            var settings = new JobReceiveQASettingsService(db).GetJobReceiveQASettingsForDocument(id, vm.DivisionId, vm.SiteId);

            if (settings == null && UserRoles.Contains("Admin"))
            {
                return RedirectToAction("Create", "JobReceiveQASettings", new { id = id }).Warning("Please create job Inspection settings");
            }
            else if (settings == null && !UserRoles.Contains("Admin"))
            {
                return View("~/Views/Shared/InValidSettings.cshtml");
            }
            vm.JobReceiveQASettings = Mapper.Map<JobReceiveQASettings, JobReceiveQASettingsViewModel>(settings);

            vm.ProcessId = settings.ProcessId;
            vm.DocDate = DateTime.Now;
            vm.DocTypeId = id;
            vm.DocNo = new  JobReceiveQAHeaderService(db).FGetNewDocNo("DocNo", ConfigurationManager.AppSettings["DataBaseSchema"] + ".JobReceiveQAHeaders", vm.DocTypeId, vm.DocDate, vm.DivisionId, vm.SiteId);
            PrepareViewBag(DocTypeId);
            ViewBag.Mode = "Add"; 
            return View("Create", vm);
        }

        // POST: /ProductMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(JobReceiveQAAttributeViewModel vm)
        {
            if (ModelState.IsValid)
            {
                #region CreateRecord
                if (vm.JobReceiveQAAttributeId <= 0)
                {
                    _JobReceiveQAAttributeService.Create(vm, User.Identity.Name);

                    try
                    {
                        db.SaveChanges();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                        PrepareViewBag(vm.DocTypeId);
                        ViewBag.Mode = "Add";
                        return View("Create", vm);
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = vm.DocTypeId,
                        DocId = vm.JobReceiveQAAttributeId,
                        ActivityType = (int)ActivityTypeContants.Added,
                        DocDate = vm.DocDate,
                        DocNo = vm.DocNo,
                        DocStatus = vm.Status,
                    }));


                    return RedirectToAction("Index", new { id = vm.JobReceiveLineId }).Success("Data saved successfully");
                }
                #endregion

                

            }
            PrepareViewBag(vm.DocTypeId);
            ViewBag.Mode = "Add";
            return View("Create", vm);
        }






        protected override void Dispose(bool disposing)
        {
            if (!string.IsNullOrEmpty((string)TempData["CSEXC"]))
            {
                CookieGenerator.CreateNotificationCookie(NotificationTypeConstants.Danger, (string)TempData["CSEXC"]);
                TempData.Remove("CSEXC");
            }
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
