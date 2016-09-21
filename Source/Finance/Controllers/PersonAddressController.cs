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
using Data.Infrastructure;
using Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Presentation;

namespace Web.Controllers
{
    [Authorize]
    public class PersonAddressController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        IPersonAddressService _personAddressService;
        IUnitOfWork _unitOfWork;
        public PersonAddressController(IPersonAddressService personAddress, IUnitOfWork unitOfWork)
        {            
            _personAddressService = personAddress;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public ActionResult Index()
        {  
            //PrepareViewBag();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var supplierloginUserid = UserManager.FindByName(User.Identity.Name).Id;
            var supplier = new PersonService(_unitOfWork).GetSupplierByLoginId(supplierloginUserid);
            var personAddress = _personAddressService.GetPersonAddressList(supplier.PersonID);
            ViewBag.PersonId = supplier.PersonID;
            return View(personAddress);
        }

        private void PrepareViewBag(int id)
        {
            var person = context.Persons.Where(m => m.PersonID == id).FirstOrDefault();
            ViewBag.personId = person.PersonID;
            ViewBag.personName = person.Name;

            var personList = new PersonService(_unitOfWork).GetPersonList().OfType<Employee>().ToList();
            ViewBag.PersonList = personList;


            List<SelectListItem> AddressTypes = new List<SelectListItem>();
            AddressTypes.Add(new SelectListItem { Text = "Office", Value = "O" });
            AddressTypes.Add(new SelectListItem { Text = "Factory", Value = "F" });
            AddressTypes.Add(new SelectListItem { Text = "Residence", Value = "R" });
            AddressTypes.Add(new SelectListItem { Text = "Warehouse", Value = "W" });
            ViewBag.AddressType = new SelectList(AddressTypes, "Value", "Text");

            ViewBag.CountryList = new SelectList(new CountryService(_unitOfWork).GetCountryList().ToList(),"CountryId","CountryName");
            ViewBag.StateList = new SelectList(new StateService(_unitOfWork).GetStateList().ToList(), "StateId", "StateName");
            ViewBag.CityList = new SelectList(new CityService(_unitOfWork).GetCityList().ToList(), "CityId", "CityName");

        }
        public JsonResult GetStatesJson(int CountryId)
        {
            return Json(new StateService(_unitOfWork).GetStateListForCountryType(CountryId).ToList());
        }

        public JsonResult GetCityJson(int StateId)
        {
            return Json(new CityService(_unitOfWork).GetCityListForStateType(StateId));
        }
        private void PrepareViewBag()
        {
            var personList = new PersonService(_unitOfWork).GetPersonList().OfType<Employee>().ToList(); 
            ViewBag.PersonList = personList;           
        }     
     
        
        //public ActionResult Create()
        //{
        //    PrepareViewBag();           
        //    return View();
        //}
        
        public ActionResult Create(int personID)
        {
            PersonAddress pa = new PersonAddress();
            pa.PersonId = personID;
            PrepareViewBag(personID);
            return View(pa);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonAddress pa)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var loginid= UserManager.FindByName(User.Identity.Name).Id;
            var supplierByLoginId = new PersonService(_unitOfWork).GetSupplierByLoginId(loginid);
           
            if (ModelState.IsValid)
            {
                pa.Person = new PersonService(_unitOfWork).GetPerson(supplierByLoginId.PersonID);
                pa.CreatedDate = DateTime.Now;
                pa.ModifiedDate = DateTime.Now;
                pa.CreatedBy = User.Identity.Name;
                pa.ModifiedBy = User.Identity.Name;
                pa.ObjectState = Model.ObjectState.Added;
                _personAddressService.Create(pa);
                _unitOfWork.Save();
                
               // return RedirectToAction("Index");
                return RedirectToAction("Index", "ProductSample").Success("Data saved successfully");
            }
            PrepareViewBag(pa.PersonId);
            return View(pa);
        }

        // GET: /BuyerDetail/Edit/5
        
        public ActionResult Edit(int id)
        {
            PrepareViewBag(id);
            PersonAddress pa = _personAddressService.GetPersonAddress(id);
            if (pa == null)
            {
                return HttpNotFound();
            }
            return View(pa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PersonAddress pa)
        {
            var persn = new PersonService(_unitOfWork).GetPerson(pa.PersonId); ;
            pa.Person = persn;

            if (ModelState.IsValid)
            {
                pa.CreatedDate = DateTime.Now;
                pa.ModifiedDate = DateTime.Now;
                pa.CreatedBy = User.Identity.Name;
                pa.ModifiedBy = User.Identity.Name;
                pa.ObjectState = Model.ObjectState.Modified;
                _personAddressService.Update(pa);
                _unitOfWork.Save();
                return RedirectToAction("Index").Success("Data saved successfully");
            }

            PrepareViewBag();
            return View(pa);
        }
        
/*      */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
