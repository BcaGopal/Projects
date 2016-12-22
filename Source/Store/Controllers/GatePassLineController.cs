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
using DocumentEvents;
using CustomEventArgs;
using JobOrderDocumentEvents;
using Reports.Controllers;

namespace Web
{

    [Authorize]
    public class GatePassLineController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private bool EventException = false;

        List<string> UserRoles = new List<string>();
        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        IGatePassLineService _GatePassLineService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        bool TimePlanValidation = true;
        string ExceptionMsg = "";
        bool Continue = true;

        public GatePassLineController(IGatePassLineService GatePass, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _GatePassLineService = GatePass;
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
            var p = _GatePassLineService.GetGatePassLineListForIndex(id).ToList();
            return Json(p, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult _Index(int id, int Status)
        {
            ViewBag.Status = Status;
            ViewBag.GatePassHeaderId = id;
            var p = _GatePassLineService.GetGatePassLineListForIndex(id).ToList();
            return PartialView(p);
        }


    
        private void PrepareViewBag(GatePassLineViewModel vm)
        {
            ViewBag.DeliveryUnitList = new UnitService(_unitOfWork).GetUnitList().ToList();
            if (vm != null)
            {
                GatePassHeaderViewModel H = new GatePassHeaderService(_unitOfWork).GetGatePassHeader(vm.GatePassHeaderId);
                ViewBag.DocNo = H.DocTypeName + "-" + H.DocNo;
            }
        }

        [HttpGet]
        public ActionResult CreateLine(int id, bool? IsProdBased)
        {
            return _Create(id, null, IsProdBased);
        }

        [HttpGet]
        public ActionResult CreateLineAfter_Submit(int id, bool? IsProdBased)
        {
            return _Create(id, null, IsProdBased);
        }

        [HttpGet]
        public ActionResult CreateLineAfter_Approve(int id, bool? IsProdBased)
        {
            return _Create(id, null, IsProdBased);
        }

        public ActionResult _Create(int Id, DateTime? date, bool? IsProdBased) 
        {
            GatePassHeader H = new GatePassHeaderService(_unitOfWork).Find(Id);
            GatePassLineViewModel s = new GatePassLineViewModel();
            s.GatePassHeaderId = H.GatePassHeaderId;
            ViewBag.Status = H.Status;            
            PrepareViewBag(s);
            ViewBag.LineMode = "Create";         
            return PartialView("_Create", s);
           

        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreatePost(GatePassLineViewModel svm)
        {
            List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();
            bool BeforeSave = true;
            GatePassHeader temp = new GatePassHeaderService(_unitOfWork).Find(svm.GatePassHeaderId);
            var PD = _GatePassLineService.GetGatePassLineListForIndex(svm.GatePassHeaderId).Where(x=>x.Product==svm.Product && x.Specification==svm.Specification).ToList();



            #region BeforeSave
            //try
            //{

            //    if (svm.GatePassLineId <= 0)
            //        BeforeSave = JobOrderDocEvents.beforeLineSaveEvent(this, new JobEventArgs(svm.GatePassHeaderId, EventModeConstants.Add), ref db);
            //    else
            //        BeforeSave = JobOrderDocEvents.beforeLineSaveEvent(this, new JobEventArgs(svm.GatePassHeaderId, EventModeConstants.Edit), ref db);

            //}
            //catch (Exception ex)
            //{
            //    string message = _exception.HandleException(ex);
            //    TempData["CSEXCL"] += message;
            //    EventException = true;
            //}

            //if (!BeforeSave)
            //    ModelState.AddModelError("", "Validation failed before save.");
            #endregion


            if (svm.Qty <= 0 || svm.Product=="" || svm.Product==null || svm.UnitId==null || svm.UnitId=="")
            {
                
                ModelState.AddModelError("Product", "The Product is required");
                ModelState.AddModelError("UnitId", "The Unit Name is required");
                ModelState.AddModelError("Qty", "The Qty is required");
            }
            if(PD.ToList().Count()!=0)
            {
                ModelState.AddModelError("Product",svm.Product +" "+ svm.Specification + " already exist");
            }
          

            GatePassLine s = Mapper.Map<GatePassLineViewModel, GatePassLine>(svm);

            ViewBag.Status = temp.Status;



            if (svm.GatePassLineId <= 0)
            {
                ViewBag.LineMode = "Create";
            }
            else
            {
                ViewBag.LineMode = "Edit";
            }

            if (ModelState.IsValid && BeforeSave && !EventException)
            {

                if (svm.GatePassLineId <= 0)
                {
                    //Posting in Stock                    
                    s.CreatedDate = DateTime.Now;
                    s.ModifiedDate = DateTime.Now;
                    s.CreatedBy = User.Identity.Name;                  
                    s.ModifiedBy = User.Identity.Name;
                    s.ObjectState = Model.ObjectState.Added;
                    db.GatePassLine.Add(s);

                    if (temp.Status != (int)StatusConstants.Drafted && temp.Status != (int)StatusConstants.Import)
                    {
                        temp.Status = (int)StatusConstants.Modified;
                        temp.ModifiedDate = DateTime.Now;
                        temp.ModifiedBy = User.Identity.Name;

                    }

                    temp.ObjectState = Model.ObjectState.Modified;
                    db.GatePassHeader.Add(temp);
                    //try
                    //{
                    //    JobOrderDocEvents.onLineSaveEvent(this, new JobEventArgs(s.GatePassHeaderId, s.GatePassLineId, EventModeConstants.Add), ref db);
                    //}
                    //catch (Exception ex)
                    //{
                    //    string message = _exception.HandleException(ex);
                    //    TempData["CSEXCL"] += message;
                    //    EventException = true;
                    //}
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


                    //try
                    //{
                    //    JobOrderDocEvents.afterLineSaveEvent(this, new JobEventArgs(s.GatePassHeaderId, s.GatePassHeaderId, EventModeConstants.Add), ref db);
                    //}
                    //catch (Exception ex)
                    //{
                    //    string message = _exception.HandleException(ex);
                    //    TempData["CSEXCL"] += message;
                    //}

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = temp.DocTypeId,
                        DocId = temp.GatePassHeaderId,
                        DocLineId = s.GatePassLineId,
                        ActivityType = (int)ActivityTypeContants.Added,                       
                        DocNo = temp.DocNo,
                        DocDate = temp.DocDate,
                        DocStatus=temp.Status,
                    }));

                    return RedirectToAction("_Create", new { id = svm.GatePassHeaderId, IsProdBased =false });
                    //return RedirectToAction("_Create", new { id = svm.GatePassHeaderId, IsProdBased = (s.ProdOrderLineId == null ? false : true) });

                }
                else
                {
                    GatePassLine templine = (from p in db.GatePassLine
                                             where p.GatePassLineId == s.GatePassLineId
                                             select p).FirstOrDefault();

                    GatePassLine ExTempLine = new GatePassLine();
                    ExTempLine = Mapper.Map<GatePassLine>(templine);

                    templine.GatePassHeaderId = s.GatePassHeaderId;
                    templine.Product = s.Product;
                    templine.Specification = s.Specification;
                    templine.Qty = s.Qty;
                    templine.UnitId = s.UnitId;
                    templine.ModifiedDate = DateTime.Now;
                    templine.ModifiedBy = User.Identity.Name;
                    templine.ObjectState = Model.ObjectState.Modified;
                    db.GatePassLine.Add(templine);
                    int Status = 0;
                    if (temp.Status != (int)StatusConstants.Drafted && temp.Status != (int)StatusConstants.Import)
                    {
                        Status = temp.Status;
                        temp.Status = (int)StatusConstants.Modified;
                        temp.ModifiedBy = User.Identity.Name;
                        temp.ModifiedDate = DateTime.Now;
                    }


                    temp.ObjectState = Model.ObjectState.Modified;
                    db.GatePassHeader.Add(temp);

             


                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = ExTempLine,
                        Obj = templine
                    });
                    
                    

                

                    XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);

                    //try
                    //{
                    //    JobOrderDocEvents.onLineSaveEvent(this, new JobEventArgs(s.GatePassHeaderId, templine.GatePassLineId, EventModeConstants.Edit), ref db);
                    //}
                    //catch (Exception ex)
                    //{
                    //    string message = _exception.HandleException(ex);
                    //    TempData["CSEXCL"] += message;
                    //    EventException = true;
                    //}

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
                        JobOrderDocEvents.afterLineSaveEvent(this, new JobEventArgs(s.GatePassHeaderId, templine.GatePassLineId, EventModeConstants.Edit), ref db);
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
                        DocId = templine.GatePassHeaderId,
                        DocLineId = templine.GatePassLineId,
                        ActivityType = (int)ActivityTypeContants.Modified,                       
                        DocNo = temp.DocNo,
                        xEModifications = Modifications,
                        DocDate = temp.DocDate,
                        DocStatus=temp.Status,
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

        [HttpGet]
        public ActionResult _ModifyLineAfterApprove(int id)
        {
            return _Modify(id);
        }

        private ActionResult _Modify(int id)
        {
            GatePassLineViewModel temp = _GatePassLineService.GetGatePassLine(id);

            GatePassHeader H = new GatePassHeaderService(_unitOfWork).Find(temp.GatePassHeaderId);

           
           
            if (temp == null)
            {
                return HttpNotFound();
            }
            PrepareViewBag(temp);

            //#region DocTypeTimeLineValidation
            //try
            //{

            //    TimePlanValidation = DocumentValidation.ValidateDocumentLine(new DocumentUniqueId { LockReason = temp.LockReason }, User.Identity.Name, out ExceptionMsg, out Continue);

            //}
            //catch (Exception ex)
            //{
            //    string message = _exception.HandleException(ex);
            //    TempData["CSEXCL"] += message;
            //    TimePlanValidation = false;
            //}

            //if (!TimePlanValidation)
            //    TempData["CSEXCL"] += ExceptionMsg;
            //#endregion

            if ((TimePlanValidation || Continue))
                ViewBag.LineMode = "Edit";

            //if (string.IsNullOrEmpty(temp.LockReason) || UserRoles.Contains("Admin"))
            //    ViewBag.LineMode = "Edit";
            //else
            //    TempData["CSEXCL"] += temp.LockReason;
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

        [HttpGet]
        public ActionResult _DeleteLine_AfterApprove(int id)
        {
            return _Delete(id);
        }

        private ActionResult _Delete(int id)
        {
            GatePassLineViewModel temp = _GatePassLineService.GetGatePassLine(id);

            GatePassHeader H = new GatePassHeaderService(_unitOfWork).Find(temp.GatePassHeaderId);

            //Getting Settings
           
            if (temp == null)
            {
                return HttpNotFound();
            }
            PrepareViewBag(temp);
            //ViewBag.LineMode = "Delete";

            #region DocTypeTimeLineValidation
            try
            {

                TimePlanValidation = DocumentValidation.ValidateDocumentLine(new DocumentUniqueId { LockReason =null}, User.Identity.Name, out ExceptionMsg, out Continue);

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

         

          return PartialView("_Create", temp);
           
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(GatePassLineViewModel vm)
        {
            bool BeforeSave = true;
            try
            {
                BeforeSave = JobOrderDocEvents.beforeLineDeleteEvent(this, new JobEventArgs(vm.GatePassHeaderId, vm.GatePassLineId), ref db);
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

                int? StockId = 0;
                int? StockProcessId = 0;
                List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

                GatePassLine GatePassLine = (from p in db.GatePassLine
                                             where p.GatePassLineId == vm.GatePassLineId
                                             select p).FirstOrDefault();
                GatePassHeader header = (from p in db.GatePassHeader
                                         where p.GatePassHeaderId == GatePassLine.GatePassHeaderId
                                         select p).FirstOrDefault();
               
                LogList.Add(new LogTypeViewModel
                {
                    Obj = Mapper.Map<GatePassLine>(GatePassLine),
                });

                //_JobOrderLineService.Delete(JobOrderLine);
                GatePassLine.ObjectState = Model.ObjectState.Deleted;
                db.GatePassLine.Remove(GatePassLine);




                if (header.Status != (int)StatusConstants.Drafted && header.Status != (int)StatusConstants.Import)
                {
                    header.Status = (int)StatusConstants.Modified;
                    header.ModifiedDate = DateTime.Now;
                    header.ModifiedBy = User.Identity.Name;
                    db.GatePassHeader.Add(header);
                }

           

                XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);

                //try
                //{
                //    JobOrderDocEvents.onLineDeleteEvent(this, new JobEventArgs(GatePassLine.GatePassHeaderId, GatePassLine.GatePassLineId), ref db);
                //}
                //catch (Exception ex)
                //{
                //    string message = _exception.HandleException(ex);
                //    TempData["CSEXCL"] += message;
                //    EventException = true;
                //}

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
                    PrepareViewBag(vm);
                    ViewBag.LineMode = "Delete";
                    return PartialView("_Create", vm);

                }

                try
                {
                    JobOrderDocEvents.afterLineDeleteEvent(this, new JobEventArgs(GatePassLine.GatePassHeaderId, GatePassLine.GatePassLineId), ref db);
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                }

                //Saving the Activity Log

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = header.DocTypeId,
                    DocId = header.GatePassHeaderId,
                    DocLineId = GatePassLine.GatePassLineId,
                    ActivityType = (int)ActivityTypeContants.Deleted,                  
                    DocNo = header.DocNo,
                    xEModifications = Modifications,
                    DocDate = header.DocDate,
                    DocStatus=header.Status,
                }));
            }

            return Json(new { success = true });

        }



        //public JsonResult GetProductDetailJson(int ProductId, int GatePassId)
        //{
        //    ProductViewModel product = new ProductService(_unitOfWork).GetProduct(ProductId);
        //    //List<Product> ProductJson = new List<Product>();

        //    //ProductJson.Add(new Product()
        //    //{
        //    //    ProductId = product.ProductId,
        //    //    StandardCost = product.StandardCost,
        //    //    UnitId = product.UnitId
        //    //});            

        //    var DealUnitId = _GatePassLineService.GetGatePassLineListForIndex(GatePassId).OrderByDescending(m => m.GatePassLineId).FirstOrDefault();

        //    //Decimal Rate = _JobOrderLineService.GetJobRate(JobOrderId, ProductId);

        //    Decimal Rate = 0;
        //    Decimal Discount = 0;
        //    Decimal Incentive = 0;
        //    Decimal Loss = 0;

          


        //    var Record = new GatePassHeaderService(_unitOfWork).Find(GatePassId);

        
        //    return Json(new { ProductId = product.ProductId, 
        //        StandardCost = Rate, 
        //        Discount = Discount, 
        //        Incentive = Incentive, 
        //        Loss = Loss, 
        //        UnitId = (!string.IsNullOrEmpty(Settings.JobUnitId) ? Settings.JobUnitId : product.UnitId), 
        //        DealUnitId = !string.IsNullOrEmpty(Settings.JobUnitId) ? Settings.JobUnitId : ((DealUnitId == null) ? (Settings.DealUnitId == null ? product.UnitId : Settings.DealUnitId) : DealUnitId.DealUnitId), 
        //        DealUnitDecimalPlaces = DlUnit.DecimalPlaces, 
        //        Specification = product.ProductSpecification });
        //}
        //public JsonResult getunitconversiondetailjson(int prodid, string UnitId, string DealUnitId, int JobOrderId)
        //{


        //    var Header = new JobOrderHeaderService(_unitOfWork).Find(JobOrderId);

        //    int DOctypeId = Header.DocTypeId;
        //    int siteId = Header.SiteId;
        //    int DivId = Header.DivisionId;


        //    UnitConversion uc = new UnitConversionService(_unitOfWork).GetUnitConversion(prodid, UnitId, (int)Header.UnitConversionForId, DealUnitId);


        //    byte DecimalPlaces = new UnitService(_unitOfWork).Find(DealUnitId).DecimalPlaces;
        //    string Text;
        //    string Value;


        //    if (uc != null)
        //    {
        //        Text = uc.Multiplier.ToString();
        //        Value = uc.Multiplier.ToString();
        //    }
        //    else
        //    {
        //        Text = "0";
        //        Value = "0";
        //    }


        //    return Json(new { Text = Text, Value = Value, DecimalPlace = DecimalPlaces });
        //}

        //public JsonResult GetPendingProdOrders(int ProductId)
        //{
        //    return Json(new ProdOrderHeaderService(_unitOfWork).GetPendingProdOrders(ProductId).ToList());
        //}

        //public JsonResult GetProdOrderDetail(int ProdOrderLineId, int JobOrderHeaderId)
        //{
        //    var temp = new ProdOrderLineService(_unitOfWork).GetProdOrderDetailBalance(ProdOrderLineId);

        //    var DealUnitId = _JobOrderLineService.GetJobOrderLineListForIndex(JobOrderHeaderId).OrderByDescending(m => m.JobOrderLineId).FirstOrDefault();

        //    var Record = new JobOrderHeaderService(_unitOfWork).Find(JobOrderHeaderId);

        //    var Settings = new JobOrderSettingsService(_unitOfWork).GetJobOrderSettingsForDocument(Record.DocTypeId, Record.DivisionId, Record.SiteId);

        //    var DlUnit = new UnitService(_unitOfWork).Find((DealUnitId == null) ? (Settings.DealUnitId == null ? temp.UnitId : Settings.DealUnitId) : DealUnitId.DealUnitId);
        //    temp.DealunitDecimalPlaces = DlUnit.DecimalPlaces;
        //    temp.DealUnitId = DlUnit.UnitId;

        //    return Json(temp);
        //}

        //public JsonResult GetProdOrderForBarCode(int ProdUId, int JobOrderHeaderId)
        //{
        //    var temp = new ProdOrderLineService(_unitOfWork).GetProdOrderForProdUid(ProdUId);
        //    var detail = GetProdOrderDetail(temp.ProdOrderLineId, JobOrderHeaderId);

        //    return Json(new { ProdOrder = temp, Detail = detail });
        //}


        //public JsonResult GetProdOrders(int id, string term, int Limit)//Order Header ID
        //{

        //    //string DocTypes = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(id, DivisionId, SiteId).filterContraDocTypes;

        //    return Json(_JobOrderLineService.GetPendingProdOrdersWithPatternMatch(id, term, Limit), JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetPendingProdOrdersHelpList(string searchTerm, int pageSize, int pageNum, int filter)//Order Header ID
        //{

        //    var temp = _JobOrderLineService.GetPendingProdOrderHelpList(filter, searchTerm).Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();

        //    var count = _JobOrderLineService.GetPendingProdOrderHelpList(filter, searchTerm).Count();

        //    ComboBoxPagedResult Data = new ComboBoxPagedResult();
        //    Data.Results = temp;
        //    Data.Total = count;

        //    return new JsonpResult
        //    {
        //        Data = Data,
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };

        //}

        //public JsonResult GetCustomProducts(int id, string term)//Indent Header ID
        //{
        //    return Json(_JobOrderLineService.GetProductHelpList(id, term), JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult SetFlagForAllowRepeatProcess()
        //{
        //    bool AllowRepeatProcess = true;

        //    return Json(AllowRepeatProcess);
        //}

        //public ActionResult IsProcessDone(string ProductUidName, int ProcessId)
        //{
        //    ProductUid ProductUid = new ProductUidService(_unitOfWork).Find(ProductUidName);
        //    int ProductUidId = 0;
        //    if (ProductUid != null)
        //    {
        //        ProductUidId = ProductUid.ProductUIDId;
        //    }

        //    return Json(new ProductUidService(_unitOfWork).IsProcessDone(ProductUidId, ProcessId));
        //}

        //public JsonResult GetProductPrevProcess(int ProductId, int GodownId, int DocTypeId)
        //{
        //    ProductPrevProcess ProductPrevProcess = new ProductService(_unitOfWork).FGetProductPrevProcess(ProductId, GodownId, DocTypeId);
        //    List<ProductPrevProcess> ProductPrevProcessJson = new List<ProductPrevProcess>();

        //    if (ProductPrevProcess != null)
        //    {
        //        ProductPrevProcessJson.Add(new ProductPrevProcess()
        //        {
        //            ProcessId = ProductPrevProcess.ProcessId
        //        });
        //        return Json(ProductPrevProcessJson);
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}

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
