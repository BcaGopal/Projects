using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Infrastructure;
using Model.Models;
using Model.ViewModels;
using Core.Common;
using System;
using Model;
using System.Threading.Tasks;
using Data.Models;


namespace Service
{
    public interface ISaleEnquiryLineService : IDisposable
    {
        SaleEnquiryLine Create(SaleEnquiryLine s);
        void Delete(int id);
        void Delete(SaleEnquiryLine s);
        SaleEnquiryLine GetSaleEnquiryLine(int id);
        SaleEnquiryLineViewModel GetSaleEnquiryLineModel(int id);
        SaleEnquiryLine Find(int id);
        void Update(SaleEnquiryLine s);
        IEnumerable<SaleEnquiryLineIndexViewModel> GetSaleEnquiryLineList(int SaleEnquiryHeaderId);

        IQueryable<SaleEnquiryLineIndexViewModel> GetSaleEnquiryLineListForIndex(int SaleEnquiryHeaderId);
        IEnumerable<SaleEnquiryLineBalance> GetSaleEnquiryForProduct(int ProductId,int BuyerId);
        bool CheckForProductExists(int ProductId, int SaleEnquiryHEaderId, int SaleEnquiryLineId);
        bool CheckForProductExists(int ProductId, int SaleEnquiryHEaderId);
        string GetBuyerSKU(int ProductId, int SaleEnquiryHEaderId);
        SaleEnquiryLineBalance GetSaleEnquiry(int LineId);
        SaleEnquiryLineViewModel GetSaleEnquiryDetailForInvoice(int id);

        IQueryable<ComboBoxResult> GetCustomProducts(int Id, string term);
    }

    public class SaleEnquiryLineService : ISaleEnquiryLineService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;

        public SaleEnquiryLineService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public SaleEnquiryLine Create(SaleEnquiryLine S)
        {
            S.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<SaleEnquiryLine>().Insert(S);
            return S;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<SaleEnquiryLine>().Delete(id);
        }

        public void Delete(SaleEnquiryLine s)
        {
            _unitOfWork.Repository<SaleEnquiryLine>().Delete(s);
        }

        public void Update(SaleEnquiryLine s)
        {
            s.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<SaleEnquiryLine>().Update(s);
        }

        public SaleEnquiryLine GetSaleEnquiryLine(int id)
        {
            return _unitOfWork.Repository<SaleEnquiryLine>().Query().Get().Where(m => m.SaleEnquiryLineId == id).FirstOrDefault();
            //return (from p in db.SaleEnquiryLine
            //        join t in db.Products on p.ProductId equals t.ProductId into table from tab in table.DefaultIfEmpty()
            //        where p.SaleEnquiryLineId == id
            //        select new SaleEnquiryLineViewModel
            //        {
            //            Amount=p.Amount,
            //            CreatedBy=p.CreatedBy,
            //            CreatedDate=p.CreatedDate,
            //            DeliveryQty=p.DeliveryQty,
            //            DeliveryUnitId=p.DeliveryUnitId,
            //            DueDate=p.DueDate,
            //            ModifiedBy=p.ModifiedBy,
            //            ModifiedDate=p.ModifiedDate,
            //            Qty=p.Qty,
            //            Rate=p.Rate,
            //            Remark=p.Remark,
            //            SaleEnquiryHeaderId=p.SaleEnquiryHeaderId,
            //            SaleEnquiryLineId=p.SaleEnquiryLineId,
            //            Specification=p.Specification,
            //            Product=tab.ProductName,
            //        }

            //            ).FirstOrDefault();
        }
        public SaleEnquiryLineViewModel GetSaleEnquiryLineModel(int id)
        {
            //return _unitOfWork.Repository<SaleEnquiryLine>().Query().Get().Where(m => m.SaleEnquiryLineId == id).FirstOrDefault();
            return (from p in db.SaleEnquiryLine
                    join t in db.Product on p.ProductId equals t.ProductId into table
                    from tab in table.DefaultIfEmpty()
                    where p.SaleEnquiryLineId == id
                    select new SaleEnquiryLineViewModel
                    {
                        Amount = p.Amount,
                        CreatedBy = p.CreatedBy,
                        CreatedDate = p.CreatedDate,
                        DealQty = p.DealQty,
                        DealUnitId = p.DealUnitId,
                        DueDate = p.DueDate,
                        ModifiedBy = p.ModifiedBy,
                        ModifiedDate = p.ModifiedDate,
                        Qty = p.Qty,
                        Rate = p.Rate,
                        Remark = p.Remark,
                        SaleEnquiryHeaderId = p.SaleEnquiryHeaderId,
                        SaleEnquiryLineId = p.SaleEnquiryLineId,
                        Specification = p.Specification,
                        ProductName = tab.ProductName,
                    }

                        ).FirstOrDefault();
        }
        public SaleEnquiryLine Find(int id)
        {
            return _unitOfWork.Repository<SaleEnquiryLine>().Find(id);
        }

        public IEnumerable<SaleEnquiryLineIndexViewModel> GetSaleEnquiryLineList(int SaleEnquiryHeaderId)
        {
            //return _unitOfWork.Repository<SaleEnquiryLine>().Query().Include(m => m.Product).Include(m=>m.SaleEnquiryHeader).Get().Where(m => m.SaleEnquiryHeaderId == SaleEnquiryHeaderId).ToList();

            return (from p in db.SaleEnquiryLine
                    where p.SaleEnquiryHeaderId == SaleEnquiryHeaderId
                    select new SaleEnquiryLineIndexViewModel
                    {
                        Amount = p.Amount,
                        DealQty = p.DealQty,
                        DealUnitId = p.DealUnitId,
                        DueDate = p.DueDate,
                        ProductName = p.Product.ProductName,
                        Qty = p.Qty,
                        Rate = p.Rate,
                        SaleEnquiryHeaderId = p.SaleEnquiryHeaderId,
                        SaleEnquiryLineId = p.SaleEnquiryLineId,
                        SaleEnquiryHeaderDocNo=p.SaleEnquiryHeader.DocNo
                    }


                );


        }

        public IQueryable<SaleEnquiryLineIndexViewModel> GetSaleEnquiryLineListForIndex(int SaleEnquiryHeaderId)
        {

            var temp = from p in db.SaleEnquiryLine
                       join Pe in db.SaleEnquiryLineExtended on p.SaleEnquiryLineId equals Pe.SaleEnquiryLineId into SaleEnquiryLineTable from SaleEnquiryLineTab in SaleEnquiryLineTable.DefaultIfEmpty()
                       join t in db.ViewSaleEnquiryBalance on p.SaleEnquiryLineId equals t.SaleEnquiryLineId into table from svb in table.DefaultIfEmpty()
                       join t1 in db.SaleEnquiryHeader on p.SaleEnquiryHeaderId equals t1.SaleEnquiryHeaderId into table1 from tab1 in table1.DefaultIfEmpty()
                       join pb in db.ViewProductBuyer on new { p.ProductId, BuyerId = tab1.SaleToBuyerId } equals new { pb.ProductId, BuyerId = pb.BuyerId } into table2
                       from tab2 in table2.DefaultIfEmpty()
                       orderby p.SaleEnquiryLineId
                       where p.SaleEnquiryHeaderId==SaleEnquiryHeaderId
                       select new SaleEnquiryLineIndexViewModel
                       {
                           BuyerSku=tab2.ProductName,
                           DealQty = p.DealQty,
                           DealUnitId=p.DealUnitId,
                           Specification=p.Specification,
                           Rate=p.Rate,
                           Amount = p.Amount,
                           DueDate = p.DueDate,
                           ProductName = tab2.ProductName,
                           ProductGroup = SaleEnquiryLineTab.ProductGroup,
                           Size = SaleEnquiryLineTab.Size,
                           Colour = SaleEnquiryLineTab.Colour,
                           ProductQuality = SaleEnquiryLineTab.ProductQuality,
                           Dimension1Name = p.Dimension1.Dimension1Name,
                           Dimension2Name = p.Dimension2.Dimension2Name,
                           Qty = p.Qty,
                           SaleEnquiryHeaderId = p.SaleEnquiryHeaderId,
                           SaleEnquiryLineId = p.SaleEnquiryLineId,
                           Remark=p.Remark,
                           ProgressPerc = (p.Qty == 0 ? 0 : (int)((((p.Qty - ((decimal?)svb.BalanceQty ?? (decimal)0)) / p.Qty) * 100))),
                           unitDecimalPlaces=p.Unit.DecimalPlaces,
                           UnitId=p.UnitId,
                       };
            return temp;
        }

        

        public IEnumerable<SaleEnquiryLineBalance> GetSaleEnquiryForProduct(int ProductId,int BuyerId)
        {
            return (from p in db.ViewSaleEnquiryBalance
                    where p.ProductId == ProductId && p.BuyerId == BuyerId && p.BalanceQty > 0
                    select new SaleEnquiryLineBalance
                    {
                        SaleEnquiryDocNo = p.SaleEnquiryNo,
                        SaleEnquiryLineId = p.SaleEnquiryLineId
                    }
                ).ToList();

        }
        public SaleEnquiryLineBalance GetSaleEnquiry(int LineId)
        {
            //var temp = _unitOfWork.Repository<SaleEnquiryLine>().Query()
            //    .Include(m => m.SaleEnquiryHeader)
            //    .Include(m => m.Product)
            //    .Get().Where(m => m.ProductId == ProductId);

            //List<SaleEnquiryLineBalance> SaleEnquiryLineBalance = new List<SaleEnquiryLineBalance>();
            //foreach (var item in temp)
            //{
            //    SaleEnquiryLineBalance.Add(new SaleEnquiryLineBalance
            //    {
            //        SaleEnquiryLineId = item.SaleEnquiryLineId,
            //        SaleEnquiryDocNo = item.SaleEnquiryHeader.DocNo
            //    });
            //}

            return (from p in db.SaleEnquiryLine
                    join t in db.SaleEnquiryHeader on p.SaleEnquiryHeaderId equals t.SaleEnquiryHeaderId into table from tab in table
                    where p.SaleEnquiryLineId == LineId
                    select new SaleEnquiryLineBalance
                    {
                        SaleEnquiryLineId = p.SaleEnquiryLineId,
                        SaleEnquiryDocNo = tab.DocNo
                    }


                ).FirstOrDefault();

        }

        public bool CheckForProductExists(int ProductId, int SaleEnquiryHeaderId, int SaleEnquiryLineId)
        {

            SaleEnquiryLine temp = (from p in db.SaleEnquiryLine
                                  where p.ProductId == ProductId && p.SaleEnquiryHeaderId == SaleEnquiryHeaderId &&p.SaleEnquiryLineId!=SaleEnquiryLineId
                                  select p).FirstOrDefault();
            if (temp != null)
                return true;
            else return false;
        }
        public bool CheckForProductExists(int ProductId, int SaleEnquiryHeaderId)
        {

            SaleEnquiryLine temp = (from p in db.SaleEnquiryLine
                                  where p.ProductId == ProductId && p.SaleEnquiryHeaderId == SaleEnquiryHeaderId
                                  select p).FirstOrDefault();
            if (temp != null)
                return true;
            else return false;
        }

        public string GetBuyerSKU(int ProductId, int SaleEnquiryHEaderId)
        {
            int BuyerID = new SaleEnquiryHeaderService(_unitOfWork).Find(SaleEnquiryHEaderId).SaleToBuyerId;

            string BuyerSku = (from p in db.ProductBuyer
                               where p.BuyerId == BuyerID && p.ProductId == ProductId
                               select p.BuyerSku
                                 ).FirstOrDefault();

            if(BuyerSku==null)
            {
                BuyerSku = "";
            }

            return BuyerSku;
        }

        public SaleEnquiryLineViewModel GetSaleEnquiryDetailForInvoice(int id)
        {

            return (from t in db.ViewSaleEnquiryBalance 
                    join p in db.SaleEnquiryLine on t.SaleEnquiryLineId equals p.SaleEnquiryLineId 
                    where p.SaleEnquiryLineId==id
                    select new SaleEnquiryLineViewModel
                    {
                        Amount = p.Amount,
                        DealQty = p.DealQty,
                        DealUnitId = p.DealUnitId,
                        DueDate = p.DueDate,
                        ProductId = p.ProductId,
                        Qty = t.BalanceQty,
                        Rate = p.Rate,
                        Remark = p.Remark,
                        Dimension1Id=p.Dimension1Id,
                        Dimension2Id=p.Dimension2Id,
                        Dimension1Name=p.Dimension1.Dimension1Name,
                        Dimension2Name=p.Dimension2.Dimension2Name,
                        SaleEnquiryDocNo = p.SaleEnquiryHeader.DocNo,
                        SaleEnquiryHeaderId = p.SaleEnquiryHeaderId,
                        SaleEnquiryLineId = p.SaleEnquiryLineId,
                        Specification = p.Specification,
                        UnitConversionMultiplier = p.UnitConversionMultiplier,
                        UnitId = p.Product.UnitId,
                        UnitName=p.Product.Unit.UnitName,
                        
                    }

                        ).FirstOrDefault();


        }

        public IQueryable<ComboBoxResult> GetCustomProducts(int Id, string term)
        {

            var SaleEnquiry = new SaleEnquiryHeaderService(_unitOfWork).Find(Id);

            var settings = new SaleEnquirySettingsService(_unitOfWork).GetSaleEnquirySettings(SaleEnquiry.DocTypeId, SaleEnquiry.DivisionId, SaleEnquiry.SiteId);

            string[] ProductTypes = null;
            if (!string.IsNullOrEmpty(settings.filterProductTypes)) { ProductTypes = settings.filterProductTypes.Split(",".ToCharArray()); }
            else { ProductTypes = new string[] { "NA" }; }

            string[] Products = null;
            if (!string.IsNullOrEmpty(settings.filterProducts)) { Products = settings.filterProducts.Split(",".ToCharArray()); }
            else { Products = new string[] { "NA" }; }

            string[] ProductGroups = null;
            if (!string.IsNullOrEmpty(settings.filterProductGroups)) { ProductGroups = settings.filterProductGroups.Split(",".ToCharArray()); }
            else { ProductGroups = new string[] { "NA" }; }

            return (from pb in db.ViewProductBuyer
                    join Pt in db.Product on pb.ProductId equals Pt.ProductId into ProductTable from ProductTab in ProductTable.DefaultIfEmpty()
                    where (string.IsNullOrEmpty(settings.filterProductTypes) ? 1 == 1 : ProductTypes.Contains(ProductTab.ProductGroup.ProductTypeId.ToString()))
                    && (string.IsNullOrEmpty(settings.filterProducts) ? 1 == 1 : Products.Contains(ProductTab.ProductId.ToString()))
                    && (string.IsNullOrEmpty(settings.filterProductGroups) ? 1 == 1 : ProductGroups.Contains(ProductTab.ProductGroupId.ToString()))
                    && (string.IsNullOrEmpty(term) ? 1 == 1 : pb.ProductName.ToLower().Contains(term.ToLower()))
                    && pb.BuyerId == SaleEnquiry.SaleToBuyerId
                    orderby pb.ProductName
                    select new ComboBoxResult
                    {
                        id = pb.ProductId.ToString(),
                        text = pb.ProductName,
                        AProp1 = "Design : " + pb.ProductGroup,
                        AProp2 = "Size : " + pb.Size,
                        TextProp1 = "Colour : " + pb.Colour,
                        TextProp2 = "Quality : " + pb.ProductQuality
                    });
        }

        public void Dispose()
        {
        }
    }
}
