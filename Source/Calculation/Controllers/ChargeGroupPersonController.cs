using System;
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
using AutoMapper;
using System.Xml.Linq;

namespace Web
{
    [Authorize]
    public class ChargeGroupPersonController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        IChargeGroupPersonService _ChargeGroupPersonService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;
        public ChargeGroupPersonController(IChargeGroupPersonService ChargeGroupPersonService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _ChargeGroupPersonService = ChargeGroupPersonService;
            _unitOfWork = unitOfWork;
            _exception = exec;
        }
        public ActionResult ChargeTypeIndex()
        {
            var chargetype = new ChargeTypeService(_unitOfWork).GetPersonChargeTypeList().ToList();
            return View("ChargeTypeIndex", chargetype);
        }
        // GET: /ChargeGroupPersonMaster/

        public ActionResult Index(int id)//ChargeTypeId
        {
            var chargeGroupPerson = _ChargeGroupPersonService.GetChargeGroupPersonList(id);
            ViewBag.id = id;
            ViewBag.Name = new ChargeTypeService(_unitOfWork).Find(id).ChargeTypeName;
            return View(chargeGroupPerson);
        }

        private void PrepareViewBag()
        {
            ViewBag.ChargeTypeList = new ChargeTypeService(_unitOfWork).GetChargeTypeList().ToList();
        }


        // GET: /ChargeGroupPersonMaster/Create

        public ActionResult Create(int id)
        {
            ChargeGroupPerson vm = new ChargeGroupPerson();
            vm.ChargeTypeId = id;
            vm.IsActive = true;
            ViewBag.id = id;
            ViewBag.Name = new ChargeTypeService(_unitOfWork).Find(id).ChargeTypeName;
            return View("Create", vm);
        }

        // POST: /ProductMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(ChargeGroupPerson pt)
        {
            if (ModelState.IsValid)
            {

                if (pt.ChargeGroupPersonId <= 0)
                {

                    pt.CreatedDate = DateTime.Now;
                    pt.ModifiedDate = DateTime.Now;
                    pt.CreatedBy = User.Identity.Name;
                    pt.ModifiedBy = User.Identity.Name;
                    pt.ObjectState = Model.ObjectState.Added;
                    _ChargeGroupPersonService.Create(pt);

                    ActivityLog log = new ActivityLog()
                    {
                        ActivityType = (int)(ActivityTypeContants.Added),
                        CreatedBy = User.Identity.Name,
                        CreatedDate = DateTime.Now,
                        DocId = pt.ChargeGroupPersonId,
                        Narration = "A new ChargeGroupPerson is created with the Id " + pt.ChargeGroupPersonId,
                    };

                    try
                    {
                        _unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        ModelState.AddModelError("", message);
                        ViewBag.id = pt.ChargeTypeId;
                        ViewBag.Name = new ChargeTypeService(_unitOfWork).Find(pt.ChargeTypeId).ChargeTypeName;
                        return View("Create", pt);

                    }

                    ViewBag.id = pt.ChargeTypeId;
                    ViewBag.Name = new ChargeTypeService(_unitOfWork).Find(pt.ChargeTypeId).ChargeTypeName;
                    return RedirectToAction("Create", new { id = pt.ChargeTypeId }).Success("Data saved successfully");

                }
                else
                {
                    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

                    ChargeGroupPerson temp = _ChargeGroupPersonService.Find(pt.ChargeGroupPersonId);

                    ChargeGroupPerson ExRec = Mapper.Map<ChargeGroupPerson>(temp);

                    temp.ChargeGroupPersonName = pt.ChargeGroupPersonName;
                    temp.IsActive = pt.IsActive;
                    temp.ModifiedDate = DateTime.Now;
                    temp.ModifiedBy = User.Identity.Name;
                    temp.ObjectState = Model.ObjectState.Modified;
                    _ChargeGroupPersonService.Update(temp);

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
                        ViewBag.id = pt.ChargeTypeId;
                        ViewBag.Name = new ChargeTypeService(_unitOfWork).Find(pt.ChargeTypeId).ChargeTypeName;
                        return View("Create", pt);
                    }

                    LogActivity.LogActivityDetail(new DocumentTypeService(_unitOfWork).Find(MasterDocTypeConstants.ChargeGroupPerson).DocumentTypeId,
                    temp.ChargeGroupPersonId,
                    null,
                    (int)ActivityTypeContants.Modified,
                    "",
                    User.Identity.Name, temp.ChargeGroupPersonName, Modifications);

                    ViewBag.id = pt.ChargeTypeId;
                    ViewBag.Name = new ChargeTypeService(_unitOfWork).Find(pt.ChargeTypeId).ChargeTypeName;
                    return RedirectToAction("Index", new { id = pt.ChargeTypeId }).Success("Data saved successfully");


                }

            }
            ViewBag.id = pt.ChargeTypeId;
            ViewBag.Name = new ChargeTypeService(_unitOfWork).Find(pt.ChargeTypeId).ChargeTypeName;
            return View("Create", pt);
        }


        // GET: /ProductMaster/Edit/5

        public ActionResult Edit(int id)
        {
            ChargeGroupPerson pt = _ChargeGroupPersonService.Find(id);
            if (pt == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = pt.ChargeTypeId;
            ViewBag.Name = new ChargeTypeService(_unitOfWork).Find(pt.ChargeTypeId).ChargeTypeName;
            return View("Create", pt);
        }


        // GET: /ProductMaster/Delete/5

        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChargeGroupPerson ChargeGroupPerson = db.ChargeGroupPerson.Find(id);
            if (ChargeGroupPerson == null)
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
                var temp = _ChargeGroupPersonService.Find(vm.id);
                LogList.Add(new LogTypeViewModel
                {
                    ExObj = temp,
                });
                _ChargeGroupPersonService.Delete(vm.id);

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

                LogActivity.LogActivityDetail(new DocumentTypeService(_unitOfWork).Find(MasterDocTypeConstants.ChargeGroupPerson).DocumentTypeId,
                temp.ChargeGroupPersonId,
                null,
                (int)ActivityTypeContants.Deleted,
                vm.Reason,
                User.Identity.Name,
                temp.ChargeGroupPersonName, Modifications);


                return Json(new { success = true });

            }
            return PartialView("_Reason", vm);
        }


        [HttpGet]
        public ActionResult NextPage(int id, int ctypeid)//CurrentHeaderId
        {
            var nextId = _ChargeGroupPersonService.NextId(id, ctypeid);
            return RedirectToAction("Edit", new { id = nextId });
        }
        [HttpGet]
        public ActionResult PrevPage(int id, int ctypeid)//CurrentHeaderId
        {
            var nextId = _ChargeGroupPersonService.PrevId(id, ctypeid);
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
