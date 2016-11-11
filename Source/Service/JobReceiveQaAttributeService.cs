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
    public interface IJobReceiveQAAttributeService : IDisposable
    {
        void Create(JobReceiveQAAttributeViewModel pt, string UserName);
        IQueryable<JobReceivePendingToQAIndex> GetJobReceiveQAAttributeList(int DocTypeId, string Uname);//DocumentTypeId
        List<QAGroupLineLineViewModel> GetJobReceiveQAAttribute(int JobReceiveLineid);//JobReceiveLineId
        Task<IEquatable<JobReceiveQAAttribute>> GetAsync();
        Task<JobReceiveQAAttribute> FindAsync(int id);
    }

    public class JobReceiveQAAttributeService : IJobReceiveQAAttributeService
    {
        ApplicationDbContext db;

        IUnitOfWork _unitOfWork;
        IJobReceiveQAHeaderService _JobReceiveQAHeaderService;
        IJobReceiveQALineService _JobReceiveQALineService;
        public JobReceiveQAAttributeService(ApplicationDbContext db)
        {

            this.db = db;
        }

        public JobReceiveQAAttributeService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public JobReceiveQAAttribute Find(int id)
        {
            return db.JobReceiveQAAttribute.Find(id);
        }

        public void Create(JobReceiveQAAttributeViewModel pt,string UserName)
        {
            JobReceiveQAHeader header = new JobReceiveQAHeader();
            header = Mapper.Map<JobReceiveQAAttributeViewModel, JobReceiveQAHeader>(pt);
            _JobReceiveQAHeaderService.Create(header, UserName);

            JobReceiveQALine Line = Mapper.Map<JobReceiveQAAttributeViewModel, JobReceiveQALine>(pt);
            Line.Sr = _JobReceiveQALineService.GetMaxSr(Line.JobReceiveQAHeaderId);
            Line.FailQty = Line.QAQty - Line.Qty;
            Line.FailDealQty = Line.FailQty * Line.UnitConversionMultiplier;
            new JobReceiveLineStatusService(_unitOfWork).UpdateJobReceiveQtyOnQA(Mapper.Map<JobReceiveQALineViewModel>(Line), pt.DocDate, ref db);
            _JobReceiveQALineService.Create(Line, UserName);


            List<QAGroupLineLineViewModel> tem = pt.QAGroupLine;

            if (tem != null)
            {
                foreach (var item in tem)
                {
                    JobReceiveQAAttribute pa = new JobReceiveQAAttribute();
                    pa.JobReceiveQALineId = Line.JobReceiveQALineId;
                    pa.QAGroupLineId = item.QAGroupLineId;
                    pa.Value = item.Value;
                    pa.Remarks = item.Remarks;
                    pa.CreatedBy = UserName;
                    pa.ModifiedBy = UserName;
                    pa.CreatedDate = DateTime.Now;
                    pa.ModifiedDate = DateTime.Now;
                    pa.ObjectState = ObjectState.Added;
                    db.JobReceiveQAAttribute.Add(pa);
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
                    join Ql in db.JobReceiveQALine on L.JobReceiveLineId equals Ql.JobReceiveLineId into JobReceiveQaLineTable from JobReceiveQaLineTab in JobReceiveQaLineTable.DefaultIfEmpty()
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
                        ProductUidName = JobReceiveLineTab.ProductUid.ProductUidName
                        
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
