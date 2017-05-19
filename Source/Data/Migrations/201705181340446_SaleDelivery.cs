namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleDelivery : DbMigration
    {
        public override void Up()
        {
            DropIndex("Web.StockAdjs", "IX_Stock_DocID");
            CreateTable(
                "Web.SaleDeliveryHeaders",
                c => new
                    {
                        SaleDeliveryHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        SaleToBuyerId = c.Int(nullable: false),
                        DeliverToPerson = c.String(maxLength: 100),
                        DeliverToPersonReference = c.String(maxLength: 20),
                        ShipToPartyAddress = c.String(maxLength: 250),
                        GatePassHeaderId = c.Int(),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleDeliveryHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.GatePassHeaders", t => t.GatePassHeaderId)
                .ForeignKey("Web.People", t => t.SaleToBuyerId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_SaleDeliveryHeader_DocID")
                .Index(t => t.SaleToBuyerId)
                .Index(t => t.GatePassHeaderId);
            
            CreateTable(
                "Web.SaleDeliveryLines",
                c => new
                    {
                        SaleDeliveryLineId = c.Int(nullable: false, identity: true),
                        SaleDeliveryHeaderId = c.Int(nullable: false),
                        SaleInvoiceLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(nullable: false, maxLength: 3),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        LockReason = c.String(),
                    })
                .PrimaryKey(t => t.SaleDeliveryLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.SaleDeliveryHeaders", t => t.SaleDeliveryHeaderId)
                .ForeignKey("Web.SaleInvoiceLines", t => t.SaleInvoiceLineId)
                .Index(t => t.SaleDeliveryHeaderId)
                .Index(t => t.SaleInvoiceLineId)
                .Index(t => t.DealUnitId);
            
            CreateTable(
                "Web.SaleDeliverySettings",
                c => new
                    {
                        SaleDeliverySettingId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisibleDimension3 = c.Boolean(),
                        isVisibleDimension4 = c.Boolean(),
                        filterLedgerAccountGroups = c.String(),
                        filterLedgerAccounts = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        filterPersonRoles = c.String(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        SqlProcGatePass = c.String(maxLength: 100),
                        UnitConversionForId = c.Byte(),
                        ImportMenuId = c.Int(),
                        ExportMenuId = c.Int(),
                        isVisibleDealUnit = c.Boolean(),
                        isVisibleProductUid = c.Boolean(),
                        ProcessId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SaleDeliverySettingId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ExportMenuId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.UnitConversionForId)
                .Index(t => t.ImportMenuId)
                .Index(t => t.ExportMenuId)
                .Index(t => t.ProcessId);
            
            CreateTable(
                "Web.ViewSaleInvoiceBalanceForDelivery",
                c => new
                    {
                        SaleInvoiceLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SaleInvoiceHeaderId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SaleInvoiceNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        BuyerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SaleInvoiceLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.ProductId);
            
            AddColumn("Web.LedgerHeaders", "DrCr", c => c.String(maxLength: 2));
            AddColumn("Web.StockLines", "Dimension3Id", c => c.Int());
            AddColumn("Web.StockLines", "Dimension4Id", c => c.Int());
            AddColumn("Web.RequisitionLines", "Dimension3Id", c => c.Int());
            AddColumn("Web.RequisitionLines", "Dimension4Id", c => c.Int());
            AddColumn("Web.DocumentTypeSettings", "ReferenceDocTypeCaption", c => c.String(maxLength: 50));
            AddColumn("Web.DocumentTypeSettings", "ReferenceDocIdCaption", c => c.String(maxLength: 50));
            AddColumn("Web.ExcessMaterialLines", "Dimension3Id", c => c.Int());
            AddColumn("Web.ExcessMaterialLines", "Dimension4Id", c => c.Int());
            AddColumn("Web.LedgerSettings", "isVisibleDrCr", c => c.Boolean());
            AddColumn("Web.LedgerSettings", "isVisibleReferenceDocId", c => c.Boolean());
            AddColumn("Web.LedgerSettings", "isVisibleReferenceDocTypeId", c => c.Boolean());
            AddColumn("Web.LedgerSettings", "filterReferenceDocTypes", c => c.String());
            AddColumn("Web.StockHeaderSettings", "isMandatoryProductUID", c => c.Boolean());
            AddColumn("Web.ViewStockInBalance", "Dimension3Id", c => c.Int());
            AddColumn("Web.ViewStockInBalance", "Dimension4Id", c => c.Int());
            AlterColumn("Web.Sites", "Address", c => c.String());
            CreateIndex("Web.StockLines", "Dimension3Id");
            CreateIndex("Web.StockLines", "Dimension4Id");
            CreateIndex("Web.RequisitionLines", "Dimension3Id");
            CreateIndex("Web.RequisitionLines", "Dimension4Id");
            CreateIndex("Web.ExcessMaterialLines", "Dimension3Id");
            CreateIndex("Web.ExcessMaterialLines", "Dimension4Id");
            CreateIndex("Web.StockAdjs", "DivisionId");
            CreateIndex("Web.StockAdjs", "SiteId");
            AddForeignKey("Web.StockLines", "Dimension3Id", "Web.Dimension3", "Dimension3Id");
            AddForeignKey("Web.StockLines", "Dimension4Id", "Web.Dimension4", "Dimension4Id");
            AddForeignKey("Web.RequisitionLines", "Dimension3Id", "Web.Dimension3", "Dimension3Id");
            AddForeignKey("Web.RequisitionLines", "Dimension4Id", "Web.Dimension4", "Dimension4Id");
            AddForeignKey("Web.ExcessMaterialLines", "Dimension3Id", "Web.Dimension3", "Dimension3Id");
            AddForeignKey("Web.ExcessMaterialLines", "Dimension4Id", "Web.Dimension4", "Dimension4Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Web.ViewSaleInvoiceBalanceForDelivery", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewSaleInvoiceBalanceForDelivery", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ViewSaleInvoiceBalanceForDelivery", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.SaleDeliverySettings", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.SaleDeliverySettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleDeliverySettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.SaleDeliverySettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.SaleDeliverySettings", "ExportMenuId", "Web.Menus");
            DropForeignKey("Web.SaleDeliverySettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleDeliverySettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleDeliveryLines", "SaleInvoiceLineId", "Web.SaleInvoiceLines");
            DropForeignKey("Web.SaleDeliveryLines", "SaleDeliveryHeaderId", "Web.SaleDeliveryHeaders");
            DropForeignKey("Web.SaleDeliveryLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.SaleDeliveryHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleDeliveryHeaders", "SaleToBuyerId", "Web.People");
            DropForeignKey("Web.SaleDeliveryHeaders", "GatePassHeaderId", "Web.GatePassHeaders");
            DropForeignKey("Web.SaleDeliveryHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleDeliveryHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ExcessMaterialLines", "Dimension4Id", "Web.Dimension4");
            DropForeignKey("Web.ExcessMaterialLines", "Dimension3Id", "Web.Dimension3");
            DropForeignKey("Web.RequisitionLines", "Dimension4Id", "Web.Dimension4");
            DropForeignKey("Web.RequisitionLines", "Dimension3Id", "Web.Dimension3");
            DropForeignKey("Web.StockLines", "Dimension4Id", "Web.Dimension4");
            DropForeignKey("Web.StockLines", "Dimension3Id", "Web.Dimension3");
            DropIndex("Web.ViewSaleInvoiceBalanceForDelivery", new[] { "ProductId" });
            DropIndex("Web.ViewSaleInvoiceBalanceForDelivery", new[] { "Dimension2Id" });
            DropIndex("Web.ViewSaleInvoiceBalanceForDelivery", new[] { "Dimension1Id" });
            DropIndex("Web.StockAdjs", new[] { "SiteId" });
            DropIndex("Web.StockAdjs", new[] { "DivisionId" });
            DropIndex("Web.SaleDeliverySettings", new[] { "ProcessId" });
            DropIndex("Web.SaleDeliverySettings", new[] { "ExportMenuId" });
            DropIndex("Web.SaleDeliverySettings", new[] { "ImportMenuId" });
            DropIndex("Web.SaleDeliverySettings", new[] { "UnitConversionForId" });
            DropIndex("Web.SaleDeliverySettings", new[] { "DivisionId" });
            DropIndex("Web.SaleDeliverySettings", new[] { "SiteId" });
            DropIndex("Web.SaleDeliverySettings", new[] { "DocTypeId" });
            DropIndex("Web.SaleDeliveryLines", new[] { "DealUnitId" });
            DropIndex("Web.SaleDeliveryLines", new[] { "SaleInvoiceLineId" });
            DropIndex("Web.SaleDeliveryLines", new[] { "SaleDeliveryHeaderId" });
            DropIndex("Web.SaleDeliveryHeaders", new[] { "GatePassHeaderId" });
            DropIndex("Web.SaleDeliveryHeaders", new[] { "SaleToBuyerId" });
            DropIndex("Web.SaleDeliveryHeaders", "IX_SaleDeliveryHeader_DocID");
            DropIndex("Web.ExcessMaterialLines", new[] { "Dimension4Id" });
            DropIndex("Web.ExcessMaterialLines", new[] { "Dimension3Id" });
            DropIndex("Web.RequisitionLines", new[] { "Dimension4Id" });
            DropIndex("Web.RequisitionLines", new[] { "Dimension3Id" });
            DropIndex("Web.StockLines", new[] { "Dimension4Id" });
            DropIndex("Web.StockLines", new[] { "Dimension3Id" });
            AlterColumn("Web.Sites", "Address", c => c.String(maxLength: 20));
            DropColumn("Web.ViewStockInBalance", "Dimension4Id");
            DropColumn("Web.ViewStockInBalance", "Dimension3Id");
            DropColumn("Web.StockHeaderSettings", "isMandatoryProductUID");
            DropColumn("Web.LedgerSettings", "filterReferenceDocTypes");
            DropColumn("Web.LedgerSettings", "isVisibleReferenceDocTypeId");
            DropColumn("Web.LedgerSettings", "isVisibleReferenceDocId");
            DropColumn("Web.LedgerSettings", "isVisibleDrCr");
            DropColumn("Web.ExcessMaterialLines", "Dimension4Id");
            DropColumn("Web.ExcessMaterialLines", "Dimension3Id");
            DropColumn("Web.DocumentTypeSettings", "ReferenceDocIdCaption");
            DropColumn("Web.DocumentTypeSettings", "ReferenceDocTypeCaption");
            DropColumn("Web.RequisitionLines", "Dimension4Id");
            DropColumn("Web.RequisitionLines", "Dimension3Id");
            DropColumn("Web.StockLines", "Dimension4Id");
            DropColumn("Web.StockLines", "Dimension3Id");
            DropColumn("Web.LedgerHeaders", "DrCr");
            DropTable("Web.ViewSaleInvoiceBalanceForDelivery");
            DropTable("Web.SaleDeliverySettings");
            DropTable("Web.SaleDeliveryLines");
            DropTable("Web.SaleDeliveryHeaders");
            CreateIndex("Web.StockAdjs", new[] { "DivisionId", "SiteId" }, unique: true, name: "IX_Stock_DocID");
        }
    }
}
