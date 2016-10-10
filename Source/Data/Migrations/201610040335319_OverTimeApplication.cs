namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OverTimeApplication : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Web.AttendanceHeaders",
                c => new
                    {
                        AttendanceHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DepartmentId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ShiftId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                    })
                .PrimaryKey(t => t.AttendanceHeaderId)
                .ForeignKey("Web.Departments", t => t.DepartmentId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.People", t => t.PersonId)
                .ForeignKey("Web.Shifts", t => t.ShiftId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo }, unique: true, name: "IX_AttendanceHeader_DocID")
                .Index(t => t.DepartmentId)
                .Index(t => t.SiteId)
                .Index(t => t.ShiftId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "Web.Shifts",
                c => new
                    {
                        ShiftId = c.Int(nullable: false, identity: true),
                        ShiftName = c.String(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(),
                    })
                .PrimaryKey(t => t.ShiftId);
            
            CreateTable(
                "Web.AttendanceLines",
                c => new
                    {
                        AttendanceLineId = c.Int(nullable: false, identity: true),
                        AttendanceHeaderId = c.Int(nullable: false),
                        DocTime = c.DateTime(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        AttendanceCategory = c.String(maxLength: 1),
                        Remark = c.String(maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.AttendanceLineId)
                .ForeignKey("Web.AttendanceHeaders", t => t.AttendanceHeaderId)
                .ForeignKey("Web.People", t => t.EmployeeId)
                .Index(t => t.AttendanceHeaderId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "Web.CarpetSkuSettings",
                c => new
                    {
                        CarpetSkuSettingsId = c.Int(nullable: false, identity: true),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleProductDesign = c.Boolean(),
                        isVisibleProductStyle = c.Boolean(),
                        isVisibleProductManufacturer = c.Boolean(),
                        isVisibleProductDesignPattern = c.Boolean(),
                        isVisibleContent = c.Boolean(),
                        isVisibleOriginCountry = c.Boolean(),
                        isVisibleInvoiceGroup = c.Boolean(),
                        isVisibleDrawbackTarrif = c.Boolean(),
                        isVisibleStandardCost = c.Boolean(),
                        isVisibleStandardWeight = c.Boolean(),
                        isVisibleSupplierDetail = c.Boolean(),
                        isVisibleSample = c.Boolean(),
                        isVisibleCounterNo = c.Boolean(),
                        isVisibleTags = c.Boolean(),
                        isVisibleDivision = c.Boolean(),
                        isVisibleColour = c.Boolean(),
                        ProductDesignId = c.Int(),
                        OriginCountryId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CarpetSkuSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Countries", t => t.OriginCountryId)
                .ForeignKey("Web.ProductDesigns", t => t.ProductDesignId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.ProductDesignId)
                .Index(t => t.OriginCountryId);
            
            DropTable("Web.CarpetSkuSettings");
        }
        
        public override void Down()
        {
            CreateTable(
                "Web.CarpetSkuSettings",
                c => new
                    {
                        CarpetSkuSettingsId = c.Int(nullable: false, identity: true),
                        isVisibleColourWays = c.Boolean(),
                        isVisibleStyle = c.Boolean(),
                        isVisibleDesigner = c.Boolean(),
                        isVisibleDesignPattern = c.Boolean(),
                        isVisibleContent = c.Boolean(),
                        isVisibleOrigin = c.Boolean(),
                        isVisibleInvoiceGroup = c.Boolean(),
                        isVisibleDrawbackTarrif = c.Boolean(),
                        isVisibleStandardCost = c.Boolean(),
                        isVisibleFinishedWeight = c.Boolean(),
                        isVisibleSupplierDetail = c.Boolean(),
                        isVisibleLeadTime = c.Boolean(),
                        isVisibleMinQty = c.Boolean(),
                        isVisibleMaxQty = c.Boolean(),
                        isVisibleSample = c.Boolean(),
                        isVisibleCounterNo = c.Boolean(),
                        isVisibleTags = c.Boolean(),
                        isVisibleDivision = c.Boolean(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CarpetSkuSettingsId);
            
            DropForeignKey("Web.CarpetSkuSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.CarpetSkuSettings", "ProductDesignId", "Web.ProductDesigns");
            DropForeignKey("Web.CarpetSkuSettings", "OriginCountryId", "Web.Countries");
            DropForeignKey("Web.CarpetSkuSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.AttendanceLines", "EmployeeId", "Web.People");
            DropForeignKey("Web.AttendanceLines", "AttendanceHeaderId", "Web.AttendanceHeaders");
            DropForeignKey("Web.AttendanceHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.AttendanceHeaders", "ShiftId", "Web.Shifts");
            DropForeignKey("Web.AttendanceHeaders", "PersonId", "Web.People");
            DropForeignKey("Web.AttendanceHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.AttendanceHeaders", "DepartmentId", "Web.Departments");
            DropIndex("Web.CarpetSkuSettings", new[] { "OriginCountryId" });
            DropIndex("Web.CarpetSkuSettings", new[] { "ProductDesignId" });
            DropIndex("Web.CarpetSkuSettings", new[] { "DivisionId" });
            DropIndex("Web.CarpetSkuSettings", new[] { "SiteId" });
            DropIndex("Web.AttendanceLines", new[] { "EmployeeId" });
            DropIndex("Web.AttendanceLines", new[] { "AttendanceHeaderId" });
            DropIndex("Web.AttendanceHeaders", new[] { "PersonId" });
            DropIndex("Web.AttendanceHeaders", new[] { "ShiftId" });
            DropIndex("Web.AttendanceHeaders", new[] { "SiteId" });
            DropIndex("Web.AttendanceHeaders", new[] { "DepartmentId" });
            DropIndex("Web.AttendanceHeaders", "IX_AttendanceHeader_DocID");
            DropTable("Web.CarpetSkuSettings");
            DropTable("Web.AttendanceLines");
            DropTable("Web.Shifts");
            DropTable("Web.AttendanceHeaders");
        }
    }
}
