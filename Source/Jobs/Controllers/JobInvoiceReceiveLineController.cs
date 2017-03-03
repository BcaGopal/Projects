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
using Model.ViewModel;
using System.Xml.Linq;
using JobInvoiceReceiveDocumentEvents;
using CustomEventArgs;
using DocumentEvents;
using Reports.Controllers;
using Presentation.Helper;

namespace Web
{

    [Authorize]
    public class JobInvoiceReceiveLineController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        List<string> UserRoles = new List<string>();
        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        private bool EventException = false;
        IJobReceiveLineService _JobReceiveLineService;
        IJobInvoiceLineService _JobInvoiceLineService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        bool TimePlanValidation = true;
        string ExceptionMsg = "";
        bool Continue = true;

        public JobInvoiceReceiveLineController(IJobReceiveLineService JobReceive, IJobInvoiceLineService SaleOrder, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _JobReceiveLineService = JobReceive;
            _JobInvoiceLineService = SaleOrder;
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
            //var p = _JobInvoiceLineService.GetLineListForIndex(id).ToList();
            //return Json(p, JsonRequestBehavior.AllowGet);
            var p = Json(_JobInvoiceLineService.GetLineListForIndex(id).ToList(), JsonRequestBehavior.AllowGet);
            p.MaxJsonLength = int.MaxValue;
            return p;

        }

        public ActionResult _ForOrder(int id, int JobworkrId)
        {
            JobInvoiceLineFilterViewModel vm = new JobInvoiceLineFilterViewModel();
            vm.JobWorkerId = JobworkrId;
            vm.JobInvoiceHeaderId = id;
            JobInvoiceHeader Header = new JobInvoiceHeaderService(_unitOfWork).Find(id);
            vm.DocumentTypeSettings = new DocumentTypeSettingsService(_unitOfWork).GetDocumentTypeSettingsForDocument(Header.DocTypeId);
            return PartialView("_OrderFilters", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _FilterPostOrders(JobInvoiceLineFilterViewModel vm)
        {
            JobInvoiceHeader Header = new JobInvoiceHeaderService(_unitOfWork).Find(vm.JobInvoiceHeaderId);
            List<JobInvoiceLineViewModel> temp = _JobInvoiceLineService.GetJobOrderForFiltersForInvoiceReceive(vm).ToList();
            JobInvoiceMasterDetailModel svm = new JobInvoiceMasterDetailModel();
            JobInvoiceSettings settings = new JobInvoiceSettingsService(_unitOfWork).GetJobInvoiceSettingsForDocument(Header.DocTypeId, Header.DivisionId, Header.SiteId);
            svm.JobInvoiceSettings = Mapper.Map<JobInvoiceSettings, JobInvoiceSettingsViewModel>(settings);
            svm.JobInvoiceLineViewModel = temp;
            return PartialView("_OrderResults", svm);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ResultsPost(JobInvoiceMasterDetailModel vm)
        {

            List<HeaderChargeViewModel> HeaderCharges = new List<HeaderChargeViewModel>();
            List<LineChargeViewModel> LineCharges = new List<LineChargeViewModel>();
            int pk = 0;
            int Cnt = 0;
            int Cnt1 = 0;
            bool HeaderChargeEdit = false;
            int Serial = _JobInvoiceLineService.GetMaxSr(vm.JobInvoiceLineViewModel.FirstOrDefault().JobInvoiceHeaderId);
            List<LineReferenceIds> RefIds = new List<LineReferenceIds>();
            Dictionary<int, decimal> LineStatus = new Dictionary<int, decimal>();


            int IncentiveId = new ChargeService(_unitOfWork).GetChargeByName(ChargeConstants.Incentive).ChargeId;
            int PenaltyId = new ChargeService(_unitOfWork).GetChargeByName(ChargeConstants.Penalty).ChargeId;

            #region BeforeSave
            bool BeforeSave = true;
            try
            {
                BeforeSave = JobInvoiceReceiveDocEvents.beforeLineSaveBulkEvent(this, new JobEventArgs(vm.JobInvoiceLineViewModel.FirstOrDefault().JobInvoiceHeaderId), ref db);
            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXCL"] += message;
                EventException = true;
            }

            if (!BeforeSave)
                ModelState.AddModelError("", "Validation failed before save");
            #endregion

            JobInvoiceHeader Header = new JobInvoiceHeaderService(_unitOfWork).Find(vm.JobInvoiceLineViewModel.FirstOrDefault().JobInvoiceHeaderId);

            JobReceiveHeader ReceiveHeader = new JobReceiveHeaderService(_unitOfWork).Find(Header.JobReceiveHeaderId.Value);

            JobInvoiceSettings Settings = new JobInvoiceSettingsService(_unitOfWork).GetJobInvoiceSettingsForDocument(Header.DocTypeId, Header.DivisionId, Header.SiteId);

            int? MaxLineId = new JobInvoiceLineChargeService(_unitOfWork).GetMaxProductCharge(Header.JobInvoiceHeaderId, "Web.JobInvoiceLines", "JobInvoiceHeaderId", "JobInvoiceLineId");

            int PersonCount = 0;
            if (!Settings.CalculationId.HasValue)
            {
                throw new Exception("Calculation not configured in Invoice settings");
            }

            int CalculationId = Settings.CalculationId ?? 0;

            List<LineDetailListViewModel> LineList = new List<LineDetailListViewModel>();


            if (ModelState.IsValid && BeforeSave && !EventException)
            {

                var JobOrderLineIds = vm.JobInvoiceLineViewModel.Where(m => m.DealQty > 0).Select(m => m.JobOrderLineId).ToArray();

                var JObOrderCostCenters = (from p in db.JobOrderLine
                                           join h in db.JobOrderHeader on p.JobOrderHeaderId equals h.JobOrderHeaderId
                                           where JobOrderLineIds.Contains(p.JobOrderLineId)
                                           select new { CostCenterId = h.CostCenterId, Rate = p.Rate, LineId = p.JobOrderLineId }).ToList();


                var BalanceQtyRecords = (from p in db.ViewJobOrderBalance
                                         where JobOrderLineIds.Contains(p.JobOrderLineId)
                                         select new { BAlQty = p.BalanceQty, LineId = p.JobOrderLineId }).ToList();

                var JobOrderLineRecords = (from p in db.JobOrderLine
                                           where JobOrderLineIds.Contains(p.JobOrderLineId)
                                           select p).ToList();

                var JobOrderLineCharges = (from p in db.JobOrderLineCharge
                                           where JobOrderLineIds.Contains(p.LineTableId) && (p.ChargeId == IncentiveId || p.ChargeId == PenaltyId)
                                           select p).ToList();

                var JobOrderHEaderIds = JobOrderLineRecords.Select(m => m.JobOrderHeaderId).ToArray();

                var JobOrderHEaderRecords = (from p in db.JobOrderHeader
                                             where JobOrderHEaderIds.Contains(p.JobOrderHeaderId)
                                             select p).ToList();


                foreach (var item in vm.JobInvoiceLineViewModel.Where(m => m.DealQty > 0))
                {
                    if (item.DealQty > 0 && item.Rate > 0)
                    {
                        var temp = (from p in JObOrderCostCenters
                                    where p.LineId == item.JobOrderLineId
                                    select new { CostCenterId = p.CostCenterId, Rate = p.Rate }).FirstOrDefault();


                        var balqty = (from p in BalanceQtyRecords
                                      where p.LineId == item.JobOrderLineId
                                      select p.BAlQty).FirstOrDefault();

                        JobOrderLine JobOrderLine = JobOrderLineRecords.Where(m => m.JobOrderLineId == (item.JobOrderLineId)).FirstOrDefault();
                        JobOrderHeader JobOrderHeader = JobOrderHEaderRecords.Where(m => m.JobOrderHeaderId == (JobOrderLine.JobOrderHeaderId)).FirstOrDefault();


                        JobReceiveLine ReceiveLine = new JobReceiveLine();
                        ReceiveLine.JobReceiveLineId = -Cnt;
                        ReceiveLine.JobReceiveHeaderId = Header.JobReceiveHeaderId.Value;
                        ReceiveLine.JobOrderLineId = item.JobOrderLineId;
                        ReceiveLine.ProductUidId = JobOrderLine.ProductUidId;
                        ReceiveLine.Qty = item.ReceiveQty;
                        ReceiveLine.UnitConversionMultiplier = JobOrderLine.UnitConversionMultiplier;
                        ReceiveLine.DealUnitId = JobOrderLine.DealUnitId;
                        ReceiveLine.DealQty = ReceiveLine.UnitConversionMultiplier * ReceiveLine.Qty;
                        ReceiveLine.LossQty = item.LossQty;
                        ReceiveLine.PassQty = item.PassQty;
                        ReceiveLine.Sr = Serial;
                        if (JobOrderLineCharges.Where(m => m.LineTableId == item.JobOrderLineId && m.ChargeId == IncentiveId).FirstOrDefault() != null)
                        {
                            ReceiveLine.IncentiveRate = JobOrderLineCharges.Where(m => m.LineTableId == item.JobOrderLineId && m.ChargeId == IncentiveId).FirstOrDefault().Rate ?? 0;
                            ReceiveLine.IncentiveAmt = ReceiveLine.IncentiveRate * item.DealQty;
                        }
                        if (JobOrderLineCharges.Where(m => m.LineTableId == item.JobOrderLineId && m.ChargeId == PenaltyId).FirstOrDefault() != null)
                        {
                            ReceiveLine.PenaltyRate = JobOrderLineCharges.Where(m => m.LineTableId == item.JobOrderLineId && m.ChargeId == PenaltyId).FirstOrDefault().Rate ?? 0;
                            ReceiveLine.PenaltyAmt = ReceiveLine.PenaltyRate * item.DealQty;
                        }
                        ReceiveLine.LotNo = item.LotNo;
                        ReceiveLine.CreatedDate = DateTime.Now;
                        ReceiveLine.ModifiedDate = DateTime.Now;
                        ReceiveLine.CreatedBy = User.Identity.Name;
                        ReceiveLine.ModifiedBy = User.Identity.Name;

                        if (LineStatus.ContainsKey(ReceiveLine.JobOrderLineId))
                        {
                            LineStatus[ReceiveLine.JobOrderLineId] = LineStatus[ReceiveLine.JobOrderLineId] + 1;
                        }
                        else
                        {
                            LineStatus.Add(ReceiveLine.JobOrderLineId, ReceiveLine.Qty + ReceiveLine.LossQty);
                        }

                        if (JobOrderLine.ProductUidId.HasValue && JobOrderLine.ProductUidId.Value > 0)
                        {

                            ProductUid Uid = new ProductUidService(_unitOfWork).Find(JobOrderLine.ProductUidId.Value);

                            ReceiveLine.ProductUidLastTransactionDocId = Uid.LastTransactionDocId;
                            ReceiveLine.ProductUidLastTransactionDocDate = Uid.LastTransactionDocDate;
                            ReceiveLine.ProductUidLastTransactionDocNo = Uid.LastTransactionDocNo;
                            ReceiveLine.ProductUidLastTransactionDocTypeId = Uid.LastTransactionDocTypeId;
                            ReceiveLine.ProductUidLastTransactionPersonId = Uid.LastTransactionPersonId;
                            ReceiveLine.ProductUidStatus = Uid.Status;
                            ReceiveLine.ProductUidCurrentProcessId = Uid.CurrenctProcessId;
                            ReceiveLine.ProductUidCurrentGodownId = Uid.CurrenctGodownId;

                            if (Header.JobWorkerId == Uid.LastTransactionPersonId || Header.SiteId == 17)
                            {

                                Uid.LastTransactionDocId = ReceiveHeader.JobReceiveHeaderId;
                                Uid.LastTransactionDocNo = ReceiveHeader.DocNo;
                                Uid.LastTransactionDocTypeId = ReceiveHeader.DocTypeId;
                                Uid.LastTransactionDocDate = ReceiveHeader.DocDate;
                                Uid.LastTransactionPersonId = ReceiveHeader.JobWorkerId;
                                Uid.CurrenctGodownId = ReceiveHeader.GodownId;
                                Uid.CurrenctProcessId = ReceiveHeader.ProcessId;
                                Uid.Status = (!string.IsNullOrEmpty(Settings.BarcodeStatusUpdate) ? Settings.BarcodeStatusUpdate : ProductUidStatusConstants.Receive);
                                if (Uid.ProcessesDone == null)
                                {
                                    Uid.ProcessesDone = "|" + ReceiveHeader.ProcessId.ToString() + "|";
                                }
                                else
                                {
                                    Uid.ProcessesDone = Uid.ProcessesDone + ",|" + ReceiveHeader.ProcessId.ToString() + "|";
                                }
                                Uid.ObjectState = Model.ObjectState.Modified;
                                db.ProductUid.Add(Uid);

                            }

                        }


                        if (Settings.isPostedInStock ?? false)
                        {
                            StockViewModel StockViewModel = new StockViewModel();

                            if (Cnt == 0)
                            {
                                StockViewModel.StockHeaderId = ReceiveHeader.StockHeaderId ?? 0;
                            }
                            else
                            {
                                if (ReceiveHeader.StockHeaderId != null && ReceiveHeader.StockHeaderId != 0)
                                {
                                    StockViewModel.StockHeaderId = (int)ReceiveHeader.StockHeaderId;
                                }
                                else
                                {
                                    StockViewModel.StockHeaderId = -1;
                                }
                            }

                            StockViewModel.StockId = -Cnt;
                            StockViewModel.DocHeaderId = ReceiveHeader.JobReceiveHeaderId;
                            StockViewModel.DocLineId = ReceiveLine.JobReceiveLineId;
                            StockViewModel.DocTypeId = ReceiveHeader.DocTypeId;
                            StockViewModel.StockHeaderDocDate = ReceiveHeader.DocDate;
                            StockViewModel.StockDocDate = ReceiveHeader.DocDate;
                            StockViewModel.DocNo = ReceiveHeader.DocNo;
                            StockViewModel.DivisionId = ReceiveHeader.DivisionId;
                            StockViewModel.SiteId = ReceiveHeader.SiteId;
                            StockViewModel.CurrencyId = null;
                            StockViewModel.PersonId = ReceiveHeader.JobWorkerId;
                            StockViewModel.ProductId = JobOrderLine.ProductId;
                            StockViewModel.HeaderFromGodownId = null;
                            //Commented copying logic from single record entry
                            //StockViewModel.HeaderGodownId = ReceiveHeader.GodownId;
                            StockViewModel.HeaderGodownId = null;
                            StockViewModel.HeaderProcessId = ReceiveHeader.ProcessId;
                            StockViewModel.GodownId = (int)ReceiveHeader.GodownId;
                            StockViewModel.HeaderRemark = ReceiveHeader.Remark;
                            StockViewModel.Status = ReceiveHeader.Status;
                            StockViewModel.ProcessId = ReceiveHeader.ProcessId;
                            StockViewModel.LotNo = ReceiveLine.LotNo;

                            if (temp != null)
                            {
                                StockViewModel.CostCenterId = temp.CostCenterId;
                                StockViewModel.Rate = temp.Rate;
                            }
                            StockViewModel.Qty_Iss = 0;
                            StockViewModel.Qty_Rec = ReceiveLine.Qty;
                            StockViewModel.ExpiryDate = null;
                            StockViewModel.Specification = JobOrderLine.Specification;
                            StockViewModel.Dimension1Id = JobOrderLine.Dimension1Id;
                            StockViewModel.Dimension2Id = JobOrderLine.Dimension2Id;
                            StockViewModel.Dimension3Id = JobOrderLine.Dimension3Id;
                            StockViewModel.Dimension4Id = JobOrderLine.Dimension4Id;
                            StockViewModel.ProductUidId = ReceiveLine.ProductUidId;
                            StockViewModel.Remark = ReceiveLine.Remark;
                            StockViewModel.Status = ReceiveHeader.Status;
                            StockViewModel.CreatedBy = User.Identity.Name;
                            StockViewModel.CreatedDate = DateTime.Now;
                            StockViewModel.ModifiedBy = User.Identity.Name;
                            StockViewModel.ModifiedDate = DateTime.Now;

                            string StockPostingError = "";
                            StockPostingError = new StockService(_unitOfWork).StockPostDB(ref StockViewModel, ref db);

                            if (StockPostingError != "")
                            {
                                string message = StockPostingError;
                                ModelState.AddModelError("", message);
                                return PartialView("_Results", vm);
                            }

                            if (Cnt == 0)
                            {
                                ReceiveHeader.StockHeaderId = StockViewModel.StockHeaderId;
                            }
                            ReceiveLine.StockId = StockViewModel.StockId;
                        }


                        if (Settings.isPostedInStockProcess ?? false)
                        {
                            StockProcessViewModel StockProcessViewModel = new StockProcessViewModel();

                            if (ReceiveHeader.StockHeaderId != null && ReceiveHeader.StockHeaderId != 0)//If Transaction ReceiveHeader Table Has Stock ReceiveHeader Id Then It will Save Here.
                            {
                                StockProcessViewModel.StockHeaderId = (int)ReceiveHeader.StockHeaderId;
                            }
                            else if (Settings.isPostedInStock ?? false)//If Stok ReceiveHeader is already posted during stock posting then this statement will Execute.So theat Stock ReceiveHeader will not generate again.
                            {
                                StockProcessViewModel.StockHeaderId = -1;
                            }
                            else if (Cnt > 0)//If function will only post in stock process then after first iteration of loop the stock header id will go -1
                            {
                                StockProcessViewModel.StockHeaderId = -1;
                            }
                            else//If function will only post in stock process then this statement will execute.For Example Job consumption.
                            {
                                StockProcessViewModel.StockHeaderId = 0;
                            }

                            StockProcessViewModel.StockProcessId = -Cnt;
                            StockProcessViewModel.DocHeaderId = ReceiveHeader.JobReceiveHeaderId;
                            StockProcessViewModel.DocLineId = ReceiveLine.JobReceiveLineId;
                            StockProcessViewModel.DocTypeId = ReceiveHeader.DocTypeId;
                            StockProcessViewModel.StockHeaderDocDate = ReceiveHeader.DocDate;
                            StockProcessViewModel.StockProcessDocDate = ReceiveHeader.DocDate;
                            StockProcessViewModel.DocNo = ReceiveHeader.DocNo;
                            StockProcessViewModel.DivisionId = ReceiveHeader.DivisionId;
                            StockProcessViewModel.SiteId = ReceiveHeader.SiteId;
                            StockProcessViewModel.CurrencyId = null;
                            StockProcessViewModel.PersonId = ReceiveHeader.JobWorkerId;
                            StockProcessViewModel.ProductId = JobOrderLine.ProductId;
                            StockProcessViewModel.HeaderFromGodownId = null;
                            //Commented copying single entry logic
                            //StockProcessViewModel.HeaderProcessId = ReceiveHeader.ProcessId;
                            //StockProcessViewModel.HeaderGodownId = ReceiveHeader.GodownId;
                            StockProcessViewModel.HeaderGodownId = null;
                            StockProcessViewModel.HeaderProcessId = null;
                            StockProcessViewModel.GodownId = (int)ReceiveHeader.GodownId;
                            StockProcessViewModel.HeaderRemark = ReceiveHeader.Remark;
                            StockProcessViewModel.Status = ReceiveHeader.Status;
                            StockProcessViewModel.ProcessId = ReceiveHeader.ProcessId;
                            StockProcessViewModel.LotNo = ReceiveLine.LotNo;

                            if (temp != null)
                            {
                                StockProcessViewModel.CostCenterId = temp.CostCenterId;
                                StockProcessViewModel.Rate = temp.Rate;
                            }
                            StockProcessViewModel.Qty_Iss = ReceiveLine.Qty + ReceiveLine.LossQty;
                            StockProcessViewModel.Qty_Rec = 0;
                            StockProcessViewModel.ExpiryDate = null;
                            StockProcessViewModel.Specification = JobOrderLine.Specification;
                            StockProcessViewModel.Dimension1Id = JobOrderLine.Dimension1Id;
                            StockProcessViewModel.Dimension2Id = JobOrderLine.Dimension2Id;
                            StockProcessViewModel.Dimension3Id = JobOrderLine.Dimension3Id;
                            StockProcessViewModel.Dimension4Id = JobOrderLine.Dimension4Id;
                            StockProcessViewModel.Remark = ReceiveLine.Remark;
                            StockProcessViewModel.ProductUidId = ReceiveLine.ProductUidId;
                            StockProcessViewModel.Status = ReceiveHeader.Status;
                            StockProcessViewModel.CreatedBy = User.Identity.Name;
                            StockProcessViewModel.CreatedDate = DateTime.Now;
                            StockProcessViewModel.ModifiedBy = User.Identity.Name;
                            StockProcessViewModel.ModifiedDate = DateTime.Now;

                            string StockProcessPostingError = "";
                            StockProcessPostingError = new StockProcessService(_unitOfWork).StockProcessPostDB(ref StockProcessViewModel, ref db);

                            if (StockProcessPostingError != "")
                            {
                                string message = StockProcessPostingError;
                                ModelState.AddModelError("", message);
                                return PartialView("_Results", vm);
                            }

                            if ((Settings.isPostedInStock ?? false) == false)
                            {
                                if (Cnt == 0)
                                {
                                    ReceiveHeader.StockHeaderId = StockProcessViewModel.StockHeaderId;
                                }
                            }

                            ReceiveLine.StockProcessId = StockProcessViewModel.StockProcessId;
                        }



                        ReceiveLine.ObjectState = Model.ObjectState.Added;
                        db.JobReceiveLine.Add(ReceiveLine);
                        new JobReceiveLineStatusService(_unitOfWork).CreateLineStatusWithInvoice(ReceiveLine.JobReceiveLineId, ReceiveLine.PassQty, ReceiveLine.PassQty * JobOrderLine.UnitConversionMultiplier, ReceiveHeader.DocDate, ref db);


                        #region BomPost

                        //Saving BOMPOST Data
                        //Saving BOMPOST Data
                        //Saving BOMPOST Data
                        //Saving BOMPOST Data
                        if (!string.IsNullOrEmpty(Settings.SqlProcConsumption))
                        {
                            var BomPostList = _JobReceiveLineService.GetBomPostingDataForWeaving(item.ProductId, item.Dimension1Id, item.Dimension2Id, null, null, ReceiveHeader.ProcessId, item.PassQty, ReceiveHeader.DocTypeId, Settings.SqlProcConsumption, ReceiveLine.JobOrderLineId, ReceiveLine.Weight).ToList();

                            foreach (var BomItem in BomPostList)
                            {
                                JobReceiveBom BomPost = new JobReceiveBom();
                                BomPost.JobReceiveBomId = -Cnt1;
                                BomPost.JobReceiveHeaderId = ReceiveHeader.JobReceiveHeaderId;
                                BomPost.JobReceiveLineId = ReceiveLine.JobReceiveLineId;
                                BomPost.CreatedBy = User.Identity.Name;
                                BomPost.CreatedDate = DateTime.Now;
                                BomPost.ModifiedBy = User.Identity.Name;
                                BomPost.ModifiedDate = DateTime.Now;
                                BomPost.ProductId = BomItem.ProductId;
                                BomPost.Qty = Convert.ToDecimal(BomItem.Qty);


                                StockProcessViewModel StockProcessBomViewModel = new StockProcessViewModel();
                                if (ReceiveHeader.StockHeaderId != null && ReceiveHeader.StockHeaderId != 0)//If Transaction ReceiveHeader Table Has Stock ReceiveHeader Id Then It will Save Here.
                                {
                                    StockProcessBomViewModel.StockHeaderId = (int)ReceiveHeader.StockHeaderId;
                                }
                                else if (Settings.isPostedInStock ?? false)//If Stok ReceiveHeader is already posted during stock posting then this statement will Execute.So theat Stock ReceiveHeader will not generate again.
                                {
                                    StockProcessBomViewModel.StockHeaderId = -1;
                                }
                                else if (Cnt1 > 0)//If function will only post in stock process then after first iteration of loop the stock header id will go -1
                                {
                                    StockProcessBomViewModel.StockHeaderId = -1;
                                }
                                else//If function will only post in stock process then this statement will execute.For Example Job consumption.
                                {
                                    StockProcessBomViewModel.StockHeaderId = 0;
                                }

                                StockProcessBomViewModel.StockProcessId = -Cnt1;
                                StockProcessBomViewModel.DocHeaderId = ReceiveHeader.JobReceiveHeaderId;
                                StockProcessBomViewModel.DocLineId = ReceiveLine.JobReceiveLineId;
                                StockProcessBomViewModel.DocTypeId = ReceiveHeader.DocTypeId;
                                StockProcessBomViewModel.StockHeaderDocDate = ReceiveHeader.DocDate;
                                StockProcessBomViewModel.StockProcessDocDate = ReceiveHeader.DocDate;
                                StockProcessBomViewModel.DocNo = ReceiveHeader.DocNo;
                                StockProcessBomViewModel.DivisionId = ReceiveHeader.DivisionId;
                                StockProcessBomViewModel.SiteId = ReceiveHeader.SiteId;
                                StockProcessBomViewModel.CurrencyId = null;
                                StockProcessBomViewModel.HeaderProcessId = null;
                                StockProcessBomViewModel.PersonId = ReceiveHeader.JobWorkerId;
                                StockProcessBomViewModel.ProductId = BomItem.ProductId;
                                StockProcessBomViewModel.HeaderFromGodownId = null;
                                StockProcessBomViewModel.HeaderGodownId = null;
                                StockProcessBomViewModel.GodownId = ReceiveHeader.GodownId;
                                StockProcessBomViewModel.ProcessId = ReceiveHeader.ProcessId;
                                StockProcessBomViewModel.LotNo = null;
                                StockProcessBomViewModel.CostCenterId = JobOrderHeader.CostCenterId;
                                StockProcessBomViewModel.Qty_Iss = BomItem.Qty;
                                StockProcessBomViewModel.Qty_Rec = 0;
                                StockProcessBomViewModel.Rate = 0;
                                StockProcessBomViewModel.ExpiryDate = null;
                                StockProcessBomViewModel.Specification = null;
                                StockProcessBomViewModel.Dimension1Id = null;
                                StockProcessBomViewModel.Dimension2Id = null;
                                StockProcessBomViewModel.Dimension3Id = null;
                                StockProcessBomViewModel.Dimension4Id = null;
                                StockProcessBomViewModel.Remark = null;
                                StockProcessBomViewModel.Status = ReceiveHeader.Status;
                                StockProcessBomViewModel.CreatedBy = User.Identity.Name;
                                StockProcessBomViewModel.CreatedDate = DateTime.Now;
                                StockProcessBomViewModel.ModifiedBy = User.Identity.Name;
                                StockProcessBomViewModel.ModifiedDate = DateTime.Now;

                                string StockProcessPostingError = "";
                                StockProcessPostingError = new StockProcessService(_unitOfWork).StockProcessPostDB(ref StockProcessBomViewModel, ref db);

                                if (StockProcessPostingError != "")
                                {
                                    ModelState.AddModelError("", StockProcessPostingError);
                                    return PartialView("_Results", vm);
                                }

                                BomPost.StockProcessId = StockProcessBomViewModel.StockProcessId;
                                BomPost.ObjectState = Model.ObjectState.Added;
                                //new JobReceiveBomService(_unitOfWork).Create(BomPost);
                                db.JobReceiveBom.Add(BomPost);


                                if (Settings.isPostedInStock == false && Settings.isPostedInStockProcess == false)
                                {
                                    if (ReceiveHeader.StockHeaderId == null)
                                    {
                                        ReceiveHeader.StockHeaderId = StockProcessBomViewModel.StockHeaderId;
                                    }
                                }
                                Cnt1 = Cnt1 + 1;
                            }
                        }

                        #endregion


                        Cnt = Cnt + 1;

                        ReceiveHeader.ObjectState = Model.ObjectState.Modified;
                        db.JobReceiveHeader.Add(ReceiveHeader);

                        JobInvoiceLine line = new JobInvoiceLine();
                        line.JobInvoiceHeaderId = item.JobInvoiceHeaderId;
                        line.JobReceiveLineId = ReceiveLine.JobReceiveLineId;
                        line.UnitConversionMultiplier = item.UnitConversionMultiplier;
                        line.JobWorkerId = item.JobWorkerId;
                        line.Rate = item.Rate;
                        line.Sr = Serial++;
                        line.DealUnitId = item.DealUnitId;
                        line.Qty = item.PassQty;
                        line.DealQty = item.DealQty;
                        line.Amount = (item.DealQty * item.Rate);
                        line.IncentiveAmt = ReceiveLine.IncentiveAmt;
                        line.IncentiveRate = ReceiveLine.IncentiveRate;
                        line.CostCenterId = item.CostCenterId;
                        line.CreatedDate = DateTime.Now;
                        line.ModifiedDate = DateTime.Now;
                        line.CreatedBy = User.Identity.Name;
                        line.ModifiedBy = User.Identity.Name;
                        line.JobInvoiceLineId = pk;

                        line.ObjectState = Model.ObjectState.Added;
                        db.JobInvoiceLine.Add(line);

                        JobInvoiceLineStatus Status = new JobInvoiceLineStatus();
                        Status.JobInvoiceLineId = line.JobInvoiceLineId;
                        Status.ObjectState = Model.ObjectState.Added;
                        db.JobInvoiceLineStatus.Add(Status);

                        LineList.Add(new LineDetailListViewModel { Amount = line.Amount, Rate = line.Rate, LineTableId = line.JobInvoiceLineId, HeaderTableId = item.JobInvoiceHeaderId, PersonID = Header.JobWorkerId, DealQty = line.DealQty, CostCenterId = line.CostCenterId });
                        RefIds.Add(new LineReferenceIds { LineId = line.JobInvoiceLineId, RefLineId = ReceiveLine.JobOrderLineId });
                        pk++;

                    }
                }

                int[] RecLineIds = null;
                RecLineIds = RefIds.Select(m => m.RefLineId).ToArray();

                var Charges = (from p in db.JobOrderLine
                               where RecLineIds.Contains(p.JobOrderLineId)
                               join LineCharge in db.JobOrderLineCharge on p.JobOrderLineId equals LineCharge.LineTableId
                               join HeaderCharge in db.JobOrderHeaderCharges on p.JobOrderHeaderId equals HeaderCharge.HeaderTableId
                               group new { p, LineCharge, HeaderCharge } by new { p.JobOrderLineId } into g
                               select new
                               {
                                   LineId = g.Key.JobOrderLineId,
                                   HeaderCharges = g.Select(m => m.HeaderCharge).ToList(),
                                   Linecharges = g.Select(m => m.LineCharge).ToList(),
                               }).ToList();



                var LineListWithReferences = (from p in LineList
                                              join t in RefIds on p.LineTableId equals t.LineId
                                              join t2 in Charges on t.RefLineId equals t2.LineId into table
                                              from LineLis in table.DefaultIfEmpty()
                                              orderby p.LineTableId
                                              select new LineDetailListViewModel
                                              {
                                                  Amount = p.Amount,
                                                  DealQty = p.DealQty,
                                                  HeaderTableId = p.HeaderTableId,
                                                  LineTableId = p.LineTableId,
                                                  PersonID = p.PersonID,
                                                  Rate = p.Rate,
                                                  CostCenterId = p.CostCenterId,
                                                  RLineCharges = (LineLis == null ? null : Mapper.Map<List<LineChargeViewModel>>(LineLis.Linecharges)),
                                              }).ToList();


                new ChargesCalculationService(_unitOfWork).CalculateCharges(LineListWithReferences, vm.JobInvoiceLineViewModel.FirstOrDefault().JobInvoiceHeaderId, CalculationId, MaxLineId, out LineCharges, out HeaderChargeEdit, out HeaderCharges, "Web.JobInvoiceHeaderCharges", "Web.JobInvoiceLineCharges", out PersonCount, Header.DocTypeId, Header.SiteId, Header.DivisionId);

                //Calculate Charges::::::
                //CalculateCharges(LineList, vm.JobInvoiceLineViewModel.FirstOrDefault().JobInvoiceHeaderId, CalculationId, MaxLineId, out LineCharges, out HeaderChargeEdit, out HeaderCharges, "Web.JobInvoiceHeaderCharges", "Web.JobInvoiceLineCharges");


                //Saving Charges
                foreach (var item in LineCharges)
                {

                    JobInvoiceLineCharge PoLineCharge = Mapper.Map<LineChargeViewModel, JobInvoiceLineCharge>(item);
                    PoLineCharge.ObjectState = Model.ObjectState.Added;
                    db.JobInvoiceLineCharge.Add(PoLineCharge);
                }


                //Saving Header charges
                for (int i = 0; i < HeaderCharges.Count(); i++)
                {

                    if (!HeaderChargeEdit)
                    {
                        JobInvoiceHeaderCharge POHeaderCharge = Mapper.Map<HeaderChargeViewModel, JobInvoiceHeaderCharge>(HeaderCharges[i]);
                        POHeaderCharge.HeaderTableId = vm.JobInvoiceLineViewModel.FirstOrDefault().JobInvoiceHeaderId;
                        if (PersonCount <= 1)
                            POHeaderCharge.PersonID = vm.JobInvoiceLineViewModel.FirstOrDefault().JobWorkerId;
                        POHeaderCharge.ObjectState = Model.ObjectState.Added;
                        db.JobInvoiceHeaderCharges.Add(POHeaderCharge);
                    }
                    else
                    {
                        var footercharge = new JobInvoiceHeaderChargeService(_unitOfWork).Find(HeaderCharges[i].Id);
                        footercharge.Rate = HeaderCharges[i].Rate;
                        footercharge.Amount = HeaderCharges[i].Amount;
                        if (PersonCount > 1 || footercharge.PersonID != vm.JobInvoiceLineViewModel.FirstOrDefault().JobWorkerId)
                            footercharge.PersonID = null;

                        footercharge.ObjectState = Model.ObjectState.Modified;
                        db.JobInvoiceHeaderCharges.Add(footercharge);
                    }

                }

                if (Header.Status != (int)StatusConstants.Drafted && Header.Status != (int)StatusConstants.Import)
                {
                    Header.Status = (int)StatusConstants.Modified;
                    //ReceiveHeader.Status = (int)StatusConstants.Modified;
                    Header.ModifiedBy = User.Identity.Name;
                    Header.ModifiedDate = DateTime.Now;
                    ReceiveHeader.ModifiedBy = User.Identity.Name;
                    ReceiveHeader.ModifiedDate = DateTime.Now;
                }
                Header.ObjectState = Model.ObjectState.Modified;
                db.JobInvoiceHeader.Add(Header);

                ReceiveHeader.ObjectState = Model.ObjectState.Modified;
                db.JobReceiveHeader.Add(ReceiveHeader);

                new JobOrderLineStatusService(_unitOfWork).UpdateJobQtyInvoiceReceiveMultiple(LineStatus, Header.DocDate, ref db);

                try
                {
                    JobInvoiceReceiveDocEvents.onLineSaveBulkEvent(this, new JobEventArgs(vm.JobInvoiceLineViewModel.FirstOrDefault().JobInvoiceHeaderId), ref db);
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
                    //_unitOfWork.Save();
                }

                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXCL"] += message;
                    return PartialView("_OrderResults", vm);
                }

                try
                {
                    JobInvoiceReceiveDocEvents.afterLineSaveBulkEvent(this, new JobEventArgs(vm.JobInvoiceLineViewModel.FirstOrDefault().JobInvoiceHeaderId), ref db);
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                }

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = Header.DocTypeId,
                    DocId = Header.JobInvoiceHeaderId,
                    ActivityType = (int)ActivityTypeContants.MultipleCreate,
                    DocNo = Header.DocNo,
                    DocDate = Header.DocDate,
                    DocStatus = Header.Status,
                }));

                return Json(new { success = true });

            }
            return PartialView("_OrderResults", vm);

        }



        private void PrepareViewBag(JobInvoiceLineViewModel vm)
        {
            ViewBag.DeliveryUnitList = new UnitService(_unitOfWork).GetUnitList().ToList();
            if (vm != null)
            {
                var temp = new JobInvoiceHeaderService(_unitOfWork).Find(vm.JobInvoiceHeaderId);
                ViewBag.DocNo = temp.DocNo;
            }
        }

        [HttpGet]
        public JsonResult ConsumptionIndex(int id)
        {
            var p = _JobReceiveLineService.GetConsumptionLineListForIndex(id).ToList();
            return Json(p, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult ByProductIndex(int id)
        {
            var p = _JobReceiveLineService.GetByProductListForIndex(id).ToList();
            return Json(p, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult CreateLine(int Id, int JobWorkerId)
        {
            return _Create(Id, JobWorkerId);
        }

        [HttpGet]
        public ActionResult CreateLineAfter_Submit(int Id, int JobWorkerId)
        {
            return _Create(Id, JobWorkerId);
        }

        [HttpGet]
        public ActionResult CreateLineAfter_Approve(int Id, int JobWorkerId)
        {
            return _Create(Id, JobWorkerId);
        }


        public ActionResult _Create(int Id, int JobWorkerId) //Id ==>Job Invoice Header Id
        {
            JobInvoiceHeader H = new JobInvoiceHeaderService(_unitOfWork).Find(Id);
            JobInvoiceLineViewModel s = new JobInvoiceLineViewModel();

            //Getting Settings
            var settings = new JobInvoiceSettingsService(_unitOfWork).GetJobInvoiceSettingsForDocument(H.DocTypeId, H.DivisionId, H.SiteId);
            s.JobInvoiceSettings = Mapper.Map<JobInvoiceSettings, JobInvoiceSettingsViewModel>(settings);

            s.DocumentTypeSettings = new DocumentTypeSettingsService(_unitOfWork).GetDocumentTypeSettingsForDocument(H.DocTypeId);

            s.DocTypeId = H.DocTypeId;
            s.SiteId = H.SiteId;
            s.JobReceiveHeaderId = H.JobReceiveHeaderId.Value;
            s.DivisionId = H.DivisionId;
            s.JobWorkerId = JobWorkerId;
            s.JobInvoiceHeaderId = H.JobInvoiceHeaderId;
            PrepareViewBag(null);
            ViewBag.DocNo = H.DocNo;
            ViewBag.LineMode = "Create";
            return PartialView("_Create", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreatePost(JobInvoiceLineViewModel svm)
        {
            #region BeforeSave
            bool BeforeSave = true;
            try
            {

                if (svm.JobInvoiceLineId <= 0)
                    BeforeSave = JobInvoiceReceiveDocEvents.beforeLineSaveEvent(this, new JobEventArgs(svm.JobInvoiceHeaderId, EventModeConstants.Add), ref db);
                else
                    BeforeSave = JobInvoiceReceiveDocEvents.beforeLineSaveEvent(this, new JobEventArgs(svm.JobInvoiceHeaderId, EventModeConstants.Edit), ref db);

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

            JobInvoiceLine InvoiceLine = Mapper.Map<JobInvoiceLineViewModel, JobInvoiceLine>(svm);
            JobReceiveLine ReceiveLine = Mapper.Map<JobInvoiceLineViewModel, JobReceiveLine>(svm);
            JobInvoiceHeader InvoiceHeader = new JobInvoiceHeaderService(_unitOfWork).Find(InvoiceLine.JobInvoiceHeaderId);
            JobReceiveHeader ReceiveHeader = new JobReceiveHeaderService(_unitOfWork).Find(InvoiceHeader.JobReceiveHeaderId.Value);

            var settings = new JobInvoiceSettingsService(_unitOfWork).GetJobInvoiceSettingsForDocument(InvoiceHeader.DocTypeId, InvoiceHeader.DivisionId, InvoiceHeader.SiteId);

            if (svm.JobOrderLineId <= 0)
            {
                ModelState.AddModelError("JobOrderLineId", "Job Order field is required");
            }

            if (svm.JobQty <= 0)
                ModelState.AddModelError("JobQty", "The Job Qty field is required");

            if (svm.ReceiveQty <= 0)
                ModelState.AddModelError("ReceiveQty", "The Rec Qty field is required");

            if (svm.JobInvoiceLineId <= 0)
            {
                ViewBag.LineMode = "Create";
            }
            else
            {
                ViewBag.LineMode = "Edit";
            }

            if (ModelState.IsValid && BeforeSave && !EventException)
            {
                if (svm.JobInvoiceLineId <= 0)
                {

                    StockViewModel StockViewModel = new StockViewModel();
                    StockProcessViewModel StockProcessViewModel = new StockProcessViewModel();

                    JobOrderLine JobOrderLine = new JobOrderLineService(_unitOfWork).Find(ReceiveLine.JobOrderLineId);
                    JobOrderHeader JobOrderHeader = new JobOrderHeaderService(_unitOfWork).Find(JobOrderLine.JobOrderHeaderId);

                    ReceiveLine.Qty = svm.ReceiveQty;
                    ReceiveLine.LossQty = svm.LossQty;
                    ReceiveLine.JobReceiveHeaderId = ReceiveHeader.JobReceiveHeaderId;
                    ReceiveLine.PassQty = svm.PassQty;
                    ReceiveLine.Weight = svm.Weight;
                    ReceiveLine.UnitConversionMultiplier = svm.UnitConversionMultiplier;
                    ReceiveLine.DealUnitId = svm.DealUnitId;
                    ReceiveLine.DealQty = svm.DealQty;
                    ReceiveLine.IncentiveRate = svm.IncentiveRate;
                    ReceiveLine.IncentiveAmt = svm.IncentiveAmt;
                    ReceiveLine.PenaltyAmt = svm.PenaltyAmt;
                    ReceiveLine.PenaltyRate = svm.PenaltyRate;
                    ReceiveLine.LockReason = "Job invoice is created.";
                    ReceiveLine.CreatedDate = DateTime.Now;
                    ReceiveLine.ModifiedDate = DateTime.Now;
                    ReceiveLine.CreatedBy = User.Identity.Name;
                    ReceiveLine.ModifiedBy = User.Identity.Name;
                    ReceiveLine.Sr = _JobReceiveLineService.GetMaxSr(ReceiveLine.JobReceiveHeaderId);

                    //Posting in Stock
                    if (settings.isPostedInStock.HasValue && settings.isPostedInStock == true)
                    {
                        StockViewModel.StockHeaderId = ReceiveHeader.StockHeaderId ?? 0;
                        StockViewModel.DocHeaderId = ReceiveHeader.JobReceiveHeaderId;
                        StockViewModel.DocLineId = ReceiveLine.JobReceiveLineId;
                        StockViewModel.DocTypeId = ReceiveHeader.DocTypeId;
                        StockViewModel.StockHeaderDocDate = ReceiveHeader.DocDate;
                        StockViewModel.StockDocDate = ReceiveHeader.DocDate;
                        StockViewModel.DocNo = ReceiveHeader.DocNo;
                        StockViewModel.DivisionId = ReceiveHeader.DivisionId;
                        StockViewModel.SiteId = ReceiveHeader.SiteId;
                        StockViewModel.CurrencyId = null;
                        StockViewModel.HeaderProcessId = null;
                        StockViewModel.PersonId = ReceiveHeader.JobWorkerId;
                        StockViewModel.ProductId = JobOrderLine.ProductId;
                        StockViewModel.HeaderFromGodownId = null;
                        StockViewModel.HeaderGodownId = null;
                        StockViewModel.GodownId = ReceiveHeader.GodownId;
                        StockViewModel.ProcessId = ReceiveHeader.ProcessId;
                        StockViewModel.LotNo = ReceiveLine.LotNo;
                        StockViewModel.CostCenterId = JobOrderHeader.CostCenterId;
                        StockViewModel.Qty_Iss = 0;
                        StockViewModel.Qty_Rec = ReceiveLine.Qty;
                        StockViewModel.Rate = JobOrderLine.Rate;
                        StockViewModel.ExpiryDate = null;
                        StockViewModel.Specification = JobOrderLine.Specification;
                        StockViewModel.Dimension1Id = JobOrderLine.Dimension1Id;
                        StockViewModel.Dimension2Id = JobOrderLine.Dimension2Id;
                        StockViewModel.Dimension3Id = JobOrderLine.Dimension3Id;
                        StockViewModel.Dimension4Id = JobOrderLine.Dimension4Id;
                        StockViewModel.HeaderRemark = ReceiveHeader.Remark;
                        StockViewModel.Remark = ReceiveLine.Remark;
                        StockViewModel.ProductUidId = ReceiveLine.ProductUidId;
                        StockViewModel.Status = ReceiveHeader.Status;
                        StockViewModel.CreatedBy = ReceiveHeader.CreatedBy;
                        StockViewModel.CreatedDate = DateTime.Now;
                        StockViewModel.ModifiedBy = ReceiveHeader.ModifiedBy;
                        StockViewModel.ModifiedDate = DateTime.Now;

                        string StockPostingError = "";
                        StockPostingError = new StockService(_unitOfWork).StockPostDB(ref StockViewModel, ref db);

                        if (StockPostingError != "")
                        {
                            ModelState.AddModelError("", StockPostingError);
                            return PartialView("_Create", svm);
                        }

                        ReceiveLine.StockId = StockViewModel.StockId;

                        if (ReceiveHeader.StockHeaderId == null)
                        {
                            ReceiveHeader.StockHeaderId = StockViewModel.StockHeaderId;
                        }
                    }




                    //Posting in StockProcess
                    if (settings.isPostedInStockProcess.HasValue && settings.isPostedInStockProcess == true)
                    {
                        if (ReceiveHeader.StockHeaderId != null && ReceiveHeader.StockHeaderId != 0)//If Transaction Header Table Has Stock Header Id Then It will Save Here.
                        {
                            StockProcessViewModel.StockHeaderId = (int)ReceiveHeader.StockHeaderId;
                        }
                        else if (settings.isPostedInStock.HasValue && settings.isPostedInStock == true)//If Stok Header is already posted during stock posting then this statement will Execute.So theat Stock Header will not generate again.
                        {
                            StockProcessViewModel.StockHeaderId = -1;
                        }
                        else//If function will only post in stock process then this statement will execute.For Example Job consumption.
                        {
                            StockProcessViewModel.StockHeaderId = 0;
                        }
                        StockProcessViewModel.DocHeaderId = ReceiveHeader.JobReceiveHeaderId;
                        StockProcessViewModel.DocLineId = ReceiveLine.JobReceiveLineId;
                        StockProcessViewModel.DocTypeId = ReceiveHeader.DocTypeId;
                        StockProcessViewModel.StockHeaderDocDate = ReceiveHeader.DocDate;
                        StockProcessViewModel.StockProcessDocDate = ReceiveHeader.DocDate;
                        StockProcessViewModel.DocNo = ReceiveHeader.DocNo;
                        StockProcessViewModel.DivisionId = ReceiveHeader.DivisionId;
                        StockProcessViewModel.SiteId = ReceiveHeader.SiteId;
                        StockProcessViewModel.CurrencyId = null;
                        StockProcessViewModel.HeaderProcessId = null;
                        StockProcessViewModel.PersonId = ReceiveHeader.JobWorkerId;
                        StockProcessViewModel.ProductId = JobOrderLine.ProductId;
                        StockProcessViewModel.HeaderFromGodownId = null;
                        StockProcessViewModel.HeaderGodownId = null;
                        StockProcessViewModel.GodownId = ReceiveHeader.GodownId;
                        StockProcessViewModel.ProcessId = ReceiveHeader.ProcessId;
                        StockProcessViewModel.LotNo = ReceiveLine.LotNo;
                        StockProcessViewModel.CostCenterId = JobOrderHeader.CostCenterId;
                        StockProcessViewModel.Qty_Iss = ReceiveLine.Qty + ReceiveLine.LossQty;
                        StockProcessViewModel.Qty_Rec = 0;
                        StockProcessViewModel.Rate = JobOrderLine.Rate;
                        StockProcessViewModel.ExpiryDate = null;
                        StockProcessViewModel.Specification = JobOrderLine.Specification;
                        StockProcessViewModel.Dimension1Id = JobOrderLine.Dimension1Id;
                        StockProcessViewModel.Dimension2Id = JobOrderLine.Dimension2Id;
                        StockProcessViewModel.Dimension3Id = JobOrderLine.Dimension3Id;
                        StockProcessViewModel.Dimension4Id = JobOrderLine.Dimension4Id;
                        StockProcessViewModel.HeaderRemark = ReceiveHeader.Remark;
                        StockProcessViewModel.Remark = ReceiveLine.Remark;
                        StockProcessViewModel.ProductUidId = ReceiveLine.ProductUidId;
                        StockProcessViewModel.Status = ReceiveHeader.Status;
                        StockProcessViewModel.CreatedBy = User.Identity.Name;
                        StockProcessViewModel.CreatedDate = DateTime.Now;
                        StockProcessViewModel.ModifiedBy = User.Identity.Name;
                        StockProcessViewModel.ModifiedDate = DateTime.Now;

                        string StockProcessPostingError = "";
                        StockProcessPostingError = new StockProcessService(_unitOfWork).StockProcessPostDB(ref StockProcessViewModel, ref db);

                        if (StockProcessPostingError != "")
                        {
                            ModelState.AddModelError("", StockProcessPostingError);
                            return PartialView("_Create", svm);
                        }

                        ReceiveLine.StockProcessId = StockProcessViewModel.StockProcessId;

                        if (settings.isPostedInStock == false)
                        {
                            if (ReceiveHeader.StockHeaderId == null)
                            {
                                ReceiveHeader.StockHeaderId = StockProcessViewModel.StockHeaderId;
                            }
                        }
                    }

                    #region BomPost

                    //Saving BOMPOST Data
                    //Saving BOMPOST Data
                    //Saving BOMPOST Data
                    //Saving BOMPOST Data
                    if (!string.IsNullOrEmpty(svm.JobInvoiceSettings.SqlProcConsumption))
                    {
                        var BomPostList = _JobReceiveLineService.GetBomPostingDataForWeaving(svm.ProductId, svm.Dimension1Id, svm.Dimension2Id, null, null, ReceiveHeader.ProcessId, ReceiveLine.PassQty, ReceiveHeader.DocTypeId, svm.JobInvoiceSettings.SqlProcConsumption, ReceiveLine.JobOrderLineId, ReceiveLine.Weight).ToList();

                        foreach (var item in BomPostList)
                        {
                            JobReceiveBom BomPost = new JobReceiveBom();
                            BomPost.JobReceiveHeaderId = ReceiveHeader.JobReceiveHeaderId;
                            BomPost.JobReceiveLineId = ReceiveLine.JobReceiveLineId;
                            BomPost.CreatedBy = User.Identity.Name;
                            BomPost.CreatedDate = DateTime.Now;
                            BomPost.ModifiedBy = User.Identity.Name;
                            BomPost.ModifiedDate = DateTime.Now;
                            BomPost.ProductId = item.ProductId;
                            BomPost.Qty = Convert.ToDecimal(item.Qty);



                            StockProcessViewModel StockProcessBomViewModel = new StockProcessViewModel();
                            if (ReceiveHeader.StockHeaderId != null && ReceiveHeader.StockHeaderId != 0)//If Transaction Header Table Has Stock Header Id Then It will Save Here.
                            {
                                StockProcessBomViewModel.StockHeaderId = (int)ReceiveHeader.StockHeaderId;
                            }
                            else if (svm.JobInvoiceSettings.isPostedInStock)//If Stok Header is already posted during stock posting then this statement will Execute.So theat Stock Header will not generate again.
                            {
                                StockProcessBomViewModel.StockHeaderId = -1;
                            }
                            else//If function will only post in stock process then this statement will execute.For Example Job consumption.
                            {
                                StockProcessBomViewModel.StockHeaderId = 0;
                            }
                            StockProcessBomViewModel.StockProcessId = -1;
                            StockProcessBomViewModel.DocHeaderId = ReceiveHeader.JobReceiveHeaderId;
                            StockProcessBomViewModel.DocLineId = ReceiveLine.JobReceiveLineId;
                            StockProcessBomViewModel.DocTypeId = ReceiveHeader.DocTypeId;
                            StockProcessBomViewModel.StockHeaderDocDate = ReceiveHeader.DocDate;
                            StockProcessBomViewModel.StockProcessDocDate = ReceiveHeader.DocDate;
                            StockProcessBomViewModel.DocNo = ReceiveHeader.DocNo;
                            StockProcessBomViewModel.DivisionId = ReceiveHeader.DivisionId;
                            StockProcessBomViewModel.SiteId = ReceiveHeader.SiteId;
                            StockProcessBomViewModel.CurrencyId = null;
                            StockProcessBomViewModel.HeaderProcessId = null;
                            StockProcessBomViewModel.PersonId = ReceiveHeader.JobWorkerId;
                            StockProcessBomViewModel.ProductId = item.ProductId;
                            StockProcessBomViewModel.HeaderFromGodownId = null;
                            StockProcessBomViewModel.HeaderGodownId = null;
                            StockProcessBomViewModel.GodownId = ReceiveHeader.GodownId;
                            StockProcessBomViewModel.ProcessId = ReceiveHeader.ProcessId;
                            StockProcessBomViewModel.LotNo = ReceiveLine.LotNo;
                            StockProcessBomViewModel.CostCenterId = JobOrderHeader.CostCenterId;
                            StockProcessBomViewModel.Qty_Iss = item.Qty;
                            StockProcessBomViewModel.Qty_Rec = 0;
                            StockProcessBomViewModel.Rate = 0;
                            StockProcessBomViewModel.ExpiryDate = null;
                            StockProcessBomViewModel.Specification = null;
                            StockProcessBomViewModel.Dimension1Id = null;
                            StockProcessBomViewModel.Dimension2Id = null;
                            StockProcessBomViewModel.Dimension3Id = null;
                            StockProcessBomViewModel.Dimension4Id = null;
                            StockProcessBomViewModel.Remark = null;
                            StockProcessBomViewModel.Status = ReceiveHeader.Status;
                            StockProcessBomViewModel.CreatedBy = User.Identity.Name;
                            StockProcessBomViewModel.CreatedDate = DateTime.Now;
                            StockProcessBomViewModel.ModifiedBy = User.Identity.Name;
                            StockProcessBomViewModel.ModifiedDate = DateTime.Now;

                            string StockProcessPostingError = "";
                            StockProcessPostingError = new StockProcessService(_unitOfWork).StockProcessPostDB(ref StockProcessBomViewModel, ref db);

                            if (StockProcessPostingError != "")
                            {
                                ModelState.AddModelError("", StockProcessPostingError);
                                return PartialView("_Create", svm);
                            }

                            BomPost.StockProcessId = StockProcessBomViewModel.StockProcessId;
                            BomPost.ObjectState = Model.ObjectState.Added;
                            db.JobReceiveBom.Add(BomPost);
                            //new JobReceiveBomService(_unitOfWork).Create(BomPost);

                            if (svm.JobInvoiceSettings.isPostedInStock == false && svm.JobInvoiceSettings.isPostedInStockProcess == false)
                            {
                                if (ReceiveHeader.StockHeaderId == null)
                                {
                                    ReceiveHeader.StockHeaderId = StockProcessBomViewModel.StockHeaderId;
                                }
                            }


                        }
                    }

                    #endregion

                    if (svm.ProductUidId != null && svm.ProductUidId > 0)
                    {
                        ProductUid Produid = (from p in db.ProductUid
                                              where p.ProductUIDId == svm.ProductUidId
                                              select p).FirstOrDefault();


                        ReceiveLine.ProductUidLastTransactionDocId = Produid.LastTransactionDocId;
                        ReceiveLine.ProductUidLastTransactionDocDate = Produid.LastTransactionDocDate;
                        ReceiveLine.ProductUidLastTransactionDocNo = Produid.LastTransactionDocNo;
                        ReceiveLine.ProductUidLastTransactionDocTypeId = Produid.LastTransactionDocTypeId;
                        ReceiveLine.ProductUidLastTransactionPersonId = Produid.LastTransactionPersonId;
                        ReceiveLine.ProductUidStatus = Produid.Status;
                        ReceiveLine.ProductUidCurrentProcessId = Produid.CurrenctProcessId;
                        ReceiveLine.ProductUidCurrentGodownId = Produid.CurrenctGodownId;



                        Produid.LastTransactionDocId = ReceiveHeader.JobReceiveHeaderId;
                        Produid.LastTransactionDocNo = ReceiveHeader.DocNo;
                        Produid.LastTransactionDocTypeId = ReceiveHeader.DocTypeId;
                        Produid.LastTransactionDocDate = ReceiveHeader.DocDate;
                        Produid.LastTransactionPersonId = ReceiveHeader.JobWorkerId;
                        Produid.CurrenctGodownId = ReceiveHeader.GodownId;
                        Produid.CurrenctProcessId = ReceiveHeader.ProcessId;
                        Produid.Status = (!string.IsNullOrEmpty(settings.BarcodeStatusUpdate) ? settings.BarcodeStatusUpdate : ProductUidStatusConstants.Receive);

                        if (Produid.ProcessesDone == null)
                        {
                            Produid.ProcessesDone = "|" + ReceiveHeader.ProcessId.ToString() + "|";
                        }
                        else
                        {
                            Produid.ProcessesDone = Produid.ProcessesDone + ",|" + ReceiveHeader.ProcessId.ToString() + "|";
                        }

                        Produid.ObjectState = Model.ObjectState.Modified;
                        db.ProductUid.Add(Produid);
                    }

                    ReceiveLine.ObjectState = Model.ObjectState.Added;
                    db.JobReceiveLine.Add(ReceiveLine);

                    new JobReceiveLineStatusService(_unitOfWork).CreateLineStatusWithInvoice(ReceiveLine.JobReceiveLineId, (ReceiveLine.PassQty + ReceiveLine.LossQty), (ReceiveLine.PassQty + ReceiveLine.LossQty) * JobOrderLine.UnitConversionMultiplier, InvoiceHeader.DocDate, ref db);



                    InvoiceLine.JobReceiveLineId = ReceiveLine.JobReceiveLineId;
                    InvoiceLine.CreatedDate = DateTime.Now;
                    InvoiceLine.ModifiedDate = DateTime.Now;
                    InvoiceLine.Sr = _JobInvoiceLineService.GetMaxSr(InvoiceLine.JobInvoiceHeaderId);
                    InvoiceLine.Qty = svm.PassQty;
                    InvoiceLine.IncentiveAmt = ReceiveLine.IncentiveAmt;
                    InvoiceLine.IncentiveRate = ReceiveLine.IncentiveRate;
                    InvoiceLine.CreatedBy = User.Identity.Name;
                    InvoiceLine.ModifiedBy = User.Identity.Name;
                    InvoiceLine.ObjectState = Model.ObjectState.Added;
                    //_JobInvoiceLineService.Create(InvoiceLine);


                    JobInvoiceLineStatus Status = new JobInvoiceLineStatus();
                    Status.JobInvoiceLineId = InvoiceLine.JobInvoiceLineId;
                    Status.ObjectState = Model.ObjectState.Added;
                    db.JobInvoiceLineStatus.Add(Status);

                    new JobOrderLineStatusService(_unitOfWork).UpdateJobQtyOnInvoiceReceive(ReceiveLine.JobOrderLineId, InvoiceLine.JobInvoiceLineId, InvoiceHeader.DocDate, (ReceiveLine.Qty + ReceiveLine.LossQty), InvoiceLine.UnitConversionMultiplier, ref db, true);


                    db.JobInvoiceLine.Add(InvoiceLine);



                    if (svm.linecharges != null)
                        foreach (var item in svm.linecharges)
                        {
                            item.LineTableId = InvoiceLine.JobInvoiceLineId;
                            item.PersonID = InvoiceLine.JobWorkerId;
                            item.HeaderTableId = InvoiceLine.JobInvoiceHeaderId;
                            item.CostCenterId = InvoiceLine.CostCenterId;
                            item.ObjectState = Model.ObjectState.Added;
                            //new JobInvoiceLineChargeService(_unitOfWork).Create(item);
                            db.JobInvoiceLineCharge.Add(item);
                        }

                    if (svm.footercharges != null)
                    {
                        int PersonCount = (from p in db.JobInvoiceLine
                                           where p.JobInvoiceHeaderId == InvoiceLine.JobInvoiceHeaderId
                                           group p by p.JobWorkerId into g
                                           select g).Count();

                        foreach (var item in svm.footercharges)
                        {

                            if (item.Id > 0)
                            {


                                var footercharge = new JobInvoiceHeaderChargeService(_unitOfWork).Find(item.Id);
                                if (PersonCount > 1 || footercharge.PersonID != InvoiceLine.JobWorkerId)
                                    footercharge.PersonID = null;

                                footercharge.Rate = item.Rate;
                                footercharge.Amount = item.Amount;
                                footercharge.ObjectState = Model.ObjectState.Modified;
                                db.JobInvoiceHeaderCharges.Add(footercharge);
                                //new JobInvoiceHeaderChargeService(_unitOfWork).Update(footercharge);

                            }

                            else
                            {
                                item.HeaderTableId = InvoiceLine.JobInvoiceHeaderId;
                                item.PersonID = InvoiceLine.JobWorkerId;
                                item.ObjectState = Model.ObjectState.Added;
                                db.JobInvoiceHeaderCharges.Add(item);
                                //new JobInvoiceHeaderChargeService(_unitOfWork).Create(item);
                            }
                        }

                    }

                    if (InvoiceHeader.Status != (int)StatusConstants.Drafted && InvoiceHeader.Status != (int)StatusConstants.Import)
                    {
                        InvoiceHeader.Status = (int)StatusConstants.Modified;
                        InvoiceHeader.ModifiedBy = User.Identity.Name;
                        InvoiceHeader.ModifiedDate = DateTime.Now;
                        ReceiveHeader.ModifiedBy = User.Identity.Name;
                        ReceiveHeader.ModifiedDate = DateTime.Now;
                    }


                    //new JobInvoiceHeaderService(_unitOfWork).Update(InvoiceHeader);
                    //new JobReceiveHeaderService(_unitOfWork).Update(ReceiveHeader);

                    InvoiceHeader.ObjectState = Model.ObjectState.Modified;
                    ReceiveHeader.ObjectState = Model.ObjectState.Modified;

                    db.JobInvoiceHeader.Add(InvoiceHeader);
                    db.JobReceiveHeader.Add(ReceiveHeader);

                    try
                    {
                        JobInvoiceReceiveDocEvents.onLineSaveEvent(this, new JobEventArgs(InvoiceLine.JobInvoiceHeaderId, InvoiceLine.JobInvoiceLineId, EventModeConstants.Add), ref db);
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
                        //_unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXCL"] += message;
                        PrepareViewBag(null);
                        ViewBag.DocNo = InvoiceHeader.DocNo;
                        return PartialView("_Create", svm);
                    }

                    try
                    {
                        JobInvoiceReceiveDocEvents.afterLineSaveEvent(this, new JobEventArgs(InvoiceLine.JobInvoiceHeaderId, InvoiceLine.JobInvoiceLineId, EventModeConstants.Add), ref db);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXCL"] += message;
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = InvoiceHeader.DocTypeId,
                        DocId = InvoiceHeader.JobInvoiceHeaderId,
                        DocLineId = InvoiceLine.JobInvoiceLineId,
                        ActivityType = (int)ActivityTypeContants.Added,
                        DocNo = InvoiceHeader.DocNo,
                        DocDate = InvoiceHeader.DocDate,
                        DocStatus = InvoiceHeader.Status,
                    }));

                    return RedirectToAction("_Create", new { id = InvoiceLine.JobInvoiceHeaderId, JobWorkerId = InvoiceLine.JobWorkerId });
                }
                else
                {

                    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();


                    int status = InvoiceHeader.Status;

                    JobInvoiceLine temp1 = _JobInvoiceLineService.Find(svm.JobInvoiceLineId);
                    JobReceiveLine temprec = _JobReceiveLineService.Find(svm.JobReceiveLineId);



                    JobOrderLine JobOrderLine = new JobOrderLineService(_unitOfWork).Find(ReceiveLine.JobOrderLineId);
                    JobOrderHeader JobOrderHeader = new JobOrderHeaderService(_unitOfWork).Find(JobOrderLine.JobOrderHeaderId);



                    if (temprec.StockId != null)
                    {
                        StockViewModel StockViewModel = new StockViewModel();
                        StockViewModel.StockHeaderId = ReceiveHeader.StockHeaderId ?? 0;
                        StockViewModel.StockId = temprec.StockId ?? 0;
                        StockViewModel.DocHeaderId = temprec.JobReceiveHeaderId;
                        StockViewModel.DocLineId = temprec.JobReceiveLineId;
                        StockViewModel.DocTypeId = ReceiveHeader.DocTypeId;
                        StockViewModel.StockHeaderDocDate = ReceiveHeader.DocDate;
                        StockViewModel.StockDocDate = ReceiveHeader.DocDate;
                        StockViewModel.DocNo = ReceiveHeader.DocNo;
                        StockViewModel.DivisionId = ReceiveHeader.DivisionId;
                        StockViewModel.SiteId = ReceiveHeader.SiteId;
                        StockViewModel.CurrencyId = null;
                        StockViewModel.HeaderProcessId = ReceiveHeader.ProcessId;
                        StockViewModel.PersonId = ReceiveHeader.JobWorkerId;
                        StockViewModel.ProductId = JobOrderLine.ProductId;
                        StockViewModel.HeaderFromGodownId = null;
                        StockViewModel.HeaderGodownId = ReceiveHeader.GodownId;
                        StockViewModel.GodownId = ReceiveHeader.GodownId;
                        StockViewModel.ProcessId = ReceiveHeader.ProcessId;
                        StockViewModel.LotNo = JobOrderLine.LotNo;
                        StockViewModel.CostCenterId = JobOrderHeader.CostCenterId;
                        StockViewModel.Qty_Iss = 0;
                        StockViewModel.Qty_Rec = temprec.Qty;
                        StockViewModel.Rate = JobOrderLine.Rate;
                        StockViewModel.ExpiryDate = null;
                        StockViewModel.Specification = JobOrderLine.Specification;
                        StockViewModel.Dimension1Id = JobOrderLine.Dimension1Id;
                        StockViewModel.Dimension2Id = JobOrderLine.Dimension2Id;
                        StockViewModel.Dimension3Id = JobOrderLine.Dimension3Id;
                        StockViewModel.Dimension4Id = JobOrderLine.Dimension4Id;
                        StockViewModel.HeaderRemark = ReceiveHeader.Remark;
                        StockViewModel.Remark = svm.Remark;
                        StockViewModel.ProductUidId = temprec.ProductUidId;
                        StockViewModel.Status = ReceiveHeader.Status;
                        StockViewModel.CreatedBy = temprec.CreatedBy;
                        StockViewModel.CreatedDate = temprec.CreatedDate;
                        StockViewModel.ModifiedBy = User.Identity.Name;
                        StockViewModel.ModifiedDate = DateTime.Now;

                        string StockPostingError = "";
                        StockPostingError = new StockService(_unitOfWork).StockPostDB(ref StockViewModel, ref db);

                        if (StockPostingError != "")
                        {
                            ModelState.AddModelError("", StockPostingError);
                            return PartialView("_Create", svm);
                        }
                    }




                    if (temprec.StockProcessId != null)
                    {
                        StockProcessViewModel StockProcessViewModel = new StockProcessViewModel();
                        StockProcessViewModel.StockHeaderId = ReceiveHeader.StockHeaderId ?? 0;
                        StockProcessViewModel.StockProcessId = temprec.StockProcessId ?? 0;
                        StockProcessViewModel.DocHeaderId = temprec.JobReceiveHeaderId;
                        StockProcessViewModel.DocLineId = temprec.JobReceiveLineId;
                        StockProcessViewModel.DocTypeId = ReceiveHeader.DocTypeId;
                        StockProcessViewModel.StockHeaderDocDate = ReceiveHeader.DocDate;
                        StockProcessViewModel.StockProcessDocDate = ReceiveHeader.DocDate;
                        StockProcessViewModel.DocNo = ReceiveHeader.DocNo;
                        StockProcessViewModel.DivisionId = ReceiveHeader.DivisionId;
                        StockProcessViewModel.SiteId = ReceiveHeader.SiteId;
                        StockProcessViewModel.CurrencyId = null;
                        StockProcessViewModel.HeaderProcessId = ReceiveHeader.ProcessId;
                        StockProcessViewModel.PersonId = ReceiveHeader.JobWorkerId;
                        StockProcessViewModel.ProductId = JobOrderLine.ProductId;
                        StockProcessViewModel.HeaderFromGodownId = null;
                        StockProcessViewModel.HeaderGodownId = ReceiveHeader.GodownId;
                        StockProcessViewModel.GodownId = ReceiveHeader.GodownId;
                        StockProcessViewModel.ProcessId = ReceiveHeader.ProcessId;
                        StockProcessViewModel.LotNo = JobOrderLine.LotNo;
                        StockProcessViewModel.CostCenterId = JobOrderHeader.CostCenterId;
                        StockProcessViewModel.Qty_Iss = svm.ReceiveQty + svm.LossQty;
                        StockProcessViewModel.Qty_Rec = 0;
                        StockProcessViewModel.Rate = JobOrderLine.Rate;
                        StockProcessViewModel.ExpiryDate = null;
                        StockProcessViewModel.Specification = JobOrderLine.Specification;
                        StockProcessViewModel.Dimension1Id = JobOrderLine.Dimension1Id;
                        StockProcessViewModel.Dimension2Id = JobOrderLine.Dimension2Id;
                        StockProcessViewModel.Dimension3Id = JobOrderLine.Dimension3Id;
                        StockProcessViewModel.Dimension4Id = JobOrderLine.Dimension4Id;
                        StockProcessViewModel.HeaderRemark = ReceiveHeader.Remark;
                        StockProcessViewModel.Remark = svm.Remark;
                        StockProcessViewModel.ProductUidId = temprec.ProductUidId;
                        StockProcessViewModel.Status = ReceiveHeader.Status;
                        StockProcessViewModel.CreatedBy = temprec.CreatedBy;
                        StockProcessViewModel.CreatedDate = temprec.CreatedDate;
                        StockProcessViewModel.ModifiedBy = User.Identity.Name;
                        StockProcessViewModel.ModifiedDate = DateTime.Now;

                        string StockProcessPostingError = "";
                        StockProcessPostingError = new StockProcessService(_unitOfWork).StockProcessPostDB(ref StockProcessViewModel, ref db);

                        if (StockProcessPostingError != "")
                        {
                            ModelState.AddModelError("", StockProcessPostingError);
                            return PartialView("_Create", svm);
                        }
                    }

                    temprec.PenaltyAmt = svm.PenaltyAmt;
                    temprec.PenaltyRate = svm.PenaltyRate;
                    temprec.IncentiveRate = svm.IncentiveRate;
                    temprec.Remark = svm.Remark;
                    temprec.UnitConversionMultiplier = svm.UnitConversionMultiplier;
                    temprec.DealUnitId = svm.DealUnitId;
                    temprec.DealQty = svm.DealQty;
                    temprec.Qty = svm.ReceiveQty;
                    temprec.LossQty = svm.LossQty;
                    temprec.PassQty = svm.PassQty;
                    temprec.Weight = svm.Weight;
                    temprec.IncentiveAmt = svm.IncentiveAmt;
                    temprec.PenaltyAmt = svm.PenaltyAmt;
                    temprec.ModifiedDate = DateTime.Now;
                    temprec.ModifiedBy = User.Identity.Name;


                    temprec.ObjectState = Model.ObjectState.Modified;
                    db.JobReceiveLine.Add(temprec);


                    //_JobReceiveLineService.Update(temprec);








                    #region BomPost

                    //Saving BOMPOST Data
                    //Saving BOMPOST Data
                    //Saving BOMPOST Data
                    //Saving BOMPOST Data

                    if (!string.IsNullOrEmpty(svm.JobInvoiceSettings.SqlProcConsumption))
                    {
                        IEnumerable<JobReceiveBom> OldBomList = new JobReceiveBomService(_unitOfWork).GetBomForLine(temprec.JobReceiveLineId);

                        foreach (var item in OldBomList)
                        {
                            if (item.StockProcessId != null)
                            {
                                new StockProcessService(_unitOfWork).Delete((int)item.StockProcessId);
                            }
                            new JobReceiveBomService(_unitOfWork).Delete(item.JobReceiveBomId);
                        }


                        var BomPostList = _JobReceiveLineService.GetBomPostingDataForWeaving(svm.ProductId, svm.Dimension1Id, svm.Dimension2Id, null, null, ReceiveHeader.ProcessId, ReceiveLine.PassQty, ReceiveHeader.DocTypeId, svm.JobInvoiceSettings.SqlProcConsumption, svm.JobOrderLineId, temprec.Weight).ToList();

                        foreach (var item in BomPostList)
                        {
                            JobReceiveBom BomPost = new JobReceiveBom();
                            BomPost.JobReceiveHeaderId = ReceiveHeader.JobReceiveHeaderId;
                            BomPost.JobReceiveLineId = temprec.JobReceiveLineId;
                            BomPost.CreatedBy = User.Identity.Name;
                            BomPost.CreatedDate = DateTime.Now;
                            BomPost.ModifiedBy = User.Identity.Name;
                            BomPost.ModifiedDate = DateTime.Now;
                            BomPost.ProductId = item.ProductId;
                            BomPost.Qty = Convert.ToDecimal(item.Qty);



                            StockProcessViewModel StockProcessBomViewModel = new StockProcessViewModel();
                            if (ReceiveHeader.StockHeaderId != null && ReceiveHeader.StockHeaderId != 0)//If Transaction Header Table Has Stock Header Id Then It will Save Here.
                            {
                                StockProcessBomViewModel.StockHeaderId = (int)ReceiveHeader.StockHeaderId;
                            }
                            else if (svm.JobInvoiceSettings.isPostedInStock)//If Stok Header is already posted during stock posting then this statement will Execute.So theat Stock Header will not generate again.
                            {
                                StockProcessBomViewModel.StockHeaderId = -1;
                            }
                            else//If function will only post in stock process then this statement will execute.For Example Job consumption.
                            {
                                StockProcessBomViewModel.StockHeaderId = 0;
                            }
                            StockProcessBomViewModel.DocHeaderId = ReceiveHeader.JobReceiveHeaderId;
                            StockProcessBomViewModel.DocLineId = temprec.JobReceiveLineId;
                            StockProcessBomViewModel.DocTypeId = ReceiveHeader.DocTypeId;
                            StockProcessBomViewModel.StockHeaderDocDate = ReceiveHeader.DocDate;
                            StockProcessBomViewModel.StockProcessDocDate = ReceiveHeader.DocDate;
                            StockProcessBomViewModel.DocNo = ReceiveHeader.DocNo;
                            StockProcessBomViewModel.DivisionId = ReceiveHeader.DivisionId;
                            StockProcessBomViewModel.SiteId = ReceiveHeader.SiteId;
                            StockProcessBomViewModel.CurrencyId = null;
                            StockProcessBomViewModel.HeaderProcessId = null;
                            StockProcessBomViewModel.PersonId = ReceiveHeader.JobWorkerId;
                            StockProcessBomViewModel.ProductId = item.ProductId;
                            StockProcessBomViewModel.HeaderFromGodownId = null;
                            StockProcessBomViewModel.HeaderGodownId = null;
                            StockProcessBomViewModel.GodownId = ReceiveHeader.GodownId;
                            StockProcessBomViewModel.ProcessId = ReceiveHeader.ProcessId;
                            StockProcessBomViewModel.LotNo = temprec.LotNo;
                            StockProcessBomViewModel.CostCenterId = JobOrderHeader.CostCenterId;
                            StockProcessBomViewModel.Qty_Iss = item.Qty;
                            StockProcessBomViewModel.Qty_Rec = 0;
                            StockProcessBomViewModel.Rate = 0;
                            StockProcessBomViewModel.ExpiryDate = null;
                            StockProcessBomViewModel.Specification = null;
                            StockProcessBomViewModel.Dimension1Id = null;
                            StockProcessBomViewModel.Dimension2Id = null;
                            StockProcessBomViewModel.Dimension3Id = null;
                            StockProcessBomViewModel.Dimension4Id = null;
                            StockProcessBomViewModel.Remark = null;
                            StockProcessBomViewModel.Status = ReceiveHeader.Status;
                            StockProcessBomViewModel.CreatedBy = User.Identity.Name;
                            StockProcessBomViewModel.CreatedDate = DateTime.Now;
                            StockProcessBomViewModel.ModifiedBy = User.Identity.Name;
                            StockProcessBomViewModel.ModifiedDate = DateTime.Now;

                            string StockProcessPostingError = "";
                            StockProcessPostingError = new StockProcessService(_unitOfWork).StockProcessPostDB(ref StockProcessBomViewModel, ref db);

                            if (StockProcessPostingError != "")
                            {
                                ModelState.AddModelError("", StockProcessPostingError);
                                return PartialView("_Create", svm);
                            }

                            BomPost.StockProcessId = StockProcessBomViewModel.StockProcessId;

                            BomPost.ObjectState = Model.ObjectState.Added;
                            //new JobReceiveBomService(_unitOfWork).Create(BomPost);
                            db.JobReceiveBom.Add(BomPost);

                            if (svm.JobInvoiceSettings.isPostedInStock == false && svm.JobInvoiceSettings.isPostedInStockProcess == false)
                            {
                                if (ReceiveHeader.StockHeaderId == null)
                                {
                                    ReceiveHeader.StockHeaderId = StockProcessBomViewModel.StockHeaderId;
                                }
                            }
                        }
                    }

                    #endregion





                    temp1.Amount = svm.Amount;
                    temp1.JobReceiveLineId = svm.JobReceiveLineId;
                    temp1.UnitConversionMultiplier = svm.UnitConversionMultiplier;
                    temp1.Qty = svm.PassQty;
                    temp1.DealQty = svm.DealQty;
                    temp1.DealUnitId = svm.DealUnitId;
                    temp1.Remark = svm.Remark;
                    temp1.Rate = svm.Rate;
                    temp1.IncentiveRate = temprec.IncentiveRate;
                    temp1.IncentiveAmt = temprec.IncentiveAmt;
                    temp1.ModifiedDate = DateTime.Now;
                    temp1.ModifiedBy = User.Identity.Name;
                    temp1.ObjectState = Model.ObjectState.Modified;
                    db.JobInvoiceLine.Add(temp1);

                    new JobOrderLineStatusService(_unitOfWork).UpdateJobQtyOnInvoiceReceive(temprec.JobOrderLineId, temp1.JobInvoiceLineId, InvoiceHeader.DocDate, (temprec.Qty + temprec.LossQty), temp1.UnitConversionMultiplier, ref db, true);

                    if (InvoiceHeader.Status != (int)StatusConstants.Drafted && InvoiceHeader.Status != (int)StatusConstants.Import)
                    {
                        InvoiceHeader.Status = (int)StatusConstants.Modified;
                        InvoiceHeader.ModifiedBy = User.Identity.Name;
                        InvoiceHeader.ModifiedDate = DateTime.Now;
                        ReceiveHeader.ModifiedBy = User.Identity.Name;
                        ReceiveHeader.ModifiedDate = DateTime.Now;
                    }

                    InvoiceHeader.ObjectState = Model.ObjectState.Modified;
                    ReceiveHeader.ObjectState = Model.ObjectState.Modified;

                    db.JobInvoiceHeader.Add(InvoiceHeader);
                    db.JobReceiveHeader.Add(ReceiveHeader);


                    JobInvoiceLine ExRec = new JobInvoiceLine();
                    ExRec = Mapper.Map<JobInvoiceLine>(temp1);

                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = ExRec,
                        Obj = temp1,
                    });


                    JobReceiveLine ExRecLine = new JobReceiveLine();
                    ExRecLine = Mapper.Map<JobReceiveLine>(temprec);
                    JobReceiveLine RecLine = new JobReceiveLine();

                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = ExRecLine,
                        Obj = temprec,
                    });



                    if (svm.linecharges != null)
                        foreach (var item in svm.linecharges)
                        {
                            var productcharge = new JobInvoiceLineChargeService(_unitOfWork).Find(item.Id);

                            productcharge.Rate = item.Rate;
                            productcharge.Amount = item.Amount;
                            productcharge.DealQty = item.DealQty;
                            productcharge.ObjectState = Model.ObjectState.Modified;
                            db.JobInvoiceLineCharge.Add(productcharge);
                            //new JobInvoiceLineChargeService(_unitOfWork).Update(productcharge);

                        }


                    if (svm.footercharges != null)
                        foreach (var item in svm.footercharges)
                        {
                            var footercharge = new JobInvoiceHeaderChargeService(_unitOfWork).Find(item.Id);

                            footercharge.Rate = item.Rate;
                            footercharge.Amount = item.Amount;

                            footercharge.ObjectState = Model.ObjectState.Modified;
                            db.JobInvoiceHeaderCharges.Add(footercharge);
                            //new JobInvoiceHeaderChargeService(_unitOfWork).Update(footercharge);

                        }

                    XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);

                    try
                    {
                        JobInvoiceReceiveDocEvents.onLineSaveEvent(this, new JobEventArgs(temp1.JobInvoiceHeaderId, temp1.JobInvoiceLineId, EventModeConstants.Edit), ref db);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXCL"] += message;
                        EventException = true;
                    }

                    try
                    {
                        db.SaveChanges();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXCL"] += message;
                        PrepareViewBag(null);
                        ViewBag.DocNo = InvoiceHeader.DocNo;
                        return PartialView("_Create", svm);
                    }

                    try
                    {
                        JobInvoiceReceiveDocEvents.afterLineSaveEvent(this, new JobEventArgs(temp1.JobInvoiceHeaderId, temp1.JobInvoiceLineId, EventModeConstants.Edit), ref db);
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                    }

                    //Saving the Activity Log

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = InvoiceHeader.DocTypeId,
                        DocId = InvoiceHeader.JobInvoiceHeaderId,
                        DocLineId = temp1.JobInvoiceLineId,
                        ActivityType = (int)ActivityTypeContants.Modified,
                        DocNo = InvoiceHeader.DocNo,
                        DocDate = InvoiceHeader.DocDate,
                        xEModifications = Modifications,
                        DocStatus = InvoiceHeader.Status,
                    }));

                    //End of Saving the Activity Log

                    return Json(new { success = true });

                }
            }
            PrepareViewBag(svm);
            ViewBag.DocNo = InvoiceHeader.DocNo;
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

        [HttpGet]
        private ActionResult _Modify(int id)
        {
            JobInvoiceLineViewModel temp = _JobInvoiceLineService.GetJobInvoiceReceiveLine(id);

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

            JobInvoiceHeader H = new JobInvoiceHeaderService(_unitOfWork).Find(temp.JobInvoiceHeaderId);

            //Getting Settings
            var settings = new JobInvoiceSettingsService(_unitOfWork).GetJobInvoiceSettingsForDocument(H.DocTypeId, H.DivisionId, H.SiteId);


            temp.JobInvoiceSettings = Mapper.Map<JobInvoiceSettings, JobInvoiceSettingsViewModel>(settings);

            temp.DocumentTypeSettings = new DocumentTypeSettingsService(_unitOfWork).GetDocumentTypeSettingsForDocument(H.DocTypeId);

            PrepareViewBag(temp);


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
            JobInvoiceLineViewModel temp = _JobInvoiceLineService.GetJobInvoiceReceiveLine(id);

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

            JobInvoiceHeader H = new JobInvoiceHeaderService(_unitOfWork).Find(temp.JobInvoiceHeaderId);
            //Getting Settings
            var settings = new JobInvoiceSettingsService(_unitOfWork).GetJobInvoiceSettingsForDocument(H.DocTypeId, H.DivisionId, H.SiteId);

            temp.JobInvoiceSettings = Mapper.Map<JobInvoiceSettings, JobInvoiceSettingsViewModel>(settings);
            temp.DocumentTypeSettings = new DocumentTypeSettingsService(_unitOfWork).GetDocumentTypeSettingsForDocument(H.DocTypeId);

            PrepareViewBag(temp);

            return PartialView("_Create", temp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]        
        public ActionResult DeletePost(JobInvoiceLineViewModel vm)
        {
            #region BeforeSave
            bool BeforeSave = true;
            try
            {
                BeforeSave = JobInvoiceReceiveDocEvents.beforeLineDeleteEvent(this, new JobEventArgs(vm.JobInvoiceHeaderId, vm.JobInvoiceLineId), ref db);
            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                EventException = true;
            }

            if (!BeforeSave)
                TempData["CSEXC"] += "Validation failed before delete.";
            #endregion

            if (BeforeSave && !EventException)
            {

                int? StockId = 0;
                int? StockProcessId = 0;
                List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();


                JobInvoiceLine JobInvoiceLine = (from p in db.JobInvoiceLine
                                                 where p.JobInvoiceLineId == vm.JobInvoiceLineId
                                                 select p).FirstOrDefault();


                JobReceiveLine JobReceiveLine = (from p in db.JobReceiveLine
                                                 where p.JobReceiveLineId == JobInvoiceLine.JobReceiveLineId
                                                 select p).FirstOrDefault();

                JobReceiveLineStatus LineStatus = (from p in db.JobReceiveLineStatus
                                                   where p.JobReceiveLineId == JobReceiveLine.JobReceiveLineId
                                                   select p).FirstOrDefault();

                LineStatus.ObjectState = Model.ObjectState.Deleted;
                db.JobReceiveLineStatus.Remove(LineStatus);

                JobInvoiceHeader header = new JobInvoiceHeaderService(_unitOfWork).Find(JobInvoiceLine.JobInvoiceHeaderId);
                JobReceiveHeader Receiveheader = new JobReceiveHeaderService(_unitOfWork).Find(JobReceiveLine.JobReceiveHeaderId);

                LogList.Add(new LogTypeViewModel
                {
                    ExObj = Mapper.Map<JobInvoiceLine>(JobInvoiceLine),
                });

                LogList.Add(new LogTypeViewModel
                {
                    ExObj = Mapper.Map<JobReceiveLine>(JobReceiveLine),
                });

                new JobOrderLineStatusService(_unitOfWork).UpdateJobQtyOnInvoiceReceive(JobReceiveLine.JobOrderLineId, JobInvoiceLine.JobInvoiceLineId, header.DocDate, 0, JobInvoiceLine.UnitConversionMultiplier, ref db, true);

                JobInvoiceLineStatus Status = new JobInvoiceLineStatus();
                Status.JobInvoiceLineId = JobInvoiceLine.JobInvoiceLineId;
                db.JobInvoiceLineStatus.Attach(Status);
                Status.ObjectState = Model.ObjectState.Deleted;

                db.JobInvoiceLineStatus.Remove(Status);

                //_JobInvoiceLineService.Delete(JobInvoiceLine);
                JobInvoiceLine.ObjectState = Model.ObjectState.Deleted;
                db.JobInvoiceLine.Remove(JobInvoiceLine);

                var chargeslist = (from p in db.JobInvoiceLineCharge
                                   where p.LineTableId == vm.JobInvoiceLineId
                                   select p).ToList();

                if (chargeslist != null)
                    foreach (var item in chargeslist)
                    {
                        item.ObjectState = Model.ObjectState.Deleted;
                        db.JobInvoiceLineCharge.Remove(item);
                        //new JobInvoiceLineChargeService(_unitOfWork).Delete(item.Id);
                    }

                if (vm.footercharges != null)
                    foreach (var item in vm.footercharges)
                    {
                        var footer = new JobInvoiceHeaderChargeService(_unitOfWork).Find(item.Id);
                        footer.Rate = item.Rate;
                        footer.Amount = item.Amount;

                        footer.ObjectState = Model.ObjectState.Modified;
                        db.JobInvoiceHeaderCharges.Add(footer);
                        //new JobInvoiceHeaderChargeService(_unitOfWork).Update(footer);
                    }



                StockId = JobReceiveLine.StockId;
                StockProcessId = JobReceiveLine.StockProcessId;

                if (vm.ProductUidId != null && vm.ProductUidId != 0)
                {
                    ProductUid ProductUid = (from p in db.ProductUid
                                             where p.ProductUIDId == vm.ProductUidId
                                             select p).FirstOrDefault();

                    if (!(JobReceiveLine.ProductUidLastTransactionDocNo == ProductUid.LastTransactionDocNo && JobReceiveLine.ProductUidLastTransactionDocTypeId == ProductUid.LastTransactionDocTypeId) || header.SiteId == 17)
                    {


                        if ((Receiveheader.DocNo != ProductUid.LastTransactionDocNo || Receiveheader.DocTypeId != ProductUid.LastTransactionDocTypeId))
                        {
                            ModelState.AddModelError("", "Bar Code Can't be deleted because this is already Proceed to another process.");
                            PrepareViewBag(vm);
                            return PartialView("_Create", vm);
                        }


                        ProductUid.LastTransactionDocDate = JobReceiveLine.ProductUidLastTransactionDocDate;
                        ProductUid.LastTransactionDocId = JobReceiveLine.ProductUidLastTransactionDocId;
                        ProductUid.LastTransactionDocNo = JobReceiveLine.ProductUidLastTransactionDocNo;
                        ProductUid.LastTransactionDocTypeId = JobReceiveLine.ProductUidLastTransactionDocTypeId;
                        ProductUid.LastTransactionPersonId = JobReceiveLine.ProductUidLastTransactionPersonId;
                        ProductUid.CurrenctGodownId = JobReceiveLine.ProductUidCurrentGodownId;
                        ProductUid.CurrenctProcessId = JobReceiveLine.ProductUidCurrentProcessId;
                        ProductUid.Status = JobReceiveLine.ProductUidStatus;

                        ProductUid.ObjectState = Model.ObjectState.Modified;

                        db.ProductUid.Add(ProductUid);

                        new StockUidService(_unitOfWork).DeleteStockUidForDocLineDB(Receiveheader.JobReceiveHeaderId, Receiveheader.DocTypeId, Receiveheader.SiteId, Receiveheader.DivisionId, ref db);

                    }
                }

                JobReceiveLine.ObjectState = Model.ObjectState.Deleted;
                db.JobReceiveLine.Remove(JobReceiveLine);

                if (StockId != null)
                {
                    new StockService(_unitOfWork).DeleteStockDB((int)StockId, ref db, true);
                }

                if (StockProcessId != null)
                {
                    new StockProcessService(_unitOfWork).DeleteStockProcessDB((int)StockProcessId, ref db, true);
                }


                if (header.Status != (int)StatusConstants.Drafted && header.Status != (int)StatusConstants.Import)
                {
                    header.Status = (int)StatusConstants.Modified;
                    header.ModifiedBy = User.Identity.Name;
                    header.ModifiedDate = DateTime.Now;

                    header.ObjectState = Model.ObjectState.Modified;

                    //Receiveheader.Status = (int)StatusConstants.Modified;
                    Receiveheader.ModifiedDate = DateTime.Now;
                    Receiveheader.ModifiedBy = User.Identity.Name;

                    Receiveheader.ObjectState = Model.ObjectState.Modified;

                    db.JobInvoiceHeader.Add(header);
                    db.JobReceiveHeader.Add(Receiveheader);
                }



                var Boms = (from p in db.JobReceiveBom
                            where p.JobReceiveLineId == vm.JobReceiveLineId
                            select p).ToList();

                var StockProcessIds = Boms.Select(m => m.StockProcessId).ToArray();

                var StockProcRecorcds = (from p in db.StockProcess
                                         where StockProcessIds.Contains(p.StockProcessId)
                                         select p).ToList();

                foreach (var item in Boms)
                {

                    if (item.StockProcessId != null)
                    {
                        var TempStockProcess = StockProcRecorcds.Where(m => m.StockProcessId == item.StockProcessId).FirstOrDefault();
                        TempStockProcess.ObjectState = Model.ObjectState.Deleted;
                        db.StockProcess.Remove(TempStockProcess);
                    }

                    item.ObjectState = Model.ObjectState.Deleted;
                    db.JobReceiveBom.Remove(item);
                }


                XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);

                try
                {
                    JobInvoiceReceiveDocEvents.onLineDeleteEvent(this, new JobEventArgs(JobInvoiceLine.JobInvoiceHeaderId, JobInvoiceLine.JobInvoiceLineId), ref db);
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
                    //_unitOfWork.Save();
                }

                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXCL"] += message;
                    PrepareViewBag(null);
                    return PartialView("_Create", vm);
                }

                try
                {
                    JobInvoiceReceiveDocEvents.afterLineDeleteEvent(this, new JobEventArgs(JobInvoiceLine.JobInvoiceHeaderId, JobInvoiceLine.JobInvoiceLineId), ref db);
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                }

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = header.DocTypeId,
                    DocId = header.JobInvoiceHeaderId,
                    DocLineId = JobInvoiceLine.JobInvoiceLineId,
                    ActivityType = (int)ActivityTypeContants.Deleted,
                    DocNo = header.DocNo,
                    DocDate = header.DocDate,
                    xEModifications = Modifications,
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

        public JsonResult GetPendingOrders(int JobWorkerId, string term, int Limit)
        {
            return Json(new JobOrderHeaderService(_unitOfWork).GetPendingJobOrdersWithPatternMatch(JobWorkerId, term, Limit).ToList());
        }

        public JsonResult GetOrderDetail(int OrderId, int InvoiceId)
        {
            return Json(new JobOrderLineService(_unitOfWork).GetLineDetailForInvoice(OrderId, InvoiceId));
        }
        public JsonResult GetProductDetailJson(int ProductId)
        {
            ProductViewModel product = new ProductService(_unitOfWork).GetProduct(ProductId);
            List<Product> ProductJson = new List<Product>();

            ProductJson.Add(new Product()
            {
                ProductId = product.ProductId,
                StandardCost = product.StandardCost,
                UnitId = product.UnitId
            });

            return Json(ProductJson);
        }

        public JsonResult getunitconversiondetailjson(int productid, string unitid, string deliveryunitid)
        {
            UnitConversion uc = new UnitConversionService(_unitOfWork).GetUnitConversion(productid, unitid, deliveryunitid);
            List<SelectListItem> unitconversionjson = new List<SelectListItem>();
            if (uc != null)
            {
                unitconversionjson.Add(new SelectListItem
                {
                    Text = uc.Multiplier.ToString(),
                    Value = uc.Multiplier.ToString()
                });
            }
            else
            {
                unitconversionjson.Add(new SelectListItem
                {
                    Text = "0",
                    Value = "0"
                });
            }


            return Json(unitconversionjson);
        }

        public JsonResult GetPendingJobOrderProducts(string searchTerm, int pageSize, int pageNum, int filter)//DocTypeId
        {

            return new JsonpResult { Data = _JobInvoiceLineService.GetPendingProductsForJobInvoice(searchTerm, pageSize, pageNum, filter), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPendingJobOrders(string searchTerm, int pageSize, int pageNum, int filter)//DocTypeId
        {
            return new JsonpResult { Data = _JobInvoiceLineService.GetPendingJobOrdersForInvoice(searchTerm, pageSize, pageNum, filter), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetCustomProducts(int id, int JobWorkerId, string term, int Limit)//Indent Header ID
        {

            var Header = db.JobInvoiceHeader.Find(id);

            var DocType = db.DocumentType.Where(m => m.DocumentTypeName == TransactionDoctypeConstants.TraceMapInvoice).FirstOrDefault();

            if (Header.DocTypeId == DocType.DocumentTypeId)
            {
                return Json(_JobInvoiceLineService.GetProductHelpListForPendingTraceMapJobOrders(id, JobWorkerId, term, Limit), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(_JobInvoiceLineService.GetProductHelpListForPendingJobOrders(id, JobWorkerId, term, Limit), JsonRequestBehavior.AllowGet);
            }


        }
    }
}
