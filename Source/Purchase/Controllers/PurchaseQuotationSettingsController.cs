﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model.Models;
using Data.Models;
using Service;
using Data.Infrastructure;
using Presentation;
using Core.Common;
using Model.ViewModel;
using AutoMapper;
using System.Xml.Linq;

namespace Web
{
    [Authorize]
    public class PurchaseQuotationSettingController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        List<string> UserRoles = new List<string>();
        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        IPurchaseQuotationSettingService _PurchaseQuotationSettingService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;
        public PurchaseQuotationSettingController(IPurchaseQuotationSettingService PurchaseQuotationSettingService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _PurchaseQuotationSettingService = PurchaseQuotationSettingService;
            _unitOfWork = unitOfWork;
            _exception = exec;

            UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;
        }

        private void PrepareViewBag()
        {
            ViewBag.UnitConvForList = (from p in db.UnitConversonFor
                                       select p).ToList();
        }

        // GET: /PurchaseQuotationSettingMaster/Create
        
        public ActionResult Create(int id)//DocTypeId
        {
            List<string> UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];
            if (!UserRoles.Contains("SysAdmin"))
            {
                return View("~/Views/Shared/InValidSettings.cshtml");
            }
            var DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            var SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            PrepareViewBag();
            var settings = new PurchaseQuotationSettingService(_unitOfWork).GetPurchaseQuotationSettingForDocument(id, DivisionId, SiteId);

            if (settings == null)
            {
                PurchaseQuotationSettingsViewModel vm = new PurchaseQuotationSettingsViewModel();
                vm.DocTypeName = new DocumentTypeService(_unitOfWork).Find(id).DocumentTypeName;
                vm.SiteId = SiteId;
                vm.DivisionId = DivisionId;
                vm.DocTypeId = id;
                return View("Create", vm);
            }
            else
            {
                PurchaseQuotationSettingsViewModel temp = AutoMapper.Mapper.Map<PurchaseQuotationSetting, PurchaseQuotationSettingsViewModel>(settings);
                temp.DocTypeName = new DocumentTypeService(_unitOfWork).Find(id).DocumentTypeName;
                return View("Create", temp);
            }

        }

        // POST: /ProductMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(PurchaseQuotationSettingsViewModel vm)
        {
            PurchaseQuotationSetting pt = AutoMapper.Mapper.Map<PurchaseQuotationSettingsViewModel, PurchaseQuotationSetting>(vm);

            if (ModelState.IsValid)
            {

                if (vm.PurchaseQuotationSettingId <= 0)
                {
                    pt.CreatedDate = DateTime.Now;
                    pt.ModifiedDate = DateTime.Now;
                    pt.CreatedBy = User.Identity.Name;
                    pt.ModifiedBy = User.Identity.Name;
                    pt.ObjectState = Model.ObjectState.Added;
                    _PurchaseQuotationSettingService.Create(pt);

                    try
                    {
                        _unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        ModelState.AddModelError("", message);
                        PrepareViewBag();
                        return View("Create", vm);
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = pt.DocTypeId,
                        DocId = pt.PurchaseQuotationSettingId,
                        ActivityType = (int)ActivityTypeContants.SettingsAdded,
                    }));


                    return RedirectToAction("Index", "PurchaseQuotationHeader", new { id=vm.DocTypeId}).Success("Data saved successfully");
                }
                else
                {
                    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

                    PurchaseQuotationSetting temp = _PurchaseQuotationSettingService.Find(pt.PurchaseQuotationSettingId);

                    PurchaseQuotationSetting ExRec = Mapper.Map<PurchaseQuotationSetting>(temp);
                 
                    temp.filterContraDocTypes = pt.filterContraDocTypes;
                    temp.filterLedgerAccountGroups = pt.filterLedgerAccountGroups;
                    temp.filterLedgerAccounts = pt.filterLedgerAccounts;                    
                    temp.filterProductGroups = pt.filterProductGroups;
                    temp.filterProducts = pt.filterProducts;
                    temp.filterProductTypes = pt.filterProductTypes;
                    temp.isEditableRate = pt.isEditableRate;
                    temp.isMandatoryCostCenter = pt.isMandatoryCostCenter;
                    temp.isMandatoryRate = pt.isMandatoryRate;
                    temp.filterContraDivisions = pt.filterContraDivisions;
                    temp.filterContraSites = pt.filterContraSites;
                    temp.CalculationId = pt.CalculationId;
                    temp.isVisibleForIndent = pt.isVisibleForIndent;
                    temp.isVisibleSalesTaxGroup = pt.isVisibleSalesTaxGroup;
                    temp.isVisibleCurrency = pt.isVisibleCurrency;
                    temp.isVisibleDeliveryTerms = pt.isVisibleDeliveryTerms;
                    temp.isVisibleShipMethod = pt.isVisibleShipMethod;
                    temp.isVisibleCostCenter = pt.isVisibleCostCenter;
                    temp.isVisibleDimension1 = pt.isVisibleDimension1;
                    temp.isVisibleDimension2 = pt.isVisibleDimension2;
                    temp.isVisibleDimension3 = pt.isVisibleDimension3;
                    temp.isVisibleDimension4 = pt.isVisibleDimension4;
                    temp.isVisibleLotNo = pt.isVisibleLotNo;
                    temp.SqlProcDocumentPrint = pt.SqlProcDocumentPrint;
                    temp.SqlProcDocumentPrint_AfterApprove = pt.SqlProcDocumentPrint_AfterApprove;
                    temp.SqlProcDocumentPrint_AfterSubmit = pt.SqlProcDocumentPrint_AfterSubmit;
                    temp.CalculateDiscountOnRate = pt.CalculateDiscountOnRate;
                    temp.UnitConversionForId = pt.UnitConversionForId;

                    temp.ModifiedDate = DateTime.Now;
                    temp.ModifiedBy = User.Identity.Name;
                    temp.ObjectState = Model.ObjectState.Modified;
                    _PurchaseQuotationSettingService.Update(temp);

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
                        PrepareViewBag();
                        return View("Create", pt);
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = temp.DocTypeId,
                        DocId = temp.PurchaseQuotationSettingId,
                        ActivityType = (int)ActivityTypeContants.SettingsModified,
                        xEModifications = Modifications,

                    }));
                    return RedirectToAction("Index", "PurchaseQuotationHeader", new { id=vm.DocTypeId}).Success("Data saved successfully");

                }

            }
            //PrepareViewBag();
            return View("Create", vm);
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
