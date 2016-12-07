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
using Presentation.Helper;
using Model.ViewModel;
using System.Xml.Linq;
using MaterialTransferDocumentEvents;
using CustomEventArgs;
using DocumentEvents;
using Reports.Controllers;

namespace Web
{

    [Authorize]
    public class MaterialTransferLineController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private bool EventException = false;

        List<string> UserRoles = new List<string>();
        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        bool TimePlanValidation = true;
        string ExceptionMsg = "";
        bool Continue = true;

        IStockLineService _StockLineService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        public MaterialTransferLineController(IStockLineService Stock, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _StockLineService = Stock;
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
            var p = _StockLineService.GetStockLineListForIndex(id).ToList();
            return Json(p, JsonRequestBehavior.AllowGet);

        }
        private void PrepareViewBag(StockLineViewModel vm)
        {
            ViewBag.DeliveryUnitList = new UnitService(_unitOfWork).GetUnitList().ToList();
            StockHeaderViewModel H = new StockHeaderService(_unitOfWork).GetStockHeader(vm.StockHeaderId);
            ViewBag.DocNo = H.DocTypeName + "-" + H.DocNo;
        }

        [HttpGet]
        public ActionResult CreateLine(int id)
        {
            return _Create(id);
        }

        [HttpGet]
        public ActionResult CreateLineAfter_Submit(int id)
        {
            return _Create(id);
        }

        public ActionResult _Create(int Id) //Id ==>Sale Order Header Id
        {
            StockHeader H = new StockHeaderService(_unitOfWork).Find(Id);
            StockLineViewModel s = new StockLineViewModel();

            //Getting Settings
            var settings = new StockHeaderSettingsService(_unitOfWork).GetStockHeaderSettingsForDocument(H.DocTypeId, H.DivisionId, H.SiteId);
            s.StockHeaderSettings = Mapper.Map<StockHeaderSettings, StockHeaderSettingsViewModel>(settings);

            s.StockHeaderId = H.StockHeaderId;
            s.GodownId = H.GodownId;
            s.DocTypeId = H.DocTypeId;
            s.FromGodownId = H.FromGodownId;
            ViewBag.Status = H.Status;
            PrepareViewBag(s);
            if (!string.IsNullOrEmpty((string)TempData["CSEXCL"]))
            {
                ViewBag.CSEXCL = TempData["CSEXCL"];
                TempData["CSEXCL"] = null;
            }
            ViewBag.LineMode = "Create";

            return PartialView("_Create", s);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreatePost(StockLineViewModel svm)
        {
            StockHeader temp = new StockHeaderService(_unitOfWork).Find(svm.StockHeaderId);

            StockLine s = Mapper.Map<StockLineViewModel, StockLine>(svm);

            if (svm.StockHeaderSettings != null)
            {
                if (svm.StockHeaderSettings.isMandatoryProcessLine == true && (svm.FromProcessId <= 0 || svm.FromProcessId == null))
                {
                    ModelState.AddModelError("FromProcessId", "The Process field is required");
                }
            }
            bool BeforeSave = true;
            try
            {

                if (svm.StockLineId <= 0)
                    BeforeSave = MaterialTransferDocEvents.beforeLineSaveEvent(this, new StockEventArgs(svm.StockHeaderId, EventModeConstants.Add), ref db);
                else
                    BeforeSave = MaterialTransferDocEvents.beforeLineSaveEvent(this, new StockEventArgs(svm.StockHeaderId, EventModeConstants.Edit), ref db);

            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXCL"] += message;
                EventException = true;
            }

            if (!BeforeSave)
                ModelState.AddModelError("", "Validation failed before save.");

            if (svm.StockLineId <= 0)
            {
                ViewBag.LineMode = "Create";
            }
            else
            {
                ViewBag.LineMode = "Edit";
            }

            if (ModelState.IsValid && BeforeSave && !EventException)
            {

                if (svm.StockLineId <= 0)
                {
                    //Posting in Stock
                    StockViewModel StockViewModel_Issue = new StockViewModel();
                    StockViewModel_Issue.StockHeaderId = temp.StockHeaderId;
                    StockViewModel_Issue.DocHeaderId = temp.StockHeaderId;
                    StockViewModel_Issue.DocLineId = s.StockLineId;
                    StockViewModel_Issue.DocTypeId = temp.DocTypeId;
                    StockViewModel_Issue.StockHeaderDocDate = temp.DocDate;
                    StockViewModel_Issue.StockDocDate = DateTime.Now.Date;
                    StockViewModel_Issue.DocNo = temp.DocNo;
                    StockViewModel_Issue.DivisionId = temp.DivisionId;
                    StockViewModel_Issue.SiteId = temp.SiteId;
                    StockViewModel_Issue.CurrencyId = null;
                    StockViewModel_Issue.HeaderProcessId = null;
                    StockViewModel_Issue.PersonId = temp.PersonId;
                    StockViewModel_Issue.ProductId = s.ProductId;
                    StockViewModel_Issue.HeaderFromGodownId = temp.FromGodownId;
                    StockViewModel_Issue.HeaderGodownId = temp.FromGodownId;
                    StockViewModel_Issue.GodownId = temp.FromGodownId ?? 0;
                    StockViewModel_Issue.ProcessId = s.FromProcessId;
                    StockViewModel_Issue.LotNo = s.LotNo;
                    StockViewModel_Issue.CostCenterId = temp.CostCenterId;
                    StockViewModel_Issue.Qty_Iss = s.Qty;
                    StockViewModel_Issue.Qty_Rec = 0;
                    StockViewModel_Issue.Weight_Iss = s.Weight;
                    StockViewModel_Issue.Weight_Rec = 0;
                    StockViewModel_Issue.Rate = s.Rate;
                    StockViewModel_Issue.ExpiryDate = null;
                    StockViewModel_Issue.Specification = s.Specification;
                    StockViewModel_Issue.Dimension1Id = s.Dimension1Id;
                    StockViewModel_Issue.Dimension2Id = s.Dimension2Id;
                    StockViewModel_Issue.Remark = s.Remark;
                    StockViewModel_Issue.Status = temp.Status;
                    StockViewModel_Issue.ProductUidId = svm.ProductUidId;
                    StockViewModel_Issue.CreatedBy = temp.CreatedBy;
                    StockViewModel_Issue.CreatedDate = DateTime.Now;
                    StockViewModel_Issue.ModifiedBy = temp.ModifiedBy;
                    StockViewModel_Issue.ModifiedDate = DateTime.Now;

                    string StockPostingError = "";
                    StockPostingError = new StockService(_unitOfWork).StockPostDB(ref StockViewModel_Issue, ref db);

                    if (StockPostingError != "")
                    {
                        ModelState.AddModelError("", StockPostingError);
                        return PartialView("_Create", svm);
                    }

                    s.FromStockId = StockViewModel_Issue.StockId;


                    StockViewModel StockViewModel_Receive = new StockViewModel();
                    StockViewModel_Receive.StockHeaderId = temp.StockHeaderId;
                    StockViewModel_Receive.StockId = -1;
                    StockViewModel_Receive.DocHeaderId = temp.StockHeaderId;
                    StockViewModel_Receive.DocLineId = s.StockLineId;
                    StockViewModel_Receive.DocTypeId = temp.DocTypeId;
                    StockViewModel_Receive.StockHeaderDocDate = temp.DocDate;
                    StockViewModel_Receive.StockDocDate = DateTime.Now.Date;
                    StockViewModel_Receive.DocNo = temp.DocNo;
                    StockViewModel_Receive.DivisionId = temp.DivisionId;
                    StockViewModel_Receive.SiteId = temp.SiteId;
                    StockViewModel_Receive.CurrencyId = null;
                    StockViewModel_Receive.HeaderProcessId = null;
                    StockViewModel_Receive.PersonId = temp.PersonId;
                    StockViewModel_Receive.ProductId = s.ProductId;
                    StockViewModel_Receive.HeaderFromGodownId = temp.FromGodownId;
                    StockViewModel_Receive.HeaderGodownId = temp.GodownId;
                    StockViewModel_Receive.GodownId = temp.GodownId ?? 0;
                    StockViewModel_Receive.ProcessId = s.FromProcessId;
                    StockViewModel_Receive.LotNo = s.LotNo;
                    StockViewModel_Receive.CostCenterId = temp.CostCenterId;
                    StockViewModel_Receive.Qty_Iss = 0;
                    StockViewModel_Receive.Qty_Rec = s.Qty;
                    StockViewModel_Receive.Weight_Iss = 0;
                    StockViewModel_Receive.Weight_Rec = s.Weight;
                    StockViewModel_Receive.Rate = s.Rate;
                    StockViewModel_Receive.ExpiryDate = null;
                    StockViewModel_Receive.Specification = s.Specification;
                    StockViewModel_Receive.Dimension1Id = s.Dimension1Id;
                    StockViewModel_Receive.Dimension2Id = s.Dimension2Id;
                    StockViewModel_Receive.Remark = s.Remark;
                    StockViewModel_Receive.Status = temp.Status;
                    StockViewModel_Receive.ProductUidId = svm.ProductUidId;
                    StockViewModel_Receive.CreatedBy = temp.CreatedBy;
                    StockViewModel_Receive.CreatedDate = DateTime.Now;
                    StockViewModel_Receive.ModifiedBy = temp.ModifiedBy;
                    StockViewModel_Receive.ModifiedDate = DateTime.Now;


                    StockPostingError = new StockService(_unitOfWork).StockPostDB(ref StockViewModel_Receive, ref db);

                    if (StockPostingError != "")
                    {
                        ModelState.AddModelError("", StockPostingError);
                        return PartialView("_Create", svm);
                    }

                    s.StockId = StockViewModel_Receive.StockId;
                    s.CreatedDate = DateTime.Now;
                    s.ModifiedDate = DateTime.Now;
                    s.CreatedBy = User.Identity.Name;
                    s.ModifiedBy = User.Identity.Name;


                    if (s.ProductUidId != null && s.ProductUidId > 0)
                    {
                        //ProductUid Produid = new ProductUidService(_unitOfWork).Find(svm.ProductUidId ?? 0);
                        ProductUid Produid = (from p in db.ProductUid
                                              where p.ProductUIDId == svm.ProductUidId
                                              select p).FirstOrDefault();


                        s.ProductUidLastTransactionDocId = Produid.LastTransactionDocId;
                        s.ProductUidLastTransactionDocDate = Produid.LastTransactionDocDate;
                        s.ProductUidLastTransactionDocNo = Produid.LastTransactionDocNo;
                        s.ProductUidLastTransactionDocTypeId = Produid.LastTransactionDocTypeId;
                        s.ProductUidLastTransactionPersonId = Produid.LastTransactionPersonId;
                        s.ProductUidStatus = Produid.Status;
                        s.ProductUidCurrentProcessId = Produid.CurrenctProcessId;
                        s.ProductUidCurrentGodownId = Produid.CurrenctGodownId;

                        Produid.LastTransactionDocId = temp.StockHeaderId;
                        Produid.LastTransactionDocNo = temp.DocNo;
                        Produid.LastTransactionDocTypeId = temp.DocTypeId;
                        Produid.LastTransactionDocDate = temp.DocDate;
                        Produid.LastTransactionPersonId = null;
                        Produid.CurrenctGodownId = temp.GodownId;
                        Produid.ObjectState = Model.ObjectState.Modified;
                        db.ProductUid.Add(Produid);
                        //new ProductUidService(_unitOfWork).Update(Produid);
                        //db.ProductUid.Add(Produid);
                    }

                    s.ObjectState = Model.ObjectState.Added;
                    db.StockLine.Add(s);
                    //_StockLineService.Create(s);


                    //StockHeader header = new StockHeaderService(_unitOfWork).Find(s.StockHeaderId);
                    if (temp.Status != (int)StatusConstants.Drafted && temp.Status != (int)StatusConstants.Import)
                    {
                        temp.Status = (int)StatusConstants.Modified;
                        temp.ModifiedBy = User.Identity.Name;
                        temp.ModifiedDate = DateTime.Now;
                        temp.ObjectState = Model.ObjectState.Modified;
                        db.StockHeader.Add(temp);
                        //new StockHeaderService(_unitOfWork).Update(temp);
                    }

                    try
                    {
                        MaterialTransferDocEvents.onLineSaveEvent(this, new StockEventArgs(s.StockHeaderId, s.StockLineId, EventModeConstants.Add), ref db);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXCL"] += message;
                        EventException = true;
                    }

                    try
                    {
                        if (EventException)
                        { throw new Exception(); }
                        db.SaveChanges();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXCL"] += message;
                        PrepareViewBag(svm);
                        return PartialView("_Create", svm);
                    }

                    try
                    {
                        MaterialTransferDocEvents.afterLineSaveEvent(this, new StockEventArgs(s.StockHeaderId, s.StockLineId, EventModeConstants.Add), ref db);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXCL"] += message;
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = temp.DocTypeId,
                        DocId = temp.StockHeaderId,
                        DocLineId = s.StockLineId,
                        ActivityType = (int)ActivityTypeContants.Added,
                        DocNo = temp.DocNo,
                        DocDate = temp.DocDate,
                        DocStatus = temp.Status,
                    }));

                    return RedirectToAction("_Create", new { id = svm.StockHeaderId });

                }


                else
                {
                    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();
                    int status = temp.Status;

                    StockLine templine = _StockLineService.Find(s.StockLineId);

                    StockLine ExRec = new StockLine();
                    ExRec = Mapper.Map<StockLine>(templine);

                    templine.ProductId = s.ProductId;
                    templine.ProductUidId = s.ProductUidId;
                    templine.RequisitionLineId = s.RequisitionLineId;
                    templine.Specification = s.Specification;
                    templine.Dimension1Id = s.Dimension1Id;
                    templine.Dimension2Id = s.Dimension2Id;
                    templine.Rate = s.Rate;
                    templine.Amount = s.Amount;
                    templine.LotNo = s.LotNo;
                    templine.FromProcessId = s.FromProcessId;
                    templine.Remark = s.Remark;
                    templine.Qty = s.Qty;
                    templine.Weight  = s.Weight;
                    templine.Remark = s.Remark;

                    templine.ModifiedDate = DateTime.Now;
                    templine.ModifiedBy = User.Identity.Name;
                    templine.ObjectState = Model.ObjectState.Modified;
                    db.StockLine.Add(templine);
                    //_StockLineService.Update(templine);

                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = ExRec,
                        Obj = templine,
                    });



                    if (templine.FromStockId != null)
                    {
                        StockViewModel StockViewModel_Issue = new StockViewModel();
                        StockViewModel_Issue.StockHeaderId = temp.StockHeaderId;
                        StockViewModel_Issue.StockId = templine.FromStockId ?? 0;
                        StockViewModel_Issue.DocHeaderId = templine.StockHeaderId;
                        StockViewModel_Issue.DocLineId = templine.StockLineId;
                        StockViewModel_Issue.DocTypeId = temp.DocTypeId;
                        StockViewModel_Issue.StockHeaderDocDate = temp.DocDate;
                        StockViewModel_Issue.StockDocDate = templine.CreatedDate.Date;
                        StockViewModel_Issue.DocNo = temp.DocNo;
                        StockViewModel_Issue.DivisionId = temp.DivisionId;
                        StockViewModel_Issue.SiteId = temp.SiteId;
                        StockViewModel_Issue.CurrencyId = null;
                        StockViewModel_Issue.HeaderProcessId = temp.ProcessId;
                        StockViewModel_Issue.PersonId = temp.PersonId;
                        StockViewModel_Issue.ProductId = s.ProductId;
                        StockViewModel_Issue.HeaderFromGodownId = null;
                        StockViewModel_Issue.HeaderGodownId = temp.FromGodownId;
                        StockViewModel_Issue.GodownId = temp.FromGodownId ?? 0;
                        StockViewModel_Issue.ProcessId = templine.FromProcessId;
                        StockViewModel_Issue.LotNo = templine.LotNo;
                        StockViewModel_Issue.CostCenterId = temp.CostCenterId;
                        StockViewModel_Issue.Qty_Iss = s.Qty;
                        StockViewModel_Issue.Qty_Rec = 0;
                        StockViewModel_Issue.Weight_Iss = s.Weight;
                        StockViewModel_Issue.Weight_Rec = 0;
                        StockViewModel_Issue.Rate = templine.Rate;
                        StockViewModel_Issue.ExpiryDate = null;
                        StockViewModel_Issue.Specification = templine.Specification;
                        StockViewModel_Issue.Dimension1Id = templine.Dimension1Id;
                        StockViewModel_Issue.Dimension2Id = templine.Dimension2Id;
                        StockViewModel_Issue.Remark = s.Remark;
                        StockViewModel_Issue.ProductUidId = s.ProductUidId;
                        StockViewModel_Issue.Status = temp.Status;
                        StockViewModel_Issue.CreatedBy = templine.CreatedBy;
                        StockViewModel_Issue.CreatedDate = templine.CreatedDate;
                        StockViewModel_Issue.ModifiedBy = User.Identity.Name;
                        StockViewModel_Issue.ModifiedDate = DateTime.Now;

                        string StockPostingError = "";
                        StockPostingError = new StockService(_unitOfWork).StockPostDB(ref StockViewModel_Issue, ref db);

                        if (StockPostingError != "")
                        {
                            ModelState.AddModelError("", StockPostingError);
                            return PartialView("_Create", svm);
                        }
                    }


                    if (templine.StockId != null)
                    {
                        StockViewModel StockViewModel_Receive = new StockViewModel();
                        StockViewModel_Receive.StockHeaderId = temp.StockHeaderId;
                        StockViewModel_Receive.StockId = templine.StockId ?? 0;
                        StockViewModel_Receive.DocHeaderId = temp.StockHeaderId;
                        StockViewModel_Receive.DocLineId = s.StockLineId;
                        StockViewModel_Receive.DocTypeId = temp.DocTypeId;
                        StockViewModel_Receive.StockHeaderDocDate = temp.DocDate;
                        StockViewModel_Receive.StockDocDate = DateTime.Now.Date;
                        StockViewModel_Receive.DocNo = temp.DocNo;
                        StockViewModel_Receive.DivisionId = temp.DivisionId;
                        StockViewModel_Receive.SiteId = temp.SiteId;
                        StockViewModel_Receive.CurrencyId = null;
                        StockViewModel_Receive.HeaderProcessId = null;
                        StockViewModel_Receive.PersonId = temp.PersonId;
                        StockViewModel_Receive.ProductId = s.ProductId;
                        StockViewModel_Receive.HeaderFromGodownId = temp.FromGodownId;
                        StockViewModel_Receive.HeaderGodownId = temp.GodownId;
                        StockViewModel_Receive.GodownId = temp.GodownId ?? 0;
                        StockViewModel_Receive.ProcessId = s.FromProcessId;
                        StockViewModel_Receive.LotNo = s.LotNo;
                        StockViewModel_Receive.CostCenterId = temp.CostCenterId;
                        StockViewModel_Receive.Qty_Iss = 0;
                        StockViewModel_Receive.Qty_Rec = s.Qty;
                        StockViewModel_Receive.Weight_Iss = 0;
                        StockViewModel_Receive.Weight_Rec = s.Weight;
                        StockViewModel_Receive.Rate = s.Rate;
                        StockViewModel_Receive.ExpiryDate = null;
                        StockViewModel_Receive.Specification = s.Specification;
                        StockViewModel_Receive.Dimension1Id = s.Dimension1Id;
                        StockViewModel_Receive.Dimension2Id = s.Dimension2Id;
                        StockViewModel_Receive.Remark = s.Remark;
                        StockViewModel_Receive.ProductUidId = s.ProductUidId;
                        StockViewModel_Receive.Status = temp.Status;
                        StockViewModel_Receive.CreatedBy = temp.CreatedBy;
                        StockViewModel_Receive.CreatedDate = DateTime.Now;
                        StockViewModel_Receive.ModifiedBy = temp.ModifiedBy;
                        StockViewModel_Receive.ModifiedDate = DateTime.Now;

                        string StockPostingError = "";
                        StockPostingError = new StockService(_unitOfWork).StockPostDB(ref StockViewModel_Receive, ref db);

                        if (StockPostingError != "")
                        {
                            ModelState.AddModelError("", StockPostingError);
                            return PartialView("_Create", svm);
                        }
                    }


                    if (temp.Status != (int)StatusConstants.Drafted && temp.Status != (int)StatusConstants.Import)
                    {
                        temp.Status = (int)StatusConstants.Modified;
                        temp.ModifiedDate = DateTime.Now;
                        temp.ModifiedBy = User.Identity.Name;
                        temp.ObjectState = Model.ObjectState.Modified;
                        db.StockHeader.Add(temp);
                        //new StockHeaderService(_unitOfWork).Update(temp);
                    }

                    XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);

                    try
                    {
                        MaterialTransferDocEvents.onLineSaveEvent(this, new StockEventArgs(s.StockHeaderId, templine.StockLineId, EventModeConstants.Edit), ref db);
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
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXCL"] += message;
                        PrepareViewBag(svm);
                        return PartialView("_Create", svm);
                    }

                    try
                    {
                        MaterialTransferDocEvents.afterLineSaveEvent(this, new StockEventArgs(s.StockHeaderId, templine.StockLineId, EventModeConstants.Edit), ref db);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                    }

                    //Saving the Activity Log

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = temp.DocTypeId,
                        DocId = templine.StockHeaderId,
                        DocLineId = templine.StockLineId,
                        ActivityType = (int)ActivityTypeContants.Modified,
                        DocNo = temp.DocNo,
                        xEModifications = Modifications,
                        DocDate = temp.DocDate,
                        DocStatus = temp.Status,
                    }));                   
                    //End of Saving the Activity Log

                    return Json(new { success = true });
                }

            }
            PrepareViewBag(svm);
            return PartialView("_Create", svm);
        }

        [HttpGet]
        public ActionResult _ModifyLine(int id)
        {
            return _Modify(id);
        }

        [HttpGet]
        public ActionResult _ModifyLineAfterSubmit(int id)
        {
            return _Modify(id);
        }

        private ActionResult _Modify(int id)
        {
            StockLineViewModel temp = _StockLineService.GetStockLine(id);

            if (temp == null)
            {
                return HttpNotFound();
            }

            #region DocTypeTimeLineValidation
            try
            {

                TimePlanValidation = DocumentValidation.ValidateDocumentLine(new DocumentUniqueId { LockReason = temp.LockReason }, User.Identity.Name, out ExceptionMsg, out Continue);

            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXCL"] += message;
                TimePlanValidation = false;
            }

            if (!TimePlanValidation)
                TempData["CSEXCL"] += ExceptionMsg;
            #endregion

            if ((TimePlanValidation || Continue))
                ViewBag.LineMode = "Edit";

            StockHeader H = new StockHeaderService(_unitOfWork).Find(temp.StockHeaderId);

            //Getting Settings
            var settings = new StockHeaderSettingsService(_unitOfWork).GetStockHeaderSettingsForDocument(H.DocTypeId, H.DivisionId, H.SiteId);
            temp.StockHeaderSettings = Mapper.Map<StockHeaderSettings, StockHeaderSettingsViewModel>(settings);
            temp.GodownId = H.GodownId;
            temp.DocTypeId = H.DocTypeId;
            PrepareViewBag(temp);
            return PartialView("_Create", temp);
        }

        public ActionResult _Detail(int id)
        {
            StockLineViewModel temp = _StockLineService.GetStockLine(id);

            if (temp == null)
            {
                return HttpNotFound();
            }          

            StockHeader H = new StockHeaderService(_unitOfWork).Find(temp.StockHeaderId);

            //Getting Settings
            var settings = new StockHeaderSettingsService(_unitOfWork).GetStockHeaderSettingsForDocument(H.DocTypeId, H.DivisionId, H.SiteId);
            temp.StockHeaderSettings = Mapper.Map<StockHeaderSettings, StockHeaderSettingsViewModel>(settings);
            temp.GodownId = H.GodownId;
            PrepareViewBag(temp);
            return PartialView("_Create", temp);
        }

        [HttpGet]
        public ActionResult _DeleteLine(int id)
        {
            return _Delete(id);
        }
        [HttpGet]
        public ActionResult _DeleteLine_AfterSubmit(int id)
        {
            return _Delete(id);
        }


        private ActionResult _Delete(int id)
        {
            StockLineViewModel temp = _StockLineService.GetStockLine(id);

            if (temp == null)
            {
                return HttpNotFound();
            }

            #region DocTypeTimeLineValidation
            try
            {

                TimePlanValidation = DocumentValidation.ValidateDocumentLine(new DocumentUniqueId { LockReason = temp.LockReason }, User.Identity.Name, out ExceptionMsg, out Continue);

            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXCL"] += message;
                TimePlanValidation = false;
            }

            if (!TimePlanValidation)
                TempData["CSEXCL"] += ExceptionMsg;
            #endregion

            if ((TimePlanValidation || Continue))
                ViewBag.LineMode = "Delete";

            StockHeader H = new StockHeaderService(_unitOfWork).Find(temp.StockHeaderId);

            //Getting Settings
            var settings = new StockHeaderSettingsService(_unitOfWork).GetStockHeaderSettingsForDocument(H.DocTypeId, H.DivisionId, H.SiteId);
            temp.StockHeaderSettings = Mapper.Map<StockHeaderSettings, StockHeaderSettingsViewModel>(settings);
            temp.GodownId = H.GodownId;
            PrepareViewBag(temp);
            return PartialView("_Create", temp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(StockLineViewModel vm)
        {
            bool BeforeSave = true;
            try
            {
                BeforeSave = MaterialTransferDocEvents.beforeLineDeleteEvent(this, new StockEventArgs(vm.StockHeaderId, vm.StockLineId), ref db);
            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                EventException = true;
            }

            if (!BeforeSave)
                TempData["CSEXC"] += "Validation failed before delete.";

            if (BeforeSave && !EventException)
            {

                List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

                int? FromStockId = 0;
                int? StockId = 0;
                int? ProdUid = 0;

                StockLine StockLine = (from p in db.StockLine
                                       where p.StockLineId == vm.StockLineId
                                       select p).FirstOrDefault();

                StockHeader header = new StockHeaderService(_unitOfWork).Find(StockLine.StockHeaderId);

                StockLine ExRec = new StockLine();
                ExRec = Mapper.Map<StockLine>(StockLine);

                LogList.Add(new LogTypeViewModel
                {
                    ExObj = ExRec,
                });

                FromStockId = StockLine.FromStockId;
                StockId = StockLine.StockId;
                ProdUid = StockLine.ProductUidId;

                if (ProdUid != null && ProdUid != 0)
                {
                    Service.ProductUidDetail ProductUidDetail = new ProductUidService(_unitOfWork).FGetProductUidLastValues((int)ProdUid, "Stock Transfer-" + vm.StockHeaderId.ToString());

                    ProductUid ProductUid = (from p in db.ProductUid
                                             where p.ProductUIDId == vm.ProductUidId
                                             select p).FirstOrDefault();


                    if (header.StockHeaderId != ProductUid.LastTransactionDocId)
                    {
                        ModelState.AddModelError("", "Bar Code Can't be deleted because this is already issue to another process.");
                        PrepareViewBag(vm);
                        return PartialView("_Create", vm);
                    }


                    ProductUid.LastTransactionDocDate = StockLine.ProductUidLastTransactionDocDate;
                    ProductUid.LastTransactionDocId = StockLine.ProductUidLastTransactionDocId;
                    ProductUid.LastTransactionDocNo = StockLine.ProductUidLastTransactionDocNo;
                    ProductUid.LastTransactionDocTypeId = StockLine.ProductUidLastTransactionDocTypeId;
                    ProductUid.LastTransactionPersonId = StockLine.ProductUidLastTransactionPersonId;
                    ProductUid.CurrenctGodownId = StockLine.ProductUidCurrentGodownId;
                    ProductUid.CurrenctProcessId = StockLine.ProductUidCurrentProcessId;
                    ProductUid.Status = StockLine.ProductUidStatus;




                    //ProductUid.LastTransactionDocDate = ProductUidDetail.LastTransactionDocDate;
                    //ProductUid.LastTransactionDocId = ProductUidDetail.LastTransactionDocId;
                    //ProductUid.LastTransactionDocNo = ProductUidDetail.LastTransactionDocNo;
                    //ProductUid.LastTransactionDocTypeId = ProductUidDetail.LastTransactionDocTypeId;
                    //ProductUid.LastTransactionPersonId = ProductUidDetail.LastTransactionPersonId;
                    //ProductUid.CurrenctGodownId = ProductUidDetail.CurrenctGodownId;
                    //ProductUid.CurrenctProcessId = ProductUidDetail.CurrenctProcessId;
                    ProductUid.ObjectState = Model.ObjectState.Modified;
                    db.ProductUid.Add(ProductUid);

                    //new ProductUidService(_unitOfWork).Update(ProductUid);
                    //db.ProductUid.Add(ProductUid);

                    new StockUidService(_unitOfWork).DeleteStockUidForDocLineDB(vm.StockHeaderId, header.DocTypeId, header.SiteId, header.DivisionId, ref db);
                }

                StockLine.ObjectState = Model.ObjectState.Deleted;
                db.StockLine.Remove(StockLine);
                //_StockLineService.Delete(StockLine);


                if (FromStockId != null)
                {
                    new StockService(_unitOfWork).DeleteStockDB((int)FromStockId, ref db, true);
                }

                if (StockId != null)
                {
                    new StockService(_unitOfWork).DeleteStockDB((int)StockId, ref db, true);
                }




                if (header.Status != (int)StatusConstants.Drafted && header.Status != (int)StatusConstants.Import)
                {
                    header.Status = (int)StatusConstants.Modified;
                    header.ObjectState = Model.ObjectState.Modified;
                    db.StockHeader.Add(header);
                    //new StockHeaderService(_unitOfWork).Update(header);
                }

                XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);

                try
                {
                    MaterialTransferDocEvents.onLineDeleteEvent(this, new StockEventArgs(StockLine.StockHeaderId, StockLine.StockLineId), ref db);
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
                        throw new Exception();

                    db.SaveChanges();
                }

                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXCL"] += message;
                    ViewBag.LineMode = "Delete";
                    return PartialView("_Create", vm);
                }

                try
                {
                    MaterialTransferDocEvents.afterLineDeleteEvent(this, new StockEventArgs(StockLine.StockHeaderId, StockLine.StockLineId), ref db);
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                }

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = header.DocTypeId,
                    DocId = header.StockHeaderId,
                    DocLineId = StockLine.StockLineId,
                    ActivityType = (int)ActivityTypeContants.Deleted,
                    DocNo = header.DocNo,
                    xEModifications = Modifications,
                    DocDate = header.DocDate,
                    DocStatus = header.Status,
                }));              

            }

            return Json(new { success = true });

        }

        public JsonResult GetProductDetailJson(int ProductId, int StockId)
        {
            ProductViewModel product = new ProductService(_unitOfWork).GetProduct(ProductId);

            return Json(new { ProductId = product.ProductId, StandardCost = product.StandardCost, UnitId = product.UnitId });
        }

        public JsonResult GetPendingProdOrders(int ProductId)
        {
            return Json(new ProdOrderHeaderService(_unitOfWork).GetPendingProdOrders(ProductId).ToList());
        }

        public JsonResult GetProdOrderDetail(int ProdOrderLineId)
        {
            return Json(new ProdOrderLineService(_unitOfWork).GetProdOrderDetailBalance(ProdOrderLineId));
        }

        public ActionResult GetCustomProducts(string searchTerm, int pageSize, int pageNum, int filter)//DocTypeId
        {

            var temp = _StockLineService.GetCustomProducts(filter, searchTerm).Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();

            var count = _StockLineService.GetCustomProducts(filter, searchTerm).Count();

            ComboBoxPagedResult Data = new ComboBoxPagedResult();
            Data.Results = temp;
            Data.Total = count;

            return new JsonpResult
            {
                Data = Data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetExcessStock(int ProductId, int? Dim1, int? Dim2, int? ProcId, string Lot, int MaterialIssueId, string ProcName)
        {
            return Json(_StockLineService.GetExcessStockForTransfer(ProductId, Dim1, Dim2, ProcId, Lot, MaterialIssueId, ProcName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductPrevProcess(int ProductId, int GodownId, int DocTypeId)
        {
            ProductPrevProcess ProductPrevProcess = new ProductService(_unitOfWork).FGetProductPrevProcess(ProductId, GodownId, DocTypeId);
            List<ProductPrevProcess> ProductPrevProcessJson = new List<ProductPrevProcess>();

            

            if (ProductPrevProcess != null)
            {
                string ProcessName = new ProcessService(_unitOfWork).Find((int)ProductPrevProcess.ProcessId).ProcessName;
                ProductPrevProcessJson.Add(new ProductPrevProcess()
                {
                    ProcessId = ProductPrevProcess.ProcessId,
                    ProcessName = ProcessName
                });
                return Json(ProductPrevProcessJson);
            }
            else
            {
                return null;
            }

        }

        public JsonResult GetProductCodeDetailJson(string ProductCode)
        {
            Product Product = (from P in db.Product
                               where P.ProductCode == ProductCode
                               select P).FirstOrDefault();

            if (Product != null)
            {
                return Json(Product);
            }
            else
            {
                return null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!string.IsNullOrEmpty((string)TempData["CSEXC"]))
                CookieGenerator.CreateNotificationCookie(NotificationTypeConstants.Danger, (string)TempData["CSEXC"]);

            TempData["CSEXC"] = null;


            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
