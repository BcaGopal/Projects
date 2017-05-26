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
using Model.ViewModels;

namespace Service
{
    public interface IJobInvoiceHeaderService : IDisposable
    {
        JobInvoiceHeader Create(JobInvoiceHeader pt);
        void Delete(int id);
        void Delete(JobInvoiceHeader pt);
        JobInvoiceHeader Find(string Name);
        JobInvoiceHeader Find(int id);
        IEnumerable<JobInvoiceHeader> GetPagedList(int pageNumber, int pageSize, out int totalRecords);
        void Update(JobInvoiceHeader pt);
        JobInvoiceHeader Add(JobInvoiceHeader pt);
        JobInvoiceHeaderViewModel GetJobInvoiceHeader(int id);//HeadeRId
        IQueryable<JobInvoiceHeaderViewModel> GetJobInvoiceHeaderList(int id, string Uname,bool AutoReceipt);
        IQueryable<JobInvoiceHeaderViewModel> GetJobInvoiceHeaderListPendingToSubmit(int id, string Uname, bool AutoReceipt);
        IQueryable<JobInvoiceHeaderViewModel> GetJobInvoiceHeaderListPendingToReview(int id, string Uname, bool AutoReceipt);
        Task<IEquatable<JobInvoiceHeader>> GetAsync();
        Task<JobInvoiceHeader> FindAsync(int id);
        int NextId(int id);
        int PrevId(int id);
        string GetMaxDocNo();
        IQueryable<ComboBoxResult> GetCustomPerson(int Id, string term);
        PersonViewModel GetJobWorkerDetail(int id);
    }

    public class JobInvoiceHeaderService : IJobInvoiceHeaderService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<JobInvoiceHeader> _JobInvoiceHeaderRepository;
        RepositoryQuery<JobInvoiceHeader> JobInvoiceHeaderRepository;
        public JobInvoiceHeaderService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _JobInvoiceHeaderRepository = new Repository<JobInvoiceHeader>(db);
            JobInvoiceHeaderRepository = new RepositoryQuery<JobInvoiceHeader>(_JobInvoiceHeaderRepository);
        }

        public JobInvoiceHeader Find(string Name)
        {
            return JobInvoiceHeaderRepository.Get().Where(i => i.DocNo == Name).FirstOrDefault();
        }


        public JobInvoiceHeader Find(int id)
        {
            return _unitOfWork.Repository<JobInvoiceHeader>().Find(id);
        }

        public JobInvoiceHeader Create(JobInvoiceHeader pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<JobInvoiceHeader>().Insert(pt);
            return pt;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<JobInvoiceHeader>().Delete(id);
        }

        public void Delete(JobInvoiceHeader pt)
        {
            _unitOfWork.Repository<JobInvoiceHeader>().Delete(pt);
        }

        public void Update(JobInvoiceHeader pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<JobInvoiceHeader>().Update(pt);
        }

        public IEnumerable<JobInvoiceHeader> GetPagedList(int pageNumber, int pageSize, out int totalRecords)
        {
            var so = _unitOfWork.Repository<JobInvoiceHeader>()
                .Query()
                .OrderBy(q => q.OrderBy(c => c.DocNo))
                .GetPage(pageNumber, pageSize, out totalRecords);

            return so;
        }
        public JobInvoiceHeaderViewModel GetJobInvoiceHeader(int id)
        {
            return (from p in db.JobInvoiceHeader
                    where p.JobInvoiceHeaderId == id
                    select new JobInvoiceHeaderViewModel
                    {
                        JobInvoiceHeaderId = p.JobInvoiceHeaderId,
                        DivisionId = p.DivisionId,
                        DocNo = p.DocNo,
                        DocDate = p.DocDate,
                        ProcessId=p.ProcessId,                        
                        DocTypeId = p.DocTypeId,
                        JobWorkerDocNo=p.JobWorkerDocNo,
                        JobWorkerDocDate=p.JobWorkerDocDate,
                        Remark = p.Remark,
                        SiteId = p.SiteId,
                        Status = p.Status,
                        JobWorkerId=p.JobWorkerId,
                        FinancierId = p.FinancierId,
                        JobReceiveHeaderId=p.JobReceiveHeaderId,
                        SalesTaxGroupPersonId = p.SalesTaxGroupPersonId,
                        ModifiedBy=p.ModifiedBy,
                        LockReason=p.LockReason,
                        CreatedBy=p.CreatedBy,
                        CreatedDate=p.CreatedDate,
                    }

                        ).FirstOrDefault();
        }

        public JobInvoiceHeader Add(JobInvoiceHeader pt)
        {
            _unitOfWork.Repository<JobInvoiceHeader>().Insert(pt);
            return pt;
        }

        public int NextId(int id)
        {
            int temp = 0;
            if (id != 0)
            {
                temp = (from p in db.JobInvoiceHeader
                        orderby p.DocDate descending, p.DocNo descending
                        select p.JobInvoiceHeaderId).AsEnumerable().SkipWhile(p => p != id).Skip(1).FirstOrDefault();
            }
            else
            {
                temp = (from p in db.JobInvoiceHeader
                        orderby p.DocDate descending, p.DocNo descending
                        select p.JobInvoiceHeaderId).FirstOrDefault();
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

                temp = (from p in db.JobInvoiceHeader
                        orderby p.DocDate descending, p.DocNo descending
                        select p.JobInvoiceHeaderId).AsEnumerable().TakeWhile(p => p != id).LastOrDefault();
            }
            else
            {
                temp = (from p in db.JobInvoiceHeader
                        orderby p.DocDate descending, p.DocNo descending
                        select p.JobInvoiceHeaderId).AsEnumerable().LastOrDefault();
            }
            if (temp != 0)
                return temp;
            else
                return id;
        }

        public string GetMaxDocNo()
        {
            int x;
            var maxVal = _unitOfWork.Repository<JobInvoiceHeader>().Query().Get().Select(i => i.DocNo).DefaultIfEmpty().ToList().Select(sx => int.TryParse(sx, out x) ? x : 0).Max();
            return (maxVal + 1).ToString();
        }

        public IQueryable<JobInvoiceHeaderViewModel> GetJobInvoiceHeaderList(int id, string Uname, bool AutoReceipt)
        {

            int SiteId=(int)System.Web.HttpContext.Current.Session["SiteId"];
            int DivisionId=(int)System.Web.HttpContext.Current.Session["DivisionId"];

            List<string> UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"]; 

            return (from p in db.JobInvoiceHeader
                    orderby p.DocDate descending, p.DocNo descending
                    where p.SiteId == SiteId && p.DivisionId == DivisionId && p.DocTypeId == id
                    && (AutoReceipt ? p.JobReceiveHeaderId!=null : 1==1)
                    select new JobInvoiceHeaderViewModel
                    {
                        DocDate=p.DocDate,
                        DocNo=p.DocNo,
                        DocTypeName=p.DocType.DocumentTypeName,
                        JobInvoiceHeaderId=p.JobInvoiceHeaderId,
                        Remark=p.Remark,
                        Status=p.Status,
                        JobWorkerDocNo=p.JobWorkerDocNo,
                        JobWorkerName=p.JobWorker.Name,
                        ModifiedBy=p.ModifiedBy,
                        ReviewCount=p.ReviewCount,
                        GodownName = AutoReceipt ? p.JobReceiveHeader.Godown.GodownName :"",
                        ReviewBy=p.ReviewBy,
                        Reviewed = (SqlFunctions.CharIndex(Uname, p.ReviewBy) > 0),
                        TotalQty = p.JobInvoiceLines.Sum(m => m.Qty),
                        DecimalPlaces = (from o in p.JobInvoiceLines
                                         join rl in db.JobReceiveLine on o.JobReceiveLineId equals rl.JobReceiveLineId
                                         join ol in db.JobOrderLine on rl.JobOrderLineId equals ol.JobOrderLineId
                                         join prod in db.Product on ol.ProductId equals prod.ProductId
                                         join u in db.Units on prod.UnitId equals u.UnitId
                                         select u.DecimalPlaces).Max(),
                    });
        }


        public IQueryable<JobInvoiceHeaderViewModel> GetJobInvoiceHeaderListPendingToSubmit(int id, string Uname, bool AutoReceipt)
        {

            List<string> UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];
            var JobOrderHeader = GetJobInvoiceHeaderList(id, Uname,AutoReceipt).AsQueryable();

            var PendingToSubmit = from p in JobOrderHeader
                                  where p.Status == (int)StatusConstants.Drafted || p.Status == (int)StatusConstants.Import || p.Status == (int)StatusConstants.Modified && (p.ModifiedBy == Uname || UserRoles.Contains("Admin"))
                                  select p;
            return PendingToSubmit;

        }

        public IQueryable<JobInvoiceHeaderViewModel> GetJobInvoiceHeaderListPendingToReview(int id, string Uname, bool AutoReceipt)
        {

            List<string> UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];
            var JobOrderHeader = GetJobInvoiceHeaderList(id, Uname, AutoReceipt).AsQueryable();

            var PendingToReview = from p in JobOrderHeader
                                  where p.Status == (int)StatusConstants.Submitted && (SqlFunctions.CharIndex(Uname, (p.ReviewBy ?? "")) == 0)
                                   select p;
            return PendingToReview;

        }


        public IQueryable<ComboBoxResult> GetCustomPerson(int Id, string term)
        {
            int DocTypeId = Id;
            int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];

            var settings = new JobInvoiceSettingsService(_unitOfWork).GetJobInvoiceSettingsForDocument(DocTypeId, DivisionId, SiteId);

            string[] PersonRoles = null;
            if (!string.IsNullOrEmpty(settings.filterPersonRoles)) { PersonRoles = settings.filterPersonRoles.Split(",".ToCharArray()); }
            else { PersonRoles = new string[] { "NA" }; }

            string DivIdStr = "|" + DivisionId.ToString() + "|";
            string SiteIdStr = "|" + SiteId.ToString() + "|";

            var list = (from p in db.Persons
                        join bus in db.BusinessEntity on p.PersonID equals bus.PersonID into BusinessEntityTable
                        from BusinessEntityTab in BusinessEntityTable.DefaultIfEmpty()
                        join pp in db.PersonProcess on p.PersonID equals pp.PersonId into PersonProcessTable
                        from PersonProcessTab in PersonProcessTable.DefaultIfEmpty()
                        join pr in db.PersonRole on p.PersonID equals pr.PersonId into PersonRoleTable
                        from PersonRoleTab in PersonRoleTable.DefaultIfEmpty()
                        where PersonProcessTab.ProcessId == settings.ProcessId
                        && (string.IsNullOrEmpty(term) ? 1 == 1 : (p.Name.ToLower().Contains(term.ToLower()) || p.Code.ToLower().Contains(term.ToLower())))
                        && (string.IsNullOrEmpty(settings.filterPersonRoles) ? 1 == 1 : PersonRoles.Contains(PersonRoleTab.RoleDocTypeId.ToString()))
                        && BusinessEntityTab.DivisionIds.IndexOf(DivIdStr) != -1
                        && BusinessEntityTab.SiteIds.IndexOf(SiteIdStr) != -1
                        && (p.IsActive == null ? 1 == 1 : p.IsActive == true)
                        group new { p } by new { p.PersonID } into Result
                        orderby Result.Max(m => m.p.Name)
                        select new ComboBoxResult
                        {
                            id = Result.Key.PersonID.ToString(),
                            text = Result.Max(m => m.p.Name + "|" + m.p.Code),
                        }
              );

            return list;
        }

        public PersonViewModel GetJobWorkerDetail(int id)
        {
            var temp = (from b in db.BusinessEntity
                        where b.PersonID == id
                        select new PersonViewModel
                        {
                            SalesTaxGroupPartyId = b.SalesTaxGroupPartyId,
                            SalesTaxGroupPartyName = b.SalesTaxGroupParty.ChargeGroupPersonName
                        }).FirstOrDefault();

            return (temp);
        }

        public void Dispose()
        {
        }


        public Task<IEquatable<JobInvoiceHeader>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<JobInvoiceHeader> FindAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
