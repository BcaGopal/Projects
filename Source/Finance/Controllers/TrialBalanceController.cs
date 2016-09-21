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
using Service;
using Data.Infrastructure;
using Presentation;
using Presentation.ViewModels;
using Core.Common;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace Web
{
    // [Authorize]
    public class TrialBalanceController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        ITrialBalanceService _TrialBalService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;
        public TrialBalanceController(ITrialBalanceService TrialBalanceService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _TrialBalService = TrialBalanceService;
            _unitOfWork = unitOfWork;
            _exception = exec;
        }

        public ActionResult GetTrialBalance()
        {
            var settings = new TrialBalanceSettingService(_unitOfWork).GetTrailBalanceSetting(User.Identity.Name);

            List<string> UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];
            if (settings == null)
            {
                return RedirectToAction("Create", "TrialBalanceSetting").Warning("Please create Trial Balance settings");
            }           

            ViewBag.DrCr = settings.DrCr;

            if (settings.DisplayType == DisplayTypeConstants.Balance)
                return View("TrialBalanceIndex");
            else
                return View("TrialBalanceSummaryIndex");
        }

        public JsonResult GetTrlBal()
        {
            return Json(new { data = _TrialBalService.GetTrialBalance(User.Identity.Name) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTrlBalSummary()
        {
            return Json(new { data = _TrialBalService.GetTrialBalanceSummary(User.Identity.Name) }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetSubTrialBalance(int? id)//LedgerAccountGroupId
        {
            ViewBag.id = id;
            if (id.HasValue && id.Value > 0)
                ViewBag.Name = new LedgerAccountGroupService(_unitOfWork).Find(id.Value).LedgerAccountGroupName;
            var settings = new TrialBalanceSettingService(_unitOfWork).GetTrailBalanceSetting(User.Identity.Name);

            ViewBag.DisplayType = settings.DisplayType;
            ViewBag.DrCr = settings.DrCr;
            if (settings.DisplayType == DisplayTypeConstants.Balance)
                return View("SubTrialBalanceIndex");
            else
                return View("SubTrialBalanceSummaryIndex");
        }
        public JsonResult GetSubTrlBal(int? id)
        {
            return Json(new { data = _TrialBalService.GetSubTrialBalance(id, User.Identity.Name) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubTrlBalSummary(int? id)
        {
            return Json(new { data = _TrialBalService.GetSubTrialBalanceSummary(id, User.Identity.Name) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLedgerBalance(int id)//LedgerAccountId
        {
            ViewBag.id = id;
            ViewBag.Name = new LedgerAccountService(_unitOfWork).Find(id).LedgerAccountName;

            var settings = new TrialBalanceSettingService(_unitOfWork).GetTrailBalanceSetting(User.Identity.Name);
            ViewBag.DrCr = settings.DrCr;

            return View("LedgerBalanceIndex");
        }

        public JsonResult GetLedgerBal(int id)
        {
            return Json(new { data = _TrialBalService.GetLedgerBalance(id, User.Identity.Name) }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DocumentMenu(int DocTypeId, int DocId)
        {
            if (DocTypeId == 0 || DocId == 0)
            {
                return View("Error");
            }

            int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

            var DocumentType = new DocumentTypeService(_unitOfWork).Find(DocTypeId);


            if (DocumentType.ControllerActionId.HasValue && DocumentType.ControllerActionId.Value > 0)
            {
                ControllerAction CA = new ControllerActionService(_unitOfWork).Find(DocumentType.ControllerActionId.Value);

                if (CA == null)
                {
                    return View("~/Views/Shared/UnderImplementation.cshtml");
                }
                else if (!string.IsNullOrEmpty(DocumentType.DomainName))
                {
                    return Redirect(System.Configuration.ConfigurationManager.AppSettings[DocumentType.DomainName] + "/" + CA.ControllerName + "/" + CA.ActionName + "/" + DocId);
                }
                else
                {
                    return RedirectToAction(CA.ActionName, CA.ControllerName, new { id = DocId });
                }
            }

            ViewBag.RetUrl = System.Web.HttpContext.Current.Request.UrlReferrer;
            HandleErrorInfo Excp = new HandleErrorInfo(new Exception("Document Settings not Configured"), "TrialBalance", "DocumentMenu");
            return View("Error", Excp);


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
