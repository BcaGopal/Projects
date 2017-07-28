namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HeaderAttributes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Web.SaleQuotationHeaderAttributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        DocumentTypeHeaderAttributeId = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.DocumentTypeHeaderAttributes", t => t.DocumentTypeHeaderAttributeId)
                .ForeignKey("Web.SaleQuotationHeaders", t => t.HeaderTableId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.DocumentTypeHeaderAttributeId);
            
            AddColumn("Web.ProductUids", "ProductUidSpecification1", c => c.String());
            AddColumn("Web.ProductSiteDetails", "ProcessId", c => c.Int());
            AddColumn("Web.ProductSiteDetails", "ExcessReceiveAllowedAgainstOrderQty", c => c.Decimal(precision: 18, scale: 4));
            AddColumn("Web.ProductSiteDetails", "ExcessReceiveAllowedAgainstOrderPer", c => c.Decimal(precision: 18, scale: 4));
            AddColumn("Web.SaleDispatchSettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.SaleEnquirySettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.SaleOrderSettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.SaleQuotationLines", "DiscountAmount", c => c.Decimal(precision: 18, scale: 4));
            AddColumn("Web.SaleQuotationLines", "SalesTaxGroupProductId", c => c.Int());
            AddColumn("Web.SaleQuotationSettings", "isVisibleSalesTaxGroupProduct", c => c.Boolean());
            AddColumn("Web.SaleQuotationSettings", "isVisibleDiscountPer", c => c.Boolean());
            AddColumn("Web.SaleQuotationSettings", "CalculateDiscountOnRate", c => c.Boolean());
            AddColumn("Web.SaleQuotationSettings", "DocumentPrintReportHeaderId", c => c.Int());
            AddColumn("Web.ViewRequisitionBalance", "Dimension3Id", c => c.Int());
            AddColumn("Web.ViewRequisitionBalance", "Dimension4Id", c => c.Int());
            AlterColumn("Web.SaleQuotationSettings", "isVisibleFromSaleEnquiry", c => c.Boolean());
            AlterColumn("Web.SaleQuotationSettings", "isVisibleAgent", c => c.Boolean());
            CreateIndex("Web.ProductSiteDetails", "ProcessId");
            CreateIndex("Web.SaleDispatchSettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.SaleEnquirySettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.SaleOrderSettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.SaleQuotationLines", "SalesTaxGroupProductId");
            CreateIndex("Web.SaleQuotationSettings", "DocumentPrintReportHeaderId");
            CreateIndex("Web.ViewRequisitionBalance", "Dimension3Id");
            CreateIndex("Web.ViewRequisitionBalance", "Dimension4Id");
            AddForeignKey("Web.ProductSiteDetails", "ProcessId", "Web.Processes", "ProcessId");
            AddForeignKey("Web.SaleDispatchSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.SaleEnquirySettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.SaleOrderSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.SaleQuotationLines", "SalesTaxGroupProductId", "Web.ChargeGroupProducts", "ChargeGroupProductId");
            AddForeignKey("Web.SaleQuotationSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders", "ReportHeaderId");
            AddForeignKey("Web.ViewRequisitionBalance", "Dimension3Id", "Web.Dimension3", "Dimension3Id");
            AddForeignKey("Web.ViewRequisitionBalance", "Dimension4Id", "Web.Dimension4", "Dimension4Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Web.ViewRequisitionBalance", "Dimension4Id", "Web.Dimension4");
            DropForeignKey("Web.ViewRequisitionBalance", "Dimension3Id", "Web.Dimension3");
            DropForeignKey("Web.SaleQuotationSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.SaleQuotationLines", "SalesTaxGroupProductId", "Web.ChargeGroupProducts");
            DropForeignKey("Web.SaleQuotationHeaderAttributes", "HeaderTableId", "Web.SaleQuotationHeaders");
            DropForeignKey("Web.SaleQuotationHeaderAttributes", "DocumentTypeHeaderAttributeId", "Web.DocumentTypeHeaderAttributes");
            DropForeignKey("Web.SaleOrderSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.SaleEnquirySettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.SaleDispatchSettings", "DocumentPrintReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.ProductSiteDetails", "ProcessId", "Web.Processes");
            DropIndex("Web.ViewRequisitionBalance", new[] { "Dimension4Id" });
            DropIndex("Web.ViewRequisitionBalance", new[] { "Dimension3Id" });
            DropIndex("Web.SaleQuotationSettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.SaleQuotationLines", new[] { "SalesTaxGroupProductId" });
            DropIndex("Web.SaleQuotationHeaderAttributes", new[] { "DocumentTypeHeaderAttributeId" });
            DropIndex("Web.SaleQuotationHeaderAttributes", new[] { "HeaderTableId" });
            DropIndex("Web.SaleOrderSettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.SaleEnquirySettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.SaleDispatchSettings", new[] { "DocumentPrintReportHeaderId" });
            DropIndex("Web.ProductSiteDetails", new[] { "ProcessId" });
            AlterColumn("Web.SaleQuotationSettings", "isVisibleAgent", c => c.Boolean(nullable: false));
            AlterColumn("Web.SaleQuotationSettings", "isVisibleFromSaleEnquiry", c => c.Boolean(nullable: false));
            DropColumn("Web.ViewRequisitionBalance", "Dimension4Id");
            DropColumn("Web.ViewRequisitionBalance", "Dimension3Id");
            DropColumn("Web.SaleQuotationSettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.SaleQuotationSettings", "CalculateDiscountOnRate");
            DropColumn("Web.SaleQuotationSettings", "isVisibleDiscountPer");
            DropColumn("Web.SaleQuotationSettings", "isVisibleSalesTaxGroupProduct");
            DropColumn("Web.SaleQuotationLines", "SalesTaxGroupProductId");
            DropColumn("Web.SaleQuotationLines", "DiscountAmount");
            DropColumn("Web.SaleOrderSettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.SaleEnquirySettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.SaleDispatchSettings", "DocumentPrintReportHeaderId");
            DropColumn("Web.ProductSiteDetails", "ExcessReceiveAllowedAgainstOrderPer");
            DropColumn("Web.ProductSiteDetails", "ExcessReceiveAllowedAgainstOrderQty");
            DropColumn("Web.ProductSiteDetails", "ProcessId");
            DropColumn("Web.ProductUids", "ProductUidSpecification1");
            DropTable("Web.SaleQuotationHeaderAttributes");
        }
    }
}
