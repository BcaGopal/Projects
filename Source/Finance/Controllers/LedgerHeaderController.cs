using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Model.Models;
using Data.Models;
using Service;
using Data.Infrastructure;
using Presentation.ViewModels;
using Presentation;
using Core.Common;
using Model.ViewModel;
using Presentation.Helper;
using AutoMapper;
using Model.ViewModels;
using System.Xml.Linq;
using System.Configuration;
using LedgerDocumentEvents;
using CustomEventArgs;
using DocumentEvents;
using Reports.Controllers;
using Reports.Reports;

namespace Web
{
    [Authorize]
    public class LedgerHeaderController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        List<string> UserRoles = new List<string>();
        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        private bool EventException = false;
        bool TimePlanValidation = true;
        string ExceptionMsg = "";
        bool Continue = true;

        ILedgerHeaderService _LedgerHeaderService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;
        public LedgerHeaderController(ILedgerHeaderService LedgerHeaderService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _LedgerHeaderService = LedgerHeaderService;
            _unitOfWork = unitOfWork;
            _exception = exec;
            if (!LedgerEvents.Initialized)
            {
                LedgerEvents Obj = new LedgerEvents();
            }

            UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;
        }

        public ActionResult DocumentTypeIndex(int id)//DocumentCategoryId
        {
            var p = new DocumentTypeService(_unitOfWork).FindByDocumentCategory(id).ToList();

            if (p != null)
            {
                if (p.Count == 1)
                    return RedirectToAction("Index", new { id = p.FirstOrDefault().DocumentTypeId });
            }

            return View("DocumentTypeList", p);
        }
        void PrepareViewBag(int DocTypeId, int? LedgerHeaderId)
        {
            ViewBag.Name = new DocumentTypeService(_unitOfWork).Find(DocTypeId).DocumentTypeName;
            ViewBag.id = DocTypeId;
            ViewBag.VoucherType = new DocumentTypeService(_unitOfWork).Find(DocTypeId).VoucherType;

            string Nature = "";

            if (LedgerHeaderId != null && LedgerHeaderId != 0)
            {
                LedgerHeader Header = new LedgerHeaderService(_unitOfWork).Find((int)LedgerHeaderId);
                if (Header.LedgerAccountId != null)
                {
                    ViewBag.LedgerAccountNature = new LedgerAccountService(_unitOfWork).GetLedgerAccountnature((int)Header.LedgerAccountId);
                }

                if (Header.DrCr != null)
                {
                    Nature = Header.DrCr;
                }
                else
                {
                    Nature = new DocumentTypeService(_unitOfWork).Find(Header.DocTypeId).Nature;
                }
            }

            ViewBag.Nature = Nature;

            List<SelectListItem> shwBal = new List<SelectListItem>();
            shwBal.Add(new SelectListItem { Text = LedgerHeaderAdjustmentTypeConstants.Payment, Value = LedgerHeaderAdjustmentTypeConstants.Payment });
            shwBal.Add(new SelectListItem { Text = LedgerHeaderAdjustmentTypeConstants.Advance, Value = LedgerHeaderAdjustmentTypeConstants.Advance });

            ViewBag.AdjustmentTypeList = new SelectList(shwBal, "Value", "Text");

            List<SelectListItem> DrCr = new List<SelectListItem>();
            DrCr.Add(new SelectListItem { Text = NatureConstants.Debit, Value = NatureConstants.Debit });
            DrCr.Add(new SelectListItem { Text = NatureConstants.Credit, Value = NatureConstants.Credit });

            ViewBag.DrCrList = new SelectList(DrCr, "Value", "Text");


        }

        // GET: /LedgerHeaderMaster/

        public ActionResult Index(int id, string IndexType)//Document TypeId
        {

            if (IndexType == "PTS")
            {
                return RedirectToAction("Index_PendingToSubmit", new { id });
            }
            else if (IndexType == "PTR")
            {
                return RedirectToAction("Index_PendingToReview", new { id });
            }
            var LedgerHeader = _LedgerHeaderService.GetLedgerHeaderList(id, User.Identity.Name);
            PrepareViewBag(id, null);
            ViewBag.PendingToSubmit = PendingToSubmitCount(id);
            ViewBag.PendingToReview = PendingToReviewCount(id);
            ViewBag.IndexStatus = "All";
            return View(LedgerHeader);
        }


        public ActionResult Index_PendingToSubmit(int id)
        {
            var PendingToSubmit = _LedgerHeaderService.GetLedgerHeaderListPendingToSubmit(id, User.Identity.Name);

            PrepareViewBag(id, null);
            ViewBag.PendingToSubmit = PendingToSubmitCount(id);
            ViewBag.PendingToReview = PendingToReviewCount(id);
            ViewBag.IndexStatus = "PTS";
            return View("Index", PendingToSubmit);
        }

        public ActionResult Index_PendingToReview(int id)
        {
            var PendingtoReview = _LedgerHeaderService.GetLedgerHeaderListPendingToReview(id, User.Identity.Name);
            PrepareViewBag(id, null);
            ViewBag.PendingToSubmit = PendingToSubmitCount(id);
            ViewBag.PendingToReview = PendingToReviewCount(id);
            ViewBag.IndexStatus = "PTR";
            return View("Index", PendingtoReview);
        }


        // GET: /LedgerHeaderMaster/Create

        public ActionResult Create(int id)
        {
            PrepareViewBag(id, null);
            LedgerHeaderViewModel vm = new LedgerHeaderViewModel();
            vm.DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            vm.SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            vm.DocDate = DateTime.Now;
            vm.PaymentFor = DateTime.Now;
            vm.CreatedDate = DateTime.Now;

            //Getting Settings
            var settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(id, vm.DivisionId, vm.SiteId);

            if (settings == null && UserRoles.Contains("Admin"))
            {
                return RedirectToAction("Create", "LedgerSetting", new { id = id }).Warning("Please create Ledger settings");
            }
            else if (settings == null && !UserRoles.Contains("Admin"))
            {
                return View("~/Views/Shared/InValidSettings.cshtml");
            }
            vm.LedgerSetting = Mapper.Map<LedgerSetting, LedgerSettingViewModel>(settings);

            DocumentType DocType = new DocumentTypeService(_unitOfWork).Find(id);
            vm.DrCr = DocType.Nature;



            vm.DocTypeId = id;
            vm.ProcessId = settings.ProcessId;
            vm.DocNo = new DocumentTypeService(_unitOfWork).FGetNewDocNo("DocNo", ConfigurationManager.AppSettings["DataBaseSchema"] + ".LedgerHeaders", vm.DocTypeId, vm.DocDate, vm.DivisionId, vm.SiteId);
            ViewBag.Mode = "Add";
            return View("Create", vm);
        }

        // POST: /ProductMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(LedgerHeaderViewModel vm)
        {

            #region BeforeSave
            bool BeforeSave = true;
            try
            {
                if (vm.LedgerHeaderId <= 0)
                    BeforeSave = LedgerDocEvents.beforeHeaderSaveEvent(this, new LedgerEventArgs(vm.LedgerHeaderId, EventModeConstants.Add), ref db);
                else
                    BeforeSave = LedgerDocEvents.beforeHeaderSaveEvent(this, new LedgerEventArgs(vm.LedgerHeaderId, EventModeConstants.Edit), ref db);
            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                EventException = true;
            }

            if (!BeforeSave)
                TempData["CSEXC"] += "Failed validation before save";
            #endregion

            #region DocTypeTimeLineValidation

            try
            {

                if (vm.LedgerHeaderId <= 0)
                    TimePlanValidation = DocumentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(vm), DocumentTimePlanTypeConstants.Create, User.Identity.Name, out ExceptionMsg, out Continue);
                else
                    TimePlanValidation = DocumentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(vm), DocumentTimePlanTypeConstants.Modify, User.Identity.Name, out ExceptionMsg, out Continue);

            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                TimePlanValidation = false;
            }

            if (!TimePlanValidation)
                TempData["CSEXC"] += ExceptionMsg;

            #endregion

            LedgerHeader pt = AutoMapper.Mapper.Map<LedgerHeaderViewModel, LedgerHeader>(vm);

            if (vm.LedgerHeaderId <= 0)
                ViewBag.Mode = "Add";
            else
                ViewBag.Mode = "Edit";

            if (vm.LedgerSetting != null)
            {

                if (vm.LedgerSetting.isVisibleHeaderCostCenter == true && vm.LedgerSetting.isMandatoryHeaderCostCenter == true && !vm.CostCenterId.HasValue)
                {
                    ModelState.AddModelError("CostCenterId", "The CostCenter field is required");
                }
                if (vm.LedgerSetting.isVisibleProcess == true && vm.LedgerSetting.isMandatoryProcess == true && !vm.ProcessId.HasValue)
                {
                    ModelState.AddModelError("ProcessId", "The Process field is required");
                }
                if (vm.LedgerSetting.isVisibleGodown == true && vm.LedgerSetting.isMandatoryGodown == true && !vm.GodownId.HasValue)
                {
                    ModelState.AddModelError("GodownId", "The Godown field is required");
                }
            }

            if (ModelState.IsValid && BeforeSave && !EventException && (TimePlanValidation || Continue))
            {
                #region CreateRecord
                if (vm.LedgerHeaderId <= 0)
                {
                    pt.CreatedDate = DateTime.Now;
                    pt.ModifiedDate = DateTime.Now;
                    pt.CreatedBy = User.Identity.Name;
                    pt.ModifiedBy = User.Identity.Name;
                    pt.ObjectState = Model.ObjectState.Added;
                    //_LedgerHeaderService.Create(pt);
                    db.LedgerHeader.Add(pt);


                    try
                    {
                        LedgerDocEvents.onHeaderSaveEvent(this, new LedgerEventArgs(pt.LedgerHeaderId, EventModeConstants.Add), ref db);
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
                        TempData["CSEXC"] += message;
                        PrepareViewBag(vm.DocTypeId, null);
                        ViewBag.Mode = "Add";
                        return View("Create", vm);
                    }

                    try
                    {
                        LedgerDocEvents.afterHeaderSaveEvent(this, new LedgerEventArgs(pt.LedgerHeaderId, EventModeConstants.Add), ref db);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = pt.DocTypeId,
                        DocId = pt.LedgerHeaderId,
                        ActivityType = (int)ActivityTypeContants.Added,
                        DocNo = pt.DocNo,
                        DocDate = pt.DocDate,
                        DocStatus = pt.Status,
                    }));

                    return RedirectToAction("Modify", new { id = pt.LedgerHeaderId }).Success("Data saved successfully");
                }
                #endregion

                #region EditRecord
                else
                {
                    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();
                    bool UpdateLedgerPosting = false;
                    int LedgerAccountId = 0;
                    LedgerHeader temp = _LedgerHeaderService.Find(pt.LedgerHeaderId);
                    if (temp.LedgerAccountId.HasValue && temp.LedgerAccountId != pt.LedgerAccountId)
                    { UpdateLedgerPosting = true; LedgerAccountId = temp.LedgerAccountId.Value; }

                    int status = temp.Status;
                    LedgerHeader ExRec = Mapper.Map<LedgerHeader>(temp);

                    if (temp.Status != (int)StatusConstants.Drafted)
                        temp.Status = (int)StatusConstants.Modified;

                    temp.DocNo = pt.DocNo;
                    temp.DocTypeId = pt.DocTypeId;
                    temp.DocDate = pt.DocDate;
                    temp.PaymentFor = pt.PaymentFor;
                    temp.AdjustmentType = pt.AdjustmentType;
                    temp.ProcessId = pt.ProcessId;
                    temp.GodownId = pt.GodownId;
                    temp.LedgerAccountId = pt.LedgerAccountId;
                    temp.DrCr = pt.DrCr;
                    temp.Narration = pt.Narration;
                    temp.ModifiedDate = DateTime.Now;
                    temp.ModifiedBy = User.Identity.Name;
                    temp.ObjectState = Model.ObjectState.Modified;
                    db.LedgerHeader.Add(temp);

                    if (UpdateLedgerPosting && temp.LedgerAccountId != LedgerAccountId)
                    {
                        //string Nature = new DocumentTypeService(_unitOfWork).Find(temp.DocTypeId).Nature;


                        string Nature = "";

                        if (temp.DrCr != null)
                        {
                            Nature = temp.DrCr;
                        }
                        else
                        {
                            Nature = new DocumentTypeService(_unitOfWork).Find(temp.DocTypeId).Nature;
                        }

                        var Ledgers = (from p in db.Ledger
                                       where (p.LedgerAccountId == LedgerAccountId || p.ContraLedgerAccountId == LedgerAccountId) && p.LedgerHeaderId == temp.LedgerHeaderId
                                       select p).ToList();

                        //UpdatingLedgerPosting::
                        //foreach (var item in Ledgers.Where(m => m.ContraLedgerAccountId == LedgerAccountId && Nature == NatureConstants.Credit ? m.AmtDr > 0 : m.AmtCr > 0))
                        foreach (var item in Ledgers.Where(m => m.ContraLedgerAccountId == LedgerAccountId))
                        {
                            item.ContraLedgerAccountId = temp.LedgerAccountId;
                            item.ObjectState = Model.ObjectState.Modified;
                            db.Ledger.Add(item);
                        }

                        //UpdatingContraLedgerPosting::
                        //foreach (var item in Ledgers.Where(m => m.LedgerAccountId == LedgerAccountId && Nature == NatureConstants.Credit ? m.AmtCr > 0 : m.AmtDr > 0))
                        foreach (var item in Ledgers.Where(m => m.LedgerAccountId == LedgerAccountId))
                        {
                            item.LedgerAccountId = temp.LedgerAccountId.Value;
                            item.ObjectState = Model.ObjectState.Modified;
                            db.Ledger.Add(item);
                        }

                    }

                    //_LedgerHeaderService.Update(temp);

                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = ExRec,
                        Obj = temp,
                    });
                    XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);

                    try
                    {
                        LedgerDocEvents.onHeaderSaveEvent(this, new LedgerEventArgs(temp.LedgerHeaderId, EventModeConstants.Edit), ref db);
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
                        TempData["CSEXC"] += message;
                        PrepareViewBag(vm.DocTypeId, null);
                        ViewBag.Mode = "Edit";
                        return View("Create", pt);
                    }

                    try
                    {
                        LedgerDocEvents.afterHeaderSaveEvent(this, new LedgerEventArgs(temp.LedgerHeaderId, EventModeConstants.Edit), ref db);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = temp.DocTypeId,
                        DocId = temp.LedgerHeaderId,
                        ActivityType = (int)ActivityTypeContants.Modified,
                        DocNo = temp.DocNo,
                        xEModifications = Modifications,
                        DocDate = temp.DocDate,
                        DocStatus = temp.Status,
                    }));

                    return RedirectToAction("Index", new { id = pt.DocTypeId }).Success("Data saved successfully");
                }
                #endregion
            }
            PrepareViewBag(vm.DocTypeId, pt.LedgerHeaderId);
            return View("Create", vm);
        }




        [HttpGet]
        public ActionResult Modify(int id, string IndexType)
        {
            LedgerHeader header = _LedgerHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Drafted)
                return Edit(id, IndexType);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult ModifyAfter_Submit(int id, string IndexType)
        {
            LedgerHeader header = _LedgerHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Submitted || header.Status == (int)StatusConstants.Modified || header.Status == (int)StatusConstants.ModificationSubmitted)
                return Edit(id, IndexType);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            LedgerHeader header = _LedgerHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Drafted)
                return Remove(id);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult DeleteAfter_Submit(int id)
        {
            LedgerHeader header = _LedgerHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Submitted || header.Status == (int)StatusConstants.Modified || header.Status == (int)StatusConstants.ModificationSubmitted)
                return Remove(id);
            else
                return HttpNotFound();
        }






        // GET: /ProductMaster/Edit/5

        private ActionResult Edit(int id, string IndexType)
        {
            ViewBag.IndexStatus = IndexType;
            LedgerHeaderViewModel pt = _LedgerHeaderService.GetLedgerHeaderVm(id);

            if (pt == null)
            {
                return HttpNotFound();
            }

            #region DocTypeTimeLineValidation
            try
            {

                TimePlanValidation = DocumentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(pt), DocumentTimePlanTypeConstants.Modify, User.Identity.Name, out ExceptionMsg, out Continue);

            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                TimePlanValidation = false;
            }

            if (!TimePlanValidation)
                TempData["CSEXC"] += ExceptionMsg;
            #endregion

            if ((!TimePlanValidation && !Continue))
            {
                return RedirectToAction("DetailInformation", new { id = id, IndexType = IndexType });
            }

            //Job Order Settings
            var settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(pt.DocTypeId, pt.DivisionId, pt.SiteId);

            if (settings == null && UserRoles.Contains("Admin"))
            {
                return RedirectToAction("Create", "LedgerSetting", new { id = pt.DocTypeId }).Warning("Please create Ledger settings");
            }
            else if (settings == null && !UserRoles.Contains("Admin"))
            {
                return View("~/Views/Shared/InValidSettings.cshtml");
            }

            pt.LedgerSetting = Mapper.Map<LedgerSetting, LedgerSettingViewModel>(settings);


            PrepareViewBag(pt.DocTypeId, id);
            ViewBag.AccountType = _LedgerHeaderService.GetLedgerAccountType(pt.LedgerAccountId ?? 0);

            ViewBag.Mode = "Edit";

            if (!(System.Web.HttpContext.Current.Request.UrlReferrer.PathAndQuery).Contains("Create"))
                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = pt.DocTypeId,
                    DocId = pt.LedgerHeaderId,
                    ActivityType = (int)ActivityTypeContants.Detail,
                    DocNo = pt.DocNo,
                    DocDate = pt.DocDate,
                    DocStatus = pt.Status,
                }));


            return View("Create", pt);
        }


        [HttpGet]
        public ActionResult DetailInformation(int id)
        {
            return RedirectToAction("Detail", new { id = id, transactionType = "detail" });
        }

        [Authorize]
        public ActionResult Detail(int id, string IndexType, string transactionType)
        {
            ViewBag.transactionType = transactionType;
            ViewBag.IndexStatus = IndexType;

            LedgerHeaderViewModel pt = _LedgerHeaderService.GetLedgerHeaderVm(id);

            var settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(pt.DocTypeId, pt.DivisionId, pt.SiteId);
            pt.LedgerSetting = Mapper.Map<LedgerSetting, LedgerSettingViewModel>(settings);

            PrepareViewBag(pt.DocTypeId, id);
            ViewBag.AccountType = _LedgerHeaderService.GetLedgerAccountType(pt.LedgerAccountId ?? 0);
            if (pt == null)
            {
                return HttpNotFound();
            }

            if (String.IsNullOrEmpty(transactionType) || transactionType == "detail")
                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = pt.DocTypeId,
                    DocId = pt.LedgerHeaderId,
                    ActivityType = (int)ActivityTypeContants.Detail,
                    DocNo = pt.DocNo,
                    DocDate = pt.DocDate,
                    DocStatus = pt.Status,
                }));

            return View("Create", pt);
        }




        public ActionResult Submit(int id, string IndexType, string TransactionType)
        {
            #region DocTypeTimeLineValidation

            LedgerHeader s = db.LedgerHeader.Find(id);
            try
            {
                TimePlanValidation = Submitvalidation(id, out ExceptionMsg);
                TempData["CSEXC"] += ExceptionMsg;
            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                TimePlanValidation = false;
            }
            try
            {
                TimePlanValidation = DocumentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(s), DocumentTimePlanTypeConstants.Submit, User.Identity.Name, out ExceptionMsg, out Continue);
                TempData["CSEXC"] += ExceptionMsg;
            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                TimePlanValidation = false;
            }

            if (!TimePlanValidation && !Continue)
            {
                return RedirectToAction("Index", new { id = s.DocTypeId, IndexType = IndexType });
            }
            #endregion
            return RedirectToAction("Detail", new { id = id, IndexType = IndexType, transactionType = string.IsNullOrEmpty(TransactionType) ? "submit" : TransactionType });
        }

        [HttpPost, ActionName("Detail")]
        [MultipleButton(Name = "Command", Argument = "Submit")]
        public ActionResult Submitted(int Id, string IndexType, string UserRemark, string IsContinue)
        {
            bool BeforeSave = true;
            try
            {
                BeforeSave = LedgerDocEvents.beforeHeaderSubmitEvent(this, new LedgerEventArgs(Id), ref db);
            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                EventException = true;
            }

            if (!BeforeSave)
                TempData["CSEXC"] += "Falied validation before submit.";

            if (ModelState.IsValid && BeforeSave && !EventException)
            {
                LedgerHeader pd = new LedgerHeaderService(_unitOfWork).Find(Id);

                if (User.Identity.Name == pd.ModifiedBy || UserRoles.Contains("Admin"))
                {
                    int ActivityType;

                    pd.Status = (int)StatusConstants.Submitted;
                    ActivityType = (int)ActivityTypeContants.Submitted;

                    pd.ReviewBy = null;
                    pd.ObjectState = Model.ObjectState.Modified;
                    db.LedgerHeader.Add(pd);
                    //_LedgerHeaderService.Update(pd);

                    try
                    {
                        LedgerDocEvents.onHeaderSubmitEvent(this, new LedgerEventArgs(Id), ref db);
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
                        TempData["CSEXC"] += message;
                        return RedirectToAction("Index", new { id = pd.DocTypeId, IndexType = IndexType }).Warning("Record cannot be submitted due to error." + message);
                    }

                    try
                    {
                        LedgerDocEvents.afterHeaderSubmitEvent(this, new LedgerEventArgs(Id), ref db);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = pd.DocTypeId,
                        DocId = pd.LedgerHeaderId,
                        ActivityType = ActivityType,
                        UserRemark = UserRemark,
                        DocNo = pd.DocNo,
                        DocDate = pd.DocDate,
                        DocStatus = pd.Status,
                    }));


                    if (!string.IsNullOrEmpty(IsContinue) && IsContinue == "True")
                    {

                        int nextId = new NextPrevIdService(_unitOfWork).GetNextPrevId(Id, pd.DocTypeId, User.Identity.Name, ForActionConstants.PendingToSubmit, "Web.LedgerHeaders", "LedgerHeaderId", PrevNextConstants.Next);

                        if (nextId == 0)
                        {


                            var PendingtoSubmitCount = PendingToSubmitCount(pd.DocTypeId);
                            if (PendingtoSubmitCount > 0)
                                return RedirectToAction("Index_PendingToSubmit", new { id = pd.DocTypeId });
                            else
                                return RedirectToAction("Index", new { id = pd.DocTypeId });

                        }

                        return RedirectToAction("Detail", new { id = nextId, IndexType = IndexType, TransactionType = "submitContinue" }).Success(pd.DocNo + " submitted successfully.");

                    }

                    else
                        return RedirectToAction("Index", new { id = pd.DocTypeId, IndexType = IndexType }).Success(pd.DocNo + " submitted successfully.");
                }
                else
                    return RedirectToAction("Index", new { id = pd.DocTypeId, IndexType = IndexType }).Warning("Record can be submitted by user " + pd.ModifiedBy + " only.");


            }

            return View();
        }



        public ActionResult Review(int id, string IndexType, string TransactionType)
        {

            return RedirectToAction("Detail", new { id = id, IndexType = IndexType, transactionType = string.IsNullOrEmpty(TransactionType) ? "review" : TransactionType });

        }


        [HttpPost, ActionName("Detail")]
        [MultipleButton(Name = "Command", Argument = "Review")]
        public ActionResult Reviewed(int Id, string IndexType, string UserRemark, string IsContinue)
        {

            if (ModelState.IsValid)
            {
                LedgerHeader pd = new LedgerHeaderService(_unitOfWork).Find(Id);

                pd.ReviewCount = (pd.ReviewCount ?? 0) + 1;
                pd.ReviewBy += User.Identity.Name + ", ";

                pd.ObjectState = Model.ObjectState.Modified;
                db.LedgerHeader.Add(pd);

                db.SaveChanges();

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = pd.DocTypeId,
                    DocId = pd.LedgerHeaderId,
                    ActivityType = (int)ActivityTypeContants.Reviewed,
                    UserRemark = UserRemark,
                    DocNo = pd.DocNo,
                    DocDate = pd.DocDate,
                    DocStatus = pd.Status,
                }));

                if (!string.IsNullOrEmpty(IsContinue) && IsContinue == "True")
                {
                    LedgerHeader HEader = _LedgerHeaderService.Find(Id);

                    int nextId = new NextPrevIdService(_unitOfWork).GetNextPrevId(Id, HEader.DocTypeId, User.Identity.Name, ForActionConstants.PendingToReview, "Web.LedgerHeaders", "LedgerHeaderId", PrevNextConstants.Next);
                    if (nextId == 0)
                    {
                        var PendingtoSubmitCount = PendingToReviewCount(HEader.DocTypeId);
                        if (PendingtoSubmitCount > 0)
                            return RedirectToAction("Index_PendingToReview", new { id = HEader.DocTypeId, IndexType = IndexType });
                        else
                            return RedirectToAction("Index", new { id = HEader.DocTypeId, IndexType = IndexType });

                    }

                    ViewBag.PendingToReview = PendingToReviewCount(Id);
                    return RedirectToAction("Detail", new { id = nextId, IndexType = IndexType, transactionType = "ReviewContinue" });
                }


                else
                    return RedirectToAction("Index", new { id = pd.DocTypeId, IndexType = IndexType });

            }

            return View();
        }









        // GET: /ProductMaster/Delete/5

        private ActionResult Remove(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LedgerHeader LedgerHeader = db.LedgerHeader.Find(id);
            if (LedgerHeader == null)
            {
                return HttpNotFound();
            }

            #region DocTypeTimeLineValidation
            try
            {
                TimePlanValidation = DocumentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(LedgerHeader), DocumentTimePlanTypeConstants.Delete, User.Identity.Name, out ExceptionMsg, out Continue);
                TempData["CSEXC"] += ExceptionMsg;
            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                TimePlanValidation = false;
            }

            if (!TimePlanValidation && !Continue)
            {
                return PartialView("AjaxError");
            }
            #endregion

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
            bool BeforeSave = true;
            int? HeaderCostCenterId = null;
            try
            {
                BeforeSave = LedgerDocEvents.beforeHeaderDeleteEvent(this, new LedgerEventArgs(vm.id), ref db);
            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                EventException = true;
            }

            if (!BeforeSave)
                TempData["CSEXC"] += "Failed validation before delete";

            if (ModelState.IsValid && BeforeSave && !EventException)
            {
                List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

                var temp = db.LedgerHeader.Find(vm.id);
                //string Nature = db.DocumentType.Find(temp.DocTypeId).Nature;

                string Nature = "";

                if (temp.DrCr != null)
                {
                    Nature = temp.DrCr;
                }
                else
                {
                    Nature = new DocumentTypeService(_unitOfWork).Find(temp.DocTypeId).Nature;
                }

                HeaderCostCenterId = temp.CostCenterId;

                LogList.Add(new LogTypeViewModel
                {
                    ExObj = Mapper.Map<LedgerHeader>(temp),
                });

                var LedgerLines = (from p in db.LedgerLine
                                   where p.LedgerHeaderId == vm.id
                                   select p).ToList();

                var CostCenterAmount = (from p in LedgerLines
                                        where p.CostCenterId != null
                                        group p by p.CostCenterId into g
                                        select g).ToList();



                var ExisitingLedgers = (from p in db.Ledger
                                        where p.LedgerHeaderId == vm.id
                                        select p).ToList();

                try
                {
                    LedgerDocEvents.onHeaderDeleteEvent(this, new LedgerEventArgs(vm.id), ref db);
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                    EventException = true;
                }

                foreach (var item in ExisitingLedgers)
                {

                    var LedgerAdjs = (from p in db.LedgerAdj
                                      where p.CrLedgerId == item.LedgerId || p.DrLedgerId == item.LedgerId
                                      select p).ToList();

                    foreach (var item2 in LedgerAdjs)
                    {
                        item2.ObjectState = Model.ObjectState.Deleted;
                        db.LedgerAdj.Remove(item2);
                        //new LedgerAdjService(_unitOfWork).Delete(item2);
                        LogList.Add(new LogTypeViewModel
                        {
                            ExObj = Mapper.Map<LedgerAdj>(item2),
                        });
                    }
                    item.ObjectState = Model.ObjectState.Deleted;
                    db.Ledger.Remove(item);
                    //new LedgerService(_unitOfWork).Delete(item);

                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = Mapper.Map<Ledger>(item),
                    });

                }


                foreach (var item in LedgerLines)
                {
                    var RefLines = (from p in db.LedgerLineRefValue
                                    where p.LedgerLineId == item.LedgerLineId
                                    select p).ToList();

                    foreach (var RefLine in RefLines)
                    {
                        RefLine.ObjectState = Model.ObjectState.Deleted;
                        db.LedgerLineRefValue.Remove(RefLine);
                        //new LedgerLineRefValueService(_unitOfWork).Delete(RefLine);
                    }


                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = Mapper.Map<LedgerLine>(item),
                    });

                    item.ObjectState = Model.ObjectState.Deleted;
                    db.LedgerLine.Remove(item);
                    //new LedgerLineService(_unitOfWork).Delete(item);

                }
                temp.ObjectState = Model.ObjectState.Deleted;
                db.LedgerHeader.Remove(temp);

                var CostCenterIds = CostCenterAmount.Select(m => m.Key).ToArray();

                var CostCenterStatus = (from p in db.CostCenterStatus
                                        where CostCenterIds.Contains(p.CostCenterId)
                                        select p).ToList();

                foreach (var item in CostCenterStatus)
                {

                    var CCStatusAmount = CostCenterAmount.Where(m => m.Key == item.CostCenterId).FirstOrDefault();

                    if (Nature == NatureConstants.Debit)
                    {
                        item.AmountCr = (item.AmountCr ?? 0) - CCStatusAmount.Sum(m => m.Amount);
                        item.ObjectState = Model.ObjectState.Modified;
                        db.CostCenterStatus.Add(item);
                        if (HeaderCostCenterId.HasValue && HeaderCostCenterId.Value != 0)
                        {
                            var HeaderCostCenterStatus = db.CostCenterStatus.Find(HeaderCostCenterId);
                            HeaderCostCenterStatus.AmountDr = (HeaderCostCenterStatus.AmountDr ?? 0) - CCStatusAmount.Sum(m => m.Amount);
                            HeaderCostCenterStatus.ObjectState = Model.ObjectState.Modified;
                            db.CostCenterStatus.Add(HeaderCostCenterStatus);
                        }
                    }
                    else
                    {
                        item.AmountDr = (item.AmountDr ?? 0) - CCStatusAmount.Sum(m => m.Amount);
                        item.ObjectState = Model.ObjectState.Modified;
                        db.CostCenterStatus.Add(item);
                        if (HeaderCostCenterId.HasValue && HeaderCostCenterId.Value != 0)
                        {
                            var HeaderCostCenterStatus = db.CostCenterStatus.Find(HeaderCostCenterId);
                            HeaderCostCenterStatus.AmountCr = (HeaderCostCenterStatus.AmountCr ?? 0) - CCStatusAmount.Sum(m => m.Amount);
                            HeaderCostCenterStatus.ObjectState = Model.ObjectState.Modified;
                            db.CostCenterStatus.Add(HeaderCostCenterStatus);
                        }
                    }

                }

                //_LedgerHeaderService.Delete(vm.id);
                XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);
                try
                {
                    if (EventException)
                        throw new Exception();

                    db.SaveChanges();
                    //_unitOfWork.Save();
                }

                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                    return PartialView("_Reason", vm);
                }

                try
                {
                    LedgerDocEvents.afterHeaderDeleteEvent(this, new LedgerEventArgs(vm.id), ref db);
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                }

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = temp.DocTypeId,
                    DocId = temp.LedgerHeaderId,
                    ActivityType = (int)ActivityTypeContants.Deleted,
                    UserRemark = vm.Reason,
                    DocNo = temp.DocNo,
                    xEModifications = Modifications,
                    DocDate = temp.DocDate,
                    DocStatus = temp.Status,
                }));

                return Json(new { success = true });
            }
            return PartialView("_Reason", vm);
        }

        [HttpGet]
        public ActionResult NextPage(int id)//CurrentHeaderId
        {
            var nextId = _LedgerHeaderService.NextId(id);
            return RedirectToAction("Edit", new { id = nextId });
        }
        [HttpGet]
        public ActionResult PrevPage(int id)//CurrentHeaderId
        {
            var nextId = _LedgerHeaderService.PrevId(id);
            return RedirectToAction("Edit", new { id = nextId });
        }

        [HttpGet]
        public ActionResult History()
        {
            //To Be Implemented
            return View("~/Views/Shared/UnderImplementation.cshtml");
        }

        //[HttpGet]
        //public ActionResult Print(int id)
        //{
        //    LedgerHeader s = _LedgerHeaderService.Find(id);
        //    //var settings = new LedgerSettingsService(_unitOfWork).GetMaterialIssueSettingsForDocument(s.DocTypeId, s.DivisionId, s.SiteId);
        //    //String query = settings.SqlProcDocumentPrint;
        //    String query = "Web.spRep_DebitNotePrint";
        //    return Redirect((string)System.Configuration.ConfigurationManager.AppSettings["CustomizeDomain"] + "/Report_DocumentPrint/DocumentPrint/?DocumentId=" + id + "&queryString=" + query);
        //}


        [HttpGet]
        private ActionResult PrintOut(int id, string SqlProcForPrint)
        {
            String query = SqlProcForPrint;
            return Redirect((string)System.Configuration.ConfigurationManager.AppSettings["CustomizeDomain"] + "/Report_DocumentPrint/DocumentPrint/?DocumentId=" + id + "&queryString=" + query);
        }

        [HttpGet]
        public ActionResult Print(int id)
        {
            LedgerHeader header = _LedgerHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Drafted)
            {
                var settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(header.DocTypeId, header.DivisionId, header.SiteId);

                if (string.IsNullOrEmpty(settings.SqlProcDocumentPrint))
                    throw new Exception("Document Print Not Configured");
                else
                    return PrintOut(id, settings.SqlProcDocumentPrint);
            }
            else
                return HttpNotFound();

        }

        [HttpGet]
        public ActionResult PrintAfter_Submit(int id)
        {
            LedgerHeader header = _LedgerHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Submitted || header.Status == (int)StatusConstants.Modified || header.Status == (int)StatusConstants.ModificationSubmitted)
            {
                var settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(header.DocTypeId, header.DivisionId, header.SiteId);
                if (string.IsNullOrEmpty(settings.SqlProcDocumentPrint_AfterSubmit))
                    throw new Exception("Document Print Not Configured");
                else
                    return PrintOut(id, settings.SqlProcDocumentPrint_AfterSubmit);
            }
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult PrintAfter_Review(int id)
        {
            LedgerHeader header = _LedgerHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Approved)
            {
                var settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(header.DocTypeId, header.DivisionId, header.SiteId);
                if (string.IsNullOrEmpty(settings.SqlProcDocumentPrint_AfterApprove))
                    throw new Exception("Document Print Not Configured");
                else
                    return PrintOut(id, settings.SqlProcDocumentPrint_AfterApprove);
            }
            else
                return HttpNotFound();
        }



        [HttpGet]
        public ActionResult Email()
        {
            //To Be Implemented
            return View("~/Views/Shared/UnderImplementation.cshtml");
        }

        [HttpGet]
        public ActionResult Report(int id)
        {
            DocumentType Dt = new DocumentType();
            Dt = new DocumentTypeService(_unitOfWork).Find(id);

            Dictionary<int, string> DefaultValue = new Dictionary<int, string>();

            if (!Dt.ReportMenuId.HasValue)
                throw new Exception("Report Menu not configured in document types");

            Model.Models.Menu menu = new MenuService(_unitOfWork).Find(Dt.ReportMenuId ?? 0);

            if (menu != null)
            {
                ReportHeader header = new ReportHeaderService(_unitOfWork).GetReportHeaderByName(menu.MenuName);

                ReportLine Line = new ReportLineService(_unitOfWork).GetReportLineByName("DocumentType", header.ReportHeaderId);
                if (Line != null)
                    DefaultValue.Add(Line.ReportLineId, id.ToString());
                ReportLine Site = new ReportLineService(_unitOfWork).GetReportLineByName("Site", header.ReportHeaderId);
                if (Site != null)
                    DefaultValue.Add(Site.ReportLineId, ((int)System.Web.HttpContext.Current.Session["SiteId"]).ToString());
                ReportLine Division = new ReportLineService(_unitOfWork).GetReportLineByName("Division", header.ReportHeaderId);
                if (Division != null)
                    DefaultValue.Add(Division.ReportLineId, ((int)System.Web.HttpContext.Current.Session["DivisionId"]).ToString());

            }

            TempData["ReportLayoutDefaultValues"] = DefaultValue;

            return Redirect((string)System.Configuration.ConfigurationManager.AppSettings["CustomizeDomain"] + "/Report_ReportPrint/ReportPrint/?MenuId=" + Dt.ReportMenuId);

        }

        public JsonResult GetNarrationList()
        {
            return Json(new NarrationService(_unitOfWork).GetNarrationNames().ToList());
        }


        public int PendingToSubmitCount(int id)
        {
            return (_LedgerHeaderService.GetLedgerHeaderListPendingToSubmit(id, User.Identity.Name)).Count();
        }

        public int PendingToReviewCount(int id)
        {
            return (_LedgerHeaderService.GetLedgerHeaderListPendingToReview(id, User.Identity.Name)).Count();
        }

        public JsonResult GetLedgerAccount(string searchTerm, int pageSize, int pageNum, int filter)//filter:PersonId
        {
            int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];

            var Settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(filter, DivisionId, SiteId);

            var temp = new LedgerLineService(_unitOfWork).GetLedgerAccounts(searchTerm, Settings.filterLedgerAccountGroupHeaders, Settings.filterPersonProcessHeaders).Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();

            var count = new LedgerLineService(_unitOfWork).GetLedgerAccounts(searchTerm, Settings.filterLedgerAccountGroupHeaders, Settings.filterPersonProcessHeaders).Count();

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
            int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];

            var Settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(filter, DivisionId, SiteId);

            var temp = new LedgerLineService(_unitOfWork).GetCostCenters(searchTerm, Settings.filterDocTypeCostCenter, Settings.filterPersonProcessHeaders).Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();

            var count = new LedgerLineService(_unitOfWork).GetCostCenters(searchTerm, Settings.filterDocTypeCostCenter, Settings.filterPersonProcessHeaders).Count();

            ComboBoxPagedResult Data = new ComboBoxPagedResult();
            Data.Results = temp;
            Data.Total = count;

            return new JsonpResult
            {
                Data = Data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult Wizard(int id)//Document Type Id
        {
            //ControllerAction ca = new ControllerActionService(_unitOfWork).Find(id);
            JobOrderHeaderViewModel vm = new JobOrderHeaderViewModel();

            vm.DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            vm.SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

            var settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(id, vm.DivisionId, vm.SiteId);

            if (settings != null)
            {
                if (settings.WizardMenuId != null)
                {
                    MenuViewModel menuviewmodel = new MenuService(_unitOfWork).GetMenu((int)settings.WizardMenuId);

                    if (menuviewmodel == null)
                    {
                        return View("~/Views/Shared/UnderImplementation.cshtml");
                    }
                    else if (!string.IsNullOrEmpty(menuviewmodel.URL))
                    {
                        return Redirect(System.Configuration.ConfigurationManager.AppSettings[menuviewmodel.URL] + "/" + menuviewmodel.ControllerName + "/" + menuviewmodel.ActionName + "/" + menuviewmodel.RouteId + "?MenuId=" + menuviewmodel.MenuId);
                    }
                    else
                    {
                        return RedirectToAction(menuviewmodel.ActionName, menuviewmodel.ControllerName, new { MenuId = menuviewmodel.MenuId, id = menuviewmodel.RouteId });
                    }
                }
            }
            return RedirectToAction("Index", new { id = id });
        }

        public ActionResult GeneratePrints(string Ids, int DocTypeId)
        {

            if (!string.IsNullOrEmpty(Ids))
            {
                int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
                int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];

                var Settings = new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(DocTypeId, DivisionId, SiteId);

                try
                {

                    List<byte[]> PdfStream = new List<byte[]>();
                    foreach (var item in Ids.Split(',').Select(Int32.Parse))
                    {

                        DirectReportPrint drp = new DirectReportPrint();

                        var pd = db.LedgerHeader.Find(item);

                        LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                        {
                            DocTypeId = pd.DocTypeId,
                            DocId = pd.LedgerHeaderId,
                            ActivityType = (int)ActivityTypeContants.Print,
                            DocNo = pd.DocNo,
                            DocDate = pd.DocDate,
                            DocStatus = pd.Status,
                        }));

                        byte[] Pdf;

                        if (pd.Status == (int)StatusConstants.Drafted || pd.Status == (int)StatusConstants.Import || pd.Status == (int)StatusConstants.Modified)
                        {
                            //LogAct(item.ToString());
                            Pdf = drp.DirectDocumentPrint(Settings.SqlProcDocumentPrint, User.Identity.Name, item);

                            PdfStream.Add(Pdf);
                        }
                        else if (pd.Status == (int)StatusConstants.Submitted || pd.Status == (int)StatusConstants.ModificationSubmitted)
                        {
                            Pdf = drp.DirectDocumentPrint(Settings.SqlProcDocumentPrint_AfterSubmit, User.Identity.Name, item);

                            PdfStream.Add(Pdf);
                        }
                        else
                        {
                            Pdf = drp.DirectDocumentPrint(Settings.SqlProcDocumentPrint_AfterApprove, User.Identity.Name, item);
                            PdfStream.Add(Pdf);
                        }

                    }

                    PdfMerger pm = new PdfMerger();

                    byte[] Merge = pm.MergeFiles(PdfStream);

                    if (Merge != null)
                        return File(Merge, "application/pdf");

                }

                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    return Json(new { success = "Error", data = message }, JsonRequestBehavior.AllowGet);
                }


                return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { success = "Error", data = "No Records Selected." }, JsonRequestBehavior.AllowGet);

        }
        #region submitValidation
        public bool Submitvalidation(int id, out string Msg)
        {
            Msg = "";
            int LedgerLine = (new LedgerService(_unitOfWork).GetLineListForReceiptVoucher(id)).Count();
            if (LedgerLine == 0)
            {
                Msg = "Add Line Record. <br />";
            }
            else
            {
                Msg = "";
            }
            return (string.IsNullOrEmpty(Msg));
        }

        #endregion submitValidation
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
