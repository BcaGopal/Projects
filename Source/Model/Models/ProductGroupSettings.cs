using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class ProductGroupSettings : EntityBase, IHistoryLog
    {
        public ProductGroupSettings()
        {
        }

        [Key]
        public int ProductGroupSettingsId { get; set; }

        [ForeignKey("ProductGroup")]
        [Display(Name = "Product Category")]
        public int ProductGroupId { get; set; }
        public virtual ProductGroup ProductGroup { get; set; }

        public int SiteId { get; set; }
        public virtual Site Site { get; set; }
        public int DivisionId { get; set; }
        public virtual Division Division { get; set; }

        [Display(Name = "QA Group")]
        public int? QAGroupId { get; set; }
        public virtual QAGroup QAGroup { get; set; }

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
