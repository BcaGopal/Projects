namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleInvoiceLineDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Web.SaleInvoiceLineDetails",
                c => new
                    {
                        SaleInvoiceLineId = c.Int(nullable: false),
                        RewardPoints = c.Decimal(precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.SaleInvoiceLineId)
                .ForeignKey("Web.SaleInvoiceLines", t => t.SaleInvoiceLineId)
                .Index(t => t.SaleInvoiceLineId);
            
            AddColumn("Web.PackingLines", "FreeQty", c => c.Decimal(precision: 18, scale: 4));
            AddColumn("Web.SaleOrderLines", "FreeQty", c => c.Decimal(precision: 18, scale: 4));
            AddColumn("Web.ImportHeaders", "FileType", c => c.String());
            AddColumn("Web.ImportLines", "MaxLength", c => c.Int());
            AddColumn("Web.SaleDispatchSettings", "isVisibleFreeQty", c => c.Boolean());
            AddColumn("Web.SaleInvoiceSettings", "isVisibleFreeQty", c => c.Boolean());
            AddColumn("Web.SaleInvoiceSettings", "isVisibleRewardPoints", c => c.Boolean());
            DropColumn("Web.ImportHeaders", "Controller");
            DropColumn("Web.ImportHeaders", "Action");
            DropColumn("Web.ImportLines", "ServiceFuncGet");
            DropColumn("Web.ImportLines", "ServiceFuncSet");
            DropColumn("Web.ImportLines", "SqlProcGet");
            DropColumn("Web.ImportLines", "SqlProcSet");
            DropColumn("Web.ImportLines", "CacheKey");
        }
        
        public override void Down()
        {
            AddColumn("Web.ImportLines", "CacheKey", c => c.String());
            AddColumn("Web.ImportLines", "SqlProcSet", c => c.String());
            AddColumn("Web.ImportLines", "SqlProcGet", c => c.String());
            AddColumn("Web.ImportLines", "ServiceFuncSet", c => c.String());
            AddColumn("Web.ImportLines", "ServiceFuncGet", c => c.String());
            AddColumn("Web.ImportHeaders", "Action", c => c.String());
            AddColumn("Web.ImportHeaders", "Controller", c => c.String());
            DropForeignKey("Web.SaleInvoiceLineDetails", "SaleInvoiceLineId", "Web.SaleInvoiceLines");
            DropIndex("Web.SaleInvoiceLineDetails", new[] { "SaleInvoiceLineId" });
            DropColumn("Web.SaleInvoiceSettings", "isVisibleRewardPoints");
            DropColumn("Web.SaleInvoiceSettings", "isVisibleFreeQty");
            DropColumn("Web.SaleDispatchSettings", "isVisibleFreeQty");
            DropColumn("Web.ImportLines", "MaxLength");
            DropColumn("Web.ImportHeaders", "FileType");
            DropColumn("Web.SaleOrderLines", "FreeQty");
            DropColumn("Web.PackingLines", "FreeQty");
            DropTable("Web.SaleInvoiceLineDetails");
        }
    }
}
