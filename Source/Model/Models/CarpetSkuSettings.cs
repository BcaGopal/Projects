using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class CarpetSkuSettings : EntityBase, IHistoryLog
    {
        public CarpetSkuSettings()
        {
        }

        [Key]
        public int CarpetSkuSettingsId { get; set; }
        public bool? isVisibleColourWays { get; set; }
        public bool? isVisibleStyle { get; set; }
        public bool? isVisibleDesigner { get; set; }
        public bool? isVisibleDesignPattern { get; set; }
        public bool? isVisibleContent { get; set; }
        public bool? isVisibleOrigin { get; set; }
        public bool? isVisibleInvoiceGroup { get; set; }
        public bool? isVisibleDrawbackTarrif { get; set; }
        public bool? isVisibleStandardCost { get; set; }
        public bool? isVisibleFinishedWeight { get; set; }
        public bool? isVisibleSupplierDetail { get; set; }
        public bool? isVisibleLeadTime { get; set; }
        public bool? isVisibleMinQty { get; set; }
        public bool? isVisibleMaxQty { get; set; }
        public bool? isVisibleSample { get; set; }
        public bool? isVisibleCounterNo { get; set; }
        public bool? isVisibleTags { get; set; }
        public bool? isVisibleDivision { get; set; }




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
