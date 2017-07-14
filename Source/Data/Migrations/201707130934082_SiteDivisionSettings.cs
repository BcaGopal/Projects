namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SiteDivisionSettings : DbMigration
    {
        public override void Up()
        {
            DropIndex("Web.JobReceiveSettings", new[] { "DocTypeId" });
            DropIndex("Web.JobReceiveSettings", new[] { "SiteId" });
            DropIndex("Web.JobReceiveSettings", new[] { "DivisionId" });
            CreateTable(
                "Web.CompanySettings",
                c => new
                    {
                        CompanySettingsId = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        isVisibleMessage = c.Boolean(),
                        isVisibleTask = c.Boolean(),
                        isVisibleNotification = c.Boolean(),
                        SiteCaption = c.String(),
                        DivisionCaption = c.String(),
                        GodownCaption = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CompanySettingsId);
            
            CreateTable(
                "Web.SiteDivisionSettings",
                c => new
                    {
                        SiteDivisionSettingsId = c.Int(nullable: false, identity: true),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        IsApplicableVAT = c.Boolean(nullable: false),
                        IsApplicableGST = c.Boolean(nullable: false),
                        ReportHeaderTextLeft = c.String(),
                        ReportHeaderTextRight = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SiteDivisionSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            AddColumn("Web.ProductCategories", "DefaultSalesTaxProductCodeId", c => c.Int());
            AddColumn("Web.ProductGroups", "DefaultSalesTaxGroupProductId", c => c.Int());
            AddColumn("Web.PackingLines", "BaleCount", c => c.Int());
            AddColumn("Web.LedgerHeaders", "ForLedgerHeaderId", c => c.Int());
            AddColumn("Web.CarpetSkuSettings", "isVisibleSalesTaxProductCode", c => c.Boolean());
            AddColumn("Web.CarpetSkuSettings", "SalesTaxProductCodeCaption", c => c.String(maxLength: 50));
            AddColumn("Web.ChargeGroupSettings", "ProcessId", c => c.Int(nullable: false));
            AddColumn("Web.LedgerSettings", "CancelDocTypeId", c => c.Int());
            AddColumn("Web.StockHeaderSettings", "isPostedInStock", c => c.Boolean(nullable: false));
            AddColumn("Web.PackingSettings", "isVisibleProductUID", c => c.Boolean());
            AddColumn("Web.PackingSettings", "isVisibleBaleCount", c => c.Boolean());
            AddColumn("Web.PackingSettings", "filterProductDivision", c => c.String());
            AddColumn("Web.PackingSettings", "filterLedgerAccountGroups", c => c.String());
            AddColumn("Web.PackingSettings", "filterLedgerAccounts", c => c.String());
            AddColumn("Web.PackingSettings", "ProcessId", c => c.Int());
            AddColumn("Web.PersonSettings", "isVisibleGstNo", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryGstNo", c => c.Boolean());
            AddColumn("Web.SaleInvoiceSettings", "DoNotUpdateProductUidStatus", c => c.Boolean());
            AddColumn("Web.StockInHandSettings", "ShowOpening", c => c.Boolean(nullable: false));
            AddColumn("Web.StockInHandSettings", "ProductTypeId", c => c.Int());
            AddColumn("Web.StockInHandSettings", "TableName", c => c.String());
            CreateIndex("Web.ProductCategories", "DefaultSalesTaxProductCodeId");
            CreateIndex("Web.ProductGroups", "DefaultSalesTaxGroupProductId");
            CreateIndex("Web.LedgerHeaders", "ForLedgerHeaderId");
            CreateIndex("Web.ChargeGroupSettings", "ProcessId");
            CreateIndex("Web.JobReceiveSettings", new[] { "DocTypeId", "SiteId", "DivisionId" }, unique: true, name: "IX_JobReceiveSettings_DocID");
            CreateIndex("Web.LedgerSettings", "CancelDocTypeId");
            CreateIndex("Web.PackingSettings", "ProcessId");
            AddForeignKey("Web.ProductCategories", "DefaultSalesTaxProductCodeId", "Web.SalesTaxProductCodes", "SalesTaxProductCodeId");
            AddForeignKey("Web.ProductGroups", "DefaultSalesTaxGroupProductId", "Web.ChargeGroupProducts", "ChargeGroupProductId");
            AddForeignKey("Web.LedgerHeaders", "ForLedgerHeaderId", "Web.LedgerHeaders", "LedgerHeaderId");
            AddForeignKey("Web.ChargeGroupSettings", "ProcessId", "Web.Processes", "ProcessId");
            AddForeignKey("Web.LedgerSettings", "CancelDocTypeId", "Web.DocumentTypes", "DocumentTypeId");
            AddForeignKey("Web.PackingSettings", "ProcessId", "Web.Processes", "ProcessId");
            DropColumn("Web.Sites", "ReportHeaderTextLeft");
            DropColumn("Web.Sites", "ReportHeaderTextRight");
        }
        
        public override void Down()
        {
            AddColumn("Web.Sites", "ReportHeaderTextRight", c => c.String());
            AddColumn("Web.Sites", "ReportHeaderTextLeft", c => c.String());
            DropForeignKey("Web.SiteDivisionSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.SiteDivisionSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PackingSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.LedgerSettings", "CancelDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ChargeGroupSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.LedgerHeaders", "ForLedgerHeaderId", "Web.LedgerHeaders");
            DropForeignKey("Web.ProductGroups", "DefaultSalesTaxGroupProductId", "Web.ChargeGroupProducts");
            DropForeignKey("Web.ProductCategories", "DefaultSalesTaxProductCodeId", "Web.SalesTaxProductCodes");
            DropIndex("Web.SiteDivisionSettings", new[] { "DivisionId" });
            DropIndex("Web.SiteDivisionSettings", new[] { "SiteId" });
            DropIndex("Web.PackingSettings", new[] { "ProcessId" });
            DropIndex("Web.LedgerSettings", new[] { "CancelDocTypeId" });
            DropIndex("Web.JobReceiveSettings", "IX_JobReceiveSettings_DocID");
            DropIndex("Web.ChargeGroupSettings", new[] { "ProcessId" });
            DropIndex("Web.LedgerHeaders", new[] { "ForLedgerHeaderId" });
            DropIndex("Web.ProductGroups", new[] { "DefaultSalesTaxGroupProductId" });
            DropIndex("Web.ProductCategories", new[] { "DefaultSalesTaxProductCodeId" });
            DropColumn("Web.StockInHandSettings", "TableName");
            DropColumn("Web.StockInHandSettings", "ProductTypeId");
            DropColumn("Web.StockInHandSettings", "ShowOpening");
            DropColumn("Web.SaleInvoiceSettings", "DoNotUpdateProductUidStatus");
            DropColumn("Web.PersonSettings", "isMandatoryGstNo");
            DropColumn("Web.PersonSettings", "isVisibleGstNo");
            DropColumn("Web.PackingSettings", "ProcessId");
            DropColumn("Web.PackingSettings", "filterLedgerAccounts");
            DropColumn("Web.PackingSettings", "filterLedgerAccountGroups");
            DropColumn("Web.PackingSettings", "filterProductDivision");
            DropColumn("Web.PackingSettings", "isVisibleBaleCount");
            DropColumn("Web.PackingSettings", "isVisibleProductUID");
            DropColumn("Web.StockHeaderSettings", "isPostedInStock");
            DropColumn("Web.LedgerSettings", "CancelDocTypeId");
            DropColumn("Web.ChargeGroupSettings", "ProcessId");
            DropColumn("Web.CarpetSkuSettings", "SalesTaxProductCodeCaption");
            DropColumn("Web.CarpetSkuSettings", "isVisibleSalesTaxProductCode");
            DropColumn("Web.LedgerHeaders", "ForLedgerHeaderId");
            DropColumn("Web.PackingLines", "BaleCount");
            DropColumn("Web.ProductGroups", "DefaultSalesTaxGroupProductId");
            DropColumn("Web.ProductCategories", "DefaultSalesTaxProductCodeId");
            DropTable("Web.SiteDivisionSettings");
            DropTable("Web.CompanySettings");
            CreateIndex("Web.JobReceiveSettings", "DivisionId");
            CreateIndex("Web.JobReceiveSettings", "SiteId");
            CreateIndex("Web.JobReceiveSettings", "DocTypeId");
        }
    }
}
