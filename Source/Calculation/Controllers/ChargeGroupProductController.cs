﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.Models;
using Data.Models;
using Service;
using Data.Infrastructure;
using Presentation;
using Presentation.ViewModels;
using Core.Common;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using Model.ViewModel;
using System.Xml.Linq;
using AutoMapper;

namespace Web
{
    [Authorize]
    public class ChargeGroupProductController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        IChargeGroupProductService _ChargeGroupProductService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;
        public ChargeGroupProductController(IChargeGroupProductService ChargeGroupProductService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _ChargeGroupProductService = ChargeGroupProductService;
            _unitOfWork = unitOfWork;
            _exception = exec;
        }
        public ActionResult ChargeTypeIndex()
        {
            var chargetype = new ChargeTypeService(_unitOfWork).GetProductChargeTypeList().ToList();
            return View("ChargeTypeIndex", chargetype);
        }

        // GET: /ChargeGroupProductMaster/

        public ActionResult Index()
        {
            var ChargeGroupProduct = _ChargeGroupProductService.GetChargeGroupProductList();
            return View(ChargeGroupProduct);
        }


        private void PrepareViewBag()
        {
            ViewBag.ChargeTypeList = new ChargeTypeService(_unitOfWork).GetChargeTypeList().ToList();
        }


        // GET: /ChargeGroupProductMaster/Create

        public ActionResult Create()
        {
            ChargeGroupProduct vm = new ChargeGroupProduct();
            vm.IsActive = true;
            return View("Create", vm);
        }

        // POST: /ProductMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(ChargeGroupProduct pt)
        {
            if (ModelState.IsValid)
            {

                if (pt.ChargeGroupProductId <= 0)
                {

                    pt.CreatedDate = DateTime.Now;
                    pt.ModifiedDate = DateTime.Now;
                    pt.CreatedBy = User.Identity.Name;
                    pt.ModifiedBy = User.Identity.Name;
                    pt.ObjectState = Model.ObjectState.Added;
                    _ChargeGroupProductService.Create(pt);

                    ActivityLog log = new ActivityLog()
                    {
                        ActivityType = (int)(ActivityTypeContants.Added),
                        CreatedBy = User.Identity.Name,
                        CreatedDate = DateTime.Now,
                        DocId = pt.ChargeGroupProductId,
                        Narration = "A new ChargeGroupProduct is created with the Id " + pt.ChargeGroupProductId,
                    };

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

                    return RedirectToAction("Create").Success("Data saved successfully");

                }
                else
                {
                    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();
                    ChargeGroupProduct temp = _ChargeGroupProductService.Find(pt.ChargeGroupProductId);

                    ChargeGroupProduct ExRec = Mapper.Map<ChargeGroupProduct>(temp);

                    temp.ChargeGroupProductName = pt.ChargeGroupProductName;
                    temp.IsActive = pt.IsActive;
                    temp.ModifiedDate = DateTime.Now;
                    temp.ModifiedBy = User.Identity.Name;
                    temp.ObjectState = Model.ObjectState.Modified;
                    _ChargeGroupProductService.Update(temp);

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

                    LogActivity.LogActivityDetail(new DocumentTypeService(_unitOfWork).Find(MasterDocTypeConstants.ChargeGroupProduct).DocumentTypeId,
                    temp.ChargeGroupProductId,
                    null,
                    (int)ActivityTypeContants.Modified,
                    "",
                    User.Identity.Name, temp.ChargeGroupProductName, Modifications);

                    return RedirectToAction("Index").Success("Data saved successfully");
                }
            }
            return View("Create", pt);
        }


        // GET: /ProductMaster/Edit/5

        public ActionResult Edit(int id)
        {
            ChargeGroupProduct pt = _ChargeGroupProductService.Find(id);
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
            ChargeGroupProduct ChargeGroupProduct = db.ChargeGroupProduct.Find(id);
            if (ChargeGroupProduct == null)
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
                var temp = _ChargeGroupProductService.Find(vm.id);

                LogList.Add(new LogTypeViewModel
                {
                    ExObj = temp,
                });

                _ChargeGroupProductService.Delete(vm.id);
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

                LogActivity.LogActivityDetail(new DocumentTypeService(_unitOfWork).Find(MasterDocTypeConstants.ChargeGroupProduct).DocumentTypeId,
                 temp.ChargeGroupProductId,
                 null,
                 (int)ActivityTypeContants.Deleted,
                 vm.Reason,
                 User.Identity.Name,
                 temp.ChargeGroupProductName, Modifications);

                return Json(new { success = true });

            }
            return PartialView("_Reason", vm);
        }


        [HttpGet]
        public ActionResult NextPage(int id, int ctypeid)//CurrentHeaderId-ChargetypeId
        {
            var nextId = _ChargeGroupProductService.NextId(id, ctypeid);
            return RedirectToAction("Edit", new { id = nextId });
        }
        [HttpGet]
        public ActionResult PrevPage(int id, int ctypeid)//CurrentHeaderId
        {
            var nextId = _ChargeGroupProductService.PrevId(id, ctypeid);
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
            //To Be Implemented
            return View("~/Views/Shared/UnderImplementation.cshtml");
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
