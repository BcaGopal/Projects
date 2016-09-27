using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Core.Common;
using Model.Models;
using Data.Models;
using Model.ViewModels;
using Service;
using Presentation.Helper;
using Data.Infrastructure;
using Presentation.ViewModels;
using AutoMapper;
using Presentation;
using System.Text;
using EmailContents;
using Model.ViewModel;
using System.Xml.Linq;
using Reports.Controllers;
using Reports.Reports;
using System.Configuration;

namespace Web
{
    [Authorize]
    public class SaleEnquiryHeaderController : System.Web.Mvc.Controller
    {

        private ApplicationDbContext context = new ApplicationDbContext();        

        List<string> UserRoles = new List<string>();
        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        bool TimePlanValidation = true;
        string ExceptionMsg = "";
        bool Continue = true;

        ISaleEnquiryHeaderService _SaleEnquiryHeaderService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;
        public SaleEnquiryHeaderController(ISaleEnquiryHeaderService PurchaseOrderHeaderService, IActivityLogService ActivityLogService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _SaleEnquiryHeaderService = PurchaseOrderHeaderService;
            _unitOfWork = unitOfWork;
            _exception = exec;

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

        // GET: /SaleEnquiryHeader/

        public ActionResult Index(int id, string IndexType)//DocumentTypeId
        {
            if (IndexType == "PTS")
            {
                return RedirectToAction("Index_PendingToSubmit", new { id });
            }
            else if (IndexType == "PTR")
            {
                return RedirectToAction("Index_PendingToReview", new { id });
            }
            IQueryable<SaleEnquiryHeaderIndexViewModel> p = _SaleEnquiryHeaderService.GetSaleEnquiryHeaderList(id, User.Identity.Name);

            ViewBag.Name = new DocumentTypeService(_unitOfWork).Find(id).DocumentTypeName;
            ViewBag.id = id;
            ViewBag.PendingToSubmit = PendingToSubmitCount(id);
            ViewBag.PendingToReview = PendingToReviewCount(id);
            ViewBag.IndexStatus = "All";
            return View(p);
        }

        public ActionResult Index_PendingToSubmit(int id)
        {
            var PendingToSubmit = _SaleEnquiryHeaderService.GetSaleEnquiryHeaderListPendingToSubmit(id, User.Identity.Name);

            ViewBag.Name = new DocumentTypeService(_unitOfWork).Find(id).DocumentTypeName;
            ViewBag.id = id;
            ViewBag.PendingToSubmit = PendingToSubmitCount(id);
            ViewBag.PendingToReview = PendingToReviewCount(id);
            ViewBag.IndexStatus = "PTS";
            return View("Index", PendingToSubmit);
        }

        public ActionResult Index_PendingToReview(int id)
        {
            var PendingtoReview = _SaleEnquiryHeaderService.GetSaleEnquiryHeaderListPendingToReview(id, User.Identity.Name);
            ViewBag.Name = new DocumentTypeService(_unitOfWork).Find(id).DocumentTypeName;
            ViewBag.id = id;
            ViewBag.PendingToSubmit = PendingToSubmitCount(id);
            ViewBag.PendingToReview = PendingToReviewCount(id);
            ViewBag.IndexStatus = "PTR";
            return View("Index", PendingtoReview);
        }


        [HttpGet]
        public ActionResult NextPage(int id, string name)//CurrentHeaderId
        {
            var nextId = _SaleEnquiryHeaderService.NextId(id);
            return RedirectToAction("Edit", new { id = nextId });
        }
        [HttpGet]
        public ActionResult PrevPage(int id, string name)//CurrentHeaderId
        {
            var nextId = _SaleEnquiryHeaderService.PrevId(id);
            return RedirectToAction("Edit", new { id = nextId });
        }

        [HttpGet]
        public ActionResult History()
        {
            //To Be Implemented
            //return View("~/Views/Shared/UnderImplementation.cshtml");
            MultipleDocumentPrint rvm = new MultipleDocumentPrint()
            {
                // id = 5,
            };
            return PartialView("MultipleDocumentPrint", rvm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult History(MultipleDocumentPrint vm)
        {
            ApplicationDbContext Db = new ApplicationDbContext();
            String query = Db.strSchemaName + ".ProcSaleEnquiryPrint " + 3.ToString();
            //String query = Db.strSchemaName + ".ProcSaleEnquiryPrintToMultiple '" + PackingId.ToString() + "'";
            String ReportTitle = "Sale Order";
            return RedirectToAction("DocumentPrint", "Report_DocumentPrint", new { queryString = query, ReportTitle = ReportTitle });
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

        [HttpGet]
        public ActionResult Remove()
        {
            //To Be Implemented
            return View("~/Views/Shared/UnderImplementation.cshtml");
        }

        private void PrepareViewBag(SaleEnquiryHeaderIndexViewModel s)
        {


            ViewBag.UnitConvForList = (from p in context.UnitConversonFor
                                       select p).ToList();
            ViewBag.Name = new DocumentTypeService(_unitOfWork).Find(s.DocTypeId).DocumentTypeName;
            ViewBag.id = s.DocTypeId;
            //ViewBag.DivisionList = new DivisionService(_unitOfWork).GetDivisionList().ToList();            
            ViewBag.ShipMethodList = new ShipMethodService(_unitOfWork).GetShipMethodList().ToList();
            ViewBag.DeliveryTermsList = new DeliveryTermsService(_unitOfWork).GetDeliveryTermsList().ToList();
            ViewBag.BuyerList = new BuyerService(_unitOfWork).GetBuyerList().ToList();
            ViewBag.CurrencyList = new CurrencyService(_unitOfWork).GetCurrencyList().ToList();
            //ViewBag.SiteList = new SiteService(_unitOfWork).GetSiteList().ToList();
            List<SelectListItem> temp = new List<SelectListItem>();
            temp.Add(new SelectListItem { Text = Enum.GetName(typeof(SaleEnquiryPriority), -10), Value = ((int)(SaleEnquiryPriority.Low)).ToString() });
            temp.Add(new SelectListItem { Text = Enum.GetName(typeof(SaleEnquiryPriority), 0), Value = ((int)(SaleEnquiryPriority.Normal)).ToString() });
            temp.Add(new SelectListItem { Text = Enum.GetName(typeof(SaleEnquiryPriority), 10), Value = ((int)(SaleEnquiryPriority.High)).ToString() });

            if (s == null)
                ViewBag.Priority = new SelectList(temp, "Value", "Text");
            else
                ViewBag.Priority = new SelectList(temp, "Value", "Text", s.Priority);

        }

        // GET: /SaleEnquiryHeader/Create

        public ActionResult Create(int id)
        {

            SaleEnquiryHeaderIndexViewModel p = new SaleEnquiryHeaderIndexViewModel();

            p.DocDate = DateTime.Now.Date;
            p.DueDate = DateTime.Now.Date;
            p.CreatedDate = DateTime.Now;
            p.DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            p.SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            SaleEnquirySettings temp = new SaleEnquirySettingsService(_unitOfWork).GetSaleEnquirySettings(id, p.DivisionId, p.SiteId);
            if (temp != null)
            {
                p.DocTypeId = id;
                p.ShipMethodId = temp.ShipMethodId;
                p.DeliveryTermsId = temp.DeliveryTermsId;
                p.Priority = temp.Priority;
                p.CurrencyId = temp.CurrencyId;
                p.UnitConversionForId = temp.UnitConversionForId;
                p.ProcessId = temp.ProcessId;
                PrepareViewBag(p);
            }
            else
            {
                return RedirectToAction("Edit", "SaleEnquirySettings", new { id = id }).Warning("Please create sale order settings");
            }

            p.SaleEnquirySettings = Mapper.Map<SaleEnquirySettings, SaleEnquirySettingsViewModel>(temp);
            ViewBag.Mode = "Add";
            p.DocNo = new DocumentTypeService(_unitOfWork).FGetNewDocNo("DocNo", ConfigurationManager.AppSettings["DataBaseSchema"] + ".SaleEnquiryHeaders", p.DocTypeId, p.DocDate, p.DivisionId, p.SiteId);
            return View("Create", p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HeaderPost(SaleEnquiryHeaderIndexViewModel svm)
        {
            if (svm.DocDate > svm.DueDate)
            {
                ModelState.AddModelError("DueDate", "DueDate cannot be greater than DocDate");
            }

            #region DocTypeTimeLineValidation

            try
            {

                if (svm.SaleEnquiryHeaderId <= 0)
                    TimePlanValidation = DocumentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(svm), DocumentTimePlanTypeConstants.Create, User.Identity.Name, out ExceptionMsg, out Continue);
                else
                    TimePlanValidation = DocumentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(svm), DocumentTimePlanTypeConstants.Modify, User.Identity.Name, out ExceptionMsg, out Continue);

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

            if (ModelState.IsValid && (TimePlanValidation || Continue))
            {

                #region CreateRecord
                if (svm.SaleEnquiryHeaderId == 0)
                {
                    SaleEnquiryHeader s = Mapper.Map<SaleEnquiryHeaderIndexViewModel, SaleEnquiryHeader>(svm);

                    s.ActualDueDate = s.DueDate;
                    s.CreatedDate = DateTime.Now;
                    s.ModifiedDate = DateTime.Now;
                    s.CreatedBy = User.Identity.Name;
                    s.ModifiedBy = User.Identity.Name;
                    s.Status = (int)StatusConstants.Drafted;
                    _SaleEnquiryHeaderService.Create(s);

                    try
                    {
                        _unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                        PrepareViewBag(svm);
                        ViewBag.Mode = "Add";
                        return View("Create", svm);
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = s.DocTypeId,
                        DocId = s.SaleEnquiryHeaderId,
                        ActivityType = (int)ActivityTypeContants.Added,
                        DocNo = s.DocNo,
                        DocDate = s.DocDate,
                        DocStatus = s.Status,
                    }));

                    return RedirectToAction("Modify", new { id = s.SaleEnquiryHeaderId }).Success("Data saved Successfully");
                } 
                #endregion

                #region EditRecord
                else
                {
                    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

                    //string tempredirect = (Request["Redirect"].ToString());
                    SaleEnquiryHeader s = Mapper.Map<SaleEnquiryHeaderIndexViewModel, SaleEnquiryHeader>(svm);
                    StringBuilder logstring = new StringBuilder();
                    SaleEnquiryHeader temp = _SaleEnquiryHeaderService.Find(s.SaleEnquiryHeaderId);

                    SaleEnquiryHeader ExRec = new SaleEnquiryHeader();
                    ExRec = Mapper.Map<SaleEnquiryHeader>(temp);

                    int status = temp.Status;

                    if (temp.Status != (int)StatusConstants.Drafted && temp.Status != (int)StatusConstants.Import)
                        temp.Status = (int)StatusConstants.Modified;

                    temp.DocTypeId = s.DocTypeId;
                    temp.DocDate = s.DocDate;
                    temp.DocNo = s.DocNo;
                    temp.BuyerEnquiryNo = s.BuyerEnquiryNo;
                    temp.SaleToBuyerId = s.SaleToBuyerId;
                    temp.BillToBuyerId = s.BillToBuyerId;
                    temp.CurrencyId = s.CurrencyId;
                    temp.Priority = s.Priority;
                    temp.UnitConversionForId = s.UnitConversionForId;
                    temp.ShipMethodId = s.ShipMethodId;
                    temp.ShipAddress = s.ShipAddress;
                    temp.DeliveryTermsId = s.DeliveryTermsId;
                    temp.Remark = s.Remark;
                    temp.DueDate = s.DueDate;
                    temp.Advance = s.Advance;
                    temp.ModifiedDate = DateTime.Now;
                    temp.ModifiedBy = User.Identity.Name;
                    _SaleEnquiryHeaderService.Update(temp);

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
                        TempData["CSEXC"] += message;
                        PrepareViewBag(svm);
                        ViewBag.Mode = "Edit";
                        return View("Create", svm);
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = temp.DocTypeId,
                        DocId = temp.SaleEnquiryHeaderId,
                        ActivityType = (int)ActivityTypeContants.Modified,
                        DocNo = temp.DocNo,
                        xEModifications = Modifications,
                        DocDate = temp.DocDate,
                        DocStatus = temp.Status,
                    }));

                    return RedirectToAction("Index", new { id = svm.DocTypeId }).Success("Data saved successfully");
                } 
                #endregion

            }
            PrepareViewBag(svm);
            ViewBag.Mode = "Add";
            return View("Create", svm);
        }

        [HttpGet]
        public ActionResult Modify(int id, string IndexType)
        {
            SaleEnquiryHeader header = _SaleEnquiryHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Drafted || header.Status == (int)StatusConstants.Import)
                return Edit(id, IndexType);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult ModifyAfter_Submit(int id, string IndexType)
        {
            SaleEnquiryHeader header = _SaleEnquiryHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Submitted || header.Status == (int)StatusConstants.Modified || header.Status == (int)StatusConstants.ModificationSubmitted)
                return Edit(id, IndexType);
            else
                return HttpNotFound();
        }


        // GET: /SaleEnquiryHeader/Edit/5
        private ActionResult Edit(int id, string IndexType)
        {
            ViewBag.IndexStatus = IndexType;

            SaleEnquiryHeader s = _SaleEnquiryHeaderService.GetSaleEnquiryHeader(id);
            if (s == null)
            {
                return HttpNotFound();
            }

            #region DocTypeTimeLineValidation
            try
            {

                TimePlanValidation = DocumentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(s), DocumentTimePlanTypeConstants.Modify, User.Identity.Name, out ExceptionMsg, out Continue);

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


            SaleEnquiryHeaderIndexViewModel svm = Mapper.Map<SaleEnquiryHeader, SaleEnquiryHeaderIndexViewModel>(s);
            PrepareViewBag(svm);
            ViewBag.Mode = "Edit";

            SaleEnquirySettings temp = new SaleEnquirySettingsService(_unitOfWork).GetSaleEnquirySettings(s.DocTypeId, s.DivisionId, s.SiteId);
            svm.SaleEnquirySettings = Mapper.Map<SaleEnquirySettings, SaleEnquirySettingsViewModel>(temp);
            svm.ProcessId = temp.ProcessId;

            if (!(System.Web.HttpContext.Current.Request.UrlReferrer.PathAndQuery).Contains("Create"))
                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = s.DocTypeId,
                    DocId = s.SaleEnquiryHeaderId,
                    ActivityType = (int)ActivityTypeContants.Detail,
                    DocNo = s.DocNo,
                    DocDate = s.DocDate,
                    DocStatus = s.Status,
                }));

            return View("Create", svm);
        }

        [HttpGet]
        public ActionResult DetailInformation(int id, string IndexType)
        {
            return RedirectToAction("Detail", new { id = id, transactionType = "detail", IndexType = IndexType });
        }


        [Authorize]
        public ActionResult Detail(int id, string transactionType, string IndexType)
        {

            ViewBag.transactionType = transactionType;
            ViewBag.IndexStatus = IndexType;

            SaleEnquiryHeader s = _SaleEnquiryHeaderService.GetSaleEnquiryHeader(id);
            SaleEnquiryHeaderIndexViewModel svm = Mapper.Map<SaleEnquiryHeader, SaleEnquiryHeaderIndexViewModel>(s);


            var settings = new SaleEnquirySettingsService(_unitOfWork).GetSaleEnquirySettings(s.DocTypeId, s.DivisionId , s.SiteId);
            svm.SaleEnquirySettings = Mapper.Map<SaleEnquirySettings, SaleEnquirySettingsViewModel>(settings);


            PrepareViewBag(svm);
            if (s == null)
            {
                return HttpNotFound();
            }

            if (String.IsNullOrEmpty(transactionType) || transactionType == "detail")
                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = s.DocTypeId,
                    DocId = s.SaleEnquiryHeaderId,
                    ActivityType = (int)ActivityTypeContants.Detail,
                    DocNo = s.DocNo,
                    DocDate = s.DocDate,
                    DocStatus = s.Status,
                }));


            return View("Create", svm);
        }


        public ActionResult Delete(int id)
        {
            SaleEnquiryHeader header = _SaleEnquiryHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Drafted || header.Status == (int)StatusConstants.Import)
                return Remove(id);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult DeleteAfter_Submit(int id)
        {
            SaleEnquiryHeader header = _SaleEnquiryHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Submitted || header.Status == (int)StatusConstants.Modified)
                return Remove(id);
            else
                return HttpNotFound();
        }


        // GET: /PurchaseOrderHeader/Delete/5

        private ActionResult Remove(int id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleEnquiryHeader SaleEnquiryHeader = _SaleEnquiryHeaderService.GetSaleEnquiryHeader(id);
            if (SaleEnquiryHeader == null)
            {
                return HttpNotFound();
            }

            #region DocTypeTimeLineValidation
            try
            {
                TimePlanValidation = DocumentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(SaleEnquiryHeader), DocumentTimePlanTypeConstants.Delete, User.Identity.Name, out ExceptionMsg, out Continue);
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

            ReasonViewModel rvm = new ReasonViewModel()
            {
                id = id,
            };
            return PartialView("_Reason", rvm);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ReasonViewModel vm)
        {
            if (ModelState.IsValid)
            {
                List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

                //string temp = (Request["Redirect"].ToString());
                //first find the Purchase Order Object based on the ID. (sience this object need to marked to be deleted IE. ObjectState.Deleted)
                var SaleEnquiryHeader = _SaleEnquiryHeaderService.GetSaleEnquiryHeader(vm.id);

                LogList.Add(new LogTypeViewModel
                {
                    ExObj = SaleEnquiryHeader,
                });

                //Then find all the Purchase Order Header Line associated with the above ProductType.
                var SaleEnquiryLine = new SaleEnquiryLineService(_unitOfWork).GetSaleEnquiryLineList(vm.id);

                //Mark ObjectState.Delete to all the Purchase Order Lines. 
                foreach (var item in SaleEnquiryLine)
                {

                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = item,
                    });

                    new SaleEnquiryLineStatusService(_unitOfWork).Delete(item.SaleEnquiryLineId);

                    new SaleEnquiryLineService(_unitOfWork).Delete(item.SaleEnquiryLineId);
                }

                // Now delete the Purhcase Order Header
                new SaleEnquiryHeaderService(_unitOfWork).Delete(vm.id);
                XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);
                //Commit the DB
                try
                {
                    _unitOfWork.Save();
                }

                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                    return PartialView("_Reason", vm);
                }

                //Logging Activity

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = SaleEnquiryHeader.DocTypeId,
                    DocId = SaleEnquiryHeader.SaleEnquiryHeaderId,
                    ActivityType = (int)ActivityTypeContants.Deleted,
                    UserRemark = vm.Reason,
                    DocNo = SaleEnquiryHeader.DocNo,
                    xEModifications = Modifications,
                    DocDate = SaleEnquiryHeader.DocDate,
                    DocStatus = SaleEnquiryHeader.Status,
                }));

                return Json(new { success = true });
            }
            return PartialView("_Reason", vm);
        }



        public ActionResult Submit(int id, string IndexType, string TransactionType)
        {
            #region DocTypeTimeLineValidation

            SaleEnquiryHeader s = context.SaleEnquiryHeader.Find(id);

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
            SaleEnquiryHeader pd = new SaleEnquiryHeaderService(_unitOfWork).Find(Id);
            if (ModelState.IsValid)
            {
                if (User.Identity.Name == pd.ModifiedBy || UserRoles.Contains("Admin"))
                {
                    int ActivityType;

                    pd.ReviewBy = null;
                    pd.Status = (int)StatusConstants.Submitted;
                    ActivityType = (int)ActivityTypeContants.Submitted;


                    int LedgerHeaderId = 0;
                    LedgerHeaderId = LedgerPost(pd);
                    pd.LedgerHeaderId = LedgerHeaderId;

                    _SaleEnquiryHeaderService.Update(pd);


                    _unitOfWork.Save();

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = pd.DocTypeId,
                        DocId = pd.SaleEnquiryHeaderId,
                        ActivityType = ActivityType,
                        UserRemark = UserRemark,
                        DocNo = pd.DocNo,
                        DocDate = pd.DocDate,
                        DocStatus = pd.Status,
                    }));

                    //SendEmail_PODrafted(Id);
                    //if (pd.Status == (int)StatusConstants.Submitted)
                    //    SaleEnquiryEmailContents.SendSaleEnquirySubmitEmail(Id);
                    //else if (pd.Status == (int)StatusConstants.ModificationSubmitted)
                    //    SaleEnquiryEmailContents.SendSaleEnquiryModifiedEmail(Id, UserRemark);

                    return RedirectToAction("Index", new { id = pd.DocTypeId, IndexType = IndexType }).Success("Record Submitted successfully.");
                }
                else
                    return RedirectToAction("Index", new { id = pd.DocTypeId, IndexType = IndexType }).Warning("Record can be submitted by user " + pd.ModifiedBy + " only.");
            }

            return View();
        }

        public int LedgerPost(SaleEnquiryHeader pd)
        {
            int LedgerHeaderId = 0;

            if (pd.LedgerHeaderId == 0 || pd.LedgerHeaderId == null)
            {
                LedgerHeader LedgerHeader = new LedgerHeader();

                LedgerHeader.DocTypeId = pd.DocTypeId;
                LedgerHeader.DocDate = pd.DocDate;
                LedgerHeader.DocNo = pd.DocNo;
                LedgerHeader.DivisionId = pd.DivisionId;
                LedgerHeader.SiteId = pd.SiteId;
                LedgerHeader.Remark = pd.Remark;
                LedgerHeader.CreatedBy = pd.CreatedBy;
                LedgerHeader.CreatedDate = DateTime.Now.Date;
                LedgerHeader.ModifiedBy = pd.ModifiedBy;
                LedgerHeader.ModifiedDate = DateTime.Now.Date;

                new LedgerHeaderService(_unitOfWork).Create(LedgerHeader);
            }
            else
            {
                LedgerHeader LedgerHeader = new LedgerHeaderService(_unitOfWork).Find((int)pd.LedgerHeaderId);

                LedgerHeader.DocTypeId = pd.DocTypeId;
                LedgerHeader.DocDate = pd.DocDate;
                LedgerHeader.DocNo = pd.DocNo;
                LedgerHeader.DivisionId = pd.DivisionId;
                LedgerHeader.SiteId = pd.SiteId;
                LedgerHeader.Remark = pd.Remark;
                LedgerHeader.ModifiedBy = pd.ModifiedBy;
                LedgerHeader.ModifiedDate = DateTime.Now.Date;

                new LedgerHeaderService(_unitOfWork).Update(LedgerHeader);

                IEnumerable<Ledger> LedgerList = new LedgerService(_unitOfWork).FindForLedgerHeader(LedgerHeader.LedgerHeaderId);

                foreach (Ledger item in LedgerList)
                {
                    new LedgerService(_unitOfWork).Delete(item);
                }

                LedgerHeaderId = LedgerHeader.LedgerHeaderId;
            }


            int SalesAc = new LedgerAccountService(_unitOfWork).Find(LedgerAccountConstants.Sale).LedgerAccountId;

            if (pd.Advance > 0)
            {
                Ledger LedgerDr = new Ledger();
                if (LedgerHeaderId != 0) { LedgerDr.LedgerHeaderId = LedgerHeaderId; }
                LedgerDr.LedgerAccountId = SalesAc;
                LedgerDr.ContraLedgerAccountId = pd.BillToBuyerId;
                LedgerDr.AmtDr = pd.Advance ?? 0;
                LedgerDr.AmtCr = 0;
                LedgerDr.Narration = "";
                new LedgerService(_unitOfWork).Create(LedgerDr);

                Ledger LedgerCr = new Ledger();
                if (LedgerHeaderId != 0) { LedgerCr.LedgerHeaderId = LedgerHeaderId; }
                LedgerCr.LedgerAccountId = pd.BillToBuyerId;
                LedgerCr.ContraLedgerAccountId = SalesAc;
                LedgerCr.AmtDr = 0;
                LedgerCr.AmtCr = pd.Advance ?? 0;
                LedgerCr.Narration = "";
                new LedgerService(_unitOfWork).Create(LedgerCr);
            }

            return LedgerHeaderId;
        }



        public ActionResult Review(int id, string IndexType, string TransactionType)
        {
            return RedirectToAction("Detail", new { id = id, IndexType = IndexType, transactionType = string.IsNullOrEmpty(TransactionType) ? "review" : TransactionType });
        }


        [HttpPost, ActionName("Detail")]
        [MultipleButton(Name = "Command", Argument = "Review")]
        public ActionResult Reviewed(int Id, string IndexType, string UserRemark, string IsContinue)
        {
            SaleEnquiryHeader pd = new SaleEnquiryHeaderService(_unitOfWork).Find(Id);

            if (ModelState.IsValid)
            {


                pd.ReviewCount = (pd.ReviewCount ?? 0) + 1;
                pd.ReviewBy += User.Identity.Name + ", ";

                _SaleEnquiryHeaderService.Update(pd);

                _unitOfWork.Save();

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = pd.DocTypeId,
                    DocId = pd.SaleEnquiryHeaderId,
                    ActivityType = (int)ActivityTypeContants.Reviewed,
                    UserRemark = UserRemark,
                    DocNo = pd.DocNo,
                    DocDate = pd.DocDate,
                    DocStatus = pd.Status,
                }));

                return RedirectToAction("Index", new { id = pd.DocTypeId, IndexType = IndexType }).Success("Record reviewed successfully.");
            }

            return RedirectToAction("Index", new { id = pd.DocTypeId, IndexType = IndexType }).Warning("Error in reviewing.");
        }

        public int PendingToSubmitCount(int id)
        {
            return (_SaleEnquiryHeaderService.GetSaleEnquiryHeaderListPendingToSubmit(id, User.Identity.Name)).Count();
        }

        public int PendingToReviewCount(int id)
        {
            return (_SaleEnquiryHeaderService.GetSaleEnquiryHeaderListPendingToReview(id, User.Identity.Name)).Count();
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
                context.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Print(int id)
        {
            String query = "Web.ProcSaleEnquiryPrint ";
            return Redirect((string)System.Configuration.ConfigurationManager.AppSettings["CustomizeDomain"] + "/Report_DocumentPrint/DocumentPrint/?DocumentId=" + id + "&queryString=" + query);
        }



        public ActionResult GeneratePrints(string Ids, int DocTypeId)
        {

            if (!string.IsNullOrEmpty(Ids))
            {
                int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
                int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];

                //var Settings = new JobOrderSettingsService(_unitOfWork).GetJobOrderSettingsForDocument(DocTypeId, DivisionId, SiteId);

                try
                {

                    List<byte[]> PdfStream = new List<byte[]>();
                    foreach (var item in Ids.Split(',').Select(Int32.Parse))
                    {

                        DirectReportPrint drp = new DirectReportPrint();

                        var pd = context.SaleEnquiryHeader.Find(item);

                        LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                        {
                            DocTypeId = pd.DocTypeId,
                            DocId = pd.SaleEnquiryHeaderId,
                            ActivityType = (int)ActivityTypeContants.Print,
                            DocNo = pd.DocNo,
                            DocDate = pd.DocDate,
                            DocStatus = pd.Status,
                        }));

                        byte[] Pdf;

                        if (pd.Status == (int)StatusConstants.Drafted || pd.Status == (int)StatusConstants.Import || pd.Status == (int)StatusConstants.Modified)
                        {
                            //LogAct(item.ToString());
                            Pdf = drp.DirectDocumentPrint("Web.ProcSaleEnquiryPrint ", User.Identity.Name, item);

                            PdfStream.Add(Pdf);
                        }
                        else if (pd.Status == (int)StatusConstants.Submitted || pd.Status == (int)StatusConstants.ModificationSubmitted)
                        {
                            Pdf = drp.DirectDocumentPrint("Web.ProcSaleEnquiryPrint ", User.Identity.Name, item);

                            PdfStream.Add(Pdf);
                        }
                        else
                        {
                            Pdf = drp.DirectDocumentPrint("Web.ProcSaleEnquiryPrint ", User.Identity.Name, item);
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


        public ActionResult Import(int id)//Document Type Id
        {
            int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

            var settings = new SaleEnquirySettingsService(_unitOfWork).GetSaleEnquirySettings(id, DivisionId, SiteId);

            if (settings != null)
            {
                if (settings.ImportMenuId != null)
                {
                    MenuViewModel menuviewmodel = new MenuService(_unitOfWork).GetMenu((int)settings.ImportMenuId);

                    if (menuviewmodel == null)
                    {
                        return View("~/Views/Shared/UnderImplementation.cshtml");
                    }
                    else if (!string.IsNullOrEmpty(menuviewmodel.URL))
                    {
                        return Redirect(System.Configuration.ConfigurationManager.AppSettings[menuviewmodel.URL] + "/" + menuviewmodel.ControllerName + "/" + menuviewmodel.ActionName + "/" + id + "?MenuId=" + menuviewmodel.MenuId);
                    }
                    else
                    {
                        return RedirectToAction(menuviewmodel.ActionName, menuviewmodel.ControllerName, new { MenuId = menuviewmodel.MenuId, id = id });
                    }
                }
            }
            return RedirectToAction("Index", new { id = id });
        }


    }
}
