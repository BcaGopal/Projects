﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class JobReceiveQASettings : EntityBase, IHistoryLog
    {

        [Key]
        public int JobReceiveQASettingsId { get; set; }

        [ForeignKey("DocType"), Display(Name = "Order Type")]
        public int DocTypeId { get; set; }
        public virtual DocumentType DocType { get; set; }

        [ForeignKey("ImportMenu")]
        [Display(Name = "ImportMenu")]
        public int? ImportMenuId { get; set; }
        public virtual Menu ImportMenu { get; set; }

        [ForeignKey("WizardMenu")]
        [Display(Name = "WizardMenu")]
        public int? WizardMenuId { get; set; }
        public virtual Menu WizardMenu { get; set; }

        public bool? isVisibleProductUID { get; set; }
        public bool? isMandatoryProductUID { get; set; }
        public bool? isVisibleDimension1 { get; set; }
        public bool? isVisibleDimension2 { get; set; }


        public int? DocTypeProductionOrderId { get; set; }

        public string filterContraDocTypes { get; set; }
        public string filterContraSites { get; set; }
        public string filterContraDivisions { get; set; }

        [ForeignKey("Process"), Display(Name = "Process")]
        public int ProcessId { get; set; }
        public virtual Process Process { get; set; }
        public int SiteId { get; set; }
        public virtual Site Site { get; set; }
        public int DivisionId { get; set; }
        public virtual Division Division { get; set; }

        public bool? isPostedInStock { get; set; }

        /// <summary>
        /// DocId will be passed as a parameter in specified procedure.
        /// Procedure should have only one parameter of type int.
        /// </summary>
        [MaxLength(100)]
        public string SqlProcDocumentPrint { get; set; }

        /// <summary>
        /// DocId will be passed as a parameter in specified procedure.
        /// Procedure should have only one parameter of type int.
        /// </summary>
        [MaxLength(100)]
        public string SqlProcDocumentPrint_AfterSubmit { get; set; }

        /// <summary>
        /// DocId will be passed as a parameter in specified procedure.
        /// Procedure should have only one parameter of type int.
        /// </summary>
        [MaxLength(100)]
        public string SqlProcDocumentPrint_AfterApprove { get; set; }

        [MaxLength(100)]
        public string DocumentPrint { get; set; }
        
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
