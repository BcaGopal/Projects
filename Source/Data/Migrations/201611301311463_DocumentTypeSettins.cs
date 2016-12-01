namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentTypeSettins : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Web.DocumentTypeSettings",
                c => new
                    {
                        DocumentTypeSettingsId = c.Int(nullable: false, identity: true),
                        DocumentTypeId = c.Int(nullable: false),
                        ProductCaption = c.String(maxLength: 50),
                        ProductGroupCaption = c.String(maxLength: 50),
                        ProductCategoryCaption = c.String(maxLength: 50),
                        Dimension1Caption = c.String(maxLength: 50),
                        Dimension2Caption = c.String(maxLength: 50),
                        Dimension3Caption = c.String(maxLength: 50),
                        Dimension4Caption = c.String(maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DocumentTypeSettingsId)
                .ForeignKey("Web.DocumentTypes", t => t.DocumentTypeId)
                .Index(t => t.DocumentTypeId);
            
            AddColumn("Web.JobInvoiceSettings", "isVisibleSpecification", c => c.Boolean());
            AddColumn("Web.JobInvoiceSettings", "isVisibleDealUnit", c => c.Boolean());
            AddColumn("Web.JobInvoiceSettings", "isVisibleWeight", c => c.Boolean());
            AddColumn("Web.JobInvoiceSettings", "isVisibleCostCenter", c => c.Boolean());
            AddColumn("Web.JobOrderSettings", "isVisibleDealUnit", c => c.Boolean());
            AddColumn("Web.JobOrderSettings", "isVisibleLineDueDate", c => c.Boolean());
            AddColumn("Web.JobOrderSettings", "isVisibleBillToParty", c => c.Boolean());
            AddColumn("Web.JobOrderSettings", "isVisibleUnitConversionFor", c => c.Boolean());
            AddColumn("Web.JobOrderSettings", "isVisibleSpecification", c => c.Boolean());
            AddColumn("Web.JobOrderSettings", "isVisibleCreditDays", c => c.Boolean());
            AddColumn("Web.JobReceiveSettings", "isVisibleSpecification", c => c.Boolean());
            AddColumn("Web.JobReceiveSettings", "isVisibleDealUnit", c => c.Boolean());
            AddColumn("Web.ViewProdOrderBalance", "BuyerId", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("Web.DocumentTypeSettings", "DocumentTypeId", "Web.DocumentTypes");
            DropIndex("Web.DocumentTypeSettings", new[] { "DocumentTypeId" });
            DropColumn("Web.ViewProdOrderBalance", "BuyerId");
            DropColumn("Web.JobReceiveSettings", "isVisibleDealUnit");
            DropColumn("Web.JobReceiveSettings", "isVisibleSpecification");
            DropColumn("Web.JobOrderSettings", "isVisibleCreditDays");
            DropColumn("Web.JobOrderSettings", "isVisibleSpecification");
            DropColumn("Web.JobOrderSettings", "isVisibleUnitConversionFor");
            DropColumn("Web.JobOrderSettings", "isVisibleBillToParty");
            DropColumn("Web.JobOrderSettings", "isVisibleLineDueDate");
            DropColumn("Web.JobOrderSettings", "isVisibleDealUnit");
            DropColumn("Web.JobInvoiceSettings", "isVisibleCostCenter");
            DropColumn("Web.JobInvoiceSettings", "isVisibleWeight");
            DropColumn("Web.JobInvoiceSettings", "isVisibleDealUnit");
            DropColumn("Web.JobInvoiceSettings", "isVisibleSpecification");
            DropTable("Web.DocumentTypeSettings");
        }
    }
}
