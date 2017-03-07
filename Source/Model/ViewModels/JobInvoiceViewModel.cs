﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class JobInvoiceHeaderViewModel
    {
        public int JobInvoiceHeaderId { get; set; }

        [Display(Name = "Invoice Type"), Required]
        public int DocTypeId { get; set; }
        public string DocTypeName { get; set; }

        [Display(Name = "Invoice Date"), Required]
        public DateTime DocDate { get; set; }

        [Display(Name = "Invoice No"), Required, MaxLength(20)]        
        public string DocNo { get; set; }

        [Display(Name = "JobWorker Doc. No."), MaxLength(20)]
        public string JobWorkerDocNo { get; set; }

        [Display(Name = "Division"), Required]        
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }

        [Display(Name = "Site"), Required]        
        public int SiteId { get; set; }
        public string SiteName { get; set; }

        public int Status { get; set; }
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }

        [Display(Name = "Remark")]
        public string Remark { get; set; }        
        public int? JobWorkerId { get; set; }
        public string JobWorkerName { get; set; }
        public JobInvoiceSettingsViewModel JobInvoiceSettings { get; set; }
        public DocumentTypeSettingsViewModel DocumentTypeSettings { get; set; }
        public string ModifiedBy { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Job Receive By")]
        public int JobReceiveById { get; set; }
        public string JobReceiveByName { get; set; }

        public int GodownId { get; set; }
        public string GodownName { get; set; }

        [Display(Name = "Job Receive")]
        public int? JobReceiveHeaderId { get; set; }
        public string JobReceiveDocNo { get; set; }
        public int? ReviewCount { get; set; }
        public string ReviewBy { get; set; }
        public bool? Reviewed { get; set; }
        public string LockReason { get; set; }
        public decimal? TotalQty { get; set; }
        public int? DecimalPlaces { get; set; }
    }


    public class JobInvoiceLineViewModel
    {
        public string ProductUidName { get; set; }
        public int? ProductUidId { get; set; }

        public int JobInvoiceLineId { get; set; }

        [Display(Name = "Job Invoice"), Required]
        public int JobInvoiceHeaderId { get; set; }
        public virtual JobInvoiceHeader JobInvoiceHeader { get; set; }                
        public int JobWorkerId { get; set; }
        public string JobWorkerName { get; set; }
        [Required]
        public Decimal UnitConversionMultiplier { get; set; }

        [Display(Name = "Job Receive"), Required]
        public int JobReceiveLineId { get; set; }
        public string JobReceiveDocNo { get; set; }
        public string JobOrderDocNo { get; set; }

        [Display(Name = "Job Order")]
        public int JobOrderLineId { get; set; }

        [Display(Name = "Deal Unit")]
        public string DealUnitId { get; set; }
        public string DealUnitName { get; set; }

        [Display(Name = "Deal Qty")]
        public decimal DealQty { get; set; }

        public int JobReceiveHeaderId { get; set; }

        public decimal IncentiveAmt { get; set; }
        public decimal IncentiveRate { get; set; }

        public decimal Weight { get; set; }

        [Display(Name = "Rate")]
        public decimal Rate { get; set; }

        [Display(Name = "Amount"), Required]
        public decimal Amount { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? Dimension1Id { get; set; }
        public string Dimension1Name { get; set; }
        public int ? Dimension2Id { get; set; }
        public string Dimension2Name { get; set; }
        public int? Dimension3Id { get; set; }
        public string Dimension3Name { get; set; }
        public int? Dimension4Id { get; set; }
        public string Dimension4Name { get; set; }
        public decimal ReceiptBalQty { get; set; }
        public decimal OrderBalanceQty { get; set; }
        public string Specification { get; set; }
        public decimal Qty { get; set; }
        [Display(Name = "Job Qty")]
        public decimal JobQty { get; set; }

        [Display(Name = "Pass Qty")]
        public decimal PassQty { get; set; }

        [Display(Name = "Loss Qty")]
        public decimal LossQty { get; set; }

        [Display(Name = "Loss Qty")]
        public decimal ReceiveQty { get; set; }

        [Display(Name = "Penalty Amount")]
        public Decimal PenaltyAmt { get; set; }
        public decimal PenaltyRate { get; set; }

        [Display(Name = "Lot No."), MaxLength(10)]
        public string LotNo { get; set; }

        public int? StockId { get; set; }

        public int? StockProcessId { get; set; }

        public string UnitId { get; set; }
        public string UnitName { get; set; }
        public byte UnitDecimalPlaces { get; set; }
        public byte DealUnitDecimalPlaces { get; set; }
        public JobInvoiceSettingsViewModel JobInvoiceSettings { get; set; }
        public DocumentTypeSettingsViewModel DocumentTypeSettings { get; set; }
        public List<JobInvoiceLineCharge> linecharges { get; set; }
        public List<JobInvoiceHeaderCharge> footercharges { get; set; }

        public int DocTypeId { get; set; }
        public string DocTypeName { get; set; }

        [Display(Name = "Division"), Required]
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }

        [Display(Name = "Site"), Required]
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string Remark { get; set; }
        public int ? CostCenterId { get; set; }
        public string InvoiceDocNo { get; set; }
        public DateTime OrderDocDate { get; set; }
        public string LockReason { get; set; }

    }


    public class JobInvoiceLineIndexViewModel
    {
        public string ProductUidName { get; set; }
        public string Dimension1Name { get; set; }
        public string Dimension2Name { get; set; }
        public string Dimension3Name { get; set; }
        public string Dimension4Name { get; set; }
        public string Specification { get; set; }
        public string LotNo { get; set; }

        public int JobInvoiceLineId { get; set; }
        public string ProductName { get; set; }
        public string JobOrderDocNo { get; set; }
        public decimal Qty { get; set; }
        public Decimal DealQty { get; set; }
        public string DealUnitId { get; set; }
        public string DealUnitName { get; set; }
        public string JobOrderHeaderDocNo { get; set; }
        public string JobReceiveHeaderDocNo { get; set; }
        [Display(Name = "Rate"), Required]
        public Decimal Rate { get; set; }

        [Display(Name = "Amount"), Required]
        public Decimal Amount { get; set; }

        [Display(Name = "Incentive Rate")]
        public Decimal? IncentiveRate { get; set; }

        [Display(Name = "Incentive Amount")]
        public Decimal? IncentiveAmt { get; set; }

        [Display(Name = "Remark")]
        public string Remark { get; set; }
        public string UnitId { get; set; }
        public string UnitName { get; set; }
        public byte UnitDecimalPlaces { get; set; }
        public byte DealUnitDecimalPlaces { get; set; }
        public int? ReceiptHeaderId { get; set; }
        public int? OrderHeaderId { get; set; }
        public int? ReceiptLineId { get; set; }
        public int? OrderLineId { get; set; }
        public int? ReceiptDocTypeId { get; set; }
        public int? OrderDocTypeId { get; set; }

    }

    public class JobInvoiceLineFilterViewModel
    {
        public int JobInvoiceHeaderId { get; set; }
        public int JobWorkerId { get; set; }
        [Display(Name = "Goods Receipt")]
        public string JobReceiveHeaderId { get; set; }
        public int DocTypeId { get; set; }
        public string JobOrderHeaderId { get; set; }
        [Display(Name = "Product")]
        public string ProductId { get; set; }
        [Display(Name = "Product Group")]
        public string ProductGroupId { get; set; }
        public DateTime ? AsOnDate { get; set; }
        public string JobWorkerIds { get; set; }
        public JobInvoiceSettingsViewModel JobInvoiceSettings { get; set; }
        public DocumentTypeSettingsViewModel DocumentTypeSettings { get; set; }
    }
    public class JobInvoiceMasterDetailModel
    {
        public List<JobInvoiceLineViewModel> JobInvoiceLineViewModel { get; set; }
        public JobInvoiceSettingsViewModel JobInvoiceSettings { get; set; }
    }
    public class JobInvoiceListViewModel
    {
        public int JobInvoiceHeaderId { get; set; }
        public int JobInvoiceLineId { get; set; }
        public string DocNo { get; set; }
        public string ReceiveDocNo { get; set; }
        public string Dimension1Name { get; set; }
        public string Dimension2Name { get; set; }
        public string Dimension3Name { get; set; }
        public string Dimension4Name { get; set; }
        public decimal BalanceQty { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
    public class JobInvoiceHeaderListViewModel
    {
        public int JobInvoiceHeaderId { get; set; }
        public int JobInvoiceLineId { get; set; }
        public string DocNo { get; set; }
        public string Dimension1Name { get; set; }
        public string Dimension2Name { get; set; }
        public string Dimension3Name { get; set; }
        public string Dimension4Name { get; set; }
        public string DocumentTypeName { get; set; }
        public string ProductName { get; set; }
        public decimal BalanceQty { get; set; }
        public string ProductUidName { get; set; }


    }


}