using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class SubTrialBalanceViewModel
    {
        public int LedgerAccountId { get; set; }
        public string LedgerAccountName { get; set; }
        public string LedgerAccountGroupName { get; set; }
        public decimal ? AmtDr { get; set; }
        public decimal ? AmtCr { get; set; }
    }

    public class SubTrialBalanceSummaryViewModel
    {
        public int LedgerAccountId { get; set; }
        public string LedgerAccountName { get; set; }
        public string LedgerAccountGroupName { get; set; }
        public decimal ? AmtDr { get; set; }
        public decimal ? AmtCr { get; set; }
        public decimal ? Opening { get; set; }
        public decimal ? Balance { get; set; }
        public string OpeningType { get; set; }
        public string BalanceType { get; set; }
    }

}
