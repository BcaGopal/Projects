using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class ProductTypeSettingsViewModel
    {
        public int ProductTypeSettingsId { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }


        public string UnitId { get; set; }


        public bool isShowMappedDimension1 { get; set; }
        public bool isShowUnMappedDimension1 { get; set; }
        public bool isApplicableDimension1 { get; set; }
        public string Dimension1Caption { get; set; }


        public bool isShowMappedDimension2 { get; set; }
        public bool isShowUnMappedDimension2 { get; set; }
        public bool isApplicableDimension2 { get; set; }
        public string Dimension2Caption { get; set; }


        public bool isShowMappedDimension3 { get; set; }
        public bool isShowUnMappedDimension3 { get; set; }
        public bool isApplicableDimension3 { get; set; }
        public string Dimension3Caption { get; set; }


        public bool isShowMappedDimension4 { get; set; }
        public bool isShowUnMappedDimension4 { get; set; }
        public bool isApplicableDimension4 { get; set; }
        public string Dimension4Caption { get; set; }

    }
}
