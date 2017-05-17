namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleInvoiceHeaderAttributes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Web.DocumentTypeAttributes",
                c => new
                    {
                        DocumentTypeAttributeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsMandatory = c.Boolean(nullable: false),
                        DataType = c.String(),
                        ListItem = c.String(),
                        DefaultValue = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        DocumentTypeId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DocumentTypeAttributeId)
                .ForeignKey("Web.DocumentTypes", t => t.DocumentTypeId)
                .Index(t => t.DocumentTypeId);
            
            CreateTable(
                "Web.SaleInvoiceHeaderAttributes",
                c => new
                    {
                        SaleInvoiceHeaderAttributeId = c.Int(nullable: false, identity: true),
                        SaleInvoiceHeaderId = c.Int(nullable: false),
                        DocumentTypeAttributeId = c.Int(nullable: false),
                        SaleInvoiceHeaderAttributeValue = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleInvoiceHeaderAttributeId)
                .ForeignKey("Web.DocumentTypeAttributes", t => t.DocumentTypeAttributeId)
                .ForeignKey("Web.SaleInvoiceHeaders", t => t.SaleInvoiceHeaderId)
                .Index(t => t.SaleInvoiceHeaderId)
                .Index(t => t.DocumentTypeAttributeId);
            
            AddColumn("Web.StockLines", "ReferenceDocTypeId", c => c.Int());
            AddColumn("Web.StockLines", "ReferenceDocId", c => c.Int());
            AddColumn("Web.StockLines", "ReferenceDocLineId", c => c.Int());
            AddColumn("Web.StockHeaderSettings", "isVisibleProcessHeader", c => c.Boolean());
            AddColumn("Web.StockHeaderSettings", "isVisibleReferenceDocId", c => c.Boolean());
            AddColumn("Web.StockHeaderSettings", "SqlProcHelpListReferenceDocId", c => c.String(maxLength: 100));
            CreateIndex("Web.StockLines", "ReferenceDocTypeId");
            AddForeignKey("Web.StockLines", "ReferenceDocTypeId", "Web.DocumentTypes", "DocumentTypeId");
        }
        
        public override void Down()
        {
            DropForeignKey("Web.SaleInvoiceHeaderAttributes", "SaleInvoiceHeaderId", "Web.SaleInvoiceHeaders");
            DropForeignKey("Web.SaleInvoiceHeaderAttributes", "DocumentTypeAttributeId", "Web.DocumentTypeAttributes");
            DropForeignKey("Web.DocumentTypeAttributes", "DocumentTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.StockLines", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropIndex("Web.SaleInvoiceHeaderAttributes", new[] { "DocumentTypeAttributeId" });
            DropIndex("Web.SaleInvoiceHeaderAttributes", new[] { "SaleInvoiceHeaderId" });
            DropIndex("Web.DocumentTypeAttributes", new[] { "DocumentTypeId" });
            DropIndex("Web.StockLines", new[] { "ReferenceDocTypeId" });
            DropColumn("Web.StockHeaderSettings", "SqlProcHelpListReferenceDocId");
            DropColumn("Web.StockHeaderSettings", "isVisibleReferenceDocId");
            DropColumn("Web.StockHeaderSettings", "isVisibleProcessHeader");
            DropColumn("Web.StockLines", "ReferenceDocLineId");
            DropColumn("Web.StockLines", "ReferenceDocId");
            DropColumn("Web.StockLines", "ReferenceDocTypeId");
            DropTable("Web.SaleInvoiceHeaderAttributes");
            DropTable("Web.DocumentTypeAttributes");
        }
    }
}
