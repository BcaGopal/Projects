﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Model.ViewModel
{
    public class CalculationProductViewModel
    {
        public int Id { get; set; }
        public string CalculationProductName { get; set; }

        public int CalculationId { get; set; }
        public string CalculationName { get; set; }

        public int LineTableId { get; set; }

        public int Sr { get; set; }

        public int ChargeId { get; set; }
        public string ChargeName { get; set; }
        public string ChargeCode { get; set; }


        public byte? AddDeduct { get; set; }
        public string AddDeductName { get; set; }

        public bool AffectCost { get; set; }
        public string AffectCostName { get; set; }
        public int? ChargeTypeId { get; set; }
        public string ChargeTypeName { get; set; }

        public int? CalculateOnId { get; set; }
        public string CalculateOnName { get; set; }
        public string CalculateOnCode { get; set; }

        public int? LedgerAccountDrId { get; set; }
        public string LedgerAccountDrName { get; set; }

        public int? LedgerAccountCrId { get; set; }
        public string LedgerAccountCrName { get; set; }

        public int? ContraLedgerAccountId { get; set; }
        public string ContraLedgerAccountName { get; set; }

        public int? CostCenterId { get; set; }
        public string CostCenterName { get; set; }

        public byte RateType { get; set; }

        public bool IsVisible { get; set; }


        public bool IncludedInBase { get; set; }
        public string IncludedInBaseName { get; set; }
        //[ForeignKey("ParentCharge")]
        public int? ParentChargeId { get; set; }
        //public virtual CalculationProduct ParentCharge { get; set; }

        public bool? IsVisibleLedgerAccountDr { get; set; }
        public int? filterLedgerAccountGroupsDrId { get; set; }
        public bool? IsVisibleLedgerAccountCr { get; set; }
        public int? filterLedgerAccountGroupsCrId { get; set; }

        public decimal Rate { get; set; }
        public decimal Amount { get; set; }        
        public decimal ? DealQty { get; set; }
        public Boolean IsActive { get; set; }
        public string ElementId { get; set; }
        public string IncludedCharges { get; set; }
        public string IncludedChargesCalculation { get; set; }
    }


    public class CalculationLineChargTrnViewModel
    {
        public int Id { get; set; }

        public int LineTableId { get; set; }
        public int Sr { get; set; }

        public int ChargeId { get; set; }
        public string ChargeName { get; set; }
        public string ChargeCode { get; set; }


        public byte? AddDeduct { get; set; }
        public string AddDeductName { get; set; }

        public bool AffectCost { get; set; }
        public string AffectCostName { get; set; }
        public int? ChargeTypeId { get; set; }
        public string ChargeTypeName { get; set; }

        public int? CalculateOnId { get; set; }
        public string CalculateOnName { get; set; }
        public string CalculateOnCode { get; set; }

        public int? LedgerAccountDrId { get; set; }
        public string LedgerAccountDrName { get; set; }

        public int? LedgerAccountCrId { get; set; }
        public string LedgerAccountCrName { get; set; }

        public int? CostCenterId { get; set; }
        public string CostCenterName { get; set; }

        public byte RateType { get; set; }

        public bool IsVisible { get; set; }


        public bool IncludedInBase { get; set; }
        public string IncludedInBaseName { get; set; }
        //[ForeignKey("ParentCharge")]
        public int? ParentChargeId { get; set; }
        //public virtual CalculationProduct ParentCharge { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }
        public Boolean IsActive { get; set; }
        public string ElementId { get; set; }
    }
}
