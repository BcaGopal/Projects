using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Infrastructure;
using Model.Models;
using System.Data.Entity;
using Core.Common;
using System;
using Model;
using System.Threading.Tasks;
using Data.Models;
using Model.ViewModels;
using Model.ViewModel;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using Model.ViewModels;
using AutoMapper;

namespace Service
{
    public interface IWeavingReceiveQACombinedService : IDisposable
    {
        JobReceiveHeader Create(WeavingReceiveQACombinedViewModel pt, string UserName);
        void Delete(int id);
        void Update(WeavingReceiveQACombinedViewModel pt, string UserName);
        Task<IEquatable<JobReceiveQAAttribute>> GetAsync();
        Task<JobReceiveQAAttribute> FindAsync(int id);
        WeavingReceiveQACombinedViewModel GetJobReceiveDetailForEdit(int JobReceiveHeaderId);//JobReceiveHeaderId
        LastValues GetLastValues(int DocTypeId);
        IQueryable<ComboBoxResult> GetCustomProduct(int filter, string term);
    }

    public class WeavingReceiveQACombinedService : IWeavingReceiveQACombinedService
    {
        ApplicationDbContext db;

        IUnitOfWork _unitOfWork;
        public WeavingReceiveQACombinedService(ApplicationDbContext db)
        {

            this.db = db;
        }

        public WeavingReceiveQACombinedService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public JobReceiveQAAttribute Find(int id)
        {
            return db.JobReceiveQAAttribute.Find(id);
        }

        public JobReceiveHeader Create(WeavingReceiveQACombinedViewModel pt, string UserName)
        {
            JobOrderLine JobOrderLine = db.JobOrderLine.Find(pt.JobOrderLineId);
            JobOrderHeader JobOrderHeader = db.JobOrderHeader.Find(JobOrderLine.JobOrderHeaderId);

            JobReceiveHeader JobReceiveHeader = new JobReceiveHeader();
            JobReceiveHeader.DocTypeId = pt.DocTypeId;
            JobReceiveHeader.DocNo = pt.DocNo;
            JobReceiveHeader.DocDate = pt.DocDate;
            JobReceiveHeader.DivisionId = pt.DivisionId;
            JobReceiveHeader.SiteId = pt.SiteId;
            JobReceiveHeader.ProcessId = pt.ProcessId;
            JobReceiveHeader.JobWorkerId = pt.JobWorkerId;
            JobReceiveHeader.JobWorkerDocNo = pt.DocNo;
            JobReceiveHeader.JobReceiveById = pt.JobReceiveById;
            JobReceiveHeader.GodownId = pt.GodownId;
            JobReceiveHeader.Remark = pt.Remark;
            JobReceiveHeader.CreatedBy = UserName;
            JobReceiveHeader.CreatedBy = UserName;
            JobReceiveHeader.CreatedDate = DateTime.Now;
            JobReceiveHeader.ModifiedBy = UserName;
            JobReceiveHeader.ModifiedDate = DateTime.Now;
            JobReceiveHeader.Status = (int)StatusConstants.Drafted;

            StockHeader StockHeader = new StockHeader();
            StockHeader.DocTypeId = JobReceiveHeader.DocTypeId;
            StockHeader.DocDate = JobReceiveHeader.DocDate;
            StockHeader.DocNo = JobReceiveHeader.DocNo;
            StockHeader.DivisionId = JobReceiveHeader.DivisionId;
            StockHeader.SiteId = JobReceiveHeader.SiteId;
            StockHeader.PersonId = JobReceiveHeader.JobWorkerId;
            StockHeader.ProcessId = JobReceiveHeader.ProcessId;
            StockHeader.FromGodownId = null;
            StockHeader.GodownId = JobReceiveHeader.GodownId;
            StockHeader.Remark = JobReceiveHeader.Remark;
            StockHeader.Status = JobReceiveHeader.Status;
            StockHeader.CreatedBy = JobReceiveHeader.CreatedBy;
            StockHeader.CreatedDate = JobReceiveHeader.CreatedDate;
            StockHeader.ModifiedBy = JobReceiveHeader.ModifiedBy;
            StockHeader.ModifiedDate = JobReceiveHeader.ModifiedDate;
            StockHeader.ObjectState = Model.ObjectState.Added;
            db.StockHeader.Add(StockHeader);


            JobReceiveHeader.StockHeaderId = StockHeader.StockHeaderId;
            JobReceiveHeader.ObjectState = Model.ObjectState.Added;
            db.JobReceiveHeader.Add(JobReceiveHeader);

            JobReceiveLine JobReceiveLine = new JobReceiveLine();
            JobReceiveLine.JobReceiveHeaderId = JobReceiveHeader.JobReceiveHeaderId;
            JobReceiveLine.JobOrderLineId = pt.JobOrderLineId;
            JobReceiveLine.ProductUidId = pt.ProductUidId;
            JobReceiveLine.Qty = pt.Qty;
            JobReceiveLine.LossQty = 0;
            JobReceiveLine.PassQty = pt.Qty;
            JobReceiveLine.LotNo = null;
            JobReceiveLine.UnitConversionMultiplier = JobOrderLine.UnitConversionMultiplier;
            JobReceiveLine.DealQty = JobReceiveLine.Qty * JobReceiveLine.UnitConversionMultiplier;
            JobReceiveLine.DealUnitId = pt.DealUnitId;
            JobReceiveLine.LotNo = null;
            JobReceiveLine.Sr = 1;
            JobReceiveLine.CreatedDate = DateTime.Now;
            JobReceiveLine.ModifiedDate = DateTime.Now;
            JobReceiveLine.CreatedBy = UserName;
            JobReceiveLine.ModifiedBy = UserName;

            Dictionary<int, decimal> LineStatus = new Dictionary<int, decimal>();
            LineStatus.Add(JobReceiveLine.JobOrderLineId, (JobReceiveLine.Qty + JobReceiveLine.LossQty));


            ProductUidHeader ProductUidHeader = new ProductUidHeader();
            ProductUidHeader.ProductId = pt.ProductId;
            ProductUidHeader.GenDocId = JobReceiveHeader.JobReceiveHeaderId;
            ProductUidHeader.GenDocNo = JobReceiveHeader.DocNo;
            ProductUidHeader.GenDocTypeId = JobReceiveHeader.DocTypeId;
            ProductUidHeader.GenDocDate = JobReceiveHeader.DocDate;
            ProductUidHeader.GenPersonId = JobReceiveHeader.JobWorkerId;
            ProductUidHeader.CreatedBy = UserName;
            ProductUidHeader.CreatedDate = DateTime.Now;
            ProductUidHeader.ModifiedBy = UserName;
            ProductUidHeader.ModifiedDate = DateTime.Now;
            ProductUidHeader.ObjectState = Model.ObjectState.Added;
            db.ProductUidHeader.Add(ProductUidHeader);



            ProductUid ProductUid = new ProductUid();
            ProductUid.ProductUidHeaderId = ProductUidHeader.ProductUidHeaderId;
            ProductUid.ProductUidName = pt.ProductUidName;
            ProductUid.ProductId = pt.ProductId;
            ProductUid.IsActive = true;
            ProductUid.CreatedBy = UserName;
            ProductUid.CreatedDate = DateTime.Now;
            ProductUid.ModifiedBy = UserName;
            ProductUid.ModifiedDate = DateTime.Now;
            ProductUid.GenLineId = null;
            ProductUid.GenDocId = JobReceiveHeader.JobReceiveHeaderId;
            ProductUid.GenDocNo = JobReceiveHeader.DocNo;
            ProductUid.GenDocTypeId = JobReceiveHeader.DocTypeId;
            ProductUid.GenDocDate = JobReceiveHeader.DocDate;
            ProductUid.GenPersonId = JobReceiveHeader.JobWorkerId;
            ProductUid.CurrenctProcessId = JobReceiveHeader.ProcessId;
            ProductUid.Status = ProductUidStatusConstants.Receive;
            ProductUid.LastTransactionDocId = JobReceiveHeader.JobReceiveHeaderId;
            ProductUid.LastTransactionDocNo = JobReceiveHeader.DocNo;
            ProductUid.LastTransactionDocTypeId = JobReceiveHeader.DocTypeId;
            ProductUid.LastTransactionDocDate = JobReceiveHeader.DocDate;
            ProductUid.LastTransactionPersonId = JobReceiveHeader.JobWorkerId;
            ProductUid.LastTransactionLineId = null;
            ProductUid.ObjectState = Model.ObjectState.Added;
            db.ProductUid.Add(ProductUid);


            Stock Stock = new Stock();
            Stock.DocDate = JobReceiveHeader.DocDate;
            Stock.ProductUidId = ProductUid.ProductUIDId;
            Stock.ProductId = pt.ProductId;
            Stock.ProcessId = JobReceiveHeader.ProcessId;
            Stock.GodownId = JobReceiveHeader.GodownId;
            Stock.LotNo = JobReceiveLine.LotNo;
            Stock.ProductUidId = JobReceiveLine.ProductUidId;
            Stock.CostCenterId = JobOrderHeader.CostCenterId;
            Stock.Qty_Iss = 0;
            Stock.Qty_Rec = JobReceiveLine.Qty;
            Stock.Remark = JobReceiveLine.Remark;
            Stock.CreatedBy = JobReceiveLine.CreatedBy;
            Stock.CreatedDate = JobReceiveLine.CreatedDate;
            Stock.ModifiedBy = JobReceiveLine.ModifiedBy;
            Stock.ModifiedDate = JobReceiveLine.ModifiedDate;
            Stock.ObjectState = Model.ObjectState.Added;
            db.Stock.Add(Stock);

            JobReceiveLine.ProductUidHeaderId = ProductUidHeader.ProductUidHeaderId;
            JobReceiveLine.ProductUidId = ProductUid.ProductUIDId;
            JobReceiveLine.StockId = Stock.StockId;
            JobReceiveLine.ObjectState = Model.ObjectState.Added;
            db.JobReceiveLine.Add(JobReceiveLine);

            if (pt.Rate != pt.XRate)
            {
                JobOrderLine.Rate = pt.Rate;
                JobOrderLine.ObjectState = ObjectState.Modified;
                db.JobOrderLine.Add(JobOrderLine);
            }


            JobReceiveQAHeader JobReceiveQAHeader = new JobReceiveQAHeader();
            JobReceiveQAHeader.DocTypeId = JobReceiveHeader.DocTypeId;
            JobReceiveQAHeader.DocDate = JobReceiveHeader.DocDate;
            JobReceiveQAHeader.DocNo = JobReceiveHeader.DocNo;
            JobReceiveQAHeader.DivisionId = JobReceiveHeader.DivisionId;
            JobReceiveQAHeader.SiteId = JobReceiveHeader.SiteId;
            JobReceiveQAHeader.ProcessId = JobReceiveHeader.ProcessId;
            JobReceiveQAHeader.JobWorkerId = JobReceiveHeader.JobWorkerId;
            JobReceiveQAHeader.QAById = JobReceiveHeader.JobReceiveById;
            JobReceiveQAHeader.Remark = JobReceiveHeader.Remark;
            JobReceiveQAHeader.Status = JobReceiveHeader.Status;
            JobReceiveQAHeader.CreatedBy = JobReceiveHeader.CreatedBy;
            JobReceiveQAHeader.CreatedDate = JobReceiveHeader.CreatedDate;
            JobReceiveQAHeader.ModifiedBy = JobReceiveHeader.ModifiedBy;
            JobReceiveQAHeader.ModifiedDate = JobReceiveHeader.ModifiedDate;
            JobReceiveQAHeader.ObjectState = Model.ObjectState.Added;
            db.JobReceiveQAHeader.Add(JobReceiveQAHeader);


            JobReceiveQALine JobReceiveQALine = new JobReceiveQALine();
            JobReceiveQALine.JobReceiveQAHeaderId = JobReceiveQAHeader.JobReceiveQAHeaderId;
            JobReceiveQALine.Sr = JobReceiveLine.Sr;
            JobReceiveQALine.ProductUidId = JobReceiveLine.ProductUidId;
            JobReceiveQALine.JobReceiveLineId = JobReceiveLine.JobReceiveLineId;
            JobReceiveQALine.QAQty = JobReceiveLine.Qty;
            JobReceiveQALine.InspectedQty = JobReceiveLine.Qty;
            JobReceiveQALine.Qty = JobReceiveLine.Qty;
            JobReceiveQALine.FailQty = 0;
            JobReceiveQALine.UnitConversionMultiplier = pt.UnitConversionMultiplier;
            JobReceiveQALine.DealQty = pt.DealQty;
            JobReceiveQALine.FailDealQty = 0;
            JobReceiveQALine.Weight = JobReceiveLine.Weight;
            JobReceiveQALine.PenaltyRate = JobReceiveLine.PenaltyRate;
            JobReceiveQALine.PenaltyAmt = JobReceiveLine.PenaltyAmt;
            JobReceiveQALine.Remark = JobReceiveLine.Remark;
            JobReceiveQALine.CreatedBy = JobReceiveLine.CreatedBy;
            JobReceiveQALine.CreatedDate = JobReceiveLine.CreatedDate;
            JobReceiveQALine.ModifiedBy = JobReceiveLine.ModifiedBy;
            JobReceiveQALine.ModifiedDate = JobReceiveLine.ModifiedDate;
            JobReceiveQALine.ObjectState = Model.ObjectState.Added;
            db.JobReceiveQALine.Add(JobReceiveQALine);


            JobReceiveQALineExtended JobReceiveQALineExtended = new JobReceiveQALineExtended();
            JobReceiveQALineExtended.JobReceiveQALineId = JobReceiveQALine.JobReceiveQALineId;
            JobReceiveQALineExtended.Length = pt.Length;
            JobReceiveQALineExtended.Width = pt.Width;
            JobReceiveQALineExtended.Height = pt.Height;
            JobReceiveQALineExtended.ObjectState = ObjectState.Added;
            db.JobReceiveQALineExtended.Add(JobReceiveQALineExtended);


            List<QAGroupLineLineViewModel> QAGroupLineList = pt.QAGroupLine;

            if (QAGroupLineList != null)
            {
                foreach (var item in QAGroupLineList)
                {
                    JobReceiveQAAttribute JobReceiveQAAttribute = new JobReceiveQAAttribute();
                    JobReceiveQAAttribute.JobReceiveQALineId = JobReceiveQALine.JobReceiveQALineId;
                    JobReceiveQAAttribute.QAGroupLineId = item.QAGroupLineId;
                    JobReceiveQAAttribute.Value = item.Value;
                    JobReceiveQAAttribute.Remark = item.Remarks;
                    JobReceiveQAAttribute.CreatedBy = UserName;
                    JobReceiveQAAttribute.ModifiedBy = UserName;
                    JobReceiveQAAttribute.CreatedDate = DateTime.Now;
                    JobReceiveQAAttribute.ModifiedDate = DateTime.Now;
                    JobReceiveQAAttribute.ObjectState = ObjectState.Added;
                    db.JobReceiveQAAttribute.Add(JobReceiveQAAttribute);
                }
            }

            return JobReceiveHeader;
        }

        public void Update(WeavingReceiveQACombinedViewModel pt, string UserName)
        {
            JobReceiveHeader JobReceiveHeader = db.JobReceiveHeader.Find(pt.JobReceiveHeaderId);
            JobReceiveHeader.DocNo = pt.DocNo;
            JobReceiveHeader.DocDate = pt.DocDate;
            JobReceiveHeader.JobWorkerId = pt.JobWorkerId;
            JobReceiveHeader.JobWorkerDocNo = pt.DocNo;
            JobReceiveHeader.JobReceiveById = pt.JobReceiveById;
            JobReceiveHeader.GodownId = pt.GodownId;
            JobReceiveHeader.Remark = pt.Remark;
            JobReceiveHeader.ModifiedBy = UserName;
            JobReceiveHeader.ModifiedDate = DateTime.Now;
            JobReceiveHeader.ObjectState = ObjectState.Modified;
            db.JobReceiveHeader.Add(JobReceiveHeader);

            StockHeader StockHeader = db.StockHeader.Find(JobReceiveHeader.StockHeaderId);
            StockHeader.DocNo = pt.DocNo;
            StockHeader.DocDate = pt.DocDate;
            StockHeader.PersonId = pt.JobWorkerId;
            StockHeader.GodownId = pt.GodownId;
            StockHeader.Remark = pt.Remark;
            StockHeader.ModifiedBy = UserName;
            StockHeader.ModifiedDate = DateTime.Now;
            StockHeader.ObjectState = ObjectState.Modified;
            db.JobReceiveHeader.Add(JobReceiveHeader);

            JobReceiveQAHeader JobReceiveQAHeader = db.JobReceiveQAHeader.Find(pt.JobReceiveQAHeaderId);
            JobReceiveQAHeader.DocDate = JobReceiveHeader.DocDate;
            JobReceiveQAHeader.DocNo = JobReceiveHeader.DocNo;
            JobReceiveQAHeader.QAById = JobReceiveHeader.JobReceiveById;
            JobReceiveQAHeader.Remark = JobReceiveHeader.Remark;
            JobReceiveQAHeader.ModifiedBy = UserName;
            JobReceiveQAHeader.ModifiedDate = DateTime.Now;
            JobReceiveQAHeader.ObjectState = ObjectState.Modified;
            db.JobReceiveQAHeader.Add(JobReceiveQAHeader);


            JobReceiveLine JobReceiveLine = db.JobReceiveLine.Find(pt.JobReceiveLineId);
            JobReceiveLine.JobOrderLineId = pt.JobOrderLineId;
            JobReceiveLine.ProductUidId = pt.ProductUidId;
            JobReceiveLine.Qty = pt.Qty;
            JobReceiveLine.LossQty = 0;
            JobReceiveLine.PassQty = pt.Qty;
            JobReceiveLine.LotNo = null;
            JobReceiveLine.UnitConversionMultiplier = pt.UnitConversionMultiplier;
            JobReceiveLine.DealQty = pt.DealQty;
            JobReceiveLine.DealUnitId = pt.DealUnitId;
            JobReceiveLine.LotNo = null;
            JobReceiveLine.Sr = 1;
            JobReceiveLine.ModifiedBy = UserName;
            JobReceiveLine.ModifiedDate = DateTime.Now;
            JobReceiveLine.ObjectState = ObjectState.Modified;
            db.JobReceiveLine.Add(JobReceiveLine);


            JobReceiveQALine JobReceiveQALine = db.JobReceiveQALine.Find(pt.JobReceiveQALineId);
            JobReceiveQALine.Weight = JobReceiveLine.Weight;
            JobReceiveQALine.UnitConversionMultiplier = JobReceiveLine.UnitConversionMultiplier;
            JobReceiveQALine.DealQty = JobReceiveLine.DealQty;
            JobReceiveQALine.FailQty = 0;
            JobReceiveQALine.FailDealQty = 0;
            JobReceiveQALine.ModifiedBy = UserName;
            JobReceiveQALine.ModifiedDate = DateTime.Now;
            new JobReceiveQALineService(db, _unitOfWork).Update(JobReceiveQALine, UserName);


            JobReceiveQALineExtended JobReceiveQALineExtended = db.JobReceiveQALineExtended.Find(pt.JobReceiveQALineId);
            JobReceiveQALineExtended.Length = pt.Length;
            JobReceiveQALineExtended.Width = pt.Width;
            JobReceiveQALineExtended.Height = pt.Height;
            JobReceiveQALineExtended.ObjectState = ObjectState.Modified;
            db.JobReceiveQALineExtended.Add(JobReceiveQALineExtended);


            List<QAGroupLineLineViewModel> QAGroupLineLineList = pt.QAGroupLine;

            if (QAGroupLineLineList != null)
            {
                foreach (var item in QAGroupLineLineList)
                {
                    if (item.JobReceiveQAAttributeId != null && item.JobReceiveQAAttributeId != 0)
                    {
                        JobReceiveQAAttribute JobReceiveQAAttribute = Find((int)item.JobReceiveQAAttributeId);
                        JobReceiveQAAttribute.QAGroupLineId = item.QAGroupLineId;
                        JobReceiveQAAttribute.Value = item.Value;
                        JobReceiveQAAttribute.Remark = item.Remarks;
                        JobReceiveQAAttribute.ModifiedBy = UserName;
                        JobReceiveQAAttribute.ModifiedDate = DateTime.Now;
                        JobReceiveQAAttribute.ObjectState = ObjectState.Modified;
                        db.JobReceiveQAAttribute.Add(JobReceiveQAAttribute);
                    }
                    else
                    {
                        JobReceiveQAAttribute JobReceiveQAAttribute = new JobReceiveQAAttribute();
                        JobReceiveQAAttribute.JobReceiveQALineId = JobReceiveQALine.JobReceiveQALineId;
                        JobReceiveQAAttribute.QAGroupLineId = item.QAGroupLineId;
                        JobReceiveQAAttribute.Value = item.Value;
                        JobReceiveQAAttribute.Remark = item.Remarks;
                        JobReceiveQAAttribute.CreatedBy = UserName;
                        JobReceiveQAAttribute.ModifiedBy = UserName;
                        JobReceiveQAAttribute.CreatedDate = DateTime.Now;
                        JobReceiveQAAttribute.ModifiedDate = DateTime.Now;
                        JobReceiveQAAttribute.ObjectState = ObjectState.Added;
                        db.JobReceiveQAAttribute.Add(JobReceiveQAAttribute);
                    }
                }
            }
        }




        public IQueryable<JobReceivePendingToQAIndex> GetJobReceiveQAAttributeList(int DocTypeId, string Uname)
        {
            var DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            var SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

            var settings = new JobReceiveQASettingsService(db).GetJobReceiveQASettingsForDocument(DocTypeId, DivisionId, SiteId);

            string[] contraDocTypes = null;
            if (!string.IsNullOrEmpty(settings.filterContraDocTypes)) { contraDocTypes = settings.filterContraDocTypes.Split(",".ToCharArray()); }
            else { contraDocTypes = new string[] { "NA" }; }

            return (from L in db.ViewJobReceiveBalanceForQA
                    join Jrl in db.JobReceiveLine on L.JobReceiveLineId equals Jrl.JobReceiveLineId into JobReceiveLineTable
                    from JobReceiveLineTab in JobReceiveLineTable.DefaultIfEmpty()
                    join H in db.JobReceiveHeader on L.JobReceiveHeaderId equals H.JobReceiveHeaderId into JobReceiveHeaderTable from JobReceiveHeaderTab in JobReceiveHeaderTable.DefaultIfEmpty()
                    orderby JobReceiveHeaderTab.DocDate, JobReceiveHeaderTab.DocNo
                    where JobReceiveHeaderTab.SiteId == SiteId && JobReceiveHeaderTab.DivisionId == DivisionId 
                    && (string.IsNullOrEmpty(settings.filterContraDocTypes) ? 1 == 1 : contraDocTypes.Contains(JobReceiveHeaderTab.DocTypeId.ToString()))
                    select new JobReceivePendingToQAIndex
                    {
                        JobReceiveHeaderId =JobReceiveHeaderTab.JobReceiveHeaderId,
                        JobReceiveLineId = L.JobReceiveLineId,
                        DocTypeName = JobReceiveHeaderTab.DocType.DocumentTypeName,
                        DocDate = JobReceiveHeaderTab.DocDate,
                        DocNo = JobReceiveHeaderTab.DocNo,
                        JobWorkerName = JobReceiveHeaderTab.JobWorker.Person.Name,
                        ProductName = JobReceiveLineTab.JobOrderLine.Product.ProductName,
                        ProductUidName = JobReceiveLineTab.ProductUid.ProductUidName,
                        DocTypeId = DocTypeId
                    }
                );
        }

        public List<QAGroupLineLineViewModel> GetJobReceiveQAAttribute(int JobReceiveLineid)
        {
            List<QAGroupLineLineViewModel> JobReceiveQAAttribute = (from L in db.JobReceiveLine
                                                                    join H in db.JobReceiveHeader on L.JobReceiveHeaderId equals H.JobReceiveHeaderId into JobReceiveHeaderTable
                                                                    from JobReceiveHeaderTab in JobReceiveHeaderTable.DefaultIfEmpty()
                                                                    join Jol in db.JobOrderLine on L.JobOrderLineId equals Jol.JobOrderLineId into JobOrderLineTable
                                                                    from JobOrderLineTab in JobOrderLineTable.DefaultIfEmpty()
                                                                    join Pp in db.ProductProcess on new { X1 = JobOrderLineTab.ProductId, X2 = JobReceiveHeaderTab.ProcessId } equals new { X1 = Pp.ProductId, X2 = (Pp.ProcessId ?? 0) } into ProductProcessTable
                                                                    from ProductProcessTab in ProductProcessTable.DefaultIfEmpty()
                                                                    join QAGl in db.QAGroupLine on ProductProcessTab.QAGroupId equals QAGl.QAGroupId into QAGroupLineTable
                                                                    from QAGroupLineTab in QAGroupLineTable.DefaultIfEmpty()
                                                                    where L.JobReceiveLineId == JobReceiveLineid && ((int?)QAGroupLineTab.QAGroupLineId ?? 0) != 0
                                                                    select new QAGroupLineLineViewModel
                                                                    {
                                                                        QAGroupLineId = QAGroupLineTab.QAGroupLineId,
                                                                        DefaultValue = QAGroupLineTab.DefaultValue,
                                                                        Value = QAGroupLineTab.DefaultValue,
                                                                        Name = QAGroupLineTab.Name,
                                                                        DataType = QAGroupLineTab.DataType,
                                                                        ListItem = QAGroupLineTab.ListItem
                                                                    }).ToList();


            return JobReceiveQAAttribute;
        }



        public WeavingReceiveQACombinedViewModel GetJobReceiveDetailForEdit(int JobReceiveHeaderId)//JobReceiveHeaderId
        {
            WeavingReceiveQACombinedViewModel WeavingReceiveQADetail = (from H in db.JobReceiveHeader
                                                                        join L in db.JobReceiveLine on H.JobReceiveHeaderId equals L.JobReceiveHeaderId into JobReceiveLineTable
                                                                        from JobReceiveLineTab in JobReceiveLineTable.DefaultIfEmpty()
                                                                        join Jrql in db.JobReceiveQALine on JobReceiveLineTab.JobReceiveLineId equals Jrql.JobReceiveLineId into JobReceiveQaLineTable
                                                                        from JobReceiveQALineTab in JobReceiveQaLineTable.DefaultIfEmpty()
                                                                        join Jol in db.JobOrderLine on JobReceiveLineTab.JobOrderLineId equals Jol.JobOrderLineId into JobOrderLineTable
                                                                        from JobOrderLineTab in JobOrderLineTable.DefaultIfEmpty()
                                                                        join Ld in db.JobReceiveQALineExtended on JobReceiveQALineTab.JobReceiveQALineId equals Ld.JobReceiveQALineId into JobReceiveQALineExtendedTable
                                                                        from JobReceiveQALineExtendedTab in JobReceiveQALineExtendedTable.DefaultIfEmpty()
                                                                        where JobReceiveLineTab.JobReceiveHeaderId == JobReceiveHeaderId
                                                                        select new WeavingReceiveQACombinedViewModel
                                                                     {
                                                                         JobReceiveHeaderId = H.JobReceiveHeaderId,
                                                                         JobReceiveLineId = JobReceiveLineTab.JobReceiveLineId,
                                                                         JobReceiveQALineId = JobReceiveQALineTab.JobReceiveQALineId,
                                                                         JobReceiveQAHeaderId = JobReceiveQALineTab.JobReceiveQAHeaderId,
                                                                         JobOrderLineId = JobReceiveLineTab.JobOrderLineId,
                                                                         JobOrderHeaderDocNo = JobOrderLineTab.JobOrderHeader.DocNo,
                                                                         GodownId = H.GodownId,
                                                                         JobWorkerId = H.JobWorkerId,
                                                                         ProductUidId = JobReceiveLineTab.ProductUidId,
                                                                         ProductUidName = JobReceiveLineTab.ProductUid.ProductUidName,
                                                                         ProductId = JobOrderLineTab.ProductId,
                                                                         ProductName = JobOrderLineTab.Product.ProductName,
                                                                         Qty = JobReceiveLineTab.Qty,
                                                                         UnitId = JobOrderLineTab.Product.UnitId,
                                                                         DealUnitId = JobReceiveLineTab.DealUnitId,
                                                                         UnitConversionMultiplier = JobReceiveLineTab.UnitConversionMultiplier,
                                                                         DealQty = JobReceiveLineTab.DealQty,
                                                                         Weight = JobReceiveLineTab.Weight,
                                                                         UnitDecimalPlaces = JobOrderLineTab.Product.Unit.DecimalPlaces,
                                                                         DealUnitDecimalPlaces = JobOrderLineTab.DealUnit.DecimalPlaces,
                                                                         Rate = JobOrderLineTab.Rate,
                                                                         XRate = JobOrderLineTab.Rate,
                                                                         PenaltyRate = JobReceiveLineTab.PenaltyRate,
                                                                         PenaltyAmt = JobReceiveLineTab.PenaltyAmt,
                                                                         DivisionId = H.DivisionId,
                                                                         SiteId = H.SiteId,
                                                                         ProcessId = H.ProcessId,
                                                                         DocDate = H.DocDate,
                                                                         DocTypeId = H.DocTypeId,
                                                                         DocNo = H.DocNo,
                                                                         JobReceiveById = JobReceiveLineTab.JobReceiveHeader.JobReceiveById,
                                                                         Remark = H.Remark,
                                                                         Length = JobReceiveQALineExtendedTab.Length,
                                                                         XLength = JobReceiveQALineExtendedTab.Length,
                                                                         Width = JobReceiveQALineExtendedTab.Width,
                                                                         XWidth = JobReceiveQALineExtendedTab.Width,
                                                                         Height = JobReceiveQALineExtendedTab.Height,
                                                                     }).FirstOrDefault();

            if (WeavingReceiveQADetail != null)
            {
                ProductDimensions ProductDimensions = new ProductService(_unitOfWork).GetProductDimensions(WeavingReceiveQADetail.ProductId, WeavingReceiveQADetail.DealUnitId, WeavingReceiveQADetail.DocTypeId);
                if (ProductDimensions != null)
                {
                    WeavingReceiveQADetail.DimensionUnitDecimalPlaces = ProductDimensions.DimensionUnitDecimalPlaces;
                }
            }

            return WeavingReceiveQADetail;
        }

        public LastValues GetLastValues(int DocTypeId)
        {
            var DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            var SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];


            var temp = (from H in db.JobReceiveHeader
                        where H.DocTypeId == DocTypeId && H.SiteId == SiteId && H.DivisionId == DivisionId
                        orderby H.JobReceiveHeaderId descending
                        select new LastValues
                        {
                            JobReceiveById = H.JobReceiveById
                        }).FirstOrDefault();

            return temp;
        }

        public void Delete(int id)//JobReceiveHeaderId
        {
            JobReceiveHeader JobReceiveHeader = db.JobReceiveHeader.Find(id);
            int StockHeaderId = (int)JobReceiveHeader.StockHeaderId;
            int ProductUidHeaderId = 0;
            int JobReceiveQAHeaderId = 0;

            IEnumerable<JobReceiveLine> JobReceiveLineList = (from L in db.JobReceiveLine where L.JobReceiveHeaderId == id select L).ToList();
            foreach (JobReceiveLine JobReceiveLine in JobReceiveLineList)
            {
                ProductUidHeaderId = JobReceiveLine.ProductUidHeaderId ?? 0;
                IEnumerable<JobReceiveQALine> JobReceiveQALineList = (from L in db.JobReceiveQALine where L.JobReceiveLineId == JobReceiveLine.JobReceiveLineId select L).ToList();

                foreach (JobReceiveQALine JobReceiveQALine in JobReceiveQALineList)
                {
                    JobReceiveQAHeaderId = JobReceiveQALine.JobReceiveQAHeaderId;
                    IEnumerable<JobReceiveQAAttribute> JobReceiveQAAttributeList = (from L in db.JobReceiveQAAttribute where L.JobReceiveQALineId == JobReceiveQALine.JobReceiveQALineId select L).ToList();
                    foreach (var JobReceiveQAAttribute in JobReceiveQAAttributeList)
                    {
                        if (JobReceiveQAAttribute.JobReceiveQAAttributeId != null)
                        {
                            JobReceiveQAAttribute.ObjectState = Model.ObjectState.Deleted;
                            db.JobReceiveQAAttribute.Remove(JobReceiveQAAttribute);
                        }
                    }

                    IEnumerable<JobReceiveQAPenalty> JobReceiveQAPenaltyList = (from L in db.JobReceiveQAPenalty where L.JobReceiveQALineId == JobReceiveQALine.JobReceiveQALineId select L).ToList();
                    foreach (var JobReceiveQAPenalty in JobReceiveQAPenaltyList)
                    {
                        if (JobReceiveQAPenalty.JobReceiveQAPenaltyId != null)
                        {
                            JobReceiveQAPenalty.ObjectState = Model.ObjectState.Deleted;
                            db.JobReceiveQAPenalty.Remove(JobReceiveQAPenalty);
                        }
                    }

                    JobReceiveQALineExtended JobReceiveQALineExtended = (from L in db.JobReceiveQALineExtended where L.JobReceiveQALineId == JobReceiveQALine.JobReceiveQALineId select L).FirstOrDefault();
                    if (JobReceiveQALineExtended != null)
                    {
                        JobReceiveQALineExtended.ObjectState = Model.ObjectState.Deleted;
                        db.JobReceiveQALineExtended.Remove(JobReceiveQALineExtended);
                    }

                    JobReceiveQALine.ObjectState = ObjectState.Deleted;
                    db.JobReceiveQALine.Remove(JobReceiveQALine);
                }
                JobReceiveLine.ObjectState = ObjectState.Deleted;
                db.JobReceiveLine.Remove(JobReceiveLine);
            }

            JobReceiveQAHeader JobReceiveQAHeader = db.JobReceiveQAHeader.Find(JobReceiveQAHeaderId);
            JobReceiveQAHeader.ObjectState = ObjectState.Deleted;
            db.JobReceiveQAHeader.Remove(JobReceiveQAHeader);

            JobReceiveHeader.ObjectState = ObjectState.Deleted;
            db.JobReceiveHeader.Remove(JobReceiveHeader);

            if (StockHeaderId != null)
            {
                IEnumerable<Stock> StockList = (from L in db.Stock where L.StockHeaderId == StockHeaderId select L).ToList();
                foreach (var Stock in StockList)
                {
                    Stock.ObjectState = ObjectState.Deleted;
                    db.Stock.Remove(Stock);
                }

                StockHeader StockHeader = db.StockHeader.Find(StockHeaderId);
                StockHeader.ObjectState = ObjectState.Deleted;
                db.StockHeader.Remove(StockHeader);
            }

            if (ProductUidHeaderId > 0)
            {
                ProductUidHeader ProductUidHeader = db.ProductUidHeader.Find(ProductUidHeaderId);
                IEnumerable<ProductUid> ProductUidList = (from P in db.ProductUid where P.ProductUidHeaderId == ProductUidHeaderId select P).ToList();
                foreach (var ProductUid in ProductUidList)
                {
                    if (ProductUid.LastTransactionDocId == null || (ProductUid.LastTransactionDocId == ProductUid.GenDocId && ProductUid.LastTransactionDocTypeId == ProductUid.GenDocTypeId))
                    {
                        ProductUid.ObjectState = Model.ObjectState.Deleted;
                        db.ProductUid.Remove(ProductUid);
                    }
                    else
                    {
                        throw new Exception("Record Cannot be deleted as its Unique Id's are in use by other documents");
                    }
                }
                ProductUidHeader.ObjectState = ObjectState.Deleted;
                db.ProductUidHeader.Remove(ProductUidHeader);
            }
        }


        public IQueryable<ComboBoxResult> GetCustomProduct(int filter, string term)
        {
            var DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            var SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

            var settings = new JobReceiveSettingsService(_unitOfWork).GetJobReceiveSettingsForDocument(filter, DivisionId, SiteId);

            string[] contraSites = null;
            if (!string.IsNullOrEmpty(settings.filterContraSites)) { contraSites = settings.filterContraSites.Split(",".ToCharArray()); }
            else { contraSites = new string[] { "NA" }; }

            string[] contraDivisions = null;
            if (!string.IsNullOrEmpty(settings.filterContraDivisions)) { contraDivisions = settings.filterContraDivisions.Split(",".ToCharArray()); }
            else { contraDivisions = new string[] { "NA" }; }

            string[] contraDocTypes = null;
            if (!string.IsNullOrEmpty(settings.filterContraDocTypes)) { contraDocTypes = settings.filterContraDocTypes.Split(",".ToCharArray()); }
            else { contraDocTypes = new string[] { "NA" }; }

            int CurrentSiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            int CurrentDivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];


            var list = (from p in db.ViewJobOrderBalance
                        join t in db.JobOrderHeader on p.JobOrderHeaderId equals t.JobOrderHeaderId
                        join t2 in db.JobOrderLine on p.JobOrderLineId equals t2.JobOrderLineId
                        join pt in db.Product on p.ProductId equals pt.ProductId into ProductTable
                        from ProductTab in ProductTable.DefaultIfEmpty()
                        join D1 in db.Dimension1 on p.Dimension1Id equals D1.Dimension1Id into Dimension1Table
                        from Dimension1Tab in Dimension1Table.DefaultIfEmpty()
                        join D2 in db.Dimension2 on p.Dimension2Id equals D2.Dimension2Id into Dimension2Table
                        from Dimension2Tab in Dimension2Table.DefaultIfEmpty()
                        where p.BalanceQty > 0
                        && ((string.IsNullOrEmpty(term) ? 1 == 1 : p.JobOrderNo.ToLower().Contains(term.ToLower()))
                        || (string.IsNullOrEmpty(term) ? 1 == 1 : ProductTab.ProductName.ToLower().Contains(term.ToLower()))
                        || (string.IsNullOrEmpty(term) ? 1 == 1 : Dimension1Tab.Dimension1Name.ToLower().Contains(term.ToLower()))
                        || (string.IsNullOrEmpty(term) ? 1 == 1 : Dimension2Tab.Dimension2Name.ToLower().Contains(term.ToLower())))
                        && (string.IsNullOrEmpty(settings.filterContraSites) ? p.SiteId == CurrentSiteId : contraSites.Contains(p.SiteId.ToString()))
                        && (string.IsNullOrEmpty(settings.filterContraDivisions) ? p.DivisionId == CurrentDivisionId : contraDivisions.Contains(p.DivisionId.ToString()))
                        && (string.IsNullOrEmpty(settings.filterContraDocTypes) ? 1 == 1 : contraDocTypes.Contains(t.DocTypeId.ToString()))
                        orderby t.DocDate, t.DocNo
                        select new ComboBoxResult
                        {
                            text = ProductTab.ProductName,
                            id = p.JobOrderLineId.ToString(),
                            TextProp1 = "Order No: " + p.JobOrderNo.ToString(),
                            TextProp2 = "BalQty: " + p.BalanceQty.ToString(),
                            AProp1 = Dimension1Tab.Dimension1Name,
                            AProp2 = Dimension2Tab.Dimension2Name
                        });

            return list;
        }
        

        public void Dispose()
        {
        }


        public Task<IEquatable<JobReceiveQAAttribute>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<JobReceiveQAAttribute> FindAsync(int id)
        {
            throw new NotImplementedException();
        }
    }

}
