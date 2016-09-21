using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Model.Models;
using Data.Models;
using Service;
using Data.Infrastructure;
using Core.Common;
using Presentation;
using Model.ViewModel;
using System.Xml.Linq;

namespace Web
{

    [Authorize]
    public class PersonBankAccountController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        IPersonBankAccountService _PersonBankAccountService;
        IPersonService _PersonService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        public PersonBankAccountController(IPersonBankAccountService PersonBankAccount, IPersonService Person, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _PersonBankAccountService = PersonBankAccount;
            _PersonService = Person;
            _unitOfWork = unitOfWork;
            _exception = exec;

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;
        }

        [HttpGet]
        public JsonResult Index(int id)
        {
            var p = _PersonBankAccountService.GetPersonBankAccountListForIndex(id);
            return Json(p, JsonRequestBehavior.AllowGet);

        }

        private void PrepareViewBag(PersonBankAccount s)
        {
        }

        public ActionResult _Create(int Id) //Id ==>Sale Order Header Id
        {
            PersonBankAccount s = new PersonBankAccount();
            s.PersonId = Id;
            PrepareViewBag(null);
            return PartialView("_Create", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreatePost(PersonBankAccount svm)
        {

            if (ModelState.IsValid)
            {
                if (svm.PersonBankAccountID == 0)
                {
                    svm.CreatedDate = DateTime.Now;
                    svm.ModifiedDate = DateTime.Now;
                    svm.CreatedBy = User.Identity.Name;
                    svm.ModifiedBy = User.Identity.Name;
                    svm.ObjectState = Model.ObjectState.Added;
                    _PersonBankAccountService.Create(svm);

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

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.PersonBankAccount).DocumentTypeId,
                        DocId = svm.PersonBankAccountID,
                        ActivityType = (int)ActivityTypeContants.Added,
                    }));


                    return RedirectToAction("_Create", new { id = svm.PersonId });
                }
                else
                {
                    svm.ModifiedBy = User.Identity.Name;
                    svm.ModifiedDate = DateTime.Now;
                    _PersonBankAccountService.Update(svm);

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

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.PersonBankAccount).DocumentTypeId,
                        DocId = svm.PersonBankAccountID,
                        ActivityType = (int)ActivityTypeContants.Modified,
                    }));

                    return Json(new { success = true });
                }
            }

            PrepareViewBag(svm);
            return PartialView("_Create", svm);
        }


        public ActionResult _Edit(int id)
        {
            PersonBankAccount temp = _PersonBankAccountService.GetPersonBankAccount(id);

            PrepareViewBag(temp);

            if (temp == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Create", temp);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonBankAccount PersonBankAccount = _PersonBankAccountService.Find(id);
            if (PersonBankAccount == null)
            {
                return HttpNotFound();
            }
            return View(PersonBankAccount);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonBankAccount PersonBankAccount = _PersonBankAccountService.Find(id);
            _PersonBankAccountService.Delete(id);


            _unitOfWork.Save();

            return RedirectToAction("Index", new { Id = PersonBankAccount.PersonId }).Success("Data deleted successfully");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(PersonBankAccount vm)
        {
            List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

            _PersonBankAccountService.Delete(vm.PersonBankAccountID);

            LogList.Add(new LogTypeViewModel
            {
                ExObj = vm,
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
                return PartialView("EditSize", vm);
            }

            LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
            {
                DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.PersonBankAccount).DocumentTypeId,
                DocId = vm.PersonBankAccountID,
                ActivityType = (int)ActivityTypeContants.Deleted,
                xEModifications = Modifications,
            }));

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
