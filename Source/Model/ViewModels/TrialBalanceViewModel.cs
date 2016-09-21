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

}
