namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChargeGroupPersonCalculation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Web.SaleInvoiceReturnHeaders", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.SaleInvoiceReturnHeaders", "SalesTaxGroupId", "Web.ChargeGroupPersons");
            DropForeignKey("Web.SaleInvoiceReturnHeaders", "SalesTaxGroupPartyId", "Web.SalesTaxGroupParties");
            DropIndex("Web.SaleInvoiceReturnHeaders", new[] { "SalesTaxGroupId" });
            DropIndex("Web.SaleInvoiceReturnHeaders", new[] { "SalesTaxGroupPartyId" });
            DropIndex("Web.SaleInvoiceReturnHeaders", new[] { "CurrencyId" });
            CreateTable(
                "Web.ChargeGroupPersonCalculations",
                c => new
                    {
                        ChargeGroupPersonCalculationId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DivisionId = c.Int(),
                        SiteId = c.Int(),
                        ChargeGroupPersonId = c.Int(nullable: false),
                        CalculationId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ChargeGroupPersonCalculationId)
                .ForeignKey("Web.Calculations", t => t.CalculationId)
                .ForeignKey("Web.ChargeGroupPersons", t => t.ChargeGroupPersonId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DivisionId, t.SiteId, t.ChargeGroupPersonId }, unique: true, name: "ChargeGroupPersonCalculation_DocID")
                .Index(t => t.CalculationId);
            
            CreateTable(
                "Web.ViewPackingBalance",
                c => new
                    {
                        PackingLineId = c.Int(nullable: false, identity: true),
                        PackingHeaderId = c.Int(nullable: false),
                        PackingNo = c.String(),
                        PackingDate = c.DateTime(nullable: false),
                        PersonId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Dimension3Id = c.Int(),
                        Dimension4Id = c.Int(),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PackingLineId);
            
            AddColumn("Web.States", "StateCode", c => c.String(nullable: false, maxLength: 20));
            AddColumn("Web.ProductGroups", "RateDecimalPlaces", c => c.Byte(nullable: false));
            AddColumn("Web.ChargeGroupProducts", "PrintingDescription", c => c.String(maxLength: 50));
            AddColumn("Web.PackingLines", "StockInId", c => c.Int());
            AddColumn("Web.ChargeGroupPersons", "PrintingDescription", c => c.String(maxLength: 50));
            AddColumn("Web.Charges", "PrintingDescription", c => c.String(maxLength: 50));
            AddColumn("Web.ChargeTypes", "Category", c => c.String(maxLength: 20));
            AddColumn("Web.CalculationLineLedgerAccounts", "IsVisibleLedgerAccountDr", c => c.Boolean());
            AddColumn("Web.CalculationLineLedgerAccounts", "filterLedgerAccountGroupsDrId", c => c.Int());
            AddColumn("Web.CalculationLineLedgerAccounts", "IsVisibleLedgerAccountCr", c => c.Boolean());
            AddColumn("Web.CalculationLineLedgerAccounts", "filterLedgerAccountGroupsCrId", c => c.Int());
            AddColumn("Web.SaleInvoiceLines", "Dimension3Id", c => c.Int());
            AddColumn("Web.SaleInvoiceLines", "Dimension4Id", c => c.Int());
            AddColumn("Web.DocumentTypeSettings", "DocIdCaption", c => c.String(maxLength: 50));
            AddColumn("Web.JobConsumptionSettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.JobInvoiceLineCharges", "IsVisibleLedgerAccountDr", c => c.Boolean());
            AddColumn("Web.JobInvoiceLineCharges", "filterLedgerAccountGroupsDrId", c => c.Int());
            AddColumn("Web.JobInvoiceLineCharges", "IsVisibleLedgerAccountCr", c => c.Boolean());
            AddColumn("Web.JobInvoiceLineCharges", "filterLedgerAccountGroupsCrId", c => c.Int());
            AddColumn("Web.JobInvoiceRateAmendmentLineCharges", "IsVisibleLedgerAccountDr", c => c.Boolean());
            AddColumn("Web.JobInvoiceRateAmendmentLineCharges", "filterLedgerAccountGroupsDrId", c => c.Int());
            AddColumn("Web.JobInvoiceRateAmendmentLineCharges", "IsVisibleLedgerAccountCr", c => c.Boolean());
            AddColumn("Web.JobInvoiceRateAmendmentLineCharges", "filterLedgerAccountGroupsCrId", c => c.Int());
            AddColumn("Web.JobInvoiceReturnHeaders", "Nature", c => c.String(maxLength: 20));
            AddColumn("Web.JobInvoiceReturnLineCharges", "IsVisibleLedgerAccountDr", c => c.Boolean());
            AddColumn("Web.JobInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId", c => c.Int());
            AddColumn("Web.JobInvoiceReturnLineCharges", "IsVisibleLedgerAccountCr", c => c.Boolean());
            AddColumn("Web.JobInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId", c => c.Int());
            AddColumn("Web.JobInvoiceSettings", "isVisibleGodown", c => c.Boolean());
            AddColumn("Web.JobInvoiceSettings", "isVisibleJobReceiveBy", c => c.Boolean());
            AddColumn("Web.JobInvoiceSettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.JobOrderInspectionRequestSettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.JobOrderInspectionSettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.JobOrderLineCharges", "IsVisibleLedgerAccountDr", c => c.Boolean());
            AddColumn("Web.JobOrderLineCharges", "filterLedgerAccountGroupsDrId", c => c.Int());
            AddColumn("Web.JobOrderLineCharges", "IsVisibleLedgerAccountCr", c => c.Boolean());
            AddColumn("Web.JobOrderLineCharges", "filterLedgerAccountGroupsCrId", c => c.Int());
            AddColumn("Web.JobOrderSettings", "isVisibleSalesTaxGroupProduct", c => c.Boolean());
            AddColumn("Web.JobOrderSettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.JobReceiveQASettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.JobReceiveSettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.StockHeaderSettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.PackingSettings", "isVisibleStockIn", c => c.Boolean());
            AddColumn("Web.PackingSettings", "isVisibleSpecification", c => c.Boolean());
            AddColumn("Web.PackingSettings", "isVisibleLotNo", c => c.Boolean());
            AddColumn("Web.PackingSettings", "isVisibleBaleNo", c => c.Boolean());
            AddColumn("Web.PackingSettings", "isVisibleDealUnit", c => c.Boolean());
            AddColumn("Web.PackingSettings", "IsMandatoryStockIn", c => c.Boolean());
            AddColumn("Web.PackingSettings", "UnitConversionForId", c => c.Byte());
            AddColumn("Web.PackingSettings", "ExportMenuId", c => c.Int());
            AddColumn("Web.PurchaseInvoiceLineCharges", "IsVisibleLedgerAccountDr", c => c.Boolean());
            AddColumn("Web.PurchaseInvoiceLineCharges", "filterLedgerAccountGroupsDrId", c => c.Int());
            AddColumn("Web.PurchaseInvoiceLineCharges", "IsVisibleLedgerAccountCr", c => c.Boolean());
            AddColumn("Web.PurchaseInvoiceLineCharges", "filterLedgerAccountGroupsCrId", c => c.Int());
            AddColumn("Web.PurchaseInvoiceReturnLineCharges", "IsVisibleLedgerAccountDr", c => c.Boolean());
            AddColumn("Web.PurchaseInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId", c => c.Int());
            AddColumn("Web.PurchaseInvoiceReturnLineCharges", "IsVisibleLedgerAccountCr", c => c.Boolean());
            AddColumn("Web.PurchaseInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId", c => c.Int());
            AddColumn("Web.PurchaseOrderLineCharges", "IsVisibleLedgerAccountDr", c => c.Boolean());
            AddColumn("Web.PurchaseOrderLineCharges", "filterLedgerAccountGroupsDrId", c => c.Int());
            AddColumn("Web.PurchaseOrderLineCharges", "IsVisibleLedgerAccountCr", c => c.Boolean());
            AddColumn("Web.PurchaseOrderLineCharges", "filterLedgerAccountGroupsCrId", c => c.Int());
            AddColumn("Web.PurchaseOrderRateAmendmentLineCharges", "IsVisibleLedgerAccountDr", c => c.Boolean());
            AddColumn("Web.PurchaseOrderRateAmendmentLineCharges", "filterLedgerAccountGroupsDrId", c => c.Int());
            AddColumn("Web.PurchaseOrderRateAmendmentLineCharges", "IsVisibleLedgerAccountCr", c => c.Boolean());
            AddColumn("Web.PurchaseOrderRateAmendmentLineCharges", "filterLedgerAccountGroupsCrId", c => c.Int());
            AddColumn("Web.PurchaseQuotationLineCharges", "IsVisibleLedgerAccountDr", c => c.Boolean());
            AddColumn("Web.PurchaseQuotationLineCharges", "filterLedgerAccountGroupsDrId", c => c.Int());
            AddColumn("Web.PurchaseQuotationLineCharges", "IsVisibleLedgerAccountCr", c => c.Boolean());
            AddColumn("Web.PurchaseQuotationLineCharges", "filterLedgerAccountGroupsCrId", c => c.Int());
            AddColumn("Web.SaleDeliveryOrderSettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.SaleDeliverySettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.SaleInvoiceLineCharges", "IsVisibleLedgerAccountDr", c => c.Boolean());
            AddColumn("Web.SaleInvoiceLineCharges", "filterLedgerAccountGroupsDrId", c => c.Int());
            AddColumn("Web.SaleInvoiceLineCharges", "IsVisibleLedgerAccountCr", c => c.Boolean());
            AddColumn("Web.SaleInvoiceLineCharges", "filterLedgerAccountGroupsCrId", c => c.Int());
            AddColumn("Web.SaleInvoiceReturnHeaders", "Nature", c => c.String(maxLength: 20));
            AddColumn("Web.SaleInvoiceReturnLineCharges", "IsVisibleLedgerAccountDr", c => c.Boolean());
            AddColumn("Web.SaleInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId", c => c.Int());
            AddColumn("Web.SaleInvoiceReturnLineCharges", "IsVisibleLedgerAccountCr", c => c.Boolean());
            AddColumn("Web.SaleInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId", c => c.Int());
            AddColumn("Web.SaleInvoiceSettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.SaleQuotationLineCharges", "IsVisibleLedgerAccountDr", c => c.Boolean());
            AddColumn("Web.SaleQuotationLineCharges", "filterLedgerAccountGroupsDrId", c => c.Int());
            AddColumn("Web.SaleQuotationLineCharges", "IsVisibleLedgerAccountCr", c => c.Boolean());
            AddColumn("Web.SaleQuotationLineCharges", "filterLedgerAccountGroupsCrId", c => c.Int());
            AddColumn("Web.SiteDivisionSettings", "SalesTaxProductCodeCaption", c => c.String(maxLength: 50));
            AddColumn("Web.SiteDivisionSettings", "SalesTaxCaption", c => c.String(maxLength: 50));
            AddColumn("Web.SiteDivisionSettings", "SalesTaxGroupProductCaption", c => c.String(maxLength: 50));
            AddColumn("Web.SiteDivisionSettings", "SalesTaxGroupPersonCaption", c => c.String(maxLength: 50));
            AddColumn("Web.SiteDivisionSettings", "SalesTaxRegistrationCaption", c => c.String(maxLength: 50));
            AddColumn("Web.ViewSaleOrderBalanceForCancellation", "SiteId", c => c.Int(nullable: false));
            AddColumn("Web.ViewSaleOrderBalanceForCancellation", "DivisionId", c => c.Int(nullable: false));
            CreateIndex("Web.States", "StateCode", unique: true, name: "IX_State_StateCode");
            CreateIndex("Web.PackingLines", "StockInId");
            CreateIndex("Web.CalculationLineLedgerAccounts", "filterLedgerAccountGroupsDrId");
            CreateIndex("Web.CalculationLineLedgerAccounts", "filterLedgerAccountGroupsCrId");
            CreateIndex("Web.SaleInvoiceLines", "Dimension3Id");
            CreateIndex("Web.SaleInvoiceLines", "Dimension4Id");
            CreateIndex("Web.JobConsumptionSettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.JobInvoiceLineCharges", "filterLedgerAccountGroupsDrId");
            CreateIndex("Web.JobInvoiceLineCharges", "filterLedgerAccountGroupsCrId");
            CreateIndex("Web.JobInvoiceRateAmendmentLineCharges", "filterLedgerAccountGroupsDrId");
            CreateIndex("Web.JobInvoiceRateAmendmentLineCharges", "filterLedgerAccountGroupsCrId");
            CreateIndex("Web.JobInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId");
            CreateIndex("Web.JobInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId");
            CreateIndex("Web.JobInvoiceSettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.JobOrderInspectionRequestSettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.JobOrderInspectionSettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.JobOrderLineCharges", "filterLedgerAccountGroupsDrId");
            CreateIndex("Web.JobOrderLineCharges", "filterLedgerAccountGroupsCrId");
            CreateIndex("Web.JobOrderSettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.JobReceiveQASettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.JobReceiveSettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.StockHeaderSettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.PackingSettings", "UnitConversionForId");
            CreateIndex("Web.PackingSettings", "ExportMenuId");
            CreateIndex("Web.PurchaseInvoiceLineCharges", "filterLedgerAccountGroupsDrId");
            CreateIndex("Web.PurchaseInvoiceLineCharges", "filterLedgerAccountGroupsCrId");
            CreateIndex("Web.PurchaseInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId");
            CreateIndex("Web.PurchaseInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId");
            CreateIndex("Web.PurchaseOrderLineCharges", "filterLedgerAccountGroupsDrId");
            CreateIndex("Web.PurchaseOrderLineCharges", "filterLedgerAccountGroupsCrId");
            CreateIndex("Web.PurchaseOrderRateAmendmentLineCharges", "filterLedgerAccountGroupsDrId");
            CreateIndex("Web.PurchaseOrderRateAmendmentLineCharges", "filterLedgerAccountGroupsCrId");
            CreateIndex("Web.PurchaseQuotationLineCharges", "filterLedgerAccountGroupsDrId");
            CreateIndex("Web.PurchaseQuotationLineCharges", "filterLedgerAccountGroupsCrId");
            CreateIndex("Web.SaleDeliveryOrderSettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.SaleDeliverySettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.SaleInvoiceLineCharges", "filterLedgerAccountGroupsDrId");
            CreateIndex("Web.SaleInvoiceLineCharges", "filterLedgerAccountGroupsCrId");
            CreateIndex("Web.SaleInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId");
            CreateIndex("Web.SaleInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId");
            CreateIndex("Web.SaleInvoiceSettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.SaleQuotationLineCharges", "filterLedgerAccountGroupsDrId");
            CreateIndex("Web.SaleQuotationLineCharges", "filterLedgerAccountGroupsCrId");
            AddForeignKey("Web.PackingLines", "StockInId", "Web.Stocks", "StockId");
            AddForeignKey("Web.CalculationLineLedgerAccounts", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.CalculationLineLedgerAccounts", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.SaleInvoiceLines", "Dimension3Id", "Web.Dimension3", "Dimension3Id");
            AddForeignKey("Web.SaleInvoiceLines", "Dimension4Id", "Web.Dimension4", "Dimension4Id");
            AddForeignKey("Web.JobConsumptionSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.JobInvoiceLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.JobInvoiceLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.JobInvoiceRateAmendmentLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.JobInvoiceRateAmendmentLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.JobInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.JobInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.JobInvoiceSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.JobOrderInspectionRequestSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.JobOrderInspectionSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.JobOrderLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.JobOrderLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.JobOrderSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.JobReceiveQASettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.JobReceiveSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.StockHeaderSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.PackingSettings", "ExportMenuId", "Web.Menus", "MenuId");
            AddForeignKey("Web.PackingSettings", "UnitConversionForId", "Web.UnitConversionFors", "UnitconversionForId");
            AddForeignKey("Web.PurchaseInvoiceLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.PurchaseInvoiceLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.PurchaseInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.PurchaseInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.PurchaseOrderLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.PurchaseOrderLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.PurchaseOrderRateAmendmentLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.PurchaseOrderRateAmendmentLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.PurchaseQuotationLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.PurchaseQuotationLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.SaleDeliveryOrderSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.SaleDeliverySettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.SaleInvoiceLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.SaleInvoiceLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.SaleInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.SaleInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.SaleInvoiceSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.SaleQuotationLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            AddForeignKey("Web.SaleQuotationLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            DropColumn("Web.SaleInvoiceReturnHeaders", "SalesTaxGroupId");
            DropColumn("Web.SaleInvoiceReturnHeaders", "SalesTaxGroupPartyId");
            DropColumn("Web.SaleInvoiceReturnHeaders", "CurrencyId");
        }
        
        public override void Down()
        {
            AddColumn("Web.SaleInvoiceReturnHeaders", "CurrencyId", c => c.Int(nullable: false));
            AddColumn("Web.SaleInvoiceReturnHeaders", "SalesTaxGroupPartyId", c => c.Int());
            AddColumn("Web.SaleInvoiceReturnHeaders", "SalesTaxGroupId", c => c.Int());
            DropForeignKey("Web.SaleQuotationLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.SaleQuotationLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.SaleInvoiceSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.SaleInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.SaleInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.SaleInvoiceLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.SaleInvoiceLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.SaleDeliverySettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.SaleDeliveryOrderSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.PurchaseQuotationLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.PurchaseQuotationLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.PurchaseOrderRateAmendmentLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.PurchaseOrderRateAmendmentLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.PurchaseOrderLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.PurchaseOrderLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.PurchaseInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.PurchaseInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.PurchaseInvoiceLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.PurchaseInvoiceLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.PackingSettings", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.PackingSettings", "ExportMenuId", "Web.Menus");
            DropForeignKey("Web.StockHeaderSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.JobReceiveSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.JobReceiveQASettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.JobOrderSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.JobOrderLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.JobOrderLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.JobOrderInspectionSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.JobOrderInspectionRequestSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.JobInvoiceSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.JobInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.JobInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.JobInvoiceRateAmendmentLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.JobInvoiceRateAmendmentLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.JobInvoiceLineCharges", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.JobInvoiceLineCharges", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.JobConsumptionSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.SaleInvoiceLines", "Dimension4Id", "Web.Dimension4");
            DropForeignKey("Web.SaleInvoiceLines", "Dimension3Id", "Web.Dimension3");
            DropForeignKey("Web.ChargeGroupPersonCalculations", "SiteId", "Web.Sites");
            DropForeignKey("Web.ChargeGroupPersonCalculations", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ChargeGroupPersonCalculations", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ChargeGroupPersonCalculations", "ChargeGroupPersonId", "Web.ChargeGroupPersons");
            DropForeignKey("Web.ChargeGroupPersonCalculations", "CalculationId", "Web.Calculations");
            DropForeignKey("Web.CalculationLineLedgerAccounts", "filterLedgerAccountGroupsDrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.CalculationLineLedgerAccounts", "filterLedgerAccountGroupsCrId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.PackingLines", "StockInId", "Web.Stocks");
            DropIndex("Web.SaleQuotationLineCharges", new[] { "filterLedgerAccountGroupsCrId" });
            DropIndex("Web.SaleQuotationLineCharges", new[] { "filterLedgerAccountGroupsDrId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.SaleInvoiceReturnLineCharges", new[] { "filterLedgerAccountGroupsCrId" });
            DropIndex("Web.SaleInvoiceReturnLineCharges", new[] { "filterLedgerAccountGroupsDrId" });
            DropIndex("Web.SaleInvoiceLineCharges", new[] { "filterLedgerAccountGroupsCrId" });
            DropIndex("Web.SaleInvoiceLineCharges", new[] { "filterLedgerAccountGroupsDrId" });
            DropIndex("Web.SaleDeliverySettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.SaleDeliveryOrderSettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.PurchaseQuotationLineCharges", new[] { "filterLedgerAccountGroupsCrId" });
            DropIndex("Web.PurchaseQuotationLineCharges", new[] { "filterLedgerAccountGroupsDrId" });
            DropIndex("Web.PurchaseOrderRateAmendmentLineCharges", new[] { "filterLedgerAccountGroupsCrId" });
            DropIndex("Web.PurchaseOrderRateAmendmentLineCharges", new[] { "filterLedgerAccountGroupsDrId" });
            DropIndex("Web.PurchaseOrderLineCharges", new[] { "filterLedgerAccountGroupsCrId" });
            DropIndex("Web.PurchaseOrderLineCharges", new[] { "filterLedgerAccountGroupsDrId" });
            DropIndex("Web.PurchaseInvoiceReturnLineCharges", new[] { "filterLedgerAccountGroupsCrId" });
            DropIndex("Web.PurchaseInvoiceReturnLineCharges", new[] { "filterLedgerAccountGroupsDrId" });
            DropIndex("Web.PurchaseInvoiceLineCharges", new[] { "filterLedgerAccountGroupsCrId" });
            DropIndex("Web.PurchaseInvoiceLineCharges", new[] { "filterLedgerAccountGroupsDrId" });
            DropIndex("Web.PackingSettings", new[] { "ExportMenuId" });
            DropIndex("Web.PackingSettings", new[] { "UnitConversionForId" });
            DropIndex("Web.StockHeaderSettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.JobReceiveSettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.JobReceiveQASettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.JobOrderSettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.JobOrderLineCharges", new[] { "filterLedgerAccountGroupsCrId" });
            DropIndex("Web.JobOrderLineCharges", new[] { "filterLedgerAccountGroupsDrId" });
            DropIndex("Web.JobOrderInspectionSettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.JobOrderInspectionRequestSettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.JobInvoiceSettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.JobInvoiceReturnLineCharges", new[] { "filterLedgerAccountGroupsCrId" });
            DropIndex("Web.JobInvoiceReturnLineCharges", new[] { "filterLedgerAccountGroupsDrId" });
            DropIndex("Web.JobInvoiceRateAmendmentLineCharges", new[] { "filterLedgerAccountGroupsCrId" });
            DropIndex("Web.JobInvoiceRateAmendmentLineCharges", new[] { "filterLedgerAccountGroupsDrId" });
            DropIndex("Web.JobInvoiceLineCharges", new[] { "filterLedgerAccountGroupsCrId" });
            DropIndex("Web.JobInvoiceLineCharges", new[] { "filterLedgerAccountGroupsDrId" });
            DropIndex("Web.JobConsumptionSettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.SaleInvoiceLines", new[] { "Dimension4Id" });
            DropIndex("Web.SaleInvoiceLines", new[] { "Dimension3Id" });
            DropIndex("Web.ChargeGroupPersonCalculations", new[] { "CalculationId" });
            DropIndex("Web.ChargeGroupPersonCalculations", "ChargeGroupPersonCalculation_DocID");
            DropIndex("Web.CalculationLineLedgerAccounts", new[] { "filterLedgerAccountGroupsCrId" });
            DropIndex("Web.CalculationLineLedgerAccounts", new[] { "filterLedgerAccountGroupsDrId" });
            DropIndex("Web.PackingLines", new[] { "StockInId" });
            DropIndex("Web.States", "IX_State_StateCode");
            DropColumn("Web.ViewSaleOrderBalanceForCancellation", "DivisionId");
            DropColumn("Web.ViewSaleOrderBalanceForCancellation", "SiteId");
            DropColumn("Web.SiteDivisionSettings", "SalesTaxRegistrationCaption");
            DropColumn("Web.SiteDivisionSettings", "SalesTaxGroupPersonCaption");
            DropColumn("Web.SiteDivisionSettings", "SalesTaxGroupProductCaption");
            DropColumn("Web.SiteDivisionSettings", "SalesTaxCaption");
            DropColumn("Web.SiteDivisionSettings", "SalesTaxProductCodeCaption");
            DropColumn("Web.SaleQuotationLineCharges", "filterLedgerAccountGroupsCrId");
            DropColumn("Web.SaleQuotationLineCharges", "IsVisibleLedgerAccountCr");
            DropColumn("Web.SaleQuotationLineCharges", "filterLedgerAccountGroupsDrId");
            DropColumn("Web.SaleQuotationLineCharges", "IsVisibleLedgerAccountDr");
            DropColumn("Web.SaleInvoiceSettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.SaleInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId");
            DropColumn("Web.SaleInvoiceReturnLineCharges", "IsVisibleLedgerAccountCr");
            DropColumn("Web.SaleInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId");
            DropColumn("Web.SaleInvoiceReturnLineCharges", "IsVisibleLedgerAccountDr");
            DropColumn("Web.SaleInvoiceReturnHeaders", "Nature");
            DropColumn("Web.SaleInvoiceLineCharges", "filterLedgerAccountGroupsCrId");
            DropColumn("Web.SaleInvoiceLineCharges", "IsVisibleLedgerAccountCr");
            DropColumn("Web.SaleInvoiceLineCharges", "filterLedgerAccountGroupsDrId");
            DropColumn("Web.SaleInvoiceLineCharges", "IsVisibleLedgerAccountDr");
            DropColumn("Web.SaleDeliverySettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.SaleDeliveryOrderSettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.PurchaseQuotationLineCharges", "filterLedgerAccountGroupsCrId");
            DropColumn("Web.PurchaseQuotationLineCharges", "IsVisibleLedgerAccountCr");
            DropColumn("Web.PurchaseQuotationLineCharges", "filterLedgerAccountGroupsDrId");
            DropColumn("Web.PurchaseQuotationLineCharges", "IsVisibleLedgerAccountDr");
            DropColumn("Web.PurchaseOrderRateAmendmentLineCharges", "filterLedgerAccountGroupsCrId");
            DropColumn("Web.PurchaseOrderRateAmendmentLineCharges", "IsVisibleLedgerAccountCr");
            DropColumn("Web.PurchaseOrderRateAmendmentLineCharges", "filterLedgerAccountGroupsDrId");
            DropColumn("Web.PurchaseOrderRateAmendmentLineCharges", "IsVisibleLedgerAccountDr");
            DropColumn("Web.PurchaseOrderLineCharges", "filterLedgerAccountGroupsCrId");
            DropColumn("Web.PurchaseOrderLineCharges", "IsVisibleLedgerAccountCr");
            DropColumn("Web.PurchaseOrderLineCharges", "filterLedgerAccountGroupsDrId");
            DropColumn("Web.PurchaseOrderLineCharges", "IsVisibleLedgerAccountDr");
            DropColumn("Web.PurchaseInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId");
            DropColumn("Web.PurchaseInvoiceReturnLineCharges", "IsVisibleLedgerAccountCr");
            DropColumn("Web.PurchaseInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId");
            DropColumn("Web.PurchaseInvoiceReturnLineCharges", "IsVisibleLedgerAccountDr");
            DropColumn("Web.PurchaseInvoiceLineCharges", "filterLedgerAccountGroupsCrId");
            DropColumn("Web.PurchaseInvoiceLineCharges", "IsVisibleLedgerAccountCr");
            DropColumn("Web.PurchaseInvoiceLineCharges", "filterLedgerAccountGroupsDrId");
            DropColumn("Web.PurchaseInvoiceLineCharges", "IsVisibleLedgerAccountDr");
            DropColumn("Web.PackingSettings", "ExportMenuId");
            DropColumn("Web.PackingSettings", "UnitConversionForId");
            DropColumn("Web.PackingSettings", "IsMandatoryStockIn");
            DropColumn("Web.PackingSettings", "isVisibleDealUnit");
            DropColumn("Web.PackingSettings", "isVisibleBaleNo");
            DropColumn("Web.PackingSettings", "isVisibleLotNo");
            DropColumn("Web.PackingSettings", "isVisibleSpecification");
            DropColumn("Web.PackingSettings", "isVisibleStockIn");
            DropColumn("Web.StockHeaderSettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.JobReceiveSettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.JobReceiveQASettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.JobOrderSettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.JobOrderSettings", "isVisibleSalesTaxGroupProduct");
            DropColumn("Web.JobOrderLineCharges", "filterLedgerAccountGroupsCrId");
            DropColumn("Web.JobOrderLineCharges", "IsVisibleLedgerAccountCr");
            DropColumn("Web.JobOrderLineCharges", "filterLedgerAccountGroupsDrId");
            DropColumn("Web.JobOrderLineCharges", "IsVisibleLedgerAccountDr");
            DropColumn("Web.JobOrderInspectionSettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.JobOrderInspectionRequestSettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.JobInvoiceSettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.JobInvoiceSettings", "isVisibleJobReceiveBy");
            DropColumn("Web.JobInvoiceSettings", "isVisibleGodown");
            DropColumn("Web.JobInvoiceReturnLineCharges", "filterLedgerAccountGroupsCrId");
            DropColumn("Web.JobInvoiceReturnLineCharges", "IsVisibleLedgerAccountCr");
            DropColumn("Web.JobInvoiceReturnLineCharges", "filterLedgerAccountGroupsDrId");
            DropColumn("Web.JobInvoiceReturnLineCharges", "IsVisibleLedgerAccountDr");
            DropColumn("Web.JobInvoiceReturnHeaders", "Nature");
            DropColumn("Web.JobInvoiceRateAmendmentLineCharges", "filterLedgerAccountGroupsCrId");
            DropColumn("Web.JobInvoiceRateAmendmentLineCharges", "IsVisibleLedgerAccountCr");
            DropColumn("Web.JobInvoiceRateAmendmentLineCharges", "filterLedgerAccountGroupsDrId");
            DropColumn("Web.JobInvoiceRateAmendmentLineCharges", "IsVisibleLedgerAccountDr");
            DropColumn("Web.JobInvoiceLineCharges", "filterLedgerAccountGroupsCrId");
            DropColumn("Web.JobInvoiceLineCharges", "IsVisibleLedgerAccountCr");
            DropColumn("Web.JobInvoiceLineCharges", "filterLedgerAccountGroupsDrId");
            DropColumn("Web.JobInvoiceLineCharges", "IsVisibleLedgerAccountDr");
            DropColumn("Web.JobConsumptionSettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.DocumentTypeSettings", "DocIdCaption");
            DropColumn("Web.SaleInvoiceLines", "Dimension4Id");
            DropColumn("Web.SaleInvoiceLines", "Dimension3Id");
            DropColumn("Web.CalculationLineLedgerAccounts", "filterLedgerAccountGroupsCrId");
            DropColumn("Web.CalculationLineLedgerAccounts", "IsVisibleLedgerAccountCr");
            DropColumn("Web.CalculationLineLedgerAccounts", "filterLedgerAccountGroupsDrId");
            DropColumn("Web.CalculationLineLedgerAccounts", "IsVisibleLedgerAccountDr");
            DropColumn("Web.ChargeTypes", "Category");
            DropColumn("Web.Charges", "PrintingDescription");
            DropColumn("Web.ChargeGroupPersons", "PrintingDescription");
            DropColumn("Web.PackingLines", "StockInId");
            DropColumn("Web.ChargeGroupProducts", "PrintingDescription");
            DropColumn("Web.ProductGroups", "RateDecimalPlaces");
            DropColumn("Web.States", "StateCode");
            DropTable("Web.ViewPackingBalance");
            DropTable("Web.ChargeGroupPersonCalculations");
            CreateIndex("Web.SaleInvoiceReturnHeaders", "CurrencyId");
            CreateIndex("Web.SaleInvoiceReturnHeaders", "SalesTaxGroupPartyId");
            CreateIndex("Web.SaleInvoiceReturnHeaders", "SalesTaxGroupId");
            AddForeignKey("Web.SaleInvoiceReturnHeaders", "SalesTaxGroupPartyId", "Web.SalesTaxGroupParties", "SalesTaxGroupPartyId");
            AddForeignKey("Web.SaleInvoiceReturnHeaders", "SalesTaxGroupId", "Web.ChargeGroupPersons", "ChargeGroupPersonId");
            AddForeignKey("Web.SaleInvoiceReturnHeaders", "CurrencyId", "Web.Currencies", "ID");
        }
    }
}
