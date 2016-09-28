namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleEnqquirySettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Web.SaleEnquirySettings",
                c => new
                    {
                        SaleEnquirySettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        CurrencyId = c.Int(nullable: false),
                        DeliveryTermsId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        UnitConversionForId = c.Byte(nullable: false),
                        isVisibleCurrency = c.Boolean(),
                        isVisibleShipMethod = c.Boolean(),
                        isVisibleDeliveryTerms = c.Boolean(),
                        isVisiblePriority = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisibleDealUnit = c.Boolean(),
                        isVisibleSpecification = c.Boolean(),
                        isVisibleProductCode = c.Boolean(),
                        isVisibleUnitConversionFor = c.Boolean(),
                        isVisibleAdvance = c.Boolean(),
                        filterLedgerAccountGroups = c.String(),
                        filterLedgerAccounts = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        ImportMenuId = c.Int(),
                        CalculationId = c.Int(),
                        ProcessId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleEnquirySettingsId)
                .ForeignKey("Web.Calculations", t => t.CalculationId)
                .ForeignKey("Web.Currencies", t => t.CurrencyId)
                .ForeignKey("Web.DeliveryTerms", t => t.DeliveryTermsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.ShipMethods", t => t.ShipMethodId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ShipMethodId)
                .Index(t => t.CurrencyId)
                .Index(t => t.DeliveryTermsId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.UnitConversionForId)
                .Index(t => t.ImportMenuId)
                .Index(t => t.CalculationId)
                .Index(t => t.ProcessId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Web.SaleEnquirySettings", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.SaleEnquirySettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleEnquirySettings", "ShipMethodId", "Web.ShipMethods");
            DropForeignKey("Web.SaleEnquirySettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.SaleEnquirySettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.SaleEnquirySettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleEnquirySettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleEnquirySettings", "DeliveryTermsId", "Web.DeliveryTerms");
            DropForeignKey("Web.SaleEnquirySettings", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.SaleEnquirySettings", "CalculationId", "Web.Calculations");
            DropIndex("Web.SaleEnquirySettings", new[] { "ProcessId" });
            DropIndex("Web.SaleEnquirySettings", new[] { "CalculationId" });
            DropIndex("Web.SaleEnquirySettings", new[] { "ImportMenuId" });
            DropIndex("Web.SaleEnquirySettings", new[] { "UnitConversionForId" });
            DropIndex("Web.SaleEnquirySettings", new[] { "DivisionId" });
            DropIndex("Web.SaleEnquirySettings", new[] { "SiteId" });
            DropIndex("Web.SaleEnquirySettings", new[] { "DeliveryTermsId" });
            DropIndex("Web.SaleEnquirySettings", new[] { "CurrencyId" });
            DropIndex("Web.SaleEnquirySettings", new[] { "ShipMethodId" });
            DropIndex("Web.SaleEnquirySettings", new[] { "DocTypeId" });
            DropTable("Web.SaleEnquirySettings");
        }
    }
}
