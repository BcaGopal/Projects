using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Surya.India.Model.Models;
using Surya.India.Data.Models;
using Surya.India.Service;
using Surya.India.Data.Infrastructure;
using Surya.India.Core.Common;
using System.Net.Http;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using Surya.India.Presentation.ViewModels;
using Surya.India.Model.ViewModels;
using AutoMapper;
using System.Configuration;
using Surya.India.Presentation.Helper;
using Surya.India.Presentation;
using System.Text;
using System.Web.Script.Serialization;
using System.Data.Entity.Validation;

namespace Surya.India.Web
{
    
    [Authorize]
    public class ClassController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        IClassService _ClassService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        public ClassController(IClassService ClassService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _ClassService = ClassService;
            _unitOfWork = unitOfWork;
            _exception = exec;
        }

        [HttpGet]
        public JsonResult Index(int id)
        {
            var p = _ClassService.GetClassList(id);                               
            return Json(p, JsonRequestBehavior.AllowGet);
            
        }


        private void PrepareViewBag()
        {

        }

        public ActionResult _Create(int Id) //Id ==>Sale Order Header Id
        {
            Sch_Class s = new Sch_Class();
            s.ProgramId = Id;

            PrepareViewBag();
            return PartialView("_Create", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreatePost(Sch_Class svm)
        {
            if (ModelState.IsValid)
            {        
                if(svm.ClassId == 0)
                {
                    svm.CreatedDate = DateTime.Now;
                    svm.ModifiedDate = DateTime.Now;
                    svm.CreatedBy = User.Identity.Name;
                    svm.ModifiedBy = User.Identity.Name;
                    svm.ObjectState = Model.ObjectState.Added;
                    _ClassService.Create(svm);


                    try
                    {
                        _unitOfWork.Save();
                    }
                   
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        ModelState.AddModelError("", message);
                        return PartialView("_Create", svm);
                    }
                    return RedirectToAction("_Create", new { id = svm.ProgramId });
                }
                else
                {
                    Sch_Class ClassDetail = _ClassService.Find(svm.ClassId);
                    StringBuilder logstring = new StringBuilder();

                    ClassDetail.ProgramId = svm.ProgramId;
                    ClassDetail.ClassName = svm.ClassName;

                    ClassDetail.ModifiedDate = DateTime.Now;
                    ClassDetail.ModifiedBy = User.Identity.Name;
                    ClassDetail.ObjectState = Model.ObjectState.Modified;
                    _ClassService.Update(ClassDetail);


                    //Saving the Activity Log
                        ActivityLog al = new ActivityLog()
                        {
                            ActivityType = (int)ActivityTypeContants.Modified,
                            DocId = ClassDetail.ClassId,
                            CreatedDate = DateTime.Now,
                            Narration = logstring.ToString(),
                            CreatedBy = User.Identity.Name,
                            //DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(TransactionDocCategoryConstants.ClassDetail).DocumentTypeId,
                        };
                        new ActivityLogService(_unitOfWork).Create(al);
                    //End of Saving the Activity Log


                    try
                    {
                        _unitOfWork.Save();
                    }
                   
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        ModelState.AddModelError("", message);
                        return PartialView("_Create", svm);

                    }
                    return Json(new { success = true });
                }
            }

            PrepareViewBag();
            return PartialView("_Create",svm);
        }

        
        public ActionResult _Edit(int id)
        {
            Sch_Class s = _ClassService.Find(id);


            PrepareViewBag();

            if (s == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Create", s);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sch_Class ClassDetail = _ClassService.Find(id);
            if (ClassDetail == null)
            {
                return HttpNotFound();
            }
            return View(ClassDetail);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(Sch_Class vm)
        {
            Sch_Class ClassDetail = _ClassService.Find(vm.ClassId);
            _ClassService.Delete(vm.ClassId);

            try
            {
                _unitOfWork.Save();
            }
            
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                ModelState.AddModelError("", message);
                return PartialView("EditSize", vm);

            }
            return Json(new { success = true });           
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
