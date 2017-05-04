﻿using System;
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



        public bool? isVisibleProductDescription { get; set; }
        public bool? isVisibleProductSpecification { get; set; }
        public bool? isVisibleProductCategory { get; set; }
        public bool? isVisibleSalesTaxGroup { get; set; }
        public bool? isVisibleSaleRate { get; set; }
        public bool? isVisibleStandardCost { get; set; }
        public bool? isVisibleTags { get; set; }
        public bool? isVisibleMinimumOrderQty { get; set; }
        public bool? isVisibleReOrderLevel { get; set; }
        public bool? isVisibleGodownId { get; set; }
        public bool? isVisibleBinLocationId { get; set; }
        public bool? isVisibleProfitMargin { get; set; }
        public bool? isVisibleCarryingCost { get; set; }
        public bool? isVisibleLotManagement { get; set; }
        public bool? isVisibleConsumptionDetail { get; set; }
        public bool? isVisibleProductProcessDetail { get; set; }


        public string ProductNameCaption { get; set; }
        public string ProductCodeCaption { get; set; }
        public string ProductDescriptionCaption { get; set; }
        public string ProductSpecificationCaption { get; set; }
        public string ProductGroupCaption { get; set; }
        public string ProductCategoryCaption { get; set; }


        [ForeignKey("ImportMenu")]
        [Display(Name = "ImportMenu")]
        public int? ImportMenuId { get; set; }
        public virtual Menu ImportMenu { get; set; }



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
