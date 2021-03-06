﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class MaterialReceiveSettings : EntityBase, IHistoryLog
    {

        [Key]
        public int MaterialReceiveSettingsId { get; set; }

        [ForeignKey("DocType"), Display(Name = "Order Type")]
        public int DocTypeId { get; set; }
        public virtual DocumentType DocType { get; set; }      

        public int SiteId { get; set; }
        public virtual Site Site { get; set; }
        public int DivisionId { get; set; }
        public virtual Division Division { get; set; }
        public bool? isVisibleMachine { get; set; }
        public bool? isMandatoryMachine { get; set; }
        public bool? isVisibleCostCenter { get; set; }
        public bool? isMandatoryCostCenter { get; set; }
        public bool? isVisibleProductUID { get; set; }
        public bool? isVisibleDimension1 { get; set; }
        public bool? isVisibleDimension2 { get; set; }
        public bool? isVisibleDimension3 { get; set; }
        public bool? isVisibleDimension4 { get; set; }
        public bool? isVisibleRate { get; set; }
        public bool? isMandatoryRate { get; set; }
        public bool? isEditableRate { get; set; }
        public bool? isVisibleLotNo { get; set; }
        public bool? isMandatoryProcessLine { get; set; }
        public bool? isVisibleProcessLine { get; set; }
        public bool? isPostedInStockProcess { get; set; }

        [MaxLength(50)]
        public string PersonFieldHeading { get; set; }       


        [MaxLength(100)]
        public string SqlProcDocumentPrint { get; set; }       
        public string filterProductTypes { get; set; }
        public string filterProductGroups { get; set; }
        public string filterProducts { get; set; }
        public string filterContraDocTypes { get; set; }
        public string filterPersonRoles { get; set; }

        [ForeignKey("Process")]
        public int ProcessId { get; set; }
        public virtual Process Process { get; set; }


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
