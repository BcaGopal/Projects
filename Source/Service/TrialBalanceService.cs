using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Infrastructure;
using Model.Models;
using Model.ViewModel;
using Core.Common;
using System;
using Model;
using System.Threading.Tasks;
using Data.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace Service
{
    public interface ITrialBalanceService : IDisposable
    {
        IEnumerable<TrialBalanceViewModel> GetTrialBalance(string UserName);
        IEnumerable<TrialBalanceSummaryViewModel> GetTrialBalanceSummary(string UserName);
        IEnumerable<SubTrialBalanceViewModel> GetSubTrialBalance(int ? id, string UserName);
        IEnumerable<SubTrialBalanceSummaryViewModel> GetSubTrialBalanceSummary(int ? id, string UserName);
        IEnumerable<LedgerBalanceViewModel> GetLedgerBalance(int id,string UserName);
    }

    public class TrialBalanceService : ITrialBalanceService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        public TrialBalanceService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }


        public IEnumerable<TrialBalanceViewModel> GetTrialBalance(string UserName)
        {

            var Settings = new TrialBalanceSettingService(_unitOfWork).GetTrailBalanceSetting(UserName);

            string SiteId = Settings.SiteIds;
            string DivisionId = Settings.DivisionIds;
            string AsOnDate = Settings.ToDate.HasValue ? Settings.ToDate.Value.ToString("dd/MMM/yyyy") : "";

            SqlParameter SqlParameterSiteId = new SqlParameter("@Site", !string.IsNullOrEmpty(SiteId) ? SiteId : (object)DBNull.Value);
            SqlParameter SqlParameterDivisionId = new SqlParameter("@Division", !string.IsNullOrEmpty(DivisionId) ? DivisionId : (object)DBNull.Value);
            SqlParameter SqlParameterDate = new SqlParameter("@AsOnDate", AsOnDate);

            IEnumerable<TrialBalanceViewModel> TrialBalanceList = db.Database.SqlQuery<TrialBalanceViewModel>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".spTrialBalance @Site, @Division, @AsOnDate", SqlParameterSiteId, SqlParameterDivisionId, SqlParameterDate).ToList();

            return TrialBalanceList;

        }

        public IEnumerable<TrialBalanceSummaryViewModel> GetTrialBalanceSummary(string UserName)
        {

            var Settings = new TrialBalanceSettingService(_unitOfWork).GetTrailBalanceSetting(UserName);

            string SiteId = Settings.SiteIds;
            string DivisionId = Settings.DivisionIds;
            string FromDate = Settings.FromDate.HasValue ? Settings.FromDate.Value.ToString("dd/MMM/yyyy") : "";
            string ToDate = Settings.ToDate.HasValue ? Settings.ToDate.Value.ToString("dd/MMM/yyyy") : "";

            SqlParameter SqlParameterSiteId = new SqlParameter("@Site", !string.IsNullOrEmpty(SiteId) ? SiteId : (object)DBNull.Value);
            SqlParameter SqlParameterDivisionId = new SqlParameter("@Division", !string.IsNullOrEmpty(DivisionId) ? DivisionId : (object)DBNull.Value);
            SqlParameter SqlParameterFromDate = new SqlParameter("@FromDate", FromDate);
            SqlParameter SqlParameterToDate = new SqlParameter("@ToDate", ToDate);

            IEnumerable<TrialBalanceSummaryViewModel> TrialBalanceList = db.Database.SqlQuery<TrialBalanceSummaryViewModel>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".spTrialBalanceSummary @Site, @Division, @FromDate, @ToDate", SqlParameterSiteId, SqlParameterDivisionId, SqlParameterFromDate, SqlParameterToDate).ToList();

            return TrialBalanceList;

        }

        public IEnumerable<SubTrialBalanceViewModel> GetSubTrialBalance(int ? id, string UserName)
        {

            var Settings = new TrialBalanceSettingService(_unitOfWork).GetTrailBalanceSetting(UserName);

            string SiteId = Settings.SiteIds;
            string DivisionId = Settings.DivisionIds;
            string AsOnDate = Settings.ToDate.HasValue ? Settings.ToDate.Value.ToString("dd/MMM/yyyy") : "";

            if(id.HasValue)
            {
                SqlParameter SqlParameterSiteId = new SqlParameter("@Site", !string.IsNullOrEmpty(SiteId) ? SiteId : (object)DBNull.Value);
                SqlParameter SqlParameterDivisionId = new SqlParameter("@Division", !string.IsNullOrEmpty(DivisionId) ? DivisionId : (object)DBNull.Value);
            SqlParameter SqlParameterDate = new SqlParameter("@AsOnDate", AsOnDate);
            SqlParameter SqlParameterLedgerAccountGroupId = new SqlParameter("@LedgerAccountGroupId", id.Value);

            IEnumerable<SubTrialBalanceViewModel> TrialBalanceList = db.Database.SqlQuery<SubTrialBalanceViewModel>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".spSubTrialBalance @Site, @Division, @AsOnDate, @LedgerAccountGroupId", SqlParameterSiteId, SqlParameterDivisionId, SqlParameterDate, SqlParameterLedgerAccountGroupId).ToList();
            return TrialBalanceList;
            }
            else
            {
                SqlParameter SqlParameterSiteId = new SqlParameter("@Site", !string.IsNullOrEmpty(SiteId) ? SiteId : (object)DBNull.Value);
                SqlParameter SqlParameterDivisionId = new SqlParameter("@Division", !string.IsNullOrEmpty(DivisionId) ? DivisionId : (object)DBNull.Value);
                SqlParameter SqlParameterDate = new SqlParameter("@AsOnDate", AsOnDate);                

                IEnumerable<SubTrialBalanceViewModel> TrialBalanceList = db.Database.SqlQuery<SubTrialBalanceViewModel>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".spSubTrialBalance @Site, @Division, @AsOnDate", SqlParameterSiteId, SqlParameterDivisionId, SqlParameterDate).ToList();
                return TrialBalanceList;
            }

           

        }

        public IEnumerable<SubTrialBalanceSummaryViewModel> GetSubTrialBalanceSummary(int ? id, string UserName)
        {

            var Settings = new TrialBalanceSettingService(_unitOfWork).GetTrailBalanceSetting(UserName);

            string SiteId = Settings.SiteIds;
            string DivisionId = Settings.DivisionIds;
            string FromDate = Settings.FromDate.HasValue ? Settings.FromDate.Value.ToString("dd/MMM/yyyy") : "";
            string ToDate = Settings.ToDate.HasValue ? Settings.ToDate.Value.ToString("dd/MMM/yyyy") : "";

            if(id.HasValue && id.Value > 0)
            {
                SqlParameter SqlParameterSiteId = new SqlParameter("@Site", !string.IsNullOrEmpty(SiteId) ? SiteId : (object)DBNull.Value);
            SqlParameter SqlParameterDivisionId = new SqlParameter("@Division", !string.IsNullOrEmpty(DivisionId) ? DivisionId : (object)DBNull.Value);
            SqlParameter SqlParameterFromDate = new SqlParameter("@FromDate", FromDate);
            SqlParameter SqlParameterToDate = new SqlParameter("@ToDate", ToDate);
            SqlParameter SqlParameterLedgerAccountGroupId = new SqlParameter("@LedgerAccountGroupId", id);

            IEnumerable<SubTrialBalanceSummaryViewModel> TrialBalanceList = db.Database.SqlQuery<SubTrialBalanceSummaryViewModel>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".spSubTrialBalanceSummary @Site, @Division, @FromDate, @ToDate, @LedgerAccountGroupId", SqlParameterSiteId, SqlParameterDivisionId, SqlParameterFromDate, SqlParameterToDate, SqlParameterLedgerAccountGroupId).ToList();

            return TrialBalanceList;
            }
            else
            {
                SqlParameter SqlParameterSiteId = new SqlParameter("@Site", !string.IsNullOrEmpty(SiteId) ? SiteId : (object)DBNull.Value);
                SqlParameter SqlParameterDivisionId = new SqlParameter("@Division", !string.IsNullOrEmpty(DivisionId) ? DivisionId : (object)DBNull.Value);
                SqlParameter SqlParameterFromDate = new SqlParameter("@FromDate", FromDate);
                SqlParameter SqlParameterToDate = new SqlParameter("@ToDate", ToDate);

                IEnumerable<SubTrialBalanceSummaryViewModel> TrialBalanceList = db.Database.SqlQuery<SubTrialBalanceSummaryViewModel>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".spSubTrialBalanceSummary @Site, @Division, @FromDate, @ToDate", SqlParameterSiteId, SqlParameterDivisionId, SqlParameterFromDate, SqlParameterToDate).ToList();

                return TrialBalanceList;
            }
        }


        public IEnumerable<LedgerBalanceViewModel> GetLedgerBalance(int id,string UserName)
        {

            var Settings = new TrialBalanceSettingService(_unitOfWork).GetTrailBalanceSetting(UserName);

            string SiteId = Settings.SiteIds;
            string DivisionId = Settings.DivisionIds;
            string FromDate = Settings.FromDate.HasValue ? Settings.FromDate.Value.ToString("dd/MMM/yyyy") : "";
            string ToDate = Settings.ToDate.HasValue ? Settings.ToDate.Value.ToString("dd/MMM/yyyy") : "";

            SqlParameter SqlParameterSiteId = new SqlParameter("@Site", !string.IsNullOrEmpty(SiteId) ? SiteId : (object)DBNull.Value);
            SqlParameter SqlParameterDivisionId = new SqlParameter("@Division", !string.IsNullOrEmpty(DivisionId) ? DivisionId : (object)DBNull.Value);
            SqlParameter SqlParameterFromDate = new SqlParameter("@FromDate", FromDate);
            SqlParameter SqlParameterToDate = new SqlParameter("@ToDate", ToDate);
            SqlParameter SqlParameterLedgerAccountId = new SqlParameter("@LedgerAccountId", id);

            IEnumerable<LedgerBalanceViewModel> TrialBalanceList = db.Database.SqlQuery<LedgerBalanceViewModel>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".spLedger @Site, @Division, @FromDate, @ToDate, @LedgerAccountId", SqlParameterSiteId, SqlParameterDivisionId, SqlParameterFromDate, SqlParameterToDate, SqlParameterLedgerAccountId).ToList();

            return TrialBalanceList;

        }

        public void Dispose()
        {
        }
    }
}
