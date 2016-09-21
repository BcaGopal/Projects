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
using System.Data.SqlClient;
using System.Configuration;


namespace Service
{
    public interface IPackingLineService : IDisposable
    {
        PackingLine Create(PackingLine s);
        void Delete(int id);
        void Delete(PackingLine s);
        void Update(PackingLine s);
        PackingLine Find(int id);

        void DeleteForPackingHeaderId(int PackingHeaderid);
        string FGetRandonNoForLabel();

        IQueryable<PackingLine> GetPackingLineList();
        IQueryable<PackingLine> GetPackingLineForHeaderId(int PackingHeaderId);
        PackingLine GetPackingLineForLineId(int PackingLineId);
        PackingLineViewModel GetPackingLineViewModelForLineId(int PackingLineId);
        //IQueryable<PackingLineViewModel> GetPackingLineViewModelForHeaderId(int PackingHeaderId);
        IEnumerable<PackingLineViewModel> GetPackingLineViewModelForHeaderId(int PackingHeaderId);
        //PendingOrderListForPacking FGetFifoSaleOrder(int ProductId, int BuyerId);
        ProductAreaDetail FGetProductArea(int ProductId);
        PackingBaleNo FGetNewPackingBaleNo(int PackingHeaderId, int? ProductInvoiceGroupId, int? SaleOrderLineId, int BaleNoPatternId, decimal DealQty);
        Decimal FGetStockForPacking(int ProductId, int SiteId, int PackingLineId);
        Decimal FGetQCStockForPacking(int ProductId, int SiteId, Decimal StockInHand);
        IEnumerable<PendingOrderListForPacking> FGetPendingOrderListForPacking(int ProductId, int BuyerId, int PackingLineId);

        IEnumerable<PendingDeliveryOrderListForPacking> FGetPendingDeliveryOrderListForPacking(int ProductId, int BuyerId, int PackingLineId);
        Decimal FGetPendingOrderQtyForPacking(int SaleOrderLineId, int PackingLineId);

        Decimal FGetPendingOrderQtyForDispatch(int SaleOrderilneId);

        Decimal FGetPendingDeliveryOrderQtyForPacking(int SaleDeliveryOrderLineId, int PackingLineId);

        Decimal FGetPendingDeliveryOrderQtyForDispatch(int SaleDeliveryOrderilneId);

        bool FSaleOrderProductMatchWithPacking(int SaleOrderLineId, int ProductId);


    }

    public class PackingLineService : IPackingLineService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;

        public PackingLineService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
        }

        public PackingLine Create(PackingLine S)
        {
            S.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<PackingLine>().Insert(S);
            return S;
        }

        public PackingLine Find(int id)
        {
            return _unitOfWork.Repository<PackingLine>().Find(id);
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<PackingLine>().Delete(id);
        }

        public void Delete(PackingLine s)
        {
            _unitOfWork.Repository<PackingLine>().Delete(s);
        }

        public void DeleteForPackingHeaderId(int PackingHeaderid)
        {
            var PackingLine = from L in db.PackingLine where L.PackingHeaderId == PackingHeaderid select new { PackingLindId = L.PackingLineId };

            foreach (var item in PackingLine)
            {
                Delete(item.PackingLindId);
            }
        }

        public void Update(PackingLine s)
        {
            s.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<PackingLine>().Update(s);
        }

        public IQueryable<PackingLine> GetPackingLineList()
        {
            return _unitOfWork.Repository<PackingLine>().Query().Get();
        }

        public IQueryable<PackingLine> GetPackingLineForHeaderId(int PackingHeaderId)
        {
            return _unitOfWork.Repository<PackingLine>().Query().Get().Where(m => m.PackingHeaderId == PackingHeaderId);
        }

        public PackingLine GetPackingLineForLineId(int PackingLineId)
        {
            return _unitOfWork.Repository<PackingLine>().Query().Get().Where(m => m.PackingLineId == PackingLineId).FirstOrDefault();
        }

        public string FGetRandonNoForLabel()
        {
            string RandomNo = "";
            Random random = new Random();
            int i = 0;
            string StrSalt = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            for (i = 1; i <= 5; i++)
            {
                RandomNo += StrSalt[random.Next(0, StrSalt.Length)];
            }


            return RandomNo;
        }

        //public IQueryable<PackingLineViewModel> GetPackingLineViewModelForHeaderId(int PackingHeaderId)
        //{
        //    IQueryable<PackingLineViewModel> packinglineviewmodel = from L in db.PackingLine
        //            join P in db.Product on L.ProductId equals P.ProductId into ProductTable from Producttab in ProductTable.DefaultIfEmpty()
        //            join S in db.SaleOrderLine on L.SaleOrderLineId equals S.SaleOrderLineId into SaleOrderLineTable from SaleOrderLineTab in SaleOrderLineTable.DefaultIfEmpty()
        //            join So in db.SaleOrderHeader on SaleOrderLineTab.SaleOrderHeaderId equals So.SaleOrderHeaderId into SaleOrderHeaderTable
        //            from SaleOrderHeaderTab in SaleOrderHeaderTable.DefaultIfEmpty()
        //            join Du in db.Units on L.DealUnitId equals Du.UnitId into DeliveryUnitTable from DeliveryUnitTab in DeliveryUnitTable.DefaultIfEmpty()
        //            join Pu in db.ProductUid on L.ProductUidId equals Pu.ProductUIDId into ProductUidTable
        //            from ProductUidTab in ProductUidTable.DefaultIfEmpty()
        //            orderby L.PackingLineId
        //            where L.PackingHeaderId == PackingHeaderId
        //            select new PackingLineViewModel
        //            {
        //                PackingHeaderId = L.PackingHeaderId,
        //                PackingLineId = L.PackingLineId,
        //                ProductUidId = L.ProductUidId,
        //                ProductUidName = ProductUidTab.ProductUidName,
        //                ProductId = L.ProductId,
        //                ProductName = Producttab.ProductName,
        //                Qty = L.Qty,
        //                SaleOrderLineId = L.SaleOrderLineId,
        //                SaleOrderNo = SaleOrderHeaderTab.DocNo,
        //                DealUnitId = L.DealUnitId,
        //                DealUnitName = DeliveryUnitTab.UnitName,
        //                DealQty = L.DealQty,
        //                BaleNo = L.BaleNo,
        //                GrossWeight = L.GrossWeight,
        //                NetWeight = L.NetWeight,
        //                Remark = L.Remark
        //            };


        //    return packinglineviewmodel;
        //}

        public IEnumerable<PackingLineViewModel> GetPackingLineViewModelForHeaderId(int PackingHeaderId)
        {
            IEnumerable<PackingLineViewModel> packinglineviewmodel = (from L in db.PackingLine
                                                                      join P in db.Product on L.ProductId equals P.ProductId into ProductTable
                                                                      from Producttab in ProductTable.DefaultIfEmpty()
                                                                      join S in db.SaleOrderLine on L.SaleOrderLineId equals S.SaleOrderLineId into SaleOrderLineTable
                                                                      from SaleOrderLineTab in SaleOrderLineTable.DefaultIfEmpty()
                                                                      join So in db.SaleOrderHeader on SaleOrderLineTab.SaleOrderHeaderId equals So.SaleOrderHeaderId into SaleOrderHeaderTable
                                                                      from SaleOrderHeaderTab in SaleOrderHeaderTable.DefaultIfEmpty()
                                                                      join Du in db.Units on L.DealUnitId equals Du.UnitId into DeliveryUnitTable
                                                                      from DeliveryUnitTab in DeliveryUnitTable.DefaultIfEmpty()
                                                                      join Pu in db.ProductUid on L.ProductUidId equals Pu.ProductUIDId into ProductUidTable
                                                                      from ProductUidTab in ProductUidTable.DefaultIfEmpty()
                                                                      orderby L.PackingLineId
                                                                      where L.PackingHeaderId == PackingHeaderId
                                                                      select new PackingLineViewModel
                                                                      {
                                                                          PackingHeaderId = L.PackingHeaderId,
                                                                          PackingLineId = L.PackingLineId,
                                                                          ProductUidId = L.ProductUidId,
                                                                          ProductUidName = ProductUidTab.ProductUidName,
                                                                          ProductId = L.ProductId,
                                                                          ProductName = Producttab.ProductName,
                                                                          Qty = L.Qty,
                                                                          SaleOrderLineId = L.SaleOrderLineId,
                                                                          SaleOrderNo = SaleOrderHeaderTab.DocNo,
                                                                          DealUnitId = L.DealUnitId,
                                                                          DealUnitName = DeliveryUnitTab.UnitName,
                                                                          DealQty = L.DealQty,
                                                                          BaleNo = L.BaleNo,
                                                                          GrossWeight = L.GrossWeight,
                                                                          NetWeight = L.NetWeight,
                                                                          Remark = L.Remark
                                                                      }).ToList();

            double x = 0;
            var p = packinglineviewmodel.OrderBy(sx => double.TryParse(sx.BaleNo, out x) ? x : 0);


            return p;
        }

        public PackingLineViewModel GetPackingLineViewModelForLineId(int PackingLineId)
        {
            return (from L in db.PackingLine
                    join P in db.FinishedProduct on L.ProductId equals P.ProductId into ProductTable
                    from ProductTab in ProductTable.DefaultIfEmpty()
                    join Pig in db.ProductInvoiceGroup on ProductTab.ProductInvoiceGroupId equals Pig.ProductInvoiceGroupId into ProductInvoiceGroupTable
                    from ProductInvoiceGroupTab in ProductInvoiceGroupTable.DefaultIfEmpty()
                    join S in db.SaleOrderLine on L.SaleOrderLineId equals S.SaleOrderLineId into SaleOrderLineTable
                    from SaleOrderLineTab in SaleOrderLineTable.DefaultIfEmpty()
                    join So in db.SaleOrderHeader on SaleOrderLineTab.SaleOrderHeaderId equals So.SaleOrderHeaderId into SaleOrderHeaderTable
                    from SaleOrderHeaderTab in SaleOrderHeaderTable.DefaultIfEmpty()
                    join Du in db.Units on L.DealUnitId equals Du.UnitId into DeliveryUnitTable
                    from DeliveryUnitTab in DeliveryUnitTable.DefaultIfEmpty()
                    join Pu in db.ProductUid on L.ProductUidId equals Pu.ProductUIDId into ProductUidTable
                    from ProductUidTab in ProductUidTable.DefaultIfEmpty()
                    orderby L.PackingLineId
                    where L.PackingLineId == PackingLineId
                    select new PackingLineViewModel
                    {
                        PackingHeaderId = L.PackingHeaderId,
                        PackingLineId = L.PackingLineId,
                        ProductUidId = L.ProductUidId,
                        ProductUidName = ProductUidTab.ProductUidName,
                        ProductId = L.ProductId,
                        ProductName = ProductTab.ProductName,
                        ProductInvoiceGroupId = ProductTab.ProductInvoiceGroupId,
                        ProductInvoiceGroupName = ProductInvoiceGroupTab.ProductInvoiceGroupName,
                        Qty = L.Qty,
                        SaleOrderLineId = L.SaleOrderLineId,
                        SaleOrderNo = SaleOrderHeaderTab.DocNo,
                        SaleDeliveryOrderLineId = L.SaleDeliveryOrderLineId,
                        SaleDeliveryOrderNo = L.SaleDeliveryOrderLine.SaleDeliveryOrderHeader.DocNo,
                        DealUnitId = L.DealUnitId,
                        DealUnitName = DeliveryUnitTab.UnitName,
                        DealQty = L.DealQty,
                        BaleNo = L.BaleNo,
                        GrossWeight = L.GrossWeight,
                        NetWeight = L.NetWeight,
                        Remark = L.Remark,
                        ImageFolderName = ProductTab.ImageFolderName,
                        ImageFileName = ProductTab.ImageFileName,
                        CreatedBy = L.CreatedBy,
                        CreatedDate = L.CreatedDate
                    }).FirstOrDefault();
        }

        //public ProductUidDetail FGetProductUidDetail(string ProductUidNo)
        //{
        //    ProductUidDetail UidDetail = (from Pu in db.ProductUid
        //            join P in db.Products on Pu.ProductId equals P.ProductId into ProductTable
        //            from Producttab in ProductTable.DefaultIfEmpty()
        //            where Pu.ProductUidName == ProductUidNo
        //            select new ProductUidDetail
        //            {
        //                ProductUidId = Pu.ProductUIDId,
        //                ProductId = Pu.ProductId,
        //                ProductName = Producttab.ProductName
        //            }).FirstOrDefault();

        //    return UidDetail;
        //}

        public PendingOrderListForPacking FGetFifoSaleOrder(int ProductId, int BuyerId)
        {
            var Packing = from L in db.PackingLine
                          where L.ProductId == ProductId
                          group new { L } by new { L.SaleOrderLineId } into Result
                          select new
                          {
                              SaleOrderLineId = Result.Key.SaleOrderLineId,
                              PackedQty = Result.Sum(i => i.L.Qty)
                          };

            PendingOrderListForPacking FifoSaleOrderLine = (from L in db.SaleOrderLine
                                                            join H in db.SaleOrderHeader on L.SaleOrderHeaderId equals H.SaleOrderHeaderId into SaleOrderHeaderTable
                                                            from SaleOrderHeaderTab in SaleOrderHeaderTable.DefaultIfEmpty()
                                                            join VPack in Packing on L.SaleOrderLineId equals VPack.SaleOrderLineId into VPackTable
                                                            from VPackTab in VPackTable.DefaultIfEmpty()
                                                            where SaleOrderHeaderTab.SaleToBuyerId == BuyerId && L.ProductId == ProductId
                                                            group new { L, SaleOrderHeaderTab, VPackTab } by new { L.SaleOrderLineId } into Result
                                                            where ((Decimal?)Result.Sum(i => i.L.Qty) ?? 0) - ((Decimal?)Result.Sum(i => i.VPackTab.PackedQty) ?? 0) > 0
                                                            orderby Result.Max(i => i.SaleOrderHeaderTab.Priority) descending, Result.Max(i => i.SaleOrderHeaderTab.DocDate)
                                                            select new PendingOrderListForPacking
                                                            {
                                                                SaleOrderLineId = Result.Key.SaleOrderLineId,
                                                                SaleOrderNo = Result.Max(i => i.SaleOrderHeaderTab.DocNo)
                                                            }).Take(1).FirstOrDefault();

            return FifoSaleOrderLine;
        }




        //public IEnumerable<FiFoSaleOrderForProduct> FGetFifoSaleOrderList(int ProductId, int BuyerId)
        //{
        //    var Packing = from L in db.PackingLine
        //                  where L.ProductId == ProductId 
        //                  group new { L } by new { L.SaleOrderLineId } into Result
        //                  select new
        //                  {
        //                      SaleOrderLineId = Result.Key.SaleOrderLineId,
        //                      PackedQty = Result.Sum(i => i.L.Qty)
        //                  };

        //    IQueryable<FiFoSaleOrderForProduct> FifoSaleOrderLineList = (from L in db.SaleOrderLine
        //                                                                 join H in db.SaleOrderHeader on L.SaleOrderHeaderId equals H.SaleOrderHeaderId into SaleOrderHeaderTable
        //                                                                 from SaleOrderHeaderTab in SaleOrderHeaderTable.DefaultIfEmpty()
        //                                                                 join VPack in Packing on L.SaleOrderLineId equals VPack.SaleOrderLineId into VPackTable
        //                                                                 from VPackTab in VPackTable.DefaultIfEmpty()
        //                                                                 where SaleOrderHeaderTab.SaleToBuyerId == BuyerId && L.ProductId == ProductId
        //                                                                 group new { L, SaleOrderHeaderTab, VPackTab } by new { L.SaleOrderLineId } into Result
        //                                                                 where ((Decimal?)Result.Sum(i => i.L.Qty) ?? 0) - ((Decimal?)Result.Sum(i => i.VPackTab.PackedQty) ?? 0) > 0
        //                                                                 orderby Result.Max(i => i.SaleOrderHeaderTab.Priority) descending, Result.Max(i => i.SaleOrderHeaderTab.DocDate) descending
        //                                                                 select new FiFoSaleOrderForProduct
        //                                                                 {
        //                                                                     SaleOrderLineId = Result.Key.SaleOrderLineId,
        //                                                                     SaleOrderNo = Result.Max(i => i.SaleOrderHeaderTab.DocNo)
        //                                                                 });


        //    return FifoSaleOrderLineList;
        //}




        public ProductAreaDetail FGetProductArea(int ProductId)
        {

            int a = (int)ProductSizeTypeConstants.StandardSize;
            ProductAreaDetail productarea = (from Ps in db.ProductSize
                                             join S in db.Size on Ps.SizeId equals S.SizeId into SizeTable
                                             from SizeTab in SizeTable.DefaultIfEmpty()
                                             where Ps.ProductId == ProductId && Ps.ProductSizeTypeId == ((int)ProductSizeTypeConstants.StandardSize)
                                             select new ProductAreaDetail
                                             {
                                                 ProductId = Ps.ProductId,
                                                 ProductArea = SizeTab.Area
                                             }).FirstOrDefault();
            return productarea;
        }







        public PackingBaleNo FGetNewPackingBaleNo(int PackingHeaderId, int? ProductInvoiceGroupId, int? SaleOrderLineId, int BaleNoPatternId, decimal DealQty)
        {
            int x = 0;
            int MaxBaleNo = 0;

            if (BaleNoPatternId == (int)(BaleNoPatternConstants.SaleOrder))
            {
                int SaleOrderHeaderId = (from L in db.SaleOrderLine where L.SaleOrderLineId == SaleOrderLineId select new { SaleOrderHeaderId = L.SaleOrderHeaderId }).FirstOrDefault().SaleOrderHeaderId;

                var BaleNoList = (from L in db.PackingLine
                                  join Sl in db.SaleOrderLine on L.SaleOrderLineId equals Sl.SaleOrderLineId into SaleOrderLineTable
                                  from SaleOrderLineTab in SaleOrderLineTable.DefaultIfEmpty()
                                  where L.PackingHeaderId == PackingHeaderId && SaleOrderLineTab.SaleOrderHeaderId == SaleOrderHeaderId
                                  select new
                                  {
                                      BaleNo = L.BaleNo
                                  }).ToList();

                if (BaleNoList != null && BaleNoList.Count != 0)
                {
                    MaxBaleNo = BaleNoList.Select(sx => int.TryParse(sx.BaleNo, out x) ? x : 0).Max();
                }
            }
            else if (BaleNoPatternId == (int)(BaleNoPatternConstants.SaleOrderSize ))
            {
                int SaleOrderHeaderId = (from L in db.SaleOrderLine where L.SaleOrderLineId == SaleOrderLineId select new { SaleOrderHeaderId = L.SaleOrderHeaderId }).FirstOrDefault().SaleOrderHeaderId;

                var BaleNoList = (from L in db.PackingLine
                                  join Sl in db.SaleOrderLine on L.SaleOrderLineId equals Sl.SaleOrderLineId into SaleOrderLineTable
                                  from SaleOrderLineTab in SaleOrderLineTable.DefaultIfEmpty()
                                  where L.PackingHeaderId == PackingHeaderId && SaleOrderLineTab.SaleOrderHeaderId == SaleOrderHeaderId && L.DealQty == DealQty
                                  select new
                                  {
                                      BaleNo = L.BaleNo
                                  }).ToList();

                if (BaleNoList != null && BaleNoList.Count != 0)
                {
                    MaxBaleNo = BaleNoList.Select(sx => int.TryParse(sx.BaleNo, out x) ? x : 0).Max();
                }
            }

            else if (BaleNoPatternId == (int)(BaleNoPatternConstants.SmallSizes))
            {               
                var BaleNoList = (from L in db.PackingLine
                                  join P in db.FinishedProduct on L.ProductId equals P.ProductId into ProductTable
                                  from ProductTab in ProductTable.DefaultIfEmpty()
                                  where L.PackingHeaderId == PackingHeaderId && ProductTab.ProductInvoiceGroupId == ProductInvoiceGroupId && L.DealQty==DealQty
                                  select new
                                  {
                                      BaleNo = L.BaleNo
                                  }).ToList();

                if (BaleNoList != null && BaleNoList.Count != 0)
                {
                    MaxBaleNo = BaleNoList.Select(sx => int.TryParse(sx.BaleNo, out x) ? x : 0).Max();
                    MaxBaleNo -= 1;
                }
            }
            else
            {
                var BaleNoList = (from L in db.PackingLine
                                  join P in db.FinishedProduct on L.ProductId equals P.ProductId into ProductTable
                                  from ProductTab in ProductTable.DefaultIfEmpty()
                                  where L.PackingHeaderId == PackingHeaderId && ProductTab.ProductInvoiceGroupId == ProductInvoiceGroupId
                                  select new
                                  {
                                      BaleNo = L.BaleNo
                                  }).ToList();

                if (BaleNoList != null && BaleNoList.Count != 0)
                {
                    MaxBaleNo = BaleNoList.Select(sx => int.TryParse(sx.BaleNo, out x) ? x : 0).Max();
                }

            }


            PackingBaleNo packingbaleno = new PackingBaleNo();
            packingbaleno.PackingHeaderId = PackingHeaderId;
            packingbaleno.NewBaleNo = MaxBaleNo + 1;

            return packingbaleno;
        }

        public IEnumerable<PendingOrderListForPacking> FGetPendingOrderListForPacking(int ProductId, int BuyerId, int PackingLineId)
        {
            SqlParameter SqlParameterProductId = new SqlParameter("@ProductId", ProductId);
            SqlParameter SqlParameterBuyerId = new SqlParameter("@BuyerId", BuyerId);
            SqlParameter SqlParameterPackingLineId = new SqlParameter("@PackingLineId", PackingLineId);

            IEnumerable<PendingOrderListForPacking> FifoSaleOrderLineList = db.Database.SqlQuery<PendingOrderListForPacking>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".ProcGetPendingOrderListForPacking @ProductId, @BuyerId, @PackingLineId", SqlParameterProductId, SqlParameterBuyerId, SqlParameterPackingLineId).ToList();

            return FifoSaleOrderLineList;
        }


        public IEnumerable<PendingDeliveryOrderListForPacking> FGetPendingDeliveryOrderListForPacking(int ProductId, int BuyerId, int PackingLineId)
        {
            SqlParameter SqlParameterProductId = new SqlParameter("@ProductId", ProductId);
            SqlParameter SqlParameterBuyerId = new SqlParameter("@BuyerId", BuyerId);
            SqlParameter SqlParameterPackingLineId = new SqlParameter("@PackingLineId", PackingLineId);

            IEnumerable<PendingDeliveryOrderListForPacking> FifoDeliveryOrderLineList = db.Database.SqlQuery<PendingDeliveryOrderListForPacking>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".ProcGetPendingDeliveryOrderListForPacking @ProductId, @BuyerId, @PackingLineId", SqlParameterProductId, SqlParameterBuyerId, SqlParameterPackingLineId).ToList();

            return FifoDeliveryOrderLineList;
        }

        public Decimal FGetStockForPacking(int ProductId, int SiteId, int PackingLineId)
        {
            SqlParameter SqlParameterProductId = new SqlParameter("@ProductId", ProductId);
            SqlParameter SqlParameterSiteId = new SqlParameter("@SiteId", SiteId);
            SqlParameter SqlParameterPackingLineId = new SqlParameter("@PackingLineId", PackingLineId);

            StockAvailableForPacking StockAvailableForPacking = db.Database.SqlQuery<StockAvailableForPacking>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".ProcGetStockForPacking @ProductId, @SiteId, @PackingLineId", SqlParameterProductId, SqlParameterSiteId, SqlParameterPackingLineId).FirstOrDefault();

            if (StockAvailableForPacking != null)
            {
                return StockAvailableForPacking.Qty;
            }
            else
            {
                return 0;
            }
        }


        public Decimal FGetQCStockForPacking(int ProductId, int SiteId, Decimal StockInHand)
        {
            SqlParameter SqlParameterProductId = new SqlParameter("@ProductId", ProductId);
            SqlParameter SqlParameterSiteId = new SqlParameter("@SiteId", SiteId);
            SqlParameter SqlParameterStockIndHand = new SqlParameter("@StockInHand", StockInHand);

            StockAvailableForPacking StockAvailableForPacking = db.Database.SqlQuery<StockAvailableForPacking>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".ProcGetQCStockForPacking @ProductId, @SiteId, @StockInHand", SqlParameterProductId, SqlParameterSiteId, SqlParameterStockIndHand).FirstOrDefault();

            if (StockAvailableForPacking != null)
            {
                return StockAvailableForPacking.Qty;
            }
            else
            {
                return 0;
            }
        }





        public Decimal FGetPendingOrderQtyForPacking(int SaleOrderilneId, int PackingLineId)
        {
            SqlParameter SqlParameterSaleOrderLineId = new SqlParameter("@SaleOrderLineId", SaleOrderilneId);
            SqlParameter SqlParameterPackingLineId = new SqlParameter("@PackingLineId", PackingLineId);

            PendingOrderQtyForPacking PendingOrderQtyForPacking = db.Database.SqlQuery<PendingOrderQtyForPacking>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".ProcGetPendingOrderQtyForPacking @SaleOrderLineId, @PackingLineId", SqlParameterSaleOrderLineId, SqlParameterPackingLineId).FirstOrDefault();

            if (PendingOrderQtyForPacking != null)
            {
                return PendingOrderQtyForPacking.Qty;
            }
            else
            {
                return 0;
            }

        }

        public bool FSaleOrderProductMatchWithPacking(int SaleOrderLineId, int ProductId)
        {
            var temp = (from L in db.SaleOrderLine
                       where L.SaleOrderLineId == SaleOrderLineId
                       select new {
                           ProductId = L.ProductId
                       }).FirstOrDefault();

            if (temp != null)
            {
                if (temp.ProductId == ProductId)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else{
                return false;
            }
        }


        public Decimal FGetPendingOrderQtyForDispatch(int SaleOrderilneId)
        {
            var temp = (from L in db.ViewSaleOrderBalance
                        where L.SaleOrderLineId == SaleOrderilneId
                        select L).FirstOrDefault();

            if (temp != null)
            {
                return temp.BalanceQty;
            }
            else
            {
                return 0;
            }
        }



        public Decimal FGetPendingDeliveryOrderQtyForPacking(int SaleDeliveryOrderilneId, int PackingLineId)
        {
            SqlParameter SqlParameterSaleDeliveryOrderLineId = new SqlParameter("@SaleDeliveryOrderLineId", SaleDeliveryOrderilneId);
            SqlParameter SqlParameterPackingLineId = new SqlParameter("@PackingLineId", PackingLineId);

            PendingOrderQtyForPacking PendingDeliveryOrderQtyForPacking = db.Database.SqlQuery<PendingOrderQtyForPacking>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".ProcGetPendingDeliveryOrderQtyForPacking @SaleDeliveryOrderLineId, @PackingLineId", SqlParameterSaleDeliveryOrderLineId, SqlParameterPackingLineId).FirstOrDefault();

            if (PendingDeliveryOrderQtyForPacking != null)
            {
                return PendingDeliveryOrderQtyForPacking.Qty;
            }
            else
            {
                return 0;
            }

        }


        public Decimal FGetPendingDeliveryOrderQtyForDispatch(int SaleDeliveryOrderilneId)
        {
            var temp = (from L in db.ViewSaleDeliveryOrderBalance
                        where L.SaleDeliveryOrderLineId == SaleDeliveryOrderilneId
                        select L).FirstOrDefault();

            if (temp != null)
            {
                return temp.BalanceQty;
            }
            else
            {
                return 0;
            }
        }

    }







    public class PendingOrderListForPacking
    {
        public int SaleOrderLineId { get; set; }
        public string SaleOrderNo { get; set; }
    }

    public class PendingDeliveryOrderListForPacking
    {
        public int SaleDeliveryOrderLineId { get; set; }
        public string SaleDeliveryOrderNo { get; set; }
        public int? ShipMethodId { get; set; }
        public int? OtherBuyerDeliveryOrders { get; set; }
    }

    public class ProductAreaDetail
    {
        public int ProductId { get; set; }
        public Decimal ProductArea { get; set; }
    }

    public class PackingBaleNo
    {
        public int PackingHeaderId { get; set; }
        public int NewBaleNo { get; set; }
    }

    public class StockAvailableForPacking
    {
        public int ProductId { get; set; }
        public Decimal Qty { get; set; }
    }

    public class PendingOrderQtyForPacking
    {
        public Decimal Qty { get; set; }
    }


}

