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
using LedgerDocumentEvents;
using CustomEventArgs;
using DocumentEvents;
using Reports.Controllers;

namespace Web
{

    [Authorize]
    public class LedgerLineController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private bool EventException = false;

        List<string> UserRoles = new List<string>();
        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        bool TimePlanValidation = true;
        string ExceptionMsg = "";
        bool Continue = true;

        ILedgerService _LedgerService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        public LedgerLineController(ILedgerService SaleOrder, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _LedgerService = SaleOrder;
            _unitOfWork = unitOfWork;
            _exception = exec;

            UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;
        }

        public void PrepareViewBag(int Id)
        {
            LedgerHeader Header = new LedgerHeaderService(_unitOfWork).Find(Id);
            ViewBag.Nature = new DocumentTypeService(_unitOfWork).Find(Header.DocTypeId).Nature;
            if (Header.LedgerAccountId != null)
            {
                ViewBag.LedgerAccountNature = new LedgerAccountService(_unitOfWork).GetLedgerAccountnature((int)Header.LedgerAccountId);
            }
        }

        [HttpGet]
        public JsonResult Index(int id)
        {
            var p = _LedgerService.GetLineListForReceiptVoucher(id).ToList();
            return Json(p, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult CreateLine(int Id, int Laid, int catid, DateTime? tempduedate, string tempchequeno)
        {
            return _Create(Id, Laid, catid, tempduedate, tempchequeno, null);
        }

        [HttpGet]
        public ActionResult CreateLineAfter_Submit(int Id, int Laid, int catid, DateTime? tempduedate, string tempchequeno)
        {
            return _Create(Id, Laid, catid, tempduedate, tempchequeno, null);
        }


        public ActionResult _Create(int Id, int Laid, int catid, DateTime? tempduedate, string tempchequeno, int? TAID) //Id ==>Sale Order Header Id
        {
            LedgersViewModel s = new LedgersViewModel();
            ViewBag.AccountType = new LedgerHeaderService(_unitOfWork).GetLedgerAccountType(Laid);
            LedgerHeader H = db.LedgerHeader.Find(Id);
            db.Entry<LedgerHeader>(H).Reference(m => m.LedgerAccount).Load();

            ViewBag.LedgerAccountName = H.LedgerAccount.LedgerAccountName;

            var LedgerLine = (from p in db.LedgerLine
                              where p.LedgerHeaderId == Id
                              orderby p.LedgerLineId descending
                              select new
                              {
                                  Name = p.LedgerAccount.LedgerAccountName,
                                  Amount = p.Amount,
                              }).FirstOrDefault();

            if (LedgerLine != null)
                ViewBag.LedgerLastTransaction = "Last Line -Name : " + LedgerLine.Name + ", " + "Amount : " + LedgerLine.Amount.ToString("0.00");

            PrepareViewBag(Id);
            s.DocumentCategoryId = catid;
            s.LedgerHeaderId = Id;
            s.ContraLedgerAccountId = Laid;
            s.DueDate = tempduedate;
            if (!string.IsNullOrEmpty(tempchequeno))
            {
                int id;
                int.TryParse(tempchequeno, out id);
                s.ChqNo = (id).ToString();
            }

            if (TAID.HasValue && TAID.Value > 0)
            {
                ViewBag.TAID = TAID;
            }

            var settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(H.DocTypeId, H.DivisionId, H.SiteId);
            s.LedgerSetting = Mapper.Map<LedgerSetting, LedgerSettingViewModel>(settings);
            ViewBag.LineMode = "Create";
            return PartialView("_Create", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreatePost(LedgersViewModel svm)
        {
            LedgerHeader header = new LedgerHeaderService(_unitOfWork).Find(svm.LedgerHeaderId);
            string Nature = new DocumentTypeService(_unitOfWork).Find(header.DocTypeId).Nature;
            //Ledger line = Mapper.Map<LedgersViewModel, Ledger>(svm);

            if (svm.LedgerLineId <= 0)
            {
                ViewBag.LineMode = "Create";
            }
            else
            {
                ViewBag.LineMode = "Edit";
            }

            if (svm.LedgerSetting != null)
            {
                if (svm.LedgerSetting.isVisibleChqNo && svm.LedgerSetting.isMandatoryChqNo == true && (string.IsNullOrEmpty(svm.ChqNo)))
                {
                    ModelState.AddModelError("ChqNo", "The ChqNo field is required");
                }
                if (svm.LedgerSetting.isVisibleLineCostCenter == true && svm.LedgerSetting.isMandatoryLineCostCenter == true && !svm.CostCenterId.HasValue)
                {
                    ModelState.AddModelError("CostCenterId", "The CostCenter field is required");
                }
                if (svm.LedgerSetting.isVisibleRefNo == true && svm.LedgerSetting.isMandatoryRefNo == true && !svm.ReferenceId.HasValue)
                {
                    ModelState.AddModelError("ReferenceId", "The Reference No field is required");
                }
            }

            bool BeforeSave = true;
            try
            {

                if (svm.LedgerHeaderId <= 0)
                    BeforeSave = LedgerDocEvents.beforeLineSaveEvent(this, new LedgerEventArgs(svm.LedgerHeaderId, EventModeConstants.Add), ref db);
                else
                    BeforeSave = LedgerDocEvents.beforeLineSaveEvent(this, new LedgerEventArgs(svm.LedgerHeaderId, EventModeConstants.Edit), ref db);

            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXCL"] += message;
                EventException = true;
            }

            if (!BeforeSave)
                ModelState.AddModelError("", "Validation failed before save.");

            if (ModelState.IsValid && BeforeSave && !EventException)
            {
                if (svm.LedgerLineId <= 0)
                {
                    LedgerLine LedgerLine = new LedgerLine();

                    LedgerLine.LedgerHeaderId = header.LedgerHeaderId;
                    LedgerLine.LedgerAccountId = svm.LedgerAccountId;
                    LedgerLine.Amount = svm.Amount;
                    LedgerLine.ChqNo = svm.ChqNo;
                    LedgerLine.ChqDate = svm.DueDate;
                    LedgerLine.CostCenterId = svm.CostCenterId;
                    LedgerLine.BaseRate = svm.BaseRate;
                    LedgerLine.BaseValue = svm.BaseValue;
                    LedgerLine.ReferenceId = svm.ReferenceId;
                    LedgerLine.ProductUidId = svm.ProductUidId;
                    LedgerLine.CreatedDate = DateTime.Now;
                    LedgerLine.ModifiedDate = DateTime.Now;
                    LedgerLine.CreatedBy = User.Identity.Name;
                    LedgerLine.Remark = svm.Remark;
                    LedgerLine.ModifiedBy = User.Identity.Name;
                    LedgerLine.ObjectState = Model.ObjectState.Added;
                    db.LedgerLine.Add(LedgerLine);
                    //new LedgerLineService(_unitOfWork).Create(LedgerLine);


                    var FromCosterName = db.CostCenter.Find(header.CostCenterId);
                    var ToCosterName = db.CostCenter.Find(svm.CostCenterId);

                    if (header.DocTypeId == 286)
                    {
                        header.Narration = header.Narration + " Form Costcenter " + FromCosterName.CostCenterName + " To " + ToCosterName.CostCenterName;
                    }

                    if (header.Status != (int)StatusConstants.Drafted)
                    {
                        header.Status = (int)StatusConstants.Modified;
                        header.ModifiedBy = User.Identity.Name;
                        header.ModifiedDate = DateTime.Now;
                        //new LedgerHeaderService(_unitOfWork).Update(header);
                        header.ObjectState = Model.ObjectState.Modified;
                        db.LedgerHeader.Add(header);
                    }

                    if (LedgerLine.CostCenterId.HasValue && LedgerLine.CostCenterId.Value != 0)
                    {
                        var CostCenterStatus = db.CostCenterStatus.Find(LedgerLine.CostCenterId);

                        if (Nature == NatureConstants.Debit)
                        {
                            CostCenterStatus.AmountCr = (CostCenterStatus.AmountCr ?? 0) + LedgerLine.Amount;
                            CostCenterStatus.ObjectState = Model.ObjectState.Modified;
                            db.CostCenterStatus.Add(CostCenterStatus);
                            if (header.CostCenterId.HasValue && header.CostCenterId.Value != 0)
                            {
                                var HeaderCostCenterStatus = db.CostCenterStatus.Find(header.CostCenterId);
                                HeaderCostCenterStatus.AmountDr = (HeaderCostCenterStatus.AmountDr ?? 0) + LedgerLine.Amount;
                                HeaderCostCenterStatus.ObjectState = Model.ObjectState.Modified;
                                db.CostCenterStatus.Add(HeaderCostCenterStatus);
                            }
                        }
                        else
                        {
                            CostCenterStatus.AmountDr = (CostCenterStatus.AmountDr ?? 0) + LedgerLine.Amount;
                            CostCenterStatus.ObjectState = Model.ObjectState.Modified;
                            db.CostCenterStatus.Add(CostCenterStatus);
                            if (header.CostCenterId.HasValue && header.CostCenterId.Value != 0)
                            {
                                var HeaderCostCenterStatus = db.CostCenterStatus.Find(header.CostCenterId);
                                HeaderCostCenterStatus.AmountCr = (HeaderCostCenterStatus.AmountCr ?? 0) + LedgerLine.Amount;
                                HeaderCostCenterStatus.ObjectState = Model.ObjectState.Modified;
                                db.CostCenterStatus.Add(HeaderCostCenterStatus);
                            }
                        }
                    }


                    #region LedgerSave
                    Ledger Ledger = new Ledger();

                    if (Nature == NatureConstants.Credit)
                    {
                        Ledger.AmtDr = LedgerLine.Amount;

                    }
                    else if (Nature == NatureConstants.Debit)
                    {
                        Ledger.AmtCr = LedgerLine.Amount;
                    }
                    Ledger.ChqNo = LedgerLine.ChqNo;
                    Ledger.ContraLedgerAccountId = header.LedgerAccountId;
                    Ledger.CostCenterId = LedgerLine.CostCenterId;
                    Ledger.DueDate = LedgerLine.ChqDate;
                    Ledger.LedgerAccountId = LedgerLine.LedgerAccountId;
                    Ledger.LedgerHeaderId = LedgerLine.LedgerHeaderId;
                    Ledger.LedgerLineId = LedgerLine.LedgerLineId;
                    Ledger.Narration = header.Narration + LedgerLine.Remark;
                    Ledger.ObjectState = Model.ObjectState.Added;
                    Ledger.LedgerId = 1;
                    db.Ledger.Add(Ledger);

                    if (LedgerLine.ReferenceId != null)
                    {
                        LedgerAdj LedgerAdj = new LedgerAdj();
                        if (Nature == NatureConstants.Credit)
                        {
                            LedgerAdj.LedgerId = (int)LedgerLine.ReferenceId;
                            LedgerAdj.DrLedgerId = Ledger.LedgerId;
                            LedgerAdj.CrLedgerId = null;
                        }
                        else
                        {
                            LedgerAdj.LedgerId = (int)LedgerLine.ReferenceId;
                            LedgerAdj.CrLedgerId = Ledger.LedgerId;
                            LedgerAdj.DrLedgerId = null;
                        }

                        LedgerAdj.Amount = LedgerLine.Amount;
                        LedgerAdj.SiteId = header.SiteId;
                        LedgerAdj.CreatedDate = DateTime.Now;
                        LedgerAdj.ModifiedDate = DateTime.Now;
                        LedgerAdj.CreatedBy = User.Identity.Name;
                        LedgerAdj.ModifiedBy = User.Identity.Name;
                        LedgerAdj.ObjectState = Model.ObjectState.Added;
                        db.LedgerAdj.Add(LedgerAdj);
                    }
                    #endregion


                    #region ContraLedgerSave
                    Ledger ContraLedger = new Ledger();
                    if (Nature == NatureConstants.Credit)
                    {
                        ContraLedger.AmtCr = LedgerLine.Amount;

                    }
                    else if (Nature == NatureConstants.Debit)
                    {
                        ContraLedger.AmtDr = LedgerLine.Amount;
                    }
                    ContraLedger.LedgerHeaderId = header.LedgerHeaderId;
                    ContraLedger.CostCenterId = header.CostCenterId;
                    ContraLedger.LedgerLineId = LedgerLine.LedgerLineId;
                    ContraLedger.LedgerAccountId = header.LedgerAccountId.Value;
                    ContraLedger.ContraLedgerAccountId = LedgerLine.LedgerAccountId;
                    ContraLedger.ObjectState = Model.ObjectState.Added;
                    db.Ledger.Add(ContraLedger);
                    #endregion

                    try
                    {
                        LedgerDocEvents.onLineSaveEvent(this, new LedgerEventArgs(LedgerLine.LedgerHeaderId, LedgerLine.LedgerLineId, EventModeConstants.Add), ref db);
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
                        PrepareViewBag(header.LedgerHeaderId);
                        ViewBag.AccountType = new LedgerHeaderService(_unitOfWork).GetLedgerAccountType(svm.ContraLedgerAccountId ?? 0);
                        return PartialView("_Create", svm);
                    }

                    try
                    {
                        LedgerDocEvents.afterLineSaveEvent(this, new LedgerEventArgs(LedgerLine.LedgerHeaderId, LedgerLine.LedgerLineId, EventModeConstants.Add), ref db);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXCL"] += message;
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = header.DocTypeId,
                        DocId = header.LedgerHeaderId,
                        DocLineId = LedgerLine.LedgerLineId,
                        ActivityType = (int)ActivityTypeContants.Added,
                        DocNo = header.DocNo,
                        DocDate = header.DocDate,
                        DocStatus = header.Status,
                    }));

                    return RedirectToAction("_Create", new { id = LedgerLine.LedgerHeaderId, Laid = svm.ContraLedgerAccountId, catid = svm.DocumentCategoryId, tempchequeno = svm.ChqNo, TAID = LedgerLine.LedgerAccountId });
                }


                else //Edit Mode 
                {
                    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();
                    int status = header.Status;

                    LedgerLine LedgerLine = new LedgerLineService(_unitOfWork).Find(svm.LedgerLineId);
                    int XLedgerAccountId = LedgerLine.LedgerAccountId;
                    LedgerLine ExRec = Mapper.Map<LedgerLine>(LedgerLine);

                    LedgerLine.LedgerAccountId = svm.LedgerAccountId;
                    LedgerLine.Amount = svm.Amount;
                    LedgerLine.ChqNo = svm.ChqNo;
                    LedgerLine.CostCenterId = svm.CostCenterId;
                    LedgerLine.Remark = svm.Remark;
                    LedgerLine.ChqDate = svm.DueDate;
                    LedgerLine.BaseRate = svm.BaseRate;
                    LedgerLine.BaseValue = svm.BaseValue;
                    LedgerLine.ReferenceId = svm.ReferenceId;
                    LedgerLine.ProductUidId = svm.ProductUidId;
                    LedgerLine.ModifiedDate = DateTime.Now;
                    LedgerLine.ModifiedBy = User.Identity.Name;
                    LedgerLine.ObjectState = Model.ObjectState.Modified;
                    db.LedgerLine.Add(LedgerLine);
                    //new LedgerLineService(_unitOfWork).Update(LedgerLine);

                    var FromCosterName = db.CostCenter.Find(header.CostCenterId);
                    var ToCosterName = db.CostCenter.Find(svm.CostCenterId);

                    if (header.DocTypeId == 286)
                    {
                        header.Narration = header.Narration + " Form Costcenter " + FromCosterName.CostCenterName + " To " + ToCosterName.CostCenterName;
                    }

                    if (header.Status != (int)StatusConstants.Drafted)
                    {
                        header.Status = (int)StatusConstants.Modified;
                        header.ModifiedDate = DateTime.Now;
                        header.ModifiedBy = User.Identity.Name;
                        header.ObjectState = Model.ObjectState.Modified;
                        db.LedgerHeader.Add(header);
                        //new LedgerHeaderService(_unitOfWork).Update(header);
                    }


                    if (LedgerLine.CostCenterId.HasValue && LedgerLine.CostCenterId.Value != 0)
                    {


                        var CostCenterStatus = db.CostCenterStatus.Find(LedgerLine.CostCenterId);

                        if (Nature == NatureConstants.Debit)
                        {

                            CostCenterStatus.AmountCr = ((CostCenterStatus.AmountCr ?? 0) - ExRec.Amount) + LedgerLine.Amount;
                            CostCenterStatus.ObjectState = Model.ObjectState.Modified;
                            db.CostCenterStatus.Add(CostCenterStatus);
                            if (header.CostCenterId.HasValue && header.CostCenterId.Value != 0)
                            {
                                var HeaderCostCenterStatus = db.CostCenterStatus.Find(header.CostCenterId);
                                HeaderCostCenterStatus.AmountDr = ((HeaderCostCenterStatus.AmountDr ?? 0) - ExRec.Amount) + LedgerLine.Amount;
                                HeaderCostCenterStatus.ObjectState = Model.ObjectState.Modified;
                                db.CostCenterStatus.Add(HeaderCostCenterStatus);
                            }
                        }
                        else
                        {

                            CostCenterStatus.AmountDr = ((CostCenterStatus.AmountDr ?? 0) - ExRec.Amount) + LedgerLine.Amount;
                            CostCenterStatus.ObjectState = Model.ObjectState.Modified;
                            db.CostCenterStatus.Add(CostCenterStatus);
                            if (header.CostCenterId.HasValue && header.CostCenterId.Value != 0)
                            {
                                var HeaderCostCenterStatus = db.CostCenterStatus.Find(header.CostCenterId);
                                HeaderCostCenterStatus.AmountCr = ((HeaderCostCenterStatus.AmountCr ?? 0) - ExRec.Amount) + LedgerLine.Amount;
                                HeaderCostCenterStatus.ObjectState = Model.ObjectState.Modified;
                                db.CostCenterStatus.Add(HeaderCostCenterStatus);
                            }
                        }
                    }


                    #region LedgerSave
                    Ledger Ledger = new Ledger();

                    if (Nature == NatureConstants.Credit)
                    {
                        Ledger = db.Ledger.Where(m => m.LedgerLineId == LedgerLine.LedgerLineId && m.LedgerAccountId == XLedgerAccountId && m.AmtDr > 0).FirstOrDefault();
                        Ledger.AmtDr = LedgerLine.Amount;
                    }
                    else if (Nature == NatureConstants.Debit)
                    {
                        Ledger = db.Ledger.Where(m => m.LedgerLineId == LedgerLine.LedgerLineId && m.LedgerAccountId == XLedgerAccountId && m.AmtCr > 0).FirstOrDefault();
                        Ledger.AmtCr = LedgerLine.Amount;
                    }
                    Ledger.ChqNo = LedgerLine.ChqNo;
                    Ledger.ContraLedgerAccountId = header.LedgerAccountId;
                    Ledger.CostCenterId = LedgerLine.CostCenterId;
                    Ledger.DueDate = LedgerLine.ChqDate;
                    Ledger.LedgerAccountId = LedgerLine.LedgerAccountId;
                    Ledger.LedgerHeaderId = LedgerLine.LedgerHeaderId;
                    Ledger.LedgerLineId = LedgerLine.LedgerLineId;
                    Ledger.Narration = header.Narration + LedgerLine.Remark;
                    Ledger.ProductUidId = LedgerLine.ProductUidId;
                    Ledger.ObjectState = Model.ObjectState.Modified;
                    db.Ledger.Add(Ledger);

                    if (LedgerLine.ReferenceId != null)
                    {
                        LedgerAdj LedgerAdj = new LedgerAdj();

                        if (Nature == NatureConstants.Credit)
                        {
                            LedgerAdj = db.LedgerAdj.Where(m => m.DrLedgerId == Ledger.LedgerId && m.LedgerId == LedgerLine.ReferenceId).FirstOrDefault();
                            LedgerAdj.LedgerId = (int)LedgerLine.ReferenceId;
                            LedgerAdj.DrLedgerId = Ledger.LedgerId;
                            LedgerAdj.CrLedgerId = null;
                        }
                        else
                        {
                            LedgerAdj = db.LedgerAdj.Where(m => m.CrLedgerId == Ledger.LedgerId && m.LedgerId == LedgerLine.ReferenceId).FirstOrDefault();
                            LedgerAdj.LedgerId = (int)LedgerLine.ReferenceId;
                            LedgerAdj.CrLedgerId = Ledger.LedgerId;
                            LedgerAdj.DrLedgerId = null;
                        }

                        LedgerAdj.Amount = LedgerLine.Amount;
                        LedgerAdj.SiteId = header.SiteId;
                        LedgerAdj.CreatedDate = DateTime.Now;
                        LedgerAdj.ModifiedDate = DateTime.Now;
                        LedgerAdj.CreatedBy = User.Identity.Name;
                        LedgerAdj.ModifiedBy = User.Identity.Name;
                        LedgerAdj.ObjectState = Model.ObjectState.Modified;
                        db.LedgerAdj.Add(LedgerAdj);
                    }
                    #endregion


                    #region ContraLedgerSave
                    Ledger ContraLedger = new Ledger();
                    if (Nature == NatureConstants.Credit)
                    {
                        ContraLedger = db.Ledger.Where(m => m.LedgerLineId == LedgerLine.LedgerLineId && m.LedgerAccountId == header.LedgerAccountId && m.AmtCr > 0).FirstOrDefault();
                        ContraLedger.AmtCr = LedgerLine.Amount;

                    }
                    else if (Nature == NatureConstants.Debit)
                    {
                        ContraLedger = db.Ledger.Where(m => m.LedgerLineId == LedgerLine.LedgerLineId && m.LedgerAccountId == header.LedgerAccountId && m.AmtDr > 0).FirstOrDefault();
                        ContraLedger.AmtDr = LedgerLine.Amount;
                    }
                    ContraLedger.LedgerHeaderId = header.LedgerHeaderId;
                    ContraLedger.CostCenterId = header.CostCenterId;
                    ContraLedger.LedgerLineId = LedgerLine.LedgerLineId;
                    ContraLedger.LedgerAccountId = header.LedgerAccountId.Value;
                    ContraLedger.ContraLedgerAccountId = LedgerLine.LedgerAccountId;
                    ContraLedger.ObjectState = Model.ObjectState.Modified;
                    db.Ledger.Add(ContraLedger);
                    #endregion


                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = ExRec,
                        Obj = LedgerLine,
                    });

                    XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);

                    try
                    {
                        LedgerDocEvents.onLineSaveEvent(this, new LedgerEventArgs(LedgerLine.LedgerHeaderId, LedgerLine.LedgerLineId, EventModeConstants.Edit), ref db);
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
                        PrepareViewBag(header.LedgerHeaderId);
                        return PartialView("_Create", svm);
                    }

                    try
                    {
                        LedgerDocEvents.afterLineSaveEvent(this, new LedgerEventArgs(LedgerLine.LedgerHeaderId, LedgerLine.LedgerLineId, EventModeConstants.Edit), ref db);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = header.DocTypeId,
                        DocId = LedgerLine.LedgerHeaderId,
                        DocLineId = LedgerLine.LedgerLineId,
                        ActivityType = (int)ActivityTypeContants.Modified,
                        DocNo = header.DocNo,
                        xEModifications = Modifications,
                        DocDate = header.DocDate,
                        DocStatus = header.Status,
                    }));

                    return Json(new { success = true });

                }
            }
            PrepareViewBag(header.LedgerHeaderId);
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

        [HttpGet]
        private ActionResult _Modify(int id)
        {
            LedgersViewModel temp = _LedgerService.GetLedgerVm(id);
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

            LedgerHeader H = db.LedgerHeader.Find(temp.LedgerHeaderId);

            db.Entry<LedgerHeader>(H).Reference(m => m.LedgerAccount).Load();

            ViewBag.LedgerAccountName = H.LedgerAccount.LedgerAccountName;

            PrepareViewBag(temp.LedgerHeaderId);
            ViewBag.AccountType = new LedgerHeaderService(_unitOfWork).GetLedgerAccountType(temp.ContraLedgerAccountId ?? 0);
            //Getting Settings
            var settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(H.DocTypeId, H.DivisionId, H.SiteId);

            temp.LedgerSetting = Mapper.Map<LedgerSetting, LedgerSettingViewModel>(settings);

            return PartialView("_Create", temp);
        }

        [HttpGet]
        public ActionResult _Detail(int id)
        {
            LedgersViewModel temp = _LedgerService.GetLedgerVm(id);
            if (temp == null)
            {
                return HttpNotFound();
            }

            LedgerHeader H = new LedgerHeaderService(_unitOfWork).Find(temp.LedgerHeaderId);

            PrepareViewBag(temp.LedgerHeaderId);
            ViewBag.AccountType = new LedgerHeaderService(_unitOfWork).GetLedgerAccountType(temp.ContraLedgerAccountId ?? 0);
            //Getting Settings
            var settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(H.DocTypeId, H.DivisionId, H.SiteId);

            temp.LedgerSetting = Mapper.Map<LedgerSetting, LedgerSettingViewModel>(settings);

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
            LedgersViewModel temp = _LedgerService.GetLedgerVm(id);

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

            LedgerHeader H = new LedgerHeaderService(_unitOfWork).Find(temp.LedgerHeaderId);

            PrepareViewBag(temp.LedgerHeaderId);
            ViewBag.AccountType = new LedgerHeaderService(_unitOfWork).GetLedgerAccountType(temp.ContraLedgerAccountId ?? 0);

            //Getting Settings
            var settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(H.DocTypeId, H.DivisionId, H.SiteId);
            temp.LedgerSetting = Mapper.Map<LedgerSetting, LedgerSettingViewModel>(settings);
            return PartialView("_Create", temp);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(LedgersViewModel vm)
        {

            bool BeforeSave = true;
            try
            {
                BeforeSave = LedgerDocEvents.beforeLineDeleteEvent(this, new LedgerEventArgs(vm.LedgerHeaderId, vm.LedgerLineId), ref db);
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

                LedgerHeader header = db.LedgerHeader.Find(vm.LedgerHeaderId);
                string Nature = new DocumentTypeService(_unitOfWork).Find(header.DocTypeId).Nature;

                int LedgerLineId = vm.LedgerLineId;
                LedgerLine Line = db.LedgerLine.Find(LedgerLineId);
                int? CostCenterId = Line.CostCenterId;


                List<Ledger> Ledgers = db.Ledger.Where(m => m.LedgerLineId == LedgerLineId).ToList();

                var LedgerIds = Ledgers.Select(m => m.LedgerId).ToArray();

                LedgerAdj LedgerAdjs = new LedgerAdj();

                if (Nature == NatureConstants.Credit)
                {
                    var crLedger = Ledgers.Where(m => m.LedgerLineId == Line.LedgerLineId && m.LedgerAccountId == Line.LedgerAccountId && m.AmtDr > 0).FirstOrDefault();
                    LedgerAdjs = db.LedgerAdj.Where(m => m.DrLedgerId == crLedger.LedgerId && m.LedgerId == Line.ReferenceId).FirstOrDefault();
                }
                else
                {
                    var drLedger = Ledgers.Where(m => m.LedgerLineId == Line.LedgerLineId && m.LedgerAccountId == Line.LedgerAccountId && m.AmtCr > 0).FirstOrDefault();
                    LedgerAdjs = db.LedgerAdj.Where(m => m.CrLedgerId == drLedger.LedgerId && m.LedgerId == Line.ReferenceId).FirstOrDefault();
                }

                if (Line.ReferenceId.HasValue && Line.ReferenceId.Value > 0)
                {
                    LedgerAdjs.ObjectState = Model.ObjectState.Deleted;
                    db.LedgerAdj.Remove(LedgerAdjs);
                }

                foreach (var item in Ledgers)
                {
                    item.ObjectState = Model.ObjectState.Deleted;
                    db.Ledger.Remove(item);
                }


                if (LedgerLineId != 0)
                {
                    var RefLines = (from p in db.LedgerLineRefValue
                                    where p.LedgerLineId == LedgerLineId
                                    select p).ToList();

                    foreach (var item in RefLines)
                    {
                        item.ObjectState = Model.ObjectState.Deleted;
                        db.LedgerLineRefValue.Remove(item);
                    }

                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = Mapper.Map<LedgerLine>(Line)
                    });

                    Line.ObjectState = Model.ObjectState.Deleted;
                    db.LedgerLine.Remove(Line);
                }


                if (header.Status != (int)StatusConstants.Drafted)
                {
                    header.Status = (int)StatusConstants.Modified;
                    header.ModifiedBy = User.Identity.Name;
                    header.ModifiedDate = DateTime.Now;
                    //new LedgerHeaderService(_unitOfWork).Update(header);
                    header.ObjectState = Model.ObjectState.Modified;
                    db.LedgerHeader.Add(header);
                }

                if (CostCenterId.HasValue && CostCenterId.Value != 0)
                {
                    var CostCenterStatus = db.CostCenterStatus.Find(CostCenterId);

                    if (Nature == NatureConstants.Debit)
                    {
                        CostCenterStatus.AmountCr = (CostCenterStatus.AmountCr ?? 0) - Line.Amount;
                        CostCenterStatus.ObjectState = Model.ObjectState.Modified;
                        db.CostCenterStatus.Add(CostCenterStatus);
                        if (header.CostCenterId.HasValue && header.CostCenterId.Value != 0)
                        {
                            var HeaderCostCenterStatus = db.CostCenterStatus.Find(header.CostCenterId);
                            HeaderCostCenterStatus.AmountDr = (HeaderCostCenterStatus.AmountDr ?? 0) - Line.Amount;
                            HeaderCostCenterStatus.ObjectState = Model.ObjectState.Modified;
                            db.CostCenterStatus.Add(HeaderCostCenterStatus);
                        }
                    }
                    else
                    {
                        CostCenterStatus.AmountDr = (CostCenterStatus.AmountDr ?? 0) - Line.Amount;
                        CostCenterStatus.ObjectState = Model.ObjectState.Modified;
                        db.CostCenterStatus.Add(CostCenterStatus);
                        if (header.CostCenterId.HasValue && header.CostCenterId.Value != 0)
                        {
                            var HeaderCostCenterStatus = db.CostCenterStatus.Find(header.CostCenterId);
                            HeaderCostCenterStatus.AmountCr = (HeaderCostCenterStatus.AmountCr ?? 0) - Line.Amount;
                            HeaderCostCenterStatus.ObjectState = Model.ObjectState.Modified;
                            db.CostCenterStatus.Add(HeaderCostCenterStatus);
                        }
                    }
                }



                XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);

                try
                {
                    LedgerDocEvents.onLineDeleteEvent(this, new LedgerEventArgs(Line.LedgerHeaderId, Line.LedgerLineId), ref db);
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
                        throw new Exception();
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXCL"] += message;
                    ViewBag.LineMode = "Delete";
                    PrepareViewBag(header.LedgerHeaderId);
                    return PartialView("_Create", vm);
                }

                try
                {
                    LedgerDocEvents.afterLineDeleteEvent(this, new LedgerEventArgs(Line.LedgerHeaderId, Line.LedgerLineId), ref db);
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                }

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = header.DocTypeId,
                    DocId = header.LedgerHeaderId,
                    DocLineId = vm.LedgerLineId,
                    ActivityType = (int)ActivityTypeContants.Deleted,
                    DocNo = header.DocNo,
                    xEModifications = Modifications,
                    DocDate = header.DocDate,
                    DocStatus = header.Status,
                }));

            }

            return Json(new { success = true });
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

        public JsonResult GetPersonPendingBills(int LedgerHeaderId, int LedgerAccountId, string ReferenceType, string term, int Limit)
        {
            List<LedgerList> LedgerList = new LedgerLineService(_unitOfWork).GetPersonPendingBills(LedgerHeaderId, LedgerAccountId, ReferenceType, term, Limit).ToList();

            return Json(LedgerList);
        }

        public JsonResult GetLedgerAcc(int CostCenterId)
        {
            CostCenter Cs = new CostCenterService(_unitOfWork).Find(CostCenterId);
            if (Cs.LedgerAccountId.HasValue)
            {
                LedgerAccount LA = new LedgerAccountService(_unitOfWork).Find(Cs.LedgerAccountId.Value);

                return Json(new { Success = true, Name = LA.LedgerAccountName, Id = LA.LedgerAccountId });
            }
            else
            {
                return Json(new { Success = false });
            }

        }

        public JsonResult FetchRate(int PersonId)
        {
            if (PersonId != 0)
            {
                LedgerAccount LA = new LedgerAccountService(_unitOfWork).Find(PersonId);
                decimal Rate = 0;

                var TdsRate = (from p in db.BusinessEntity
                               where p.PersonID == LA.PersonId
                               let TDSGroupId = p.TdsGroupId
                               let TDSCatId = p.TdsCategoryId
                               from T in db.TdsRate
                               where T.TdsCategoryId == TDSCatId && T.TdsGroupId == TDSGroupId
                               select T).FirstOrDefault();
                if (TdsRate != null)
                {
                    Rate = TdsRate.Percentage;
                }

                return Json(new { Success = true, Rate = Rate });
            }


            return Json(new { Success = true, Rate = 0 });
        }


        public JsonResult GetLedgerAccount(string searchTerm, int pageSize, int pageNum, int filter)//filter:PersonId
        {

            LedgerHeader Ledger = new LedgerHeaderService(_unitOfWork).Find(filter);

            var Settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(Ledger.DocTypeId, Ledger.DivisionId, Ledger.SiteId);

            var Query = new LedgerLineService(_unitOfWork).GetLedgerAccounts(searchTerm, Settings.filterLedgerAccountGroupLines, (Ledger.ProcessId.HasValue ? Ledger.ProcessId.ToString() : Settings.filterPersonProcessLines));

            var temp = Query.Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();

            var count = Query.Count();

            ComboBoxPagedResult Data = new ComboBoxPagedResult();
            Data.Results = temp;
            Data.Total = count;

            return new JsonpResult
            {
                Data = Data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetCostCenters(string searchTerm, int pageSize, int pageNum, int filter)//filter:PersonId
        {

            LedgerHeader Ledger = new LedgerHeaderService(_unitOfWork).Find(filter);

            var Settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(Ledger.DocTypeId, Ledger.DivisionId, Ledger.SiteId);

            var temp = new LedgerLineService(_unitOfWork).GetCostCenters(searchTerm, Settings.filterDocTypeCostCenter, Settings.filterPersonProcessLines).Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();

            var count = new LedgerLineService(_unitOfWork).GetCostCenters(searchTerm, Settings.filterDocTypeCostCenter, Settings.filterPersonProcessLines).Count();

            ComboBoxPagedResult Data = new ComboBoxPagedResult();
            Data.Results = temp;
            Data.Total = count;

            return new JsonpResult
            {
                Data = Data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetProductUidValidation(string ProductUID)
        {
            return Json(new ProductUidService(_unitOfWork).ValidateUID(ProductUID), JsonRequestBehavior.AllowGet);
        }

    }
}
