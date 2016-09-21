using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Model.Models;
using Data.Models;
using Service;
using Data.Infrastructure;
using Core.Common;
using Model.ViewModels;
using AutoMapper;
using Presentation;
using Model.ViewModel;
using System.Xml.Linq;

namespace Web
{

    [Authorize]
    public class PersonContactController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        IPersonContactService _PersonContactService;
        IPersonService _PersonService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        public PersonContactController(IPersonContactService PersonContact, IPersonService Person, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _PersonContactService = PersonContact;
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
            var p = _PersonContactService.GetPersonContactListForIndex(id);
            return Json(p, JsonRequestBehavior.AllowGet);

        }

        private void PrepareViewBag(PersonContactViewModel s)
        {
            ViewBag.PersonContactTypeList = new PersonContactTypeService(_unitOfWork).GetPersonContactTypeList().ToList();
        }

        public ActionResult _Create(int Id) //Id ==>Sale Order Header Id
        {
            PersonContactViewModel s = new PersonContactViewModel();
            s.PersonID = Id;
            PrepareViewBag(null);
            return PartialView("_Create", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreatePost(PersonContactViewModel svm)
        {

            if (ModelState.IsValid)
            {
                if (svm.PersonContactId == 0)
                {

                    Person person = new Person();
                    PersonContact personcontact = new PersonContact();

                    person.Name = svm.Name;
                    person.Suffix = svm.Suffix;
                    person.Code = svm.Code;
                    person.Phone = svm.Phone;
                    person.Mobile = svm.Mobile;
                    person.Email = svm.Email;
                    person.CreatedDate = DateTime.Now;
                    person.ModifiedDate = DateTime.Now;
                    person.CreatedBy = User.Identity.Name;
                    person.ModifiedBy = User.Identity.Name;
                    person.ObjectState = Model.ObjectState.Added;
                    _PersonService.Create(person);

                    personcontact.PersonId = svm.PersonID;
                    personcontact.ContactId = person.PersonID;
                    personcontact.PersonContactTypeId = svm.PersonContactTypeId;
                    personcontact.CreatedDate = DateTime.Now;
                    personcontact.ModifiedDate = DateTime.Now;
                    personcontact.CreatedBy = User.Identity.Name;
                    personcontact.ModifiedBy = User.Identity.Name;
                    personcontact.ObjectState = Model.ObjectState.Added;
                    _PersonContactService.Create(personcontact);

                    try
                    {
                        _unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        ModelState.AddModelError("", message);
                        PrepareViewBag(null);
                        return PartialView("_Create", svm);
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.PersonContact).DocumentTypeId,
                        DocId = person.PersonID,
                        ActivityType = (int)ActivityTypeContants.Added,
                    }));

                    return RedirectToAction("_Create", new { id = person.PersonID });
                }
                else
                {
                    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

                    PersonContact personcontact = _PersonContactService.Find(svm.PersonContactId);
                    Person person = _PersonService.Find(personcontact.ContactId);

                    PersonContact ExRec = Mapper.Map<PersonContact>(personcontact);
                    Person ExRec2 = Mapper.Map<Person>(person);

                    person.Name = svm.Name;
                    person.Suffix = svm.Suffix;
                    person.Code = svm.Code;
                    person.Phone = svm.Phone;
                    person.Mobile = svm.Mobile;
                    person.Email = svm.Email;
                    person.ModifiedBy = User.Identity.Name;
                    person.ModifiedDate = DateTime.Now;
                    _PersonService.Update(person);

                    personcontact.PersonContactTypeId = svm.PersonContactTypeId;
                    _PersonContactService.Update(personcontact);

                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = ExRec,
                        Obj = personcontact,
                    });
                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = ExRec2,
                        Obj = person,
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
                        PrepareViewBag(null);
                        return PartialView("_Create", svm);
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.PersonContact).DocumentTypeId,
                        DocId = person.PersonID,
                        ActivityType = (int)ActivityTypeContants.Modified,
                        xEModifications = Modifications,
                    }));

                    return Json(new { success = true });

                }
            }

            PrepareViewBag(svm);
            return PartialView("_Create", svm);
        }


        public ActionResult _Edit(int id)
        {
            PersonContact temp = _PersonContactService.GetPersonContact(id);
            Person person = _PersonService.Find(temp.ContactId);
            PersonContactViewModel s = Mapper.Map<PersonContact, PersonContactViewModel>(temp);

            s.Name = person.Name;
            s.Mobile = person.Mobile;
            s.Phone = person.Phone;
            s.Email = person.Email;
            s.Code = person.Code;
            s.Suffix = person.Suffix;

            PrepareViewBag(s);

            if (temp == null)
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
            PersonContact PersonContact = _PersonContactService.Find(id);
            if (PersonContact == null)
            {
                return HttpNotFound();
            }
            return View(PersonContact);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonContact PersonContact = _PersonContactService.Find(id);
            _PersonContactService.Delete(id);


            _unitOfWork.Save();

            return RedirectToAction("Index", new { Id = PersonContact.PersonId }).Success("Data deleted successfully");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(PersonContactViewModel vm)
        {
            List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

            PersonContact PersonContact = _PersonContactService.Find(vm.PersonContactId);
            Person person = _PersonService.Find(PersonContact.ContactId);

            LogList.Add(new LogTypeViewModel
            {
                ExObj = PersonContact,
            });
            LogList.Add(new LogTypeViewModel
            {
                ExObj = person,
            });
            _PersonContactService.Delete(vm.PersonContactId);
            _PersonService.Delete(person.PersonID);
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
                DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.PersonContact).DocumentTypeId,
                DocId = person.PersonID,
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
