namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImportLayout : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Web.ViewSaleEnquiryBalance", "Dimension3Id", "Web.Dimension3");
            DropForeignKey("Web.ViewSaleEnquiryBalance", "Dimension4Id", "Web.Dimension4");
            DropForeignKey("Web.ViewSaleEnquiryBalance", "DocType_DocumentTypeId", "Web.DocumentTypes");
            DropIndex("Web.SaleQuotationHeaders", new[] { "ParentSaleQuotationHeaderId" });
            DropIndex("Web.ViewSaleEnquiryBalance", new[] { "Dimension3Id" });
            DropIndex("Web.ViewSaleEnquiryBalance", new[] { "Dimension4Id" });
            DropIndex("Web.ViewSaleEnquiryBalance", new[] { "DocType_DocumentTypeId" });
            DropPrimaryKey("Web.SaleQuotationHeaderDetails");
            CreateTable(
                "Web.ImportHeaders",
                c => new
                    {
                        ImportHeaderId = c.Int(nullable: false, identity: true),
                        ImportName = c.String(),
                        Controller = c.String(),
                        Action = c.String(),
                        SqlProc = c.String(),
                        Notes = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ImportHeaderId);
            
            CreateTable(
                "Web.ImportLines",
                c => new
                    {
                        ImportLineId = c.Int(nullable: false, identity: true),
                        ImportHeaderId = c.Int(nullable: false),
                        DisplayName = c.String(nullable: false),
                        FieldName = c.String(nullable: false),
                        DataType = c.String(nullable: false),
                        Type = c.String(nullable: false),
                        ListItem = c.String(),
                        DefaultValue = c.String(),
                        IsVisible = c.Boolean(nullable: false),
                        ServiceFuncGet = c.String(),
                        ServiceFuncSet = c.String(),
                        SqlProcGetSet = c.String(maxLength: 100),
                        SqlProcGet = c.String(),
                        SqlProcSet = c.String(),
                        CacheKey = c.String(),
                        Serial = c.Int(nullable: false),
                        NoOfCharToEnter = c.Int(),
                        SqlParameter = c.String(),
                        IsCollapse = c.Boolean(nullable: false),
                        IsMandatory = c.Boolean(nullable: false),
                        PlaceHolder = c.String(),
                        ToolTip = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ImportLineId)
                .ForeignKey("Web.ImportHeaders", t => t.ImportHeaderId)
                .Index(t => t.ImportHeaderId);
            
            CreateTable(
                "Web.ViewSaleEnquiryBalanceForQuotation",
                c => new
                    {
                        SaleEnquiryLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SaleEnquiryHeaderId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Dimension3Id = c.Int(),
                        Dimension4Id = c.Int(),
                        DocTypeId = c.Int(),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SaleEnquiryNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        BuyerId = c.Int(nullable: false),
                        EnquiryDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SaleEnquiryLineId);
            
            AddColumn("Web.JobOrderSettings", "isVisibleProcessHeader", c => c.Boolean());
            AlterColumn("Web.LedgerAccountGroups", "OMSId", c => c.String());
            AlterColumn("Web.SaleQuotationHeaders", "ParentSaleQuotationHeaderId", c => c.Int());
            AlterColumn("Web.SaleQuotationHeaderDetails", "SaleQuotationHeaderId", c => c.Int(nullable: false));
            AddPrimaryKey("Web.SaleQuotationHeaderDetails", "SaleQuotationHeaderId");
            CreateIndex("Web.SaleQuotationHeaders", "ParentSaleQuotationHeaderId");
            CreateIndex("Web.SaleQuotationHeaderDetails", "SaleQuotationHeaderId");
            AddForeignKey("Web.SaleQuotationHeaderDetails", "SaleQuotationHeaderId", "Web.SaleQuotationHeaders", "SaleQuotationHeaderId");
            DropColumn("Web.ViewSaleEnquiryBalance", "Dimension3Id");
            DropColumn("Web.ViewSaleEnquiryBalance", "Dimension4Id");
            DropColumn("Web.ViewSaleEnquiryBalance", "DocTypeId");
            DropColumn("Web.ViewSaleEnquiryBalance", "DocType_DocumentTypeId");
        }
        
        public override void Down()
        {
            AddColumn("Web.ViewSaleEnquiryBalance", "DocType_DocumentTypeId", c => c.Int());
            AddColumn("Web.ViewSaleEnquiryBalance", "DocTypeId", c => c.Int());
            AddColumn("Web.ViewSaleEnquiryBalance", "Dimension4Id", c => c.Int());
            AddColumn("Web.ViewSaleEnquiryBalance", "Dimension3Id", c => c.Int());
            DropForeignKey("Web.SaleQuotationHeaderDetails", "SaleQuotationHeaderId", "Web.SaleQuotationHeaders");
            DropForeignKey("Web.ImportLines", "ImportHeaderId", "Web.ImportHeaders");
            DropIndex("Web.SaleQuotationHeaderDetails", new[] { "SaleQuotationHeaderId" });
            DropIndex("Web.SaleQuotationHeaders", new[] { "ParentSaleQuotationHeaderId" });
            DropIndex("Web.ImportLines", new[] { "ImportHeaderId" });
            DropPrimaryKey("Web.SaleQuotationHeaderDetails");
            AlterColumn("Web.SaleQuotationHeaderDetails", "SaleQuotationHeaderId", c => c.Int(nullable: false, identity: true));
            AlterColumn("Web.SaleQuotationHeaders", "ParentSaleQuotationHeaderId", c => c.Int(nullable: false));
            AlterColumn("Web.LedgerAccountGroups", "OMSId", c => c.String(maxLength: 50));
            DropColumn("Web.JobOrderSettings", "isVisibleProcessHeader");
            DropTable("Web.ViewSaleEnquiryBalanceForQuotation");
            DropTable("Web.ImportLines");
            DropTable("Web.ImportHeaders");
            AddPrimaryKey("Web.SaleQuotationHeaderDetails", "SaleQuotationHeaderId");
            CreateIndex("Web.ViewSaleEnquiryBalance", "DocType_DocumentTypeId");
            CreateIndex("Web.ViewSaleEnquiryBalance", "Dimension4Id");
            CreateIndex("Web.ViewSaleEnquiryBalance", "Dimension3Id");
            CreateIndex("Web.SaleQuotationHeaders", "ParentSaleQuotationHeaderId");
            AddForeignKey("Web.ViewSaleEnquiryBalance", "DocType_DocumentTypeId", "Web.DocumentTypes", "DocumentTypeId");
            AddForeignKey("Web.ViewSaleEnquiryBalance", "Dimension4Id", "Web.Dimension4", "Dimension4Id");
            AddForeignKey("Web.ViewSaleEnquiryBalance", "Dimension3Id", "Web.Dimension3", "Dimension3Id");
        }
    }
}
