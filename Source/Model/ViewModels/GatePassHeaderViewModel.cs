using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels
{
    public class  GatePassHeaderViewModel
    {
            public int GatePassHeaderId { get; set; }
            public string DocNo { get; set; }
            public DateTime DocDate { get; set; }
            public int Status { get; set; }
            public string Name { get; set; }
            public string Product { get; set;}

            public decimal Qty { get; set; }

            public string UnitId { get; set; }
         public string Remark { get; set; }
    }

    
}
