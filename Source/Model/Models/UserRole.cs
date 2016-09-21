using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class UserRole : EntityBase
    {
        [Key]
        public int UserRoleId { get; set; }
        
        [MaxLength(128)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
                
        [MaxLength(128)]
        public string RoleId { get; set; }

        public DateTime? ExpiryDate { get; set; }

    }
}
