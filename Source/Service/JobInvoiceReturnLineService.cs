using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Infrastructure;
using Model.Models;

using Core.Common;
using System;
using Model;
using System.Threading.Tasks;
using Data.Models;
using Model.ViewModel;
using Model.ViewModels;

namespace Service
{
    public interface IJobInvoiceReturnLineService : IDisposable
    {
        JobInvoiceReturnLine Create(JobInvoiceReturnLine pt);
        void Delete(int id);
        void Delete(JobInvoiceReturnLine pt);
        JobInvoiceReturnLine Find(int id);
        void Update(JobInvoiceReturnLine pt);
        IEnumerable<JobInvoiceReturnLineIndexViewModel> GetLineListForIndex(int HeaderId);
        Task<IEquatable<JobInvoiceReturnLine>> GetAsync();
        Task<JobInvoiceReturnLine> FindAsync(int id);
        JobInvoiceReturnLineViewModel GetJobInvoiceReturnLine(int id);
        int NextId(int id);
        int PrevId(int id);
        IEnumerable<JobInvoiceReturnLineViewModel> GetJobReceiveForFilters(JobInvoiceReturnLineFilterViewModel vm);
        IEnumerable<JobInvoiceReturnLineViewModel> GetJobInvoiceForFilters(JobInvoiceReturnLineFilterViewModel vm);
        IEnumerable<JobInvoiceListViewModel> GetPendingJobInvoiceHelpList(int Id, string term);
        IEnumerable<JobReceiveListViewModel> GetPendingJobReceiveHelpList(int Id, string term);
        IEnumerable<ComboBoxList> GetProductHelpList(int Id, string term);
        int GetMaxSr(int id);
        JobInvoiceLineViewModel GetJobInvoiceLineBalance(int id);
        object GetProductUidDetail(string ProductUidName, int filter);
        string GetFirstBarCodeForReturn(int JobReceiveLineId);
        string GetFirstBarCodeForReturn(int[] JobReceiveLineIds);
        List<ComboBoxList> GetPendingBarCodesList(string id, int GodownId);
    }

    public class JobInvoiceReturnLineService : IJobInvoiceReturnLineService
    {
        ApplicationDbContext db;
        public JobInvoiceReturnLineService(ApplicationDbContext db)
        {
            this.db = db;
        }


        public JobInvoiceReturnLine Find(int id)
        {
            return db.JobInvoiceReturnLine.Find(id);
        }

        public JobInvoiceReturnLine Create(JobInvoiceReturnLine pt)
        {
            pt.ObjectState = ObjectState.Added;
            db.JobInvoiceReturnLine.Add(pt);
            return pt;
        }

        public void Delete(int id)
        {
            var temp = db.JobInvoiceReturnLine.Find(id);
            temp.ObjectState = Model.ObjectState.Deleted;
            db.JobInvoiceReturnLine.Remove(temp);
        }

        public void Delete(JobInvoiceReturnLine pt)
        {
            pt.ObjectState = Model.ObjectState.Deleted;
            db.JobInvoiceReturnLine.Remove(pt);
        }
        public IEnumerable<JobInvoiceReturnLineViewModel> GetJobReceiveForFilters(JobInvoiceReturnLineFilterViewModel vm)
        {
            string[] ProductIdArr = null;
            if (!string.IsNullOrEmpty(vm.ProductId)) { ProductIdArr = vm.ProductId.Split(",".ToCharArray()); }
            else { ProductIdArr = new string[] { "NA" }; }

            string[] SaleOrderIdArr = null;
            if (!string.IsNullOrEmpty(vm.JobReceiveHeaderId)) { SaleOrderIdArr = vm.JobReceiveHeaderId.Split(",".ToCharArray()); }
            else { SaleOrderIdArr = new string[] { "NA" }; }

            string[] ProductGroupIdArr = null;
            if (!string.IsNullOrEmpty(vm.ProductGroupId)) { ProductGroupIdArr = vm.ProductGroupId.Split(",".ToCharArray()); }
            else { ProductGroupIdArr = new string[] { "NA" }; }

            var temp = (from p in db.ViewJobInvoiceBalance
                        join l in db.JobInvoiceLine on p.JobInvoiceLineId equals l.JobInvoiceLineId into linetable
                        from linetab in linetable.DefaultIfEmpty()
                        join t in db.JobInvoiceHeader on p.JobInvoiceHeaderId equals t.JobInvoiceHeaderId into table
                        from tab in table.DefaultIfEmpty()
                        join t1 in db.JobReceiveLine on p.JobReceiveLineId equals t1.JobReceiveLineId into table1
                        from tab1 in table1.DefaultIfEmpty()
                        join t2 in db.JobReceiveHeader on tab1.JobReceiveHeaderId equals t2.JobReceiveHeaderId
                        join jo in db.JobOrderLine on tab1.JobOrderLineId equals jo.JobOrderLineId
                        join product in db.Product on p.ProductId equals product.ProductId into table2
                        from tab2 in table2.DefaultIfEmpty()
                        where (string.IsNullOrEmpty(vm.ProductId) ? 1 == 1 : ProductIdArr.Contains(p.ProductId.ToString()))
                        && (string.IsNullOrEmpty(vm.JobReceiveHeaderId) ? 1 == 1 : SaleOrderIdArr.Contains(p.JobReceiveHeaderId.ToString()))
                        && (string.IsNullOrEmpty(vm.ProductGroupId) ? 1 == 1 : ProductGroupIdArr.Contains(tab2.ProductGroupId.ToString()))
                        && p.BalanceQty > 0
                        orderby t2.DocDate, t2.DocNo, tab1.Sr
                        select new JobInvoiceReturnLineViewModel
                        {
                            Dimension1Name = jo.Dimension1.Dimension1Name,
                            Dimension2Name = jo.Dimension2.Dimension2Name,
                            Specification = jo.Specification,
                            InvoiceBalQty = p.BalanceQty,
                            Qty = p.BalanceQty,
                            JobInvoiceHeaderDocNo = tab.DocNo,
                            ProductName = tab2.ProductName,
                            ProductId = p.ProductId,
                            JobInvoiceReturnHeaderId = vm.JobInvoiceReturnHeaderId,
                            JobInvoiceLineId = p.JobInvoiceLineId,
                            UnitId = tab2.UnitId,
                            UnitConversionMultiplier = linetab.UnitConversionMultiplier,
                            DealUnitId = linetab.DealUnitId,
                            Rate = linetab.Rate,
                            //RateAfterDiscount = (linetab.Amount / linetab.DealQty),
                            unitDecimalPlaces = tab2.Unit.DecimalPlaces,
                            DealunitDecimalPlaces = linetab.DealUnit.DecimalPlaces,
                            ProductUidName = tab1.ProductUid.ProductUidName,
                            ProductUidId = tab1.ProductUidId,
                        }

                        );
            return temp;
        }
        public IEnumerable<JobInvoiceReturnLineViewModel> GetJobInvoiceForFilters(JobInvoiceReturnLineFilterViewModel vm)
        {
            string[] ProductIdArr = null;
            if (!string.IsNullOrEmpty(vm.ProductId)) { ProductIdArr = vm.ProductId.Split(",".ToCharArray()); }
            else { ProductIdArr = new string[] { "NA" }; }

            string[] SaleOrderIdArr = null;
            if (!string.IsNullOrEmpty(vm.JobInvoiceHeaderId)) { SaleOrderIdArr = vm.JobInvoiceHeaderId.Split(",".ToCharArray()); }
            else { SaleOrderIdArr = new string[] { "NA" }; }

            string[] ProductGroupIdArr = null;
            if (!string.IsNullOrEmpty(vm.ProductGroupId)) { ProductGroupIdArr = vm.ProductGroupId.Split(",".ToCharArray()); }
            else { ProductGroupIdArr = new string[] { "NA" }; }
            //ToChange View to get Joborders instead of goodsreceipts
            var temp = (from p in db.ViewJobInvoiceBalance
                        join l in db.JobInvoiceLine on p.JobInvoiceLineId equals l.JobInvoiceLineId into linetable
                        from linetab in linetable.DefaultIfEmpty()
                        join h in db.JobInvoiceHeader on linetab.JobInvoiceHeaderId equals h.JobInvoiceHeaderId
                        join product in db.Product on p.ProductId equals product.ProductId into table2
                        from tab2 in table2.DefaultIfEmpty()
                        join t1 in db.JobReceiveLine on p.JobReceiveLineId equals t1.JobReceiveLineId into table1
                        from tab1 in table1.DefaultIfEmpty()
                        join jo in db.JobOrderLine on tab1.JobOrderLineId equals jo.JobOrderLineId
                        where (string.IsNullOrEmpty(vm.ProductId) ? 1 == 1 : ProductIdArr.Contains(p.ProductId.ToString()))
                        && (string.IsNullOrEmpty(vm.JobInvoiceHeaderId) ? 1 == 1 : SaleOrderIdArr.Contains(p.JobInvoiceHeaderId.ToString()))
                        && (string.IsNullOrEmpty(vm.ProductGroupId) ? 1 == 1 : ProductGroupIdArr.Contains(tab2.ProductGroupId.ToString()))
                        && p.BalanceQty > 0
                        orderby h.DocDate, h.DocNo, linetab.Sr
                        select new JobInvoiceReturnLineViewModel
                        {
                            Dimension1Name = jo.Dimension1.Dimension1Name,
                            Dimension2Name = jo.Dimension2.Dimension2Name,
                            Specification = jo.Specification,
                            InvoiceBalQty = p.BalanceQty,
                            Qty = p.BalanceQty,
                            JobInvoiceHeaderDocNo = p.JobInvoiceNo,
                            ProductName = tab2.ProductName,
                            ProductId = p.ProductId,
                            JobInvoiceReturnHeaderId = vm.JobInvoiceReturnHeaderId,
                            JobInvoiceLineId = p.JobInvoiceLineId,
                            UnitId = tab2.UnitId,
                            UnitConversionMultiplier = linetab.UnitConversionMultiplier,
                            DealUnitId = linetab.DealUnitId,
                            Rate = linetab.Rate,
                            //RateAfterDiscount = (linetab.Amount / linetab.DealQty),
                            unitDecimalPlaces = tab2.Unit.DecimalPlaces,
                            DealunitDecimalPlaces = linetab.DealUnit.DecimalPlaces,
                            ProductUidName = tab1.ProductUid.ProductUidName,
                            ProductUidId = tab1.ProductUidId,
                        }

                        );
            return temp;
        }
        public void Update(JobInvoiceReturnLine pt)
        {
            pt.ObjectState = ObjectState.Modified;
            db.JobInvoiceReturnLine.Add(pt);
        }
        public IEnumerable<JobInvoiceReturnLineIndexViewModel> GetLineListForIndex(int HeaderId)
        {
            return (from p in db.JobInvoiceReturnLine
                    join t in db.JobInvoiceLine on p.JobInvoiceLineId equals t.JobInvoiceLineId into table
                    from tab in table.DefaultIfEmpty()
                    join t in db.JobReceiveLine on tab.JobReceiveLineId equals t.JobReceiveLineId into table1
                    from tab1 in table1.DefaultIfEmpty()
                    join t in db.JobReceiveHeader on tab1.JobReceiveHeaderId equals t.JobReceiveHeaderId into table3
                    from tab3 in table3.DefaultIfEmpty()
                    join jo in db.JobOrderLine on tab1.JobOrderLineId equals jo.JobOrderLineId
                    join t in db.Product on jo.ProductId equals t.ProductId into table2
                    from tab2 in table2.DefaultIfEmpty()
                    where p.JobInvoiceReturnHeaderId == HeaderId
                    orderby p.Sr
                    select new JobInvoiceReturnLineIndexViewModel
                    {

                        ProductName = tab2.ProductName,
                        Qty = p.Qty,
                        JobInvoiceReturnLineId = p.JobInvoiceReturnLineId,
                        UnitId = tab2.UnitId,
                        Specification = jo.Specification,
                        Dimension1Name = jo.Dimension1.Dimension1Name,
                        Dimension2Name = jo.Dimension2.Dimension2Name,
                        LotNo = tab1.LotNo,
                        JobGoodsRecieptHeaderDocNo = tab3.DocNo,
                        JobInvoiceHeaderDocNo = tab.JobInvoiceHeader.DocNo,
                        DealQty = p.DealQty,
                        DealUnitId = p.DealUnitId,
                        unitDecimalPlaces = tab2.Unit.DecimalPlaces,
                        DealunitDecimalPlaces = p.DealUnit.DecimalPlaces,
                        Rate = p.Rate,
                        Amount = p.Amount,
                        Remark = p.Remark,
                        ProductUidName = tab1.ProductUid.ProductUidName,
                        UnitName = tab2.Unit.UnitName,
                        DealUnitName = p.DealUnit.UnitName,
                    }
                        );
        }
        public JobInvoiceReturnLineViewModel GetJobInvoiceReturnLine(int id)
        {
            return (from p in db.JobInvoiceReturnLine
                    join t in db.JobInvoiceLine on p.JobInvoiceLineId equals t.JobInvoiceLineId into table
                    from tab in table.DefaultIfEmpty()
                    join t4 in db.ViewJobInvoiceBalance on p.JobInvoiceLineId equals t4.JobInvoiceLineId into table4
                    from tab4 in table4.DefaultIfEmpty()
                    join t3 in db.JobReceiveLine on tab.JobReceiveLineId equals t3.JobReceiveLineId into table3
                    from tab3 in table3.DefaultIfEmpty()
                    join jo in db.JobOrderLine on tab3.JobOrderLineId equals jo.JobOrderLineId
                    join t2 in db.JobInvoiceReturnHeader on p.JobInvoiceReturnHeaderId equals t2.JobInvoiceReturnHeaderId into table2
                    from tab2 in table2.DefaultIfEmpty()
                    join t in db.JobInvoiceHeader on tab.JobInvoiceHeaderId equals t.JobInvoiceHeaderId into table1
                    from tab1 in table1.DefaultIfEmpty()
                    where p.JobInvoiceReturnLineId == id
                    select new JobInvoiceReturnLineViewModel
                    {

                        JobWorkerId = tab2.JobWorkerId,
                        ProductId = jo.ProductId,
                        JobInvoiceLineId = p.JobInvoiceLineId,
                        JobInvoiceHeaderDocNo = tab1.DocNo,
                        JobInvoiceReturnHeaderId = p.JobInvoiceReturnHeaderId,
                        JobInvoiceReturnLineId = p.JobInvoiceReturnLineId,
                        Rate = p.Rate,
                        Amount = p.Amount,
                        UnitConversionMultiplier = p.UnitConversionMultiplier,
                        DealQty = p.DealQty,
                        DealUnitId = p.DealUnitId,
                        Qty = p.Qty,
                        InvoiceBalQty = ((p.JobInvoiceLineId == null || tab3 == null) ? p.Qty : p.Qty + (tab4 == null ? 0 : tab4.BalanceQty)),
                        Remark = p.Remark,
                        UnitId = tab3.Product.UnitId,
                        Dimension1Id = jo.Dimension1Id,
                        Dimension1Name = jo.Dimension1.Dimension1Name,
                        Dimension2Id = jo.Dimension2Id,
                        Dimension2Name = jo.Dimension2.Dimension2Name,
                        Specification = jo.Specification,
                        LotNo = tab3.LotNo,
                        LockReason = p.LockReason,
                        ProductUidId = tab3.ProductUidId,
                        ProductUidName = tab3.ProductUid.ProductUidName,
                    }
                        ).FirstOrDefault();
        }

        public int NextId(int id)
        {
            int temp = 0;
            if (id != 0)
            {
                temp = (from p in db.JobInvoiceReturnLine
                        orderby p.JobInvoiceReturnLineId
                        select p.JobInvoiceReturnLineId).AsEnumerable().SkipWhile(p => p != id).Skip(1).FirstOrDefault();
            }
            else
            {
                temp = (from p in db.JobInvoiceReturnLine
                        orderby p.JobInvoiceReturnLineId
                        select p.JobInvoiceReturnLineId).FirstOrDefault();
            }
            if (temp != 0)
                return temp;
            else
                return id;
        }

        public int PrevId(int id)
        {

            int temp = 0;
            if (id != 0)
            {

                temp = (from p in db.JobInvoiceReturnLine
                        orderby p.JobInvoiceReturnLineId
                        select p.JobInvoiceReturnLineId).AsEnumerable().TakeWhile(p => p != id).LastOrDefault();
            }
            else
            {
                temp = (from p in db.JobInvoiceReturnLine
                        orderby p.JobInvoiceReturnLineId
                        select p.JobInvoiceReturnLineId).AsEnumerable().LastOrDefault();
            }
            if (temp != 0)
                return temp;
            else
                return id;
        }

        public IEnumerable<JobInvoiceListViewModel> GetPendingJobInvoiceHelpList(int Id, string term)
        {

            var InvoiceReturnHeader = new JobInvoiceReturnHeaderService(db).Find(Id);

            var settings = db.JobInvoiceSettings
            .Where(m => m.DocTypeId == InvoiceReturnHeader.DocTypeId && m.DivisionId == InvoiceReturnHeader.DivisionId && m.SiteId == InvoiceReturnHeader.SiteId).FirstOrDefault();

            string[] contraDocTypes = null;
            if (!string.IsNullOrEmpty(settings.filterContraDocTypes)) { contraDocTypes = settings.filterContraDocTypes.Split(",".ToCharArray()); }
            else { contraDocTypes = new string[] { "NA" }; }

            string[] contraSites = null;
            if (!string.IsNullOrEmpty(settings.filterContraSites)) { contraSites = settings.filterContraSites.Split(",".ToCharArray()); }
            else { contraSites = new string[] { "NA" }; }

            string[] contraDivisions = null;
            if (!string.IsNullOrEmpty(settings.filterContraDivisions)) { contraDivisions = settings.filterContraDivisions.Split(",".ToCharArray()); }
            else { contraDivisions = new string[] { "NA" }; }

            int CurrentSiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            int CurrentDivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];

            var list = (from p in db.ViewJobInvoiceBalance
                        where (string.IsNullOrEmpty(term) ? 1 == 1 : p.JobInvoiceNo.ToLower().Contains(term.ToLower())) && p.JobWorkerId == InvoiceReturnHeader.JobWorkerId && p.BalanceQty > 0
                        && (string.IsNullOrEmpty(settings.filterContraDocTypes) ? 1 == 1 : contraDocTypes.Contains(p.JobInvoiceDocTypeId.ToString()))
                          && (string.IsNullOrEmpty(settings.filterContraSites) ? p.SiteId == CurrentSiteId : contraSites.Contains(p.SiteId.ToString()))
                    && (string.IsNullOrEmpty(settings.filterContraDivisions) ? p.DivisionId == CurrentDivisionId : contraDivisions.Contains(p.DivisionId.ToString()))
                        group new { p } by p.JobInvoiceHeaderId into g
                        select new JobInvoiceListViewModel
                        {
                            DocNo = g.Max(m => m.p.JobInvoiceNo),
                            JobInvoiceHeaderId = g.Key,
                        }
                          ).Take(20);

            return list.ToList();
        }

        public IEnumerable<JobReceiveListViewModel> GetPendingJobReceiveHelpList(int Id, string term)
        {

            var InvoiceReturnHeader = new JobInvoiceReturnHeaderService(db).Find(Id);

            var settings = db.JobInvoiceSettings
            .Where(m => m.DocTypeId == InvoiceReturnHeader.DocTypeId && m.DivisionId == InvoiceReturnHeader.DivisionId && m.SiteId == InvoiceReturnHeader.SiteId).FirstOrDefault();

            string[] contraDocTypes = null;
            if (!string.IsNullOrEmpty(settings.filterContraDocTypes)) { contraDocTypes = settings.filterContraDocTypes.Split(",".ToCharArray()); }
            else { contraDocTypes = new string[] { "NA" }; }

            string[] contraSites = null;
            if (!string.IsNullOrEmpty(settings.filterContraSites)) { contraSites = settings.filterContraSites.Split(",".ToCharArray()); }
            else { contraSites = new string[] { "NA" }; }

            string[] contraDivisions = null;
            if (!string.IsNullOrEmpty(settings.filterContraDivisions)) { contraDivisions = settings.filterContraDivisions.Split(",".ToCharArray()); }
            else { contraDivisions = new string[] { "NA" }; }

            int CurrentSiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            int CurrentDivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];

            var list = (from p in db.ViewJobInvoiceBalance
                        where (string.IsNullOrEmpty(term) ? 1 == 1 : p.JobReceiveNo.ToLower().Contains(term.ToLower())) && p.JobWorkerId == InvoiceReturnHeader.JobWorkerId && p.BalanceQty > 0
                        && (string.IsNullOrEmpty(settings.filterContraDocTypes) ? 1 == 1 : contraDocTypes.Contains(p.JobInvoiceDocTypeId.ToString()))
                         && (string.IsNullOrEmpty(settings.filterContraSites) ? p.SiteId == CurrentSiteId : contraSites.Contains(p.SiteId.ToString()))
                    && (string.IsNullOrEmpty(settings.filterContraDivisions) ? p.DivisionId == CurrentDivisionId : contraDivisions.Contains(p.DivisionId.ToString()))
                        group new { p } by p.JobReceiveHeaderId into g
                        select new JobReceiveListViewModel
                        {
                            DocNo = g.Max(m => m.p.JobReceiveNo),
                            JobReceiveHeaderId = g.Key,
                        }
                          ).Take(20);

            return list.ToList();
        }


        public IEnumerable<ComboBoxList> GetProductHelpList(int Id, string term)
        {
            var JobInvoiceReturn = new JobInvoiceReturnHeaderService(db).Find(Id);

            var settings = db.JobInvoiceSettings
            .Where(m => m.DocTypeId == JobInvoiceReturn.DocTypeId && m.DivisionId == JobInvoiceReturn.DivisionId && m.SiteId == JobInvoiceReturn.SiteId).FirstOrDefault();

            string[] ProductTypes = null;
            if (!string.IsNullOrEmpty(settings.filterProductTypes)) { ProductTypes = settings.filterProductTypes.Split(",".ToCharArray()); }
            else { ProductTypes = new string[] { "NA" }; }

            var list = (from p in db.Product
                        where (string.IsNullOrEmpty(term) ? 1 == 1 : p.ProductName.ToLower().Contains(term.ToLower()))
                        && (string.IsNullOrEmpty(settings.filterProductTypes) ? 1 == 1 : ProductTypes.Contains(p.ProductGroup.ProductTypeId.ToString()))
                        group new { p } by p.ProductId into g
                        select new ComboBoxList
                        {
                            PropFirst = g.Max(m => m.p.ProductName),
                            Id = g.Key,

                            //    DocumentTypeName=g.Max(p=>p.p.DocumentTypeShortName)
                        }
                          ).Take(20);

            return list.ToList();
        }

        public int GetMaxSr(int id)
        {
            var Max = (from p in db.JobInvoiceReturnLine
                       where p.JobInvoiceReturnHeaderId == id
                       select p.Sr
                        );

            if (Max.Count() > 0)
                return Max.Max(m => m ?? 0) + 1;
            else
                return (1);
        }

        public JobInvoiceLineViewModel GetJobInvoiceLineBalance(int id)
        {
            return (from b in db.ViewJobInvoiceBalance
                    join p in db.JobInvoiceLine on b.JobInvoiceLineId equals p.JobInvoiceLineId
                    join t in db.JobReceiveLine on p.JobReceiveLineId equals t.JobReceiveLineId into table
                    from tab in table.DefaultIfEmpty()
                    join jo in db.JobOrderLine on tab.JobOrderLineId equals jo.JobOrderLineId
                    join t2 in db.JobInvoiceHeader on p.JobInvoiceHeaderId equals t2.JobInvoiceHeaderId into table2
                    from tab2 in table2.DefaultIfEmpty()
                    join t in db.JobReceiveHeader on tab.JobReceiveHeaderId equals t.JobReceiveHeaderId into table1
                    from tab1 in table1.DefaultIfEmpty()

                    where p.JobInvoiceLineId == id
                    select new JobInvoiceLineViewModel
                    {

                        JobWorkerId = tab2.JobWorkerId.Value,
                        Amount = p.Amount,
                        ProductId = jo.ProductId,
                        ProductName = jo.Product.ProductName,
                        JobReceiveLineId = p.JobReceiveLineId,
                        JobReceiveDocNo = tab1.DocNo,
                        JobInvoiceHeaderId = p.JobInvoiceHeaderId,
                        JobInvoiceLineId = p.JobInvoiceLineId,
                        Qty = b.BalanceQty,
                        Rate = p.Rate,
                        Remark = p.Remark,
                        UnitConversionMultiplier = p.UnitConversionMultiplier,
                        DealUnitId = p.DealUnitId,
                        DealQty = p.DealQty,
                        UnitId = jo.Product.UnitId,
                        Dimension1Id = jo.Dimension1Id,
                        Dimension1Name = jo.Dimension1.Dimension1Name,
                        Dimension2Id = jo.Dimension2Id,
                        Dimension2Name = jo.Dimension2.Dimension2Name,
                        Specification = jo.Specification,
                        LotNo = tab.LotNo,
                        //DiscountPer = p.DiscountPer
                        Weight = (tab.Weight / tab.Qty) * b.BalanceQty,
                    }
                        ).FirstOrDefault();
        }


        public object GetProductUidDetail(string ProductUidName, int filter)
        {
            var Header = new JobInvoiceReturnHeaderService(db).Find(filter);

            var temp = (from i in db.JobInvoiceLine
                        join L in db.JobReceiveLine on i.JobReceiveLineId equals L.JobReceiveLineId
                        join H in db.JobReceiveHeader on L.JobReceiveHeaderId equals H.JobReceiveHeaderId into JobReceiveHeaderTable
                        from JobReceiveHeaderTab in JobReceiveHeaderTable.DefaultIfEmpty()
                        join Jol in db.JobOrderLine on L.JobOrderLineId equals Jol.JobOrderLineId into JobOrderLineTable
                        from JobOrderLineTab in JobOrderLineTable.DefaultIfEmpty()
                        join Pu in db.ProductUid on (JobOrderLineTab.ProductUidHeaderId == null ? JobOrderLineTab.ProductUidId : L.ProductUidId) equals Pu.ProductUIDId into ProductUidTable
                        from ProductUidTab in ProductUidTable.DefaultIfEmpty()
                        where ProductUidTab.ProductUidName == ProductUidName && JobReceiveHeaderTab.ProcessId == Header.ProcessId && JobReceiveHeaderTab.SiteId == Header.SiteId
                        && JobReceiveHeaderTab.DivisionId == Header.DivisionId && JobReceiveHeaderTab.JobWorkerId == Header.JobWorkerId
                        orderby JobReceiveHeaderTab.DocDate
                        select new
                        {
                            ProductUidId = ProductUidTab.ProductUIDId,
                            JobInvoiceLineId = i.JobInvoiceLineId,
                            JobInvoiceDocNo = i.JobInvoiceHeader.DocNo,
                            Success = (i.JobInvoiceHeader.JobWorkerId == Header.JobWorkerId ? true : false),
                            ProdUidHeaderId = JobOrderLineTab.ProductUidHeaderId,
                        }).ToList().Last();
            return temp;
        }

        public string GetFirstBarCodeForReturn(int JobInvoiceLineId)
        {
            return (from t2 in db.JobInvoiceLine
                    join p in db.JobReceiveLine on t2.JobReceiveLineId equals p.JobReceiveLineId
                    where t2.JobInvoiceLineId == JobInvoiceLineId
                    join t in db.ProductUid on p.ProductUidId equals t.ProductUIDId
                    select p.ProductUidId).FirstOrDefault().ToString();
        }

        public string GetFirstBarCodeForReturn(int[] JobInvoiceLineIds)
        {
            return (from t2 in db.JobInvoiceLine
                    join p in db.JobReceiveLine on t2.JobReceiveLineId equals p.JobReceiveLineId
                    where JobInvoiceLineIds.Contains(t2.JobInvoiceLineId)
                    join t in db.ProductUid on p.ProductUidId equals t.ProductUIDId
                    orderby t.ProductUidName
                    select p.ProductUidId).FirstOrDefault().ToString();
        }

        public List<ComboBoxList> GetPendingBarCodesList(string id, int GodownId)
        {
            List<ComboBoxList> Barcodes = new List<ComboBoxList>();

            int[] JobInvoiceLine = id.Split(',').Select(Int32.Parse).ToArray();

            var InvoiceRecords = (from p in db.ViewJobInvoiceBalance
                                  join t in db.JobReceiveLine on p.JobReceiveLineId equals t.JobReceiveLineId
                                  where JobInvoiceLine.Contains(p.JobInvoiceLineId)
                                  select t).ToList();

            int[] BalanceRecRecordsProdUIds = InvoiceRecords.Select(m => m.ProductUidId.Value).ToArray();

            using (ApplicationDbContext context = new ApplicationDbContext())
            {

                //context.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                context.Configuration.AutoDetectChangesEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;

                //context.Database.CommandTimeout = 30000;

                var Temp = (from p in context.ProductUid
                            where BalanceRecRecordsProdUIds.Contains(p.ProductUIDId)
                            && p.CurrenctGodownId == GodownId && p.Status == ProductUidStatusConstants.Receive
                            orderby p.ProductUidName
                            select new { Id = p.ProductUIDId, Name = p.ProductUidName }).ToList();
                foreach (var item in Temp)
                {
                    Barcodes.Add(new ComboBoxList
                    {
                        Id = item.Id,
                        PropFirst = item.Name,
                    });
                }
            }



            return Barcodes;
        }

        public void Dispose()
        {
        }


        public Task<IEquatable<JobInvoiceReturnLine>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<JobInvoiceReturnLine> FindAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
