using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class SaleEnquiryLineExtended : EntityBase
    {
        [Key]
        [ForeignKey("SaleEnquiryLine")]
        public int SaleEnquiryLineId { get; set; }
        public JobOrderHeader SaleEnquiryLine { get; set; }
        public string ProductGroup { get; set; }
        public string Size { get; set; }
        public string ProductQuality { get; set; }
        public string Colour { get; set; }
    }
}
