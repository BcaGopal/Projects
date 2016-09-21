using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class JobInvoiceSettings : EntityBase, IHistoryLog
    {

        [Key]
        public int JobInvoiceSettingsId { get; set; }

        [ForeignKey("DocType"), Display(Name = "Order Type")]
        public int DocTypeId { get; set; }
        public virtual DocumentType DocType { get; set; }      

        public int SiteId { get; set; }
        public virtual Site Site { get; set; }
        public int DivisionId { get; set; }
        public virtual Division Division { get; set; }
        public bool? isVisibleMachine { get; set; }
        public bool? isMandatoryMachine { get; set; }
        public bool? isVisibleProductUID { get; set; }
        public bool? isVisibleDimension1 { get; set; }
        public bool? isVisibleDimension2 { get; set; }        
        public bool? isVisibleLotNo { get; set; }
        public bool? isVisibleLoss { get; set; }
        public bool? isVisibleHeaderJobWorker { get; set; }
        public bool? isPostedInStock { get; set; }
        public bool? isPostedInStockProcess { get; set; }
        public bool? isPostedInStockVirtual { get; set; }
        public bool? isAutoCreateJobReceive { get; set; }

        /// <summary>
        /// DocId will be passed as a parameter in specified procedure.
        /// Procedure should have only one parameter of type int.
        /// </summary>
        [MaxLength(100)]
        public string SqlProcDocumentPrint { get; set; }

        /// <summary>
        /// DocId will be passed as a parameter in specified procedure.
        /// Procedure should have only one parameter of type int.
        /// </summary>
        [MaxLength(100)]
        public string SqlProcDocumentPrint_AfterSubmit { get; set; }

        /// <summary>
        /// DocId will be passed as a parameter in specified procedure.
        /// Procedure should have only one parameter of type int.
        /// </summary>
        [MaxLength(100)]
        public string SqlProcDocumentPrint_AfterApprove { get; set; }

        [MaxLength(100)]
        public string DocumentPrint { get; set; }
        
        [MaxLength (100)]
        public string SqlProcConsumption { get; set; }

        [MaxLength(100)]
        public string SqlProcGatePass { get; set; }
        public string SqlProcGenProductUID { get; set; }
        public string filterLedgerAccountGroups { get; set; }
        public string filterLedgerAccounts { get; set; }
       
        public string filterProductTypes { get; set; }
        public string filterProductGroups { get; set; }
        public string filterProducts { get; set; }
        public string filterContraDocTypes { get; set; }
        public string filterContraSites { get; set; }
        public string filterContraDivisions { get; set; }

        [ForeignKey("Process")]
        public int ProcessId { get; set; }
        public virtual Process Process { get; set; }

        [ForeignKey("ImportMenu")]
        [Display(Name = "ImportMenu")]
        public int? ImportMenuId { get; set; }
        public virtual Menu ImportMenu { get; set; }

        [ForeignKey("WizardMenu")]
        [Display(Name = "WizardMenu")]
        public int? WizardMenuId { get; set; }
        public virtual Menu WizardMenu { get; set; }


        [ForeignKey("Calculation")]
        public int ? CalculationId { get; set; }
        public virtual Calculation Calculation { get; set; }

        [ForeignKey("JobReceiveDocType"), Display(Name = "Order Type")]
        public int ? JobReceiveDocTypeId { get; set; }
        public virtual DocumentType JobReceiveDocType { get; set; }
        public int? AmountRoundOff { get; set; }

        [ForeignKey("ReturnDocType")]
        public int ? JobReturnDocTypeId { get; set; }
        public virtual DocumentType ReturnDocType { get; set; }

        [MaxLength(20)]
        public string BarcodeStatusUpdate { get; set; }
        public int? NoOfPrintCopies { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }


    }
}
