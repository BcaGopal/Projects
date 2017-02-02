namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonRole : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Web.PersonRoles",
                c => new
                    {
                        PersonRoleId = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        RoleDocTypeId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonRoleId)
                .ForeignKey("Web.People", t => t.PersonId)
                .ForeignKey("Web.DocumentTypes", t => t.RoleDocTypeId)
                .Index(t => new { t.PersonId, t.RoleDocTypeId }, unique: true, name: "IX_PersonRole_DocId");
            
            AddColumn("Web.Stocks", "DocLineId", c => c.Int());
            AddColumn("Web.BomDetails", "BaseProcessId", c => c.Int());
            AddColumn("Web.DocumentTypeSettings", "DealQtyCaption", c => c.String(maxLength: 50));
            AddColumn("Web.DocumentTypeSettings", "WeightCaption", c => c.String(maxLength: 50));
            AddColumn("Web.MaterialPlanSettings", "isVisiblePurchPlanQty", c => c.Boolean());
            AddColumn("Web.PersonSettings", "DefaultProcessId", c => c.Int());
            CreateIndex("Web.BomDetails", "BaseProcessId");
            CreateIndex("Web.PersonSettings", "DefaultProcessId");
            AddForeignKey("Web.BomDetails", "BaseProcessId", "Web.Processes", "ProcessId");
            AddForeignKey("Web.PersonSettings", "DefaultProcessId", "Web.Processes", "ProcessId");
        }
        
        public override void Down()
        {
            DropForeignKey("Web.PersonSettings", "DefaultProcessId", "Web.Processes");
            DropForeignKey("Web.PersonRoles", "RoleDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PersonRoles", "PersonId", "Web.People");
            DropForeignKey("Web.BomDetails", "BaseProcessId", "Web.Processes");
            DropIndex("Web.PersonSettings", new[] { "DefaultProcessId" });
            DropIndex("Web.PersonRoles", "IX_PersonRole_DocId");
            DropIndex("Web.BomDetails", new[] { "BaseProcessId" });
            DropColumn("Web.PersonSettings", "DefaultProcessId");
            DropColumn("Web.MaterialPlanSettings", "isVisiblePurchPlanQty");
            DropColumn("Web.DocumentTypeSettings", "WeightCaption");
            DropColumn("Web.DocumentTypeSettings", "DealQtyCaption");
            DropColumn("Web.BomDetails", "BaseProcessId");
            DropColumn("Web.Stocks", "DocLineId");
            DropTable("Web.PersonRoles");
        }
    }
}
