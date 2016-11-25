using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class ProductCategorySettings : EntityBase, IHistoryLog
    {
        public ProductCategorySettings()
        {
        }

        [Key]
        public int ProductCategorySettingsId { get; set; }

        [ForeignKey("ProductCategory")]
        [Display(Name = "Product Category")]
        public int ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }

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
