namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserReferral : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Web.PackingLines", "SaleEnquiryLine_SaleEnquiryLineId", "Web.SaleEnquiryLines");
            DropIndex("Web.Products", "IX_Product_ProductCode");
            DropIndex("Web.PackingLines", new[] { "SaleEnquiryLine_SaleEnquiryLineId" });
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
                        isVisibleProductionRemark = c.Boolean(),
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
            
            CreateTable(
                "Web.OverTimeApplicationHeaders",
                c => new
                    {
                        OverTimeApplicationId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DepartmentId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        GodownId = c.Int(nullable: false),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        ReferenceDocNo = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.OverTimeApplicationId)
                .ForeignKey("Web.Departments", t => t.DepartmentId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.People", t => t.PersonId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DepartmentId }, unique: true, name: "IX_OverTimeApplication_DocID")
                .Index(t => t.SiteId)
                .Index(t => t.PersonId)
                .Index(t => t.GodownId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.OverTimeApplicationLines",
                c => new
                    {
                        OverTimeApplicationLineId = c.Int(nullable: false, identity: true),
                        OverTimeApplicationHeaderId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.OverTimeApplicationLineId)
                .ForeignKey("Web.People", t => t.EmployeeId)
                .ForeignKey("Web.OverTimeApplicationHeaders", t => t.OverTimeApplicationHeaderId)
                .Index(t => t.OverTimeApplicationHeaderId)
                .Index(t => t.EmployeeId);
            
            AddColumn("Web.SaleOrderHeaders", "ReferenceDocTypeId", c => c.Int());
            AddColumn("Web.SaleOrderHeaders", "ReferenceDocId", c => c.Int());
            AddColumn("Web.SaleEnquirySettings", "SaleOrderDocTypeId", c => c.Int());
            AlterColumn("Web.Products", "ProductCode", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("Web.Products", "ProductCode", unique: true, name: "IX_Product_ProductCode");
            CreateIndex("Web.SaleOrderHeaders", "ReferenceDocTypeId");
            CreateIndex("Web.SaleEnquirySettings", "SaleOrderDocTypeId");
            AddForeignKey("Web.SaleOrderHeaders", "ReferenceDocTypeId", "Web.DocumentTypes", "DocumentTypeId");
            AddForeignKey("Web.SaleEnquirySettings", "SaleOrderDocTypeId", "Web.DocumentTypes", "DocumentTypeId");
            DropColumn("Web.PackingLines", "SaleEnquiryLine_SaleEnquiryLineId");
            //DropTable("Web.CarpetSkuSettings");
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
            
            AddColumn("Web.PackingLines", "SaleEnquiryLine_SaleEnquiryLineId", c => c.Int());
            DropForeignKey("Web.SaleEnquirySettings", "SaleOrderDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.OverTimeApplicationLines", "OverTimeApplicationHeaderId", "Web.OverTimeApplicationHeaders");
            DropForeignKey("Web.OverTimeApplicationLines", "EmployeeId", "Web.People");
            DropForeignKey("Web.OverTimeApplicationHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.OverTimeApplicationHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.OverTimeApplicationHeaders", "PersonId", "Web.People");
            DropForeignKey("Web.OverTimeApplicationHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.OverTimeApplicationHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.OverTimeApplicationHeaders", "DepartmentId", "Web.Departments");
            DropForeignKey("Web.CarpetSkuSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.CarpetSkuSettings", "ProductDesignId", "Web.ProductDesigns");
            DropForeignKey("Web.CarpetSkuSettings", "OriginCountryId", "Web.Countries");
            DropForeignKey("Web.CarpetSkuSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.AttendanceLines", "EmployeeId", "Web.People");
            DropForeignKey("Web.AttendanceLines", "AttendanceHeaderId", "Web.AttendanceHeaders");
            DropForeignKey("Web.AttendanceHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleOrderHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.AttendanceHeaders", "ShiftId", "Web.Shifts");
            DropForeignKey("Web.AttendanceHeaders", "PersonId", "Web.People");
            DropForeignKey("Web.AttendanceHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.AttendanceHeaders", "DepartmentId", "Web.Departments");
            DropIndex("Web.SaleEnquirySettings", new[] { "SaleOrderDocTypeId" });
            DropIndex("Web.OverTimeApplicationLines", new[] { "EmployeeId" });
            DropIndex("Web.OverTimeApplicationLines", new[] { "OverTimeApplicationHeaderId" });
            DropIndex("Web.OverTimeApplicationHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.OverTimeApplicationHeaders", new[] { "GodownId" });
            DropIndex("Web.OverTimeApplicationHeaders", new[] { "PersonId" });
            DropIndex("Web.OverTimeApplicationHeaders", new[] { "SiteId" });
            DropIndex("Web.OverTimeApplicationHeaders", "IX_OverTimeApplication_DocID");
            DropIndex("Web.CarpetSkuSettings", new[] { "OriginCountryId" });
            DropIndex("Web.CarpetSkuSettings", new[] { "ProductDesignId" });
            DropIndex("Web.CarpetSkuSettings", new[] { "DivisionId" });
            DropIndex("Web.CarpetSkuSettings", new[] { "SiteId" });
            DropIndex("Web.AttendanceLines", new[] { "EmployeeId" });
            DropIndex("Web.AttendanceLines", new[] { "AttendanceHeaderId" });
            DropIndex("Web.SaleOrderHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.Products", "IX_Product_ProductCode");
            DropIndex("Web.AttendanceHeaders", new[] { "PersonId" });
            DropIndex("Web.AttendanceHeaders", new[] { "ShiftId" });
            DropIndex("Web.AttendanceHeaders", new[] { "SiteId" });
            DropIndex("Web.AttendanceHeaders", new[] { "DepartmentId" });
            DropIndex("Web.AttendanceHeaders", "IX_AttendanceHeader_DocID");
            AlterColumn("Web.Products", "ProductCode", c => c.String(nullable: false, maxLength: 20));
            DropColumn("Web.SaleEnquirySettings", "SaleOrderDocTypeId");
            DropColumn("Web.SaleOrderHeaders", "ReferenceDocId");
            DropColumn("Web.SaleOrderHeaders", "ReferenceDocTypeId");
            DropTable("Web.OverTimeApplicationLines");
            DropTable("Web.OverTimeApplicationHeaders");
            DropTable("Web.CarpetSkuSettings");
            DropTable("Web.AttendanceLines");
            DropTable("Web.Shifts");
            DropTable("Web.AttendanceHeaders");
            CreateIndex("Web.PackingLines", "SaleEnquiryLine_SaleEnquiryLineId");
            CreateIndex("Web.Products", "ProductCode", unique: true, name: "IX_Product_ProductCode");
            AddForeignKey("Web.PackingLines", "SaleEnquiryLine_SaleEnquiryLineId", "Web.SaleEnquiryLines", "SaleEnquiryLineId");
        }
    }
}
