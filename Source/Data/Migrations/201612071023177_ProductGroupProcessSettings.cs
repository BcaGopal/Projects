namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductGroupProcessSettings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Web.ProductCategorySettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ProductCategorySettings", "QAGroupId", "Web.QAGroups");
            DropForeignKey("Web.ProductCategorySettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.ProductGroupSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ProductGroupSettings", "QAGroupId", "Web.QAGroups");
            DropForeignKey("Web.ProductGroupSettings", "SiteId", "Web.Sites");
            DropIndex("Web.ProductCategorySettings", new[] { "SiteId" });
            DropIndex("Web.ProductCategorySettings", new[] { "DivisionId" });
            DropIndex("Web.ProductCategorySettings", new[] { "QAGroupId" });
            DropIndex("Web.ProductGroupSettings", new[] { "SiteId" });
            DropIndex("Web.ProductGroupSettings", new[] { "DivisionId" });
            DropIndex("Web.ProductGroupSettings", new[] { "QAGroupId" });
            RenameIndex(table: "Web.ProductCategoryProcessSettings", name: "IX_ProductCategory_DocID", newName: "IX_ProductCategoryProcessSettings_DocID");
            AddColumn("Web.JobReceiveQASettings", "isVisibleMarks", c => c.Boolean());
            AddColumn("Web.JobReceiveQASettings", "isVisibleDealUnit", c => c.Boolean());
            AddColumn("Web.JobReceiveQASettings", "IsVisibleInspectedQty", c => c.Boolean());
            AddColumn("Web.JobReceiveQASettings", "IsVisiblePenalty", c => c.Boolean());
            AddColumn("Web.JobReceiveQASettings", "IsVisibleSpecification", c => c.Boolean());
            AddColumn("Web.JobReceiveQASettings", "IsVisibleWeight", c => c.Boolean());
            DropColumn("Web.ProductCategorySettings", "SiteId");
            DropColumn("Web.ProductCategorySettings", "DivisionId");
            DropColumn("Web.ProductCategorySettings", "QAGroupId");
            DropColumn("Web.ProductGroupSettings", "SiteId");
            DropColumn("Web.ProductGroupSettings", "DivisionId");
            DropColumn("Web.ProductGroupSettings", "QAGroupId");
        }
        
        public override void Down()
        {
            AddColumn("Web.ProductGroupSettings", "QAGroupId", c => c.Int());
            AddColumn("Web.ProductGroupSettings", "DivisionId", c => c.Int(nullable: false));
            AddColumn("Web.ProductGroupSettings", "SiteId", c => c.Int(nullable: false));
            AddColumn("Web.ProductCategorySettings", "QAGroupId", c => c.Int());
            AddColumn("Web.ProductCategorySettings", "DivisionId", c => c.Int(nullable: false));
            AddColumn("Web.ProductCategorySettings", "SiteId", c => c.Int(nullable: false));
            DropColumn("Web.JobReceiveQASettings", "IsVisibleWeight");
            DropColumn("Web.JobReceiveQASettings", "IsVisibleSpecification");
            DropColumn("Web.JobReceiveQASettings", "IsVisiblePenalty");
            DropColumn("Web.JobReceiveQASettings", "IsVisibleInspectedQty");
            DropColumn("Web.JobReceiveQASettings", "isVisibleDealUnit");
            DropColumn("Web.JobReceiveQASettings", "isVisibleMarks");
            RenameIndex(table: "Web.ProductCategoryProcessSettings", name: "IX_ProductCategoryProcessSettings_DocID", newName: "IX_ProductCategory_DocID");
            CreateIndex("Web.ProductGroupSettings", "QAGroupId");
            CreateIndex("Web.ProductGroupSettings", "DivisionId");
            CreateIndex("Web.ProductGroupSettings", "SiteId");
            CreateIndex("Web.ProductCategorySettings", "QAGroupId");
            CreateIndex("Web.ProductCategorySettings", "DivisionId");
            CreateIndex("Web.ProductCategorySettings", "SiteId");
            AddForeignKey("Web.ProductGroupSettings", "SiteId", "Web.Sites", "SiteId");
            AddForeignKey("Web.ProductGroupSettings", "QAGroupId", "Web.QAGroups", "QAGroupId");
            AddForeignKey("Web.ProductGroupSettings", "DivisionId", "Web.Divisions", "DivisionId");
            AddForeignKey("Web.ProductCategorySettings", "SiteId", "Web.Sites", "SiteId");
            AddForeignKey("Web.ProductCategorySettings", "QAGroupId", "Web.QAGroups", "QAGroupId");
            AddForeignKey("Web.ProductCategorySettings", "DivisionId", "Web.Divisions", "DivisionId");
        }
    }
}
