using System.ComponentModel.DataAnnotations;

// New namespace imports:
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using Model.Models;
using System;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.ViewModels
{
    public class ProductConsumptionHeaderViewModel : EntityBase
    {
        public int BaseProductId { get; set; }
        public string BaseProductName { get; set; }

        [Index("IX_Design_Code", IsUnique = true)]
        [Display(Name = "Design"),Required]
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        [Display(Name = "Quality")]
        public int ProductQualityId { get; set; }
        public string ProductQualityName { get; set; }
    }


    public class ProductConsumptionLineViewModel : EntityBase
    {
        public int BomDetailId { get; set; }

        [ForeignKey("BaseProduct"), Display(Name = "Base Product")]
        public int BaseProductId { get; set; }
        public string BaseProductName { get; set; }

        public decimal BatchQty { get; set; }

        [ForeignKey("Product"), Display(Name = "Product")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        [ForeignKey("Dimension1"), Display(Name = "Shade")]
        public int? Dimension1Id { get; set; }
        public string Dimension1Name { get; set; }

        [Display(Name = "Product Group")]
        public string ProductGroupName { get; set; }

        public decimal ConsumptionPer { get; set; }

        public decimal Qty { get; set; }

        public string UnitName { get; set; }
        public string QualityName { get; set; }
        public Decimal Weight  { get; set; }
    }
}
