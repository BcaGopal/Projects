using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class TrialBalanceViewModel
    {
        public int LedgerAccountGroupId { get; set; }
        public string LedgerAccountGroupName { get; set; }
        public decimal ? AmtDr { get; set; }
        public decimal ? AmtCr { get; set; }
    }

    public class TrialBalanceSummaryViewModel
    {
        public int LedgerAccountGroupId { get; set; }
        public string LedgerAccountGroupName { get; set; }
        public decimal ? AmtDr { get; set; }
        public decimal ? AmtCr { get; set; }
        public string OpeningType { get; set; }
        public string BalanceType { get; set; }
        public decimal ? Balance { get; set; }
        public decimal ? Opening { get; set; }
    }

    public class ProfitAndLossSummaryViewModel
    {
        public int Sr { get; set; }
        public int DrGroupId { get; set; }
        public string DrParticular { get; set; }
        public string AmtDr { get; set; }
        public int CrGroupId { get; set; }
        public string CrParticular { get; set; }
        public string AmtCr { get; set; }
    }

    public class BalanceSheetSummaryViewModel
    {
        public Int64 ? Sr { get; set; }
        
        public int? AssetMainGroupId { get; set; }
        public int? Assetlabel { get; set; }
        public int ?  AssetGroupId { get; set; }
        public string AssetGroupName { get; set; }
        public decimal? SubAssetAmount { get; set; }
        public decimal? AssetAmount { get; set; }
        public int? LiabilityMainGroupId { get; set; }
        public int? Liabilitylabel { get; set; }
        public int ? LiabilityGroupId { get; set; }
        public string LiabilityGroupName { get; set; }
        public decimal? SubLiabilityAmount { get; set; }
        public decimal? LiabilityAmount { get; set; }
    }

}
