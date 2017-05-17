using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class SaleInvoiceHeaderAttributes : EntityBase, IHistoryLog
    {      
        [Key]
        public int SaleInvoiceHeaderAttributeId { get; set; }

        [ForeignKey("SaleInvoiceHeader")]
        public int SaleInvoiceHeaderId { get; set; }
        public virtual SaleInvoiceHeader SaleInvoiceHeader { get; set; }

        [ForeignKey("DocumentTypeAttribute")]
        public int DocumentTypeAttributeId { get; set; }
        public virtual DocumentTypeAttribute DocumentTypeAttribute { get; set; }

        public string SaleInvoiceHeaderAttributeValue { get; set; }

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
