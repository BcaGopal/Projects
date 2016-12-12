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

    
       
       

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult _ResultsPost(JobOrderMasterDetailModel vm)
        //{
        //    int Cnt = 0;
        //    int Serial = _JobOrderLineService.GetMaxSr(vm.JobOrderLineViewModel.FirstOrDefault().JobOrderHeaderId);
        //    List<HeaderChargeViewModel> HeaderCharges = new List<HeaderChargeViewModel>();
        //    List<LineChargeViewModel> LineCharges = new List<LineChargeViewModel>();
        //    Dictionary<int, decimal> LineStatus = new Dictionary<int, decimal>();
        //    List<JobOrderLine> BarCodesPendingToUpdate = new List<JobOrderLine>();

        //    bool BeforeSave = true;
        //    try
        //    {
        //        BeforeSave = JobOrderDocEvents.beforeLineSaveBulkEvent(this, new JobEventArgs(vm.JobOrderLineViewModel.FirstOrDefault().JobOrderHeaderId), ref db);
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = _exception.HandleException(ex);
        //        TempData["CSEXCL"] += message;
        //        EventException = true;
        //    }

        //    if (!BeforeSave)
        //        ModelState.AddModelError("", "Validation failed before save");


        //    int pk = 0;
        //    bool HeaderChargeEdit = false;

        //    JobOrderHeader Header = new JobOrderHeaderService(_unitOfWork).Find(vm.JobOrderLineViewModel.FirstOrDefault().JobOrderHeaderId);

        //    JobOrderSettings Settings = new JobOrderSettingsService(_unitOfWork).GetJobOrderSettingsForDocument(Header.DocTypeId, Header.DivisionId, Header.SiteId);

        //    int? MaxLineId = new JobOrderLineChargeService(_unitOfWork).GetMaxProductCharge(Header.JobOrderHeaderId, "Web.JobOrderLines", "JobOrderHeaderId", "JobOrderLineId");

        //    int PersonCount = 0;

        //    decimal Qty = vm.JobOrderLineViewModel.Where(m => m.Rate > 0).Sum(m => m.Qty);

        //    int CalculationId = Settings.CalculationId ?? 0;
        //    //List<string> uids = new List<string>();

        //    //if (!string.IsNullOrEmpty(Settings.SqlProcGenProductUID))
        //    //{
        //    //    uids = _JobOrderLineService.GetProcGenProductUids(Header.DocTypeId, Qty, Header.DivisionId, Header.SiteId);
        //    //}

        //    List<LineDetailListViewModel> LineList = new List<LineDetailListViewModel>();

        //    if (ModelState.IsValid && BeforeSave && !EventException)
        //    {
        //        foreach (var item in vm.JobOrderLineViewModel)
        //        {
        //            //if (item.Qty > 0 &&  ((Settings.isMandatoryRate.HasValue && Settings.isMandatoryRate == true )? item.Rate > 0 : 1 == 1))
        //            if (item.Qty > 0 && item.UnitConversionMultiplier > 0 && ((Settings.isVisibleRate == true && Settings.isMandatoryRate == true && item.Rate > 0) || (Settings.isVisibleRate == false || Settings.isVisibleRate == true && Settings.isMandatoryRate == false)))
        //            {
        //                JobOrderLine line = new JobOrderLine();

        //                if (Settings.isPostedInStock ?? false)
        //                {
        //                    StockViewModel StockViewModel = new StockViewModel();

        //                    if (Cnt == 0)
        //                    {
        //                        StockViewModel.StockHeaderId = Header.StockHeaderId ?? 0;
        //                    }
        //                    else
        //                    {
        //                        if (Header.StockHeaderId != null && Header.StockHeaderId != 0)
        //                        {
        //                            StockViewModel.StockHeaderId = (int)Header.StockHeaderId;
        //                        }
        //                        else
        //                        {
        //                            StockViewModel.StockHeaderId = -1;
        //                        }
        //                    }

        //                    StockViewModel.StockId = -Cnt;
        //                    StockViewModel.DocHeaderId = Header.JobOrderHeaderId;
        //                    StockViewModel.DocLineId = line.JobOrderLineId;
        //                    StockViewModel.DocTypeId = Header.DocTypeId;
        //                    StockViewModel.StockHeaderDocDate = Header.DocDate;
        //                    StockViewModel.StockDocDate = DateTime.Now.Date;
        //                    StockViewModel.DocNo = Header.DocNo;
        //                    StockViewModel.DivisionId = Header.DivisionId;
        //                    StockViewModel.SiteId = Header.SiteId;
        //                    StockViewModel.CurrencyId = null;
        //                    StockViewModel.PersonId = Header.JobWorkerId;
        //                    StockViewModel.ProductId = item.ProductId;
        //                    StockViewModel.HeaderFromGodownId = null;
        //                    StockViewModel.HeaderGodownId = Header.GodownId;
        //                    StockViewModel.HeaderProcessId = Header.ProcessId;
        //                    StockViewModel.GodownId = (int)Header.GodownId;
        //                    StockViewModel.Remark = Header.Remark;
        //                    StockViewModel.Status = Header.Status;
        //                    StockViewModel.ProcessId = Header.ProcessId;
        //                    StockViewModel.LotNo = null;
        //                    StockViewModel.CostCenterId = Header.CostCenterId;
        //                    StockViewModel.Qty_Iss = item.Qty;
        //                    StockViewModel.Qty_Rec = 0;
        //                    StockViewModel.Rate = item.Rate;
        //                    StockViewModel.ExpiryDate = null;
        //                    StockViewModel.Specification = item.Specification;
        //                    StockViewModel.Dimension1Id = item.Dimension1Id;
        //                    StockViewModel.Dimension2Id = item.Dimension2Id;
        //                    StockViewModel.ProductUidId = item.ProductUidId;
        //                    StockViewModel.CreatedBy = User.Identity.Name;
        //                    StockViewModel.CreatedDate = DateTime.Now;
        //                    StockViewModel.ModifiedBy = User.Identity.Name;
        //                    StockViewModel.ModifiedDate = DateTime.Now;

        //                    string StockPostingError = "";
        //                    StockPostingError = new StockService(_unitOfWork).StockPostDB(ref StockViewModel, ref db);

        //                    if (StockPostingError != "")
        //                    {
        //                        string message = StockPostingError;
        //                        ModelState.AddModelError("", message);
        //                        return PartialView("_Results", vm);
        //                    }

        //                    if (Cnt == 0)
        //                    {
        //                        Header.StockHeaderId = StockViewModel.StockHeaderId;
        //                    }
        //                    line.StockId = StockViewModel.StockId;
        //                }



        //                if (Settings.isPostedInStockProcess ?? false)
        //                {
        //                    StockProcessViewModel StockProcessViewModel = new StockProcessViewModel();

        //                    if (Header.StockHeaderId != null && Header.StockHeaderId != 0)//If Transaction Header Table Has Stock Header Id Then It will Save Here.
        //                    {
        //                        StockProcessViewModel.StockHeaderId = (int)Header.StockHeaderId;
        //                    }
        //                    else if (Settings.isPostedInStock ?? false)//If Stok Header is already posted during stock posting then this statement will Execute.So theat Stock Header will not generate again.
        //                    {
        //                        StockProcessViewModel.StockHeaderId = -1;
        //                    }
        //                    else if (Cnt > 0)//If function will only post in stock process then after first iteration of loop the stock header id will go -1
        //                    {
        //                        StockProcessViewModel.StockHeaderId = -1;
        //                    }
        //                    else//If function will only post in stock process then this statement will execute.For Example Job consumption.
        //                    {
        //                        StockProcessViewModel.StockHeaderId = 0;
        //                    }
        //                    StockProcessViewModel.StockProcessId = -Cnt;
        //                    StockProcessViewModel.DocHeaderId = Header.JobOrderHeaderId;
        //                    StockProcessViewModel.DocLineId = line.JobOrderLineId;
        //                    StockProcessViewModel.DocTypeId = Header.DocTypeId;
        //                    StockProcessViewModel.StockHeaderDocDate = Header.DocDate;
        //                    StockProcessViewModel.StockProcessDocDate = DateTime.Now.Date;
        //                    StockProcessViewModel.DocNo = Header.DocNo;
        //                    StockProcessViewModel.DivisionId = Header.DivisionId;
        //                    StockProcessViewModel.SiteId = Header.SiteId;
        //                    StockProcessViewModel.CurrencyId = null;
        //                    StockProcessViewModel.PersonId = Header.JobWorkerId;
        //                    StockProcessViewModel.ProductId = item.ProductId;
        //                    StockProcessViewModel.HeaderFromGodownId = null;
        //                    StockProcessViewModel.HeaderGodownId = Header.GodownId;
        //                    StockProcessViewModel.HeaderProcessId = Header.ProcessId;
        //                    StockProcessViewModel.GodownId = (int)Header.GodownId;
        //                    StockProcessViewModel.Remark = Header.Remark;
        //                    StockProcessViewModel.Status = Header.Status;
        //                    StockProcessViewModel.ProcessId = Header.ProcessId;
        //                    StockProcessViewModel.LotNo = null;
        //                    StockProcessViewModel.CostCenterId = Header.CostCenterId;
        //                    StockProcessViewModel.Qty_Iss = 0;
        //                    StockProcessViewModel.Qty_Rec = item.Qty;
        //                    StockProcessViewModel.Rate = item.Rate;
        //                    StockProcessViewModel.ExpiryDate = null;
        //                    StockProcessViewModel.Specification = item.Specification;
        //                    StockProcessViewModel.Dimension1Id = item.Dimension1Id;
        //                    StockProcessViewModel.Dimension2Id = item.Dimension2Id;
        //                    StockProcessViewModel.ProductUidId = item.ProductUidId;
        //                    StockProcessViewModel.CreatedBy = User.Identity.Name;
        //                    StockProcessViewModel.CreatedDate = DateTime.Now;
        //                    StockProcessViewModel.ModifiedBy = User.Identity.Name;
        //                    StockProcessViewModel.ModifiedDate = DateTime.Now;

        //                    string StockProcessPostingError = "";
        //                    StockProcessPostingError = new StockProcessService(_unitOfWork).StockProcessPostDB(ref StockProcessViewModel, ref db);

        //                    if (StockProcessPostingError != "")
        //                    {
        //                        string message = StockProcessPostingError;
        //                        ModelState.AddModelError("", message);
        //                        return PartialView("_Results", vm);
        //                    }


        //                    if ((Settings.isPostedInStock ?? false) == false)
        //                    {
        //                        if (Cnt == 0)
        //                        {
        //                            Header.StockHeaderId = StockProcessViewModel.StockHeaderId;
        //                        }
        //                    }

        //                    line.StockProcessId = StockProcessViewModel.StockProcessId;
        //                }


        //                line.JobOrderHeaderId = item.JobOrderHeaderId;
        //                line.ProdOrderLineId = item.ProdOrderLineId;
        //                line.ProductId = item.ProductId;
        //                line.Dimension1Id = item.Dimension1Id;
        //                line.Dimension2Id = item.Dimension2Id;
        //                line.Specification = item.Specification;
        //                line.Qty = item.Qty;
        //                line.UnitId = item.UnitId;
        //                line.Sr = Serial++;
        //                line.LossQty = item.LossQty;
        //                line.NonCountedQty = item.NonCountedQty;
        //                line.Rate = item.Rate;
        //                line.DealQty = item.UnitConversionMultiplier * item.Qty;
        //                line.DealUnitId = item.DealUnitId;
        //                line.Amount = DecimalRoundOff.amountToFixed((line.DealQty * line.Rate), Settings.AmountRoundOff);
        //                line.UnitConversionMultiplier = item.UnitConversionMultiplier;
        //                line.CreatedDate = DateTime.Now;
        //                line.ModifiedDate = DateTime.Now;
        //                line.CreatedBy = User.Identity.Name;
        //                line.ModifiedBy = User.Identity.Name;
        //                line.JobOrderLineId = pk;
        //                line.ObjectState = Model.ObjectState.Added;
        //                //_JobOrderLineService.Create(line);
        //                db.JobOrderLine.Add(line);

        //                if (line.ProdOrderLineId.HasValue)
        //                    LineStatus.Add(line.ProdOrderLineId.Value, line.Qty);


        //                new JobOrderLineStatusService(_unitOfWork).CreateLineStatus(line.JobOrderLineId, ref db, true);

        //                BarCodesPendingToUpdate.Add(line);


        //                #region BOMPost

        //                //Saving BOMPOST Data
        //                //Saving BOMPOST Data
        //                //Saving BOMPOST Data
        //                //Saving BOMPOST Data
        //                if (!string.IsNullOrEmpty(Settings.SqlProcConsumption))
        //                {
        //                    var BomPostList = _JobOrderLineService.GetBomPostingDataForWeaving(line.ProductId, line.Dimension1Id, line.Dimension2Id, Header.ProcessId, line.Qty, Header.DocTypeId, Settings.SqlProcConsumption).ToList();

        //                    foreach (var Bomitem in BomPostList)
        //                    {
        //                        JobOrderBom BomPost = new JobOrderBom();
        //                        BomPost.CreatedBy = User.Identity.Name;
        //                        BomPost.CreatedDate = DateTime.Now;
        //                        BomPost.Dimension1Id = Bomitem.Dimension1Id;
        //                        BomPost.Dimension2Id = Bomitem.Dimension2Id;
        //                        BomPost.JobOrderHeaderId = line.JobOrderHeaderId;
        //                        BomPost.JobOrderLineId = line.JobOrderLineId;
        //                        BomPost.ModifiedBy = User.Identity.Name;
        //                        BomPost.ModifiedDate = DateTime.Now;
        //                        BomPost.ProductId = Bomitem.ProductId;
        //                        BomPost.Qty = Convert.ToDecimal(Bomitem.Qty);
        //                        BomPost.BOMQty = Convert.ToDecimal(Bomitem.BOMQty);
        //                        BomPost.ObjectState = Model.ObjectState.Added;
        //                        db.JobOrderBom.Add(BomPost);
        //                        //new JobOrderBomService(_unitOfWork).Create(BomPost);
        //                    }
        //                }



        //                #endregion


        //                if (Settings != null)
        //                {
        //                    new CommonService().ExecuteCustomiseEvents(Settings.Event_OnLineSave, new object[] { _unitOfWork, line.JobOrderHeaderId, line.JobOrderLineId, "A" });
        //                }


        //                if (Settings.CalculationId.HasValue)
        //                {
        //                    LineList.Add(new LineDetailListViewModel { Amount = line.Amount, Rate = line.Rate, LineTableId = line.JobOrderLineId, HeaderTableId = item.JobOrderHeaderId, PersonID = Header.JobWorkerId, DealQty = line.DealQty });
        //                }
        //                pk++;
        //                Cnt = Cnt + 1;
        //            }

        //        }
        //        if (Header.Status != (int)StatusConstants.Drafted && Header.Status != (int)StatusConstants.Import)
        //        {
        //            Header.Status = (int)StatusConstants.Modified;
        //            Header.ModifiedBy = User.Identity.Name;
        //            Header.ModifiedDate = DateTime.Now;
        //        }
        //        //new JobOrderHeaderService(_unitOfWork).Update(Header);
        //        Header.ObjectState = Model.ObjectState.Modified;
        //        db.JobOrderHeader.Add(Header);

        //        new ProdOrderLineStatusService(_unitOfWork).UpdateProdQtyJobOrderMultiple(LineStatus, Header.DocDate, ref db);

        //        try
        //        {
        //            if (Settings.CalculationId.HasValue)
        //            {
        //                new ChargesCalculationService(_unitOfWork).CalculateCharges(LineList, vm.JobOrderLineViewModel.FirstOrDefault().JobOrderHeaderId, CalculationId, MaxLineId, out LineCharges, out HeaderChargeEdit, out HeaderCharges, "Web.JobOrderHeaderCharges", "Web.JobOrderLineCharges", out PersonCount, Header.DocTypeId, Header.SiteId, Header.DivisionId);

        //                //Saving Charges
        //                foreach (var item in LineCharges)
        //                {

        //                    JobOrderLineCharge PoLineCharge = Mapper.Map<LineChargeViewModel, JobOrderLineCharge>(item);
        //                    PoLineCharge.ObjectState = Model.ObjectState.Added;
        //                    db.JobOrderLineCharge.Add(PoLineCharge);
        //                    //new JobOrderLineChargeService(_unitOfWork).Create(PoLineCharge);

        //                }


        //                //Saving Header charges
        //                for (int i = 0; i < HeaderCharges.Count(); i++)
        //                {

        //                    if (!HeaderChargeEdit)
        //                    {
        //                        JobOrderHeaderCharge POHeaderCharge = Mapper.Map<HeaderChargeViewModel, JobOrderHeaderCharge>(HeaderCharges[i]);
        //                        POHeaderCharge.HeaderTableId = vm.JobOrderLineViewModel.FirstOrDefault().JobOrderHeaderId;
        //                        POHeaderCharge.PersonID = Header.JobWorkerId;
        //                        POHeaderCharge.ObjectState = Model.ObjectState.Added;
        //                        db.JobOrderHeaderCharges.Add(POHeaderCharge);
        //                        //new JobOrderHeaderChargeService(_unitOfWork).Create(POHeaderCharge);
        //                    }
        //                    else
        //                    {
        //                        var footercharge = new JobOrderHeaderChargeService(_unitOfWork).Find(HeaderCharges[i].Id);
        //                        footercharge.Rate = HeaderCharges[i].Rate;
        //                        footercharge.Amount = HeaderCharges[i].Amount;
        //                        footercharge.ObjectState = Model.ObjectState.Modified;
        //                        db.JobOrderHeaderCharges.Add(footercharge);
        //                        //new JobOrderHeaderChargeService(_unitOfWork).Update(footercharge);
        //                    }

        //                }
        //            }



        //            //_unitOfWork.Save();
        //        }

        //        catch (Exception ex)
        //        {
        //            string message = _exception.HandleException(ex);
        //            TempData["CSEXCL"] += message;
        //            return PartialView("_Results", vm);
        //        }

        //        try
        //        {
        //            JobOrderDocEvents.onLineSaveBulkEvent(this, new JobEventArgs(vm.JobOrderLineViewModel.FirstOrDefault().JobOrderHeaderId), ref db);
        //        }
        //        catch (Exception ex)
        //        {
        //            string message = _exception.HandleException(ex);
        //            TempData["CSEXCL"] += message;
        //            EventException = true;
        //        }

        //        try
        //        {
        //            if (EventException)
        //            { throw new Exception(); }
        //            db.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            string message = _exception.HandleException(ex);
        //            TempData["CSEXCL"] += message;
        //            return PartialView("_Results", vm);
        //        }


        //        if (Settings != null)
        //        {
        //            if (Settings.Event_AfterLineSave != null)
        //            {
        //                IEnumerable<JobOrderLineViewModel> linelist = new JobOrderLineService(_unitOfWork).GetJobOrderLineListForIndex(Header.JobOrderHeaderId);

        //                foreach (var item in linelist)
        //                {
        //                    new CommonService().ExecuteCustomiseEvents(Settings.Event_AfterLineSave, new object[] { _unitOfWork, item.JobOrderHeaderId, item.JobOrderLineId, "A" });
        //                }
        //            }
        //        }

        //        try
        //        {
        //            JobOrderDocEvents.afterLineSaveBulkEvent(this, new JobEventArgs(vm.JobOrderLineViewModel.FirstOrDefault().JobOrderHeaderId), ref db);
        //        }
        //        catch (Exception ex)
        //        {
        //            string message = _exception.HandleException(ex);
        //            TempData["CSEXC"] += message;
        //        }

        //        LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
        //        {
        //            DocTypeId = Header.DocTypeId,
        //            DocId = Header.JobOrderHeaderId,
        //            ActivityType = (int)ActivityTypeContants.MultipleCreate,                  
        //            DocNo = Header.DocNo,
        //            DocDate = Header.DocDate,
        //            DocStatus=Header.Status,
        //        }));

        //        return Json(new { success = true });

        //    }
        //    return PartialView("_Results", vm);

        //}


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

          

       

            #region BeforeSave
            try
            {

                if (svm.GatePassLineId <= 0)
                    BeforeSave = JobOrderDocEvents.beforeLineSaveEvent(this, new JobEventArgs(svm.GatePassHeaderId, EventModeConstants.Add), ref db);
                else
                    BeforeSave = JobOrderDocEvents.beforeLineSaveEvent(this, new JobEventArgs(svm.GatePassHeaderId, EventModeConstants.Edit), ref db);

            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXCL"] += message;
                EventException = true;
            }

            if (!BeforeSave)
                ModelState.AddModelError("", "Validation failed before save.");
            #endregion


            if (svm.Qty <= 0 || svm.Product=="" || svm.Product==null)
            {
                ModelState.AddModelError("Product", "The Product is required");
                ModelState.AddModelError("Qty", "The Qty is required");
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


                    try
                    {
                        JobOrderDocEvents.afterLineSaveEvent(this, new JobEventArgs(s.GatePassHeaderId, s.GatePassHeaderId, EventModeConstants.Add), ref db);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXCL"] += message;
                    }

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
