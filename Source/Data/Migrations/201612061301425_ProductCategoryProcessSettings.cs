namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductCategoryProcessSettings : DbMigration
    {
        public override void Up()
        {
            DropIndex("Web.SaleEnquiryHeaders", new[] { "DocTypeId" });
            DropIndex("Web.SaleEnquiryHeaders", new[] { "DivisionId" });
            DropIndex("Web.SaleEnquiryHeaders", new[] { "SiteId" });
            CreateTable(
                "Web.ProductCategoryProcessSettings",
                c => new
                    {
                        ProductCategoryProcessSettingsId = c.Int(nullable: false, identity: true),
                        ProductCategoryId = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        QAGroupId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductCategoryProcessSettingsId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.ProductCategories", t => t.ProductCategoryId)
                .ForeignKey("Web.QAGroups", t => t.QAGroupId)
                .Index(t => new { t.ProductCategoryId, t.ProcessId }, unique: true, name: "IX_ProductCategory_DocID")
                .Index(t => t.QAGroupId);
            
            AddColumn("Web.MaterialPlanHeaders", "MachineId", c => c.Int());
            AddColumn("Web.PurchaseIndentCancelLines", "MaterialPlanCancelLineId", c => c.Int());
            AddColumn("Web.CarpetSkuSettings", "isVisibleMapScale", c => c.Boolean());
            AddColumn("Web.CarpetSkuSettings", "isVisibleCBM", c => c.Boolean());
            AddColumn("Web.JobReceiveSettings", "IsVisibleIncentive", c => c.Boolean());
            AddColumn("Web.JobReceiveSettings", "IsVisiblePenalty", c => c.Boolean());
            AddColumn("Web.JobReceiveSettings", "IsVisiblePassQty", c => c.Boolean());
            AddColumn("Web.MaterialPlanSettings", "isVisibleMachine", c => c.Boolean());
            AddColumn("Web.MaterialPlanSettings", "isMandatoryMachine", c => c.Boolean());
            AddColumn("Web.ProdOrderCancelLines", "MaterialPlanCancelLineId", c => c.Int());
            AddColumn("Web.SaleEnquirySettings", "DealUnitId", c => c.String(maxLength: 3));
            AddColumn("Web.SaleEnquirySettings", "isVisibleCreditDays", c => c.Boolean());
            CreateIndex("Web.MaterialPlanHeaders", "MachineId");
            CreateIndex("Web.PurchaseIndentCancelLines", "MaterialPlanCancelLineId");
            CreateIndex("Web.ProdOrderCancelLines", "MaterialPlanCancelLineId");
            CreateIndex("Web.SaleEnquiryHeaders", new[] { "DocTypeId", "DocNo", "DivisionId", "SiteId" }, unique: true, name: "IX_SaleEnquiryHeader_DocID");
            CreateIndex("Web.SaleEnquirySettings", "DealUnitId");
            AddForeignKey("Web.MaterialPlanHeaders", "MachineId", "Web.Machines", "ProductID");
            AddForeignKey("Web.PurchaseIndentCancelLines", "MaterialPlanCancelLineId", "Web.MaterialPlanCancelLines", "MaterialPlanCancelLineId");
            AddForeignKey("Web.ProdOrderCancelLines", "MaterialPlanCancelLineId", "Web.MaterialPlanCancelLines", "MaterialPlanCancelLineId");
            AddForeignKey("Web.SaleEnquirySettings", "DealUnitId", "Web.Units", "UnitId");
        }
        
        public override void Down()
        {
            DropForeignKey("Web.SaleEnquirySettings", "DealUnitId", "Web.Units");
            DropForeignKey("Web.ProductCategoryProcessSettings", "QAGroupId", "Web.QAGroups");
            DropForeignKey("Web.ProductCategoryProcessSettings", "ProductCategoryId", "Web.ProductCategories");
            DropForeignKey("Web.ProductCategoryProcessSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.ProdOrderCancelLines", "MaterialPlanCancelLineId", "Web.MaterialPlanCancelLines");
            DropForeignKey("Web.PurchaseIndentCancelLines", "MaterialPlanCancelLineId", "Web.MaterialPlanCancelLines");
            DropForeignKey("Web.MaterialPlanHeaders", "MachineId", "Web.Machines");
            DropIndex("Web.SaleEnquirySettings", new[] { "DealUnitId" });
            DropIndex("Web.SaleEnquiryHeaders", "IX_SaleEnquiryHeader_DocID");
            DropIndex("Web.ProductCategoryProcessSettings", new[] { "QAGroupId" });
            DropIndex("Web.ProductCategoryProcessSettings", "IX_ProductCategory_DocID");
            DropIndex("Web.ProdOrderCancelLines", new[] { "MaterialPlanCancelLineId" });
            DropIndex("Web.PurchaseIndentCancelLines", new[] { "MaterialPlanCancelLineId" });
            DropIndex("Web.MaterialPlanHeaders", new[] { "MachineId" });
            DropColumn("Web.SaleEnquirySettings", "isVisibleCreditDays");
            DropColumn("Web.SaleEnquirySettings", "DealUnitId");
            DropColumn("Web.ProdOrderCancelLines", "MaterialPlanCancelLineId");
            DropColumn("Web.MaterialPlanSettings", "isMandatoryMachine");
            DropColumn("Web.MaterialPlanSettings", "isVisibleMachine");
            DropColumn("Web.JobReceiveSettings", "IsVisiblePassQty");
            DropColumn("Web.JobReceiveSettings", "IsVisiblePenalty");
            DropColumn("Web.JobReceiveSettings", "IsVisibleIncentive");
            DropColumn("Web.CarpetSkuSettings", "isVisibleCBM");
            DropColumn("Web.CarpetSkuSettings", "isVisibleMapScale");
            DropColumn("Web.PurchaseIndentCancelLines", "MaterialPlanCancelLineId");
            DropColumn("Web.MaterialPlanHeaders", "MachineId");
            DropTable("Web.ProductCategoryProcessSettings");
            CreateIndex("Web.SaleEnquiryHeaders", "SiteId");
            CreateIndex("Web.SaleEnquiryHeaders", "DivisionId");
            CreateIndex("Web.SaleEnquiryHeaders", "DocTypeId");
        }
    }
}
