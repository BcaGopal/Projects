namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleEnqquiry : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Web.MaterialPlanCancelForSaleOrders", "MaterialPlanForSaleOrderLineId", "Web.MaterialPlanForSaleOrderLines");
            DropIndex("Web.MaterialPlanCancelForSaleOrders", new[] { "MaterialPlanForSaleOrderLineId" });
            DropIndex("Web.MaterialPlanSettings", new[] { "DocTypePurchaseIndentId" });
            DropIndex("Web.MaterialPlanSettings", new[] { "DocTypeProductionOrderId" });
            CreateTable(
                "Web.SaleEnquiryHeaders",
                c => new
                    {
                        SaleEnquiryHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        BuyerEnquiryNo = c.String(maxLength: 20),
                        DueDate = c.DateTime(nullable: false),
                        ActualDueDate = c.DateTime(nullable: false),
                        SaleToBuyerId = c.Int(nullable: false),
                        BillToBuyerId = c.Int(nullable: false),
                        CurrencyId = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        ShipAddress = c.String(maxLength: 250),
                        DeliveryTermsId = c.Int(nullable: false),
                        TermsAndConditions = c.String(),
                        CreditDays = c.Int(nullable: false),
                        LedgerHeaderId = c.Int(),
                        StockHeaderId = c.Int(),
                        Status = c.Int(nullable: false),
                        UnitConversionForId = c.Byte(nullable: false),
                        Advance = c.Decimal(precision: 18, scale: 4),
                        Remark = c.String(),
                        LockReason = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleEnquiryHeaderId)
                .ForeignKey("Web.Buyers", t => t.BillToBuyerId)
                .ForeignKey("Web.Currencies", t => t.CurrencyId)
                .ForeignKey("Web.DeliveryTerms", t => t.DeliveryTermsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.LedgerHeaders", t => t.LedgerHeaderId)
                .ForeignKey("Web.People", t => t.SaleToBuyerId)
                .ForeignKey("Web.ShipMethods", t => t.ShipMethodId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.StockHeaders", t => t.StockHeaderId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.SaleToBuyerId)
                .Index(t => t.BillToBuyerId)
                .Index(t => t.CurrencyId)
                .Index(t => t.ShipMethodId)
                .Index(t => t.DeliveryTermsId)
                .Index(t => t.LedgerHeaderId)
                .Index(t => t.StockHeaderId)
                .Index(t => t.UnitConversionForId);
            
            CreateTable(
                "Web.SaleEnquiryLines",
                c => new
                    {
                        SaleEnquiryLineId = c.Int(nullable: false, identity: true),
                        SaleEnquiryHeaderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Specification = c.String(maxLength: 50),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DueDate = c.DateTime(),
                        DealUnitId = c.String(maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitConversionMultiplier = c.Decimal(precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DiscountPer = c.Decimal(precision: 18, scale: 4),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocLineId = c.Int(),
                        Remark = c.String(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleEnquiryLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.SaleEnquiryHeaders", t => t.SaleEnquiryHeaderId)
                .Index(t => new { t.SaleEnquiryHeaderId, t.ProductId, t.DueDate }, unique: true, name: "IX_SaleEnquiryLine_SaleOrdeHeaderProductDueDate")
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.DealUnitId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.SaleEnquiryLineExtendeds",
                c => new
                    {
                        SaleEnquiryLineId = c.Int(nullable: false),
                        ProductGroup = c.String(),
                        Size = c.String(),
                        ProductQuality = c.String(),
                        Colour = c.String(),
                    })
                .PrimaryKey(t => t.SaleEnquiryLineId)
                .ForeignKey("Web.JobOrderHeaders", t => t.SaleEnquiryLineId)
                .Index(t => t.SaleEnquiryLineId);
            
            CreateTable(
                "Web.ViewMaterialPlanForProdOrderBalance",
                c => new
                    {
                        MaterialPlanForProdOrderId = c.Int(nullable: false, identity: true),
                        MaterialPlanHeaderId = c.Int(nullable: false),
                        ProdOrderLineId = c.Int(nullable: false),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.MaterialPlanForProdOrderId);
            
            CreateTable(
                "Web.ViewMaterialPlanForProdOrderLineBalance",
                c => new
                    {
                        MaterialPlanForProdOrderLineId = c.Int(nullable: false, identity: true),
                        MaterialPlanForProdOrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProcessId = c.Int(),
                        MaterialPlanLineId = c.Int(),
                    })
                .PrimaryKey(t => t.MaterialPlanForProdOrderLineId);
            
            CreateTable(
                "Web.ViewMaterialPlanForSaleOrderBalance",
                c => new
                    {
                        MaterialPlanForSaleOrderId = c.Int(nullable: false, identity: true),
                        MaterialPlanHeaderId = c.Int(nullable: false),
                        SaleOrderLineId = c.Int(nullable: false),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        MaterialPlanLineId = c.Int(),
                    })
                .PrimaryKey(t => t.MaterialPlanForSaleOrderId);
            
            CreateTable(
                "Web.ViewSaleEnquiryBalance",
                c => new
                    {
                        SaleEnquiryLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SaleEnquiryHeaderId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SaleEnquiryNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        BuyerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SaleEnquiryLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.ProductId);
            
            AddColumn("Web.PackingLines", "SaleEnquiryLine_SaleEnquiryLineId", c => c.Int());
            AddColumn("Web.Stocks", "StockStatus", c => c.String(maxLength: 20));
            AddColumn("Web.SaleOrderHeaders", "StockHeaderId", c => c.Int());
            AddColumn("Web.JobReceiveQAHeaders", "StockHeaderId", c => c.Int());
            AddColumn("Web.JobReceiveQASettings", "isPostedInStock", c => c.Boolean());
            AddColumn("Web.MaterialPlanCancelForSaleOrders", "MaterialPlanForSaleOrderId", c => c.Int(nullable: false));
            AlterColumn("Web.ProductGroups", "Sr", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("Web.MaterialPlanSettings", "DocTypePurchaseIndentId", c => c.Int());
            AlterColumn("Web.MaterialPlanSettings", "DocTypeProductionOrderId", c => c.Int());
            CreateIndex("Web.PackingLines", "SaleEnquiryLine_SaleEnquiryLineId");
            CreateIndex("Web.SaleOrderHeaders", "StockHeaderId");
            CreateIndex("Web.JobReceiveQAHeaders", "StockHeaderId");
            CreateIndex("Web.MaterialPlanCancelForSaleOrders", "MaterialPlanForSaleOrderId");
            CreateIndex("Web.MaterialPlanSettings", "DocTypePurchaseIndentId");
            CreateIndex("Web.MaterialPlanSettings", "DocTypeProductionOrderId");
            AddForeignKey("Web.SaleOrderHeaders", "StockHeaderId", "Web.StockHeaders", "StockHeaderId");
            AddForeignKey("Web.JobReceiveQAHeaders", "StockHeaderId", "Web.StockHeaders", "StockHeaderId");
            AddForeignKey("Web.MaterialPlanCancelForSaleOrders", "MaterialPlanForSaleOrderId", "Web.MaterialPlanForSaleOrders", "MaterialPlanForSaleOrderId");
            AddForeignKey("Web.PackingLines", "SaleEnquiryLine_SaleEnquiryLineId", "Web.SaleEnquiryLines", "SaleEnquiryLineId");
            DropColumn("Web.MaterialPlanCancelForSaleOrders", "MaterialPlanForSaleOrderLineId");
            DropColumn("Web.ViewMaterialPlanBalance", "DocTypeName");
        }
        
        public override void Down()
        {
            AddColumn("Web.ViewMaterialPlanBalance", "DocTypeName", c => c.String());
            AddColumn("Web.MaterialPlanCancelForSaleOrders", "MaterialPlanForSaleOrderLineId", c => c.Int(nullable: false));
            DropForeignKey("Web.ViewSaleEnquiryBalance", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewSaleEnquiryBalance", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ViewSaleEnquiryBalance", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.SaleEnquiryLineExtendeds", "SaleEnquiryLineId", "Web.JobOrderHeaders");
            DropForeignKey("Web.SaleEnquiryHeaders", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.SaleEnquiryHeaders", "StockHeaderId", "Web.StockHeaders");
            DropForeignKey("Web.SaleEnquiryHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleEnquiryHeaders", "ShipMethodId", "Web.ShipMethods");
            DropForeignKey("Web.SaleEnquiryHeaders", "SaleToBuyerId", "Web.People");
            DropForeignKey("Web.SaleEnquiryLines", "SaleEnquiryHeaderId", "Web.SaleEnquiryHeaders");
            DropForeignKey("Web.SaleEnquiryLines", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleEnquiryLines", "ProductId", "Web.Products");
            DropForeignKey("Web.PackingLines", "SaleEnquiryLine_SaleEnquiryLineId", "Web.SaleEnquiryLines");
            DropForeignKey("Web.SaleEnquiryLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.SaleEnquiryLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.SaleEnquiryLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.SaleEnquiryHeaders", "LedgerHeaderId", "Web.LedgerHeaders");
            DropForeignKey("Web.SaleEnquiryHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleEnquiryHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleEnquiryHeaders", "DeliveryTermsId", "Web.DeliveryTerms");
            DropForeignKey("Web.SaleEnquiryHeaders", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.SaleEnquiryHeaders", "BillToBuyerId", "Web.Buyers");
            DropForeignKey("Web.MaterialPlanCancelForSaleOrders", "MaterialPlanForSaleOrderId", "Web.MaterialPlanForSaleOrders");
            DropForeignKey("Web.JobReceiveQAHeaders", "StockHeaderId", "Web.StockHeaders");
            DropForeignKey("Web.SaleOrderHeaders", "StockHeaderId", "Web.StockHeaders");
            DropIndex("Web.ViewSaleEnquiryBalance", new[] { "ProductId" });
            DropIndex("Web.ViewSaleEnquiryBalance", new[] { "Dimension2Id" });
            DropIndex("Web.ViewSaleEnquiryBalance", new[] { "Dimension1Id" });
            DropIndex("Web.SaleEnquiryLineExtendeds", new[] { "SaleEnquiryLineId" });
            DropIndex("Web.SaleEnquiryLines", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.SaleEnquiryLines", new[] { "DealUnitId" });
            DropIndex("Web.SaleEnquiryLines", new[] { "Dimension2Id" });
            DropIndex("Web.SaleEnquiryLines", new[] { "Dimension1Id" });
            DropIndex("Web.SaleEnquiryLines", "IX_SaleEnquiryLine_SaleOrdeHeaderProductDueDate");
            DropIndex("Web.SaleEnquiryHeaders", new[] { "UnitConversionForId" });
            DropIndex("Web.SaleEnquiryHeaders", new[] { "StockHeaderId" });
            DropIndex("Web.SaleEnquiryHeaders", new[] { "LedgerHeaderId" });
            DropIndex("Web.SaleEnquiryHeaders", new[] { "DeliveryTermsId" });
            DropIndex("Web.SaleEnquiryHeaders", new[] { "ShipMethodId" });
            DropIndex("Web.SaleEnquiryHeaders", new[] { "CurrencyId" });
            DropIndex("Web.SaleEnquiryHeaders", new[] { "BillToBuyerId" });
            DropIndex("Web.SaleEnquiryHeaders", new[] { "SaleToBuyerId" });
            DropIndex("Web.SaleEnquiryHeaders", new[] { "SiteId" });
            DropIndex("Web.SaleEnquiryHeaders", new[] { "DivisionId" });
            DropIndex("Web.SaleEnquiryHeaders", new[] { "DocTypeId" });
            DropIndex("Web.MaterialPlanSettings", new[] { "DocTypeProductionOrderId" });
            DropIndex("Web.MaterialPlanSettings", new[] { "DocTypePurchaseIndentId" });
            DropIndex("Web.MaterialPlanCancelForSaleOrders", new[] { "MaterialPlanForSaleOrderId" });
            DropIndex("Web.JobReceiveQAHeaders", new[] { "StockHeaderId" });
            DropIndex("Web.SaleOrderHeaders", new[] { "StockHeaderId" });
            DropIndex("Web.PackingLines", new[] { "SaleEnquiryLine_SaleEnquiryLineId" });
            AlterColumn("Web.MaterialPlanSettings", "DocTypeProductionOrderId", c => c.Int(nullable: false));
            AlterColumn("Web.MaterialPlanSettings", "DocTypePurchaseIndentId", c => c.Int(nullable: false));
            AlterColumn("Web.ProductGroups", "Sr", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            DropColumn("Web.MaterialPlanCancelForSaleOrders", "MaterialPlanForSaleOrderId");
            DropColumn("Web.JobReceiveQASettings", "isPostedInStock");
            DropColumn("Web.JobReceiveQAHeaders", "StockHeaderId");
            DropColumn("Web.SaleOrderHeaders", "StockHeaderId");
            DropColumn("Web.Stocks", "StockStatus");
            DropColumn("Web.PackingLines", "SaleEnquiryLine_SaleEnquiryLineId");
            DropTable("Web.ViewSaleEnquiryBalance");
            DropTable("Web.ViewMaterialPlanForSaleOrderBalance");
            DropTable("Web.ViewMaterialPlanForProdOrderLineBalance");
            DropTable("Web.ViewMaterialPlanForProdOrderBalance");
            DropTable("Web.SaleEnquiryLineExtendeds");
            DropTable("Web.SaleEnquiryLines");
            DropTable("Web.SaleEnquiryHeaders");
            CreateIndex("Web.MaterialPlanSettings", "DocTypeProductionOrderId");
            CreateIndex("Web.MaterialPlanSettings", "DocTypePurchaseIndentId");
            CreateIndex("Web.MaterialPlanCancelForSaleOrders", "MaterialPlanForSaleOrderLineId");
            AddForeignKey("Web.MaterialPlanCancelForSaleOrders", "MaterialPlanForSaleOrderLineId", "Web.MaterialPlanForSaleOrderLines", "MaterialPlanForSaleOrderLineId");
        }
    }
}
