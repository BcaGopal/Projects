namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanySettings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Web.PackingLines", "SaleQuotationLine_SaleQuotationLineId", "Web.SaleQuotationLines");
            DropIndex("Web.PackingLines", new[] { "SaleQuotationLine_SaleQuotationLineId" });
            DropColumn("Web.PackingLines", "SaleQuotationLine_SaleQuotationLineId");
        }
        
        public override void Down()
        {
            AddColumn("Web.PackingLines", "SaleQuotationLine_SaleQuotationLineId", c => c.Int());
            CreateIndex("Web.PackingLines", "SaleQuotationLine_SaleQuotationLineId");
            AddForeignKey("Web.PackingLines", "SaleQuotationLine_SaleQuotationLineId", "Web.SaleQuotationLines", "SaleQuotationLineId");
        }
    }
}
