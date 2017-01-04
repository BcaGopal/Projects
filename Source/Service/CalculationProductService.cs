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

namespace Service
{
    public interface ICalculationProductService : IDisposable
    {
        CalculationProduct Create(CalculationProduct pt);
        void Delete(int id);
        void Delete(CalculationProduct pt);
        CalculationProduct Find(int id);
        IEnumerable<CalculationProduct> GetPagedList(int pageNumber, int pageSize, out int totalRecords);
        void Update(CalculationProduct pt);
        CalculationProduct Add(CalculationProduct pt);
        IEnumerable<CalculationProduct> GetCalculationProductList();
        IEnumerable<CalculationProductViewModel> GetCalculationProductList(int CalculationID, int DocumentTypeId, int SiteId, int DivisionId);
        Task<IEquatable<CalculationProduct>> GetAsync();
        Task<CalculationProduct> FindAsync(int id);
        IEnumerable<CalculationProductViewModel> GetCalculationListForIndex(int id);//CalculationId
        CalculationProductViewModel GetCalculationProduct(int id);//CalculationProductId
        IEnumerable<CalculationProductViewModel> GetCalculationProductListForDropDown();
        int NextId(int id);
        int PrevId(int id);
    }

    public class CalculationProductService : ICalculationProductService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<CalculationProduct> _CalculationProductRepository;
        RepositoryQuery<CalculationProduct> CalculationProductRepository;

        public CalculationProductService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _CalculationProductRepository = new Repository<CalculationProduct>(db);
            CalculationProductRepository = new RepositoryQuery<CalculationProduct>(_CalculationProductRepository);
        }


        public CalculationProduct Find(int id)
        {
            return _unitOfWork.Repository<CalculationProduct>().Find(id);
        }
        public CalculationProductViewModel GetCalculationProduct(int id)
        {
            return (from p in db.CalculationProduct
                    where p.CalculationProductId == id
                    select new CalculationProductViewModel
                    {
                       AddDeduct=p.AddDeduct,
                       AffectCost=p.AffectCost,
                       CalculateOnId=p.CalculateOnId,
                       CalculationId=p.CalculationId,
                       Id=p.CalculationProductId,
                       ChargeId=p.ChargeId,
                       ChargeTypeId=p.ChargeTypeId,
                       CostCenterId=p.CostCenterId,
                       IncludedInBase=p.IncludedInBase,
                       IsActive=p.IsActive,
                       Amount=p.Amount,
                       RateType=p.RateType,
                       IsVisible=p.IsVisible,
                       ParentChargeId=p.ParentChargeId,
                       Rate=p.Rate,
                       Sr=p.Sr,
                       IncludedCharges=p.IncludedCharges,
                       IncludedChargesCalculation=p.IncludedChargesCalculation,
                    }

                        ).FirstOrDefault();


        }
        public IEnumerable<CalculationProductViewModel> GetCalculationProductListForDropDown()
        {
            return (from p in db.CalculationProduct
                    join t in db.Charge on p.ChargeId equals t.ChargeId
                    select new CalculationProductViewModel
                    {
                        Id = p.CalculationProductId,
                        CalculationProductName = t.ChargeName,
                    }
                        );
        }
        public IEnumerable<CalculationProductViewModel> GetCalculationListForIndex(int id)
        {
            var Query = (from p in db.CalculationProduct
                        where p.CalculationId == id
                        orderby p.CalculationProductId
                        select new CalculationProductViewModel
                        {
                            AddDeduct = p.AddDeduct,
                            //AddDeductName=(p.AddDeduct==null?"":(p.AddDeduct==true?"Add":"Deduction")),
                            //AddDeductName = p.AddDeduct,
                            AffectCost = p.AffectCost,
                            AffectCostName = (p.AffectCost == true ? "Yes" : "No"),
                            CalculateOnId = p.CalculateOnId,
                            CalculateOnName = p.CalculateOn.ChargeName,
                            Id = p.CalculationProductId,
                            ChargeName = p.Charge.ChargeName,
                            ChargeTypeName = p.ChargeType.ChargeTypeName,
                            CostCenterName = p.CostCenter.CostCenterName,
                            IncludedInBase = p.IncludedInBase,
                            IncludedInBaseName = (p.IncludedInBase == true ? "Yes" : "No"),
                            IsActive = p.IsActive,
                            Rate = p.Rate,
                            ParentChargeId = p.ParentChargeId,

                        }).ToList();

            var Result = Query.Select(p => new CalculationProductViewModel
            {
                AddDeduct = p.AddDeduct,               
                AddDeductName = (p.AddDeduct.HasValue ? Enum.GetName(typeof(AddDeductEnum), p.AddDeduct) : ""),
                AffectCost = p.AffectCost,
                AffectCostName = (p.AffectCost == true ? "Yes" : "No"),
                CalculateOnId = p.CalculateOnId,
                CalculateOnName = p.CalculateOnName,
                Id = p.Id,
                ChargeName = p.ChargeName,
                ChargeTypeName = p.ChargeTypeName,
                CostCenterName = p.CostCenterName,
                IncludedInBase = p.IncludedInBase,
                IncludedInBaseName = (p.IncludedInBaseName),
                IsActive = p.IsActive,
                Rate = p.Rate,
                ParentChargeId = p.ParentChargeId,
            }).AsEnumerable();

            return (Result);
        }

        public CalculationProduct Create(CalculationProduct pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<CalculationProduct>().Insert(pt);
            return pt;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<CalculationProduct>().Delete(id);
        }

        public void Delete(CalculationProduct pt)
        {
            _unitOfWork.Repository<CalculationProduct>().Delete(pt);
        }

        public void Update(CalculationProduct pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<CalculationProduct>().Update(pt);
        }

        public IEnumerable<CalculationProduct> GetPagedList(int pageNumber, int pageSize, out int totalRecords)
        {
            var so = _unitOfWork.Repository<CalculationProduct>()
                .Query()
                .OrderBy(q => q.OrderBy(c => c.CalculationProductId))                
                .GetPage(pageNumber, pageSize, out totalRecords);

            return so;
        }

        public IEnumerable<CalculationProduct> GetCalculationProductList()
        {
            var pt = _unitOfWork.Repository<CalculationProduct>().Query().Get().OrderBy(m=>m.CalculationProductId);
            return pt;
        }
        public IEnumerable<CalculationProductViewModel> GetCalculationProductList(int CalculationID, int DocumentTypeId, int SiteId, int DivisionId)
        {          

            return (from p in db.CalculationProduct                    
                    join t in db.CalculationLineLedgerAccount.Where(m=>m.DocTypeId==DocumentTypeId && m.SiteId == SiteId && m.DivisionId== DivisionId) on p.CalculationProductId equals t.CalculationProductId into table1 from tab1 in table1.DefaultIfEmpty()
                    where p.CalculationId == CalculationID
                    orderby p.Sr
                    select new CalculationProductViewModel
                    {
                        AddDeduct=p.AddDeduct,
                        AffectCost=p.AffectCost,
                        CalculateOnId=p.CalculateOnId,
                        CalculateOnName=p.CalculateOn.ChargeName,
                        CalculateOnCode=p.CalculateOn.ChargeCode,
                        CalculationId=p.CalculationId,
                        CalculationName=p.Calculation.CalculationName,
                        ChargeId=p.ChargeId,
                        ChargeName=p.Charge.ChargeName,
                        ChargeCode=p.Charge.ChargeCode,
                        ChargeTypeId=p.ChargeTypeId,
                        ChargeTypeName=p.ChargeType.ChargeTypeName,
                        CostCenterId=p.CostCenterId,
                        CostCenterName=p.CostCenter.CostCenterName,
                        IncludedInBase=p.IncludedInBase,
                        LedgerAccountCrId=tab1.LedgerAccountCrId,
                        LedgerAccountCrName = tab1.LedgerAccountCr.LedgerAccountName,
                        LedgerAccountDrId = tab1.LedgerAccountDrId,
                        LedgerAccountDrName = tab1.LedgerAccountDr.LedgerAccountName,
                        ContraLedgerAccountId=tab1.ContraLedgerAccountId,
                        ContraLedgerAccountName=tab1.ContraLedgerAccount.LedgerAccountName,
                        Rate=p.Rate,
                        Sr=p.Sr,
                        RateType=p.RateType,
                        IsVisible=p.IsVisible,
                        Amount=p.Amount,
                        ParentChargeId=p.ParentChargeId,
                        ElementId="CALL_"+p.Charge.ChargeCode,
                        IncludedCharges=p.IncludedCharges,
                        IncludedChargesCalculation=p.IncludedChargesCalculation,

                    }
                        );
        }

        public CalculationProduct Add(CalculationProduct pt)
        {
            _unitOfWork.Repository<CalculationProduct>().Insert(pt);
            return pt;
        }

        public int NextId(int id)
        {
            int temp = 0;
            if (id != 0)
            {
                temp = (from p in db.CalculationProduct
                        orderby p.CalculationProductId
                        select p.CalculationProductId).AsEnumerable().SkipWhile(p => p != id).Skip(1).FirstOrDefault();
            }
            else
            {
                temp = (from p in db.CalculationProduct
                        orderby p.CalculationProductId
                        select p.CalculationProductId).FirstOrDefault();
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
                temp = (from p in db.CalculationProduct
                        orderby p.CalculationProductId
                        select p.CalculationProductId).AsEnumerable().TakeWhile(p => p != id).LastOrDefault();
            }
            else
            {
                temp = (from p in db.CalculationProduct
                        orderby p.CalculationProductId
                        select p.CalculationProductId).AsEnumerable().LastOrDefault();
            }
            if (temp != 0)
                return temp;
            else
                return id;
        }

        public void Dispose()
        {
        }


        public Task<IEquatable<CalculationProduct>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CalculationProduct> FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        //public void SaveProductCharge(CalculationLineCharge x) 
        //{            
        //    x.Id = 1;
        //    UnitOfWo




        //}

    }
}
