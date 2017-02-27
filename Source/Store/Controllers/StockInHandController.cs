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
using Model.ViewModel;

namespace Web
{
    // [Authorize]
    public class StockInHandController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        IStockInHandService _StockInHandService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;
        public StockInHandController(IStockInHandService StockInHandService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _StockInHandService = StockInHandService;
            _unitOfWork = unitOfWork;
            _exception = exec;
        }


        public ActionResult ProductTypeIndex()
        {

            var pt = new ProductTypeService(_unitOfWork).GetRawAndOtherMaterialProductTypes().Where(m => m.IsActive != false).ToList();

            return View("ProductTypeIndex", pt);
        }


        public ActionResult GetStockInHand(int id)
        {
            var settings = new StockInHandSettingService(_unitOfWork).GetTrailBalanceSetting(User.Identity.Name);
            List<string> UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];
            if (settings == null)
            {
                return RedirectToAction("Create", "StockInHandSetting", new { ProductTypeId = id }).Warning("Please create Stock In Hand settings");
            }

            Dimension1Types Dimension1Type = new ProductTypeService(_unitOfWork).GetProductTypeDimension1Types(id);
            Dimension2Types Dimension2Type = new ProductTypeService(_unitOfWork).GetProductTypeDimension2Types(id);


            if (Dimension1Type == null)
            {
                ViewBag.Dimension1TypeName = "Dimension1";
            }
            else
            {
                ViewBag.Dimension1TypeName = Dimension1Type.Dimension1TypeName;
            }

            if (Dimension2Type == null)
            {
                ViewBag.Dimension2TypeName = "Dimension2";
            }
            else
            {
                ViewBag.Dimension2TypeName = Dimension2Type.Dimension2TypeName;
            }

            ViewBag.id = id;
            ViewBag.GroupOn = settings.GroupOn;
            //if(settings.DisplayType==DisplayTypeConstants.Balance)
            return View("StockInHandIndex");
            //else
            //    return View("StockInHandSummaryIndex");
        }

        public JsonResult GetStockInHandJson(int id)
        {
            return Json(new { data = _StockInHandService.GetStockInHand(id, User.Identity.Name) }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetStockLedger(int? ProductId, int? Dim1, int? Dim2, int? Process, string LotNo, int? Godown)//LedgerAccountId
        {
            var Name = "";
            var TempName = "";
            ViewBag.ProductId = ProductId;
            Name  = (from p in db.Product
                            where p.ProductId == ProductId
                            select p).AsNoTracking().FirstOrDefault().ProductName;

            if (Dim1!=null)
            {
                TempName = "";
                TempName = (from P in db.Dimension1
                           join PT in db.ProductTypes on P.ProductTypeId equals PT.ProductTypeId into tablePT
                           from tabPT in tablePT.DefaultIfEmpty()
                           join DT in db.Dimension1Types on tabPT.Dimension1TypeId equals DT.Dimension1TypeId into tableDT
                           from tabDT in tableDT.DefaultIfEmpty()
                           where P.Dimension1Id  == Dim1
                           select ", " + tabDT.Dimension1TypeName +" : "+P.Dimension1Name
                           ).AsNoTracking().FirstOrDefault();

                if( TempName!="")
                {
                    Name = Name + TempName;
                }


            }

            if (Dim2 != null)
            {
                TempName = "";
                TempName = (from P in db.Dimension2
                            join PT in db.ProductTypes on P.ProductTypeId equals PT.ProductTypeId into tablePT
                            from tabPT in tablePT.DefaultIfEmpty()
                            join DT in db.Dimension2Types on tabPT.Dimension1TypeId equals DT.Dimension2TypeId into tableDT
                            from tabDT in tableDT.DefaultIfEmpty()
                            where P.Dimension2Id == Dim2
                            select ", " + tabDT.Dimension2TypeName + " : " + P.Dimension2Name
                           ).AsNoTracking().FirstOrDefault();

                if (TempName != "")
                {
                    Name = Name + TempName;
                }
            }

            if (LotNo != null && LotNo != "")
            {
                Name = Name + ", LotNo : " + LotNo;
            }

            ViewBag.Name = Name;
            ViewBag.Dim1 = Dim1;
            ViewBag.Dim2 = Dim2;
            ViewBag.Process = Process;
            ViewBag.LotNo = LotNo;
            ViewBag.Godown = Godown;


            var settings = new StockInHandSettingService(_unitOfWork).GetTrailBalanceSetting(User.Identity.Name);
            ViewBag.GroupOn = settings.GroupOn;

            return View("StockLedgerIndex");
        }

        public JsonResult GetStockLedgerJson(int? ProductId, int? Dim1, int? Dim2, int? Process, string LotNo, int? Godown)
        {
            //return Json(new { data = _StockInHandService.GetStockLedger(ProductId, Dim1, Dim2, Process, LotNo, Godown, User.Identity.Name) }, JsonRequestBehavior.AllowGet);
            var T = Json(new { data = _StockInHandService.GetStockLedger(ProductId, Dim1, Dim2, Process, LotNo, Godown, User.Identity.Name).ToList() }, JsonRequestBehavior.AllowGet);
            T.MaxJsonLength = int.MaxValue;
            return T;

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
            HandleErrorInfo Excp = new HandleErrorInfo(new Exception("Document Settings not Configured"), "StockInHand", "DocumentMenu");
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
