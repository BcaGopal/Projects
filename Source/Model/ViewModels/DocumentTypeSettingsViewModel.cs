﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class DocumentTypeSettingsViewModel
    {
        [Key]
        public int DocumentTypeSettingsId { get; set; }


        public int DocumentTypeId { get; set; }
        public virtual DocumentType DocumentType { get; set; }

        [MaxLength(50)]
        public string ProductCaption { get; set; }

        [MaxLength(50)]
        public string ProductGroupCaption { get; set; }

        [MaxLength(50)]
        public string ProductCategoryCaption { get; set; }

        [MaxLength(50)]
        public string Dimension1Caption { get; set; }

        [MaxLength(50)]
        public string Dimension2Caption { get; set; }

        [MaxLength(50)]
        public string Dimension3Caption { get; set; }

        [MaxLength(50)]
        public string Dimension4Caption { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        [MaxLength(50)]
        public string OMSId { get; set; }
    }
}