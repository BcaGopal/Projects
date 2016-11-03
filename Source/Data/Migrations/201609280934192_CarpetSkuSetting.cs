//namespace Data.Migrations
//{
//    using System;
//    using System.Data.Entity.Migrations;
    
//    public partial class CarpetSkuSetting : DbMigration                                    
//    {
//        public override void Up()
//        {
//            DropIndex("Web.SaleEnquiryLines", "IX_SaleEnquiryLine_SaleOrdeHeaderProductDueDate");
//            CreateTable(
//                "Web.CarpetSkuSettings",
//                c => new
//                    {
//                        CarpetSkuSettingsId = c.Int(nullable: false, identity: true),
//                        isVisibleColourWays = c.Boolean(),
//                        isVisibleStyle = c.Boolean(),
//                        isVisibleDesigner = c.Boolean(),
//                        isVisibleDesignPattern = c.Boolean(),
//                        isVisibleContent = c.Boolean(),
//                        isVisibleOrigin = c.Boolean(),
//                        isVisibleInvoiceGroup = c.Boolean(),
//                        isVisibleDrawbackTarrif = c.Boolean(),
//                        isVisibleStandardCost = c.Boolean(),
//                        isVisibleFinishedWeight = c.Boolean(),
//                        isVisibleSupplierDetail = c.Boolean(),
//                        isVisibleLeadTime = c.Boolean(),
//                        isVisibleMinQty = c.Boolean(),
//                        isVisibleMaxQty = c.Boolean(),
//                        isVisibleSample = c.Boolean(),
//                        isVisibleCounterNo = c.Boolean(),
//                        isVisibleTags = c.Boolean(),
//                        isVisibleDivision = c.Boolean(),
//                        CreatedBy = c.String(),
//                        ModifiedBy = c.String(),
//                        CreatedDate = c.DateTime(nullable: false),
//                        ModifiedDate = c.DateTime(nullable: false),
//                        OMSId = c.String(maxLength: 50),
//                    })
//                .PrimaryKey(t => t.CarpetSkuSettingsId);
            
//            CreateTable(
//                "Web.ViewProductBuyer",
//                c => new
//                    {
//                        ProductId = c.Int(nullable: false, identity: true),
//                        ProductName = c.String(),
//                        ProductGroup = c.String(),
//                        Size = c.String(),
//                        Colour = c.String(),
//                        ProductQuality = c.String(),
//                        BuyerId = c.Int(nullable: false),
//                    })
//                .PrimaryKey(t => t.ProductId);
            
//            AddColumn("Web.MaterialPlanSettings", "PlanType", c => c.String(nullable: false));
//            AddColumn("Web.SaleEnquiryLines", "UnitId", c => c.String(maxLength: 3));
//            AlterColumn("Web.SaleEnquiryLines", "ProductId", c => c.Int());
//            CreateIndex("Web.SaleEnquiryLines", new[] { "SaleEnquiryHeaderId", "ProductId", "DueDate" }, unique: true, name: "IX_SaleEnquiryLine_SaleOrdeHeaderProductDueDate");
//            CreateIndex("Web.SaleEnquiryLines", "UnitId");
//            AddForeignKey("Web.SaleEnquiryLines", "UnitId", "Web.Units", "UnitId");
//        }
        
//        public override void Down()
//        {
//            DropForeignKey("Web.SaleEnquiryLines", "UnitId", "Web.Units");
//            DropIndex("Web.SaleEnquiryLines", new[] { "UnitId" });
//            DropIndex("Web.SaleEnquiryLines", "IX_SaleEnquiryLine_SaleOrdeHeaderProductDueDate");
//            AlterColumn("Web.SaleEnquiryLines", "ProductId", c => c.Int(nullable: false));
//            DropColumn("Web.SaleEnquiryLines", "UnitId");
//            DropColumn("Web.MaterialPlanSettings", "PlanType");
//            DropTable("Web.ViewProductBuyer");
//            DropTable("Web.CarpetSkuSettings");
//            CreateIndex("Web.SaleEnquiryLines", new[] { "SaleEnquiryHeaderId", "ProductId", "DueDate" }, unique: true, name: "IX_SaleEnquiryLine_SaleOrdeHeaderProductDueDate");
//        }
//    }
//}
