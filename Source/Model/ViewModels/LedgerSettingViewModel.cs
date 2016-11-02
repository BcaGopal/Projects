using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class LedgerSettingViewModel
    {
        public int LedgerSettingId { get; set; }
        public int DocTypeId { get; set; }
        public string DocTypeName { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public bool isVisibleLineCostCenter { get; set; }
        public bool isMandatoryLineCostCenter { get; set; }
        public bool isVisibleHeaderCostCenter { get; set; }
        public bool isMandatoryHeaderCostCenter { get; set; }
        public bool isVisibleChqNo { get; set; }
        public bool isMandatoryChqNo { get; set; }
        public bool isVisibleRefNo { get; set; }
        public bool isMandatoryRefNo { get; set; }
        public bool isVisibleProcess { get; set; }
        public bool isMandatoryProcess { get; set; }
        public bool isVisibleGodown { get; set; }
        public bool isMandatoryGodown { get; set; }
        public bool isVisibleProductUid { get; set; }        
        public string filterLedgerAccountGroupHeaders { get; set; }
        public string filterLedgerAccountGroupLines { get; set; }
        public string filterPersonProcessHeaders { get; set; }
        public string filterPersonProcessLines { get; set; }
        public string filterDocTypeCostCenter { get; set; }
        public string filterContraDocTypes { get; set; }
        public string filterContraSites { get; set; }
        public string filterContraDivisions { get; set; }
        public string SqlProcReferenceNo { get; set; }
        public string BaseValueText { get; set; }
        public string BaseRateText { get; set; }
        public int? ProcessId { get; set; }
        public string ProcessName { get; set; }
        public int? WizardMenuId { get; set; }       
        [MaxLength(100)]
        public string SqlProcDocumentPrint { get; set; }

        [MaxLength(100)]
        public string SqlProcDocumentPrint_AfterSubmit { get; set; }

        [MaxLength(100)]
        public string SqlProcDocumentPrint_AfterApprove { get; set; }

    }
}
