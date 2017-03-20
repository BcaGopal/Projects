using Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Reports.Models
{
    public class ReportUIDValues : EntityBase
    {
        [Key]
        public int Id { get; set; }
        public Guid UID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
