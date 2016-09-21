using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using Model.Models;
using System;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.ViewModels
{
    public class PersonViewModel 
    {
        public PersonViewModel()
        {
            PersonContacts = new List<PersonContact>();
        }

       
        public int PersonID { get; set; }

       
        public string PersonType { get; set; }
        public string Name { get; set; }

        [MaxLength(5),Required]
        public string Code { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<PersonContact> PersonContacts { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        
    }
}
