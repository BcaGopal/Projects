namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductTypeSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Web.ProductTypeSettings",
                c => new
                    {
                        ProductTypeSettingsId = c.Int(nullable: false, identity: true),
                        ProductTypeId = c.Int(nullable: false),
                        UnitId = c.String(),
                        isShowMappedDimension1 = c.Boolean(),
                        isShowUnMappedDimension1 = c.Boolean(),
                        isApplicableDimension1 = c.Boolean(),
                        Dimension1Caption = c.String(),
                        isShowMappedDimension2 = c.Boolean(),
                        isShowUnMappedDimension2 = c.Boolean(),
                        isApplicableDimension2 = c.Boolean(),
                        Dimension2Caption = c.String(),
                        isShowMappedDimension3 = c.Boolean(),
                        isShowUnMappedDimension3 = c.Boolean(),
                        isApplicableDimension3 = c.Boolean(),
                        Dimension3Caption = c.String(),
                        isShowMappedDimension4 = c.Boolean(),
                        isShowUnMappedDimension4 = c.Boolean(),
                        isApplicableDimension4 = c.Boolean(),
                        Dimension4Caption = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProductTypeSettingsId)
                .ForeignKey("Web.DocumentTypes", t => t.ProductTypeId)
                .Index(t => t.ProductTypeId);
            
            AddColumn("Web.Units", "DimensionUnitId", c => c.String(maxLength: 3));
            AddColumn("Web.PersonSettings", "isVisibleCreditDays", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isVisibleCreditLimit", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryAddress", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryCity", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryZipCode", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryMobile", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryEmail", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryPanNo", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryGuarantor", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryTdsCategory", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryTdsGroup", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatorySalesTaxGroup", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryCstNo", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryTinNo", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryCreditDays", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryCreditLimit", c => c.Boolean());
            AddColumn("Web.PersonSettings", "LedgerAccountGroupId", c => c.Int(nullable: false));
            AlterColumn("Web.PersonAddresses", "Zipcode", c => c.String(maxLength: 6));
            CreateIndex("Web.PersonSettings", "LedgerAccountGroupId");
            AddForeignKey("Web.PersonSettings", "LedgerAccountGroupId", "Web.LedgerAccountGroups", "LedgerAccountGroupId");
            DropColumn("Web.PersonSettings", "AccountGroupId");
        }
        
        public override void Down()
        {
            AddColumn("Web.PersonSettings", "AccountGroupId", c => c.Int(nullable: false));
            DropForeignKey("Web.ProductTypeSettings", "ProductTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PersonSettings", "LedgerAccountGroupId", "Web.LedgerAccountGroups");
            DropIndex("Web.ProductTypeSettings", new[] { "ProductTypeId" });
            DropIndex("Web.PersonSettings", new[] { "LedgerAccountGroupId" });
            AlterColumn("Web.PersonAddresses", "Zipcode", c => c.String(nullable: false, maxLength: 6));
            DropColumn("Web.PersonSettings", "LedgerAccountGroupId");
            DropColumn("Web.PersonSettings", "isMandatoryCreditLimit");
            DropColumn("Web.PersonSettings", "isMandatoryCreditDays");
            DropColumn("Web.PersonSettings", "isMandatoryTinNo");
            DropColumn("Web.PersonSettings", "isMandatoryCstNo");
            DropColumn("Web.PersonSettings", "isMandatorySalesTaxGroup");
            DropColumn("Web.PersonSettings", "isMandatoryTdsGroup");
            DropColumn("Web.PersonSettings", "isMandatoryTdsCategory");
            DropColumn("Web.PersonSettings", "isMandatoryGuarantor");
            DropColumn("Web.PersonSettings", "isMandatoryPanNo");
            DropColumn("Web.PersonSettings", "isMandatoryEmail");
            DropColumn("Web.PersonSettings", "isMandatoryMobile");
            DropColumn("Web.PersonSettings", "isMandatoryZipCode");
            DropColumn("Web.PersonSettings", "isMandatoryCity");
            DropColumn("Web.PersonSettings", "isMandatoryAddress");
            DropColumn("Web.PersonSettings", "isVisibleCreditLimit");
            DropColumn("Web.PersonSettings", "isVisibleCreditDays");
            DropColumn("Web.Units", "DimensionUnitId");
            DropTable("Web.ProductTypeSettings");
        }
    }
}
