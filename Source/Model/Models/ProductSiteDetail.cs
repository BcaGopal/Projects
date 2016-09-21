using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class ProductSiteDetail : EntityBase, IHistoryLog
    {
        [Key]
        public int ProductSiteDetailId { get; set; }


        [ForeignKey("Product")]
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }


        [ForeignKey("Division")]
        [Display(Name = "Division")]
        public int DivisionId { get; set; }
        public virtual Division Division { get; set; }

        [ForeignKey("Site")]
        [Display(Name = "Site")]
        public int SiteId { get; set; }
        public virtual Site Site { get; set; }


        [Display (Name ="Minimum Order Qty")]
        public Decimal? MinimumOrderQty { get; set; }

        [Display(Name = "ReOrder Level")]
        public Decimal? ReOrderLevel { get; set; }

        [ForeignKey("Godown")]
        [Display(Name = "Godown")]
        public int? GodownId { get; set; }        
        public virtual Godown Godown { get; set; }

        [Display(Name = "Bin Location")]
        [MaxLength(20,ErrorMessage ="{0} can not exceed {1} characters")]
        public string BinLocation { get; set; }




        [Display(Name = "Is Active ?")]
        public Boolean IsActive { get; set; }
        [Display(Name = "LOT Management")]
        public Boolean LotManagement { get; set; }

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
