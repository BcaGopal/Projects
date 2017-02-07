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
using System.Data.Entity.SqlServer;

namespace Service
{
    public interface IMaterialPlanCancelHeaderService : IDisposable
    {
        MaterialPlanCancelHeader Create(MaterialPlanCancelHeader pt);
        void Delete(int id);
        void Delete(MaterialPlanCancelHeader pt);
        MaterialPlanCancelHeader Find(int id);
        IEnumerable<MaterialPlanCancelHeader> GetPagedList(int pageNumber, int pageSize, out int totalRecords);
        void Update(MaterialPlanCancelHeader pt);
        MaterialPlanCancelHeader Add(MaterialPlanCancelHeader pt);
        IQueryable<MaterialPlanCancelHeaderViewModel> GetMaterialPlanCancelHeaderList(int DocTypeId, string Uname);
        IQueryable<MaterialPlanCancelHeaderViewModel> GetMaterialPlanCancelHeaderListPendingToSubmit(int DocTypeId, string Uname);
        IQueryable<MaterialPlanCancelHeaderViewModel> GetMaterialPlanCancelHeaderListPendingToReview(int DocTypeId, string Uname);
        Task<IEquatable<MaterialPlanCancelHeader>> GetAsync();
        Task<MaterialPlanCancelHeader> FindAsync(int id);
        int NextId(int id);
        int PrevId(int id);
        string GetMaxDocNo();
    }

    public class MaterialPlanCancelHeaderService : IMaterialPlanCancelHeaderService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<MaterialPlanCancelHeader> _MaterialPlanCancelHeaderRepository;
        RepositoryQuery<MaterialPlanCancelHeader> MaterialPlanCancelHeaderRepository;
        public MaterialPlanCancelHeaderService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _MaterialPlanCancelHeaderRepository = new Repository<MaterialPlanCancelHeader>(db);
            MaterialPlanCancelHeaderRepository = new RepositoryQuery<MaterialPlanCancelHeader>(_MaterialPlanCancelHeaderRepository);
        }


        public MaterialPlanCancelHeader Find(int id)
        {
            return _unitOfWork.Repository<MaterialPlanCancelHeader>().Find(id);
        }

        public MaterialPlanCancelHeader Create(MaterialPlanCancelHeader pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<MaterialPlanCancelHeader>().Insert(pt);
            return pt;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<MaterialPlanCancelHeader>().Delete(id);
        }

        public void Delete(MaterialPlanCancelHeader pt)
        {
            _unitOfWork.Repository<MaterialPlanCancelHeader>().Delete(pt);
        }

        public void Update(MaterialPlanCancelHeader pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<MaterialPlanCancelHeader>().Update(pt);
        }

        public IEnumerable<MaterialPlanCancelHeader> GetPagedList(int pageNumber, int pageSize, out int totalRecords)
        {
            var so = _unitOfWork.Repository<MaterialPlanCancelHeader>()
                .Query()
                .OrderBy(q => q.OrderBy(c => c.DocNo))
                .GetPage(pageNumber, pageSize, out totalRecords);

            return so;
        }

        public IQueryable<MaterialPlanCancelHeaderViewModel> GetMaterialPlanCancelHeaderList(int DocTypeId, string Uname)
        {

            int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

            return (from p in db.MaterialPlanCancelHeader
                    orderby p.DocDate descending, p.DocNo descending
                    where p.DocTypeId == DocTypeId && p.SiteId == SiteId && p.DivisionId == DivisionId
                    select new MaterialPlanCancelHeaderViewModel
                    {
                        DocDate = p.DocDate,
                        DocNo = p.DocNo,
                        DocTypeName = p.DocType.DocumentTypeName,
                        MaterialPlanCancelHeaderId = p.MaterialPlanCancelHeaderId,
                        Remark = p.Remark,
                        Status = p.Status,
                        ModifiedBy=p.ModifiedBy,
                        ReviewCount = p.ReviewCount,
                        ReviewBy = p.ReviewBy,
                        Reviewed = (SqlFunctions.CharIndex(Uname, p.ReviewBy) > 0),
                        BuyerName=p.Buyer.Person.Name,
                    }
                        );

        }

        public IQueryable<MaterialPlanCancelHeaderViewModel> GetMaterialPlanCancelHeaderListPendingToSubmit(int id, string Uname)
        {

            List<string> UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];
            var LedgerHeader = GetMaterialPlanCancelHeaderList(id, Uname).AsQueryable();

            var PendingToSubmit = from p in LedgerHeader
                                  where p.Status == (int)StatusConstants.Drafted || p.Status == (int)StatusConstants.Modified && (p.ModifiedBy == Uname || UserRoles.Contains("Admin"))
                                  select p;
            return PendingToSubmit;

        }

        public IQueryable<MaterialPlanCancelHeaderViewModel> GetMaterialPlanCancelHeaderListPendingToReview(int id, string Uname)
        {

            List<string> UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];
            var LedgerHeader = GetMaterialPlanCancelHeaderList(id, Uname).AsQueryable();

            var PendingToReview = from p in LedgerHeader
                                  where p.Status == (int)StatusConstants.Submitted && (SqlFunctions.CharIndex(Uname, (p.ReviewBy ?? "")) == 0)
                                  select p;
            return PendingToReview;

        }

        public MaterialPlanCancelHeader Add(MaterialPlanCancelHeader pt)
        {
            _unitOfWork.Repository<MaterialPlanCancelHeader>().Insert(pt);
            return pt;
        }

        public int NextId(int id)
        {
            int temp = 0;
            if (id != 0)
            {
                temp = (from p in db.MaterialPlanCancelHeader
                        orderby p.DocDate descending, p.DocNo descending
                        select p.MaterialPlanCancelHeaderId).AsEnumerable().SkipWhile(p => p != id).Skip(1).FirstOrDefault();
            }
            else
            {
                temp = (from p in db.MaterialPlanCancelHeader
                        orderby p.DocDate descending, p.DocNo descending
                        select p.MaterialPlanCancelHeaderId).FirstOrDefault();
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

                temp = (from p in db.MaterialPlanCancelHeader
                        orderby p.DocDate descending, p.DocNo descending
                        select p.MaterialPlanCancelHeaderId).AsEnumerable().TakeWhile(p => p != id).LastOrDefault();
            }
            else
            {
                temp = (from p in db.MaterialPlanCancelHeader
                        orderby p.DocDate descending, p.DocNo descending
                        select p.MaterialPlanCancelHeaderId).AsEnumerable().LastOrDefault();
            }
            if (temp != 0)
                return temp;
            else
                return id;
        }

        public string GetMaxDocNo()
        {
            int x;
            var maxVal = _unitOfWork.Repository<MaterialPlanCancelHeader>().Query().Get().Select(i => i.DocNo).DefaultIfEmpty().ToList().Select(sx => int.TryParse(sx, out x) ? x : 0).Max();
            return (maxVal + 1).ToString();
        }

        public void Dispose()
        {
        }


        public Task<IEquatable<MaterialPlanCancelHeader>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MaterialPlanCancelHeader> FindAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
