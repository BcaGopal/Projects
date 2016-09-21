using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class JobReceiveSettingsViewModel
    {
        public int JobReceiveSettingsId { get; set; }        
        public int DocTypeId { get; set; }
        public string DocTypeName { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public bool isVisibleMachine { get; set; }
        public bool isMandatoryMachine { get; set; }
        public bool isVisibleCostCenter { get; set; }
        public bool isMandatoryCostCenter { get; set; }
        public bool isVisibleProductUID { get; set; }
        public bool isVisibleDimension1 { get; set; }
        public bool isVisibleDimension2 { get; set; }
        public bool isVisibleLoss { get; set; }
        public bool isVisibleUncountableQty { get; set; }
        public bool IsVisibleForOrderMultiple { get; set; }
        public bool IsVisibleWeight { get; set; }
        public bool IsMandatoryWeight { get; set; }
        public string SqlProcConsumption { get; set; }
        public string SqlProcDocumentPrint { get; set; }
        public string DocumentPrint { get; set; }
        public byte UnitConversionForId { get; set; }
        public string UnitConversionForName { get; set; }
        public bool isVisibleRate { get; set; }
        public bool isMandatoryRate { get; set; }
        public bool isEditableRate { get; set; }
        public bool isVisibleLotNo { get; set; }
        public bool isMandatoryProcessLine { get; set; }
        public bool isPostedInStock { get; set; }
        public bool isPostedInStockProcess { get; set; }
        public bool isPostedInStockVirtual { get; set; }

        public string filterLedgerAccountGroups { get; set; }
        public string filterLedgerAccounts { get; set; }        
        [Range(1,int.MaxValue,ErrorMessage="Process field is required")]
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }        
        public int ? CalculationId { get; set; }
        public string CalculationName { get; set; }
        public string SqlProcGenProductUID { get; set; }
        public string filterProductTypes { get; set; }
        public string filterProductGroups { get; set; }
        public string filterProducts { get; set; }
        public string filterContraDocTypes { get; set; }
        public string filterContraSites { get; set; }
        public string filterContraDivisions { get; set; }
        public string SqlProcGatePass { get; set; }

        [MaxLength(20)]
        public string BarcodeStatusUpdate { get; set; }

        [MaxLength(100)]
        public string SqlProcDocumentPrint_AfterSubmit { get; set; }

        [MaxLength(100)]
        public string SqlProcDocumentPrint_AfterApprove { get; set; }

        [MaxLength(20)]
        public string StockQty { get; set; }
    }
}
