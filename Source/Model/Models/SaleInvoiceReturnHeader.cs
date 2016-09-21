﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class SaleInvoiceReturnHeader : EntityBase, IHistoryLog
    {


        [Key]        
        public int SaleInvoiceReturnHeaderId { get; set; }
                        
        [Display(Name = "Return Type"),Required]
        [ForeignKey("DocType")]
        [Index("IX_SaleInvoiceReturnHeader_DocID", IsUnique = true, Order = 1)]
        public int DocTypeId { get; set; }
        public virtual DocumentType DocType { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Return Date"),Required ]
        public DateTime  DocDate { get; set; }
        
        [Display(Name = "Return No"),Required,MaxLength(20,ErrorMessage = "{0} can not exceed {1} characters")]
        [Index("IX_SaleInvoiceReturnHeader_DocID", IsUnique = true, Order = 2)]
        public string DocNo { get; set; }

        [ForeignKey("Reason"), Display(Name = "Reason")]
        public int ReasonId { get; set; }
        public virtual Reason Reason { get; set; }

        [Display(Name = "Division"),Required ]
        [ForeignKey("Division")]
        [Index("IX_SaleInvoiceReturnHeader_DocID", IsUnique = true, Order = 3)]
        public int DivisionId { get; set; }
        public virtual  Division  Division { get; set; }

        [Display(Name = "Site"), Required]
        [ForeignKey("Site")]
        [Index("IX_SaleInvoiceReturnHeader_DocID", IsUnique = true, Order = 4)]
        public int SiteId { get; set; }
        public virtual Site Site { get; set; }

        [Display(Name = "Ledger Header")]
        [ForeignKey("LedgerHeader")]
        public int? LedgerHeaderId { get; set; }
        public virtual LedgerHeader LedgerHeader { get; set; }

        public Decimal? ExchangeRate { get; set; }


        [ForeignKey("Buyer")]
        [Display(Name = "Buyer Name")]
        public int BuyerId { get; set; }
        public virtual Buyer Buyer { get; set; }

        public bool CalculateDiscountOnRate { get; set; }        

        [ForeignKey("SalesTaxGroup")]
        public int? SalesTaxGroupId { get; set; }
        public virtual ChargeGroupPerson SalesTaxGroup { get; set; }

        [ForeignKey("SalesTaxGroupParty")]
        public int? SalesTaxGroupPartyId { get; set; }
        public virtual SalesTaxGroupParty SalesTaxGroupParty { get; set; }

        [Display(Name = "Currency"), Required]
        [ForeignKey("Currency")]
        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }


        [ForeignKey("SaleDispatchReturnHeader")]
        [Display(Name = "Sale Goods Return")]
        public int? SaleDispatchReturnHeaderId { get; set; }
        public virtual SaleDispatchReturnHeader SaleDispatchReturnHeader { get; set; }

        [Display(Name = "Remark"),Required]
        public string Remark { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; }

        [Display(Name = "Lock Reason")]
        public string LockReason { get; set; }

        [Display(Name = "Lock Reason Delete")]
        public string LockReasonDelete { get; set; }
        public int? ReviewCount { get; set; }
        public string ReviewBy { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [MaxLength(50)]
        public string OMSId { get; set; }

    }
}
