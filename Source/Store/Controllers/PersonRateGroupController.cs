﻿using System;
using System.Collections.Generic;
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
using System.Xml.Linq;

namespace Web
{
    [Authorize]
    public class PersonRateGroupController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        IPersonRateGroupService _PersonRateGroupService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();
        public PersonRateGroupController(IPersonRateGroupService PersonRateGroupService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _PersonRateGroupService = PersonRateGroupService;
            _unitOfWork = unitOfWork;
            _exception = exec;

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;
        }
        // GET: /ProductMaster/

        public ActionResult Index()
        {
            var PersonRateGroup = _PersonRateGroupService.GetPersonRateGroupList();
            return View(PersonRateGroup);
        }


        // GET: /ProductMaster/Create

        public ActionResult Create()
        {
            PersonRateGroup vm = new PersonRateGroup();
            vm.IsActive = true;
            return View("Create", vm);
        }

        // POST: /ProductMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(PersonRateGroup vm)
        {
            PersonRateGroup pt = vm;


            if (ModelState.IsValid)
            {

                if (vm.PersonRateGroupId <= 0)
                {
                    int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
                    int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];


                    pt.SiteId = SiteId;
                    pt.DivisionId = DivisionId;
                    pt.CreatedDate = DateTime.Now;
                    pt.ModifiedDate = DateTime.Now;
                    pt.CreatedBy = User.Identity.Name;
                    pt.ModifiedBy = User.Identity.Name;
                    pt.ObjectState = Model.ObjectState.Added;
                    _PersonRateGroupService.Create(pt);

                    try
                    {
                        _unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        ModelState.AddModelError("", message);
                        return View("Create", vm);
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = new DocumentTypeService(_unitOfWork).Find(MasterDocTypeConstants.PersonRateGroup).DocumentTypeId,
                        DocId = pt.PersonRateGroupId,
                        ActivityType = (int)ActivityTypeContants.Added,
                    }));

                    return RedirectToAction("Create").Success("Data saved successfully");
                }

                else
                {
                    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();
                    PersonRateGroup temp = _PersonRateGroupService.Find(pt.PersonRateGroupId);

                    PersonRateGroup ExRec = Mapper.Map<PersonRateGroup>(temp);

                    temp.PersonRateGroupName = pt.PersonRateGroupName;
                    temp.Processes = pt.Processes;
                    temp.IsActive = pt.IsActive;
                    temp.ModifiedDate = DateTime.Now;
                    temp.ModifiedBy = User.Identity.Name;
                    temp.ObjectState = Model.ObjectState.Modified;
                    _PersonRateGroupService.Update(temp);

                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = ExRec,
                        Obj = temp,
                    });
                    XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);

                    try
                    {
                        _unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        ModelState.AddModelError("", message);
                        return View("Create", pt);
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = new DocumentTypeService(_unitOfWork).Find(MasterDocTypeConstants.PersonRateGroup).DocumentTypeId,
                        DocId = temp.PersonRateGroupId,
                        ActivityType = (int)ActivityTypeContants.Modified,
                        xEModifications = Modifications,
                    }));

                    return RedirectToAction("Index").Success("Data saved successfully");

                }
            }
            return View("Create", vm);
        }


        // GET: /ProductMaster/Edit/5

        public ActionResult Edit(int id)
        {
            PersonRateGroup pt = _PersonRateGroupService.Find(id);
            if (pt == null)
            {
                return HttpNotFound();
            }
            return View("Create", pt);
        }

        // GET: /ProductMaster/Delete/5

        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonRateGroup PersonRateGroup = _PersonRateGroupService.Find(id);

            if (PersonRateGroup == null)
            {
                return HttpNotFound();
            }

            ReasonViewModel vm = new ReasonViewModel()
            {
                id = id,
            };

            return PartialView("_Reason", vm);
        }

        // POST: /ProductMaster/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(ReasonViewModel vm)
        {
            List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();
            if (ModelState.IsValid)
            {
                var temp = _PersonRateGroupService.Find(vm.id);

                LogList.Add(new LogTypeViewModel
                {
                    ExObj = temp,
                });

                _PersonRateGroupService.Delete(vm.id);
                XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);
                try
                {
                    _unitOfWork.Save();
                }

                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    ModelState.AddModelError("", message);
                    return PartialView("_Reason", vm);
                }

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = new DocumentTypeService(_unitOfWork).Find(MasterDocTypeConstants.PersonRateGroup).DocumentTypeId,
                    DocId = temp.PersonRateGroupId,
                    ActivityType = (int)ActivityTypeContants.Deleted,
                    UserRemark = vm.Reason,
                    xEModifications = Modifications,
                }));

                return Json(new { success = true });

            }
            return PartialView("_Reason", vm);
        }


        [HttpGet]
        public ActionResult NextPage(int id)//CurrentHeaderId
        {
            var nextId = _PersonRateGroupService.NextId(id);
            return RedirectToAction("Edit", new { id = nextId });
        }
        [HttpGet]
        public ActionResult PrevPage(int id)//CurrentHeaderId
        {
            var nextId = _PersonRateGroupService.PrevId(id);
            return RedirectToAction("Edit", new { id = nextId });
        }

        [HttpGet]
        public ActionResult History()
        {
            //To Be Implemented
            return View("~/Views/Shared/UnderImplementation.cshtml");
        }
        [HttpGet]
        public ActionResult Print()
        {
            //To Be Implemented
            return View("~/Views/Shared/UnderImplementation.cshtml");
        }
        [HttpGet]
        public ActionResult Email()
        {
            //To Be Implemented
            return View("~/Views/Shared/UnderImplementation.cshtml");
        }

        [HttpGet]
        public ActionResult Report()
        {

            DocumentType Dt = new DocumentType();
            Dt = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.PersonRateGroup);

            return Redirect((string)System.Configuration.ConfigurationManager.AppSettings["CustomizeDomain"] + "/Report_ReportPrint/ReportPrint/?MenuId=" + Dt.ReportMenuId);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
