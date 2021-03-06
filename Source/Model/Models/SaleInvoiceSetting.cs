﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class SaleInvoiceSetting : EntityBase, IHistoryLog
    {

        [Key]
        public int SaleInvoiceSettingId { get; set; }

        [ForeignKey("DocType"), Display(Name = "Order Type")]
        public int DocTypeId { get; set; }
        public virtual DocumentType DocType { get; set; }
        public int SiteId { get; set; }
        public virtual Site Site { get; set; }
        public int DivisionId { get; set; }
        public virtual Division Division { get; set; }
        public bool? isVisibleDimension1 { get; set; }
        public bool? isVisibleDimension2 { get; set; }
        public bool? isVisibleDimension3 { get; set; }
        public bool? isVisibleDimension4 { get; set; }
        public bool? isVisiblePromoCode { get; set; }
        public bool? isVisibleLotNo { get; set; }
        public bool? isVisibleFinancier { get; set; }
        public bool? isVisibleSalesExecutive { get; set; }
        public bool? isVisibleFreeQty { get; set; }
        public bool? isVisibleRewardPoints { get; set; }
        public bool? isVisibleTermsAndConditions { get; set; }

        public bool CalculateDiscountOnRate { get; set; }
        public string filterLedgerAccountGroups { get; set; }
        public string filterLedgerAccounts { get; set; }
        public string filterProductTypes { get; set; }
        public string filterProductGroups { get; set; }
        public string filterProducts { get; set; }
        public string filterContraDocTypes { get; set; }
        public string filterContraSites { get; set; }
        public string filterContraDivisions { get; set; }
        public string filterPersonRoles { get; set; }

        [ForeignKey("DocTypePackingHeader"), Display(Name = "Production Order Type")]
        public int? DocTypePackingHeaderId { get; set; }
        public virtual DocumentType DocTypePackingHeader { get; set; }

        [ForeignKey("SaleDispatchDocType"), Display(Name = "Goods ReceiptId")]
        public int? SaleDispatchDocTypeId { get; set; }
        public virtual DocumentType SaleDispatchDocType { get; set; }

        [ForeignKey("SaleInvoiceReturnDocType")]
        public int? SaleInvoiceReturnDocTypeId { get; set; }
        public virtual DocumentType SaleInvoiceReturnDocType { get; set; }


        public bool? DoNotUpdateProductUidStatus { get; set; }

        [MaxLength(100)]
        public string SqlProcProductUidHelpList { get; set; }

        [ForeignKey("Calculation")]
        public int CalculationId { get; set; }
        public virtual Calculation Calculation { get; set; }

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
        public string SqlProcGatePass { get; set; }


        [ForeignKey("UnitConversionFor")]
        [Display(Name = "Unit Conversion For Type")]
        public byte? UnitConversionForId { get; set; }
        public virtual UnitConversionFor UnitConversionFor { get; set; }

        [ForeignKey("DocTypeDispatchReturn"), Display(Name = "Dispatch Return Type")]
        public int? DocTypeDispatchReturnId { get; set; }
        public virtual DocumentType DocTypeDispatchReturn { get; set; }

        [ForeignKey("ImportMenu")]
        [Display(Name = "ImportMenu")]
        public int? ImportMenuId { get; set; }
        public virtual Menu ImportMenu { get; set; }

        [ForeignKey("ExportMenu")]
        [Display(Name = "ExportMenu")]
        public int? ExportMenuId { get; set; }
        public virtual Menu ExportMenu { get; set; }


        public bool? isVisibleAgent { get; set; }
        public bool? isVisibleCurrency { get; set; }
        public bool? isVisibleDeliveryTerms { get; set; }
        public bool? isVisibleDealUnit { get; set; }
        public bool? isVisibleShipMethod { get; set; }
        public bool? isVisibleSpecification { get; set; }
        public bool? isVisibleSalesTaxGroupPerson { get; set; }
        public bool? isVisibleSalesTaxGroupProduct { get; set; }
        public bool? isVisibleProductUid { get; set; }
        public bool? isVisibleProductCode { get; set; }
        public bool? isVisibleBaleNo { get; set; }
        public bool? isVisibleDiscountPer { get; set; }
        public bool? isVisibleForSaleOrder { get; set; }
        public bool? isVisibleWeight { get; set; }
        public bool? isVisibleCreditDays { get; set; }
        public bool? isVisibleShipToPartyAddress { get; set; }


        [ForeignKey("Currency")]
        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }


        [ForeignKey("Process")]
        public int? ProcessId { get; set; }
        public virtual Process Process { get; set; }

        [ForeignKey("DeliveryTerms")]
        public int DeliveryTermsId { get; set; }
        public virtual DeliveryTerms DeliveryTerms { get; set; }
        
        [ForeignKey("ShipMethod")]
        public int ShipMethodId { get; set; }
        public virtual ShipMethod ShipMethod { get; set; }

        [ForeignKey("SalesTaxGroupPerson")]
        [Display(Name = "SalesTaxGroupPerson")]
        public int? SalesTaxGroupPersonId { get; set; }
        public virtual ChargeGroupPerson SalesTaxGroupPerson { get; set; }


        [ForeignKey("Godown")]
        public int GodownId { get; set; }
        public virtual Godown Godown { get; set; }

        [ForeignKey("DocumentPrintReportHeader")]
        public int? DocumentPrintReportHeaderId { get; set; }
        public virtual ReportHeader DocumentPrintReportHeader { get; set; }




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
