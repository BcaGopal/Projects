namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobReceiveQA : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Web.FeetConversionToCms",
                c => new
                    {
                        FeetConversionToCmsId = c.Int(nullable: false, identity: true),
                        Feet = c.Int(nullable: false),
                        Inch = c.Int(nullable: false),
                        Cms = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.FeetConversionToCmsId);
            
            CreateTable(
                "Web.JobReceiveQAAttributes",
                c => new
                    {
                        JobReceiveQAAttributeId = c.Int(nullable: false, identity: true),
                        JobReceiveQALineId = c.Int(nullable: false),
                        QAGroupLineId = c.Int(nullable: false),
                        Value = c.String(),
                        Remarks = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobReceiveQAAttributeId)
                .ForeignKey("Web.JobReceiveQALines", t => t.JobReceiveQALineId)
                .ForeignKey("Web.QAGroupLines", t => t.QAGroupLineId)
                .Index(t => t.JobReceiveQALineId)
                .Index(t => t.QAGroupLineId);
            
            CreateTable(
                "Web.QAGroupLines",
                c => new
                    {
                        QAGroupLineId = c.Int(nullable: false, identity: true),
                        QAGroupId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        IsMandatory = c.Boolean(nullable: false),
                        DataType = c.String(),
                        ListItem = c.String(),
                        DefaultValue = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.QAGroupLineId)
                .ForeignKey("Web.QAGroups", t => t.QAGroupId)
                .Index(t => t.QAGroupId);
            
            CreateTable(
                "Web.QAGroups",
                c => new
                    {
                        QAGroupId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        QaGroupName = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.QAGroupId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .Index(t => new { t.DocTypeId, t.QaGroupName }, unique: true, name: "IX_QAGroup_Name");
            
            CreateTable(
                "Web.JobReceiveQALineDetails",
                c => new
                    {
                        JobReceiveQALineId = c.Int(nullable: false),
                        Length = c.Decimal(precision: 18, scale: 4),
                        Width = c.Decimal(precision: 18, scale: 4),
                        Height = c.Decimal(precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.JobReceiveQALineId)
                .ForeignKey("Web.JobReceiveQALines", t => t.JobReceiveQALineId)
                .Index(t => t.JobReceiveQALineId);
            
            CreateTable(
                "Web.JobReceiveQAPenalties",
                c => new
                    {
                        JobReceiveQAPenaltyId = c.Int(nullable: false, identity: true),
                        Sr = c.Int(nullable: false),
                        JobReceiveQALineId = c.Int(nullable: false),
                        ReasonId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remarks = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobReceiveQAPenaltyId)
                .ForeignKey("Web.JobReceiveQALines", t => t.JobReceiveQALineId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .Index(t => t.JobReceiveQALineId)
                .Index(t => t.ReasonId);
            
            CreateTable(
                "Web.ProductBuyerSettings",
                c => new
                    {
                        ProductBuyerSettingsId = c.Int(nullable: false, identity: true),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        BuyerSpecificationDisplayName = c.String(maxLength: 50),
                        BuyerSpecification1DisplayName = c.String(maxLength: 50),
                        BuyerSpecification2DisplayName = c.String(maxLength: 50),
                        BuyerSpecification3DisplayName = c.String(maxLength: 50),
                        BuyerSpecification4DisplayName = c.String(maxLength: 50),
                        BuyerSpecification5DisplayName = c.String(maxLength: 50),
                        BuyerSpecification6DisplayName = c.String(maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductBuyerSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.ViewDesignColourConsumption",
                c => new
                    {
                        ProductGroupId = c.Int(nullable: false, identity: true),
                        ProductGroupName = c.String(),
                        ColourId = c.Int(nullable: false),
                        ColourName = c.String(),
                        ProductQualityId = c.Int(nullable: false),
                        ProductQualityName = c.String(),
                        Weight = c.Decimal(precision: 18, scale: 4),
                        BomProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductGroupId);
            
            AddColumn("Web.Products", "GrossWeight", c => c.Decimal(precision: 18, scale: 4));
            AddColumn("Web.JobOrderBoms", "BOMQty", c => c.Decimal(precision: 18, scale: 4));
            AddColumn("Web.ProductBuyers", "BuyerProductCode", c => c.String(maxLength: 50));
            AddColumn("Web.PurchaseIndentCancelHeaders", "MaterialPlanCancelHeaderId", c => c.Int());
            AddColumn("Web.CarpetSkuSettings", "isVisibleGrossWeight", c => c.Boolean());
            AddColumn("Web.Companies", "TitleCase", c => c.String(maxLength: 20));
            AddColumn("Web.JobReceiveLines", "ProductUidHeaderId", c => c.Int());
            AddColumn("Web.JobReceiveQASettings", "DocTypeProductionOrderId", c => c.Int());
            AddColumn("Web.Ledgers", "ProductUidId", c => c.Int());
            AddColumn("Web.LedgerLines", "ProductUidId", c => c.Int());
            AddColumn("Web.LedgerSettings", "isVisibleProductUid", c => c.Boolean());
            AddColumn("Web.MaterialPlanSettings", "WizardMenuId", c => c.Int());
            AddColumn("Web.ProdOrderCancelHeaders", "MaterialPlanCancelHeaderId", c => c.Int());
            AddColumn("Web.ProductProcesses", "QAGroupId", c => c.Int());
            AddColumn("Web.SaleDispatchSettings", "isVisibleStockIn", c => c.Boolean());
            AddColumn("Web.SaleDispatchSettings", "IsMandatoryStockIn", c => c.Boolean());
            AddColumn("Web.ViewSaleDispatchBalance", "Sr", c => c.Int());
            CreateIndex("Web.PurchaseIndentCancelHeaders", "MaterialPlanCancelHeaderId");
            CreateIndex("Web.JobReceiveLines", "ProductUidHeaderId");
            CreateIndex("Web.Ledgers", "ProductUidId");
            CreateIndex("Web.LedgerLines", "ProductUidId");
            CreateIndex("Web.MaterialPlanSettings", "WizardMenuId");
            CreateIndex("Web.ProdOrderCancelHeaders", "MaterialPlanCancelHeaderId");
            CreateIndex("Web.ProductProcesses", "QAGroupId");
            AddForeignKey("Web.PurchaseIndentCancelHeaders", "MaterialPlanCancelHeaderId", "Web.MaterialPlanCancelHeaders", "MaterialPlanCancelHeaderId");
            AddForeignKey("Web.JobReceiveLines", "ProductUidHeaderId", "Web.ProductUidHeaders", "ProductUidHeaderId");
            AddForeignKey("Web.LedgerLines", "ProductUidId", "Web.ProductUids", "ProductUIDId");
            AddForeignKey("Web.Ledgers", "ProductUidId", "Web.ProductUids", "ProductUIDId");
            AddForeignKey("Web.MaterialPlanSettings", "WizardMenuId", "Web.Menus", "MenuId");
            AddForeignKey("Web.ProdOrderCancelHeaders", "MaterialPlanCancelHeaderId", "Web.MaterialPlanCancelHeaders", "MaterialPlanCancelHeaderId");
            AddForeignKey("Web.ProductProcesses", "QAGroupId", "Web.QAGroups", "QAGroupId");
            DropTable("Web.UserReferrals");
        }
        
        public override void Down()
        {
            CreateTable(
                "Web.UserReferrals",
                c => new
                    {
                        UserReferralId = c.Guid(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserReferralId);
            
            DropForeignKey("Web.ProductProcesses", "QAGroupId", "Web.QAGroups");
            DropForeignKey("Web.ProductBuyerSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.ProductBuyerSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ProdOrderCancelHeaders", "MaterialPlanCancelHeaderId", "Web.MaterialPlanCancelHeaders");
            DropForeignKey("Web.MaterialPlanSettings", "WizardMenuId", "Web.Menus");
            DropForeignKey("Web.Ledgers", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.LedgerLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.JobReceiveQAPenalties", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.JobReceiveQAPenalties", "JobReceiveQALineId", "Web.JobReceiveQALines");
            DropForeignKey("Web.JobReceiveQALineDetails", "JobReceiveQALineId", "Web.JobReceiveQALines");
            DropForeignKey("Web.JobReceiveQAAttributes", "QAGroupLineId", "Web.QAGroupLines");
            DropForeignKey("Web.QAGroupLines", "QAGroupId", "Web.QAGroups");
            DropForeignKey("Web.QAGroups", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobReceiveQAAttributes", "JobReceiveQALineId", "Web.JobReceiveQALines");
            DropForeignKey("Web.JobReceiveLines", "ProductUidHeaderId", "Web.ProductUidHeaders");
            DropForeignKey("Web.PurchaseIndentCancelHeaders", "MaterialPlanCancelHeaderId", "Web.MaterialPlanCancelHeaders");
            DropIndex("Web.ProductProcesses", new[] { "QAGroupId" });
            DropIndex("Web.ProductBuyerSettings", new[] { "DivisionId" });
            DropIndex("Web.ProductBuyerSettings", new[] { "SiteId" });
            DropIndex("Web.ProdOrderCancelHeaders", new[] { "MaterialPlanCancelHeaderId" });
            DropIndex("Web.MaterialPlanSettings", new[] { "WizardMenuId" });
            DropIndex("Web.LedgerLines", new[] { "ProductUidId" });
            DropIndex("Web.Ledgers", new[] { "ProductUidId" });
            DropIndex("Web.JobReceiveQAPenalties", new[] { "ReasonId" });
            DropIndex("Web.JobReceiveQAPenalties", new[] { "JobReceiveQALineId" });
            DropIndex("Web.JobReceiveQALineDetails", new[] { "JobReceiveQALineId" });
            DropIndex("Web.QAGroups", "IX_QAGroup_Name");
            DropIndex("Web.QAGroupLines", new[] { "QAGroupId" });
            DropIndex("Web.JobReceiveQAAttributes", new[] { "QAGroupLineId" });
            DropIndex("Web.JobReceiveQAAttributes", new[] { "JobReceiveQALineId" });
            DropIndex("Web.JobReceiveLines", new[] { "ProductUidHeaderId" });
            DropIndex("Web.PurchaseIndentCancelHeaders", new[] { "MaterialPlanCancelHeaderId" });
            DropColumn("Web.ViewSaleDispatchBalance", "Sr");
            DropColumn("Web.SaleDispatchSettings", "IsMandatoryStockIn");
            DropColumn("Web.SaleDispatchSettings", "isVisibleStockIn");
            DropColumn("Web.ProductProcesses", "QAGroupId");
            DropColumn("Web.ProdOrderCancelHeaders", "MaterialPlanCancelHeaderId");
            DropColumn("Web.MaterialPlanSettings", "WizardMenuId");
            DropColumn("Web.LedgerSettings", "isVisibleProductUid");
            DropColumn("Web.LedgerLines", "ProductUidId");
            DropColumn("Web.Ledgers", "ProductUidId");
            DropColumn("Web.JobReceiveQASettings", "DocTypeProductionOrderId");
            DropColumn("Web.JobReceiveLines", "ProductUidHeaderId");
            DropColumn("Web.Companies", "TitleCase");
            DropColumn("Web.CarpetSkuSettings", "isVisibleGrossWeight");
            DropColumn("Web.PurchaseIndentCancelHeaders", "MaterialPlanCancelHeaderId");
            DropColumn("Web.ProductBuyers", "BuyerProductCode");
            DropColumn("Web.JobOrderBoms", "BOMQty");
            DropColumn("Web.Products", "GrossWeight");
            DropTable("Web.ViewDesignColourConsumption");
            DropTable("Web.ProductBuyerSettings");
            DropTable("Web.JobReceiveQAPenalties");
            DropTable("Web.JobReceiveQALineDetails");
            DropTable("Web.QAGroups");
            DropTable("Web.QAGroupLines");
            DropTable("Web.JobReceiveQAAttributes");
            DropTable("Web.FeetConversionToCms");
        }
    }
}
