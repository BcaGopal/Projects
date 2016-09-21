using Model.Models;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels
{
    public class ReportMasterViewModel
    {
        public int ReportHeaderId { get; set; }
        public bool closeOnSelect { get; set; }
        public ReportHeader ReportHeader { get; set; }        
        public List<ReportLine> ReportLine{ get; set; }

    }
}
