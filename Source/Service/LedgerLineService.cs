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
using System.Data.SqlClient;
using System.Configuration;

namespace Service
{
    public interface ILedgerLineService : IDisposable
    {
        LedgerLine Create(LedgerLine pt);
        void Delete(int id);
        void Delete(LedgerLine pt);
        LedgerLine Find(int id);
        IEnumerable<LedgerLine> GetPagedList(int pageNumber, int pageSize, out int totalRecords);
        void Update(LedgerLine pt);
        LedgerLine Add(LedgerLine pt);
        IEnumerable<LedgerLine> GetLedgerLineList();
        Task<IEquatable<LedgerLine>> GetAsync();
        Task<LedgerLine> FindAsync(int id);
        int NextId(int id);
        int PrevId(int id);
      //  LedgerLine FindByLedgerHeader(int id);
        IEnumerable<LedgerLine> FindByLedgerHeader(int id);
        IQueryable<ComboBoxResult> GetLedgerAccounts(string term, string AccGroups, string Process);
        IQueryable<ComboBoxResult> GetCostCenters(string term, string DocTypes, string Process);        
    }

    public class LedgerLineService : ILedgerLineService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<LedgerLine> _LedgerLineRepository;
        RepositoryQuery<LedgerLine> LedgerLineRepository;
        public LedgerLineService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _LedgerLineRepository = new Repository<LedgerLine>(db);
            LedgerLineRepository = new RepositoryQuery<LedgerLine>(_LedgerLineRepository);
        }

        //public LedgerLine FindByLedgerHeader(int id)
        //{
        //    return (from p in db.LedgerLine
        //            where p.LedgerHeaderId == id
        //            select p).FirstOrDefault();
        //}

        //public LedgerLine FindByLedgerHeader(int id)
        //{
        //    return (from p in db.LedgerLine
        //            where p.LedgerHeaderId == id
        //            select p).FirstOrDefault();
        //}

        public IEnumerable<LedgerLine> FindByLedgerHeader(int id)
        {
            var pt = _unitOfWork.Repository<LedgerLine>().Query().Get().Where(m=>m.LedgerHeaderId == id).OrderBy(m => m.LedgerLineId);

            return pt;
        }

        public LedgerLine Find(int id)
        {
            return _unitOfWork.Repository<LedgerLine>().Find(id);
        }

        public LedgerLine Create(LedgerLine pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<LedgerLine>().Insert(pt);
            return pt;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<LedgerLine>().Delete(id);
        }

        public void Delete(LedgerLine pt)
        {
            _unitOfWork.Repository<LedgerLine>().Delete(pt);
        }
        public void Update(LedgerLine pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<LedgerLine>().Update(pt);
        }

        public IEnumerable<LedgerLine> GetPagedList(int pageNumber, int pageSize, out int totalRecords)
        {
            var so = _unitOfWork.Repository<LedgerLine>()
                .Query()
                .OrderBy(q => q.OrderBy(c => c.LedgerLineId))
                .GetPage(pageNumber, pageSize, out totalRecords);

            return so;
        }

        public IEnumerable<LedgerLine> GetLedgerLineList()
        {
            var pt = _unitOfWork.Repository<LedgerLine>().Query().Get().OrderBy(m => m.LedgerLineId);

            return pt;
        }

        public LedgerLine Add(LedgerLine pt)
        {
            _unitOfWork.Repository<LedgerLine>().Insert(pt);
            return pt;
        }

        public int NextId(int id)
        {
            int temp = 0;
            if (id != 0)
            {
                temp = (from p in db.LedgerLine
                        orderby p.LedgerLineId
                        select p.LedgerLineId).AsEnumerable().SkipWhile(p => p != id).Skip(1).FirstOrDefault();
            }
            else
            {
                temp = (from p in db.LedgerLine
                        orderby p.LedgerLineId
                        select p.LedgerLineId).FirstOrDefault();
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

                temp = (from p in db.LedgerLine
                        orderby p.LedgerLineId
                        select p.LedgerLineId).AsEnumerable().TakeWhile(p => p != id).LastOrDefault();
            }
            else
            {
                temp = (from p in db.LedgerLine
                        orderby p.LedgerLineId
                        select p.LedgerLineId).AsEnumerable().LastOrDefault();
            }
            if (temp != 0)
                return temp;
            else
                return id;
        }

        public IEnumerable<LedgerList> GetPersonPendingBills(int LedgerHeaderId, int LedgerAccountId, string ReferenceType, string term, int Limit)
        {

            LedgerHeader Ledger = new LedgerHeaderService(_unitOfWork).Find(LedgerHeaderId);

            var Settings=new LedgerSettingService(_unitOfWork).GetLedgerSettingForDocument(Ledger.DocTypeId,Ledger.DivisionId,Ledger.SiteId);

            IEnumerable<LedgerList> PendingBillList =new List<LedgerList>();

            if(!string.IsNullOrEmpty(Settings.SqlProcReferenceNo))
            { 
            SqlParameter SqlParameterLedgerAccountId = new SqlParameter("@LedgerAccountId", LedgerAccountId);
            SqlParameter SqlParameterReferenceType = new SqlParameter("@ReferenceType", ReferenceType);
            SqlParameter SqlParameterLimit = new SqlParameter("@Limit", Limit);
            SqlParameter SqlParameterTerm = new SqlParameter("@Term", term);

            PendingBillList= db.Database.SqlQuery<LedgerList>("" + Settings.SqlProcReferenceNo + " @LedgerAccountId, @ReferenceType, @Limit, @Term", SqlParameterLedgerAccountId, SqlParameterReferenceType, SqlParameterLimit, SqlParameterTerm).ToList();
            }

            return PendingBillList;
        }

        


        public void Dispose()
        {
        }


        public Task<IEquatable<LedgerLine>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LedgerLine> FindAsync(int id)
        {
            throw new NotImplementedException();
        }



        public IQueryable<ComboBoxResult> GetLedgerAccounts(string term, string AccGroups, string Process)
        {          

            string[] ContraAccGroups = null;
            if (!string.IsNullOrEmpty(AccGroups)) { ContraAccGroups = AccGroups.Split(",".ToCharArray()); }
            else { ContraAccGroups = new string[] { "NA" }; }


            string[] ContraProcess = null;
            if (!string.IsNullOrEmpty(Process)) { ContraProcess = Process.Split(",".ToCharArray()); }
            else { ContraProcess = new string[] { "NA" }; }

            int CurrentSiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            int CurrentDivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];

            string DivId = "|" + CurrentDivisionId.ToString() + "|";
            string SiteId = "|" + CurrentSiteId.ToString() + "|";


            var temp = (from p in db.LedgerAccount
                        join t2 in db.BusinessEntity on p.PersonId equals t2.PersonID into table2 from tab2 in table2.DefaultIfEmpty()
                        join t in db.Persons on tab2.PersonID equals t.PersonID into table
                        from tab in table.DefaultIfEmpty()
                        join t3 in db.PersonProcess on tab.PersonID equals t3.PersonId into table3 from perproc in table3.DefaultIfEmpty()
                        where (string.IsNullOrEmpty(AccGroups) ? 1 == 1 : ContraAccGroups.Contains(p.LedgerAccountGroupId.ToString()))
                        && (string.IsNullOrEmpty(term) ? 1 == 1 : (p.LedgerAccountName.ToLower().Contains(term.ToLower())) || tab.Code.ToLower().Contains(term.ToLower()))
                        && (tab2 == null || ((string.IsNullOrEmpty(Process) ? 1 == 1 : ContraProcess.Contains(perproc.ProcessId.ToString())) && tab2.DivisionIds.IndexOf(DivId) != -1
                        && tab2.SiteIds.IndexOf(SiteId) != -1))
                        select new ComboBoxResult
                        {
                            text=p.LedgerAccountName + (tab2==null? "" : " | " + tab.Suffix +" | " + tab.Code),
                            id=p.LedgerAccountId.ToString(),
                        });

            var GroupedRec = from p in temp
                             group p by p.id into g
                             orderby g.Max(m=>m.text)
                             select new ComboBoxResult
                             {
                                 text = g.Max(m => m.text),
                                 id = g.Max(m => m.id),
                             };

            return GroupedRec;
        }

        public IQueryable<ComboBoxResult> GetCostCenters(string term, string DocTypes, string Process)
        {

            string[] ContraDocTypes = null;
            if (!string.IsNullOrEmpty(DocTypes)) { ContraDocTypes = DocTypes.Split(",".ToCharArray()); }
            else { ContraDocTypes = new string[] { "NA" }; }

            string[] ContraProcess = null;
            if (!string.IsNullOrEmpty(Process)) { ContraProcess = Process.Split(",".ToCharArray()); }
            else { ContraProcess = new string[] { "NA" }; }

            int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];


            var temp = (from p in db.CostCenter
                        where (string.IsNullOrEmpty(DocTypes) ? 1 == 1 : ContraDocTypes.Contains(p.DocTypeId.ToString()))
                        && (string.IsNullOrEmpty(term) ? 1 == 1 : p.CostCenterName.ToLower().Contains(term.ToLower()))
                        && (string.IsNullOrEmpty(Process) ? 1 == 1 : ContraProcess.Contains(p.ProcessId.ToString()))
                        && (string.IsNullOrEmpty(p.SiteId.ToString()) ? 1 == 1 : p.SiteId==SiteId )
                        && (string.IsNullOrEmpty(p.DivisionId.ToString()) ? 1 == 1 : p.DivisionId==DivisionId )
                        && p.IsActive == true
                        orderby p.CostCenterName
                        select new ComboBoxResult
                        {
                            text = p.CostCenterName +" | "+p.DocType.DocumentTypeShortName,
                            id = p.CostCenterId.ToString(),
                        });
            return temp;
        }


        public IQueryable<ComboBoxResult> GetReferenceDocIds(int Id, string term)
        {
            SqlParameter SqlParameterDocTypeId = new SqlParameter("@DocTypeId", Id);

            IQueryable<ComboBoxResult> ComboBoxResult = db.Database.SqlQuery<ComboBoxResult>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".spGetHelpListReferenceDocIds" + " @DocTypeId", SqlParameterDocTypeId).ToList().AsQueryable();

            return (from p in ComboBoxResult
                    where (string.IsNullOrEmpty(term) ? 1 == 1 : p.text.ToLower().Contains(term.ToLower()))
                    select new ComboBoxResult
                    {
                        id = p.id,
                        text = p.text,
                    });
        }

        public ComboBoxResult SetReferenceDocIds(int Id, int DocTypeId)
        {
            SqlParameter SqlParameterDocId = new SqlParameter("@DocId", Id);
            SqlParameter SqlParameterDocTypeId = new SqlParameter("@DocTypeId", DocTypeId);

            ComboBoxResult ComboBoxResult = db.Database.SqlQuery<ComboBoxResult>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".spSetHelpListReferenceDocIds" + " @DocTypeId, @DocId", SqlParameterDocTypeId, SqlParameterDocId).FirstOrDefault();

            return ComboBoxResult;
        }

    }

    public class LedgerList
    {
        public int LedgerId { get; set; }

        public string LedgerDocNo { get; set; }
        public Decimal Amount { get; set; }
    }
}



