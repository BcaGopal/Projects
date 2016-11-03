//namespace Data.Migrations
//{
//    using System;
//    using System.Data.Entity.Migrations;

//    public partial class CarpetSkuSettingCorrection : DbMigration
//    {
//        public override void Up()
//        {
//            CreateTable(
//                "Web.CarpetSkuSettings",
//                c => new
//                {
//                    CarpetSkuSettingsId = c.Int(nullable: false, identity: true),
//                    SiteId = c.Int(nullable: false),
//                    DivisionId = c.Int(nullable: false),
//                    isVisibleProductDesign = c.Boolean(),
//                    isVisibleProductStyle = c.Boolean(),
//                    isVisibleProductManufacturer = c.Boolean(),
//                    isVisibleProductDesignPattern = c.Boolean(),
//                    isVisibleContent = c.Boolean(),
//                    isVisibleOriginCountry = c.Boolean(),
//                    isVisibleInvoiceGroup = c.Boolean(),
//                    isVisibleDrawbackTarrif = c.Boolean(),
//                    isVisibleStandardCost = c.Boolean(),
//                    isVisibleStandardWeight = c.Boolean(),
//                    isVisibleSupplierDetail = c.Boolean(),
//                    isVisibleSample = c.Boolean(),
//                    isVisibleCounterNo = c.Boolean(),
//                    isVisibleTags = c.Boolean(),
//                    isVisibleDivision = c.Boolean(),
//                    isVisibleColour = c.Boolean(),
//                    CreatedBy = c.String(),
//                    ModifiedBy = c.String(),
//                    CreatedDate = c.DateTime(nullable: false),
//                    ModifiedDate = c.DateTime(nullable: false),
//                    OMSId = c.String(maxLength: 50),
//                })
//                .PrimaryKey(t => t.CarpetSkuSettingsId)
//                .ForeignKey("Web.Divisions", t => t.DivisionId)
//                .ForeignKey("Web.Sites", t => t.SiteId)
//                .Index(t => t.SiteId)
//                .Index(t => t.DivisionId);

//            DropTable("Web.CarpetSkuSettings");
//        }

//        public override void Down()
//        {
//            CreateTable(
//                "Web.CarpetSkuSettings",
//                c => new
//                {
//                    CarpetSkuSettingsId = c.Int(nullable: false, identity: true),
//                    isVisibleColourWays = c.Boolean(),
//                    isVisibleStyle = c.Boolean(),
//                    isVisibleDesigner = c.Boolean(),
//                    isVisibleDesignPattern = c.Boolean(),
//                    isVisibleContent = c.Boolean(),
//                    isVisibleOrigin = c.Boolean(),
//                    isVisibleInvoiceGroup = c.Boolean(),
//                    isVisibleDrawbackTarrif = c.Boolean(),
//                    isVisibleStandardCost = c.Boolean(),
//                    isVisibleFinishedWeight = c.Boolean(),
//                    isVisibleSupplierDetail = c.Boolean(),
//                    isVisibleLeadTime = c.Boolean(),
//                    isVisibleMinQty = c.Boolean(),
//                    isVisibleMaxQty = c.Boolean(),
//                    isVisibleSample = c.Boolean(),
//                    isVisibleCounterNo = c.Boolean(),
//                    isVisibleTags = c.Boolean(),
//                    isVisibleDivision = c.Boolean(),
//                    CreatedBy = c.String(),
//                    ModifiedBy = c.String(),
//                    CreatedDate = c.DateTime(nullable: false),
//                    ModifiedDate = c.DateTime(nullable: false),
//                    OMSId = c.String(maxLength: 50),
//                })
//                .PrimaryKey(t => t.CarpetSkuSettingsId);

//            DropForeignKey("Web.CarpetSkuSettings", "SiteId", "Web.Sites");
//            DropForeignKey("Web.CarpetSkuSettings", "DivisionId", "Web.Divisions");
//            DropIndex("Web.CarpetSkuSettings", new[] { "DivisionId" });
//            DropIndex("Web.CarpetSkuSettings", new[] { "SiteId" });
//            DropTable("Web.CarpetSkuSettings");
//        }
//    }
//}
