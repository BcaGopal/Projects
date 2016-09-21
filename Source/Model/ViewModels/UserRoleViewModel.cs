using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class UserRoleViewModel : EntityBase
    {        
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FromUserId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string RolesList { get; set; }
        public string RoleIdList { get; set; }
        public DateTime? ExpiryDate { get; set; }

    }
}
