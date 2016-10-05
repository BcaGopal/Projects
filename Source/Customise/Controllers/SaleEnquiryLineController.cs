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
using System.Text;
using Model.ViewModel;
using System.Xml.Linq;
using Reports.Controllers;
using Presentation.Helper;
using Model.DatabaseViews;

namespace Web
{

    [Authorize]
    public class SaleEnquiryLineController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        List<string> UserRoles = new List<string>();
        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        ISaleEnquiryLineService _SaleEnquiryLineService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        bool TimePlanValidation = true;
        string ExceptionMsg = "";
        bool Continue = true;

        public SaleEnquiryLineController(ISaleEnquiryLineService SaleEnquiry, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _SaleEnquiryLineService = SaleEnquiry;
            _unitOfWork = unitOfWork;
            _exception = exec;

            UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;
        }
   
        [HttpGet]
        public JsonResult Index(int id)
        {
            var p = _SaleEnquiryLineService.GetSaleEnquiryLineListForIndex(id).ToList();
            return Json(p, JsonRequestBehavior.AllowGet);

        }

        private void PrepareViewBag(SaleEnquiryHeader H)
        {
            ViewBag.Docno = H.DocNo;
            ViewBag.DeliveryUnitList = new UnitService(_unitOfWork).GetUnitList().ToList();
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
            SaleEnquiryHeader H = new SaleEnquiryHeaderService(_unitOfWork).GetSaleEnquiryHeader(Id);
            SaleEnquiryLineViewModel s = new SaleEnquiryLineViewModel();
            s.SaleEnquiryHeaderId = H.SaleEnquiryHeaderId;
            s.UnitId = "PCS";
            ViewBag.DocNo = H.DocNo;
            ViewBag.Status = H.Status;
            ViewBag.LineMode = "Create";


            var settings = new SaleEnquirySettingsService(_unitOfWork).GetSaleEnquirySettings(H.DocTypeId, H.DivisionId, H.SiteId);
            s.SaleEnquirySettings = Mapper.Map<SaleEnquirySettings, SaleEnquirySettingsViewModel>(settings);

            PrepareViewBag(H);
            return PartialView("_Create", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreatePost(SaleEnquiryLineViewModel svm)
        {
            SaleEnquiryLine s = Mapper.Map<SaleEnquiryLineViewModel, SaleEnquiryLine>(svm);
            SaleEnquiryHeader temp = new SaleEnquiryHeaderService(_unitOfWork).Find(s.SaleEnquiryHeaderId);
            //if (Command == "Submit" && (s.ProductId == 0))
            //    return RedirectToAction("Submit", "SaleEnquiryHeader", new { id = s.SaleEnquiryHeaderId }).Success("Data saved successfully");
            if (svm.Qty <= 0)
            {
                ModelState.AddModelError("Qty", "Please Check Qty");
            }

            if (svm.DueDate < temp.DocDate)
            {
                ModelState.AddModelError("DueDate", "DueDate greater than DocDate");
            }

            if (svm.SaleEnquiryLineId <= 0)
            {
                ViewBag.LineMode = "Create";
            }
            else
            {
                ViewBag.LineMode = "Edit";
            }

            if (ModelState.IsValid)
            {
                if (svm.SaleEnquiryLineId <= 0)
                {
                    s.CreatedDate = DateTime.Now;
                    s.ModifiedDate = DateTime.Now;
                    s.CreatedBy = User.Identity.Name;
                    s.ModifiedBy = User.Identity.Name;
                    s.ObjectState = Model.ObjectState.Added;
                    _SaleEnquiryLineService.Create(s);

                    SaleEnquiryLineExtended Extended = new SaleEnquiryLineExtended();
                    Extended.SaleEnquiryLineId = s.SaleEnquiryLineId;
                    Extended.ProductGroup = svm.ProductGroup;
                    Extended.Size = svm.Size;
                    Extended.ProductQuality = svm.ProductQuality;
                    Extended.Colour = svm.Colour;
                    new SaleEnquiryLineExtendedService(_unitOfWork).Create(Extended);

                    //new SaleEnquiryLineStatusService(_unitOfWork).CreateLineStatus(s.SaleEnquiryLineId);

                    SaleEnquiryHeader header = new SaleEnquiryHeaderService(_unitOfWork).Find(s.SaleEnquiryHeaderId);
                    if (header.Status != (int)StatusConstants.Drafted && header.Status != (int)StatusConstants.Import)
                    {
                        header.Status = (int)StatusConstants.Modified;
                        header.ModifiedDate = DateTime.Now;
                        header.ModifiedBy = User.Identity.Name;
                        new SaleEnquiryHeaderService(_unitOfWork).Update(header);
                    }


                    try
                    {
                        _unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXCL"] += message;
                        PrepareViewBag(temp);
                        return PartialView("_Create", svm);
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = header.DocTypeId,
                        DocId = header.SaleEnquiryHeaderId,
                        DocLineId = s.SaleEnquiryLineId,
                        ActivityType = (int)ActivityTypeContants.Added,
                        DocNo = header.DocNo,
                        DocDate = header.DocDate,
                        DocStatus = header.Status,
                    }));


                    return RedirectToAction("_Create", new { id = s.SaleEnquiryHeaderId });
                }
                else
                {
                    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();


                    SaleEnquiryHeader header = new SaleEnquiryHeaderService(_unitOfWork).Find(svm.SaleEnquiryHeaderId);
                    StringBuilder logstring = new StringBuilder();
                    int status = header.Status;
                    SaleEnquiryLine temp1 = _SaleEnquiryLineService.Find(svm.SaleEnquiryLineId);

                    SaleEnquiryLine ExRec = new SaleEnquiryLine();
                    ExRec = Mapper.Map<SaleEnquiryLine>(temp1);

                    //End of Tracking the Modifications::

                    temp1.DueDate = svm.DueDate;
                    temp1.ProductId = svm.ProductId ?? 0;
                    temp1.Specification = svm.Specification;
                    temp1.Dimension1Id = svm.Dimension1Id;
                    temp1.Dimension2Id = svm.Dimension2Id;
                    temp1.Qty = svm.Qty ?? 0;
                    temp1.UnitId = svm.UnitId;
                    temp1.DealQty = svm.DealQty ?? 0;
                    temp1.DealUnitId = svm.DealUnitId;
                    temp1.UnitConversionMultiplier = svm.UnitConversionMultiplier;
                    temp1.Rate = svm.Rate ?? 0;
                    temp1.Amount = svm.Amount ?? 0;
                    temp1.Remark = svm.Remark;
                    temp1.ModifiedDate = DateTime.Now;
                    temp1.ModifiedBy = User.Identity.Name;
                    _SaleEnquiryLineService.Update(temp1);

                    SaleEnquiryLineExtended Extended = new SaleEnquiryLineExtendedService(_unitOfWork).Find(svm.SaleEnquiryLineId);
                    Extended.ProductGroup = svm.ProductGroup;
                    Extended.Size = svm.Size;
                    Extended.ProductQuality = svm.ProductQuality;
                    Extended.Colour = svm.Colour;
                    new SaleEnquiryLineExtendedService(_unitOfWork).Update(Extended);


                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = ExRec,
                        Obj = temp1,
                    });

                    if (header.Status != (int)StatusConstants.Drafted && header.Status != (int)StatusConstants.Import)
                    {

                        header.Status = (int)StatusConstants.Modified;
                        header.ModifiedBy = User.Identity.Name;
                        header.ModifiedDate = DateTime.Now;
                        new SaleEnquiryHeaderService(_unitOfWork).Update(header);

                    }
                    XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);
                    try
                    {
                        _unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXCL"] += message;
                        PrepareViewBag(temp);
                        return PartialView("_Create", svm);
                    }

                    //Saving the Activity Log

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = temp.DocTypeId,
                        DocId = temp1.SaleEnquiryHeaderId,
                        DocLineId = temp1.SaleEnquiryLineId,
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

            ViewBag.Status = temp.Status;
            PrepareViewBag(temp);
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
            SaleEnquiryLine temp = _SaleEnquiryLineService.GetSaleEnquiryLine(id);
            SaleEnquiryLineExtended Extended = new SaleEnquiryLineExtendedService(_unitOfWork).Find(id);

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

            SaleEnquiryHeader H = new SaleEnquiryHeaderService(_unitOfWork).GetSaleEnquiryHeader(temp.SaleEnquiryHeaderId);
            ViewBag.DocNo = H.DocNo;
            SaleEnquiryLineViewModel s = Mapper.Map<SaleEnquiryLine, SaleEnquiryLineViewModel>(temp);

            s.ProductGroup = Extended.ProductGroup;
            s.Size = Extended.Size;
            s.ProductQuality = Extended.ProductQuality;
            s.Colour = Extended.Colour;



            var settings = new SaleEnquirySettingsService(_unitOfWork).GetSaleEnquirySettings(H.DocTypeId, H.DivisionId, H.SiteId);
            s.SaleEnquirySettings = Mapper.Map<SaleEnquirySettings, SaleEnquirySettingsViewModel>(settings);

            PrepareViewBag(H);

            return PartialView("_Create", s);
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


        [HttpGet]
        private ActionResult _Delete(int id)
        {
            SaleEnquiryLine temp = _SaleEnquiryLineService.GetSaleEnquiryLine(id);
            SaleEnquiryLineExtended Extended = new SaleEnquiryLineExtendedService(_unitOfWork).Find(id);

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

            SaleEnquiryHeader H = new SaleEnquiryHeaderService(_unitOfWork).GetSaleEnquiryHeader(temp.SaleEnquiryHeaderId);
            ViewBag.DocNo = H.DocNo;


            SaleEnquiryLineViewModel s = Mapper.Map<SaleEnquiryLine, SaleEnquiryLineViewModel>(temp);

            s.ProductGroup = Extended.ProductGroup;
            s.Size = Extended.Size;
            s.ProductQuality = Extended.ProductQuality;
            s.Colour = Extended.Colour;

            var settings = new SaleEnquirySettingsService(_unitOfWork).GetSaleEnquirySettings(H.DocTypeId, H.DivisionId, H.SiteId);
            s.SaleEnquirySettings = Mapper.Map<SaleEnquirySettings, SaleEnquirySettingsViewModel>(settings);


            PrepareViewBag(H);

            return PartialView("_Create", s);
        }

        [HttpGet]
        private ActionResult _Detail(int id)
        {
            SaleEnquiryLine temp = _SaleEnquiryLineService.GetSaleEnquiryLine(id);

            if (temp == null)
            {
                return HttpNotFound();
            }

            SaleEnquiryHeader H = new SaleEnquiryHeaderService(_unitOfWork).GetSaleEnquiryHeader(temp.SaleEnquiryHeaderId);
            ViewBag.DocNo = H.DocNo;
            SaleEnquiryLineViewModel s = Mapper.Map<SaleEnquiryLine, SaleEnquiryLineViewModel>(temp);
            PrepareViewBag(H);

            return PartialView("_Create", s);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(SaleEnquiryLineViewModel vm)
        {
            List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();
            new SaleEnquiryLineExtendedService(_unitOfWork).Delete(vm.SaleEnquiryLineId);

            SaleEnquiryLine SaleEnquiryLine = _SaleEnquiryLineService.Find(vm.SaleEnquiryLineId);
            //new SaleEnquiryLineStatusService(_unitOfWork).Delete(vm.SaleEnquiryLineId);
            _SaleEnquiryLineService.Delete(vm.SaleEnquiryLineId);
            SaleEnquiryHeader header = new SaleEnquiryHeaderService(_unitOfWork).Find(SaleEnquiryLine.SaleEnquiryHeaderId);

            if (header.Status != (int)StatusConstants.Drafted && header.Status != (int)StatusConstants.Import)
            {
                header.Status = (int)StatusConstants.Modified;
                header.ModifiedBy = User.Identity.Name;
                header.ModifiedDate = DateTime.Now;
                new SaleEnquiryHeaderService(_unitOfWork).Update(header);
            }

            LogList.Add(new LogTypeViewModel
            {
                Obj = SaleEnquiryLine,
            });

            XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);

            try
            {
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXCL"] += message;
                ViewBag.Docno = header.DocNo;
                ViewBag.DeliveryUnitList = new UnitService(_unitOfWork).GetUnitList().ToList();
                return PartialView("_Create", vm);
            }

            LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
            {
                DocTypeId = header.DocTypeId,
                DocId = header.SaleEnquiryHeaderId,
                DocLineId = SaleEnquiryLine.SaleEnquiryLineId,
                ActivityType = (int)ActivityTypeContants.Deleted,
                DocNo = header.DocNo,
                xEModifications = Modifications,
                DocDate = header.DocDate,
                DocStatus = header.Status,
            }));

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

        public JsonResult GetUnitConversionDetailJson(int ProductId, string UnitId, string DeliveryUnitId, int HeaderId)
        {

            int UnitConversionForId = new SaleEnquiryHeaderService(_unitOfWork).Find(HeaderId).UnitConversionForId;

            UnitConversion uc = new UnitConversionService(_unitOfWork).GetUnitConversionForUCF(ProductId, UnitId, DeliveryUnitId, UnitConversionForId);
            List<SelectListItem> UnitConversionJson = new List<SelectListItem>();
            if (uc != null)
            {
                UnitConversionJson.Add(new SelectListItem
                {
                    Text = uc.Multiplier.ToString(),
                    Value = uc.Multiplier.ToString()
                });
            }
            else
            {
                UnitConversionJson.Add(new SelectListItem
                {
                    Text = "0",
                    Value = "0"
                });
            }

            return Json(UnitConversionJson);
        }

        public JsonResult GetProductDetailJson(int ProductId)
        {
            ProductViewModel product = new ProductService(_unitOfWork).GetProduct(ProductId);
            List<Product> ProductJson = new List<Product>();

            ProductJson.Add(new Product()
            {
                ProductId = product.ProductId,
                StandardCost = product.StandardCost,
                UnitId = product.UnitId,
                ProductSpecification = product.ProductSpecification
            });

            return Json(ProductJson);
        }

        public JsonResult GetProductCustomDetailJson(int ProductId, int SaleEnquiryHeaderId)
        {
            SaleEnquiryHeader Header = new SaleEnquiryHeaderService(_unitOfWork).Find(SaleEnquiryHeaderId);

            ProductCustomDetailViewModel ProductCustomDetail = (from P in db.ViewProductBuyer
                                                                where P.ProductId == ProductId && P.BuyerId == Header.SaleToBuyerId
                                                                select new ProductCustomDetailViewModel
                                                                {
                                                                    ProductGroup = P.ProductGroup,
                                                                    Size = P.Size,
                                                                    ProductQuality = P.ProductQuality,
                                                                    Colour = P.Colour
                                                                }).FirstOrDefault();



            List<ProductCustomDetailViewModel> ProductCustomDetailJson = new List<ProductCustomDetailViewModel>();

            ProductCustomDetailJson.Add(new ProductCustomDetailViewModel()
            {
                ProductGroup = ProductCustomDetail.ProductGroup,
                Size = ProductCustomDetail.Size,
                ProductQuality = ProductCustomDetail.ProductQuality,
                Colour = ProductCustomDetail.Colour
            });

            return Json(ProductCustomDetailJson);
        }

        public JsonResult CheckForValidationinEdit(int ProductId, int SaleEnquiryHeaderId, int SaleEnquiryLineId)
        {
            var temp = (_SaleEnquiryLineService.CheckForProductExists(ProductId, SaleEnquiryHeaderId, SaleEnquiryLineId));
            return Json(new { returnvalue = temp });
        }

        public JsonResult CheckForValidation(int ProductId, int SaleEnquiryHeaderId)
        {
            var temp = (_SaleEnquiryLineService.CheckForProductExists(ProductId, SaleEnquiryHeaderId));
            return Json(new { returnvalue = temp });
        }


        public JsonResult GetBuyerSKU(int ProductId, int SaleEnquiryHeaderId)
        {
            string temp = (_SaleEnquiryLineService.GetBuyerSKU(ProductId, SaleEnquiryHeaderId));
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomProducts(string searchTerm, int pageSize, int pageNum, int filter)//DocTypeId
        {
            var Query = _SaleEnquiryLineService.GetCustomProducts(filter, searchTerm);
            var temp = Query.Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();

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

        public JsonResult SetSingleBuyerProduct(int Ids)
        {
            ComboBoxResult ProductJson = new ComboBoxResult();

            IEnumerable<ViewProductBuyer> prod = from p in db.ViewProductBuyer
                                        where p.ProductId == Ids
                                        select p;

            ProductJson.id = prod.FirstOrDefault().ProductId.ToString();
            ProductJson.text = prod.FirstOrDefault().ProductName;

            return Json(ProductJson);
        }



    }
    public class ProductCustomDetailViewModel
    {
        public int ProductId { get; set; }
        public string ProductGroup { get; set; }
        public string Size { get; set; }
        public string ProductQuality { get; set; }
        public string Colour { get; set; }
    }
}
