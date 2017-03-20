using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model.Models;
using Data.Models;
using Service;
using Data.Infrastructure;
using Core.Common;
using Model.ViewModels;
using AutoMapper;
using Model.ViewModel;
using System.Xml.Linq;

using Model.ViewModels;

namespace Web
{

    [Authorize]
    public class PersonCreationController : System.Web.Mvc.Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        


        List<string> UserRoles = new List<string>();
        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;


        public PersonCreationController(IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _unitOfWork = unitOfWork;
            _exception = exec;

            UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;
        }






        public ActionResult _Create(int? id) 
        {
            BuyerViewModel p = new BuyerViewModel();
            p.IsActive = true;
            

            if (id != null && id !=0)
            {
                p = GetBuyerViewModel((int)id);
            }
            else
            {
                NewDocNoViewModel NewPersonCode = db.Database.SqlQuery<NewDocNoViewModel>("" + System.Configuration.ConfigurationManager.AppSettings["DataBaseSchema"] + ".GetNewPersonCode ").FirstOrDefault();
                if (NewPersonCode != null)
                {
                    p.Code = NewPersonCode.NewDocNo;
                }

                NewDocNoViewModel NewPersonSuffix = db.Database.SqlQuery<NewDocNoViewModel>("" + System.Configuration.ConfigurationManager.AppSettings["DataBaseSchema"] + ".GetNewPersonSuffix ").FirstOrDefault();
                if (NewPersonSuffix != null)
                {
                    p.Suffix = NewPersonSuffix.NewDocNo;
                }
            }

            return PartialView("_Create", p);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult _CreatePost(BuyerViewModel buyerVm)
        {
            if (ModelState.IsValid)
            {
                if (buyerVm.PersonId == 0)
                {
                    Person person = Mapper.Map<BuyerViewModel, Person>(buyerVm);
                    BusinessEntity businessentity = Mapper.Map<BuyerViewModel, BusinessEntity>(buyerVm);
                    PersonAddress personaddress = Mapper.Map<BuyerViewModel, PersonAddress>(buyerVm);
                    LedgerAccount account = Mapper.Map<BuyerViewModel, LedgerAccount>(buyerVm);

                    person.IsActive = true;
                    person.CreatedDate = DateTime.Now;
                    person.ModifiedDate = DateTime.Now;
                    person.CreatedBy = User.Identity.Name;
                    person.ModifiedBy = User.Identity.Name;
                    person.ObjectState = Model.ObjectState.Added;
                    new PersonService(_unitOfWork).Create(person);

                    new  BusinessEntityService(_unitOfWork).Create(businessentity);

                    personaddress.AddressType = AddressTypeConstants.Work;
                    personaddress.CreatedDate = DateTime.Now;
                    personaddress.ModifiedDate = DateTime.Now;
                    personaddress.CreatedBy = User.Identity.Name;
                    personaddress.ModifiedBy = User.Identity.Name;
                    personaddress.ObjectState = Model.ObjectState.Added;
                    new PersonAddressService(_unitOfWork).Create(personaddress);


                    account.LedgerAccountName = person.Name;
                    account.LedgerAccountSuffix = person.Suffix;
                    account.LedgerAccountGroupId = new LedgerAccountGroupService(_unitOfWork).Find(LedgerAccountGroupConstants.SundryCreditors).LedgerAccountGroupId;
                    account.CreatedDate = DateTime.Now;
                    account.ModifiedDate = DateTime.Now;
                    account.CreatedBy = User.Identity.Name;
                    account.ModifiedBy = User.Identity.Name;
                    account.ObjectState = Model.ObjectState.Added;
                    new LedgerAccountService(_unitOfWork).Create(account);


                    if (buyerVm.CstNo != "" && buyerVm.CstNo != null)
                    {
                        PersonRegistration personregistration = new PersonRegistration();
                        personregistration.RegistrationType = PersonRegistrationType.CstNo;
                        personregistration.RegistrationNo = buyerVm.CstNo;
                        personregistration.CreatedDate = DateTime.Now;
                        personregistration.ModifiedDate = DateTime.Now;
                        personregistration.CreatedBy = User.Identity.Name;
                        personregistration.ModifiedBy = User.Identity.Name;
                        personregistration.ObjectState = Model.ObjectState.Added;
                        new PersonRegistrationService(_unitOfWork).Create(personregistration);
                    }

                    if (buyerVm.TinNo != "" && buyerVm.TinNo != null)
                    {
                        PersonRegistration personregistration = new PersonRegistration();
                        personregistration.RegistrationType = PersonRegistrationType.TinNo;
                        personregistration.RegistrationNo = buyerVm.TinNo;
                        personregistration.CreatedDate = DateTime.Now;
                        personregistration.ModifiedDate = DateTime.Now;
                        personregistration.CreatedBy = User.Identity.Name;
                        personregistration.ModifiedBy = User.Identity.Name;
                        personregistration.ObjectState = Model.ObjectState.Added;
                        new PersonRegistrationService(_unitOfWork).Create(personregistration);
                    }

                    if (buyerVm.PANNo != "" && buyerVm.PANNo != null)
                    {
                        PersonRegistration personregistration = new PersonRegistration();
                        personregistration.RegistrationType = PersonRegistrationType.PANNo;
                        personregistration.RegistrationNo = buyerVm.PANNo;
                        personregistration.CreatedDate = DateTime.Now;
                        personregistration.ModifiedDate = DateTime.Now;
                        personregistration.CreatedBy = User.Identity.Name;
                        personregistration.ModifiedBy = User.Identity.Name;
                        personregistration.ObjectState = Model.ObjectState.Added;
                        new PersonRegistrationService(_unitOfWork).Create(personregistration);
                    }


                    try
                    {
                        _unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        ModelState.AddModelError("", message);
                        return View(buyerVm);

                    }

                    return Json(new { success = true, PersonId = buyerVm.PersonId, Name = buyerVm.Name });
                }
                else
                {
                    //string tempredirect = (Request["Redirect"].ToString());
                    Person person = Mapper.Map<BuyerViewModel, Person>(buyerVm);
                    BusinessEntity businessentity = Mapper.Map<BuyerViewModel, BusinessEntity>(buyerVm);
                    PersonAddress personaddress = new PersonAddressService(_unitOfWork).Find(buyerVm.PersonAddressID);
                    LedgerAccount account = new LedgerAccountService(_unitOfWork).Find(buyerVm.AccountId);
                    PersonRegistration PersonCst = new PersonRegistrationService(_unitOfWork).Find(buyerVm.PersonRegistrationCstNoID);
                    PersonRegistration PersonTin = new PersonRegistrationService(_unitOfWork).Find(buyerVm.PersonRegistrationTinNoID);
                    PersonRegistration PersonPAN = new PersonRegistrationService(_unitOfWork).Find(buyerVm.PersonRegistrationPANNoID);


                    person.IsActive = true;
                    person.ModifiedDate = DateTime.Now;
                    person.ModifiedBy = User.Identity.Name;
                    new PersonService(_unitOfWork).Update(person);


                    new BusinessEntityService(_unitOfWork).Update(businessentity);

                    personaddress.Address = buyerVm.Address;
                    personaddress.CityId = buyerVm.CityId;
                    personaddress.Zipcode = buyerVm.Zipcode;
                    personaddress.ModifiedDate = DateTime.Now;
                    personaddress.ModifiedBy = User.Identity.Name;
                    personaddress.ObjectState = Model.ObjectState.Modified;
                    new PersonAddressService(_unitOfWork).Update(personaddress);

                    account.LedgerAccountName = person.Name;
                    account.LedgerAccountSuffix = person.Suffix;
                    account.ModifiedDate = DateTime.Now;
                    account.ModifiedBy = User.Identity.Name;
                    new LedgerAccountService(_unitOfWork).Update(account);

                    if (buyerVm.CstNo != null && buyerVm.CstNo != "")
                    {
                        if (PersonCst != null)
                        {
                            PersonCst.RegistrationNo = buyerVm.CstNo;
                            new PersonRegistrationService(_unitOfWork).Update(PersonCst);
                        }
                        else
                        {
                            PersonRegistration personregistration = new PersonRegistration();
                            personregistration.PersonId = buyerVm.PersonId;
                            personregistration.RegistrationType = PersonRegistrationType.CstNo;
                            personregistration.RegistrationNo = buyerVm.CstNo;
                            personregistration.CreatedDate = DateTime.Now;
                            personregistration.ModifiedDate = DateTime.Now;
                            personregistration.CreatedBy = User.Identity.Name;
                            personregistration.ModifiedBy = User.Identity.Name;
                            personregistration.ObjectState = Model.ObjectState.Added;
                            new PersonRegistrationService(_unitOfWork).Create(personregistration);
                        }
                    }

                    if (buyerVm.TinNo != null && buyerVm.TinNo != "")
                    {
                        if (PersonTin != null)
                        {
                            PersonTin.RegistrationNo = buyerVm.TinNo;
                            new PersonRegistrationService(_unitOfWork).Update(PersonTin);
                        }
                        else
                        {
                            PersonRegistration personregistration = new PersonRegistration();
                            personregistration.PersonId = buyerVm.PersonId;
                            personregistration.RegistrationType = PersonRegistrationType.TinNo;
                            personregistration.RegistrationNo = buyerVm.TinNo;
                            personregistration.CreatedDate = DateTime.Now;
                            personregistration.ModifiedDate = DateTime.Now;
                            personregistration.CreatedBy = User.Identity.Name;
                            personregistration.ModifiedBy = User.Identity.Name;
                            personregistration.ObjectState = Model.ObjectState.Added;
                            new PersonRegistrationService(_unitOfWork).Create(personregistration);
                        }
                    }

                    if (buyerVm.PANNo != null && buyerVm.PANNo != "")
                    {
                        if (PersonPAN != null)
                        {
                            PersonPAN.RegistrationNo = buyerVm.PANNo;
                            new PersonRegistrationService(_unitOfWork).Update(PersonPAN);
                        }
                        else
                        {
                            PersonRegistration personregistration = new PersonRegistration();
                            personregistration.PersonId = buyerVm.PersonId;
                            personregistration.RegistrationType = PersonRegistrationType.PANNo;
                            personregistration.RegistrationNo = buyerVm.PANNo;
                            personregistration.CreatedDate = DateTime.Now;
                            personregistration.ModifiedDate = DateTime.Now;
                            personregistration.CreatedBy = User.Identity.Name;
                            personregistration.ModifiedBy = User.Identity.Name;
                            personregistration.ObjectState = Model.ObjectState.Added;
                            new PersonRegistrationService(_unitOfWork).Create(personregistration);
                        }
                    }


                    try
                    {
                        _unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        ModelState.AddModelError("", message);
                        return View("Create", buyerVm);
                    }

                    return Json(new { success = true, PersonId = buyerVm.PersonId, Name = buyerVm.Name });
                }
            }
            return View(buyerVm);

        }



        public JsonResult GetEMailDetailJson(string Email)
        {
            var CustomerDetail = (from p in db.Persons
                                  where p.Email == Email
                                  select new 
                                  {
                                      PersonId = p.PersonID
                                  }).FirstOrDefault();

            BuyerViewModel vm = null;
            List<BuyerViewModel> BuyerViewModelJson = new List<BuyerViewModel>();
            if (CustomerDetail != null)
            {
                vm = GetBuyerViewModel(CustomerDetail.PersonId);
                BuyerViewModelJson.Add(vm);
                return Json(BuyerViewModelJson);
            }
            else
            {
                return null;
            }
        }


        public BuyerViewModel GetBuyerViewModel(int id)
        {
            BuyerViewModel buyerviewmodel = (from bus in db.BusinessEntity
                                             join p in db.Persons on bus.PersonID equals p.PersonID into PersonTable
                                             from PersonTab in PersonTable.DefaultIfEmpty()
                                             join pa in db.PersonAddress on PersonTab.PersonID equals pa.PersonId into PersonAddressTable
                                             from PersonAddressTab in PersonAddressTable.DefaultIfEmpty()
                                             join ac in db.LedgerAccount on PersonTab.PersonID equals ac.PersonId into AccountTable
                                             from AccountTab in AccountTable.DefaultIfEmpty()
                                             where PersonTab.PersonID == id
                                             select new BuyerViewModel
                                             {
                                                 PersonId = PersonTab.PersonID,
                                                 Name = PersonTab.Name,
                                                 Suffix = PersonTab.Suffix,
                                                 Code = PersonTab.Code,
                                                 Phone = PersonTab.Phone,
                                                 Mobile = PersonTab.Mobile,
                                                 Email = PersonTab.Email,
                                                 Address = PersonAddressTab.Address,
                                                 CityId = PersonAddressTab.CityId,
                                                 CityName = PersonAddressTab.City.CityName,
                                                 Zipcode = PersonAddressTab.Zipcode,
                                                 PersonRateGroupId = bus.PersonRateGroupId,
                                                 CreaditDays = bus.CreaditDays,
                                                 CreaditLimit = bus.CreaditLimit,
                                                 IsActive = PersonTab.IsActive,
                                                 SalesTaxGroupPartyId = bus.SalesTaxGroupPartyId,
                                                 CreatedBy = PersonTab.CreatedBy,
                                                 CreatedDate = PersonTab.CreatedDate,
                                                 SiteIds = bus.SiteIds,
                                                 Tags = PersonTab.Tags,
                                                 ImageFileName = PersonTab.ImageFileName,
                                                 ImageFolderName = PersonTab.ImageFolderName,
                                                 IsSisterConcern = (bool?)bus.IsSisterConcern ?? false,
                                                 AccountId = (int?)AccountTab.LedgerAccountId ?? 0,
                                                 PersonAddressID = (PersonAddressTab == null ? 0 : PersonAddressTab.PersonAddressID),
                                                 LedgerAccountGroupId = (int?)AccountTab.LedgerAccountGroupId ?? 0,
                                             }).FirstOrDefault();

            var PersonRegistration = (from pp in db.PersonRegistration
                                      where pp.PersonId == id
                                      select new
                                      {
                                          PersonRegistrationId = pp.PersonRegistrationID,
                                          RregistrationType = pp.RegistrationType,
                                          RregistrationNo = pp.RegistrationNo
                                      }).ToList();

            if (PersonRegistration != null)
            {
                foreach (var item in PersonRegistration)
                {
                    if (item.RregistrationType == PersonRegistrationType.CstNo)
                    {
                        buyerviewmodel.PersonRegistrationCstNoID = item.PersonRegistrationId;
                        buyerviewmodel.CstNo = item.RregistrationNo;
                    }

                    if (item.RregistrationType == PersonRegistrationType.TinNo)
                    {
                        buyerviewmodel.PersonRegistrationTinNoID = item.PersonRegistrationId;
                        buyerviewmodel.TinNo = item.RregistrationNo;
                    }

                    if (item.RregistrationType == PersonRegistrationType.PANNo)
                    {
                        buyerviewmodel.PersonRegistrationPANNoID = item.PersonRegistrationId;
                        buyerviewmodel.PANNo = item.RregistrationNo;
                    }
                }
            }

            return buyerviewmodel;
        }

    }
}
