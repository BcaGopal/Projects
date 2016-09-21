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
using Surya.India.Presentation.ViewModels;
using Surya.India.Presentation;
using Surya.India.Core.Common;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace Surya.India.Web
{
    [Authorize]
    public class ProgramController : System.Web.Mvc.Controller
    {
          private ApplicationDbContext db = new ApplicationDbContext();

          IProgramService _ProgramService;
          IUnitOfWork _unitOfWork;
          IExceptionHandlingService _exception;
          public ProgramController(IProgramService ProgramService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
          {
              _ProgramService = ProgramService;
              _unitOfWork = unitOfWork;
              _exception = exec;
          }
        // GET: /ProgramMaster/
        
          public ActionResult Index()
          { 
              var Sch_Program = _ProgramService.GetProgramList().ToList();
              return View(Sch_Program);
              //return RedirectToAction("Create");
          }

          // GET: /ProgramMaster/Create
        
          public ActionResult Create()
          {
              Sch_Program vm = new Sch_Program();
              vm.IsActive = true;
              return View("Create",vm);
          }

        // POST: /ProductMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
          public ActionResult Post(Sch_Program vm)
          {
              Sch_Program pt = vm; 
              if (ModelState.IsValid)
              {                  


                  if(vm.ProgramId<=0)
                  { 
                  pt.CreatedDate = DateTime.Now;
                  pt.ModifiedDate = DateTime.Now;
                  pt.CreatedBy = User.Identity.Name;
                  pt.ModifiedBy = User.Identity.Name;
                  pt.ObjectState = Model.ObjectState.Added;
                  _ProgramService.Create(pt);

                  ActivityLog log = new ActivityLog()
                  {
                      ActivityType = (int)(ActivityTypeContants.Added),
                      CreatedBy = User.Identity.Name,
                      CreatedDate = DateTime.Now,
                      DocId = pt.ProgramId,
                      Narration = "A new Sch_Program is created with the Id " + pt.ProgramId,
                  };

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


                  return RedirectToAction("Create").Success("Data saved successfully");
                  }
                  else
                  {

                      Sch_Program temp = _ProgramService.Find(pt.ProgramId);
                      temp.ProgramName = pt.ProgramName;
                      temp.WEF = pt.WEF;
                      temp.IsActive = pt.IsActive;
                      temp.ModifiedDate = DateTime.Now;
                      temp.ModifiedBy = User.Identity.Name;
                      temp.ObjectState = Model.ObjectState.Modified;
                      _ProgramService.Update(temp);

                      ActivityLog log = new ActivityLog()
                      {
                          ActivityType = (int)(ActivityTypeContants.Modified),
                          CreatedBy = User.Identity.Name,
                          CreatedDate = DateTime.Now,
                          DocId = pt.ProgramId,
                          Narration = "Sch_Program is modified with the name " + pt.ProgramName,
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


                      return RedirectToAction("Index").Success("Data saved successfully");


                  }








              }
              return View("Create", vm);
          }


        // GET: /ProductMaster/Edit/5
        
        public ActionResult Edit(int id)
        {
            Sch_Program pt = _ProgramService.Find(id);
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
            Sch_Program Sch_Program = db.Sch_Program.Find(id);
            if (Sch_Program == null)
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
            if(ModelState.IsValid)
            {
                var temp = _ProgramService.Find(vm.id);


                IEnumerable<Sch_Class> ClassList = new ClassService(_unitOfWork).GetClassList(vm.id);
                IEnumerable<Sch_Stream> StreamList = new StreamService(_unitOfWork).GetStreamList(vm.id);


                foreach(Sch_Class item in ClassList)
                {
                    new ClassService(_unitOfWork).Delete(item);
                }

                foreach (Sch_Stream item in StreamList)
                {
                    new StreamService(_unitOfWork).Delete(item);
                }

                _ProgramService.Delete(vm.id);

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


                return Json(new { success = true });

            }
            return PartialView("_Reason", vm);
        }

        [HttpGet]
        public ActionResult NextPage(int id)//CurrentHeaderId
        {
            var nextId = _ProgramService.NextId(id);
            return RedirectToAction("Edit", new { id = nextId });
        }
        [HttpGet]
        public ActionResult PrevPage(int id)//CurrentHeaderId
        {
            var nextId = _ProgramService.PrevId(id);
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
            //Dt = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.Sch_Program);

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
