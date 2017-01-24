using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using System.ComponentModel.DataAnnotations;
using Model.ViewModels;



namespace Model.ViewModel
{
    public class WeavingReceiveQACombinedViewModel
    {
        public int JobReceiveHeaderId { get; set; }
        public int JobReceiveLineId { get; set; }

        public int JobReceiveQAHeaderId { get; set; }

        public int JobReceiveQALineId { get; set; }
        public int JobReceiveQAAttributeId { get; set; }

        public int DocTypeId { get; set; }
        public DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public int DivisionId { get; set; }
        public int SiteId { get; set; }
        public int GodownId { get; set; }
        public int ProcessId { get; set; }
        public int JobWorkerId { get; set; }
        public int JobReceiveById { get; set; }
        public int? ProductUidId { get; set; }
        public string ProductUidName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int JobOrderLineId { get; set; }
        public string JobOrderHeaderDocNo { get; set; }
        public decimal Qty { get; set; }
        public decimal BalanceQty { get; set; }
        public byte UnitDecimalPlaces { get; set; }
        public byte DealUnitDecimalPlaces { get; set; }
        public string UnitId { get; set; }
        public string DealUnitId { get; set; }
        public Decimal UnitConversionMultiplier { get; set; }
        public decimal DealQty { get; set; }
        public decimal Weight { get; set; }
        public decimal Rate { get; set; }
        public decimal? PenaltyRate { get; set; }
        public decimal? PenaltyAmt { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<QAGroupLineLineViewModel> QAGroupLine { get; set; }
        public JobReceiveSettingsViewModel JobReceiveSettings { get; set; }
        public JobReceiveQASettingsViewModel JobReceiveQASettings { get; set; }
        public DocumentTypeSettingsViewModel DocumentTypeSettings { get; set; }
        public Decimal? Length { get; set; }
        public Decimal? Width { get; set; }
        public Decimal? Height { get; set; }
        public int? DimensionUnitDecimalPlaces { get; set; }

        public Decimal? XRate { get; set; }
        public Decimal? XLength { get; set; }
        public Decimal? XWidth { get; set; }
        public string OMSId { get; set; }

    }
}
