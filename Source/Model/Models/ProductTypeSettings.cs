using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class ProductTypeSettings : EntityBase, IHistoryLog
    {

        [Key]
        public int ProductTypeSettingsId { get; set; }

        [ForeignKey("ProductType"), Display(Name = "Order Type")]
        public int ProductTypeId { get; set; }
        public virtual DocumentType ProductType { get; set; }



        public string UnitId { get; set; }


        public bool? isShowMappedDimension1 { get; set; }
        public bool? isShowUnMappedDimension1 { get; set; }
        public bool? isApplicableDimension1 { get; set; }
        public string Dimension1Caption { get; set; }


        public bool? isShowMappedDimension2 { get; set; }
        public bool? isShowUnMappedDimension2 { get; set; }
        public bool? isApplicableDimension2 { get; set; }
        public string Dimension2Caption { get; set; }


        public bool? isShowMappedDimension3 { get; set; }
        public bool? isShowUnMappedDimension3 { get; set; }
        public bool? isApplicableDimension3 { get; set; }
        public string Dimension3Caption { get; set; }


        public bool? isShowMappedDimension4 { get; set; }
        public bool? isShowUnMappedDimension4 { get; set; }
        public bool? isApplicableDimension4 { get; set; }
        public string Dimension4Caption { get; set; }   


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
