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
using Model.ViewModels;

namespace Service
{
    public interface IBomDetailService : IDisposable
    {
        BomDetail Create(BomDetail pt);
        void Delete(int id);
        void Delete(BomDetail pt);
        BomDetail Find(int id);
        IEnumerable<BomDetail> GetPagedList(int pageNumber, int pageSize, out int totalRecords);
        void Update(BomDetail pt);
        BomDetail Add(BomDetail pt);
        IEnumerable<BomDetail> GetBomDetailList();
        IEnumerable<BomDetail> GetBomDetailList(int BaseProductId);

        Task<IEquatable<BomDetail>> GetAsync();
        Task<BomDetail> FindAsync(int id);
        int NextId(int id);
        int PrevId(int id);

        //For Design Consumption
        DesignConsumptionHeaderViewModel GetDesignConsumptionHeaderViewModel(int ProductId);
        ProductConsumptionHeaderViewModel GetDesignConsumptionHeaderViewModelForProduct(int ProductId);
        IEnumerable<DesignConsumptionLineViewModel> GetDesignConsumptionFaceContentForIndex(int BaseProductId);
        IEnumerable<ProductConsumptionLineViewModel> GetDesignConsumptionFaceContentForIndexForProduct(int BaseProductId);
        IEnumerable<DesignConsumptionLineViewModel> GetDesignConsumptionOtherContentForIndex(int BaseProductId);
        IEnumerable<ProductConsumptionLineViewModel> GetDesignConsumptionOtherContentForIndexForProduct(int BaseProductId);
        DesignConsumptionLineViewModel GetDesignConsumptionLineForEdit(int BomDetailId);
        ProductConsumptionLineViewModel GetDesignConsumptionLineForEditForProduct(int BomDetailId);
        IQueryable<DesignConsumptionHeaderViewModel> GetDesignConsumptionHeaderViewModelForIndex();
        IQueryable<ProductConsumptionHeaderViewModel> GetDesignConsumptionHeaderViewModelForIndexForProduct();
        bool CheckForProductShadeExists(int ProductId, int? Dimension1Id, int BaseProductId, int BomDetailId);
        bool CheckForProductShadeExists(int ProductId, int? Dimension1Id, int BaseProductId);


        //For Finished Product Consumtion
        IEnumerable<FinishedProductConsumptionLineViewModel> GetFinishedProductConsumptionForIndex(int BaseProductId);
        FinishedProductConsumptionLineViewModel GetFinishedProductConsumptionLineForEdit(int BomDetailId);
        bool CheckForProductExists(int ProductId, int BaseProductId, int BomDetailId);
        bool CheckForProductExists(int ProductId, int BaseProductId);

        //For Both Desing and Finishing Product Cobsumption
        ProductWithGroupAndUnit GetProductGroupAndUnit(int ProductId);
        DesignConsumptionLineViewModel GetBaseProductDetail(int BaseProductId);
        ProductConsumptionLineViewModel GetBaseProductDetailForProduct(int BaseProductId);

        BomDetail GetExistingBaseProduct(int id);

    }

    public class BomDetailService : IBomDetailService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<BomDetail> _BomDetailRepository;
        RepositoryQuery<BomDetail> BomDetailRepository;
        public BomDetailService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _BomDetailRepository = new Repository<BomDetail>(db);
            BomDetailRepository = new RepositoryQuery<BomDetail>(_BomDetailRepository);
        }

        public BomDetail Find(int id)
        {
            return _unitOfWork.Repository<BomDetail>().Find(id);
        }

        public BomDetail Create(BomDetail pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<BomDetail>().Insert(pt);
            return pt;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<BomDetail>().Delete(id);
        }

        public void Delete(BomDetail pt)
        {
            _unitOfWork.Repository<BomDetail>().Delete(pt);
        }

        public void Update(BomDetail pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<BomDetail>().Update(pt);
        }

        public IEnumerable<BomDetail> GetPagedList(int pageNumber, int pageSize, out int totalRecords)
        {
            var so = _unitOfWork.Repository<BomDetail>()
                .Query()
                //.OrderBy(q => q.OrderBy(c => c.BomDetailName))                
                .GetPage(pageNumber, pageSize, out totalRecords);

            return so;
        }

        public IEnumerable<BomDetail> GetBomDetailList()
        {
            var pt = _unitOfWork.Repository<BomDetail>().Query().Get();//.OrderBy(m=>m.BomDetailName);
            return pt;
        }

        public IEnumerable<BomDetail> GetBomDetailList(int BaseProductId)
        {
            var pt = _unitOfWork.Repository<BomDetail>().Query().Get().Where(m => m.BaseProductId == BaseProductId);
            return pt;
        }

        public BomDetail Add(BomDetail pt)
        {
            _unitOfWork.Repository<BomDetail>().Insert(pt);
            return pt;
        }

        public int NextId(int id)
        {

            int temp = 0;
            if (id != 0)
            {
                temp = (from p in db.Product
                        let RefId=db.Product.Where(m=>m.ProductId==id).FirstOrDefault().ReferenceDocTypeId
                        join pc in db.ProductGroups on p.ProductGroupId equals pc.ProductGroupId into ProductGroupTable
                        from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                        join pc in db.ProductTypes on ProductGroupTab.ProductTypeId equals pc.ProductTypeId into ProductTypeTable
                        from ProductTypeTab in ProductTypeTable.DefaultIfEmpty()
                        join pn in db.ProductNature on ProductTypeTab.ProductNatureId equals pn.ProductNatureId into ProductNatureTable
                        from ProductNatureTab in ProductNatureTable.DefaultIfEmpty()
                        where ProductNatureTab.ProductNatureName == ProductNatureConstants.Bom && p.ReferenceDocTypeId == RefId
                        orderby p.ProductName
                        select p.ProductId).AsEnumerable().SkipWhile(p => p != id).Skip(1).FirstOrDefault();
            }
            else
            {
                temp = (from p in db.Product
                        let RefId = db.Product.Where(m => m.ProductId == id).FirstOrDefault().ReferenceDocTypeId
                        join pc in db.ProductGroups on p.ProductGroupId equals pc.ProductGroupId into ProductGroupTable
                        from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                        join pc in db.ProductTypes on ProductGroupTab.ProductTypeId equals pc.ProductTypeId into ProductTypeTable
                        from ProductTypeTab in ProductTypeTable.DefaultIfEmpty()
                        join pn in db.ProductNature on ProductTypeTab.ProductNatureId equals pn.ProductNatureId into ProductNatureTable
                        from ProductNatureTab in ProductNatureTable.DefaultIfEmpty()
                        where ProductNatureTab.ProductNatureName == ProductNatureConstants.Bom && p.ReferenceDocTypeId == RefId
                        orderby p.ProductName
                        select p.ProductId).FirstOrDefault();
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

                temp = (from p in db.Product
                        let RefId = db.Product.Where(m => m.ProductId == id).FirstOrDefault().ReferenceDocTypeId
                        join pc in db.ProductGroups on p.ProductGroupId equals pc.ProductGroupId into ProductGroupTable
                        from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                        join pc in db.ProductTypes on ProductGroupTab.ProductTypeId equals pc.ProductTypeId into ProductTypeTable
                        from ProductTypeTab in ProductTypeTable.DefaultIfEmpty()
                        join pn in db.ProductNature on ProductTypeTab.ProductNatureId equals pn.ProductNatureId into ProductNatureTable
                        from ProductNatureTab in ProductNatureTable.DefaultIfEmpty()
                        where ProductNatureTab.ProductNatureName == ProductNatureConstants.Bom && p.ReferenceDocTypeId == RefId
                        orderby p.ProductName
                        select p.ProductId).AsEnumerable().TakeWhile(p => p != id).LastOrDefault();
            }
            else
            {
                temp = (from p in db.Product
                        let RefId = db.Product.Where(m => m.ProductId == id).FirstOrDefault().ReferenceDocTypeId
                        join pc in db.ProductGroups on p.ProductGroupId equals pc.ProductGroupId into ProductGroupTable
                        from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                        join pc in db.ProductTypes on ProductGroupTab.ProductTypeId equals pc.ProductTypeId into ProductTypeTable
                        from ProductTypeTab in ProductTypeTable.DefaultIfEmpty()
                        join pn in db.ProductNature on ProductTypeTab.ProductNatureId equals pn.ProductNatureId into ProductNatureTable
                        from ProductNatureTab in ProductNatureTable.DefaultIfEmpty()
                        where ProductNatureTab.ProductNatureName == ProductNatureConstants.Bom && p.ReferenceDocTypeId == RefId
                        orderby p.ProductName
                        select p.ProductId).AsEnumerable().LastOrDefault();
            }
            if (temp != 0)
                return temp;
            else
                return id;
        }


        public void Dispose()
        {
        }


        public Task<IEquatable<BomDetail>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BomDetail> FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DesignConsumptionLineViewModel> GetDesignConsumptionFaceContentForIndex(int BaseProductId)
        {
            var ProductFaceContentGroups = from p in db.Product
                                           join pg in db.ProductGroups on p.ProductName equals pg.ProductGroupName into ProductGroupTable
                                           from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                           join fp in db.FinishedProduct on ProductGroupTab.ProductGroupId equals fp.ProductGroupId into FinishedProductTable
                                           from FinishedProductTab in FinishedProductTable.DefaultIfEmpty()
                                           join pcl in db.ProductContentLine on FinishedProductTab.FaceContentId equals pcl.ProductContentHeaderId into ProductContentLineTable
                                           from ProductContentLineTab in ProductContentLineTable.DefaultIfEmpty()
                                           where p.ProductId == BaseProductId && ((int?)ProductContentLineTab.ProductGroupId ?? 0) != 0
                                           group new { ProductContentLineTab } by new { ProductContentLineTab.ProductGroupId } into Result
                                           select new
                                           {
                                               ProductGroupId = Result.Key.ProductGroupId
                                           };


            Decimal Weight = 1;
            var Temp = GetBaseProductDetail(BaseProductId);
            if (Temp != null)
            {
                Weight = Temp.Weight;            
            }
            


            IEnumerable<DesignConsumptionLineViewModel> svm = (from b in db.BomDetail
                                                               join d in db.Dimension1 on b.Dimension1Id equals d.Dimension1Id into Dimension1Table
                                                               from Dimension1Tab in Dimension1Table.DefaultIfEmpty()
                                                               join p in db.Product on b.ProductId equals p.ProductId into ProductTable
                                                               from ProductTab in ProductTable.DefaultIfEmpty()
                                                               join pg in db.ProductGroups on ProductTab.ProductGroupId equals pg.ProductGroupId into ProductGroupTable
                                                               from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                                               join U in db.Units on ProductTab.UnitId equals U.UnitId into UnitTable
                                                               from UnitTab in UnitTable.DefaultIfEmpty()
                                                               join pcon in ProductFaceContentGroups on ProductTab.ProductGroupId equals pcon.ProductGroupId into ProductFaceContentTable
                                                               from ProductFaceContentTab in ProductFaceContentTable.DefaultIfEmpty()
                                                               where b.BaseProductId == BaseProductId && ((int?)ProductFaceContentTab.ProductGroupId ?? 0) != 0
                                                               select new DesignConsumptionLineViewModel
                                                               {
                                                                   BomDetailId = b.BomDetailId,
                                                                   BaseProductId = b.BaseProductId,
                                                                   ProductName = ProductTab.ProductName,
                                                                   Dimension1Name = Dimension1Tab.Dimension1Name,
                                                                   ProductGroupName = ProductGroupTab.ProductGroupName,
                                                                   ConsumptionPer = Math.Round(b.Qty * 100/Weight,2),
                                                                   Qty = b.Qty,
                                                                   UnitName = UnitTab.UnitName
                                                               }).ToList();





            return svm;
        }


        public IEnumerable<ProductConsumptionLineViewModel> GetDesignConsumptionFaceContentForIndexForProduct(int BaseProductId)
        {
            var ProductFaceContentGroups = from p in db.Product
                                           join fp in db.FinishedProduct on p.ReferenceDocId equals fp.ProductId into FinishedProductTable
                                           from FinishedProductTab in FinishedProductTable.DefaultIfEmpty()
                                           join pcl in db.ProductContentLine on FinishedProductTab.FaceContentId equals pcl.ProductContentHeaderId into ProductContentLineTable
                                           from ProductContentLineTab in ProductContentLineTable.DefaultIfEmpty()
                                           where p.ProductId == BaseProductId && ((int?)ProductContentLineTab.ProductGroupId ?? 0) != 0
                                           group new { ProductContentLineTab } by new { ProductContentLineTab.ProductGroupId } into Result
                                           select new
                                           {
                                               ProductGroupId = Result.Key.ProductGroupId
                                           };


            Decimal Weight = 1;
            var Temp = GetBaseProductDetailForProduct(BaseProductId);
            if (Temp != null)
            {
                Weight = Temp.Weight;
            }

            IEnumerable<ProductConsumptionLineViewModel> svm = (from b in db.BomDetail
                                                                join d in db.Dimension1 on b.Dimension1Id equals d.Dimension1Id into Dimension1Table
                                                                from Dimension1Tab in Dimension1Table.DefaultIfEmpty()
                                                                join p in db.Product on b.ProductId equals p.ProductId into ProductTable
                                                                from ProductTab in ProductTable.DefaultIfEmpty()
                                                                join pg in db.ProductGroups on ProductTab.ProductGroupId equals pg.ProductGroupId into ProductGroupTable
                                                                from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                                                join U in db.Units on ProductTab.UnitId equals U.UnitId into UnitTable
                                                                from UnitTab in UnitTable.DefaultIfEmpty()
                                                                join pcon in ProductFaceContentGroups on ProductTab.ProductGroupId equals pcon.ProductGroupId into ProductFaceContentTable
                                                                from ProductFaceContentTab in ProductFaceContentTable.DefaultIfEmpty()
                                                                where b.BaseProductId == BaseProductId && ((int?)ProductFaceContentTab.ProductGroupId ?? 0) != 0
                                                                select new ProductConsumptionLineViewModel
                                                               {
                                                                   BomDetailId = b.BomDetailId,
                                                                   BaseProductId = b.BaseProductId,
                                                                   ProductName = ProductTab.ProductName,
                                                                   Dimension1Name = Dimension1Tab.Dimension1Name,
                                                                   ProductGroupName = ProductGroupTab.ProductGroupName,
                                                                   //ConsumptionPer = b.ConsumptionPer,
                                                                   ConsumptionPer = Math.Round(b.Qty * 100 / Weight, 2),
                                                                   Qty = b.Qty,
                                                                   UnitName = UnitTab.UnitName
                                                               }).ToList();





            return svm;
        }



        public IEnumerable<DesignConsumptionLineViewModel> GetDesignConsumptionOtherContentForIndex(int BaseProductId)
        {
            var ProductFaceContentGroups = from p in db.Product
                                           join pg in db.ProductGroups on p.ProductName equals pg.ProductGroupName into ProductGroupTable
                                           from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                           join fp in db.FinishedProduct on ProductGroupTab.ProductGroupId equals fp.ProductGroupId into FinishedProductTable
                                           from FinishedProductTab in FinishedProductTable.DefaultIfEmpty()
                                           join pcl in db.ProductContentLine on FinishedProductTab.FaceContentId equals pcl.ProductContentHeaderId into ProductContentLineTable
                                           from ProductContentLineTab in ProductContentLineTable.DefaultIfEmpty()
                                           where p.ProductId == BaseProductId && ((int?)ProductContentLineTab.ProductGroupId ?? 0) != 0
                                           group new { ProductContentLineTab } by new { ProductContentLineTab.ProductGroupId } into Result
                                           select new
                                           {
                                               ProductGroupId = Result.Key.ProductGroupId
                                           };


            Decimal Weight = 1;
            var Temp = GetBaseProductDetail(BaseProductId);
            if (Temp != null)
            {
                Weight = Temp.Weight;
            }

            IEnumerable<DesignConsumptionLineViewModel> svm = (from b in db.BomDetail
                                                               join d in db.Dimension1 on b.Dimension1Id equals d.Dimension1Id into Dimension1Table
                                                               from Dimension1Tab in Dimension1Table.DefaultIfEmpty()
                                                               join p in db.Product on b.ProductId equals p.ProductId into ProductTable
                                                               from ProductTab in ProductTable.DefaultIfEmpty()
                                                               join pg in db.ProductGroups on ProductTab.ProductGroupId equals pg.ProductGroupId into ProductGroupTable
                                                               from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                                               join U in db.Units on ProductTab.UnitId equals U.UnitId into UnitTable
                                                               from UnitTab in UnitTable.DefaultIfEmpty()
                                                               join pcon in ProductFaceContentGroups on ProductTab.ProductGroupId equals pcon.ProductGroupId into ProductFaceContentTable
                                                               from ProductFaceContentTab in ProductFaceContentTable.DefaultIfEmpty()
                                                               where b.BaseProductId == BaseProductId && ((int?)ProductFaceContentTab.ProductGroupId ?? 0) == 0
                                                               select new DesignConsumptionLineViewModel
                                                               {
                                                                   BomDetailId = b.BomDetailId,
                                                                   BaseProductId = b.BaseProductId,
                                                                   ProductName = ProductTab.ProductName,
                                                                   Dimension1Name = Dimension1Tab.Dimension1Name,
                                                                   ProductGroupName = ProductGroupTab.ProductGroupName,
                                                                   //ConsumptionPer = b.ConsumptionPer,
                                                                   ConsumptionPer = Math.Round(b.Qty * 100 / Weight, 2),
                                                                   Qty = b.Qty,
                                                                   UnitName = UnitTab.UnitName
                                                               });





            return svm.ToList();
        }

        public IEnumerable<ProductConsumptionLineViewModel> GetDesignConsumptionOtherContentForIndexForProduct(int BaseProductId)
        {
            var ProductFaceContentGroups = from p in db.Product
                                           join fp in db.FinishedProduct on p.ReferenceDocId equals fp.ProductId into FinishedProductTable
                                           from FinishedProductTab in FinishedProductTable.DefaultIfEmpty()
                                           join pcl in db.ProductContentLine on FinishedProductTab.FaceContentId equals pcl.ProductContentHeaderId into ProductContentLineTable
                                           from ProductContentLineTab in ProductContentLineTable.DefaultIfEmpty()
                                           where p.ProductId == BaseProductId && ((int?)ProductContentLineTab.ProductGroupId ?? 0) != 0
                                           group new { ProductContentLineTab } by new { ProductContentLineTab.ProductGroupId } into Result
                                           select new
                                           {
                                               ProductGroupId = Result.Key.ProductGroupId
                                           };


            Decimal Weight = 1;
            var Temp = GetBaseProductDetailForProduct(BaseProductId);
            if (Temp != null)
            {
                Weight = Temp.Weight;
            }


            IEnumerable<ProductConsumptionLineViewModel> svm = (from b in db.BomDetail
                                                                join d in db.Dimension1 on b.Dimension1Id equals d.Dimension1Id into Dimension1Table
                                                                from Dimension1Tab in Dimension1Table.DefaultIfEmpty()
                                                                join p in db.Product on b.ProductId equals p.ProductId into ProductTable
                                                                from ProductTab in ProductTable.DefaultIfEmpty()
                                                                join pg in db.ProductGroups on ProductTab.ProductGroupId equals pg.ProductGroupId into ProductGroupTable
                                                                from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                                                join U in db.Units on ProductTab.UnitId equals U.UnitId into UnitTable
                                                                from UnitTab in UnitTable.DefaultIfEmpty()
                                                                join pcon in ProductFaceContentGroups on ProductTab.ProductGroupId equals pcon.ProductGroupId into ProductFaceContentTable
                                                                from ProductFaceContentTab in ProductFaceContentTable.DefaultIfEmpty()
                                                                where b.BaseProductId == BaseProductId && ((int?)ProductFaceContentTab.ProductGroupId ?? 0) == 0
                                                                select new ProductConsumptionLineViewModel
                                                               {
                                                                   BomDetailId = b.BomDetailId,
                                                                   BaseProductId = b.BaseProductId,
                                                                   ProductName = ProductTab.ProductName,
                                                                   Dimension1Name = Dimension1Tab.Dimension1Name,
                                                                   ProductGroupName = ProductGroupTab.ProductGroupName,
                                                                   //ConsumptionPer = b.ConsumptionPer,
                                                                   ConsumptionPer = Math.Round(b.Qty * 100 / Weight, 2),
                                                                   Qty = b.Qty,
                                                                   UnitName = UnitTab.UnitName
                                                               });





            return svm.ToList();
        }

        public IEnumerable<FinishedProductConsumptionLineViewModel> GetFinishedProductConsumptionForIndex(int BaseProductId)
        {
            IEnumerable<FinishedProductConsumptionLineViewModel> svm = (from b in db.BomDetail
                                                                        join p in db.Product on b.ProductId equals p.ProductId into ProductTable
                                                                        from ProductTab in ProductTable.DefaultIfEmpty()
                                                                        join pg in db.ProductGroups on ProductTab.ProductGroupId equals pg.ProductGroupId into ProductGroupTable
                                                                        from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                                                        join U in db.Units on ProductTab.UnitId equals U.UnitId into UnitTable
                                                                        from UnitTab in UnitTable.DefaultIfEmpty()
                                                                        where b.BaseProductId == BaseProductId
                                                                        select new FinishedProductConsumptionLineViewModel
                                                                        {
                                                                            BomDetailId = b.BomDetailId,
                                                                            BaseProductId = b.BaseProductId,
                                                                            ProductName = ProductTab.ProductName,
                                                                            ProductGroupName = ProductGroupTab.ProductGroupName,
                                                                            Qty = b.Qty,
                                                                            UnitName = UnitTab.UnitName
                                                                        }).ToList();



            return svm;
        }



        public DesignConsumptionLineViewModel GetDesignConsumptionLineForEdit(int BomDetailId)
        {
            BomDetail BomDetail = Find(BomDetailId);
            Decimal Weight = 1;
            var Temp = GetBaseProductDetail(BomDetail.BaseProductId);
            if (Temp != null)
            {
                Weight = Temp.Weight;
            }


            DesignConsumptionLineViewModel svm = (from b in db.BomDetail
                                                  join p in db.Product on b.ProductId equals p.ProductId into ProductTable
                                                  from ProductTab in ProductTable.DefaultIfEmpty()
                                                  join pg in db.ProductGroups on ProductTab.ProductGroupId equals pg.ProductGroupId into ProductGroupTable
                                                  from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                                  join U in db.Units on ProductTab.UnitId equals U.UnitId into UnitTable
                                                  from UnitTab in UnitTable.DefaultIfEmpty()
                                                  where b.BomDetailId == BomDetailId
                                                  select new DesignConsumptionLineViewModel
                                                  {
                                                      BomDetailId = b.BomDetailId,
                                                      BaseProductId = b.BaseProductId,
                                                      ProductId = b.ProductId,
                                                      Dimension1Id = b.Dimension1Id,
                                                      ProductGroupName = ProductGroupTab.ProductGroupName,
                                                      //ConsumptionPer = b.ConsumptionPer,
                                                      ConsumptionPer = Math.Round(b.Qty * 100/Weight,2),
                                                      Qty = b.Qty,
                                                      UnitName = UnitTab.UnitName
                                                  }).FirstOrDefault();
            return svm;
        }


        public ProductConsumptionLineViewModel GetDesignConsumptionLineForEditForProduct(int BomDetailId)
        {
            BomDetail BomDetail = Find(BomDetailId);
            Decimal Weight = 1;
            var Temp = GetBaseProductDetailForProduct(BomDetail.BaseProductId);
            if (Temp != null)
            {
                Weight = Temp.Weight;
            }

            ProductConsumptionLineViewModel svm = (from b in db.BomDetail
                                                   join p in db.Product on b.ProductId equals p.ProductId into ProductTable
                                                   from ProductTab in ProductTable.DefaultIfEmpty()
                                                   join pg in db.ProductGroups on ProductTab.ProductGroupId equals pg.ProductGroupId into ProductGroupTable
                                                   from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                                   join U in db.Units on ProductTab.UnitId equals U.UnitId into UnitTable
                                                   from UnitTab in UnitTable.DefaultIfEmpty()
                                                   where b.BomDetailId == BomDetailId
                                                   select new ProductConsumptionLineViewModel
                                                   {
                                                       BomDetailId = b.BomDetailId,
                                                       BaseProductId = b.BaseProductId,
                                                       ProductId = b.ProductId,
                                                       Dimension1Id = b.Dimension1Id,
                                                       ProductGroupName = ProductGroupTab.ProductGroupName,
                                                       //ConsumptionPer = b.ConsumptionPer,
                                                       ConsumptionPer = Math.Round(b.Qty * 100 / Weight, 2),
                                                       Qty = b.Qty,
                                                       UnitName = UnitTab.UnitName
                                                   }).FirstOrDefault();
            return svm;
        }


        public FinishedProductConsumptionLineViewModel GetFinishedProductConsumptionLineForEdit(int BomDetailId)
        {
            FinishedProductConsumptionLineViewModel svm = (from b in db.BomDetail
                                                           join p in db.Product on b.ProductId equals p.ProductId into ProductTable
                                                           from ProductTab in ProductTable.DefaultIfEmpty()
                                                           join pg in db.ProductGroups on ProductTab.ProductGroupId equals pg.ProductGroupId into ProductGroupTable
                                                           from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                                           join U in db.Units on ProductTab.UnitId equals U.UnitId into UnitTable
                                                           from UnitTab in UnitTable.DefaultIfEmpty()
                                                           where b.BomDetailId == BomDetailId
                                                           select new FinishedProductConsumptionLineViewModel
                                                           {
                                                               BomDetailId = b.BomDetailId,
                                                               BaseProductId = b.BaseProductId,
                                                               ProductId = b.ProductId,
                                                               ProductGroupName = ProductGroupTab.ProductGroupName,
                                                               Qty = b.Qty,
                                                               UnitName = UnitTab.UnitName
                                                           }).FirstOrDefault();
            return svm;
        }


        public DesignConsumptionHeaderViewModel GetDesignConsumptionHeaderViewModel(int ProductId)
        {
            DesignConsumptionHeaderViewModel svm = (from p in db.Product
                                                    join Ig in db.ProductGroups on p.ProductName equals Ig.ProductGroupName into ProductGroupTable
                                                    from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                                    where p.ProductId == ProductId
                                                    select new DesignConsumptionHeaderViewModel
                                                    {
                                                        BaseProductId = p.ProductId,
                                                        ProductGroupId = ProductGroupTab.ProductGroupId,
                                                        ProductGroupName = ProductGroupTab.ProductGroupName
                                                    }).FirstOrDefault();

            if (svm != null)
            {
                var p = new ProductGroupService(_unitOfWork).GetProductGroupQuality(svm.ProductGroupId);

                if (p != null)
                {
                    svm.ProductQualityName = p.ProductQualityName;
                }
            }

            return svm;
        }

        public ProductConsumptionHeaderViewModel GetDesignConsumptionHeaderViewModelForProduct(int ProductId)
        {
            ProductConsumptionHeaderViewModel svm = (from p in db.Product
                                                     where p.ProductId == ProductId
                                                     join t in db.Product on p.ReferenceDocId equals t.ProductId into table
                                                     from tab in table.DefaultIfEmpty()
                                                     select new ProductConsumptionHeaderViewModel
                                                    {
                                                        BaseProductId = p.ProductId,
                                                        ProductId = tab.ProductId,
                                                        ProductName = tab.ProductName,

                                                    }).FirstOrDefault();

            if (svm != null)
            {
                var p = new ProductQualityService(_unitOfWork).FindProductQuality(svm.ProductId);

                if (p != null)
                {
                    svm.ProductQualityName = p.ProductQualityName;
                }
            }

            return svm;
        }

        public IQueryable<DesignConsumptionHeaderViewModel> GetDesignConsumptionHeaderViewModelForIndex()
        {

            var RefDocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.ProductGroup).DocumentTypeId;

            IQueryable<DesignConsumptionHeaderViewModel> svm = from p in db.Product
                                                               join pc in db.ProductGroups on p.ProductGroupId equals pc.ProductGroupId into ProductGroupTable
                                                               from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                                               join pc in db.ProductTypes on ProductGroupTab.ProductTypeId equals pc.ProductTypeId into ProductTypeTable
                                                               from ProductTypeTab in ProductTypeTable.DefaultIfEmpty()
                                                               join pn in db.ProductNature on ProductTypeTab.ProductNatureId equals pn.ProductNatureId into ProductNatureTable
                                                               from ProductNatureTab in ProductNatureTable.DefaultIfEmpty()
                                                               where ProductNatureTab.ProductNatureName == ProductNatureConstants.Bom && p.ReferenceDocTypeId == RefDocTypeId
                                                               orderby p.ProductName
                                                               select new DesignConsumptionHeaderViewModel
                                                               {
                                                                   BaseProductId = p.ProductId,
                                                                   BaseProductName = p.ProductName,
                                                               };

            return svm;
        }


        public IQueryable<ProductConsumptionHeaderViewModel> GetDesignConsumptionHeaderViewModelForIndexForProduct()
        {

            var RefDocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.Product).DocumentTypeId;

            IQueryable<ProductConsumptionHeaderViewModel> svm = from p in db.Product
                                                                join pc in db.ProductGroups on p.ProductGroupId equals pc.ProductGroupId into ProductGroupTable
                                                                from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                                                join t2 in db.Product on p.ReferenceDocId equals t2.ProductId
                                                                join pc in db.ProductTypes on ProductGroupTab.ProductTypeId equals pc.ProductTypeId into ProductTypeTable
                                                                from ProductTypeTab in ProductTypeTable.DefaultIfEmpty()
                                                                join pn in db.ProductNature on ProductTypeTab.ProductNatureId equals pn.ProductNatureId into ProductNatureTable
                                                                from ProductNatureTab in ProductNatureTable.DefaultIfEmpty()
                                                                where ProductNatureTab.ProductNatureName == ProductNatureConstants.Bom && p.ReferenceDocTypeId == RefDocTypeId
                                                                orderby p.ProductName
                                                                select new ProductConsumptionHeaderViewModel
                                                               {
                                                                   BaseProductId = p.ProductId,
                                                                   BaseProductName = t2.ProductName,
                                                               };

            return svm;
        }





        public ProductWithGroupAndUnit GetProductGroupAndUnit(int ProductId)
        {
            ProductWithGroupAndUnit svm = (from p in db.Product
                                           join pg in db.ProductGroups on p.ProductGroupId equals pg.ProductGroupId into ProductGroupTable
                                           from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                           join u in db.Units on p.UnitId equals u.UnitId into UnitTable
                                           from UnitTab in UnitTable.DefaultIfEmpty()
                                           where p.ProductId == ProductId
                                           select new ProductWithGroupAndUnit
                                            {
                                                ProductGroupId = p.ProductGroupId,
                                                ProductGroupName = ProductGroupTab.ProductGroupName,
                                                UnitName = UnitTab.UnitName
                                            }).FirstOrDefault();

            return svm;
        }


        public bool CheckForProductShadeExists(int ProductId, int? Dimension1Id, int BaseProductId, int BomDetailId)
        {

            BomDetail temp = (from p in db.BomDetail
                              where p.ProductId == ProductId && p.Dimension1Id == Dimension1Id && p.BaseProductId == BaseProductId && p.BomDetailId != BomDetailId
                              select p).FirstOrDefault();
            if (temp != null)
                return true;
            else return false;
        }

        public bool CheckForProductShadeExists(int ProductId, int? Dimension1Id, int BaseProductId)
        {

            BomDetail temp = (from p in db.BomDetail
                              where p.ProductId == ProductId && p.Dimension1Id == Dimension1Id && p.BaseProductId == BaseProductId
                              select p).FirstOrDefault();
            if (temp != null)
                return true;
            else return false;
        }







        public bool CheckForProductExists(int ProductId, int BaseProductId, int BomDetailId)
        {

            BomDetail temp = (from p in db.BomDetail
                              where p.ProductId == ProductId && p.BaseProductId == BaseProductId && p.BomDetailId != BomDetailId
                              select p).FirstOrDefault();
            if (temp != null)
                return true;
            else return false;
        }

        public bool CheckForProductExists(int ProductId, int BaseProductId)
        {

            BomDetail temp = (from p in db.BomDetail
                              where p.ProductId == ProductId && p.BaseProductId == BaseProductId
                              select p).FirstOrDefault();
            if (temp != null)
                return true;
            else return false;
        }

        public DesignConsumptionLineViewModel GetBaseProductDetail(int BaseProductId)
        {
            var temp = from p in db.Product
                       join pg in db.ProductGroups on p.ProductName equals pg.ProductGroupName into ProductGroupTable
                       from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                       join f in db.FinishedProduct on ProductGroupTab.ProductGroupId equals f.ProductGroupId into FinishedProductTable
                       from FinishedProductTab in FinishedProductTable.DefaultIfEmpty()
                       join Q in db.ProductQuality on FinishedProductTab.ProductQualityId equals Q.ProductQualityId into QualityTable
                       from QualityTab in QualityTable.DefaultIfEmpty()
                       where p.ProductId == BaseProductId
                       select new DesignConsumptionLineViewModel
                        {
                            BaseProductId = p.ProductId,
                            DesignName = ProductGroupTab.ProductGroupName,
                            QualityName = QualityTab.ProductQualityName,
                            Weight = QualityTab.Weight
                        };


            return temp.FirstOrDefault();
        }

        public ProductConsumptionLineViewModel GetBaseProductDetailForProduct(int BaseProductId)
        {
            var temp = from p in db.Product
                       join t in db.FinishedProduct on p.ReferenceDocId equals t.ProductId into table
                       from tab in table.DefaultIfEmpty()
                       join Q in db.ProductQuality on tab.ProductQualityId equals Q.ProductQualityId into QualityTable
                       from QualityTab in QualityTable.DefaultIfEmpty()
                       where p.ProductId == BaseProductId
                       select new ProductConsumptionLineViewModel
                       {
                           BaseProductId = p.ProductId,
                           ProductName = tab.ProductName,
                           QualityName = QualityTab.ProductQualityName,
                           Weight = QualityTab.Weight
                       };


            return temp.FirstOrDefault();
        }

        public BomDetail GetExistingBaseProduct(int id)
        {

            return (from p in db.BomDetail
                    where p.BaseProductId == id
                    select p).FirstOrDefault();

        }
    }

    public class ProductWithGroupAndUnit
    {
        public int? ProductGroupId { get; set; }
        public string ProductGroupName { get; set; }
        public string UnitName { get; set; }
    }
}
