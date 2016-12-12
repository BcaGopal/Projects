using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Core.Common;
using Model.Models;
using Data.Models;
using Service;
using Presentation.Helper;
using Data.Infrastructure;
using Presentation.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System.Configuration;
using Presentation;
using Model.ViewModel;
using System.Data.SqlClient;
using System.Xml.Linq;
using CustomEventArgs;
using DocumentEvents;
using JobOrderDocumentEvents;
using Reports.Reports;
using Reports.Controllers;
namespace Web
{

    [Authorize]
    public class GatePassHeaderController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private bool EventException = false;

        List<string> UserRoles = new List<string>();
        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        bool TimePlanValidation = true;
        string ExceptionMsg = "";
        bool Continue = true;

        IGatePassHeaderService _GatePassHeaderService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        public GatePassHeaderController(IGatePassHeaderService GatePassHeaderService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _GatePassHeaderService = GatePassHeaderService;
            _exception = exec;
            _unitOfWork = unitOfWork;
            if (!JobOrderEvents.Initialized)
            {
                JobOrderEvents Obj = new JobOrderEvents();
            }

            UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;

        }

        // GET: /JobOrderHeader/


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

        public ActionResult Index(int id, string IndexType)//DocumentTypeId 
        {
            //var IpAddr = GetIPAddress();

            if (IndexType == "PTS")
            {
                return RedirectToAction("Index_PendingToSubmit", new { id });
            }
            else if (IndexType == "PTR")
            {
                return RedirectToAction("Index_PendingToReview", new { id });
            }
            IQueryable<GatePassHeaderViewModel> p =_GatePassHeaderService.GetGatePassHeaderList(id, User.Identity.Name);
            ViewBag.Name = new DocumentTypeService(_unitOfWork).Find(id).DocumentTypeName;
            PrepareViewBag(id);
            ViewBag.PendingToSubmit = PendingToSubmitCount(id);
            ViewBag.PendingToReview = PendingToReviewCount(id);
            ViewBag.IndexStatus = "All";
            ViewBag.id = id;
            return View(p);
        }

        public ActionResult Index_PendingToSubmit(int id)
        {
            IQueryable<GatePassHeaderViewModel> p = _GatePassHeaderService.GetGatePassHeaderListPendingToSubmit(id, User.Identity.Name);

            PrepareViewBag(id);
            ViewBag.PendingToSubmit = PendingToSubmitCount(id);
            ViewBag.PendingToReview = PendingToReviewCount(id);
            ViewBag.IndexStatus = "PTS";
            return View("Index", p);
        }

        public ActionResult Index_PendingToReview(int id)
        {
            IQueryable<GatePassHeaderViewModel> p = _GatePassHeaderService.GetGatePassHeaderListPendingToReview(id, User.Identity.Name);
            PrepareViewBag(id);
            ViewBag.PendingToSubmit = PendingToSubmitCount(id);
            ViewBag.PendingToReview = PendingToReviewCount(id);
            ViewBag.IndexStatus = "PTR";
            return View("Index", p);
        }

        private void PrepareViewBag(int id)
        {
            DocumentType DocType = new DocumentTypeService(_unitOfWork).Find(id);
            int Cid = DocType.DocumentCategoryId;
            ViewBag.DocTypeList = new DocumentTypeService(_unitOfWork).FindByDocumentCategory(Cid).ToList();
            ViewBag.Name = DocType.DocumentTypeName;
            ViewBag.id = id;
            ViewBag.UnitConvForList = (from p in context.UnitConversonFor
                                       select p).ToList();

            var DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            var SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

            //var settings = new JobOrderSettingsService(_unitOfWork).GetJobOrderSettingsForDocument(id, DivisionId, SiteId);
            //if (settings != null)
            //{
            //    ViewBag.WizardId = settings.WizardMenuId;
            //    ViewBag.IsPostedInStock = settings.isPostedInStock;
            //}

        }


        //[HttpGet]
        //public ActionResult BarcodePrint(int id)
        //{

        //    string GenDocId = "";

        //    JobOrderHeader header = _JobOrderHeaderService.Find(id);
        //    GenDocId = header.DocTypeId.ToString() + '-' + header.DocNo;
        //    //return RedirectToAction("PrintBarCode", "Report_BarcodePrint", new { GenHeaderId = id });
        //    return Redirect((string)System.Configuration.ConfigurationManager.AppSettings["CustomizeDomain"] + "/Report_BarcodePrint/PrintBarCode/?GenHeaderId=" + GenDocId + "&queryString=" + GenDocId);


        //}


        //public JsonResult GetJobWorkerHelpList(int Processid, string term)//Order Header ID
        //{

        //    return Json(_JobOrderHeaderService.GetJobWorkerHelpList(Processid, term), JsonRequestBehavior.AllowGet);
        //}


        // GET: /JobOrderHeader/Create

        public ActionResult Create(int id)//DocumentTypeId
        {
            GatePassHeaderViewModel p = new GatePassHeaderViewModel();

            p.DocDate = DateTime.Now;        
            p.CreatedDate = DateTime.Now;
            p.DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            p.SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            p.DocTypeId = id;
            PrepareViewBag(id);
            p.GodownId = (int)System.Web.HttpContext.Current.Session["DefaultGodownId"];
            p.DocNo = new DocumentTypeService(_unitOfWork).FGetNewDocNo("DocNo", ConfigurationManager.AppSettings["DataBaseSchema"] + ".Gatepassheaders", p.DocTypeId, p.DocDate, p.DivisionId, p.SiteId);
            ViewBag.Mode = "Add";
            ViewBag.Name = new DocumentTypeService(_unitOfWork).Find(id).DocumentTypeName;
            ViewBag.id = id;
            return View(p);
            //Getting Settings
            //var settings = new JobOrderSettingsService(_unitOfWork).GetJobOrderSettingsForDocument(id, p.DivisionId, p.SiteId);

            //if (settings == null && UserRoles.Contains("Admin"))
            //{
            //    return RedirectToAction("Create", "JobOrderSettings", new { id = id }).Warning("Please create job order settings");
            //}
            //else if (settings == null && !UserRoles.Contains("Admin"))
            //{
            //    return View("~/Views/Shared/InValidSettings.cshtml");
            //}
            //p.JobOrderSettings = Mapper.Map<JobOrderSettings, JobOrderSettingsViewModel>(settings);


            //List<PerkViewModel> Perks = new List<PerkViewModel>();
            //Perks
            //if (p.JobOrderSettings.Perks != null)
            //{
            //    foreach (var item in p.JobOrderSettings.Perks.Split(',').ToList())
            //    {
            //        PerkViewModel temp = Mapper.Map<Perk, PerkViewModel>(new PerkService(_unitOfWork).Find(Convert.ToInt32(item)));
            //        var DocTypePerk = (from p2 in context.PerkDocumentType
            //                           where p2.DocTypeId == id && p2.PerkId == temp.PerkId && p2.SiteId == p.SiteId && p2.DivisionId == p.DivisionId
            //                           select p2).FirstOrDefault();
            //        if (DocTypePerk != null)
            //        {
            //            temp.Base = DocTypePerk.Base;
            //            temp.Worth = DocTypePerk.Worth;
            //            temp.CostConversionMultiplier = DocTypePerk.CostConversionMultiplier;
            //            temp.IsEditableRate = DocTypePerk.IsEditableRate;
            //        }
            //        else
            //        {
            //            temp.Base = 0;
            //            temp.Worth = 0;
            //            temp.CostConversionMultiplier = 0;
            //            temp.IsEditableRate = true;
            //        }
            //        Perks.Add(temp);
            //    }
            //}

            //if (p.JobOrderSettings.isVisibleCostCenter)
            //{
            //    p.CostCenterName = new JobOrderHeaderService(_unitOfWork).FGetJobOrderCostCenter(p.DocTypeId, p.DocDate, p.DivisionId, p.SiteId);
            //}

            //p.PerkViewModel = Perks;
            // p.UnitConversionForId = settings.UnitConversionForId;
            //p.ProcessId = settings.ProcessId;


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(GatePassHeaderViewModel svm)
        {
            bool BeforeSave = true;
            bool CostCenterGenerated = false;

            GatePassHeader s = Mapper.Map<GatePassHeaderViewModel, GatePassHeader>(svm);

            //var settings = new JobOrderSettingsService(_unitOfWork).GetJobOrderSettingsForDocument(svm.DocTypeId, svm.DivisionId, svm.SiteId);

            //if (settings != null)
            //{
            //    if (svm.JobOrderSettings.isMandatoryCostCenter == true && (string.IsNullOrEmpty(svm.CostCenterName)))
            //    {
            //        ModelState.AddModelError("CostCenterName", "The CostCenter field is required");
            //    }
            //    if (svm.JobOrderSettings.isMandatoryMachine == true && (svm.MachineId <= 0 || svm.MachineId == null))
            //    {
            //        ModelState.AddModelError("MachineId", "The Machine field is required");
            //    }
            //    if (svm.JobOrderSettings.isVisibleGodown == true && svm.JobOrderSettings.isMandatoryGodown == true && !svm.GodownId.HasValue)
            //    {
            //        ModelState.AddModelError("GodownId", "The Godown field is required");
            //    }
            //    if (settings.MaxDays.HasValue && (svm.DueDate - svm.DocDate).Days > settings.MaxDays.Value)
            //    {
            //        ModelState.AddModelError("DueDate", "DueDate is exceeding MaxDueDays.");
            //    }
            //}

            //if (!string.IsNullOrEmpty(svm.CostCenterName))
            //{
            //    string CostCenterValidation = _JobOrderHeaderService.ValidateCostCenter(svm.DocTypeId, svm.JobOrderHeaderId, svm.JobWorkerId, svm.CostCenterName);
            //    if (!string.IsNullOrEmpty(CostCenterValidation))
            //        ModelState.AddModelError("CostCenterName", CostCenterValidation);
            //}
            #region BeforeSaveEvents

            try
            {

                if (svm.GatePassHeaderId <= 0)
                    BeforeSave = JobOrderDocEvents.beforeHeaderSaveEvent(this, new JobEventArgs(svm.GatePassHeaderId,EventModeConstants.Add), ref context);
                else
                    BeforeSave = JobOrderDocEvents.beforeHeaderSaveEvent(this, new JobEventArgs(svm.GatePassHeaderId, EventModeConstants.Edit), ref context);

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

                if (svm.GatePassHeaderId <= 0)
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

            if (ModelState.IsValid && BeforeSave && !EventException && (TimePlanValidation || Continue))
            {
                //CreateLogic
                #region CreateRecord
                if (svm.GatePassHeaderId <= 0)
                {
                    s.GodownId= (int)System.Web.HttpContext.Current.Session["DefaultGodownId"];
                    s.CreatedDate = DateTime.Now;
                    s.ModifiedDate = DateTime.Now;   
                    s.CreatedBy = User.Identity.Name;
                    s.ModifiedBy = User.Identity.Name;
                    s.Status = (int)StatusConstants.Drafted;
                    s.ObjectState = Model.ObjectState.Added;
                    context.GatePassHeader.Add(s);
                    try
                    {
                        JobOrderDocEvents.onHeaderSaveEvent(this, new JobEventArgs(s.GatePassHeaderId, EventModeConstants.Add), ref context);
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
                        //_unitOfWork.Save();
                        context.SaveChanges();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                        PrepareViewBag(svm.DocTypeId);
                        ViewBag.Mode = "Add";
                        return View("Create", svm);
                    }

                   

                    try
                    {
                        JobOrderDocEvents.afterHeaderSaveEvent(this, new JobEventArgs(s.GatePassHeaderId, EventModeConstants.Add), ref context);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = s.DocTypeId,
                        DocId = s.GatePassHeaderId,
                        ActivityType = (int)ActivityTypeContants.Added,
                        DocNo = s.DocNo,
                        DocDate = s.DocDate,
                        DocStatus = s.Status,
                    }));

                    //Update DocId in COstCenter
                 

                    return RedirectToAction("Modify", "GatePassHeader", new { Id = s.GatePassHeaderId }).Success("Data saved successfully");

                }
                #endregion


                //EditLogic
                #region EditRecord

                else
                {
                    bool GodownChanged = false;
                    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

                    GatePassHeader temp = context.GatePassHeader.Find(s.GatePassHeaderId);

                    GodownChanged = (temp.GodownId == s.GodownId) ? false : true;

                    GatePassHeader ExRec = Mapper.Map<GatePassHeader>(temp);

                    int status = temp.Status;

                    if (temp.Status != (int)StatusConstants.Drafted && temp.Status != (int)StatusConstants.Import)
                        temp.Status = (int)StatusConstants.Modified;

                    temp.DocDate = s.DocDate;
                    temp.PersonId = s.PersonId;
                    temp.GodownId =(int)System.Web.HttpContext.Current.Session["DefaultGodownId"];
                    temp.DocNo = s.DocNo;
                    temp.Remark = s.Remark;                 
                    temp.ModifiedDate = DateTime.Now;
                    temp.ModifiedBy = User.Identity.Name;
                    temp.ObjectState = Model.ObjectState.Modified;
                    context.GatePassHeader.Add(temp);
                    //_JobOrderHeaderService.Update(temp);
                    


                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = ExRec,
                        Obj = temp,
                    });

                    XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);

                    try
                    {
                        JobOrderDocEvents.onHeaderSaveEvent(this, new JobEventArgs(s.GatePassHeaderId, EventModeConstants.Edit), ref context);
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

                        context.SaveChanges();
                        //_unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);

                        PrepareViewBag(svm.DocTypeId);
                        TempData["CSEXC"] += message;
                        ViewBag.id = svm.DocTypeId;
                        ViewBag.Mode = "Edit";
                        return View("Create", svm);
                    }

                    try
                    {
                        JobOrderDocEvents.afterHeaderSaveEvent(this, new JobEventArgs(s.GatePassHeaderId, EventModeConstants.Edit), ref context);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = temp.DocTypeId,
                        DocId = temp.GatePassHeaderId,
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
            PrepareViewBag(svm.DocTypeId);
            ViewBag.Mode = "Add";
            return View("Create", svm);
        }


        [HttpGet]
        public ActionResult Modify(int id, string IndexType)
        {
            GatePassHeader header = _GatePassHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Drafted || header.Status == (int)StatusConstants.Import)
                return Edit(id, IndexType);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult ModifyAfter_Submit(int id, string IndexType)
        {
            GatePassHeader header = _GatePassHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Submitted || header.Status == (int)StatusConstants.Modified)
                return Edit(id, IndexType);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult ModifyAfter_Approve(int id, string IndexType)
        {
            GatePassHeader header = _GatePassHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Approved)
                return Edit(id, IndexType);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            GatePassHeader header = _GatePassHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Drafted || header.Status == (int)StatusConstants.Import)
                return Remove(id);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult DeleteAfter_Submit(int id)
        {
            GatePassHeader header = _GatePassHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Submitted || header.Status == (int)StatusConstants.Modified)
                return Remove(id);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult DeleteAfter_Approve(int id)
        {
            GatePassHeader header = _GatePassHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Approved)
                return Remove(id);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult DetailInformation(int id, int? DocLineId, string IndexType)
        {
            return RedirectToAction("Detail", new { id = id, transactionType = "detail", DocLineId = DocLineId, IndexType = IndexType });
        }



        // GET: /JobOrderHeader/Edit/5
        private ActionResult Edit(int id, string IndexType)
        {
            ViewBag.IndexStatus = IndexType;
            GatePassHeaderViewModel s = _GatePassHeaderService.GetGatePassHeader(id);
            #region DocTypeTimeLineValidation
            //try
            //{

            //    TimePlanValidation = DocumentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(s), DocumentTimePlanTypeConstants.Modify, User.Identity.Name, out ExceptionMsg, out Continue);

            //}
            //catch (Exception ex)
            //{
            //    string message = _exception.HandleException(ex);
            //    TempData["CSEXC"] += message;
            //    TimePlanValidation = false;
            //}

            //if (!TimePlanValidation)
            //    TempData["CSEXC"] += ExceptionMsg;
            #endregion

            //if ((!TimePlanValidation && !Continue))
            //{
            //    return RedirectToAction("DetailInformation", new { id = id, IndexType = IndexType });
            //}

            //Job Order Settings
         

           // s.JobOrderSettings = Mapper.Map<JobOrderSettings, JobOrderSettingsViewModel>(settings);

            ////Perks
           // s.PerkViewModel = new PerkService(_unitOfWork).GetPerkListForDocumentTypeForEdit(id).ToList();

            //if (s.PerkViewModel.Count == 0)
            //{
            //    List<PerkViewModel> Perks = new List<PerkViewModel>();
            //    if (s.JobOrderSettings.Perks != null)
            //        foreach (var item in s.JobOrderSettings.Perks.Split(',').ToList())
            //        {
            //            PerkViewModel temp = Mapper.Map<Perk, PerkViewModel>(new PerkService(_unitOfWork).Find(Convert.ToInt32(item)));

            //            var DocTypePerk = (from p2 in context.PerkDocumentType
            //                               where p2.DocTypeId == s.DocTypeId && p2.PerkId == temp.PerkId && p2.SiteId == s.SiteId && p2.DivisionId == s.DivisionId
            //                               select p2).FirstOrDefault();
            //            if (DocTypePerk != null)
            //            {
            //                temp.Base = DocTypePerk.Base;
            //                temp.Worth = DocTypePerk.Worth;
            //                temp.CostConversionMultiplier = DocTypePerk.CostConversionMultiplier;
            //                temp.IsEditableRate = DocTypePerk.IsEditableRate;
            //            }
            //            else
            //            {
            //                temp.Base = 0;
            //                temp.Worth = 0;
            //                temp.CostConversionMultiplier = 0;
            //                temp.IsEditableRate = true;
            //            }

            //            Perks.Add(temp);
            //        }
            //    s.PerkViewModel = Perks;
            //}
            PrepareViewBag(s.DocTypeId);
            if (s == null)
            {
                return HttpNotFound();
            }

            //ViewBag.transactionType = "detail";

            ViewBag.Mode = "Edit";
            ViewBag.transactionType = "";

            ViewBag.Name = new DocumentTypeService(_unitOfWork).Find(s.DocTypeId).DocumentTypeName;
            ViewBag.id = s.DocTypeId;

            if (!(System.Web.HttpContext.Current.Request.UrlReferrer.PathAndQuery).Contains("Create"))
                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = s.DocTypeId,
                    DocId = s.GatePassHeaderId,
                    ActivityType = (int)ActivityTypeContants.Detail,
                    DocNo = s.DocNo,
                    DocDate = s.DocDate,
                    DocStatus = s.Status,
                }));

            return View("Create", s);
        }

        private ActionResult Remove(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GatePassHeader GatePassHeader = _GatePassHeaderService.Find(id);

            if (GatePassHeader == null)
            {
                return HttpNotFound();
            }

            #region DocTypeTimeLineValidation

            try
            {
                TimePlanValidation = DocumentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(GatePassHeader), DocumentTimePlanTypeConstants.Delete, User.Identity.Name, out ExceptionMsg, out Continue);
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



        [Authorize]
        public ActionResult Detail(int id, string IndexType, string transactionType, int? DocLineId)
        {
            if (DocLineId.HasValue)
                ViewBag.DocLineId = DocLineId;
            //Saving ViewBag Data::

            ViewBag.transactionType = transactionType;
            ViewBag.IndexStatus = IndexType;

            GatePassHeaderViewModel s = _GatePassHeaderService.GetGatePassHeader(id);
            //Getting Settings
            

           

            ////Perks
           

            PrepareViewBag(s.DocTypeId);
            if (s == null)
            {
                return HttpNotFound();
            }

            if (String.IsNullOrEmpty(transactionType) || transactionType == "detail")
                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = s.DocTypeId,
                    DocId = s.GatePassHeaderId,
                    ActivityType = (int)ActivityTypeContants.Detail,
                    DocNo = s.DocNo,
                    DocDate = s.DocDate,
                    DocStatus = s.Status,
                }));


            return View("Create", s);
        }


        // GET: /PurchaseOrderHeader/Delete/5

        //public ActionResult Delete(int id, string PrevAction, string PrevController)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    JobOrderHeader JobOrderHeader = _JobOrderHeaderService.Find(id);
        //    if (JobOrderHeader == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ReasonViewModel rvm = new ReasonViewModel()
        //    {
        //        id = id,
        //    };
        //    return PartialView("_Reason", rvm);
        //}



        // POST: /PurchaseOrderHeader/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(ReasonViewModel vm)
        {
            List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();
            bool BeforeSave = true;

            try
            {
                BeforeSave = JobOrderDocEvents.beforeHeaderDeleteEvent(this, new JobEventArgs(vm.id), ref context);
            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                EventException = true;
            }

            if (!BeforeSave)
                TempData["CSEXC"] += "Failed validation before delete";

            var GatePassHeader = (from p in context.GatePassHeader
                                  where p.GatePassHeaderId == vm.id
                                  select p).FirstOrDefault();

           

        

            if (ModelState.IsValid && BeforeSave && !EventException)
            {
                
                LogList.Add(new LogTypeViewModel
                {
                    ExObj = Mapper.Map<GatePassHeader>(GatePassHeader),
                });
                
                var GatePassLine = (from p in context.GatePassLine
                                    where p.GatePassHeaderId == vm.id
                                    select p).ToList();

                var GeLineIds = GatePassLine.Select(m => m.GatePassLineId).ToArray();

                try
                {
                    JobOrderDocEvents.onHeaderDeleteEvent(this, new JobEventArgs(vm.id), ref context);
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                    EventException = true;
                }

                //Mark ObjectState.Delete to all the Purchase Order Lines. 
                foreach (var item in GatePassLine)
                {

                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = Mapper.Map<GatePassLine>(item),
                    });


                  


                    //var linecharges = new JobOrderLineChargeService(_unitOfWork).GetCalculationProductList(item.JobOrderLineId);
                    //foreach (var citem in linecharges)
                    //    new JobOrderLineChargeService(_unitOfWork).Delete(citem.Id);

                    item.ObjectState = Model.ObjectState.Deleted;
                    context.GatePassLine.Remove(item);


                }
               

                // Now delete the Purhcase Order Header
                //_JobOrderHeaderService.Delete(JobOrderHeader);

                int ReferenceDocId = GatePassHeader.GatePassHeaderId;
                int ReferenceDocTypeId = GatePassHeader.DocTypeId;

                GatePassHeader.ObjectState = Model.ObjectState.Deleted;
                context.GatePassHeader.Remove(GatePassHeader);


                //ForDeleting Generated CostCenter:::

                XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);


                //Commit the DB
                try
                {
                    if (EventException)
                        throw new Exception();

                    context.SaveChanges();
                }

                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                    return PartialView("_Reason", vm);
                }

                try
                {
                    JobOrderDocEvents.afterHeaderDeleteEvent(this, new JobEventArgs(vm.id), ref context);
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                }

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = GatePassHeader.DocTypeId,
                    DocId = GatePassHeader.GatePassHeaderId,
                    ActivityType = (int)ActivityTypeContants.Deleted,
                    UserRemark = vm.Reason,
                    DocNo = GatePassHeader.DocNo,
                    xEModifications = Modifications,
                    DocDate = GatePassHeader.DocDate,
                    DocStatus = GatePassHeader.Status,
                }));

                return Json(new { success = true });
            }
            return PartialView("_Reason", vm);
        }


        public ActionResult Submit(int id, string IndexType, string TransactionType)
        {

            #region DocTypeTimeLineValidation
            GatePassHeader s = context.GatePassHeader.Find(id);

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
        public ActionResult Submitted(int Id, string IndexType, string UserRemark, string IsContinue, string GenGatePass)
        {

            bool BeforeSave = true;
            try
            {
                BeforeSave = JobOrderDocEvents.beforeHeaderSubmitEvent(this, new JobEventArgs(Id), ref context);
            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                EventException = true;
            }

            if (!BeforeSave)
                TempData["CSEXC"] += "Falied validation before submit.";

            GatePassHeader pd = context.GatePassHeader.Find(Id);


            if (ModelState.IsValid && BeforeSave && !EventException)
            {
                int Cnt = 0;
                int CountUid = 0;
                //JobOrderHeader pd = new JobOrderHeaderService(_unitOfWork).Find(Id);              

                int ActivityType;
                if (User.Identity.Name == pd.ModifiedBy || UserRoles.Contains("Admin"))
                {

                    pd.Status = (int)StatusConstants.Submitted;
                    ActivityType = (int)ActivityTypeContants.Submitted;                  
                    pd.ReviewBy = null;
                    pd.ObjectState = Model.ObjectState.Modified;
                    context.GatePassHeader.Add(pd);
                    try
                    {
                        JobOrderDocEvents.onHeaderSubmitEvent(this, new JobEventArgs(Id), ref context);
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

                        context.SaveChanges();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                        return RedirectToAction("Index", new { id = pd.DocTypeId });
                    }



                  

                    try
                    {
                        JobOrderDocEvents.afterHeaderSubmitEvent(this, new JobEventArgs(Id), ref context);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                    }


                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = pd.DocTypeId,
                        DocId = pd.GatePassHeaderId,
                        ActivityType = ActivityType,
                        UserRemark = UserRemark,
                        DocNo = pd.DocNo,
                        DocDate = pd.DocDate,
                        DocStatus = pd.Status,
                    }));



                    string ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"] + "/" + "GatePassHeader" + "/" + "Index" + "/" + pd.DocTypeId + "?IndexType=" + IndexType;
                    if (!string.IsNullOrEmpty(IsContinue) && IsContinue == "True")
                    {

                        int nextId = new NextPrevIdService(_unitOfWork).GetNextPrevId(Id, pd.DocTypeId, User.Identity.Name, ForActionConstants.PendingToSubmit, "Web.Gatepassheaders", "GatePassHeaderId", PrevNextConstants.Next);

                        if (nextId == 0)
                        {
                            var PendingtoSubmitCount = _GatePassHeaderService.GetGatePassHeaderListPendingToSubmit(pd.DocTypeId, User.Identity.Name).Count();
                            if (PendingtoSubmitCount > 0)
                                ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"] + "/" + "GatePassHeader" + "/" + "Index_PendingToSubmit" + "/" + pd.DocTypeId + "?IndexType=" + IndexType;
                            else
                                ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"] + "/" + "GatePassHeader" + "/" + "Index" + "/" + pd.DocTypeId + "?IndexType=" + IndexType;
                        }
                        else
                            ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"] + "/" + "GatePassHeader" + "/" + "Submit" + "/" + nextId + "?TransactionType=submitContinue&IndexType=" + IndexType;
                    }

                    #region "For Calling Customise Menu"
                    //int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
                    //int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

                    //var settings = new JobOrderSettingsService(_unitOfWork).GetJobOrderSettingsForDocument(pd.DocTypeId, DivisionId, SiteId);

                    //if (settings != null)
                    //{
                    //    if (settings.OnSubmitMenuId != null)
                    //    {
                    //        return Action_Menu(Id, (int)settings.OnSubmitMenuId, ReturnUrl);
                    //    }
                    //    else
                    //        return Redirect(ReturnUrl);
                    //}
                   #endregion

                }
                else
                    return RedirectToAction("Index", new { id = pd.DocTypeId, IndexType = IndexType }).Warning("Record can be submitted by user " + pd.ModifiedBy + " only.");
            }

            return RedirectToAction("Index", new { id = pd.DocTypeId, IndexType = IndexType });
        }





        public ActionResult Review(int id, string IndexType, string TransactionType)
        {
            return RedirectToAction("Detail", new { id = id, IndexType = IndexType, transactionType = string.IsNullOrEmpty(TransactionType) ? "review" : TransactionType });
        }


        [HttpPost, ActionName("Detail")]
        [MultipleButton(Name = "Command", Argument = "Review")]
        public ActionResult Reviewed(int Id, string IndexType, string UserRemark, string IsContinue)
        {
            GatePassHeader pd = context.GatePassHeader.Find(Id);

            if (ModelState.IsValid)
            {
                pd.ReviewCount = (pd.ReviewCount ?? 0) + 1;
                pd.ReviewBy += User.Identity.Name + ", ";
                pd.ObjectState = Model.ObjectState.Modified;
                context.GatePassHeader.Add(pd);

                try
                {
                    JobOrderDocEvents.onHeaderReviewEvent(this, new JobEventArgs(Id), ref context);
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                }

                context.SaveChanges();

                try
                {
                    JobOrderDocEvents.afterHeaderReviewEvent(this, new JobEventArgs(Id), ref context);
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                }

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = pd.DocTypeId,
                    DocId = pd.GatePassHeaderId,
                    ActivityType = (int)ActivityTypeContants.Reviewed,
                    UserRemark = UserRemark,
                    DocNo = pd.DocNo,
                    DocDate = pd.DocDate,
                    DocStatus = pd.Status,
                }));



                //if (!string.IsNullOrEmpty(IsContinue) && IsContinue == "True")
                //{
                //    JobOrderHeader HEader = _JobOrderHeaderService.Find(Id);

                //    int nextId = new NextPrevIdService(_unitOfWork).GetNextPrevId(Id, HEader.DocTypeId, User.Identity.Name, ForActionConstants.PendingToReview, "Web.JobOrderHeaders", "JobOrderHeaderId", PrevNextConstants.Next);
                //    if (nextId == 0)
                //    {
                //        var PendingtoSubmitCount = _JobOrderHeaderService.GetJobOrderHeaderListPendingToReview(HEader.DocTypeId, User.Identity.Name).Count();
                //        if (PendingtoSubmitCount > 0)
                //            return RedirectToAction("Index_PendingToReview", new { id = HEader.DocTypeId });
                //        else
                //            return RedirectToAction("Index", new { id = HEader.DocTypeId });

                //    }

                //    ViewBag.PendingToReview = PendingToReviewCount(Id);
                //    return RedirectToAction("Detail", new { id = nextId, transactionType = "ReviewContinue" });
                //}
                //else
                //    return RedirectToAction("Index", new { id = pd.DocTypeId }).Success(pd.DocNo + " Reviewed Successfully.");

                string ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"] + "/" + "GatePassHeader" + "/" + "Index" + "/" + pd.DocTypeId + "?IndexType=" + IndexType;
                if (!string.IsNullOrEmpty(IsContinue) && IsContinue == "True")
                {

                    int nextId = new NextPrevIdService(_unitOfWork).GetNextPrevId(Id, pd.DocTypeId, User.Identity.Name, ForActionConstants.PendingToReview, "Web.Gatepassheaders", "GatePassHeaderId", PrevNextConstants.Next);

                    if (nextId == 0)
                    {
                        var PendingtoSubmitCount = _GatePassHeaderService.GetGatePassHeaderListPendingToSubmit(pd.DocTypeId, User.Identity.Name).Count();
                        if (PendingtoSubmitCount > 0)
                            ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"] + "/" + "GatePassHeader" + "/" + "Index_PendingToReview" + "/" + pd.DocTypeId + "?IndexType=" + IndexType;
                        else
                            ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"] + "/" + "GatePassHeader" + "/" + "Index" + "/" + pd.DocTypeId + "?IndexType=" + IndexType;
                    }
                    else
                        ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"] + "/" + "GatePassHeader" + "/" + "Review" + "/" + nextId + "?TransactionType=ReviewContinue&IndexType=" + IndexType;
                }


                return Redirect(ReturnUrl);

            }

            return RedirectToAction("Index", new { id = pd.DocTypeId, IndexType = IndexType }).Warning("Error in Reviewing.");
        }

        [HttpGet]
        public ActionResult Report(int id)
        {
            DocumentType Dt = new DocumentType();
            Dt = new DocumentTypeService(_unitOfWork).Find(id);

            JobOrderSettings SEttings = new JobOrderSettingsService(_unitOfWork).GetJobOrderSettingsForDocument(Dt.DocumentTypeId, (int)System.Web.HttpContext.Current.Session["DivisionId"], (int)System.Web.HttpContext.Current.Session["SiteId"]);

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
                //ReportLine Process = new ReportLineService(_unitOfWork).GetReportLineByName("Process", header.ReportHeaderId);
                //if (Process != null)
                //    DefaultValue.Add(Process.ReportLineId, ((int)SEttings.ProcessId).ToString());
            }

            TempData["ReportLayoutDefaultValues"] = DefaultValue;

            return Redirect((string)System.Configuration.ConfigurationManager.AppSettings["CustomizeDomain"] + "/Report_ReportPrint/ReportPrint/?MenuId=" + Dt.ReportMenuId);

        }


       


       

        public ActionResult Action_Menu(int Id, int MenuId, string ReturnUrl)
        {
            MenuViewModel menuviewmodel = new MenuService(_unitOfWork).GetMenu(MenuId);

            if (menuviewmodel != null)
            {
                if (!string.IsNullOrEmpty(menuviewmodel.URL))
                {
                    return Redirect(System.Configuration.ConfigurationManager.AppSettings[menuviewmodel.URL] + "/" + menuviewmodel.ControllerName + "/" + menuviewmodel.ActionName + "?Id=" + Id + "&ReturnUrl=" + ReturnUrl);
                }
                else
                {
                    return RedirectToAction(menuviewmodel.ActionName, menuviewmodel.ControllerName, new { id = Id, ReturnUrl = ReturnUrl });
                }
            }
            return null;
        }

        public int PendingToSubmitCount(int id)
        {
            return (_GatePassHeaderService.GetGatePassHeaderListPendingToSubmit(id, User.Identity.Name)).Count();
        }

        public int PendingToReviewCount(int id)
        {
            return (_GatePassHeaderService.GetGatePassHeaderListPendingToReview(id, User.Identity.Name)).Count();
        }

        [HttpGet]
        public ActionResult NextPage(int DocId, int DocTypeId)//CurrentHeaderId
        {
            var nextId = new NextPrevIdService(_unitOfWork).GetNextPrevId(DocId, DocTypeId, User.Identity.Name, "", "Web.Gatepassheaders", "GatePassHeaderId", PrevNextConstants.Next);
            return Edit(nextId, "");
        }
        [HttpGet]
        public ActionResult PrevPage(int DocId, int DocTypeId)//CurrentHeaderId
        {
            var PrevId = new NextPrevIdService(_unitOfWork).GetNextPrevId(DocId, DocTypeId, User.Identity.Name, "", "Web.Gatepassheaders", "GatePassHeaderId", PrevNextConstants.Prev);
            return Edit(PrevId, "");
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

   

        //public ActionResult GenerateGatePass(string Ids, int DocTypeId)
        //{

        //    if (!string.IsNullOrEmpty(Ids))
        //    {
        //        int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
        //        int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
        //        int PK = 0;

        //        var Settings = new JobOrderSettingsService(_unitOfWork).GetJobOrderSettingsForDocument(DocTypeId, DivisionId, SiteId);
        //        var GatePassDocTypeID = new DocumentTypeService(_unitOfWork).FindByName(TransactionDocCategoryConstants.GatePass).DocumentTypeId;
        //        string JobHeaderIds = "";

        //        try
        //        {

        //            foreach (var item in Ids.Split(',').Select(Int32.Parse))
        //            {
        //                TimePlanValidation = true;

        //                var pd = context.JobOrderHeader.Find(item);

        //                if (!pd.GatePassHeaderId.HasValue)
        //                {
        //                    #region DocTypeTimeLineValidation
        //                    try
        //                    {

        //                        TimePlanValidation = DocumentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(pd), DocumentTimePlanTypeConstants.GatePassCreate, User.Identity.Name, out ExceptionMsg, out Continue);

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        string message = _exception.HandleException(ex);
        //                        TempData["CSEXC"] += message;
        //                        TimePlanValidation = false;
        //                    }
        //                    #endregion

        //                    if ((TimePlanValidation || Continue))
        //                    {
        //                        if (Settings.isPostedInStock.HasValue && Settings.isPostedInStock == true)
        //                        {

        //                            if (!String.IsNullOrEmpty(Settings.SqlProcGatePass) && pd.Status == (int)StatusConstants.Submitted && !pd.GatePassHeaderId.HasValue)
        //                            {

        //                                SqlParameter SqlParameterUserId = new SqlParameter("@Id", item);
        //                                IEnumerable<GatePassGeneratedViewModel> GatePasses = context.Database.SqlQuery<GatePassGeneratedViewModel>(Settings.SqlProcGatePass + " @Id", SqlParameterUserId).ToList();

        //                                if (pd.GatePassHeaderId == null)
        //                                {
        //                                    SqlParameter DocDate = new SqlParameter("@DocDate", DateTime.Now.Date);
        //                                    DocDate.SqlDbType = SqlDbType.DateTime;
        //                                    SqlParameter Godown = new SqlParameter("@GodownId", pd.GodownId);
        //                                    SqlParameter DocType = new SqlParameter("@DocTypeId", GatePassDocTypeID);
        //                                    GatePassHeader GPHeader = new GatePassHeader();
        //                                    GPHeader.CreatedBy = User.Identity.Name;
        //                                    GPHeader.CreatedDate = DateTime.Now;
        //                                    GPHeader.DivisionId = pd.DivisionId;
        //                                    GPHeader.DocDate = DateTime.Now.Date;
        //                                    GPHeader.DocNo = context.Database.SqlQuery<string>("Web.GetNewDocNoGatePass @DocTypeId, @DocDate, @GodownId ", DocType, DocDate, Godown).FirstOrDefault();
        //                                    GPHeader.DocTypeId = GatePassDocTypeID;
        //                                    GPHeader.ModifiedBy = User.Identity.Name;
        //                                    GPHeader.ModifiedDate = DateTime.Now;
        //                                    GPHeader.Remark = pd.Remark;
        //                                    GPHeader.PersonId = pd.JobWorkerId;
        //                                    GPHeader.SiteId = pd.SiteId;
        //                                    GPHeader.GodownId = pd.GodownId ?? 0;
        //                                    GPHeader.GatePassHeaderId = PK++;
        //                                    GPHeader.ReferenceDocTypeId = pd.DocTypeId;
        //                                    GPHeader.ReferenceDocId = pd.JobOrderHeaderId;
        //                                    GPHeader.ReferenceDocNo = pd.DocNo;
        //                                    GPHeader.ObjectState = Model.ObjectState.Added;
        //                                    context.GatePassHeader.Add(GPHeader);

        //                                    //new GatePassHeaderService(_unitOfWork).Create(GPHeader);


        //                                    foreach (GatePassGeneratedViewModel GatepassLine in GatePasses)
        //                                    {
        //                                        GatePassLine Gline = new GatePassLine();
        //                                        Gline.CreatedBy = User.Identity.Name;
        //                                        Gline.CreatedDate = DateTime.Now;
        //                                        Gline.GatePassHeaderId = GPHeader.GatePassHeaderId;
        //                                        Gline.ModifiedBy = User.Identity.Name;
        //                                        Gline.ModifiedDate = DateTime.Now;
        //                                        Gline.Product = GatepassLine.ProductName;
        //                                        Gline.Qty = GatepassLine.Qty;
        //                                        Gline.Specification = GatepassLine.Specification;
        //                                        Gline.UnitId = GatepassLine.UnitId;

        //                                        // new GatePassLineService(_unitOfWork).Create(Gline);
        //                                        Gline.ObjectState = Model.ObjectState.Added;
        //                                        context.GatePassLine.Add(Gline);
        //                                    }

        //                                    pd.GatePassHeaderId = GPHeader.GatePassHeaderId;

        //                                    pd.ObjectState = Model.ObjectState.Modified;
        //                                    context.JobOrderHeader.Add(pd);

        //                                    JobHeaderIds += pd.StockHeaderId + ", ";
        //                                }

        //                                context.SaveChanges();
        //                            }

        //                        }
        //                    }
        //                    else
        //                        TempData["CSEXC"] += ExceptionMsg;
        //                }


        //            }


        //            //_unitOfWork.Save();
        //        }

        //        catch (Exception ex)
        //        {
        //            string message = _exception.HandleException(ex);
        //            return Json(new { success = "Error", data = message }, JsonRequestBehavior.AllowGet);
        //        }

        //        LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
        //        {
        //            DocTypeId = GatePassDocTypeID,
        //            ActivityType = (int)ActivityTypeContants.Added,
        //            Narration = "GatePass created for Job Orders " + JobHeaderIds,
        //        }));

        //        if (string.IsNullOrEmpty((string)TempData["CSEXC"]))
        //            return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet).Success("Gate passes generated successfully");
        //        else
        //            return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet);

        //    }
        //    return Json(new { success = "Error", data = "No Records Selected." }, JsonRequestBehavior.AllowGet);

        //}




        //public ActionResult DeleteGatePass(int Id)
        //{
        //    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();
        //    if (Id > 0)
        //    {
        //        int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
        //        int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];

        //        try
        //        {

        //            var pd = context.JobOrderHeader.Find(Id);

        //            #region DocTypeTimeLineValidation
        //            try
        //            {

        //                TimePlanValidation = DocumentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(pd), DocumentTimePlanTypeConstants.GatePassCancel, User.Identity.Name, out ExceptionMsg, out Continue);

        //            }
        //            catch (Exception ex)
        //            {
        //                string message = _exception.HandleException(ex);
        //                TempData["CSEXC"] += message;
        //                TimePlanValidation = false;
        //            }

        //            if (!TimePlanValidation && !Continue)
        //                throw new Exception(ExceptionMsg);
        //            #endregion

        //            var GatePass = context.GatePassHeader.Find(pd.GatePassHeaderId);

        //            //LogList.Add(new LogTypeViewModel
        //            //{
        //            //    ExObj = GatePass,
        //            //});

        //            //var GatePassLines = (from p in context.GatePassLine
        //            //                     where p.GatePassHeaderId == GatePass.GatePassHeaderId
        //            //                     select p).ToList();

        //            //foreach (var item in GatePassLines)
        //            //{
        //            //    LogList.Add(new LogTypeViewModel
        //            //    {
        //            //        ExObj = item,
        //            //    });
        //            //    item.ObjectState = Model.ObjectState.Deleted;
        //            //    context.GatePassLine.Remove(item);
        //            //}
        //            if (GatePass.Status != (int)StatusConstants.Submitted)
        //            {
        //                pd.GatePassHeaderId = null;
        //                pd.Status = (int)StatusConstants.Modified;
        //                pd.ModifiedBy = User.Identity.Name;
        //                pd.ModifiedDate = DateTime.Now;
        //                pd.IsGatePassPrinted = false;
        //                pd.ObjectState = Model.ObjectState.Modified;
        //                context.JobOrderHeader.Add(pd);

        //                GatePass.Status = (int)StatusConstants.Cancel;
        //                GatePass.ObjectState = Model.ObjectState.Modified;
        //                context.GatePassHeader.Add(GatePass);

        //                XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);

        //                context.SaveChanges();

        //                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
        //                {
        //                    DocTypeId = GatePass.DocTypeId,
        //                    DocId = GatePass.GatePassHeaderId,
        //                    ActivityType = (int)ActivityTypeContants.Deleted,
        //                    DocNo = GatePass.DocNo,
        //                    DocDate = GatePass.DocDate,
        //                    xEModifications = Modifications,
        //                    DocStatus = GatePass.Status,
        //                }));

        //            }
        //            else
        //                throw new Exception("Gatepass cannot be deleted because it is already submitted");

        //        }

        //        catch (Exception ex)
        //        {
        //            string message = _exception.HandleException(ex);
        //            return Json(new { success = "Error", data = message }, JsonRequestBehavior.AllowGet);
        //        }

        //        return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet).Success("Gate pass Deleted successfully");

        //    }
        //    return Json(new { success = "Error", data = "No Records Selected." }, JsonRequestBehavior.AllowGet);

        //}





        public ActionResult GeneratePrints(string Ids, int DocTypeId)
        {

            if (!string.IsNullOrEmpty(Ids))
            {
                int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
                int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];

                var Settings = new JobOrderSettingsService(_unitOfWork).GetJobOrderSettingsForDocument(DocTypeId, DivisionId, SiteId);

                string ReportSql = "";

                if (!string.IsNullOrEmpty(Settings.DocumentPrint))
                    ReportSql = context.ReportHeader.Where((m) => m.ReportName == Settings.DocumentPrint).FirstOrDefault().ReportSQL;

                try
                {

                    List<byte[]> PdfStream = new List<byte[]>();
                    foreach (var item in Ids.Split(',').Select(Int32.Parse))
                    {
                        int Copies = 1;
                        int AdditionalCopies = Settings.NoOfPrintCopies ?? 0;
                        bool UpdateGatePassPrint = true;

                        DirectReportPrint drp = new DirectReportPrint();

                        var pd = context.JobOrderHeader.Find(item);

                        LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                        {
                            DocTypeId = pd.DocTypeId,
                            DocId = pd.JobOrderHeaderId,
                            ActivityType = (int)ActivityTypeContants.Print,
                            DocNo = pd.DocNo,
                            DocDate = pd.DocDate,
                            DocStatus = pd.Status,
                        }));

                        do
                        {
                            byte[] Pdf;

                            if (!string.IsNullOrEmpty(ReportSql))
                            {
                                Pdf = drp.rsDirectDocumentPrint(ReportSql, User.Identity.Name, item);
                                PdfStream.Add(Pdf);
                            }
                            else
                            {
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

                            if (UpdateGatePassPrint && !(pd.IsGatePassPrinted ?? false))
                            {
                                if (pd.GatePassHeaderId.HasValue)
                                {
                                    pd.IsGatePassPrinted = true;
                                    pd.ObjectState = Model.ObjectState.Modified;
                                    context.JobOrderHeader.Add(pd);
                                    context.SaveChanges();

                                    UpdateGatePassPrint = false;
                                    Copies = AdditionalCopies;
                                    if (Copies > 0)
                                        continue;
                                }
                            }

                            Copies--;

                        } while (Copies > 0);

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

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        //public JsonResult ValidateCostCenter(int DocTypeId, int HeaderId, int JobWorkerId, string CostCenterName)
        //{
        //    return Json(_JobOrderHeaderService.ValidateCostCenter(DocTypeId, HeaderId, JobWorkerId, CostCenterName), JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult GetLineProgress(int LineId)
        //{

        //    var ProgressDetail = _JobOrderHeaderService.GetLineProgressDetail(LineId);

        //    return PartialView("_LineProgress", ProgressDetail);
        //}

    }
}
