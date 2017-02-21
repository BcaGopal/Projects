using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Company.Models
{
    public class PaymentMode : EntityBase, IHistoryLog
    {
        [Key]
        public int PaymentModeId { get; set; }
        [Display (Name="Name")]
        [MaxLength(50, ErrorMessage = "PaymentMode Name cannot exceed 50 characters"), Required]
        [Index("IX_PaymentMode_PaymentModeName", IsUnique = true)]
        public string PaymentModeName { get; set; }
      
        

        [Display(Name = "Is Active ?")]
        public Boolean IsActive { get; set; }

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
