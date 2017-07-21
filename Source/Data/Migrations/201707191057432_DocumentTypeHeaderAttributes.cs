namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentTypeHeaderAttributes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Web.ChargeGroupProducts", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.ChargeGroupPersons", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.ChargeGroupSettings", "ChargableLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.SaleInvoiceLines", "SalesTaxGroupId", "Web.SalesTaxGroups");
            DropForeignKey("Web.SaleInvoiceSettings", "SalesTaxGroupId", "Web.SalesTaxGroups");
            DropIndex("Web.ChargeGroupProducts", new[] { "ChargeTypeId" });
            DropIndex("Web.ChargeGroupPersons", new[] { "ChargeTypeId" });
            DropIndex("Web.ChargeGroupSettings", new[] { "ChargableLedgerAccountId" });
            DropIndex("Web.SaleInvoiceLines", new[] { "SalesTaxGroupId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "SalesTaxGroupId" });
            CreateTable(
                "Web.DocumentTypeHeaderAttributes",
                c => new
                    {
                        DocumentTypeHeaderAttributeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsMandatory = c.Boolean(nullable: false),
                        DataType = c.String(),
                        ListItem = c.String(),
                        Value = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        DocumentTypeId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DocumentTypeHeaderAttributeId)
                .ForeignKey("Web.DocumentTypes", t => t.DocumentTypeId)
                .Index(t => t.DocumentTypeId);
            
            CreateTable(
                "Web.JobOrderHeaderAttributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        DocumentTypeAttributeId = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.DocumentTypeHeaderAttributes", t => t.DocumentTypeAttributeId)
                .ForeignKey("Web.JobOrderHeaders", t => t.HeaderTableId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.DocumentTypeAttributeId);
            
            CreateTable(
                "Web.ViewStockProcessBalance",
                c => new
                    {
                        StockProcessBalanceId = c.Int(nullable: false, identity: true),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        ProcessId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Dimension3Id = c.Int(),
                        Dimension4Id = c.Int(),
                        LotNo = c.String(),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.StockProcessBalanceId);
            
            AddColumn("Web.Buyers", "ExtraSaleOrderHeaderId", c => c.Int());
            AddColumn("Web.JobOrderLines", "StockInId", c => c.Int());
            AddColumn("Web.SaleInvoiceHeaders", "SalesTaxGroupPersonId", c => c.Int());
            AddColumn("Web.SaleInvoiceLines", "SalesTaxGroupProductId", c => c.Int());
            AddColumn("Web.JobInvoiceSettings", "SalesTaxGroupPersonId", c => c.Int());
            AddColumn("Web.JobOrderSettings", "isVisibleStockIn", c => c.Boolean());
            AddColumn("Web.SaleInvoiceSettings", "isVisibleSalesTaxGroupPerson", c => c.Boolean());
            AddColumn("Web.SaleInvoiceSettings", "isVisibleSalesTaxGroupProduct", c => c.Boolean());
            AddColumn("Web.SaleInvoiceSettings", "SalesTaxGroupPersonId", c => c.Int());
            AddColumn("Web.WeavingRetensions", "ProductQualityId", c => c.Int());
            AlterColumn("Web.GatePassLines", "Product", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("Web.GatePassLines", "Specification", c => c.String(maxLength: 255));
            CreateIndex("Web.JobOrderLines", "StockInId");
            CreateIndex("Web.SaleInvoiceHeaders", "SalesTaxGroupPersonId");
            CreateIndex("Web.SaleInvoiceLines", "SalesTaxGroupProductId");
            CreateIndex("Web.JobInvoiceSettings", "SalesTaxGroupPersonId");
            CreateIndex("Web.SaleInvoiceSettings", "SalesTaxGroupPersonId");
            CreateIndex("Web.WeavingRetensions", "ProductQualityId");
            AddForeignKey("Web.JobOrderLines", "StockInId", "Web.Stocks", "StockId");
            AddForeignKey("Web.SaleInvoiceLines", "SalesTaxGroupProductId", "Web.ChargeGroupProducts", "ChargeGroupProductId");
            AddForeignKey("Web.SaleInvoiceHeaders", "SalesTaxGroupPersonId", "Web.ChargeGroupPersons", "ChargeGroupPersonId");
            AddForeignKey("Web.JobInvoiceSettings", "SalesTaxGroupPersonId", "Web.ChargeGroupPersons", "ChargeGroupPersonId");
            AddForeignKey("Web.SaleInvoiceSettings", "SalesTaxGroupPersonId", "Web.ChargeGroupPersons", "ChargeGroupPersonId");
            AddForeignKey("Web.WeavingRetensions", "ProductQualityId", "Web.ProductQualities", "ProductQualityId");
            DropColumn("Web.ChargeGroupProducts", "ChargeTypeId");
            DropColumn("Web.ChargeGroupPersons", "ChargeTypeId");
            DropColumn("Web.ChargeGroupSettings", "ChargableLedgerAccountId");
            DropColumn("Web.SaleInvoiceLines", "SalesTaxGroupId");
            DropColumn("Web.SaleInvoiceSettings", "isVisibleSalesTaxGroup");
            DropColumn("Web.SaleInvoiceSettings", "SalesTaxGroupId");
        }
        
        public override void Down()
        {

            
            AddColumn("Web.SaleInvoiceSettings", "SalesTaxGroupId", c => c.Int(nullable: false));
            AddColumn("Web.SaleInvoiceSettings", "isVisibleSalesTaxGroup", c => c.Boolean());
            AddColumn("Web.SaleInvoiceLines", "SalesTaxGroupId", c => c.Int());
            AddColumn("Web.ChargeGroupSettings", "ChargableLedgerAccountId", c => c.Int(nullable: false));
            AddColumn("Web.ChargeGroupPersons", "ChargeTypeId", c => c.Int(nullable: false));
            AddColumn("Web.ChargeGroupProducts", "ChargeTypeId", c => c.Int(nullable: false));
            DropForeignKey("Web.WeavingRetensions", "ProductQualityId", "Web.ProductQualities");
            DropForeignKey("Web.SaleInvoiceSettings", "SalesTaxGroupPersonId", "Web.ChargeGroupPersons");
            DropForeignKey("Web.JobOrderHeaderAttributes", "HeaderTableId", "Web.JobOrderHeaders");
            DropForeignKey("Web.JobOrderHeaderAttributes", "DocumentTypeAttributeId", "Web.DocumentTypeHeaderAttributes");
            DropForeignKey("Web.JobInvoiceSettings", "SalesTaxGroupPersonId", "Web.ChargeGroupPersons");
            DropForeignKey("Web.DocumentTypeHeaderAttributes", "DocumentTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleInvoiceHeaders", "SalesTaxGroupPersonId", "Web.ChargeGroupPersons");
            DropForeignKey("Web.SaleInvoiceLines", "SalesTaxGroupProductId", "Web.ChargeGroupProducts");
            DropForeignKey("Web.JobOrderLines", "StockInId", "Web.Stocks");
            DropIndex("Web.WeavingRetensions", new[] { "ProductQualityId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "SalesTaxGroupPersonId" });
            DropIndex("Web.JobOrderHeaderAttributes", new[] { "DocumentTypeAttributeId" });
            DropIndex("Web.JobOrderHeaderAttributes", new[] { "HeaderTableId" });
            DropIndex("Web.JobInvoiceSettings", new[] { "SalesTaxGroupPersonId" });
            DropIndex("Web.DocumentTypeHeaderAttributes", new[] { "DocumentTypeId" });
            DropIndex("Web.SaleInvoiceLines", new[] { "SalesTaxGroupProductId" });
            DropIndex("Web.SaleInvoiceHeaders", new[] { "SalesTaxGroupPersonId" });
            DropIndex("Web.JobOrderLines", new[] { "StockInId" });
            AlterColumn("Web.GatePassLines", "Specification", c => c.String(maxLength: 50));
            AlterColumn("Web.GatePassLines", "Product", c => c.String(nullable: false, maxLength: 100));
            DropColumn("Web.WeavingRetensions", "ProductQualityId");
            DropColumn("Web.SaleInvoiceSettings", "SalesTaxGroupPersonId");
            DropColumn("Web.SaleInvoiceSettings", "isVisibleSalesTaxGroupProduct");
            DropColumn("Web.SaleInvoiceSettings", "isVisibleSalesTaxGroupPerson");
            DropColumn("Web.JobOrderSettings", "isVisibleStockIn");
            DropColumn("Web.JobInvoiceSettings", "SalesTaxGroupPersonId");
            DropColumn("Web.SaleInvoiceLines", "SalesTaxGroupProductId");
            DropColumn("Web.SaleInvoiceHeaders", "SalesTaxGroupPersonId");
            DropColumn("Web.JobOrderLines", "StockInId");
            DropColumn("Web.Buyers", "ExtraSaleOrderHeaderId");
            DropTable("Web.ViewStockProcessBalance");
            DropTable("Web.JobOrderHeaderAttributes");
            DropTable("Web.DocumentTypeHeaderAttributes");
            CreateIndex("Web.SaleInvoiceSettings", "SalesTaxGroupId");
            CreateIndex("Web.SaleInvoiceLines", "SalesTaxGroupId");
            CreateIndex("Web.ChargeGroupSettings", "ChargableLedgerAccountId");
            CreateIndex("Web.ChargeGroupPersons", "ChargeTypeId");
            CreateIndex("Web.ChargeGroupProducts", "ChargeTypeId");
            AddForeignKey("Web.SaleInvoiceSettings", "SalesTaxGroupId", "Web.SalesTaxGroups", "SalesTaxGroupId");
            AddForeignKey("Web.SaleInvoiceLines", "SalesTaxGroupId", "Web.SalesTaxGroups", "SalesTaxGroupId");
            AddForeignKey("Web.ChargeGroupSettings", "ChargableLedgerAccountId", "Web.LedgerAccounts", "LedgerAccountId");
            AddForeignKey("Web.ChargeGroupPersons", "ChargeTypeId", "Web.ChargeTypes", "ChargeTypeId");
            AddForeignKey("Web.ChargeGroupProducts", "ChargeTypeId", "Web.ChargeTypes", "ChargeTypeId");
        }
    }
}
