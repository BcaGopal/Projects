﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels
{
    public class SaleDeliveryOrderSettingsViewModel
    {
        public int SaleDeliveryOrderSettingsId { get; set; }
        public int DocTypeId { get; set; }
        public string DocTypeName { get; set; }
        public int? ImportMenuId { get; set; }
        public string ImportMenuName { get; set; }
        public bool isVisibleProductUID { get; set; }
        public bool isMandatoryProductUID { get; set; }
        public bool isVisibleDimension1 { get; set; }
        public bool isVisibleDimension2 { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string OMSId { get; set; }
    }
}
