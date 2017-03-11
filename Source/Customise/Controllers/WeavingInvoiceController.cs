using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model.Models;
using Data.Models;
using Service;
using Data.Infrastructure;
using Core.Common;
using Model.ViewModel;
using AutoMapper;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Configuration;

namespace Web
{
    [Authorize]
    public class WeavingInvoiceController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        List<string> UserRoles = new List<string>();
        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        IJobInvoiceHeaderService _JobInvoiceHeaderService;
        IJobInvoiceLineService _JobInvoiceLineService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;
        public WeavingInvoiceController(IJobInvoiceLineService SaleOrder, IJobInvoiceHeaderService JobInvoiceHeaderService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _JobInvoiceLineService = SaleOrder;
            _JobInvoiceHeaderService = JobInvoiceHeaderService;
            _unitOfWork = unitOfWork;
            _exception = exec;

            UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;
        }

        public void PrepareViewBag(int id)
        {
            var Header = _JobInvoiceHeaderService.Find(id);
            ViewBag.Name = new DocumentTypeService(_unitOfWork).Find(Header.DocTypeId).DocumentTypeName;
            ViewBag.DocNo = Header.DocNo;

            if (Header.Status == (int)StatusConstants.Drafted || Header.Status == (int)StatusConstants.Import)
            {
                ViewBag.Url = System.Configuration.ConfigurationManager.AppSettings["JobsDomain"] + "/JobInvoiceHeader/Modify/" + id;
            }
            else if (Header.Status == (int)StatusConstants.Submitted || Header.Status == (int)StatusConstants.ModificationSubmitted || Header.Status == (int)StatusConstants.Modified)
            {
                ViewBag.Url = System.Configuration.ConfigurationManager.AppSettings["JobsDomain"] + "/JobInvoiceHeader/ModifyAfter_Submit/" + id;
            }
            else if (Header.Status == (int)StatusConstants.Approved || Header.Status == (int)StatusConstants.Closed)
            {
                ViewBag.Url = System.Configuration.ConfigurationManager.AppSettings["JobsDomain"] + "/JobInvoiceHeader/ModifyAfter_Approve/" + id;
            }


        }


        [HttpGet]
        public ActionResult GetSummary(int id)
        {
            var Header = _JobInvoiceHeaderService.Find(id);

            SqlParameter SqlParameterJobInvoiceHeaderId = new SqlParameter("@JobInvoiceHeaderId", id);

            List<JobInvoiceSummaryViewModel> JobInvoices = db.Database.SqlQuery<JobInvoiceSummaryViewModel>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".sp_GetPersonDueAndAdvance @JobInvoiceHeaderId", SqlParameterJobInvoiceHeaderId).ToList();


            JobInvoiceSummaryDetailViewModel vm = new JobInvoiceSummaryDetailViewModel();
            vm.JobInvoiceHeaderId = id;
            vm.DocData = Header.DocDate;
            vm.JobInvoiceSummaryViewModel = JobInvoices;
            PrepareViewBag(id);

            if (JobInvoices.Count() == 0)
                return Redirect(System.Configuration.ConfigurationManager.AppSettings["JobsDomain"] + "/JobInvoiceHeader/Index/" + Header.DocTypeId);
            else
                return View("Summary", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostSummary(JobInvoiceSummaryDetailViewModel vm)
        {

            //TempData["CSEXC"] = "Customize Test Exception";

            List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();
            bool Modified = false;
            int Id = vm.JobInvoiceHeaderId;

            var Header = _JobInvoiceHeaderService.Find(Id);

            var JobInvoices = (from p in db.JobInvoiceLine
                               join t in db.JobOrderLine on p.JobReceiveLine.JobOrderLineId equals t.JobOrderLineId
                               where p.JobInvoiceHeaderId == Id
                               group t by new { t.ProductId, t.JobOrderHeaderId } into g
                               select g.Key).ToList();

            foreach (var item in vm.JobInvoiceSummaryViewModel)
            {
                LedgerHeader LedgerHeader = new LedgerHeader();
                LedgerHeader.DocTypeId = new DocumentTypeService(_unitOfWork).Find(TransactionDoctypeConstants.AdvanceAmtAdjustment).DocumentTypeId;
                LedgerHeader.DocDate = DateTime.Now.Date;
                LedgerHeader.DivisionId = Header.DivisionId;
                LedgerHeader.SiteId = Header.SiteId;
                LedgerHeader.DocNo = new DocumentTypeService(_unitOfWork).FGetNewDocNo("DocNo", ConfigurationManager.AppSettings["DataBaseSchema"] + ".LedgerHeaders", LedgerHeader.DocTypeId, LedgerHeader.DocDate, LedgerHeader.DivisionId, LedgerHeader.SiteId);
                LedgerHeader.LedgerAccountId = new LedgerAccountService(_unitOfWork).GetLedgerAccountByPersondId(item.PersonId).LedgerAccountId;
                LedgerHeader.Narration = "Advance Adjusted";
                LedgerHeader.Status = (int)StatusConstants.Submitted;
                LedgerHeader.ProcessId = new ProcessService(_unitOfWork).Find(ProcessConstants.Weaving).ProcessId;
                LedgerHeader.CostCenterId = 0;
                LedgerHeader.AdjustmentType = "Payment";
                LedgerHeader.PaymentFor = DateTime.Now.Date;
                LedgerHeader.CreatedDate = DateTime.Now;
                LedgerHeader.ModifiedDate = DateTime.Now;
                LedgerHeader.CreatedBy = User.Identity.Name;
                LedgerHeader.ModifiedBy = User.Identity.Name;
                LedgerHeader.ObjectState = Model.ObjectState.Added;
                new LedgerHeaderService(_unitOfWork).Create(LedgerHeader);


                LedgerLine LedgerLine = new LedgerLine();
                LedgerLine.LedgerHeaderId = LedgerHeader.LedgerHeaderId;
                LedgerLine.LedgerAccountId = (int)LedgerHeader.LedgerAccountId;
                LedgerLine.CostCenterId = null;
                LedgerLine.Amount = 0;
                LedgerLine.CreatedDate = DateTime.Now;
                LedgerLine.ModifiedDate = DateTime.Now;
                LedgerLine.CreatedBy = User.Identity.Name;
                LedgerLine.ModifiedBy = User.Identity.Name;
                LedgerLine.ObjectState = Model.ObjectState.Added;
                new LedgerLineService(_unitOfWork).Create(LedgerLine);


                Ledger LedgerDr = new Ledger();
                LedgerDr.LedgerHeaderId = LedgerHeader.LedgerHeaderId;
                LedgerDr.LedgerAccountId = LedgerLine.LedgerAccountId;
                LedgerDr.ContraLedgerAccountId = LedgerLine.LedgerAccountId;
                LedgerDr.CostCenterId = LedgerHeader.CostCenterId;
                LedgerDr.AmtDr = LedgerLine.Amount;
                LedgerDr.AmtCr = 0;
                LedgerDr.Narration = "Advance Adjusted.";
                LedgerDr.LedgerLineId = LedgerLine.LedgerLineId;
                LedgerDr.ObjectState = Model.ObjectState.Added;
                new LedgerService(_unitOfWork).Create(LedgerDr);


                Ledger LedgerCr = new Ledger();
                LedgerCr.LedgerHeaderId = LedgerHeader.LedgerHeaderId;
                LedgerCr.LedgerAccountId = LedgerLine.LedgerAccountId;
                LedgerCr.ContraLedgerAccountId = LedgerLine.LedgerAccountId;
                LedgerCr.CostCenterId = null;
                LedgerCr.AmtDr = 0;
                LedgerCr.AmtCr = LedgerLine.Amount;
                LedgerCr.Narration = null;
                LedgerCr.LedgerLineId = LedgerLine.LedgerLineId;
                LedgerCr.ObjectState = Model.ObjectState.Added;
                new LedgerService(_unitOfWork).Create(LedgerCr);
            }



            try
            {
                _unitOfWork.Save();
            }

            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                ModelState.AddModelError("", message);
                PrepareViewBag(vm.JobInvoiceHeaderId);
                return Json(new { Success = false });
            }

            LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
            {
                DocTypeId = Header.DocTypeId,
                DocId = Header.JobInvoiceHeaderId,
                ActivityType = (int)ActivityTypeContants.Modified,
                DocNo = Header.DocNo,
                //xEModifications = Modifications,
                DocDate = Header.DocDate,
                DocStatus = Header.Status,
            }));

            string RetUrl = "";


            if (Header.Status == (int)StatusConstants.Drafted || Header.Status == (int)StatusConstants.Import)
            {
                RetUrl = System.Configuration.ConfigurationManager.AppSettings["JobsDomain"] + "/JobInvoiceHeader/Modify/" + Header.JobInvoiceHeaderId;
            }
            else if (Header.Status == (int)StatusConstants.Submitted || Header.Status == (int)StatusConstants.ModificationSubmitted || Header.Status == (int)StatusConstants.Modified)
            {
                RetUrl = System.Configuration.ConfigurationManager.AppSettings["JobsDomain"] + "/JobInvoiceHeader/ModifyAfter_Submit/" + Header.JobInvoiceHeaderId;
            }
            else if (Header.Status == (int)StatusConstants.Approved || Header.Status == (int)StatusConstants.Closed)
            {
                RetUrl = System.Configuration.ConfigurationManager.AppSettings["JobsDomain"] + "/JobInvoiceHeader/ModifyAfter_Approve/" + Header.JobInvoiceHeaderId;
            }
            else
                RetUrl = System.Configuration.ConfigurationManager.AppSettings["JobsDomain"] + "/JobInvoiceHeader/Index/" + Header.DocTypeId;

            return Json(new { Success = true, Url = RetUrl });
        }







        protected override void Dispose(bool disposing)
        {
            if (!string.IsNullOrEmpty((string)TempData["CSEXC"]))
                CookieGenerator.CreateNotificationCookie(NotificationTypeConstants.Danger, (string)TempData["CSEXC"]);

            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class JobInvoiceSummaryViewModel
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public Decimal InvoiceAmount { get; set; }
        public Decimal AdvanceAmount { get; set; }
        public Decimal ToAdjust { get; set; }
        public Decimal BalanceForAdjustment { get; set; }
    }

    public class JobInvoiceSummaryDetailViewModel
    {
        public List<JobInvoiceSummaryViewModel> JobInvoiceSummaryViewModel { get; set; }
        public int JobInvoiceHeaderId { get; set; }
        public int JobWorkerId { get; set; }
        public DateTime DocData { get; set; }
    }
}
