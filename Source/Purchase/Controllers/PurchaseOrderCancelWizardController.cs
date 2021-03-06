﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Core.Common;
using Model.Models;
using Data.Models;
using Model.ViewModels;
using Service;
using Presentation.Helper;
using Data.Infrastructure;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System.Configuration;
using Presentation;
using Model.ViewModel;
using PurchaseOrderCancelDocumentEvents;
using CustomEventArgs;
using DocumentEvents;



namespace Web
{
    [Authorize]
    public class PurchaseOrderCancelWizardController : System.Web.Mvc.Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private bool EventException = false;

        List<string> UserRoles = new List<string>();
        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        IPurchaseOrderCancelHeaderService _PurchaseOrderCancelHeaderService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        public PurchaseOrderCancelWizardController(IPurchaseOrderCancelHeaderService _service, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _PurchaseOrderCancelHeaderService = _service;
            _exception = exec;
            _unitOfWork = unitOfWork;
            if (!PurchaseOrderCancelEvents.Initialized)
            {
                PurchaseOrderCancelEvents Obj = new PurchaseOrderCancelEvents();
            }

            UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;
        }

        public void PrepareViewBag(int id)
        {
            DocumentType DocType = new DocumentTypeService(_unitOfWork).Find(id);
            ViewBag.Name = DocType.DocumentTypeName;
            ViewBag.id = id;
            ViewBag.ReasonList = new ReasonService(_unitOfWork).GetReasonList(DocType.DocumentTypeName).ToList();

        }
        public ActionResult DocumentTypeIndex(int id)//DocumentCategoryId
        {
            var p = new DocumentTypeService(_unitOfWork).FindByDocumentCategory(id).ToList();

            if (p != null)
            {
                if (p.Count == 1)
                    return RedirectToAction("OrderCancelWizard", new { id = p.FirstOrDefault().DocumentTypeId });
            }

            return View("DocumentTypeList", p);
        }

        public ActionResult OrderCancelWizard(int id)//DocumentTypeId
        {
            PrepareViewBag(id);
            PurchaseOrderCancelHeaderViewModel vm = new PurchaseOrderCancelHeaderViewModel();
            vm.DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            vm.SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            ViewBag.ReasonList = new ReasonService(_unitOfWork).GetReasonList(TransactionDocCategoryConstants.PurchaseOrderCancel).ToList();
            //Getting Settings
            var settings = new PurchaseOrderSettingService(_unitOfWork).GetPurchaseOrderSettingForDocument(id, vm.DivisionId, vm.SiteId);

            if (settings == null && UserRoles.Contains("SysAdmin"))
            {
                return RedirectToAction("CreatePurchaseOrderCancel", "PurchaseOrderSettings", new { id = id }).Warning("Please create Purchase order cancel settings");
            }
            else if (settings == null && !UserRoles.Contains("SysAdmin"))
            {
                return View("~/Views/Shared/InValidSettings.cshtml");
            }
            return View();
        }

        public JsonResult AjaxGetJsonData(int DocType, DateTime? FromDate, DateTime? ToDate, string PurchaseOrderHeaderId, string SupplierId
            , string ProductId, string Dimension1Id, string Dimension2Id, string ProductGroupId, string ProductCategoryId, decimal? BalanceQty, decimal CancelQty
            , decimal? MultiplierGT, decimal? MultiplierLT, string Sample)
        {
            string search = Request.Form["search[value]"];
            int sortColumn = -1;
            string sortDirection = "asc";
            string SortColName = "";


            // note: we only sort one column at a time
            if (Request.Form["order[0][column]"] != null)
            {
                sortColumn = int.Parse(Request.Form["order[0][column]"]);
                SortColName = Request.Form["columns[" + sortColumn + "][data]"];
            }
            if (Request.Form["order[0][dir]"] != null)
            {
                sortDirection = Request.Form["order[0][dir]"];
            }

            bool Success = true;

            var data = FilterData(DocType, FromDate, ToDate, PurchaseOrderHeaderId, SupplierId,
                                            ProductId, Dimension1Id, Dimension2Id, ProductGroupId, ProductCategoryId, BalanceQty, CancelQty, MultiplierGT, MultiplierLT, Sample);

            var RecCount = data.Count();

            if (RecCount > 1000 || RecCount == 0)
            {
                Success = false;
                return Json(new { Success = Success, Message = (RecCount > 1000 ? "No of records exceeding 1000." : "No Records found.") }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var CList = data.ToList().Select(m => new PurchaseOrderCancelWizardViewModel
                {
                    SOrderDate = m.OrderDate.ToString("dd/MMM/yyyy"),
                    PurchaseOrderLineId = m.PurchaseOrderLineId,
                    OrderNo = m.OrderNo,
                    SupplierName = m.SupplierName,
                    ProductName = m.ProductName,
                    SupplierId = m.SupplierId,
                    Dimension1Name = m.Dimension1Name,
                    Dimension2Name = m.Dimension2Name,
                    BalanceQty = m.BalanceQty,
                    CancelQty = m.CancelQty,
                    ProductGroupName = m.ProductGroupName
                }).ToList();

                return Json(new { Data = CList, Success = Success }, JsonRequestBehavior.AllowGet);
            }
        }


        private static int TOTAL_ROWS = 0;
        //private static readonly List<DataItem> _data = CreateData();    
        public class DataTableData
        {
            public int draw { get; set; }
            public int recordsTotal { get; set; }
            public int recordsFiltered { get; set; }
            public List<PurchaseOrderCancelWizardViewModel> data { get; set; }
        }

        private int SortString(string s1, string s2, string sortDirection)
        {
            return sortDirection == "asc" ? s1.CompareTo(s2) : s2.CompareTo(s1);
        }

        private int SortInteger(string s1, string s2, string sortDirection)
        {
            int i1 = int.Parse(s1);
            int i2 = int.Parse(s2);
            return sortDirection == "asc" ? i1.CompareTo(i2) : i2.CompareTo(i1);
        }

        private int SortDateTime(string s1, string s2, string sortDirection)
        {
            DateTime d1 = DateTime.Parse(s1);
            DateTime d2 = DateTime.Parse(s2);
            return sortDirection == "asc" ? d1.CompareTo(d2) : d2.CompareTo(d1);
        }

        // here we simulate SQL search, sorting and paging operations
        private IQueryable<PurchaseOrderCancelWizardViewModel> FilterData(int DocType, DateTime? FromDate, DateTime? ToDate,
                                                                    string PurchaseOrderHeaderId, string SupplierId, string ProductId, string Dimension1Id,
            string Dimension2Id, string ProductGroupId, string ProductCategoryId, decimal? BalanceQty, decimal CancelQty, decimal? MultiplierGT, decimal? MultiplierLT, string Sample)
        {

            List<int> PurchaseOrderHeaderIds = new List<int>();
            if (!string.IsNullOrEmpty(PurchaseOrderHeaderId))
                foreach (var item in PurchaseOrderHeaderId.Split(','))
                    PurchaseOrderHeaderIds.Add(Convert.ToInt32(item));


            List<int> SupplierIds = new List<int>();
            if (!string.IsNullOrEmpty(SupplierId))
                foreach (var item in SupplierId.Split(','))
                    SupplierIds.Add(Convert.ToInt32(item));

            //List<int> ProductIds = new List<int>();
            //if (!string.IsNullOrEmpty(ProductId))
            //    foreach (var item in ProductId.Split(','))
            //        ProductIds.Add(Convert.ToInt32(item));

            List<int> Dimension1Ids = new List<int>();
            if (!string.IsNullOrEmpty(Dimension1Id))
                foreach (var item in Dimension1Id.Split(','))
                    Dimension1Ids.Add(Convert.ToInt32(item));

            List<int> Dimension2Ids = new List<int>();
            if (!string.IsNullOrEmpty(Dimension2Id))
                foreach (var item in Dimension2Id.Split(','))
                    Dimension2Ids.Add(Convert.ToInt32(item));

            //List<int> ProductGroupIds = new List<int>();
            //if (!string.IsNullOrEmpty(ProductGroupId))
            //    foreach (var item in ProductGroupId.Split(','))
            //        ProductGroupIds.Add(Convert.ToInt32(item));

            int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

            var Settings = new PurchaseOrderSettingService(_unitOfWork).GetPurchaseOrderSettingForDocument(DocType, DivisionId, SiteId);

            string[] contraDocTypes = null;
            if (!string.IsNullOrEmpty(Settings.filterContraDocTypes)) { contraDocTypes = Settings.filterContraDocTypes.Split(",".ToCharArray()); }
            else { contraDocTypes = new string[] { "NA" }; }

            IQueryable<PurchaseOrderCancelWizardViewModel> _data = from p in db.ViewPurchaseOrderBalance
                                                                   join t in db.PurchaseOrderLine on p.PurchaseOrderLineId equals t.PurchaseOrderLineId
                                                                   join jw in db.Persons on p.SupplierId equals jw.PersonID into jwtable
                                                                   from jwtab in jwtable.DefaultIfEmpty()
                                                                   join prod in db.FinishedProduct on p.ProductId equals prod.ProductId into prodtable
                                                                   from prodtab in prodtable.DefaultIfEmpty()
                                                                   join dim1 in db.Dimension1 on t.Dimension1Id equals dim1.Dimension1Id into dimtable
                                                                   from dimtab in dimtable.DefaultIfEmpty()
                                                                   join dim2 in db.Dimension2 on t.Dimension2Id equals dim2.Dimension2Id into dim2table
                                                                   from dim2tab in dim2table.DefaultIfEmpty()
                                                                   join pg in db.ProductGroups on prodtab.ProductGroupId equals pg.ProductGroupId into pgtable
                                                                   from pgtab in pgtable.DefaultIfEmpty()
                                                                   join pc in db.ProductCategory on prodtab.ProductCategoryId equals pc.ProductCategoryId into pctable
                                                                   from pctab in pctable.DefaultIfEmpty()
                                                                   where p.BalanceQty > 0
                                                                   && (string.IsNullOrEmpty(Settings.filterContraDocTypes) ? 1 == 1 : contraDocTypes.Contains(t.PurchaseOrderHeader.DocTypeId.ToString()))
                                                                   select new PurchaseOrderCancelWizardViewModel
                                                                {
                                                                    OrderDate = p.OrderDate,
                                                                    OrderNo = p.PurchaseOrderNo,
                                                                    PurchaseOrderLineId = p.PurchaseOrderLineId,
                                                                    BalanceQty = p.BalanceQty,
                                                                    CancelQty = CancelQty,
                                                                    SupplierName = jwtab.Name,
                                                                    ProductName = prodtab.ProductName,
                                                                    Dimension1Name = dimtab.Dimension1Name,
                                                                    Dimension2Name = dim2tab.Dimension2Name,
                                                                    PurchaseOrderHeaderId = p.PurchaseOrderHeaderId,
                                                                    SupplierId = p.SupplierId,
                                                                    ProductGroupId = pgtab.ProductGroupId,
                                                                    ProductGroupName = pgtab.ProductGroupName,
                                                                    ProductCategoryId = pctab.ProductCategoryId,
                                                                    ProductCategoryName = pctab.ProductCategoryName,
                                                                    ProdId = p.ProductId,
                                                                    Dimension1Id = t.Dimension1Id,
                                                                    Dimension2Id = t.Dimension2Id,
                                                                    UnitConversionMultiplier = t.UnitConversionMultiplier,
                                                                    Sample = prodtab.IsSample,
                                                                };



            //if (FromDate.HasValue)
            //    _data = from p in _data
            //            where p.OrderDate >= FromDate
            //            select p;

            if (FromDate.HasValue)
                _data = _data.Where(m => m.OrderDate >= FromDate);

            if (ToDate.HasValue)
                _data = _data.Where(m => m.OrderDate <= ToDate);

            if (BalanceQty.HasValue && BalanceQty.Value > 0)
                _data = _data.Where(m => m.BalanceQty == BalanceQty.Value);

            if (MultiplierGT.HasValue)
                _data = _data.Where(m => m.UnitConversionMultiplier >= MultiplierGT.Value);

            if (MultiplierLT.HasValue)
                _data = _data.Where(m => m.UnitConversionMultiplier <= MultiplierLT.Value);


            if (!string.IsNullOrEmpty(PurchaseOrderHeaderId))
                _data = _data.Where(m => PurchaseOrderHeaderIds.Contains(m.PurchaseOrderHeaderId));

            if (!string.IsNullOrEmpty(SupplierId))
                _data = _data.Where(m => SupplierIds.Contains(m.SupplierId));

            if (!string.IsNullOrEmpty(ProductId))
                _data = _data.Where(m => m.ProductName.Contains(ProductId));

            if (!string.IsNullOrEmpty(Dimension1Id))
                _data = _data.Where(m => Dimension1Ids.Contains(m.Dimension1Id ?? 0));

            if (!string.IsNullOrEmpty(Dimension2Id))
                _data = _data.Where(m => Dimension2Ids.Contains(m.Dimension2Id ?? 0));

            if (!string.IsNullOrEmpty(ProductGroupId))
                _data = _data.Where(m => m.ProductGroupName.Contains(ProductGroupId));

            if (!string.IsNullOrEmpty(ProductCategoryId))
                _data = _data.Where(m => m.ProductCategoryName.Contains(ProductCategoryId));

            if (!string.IsNullOrEmpty(Sample) && Sample != "Include")
            {
                if (Sample == "Exclude")
                    _data = _data.Where(m => m.Sample == false);
                else if (Sample == "Only")
                    _data = _data.Where(m => m.Sample == true);
            }

            _data = _data.OrderBy(m => m.OrderDate).ThenBy(m => m.OrderNo);

            // get just one page of data
            return _data.Select(m => new PurchaseOrderCancelWizardViewModel
            {
                OrderDate = m.OrderDate,
                OrderNo = m.OrderNo,
                PurchaseOrderLineId = m.PurchaseOrderLineId,
                BalanceQty = m.BalanceQty,
                CancelQty = m.CancelQty,
                SupplierName = m.SupplierName,
                ProductName = m.ProductName,
                Dimension1Name = m.Dimension1Name,
                Dimension2Name = m.Dimension2Name,
                PurchaseOrderHeaderId = m.PurchaseOrderHeaderId,
                SupplierId = m.SupplierId,
                ProductGroupId = m.ProductGroupId,
                ProductGroupName = m.ProductGroupName,
                ProductCategoryId = m.ProductCategoryId,
                ProductCategoryName = m.ProductCategoryName,
                ProdId = m.ProdId,
                Dimension1Id = m.Dimension1Id,
                Dimension2Id = m.Dimension2Id,
                UnitConversionMultiplier = m.UnitConversionMultiplier,
                Sample = m.Sample,
            });

        }

        public ActionResult ConfirmedPurchaseOrders(List<PurchaseOrderCancelWizardViewModel> ConfirmedList, int DocTypeId, string UserRemark, int ReasonId)
        {
            //System.Web.HttpContext.Current.Session["BalanceQtyAmendmentWizardOrders"] = ConfirmedList;
            //return Json(new { Success = "URL", Data = "/PurchaseOrderCancelWizard/Create/" + DocTypeId }, JsonRequestBehavior.AllowGet);

            if (ConfirmedList.Count() > 0 && ConfirmedList.GroupBy(m => m.SupplierId).Count() > 1)
                return Json(new { Success = false, Data = " Multiple Headers are selected. " }, JsonRequestBehavior.AllowGet);
            else if (ConfirmedList.Count() == 0)
                return Json(new { Success = false, Data = " No Records are selected. " }, JsonRequestBehavior.AllowGet);
            else
            {

                int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
                int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

                bool BeforeSave = true;
                int Serial = 1;
                Dictionary<int, decimal> LineStatus = new Dictionary<int, decimal>();

                try
                {
                    BeforeSave = PurchaseOrderCancelDocEvents.beforeWizardSaveEvent(this, new PurchaseEventArgs(0), ref db);
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    return Json(new { Success = false, Data = message }, JsonRequestBehavior.AllowGet);
                }


                if (!BeforeSave)
                    TempData["CSEXC"] += "Failed validation before save";


                int Cnt = 0;
                int Sr = 0;


                PurchaseOrderSetting Settings = new PurchaseOrderSettingService(_unitOfWork).GetPurchaseOrderSettingForDocument(DocTypeId, DivisionId, SiteId);

                int? MaxLineId = 0;

                if (ModelState.IsValid && BeforeSave && !EventException)
                {

                    PurchaseOrderCancelHeader pt = new PurchaseOrderCancelHeader();

                    //Getting Settings
                    pt.SiteId = SiteId;
                    pt.SupplierId = ConfirmedList.FirstOrDefault().SupplierId;
                    pt.DivisionId = DivisionId;
                    pt.Remark = UserRemark;
                    pt.DocTypeId = DocTypeId;
                    pt.ReasonId = ReasonId;
                    pt.DocDate = DateTime.Now;
                    pt.DocNo = new DocumentTypeService(_unitOfWork).FGetNewDocNo("DocNo", ConfigurationManager.AppSettings["DataBaseSchema"] + ".PurchaseOrderCancelHeaders", pt.DocTypeId, pt.DocDate, pt.DivisionId, pt.SiteId);
                    pt.ModifiedBy = User.Identity.Name;
                    pt.ModifiedDate = DateTime.Now;
                    pt.CreatedBy = User.Identity.Name;
                    pt.CreatedDate = DateTime.Now;

                    pt.Status = (int)StatusConstants.Drafted;
                    pt.ObjectState = Model.ObjectState.Added;

                    db.PurchaseOrderCancelHeader.Add(pt);

                    var SelectedPurchaseOrders = ConfirmedList;

                    var PurchaseOrderLineIds = SelectedPurchaseOrders.Select(m => m.PurchaseOrderLineId).ToArray();

                    var PurchaseOrderBalanceRecords = (from p in db.ViewPurchaseOrderBalance
                                                       where PurchaseOrderLineIds.Contains(p.PurchaseOrderLineId)
                                                       select p).AsNoTracking().ToList();

                    var PurchaseOrderRecords = (from p in db.PurchaseOrderLine
                                                where PurchaseOrderLineIds.Contains(p.PurchaseOrderLineId)
                                                select p).AsNoTracking().ToList();

                    foreach (var item in SelectedPurchaseOrders)
                    {
                        PurchaseOrderLine orderline = PurchaseOrderRecords.Where(m => m.PurchaseOrderLineId == item.PurchaseOrderLineId).FirstOrDefault();
                        var balorderline = PurchaseOrderBalanceRecords.Where(m => m.PurchaseOrderLineId == item.PurchaseOrderLineId).FirstOrDefault();

                        if (item.CancelQty <= PurchaseOrderBalanceRecords.Where(m => m.PurchaseOrderLineId == item.PurchaseOrderLineId).FirstOrDefault().BalanceQty )
                        {
                            PurchaseOrderCancelLine line = new PurchaseOrderCancelLine();

                            line.PurchaseOrderCancelHeaderId = pt.PurchaseOrderCancelHeaderId;
                            line.PurchaseOrderLineId = item.PurchaseOrderLineId;
                            line.Qty = PurchaseOrderBalanceRecords.Where(m => m.PurchaseOrderLineId == item.PurchaseOrderLineId).FirstOrDefault().BalanceQty;
                            line.Qty = item.CancelQty;
                            line.Sr = Serial++;
                            line.PurchaseOrderCancelLineId = Cnt;
                            line.CreatedDate = DateTime.Now;
                            line.ModifiedDate = DateTime.Now;
                            line.CreatedBy = User.Identity.Name;
                            line.ModifiedBy = User.Identity.Name;
                            LineStatus.Add(line.PurchaseOrderLineId, line.Qty);

                            line.ObjectState = Model.ObjectState.Added;
                            db.PurchaseOrderCancelLine.Add(line);
                            Cnt = Cnt + 1;

                        }
                    }

                    new PurchaseOrderLineStatusService(_unitOfWork).UpdatePurchaseQtyCancelMultiple(LineStatus, pt.DocDate, ref db);

                    try
                    {
                        PurchaseOrderCancelDocEvents.onWizardSaveEvent(this, new PurchaseEventArgs(pt.PurchaseOrderCancelHeaderId, EventModeConstants.Add), ref db);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                        EventException = true;
                    }

                    try
                    {
                        if (EventException)
                        { throw new Exception(); }
                        db.SaveChanges();
                        //_unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        return Json(new { Success = false, Data = message }, JsonRequestBehavior.AllowGet);
                    }

                    try
                    {
                        PurchaseOrderCancelDocEvents.afterWizardSaveEvent(this, new PurchaseEventArgs(pt.PurchaseOrderCancelHeaderId, EventModeConstants.Add), ref db);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = pt.DocTypeId,
                        DocId = pt.PurchaseOrderCancelHeaderId,
                        ActivityType = (int)ActivityTypeContants.WizardCreate,
                        DocNo = pt.DocNo,
                        DocDate = pt.DocDate,
                        DocStatus = pt.Status,
                    }));

                    return Json(new { Success = "URL", Data = "/PurchaseOrderCancelHeader/Submit/" + pt.PurchaseOrderCancelHeaderId }, JsonRequestBehavior.AllowGet);


                }

                else
                    return Json(new { Success = false, Data = "ModelState is Invalid" }, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult SelectedRecords(List<PurchaseOrderCancelWizardViewModel> SelectedRecords)
        {
            var OrderIds = SelectedRecords.Select(m => m.PurchaseOrderLineId).ToArray();
            var RecordDetails = (from p in db.ViewPurchaseOrderBalance
                                 join t in db.PurchaseOrderLine on p.PurchaseOrderLineId equals t.PurchaseOrderLineId
                                 join h in db.PurchaseOrderHeader on t.PurchaseOrderHeaderId equals h.PurchaseOrderHeaderId
                                 where OrderIds.Contains(p.PurchaseOrderLineId)
                                 select new PurchaseOrderCancelWizardViewModel
                                 {
                                     OrderDate = p.OrderDate,
                                     OrderNo = p.PurchaseOrderNo,
                                     PurchaseOrderLineId = p.PurchaseOrderLineId,
                                     BalanceQty = p.BalanceQty,
                                     SupplierName = h.Supplier.Name,
                                     ProductName = t.Product.ProductName,
                                     Dimension1Name = t.Dimension1.Dimension1Name,
                                     Dimension2Name = t.Dimension2.Dimension2Name,
                                     ProductGroupName = t.Product.ProductGroup.ProductGroupName

                                 }).ToList();

            var RecordDetailList = RecordDetails.Select(m => new PurchaseOrderCancelWizardViewModel
            {
                SOrderDate = m.OrderDate.ToString("dd/MMM/yyyy"),
                PurchaseOrderLineId = m.PurchaseOrderLineId,
                OrderNo = m.OrderNo,
                SupplierName = m.SupplierName,
                ProductName = m.ProductName,
                Dimension1Name = m.Dimension1Name,
                Dimension2Name = m.Dimension2Name,
                BalanceQty = m.BalanceQty,
                CancelQty = SelectedRecords.Where(t => t.PurchaseOrderLineId == m.PurchaseOrderLineId).FirstOrDefault().CancelQty,
                ProductGroupName = m.ProductGroupName
            }).ToList();

            return PartialView("_SelectedRecords", RecordDetailList);

        }




        public ActionResult Create(int id)//DocumentTypeId
        {
            PrepareViewBag(id);
            PurchaseOrderCancelHeaderViewModel vm = new PurchaseOrderCancelHeaderViewModel();
            vm.DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            vm.SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

            //Getting Settings
            var settings = new PurchaseOrderSettingService(_unitOfWork).GetPurchaseOrderSettingForDocument(id, vm.DivisionId, vm.SiteId);
            vm.PurchaseOrderSettings = Mapper.Map<PurchaseOrderSetting, PurchaseOrderSettingsViewModel>(settings);
            vm.DocTypeId = id;
            vm.DocDate = DateTime.Now;
            vm.DocNo = new DocumentTypeService(_unitOfWork).FGetNewDocNo("DocNo", ConfigurationManager.AppSettings["DataBaseSchema"] + ".PurchaseOrderCancelHeaders", vm.DocTypeId, vm.DocDate, vm.DivisionId, vm.SiteId);
            ViewBag.Mode = "Add";
            return View("Create", vm);
        }



        public ActionResult Filters(int DocTypeId, DateTime? FromDate, DateTime? ToDate,
            string PurchaseOrderHeaderId, string SupplierId, string ProductId, string Dimension1Id, string Dimension2Id, string ProductGroupId,
            string ProductCategoryId, decimal? BalanceQty, decimal CancelQty, decimal? MultiplierGT, decimal? MultiplierLT, string Sample)
        {
            PurchaseOrderCancelWizardFilterViewModel vm = new PurchaseOrderCancelWizardFilterViewModel();

            List<SelectListItem> tempSOD = new List<SelectListItem>();
            tempSOD.Add(new SelectListItem { Text = "Include Sample", Value = "Include" });
            tempSOD.Add(new SelectListItem { Text = "Exculde Sample", Value = "Exculde" });
            tempSOD.Add(new SelectListItem { Text = "Only Sample", Value = "Only" });

            ViewBag.SOD = new SelectList(tempSOD, "Value", "Text", Sample);


            vm.DocTypeId = DocTypeId;
            vm.FromDate = FromDate;
            vm.ToDate = ToDate;
            vm.PurchaseOrderHeaderId = PurchaseOrderHeaderId;
            vm.SupplierId = SupplierId;
            vm.ProductId = ProductId;
            vm.Dimension1Id = Dimension1Id;
            vm.Dimension2Id = Dimension2Id;
            vm.ProductGroupId = ProductGroupId;
            vm.ProductCategoryId = ProductCategoryId;
            vm.BalanceQty = BalanceQty;
            vm.CancelQty = CancelQty;
            vm.MultiplierGT = MultiplierGT;
            vm.MultiplierLT = MultiplierLT;
            vm.Sample = Sample;
            return PartialView("_Filters", vm);
        }


        public JsonResult GetPendingPurchaseOrdersHelpList(string searchTerm, int pageSize, int pageNum, int filter)//Order Header ID
        {

            var Records = new PurchaseOrderCancelLineService(_unitOfWork).GetPendingOrderHelpList(filter, searchTerm);

            var temp = Records.Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();

            var count = Records.Count();

            ComboBoxPagedResult Data = new ComboBoxPagedResult();
            Data.Results = temp;
            Data.Total = count;

            return new JsonpResult
            {
                Data = Data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        public JsonResult GetPendingSupplierHelpList(string searchTerm, int pageSize, int pageNum, int filter)//Order Header ID
        {
            var Records = new PurchaseOrderCancelLineService(_unitOfWork).GetPendingSupplierHelpList(filter, searchTerm);

            var temp = Records.Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();

            var count = Records.Count();

            ComboBoxPagedResult Data = new ComboBoxPagedResult();
            Data.Results = temp;
            Data.Total = count;

            return new JsonpResult
            {
                Data = Data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        public JsonResult GetPendingProductHelpList(string searchTerm, int pageSize, int pageNum, int filter)//Order Header ID
        {
            var Records = new PurchaseOrderCancelLineService(_unitOfWork).GetPendingProductHelpList(filter, searchTerm);

            var temp = Records.Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();

            var count = Records.Count();

            ComboBoxPagedResult Data = new ComboBoxPagedResult();
            Data.Results = temp;
            Data.Total = count;

            return new JsonpResult
            {
                Data = Data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }



        protected override void Dispose(bool disposing)
        {
            if (!string.IsNullOrEmpty((string)TempData["CSEXC"]))
            {
                CookieGenerator.CreateNotificationCookie(NotificationTypeConstants.Danger, (string)TempData["CSEXC"]);
                TempData.Remove("CSEXC");
            }
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }


}
