namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attributes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Web.JobInvoiceHeaderAttributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        DocumentTypeHeaderAttributeId = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.DocumentTypeHeaderAttributes", t => t.DocumentTypeHeaderAttributeId)
                .ForeignKey("Web.JobInvoiceHeaders", t => t.HeaderTableId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.DocumentTypeHeaderAttributeId);
            
            CreateTable(
                "Web.StockHeaderAttributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        DocumentTypeHeaderAttributeId = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.DocumentTypeHeaderAttributes", t => t.DocumentTypeHeaderAttributeId)
                .ForeignKey("Web.StockHeaders", t => t.HeaderTableId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.DocumentTypeHeaderAttributeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Web.StockHeaderAttributes", "HeaderTableId", "Web.StockHeaders");
            DropForeignKey("Web.StockHeaderAttributes", "DocumentTypeHeaderAttributeId", "Web.DocumentTypeHeaderAttributes");
            DropForeignKey("Web.JobInvoiceHeaderAttributes", "HeaderTableId", "Web.JobInvoiceHeaders");
            DropForeignKey("Web.JobInvoiceHeaderAttributes", "DocumentTypeHeaderAttributeId", "Web.DocumentTypeHeaderAttributes");
            DropIndex("Web.StockHeaderAttributes", new[] { "DocumentTypeHeaderAttributeId" });
            DropIndex("Web.StockHeaderAttributes", new[] { "HeaderTableId" });
            DropIndex("Web.JobInvoiceHeaderAttributes", new[] { "DocumentTypeHeaderAttributeId" });
            DropIndex("Web.JobInvoiceHeaderAttributes", new[] { "HeaderTableId" });
            DropTable("Web.StockHeaderAttributes");
            DropTable("Web.JobInvoiceHeaderAttributes");
        }
    }
}
