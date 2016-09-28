using Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DatabaseViews
{

    [Table("ViewProductBuyer")]
    public class ViewProductBuyer
    {
        [Key]
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductGroup { get; set; }
        public string Size { get; set; }
        public string Colour { get; set; }
        public string ProductQuality { get; set; }
        public int BuyerId { get; set; }
        
    }
}
