namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChargeGroupSettings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Web.JobReceiveLines", "MachineId", "Web.Products");
            DropIndex("Web.JobReceiveLines", new[] { "MachineId" });
            CreateTable(
                "Web.ChargeGroupSettings",
                c => new
                    {
                        ChargeGroupSettingsId = c.Int(nullable: false, identity: true),
                        ChargeTypeId = c.Int(nullable: false),
                        ChargeGroupPersonId = c.Int(nullable: false),
                        ChargeGroupProductId = c.Int(nullable: false),
                        ChargableLedgerAccountId = c.Int(nullable: false),
                        ChargePer = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ChargeLedgerAccountId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ChargeGroupSettingsId)
                .ForeignKey("Web.LedgerAccounts", t => t.ChargableLedgerAccountId)
                .ForeignKey("Web.ChargeGroupPersons", t => t.ChargeGroupPersonId)
                .ForeignKey("Web.ChargeGroupProducts", t => t.ChargeGroupProductId)
                .ForeignKey("Web.LedgerAccounts", t => t.ChargeLedgerAccountId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.ChargeGroupPersonId)
                .Index(t => t.ChargeGroupProductId)
                .Index(t => t.ChargableLedgerAccountId)
                .Index(t => t.ChargeLedgerAccountId);
            
            AddColumn("Web.JobInvoiceLines", "DiscountAmt", c => c.Decimal(precision: 18, scale: 4));
            AddColumn("Web.JobReceiveLines", "ProductId", c => c.Int());
            AddColumn("Web.JobReceiveLines", "Dimension1Id", c => c.Int());
            AddColumn("Web.JobReceiveLines", "Dimension2Id", c => c.Int());
            AddColumn("Web.JobReceiveLines", "Dimension3Id", c => c.Int());
            AddColumn("Web.JobReceiveLines", "Dimension4Id", c => c.Int());
            AddColumn("Web.ProductTypeSettings", "ImportMenuId", c => c.Int());
            AddColumn("Web.SaleInvoiceSettings", "isVisibleCreditDays", c => c.Boolean());
            CreateIndex("Web.JobReceiveLines", "ProductId");
            CreateIndex("Web.JobReceiveLines", "Dimension1Id");
            CreateIndex("Web.JobReceiveLines", "Dimension2Id");
            CreateIndex("Web.JobReceiveLines", "Dimension3Id");
            CreateIndex("Web.JobReceiveLines", "Dimension4Id");
            CreateIndex("Web.ProductTypeSettings", "ImportMenuId");
            AddForeignKey("Web.JobReceiveLines", "Dimension1Id", "Web.Dimension1", "Dimension1Id");
            AddForeignKey("Web.JobReceiveLines", "Dimension2Id", "Web.Dimension2", "Dimension2Id");
            AddForeignKey("Web.JobReceiveLines", "Dimension3Id", "Web.Dimension3", "Dimension3Id");
            AddForeignKey("Web.JobReceiveLines", "Dimension4Id", "Web.Dimension4", "Dimension4Id");
            AddForeignKey("Web.ProductTypeSettings", "ImportMenuId", "Web.Menus", "MenuId");
            AddForeignKey("Web.JobReceiveLines", "ProductId", "Web.Products", "ProductId");
        }
        
        public override void Down()
        {
            DropForeignKey("Web.JobReceiveLines", "ProductId", "Web.Products");
            DropForeignKey("Web.ProductTypeSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.JobReceiveLines", "Dimension4Id", "Web.Dimension4");
            DropForeignKey("Web.JobReceiveLines", "Dimension3Id", "Web.Dimension3");
            DropForeignKey("Web.JobReceiveLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.JobReceiveLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ChargeGroupSettings", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.ChargeGroupSettings", "ChargeLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.ChargeGroupSettings", "ChargeGroupProductId", "Web.ChargeGroupProducts");
            DropForeignKey("Web.ChargeGroupSettings", "ChargeGroupPersonId", "Web.ChargeGroupPersons");
            DropForeignKey("Web.ChargeGroupSettings", "ChargableLedgerAccountId", "Web.LedgerAccounts");
            DropIndex("Web.ProductTypeSettings", new[] { "ImportMenuId" });
            DropIndex("Web.JobReceiveLines", new[] { "Dimension4Id" });
            DropIndex("Web.JobReceiveLines", new[] { "Dimension3Id" });
            DropIndex("Web.JobReceiveLines", new[] { "Dimension2Id" });
            DropIndex("Web.JobReceiveLines", new[] { "Dimension1Id" });
            DropIndex("Web.JobReceiveLines", new[] { "ProductId" });
            DropIndex("Web.ChargeGroupSettings", new[] { "ChargeLedgerAccountId" });
            DropIndex("Web.ChargeGroupSettings", new[] { "ChargableLedgerAccountId" });
            DropIndex("Web.ChargeGroupSettings", new[] { "ChargeGroupProductId" });
            DropIndex("Web.ChargeGroupSettings", new[] { "ChargeGroupPersonId" });
            DropIndex("Web.ChargeGroupSettings", new[] { "ChargeTypeId" });
            DropColumn("Web.SaleInvoiceSettings", "isVisibleCreditDays");
            DropColumn("Web.ProductTypeSettings", "ImportMenuId");
            DropColumn("Web.JobReceiveLines", "Dimension4Id");
            DropColumn("Web.JobReceiveLines", "Dimension3Id");
            DropColumn("Web.JobReceiveLines", "Dimension2Id");
            DropColumn("Web.JobReceiveLines", "Dimension1Id");
            DropColumn("Web.JobReceiveLines", "ProductId");
            DropColumn("Web.JobInvoiceLines", "DiscountAmt");
            DropTable("Web.ChargeGroupSettings");
            CreateIndex("Web.JobReceiveLines", "MachineId");
            AddForeignKey("Web.JobReceiveLines", "MachineId", "Web.Products", "ProductId");
        }
    }
}
