namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductCategorySettings : DbMigration
    {
        public override void Up()
        {
            DropIndex("Web.JobOrderLines", new[] { "JobOrderHeaderId" });
            DropIndex("Web.JobOrderLines", new[] { "ProductUidId" });
            DropIndex("Web.JobOrderLines", new[] { "ProductId" });
            DropIndex("Web.JobOrderLines", new[] { "ProdOrderLineId" });
            DropIndex("Web.JobOrderLines", new[] { "Dimension1Id" });
            DropIndex("Web.JobOrderLines", new[] { "Dimension2Id" });
            DropIndex("Web.JobOrderLines", new[] { "FromProcessId" });
            DropIndex("Web.JobInvoiceLines", new[] { "JobInvoiceHeaderId" });
            DropIndex("Web.JobInvoiceLines", new[] { "JobReceiveLineId" });
            DropIndex("Web.JobReceiveLines", new[] { "JobReceiveHeaderId" });
            DropIndex("Web.JobReceiveLines", new[] { "ProductUidId" });
            DropIndex("Web.JobReceiveLines", new[] { "JobOrderLineId" });
            CreateTable(
                "Web.ProductCategorySettings",
                c => new
                    {
                        ProductCategorySettingsId = c.Int(nullable: false, identity: true),
                        ProductCategoryId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        QAGroupId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductCategorySettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.ProductCategories", t => t.ProductCategoryId)
                .ForeignKey("Web.QAGroups", t => t.QAGroupId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.ProductCategoryId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.QAGroupId);
            
            CreateTable(
                "Web.ProductGroupSettings",
                c => new
                    {
                        ProductGroupSettingsId = c.Int(nullable: false, identity: true),
                        ProductGroupId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        QAGroupId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductGroupSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.ProductGroups", t => t.ProductGroupId)
                .ForeignKey("Web.QAGroups", t => t.QAGroupId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.ProductGroupId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.QAGroupId);
            
            AddColumn("Web.CarpetSkuSettings", "UnitConversions", c => c.String());
            AddColumn("Web.JobOrderSettings", "filterProductCategories", c => c.String());
            AddColumn("Web.JobReceiveQAAttributes", "Remark", c => c.String());
            AddColumn("Web.JobReceiveQAPenalties", "Remark", c => c.String());
            AddColumn("Web.OverTimeApplicationHeaders", "DivisionId", c => c.Int(nullable: false));
            AddColumn("Web.OverTimeApplicationHeaders", "ReviewCount", c => c.Int());
            AddColumn("Web.OverTimeApplicationHeaders", "ReviewBy", c => c.String());
            CreateIndex("Web.JobOrderLines", new[] { "JobOrderHeaderId", "ProductUidId", "ProductId", "Dimension1Id", "Dimension2Id", "ProdOrderLineId", "FromProcessId", "LotNo", "Specification" }, unique: true, name: "IX_JobOrderLine_Unique");
            CreateIndex("Web.JobInvoiceLines", new[] { "JobInvoiceHeaderId", "JobReceiveLineId" }, unique: true, name: "IX_JobInvoiceLine_Unique");
            CreateIndex("Web.JobReceiveLines", new[] { "JobReceiveHeaderId", "JobOrderLineId", "ProductUidId", "LotNo" }, unique: true, name: "IX_JobReceiveLine_Unique");
            CreateIndex("Web.OverTimeApplicationHeaders", "DivisionId");
            AddForeignKey("Web.OverTimeApplicationHeaders", "DivisionId", "Web.Divisions", "DivisionId");
            DropColumn("Web.JobReceiveQAAttributes", "Remarks");
            DropColumn("Web.JobReceiveQAPenalties", "Remarks");
        }
        
        public override void Down()
        {
            AddColumn("Web.JobReceiveQAPenalties", "Remarks", c => c.String());
            AddColumn("Web.JobReceiveQAAttributes", "Remarks", c => c.String());
            DropForeignKey("Web.ProductGroupSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.ProductGroupSettings", "QAGroupId", "Web.QAGroups");
            DropForeignKey("Web.ProductGroupSettings", "ProductGroupId", "Web.ProductGroups");
            DropForeignKey("Web.ProductGroupSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ProductCategorySettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.ProductCategorySettings", "QAGroupId", "Web.QAGroups");
            DropForeignKey("Web.ProductCategorySettings", "ProductCategoryId", "Web.ProductCategories");
            DropForeignKey("Web.ProductCategorySettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.OverTimeApplicationHeaders", "DivisionId", "Web.Divisions");
            DropIndex("Web.ProductGroupSettings", new[] { "QAGroupId" });
            DropIndex("Web.ProductGroupSettings", new[] { "DivisionId" });
            DropIndex("Web.ProductGroupSettings", new[] { "SiteId" });
            DropIndex("Web.ProductGroupSettings", new[] { "ProductGroupId" });
            DropIndex("Web.ProductCategorySettings", new[] { "QAGroupId" });
            DropIndex("Web.ProductCategorySettings", new[] { "DivisionId" });
            DropIndex("Web.ProductCategorySettings", new[] { "SiteId" });
            DropIndex("Web.ProductCategorySettings", new[] { "ProductCategoryId" });
            DropIndex("Web.OverTimeApplicationHeaders", new[] { "DivisionId" });
            DropIndex("Web.JobReceiveLines", "IX_JobReceiveLine_Unique");
            DropIndex("Web.JobInvoiceLines", "IX_JobInvoiceLine_Unique");
            DropIndex("Web.JobOrderLines", "IX_JobOrderLine_Unique");
            DropColumn("Web.OverTimeApplicationHeaders", "ReviewBy");
            DropColumn("Web.OverTimeApplicationHeaders", "ReviewCount");
            DropColumn("Web.OverTimeApplicationHeaders", "DivisionId");
            DropColumn("Web.JobReceiveQAPenalties", "Remark");
            DropColumn("Web.JobReceiveQAAttributes", "Remark");
            DropColumn("Web.JobOrderSettings", "filterProductCategories");
            DropColumn("Web.CarpetSkuSettings", "UnitConversions");
            DropTable("Web.ProductGroupSettings");
            DropTable("Web.ProductCategorySettings");
            CreateIndex("Web.JobReceiveLines", "JobOrderLineId");
            CreateIndex("Web.JobReceiveLines", "ProductUidId");
            CreateIndex("Web.JobReceiveLines", "JobReceiveHeaderId");
            CreateIndex("Web.JobInvoiceLines", "JobReceiveLineId");
            CreateIndex("Web.JobInvoiceLines", "JobInvoiceHeaderId");
            CreateIndex("Web.JobOrderLines", "FromProcessId");
            CreateIndex("Web.JobOrderLines", "Dimension2Id");
            CreateIndex("Web.JobOrderLines", "Dimension1Id");
            CreateIndex("Web.JobOrderLines", "ProdOrderLineId");
            CreateIndex("Web.JobOrderLines", "ProductId");
            CreateIndex("Web.JobOrderLines", "ProductUidId");
            CreateIndex("Web.JobOrderLines", "JobOrderHeaderId");
        }
    }
}
