namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Web._Users",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.UserName);
            
            CreateTable(
                "Web.ActivityLogs",
                c => new
                    {
                        ActivityLogId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(),
                        DocId = c.Int(nullable: false),
                        DocNo = c.String(),
                        DocDate = c.DateTime(),
                        DocLineId = c.Int(),
                        ActivityType = c.Int(nullable: false),
                        Narration = c.String(),
                        UserRemark = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UploadDate = c.DateTime(),
                        TransactionId = c.String(maxLength: 50),
                        TransactionError = c.String(),
                        Modifications = c.String(),
                        ControllerName = c.String(maxLength: 50),
                        ActionName = c.String(maxLength: 50),
                        DocStatus = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ActivityLogId)
                .ForeignKey("Web.ActivityTypes", t => t.ActivityType)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ActivityType);
            
            CreateTable(
                "Web.ActivityTypes",
                c => new
                    {
                        ActivityTypeId = c.Int(nullable: false, identity: true),
                        ActivityTypeName = c.String(nullable: false, maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ActivityTypeId)
                .Index(t => t.ActivityTypeName, unique: true, name: "IX_ActivityType_ActivityTypeName");
            
            CreateTable(
                "Web.DocumentTypes",
                c => new
                    {
                        DocumentTypeId = c.Int(nullable: false, identity: true),
                        DocumentTypeShortName = c.String(nullable: false, maxLength: 5),
                        DocumentTypeName = c.String(nullable: false, maxLength: 50),
                        DocumentCategoryId = c.Int(nullable: false),
                        ControllerActionId = c.Int(),
                        DomainName = c.String(maxLength: 25),
                        VoucherType = c.String(maxLength: 20),
                        IsSystemDefine = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ReportMenuId = c.Int(),
                        Nature = c.String(maxLength: 10),
                        IconDisplayName = c.String(),
                        ImageFileName = c.String(),
                        ImageFolderName = c.String(),
                        SupportGatePass = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        DatabaseTableName = c.String(maxLength: 50),
                        ControllerName = c.String(maxLength: 50),
                        ActionNamePendingToSubmit = c.String(maxLength: 50),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DocumentTypeId)
                .ForeignKey("Web.ControllerActions", t => t.ControllerActionId)
                .ForeignKey("Web.DocumentCategories", t => t.DocumentCategoryId)
                .ForeignKey("Web.Menus", t => t.ReportMenuId)
                .Index(t => t.DocumentTypeShortName, unique: true, name: "IX_DocumentType_DocumentTypeShortName")
                .Index(t => t.DocumentTypeName, unique: true, name: "IX_DocumentType_DocumentTypeName")
                .Index(t => t.DocumentCategoryId)
                .Index(t => t.ControllerActionId)
                .Index(t => t.ReportMenuId);
            
            CreateTable(
                "Web.ControllerActions",
                c => new
                    {
                        ControllerActionId = c.Int(nullable: false, identity: true),
                        ControllerId = c.Int(),
                        ControllerName = c.String(maxLength: 100),
                        ActionName = c.String(nullable: false, maxLength: 100),
                        PubModuleName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ControllerActionId)
                .ForeignKey("Web.MvcControllers", t => t.ControllerId)
                .Index(t => t.ControllerId)
                .Index(t => new { t.ControllerName, t.ActionName }, unique: true, name: "IX_ControllerAction_ControllerName");
            
            CreateTable(
                "Web.MvcControllers",
                c => new
                    {
                        ControllerId = c.Int(nullable: false, identity: true),
                        ControllerName = c.String(nullable: false, maxLength: 100),
                        ParentControllerId = c.Int(),
                        PubModuleName = c.String(maxLength: 30),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ControllerId)
                .ForeignKey("Web.MvcControllers", t => t.ParentControllerId)
                .Index(t => new { t.ControllerName, t.PubModuleName }, unique: true, name: "IX_Controller_ControllerName")
                .Index(t => t.ParentControllerId);
            
            CreateTable(
                "Web.DocumentCategories",
                c => new
                    {
                        DocumentCategoryId = c.Int(nullable: false, identity: true),
                        DocumentCategoryName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DocumentCategoryId)
                .Index(t => t.DocumentCategoryName, unique: true, name: "IX_DocumentCategory_DocumentCategoryName");
            
            CreateTable(
                "Web.Menus",
                c => new
                    {
                        MenuId = c.Int(nullable: false, identity: true),
                        MenuName = c.String(nullable: false, maxLength: 50),
                        Srl = c.String(nullable: false, maxLength: 5),
                        IconName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 100),
                        URL = c.String(maxLength: 100),
                        ModuleId = c.Int(nullable: false),
                        SubModuleId = c.Int(nullable: false),
                        ControllerActionId = c.Int(nullable: false),
                        IsVisible = c.Boolean(),
                        isDivisionBased = c.Boolean(),
                        isSiteBased = c.Boolean(),
                        RouteId = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MenuId)
                .ForeignKey("Web.ControllerActions", t => t.ControllerActionId)
                .ForeignKey("Web.MenuModules", t => t.ModuleId)
                .ForeignKey("Web.MenuSubModules", t => t.SubModuleId)
                .Index(t => new { t.MenuName, t.ModuleId, t.SubModuleId }, unique: true, name: "IX_Menu_MenuName")
                .Index(t => t.ControllerActionId);
            
            CreateTable(
                "Web.MenuModules",
                c => new
                    {
                        ModuleId = c.Int(nullable: false, identity: true),
                        ModuleName = c.String(nullable: false, maxLength: 50),
                        Srl = c.Int(nullable: false),
                        IconName = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        URL = c.String(maxLength: 100),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ModuleId)
                .Index(t => t.ModuleName, unique: true, name: "IX_Module_ModuleName");
            
            CreateTable(
                "Web.MenuSubModules",
                c => new
                    {
                        SubModuleId = c.Int(nullable: false, identity: true),
                        SubModuleName = c.String(nullable: false, maxLength: 50),
                        IconName = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SubModuleId)
                .Index(t => t.SubModuleName, unique: true, name: "IX_SubModule_SubModuleName");
            
            CreateTable(
                "Web.Agents",
                c => new
                    {
                        PersonID = c.Int(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("Web.People", t => t.PersonID)
                .Index(t => t.PersonID);
            
            CreateTable(
                "Web.People",
                c => new
                    {
                        PersonID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Suffix = c.String(nullable: false, maxLength: 20),
                        Code = c.String(nullable: false, maxLength: 20),
                        Description = c.String(maxLength: 500),
                        Phone = c.String(maxLength: 11),
                        Mobile = c.String(maxLength: 10),
                        Email = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        Tags = c.String(),
                        ImageFolderName = c.String(maxLength: 100),
                        ImageFileName = c.String(maxLength: 100),
                        IsSisterConcern = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        Nature = c.String(maxLength: 20),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("Web.Users", t => t.ApplicationUser_Id)
                .Index(t => t.Code, unique: true, name: "IX_Person_Code")
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "Web.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "Web.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Users", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "Web.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("Web.Users", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "Web.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("Web.AspNetRoles", t => t.RoleId)
                .ForeignKey("Web.Users", t => t.IdentityUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "Web.PersonAddresses",
                c => new
                    {
                        PersonAddressID = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        AddressType = c.String(),
                        Address = c.String(),
                        CityId = c.Int(),
                        AreaId = c.Int(),
                        Zipcode = c.String(nullable: false, maxLength: 6),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonAddressID)
                .ForeignKey("Web.Areas", t => t.AreaId)
                .ForeignKey("Web.Cities", t => t.CityId)
                .ForeignKey("Web.People", t => t.PersonId)
                .Index(t => t.PersonId)
                .Index(t => t.CityId)
                .Index(t => t.AreaId);
            
            CreateTable(
                "Web.Areas",
                c => new
                    {
                        AreaId = c.Int(nullable: false, identity: true),
                        AreaName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.AreaId)
                .Index(t => t.AreaName, unique: true, name: "IX_Area_AreaName");
            
            CreateTable(
                "Web.Cities",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        CityName = c.String(nullable: false, maxLength: 50),
                        StateId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CityId)
                .ForeignKey("Web.States", t => t.StateId)
                .Index(t => t.CityName, unique: true, name: "IX_City_CityName")
                .Index(t => t.StateId);
            
            CreateTable(
                "Web.States",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        StateName = c.String(nullable: false, maxLength: 50),
                        CountryId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        Calculation_CalculationId = c.Int(),
                        Charge_ChargeId = c.Int(),
                    })
                .PrimaryKey(t => t.StateId)
                .ForeignKey("Web.Countries", t => t.CountryId)
                .ForeignKey("Web.Calculations", t => t.Calculation_CalculationId)
                .ForeignKey("Web.Charges", t => t.Charge_ChargeId)
                .Index(t => t.StateName, unique: true, name: "IX_State_StateName")
                .Index(t => t.CountryId)
                .Index(t => t.Calculation_CalculationId)
                .Index(t => t.Charge_ChargeId);
            
            CreateTable(
                "Web.Countries",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        CountryName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CountryId)
                .Index(t => t.CountryName, unique: true, name: "IX_Country_Country");
            
            CreateTable(
                "Web.PersonContacts",
                c => new
                    {
                        PersonContactID = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        PersonContactTypeId = c.Int(nullable: false),
                        ContactId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        Person_PersonID = c.Int(),
                    })
                .PrimaryKey(t => t.PersonContactID)
                .ForeignKey("Web.People", t => t.ContactId)
                .ForeignKey("Web.People", t => t.PersonId)
                .ForeignKey("Web.PersonContactTypes", t => t.PersonContactTypeId)
                .ForeignKey("Web.People", t => t.Person_PersonID)
                .Index(t => t.PersonId)
                .Index(t => t.PersonContactTypeId)
                .Index(t => t.ContactId)
                .Index(t => t.Person_PersonID);
            
            CreateTable(
                "Web.PersonContactTypes",
                c => new
                    {
                        PersonContactTypeId = c.Int(nullable: false, identity: true),
                        PersonContactTypeName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonContactTypeId)
                .Index(t => t.PersonContactTypeName, unique: true, name: "IX_PersonContactType_PersonContactTypeName");
            
            CreateTable(
                "Web.AspNetRoles1",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Web.AspNetUserRoles1",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("Web.AspNetRoles1", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "Web.BomDetails",
                c => new
                    {
                        BomDetailId = c.Int(nullable: false, identity: true),
                        BaseProductId = c.Int(nullable: false),
                        BatchQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProductId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ConsumptionPer = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProcessId = c.Int(),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        DevloperNotes = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.BomDetailId)
                .ForeignKey("Web.Products", t => t.BaseProductId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.BaseProductId)
                .Index(t => t.ProductId)
                .Index(t => t.ProcessId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductCode = c.String(nullable: false, maxLength: 20),
                        ProductName = c.String(nullable: false, maxLength: 50),
                        ProductDescription = c.String(maxLength: 1000),
                        ProductSpecification = c.String(),
                        StandardCost = c.Decimal(precision: 18, scale: 4),
                        ProductGroupId = c.Int(),
                        SalesTaxGroupProductId = c.Int(),
                        DrawBackTariffHeadId = c.Int(),
                        UnitId = c.String(maxLength: 3),
                        DivisionId = c.Int(nullable: false),
                        ImageFolderName = c.String(maxLength: 100),
                        ImageFileName = c.String(maxLength: 100),
                        StandardWeight = c.Decimal(precision: 18, scale: 4),
                        Tags = c.String(),
                        CBM = c.Decimal(precision: 18, scale: 4),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.DrawBackTariffHeads", t => t.DrawBackTariffHeadId)
                .ForeignKey("Web.ProductGroups", t => t.ProductGroupId)
                .ForeignKey("Web.SalesTaxGroupProducts", t => t.SalesTaxGroupProductId)
                .ForeignKey("Web.Units", t => t.UnitId)
                .Index(t => t.ProductCode, unique: true, name: "IX_Product_ProductCode")
                .Index(t => t.ProductName, unique: true, name: "IX_Product_ProductName")
                .Index(t => t.ProductGroupId)
                .Index(t => t.SalesTaxGroupProductId)
                .Index(t => t.DrawBackTariffHeadId)
                .Index(t => t.UnitId)
                .Index(t => t.DivisionId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.Divisions",
                c => new
                    {
                        DivisionId = c.Int(nullable: false, identity: true),
                        DivisionName = c.String(nullable: false, maxLength: 50),
                        Address = c.String(maxLength: 250),
                        LstNo = c.String(maxLength: 20),
                        CstNo = c.String(maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        ThemeColour = c.String(maxLength: 25),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DivisionId)
                .Index(t => t.DivisionName, unique: true, name: "IX_Division_Division");
            
            CreateTable(
                "Web.DrawBackTariffHeads",
                c => new
                    {
                        DrawBackTariffHeadId = c.Int(nullable: false, identity: true),
                        DrawBackTariffHeadName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DrawBackTariffHeadId)
                .Index(t => t.DrawBackTariffHeadName, unique: true, name: "IX_DrawBackTariffHead_DrawBackTariffHeadName");
            
            CreateTable(
                "Web.JobOrderBoms",
                c => new
                    {
                        JobOrderBomId = c.Int(nullable: false, identity: true),
                        JobOrderHeaderId = c.Int(nullable: false),
                        JobOrderLineId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobOrderBomId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.JobOrderHeaders", t => t.JobOrderHeaderId)
                .ForeignKey("Web.JobOrderLines", t => t.JobOrderLineId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.JobOrderHeaderId)
                .Index(t => t.JobOrderLineId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.Dimension1",
                c => new
                    {
                        Dimension1Id = c.Int(nullable: false, identity: true),
                        Dimension1Name = c.String(nullable: false, maxLength: 50),
                        ProductTypeId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 50),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Dimension1Id)
                .ForeignKey("Web.ProductTypes", t => t.ProductTypeId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .Index(t => t.Dimension1Name, unique: true, name: "IX_Dimension1_Dimension1Name")
                .Index(t => t.ProductTypeId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.ProductTypes",
                c => new
                    {
                        ProductTypeId = c.Int(nullable: false, identity: true),
                        ProductTypeName = c.String(nullable: false, maxLength: 50),
                        ProductNatureId = c.Int(nullable: false),
                        ImageFolderName = c.String(maxLength: 100),
                        ImageFileName = c.String(maxLength: 100),
                        IsCustomUI = c.Boolean(),
                        IsActive = c.Boolean(nullable: false),
                        IsPostedInStock = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        Dimension1TypeId = c.Int(),
                        Dimension2TypeId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductTypeId)
                .ForeignKey("Web.Dimension1Types", t => t.Dimension1TypeId)
                .ForeignKey("Web.Dimension2Types", t => t.Dimension2TypeId)
                .ForeignKey("Web.ProductNatures", t => t.ProductNatureId)
                .Index(t => t.ProductTypeName, unique: true, name: "IX_ProductType_ProductTypeName")
                .Index(t => t.ProductNatureId)
                .Index(t => t.Dimension1TypeId)
                .Index(t => t.Dimension2TypeId);
            
            CreateTable(
                "Web.Dimension1Types",
                c => new
                    {
                        Dimension1TypeId = c.Int(nullable: false, identity: true),
                        Dimension1TypeName = c.String(nullable: false, maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Dimension1TypeId)
                .Index(t => t.Dimension1TypeName, unique: true, name: "IX_Dimension1Type_Dimension1TypeName");
            
            CreateTable(
                "Web.Dimension2Types",
                c => new
                    {
                        Dimension2TypeId = c.Int(nullable: false, identity: true),
                        Dimension2TypeName = c.String(nullable: false, maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Dimension2TypeId)
                .Index(t => t.Dimension2TypeName, unique: true, name: "IX_Dimension2Type_Dimension2TypeName");
            
            CreateTable(
                "Web.ProductCategories",
                c => new
                    {
                        ProductCategoryId = c.Int(nullable: false, identity: true),
                        ProductCategoryName = c.String(nullable: false, maxLength: 50),
                        ProductTypeId = c.Int(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ImageFolderName = c.String(maxLength: 100),
                        ImageFileName = c.String(maxLength: 100),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductCategoryId)
                .ForeignKey("Web.ProductTypes", t => t.ProductTypeId)
                .Index(t => t.ProductCategoryName, unique: true, name: "IX_ProductCategory_ProductCategoryName")
                .Index(t => t.ProductTypeId);
            
            CreateTable(
                "Web.ProductDesigns",
                c => new
                    {
                        ProductDesignId = c.Int(nullable: false, identity: true),
                        ProductDesignName = c.String(nullable: false, maxLength: 50),
                        ProductTypeId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductDesignId)
                .ForeignKey("Web.ProductTypes", t => t.ProductTypeId)
                .Index(t => t.ProductDesignName, unique: true, name: "IX_ProductDesign_ProductDesignName")
                .Index(t => t.ProductTypeId);
            
            CreateTable(
                "Web.ProductGroups",
                c => new
                    {
                        ProductGroupId = c.Int(nullable: false, identity: true),
                        ProductGroupName = c.String(nullable: false, maxLength: 50),
                        ProductTypeId = c.Int(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ImageFolderName = c.String(maxLength: 100),
                        ImageFileName = c.String(maxLength: 100),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductGroupId)
                .ForeignKey("Web.ProductTypes", t => t.ProductTypeId)
                .Index(t => t.ProductGroupName, unique: true, name: "IX_ProductGroup_ProductGroupName")
                .Index(t => t.ProductTypeId);
            
            CreateTable(
                "Web.ProductNatures",
                c => new
                    {
                        ProductNatureId = c.Int(nullable: false, identity: true),
                        ProductNatureName = c.String(nullable: false, maxLength: 50),
                        ImageFolderName = c.String(maxLength: 100),
                        ImageFileName = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductNatureId)
                .Index(t => t.ProductNatureName, unique: true, name: "IX_ProductNature_ProductNatureName");
            
            CreateTable(
                "Web.ProductTypeAttributes",
                c => new
                    {
                        ProductTypeAttributeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsMandatory = c.Boolean(nullable: false),
                        DataType = c.String(),
                        ListItem = c.String(),
                        DefaultValue = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        ProductType_ProductTypeId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductTypeAttributeId)
                .ForeignKey("Web.ProductTypes", t => t.ProductType_ProductTypeId)
                .Index(t => t.ProductType_ProductTypeId);
            
            CreateTable(
                "Web.Dimension2",
                c => new
                    {
                        Dimension2Id = c.Int(nullable: false, identity: true),
                        Dimension2Name = c.String(nullable: false, maxLength: 50),
                        ProductTypeId = c.Int(),
                        IsSystemDefine = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 50),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Dimension2Id)
                .ForeignKey("Web.ProductTypes", t => t.ProductTypeId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .Index(t => t.Dimension2Name, unique: true, name: "IX_Dimension2_Dimension2Name")
                .Index(t => t.ProductTypeId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.JobOrderHeaders",
                c => new
                    {
                        JobOrderHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        ActualDueDate = c.DateTime(nullable: false),
                        ActualDocDate = c.DateTime(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        BillToPartyId = c.Int(nullable: false),
                        OrderById = c.Int(),
                        GodownId = c.Int(),
                        ProcessId = c.Int(nullable: false),
                        CostCenterId = c.Int(),
                        MachineId = c.Int(),
                        TermsAndConditions = c.String(),
                        CreditDays = c.Int(),
                        Remark = c.String(),
                        GatePassHeaderId = c.Int(),
                        StockHeaderId = c.Int(),
                        Status = c.Int(nullable: false),
                        UnitConversionForId = c.Byte(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        Priority = c.Int(nullable: false),
                        IsGatePassPrinted = c.Boolean(),
                        OMSId = c.String(maxLength: 50),
                        ReferentialCheckSum = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobOrderHeaderId)
                .ForeignKey("Web.JobWorkers", t => t.BillToPartyId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.GatePassHeaders", t => t.GatePassHeaderId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.Machines", t => t.MachineId)
                .ForeignKey("Web.Employees", t => t.OrderById)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.StockHeaders", t => t.StockHeaderId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.BillToPartyId)
                .Index(t => t.OrderById)
                .Index(t => t.GodownId)
                .Index(t => t.ProcessId)
                .Index(t => t.CostCenterId)
                .Index(t => t.MachineId)
                .Index(t => t.GatePassHeaderId)
                .Index(t => t.StockHeaderId)
                .Index(t => t.UnitConversionForId);
            
            CreateTable(
                "Web.JobWorkers",
                c => new
                    {
                        PersonID = c.Int(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("Web.People", t => t.PersonID)
                .Index(t => t.PersonID);
            
            CreateTable(
                "Web.CostCenters",
                c => new
                    {
                        CostCenterId = c.Int(nullable: false, identity: true),
                        CostCenterName = c.String(nullable: false, maxLength: 50),
                        DocTypeId = c.Int(),
                        DivisionId = c.Int(),
                        SiteId = c.Int(),
                        Status = c.Int(nullable: false),
                        LedgerAccountId = c.Int(),
                        ParentCostCenterId = c.Int(),
                        ProcessId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        ReferenceDocTypeId = c.Int(),
                        StartDate = c.DateTime(),
                        CloseDate = c.DateTime(),
                        ReferenceDocId = c.Int(),
                        ReferenceDocNo = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CostCenterId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.ParentCostCenterId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.CostCenterName, unique: true, name: "IX_CostCenter_CostCenterName")
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.LedgerAccountId)
                .Index(t => t.ParentCostCenterId)
                .Index(t => t.ProcessId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.LedgerAccounts",
                c => new
                    {
                        LedgerAccountId = c.Int(nullable: false, identity: true),
                        LedgerAccountName = c.String(nullable: false, maxLength: 100),
                        LedgerAccountSuffix = c.String(nullable: false, maxLength: 20),
                        PersonId = c.Int(),
                        LedgerAccountGroupId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.LedgerAccountId)
                .ForeignKey("Web.LedgerAccountGroups", t => t.LedgerAccountGroupId)
                .ForeignKey("Web.People", t => t.PersonId)
                .Index(t => new { t.LedgerAccountName, t.LedgerAccountSuffix }, unique: true, name: "IX_LedgerAccount_LedgerAccountName")
                .Index(t => t.PersonId)
                .Index(t => t.LedgerAccountGroupId);
            
            CreateTable(
                "Web.LedgerAccountGroups",
                c => new
                    {
                        LedgerAccountGroupId = c.Int(nullable: false, identity: true),
                        LedgerAccountGroupName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        LedgerAccountType = c.String(maxLength: 10),
                        LedgerAccountNature = c.String(maxLength: 10),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.LedgerAccountGroupId)
                .Index(t => t.LedgerAccountGroupName, unique: true, name: "IX_LedgerAccountGroup_LedgerAccountGroupName");
            
            CreateTable(
                "Web.Processes",
                c => new
                    {
                        ProcessId = c.Int(nullable: false, identity: true),
                        ProcessCode = c.String(nullable: false, maxLength: 50),
                        ProcessName = c.String(nullable: false, maxLength: 50),
                        ParentProcessId = c.Int(),
                        AccountId = c.Int(nullable: false),
                        IsAffectedStock = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CostCenterId = c.Int(),
                        ProcessSr = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProcessId)
                .ForeignKey("Web.LedgerAccounts", t => t.AccountId)
                .ForeignKey("Web.Processes", t => t.ParentProcessId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .Index(t => t.ProcessCode, unique: true, name: "IX_Process_ProcessCode")
                .Index(t => t.ProcessName, unique: true, name: "IX_Process_ProcessName")
                .Index(t => t.ParentProcessId)
                .Index(t => t.AccountId)
                .Index(t => t.CostCenterId);
            
            CreateTable(
                "Web.Sites",
                c => new
                    {
                        SiteId = c.Int(nullable: false, identity: true),
                        SiteName = c.String(nullable: false, maxLength: 50),
                        SiteCode = c.String(maxLength: 250),
                        Address = c.String(maxLength: 20),
                        PhoneNo = c.String(maxLength: 50),
                        CityId = c.Int(nullable: false),
                        PersonId = c.Int(),
                        DefaultGodownId = c.Int(),
                        DefaultDivisionId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        ThemeColour = c.String(maxLength: 25),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SiteId)
                .ForeignKey("Web.Cities", t => t.CityId)
                .ForeignKey("Web.Divisions", t => t.DefaultDivisionId)
                .ForeignKey("Web.Godowns", t => t.DefaultGodownId)
                .ForeignKey("Web.People", t => t.PersonId)
                .Index(t => t.CityId)
                .Index(t => t.PersonId)
                .Index(t => t.DefaultGodownId)
                .Index(t => t.DefaultDivisionId);
            
            CreateTable(
                "Web.Godowns",
                c => new
                    {
                        GodownId = c.Int(nullable: false, identity: true),
                        GodownName = c.String(nullable: false, maxLength: 50),
                        SiteId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        GateId = c.Int(),
                    })
                .PrimaryKey(t => t.GodownId)
                .ForeignKey("Web.Gates", t => t.GateId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.GodownName, unique: true, name: "IX_Godown_GodownName")
                .Index(t => t.SiteId)
                .Index(t => t.GateId);
            
            CreateTable(
                "Web.Gates",
                c => new
                    {
                        GateId = c.Int(nullable: false, identity: true),
                        GateName = c.String(nullable: false, maxLength: 50),
                        SiteId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.GateId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.GateName, unique: true, name: "IX_Gate_GateName")
                .Index(t => t.SiteId);
            
            CreateTable(
                "Web.GatePassHeaders",
                c => new
                    {
                        GatePassHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
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
                .PrimaryKey(t => t.GatePassHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.People", t => t.PersonId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId }, unique: true, name: "IX_GatePassHeader_DocID")
                .Index(t => t.SiteId)
                .Index(t => t.PersonId)
                .Index(t => t.GodownId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.GatePassLines",
                c => new
                    {
                        GatePassLineId = c.Int(nullable: false, identity: true),
                        GatePassHeaderId = c.Int(nullable: false),
                        Product = c.String(nullable: false, maxLength: 100),
                        Specification = c.String(maxLength: 50),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitId = c.String(maxLength: 3),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.GatePassLineId)
                .ForeignKey("Web.GatePassHeaders", t => t.GatePassHeaderId)
                .ForeignKey("Web.Units", t => t.UnitId)
                .Index(t => t.GatePassHeaderId)
                .Index(t => t.UnitId);
            
            CreateTable(
                "Web.Units",
                c => new
                    {
                        UnitId = c.String(nullable: false, maxLength: 3),
                        UnitName = c.String(nullable: false, maxLength: 20),
                        Symbol = c.String(maxLength: 1),
                        FractionName = c.String(maxLength: 20),
                        FractionUnits = c.Int(),
                        FractionSymbol = c.String(maxLength: 1),
                        DecimalPlaces = c.Byte(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.UnitId)
                .Index(t => t.UnitName, unique: true, name: "IX_Unit_UnitName");
            
            CreateTable(
                "Web.JobOrderLines",
                c => new
                    {
                        JobOrderLineId = c.Int(nullable: false, identity: true),
                        JobOrderHeaderId = c.Int(nullable: false),
                        ProductUidId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        ProdOrderLineId = c.Int(),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Specification = c.String(maxLength: 50),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DueDate = c.DateTime(),
                        LotNo = c.String(maxLength: 50),
                        FromProcessId = c.Int(),
                        UnitId = c.String(maxLength: 3),
                        DealUnitId = c.String(maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        NonCountedQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        LossQty = c.Decimal(precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        StockId = c.Int(),
                        StockProcessId = c.Int(),
                        ProductUidHeaderId = c.Int(),
                        Sr = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        ProductUidLastTransactionDocId = c.Int(),
                        ProductUidLastTransactionDocNo = c.String(),
                        ProductUidLastTransactionDocTypeId = c.Int(),
                        ProductUidLastTransactionDocDate = c.DateTime(),
                        ProductUidLastTransactionPersonId = c.Int(),
                        ProductUidCurrentGodownId = c.Int(),
                        ProductUidCurrentProcessId = c.Int(),
                        ProductUidStatus = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.JobOrderLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Processes", t => t.FromProcessId)
                .ForeignKey("Web.JobOrderHeaders", t => t.JobOrderHeaderId)
                .ForeignKey("Web.ProdOrderLines", t => t.ProdOrderLineId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.Godowns", t => t.ProductUidCurrentGodownId)
                .ForeignKey("Web.Processes", t => t.ProductUidCurrentProcessId)
                .ForeignKey("Web.ProductUidHeaders", t => t.ProductUidHeaderId)
                .ForeignKey("Web.DocumentTypes", t => t.ProductUidLastTransactionDocTypeId)
                .ForeignKey("Web.People", t => t.ProductUidLastTransactionPersonId)
                .ForeignKey("Web.Stocks", t => t.StockId)
                .ForeignKey("Web.StockProcesses", t => t.StockProcessId)
                .ForeignKey("Web.Units", t => t.UnitId)
                .Index(t => t.JobOrderHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.ProductId)
                .Index(t => t.ProdOrderLineId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.FromProcessId)
                .Index(t => t.UnitId)
                .Index(t => t.DealUnitId)
                .Index(t => t.StockId)
                .Index(t => t.StockProcessId)
                .Index(t => t.ProductUidHeaderId)
                .Index(t => t.ProductUidLastTransactionDocTypeId)
                .Index(t => t.ProductUidLastTransactionPersonId)
                .Index(t => t.ProductUidCurrentGodownId)
                .Index(t => t.ProductUidCurrentProcessId);
            
            CreateTable(
                "Web.ProdOrderLines",
                c => new
                    {
                        ProdOrderLineId = c.Int(nullable: false, identity: true),
                        ProdOrderHeaderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Specification = c.String(maxLength: 50),
                        ProcessId = c.Int(),
                        MaterialPlanLineId = c.Int(),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocLineId = c.Int(),
                        Sr = c.Int(),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProdOrderLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.MaterialPlanLines", t => t.MaterialPlanLineId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.ProdOrderHeaders", t => t.ProdOrderHeaderId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .Index(t => t.ProdOrderHeaderId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.ProcessId)
                .Index(t => t.MaterialPlanLineId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.MaterialPlanLines",
                c => new
                    {
                        MaterialPlanLineId = c.Int(nullable: false, identity: true),
                        MaterialPlanHeaderId = c.Int(nullable: false),
                        GeneratedFor = c.String(maxLength: 10),
                        ProductId = c.Int(nullable: false),
                        RequiredQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DueDate = c.DateTime(),
                        ExcessStockQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        StockPlanQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProdPlanQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PurchPlanQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProcessId = c.Int(),
                        Remark = c.String(),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Specification = c.String(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        Sr = c.Int(),
                    })
                .PrimaryKey(t => t.MaterialPlanLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.MaterialPlanHeaders", t => t.MaterialPlanHeaderId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.MaterialPlanHeaderId)
                .Index(t => t.ProductId)
                .Index(t => t.ProcessId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.MaterialPlanHeaders",
                c => new
                    {
                        MaterialPlanHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 20),
                        BuyerId = c.Int(),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        GodownId = c.Int(),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        LockReason = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.MaterialPlanHeaderId)
                .ForeignKey("Web.Buyers", t => t.BuyerId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.BuyerId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.GodownId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.Buyers",
                c => new
                    {
                        PersonID = c.Int(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("Web.People", t => t.PersonID)
                .Index(t => t.PersonID);
            
            CreateTable(
                "Web.ProdOrderHeaders",
                c => new
                    {
                        ProdOrderHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        BuyerId = c.Int(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        MaterialPlanHeaderId = c.Int(),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProdOrderHeaderId)
                .ForeignKey("Web.Buyers", t => t.BuyerId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.MaterialPlanHeaders", t => t.MaterialPlanHeaderId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.ReferenceDocTypeId)
                .Index(t => t.BuyerId)
                .Index(t => t.MaterialPlanHeaderId);
            
            CreateTable(
                "Web.ProductUids",
                c => new
                    {
                        ProductUIDId = c.Int(nullable: false, identity: true),
                        ProductUidName = c.String(nullable: false, maxLength: 50),
                        ProductUidHeaderId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        LotNo = c.String(maxLength: 50),
                        GenDocId = c.Int(),
                        GenLineId = c.Int(),
                        GenDocNo = c.String(),
                        GenDocTypeId = c.Int(),
                        GenDocDate = c.DateTime(),
                        GenPersonId = c.Int(),
                        LastTransactionDocId = c.Int(),
                        LastTransactionLineId = c.Int(),
                        LastTransactionDocNo = c.String(),
                        LastTransactionDocTypeId = c.Int(),
                        LastTransactionDocDate = c.DateTime(),
                        LastTransactionPersonId = c.Int(),
                        CurrenctGodownId = c.Int(),
                        CurrenctProcessId = c.Int(),
                        Status = c.String(maxLength: 10),
                        ProcessesDone = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductUIDId)
                .ForeignKey("Web.Godowns", t => t.CurrenctGodownId)
                .ForeignKey("Web.Processes", t => t.CurrenctProcessId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.DocumentTypes", t => t.GenDocTypeId)
                .ForeignKey("Web.Buyers", t => t.GenPersonId)
                .ForeignKey("Web.DocumentTypes", t => t.LastTransactionDocTypeId)
                .ForeignKey("Web.People", t => t.LastTransactionPersonId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductUidHeaders", t => t.ProductUidHeaderId)
                .Index(t => t.ProductUidName, unique: true, name: "IX_ProductUid_ProductUidName")
                .Index(t => t.ProductUidHeaderId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.GenDocTypeId)
                .Index(t => t.GenPersonId)
                .Index(t => t.LastTransactionDocTypeId)
                .Index(t => t.LastTransactionPersonId)
                .Index(t => t.CurrenctGodownId)
                .Index(t => t.CurrenctProcessId);
            
            CreateTable(
                "Web.PackingLines",
                c => new
                    {
                        PackingLineId = c.Int(nullable: false, identity: true),
                        PackingHeaderId = c.Int(nullable: false),
                        ProductUidId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        SaleOrderLineId = c.Int(),
                        SaleDeliveryOrderLineId = c.Int(),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BaleNo = c.String(maxLength: 10),
                        LotNo = c.String(maxLength: 50),
                        FromProcessId = c.Int(),
                        PartyProductUid = c.String(maxLength: 50),
                        DealUnitId = c.String(nullable: false, maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        GrossWeight = c.Decimal(nullable: false, precision: 18, scale: 4),
                        NetWeight = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        Specification = c.String(),
                        LockReason = c.String(),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        StockIssueId = c.Int(),
                        StockReceiveId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        ProductUidLastTransactionDocId = c.Int(),
                        ProductUidLastTransactionDocNo = c.String(),
                        ProductUidLastTransactionDocTypeId = c.Int(),
                        ProductUidLastTransactionDocDate = c.DateTime(),
                        ProductUidLastTransactionPersonId = c.Int(),
                        ProductUidCurrentGodownId = c.Int(),
                        ProductUidCurrentProcessId = c.Int(),
                        ProductUidStatus = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.PackingLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Processes", t => t.FromProcessId)
                .ForeignKey("Web.PackingHeaders", t => t.PackingHeaderId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.Godowns", t => t.ProductUidCurrentGodownId)
                .ForeignKey("Web.Processes", t => t.ProductUidCurrentProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.ProductUidLastTransactionDocTypeId)
                .ForeignKey("Web.People", t => t.ProductUidLastTransactionPersonId)
                .ForeignKey("Web.SaleDeliveryOrderLines", t => t.SaleDeliveryOrderLineId)
                .ForeignKey("Web.SaleOrderLines", t => t.SaleOrderLineId)
                .ForeignKey("Web.Stocks", t => t.StockIssueId)
                .ForeignKey("Web.Stocks", t => t.StockReceiveId)
                .Index(t => t.PackingHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.SaleOrderLineId)
                .Index(t => t.SaleDeliveryOrderLineId)
                .Index(t => t.FromProcessId)
                .Index(t => t.DealUnitId)
                .Index(t => t.StockIssueId)
                .Index(t => t.StockReceiveId)
                .Index(t => t.ProductUidLastTransactionDocTypeId)
                .Index(t => t.ProductUidLastTransactionPersonId)
                .Index(t => t.ProductUidCurrentGodownId)
                .Index(t => t.ProductUidCurrentProcessId);
            
            CreateTable(
                "Web.PackingHeaders",
                c => new
                    {
                        PackingHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        StockHeaderId = c.Int(),
                        BuyerId = c.Int(nullable: false),
                        JobWorkerId = c.Int(),
                        GodownId = c.Int(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        DealUnitId = c.String(maxLength: 3),
                        BaleNoPattern = c.Byte(nullable: false),
                        Remark = c.String(),
                        LockReason = c.String(),
                        Status = c.Int(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PackingHeaderId)
                .ForeignKey("Web.People", t => t.BuyerId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.ShipMethods", t => t.ShipMethodId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.StockHeaders", t => t.StockHeaderId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_PackingHeader_DocID")
                .Index(t => t.StockHeaderId)
                .Index(t => t.BuyerId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.GodownId)
                .Index(t => t.ShipMethodId)
                .Index(t => t.DealUnitId);
            
            CreateTable(
                "Web.ShipMethods",
                c => new
                    {
                        ShipMethodId = c.Int(nullable: false, identity: true),
                        ShipMethodName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ShipMethodId)
                .Index(t => t.ShipMethodName, unique: true, name: "IX_ShipMethod_ShipMethodName");
            
            CreateTable(
                "Web.StockHeaders",
                c => new
                    {
                        StockHeaderId = c.Int(nullable: false, identity: true),
                        DocHeaderId = c.Int(),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        CurrencyId = c.Int(),
                        PersonId = c.Int(),
                        ProcessId = c.Int(),
                        FromGodownId = c.Int(),
                        GodownId = c.Int(),
                        GatePassHeaderId = c.Int(),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        CostCenterId = c.Int(),
                        MachineId = c.Int(),
                        LedgerHeaderId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        LedgerAccountId = c.Int(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        LockReason = c.String(),
                        IsGatePassPrinted = c.Boolean(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.StockHeaderId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Currencies", t => t.CurrencyId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Godowns", t => t.FromGodownId)
                .ForeignKey("Web.GatePassHeaders", t => t.GatePassHeaderId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountId)
                .ForeignKey("Web.LedgerHeaders", t => t.LedgerHeaderId)
                .ForeignKey("Web.Machines", t => t.MachineId)
                .ForeignKey("Web.People", t => t.PersonId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_StockHeader_DocID")
                .Index(t => t.CurrencyId)
                .Index(t => t.PersonId)
                .Index(t => t.ProcessId)
                .Index(t => t.FromGodownId)
                .Index(t => t.GodownId)
                .Index(t => t.GatePassHeaderId)
                .Index(t => t.CostCenterId)
                .Index(t => t.MachineId)
                .Index(t => t.LedgerHeaderId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.LedgerAccountId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.Currencies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Symbol = c.String(nullable: false, maxLength: 5),
                        FractionName = c.String(maxLength: 20),
                        FractionUnits = c.Int(),
                        FractionSymbol = c.String(maxLength: 1),
                        BaseCurrencyRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true, name: "IX_Currency_Name");
            
            CreateTable(
                "Web.LedgerHeaders",
                c => new
                    {
                        LedgerHeaderId = c.Int(nullable: false, identity: true),
                        DocHeaderId = c.Int(),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        PaymentFor = c.DateTime(),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        LedgerAccountId = c.Int(),
                        ProcessId = c.Int(),
                        CostCenterId = c.Int(),
                        CreditDays = c.Int(),
                        GodownId = c.Int(),
                        Narration = c.String(),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        LockReason = c.String(),
                        AdjustmentType = c.String(maxLength: 20),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.LedgerHeaderId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_LedgerHeader_DocID")
                .Index(t => t.LedgerAccountId)
                .Index(t => t.ProcessId)
                .Index(t => t.CostCenterId)
                .Index(t => t.GodownId);
            
            CreateTable(
                "Web.Machines",
                c => new
                    {
                        MachineId = c.Int(nullable: false, identity: true),
                        MachineName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.MachineId)
                .Index(t => t.MachineName, unique: true, name: "IX_Machine_MachineName");
            
            CreateTable(
                "Web.StockLines",
                c => new
                    {
                        StockLineId = c.Int(nullable: false, identity: true),
                        StockHeaderId = c.Int(nullable: false),
                        ProductUidId = c.Int(),
                        RequisitionLineId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        FromProcessId = c.Int(),
                        LotNo = c.String(maxLength: 10),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        FromStockId = c.Int(),
                        StockId = c.Int(),
                        StockProcessId = c.Int(),
                        FromStockProcessId = c.Int(),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Specification = c.String(maxLength: 50),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CostCenterId = c.Int(),
                        DocNature = c.String(maxLength: 1),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        Sr = c.Int(),
                        Weight = c.Decimal(precision: 18, scale: 4),
                        ProductUidLastTransactionDocId = c.Int(),
                        ProductUidLastTransactionDocNo = c.String(),
                        ProductUidLastTransactionDocTypeId = c.Int(),
                        ProductUidLastTransactionDocDate = c.DateTime(),
                        ProductUidLastTransactionPersonId = c.Int(),
                        ProductUidCurrentGodownId = c.Int(),
                        ProductUidCurrentProcessId = c.Int(),
                        ProductUidStatus = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.StockLineId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Processes", t => t.FromProcessId)
                .ForeignKey("Web.Stocks", t => t.FromStockId)
                .ForeignKey("Web.StockProcesses", t => t.FromStockProcessId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.Godowns", t => t.ProductUidCurrentGodownId)
                .ForeignKey("Web.Processes", t => t.ProductUidCurrentProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.ProductUidLastTransactionDocTypeId)
                .ForeignKey("Web.People", t => t.ProductUidLastTransactionPersonId)
                .ForeignKey("Web.RequisitionLines", t => t.RequisitionLineId)
                .ForeignKey("Web.Stocks", t => t.StockId)
                .ForeignKey("Web.StockHeaders", t => t.StockHeaderId)
                .ForeignKey("Web.StockProcesses", t => t.StockProcessId)
                .Index(t => t.StockHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.RequisitionLineId)
                .Index(t => t.ProductId)
                .Index(t => t.FromProcessId)
                .Index(t => t.FromStockId)
                .Index(t => t.StockId)
                .Index(t => t.StockProcessId)
                .Index(t => t.FromStockProcessId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.CostCenterId)
                .Index(t => t.ProductUidLastTransactionDocTypeId)
                .Index(t => t.ProductUidLastTransactionPersonId)
                .Index(t => t.ProductUidCurrentGodownId)
                .Index(t => t.ProductUidCurrentProcessId);
            
            CreateTable(
                "Web.Stocks",
                c => new
                    {
                        StockId = c.Int(nullable: false, identity: true),
                        StockHeaderId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        ProductUidId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        ProcessId = c.Int(),
                        GodownId = c.Int(nullable: false),
                        LotNo = c.String(maxLength: 50),
                        CostCenterId = c.Int(),
                        Qty_Iss = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Qty_Rec = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        ExpiryDate = c.DateTime(),
                        Specification = c.String(),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        DocQty_Iss = c.Decimal(precision: 18, scale: 4),
                        DocQty_Rec = c.Decimal(precision: 18, scale: 4),
                        Weight_Iss = c.Decimal(precision: 18, scale: 4),
                        Weight_Rec = c.Decimal(precision: 18, scale: 4),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.StockId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.StockHeaders", t => t.StockHeaderId)
                .Index(t => t.StockHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.ProductId)
                .Index(t => t.ProcessId)
                .Index(t => t.GodownId)
                .Index(t => t.CostCenterId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.StockProcesses",
                c => new
                    {
                        StockProcessId = c.Int(nullable: false, identity: true),
                        StockHeaderId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ProductUidId = c.Int(),
                        ProcessId = c.Int(),
                        GodownId = c.Int(),
                        LotNo = c.String(maxLength: 50),
                        CostCenterId = c.Int(),
                        Qty_Iss = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Qty_Rec = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Weight_Iss = c.Decimal(precision: 18, scale: 4),
                        Weight_Rec = c.Decimal(precision: 18, scale: 4),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        ExpiryDate = c.DateTime(),
                        Specification = c.String(),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Remark = c.String(),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.StockProcessId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.StockHeaders", t => t.StockHeaderId)
                .Index(t => t.StockHeaderId)
                .Index(t => t.ProductId)
                .Index(t => t.ProductUidId)
                .Index(t => t.ProcessId)
                .Index(t => t.GodownId)
                .Index(t => t.CostCenterId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.RequisitionLines",
                c => new
                    {
                        RequisitionLineId = c.Int(nullable: false, identity: true),
                        RequisitionHeaderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Specification = c.String(maxLength: 50),
                        DueDate = c.DateTime(),
                        ProcessId = c.Int(),
                        LockReason = c.String(),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RequisitionLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.RequisitionHeaders", t => t.RequisitionHeaderId)
                .Index(t => t.RequisitionHeaderId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.ProcessId);
            
            CreateTable(
                "Web.RequisitionHeaders",
                c => new
                    {
                        RequisitionHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        CostCenterId = c.Int(),
                        ReasonId = c.Int(nullable: false),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        LockReason = c.String(),
                        Remark = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RequisitionHeaderId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.People", t => t.PersonId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_RequisitionHeader_DocID")
                .Index(t => t.PersonId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ReasonId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.Reasons",
                c => new
                    {
                        ReasonId = c.Int(nullable: false, identity: true),
                        ReasonName = c.String(nullable: false, maxLength: 100),
                        DocumentCategoryId = c.Int(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ReasonId)
                .ForeignKey("Web.DocumentCategories", t => t.DocumentCategoryId)
                .Index(t => new { t.ReasonName, t.DocumentCategoryId }, unique: true, name: "IX_Reason_ReasonName");
            
            CreateTable(
                "Web.SaleDeliveryOrderLines",
                c => new
                    {
                        SaleDeliveryOrderLineId = c.Int(nullable: false, identity: true),
                        SaleOrderLineId = c.Int(nullable: false),
                        SaleDeliveryOrderHeaderId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DueDate = c.DateTime(),
                        Remark = c.String(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Sr = c.Int(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleDeliveryOrderLineId)
                .ForeignKey("Web.SaleDeliveryOrderHeaders", t => t.SaleDeliveryOrderHeaderId)
                .ForeignKey("Web.SaleOrderLines", t => t.SaleOrderLineId)
                .Index(t => t.SaleOrderLineId)
                .Index(t => t.SaleDeliveryOrderHeaderId);
            
            CreateTable(
                "Web.SaleDeliveryOrderHeaders",
                c => new
                    {
                        SaleDeliveryOrderHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        BuyerId = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        ShipAddress = c.String(maxLength: 250),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        LockReason = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleDeliveryOrderHeaderId)
                .ForeignKey("Web.Buyers", t => t.BuyerId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.ShipMethods", t => t.ShipMethodId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.BuyerId)
                .Index(t => t.ShipMethodId);
            
            CreateTable(
                "Web.SaleOrderLines",
                c => new
                    {
                        SaleOrderLineId = c.Int(nullable: false, identity: true),
                        SaleOrderHeaderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Specification = c.String(maxLength: 50),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DueDate = c.DateTime(),
                        DealUnitId = c.String(maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitConversionMultiplier = c.Decimal(precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DiscountPer = c.Decimal(precision: 18, scale: 4),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocLineId = c.Int(),
                        Remark = c.String(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleOrderLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.SaleOrderHeaders", t => t.SaleOrderHeaderId)
                .Index(t => new { t.SaleOrderHeaderId, t.ProductId, t.DueDate }, unique: true, name: "IX_SaleOrderLine_SaleOrdeHeaderProductDueDate")
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.DealUnitId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.SaleOrderHeaders",
                c => new
                    {
                        SaleOrderHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        BuyerOrderNo = c.String(maxLength: 20),
                        DueDate = c.DateTime(nullable: false),
                        ActualDueDate = c.DateTime(nullable: false),
                        SaleToBuyerId = c.Int(nullable: false),
                        BillToBuyerId = c.Int(nullable: false),
                        CurrencyId = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        ShipAddress = c.String(maxLength: 250),
                        DeliveryTermsId = c.Int(nullable: false),
                        TermsAndConditions = c.String(),
                        CreditDays = c.Int(nullable: false),
                        LedgerHeaderId = c.Int(),
                        Status = c.Int(nullable: false),
                        UnitConversionForId = c.Byte(nullable: false),
                        Advance = c.Decimal(precision: 18, scale: 4),
                        Remark = c.String(),
                        LockReason = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleOrderHeaderId)
                .ForeignKey("Web.Buyers", t => t.BillToBuyerId)
                .ForeignKey("Web.Currencies", t => t.CurrencyId)
                .ForeignKey("Web.DeliveryTerms", t => t.DeliveryTermsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.LedgerHeaders", t => t.LedgerHeaderId)
                .ForeignKey("Web.People", t => t.SaleToBuyerId)
                .ForeignKey("Web.ShipMethods", t => t.ShipMethodId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.SaleToBuyerId)
                .Index(t => t.BillToBuyerId)
                .Index(t => t.CurrencyId)
                .Index(t => t.ShipMethodId)
                .Index(t => t.DeliveryTermsId)
                .Index(t => t.LedgerHeaderId)
                .Index(t => t.UnitConversionForId);
            
            CreateTable(
                "Web.DeliveryTerms",
                c => new
                    {
                        DeliveryTermsId = c.Int(nullable: false, identity: true),
                        DeliveryTermsName = c.String(nullable: false, maxLength: 50),
                        PrintingDescription = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DeliveryTermsId)
                .Index(t => t.DeliveryTermsName, unique: true, name: "IX_DeliveryTerms_DeliveryTermsName");
            
            CreateTable(
                "Web.UnitConversionFors",
                c => new
                    {
                        UnitconversionForId = c.Byte(nullable: false),
                        UnitconversionForName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.UnitconversionForId);
            
            CreateTable(
                "Web.ProductUidHeaders",
                c => new
                    {
                        ProductUidHeaderId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        LotNo = c.String(maxLength: 50),
                        GenDocId = c.Int(),
                        GenDocNo = c.String(),
                        GenDocTypeId = c.Int(),
                        GenDocDate = c.DateTime(),
                        GenPersonId = c.Int(),
                        GenRemark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductUidHeaderId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.DocumentTypes", t => t.GenDocTypeId)
                .ForeignKey("Web.People", t => t.GenPersonId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.GenDocTypeId)
                .Index(t => t.GenPersonId);
            
            CreateTable(
                "Web.Employees",
                c => new
                    {
                        PersonID = c.Int(nullable: false),
                        DesignationID = c.Int(nullable: false),
                        DepartmentID = c.Int(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("Web.Departments", t => t.DepartmentID)
                .ForeignKey("Web.Designations", t => t.DesignationID)
                .ForeignKey("Web.People", t => t.PersonID)
                .Index(t => t.PersonID)
                .Index(t => t.DesignationID)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "Web.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DepartmentId)
                .Index(t => t.DepartmentName, unique: true, name: "IX_Department_DepartmentName");
            
            CreateTable(
                "Web.Designations",
                c => new
                    {
                        DesignationId = c.Int(nullable: false, identity: true),
                        DesignationName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DesignationId)
                .Index(t => t.DesignationName, unique: true, name: "IX_Designation_DesignationName");
            
            CreateTable(
                "Web.ProductAttributes",
                c => new
                    {
                        ProductAttributeId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        ProductTypeAttributeId = c.Int(nullable: false),
                        ProductAttributeValue = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductAttributeId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Web.ProductBuyers",
                c => new
                    {
                        ProductBuyerId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        BuyerId = c.Int(nullable: false),
                        BuyerSku = c.String(maxLength: 50),
                        BuyerUpcCode = c.String(maxLength: 20),
                        BuyerSpecification = c.String(maxLength: 50),
                        BuyerSpecification1 = c.String(maxLength: 50),
                        BuyerSpecification2 = c.String(maxLength: 50),
                        BuyerSpecification3 = c.String(maxLength: 50),
                        BuyerSpecification4 = c.String(maxLength: 50),
                        BuyerSpecification5 = c.String(maxLength: 50),
                        BuyerSpecification6 = c.String(maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductBuyerId)
                .ForeignKey("Web.Buyers", t => t.BuyerId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.BuyerId);
            
            CreateTable(
                "Web.ProductIncludedAccessories",
                c => new
                    {
                        ProductIncludedAccessoriesId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        AccessoryId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        Product_ProductId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductIncludedAccessoriesId)
                .ForeignKey("Web.Products", t => t.AccessoryId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.Products", t => t.Product_ProductId)
                .Index(t => t.ProductId, unique: true, name: "IX_ProductIncludedAccessories_ProductId")
                .Index(t => t.AccessoryId, unique: true, name: "IX_ProductIncludedAccessories_AccessoryId")
                .Index(t => t.Product_ProductId);
            
            CreateTable(
                "Web.ProductRelatedAccessories",
                c => new
                    {
                        ProductRelatedAccessoriesId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        AccessoryId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        Product_ProductId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductRelatedAccessoriesId)
                .ForeignKey("Web.Products", t => t.AccessoryId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.Products", t => t.Product_ProductId)
                .Index(t => t.ProductId, unique: true, name: "IX_ProductRelatedAccessories_ProductId")
                .Index(t => t.AccessoryId, unique: true, name: "IX_ProductRelatedAccessories_AccessoryId")
                .Index(t => t.Product_ProductId);
            
            CreateTable(
                "Web.ProductSizes",
                c => new
                    {
                        ProductSizeId = c.Int(nullable: false, identity: true),
                        ProductSizeTypeId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        SizeId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductSizeId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductSizeTypes", t => t.ProductSizeTypeId)
                .ForeignKey("Web.Sizes", t => t.SizeId)
                .Index(t => t.ProductSizeTypeId)
                .Index(t => t.ProductId)
                .Index(t => t.SizeId);
            
            CreateTable(
                "Web.ProductSizeTypes",
                c => new
                    {
                        ProductSizeTypeId = c.Int(nullable: false, identity: true),
                        ProductSizeTypeName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductSizeTypeId)
                .Index(t => t.ProductSizeTypeName, unique: true, name: "IX_ProductSizeType_ProductSizeTypeName");
            
            CreateTable(
                "Web.Sizes",
                c => new
                    {
                        SizeId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SizeName = c.String(nullable: false, maxLength: 50),
                        ProductShapeId = c.Int(),
                        UnitId = c.String(maxLength: 3),
                        Length = c.Decimal(nullable: false, precision: 18, scale: 4),
                        LengthFraction = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Width = c.Decimal(nullable: false, precision: 18, scale: 4),
                        WidthFraction = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Height = c.Decimal(nullable: false, precision: 18, scale: 4),
                        HeightFraction = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Area = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Perimeter = c.Decimal(nullable: false, precision: 18, scale: 4),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SizeId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.ProductTypes", t => t.ProductShapeId)
                .ForeignKey("Web.Units", t => t.UnitId)
                .ForeignKey("Web.ProductShapes", t => t.ProductShapeId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SizeName, unique: true, name: "IX_Size_SizeName")
                .Index(t => t.ProductShapeId)
                .Index(t => t.UnitId);
            
            CreateTable(
                "Web.ProductSuppliers",
                c => new
                    {
                        ProductSupplierId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        LeadTime = c.Int(),
                        MinimumOrderQty = c.Decimal(precision: 18, scale: 4),
                        MaximumOrderQty = c.Decimal(precision: 18, scale: 4),
                        Cost = c.Decimal(precision: 18, scale: 4),
                        Default = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductSupplierId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.Suppliers", t => t.SupplierId)
                .Index(t => t.ProductId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "Web.Suppliers",
                c => new
                    {
                        PersonID = c.Int(nullable: false),
                        SalesTaxGroupPartyId = c.Int(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.SalesTaxGroupParties", t => t.SalesTaxGroupPartyId)
                .Index(t => t.PersonID)
                .Index(t => t.SalesTaxGroupPartyId);
            
            CreateTable(
                "Web.SalesTaxGroupParties",
                c => new
                    {
                        SalesTaxGroupPartyId = c.Int(nullable: false, identity: true),
                        SalesTaxGroupPartyName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SalesTaxGroupPartyId)
                .Index(t => t.SalesTaxGroupPartyName, unique: true, name: "IX_SalesTaxGroupParty_SalesTaxGroupPartyName");
            
            CreateTable(
                "Web.PurchaseGoodsReceiptLines",
                c => new
                    {
                        PurchaseGoodsReceiptLineId = c.Int(nullable: false, identity: true),
                        PurchaseGoodsReceiptHeaderId = c.Int(nullable: false),
                        PurchaseOrderLineId = c.Int(),
                        PurchaseIndentLineId = c.Int(),
                        ProductUidId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        LotNo = c.String(maxLength: 20),
                        DocQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DebitNoteAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DebitNoteReason = c.String(maxLength: 50),
                        isUninspected = c.Boolean(),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(nullable: false, maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BaleNo = c.String(maxLength: 10),
                        StockId = c.Int(),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocLineId = c.Int(),
                        Sr = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Remark = c.String(),
                        Specification = c.String(maxLength: 50),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        ProductUidLastTransactionDocId = c.Int(),
                        ProductUidLastTransactionDocNo = c.String(),
                        ProductUidLastTransactionDocTypeId = c.Int(),
                        ProductUidLastTransactionDocDate = c.DateTime(),
                        ProductUidLastTransactionPersonId = c.Int(),
                        ProductUidCurrentGodownId = c.Int(),
                        ProductUidCurrentProcessId = c.Int(),
                        ProductUidStatus = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.PurchaseGoodsReceiptLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.Godowns", t => t.ProductUidCurrentGodownId)
                .ForeignKey("Web.Processes", t => t.ProductUidCurrentProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.ProductUidLastTransactionDocTypeId)
                .ForeignKey("Web.People", t => t.ProductUidLastTransactionPersonId)
                .ForeignKey("Web.PurchaseGoodsReceiptHeaders", t => t.PurchaseGoodsReceiptHeaderId)
                .ForeignKey("Web.PurchaseIndentLines", t => t.PurchaseIndentLineId)
                .ForeignKey("Web.PurchaseOrderLines", t => t.PurchaseOrderLineId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Stocks", t => t.StockId)
                .Index(t => t.PurchaseGoodsReceiptHeaderId)
                .Index(t => t.PurchaseOrderLineId)
                .Index(t => t.PurchaseIndentLineId)
                .Index(t => t.ProductUidId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.DealUnitId)
                .Index(t => t.StockId)
                .Index(t => t.ReferenceDocTypeId)
                .Index(t => t.ProductUidLastTransactionDocTypeId)
                .Index(t => t.ProductUidLastTransactionPersonId)
                .Index(t => t.ProductUidCurrentGodownId)
                .Index(t => t.ProductUidCurrentProcessId);
            
            CreateTable(
                "Web.PurchaseGoodsReceiptHeaders",
                c => new
                    {
                        PurchaseGoodsReceiptHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        GodownId = c.Int(nullable: false),
                        SupplierDocNo = c.String(maxLength: 20),
                        SupplierDocDate = c.DateTime(),
                        PurchaseWaybillId = c.Int(),
                        GateInId = c.Int(),
                        RoadPermitFormId = c.Int(),
                        StockHeaderId = c.Int(),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        UnitConversionForId = c.Byte(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseGoodsReceiptHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.GateIns", t => t.GateInId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.PurchaseWaybills", t => t.PurchaseWaybillId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.ProductUids", t => t.RoadPermitFormId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.StockHeaders", t => t.StockHeaderId)
                .ForeignKey("Web.Suppliers", t => t.SupplierId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_PurchaseGoodsReceiptHeader_DocID")
                .Index(t => t.SupplierId)
                .Index(t => t.GodownId)
                .Index(t => t.PurchaseWaybillId)
                .Index(t => t.GateInId)
                .Index(t => t.RoadPermitFormId)
                .Index(t => t.StockHeaderId)
                .Index(t => t.ReferenceDocTypeId)
                .Index(t => t.UnitConversionForId);
            
            CreateTable(
                "Web.GateIns",
                c => new
                    {
                        GateInId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProductDescription = c.String(),
                        VehicleNo = c.String(maxLength: 20),
                        Transporter = c.String(maxLength: 250),
                        VehicleGrossWeight = c.Decimal(nullable: false, precision: 18, scale: 4),
                        VehicleTareWeight = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DiverName = c.String(maxLength: 250),
                        NoOfPackages = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.GateInId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_GateIn_DocID");
            
            CreateTable(
                "Web.PurchaseWaybills",
                c => new
                    {
                        PurchaseWaybillId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        EntryNo = c.String(nullable: false, maxLength: 20),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 20),
                        TransporterId = c.Int(nullable: false),
                        ConsignerId = c.Int(nullable: false),
                        ReferenceDocNo = c.String(maxLength: 30),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        DeliveryPoint = c.String(maxLength: 100),
                        EstimatedDeliveryDate = c.DateTime(nullable: false),
                        FreightType = c.String(nullable: false),
                        FromCityId = c.Int(nullable: false),
                        ToCityId = c.Int(nullable: false),
                        ProductDescription = c.String(maxLength: 100),
                        PrivateMark = c.String(maxLength: 30),
                        NoOfPackages = c.String(nullable: false),
                        ActualWeight = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ChargedWeight = c.Decimal(precision: 18, scale: 4),
                        ContainerNo = c.String(maxLength: 20),
                        FreightAmt = c.Decimal(precision: 18, scale: 4),
                        OtherCharges = c.Decimal(precision: 18, scale: 4),
                        ServiceTax = c.Decimal(precision: 18, scale: 4),
                        ServiceTaxPer = c.Decimal(precision: 18, scale: 4),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        FreightDescription = c.String(),
                        IsDoorDelivery = c.Boolean(nullable: false),
                        IsPOD = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseWaybillId)
                .ForeignKey("Web.People", t => t.ConsignerId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Cities", t => t.FromCityId)
                .ForeignKey("Web.ShipMethods", t => t.ShipMethodId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Cities", t => t.ToCityId)
                .ForeignKey("Web.Transporters", t => t.TransporterId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId }, unique: true, name: "IX_PurchaseWaybill_DocID")
                .Index(t => t.EntryNo, unique: true, name: "IX_PurchaseWaybill_EntryNo")
                .Index(t => t.TransporterId)
                .Index(t => t.ConsignerId)
                .Index(t => t.SiteId, unique: true, name: "IX_PurchaseTransportGR_DocID")
                .Index(t => t.ShipMethodId)
                .Index(t => t.FromCityId)
                .Index(t => t.ToCityId);
            
            CreateTable(
                "Web.Transporters",
                c => new
                    {
                        PersonID = c.Int(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("Web.People", t => t.PersonID)
                .Index(t => t.PersonID);
            
            CreateTable(
                "Web.PurchaseIndentLines",
                c => new
                    {
                        PurchaseIndentLineId = c.Int(nullable: false, identity: true),
                        PurchaseIndentHeaderId = c.Int(nullable: false),
                        MaterialPlanLineId = c.Int(),
                        DueDate = c.DateTime(),
                        Specification = c.String(),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        Sr = c.Int(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseIndentLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.MaterialPlanLines", t => t.MaterialPlanLineId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.PurchaseIndentHeaders", t => t.PurchaseIndentHeaderId)
                .Index(t => t.PurchaseIndentHeaderId)
                .Index(t => t.MaterialPlanLineId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.PurchaseIndentCancelLines",
                c => new
                    {
                        PurchaseIndentCancelLineId = c.Int(nullable: false, identity: true),
                        PurchaseIndentCancelHeaderId = c.Int(nullable: false),
                        PurchaseIndentLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        Sr = c.Int(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseIndentCancelLineId)
                .ForeignKey("Web.PurchaseIndentCancelHeaders", t => t.PurchaseIndentCancelHeaderId)
                .ForeignKey("Web.PurchaseIndentLines", t => t.PurchaseIndentLineId)
                .Index(t => t.PurchaseIndentCancelHeaderId)
                .Index(t => t.PurchaseIndentLineId);
            
            CreateTable(
                "Web.PurchaseIndentCancelHeaders",
                c => new
                    {
                        PurchaseIndentCancelHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        ReasonId = c.Int(nullable: false),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseIndentCancelHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_PurchaseIndentCancelHeader_DocID")
                .Index(t => t.ReasonId);
            
            CreateTable(
                "Web.PurchaseIndentHeaders",
                c => new
                    {
                        PurchaseIndentHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        ReasonId = c.Int(),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        MaterialPlanHeaderId = c.Int(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseIndentHeaderId)
                .ForeignKey("Web.Departments", t => t.DepartmentId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.MaterialPlanHeaders", t => t.MaterialPlanHeaderId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_PurchaseIndentHeader_DocID")
                .Index(t => t.DepartmentId)
                .Index(t => t.ReasonId)
                .Index(t => t.MaterialPlanHeaderId);
            
            CreateTable(
                "Web.PurchaseOrderLines",
                c => new
                    {
                        PurchaseOrderLineId = c.Int(nullable: false, identity: true),
                        PurchaseOrderHeaderId = c.Int(nullable: false),
                        PurchaseIndentLineId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Specification = c.String(maxLength: 50),
                        SalesTaxGroupId = c.Int(),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ShipDate = c.DateTime(),
                        LotNo = c.String(maxLength: 10),
                        DueDate = c.DateTime(),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(nullable: false, maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        DiscountPer = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        Remark = c.String(),
                        ProductUidHeaderId = c.Int(),
                        Sr = c.Int(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductUidHeaders", t => t.ProductUidHeaderId)
                .ForeignKey("Web.PurchaseIndentLines", t => t.PurchaseIndentLineId)
                .ForeignKey("Web.PurchaseOrderHeaders", t => t.PurchaseOrderHeaderId)
                .ForeignKey("Web.SalesTaxGroups", t => t.SalesTaxGroupId)
                .Index(t => t.PurchaseOrderHeaderId)
                .Index(t => t.PurchaseIndentLineId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.SalesTaxGroupId)
                .Index(t => t.DealUnitId)
                .Index(t => t.ProductUidHeaderId);
            
            CreateTable(
                "Web.PurchaseOrderCancelLines",
                c => new
                    {
                        PurchaseOrderCancelLineId = c.Int(nullable: false, identity: true),
                        PurchaseOrderCancelHeaderId = c.Int(nullable: false),
                        PurchaseOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        Sr = c.Int(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderCancelLineId)
                .ForeignKey("Web.PurchaseOrderCancelHeaders", t => t.PurchaseOrderCancelHeaderId)
                .ForeignKey("Web.PurchaseOrderLines", t => t.PurchaseOrderLineId)
                .Index(t => t.PurchaseOrderCancelHeaderId)
                .Index(t => t.PurchaseOrderLineId);
            
            CreateTable(
                "Web.PurchaseOrderCancelHeaders",
                c => new
                    {
                        PurchaseOrderCancelHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        ReasonId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        Remark = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        LockReason = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderCancelHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Suppliers", t => t.SupplierId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_PurchaseOrderCancelHeader_DocID")
                .Index(t => t.ReasonId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "Web.PurchaseOrderHeaders",
                c => new
                    {
                        PurchaseOrderHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        ActualDueDate = c.DateTime(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        DeliveryTermsId = c.Int(),
                        TermsAndConditions = c.String(),
                        ShipAddress = c.String(),
                        Remark = c.String(),
                        CurrencyId = c.Int(nullable: false),
                        SalesTaxGroupPersonId = c.Int(),
                        SupplierShipDate = c.DateTime(),
                        SupplierRemark = c.String(),
                        CreditDays = c.Int(),
                        ProgressPer = c.Int(),
                        isUninspected = c.Boolean(),
                        CalculateDiscountOnRate = c.Boolean(nullable: false),
                        UnitConversionForId = c.Byte(),
                        Status = c.Int(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        LockReason = c.String(),
                        Priority = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ApprovedBy = c.String(),
                        ApprovedDate = c.DateTime(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderHeaderId)
                .ForeignKey("Web.Currencies", t => t.CurrencyId)
                .ForeignKey("Web.DeliveryTerms", t => t.DeliveryTermsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.ChargeGroupPersons", t => t.SalesTaxGroupPersonId)
                .ForeignKey("Web.ShipMethods", t => t.ShipMethodId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.People", t => t.SupplierId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId, t.SupplierId }, unique: true, name: "IX_PurchaseOrderHeader_DocID")
                .Index(t => t.ShipMethodId)
                .Index(t => t.DeliveryTermsId)
                .Index(t => t.CurrencyId)
                .Index(t => t.SalesTaxGroupPersonId)
                .Index(t => t.UnitConversionForId);
            
            CreateTable(
                "Web.ChargeGroupPersons",
                c => new
                    {
                        ChargeGroupPersonId = c.Int(nullable: false, identity: true),
                        ChargeGroupPersonName = c.String(nullable: false, maxLength: 50),
                        ChargeTypeId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ChargeGroupPersonId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .Index(t => t.ChargeGroupPersonName, unique: true, name: "IX_ChargeGroupPerson_ChargeGroupPersonName")
                .Index(t => t.ChargeTypeId);
            
            CreateTable(
                "Web.ChargeTypes",
                c => new
                    {
                        ChargeTypeId = c.Int(nullable: false, identity: true),
                        ChargeTypeName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        isCommentNeeded = c.Boolean(nullable: false),
                        isPersonBased = c.Boolean(nullable: false),
                        isProductBased = c.Boolean(nullable: false),
                        ImageFolderName = c.String(maxLength: 100),
                        ImageFileName = c.String(maxLength: 100),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ChargeTypeId)
                .Index(t => t.ChargeTypeName, unique: true, name: "IX_Charges_ChargesName");
            
            CreateTable(
                "Web.SalesTaxGroups",
                c => new
                    {
                        SalesTaxGroupId = c.Int(nullable: false, identity: true),
                        SalesTaxGroupProductId = c.Int(nullable: false),
                        SalesTaxGroupPartyId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SalesTaxGroupId)
                .ForeignKey("Web.SalesTaxGroupParties", t => t.SalesTaxGroupPartyId)
                .ForeignKey("Web.SalesTaxGroupProducts", t => t.SalesTaxGroupProductId)
                .Index(t => t.SalesTaxGroupProductId)
                .Index(t => t.SalesTaxGroupPartyId);
            
            CreateTable(
                "Web.SalesTaxGroupProducts",
                c => new
                    {
                        SalesTaxGroupProductId = c.Int(nullable: false, identity: true),
                        SalesTaxGroupProductName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SalesTaxGroupProductId)
                .Index(t => t.SalesTaxGroupProductName, unique: true, name: "IX_SalesTaxGroupProduct_SalesTaxGroupProductName");
            
            CreateTable(
                "Web.Colours",
                c => new
                    {
                        ColourId = c.Int(nullable: false, identity: true),
                        ColourName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ColourId)
                .Index(t => t.ColourName, unique: true, name: "IX_Colour_ColourName");
            
            CreateTable(
                "Web.ProductContentHeaders",
                c => new
                    {
                        ProductContentHeaderId = c.Int(nullable: false, identity: true),
                        ProductContentName = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductContentHeaderId)
                .Index(t => t.ProductContentName, unique: true, name: "IX_ProductContent_ProductContentName");
            
            CreateTable(
                "Web.ProductContentLines",
                c => new
                    {
                        ProductContentLineId = c.Int(nullable: false, identity: true),
                        ProductContentHeaderId = c.Int(nullable: false),
                        ProductGroupId = c.Int(nullable: false),
                        ContentPer = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductContentLineId)
                .ForeignKey("Web.ProductContentHeaders", t => t.ProductContentHeaderId)
                .ForeignKey("Web.ProductGroups", t => t.ProductGroupId)
                .Index(t => t.ProductContentHeaderId)
                .Index(t => t.ProductGroupId);
            
            CreateTable(
                "Web.DescriptionOfGoods",
                c => new
                    {
                        DescriptionOfGoodsId = c.Int(nullable: false, identity: true),
                        DescriptionOfGoodsName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DescriptionOfGoodsId)
                .Index(t => t.DescriptionOfGoodsName, unique: true, name: "IX_DescriptionOfGoods_DescriptionOfGoodsName");
            
            CreateTable(
                "Web.ProcessSequenceHeaders",
                c => new
                    {
                        ProcessSequenceHeaderId = c.Int(nullable: false, identity: true),
                        ProcessSequenceHeaderName = c.String(nullable: false, maxLength: 50),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        CheckSum = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProcessSequenceHeaderId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .Index(t => t.ProcessSequenceHeaderName, unique: true, name: "IX_ProcessSequence_ProcessSequenceHeaderName")
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.ProductCollections",
                c => new
                    {
                        ProductCollectionId = c.Int(nullable: false, identity: true),
                        ProductCollectionName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        ProductTypeId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductCollectionId)
                .ForeignKey("Web.ProductTypes", t => t.ProductTypeId)
                .Index(t => t.ProductCollectionName, unique: true, name: "IX_ProductCollection_ProductCollectionName")
                .Index(t => t.ProductTypeId);
            
            CreateTable(
                "Web.ProductDesignPatterns",
                c => new
                    {
                        ProductDesignPatternId = c.Int(nullable: false, identity: true),
                        ProductDesignPatternName = c.String(nullable: false, maxLength: 50),
                        ProductTypeId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductDesignPatternId)
                .ForeignKey("Web.ProductTypes", t => t.ProductTypeId)
                .Index(t => t.ProductDesignPatternName, unique: true, name: "IX_ProductDesignPattern_ProductDesignName")
                .Index(t => t.ProductTypeId);
            
            CreateTable(
                "Web.ProductInvoiceGroups",
                c => new
                    {
                        ProductInvoiceGroupId = c.Int(nullable: false, identity: true),
                        ProductInvoiceGroupName = c.String(nullable: false, maxLength: 50),
                        ItcHsCode = c.String(maxLength: 25),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Knots = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DivisionId = c.Int(nullable: false),
                        IsSample = c.Boolean(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SeparateWeightInInvoice = c.Boolean(nullable: false),
                        DescriptionOfGoodsId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductInvoiceGroupId)
                .ForeignKey("Web.DescriptionOfGoods", t => t.DescriptionOfGoodsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .Index(t => t.ProductInvoiceGroupName, unique: true, name: "IX_ProductInvoiceGroup_ProductInvoiceGroupName")
                .Index(t => t.DivisionId)
                .Index(t => t.DescriptionOfGoodsId);
            
            CreateTable(
                "Web.ProductQualities",
                c => new
                    {
                        ProductQualityId = c.Int(nullable: false, identity: true),
                        ProductQualityName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        ProductTypeId = c.Int(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductQualityId)
                .ForeignKey("Web.ProductTypes", t => t.ProductTypeId)
                .Index(t => t.ProductQualityName, unique: true, name: "IX_ProductQuality_ProductQualityName")
                .Index(t => t.ProductTypeId);
            
            CreateTable(
                "Web.ProductStyles",
                c => new
                    {
                        ProductStyleId = c.Int(nullable: false, identity: true),
                        ProductStyleName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductStyleId)
                .Index(t => t.ProductStyleName, unique: true, name: "IX_ProductStyle_ProductStyleName");
            
            CreateTable(
                "Web.BusinessEntities",
                c => new
                    {
                        PersonID = c.Int(nullable: false),
                        ParentId = c.Int(),
                        TdsCategoryId = c.Int(),
                        TdsGroupId = c.Int(),
                        GuarantorId = c.Int(),
                        SalesTaxGroupPartyId = c.Int(),
                        IsSisterConcern = c.Boolean(nullable: false),
                        PersonRateGroupId = c.Int(),
                        ServiceTaxCategoryId = c.Int(),
                        CreaditDays = c.Int(),
                        CreaditLimit = c.Decimal(precision: 18, scale: 4),
                        DivisionIds = c.String(maxLength: 100),
                        SiteIds = c.String(maxLength: 100),
                        OMSId = c.String(maxLength: 50),
                        Buyer_PersonID = c.Int(),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("Web.Buyers", t => t.Buyer_PersonID)
                .ForeignKey("Web.People", t => t.GuarantorId)
                .ForeignKey("Web.People", t => t.ParentId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.PersonRateGroups", t => t.PersonRateGroupId)
                .ForeignKey("Web.SalesTaxGroupParties", t => t.SalesTaxGroupPartyId)
                .ForeignKey("Web.ServiceTaxCategories", t => t.ServiceTaxCategoryId)
                .ForeignKey("Web.TdsCategories", t => t.TdsCategoryId)
                .ForeignKey("Web.TdsGroups", t => t.TdsGroupId)
                .Index(t => t.PersonID)
                .Index(t => t.ParentId)
                .Index(t => t.TdsCategoryId)
                .Index(t => t.TdsGroupId)
                .Index(t => t.GuarantorId)
                .Index(t => t.SalesTaxGroupPartyId)
                .Index(t => t.PersonRateGroupId)
                .Index(t => t.ServiceTaxCategoryId)
                .Index(t => t.Buyer_PersonID);
            
            CreateTable(
                "Web.PersonRateGroups",
                c => new
                    {
                        PersonRateGroupId = c.Int(nullable: false, identity: true),
                        PersonRateGroupName = c.String(nullable: false, maxLength: 50),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        Processes = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonRateGroupId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.PersonRateGroupName, t.DivisionId, t.SiteId }, unique: true, name: "IX_PersonRateGroup_PersonRateGroupName");
            
            CreateTable(
                "Web.ServiceTaxCategories",
                c => new
                    {
                        ServiceTaxCategoryId = c.Int(nullable: false, identity: true),
                        ServiceTaxCategoryName = c.String(nullable: false, maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ServiceTaxCategoryId)
                .Index(t => t.ServiceTaxCategoryName, unique: true, name: "IX_ServiceTaxCategory_ServiceTaxCategoryName");
            
            CreateTable(
                "Web.TdsCategories",
                c => new
                    {
                        TdsCategoryId = c.Int(nullable: false, identity: true),
                        TdsCategoryName = c.String(nullable: false, maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.TdsCategoryId)
                .Index(t => t.TdsCategoryName, unique: true, name: "IX_TdsCategory_TdsCategoryName");
            
            CreateTable(
                "Web.TdsGroups",
                c => new
                    {
                        TdsGroupId = c.Int(nullable: false, identity: true),
                        TdsGroupName = c.String(nullable: false, maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.TdsGroupId)
                .Index(t => t.TdsGroupName, unique: true, name: "IX_TdsGroup_TdsGroupName");
            
            CreateTable(
                "Web.BusinessSessions",
                c => new
                    {
                        BusinessSessionId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        BusinessSessionName = c.String(),
                        FromDate = c.DateTime(nullable: false),
                        UptoDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        LockReason = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.BusinessSessionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .Index(t => t.DocTypeId);
            
            CreateTable(
                "Web.Calculations",
                c => new
                    {
                        CalculationId = c.Int(nullable: false, identity: true),
                        CalculationName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CalculationId)
                .Index(t => t.CalculationName, unique: true, name: "IX_Calculation_Calculation");
            
            CreateTable(
                "Web.CalculationFooters",
                c => new
                    {
                        CalculationFooterLineId = c.Int(nullable: false, identity: true),
                        CalculationId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        CalculateOnId = c.Int(),
                        ProductChargeId = c.Int(),
                        CostCenterId = c.Int(),
                        PersonId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        RoundOff = c.Decimal(precision: 18, scale: 4),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        IsVisible = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CalculationFooterLineId)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Calculations", t => t.CalculationId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonId)
                .ForeignKey("Web.Charges", t => t.ProductChargeId)
                .Index(t => new { t.CalculationId, t.ChargeId }, unique: true, name: "IX_CalculationLine_CalculationLineName")
                .Index(t => t.ChargeTypeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.ProductChargeId)
                .Index(t => t.CostCenterId)
                .Index(t => t.PersonId)
                .Index(t => t.ParentChargeId)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId);
            
            CreateTable(
                "Web.Charges",
                c => new
                    {
                        ChargeId = c.Int(nullable: false, identity: true),
                        ChargeName = c.String(nullable: false, maxLength: 50),
                        ChargeCode = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ChargeId)
                .Index(t => t.ChargeName, unique: true, name: "IX_Charge_Charge")
                .Index(t => t.ChargeCode, unique: true, name: "IX_Charge_ChargeCode");
            
            CreateTable(
                "Web.CalculationHeaderLedgerAccounts",
                c => new
                    {
                        CalculationHeaderLedgerAccountId = c.Int(nullable: false, identity: true),
                        CalculationId = c.Int(nullable: false),
                        CalculationFooterId = c.Int(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CalculationHeaderLedgerAccountId)
                .ForeignKey("Web.Calculations", t => t.CalculationId)
                .ForeignKey("Web.CalculationFooters", t => t.CalculationFooterId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.CalculationId)
                .Index(t => new { t.CalculationFooterId, t.DocTypeId }, unique: true, name: "IX_CalculationHeaderLedgerAccount_UniqueRow")
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "Web.CalculationLineLedgerAccounts",
                c => new
                    {
                        CalculationLineLedgerAccountId = c.Int(nullable: false, identity: true),
                        CalculationId = c.Int(nullable: false),
                        CalculationProductId = c.Int(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CalculationLineLedgerAccountId)
                .ForeignKey("Web.Calculations", t => t.CalculationId)
                .ForeignKey("Web.CalculationProducts", t => t.CalculationProductId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.CalculationId)
                .Index(t => new { t.CalculationProductId, t.DocTypeId }, unique: true, name: "IX_CalculationLineLedgerAccount_UniqueRow")
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "Web.CalculationProducts",
                c => new
                    {
                        CalculationProductId = c.Int(nullable: false, identity: true),
                        CalculationId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        CalculateOnId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        RoundOff = c.Decimal(precision: 18, scale: 4),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        PersonId = c.Int(),
                        IsVisible = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CalculationProductId)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Calculations", t => t.CalculationId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonId)
                .Index(t => new { t.CalculationId, t.ChargeId }, unique: true, name: "IX_CalculationProduct_CalculationProductName")
                .Index(t => t.ChargeTypeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "Web.ChargeGroupProducts",
                c => new
                    {
                        ChargeGroupProductId = c.Int(nullable: false, identity: true),
                        ChargeGroupProductName = c.String(nullable: false, maxLength: 50),
                        ChargeTypeId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ChargeGroupProductId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .Index(t => t.ChargeGroupProductName, unique: true, name: "IX_ChargeGroupProduct_ChargeGroupProductName")
                .Index(t => t.ChargeTypeId);
            
            CreateTable(
                "Web.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 50),
                        Address = c.String(maxLength: 250),
                        CityId = c.Int(),
                        LstNo = c.String(maxLength: 20),
                        CstNo = c.String(maxLength: 20),
                        TinNo = c.String(maxLength: 20),
                        IECNo = c.String(maxLength: 20),
                        Phone = c.String(maxLength: 15),
                        Fax = c.String(maxLength: 15),
                        CurrencyId = c.Int(nullable: false),
                        ExciseDivision = c.String(maxLength: 100),
                        DirectorName = c.String(maxLength: 100),
                        BankName = c.String(maxLength: 100),
                        BankBranch = c.String(maxLength: 250),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("Web.Cities", t => t.CityId)
                .ForeignKey("Web.Currencies", t => t.CurrencyId)
                .Index(t => t.CompanyName, unique: true, name: "IX_Company_Company")
                .Index(t => t.CityId)
                .Index(t => t.CurrencyId);
            
            CreateTable(
                "Web.CostCenterStatus",
                c => new
                    {
                        CostCenterId = c.Int(nullable: false),
                        ProductId = c.Int(),
                        AmountDr = c.Decimal(precision: 18, scale: 4),
                        AmountCr = c.Decimal(precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.CostCenterId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Web.CostCenterStatusExtendeds",
                c => new
                    {
                        CostCenterId = c.Int(nullable: false),
                        MaterialIssueQty = c.Decimal(precision: 18, scale: 4),
                        MaterialIssueDate = c.DateTime(),
                        MaterialReturnQty = c.Decimal(precision: 18, scale: 4),
                        MaterialReturnDate = c.DateTime(),
                        RequisitionProductCount = c.Int(),
                        MaterialIssueProductCount = c.Int(),
                        MaterialReturnProductCount = c.Int(),
                        BOMQty = c.Decimal(precision: 18, scale: 4),
                        BOMCancelQty = c.Decimal(precision: 18, scale: 4),
                        ConsumeQty = c.Decimal(precision: 18, scale: 4),
                        RateSettlementQty = c.Decimal(precision: 18, scale: 4),
                        RateSettlementDate = c.DateTime(),
                        RateSettlementAmount = c.Decimal(precision: 18, scale: 4),
                        TransferQty = c.Decimal(precision: 18, scale: 4),
                        TransferDate = c.DateTime(),
                        TransferAmount = c.Decimal(precision: 18, scale: 4),
                        ConsumptionAdjustmentQty = c.Decimal(precision: 18, scale: 4),
                        ConsumptionAdjustmentDate = c.DateTime(),
                        OrderQty = c.Decimal(precision: 18, scale: 4),
                        OrderDealQty = c.Decimal(precision: 18, scale: 4),
                        OrderCancelQty = c.Decimal(precision: 18, scale: 4),
                        OrderCancelDealQty = c.Decimal(precision: 18, scale: 4),
                        ReceiveQty = c.Decimal(precision: 18, scale: 4),
                        ReceiveDealQty = c.Decimal(precision: 18, scale: 4),
                        InvoiceQty = c.Decimal(precision: 18, scale: 4),
                        InvoiceDealQty = c.Decimal(precision: 18, scale: 4),
                        PendingToInvoiceAmount = c.Decimal(precision: 18, scale: 4),
                        InvoiceAmount = c.Decimal(precision: 18, scale: 4),
                        ReceiveIncentiveAmount = c.Decimal(precision: 18, scale: 4),
                        ReceivePenaltyAmount = c.Decimal(precision: 18, scale: 4),
                        TimeIncentiveAmount = c.Decimal(precision: 18, scale: 4),
                        TimePenaltyAmount = c.Decimal(precision: 18, scale: 4),
                        FragmentationPenaltyAmount = c.Decimal(precision: 18, scale: 4),
                        SchemeIncentiveAmount = c.Decimal(precision: 18, scale: 4),
                        DebitAmount = c.Decimal(precision: 18, scale: 4),
                        CreditAmount = c.Decimal(precision: 18, scale: 4),
                        PaymentAmount = c.Decimal(precision: 18, scale: 4),
                        TDSAmount = c.Decimal(precision: 18, scale: 4),
                        ProductId = c.Int(),
                        ReturnQty = c.Decimal(precision: 18, scale: 4),
                        ReturnDealQty = c.Decimal(precision: 18, scale: 4),
                        PaymentCancelAmount = c.Decimal(precision: 18, scale: 4),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        WeavingReceipt = c.Decimal(precision: 18, scale: 4),
                        AmountTransferDate = c.DateTime(),
                        ReturnConsumeQty = c.Decimal(precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.CostCenterId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .Index(t => t.CostCenterId);
            
            CreateTable(
                "Web.Counters",
                c => new
                    {
                        CounterId = c.Int(nullable: false, identity: true),
                        ImageFolderName = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CounterId);
            
            CreateTable(
                "Web.Couriers",
                c => new
                    {
                        PersonID = c.Int(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("Web.People", t => t.PersonID)
                .Index(t => t.PersonID);
            
            CreateTable(
                "Web.CurrencyConversions",
                c => new
                    {
                        CurrencyConversionsId = c.Int(nullable: false, identity: true),
                        FromQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        FromCurrencyId = c.Int(nullable: false),
                        ToQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ToCurrencyId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.CurrencyConversionsId)
                .ForeignKey("Web.Currencies", t => t.FromCurrencyId)
                .ForeignKey("Web.Currencies", t => t.ToCurrencyId)
                .Index(t => t.FromCurrencyId)
                .Index(t => t.ToCurrencyId);
            
            CreateTable(
                "Web.CustomDetails",
                c => new
                    {
                        CustomDetailId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        SaleInvoiceHeaderId = c.Int(),
                        TRNo = c.String(maxLength: 20),
                        TRDate = c.DateTime(),
                        TRCourierId = c.Int(),
                        TRCourierDate = c.DateTime(),
                        TRCourierRefNo = c.String(maxLength: 50),
                        ShippingBillNo = c.String(maxLength: 50),
                        ShippingBillDate = c.DateTime(),
                        CustomSealNo = c.String(maxLength: 50),
                        LineSealNo = c.String(maxLength: 50),
                        NoOfPackages = c.Decimal(precision: 18, scale: 4),
                        ActualWeight = c.Decimal(precision: 18, scale: 4),
                        ChargedWeight = c.Decimal(precision: 18, scale: 4),
                        ContainerNo = c.String(maxLength: 50),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CustomDetailId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.SaleInvoiceHeaders", t => t.SaleInvoiceHeaderId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.People", t => t.TRCourierId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_CustomDetail_DocID")
                .Index(t => t.SaleInvoiceHeaderId)
                .Index(t => t.TRCourierId);
            
            CreateTable(
                "Web.SaleInvoiceHeaders",
                c => new
                    {
                        SaleInvoiceHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        LedgerHeaderId = c.Int(),
                        SaleToBuyerId = c.Int(nullable: false),
                        BillToBuyerId = c.Int(nullable: false),
                        AgentId = c.Int(),
                        CurrencyId = c.Int(nullable: false),
                        ExchangeRate = c.Decimal(precision: 18, scale: 4),
                        CreditLimit = c.Decimal(precision: 18, scale: 4),
                        CreditDays = c.Decimal(precision: 18, scale: 4),
                        CurrentBalance = c.Decimal(precision: 18, scale: 4),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        LockReason = c.String(),
                        SaleDispatchHeaderId = c.Int(),
                        CalculateDiscountOnRate = c.Boolean(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleInvoiceHeaderId)
                .ForeignKey("Web.People", t => t.AgentId)
                .ForeignKey("Web.Buyers", t => t.BillToBuyerId)
                .ForeignKey("Web.Currencies", t => t.CurrencyId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.LedgerHeaders", t => t.LedgerHeaderId)
                .ForeignKey("Web.SaleDispatchHeaders", t => t.SaleDispatchHeaderId)
                .ForeignKey("Web.People", t => t.SaleToBuyerId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_SaleInvoiceHeader_DocID")
                .Index(t => t.LedgerHeaderId)
                .Index(t => t.SaleToBuyerId)
                .Index(t => t.BillToBuyerId)
                .Index(t => t.AgentId)
                .Index(t => t.CurrencyId)
                .Index(t => t.SaleDispatchHeaderId);
            
            CreateTable(
                "Web.SaleDispatchHeaders",
                c => new
                    {
                        SaleDispatchHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        SaleToBuyerId = c.Int(nullable: false),
                        ShipToPartyAddress = c.String(maxLength: 250),
                        GateEntryNo = c.String(maxLength: 20),
                        FormNo = c.String(maxLength: 20),
                        Transporter = c.String(maxLength: 100),
                        DeliveryTermsId = c.Int(nullable: false),
                        ShipMethodId = c.Int(),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        GatePassHeaderId = c.Int(),
                        StockHeaderId = c.Int(),
                        PackingHeaderId = c.Int(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleDispatchHeaderId)
                .ForeignKey("Web.DeliveryTerms", t => t.DeliveryTermsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.GatePassHeaders", t => t.GatePassHeaderId)
                .ForeignKey("Web.PackingHeaders", t => t.PackingHeaderId)
                .ForeignKey("Web.People", t => t.SaleToBuyerId)
                .ForeignKey("Web.ShipMethods", t => t.ShipMethodId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.StockHeaders", t => t.StockHeaderId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_SaleDispatchHeader_DocID")
                .Index(t => t.SaleToBuyerId)
                .Index(t => t.DeliveryTermsId)
                .Index(t => t.ShipMethodId)
                .Index(t => t.GatePassHeaderId)
                .Index(t => t.StockHeaderId)
                .Index(t => t.PackingHeaderId);
            
            CreateTable(
                "Web.SaleDispatchLines",
                c => new
                    {
                        SaleDispatchLineId = c.Int(nullable: false, identity: true),
                        SaleDispatchHeaderId = c.Int(nullable: false),
                        PackingLineId = c.Int(nullable: false),
                        GodownId = c.Int(nullable: false),
                        StockId = c.Int(),
                        StockInId = c.Int(),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        ProductUidLastTransactionDocId = c.Int(),
                        ProductUidLastTransactionDocNo = c.String(),
                        ProductUidLastTransactionDocTypeId = c.Int(),
                        ProductUidLastTransactionDocDate = c.DateTime(),
                        ProductUidLastTransactionPersonId = c.Int(),
                        ProductUidCurrentGodownId = c.Int(),
                        ProductUidCurrentProcessId = c.Int(),
                        ProductUidStatus = c.String(maxLength: 10),
                        LockReason = c.String(),
                    })
                .PrimaryKey(t => t.SaleDispatchLineId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.PackingLines", t => t.PackingLineId)
                .ForeignKey("Web.Godowns", t => t.ProductUidCurrentGodownId)
                .ForeignKey("Web.Processes", t => t.ProductUidCurrentProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.ProductUidLastTransactionDocTypeId)
                .ForeignKey("Web.People", t => t.ProductUidLastTransactionPersonId)
                .ForeignKey("Web.SaleDispatchHeaders", t => t.SaleDispatchHeaderId)
                .ForeignKey("Web.Stocks", t => t.StockId)
                .ForeignKey("Web.Stocks", t => t.StockInId)
                .Index(t => t.SaleDispatchHeaderId)
                .Index(t => t.PackingLineId)
                .Index(t => t.GodownId)
                .Index(t => t.StockId)
                .Index(t => t.StockInId)
                .Index(t => t.ProductUidLastTransactionDocTypeId)
                .Index(t => t.ProductUidLastTransactionPersonId)
                .Index(t => t.ProductUidCurrentGodownId)
                .Index(t => t.ProductUidCurrentProcessId);
            
            CreateTable(
                "Web.SaleInvoiceLines",
                c => new
                    {
                        SaleInvoiceLineId = c.Int(nullable: false, identity: true),
                        SaleInvoiceHeaderId = c.Int(nullable: false),
                        SaleDispatchLineId = c.Int(nullable: false),
                        SaleOrderLineId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SalesTaxGroupId = c.Int(),
                        ProductInvoiceGroupId = c.Int(),
                        DealUnitId = c.String(nullable: false, maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitConversionMultiplier = c.Decimal(precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Weight = c.Decimal(precision: 18, scale: 4),
                        PromoCodeId = c.Int(),
                        DiscountPer = c.Decimal(precision: 18, scale: 4),
                        DiscountAmount = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        LockReason = c.String(),
                        Sr = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleInvoiceLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductInvoiceGroups", t => t.ProductInvoiceGroupId)
                .ForeignKey("Web.PromoCodes", t => t.PromoCodeId)
                .ForeignKey("Web.SaleDispatchLines", t => t.SaleDispatchLineId)
                .ForeignKey("Web.SaleInvoiceHeaders", t => t.SaleInvoiceHeaderId)
                .ForeignKey("Web.SaleOrderLines", t => t.SaleOrderLineId)
                .ForeignKey("Web.SalesTaxGroups", t => t.SalesTaxGroupId)
                .Index(t => t.SaleInvoiceHeaderId)
                .Index(t => t.SaleDispatchLineId)
                .Index(t => t.SaleOrderLineId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.SalesTaxGroupId)
                .Index(t => t.ProductInvoiceGroupId)
                .Index(t => t.DealUnitId)
                .Index(t => t.PromoCodeId);
            
            CreateTable(
                "Web.PromoCodes",
                c => new
                    {
                        PromoCodeId = c.Int(nullable: false, identity: true),
                        PromoCodeName = c.String(nullable: false, maxLength: 50),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        ProductId = c.Int(),
                        ProductGroupId = c.Int(),
                        ProductCategoryId = c.Int(),
                        ProductTypeId = c.Int(),
                        MinInvoiceValue = c.Decimal(precision: 18, scale: 4),
                        DiscountPer = c.Decimal(precision: 18, scale: 4),
                        FlatDiscount = c.Decimal(precision: 18, scale: 4),
                        MaxDiscountAmount = c.Decimal(precision: 18, scale: 4),
                        IsApplicableOnce = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PromoCodeId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductCategories", t => t.ProductCategoryId)
                .ForeignKey("Web.ProductGroups", t => t.ProductGroupId)
                .ForeignKey("Web.ProductTypes", t => t.ProductTypeId)
                .Index(t => t.PromoCodeName, unique: true, name: "IX_PromoCode_PromoCode")
                .Index(t => t.ProductId)
                .Index(t => t.ProductGroupId)
                .Index(t => t.ProductCategoryId)
                .Index(t => t.ProductTypeId);
            
            CreateTable(
                "Web.DispatchWaybillHeaders",
                c => new
                    {
                        DispatchWaybillHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ConsigneeId = c.Int(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        SaleInvoiceHeaderId = c.Int(nullable: false),
                        TransporterId = c.Int(nullable: false),
                        DeliveryOffice = c.String(maxLength: 100),
                        WaybillNo = c.String(nullable: false, maxLength: 50),
                        WaybillDate = c.DateTime(nullable: false),
                        EstimatedDeliveryDate = c.DateTime(nullable: false),
                        PaymentType = c.String(nullable: false, maxLength: 20),
                        FromCityId = c.Int(nullable: false),
                        ToCityId = c.Int(nullable: false),
                        RouteId = c.Int(nullable: false),
                        ProductDescription = c.String(),
                        PrivateMark = c.String(maxLength: 100),
                        NoOfPackages = c.String(maxLength: 50),
                        ActualWeight = c.Decimal(precision: 18, scale: 4),
                        ChargedWeight = c.Decimal(precision: 18, scale: 4),
                        ContainerNo = c.String(maxLength: 50),
                        Freight = c.Decimal(precision: 18, scale: 4),
                        OtherCharges = c.Decimal(precision: 18, scale: 4),
                        ServiceTaxPer = c.Decimal(precision: 18, scale: 4),
                        ServiceTaxAmount = c.Decimal(precision: 18, scale: 4),
                        TotalAmount = c.Decimal(precision: 18, scale: 4),
                        Remark = c.String(),
                        IsPreCarriage = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DispatchWaybillHeaderId)
                .ForeignKey("Web.Buyers", t => t.ConsigneeId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Cities", t => t.FromCityId)
                .ForeignKey("Web.Routes", t => t.RouteId)
                .ForeignKey("Web.SaleInvoiceHeaders", t => t.SaleInvoiceHeaderId)
                .ForeignKey("Web.ShipMethods", t => t.ShipMethodId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Cities", t => t.ToCityId)
                .ForeignKey("Web.Transporters", t => t.TransporterId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_DispatchWaybillHeader_DocID")
                .Index(t => t.ConsigneeId)
                .Index(t => t.ShipMethodId)
                .Index(t => t.SaleInvoiceHeaderId)
                .Index(t => t.TransporterId)
                .Index(t => t.FromCityId)
                .Index(t => t.ToCityId)
                .Index(t => t.RouteId);
            
            CreateTable(
                "Web.Routes",
                c => new
                    {
                        RouteId = c.Int(nullable: false, identity: true),
                        RouteName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RouteId)
                .Index(t => t.RouteName, unique: true, name: "IX_Route_RouteName");
            
            CreateTable(
                "Web.DispatchWaybillLines",
                c => new
                    {
                        DispatchWaybillLineId = c.Int(nullable: false, identity: true),
                        DispatchWaybillHeaderId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        ReceiveDateTime = c.DateTime(),
                        ReceiveRemark = c.String(),
                        ForwardingDateTime = c.DateTime(),
                        ForwardedBy = c.String(maxLength: 250),
                        ForwardingRemark = c.String(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DispatchWaybillLineId)
                .ForeignKey("Web.Cities", t => t.CityId)
                .ForeignKey("Web.DispatchWaybillHeaders", t => t.DispatchWaybillHeaderId)
                .Index(t => t.DispatchWaybillHeaderId)
                .Index(t => t.CityId);
            
            CreateTable(
                "Web.DocEmailContents",
                c => new
                    {
                        DocEmailContentId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        ActivityTypeId = c.Int(nullable: false),
                        ProcEmailContent = c.String(maxLength: 100),
                        AttachmentTypes = c.String(maxLength: 100),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DocEmailContentId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.DocNotificationContents",
                c => new
                    {
                        DocNotificationContentId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        ActivityTypeId = c.Int(nullable: false),
                        ProcNotificationContent = c.String(maxLength: 100),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DocNotificationContentId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.DocSmsContents",
                c => new
                    {
                        DocSmsContentId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        ActivityTypeId = c.Int(nullable: false),
                        ProcSmsContent = c.String(maxLength: 100),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DocSmsContentId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.DocumentAttachments",
                c => new
                    {
                        DocumentAttachmentId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocId = c.Int(nullable: false),
                        FileFolderName = c.String(),
                        FileName = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentAttachmentId);
            
            CreateTable(
                "Web.DocumentStatus",
                c => new
                    {
                        DocumentStatusId = c.Int(nullable: false, identity: true),
                        DocumentStatusName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DocumentStatusId)
                .Index(t => t.DocumentStatusName, unique: true, name: "IX_DocumentStatus_DocumentStatusName");
            
            CreateTable(
                "Web.DocumentTypeDivisions",
                c => new
                    {
                        DocumentTypeDivisionId = c.Int(nullable: false, identity: true),
                        DocumentTypeId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DocumentTypeDivisionId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocumentTypeId)
                .Index(t => t.DocumentTypeId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.DocumentTypeSites",
                c => new
                    {
                        DocumentTypeSiteId = c.Int(nullable: false, identity: true),
                        DocumentTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DocumentTypeSiteId)
                .ForeignKey("Web.DocumentTypes", t => t.DocumentTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocumentTypeId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "Web.DocumentTypeTimeExtensions",
                c => new
                    {
                        DocumentTypeTimeExtensionId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        Type = c.String(),
                        ExpiryDate = c.DateTime(nullable: false),
                        UserName = c.String(),
                        Reason = c.String(),
                        NoOfRecords = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentTypeTimeExtensionId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "Web.DocumentTypeTimePlans",
                c => new
                    {
                        DocumentTypeTimePlanId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        Type = c.String(),
                        Days = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentTypeTimePlanId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "Web.ExcessMaterialHeaders",
                c => new
                    {
                        ExcessMaterialHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        CurrencyId = c.Int(),
                        PersonId = c.Int(),
                        ProcessId = c.Int(),
                        GodownId = c.Int(),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ExcessMaterialHeaderId)
                .ForeignKey("Web.Currencies", t => t.CurrencyId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.People", t => t.PersonId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_ExcessMaterialHeader_DocID")
                .Index(t => t.CurrencyId)
                .Index(t => t.PersonId)
                .Index(t => t.ProcessId)
                .Index(t => t.GodownId);
            
            CreateTable(
                "Web.ExcessMaterialLines",
                c => new
                    {
                        ExcessMaterialLineId = c.Int(nullable: false, identity: true),
                        ExcessMaterialHeaderId = c.Int(nullable: false),
                        ProductUidId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        ProcessId = c.Int(),
                        LotNo = c.String(maxLength: 10),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        Sr = c.Int(),
                        ProductUidLastTransactionDocId = c.Int(),
                        ProductUidLastTransactionDocNo = c.String(),
                        ProductUidLastTransactionDocTypeId = c.Int(),
                        ProductUidLastTransactionDocDate = c.DateTime(),
                        ProductUidLastTransactionPersonId = c.Int(),
                        ProductUidCurrentGodownId = c.Int(),
                        ProductUidCurrentProcessId = c.Int(),
                        ProductUidStatus = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ExcessMaterialLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.ExcessMaterialHeaders", t => t.ExcessMaterialHeaderId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.Godowns", t => t.ProductUidCurrentGodownId)
                .ForeignKey("Web.Processes", t => t.ProductUidCurrentProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.ProductUidLastTransactionDocTypeId)
                .ForeignKey("Web.People", t => t.ProductUidLastTransactionPersonId)
                .Index(t => t.ExcessMaterialHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.ProductId)
                .Index(t => t.ProcessId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.ProductUidLastTransactionDocTypeId)
                .Index(t => t.ProductUidLastTransactionPersonId)
                .Index(t => t.ProductUidCurrentGodownId)
                .Index(t => t.ProductUidCurrentProcessId);
            
            CreateTable(
                "Web.ExcessMaterialSettings",
                c => new
                    {
                        ExcessMaterialSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleProductUID = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        isMandatoryProcessLine = c.Boolean(),
                        isVisibleProcessLine = c.Boolean(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        filterProductTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        ProcessId = c.Int(),
                        ImportMenuId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ExcessMaterialSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.ProcessId)
                .Index(t => t.ImportMenuId);
            
            CreateTable(
                "Web.InspectionQaAttributes",
                c => new
                    {
                        InspectionQaAttributesId = c.Int(nullable: false, identity: true),
                        InspectionHeaderId = c.Int(nullable: false),
                        ProductTypeQaAttributeId = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.InspectionQaAttributesId)
                .ForeignKey("Web.ProductTypeQaAttributes", t => t.ProductTypeQaAttributeId)
                .Index(t => t.ProductTypeQaAttributeId);
            
            CreateTable(
                "Web.ProductTypeQaAttributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ProductTypeId = c.Int(nullable: false),
                        DefaultValue = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.ProductTypes", t => t.ProductTypeId)
                .Index(t => t.ProductTypeId);
            
            CreateTable(
                "Web.JobConsumptionSettings",
                c => new
                    {
                        JobConsumptionSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleMachine = c.Boolean(),
                        isMandatoryMachine = c.Boolean(),
                        isVisibleCostCenter = c.Boolean(),
                        isMandatoryCostCenter = c.Boolean(),
                        isVisibleProductUID = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        isMandatoryProcessLine = c.Boolean(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        ProcessId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobConsumptionSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.ProcessId);
            
            CreateTable(
                "Web.JobInstructions",
                c => new
                    {
                        JobInstructionId = c.Int(nullable: false, identity: true),
                        JobInstructionDescription = c.String(nullable: false, maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobInstructionId)
                .Index(t => t.JobInstructionDescription, unique: true, name: "IX_JobInstruction_JobInstructionDescription");
            
            CreateTable(
                "Web.JobInvoiceAmendmentHeaders",
                c => new
                    {
                        JobInvoiceAmendmentHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 10),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        JobWorkerId = c.Int(),
                        OrderById = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        LockReason = c.String(),
                    })
                .PrimaryKey(t => t.JobInvoiceAmendmentHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.Employees", t => t.OrderById)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.OrderById)
                .Index(t => t.ProcessId);
            
            CreateTable(
                "Web.JobInvoiceAmendmentHeaderCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        ProductChargeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.JobInvoiceAmendmentHeaders", t => t.HeaderTableId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.Charges", t => t.ProductChargeId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.ProductChargeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.JobInvoiceHeaders",
                c => new
                    {
                        JobInvoiceHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        JobWorkerDocNo = c.String(maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        LedgerHeaderId = c.Int(),
                        JobWorkerId = c.Int(),
                        Status = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        JobReceiveHeaderId = c.Int(),
                        CreditDays = c.Int(),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                    })
                .PrimaryKey(t => t.JobInvoiceHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.JobReceiveHeaders", t => t.JobReceiveHeaderId)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.LedgerHeaders", t => t.LedgerHeaderId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_JobInvoiceHeader_DocID")
                .Index(t => t.LedgerHeaderId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.ProcessId)
                .Index(t => t.JobReceiveHeaderId);
            
            CreateTable(
                "Web.JobInvoiceLines",
                c => new
                    {
                        JobInvoiceLineId = c.Int(nullable: false, identity: true),
                        JobInvoiceHeaderId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        JobReceiveLineId = c.Int(nullable: false),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(maxLength: 3),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        IncentiveRate = c.Decimal(precision: 18, scale: 4),
                        IncentiveAmt = c.Decimal(precision: 18, scale: 4),
                        Remark = c.String(),
                        Sr = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        CostCenterId = c.Int(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobInvoiceLineId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.JobInvoiceHeaders", t => t.JobInvoiceHeaderId)
                .ForeignKey("Web.JobReceiveLines", t => t.JobReceiveLineId)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .Index(t => t.JobInvoiceHeaderId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.JobReceiveLineId)
                .Index(t => t.DealUnitId)
                .Index(t => t.CostCenterId);
            
            CreateTable(
                "Web.JobReceiveLines",
                c => new
                    {
                        JobReceiveLineId = c.Int(nullable: false, identity: true),
                        JobReceiveHeaderId = c.Int(nullable: false),
                        ProductUidId = c.Int(),
                        JobOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PassQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        MachineId = c.Int(),
                        LossPer = c.Decimal(nullable: false, precision: 18, scale: 4),
                        LossQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PenaltyAmt = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PenaltyRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        IncentiveRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        IncentiveAmt = c.Decimal(precision: 18, scale: 4),
                        LotNo = c.String(maxLength: 10),
                        Remark = c.String(),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 4),
                        StockId = c.Int(),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocLineId = c.Int(),
                        StockProcessId = c.Int(),
                        Sr = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        ProductUidLastTransactionDocId = c.Int(),
                        ProductUidLastTransactionDocNo = c.String(),
                        ProductUidLastTransactionDocTypeId = c.Int(),
                        ProductUidLastTransactionDocDate = c.DateTime(),
                        ProductUidLastTransactionPersonId = c.Int(),
                        ProductUidCurrentGodownId = c.Int(),
                        ProductUidCurrentProcessId = c.Int(),
                        ProductUidStatus = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.JobReceiveLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.JobOrderLines", t => t.JobOrderLineId)
                .ForeignKey("Web.JobReceiveHeaders", t => t.JobReceiveHeaderId)
                .ForeignKey("Web.Products", t => t.MachineId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.Godowns", t => t.ProductUidCurrentGodownId)
                .ForeignKey("Web.Processes", t => t.ProductUidCurrentProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.ProductUidLastTransactionDocTypeId)
                .ForeignKey("Web.People", t => t.ProductUidLastTransactionPersonId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Stocks", t => t.StockId)
                .ForeignKey("Web.StockProcesses", t => t.StockProcessId)
                .Index(t => t.JobReceiveHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.JobOrderLineId)
                .Index(t => t.DealUnitId)
                .Index(t => t.MachineId)
                .Index(t => t.StockId)
                .Index(t => t.ReferenceDocTypeId)
                .Index(t => t.StockProcessId)
                .Index(t => t.ProductUidLastTransactionDocTypeId)
                .Index(t => t.ProductUidLastTransactionPersonId)
                .Index(t => t.ProductUidCurrentGodownId)
                .Index(t => t.ProductUidCurrentProcessId);
            
            CreateTable(
                "Web.JobReceiveHeaders",
                c => new
                    {
                        JobReceiveHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        JobWorkerDocNo = c.String(maxLength: 20),
                        JobReceiveById = c.Int(nullable: false),
                        GodownId = c.Int(nullable: false),
                        Remark = c.String(),
                        StockHeaderId = c.Int(),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobReceiveHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.Employees", t => t.JobReceiveById)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.StockHeaders", t => t.StockHeaderId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_JobReceiveHeader_DocID")
                .Index(t => t.ProcessId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.JobReceiveById)
                .Index(t => t.GodownId)
                .Index(t => t.StockHeaderId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.JobInvoiceHeaderCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        ProductChargeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.JobInvoiceHeaders", t => t.HeaderTableId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.Charges", t => t.ProductChargeId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.ProductChargeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.JobInvoiceLineCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineTableId = c.Int(nullable: false),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        DealQty = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.JobInvoiceLines", t => t.LineTableId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .Index(t => t.LineTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.JobInvoiceLineStatus",
                c => new
                    {
                        JobInvoiceLineId = c.Int(nullable: false),
                        ReturnQty = c.Decimal(precision: 18, scale: 4),
                        ReturnDealQty = c.Decimal(precision: 18, scale: 4),
                        ReturnWeight = c.Decimal(precision: 18, scale: 4),
                        ReturnDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.JobInvoiceLineId)
                .ForeignKey("Web.JobInvoiceLines", t => t.JobInvoiceLineId)
                .Index(t => t.JobInvoiceLineId);
            
            CreateTable(
                "Web.JobInvoiceRateAmendmentLines",
                c => new
                    {
                        JobInvoiceRateAmendmentLineId = c.Int(nullable: false, identity: true),
                        JobInvoiceAmendmentHeaderId = c.Int(nullable: false),
                        JobInvoiceLineId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        JobInvoiceRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        AmendedRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PenaltyAmt = c.Decimal(nullable: false, precision: 18, scale: 4),
                        IncentiveAmt = c.Decimal(precision: 18, scale: 4),
                        Remark = c.String(),
                        Sr = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        LockReason = c.String(),
                    })
                .PrimaryKey(t => t.JobInvoiceRateAmendmentLineId)
                .ForeignKey("Web.JobInvoiceAmendmentHeaders", t => t.JobInvoiceAmendmentHeaderId)
                .ForeignKey("Web.JobInvoiceLines", t => t.JobInvoiceLineId)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .Index(t => t.JobInvoiceAmendmentHeaderId)
                .Index(t => t.JobInvoiceLineId)
                .Index(t => t.JobWorkerId);
            
            CreateTable(
                "Web.JobInvoiceRateAmendmentLineCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineTableId = c.Int(nullable: false),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        DealQty = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.JobInvoiceRateAmendmentLines", t => t.LineTableId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .Index(t => t.LineTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.JobInvoiceReturnHeaders",
                c => new
                    {
                        JobInvoiceReturnHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        ReasonId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        LedgerHeaderId = c.Int(),
                        ExchangeRate = c.Decimal(precision: 18, scale: 4),
                        JobWorkerId = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        JobReturnHeaderId = c.Int(),
                        Remark = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobInvoiceReturnHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.JobReturnHeaders", t => t.JobReturnHeaderId)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.LedgerHeaders", t => t.LedgerHeaderId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_JobInvoiceReturnHeader_DocID")
                .Index(t => t.ReasonId)
                .Index(t => t.LedgerHeaderId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.ProcessId)
                .Index(t => t.JobReturnHeaderId);
            
            CreateTable(
                "Web.JobReturnHeaders",
                c => new
                    {
                        JobReturnHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        ReasonId = c.Int(nullable: false),
                        DocNo = c.String(maxLength: 10),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        OrderById = c.Int(nullable: false),
                        GodownId = c.Int(nullable: false),
                        GatePassHeaderId = c.Int(),
                        StockHeaderId = c.Int(),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                    })
                .PrimaryKey(t => t.JobReturnHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.GatePassHeaders", t => t.GatePassHeaderId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.Employees", t => t.OrderById)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.StockHeaders", t => t.StockHeaderId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ReasonId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.ProcessId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.OrderById)
                .Index(t => t.GodownId)
                .Index(t => t.GatePassHeaderId)
                .Index(t => t.StockHeaderId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.JobReturnLines",
                c => new
                    {
                        JobReturnLineId = c.Int(nullable: false, identity: true),
                        JobReturnHeaderId = c.Int(nullable: false),
                        JobReceiveLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        LossPer = c.Decimal(nullable: false, precision: 18, scale: 4),
                        LossQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(nullable: false, maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Sr = c.Int(),
                        StockId = c.Int(),
                        StockProcessId = c.Int(),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocLineId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        ProductUidLastTransactionDocId = c.Int(),
                        ProductUidLastTransactionDocNo = c.String(),
                        ProductUidLastTransactionDocTypeId = c.Int(),
                        ProductUidLastTransactionDocDate = c.DateTime(),
                        ProductUidLastTransactionPersonId = c.Int(),
                        ProductUidCurrentGodownId = c.Int(),
                        ProductUidCurrentProcessId = c.Int(),
                        ProductUidStatus = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.JobReturnLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.JobReceiveLines", t => t.JobReceiveLineId)
                .ForeignKey("Web.Godowns", t => t.ProductUidCurrentGodownId)
                .ForeignKey("Web.Processes", t => t.ProductUidCurrentProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.ProductUidLastTransactionDocTypeId)
                .ForeignKey("Web.People", t => t.ProductUidLastTransactionPersonId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Stocks", t => t.StockId)
                .ForeignKey("Web.StockProcesses", t => t.StockProcessId)
                .ForeignKey("Web.JobReturnHeaders", t => t.JobReturnHeaderId)
                .Index(t => t.JobReturnHeaderId)
                .Index(t => t.JobReceiveLineId)
                .Index(t => t.DealUnitId)
                .Index(t => t.StockId)
                .Index(t => t.StockProcessId)
                .Index(t => t.ReferenceDocTypeId)
                .Index(t => t.ProductUidLastTransactionDocTypeId)
                .Index(t => t.ProductUidLastTransactionPersonId)
                .Index(t => t.ProductUidCurrentGodownId)
                .Index(t => t.ProductUidCurrentProcessId);
            
            CreateTable(
                "Web.JobInvoiceReturnHeaderCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        ProductChargeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.JobInvoiceReturnHeaders", t => t.HeaderTableId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.Charges", t => t.ProductChargeId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.ProductChargeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.JobInvoiceReturnLines",
                c => new
                    {
                        JobInvoiceReturnLineId = c.Int(nullable: false, identity: true),
                        JobInvoiceReturnHeaderId = c.Int(nullable: false),
                        JobInvoiceLineId = c.Int(nullable: false),
                        JobReturnLineId = c.Int(),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(nullable: false, maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        Sr = c.Int(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobInvoiceReturnLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.JobInvoiceLines", t => t.JobInvoiceLineId)
                .ForeignKey("Web.JobInvoiceReturnHeaders", t => t.JobInvoiceReturnHeaderId)
                .ForeignKey("Web.JobReturnLines", t => t.JobReturnLineId)
                .Index(t => t.JobInvoiceReturnHeaderId)
                .Index(t => t.JobInvoiceLineId)
                .Index(t => t.JobReturnLineId)
                .Index(t => t.DealUnitId);
            
            CreateTable(
                "Web.JobInvoiceReturnLineCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineTableId = c.Int(nullable: false),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        DealQty = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.JobInvoiceReturnLines", t => t.LineTableId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .Index(t => t.LineTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.JobInvoiceSettings",
                c => new
                    {
                        JobInvoiceSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleMachine = c.Boolean(),
                        isMandatoryMachine = c.Boolean(),
                        isVisibleProductUID = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        isVisibleLoss = c.Boolean(),
                        isVisibleHeaderJobWorker = c.Boolean(),
                        isPostedInStock = c.Boolean(),
                        isPostedInStockProcess = c.Boolean(),
                        isPostedInStockVirtual = c.Boolean(),
                        isAutoCreateJobReceive = c.Boolean(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        DocumentPrint = c.String(maxLength: 100),
                        SqlProcConsumption = c.String(maxLength: 100),
                        SqlProcGatePass = c.String(maxLength: 100),
                        SqlProcGenProductUID = c.String(),
                        filterLedgerAccountGroups = c.String(),
                        filterLedgerAccounts = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        ProcessId = c.Int(nullable: false),
                        ImportMenuId = c.Int(),
                        WizardMenuId = c.Int(),
                        CalculationId = c.Int(),
                        JobReceiveDocTypeId = c.Int(),
                        AmountRoundOff = c.Int(),
                        JobReturnDocTypeId = c.Int(),
                        BarcodeStatusUpdate = c.String(maxLength: 20),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobInvoiceSettingsId)
                .ForeignKey("Web.Calculations", t => t.CalculationId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.DocumentTypes", t => t.JobReceiveDocTypeId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.JobReturnDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Menus", t => t.WizardMenuId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.ProcessId)
                .Index(t => t.ImportMenuId)
                .Index(t => t.WizardMenuId)
                .Index(t => t.CalculationId)
                .Index(t => t.JobReceiveDocTypeId)
                .Index(t => t.JobReturnDocTypeId);
            
            CreateTable(
                "Web.JobOrderAmendmentHeaders",
                c => new
                    {
                        JobOrderAmendmentHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 10),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        JobWorkerId = c.Int(),
                        OrderById = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        LedgerHeaderId = c.Int(),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        LockReason = c.String(),
                    })
                .PrimaryKey(t => t.JobOrderAmendmentHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.LedgerHeaders", t => t.LedgerHeaderId)
                .ForeignKey("Web.Employees", t => t.OrderById)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.OrderById)
                .Index(t => t.ProcessId)
                .Index(t => t.LedgerHeaderId);
            
            CreateTable(
                "Web.JobOrderByProducts",
                c => new
                    {
                        JobOrderByProductId = c.Int(nullable: false, identity: true),
                        JobOrderHeaderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobOrderByProductId)
                .ForeignKey("Web.JobOrderHeaders", t => t.JobOrderHeaderId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.JobOrderHeaderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Web.JobOrderCancelBoms",
                c => new
                    {
                        JobOrderCancelBomId = c.Int(nullable: false, identity: true),
                        JobOrderCancelHeaderId = c.Int(nullable: false),
                        JobOrderCancelLineId = c.Int(),
                        JobOrderHeaderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobOrderCancelBomId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.JobOrderCancelHeaders", t => t.JobOrderCancelHeaderId)
                .ForeignKey("Web.JobOrderCancelLines", t => t.JobOrderCancelLineId)
                .ForeignKey("Web.JobOrderHeaders", t => t.JobOrderHeaderId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.JobOrderCancelHeaderId)
                .Index(t => t.JobOrderCancelLineId)
                .Index(t => t.JobOrderHeaderId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.JobOrderCancelHeaders",
                c => new
                    {
                        JobOrderCancelHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        ReasonId = c.Int(nullable: false),
                        DocNo = c.String(maxLength: 10),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        OrderById = c.Int(nullable: false),
                        GodownId = c.Int(),
                        StockHeaderId = c.Int(),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                    })
                .PrimaryKey(t => t.JobOrderCancelHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.Employees", t => t.OrderById)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.StockHeaders", t => t.StockHeaderId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ReasonId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.ProcessId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.OrderById)
                .Index(t => t.GodownId)
                .Index(t => t.StockHeaderId);
            
            CreateTable(
                "Web.JobOrderCancelLines",
                c => new
                    {
                        JobOrderCancelLineId = c.Int(nullable: false, identity: true),
                        JobOrderCancelHeaderId = c.Int(nullable: false),
                        ProductUidId = c.Int(),
                        JobOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        StockId = c.Int(),
                        StockProcessId = c.Int(),
                        Sr = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        ProductUidLastTransactionDocId = c.Int(),
                        ProductUidLastTransactionDocNo = c.String(),
                        ProductUidLastTransactionDocTypeId = c.Int(),
                        ProductUidLastTransactionDocDate = c.DateTime(),
                        ProductUidLastTransactionPersonId = c.Int(),
                        ProductUidCurrentGodownId = c.Int(),
                        ProductUidCurrentProcessId = c.Int(),
                        ProductUidStatus = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.JobOrderCancelLineId)
                .ForeignKey("Web.JobOrderLines", t => t.JobOrderLineId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.Godowns", t => t.ProductUidCurrentGodownId)
                .ForeignKey("Web.Processes", t => t.ProductUidCurrentProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.ProductUidLastTransactionDocTypeId)
                .ForeignKey("Web.People", t => t.ProductUidLastTransactionPersonId)
                .ForeignKey("Web.Stocks", t => t.StockId)
                .ForeignKey("Web.StockProcesses", t => t.StockProcessId)
                .ForeignKey("Web.JobOrderCancelHeaders", t => t.JobOrderCancelHeaderId)
                .Index(t => t.JobOrderCancelHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.JobOrderLineId)
                .Index(t => t.StockId)
                .Index(t => t.StockProcessId)
                .Index(t => t.ProductUidLastTransactionDocTypeId)
                .Index(t => t.ProductUidLastTransactionPersonId)
                .Index(t => t.ProductUidCurrentGodownId)
                .Index(t => t.ProductUidCurrentProcessId);
            
            CreateTable(
                "Web.JobOrderHeaderCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        ProductChargeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.JobOrderHeaders", t => t.HeaderTableId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.Charges", t => t.ProductChargeId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.ProductChargeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.JobOrderHeaderStatus",
                c => new
                    {
                        JobOrderHeaderId = c.Int(nullable: false),
                        BOMQty = c.Decimal(precision: 18, scale: 4),
                        IsTimeIncentiveProcessed = c.Boolean(),
                        ReceiveQty = c.Decimal(precision: 18, scale: 4),
                        ReceiveDealQty = c.Decimal(precision: 18, scale: 4),
                        ReceiveDate = c.DateTime(),
                        CancelQty = c.Decimal(precision: 18, scale: 4),
                        CancelDealQty = c.Decimal(precision: 18, scale: 4),
                        CancelDate = c.DateTime(),
                        IsTimePenaltyProcessed = c.Boolean(),
                        IsSmallChunkPenaltyProcessed = c.Boolean(),
                        TimePenaltyCount = c.Int(),
                        JobReceiveCount = c.Int(),
                        TimeIncentiveId = c.Int(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobOrderHeaderId)
                .ForeignKey("Web.JobOrderHeaders", t => t.JobOrderHeaderId)
                .Index(t => t.JobOrderHeaderId);
            
            CreateTable(
                "Web.JobOrderInspectionHeaders",
                c => new
                    {
                        JobOrderInspectionHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        InspectionById = c.Int(nullable: false),
                        Remark = c.String(),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobOrderInspectionHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Employees", t => t.InspectionById)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_JobOrderInspectionHeader_DocID")
                .Index(t => t.ProcessId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.InspectionById)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.JobOrderInspectionLines",
                c => new
                    {
                        JobOrderInspectionLineId = c.Int(nullable: false, identity: true),
                        JobOrderInspectionHeaderId = c.Int(nullable: false),
                        Sr = c.Int(),
                        ProductUidId = c.Int(),
                        JobOrderInspectionRequestLineId = c.Int(),
                        JobOrderLineId = c.Int(nullable: false),
                        InspectedQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Marks = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        ImageFolderName = c.String(maxLength: 100),
                        ImageFileName = c.String(maxLength: 100),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocLineId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobOrderInspectionLineId)
                .ForeignKey("Web.JobOrderInspectionHeaders", t => t.JobOrderInspectionHeaderId)
                .ForeignKey("Web.JobOrderInspectionRequestLines", t => t.JobOrderInspectionRequestLineId)
                .ForeignKey("Web.JobOrderLines", t => t.JobOrderLineId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .Index(t => t.JobOrderInspectionHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.JobOrderInspectionRequestLineId)
                .Index(t => t.JobOrderLineId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.JobOrderInspectionRequestLines",
                c => new
                    {
                        JobOrderInspectionRequestLineId = c.Int(nullable: false, identity: true),
                        JobOrderInspectionRequestHeaderId = c.Int(nullable: false),
                        Sr = c.Int(),
                        ProductUidId = c.Int(),
                        JobOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobOrderInspectionRequestLineId)
                .ForeignKey("Web.JobOrderInspectionRequestHeaders", t => t.JobOrderInspectionRequestHeaderId)
                .ForeignKey("Web.JobOrderLines", t => t.JobOrderLineId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .Index(t => t.JobOrderInspectionRequestHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.JobOrderLineId);
            
            CreateTable(
                "Web.JobOrderInspectionRequestHeaders",
                c => new
                    {
                        JobOrderInspectionRequestHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 10),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        RequestBy = c.String(maxLength: 10),
                        AcceptedYn = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                    })
                .PrimaryKey(t => t.JobOrderInspectionRequestHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.ProcessId)
                .Index(t => t.JobWorkerId);
            
            CreateTable(
                "Web.JobOrderInspectionRequestCancelHeaders",
                c => new
                    {
                        JobOrderInspectionRequestCancelHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 10),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        ReasonId = c.Int(nullable: false),
                        RequestBy = c.String(maxLength: 10),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                    })
                .PrimaryKey(t => t.JobOrderInspectionRequestCancelHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.ProcessId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.ReasonId);
            
            CreateTable(
                "Web.JobOrderInspectionRequestCancelLines",
                c => new
                    {
                        JobOrderInspectionRequestCancelLineId = c.Int(nullable: false, identity: true),
                        JobOrderInspectionRequestCancelHeaderId = c.Int(nullable: false),
                        Sr = c.Int(),
                        ProductUidId = c.Int(),
                        JobOrderInspectionRequestLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobOrderInspectionRequestCancelLineId)
                .ForeignKey("Web.JobOrderInspectionRequestCancelHeaders", t => t.JobOrderInspectionRequestCancelHeaderId)
                .ForeignKey("Web.JobOrderInspectionRequestLines", t => t.JobOrderInspectionRequestLineId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .Index(t => t.JobOrderInspectionRequestCancelHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.JobOrderInspectionRequestLineId);
            
            CreateTable(
                "Web.JobOrderInspectionRequestSettings",
                c => new
                    {
                        JobOrderInspectionRequestSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        ImportMenuId = c.Int(),
                        isVisibleProductUID = c.Boolean(),
                        isMandatoryProductUID = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        DocumentPrint = c.String(maxLength: 100),
                        ProcessId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobOrderInspectionRequestSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ImportMenuId)
                .Index(t => t.ProcessId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.JobOrderInspectionSettings",
                c => new
                    {
                        JobOrderInspectionSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        ImportMenuId = c.Int(),
                        isVisibleProductUID = c.Boolean(),
                        isMandatoryProductUID = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        ProcessId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        DocumentPrint = c.String(maxLength: 100),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobOrderInspectionSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ImportMenuId)
                .Index(t => t.ProcessId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.JobOrderJobOrders",
                c => new
                    {
                        JobOrderJobOrderId = c.Int(nullable: false, identity: true),
                        JobOrderHeaderId = c.Int(nullable: false),
                        GenJobOrderHeaderId = c.Int(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobOrderJobOrderId)
                .ForeignKey("Web.JobOrderHeaders", t => t.GenJobOrderHeaderId)
                .ForeignKey("Web.JobOrderHeaders", t => t.JobOrderHeaderId)
                .Index(t => t.JobOrderHeaderId)
                .Index(t => t.GenJobOrderHeaderId);
            
            CreateTable(
                "Web.JobOrderLineCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineTableId = c.Int(nullable: false),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        DealQty = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.JobOrderLines", t => t.LineTableId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .Index(t => t.LineTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.JobOrderLineExtendeds",
                c => new
                    {
                        JobOrderLineId = c.Int(nullable: false),
                        OtherUnitId = c.String(maxLength: 3),
                        OtherUnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OtherRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OtherAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OtherQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.JobOrderLineId)
                .ForeignKey("Web.JobOrderLines", t => t.JobOrderLineId)
                .ForeignKey("Web.Units", t => t.OtherUnitId)
                .Index(t => t.JobOrderLineId)
                .Index(t => t.OtherUnitId);
            
            CreateTable(
                "Web.JobOrderLineStatus",
                c => new
                    {
                        JobOrderLineId = c.Int(nullable: false),
                        CancelQty = c.Decimal(precision: 18, scale: 4),
                        CancelDealQty = c.Decimal(precision: 18, scale: 4),
                        CancelDate = c.DateTime(),
                        AmendmentQty = c.Decimal(precision: 18, scale: 4),
                        AmendmentDealQty = c.Decimal(precision: 18, scale: 4),
                        AmendmentDate = c.DateTime(),
                        ReceiveQty = c.Decimal(precision: 18, scale: 4),
                        ReceiveDealQty = c.Decimal(precision: 18, scale: 4),
                        ReceiveDate = c.DateTime(),
                        InvoiceQty = c.Decimal(precision: 18, scale: 4),
                        InvoiceDealQty = c.Decimal(precision: 18, scale: 4),
                        InvoiceDate = c.DateTime(),
                        PaymentQty = c.Decimal(precision: 18, scale: 4),
                        PaymentDate = c.DateTime(),
                        ReturnQty = c.Decimal(precision: 18, scale: 4),
                        ReturnDealQty = c.Decimal(precision: 18, scale: 4),
                        ReturnDate = c.DateTime(),
                        RateAmendmentRate = c.Decimal(precision: 18, scale: 4),
                        RateAmendmentDate = c.DateTime(),
                        ExcessJobReceiveReviewBy = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.JobOrderLineId)
                .ForeignKey("Web.JobOrderLines", t => t.JobOrderLineId)
                .Index(t => t.JobOrderLineId);
            
            CreateTable(
                "Web.JobOrderPerks",
                c => new
                    {
                        JobOrderPerkId = c.Int(nullable: false, identity: true),
                        JobOrderHeaderId = c.Int(nullable: false),
                        PerkId = c.Int(nullable: false),
                        Base = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Worth = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CostConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobOrderPerkId)
                .ForeignKey("Web.JobOrderHeaders", t => t.JobOrderHeaderId)
                .ForeignKey("Web.Perks", t => t.PerkId)
                .Index(t => t.JobOrderHeaderId)
                .Index(t => t.PerkId);
            
            CreateTable(
                "Web.Perks",
                c => new
                    {
                        PerkId = c.Int(nullable: false, identity: true),
                        PerkName = c.String(nullable: false, maxLength: 50),
                        BaseDescription = c.String(),
                        Base = c.Decimal(nullable: false, precision: 18, scale: 4),
                        WorthDescription = c.String(),
                        Worth = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CostConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PerkId)
                .Index(t => t.PerkName, unique: true, name: "IX_Perk_Perk");
            
            CreateTable(
                "Web.JobOrderQtyAmendmentLines",
                c => new
                    {
                        JobOrderQtyAmendmentLineId = c.Int(nullable: false, identity: true),
                        JobOrderAmendmentHeaderId = c.Int(nullable: false),
                        JobOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Sr = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobOrderQtyAmendmentLineId)
                .ForeignKey("Web.JobOrderAmendmentHeaders", t => t.JobOrderAmendmentHeaderId)
                .ForeignKey("Web.JobOrderLines", t => t.JobOrderLineId)
                .Index(t => t.JobOrderAmendmentHeaderId)
                .Index(t => t.JobOrderLineId);
            
            CreateTable(
                "Web.JobOrderRateAmendmentLines",
                c => new
                    {
                        JobOrderRateAmendmentLineId = c.Int(nullable: false, identity: true),
                        JobOrderAmendmentHeaderId = c.Int(nullable: false),
                        JobOrderLineId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        JobOrderRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        AmendedRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        Sr = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        LockReason = c.String(),
                    })
                .PrimaryKey(t => t.JobOrderRateAmendmentLineId)
                .ForeignKey("Web.JobOrderAmendmentHeaders", t => t.JobOrderAmendmentHeaderId)
                .ForeignKey("Web.JobOrderLines", t => t.JobOrderLineId)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .Index(t => t.JobOrderAmendmentHeaderId)
                .Index(t => t.JobOrderLineId)
                .Index(t => t.JobWorkerId);
            
            CreateTable(
                "Web.JobOrderSettings",
                c => new
                    {
                        JobOrderSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleMachine = c.Boolean(),
                        isMandatoryMachine = c.Boolean(),
                        isVisibleCostCenter = c.Boolean(),
                        isMandatoryCostCenter = c.Boolean(),
                        isVisibleProductUID = c.Boolean(),
                        isMandatoryProductUID = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisibleRate = c.Boolean(),
                        isMandatoryRate = c.Boolean(),
                        isVisibleGodown = c.Boolean(),
                        isMandatoryGodown = c.Boolean(),
                        isEditableRate = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        isVisibleLoss = c.Boolean(),
                        isVisibleUncountableQty = c.Boolean(),
                        isMandatoryProcessLine = c.Boolean(),
                        isVisibleProcessLine = c.Boolean(),
                        isVisibleJobWorkerLine = c.Boolean(),
                        isUniqueCostCenter = c.Boolean(),
                        PersonWiseCostCenter = c.Boolean(),
                        isPostedInStock = c.Boolean(),
                        isPostedInStockProcess = c.Boolean(),
                        isPostedInStockVirtual = c.Boolean(),
                        RetensionCostCenter = c.Int(),
                        isVisibleFromProdOrder = c.Boolean(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        SqlProcConsumption = c.String(maxLength: 100),
                        SqlProcGenProductUID = c.String(maxLength: 100),
                        DocumentPrint = c.String(maxLength: 100),
                        SqlProcGatePass = c.String(maxLength: 100),
                        filterLedgerAccountGroups = c.String(),
                        filterLedgerAccounts = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        UnitConversionForId = c.Byte(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        CalculationId = c.Int(),
                        Perks = c.String(),
                        ImportMenuId = c.Int(),
                        WizardMenuId = c.Int(),
                        JobUnitId = c.String(maxLength: 3),
                        OnSubmitMenuId = c.Int(),
                        OnApproveMenuId = c.Int(),
                        NonCountedQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        LossQty = c.Decimal(precision: 18, scale: 4),
                        DealUnitId = c.String(maxLength: 3),
                        DueDays = c.Int(),
                        AmountRoundOff = c.Int(),
                        BarcodeStatusUpdate = c.String(maxLength: 20),
                        FilterProductDivision = c.String(),
                        Event_OnHeaderSave = c.String(),
                        Event_OnHeaderDelete = c.String(),
                        Event_OnHeaderSubmit = c.String(),
                        Event_OnHeaderApprove = c.String(),
                        Event_OnHeaderPrint = c.String(),
                        Event_OnLineSave = c.String(),
                        Event_OnLineDelete = c.String(),
                        Event_AfterHeaderSave = c.String(),
                        Event_AfterHeaderDelete = c.String(),
                        Event_AfterHeaderSubmit = c.String(),
                        Event_AfterHeaderApprove = c.String(),
                        Event_AfterLineSave = c.String(),
                        Event_AfterLineDelete = c.String(),
                        MaxDays = c.Int(),
                        ExcessQtyAllowedPer = c.Int(nullable: false),
                        NoOfPrintCopies = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobOrderSettingsId)
                .ForeignKey("Web.Calculations", t => t.CalculationId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Units", t => t.JobUnitId)
                .ForeignKey("Web.Menus", t => t.OnApproveMenuId)
                .ForeignKey("Web.Menus", t => t.OnSubmitMenuId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .ForeignKey("Web.Menus", t => t.WizardMenuId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.UnitConversionForId)
                .Index(t => t.ProcessId)
                .Index(t => t.CalculationId)
                .Index(t => t.ImportMenuId)
                .Index(t => t.WizardMenuId)
                .Index(t => t.JobUnitId)
                .Index(t => t.OnSubmitMenuId)
                .Index(t => t.OnApproveMenuId)
                .Index(t => t.DealUnitId);
            
            CreateTable(
                "Web.JobReceiveBoms",
                c => new
                    {
                        JobReceiveBomId = c.Int(nullable: false, identity: true),
                        JobReceiveHeaderId = c.Int(nullable: false),
                        JobReceiveLineId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        CostCenterId = c.Int(),
                        LotNo = c.String(maxLength: 50),
                        StockProcessId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobReceiveBomId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.JobReceiveHeaders", t => t.JobReceiveHeaderId)
                .ForeignKey("Web.JobReceiveLines", t => t.JobReceiveLineId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.StockProcesses", t => t.StockProcessId)
                .Index(t => t.JobReceiveHeaderId)
                .Index(t => t.JobReceiveLineId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.CostCenterId)
                .Index(t => t.StockProcessId);
            
            CreateTable(
                "Web.JobReceiveByProducts",
                c => new
                    {
                        JobReceiveByProductId = c.Int(nullable: false, identity: true),
                        JobReceiveHeaderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        LotNo = c.String(maxLength: 50),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobReceiveByProductId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.JobReceiveHeaders", t => t.JobReceiveHeaderId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.JobReceiveHeaderId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.JobReceiveLineStatus",
                c => new
                    {
                        JobReceiveLineId = c.Int(nullable: false),
                        QaFailQty = c.Decimal(precision: 18, scale: 4),
                        QaFailDealQty = c.Decimal(precision: 18, scale: 4),
                        QaWeight = c.Decimal(precision: 18, scale: 4),
                        QaPenalty = c.Decimal(precision: 18, scale: 4),
                        QaDate = c.DateTime(),
                        ReturnQty = c.Decimal(precision: 18, scale: 4),
                        ReturnDealQty = c.Decimal(precision: 18, scale: 4),
                        ReturnWeight = c.Decimal(precision: 18, scale: 4),
                        ReturnDate = c.DateTime(),
                        InvoiceQty = c.Decimal(precision: 18, scale: 4),
                        InvoiceDealQty = c.Decimal(precision: 18, scale: 4),
                        InvoiceWeight = c.Decimal(precision: 18, scale: 4),
                        InvoiceDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.JobReceiveLineId)
                .ForeignKey("Web.JobReceiveLines", t => t.JobReceiveLineId)
                .Index(t => t.JobReceiveLineId);
            
            CreateTable(
                "Web.JobReceiveQAHeaders",
                c => new
                    {
                        JobReceiveQAHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        QAById = c.Int(nullable: false),
                        Remark = c.String(),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobReceiveQAHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Employees", t => t.QAById)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_JobReceiveQAHeader_DocID")
                .Index(t => t.ProcessId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.QAById)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.JobReceiveQALines",
                c => new
                    {
                        JobReceiveQALineId = c.Int(nullable: false, identity: true),
                        JobReceiveQAHeaderId = c.Int(nullable: false),
                        Sr = c.Int(),
                        ProductUidId = c.Int(),
                        JobReceiveLineId = c.Int(nullable: false),
                        QAQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        InspectedQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        FailQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        FailDealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PenaltyRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PenaltyAmt = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Marks = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        ImageFolderName = c.String(maxLength: 100),
                        ImageFileName = c.String(maxLength: 100),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocLineId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobReceiveQALineId)
                .ForeignKey("Web.JobReceiveLines", t => t.JobReceiveLineId)
                .ForeignKey("Web.JobReceiveQAHeaders", t => t.JobReceiveQAHeaderId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .Index(t => t.JobReceiveQAHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.JobReceiveLineId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.JobReceiveQASettings",
                c => new
                    {
                        JobReceiveQASettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        ImportMenuId = c.Int(),
                        WizardMenuId = c.Int(),
                        isVisibleProductUID = c.Boolean(),
                        isMandatoryProductUID = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        ProcessId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        DocumentPrint = c.String(maxLength: 100),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobReceiveQASettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Menus", t => t.WizardMenuId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ImportMenuId)
                .Index(t => t.WizardMenuId)
                .Index(t => t.ProcessId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.JobReceiveSettings",
                c => new
                    {
                        JobReceiveSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleMachine = c.Boolean(),
                        isMandatoryMachine = c.Boolean(),
                        isVisibleProductUID = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        isVisibleLoss = c.Boolean(),
                        IsVisibleWeight = c.Boolean(),
                        IsMandatoryWeight = c.Boolean(),
                        IsVisibleForOrderMultiple = c.Boolean(),
                        isPostedInStock = c.Boolean(),
                        isPostedInStockProcess = c.Boolean(),
                        isPostedInStockVirtual = c.Boolean(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        SqlProcGatePass = c.String(maxLength: 100),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        DocumentPrint = c.String(maxLength: 100),
                        SqlProcConsumption = c.String(maxLength: 100),
                        SqlProcGenProductUID = c.String(),
                        filterLedgerAccountGroups = c.String(),
                        filterLedgerAccounts = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        ProcessId = c.Int(nullable: false),
                        ImportMenuId = c.Int(),
                        WizardMenuId = c.Int(),
                        OnSubmitMenuId = c.Int(),
                        OnApproveMenuId = c.Int(),
                        CalculationId = c.Int(),
                        BarcodeStatusUpdate = c.String(maxLength: 20),
                        StockQty = c.String(maxLength: 20),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobReceiveSettingsId)
                .ForeignKey("Web.Calculations", t => t.CalculationId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Menus", t => t.OnApproveMenuId)
                .ForeignKey("Web.Menus", t => t.OnSubmitMenuId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Menus", t => t.WizardMenuId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.ProcessId)
                .Index(t => t.ImportMenuId)
                .Index(t => t.WizardMenuId)
                .Index(t => t.OnSubmitMenuId)
                .Index(t => t.OnApproveMenuId)
                .Index(t => t.CalculationId);
            
            CreateTable(
                "Web.JobReturnBoms",
                c => new
                    {
                        JobReturnBomId = c.Int(nullable: false, identity: true),
                        JobReturnHeaderId = c.Int(nullable: false),
                        JobReturnLineId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        CostCenterId = c.Int(),
                        LotNo = c.String(maxLength: 50),
                        StockProcessId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.JobReturnBomId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.JobReturnHeaders", t => t.JobReturnHeaderId)
                .ForeignKey("Web.JobReturnLines", t => t.JobReturnLineId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.StockProcesses", t => t.StockProcessId)
                .Index(t => t.JobReturnHeaderId)
                .Index(t => t.JobReturnLineId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.CostCenterId)
                .Index(t => t.StockProcessId);
            
            CreateTable(
                "Web.LeaveTypes",
                c => new
                    {
                        LeaveTypeId = c.Int(nullable: false, identity: true),
                        LeaveTypeName = c.String(nullable: false, maxLength: 50),
                        SiteId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.LeaveTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.LeaveTypeName, unique: true, name: "IX_LeaveType_LeaveTypeName")
                .Index(t => t.SiteId);
            
            CreateTable(
                "Web.Ledgers",
                c => new
                    {
                        LedgerId = c.Int(nullable: false, identity: true),
                        LedgerHeaderId = c.Int(nullable: false),
                        LedgerLineId = c.Int(),
                        LedgerAccountId = c.Int(nullable: false),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        AmtDr = c.Decimal(nullable: false, precision: 18, scale: 4),
                        AmtCr = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Narration = c.String(maxLength: 250),
                        ContraText = c.String(),
                        ChqNo = c.String(maxLength: 10),
                        DueDate = c.DateTime(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.LedgerId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountId)
                .ForeignKey("Web.LedgerHeaders", t => t.LedgerHeaderId)
                .ForeignKey("Web.LedgerLines", t => t.LedgerLineId)
                .Index(t => t.LedgerHeaderId)
                .Index(t => t.LedgerLineId)
                .Index(t => t.LedgerAccountId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId);
            
            CreateTable(
                "Web.LedgerLines",
                c => new
                    {
                        LedgerLineId = c.Int(nullable: false, identity: true),
                        LedgerHeaderId = c.Int(nullable: false),
                        LedgerAccountId = c.Int(nullable: false),
                        ReferenceId = c.Int(),
                        ChqNo = c.String(maxLength: 10),
                        ChqDate = c.DateTime(),
                        CostCenterId = c.Int(),
                        BaseValue = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BaseRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        ReferenceDocLineId = c.Int(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.LedgerLineId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountId)
                .ForeignKey("Web.LedgerHeaders", t => t.LedgerHeaderId)
                .Index(t => t.LedgerHeaderId)
                .Index(t => t.LedgerAccountId)
                .Index(t => t.CostCenterId);
            
            CreateTable(
                "Web.LedgerAdjs",
                c => new
                    {
                        LedgerAdjId = c.Int(nullable: false, identity: true),
                        LedgerId = c.Int(nullable: false),
                        DrLedgerId = c.Int(),
                        CrLedgerId = c.Int(),
                        SiteId = c.Int(nullable: false),
                        Adj_Type = c.String(maxLength: 20),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.LedgerAdjId)
                .ForeignKey("Web.Ledgers", t => t.CrLedgerId)
                .ForeignKey("Web.Ledgers", t => t.DrLedgerId)
                .ForeignKey("Web.Ledgers", t => t.LedgerId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.LedgerId)
                .Index(t => t.DrLedgerId)
                .Index(t => t.CrLedgerId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "Web.LedgerLineRefValues",
                c => new
                    {
                        LedgerLineRefValueId = c.Int(nullable: false, identity: true),
                        LedgerLineId = c.Int(nullable: false),
                        Head = c.String(maxLength: 50),
                        Value = c.String(maxLength: 50),
                        OMSId = c.String(),
                    })
                .PrimaryKey(t => t.LedgerLineRefValueId)
                .ForeignKey("Web.LedgerLines", t => t.LedgerLineId)
                .Index(t => t.LedgerLineId);
            
            CreateTable(
                "Web.LedgerSettings",
                c => new
                    {
                        LedgerSettingId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleLineCostCenter = c.Boolean(),
                        isMandatoryLineCostCenter = c.Boolean(),
                        isVisibleHeaderCostCenter = c.Boolean(),
                        isMandatoryHeaderCostCenter = c.Boolean(),
                        isVisibleChqNo = c.Boolean(),
                        isMandatoryChqNo = c.Boolean(),
                        isVisibleRefNo = c.Boolean(),
                        isMandatoryRefNo = c.Boolean(),
                        isVisibleProcess = c.Boolean(),
                        isMandatoryProcess = c.Boolean(),
                        isVisibleGodown = c.Boolean(),
                        isMandatoryGodown = c.Boolean(),
                        filterLedgerAccountGroupHeaders = c.String(),
                        filterPersonProcessHeaders = c.String(),
                        filterPersonProcessLines = c.String(),
                        filterLedgerAccountGroupLines = c.String(),
                        filterDocTypeCostCenter = c.String(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        SqlProcReferenceNo = c.String(),
                        ProcessId = c.Int(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        BaseValueText = c.String(maxLength: 50),
                        BaseRateText = c.String(maxLength: 50),
                        WizardMenuId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.LedgerSettingId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Menus", t => t.WizardMenuId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.ProcessId)
                .Index(t => t.WizardMenuId);
            
            CreateTable(
                "Web.Manufacturers",
                c => new
                    {
                        PersonID = c.Int(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("Web.People", t => t.PersonID)
                .Index(t => t.PersonID);
            
            CreateTable(
                "Web.StockHeaderSettings",
                c => new
                    {
                        StockHeaderSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleMachine = c.Boolean(),
                        isMandatoryMachine = c.Boolean(),
                        isVisibleHeaderCostCenter = c.Boolean(),
                        isMandatoryHeaderCostCenter = c.Boolean(),
                        isVisibleLineCostCenter = c.Boolean(),
                        isMandatoryLineCostCenter = c.Boolean(),
                        isVisibleProductUID = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisibleRate = c.Boolean(),
                        isVisibleSpecification = c.Boolean(),
                        isMandatoryRate = c.Boolean(),
                        isEditableRate = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        isMandatoryProcessLine = c.Boolean(),
                        isVisibleProcessLine = c.Boolean(),
                        isPostedInStockProcess = c.Boolean(),
                        isPostedInLedger = c.Boolean(),
                        isProductHelpFromStockProcess = c.Boolean(),
                        AdjLedgerAccountId = c.Int(),
                        isVisibleMaterialRequest = c.Boolean(),
                        PersonFieldHeading = c.String(maxLength: 50),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlFuncCurrentStock = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcGatePass = c.String(maxLength: 100),
                        SqlProcStockProcessPost = c.String(maxLength: 100),
                        SqlProcStockProcessBalance = c.String(maxLength: 100),
                        filterProductTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraProductDivisions = c.String(),
                        filterContraDocTypes = c.String(),
                        isVisibleWeight = c.Boolean(),
                        WeightCaption = c.String(),
                        LineRoundOff = c.Int(),
                        ProcessId = c.Int(),
                        OnSubmitMenuId = c.Int(),
                        ImportMenuId = c.Int(),
                        BarcodeStatusUpdate = c.String(maxLength: 20),
                        NoOfPrintCopies = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StockHeaderSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Menus", t => t.OnSubmitMenuId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.ProcessId)
                .Index(t => t.OnSubmitMenuId)
                .Index(t => t.ImportMenuId);
            
            CreateTable(
                "Web.MaterialPlanForJobOrders",
                c => new
                    {
                        MaterialPlanForJobOrderId = c.Int(nullable: false, identity: true),
                        MaterialPlanHeaderId = c.Int(nullable: false),
                        JobOrderLineId = c.Int(nullable: false),
                        MaterialPlanLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OMSId = c.String(maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MaterialPlanForJobOrderId)
                .ForeignKey("Web.JobOrderLines", t => t.JobOrderLineId)
                .ForeignKey("Web.MaterialPlanHeaders", t => t.MaterialPlanHeaderId)
                .ForeignKey("Web.MaterialPlanLines", t => t.MaterialPlanLineId)
                .Index(t => t.MaterialPlanHeaderId)
                .Index(t => t.JobOrderLineId)
                .Index(t => t.MaterialPlanLineId);
            
            CreateTable(
                "Web.MaterialPlanForJobOrderLines",
                c => new
                    {
                        MaterialPlanForJobOrderLineId = c.Int(nullable: false, identity: true),
                        MaterialPlanForJobOrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Specification = c.String(maxLength: 50),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        MaterialPlanLineId = c.Int(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.MaterialPlanForJobOrderLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.MaterialPlanForJobOrders", t => t.MaterialPlanForJobOrderId)
                .ForeignKey("Web.MaterialPlanLines", t => t.MaterialPlanLineId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.MaterialPlanForJobOrderId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.MaterialPlanLineId);
            
            CreateTable(
                "Web.MaterialPlanForProdOrders",
                c => new
                    {
                        MaterialPlanForProdOrderId = c.Int(nullable: false, identity: true),
                        MaterialPlanHeaderId = c.Int(nullable: false),
                        ProdOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OMSId = c.String(maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Sr = c.Int(),
                    })
                .PrimaryKey(t => t.MaterialPlanForProdOrderId)
                .ForeignKey("Web.MaterialPlanHeaders", t => t.MaterialPlanHeaderId)
                .ForeignKey("Web.ProdOrderLines", t => t.ProdOrderLineId)
                .Index(t => t.MaterialPlanHeaderId)
                .Index(t => t.ProdOrderLineId);
            
            CreateTable(
                "Web.MaterialPlanForProdOrderLines",
                c => new
                    {
                        MaterialPlanForProdOrderLineId = c.Int(nullable: false, identity: true),
                        MaterialPlanForProdOrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        ProcessId = c.Int(),
                        MaterialPlanLineId = c.Int(),
                        OMSId = c.String(maxLength: 50),
                        Sr = c.Int(),
                    })
                .PrimaryKey(t => t.MaterialPlanForProdOrderLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.MaterialPlanForProdOrders", t => t.MaterialPlanForProdOrderId)
                .ForeignKey("Web.MaterialPlanLines", t => t.MaterialPlanLineId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.MaterialPlanForProdOrderId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.ProcessId)
                .Index(t => t.MaterialPlanLineId);
            
            CreateTable(
                "Web.MaterialPlanForSaleOrders",
                c => new
                    {
                        MaterialPlanForSaleOrderId = c.Int(nullable: false, identity: true),
                        MaterialPlanHeaderId = c.Int(nullable: false),
                        SaleOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        MaterialPlanLineId = c.Int(),
                        OMSId = c.String(maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Sr = c.Int(),
                    })
                .PrimaryKey(t => t.MaterialPlanForSaleOrderId)
                .ForeignKey("Web.MaterialPlanHeaders", t => t.MaterialPlanHeaderId)
                .ForeignKey("Web.MaterialPlanLines", t => t.MaterialPlanLineId)
                .ForeignKey("Web.SaleOrderLines", t => t.SaleOrderLineId)
                .Index(t => t.MaterialPlanHeaderId)
                .Index(t => t.SaleOrderLineId)
                .Index(t => t.MaterialPlanLineId);
            
            CreateTable(
                "Web.MaterialPlanForSaleOrderLines",
                c => new
                    {
                        MaterialPlanForSaleOrderLineId = c.Int(nullable: false, identity: true),
                        MaterialPlanForSaleOrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Specification = c.String(maxLength: 50),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        MaterialPlanLineId = c.Int(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.MaterialPlanForSaleOrderLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.MaterialPlanForSaleOrders", t => t.MaterialPlanForSaleOrderId)
                .ForeignKey("Web.MaterialPlanLines", t => t.MaterialPlanLineId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.MaterialPlanForSaleOrderId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.MaterialPlanLineId);
            
            CreateTable(
                "Web.MaterialPlanSettings",
                c => new
                    {
                        MaterialPlanSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isMandatoryProcessLine = c.Boolean(),
                        SqlProcConsumption = c.String(),
                        SqlProcConsumptionSummary = c.String(),
                        PendingProdOrderList = c.String(),
                        filterProcesses = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        filterProductTypesConsumption = c.String(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        DocTypePurchaseIndentId = c.Int(nullable: false),
                        DocTypeProductionOrderId = c.Int(nullable: false),
                        GodownId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MaterialPlanSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeProductionOrderId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypePurchaseIndentId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.DocTypePurchaseIndentId)
                .Index(t => t.DocTypeProductionOrderId)
                .Index(t => t.GodownId);
            
            CreateTable(
                "Web.MaterialReceiveSettings",
                c => new
                    {
                        MaterialReceiveSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleMachine = c.Boolean(),
                        isMandatoryMachine = c.Boolean(),
                        isVisibleCostCenter = c.Boolean(),
                        isMandatoryCostCenter = c.Boolean(),
                        isVisibleProductUID = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisibleRate = c.Boolean(),
                        isMandatoryRate = c.Boolean(),
                        isEditableRate = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        isMandatoryProcessLine = c.Boolean(),
                        isVisibleProcessLine = c.Boolean(),
                        isPostedInStockProcess = c.Boolean(),
                        PersonFieldHeading = c.String(maxLength: 50),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        ProcessId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MaterialReceiveSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.ProcessId);
            
            CreateTable(
                "Web.MaterialRequestSettings",
                c => new
                    {
                        MaterialRequestSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleCostCenter = c.Boolean(),
                        isMandatoryCostCenter = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        isMandatoryProcessLine = c.Boolean(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MaterialRequestSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.Narrations",
                c => new
                    {
                        NarrationId = c.Int(nullable: false, identity: true),
                        NarrationName = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.NarrationId)
                .Index(t => t.NarrationName, unique: true, name: "IX_Narration_NarrationName");
            
            CreateTable(
                "Web.PackingSettings",
                c => new
                    {
                        PackingSettingId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleCostCenter = c.Boolean(),
                        isMandatoryCostCenter = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        ExtraSaleOrderNo = c.String(maxLength: 20),
                        ImportMenuId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PackingSettingId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.ImportMenuId);
            
            CreateTable(
                "Web.PerkDocumentTypes",
                c => new
                    {
                        PerkDocumentTypeId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        PerkId = c.Int(nullable: false),
                        RateDocTypeId = c.Int(),
                        RateDocId = c.Int(),
                        Base = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Worth = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CostConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        IsEditableRate = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PerkDocumentTypeId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Perks", t => t.PerkId)
                .ForeignKey("Web.DocumentTypes", t => t.RateDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.PerkId)
                .Index(t => t.RateDocTypeId);
            
            CreateTable(
                "Web.PersonBankAccounts",
                c => new
                    {
                        PersonBankAccountID = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        BankName = c.String(maxLength: 200),
                        BankBranch = c.String(maxLength: 500),
                        BankCode = c.String(maxLength: 50),
                        AccountNo = c.String(maxLength: 20),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonBankAccountID)
                .ForeignKey("Web.People", t => t.PersonId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "Web.PersonCustomGroupHeaders",
                c => new
                    {
                        PersonCustomGroupId = c.Int(nullable: false, identity: true),
                        PersonCustomGroupName = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonCustomGroupId)
                .Index(t => t.PersonCustomGroupName, unique: true, name: "IX_PersonCustomGroup_PersonCustomGroupName");
            
            CreateTable(
                "Web.PersonCustomGroupLines",
                c => new
                    {
                        PersonCustomGroupLineId = c.Int(nullable: false, identity: true),
                        PersonCustomGroupHeaderId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonCustomGroupLineId)
                .ForeignKey("Web.People", t => t.PersonId)
                .ForeignKey("Web.PersonCustomGroupHeaders", t => t.PersonCustomGroupHeaderId)
                .Index(t => t.PersonCustomGroupHeaderId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "Web.PersonDocuments",
                c => new
                    {
                        PersonDocumentID = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Description = c.String(maxLength: 500),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        ImageFolderName = c.String(maxLength: 100),
                        ImageFileName = c.String(maxLength: 100),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonDocumentID)
                .ForeignKey("Web.People", t => t.PersonId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "Web.PersonProcesses",
                c => new
                    {
                        PersonProcessId = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        PersonRateGroupId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonProcessId)
                .ForeignKey("Web.People", t => t.PersonId)
                .ForeignKey("Web.PersonRateGroups", t => t.PersonRateGroupId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .Index(t => t.PersonId)
                .Index(t => t.ProcessId)
                .Index(t => t.PersonRateGroupId);
            
            CreateTable(
                "Web.PersonRegistrations",
                c => new
                    {
                        PersonRegistrationID = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        RegistrationType = c.String(maxLength: 30),
                        RegistrationNo = c.String(maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PersonRegistrationID)
                .ForeignKey("Web.People", t => t.PersonId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "Web.ProcessSequenceLines",
                c => new
                    {
                        ProcessSequenceLineId = c.Int(nullable: false, identity: true),
                        ProcessSequenceHeaderId = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        Sequence = c.Int(nullable: false),
                        Days = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        ProductRateGroupId = c.Int(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProcessSequenceLineId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.ProcessSequenceHeaders", t => t.ProcessSequenceHeaderId)
                .ForeignKey("Web.ProductRateGroups", t => t.ProductRateGroupId)
                .Index(t => t.ProcessSequenceHeaderId)
                .Index(t => t.ProcessId)
                .Index(t => t.ProductRateGroupId);
            
            CreateTable(
                "Web.ProductRateGroups",
                c => new
                    {
                        ProductRateGroupId = c.Int(nullable: false, identity: true),
                        ProductRateGroupName = c.String(nullable: false, maxLength: 50),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        Processes = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductRateGroupId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.ProductRateGroupName, t.DivisionId, t.SiteId }, unique: true, name: "IX_ProductRateGroup_ProductRateGroupName");
            
            CreateTable(
                "Web.ProcessSettings",
                c => new
                    {
                        ProcessSettingsId = c.Int(nullable: false, identity: true),
                        ProcessId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isApplicable = c.Boolean(),
                        RateListMenuId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProcessSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Menus", t => t.RateListMenuId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.ProcessId, t.SiteId, t.DivisionId }, unique: true, name: "IX_ProcessSetting_UniqueKey")
                .Index(t => t.RateListMenuId);
            
            CreateTable(
                "Web.ProdOrderCancelHeaders",
                c => new
                    {
                        ProdOrderCancelHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        Remark = c.String(),
                        LockReason = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProdOrderCancelHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_ProdOrderCancelHeader_DocID")
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.ProdOrderCancelLines",
                c => new
                    {
                        ProdOrderCancelLineId = c.Int(nullable: false, identity: true),
                        ProdOrderCancelHeaderId = c.Int(nullable: false),
                        ProdOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocLineId = c.Int(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProdOrderCancelLineId)
                .ForeignKey("Web.ProdOrderCancelHeaders", t => t.ProdOrderCancelHeaderId)
                .ForeignKey("Web.ProdOrderLines", t => t.ProdOrderLineId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .Index(t => t.ProdOrderCancelHeaderId)
                .Index(t => t.ProdOrderLineId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.ProdOrderHeaderStatus",
                c => new
                    {
                        ProdOrderHeaderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProdOrderHeaderId)
                .ForeignKey("Web.ProdOrderHeaders", t => t.ProdOrderHeaderId)
                .Index(t => t.ProdOrderHeaderId);
            
            CreateTable(
                "Web.ProdOrderLineStatus",
                c => new
                    {
                        ProdOrderLineId = c.Int(nullable: false),
                        CancelQty = c.Decimal(precision: 18, scale: 4),
                        CancelDate = c.DateTime(),
                        AmendmentQty = c.Decimal(precision: 18, scale: 4),
                        AmendmentDate = c.DateTime(),
                        JobOrderQty = c.Decimal(precision: 18, scale: 4),
                        JobOrderDate = c.DateTime(),
                        ExcessJobOrderReviewBy = c.String(),
                    })
                .PrimaryKey(t => t.ProdOrderLineId)
                .ForeignKey("Web.ProdOrderLines", t => t.ProdOrderLineId)
                .Index(t => t.ProdOrderLineId);
            
            CreateTable(
                "Web.ProdOrderSettings",
                c => new
                    {
                        ProdOrderSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isMandatoryProcessLine = c.Boolean(),
                        filterProcesses = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        filterContraDocTypes = c.String(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProdOrderSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.ProductAlias",
                c => new
                    {
                        ProductAliasId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        ProductAliasName = c.String(nullable: false, maxLength: 50),
                        ProductId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        LockReason = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductAliasId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => new { t.DocTypeId, t.ProductAliasName }, unique: true, name: "IX_ProductAlias_ProductAliasName")
                .Index(t => t.ProductId);
            
            CreateTable(
                "Web.ProductCustomGroupHeaders",
                c => new
                    {
                        ProductCustomGroupId = c.Int(nullable: false, identity: true),
                        ProductCustomGroupName = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductCustomGroupId)
                .Index(t => t.ProductCustomGroupName, unique: true, name: "IX_ProductCustomGroup_ProductCustomGroupName");
            
            CreateTable(
                "Web.ProductCustomGroupLines",
                c => new
                    {
                        ProductCustomGroupLineId = c.Int(nullable: false, identity: true),
                        ProductCustomGroupHeaderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductCustomGroupLineId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductCustomGroupHeaders", t => t.ProductCustomGroupHeaderId)
                .Index(t => t.ProductCustomGroupHeaderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Web.ProductionOrderSettings",
                c => new
                    {
                        ProductionOrderSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isMandatoryProcessLine = c.Boolean(),
                        filterProcesses = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProductionOrderSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.ProductProcesses",
                c => new
                    {
                        ProductProcessId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        ProcessId = c.Int(),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        PurchProd = c.String(maxLength: 20),
                        Sr = c.Int(),
                        ProductRateGroupId = c.Int(),
                        Instructions = c.String(maxLength: 250),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductProcessId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductRateGroups", t => t.ProductRateGroupId)
                .Index(t => t.ProductId)
                .Index(t => t.ProcessId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.ProductRateGroupId);
            
            CreateTable(
                "Web.ProductShapes",
                c => new
                    {
                        ProductShapeId = c.Int(nullable: false, identity: true),
                        ProductShapeName = c.String(nullable: false, maxLength: 50),
                        ProductShapeShortName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemDefine = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductShapeId)
                .Index(t => t.ProductShapeName, unique: true, name: "IX_ProductShape_ProductShapeName");
            
            CreateTable(
                "Web.ProductSiteDetails",
                c => new
                    {
                        ProductSiteDetailId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        MinimumOrderQty = c.Decimal(precision: 18, scale: 4),
                        ReOrderLevel = c.Decimal(precision: 18, scale: 4),
                        GodownId = c.Int(),
                        BinLocation = c.String(maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        LotManagement = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductSiteDetailId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.ProductId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.GodownId);
            
            CreateTable(
                "Web.ProductTags",
                c => new
                    {
                        ProductTagId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductTagId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.Tags", t => t.TagId)
                .Index(t => t.ProductId)
                .Index(t => t.TagId);
            
            CreateTable(
                "Web.Tags",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        TagName = c.String(nullable: false, maxLength: 50),
                        TagType = c.String(maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.TagId)
                .Index(t => t.TagName, unique: true, name: "IX_Tag_TagName");
            
            CreateTable(
                "Web.ProductUidSiteDetails",
                c => new
                    {
                        ProductUIDId = c.Int(nullable: false, identity: true),
                        ProductUidName = c.String(),
                        ProductUidHeaderId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        LotNo = c.String(maxLength: 50),
                        GenDocId = c.Int(),
                        GenLineId = c.Int(),
                        GenDocNo = c.String(),
                        GenDocTypeId = c.Int(),
                        GenDocDate = c.DateTime(),
                        GenPersonId = c.Int(),
                        LastTransactionDocId = c.Int(),
                        LastTransactionLineId = c.Int(),
                        LastTransactionDocNo = c.String(),
                        LastTransactionDocTypeId = c.Int(),
                        LastTransactionDocDate = c.DateTime(),
                        LastTransactionPersonId = c.Int(),
                        CurrenctGodownId = c.Int(),
                        CurrenctProcessId = c.Int(),
                        Status = c.String(maxLength: 10),
                        ProcessesDone = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductUIDId)
                .ForeignKey("Web.Godowns", t => t.CurrenctGodownId)
                .ForeignKey("Web.Processes", t => t.CurrenctProcessId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.DocumentTypes", t => t.GenDocTypeId)
                .ForeignKey("Web.Buyers", t => t.GenPersonId)
                .ForeignKey("Web.DocumentTypes", t => t.LastTransactionDocTypeId)
                .ForeignKey("Web.People", t => t.LastTransactionPersonId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductUidHeaders", t => t.ProductUidHeaderId)
                .Index(t => t.ProductUidHeaderId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.GenDocTypeId)
                .Index(t => t.GenPersonId)
                .Index(t => t.LastTransactionDocTypeId)
                .Index(t => t.LastTransactionPersonId)
                .Index(t => t.CurrenctGodownId)
                .Index(t => t.CurrenctProcessId);
            
            CreateTable(
                "Web.PurchaseGoodsReceiptSettings",
                c => new
                    {
                        PurchaseGoodsReceiptSettingId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleProductUID = c.Boolean(),
                        isVisibleCostCenter = c.Boolean(),
                        isMandatoryCostCenter = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        isPostedInStockVirtual = c.Boolean(),
                        filterLedgerAccountGroups = c.String(),
                        filterLedgerAccounts = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        SqlProcGenProductUID = c.String(maxLength: 100),
                        SqlProcGatePass = c.String(maxLength: 100),
                        UnitConversionForId = c.Byte(),
                        ImportMenuId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseGoodsReceiptSettingId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.UnitConversionForId)
                .Index(t => t.ImportMenuId);
            
            CreateTable(
                "Web.PurchaseGoodsReturnHeaders",
                c => new
                    {
                        PurchaseGoodsReturnHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        ReasonId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        Remark = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        GatePassHeaderId = c.Int(),
                        StockHeaderId = c.Int(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        GodownId = c.Int(nullable: false),
                        LockReason = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseGoodsReturnHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.GatePassHeaders", t => t.GatePassHeaderId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.StockHeaders", t => t.StockHeaderId)
                .ForeignKey("Web.Suppliers", t => t.SupplierId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_PurchaseGoodsReturnHeader_DocID")
                .Index(t => t.ReasonId)
                .Index(t => t.SupplierId)
                .Index(t => t.GatePassHeaderId)
                .Index(t => t.StockHeaderId)
                .Index(t => t.GodownId);
            
            CreateTable(
                "Web.PurchaseGoodsReturnLines",
                c => new
                    {
                        PurchaseGoodsReturnLineId = c.Int(nullable: false, identity: true),
                        PurchaseGoodsReturnHeaderId = c.Int(nullable: false),
                        PurchaseGoodsReceiptLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(nullable: false, maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Weight = c.Decimal(precision: 18, scale: 4),
                        Remark = c.String(),
                        StockId = c.Int(),
                        Sr = c.Int(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        ProductUidLastTransactionDocId = c.Int(),
                        ProductUidLastTransactionDocNo = c.String(),
                        ProductUidLastTransactionDocTypeId = c.Int(),
                        ProductUidLastTransactionDocDate = c.DateTime(),
                        ProductUidLastTransactionPersonId = c.Int(),
                        ProductUidCurrentGodownId = c.Int(),
                        ProductUidCurrentProcessId = c.Int(),
                        ProductUidStatus = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.PurchaseGoodsReturnLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.Godowns", t => t.ProductUidCurrentGodownId)
                .ForeignKey("Web.Processes", t => t.ProductUidCurrentProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.ProductUidLastTransactionDocTypeId)
                .ForeignKey("Web.People", t => t.ProductUidLastTransactionPersonId)
                .ForeignKey("Web.PurchaseGoodsReceiptLines", t => t.PurchaseGoodsReceiptLineId)
                .ForeignKey("Web.PurchaseGoodsReturnHeaders", t => t.PurchaseGoodsReturnHeaderId)
                .ForeignKey("Web.Stocks", t => t.StockId)
                .Index(t => t.PurchaseGoodsReturnHeaderId)
                .Index(t => t.PurchaseGoodsReceiptLineId)
                .Index(t => t.DealUnitId)
                .Index(t => t.StockId)
                .Index(t => t.ProductUidLastTransactionDocTypeId)
                .Index(t => t.ProductUidLastTransactionPersonId)
                .Index(t => t.ProductUidCurrentGodownId)
                .Index(t => t.ProductUidCurrentProcessId);
            
            CreateTable(
                "Web.PurchaseIndentLineStatus",
                c => new
                    {
                        PurchaseIndentLineId = c.Int(nullable: false),
                        CancelQty = c.Decimal(precision: 18, scale: 4),
                        CancelDealQty = c.Decimal(precision: 18, scale: 4),
                        CancelDate = c.DateTime(),
                        AmendmentQty = c.Decimal(precision: 18, scale: 4),
                        AmendmentDealQty = c.Decimal(precision: 18, scale: 4),
                        AmendmentDate = c.DateTime(),
                        OrderQty = c.Decimal(precision: 18, scale: 4),
                        OrderDate = c.DateTime(),
                        ExcessPurchaseOrderReviewBy = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.PurchaseIndentLineId)
                .ForeignKey("Web.PurchaseIndentLines", t => t.PurchaseIndentLineId)
                .Index(t => t.PurchaseIndentLineId);
            
            CreateTable(
                "Web.PurchaseIndentSettings",
                c => new
                    {
                        PurchaseIndentSettingId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleCostCenter = c.Boolean(),
                        isMandatoryCostCenter = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseIndentSettingId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.PurchaseInvoiceAmendmentHeaders",
                c => new
                    {
                        PurchaseInvoiceAmendmentHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 10),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseInvoiceAmendmentHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Suppliers", t => t.SupplierId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "Web.PurchaseInvoiceHeaders",
                c => new
                    {
                        PurchaseInvoiceHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        LedgerHeaderId = c.Int(),
                        ExchangeRate = c.Decimal(precision: 18, scale: 4),
                        CurrencyId = c.Int(nullable: false),
                        SupplierDocNo = c.String(maxLength: 20),
                        SupplierDocDate = c.DateTime(nullable: false),
                        CreditDays = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        BillingAccountId = c.Int(nullable: false),
                        SalesTaxGroupId = c.Int(),
                        SalesTaxGroupPartyId = c.Int(),
                        TermsAndConditions = c.String(),
                        UnitConversionForId = c.Byte(),
                        PurchaseGoodsReceiptHeaderId = c.Int(),
                        DeliveryTermsId = c.Int(),
                        ShipMethodId = c.Int(),
                        CalculateDiscountOnRate = c.Boolean(nullable: false),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        LockReason = c.String(),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseInvoiceHeaderId)
                .ForeignKey("Web.Suppliers", t => t.BillingAccountId)
                .ForeignKey("Web.Currencies", t => t.CurrencyId)
                .ForeignKey("Web.DeliveryTerms", t => t.DeliveryTermsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.LedgerHeaders", t => t.LedgerHeaderId)
                .ForeignKey("Web.PurchaseGoodsReceiptHeaders", t => t.PurchaseGoodsReceiptHeaderId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.ChargeGroupPersons", t => t.SalesTaxGroupId)
                .ForeignKey("Web.SalesTaxGroupParties", t => t.SalesTaxGroupPartyId)
                .ForeignKey("Web.ShipMethods", t => t.ShipMethodId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Suppliers", t => t.SupplierId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_PurchaseInvoiceHeader_DocID")
                .Index(t => t.LedgerHeaderId)
                .Index(t => t.CurrencyId)
                .Index(t => t.SupplierId)
                .Index(t => t.BillingAccountId)
                .Index(t => t.SalesTaxGroupId)
                .Index(t => t.SalesTaxGroupPartyId)
                .Index(t => t.UnitConversionForId)
                .Index(t => t.PurchaseGoodsReceiptHeaderId)
                .Index(t => t.DeliveryTermsId)
                .Index(t => t.ShipMethodId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.PurchaseInvoiceLines",
                c => new
                    {
                        PurchaseInvoiceLineId = c.Int(nullable: false, identity: true),
                        PurchaseInvoiceHeaderId = c.Int(nullable: false),
                        PurchaseGoodsReceiptLineId = c.Int(nullable: false),
                        SalesTaxGroupId = c.Int(),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(nullable: false, maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DocQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DiscountPer = c.Decimal(precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        LockReason = c.String(),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocLineId = c.Int(),
                        Sr = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseInvoiceLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.PurchaseGoodsReceiptLines", t => t.PurchaseGoodsReceiptLineId)
                .ForeignKey("Web.PurchaseInvoiceHeaders", t => t.PurchaseInvoiceHeaderId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.SalesTaxGroups", t => t.SalesTaxGroupId)
                .Index(t => t.PurchaseInvoiceHeaderId)
                .Index(t => t.PurchaseGoodsReceiptLineId)
                .Index(t => t.SalesTaxGroupId)
                .Index(t => t.DealUnitId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.PurchaseInvoiceHeaderCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        ProductChargeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.Charges", t => t.ProductChargeId)
                .ForeignKey("Web.PurchaseInvoiceHeaders", t => t.HeaderTableId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.ProductChargeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.PurchaseInvoiceLineCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineTableId = c.Int(nullable: false),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        DealQty = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.PurchaseInvoiceLines", t => t.LineTableId)
                .Index(t => t.LineTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.PurchaseInvoiceRateAmendmentLines",
                c => new
                    {
                        PurchaseInvoiceRateAmendmentLineId = c.Int(nullable: false, identity: true),
                        PurchaseInvoiceAmendmentHeaderId = c.Int(nullable: false),
                        PurchaseInvoiceLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseInvoiceRateAmendmentLineId)
                .ForeignKey("Web.PurchaseInvoiceAmendmentHeaders", t => t.PurchaseInvoiceAmendmentHeaderId)
                .ForeignKey("Web.PurchaseInvoiceLines", t => t.PurchaseInvoiceLineId)
                .Index(t => t.PurchaseInvoiceAmendmentHeaderId)
                .Index(t => t.PurchaseInvoiceLineId);
            
            CreateTable(
                "Web.PurchaseInvoiceReturnHeaders",
                c => new
                    {
                        PurchaseInvoiceReturnHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        ReasonId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        LedgerHeaderId = c.Int(),
                        ExchangeRate = c.Decimal(precision: 18, scale: 4),
                        SupplierId = c.Int(nullable: false),
                        CalculateDiscountOnRate = c.Boolean(nullable: false),
                        SalesTaxGroupId = c.Int(),
                        SalesTaxGroupPartyId = c.Int(),
                        CurrencyId = c.Int(nullable: false),
                        PurchaseGoodsReturnHeaderId = c.Int(),
                        Remark = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseInvoiceReturnHeaderId)
                .ForeignKey("Web.Currencies", t => t.CurrencyId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.LedgerHeaders", t => t.LedgerHeaderId)
                .ForeignKey("Web.PurchaseGoodsReturnHeaders", t => t.PurchaseGoodsReturnHeaderId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.ChargeGroupPersons", t => t.SalesTaxGroupId)
                .ForeignKey("Web.SalesTaxGroupParties", t => t.SalesTaxGroupPartyId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Suppliers", t => t.SupplierId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_PurchaseInvoiceReturnHeader_DocID")
                .Index(t => t.ReasonId)
                .Index(t => t.LedgerHeaderId)
                .Index(t => t.SupplierId)
                .Index(t => t.SalesTaxGroupId)
                .Index(t => t.SalesTaxGroupPartyId)
                .Index(t => t.CurrencyId)
                .Index(t => t.PurchaseGoodsReturnHeaderId);
            
            CreateTable(
                "Web.PurchaseInvoiceReturnHeaderCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        ProductChargeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.Charges", t => t.ProductChargeId)
                .ForeignKey("Web.PurchaseInvoiceReturnHeaders", t => t.HeaderTableId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.ProductChargeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.PurchaseInvoiceReturnLines",
                c => new
                    {
                        PurchaseInvoiceReturnLineId = c.Int(nullable: false, identity: true),
                        PurchaseInvoiceReturnHeaderId = c.Int(nullable: false),
                        PurchaseInvoiceLineId = c.Int(nullable: false),
                        PurchaseGoodsReturnLineId = c.Int(),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(nullable: false, maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DiscountPer = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        Sr = c.Int(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseInvoiceReturnLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.PurchaseGoodsReturnLines", t => t.PurchaseGoodsReturnLineId)
                .ForeignKey("Web.PurchaseInvoiceLines", t => t.PurchaseInvoiceLineId)
                .ForeignKey("Web.PurchaseInvoiceReturnHeaders", t => t.PurchaseInvoiceReturnHeaderId)
                .Index(t => t.PurchaseInvoiceReturnHeaderId)
                .Index(t => t.PurchaseInvoiceLineId)
                .Index(t => t.PurchaseGoodsReturnLineId)
                .Index(t => t.DealUnitId);
            
            CreateTable(
                "Web.PurchaseInvoiceReturnLineCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineTableId = c.Int(nullable: false),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        DealQty = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.PurchaseInvoiceReturnLines", t => t.LineTableId)
                .Index(t => t.LineTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.PurchaseInvoiceSettings",
                c => new
                    {
                        PurchaseInvoiceSettingId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleCostCenter = c.Boolean(),
                        isMandatoryCostCenter = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isMandatoryRate = c.Boolean(),
                        isEditableRate = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        CalculateDiscountOnRate = c.Boolean(nullable: false),
                        filterLedgerAccountGroups = c.String(),
                        filterLedgerAccounts = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        DocTypeGoodsReturnId = c.Int(),
                        PurchaseGoodsReceiptDocTypeId = c.Int(),
                        CalculationId = c.Int(nullable: false),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        SqlProcGatePass = c.String(maxLength: 100),
                        UnitConversionForId = c.Byte(),
                        ImportMenuId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseInvoiceSettingId)
                .ForeignKey("Web.Calculations", t => t.CalculationId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeGoodsReturnId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.DocumentTypes", t => t.PurchaseGoodsReceiptDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.DocTypeGoodsReturnId)
                .Index(t => t.PurchaseGoodsReceiptDocTypeId)
                .Index(t => t.CalculationId)
                .Index(t => t.UnitConversionForId)
                .Index(t => t.ImportMenuId);
            
            CreateTable(
                "Web.PurchaseOrderAmendmentHeaders",
                c => new
                    {
                        PurchaseOrderAmendmentHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 10),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderAmendmentHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Suppliers", t => t.SupplierId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "Web.PurchaseOrderAmendmentHeaderCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        ProductChargeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.Charges", t => t.ProductChargeId)
                .ForeignKey("Web.PurchaseOrderAmendmentHeaders", t => t.HeaderTableId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.ProductChargeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.PurchaseOrderHeaderCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        ProductChargeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.Charges", t => t.ProductChargeId)
                .ForeignKey("Web.PurchaseOrderHeaders", t => t.HeaderTableId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.ProductChargeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.PurchaseOrderHeaderStatus",
                c => new
                    {
                        PurchaseOrderHeaderId = c.Int(nullable: false),
                        BOMQty = c.Decimal(precision: 18, scale: 4),
                        IsTimeIncentiveProcessed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseOrderHeaderId)
                .ForeignKey("Web.PurchaseOrderHeaders", t => t.PurchaseOrderHeaderId)
                .Index(t => t.PurchaseOrderHeaderId);
            
            CreateTable(
                "Web.PurchaseOrderInspectionHeaders",
                c => new
                    {
                        PurchaseOrderInspectionHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        InspectionById = c.Int(nullable: false),
                        Remark = c.String(),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderInspectionHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Employees", t => t.InspectionById)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Suppliers", t => t.SupplierId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_PurchaseOrderInspectionHeader_DocID")
                .Index(t => t.ProcessId)
                .Index(t => t.SupplierId)
                .Index(t => t.InspectionById)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.PurchaseOrderInspectionLines",
                c => new
                    {
                        PurchaseOrderInspectionLineId = c.Int(nullable: false, identity: true),
                        PurchaseOrderInspectionHeaderId = c.Int(nullable: false),
                        ProductUidId = c.Int(),
                        PurchaseOrderInspectionRequestLineId = c.Int(),
                        PurchaseOrderLineId = c.Int(nullable: false),
                        InspectedQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Marks = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        ImageFolderName = c.String(maxLength: 100),
                        ImageFileName = c.String(maxLength: 100),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocLineId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        Sr = c.Int(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderInspectionLineId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.PurchaseOrderInspectionHeaders", t => t.PurchaseOrderInspectionHeaderId)
                .ForeignKey("Web.PurchaseOrderInspectionRequestLines", t => t.PurchaseOrderInspectionRequestLineId)
                .ForeignKey("Web.PurchaseOrderLines", t => t.PurchaseOrderLineId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .Index(t => t.PurchaseOrderInspectionHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.PurchaseOrderInspectionRequestLineId)
                .Index(t => t.PurchaseOrderLineId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.PurchaseOrderInspectionRequestLines",
                c => new
                    {
                        PurchaseOrderInspectionRequestLineId = c.Int(nullable: false, identity: true),
                        PurchaseOrderInspectionRequestHeaderId = c.Int(nullable: false),
                        ProductUidId = c.Int(),
                        PurchaseOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        Sr = c.Int(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderInspectionRequestLineId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.PurchaseOrderInspectionRequestHeaders", t => t.PurchaseOrderInspectionRequestHeaderId)
                .ForeignKey("Web.PurchaseOrderLines", t => t.PurchaseOrderLineId)
                .Index(t => t.PurchaseOrderInspectionRequestHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.PurchaseOrderLineId);
            
            CreateTable(
                "Web.PurchaseOrderInspectionRequestHeaders",
                c => new
                    {
                        PurchaseOrderInspectionRequestHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 10),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        RequestBy = c.String(maxLength: 10),
                        AcceptedYn = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                    })
                .PrimaryKey(t => t.PurchaseOrderInspectionRequestHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Suppliers", t => t.SupplierId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.ProcessId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "Web.PurchaseOrderInspectionRequestCancelHeaders",
                c => new
                    {
                        PurchaseOrderInspectionRequestCancelHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 10),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        ReasonId = c.Int(nullable: false),
                        RequestBy = c.String(maxLength: 10),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                    })
                .PrimaryKey(t => t.PurchaseOrderInspectionRequestCancelHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Suppliers", t => t.SupplierId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.ProcessId)
                .Index(t => t.SupplierId)
                .Index(t => t.ReasonId);
            
            CreateTable(
                "Web.PurchaseOrderInspectionRequestCancelLines",
                c => new
                    {
                        PurchaseOrderInspectionRequestCancelLineId = c.Int(nullable: false, identity: true),
                        PurchaseOrderInspectionRequestCancelHeaderId = c.Int(nullable: false),
                        Sr = c.Int(),
                        ProductUidId = c.Int(),
                        PurchaseOrderInspectionRequestLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderInspectionRequestCancelLineId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.PurchaseOrderInspectionRequestCancelHeaders", t => t.PurchaseOrderInspectionRequestCancelHeaderId)
                .ForeignKey("Web.PurchaseOrderInspectionRequestLines", t => t.PurchaseOrderInspectionRequestLineId)
                .Index(t => t.PurchaseOrderInspectionRequestCancelHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.PurchaseOrderInspectionRequestLineId);
            
            CreateTable(
                "Web.PurchaseOrderInspectionRequestSettings",
                c => new
                    {
                        PurchaseOrderInspectionRequestSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        ImportMenuId = c.Int(),
                        isVisibleProductUID = c.Boolean(),
                        isMandatoryProductUID = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        ProcessId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderInspectionRequestSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ImportMenuId)
                .Index(t => t.ProcessId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.PurchaseOrderInspectionSettings",
                c => new
                    {
                        PurchaseOrderInspectionSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        ImportMenuId = c.Int(),
                        isVisibleProductUID = c.Boolean(),
                        isMandatoryProductUID = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        ProcessId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderInspectionSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ImportMenuId)
                .Index(t => t.ProcessId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.PurchaseOrderLineCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineTableId = c.Int(nullable: false),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        DealQty = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.PurchaseOrderLines", t => t.LineTableId)
                .Index(t => t.LineTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.PurchaseOrderLineStatus",
                c => new
                    {
                        PurchaseOrderLineId = c.Int(nullable: false),
                        CancelQty = c.Decimal(precision: 18, scale: 4),
                        CancelDate = c.DateTime(),
                        AmendmentQty = c.Decimal(precision: 18, scale: 4),
                        AmendmentDate = c.DateTime(),
                        ReceiveQty = c.Decimal(precision: 18, scale: 4),
                        ReceiveDate = c.DateTime(),
                        InvoiceQty = c.Decimal(precision: 18, scale: 4),
                        InvoiceDate = c.DateTime(),
                        PaymentQty = c.Decimal(precision: 18, scale: 4),
                        PaymentDate = c.DateTime(),
                        ReturnQty = c.Decimal(precision: 18, scale: 4),
                        ReturnDate = c.DateTime(),
                        RateAmendmentRate = c.Decimal(precision: 18, scale: 4),
                        RateAmendmentDate = c.DateTime(),
                        ExcessGoodsReceiptReviewBy = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.PurchaseOrderLineId)
                .ForeignKey("Web.PurchaseOrderLines", t => t.PurchaseOrderLineId)
                .Index(t => t.PurchaseOrderLineId);
            
            CreateTable(
                "Web.PurchaseOrderQtyAmendmentLines",
                c => new
                    {
                        PurchaseOrderQtyAmendmentLineId = c.Int(nullable: false, identity: true),
                        PurchaseOrderAmendmentHeaderId = c.Int(nullable: false),
                        PurchaseOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderQtyAmendmentLineId)
                .ForeignKey("Web.PurchaseOrderAmendmentHeaders", t => t.PurchaseOrderAmendmentHeaderId)
                .ForeignKey("Web.PurchaseOrderLines", t => t.PurchaseOrderLineId)
                .Index(t => t.PurchaseOrderAmendmentHeaderId)
                .Index(t => t.PurchaseOrderLineId);
            
            CreateTable(
                "Web.PurchaseOrderRateAmendmentLines",
                c => new
                    {
                        PurchaseOrderRateAmendmentLineId = c.Int(nullable: false, identity: true),
                        PurchaseOrderAmendmentHeaderId = c.Int(nullable: false),
                        PurchaseOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PurchaseOrderRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        AmendedRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        LockReason = c.String(),
                        Sr = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseOrderRateAmendmentLineId)
                .ForeignKey("Web.PurchaseOrderAmendmentHeaders", t => t.PurchaseOrderAmendmentHeaderId)
                .ForeignKey("Web.PurchaseOrderLines", t => t.PurchaseOrderLineId)
                .Index(t => t.PurchaseOrderAmendmentHeaderId)
                .Index(t => t.PurchaseOrderLineId);
            
            CreateTable(
                "Web.PurchaseOrderRateAmendmentLineCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineTableId = c.Int(nullable: false),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        DealQty = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.PurchaseOrderRateAmendmentLines", t => t.LineTableId)
                .Index(t => t.LineTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.PurchaseOrderSettings",
                c => new
                    {
                        PurchaseOrderSettingId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleCostCenter = c.Boolean(),
                        isMandatoryCostCenter = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isMandatoryRate = c.Boolean(),
                        isAllowedWithoutQuotation = c.Boolean(),
                        isEditableRate = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        CalculateDiscountOnRate = c.Boolean(nullable: false),
                        isPostedInStockVirtual = c.Boolean(),
                        filterLedgerAccountGroups = c.String(),
                        filterLedgerAccounts = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        CalculationId = c.Int(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        SqlProcGenProductUID = c.String(maxLength: 100),
                        UnitConversionForId = c.Byte(),
                        TermsAndConditions = c.String(),
                        OnSubmitMenuId = c.Int(),
                        OnApproveMenuId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseOrderSettingId)
                .ForeignKey("Web.Calculations", t => t.CalculationId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.OnApproveMenuId)
                .ForeignKey("Web.Menus", t => t.OnSubmitMenuId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.CalculationId)
                .Index(t => t.UnitConversionForId)
                .Index(t => t.OnSubmitMenuId)
                .Index(t => t.OnApproveMenuId);
            
            CreateTable(
                "Web.PurchaseQuotationHeaders",
                c => new
                    {
                        PurchaseQuotationHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        BillingAccountId = c.Int(nullable: false),
                        CurrencyId = c.Int(),
                        VendorQuotationNo = c.String(nullable: false, maxLength: 20),
                        VendorQuotationDate = c.DateTime(nullable: false),
                        TermsAndConditions = c.String(),
                        ExchangeRate = c.Decimal(precision: 18, scale: 4),
                        CreditDays = c.Int(nullable: false),
                        SalesTaxGroupId = c.Int(),
                        UnitConversionForId = c.Byte(),
                        DeliveryTermsId = c.Int(),
                        ShipMethodId = c.Int(),
                        CalculateDiscountOnRate = c.Boolean(nullable: false),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseQuotationHeaderId)
                .ForeignKey("Web.Suppliers", t => t.BillingAccountId)
                .ForeignKey("Web.Currencies", t => t.CurrencyId)
                .ForeignKey("Web.DeliveryTerms", t => t.DeliveryTermsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.ChargeGroupPersons", t => t.SalesTaxGroupId)
                .ForeignKey("Web.ShipMethods", t => t.ShipMethodId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Suppliers", t => t.SupplierId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_PurchaseQuotationHeader_DocID")
                .Index(t => t.SupplierId)
                .Index(t => t.BillingAccountId)
                .Index(t => t.CurrencyId)
                .Index(t => t.SalesTaxGroupId)
                .Index(t => t.UnitConversionForId)
                .Index(t => t.DeliveryTermsId)
                .Index(t => t.ShipMethodId);
            
            CreateTable(
                "Web.PurchaseQuotationLines",
                c => new
                    {
                        PurchaseQuotationLineId = c.Int(nullable: false, identity: true),
                        PurchaseQuotationHeaderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        PurchaseIndentLineId = c.Int(),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Specification = c.String(maxLength: 50),
                        LotNo = c.String(maxLength: 20),
                        SalesTaxGroupId = c.Int(),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(nullable: false, maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DiscountPer = c.Decimal(precision: 18, scale: 4),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        LockReason = c.String(),
                        Sr = c.Int(),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PurchaseQuotationLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.PurchaseIndentLines", t => t.PurchaseIndentLineId)
                .ForeignKey("Web.PurchaseQuotationHeaders", t => t.PurchaseQuotationHeaderId)
                .ForeignKey("Web.SalesTaxGroups", t => t.SalesTaxGroupId)
                .Index(t => t.PurchaseQuotationHeaderId)
                .Index(t => t.ProductId)
                .Index(t => t.PurchaseIndentLineId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.SalesTaxGroupId)
                .Index(t => t.DealUnitId);
            
            CreateTable(
                "Web.PurchaseQuotationHeaderCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        ProductChargeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.Charges", t => t.ProductChargeId)
                .ForeignKey("Web.PurchaseQuotationHeaders", t => t.HeaderTableId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.ProductChargeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.PurchaseQuotationLineCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineTableId = c.Int(nullable: false),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        DealQty = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.PurchaseQuotationLines", t => t.LineTableId)
                .Index(t => t.LineTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.PurchaseQuotationSettings",
                c => new
                    {
                        PurchaseQuotationSettingId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleCostCenter = c.Boolean(),
                        isMandatoryCostCenter = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isMandatoryRate = c.Boolean(),
                        isAllowedWithoutQuotation = c.Boolean(),
                        isEditableRate = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        CalculateDiscountOnRate = c.Boolean(nullable: false),
                        isPostedInStockVirtual = c.Boolean(),
                        filterLedgerAccountGroups = c.String(),
                        filterLedgerAccounts = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        isVisibleForIndent = c.Boolean(),
                        isVisibleSalesTaxGroup = c.Boolean(),
                        isVisibleCurrency = c.Boolean(),
                        isVisibleDeliveryTerms = c.Boolean(),
                        isVisibleShipMethod = c.Boolean(),
                        CalculationId = c.Int(nullable: false),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        SqlProcGenProductUID = c.String(maxLength: 100),
                        UnitConversionForId = c.Byte(),
                        TermsAndConditions = c.String(),
                        OnSubmitMenuId = c.Int(),
                        OnApproveMenuId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseQuotationSettingId)
                .ForeignKey("Web.Calculations", t => t.CalculationId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.OnApproveMenuId)
                .ForeignKey("Web.Menus", t => t.OnSubmitMenuId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.CalculationId)
                .Index(t => t.UnitConversionForId)
                .Index(t => t.OnSubmitMenuId)
                .Index(t => t.OnApproveMenuId);
            
            CreateTable(
                "Web.RateConversionSettings",
                c => new
                    {
                        RateConversionSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleMachine = c.Boolean(),
                        isMandatoryMachine = c.Boolean(),
                        isVisibleCostCenter = c.Boolean(),
                        isMandatoryCostCenter = c.Boolean(),
                        isVisibleProductUID = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        isMandatoryProcessLine = c.Boolean(),
                        isPostedInStockProcess = c.Boolean(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        ProcessId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RateConversionSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.ProcessId);
            
            CreateTable(
                "Web.RateLists",
                c => new
                    {
                        RateListId = c.Int(nullable: false, identity: true),
                        WEF = c.DateTime(nullable: false),
                        ProcessId = c.Int(),
                        PersonRateGroupId = c.Int(),
                        DocTypeId = c.Int(),
                        DocId = c.Int(),
                        ProductId = c.Int(),
                        WeightageGreaterOrEqual = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(maxLength: 3),
                        Loss = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnCountedQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RateListId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.PersonRateGroups", t => t.PersonRateGroupId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.ProcessId)
                .Index(t => t.PersonRateGroupId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ProductId)
                .Index(t => t.DealUnitId);
            
            CreateTable(
                "Web.RateListHeaders",
                c => new
                    {
                        RateListHeaderId = c.Int(nullable: false, identity: true),
                        EffectiveDate = c.DateTime(nullable: false),
                        ProcessId = c.Int(nullable: false),
                        RateListName = c.String(nullable: false, maxLength: 50),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        CalculateWeightageOn = c.String(nullable: false, maxLength: 20),
                        WeightageGreaterOrEqual = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(maxLength: 3),
                        CloseDate = c.DateTime(),
                        Description = c.String(),
                        MinRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        MaxRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RateListHeaderId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.EffectiveDate, t.ProcessId, t.DivisionId, t.SiteId, t.WeightageGreaterOrEqual }, unique: true, name: "IX_RateListHeader_DocID")
                .Index(t => new { t.RateListName, t.DivisionId, t.SiteId }, unique: true, name: "IX_RateListHeader_Name")
                .Index(t => t.DealUnitId);
            
            CreateTable(
                "Web.RateListHistories",
                c => new
                    {
                        RateListId = c.Int(nullable: false, identity: true),
                        WEF = c.DateTime(nullable: false),
                        ProcessId = c.Int(),
                        PersonRateGroupId = c.Int(),
                        DocTypeId = c.Int(),
                        DocId = c.Int(),
                        ProductId = c.Int(),
                        WeightageGreaterOrEqual = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(maxLength: 3),
                        Loss = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnCountedQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RateListId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.PersonRateGroups", t => t.PersonRateGroupId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.ProcessId)
                .Index(t => t.PersonRateGroupId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ProductId)
                .Index(t => t.DealUnitId);
            
            CreateTable(
                "Web.RateListLines",
                c => new
                    {
                        RateListLineId = c.Int(nullable: false, identity: true),
                        RateListHeaderId = c.Int(nullable: false),
                        PersonRateGroupId = c.Int(),
                        ProductRateGroupId = c.Int(),
                        ProductId = c.Int(),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Incentive = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Loss = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnCountedQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RateListLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.PersonRateGroups", t => t.PersonRateGroupId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductRateGroups", t => t.ProductRateGroupId)
                .ForeignKey("Web.RateListHeaders", t => t.RateListHeaderId)
                .Index(t => t.RateListHeaderId)
                .Index(t => t.PersonRateGroupId)
                .Index(t => t.ProductRateGroupId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.RateListLineHistories",
                c => new
                    {
                        RateListLineId = c.Int(nullable: false, identity: true),
                        RateListHeaderId = c.Int(nullable: false),
                        PersonRateGroupId = c.Int(),
                        ProductRateGroupId = c.Int(),
                        ProductId = c.Int(),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Incentive = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Loss = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnCountedQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RateListLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.PersonRateGroups", t => t.PersonRateGroupId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductRateGroups", t => t.ProductRateGroupId)
                .ForeignKey("Web.RateListHeaders", t => t.RateListHeaderId)
                .Index(t => t.RateListHeaderId)
                .Index(t => t.PersonRateGroupId)
                .Index(t => t.ProductRateGroupId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.RateListPersonRateGroups",
                c => new
                    {
                        RateListPersonRateGroupId = c.Int(nullable: false, identity: true),
                        RateListHeaderId = c.Int(nullable: false),
                        PersonRateGroupId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RateListPersonRateGroupId)
                .ForeignKey("Web.PersonRateGroups", t => t.PersonRateGroupId)
                .ForeignKey("Web.RateListHeaders", t => t.RateListHeaderId)
                .Index(t => t.RateListHeaderId)
                .Index(t => t.PersonRateGroupId);
            
            CreateTable(
                "Web.RateListProductRateGroups",
                c => new
                    {
                        RateListProductRateGroupId = c.Int(nullable: false, identity: true),
                        RateListHeaderId = c.Int(nullable: false),
                        ProductRateGroupId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RateListProductRateGroupId)
                .ForeignKey("Web.ProductRateGroups", t => t.ProductRateGroupId)
                .ForeignKey("Web.RateListHeaders", t => t.RateListHeaderId)
                .Index(t => t.RateListHeaderId)
                .Index(t => t.ProductRateGroupId);
            
            CreateTable(
                "Web.ReportColumns",
                c => new
                    {
                        ReportColumnId = c.Int(nullable: false, identity: true),
                        ReportHeaderId = c.Int(nullable: false),
                        SubReportId = c.Int(nullable: false),
                        SubReportHeaderId = c.Int(),
                        DisplayName = c.String(nullable: false),
                        FieldName = c.String(nullable: false),
                        IsVisible = c.Boolean(nullable: false),
                        width = c.String(),
                        minWidth = c.String(),
                        AggregateFunc = c.String(),
                        TestAlignment = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReportColumnId)
                .ForeignKey("Web.ReportHeaders", t => t.ReportHeaderId)
                .ForeignKey("Web.SubReports", t => t.SubReportId)
                .ForeignKey("Web.ReportHeaders", t => t.SubReportHeaderId)
                .Index(t => t.ReportHeaderId)
                .Index(t => t.SubReportId)
                .Index(t => t.SubReportHeaderId);
            
            CreateTable(
                "Web.ReportHeaders",
                c => new
                    {
                        ReportHeaderId = c.Int(nullable: false, identity: true),
                        ReportName = c.String(),
                        Controller = c.String(),
                        Action = c.String(),
                        SqlProc = c.String(),
                        Notes = c.String(),
                        ParentReportHeaderId = c.Int(),
                        ReportSQL = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReportHeaderId);
            
            CreateTable(
                "Web.ReportLines",
                c => new
                    {
                        ReportLineId = c.Int(nullable: false, identity: true),
                        ReportHeaderId = c.Int(nullable: false),
                        DisplayName = c.String(nullable: false),
                        FieldName = c.String(nullable: false),
                        DataType = c.String(nullable: false),
                        Type = c.String(nullable: false),
                        ListItem = c.String(),
                        DefaultValue = c.String(),
                        IsVisible = c.Boolean(nullable: false),
                        ServiceFuncGet = c.String(),
                        ServiceFuncSet = c.String(),
                        SqlProcGetSet = c.String(maxLength: 100),
                        SqlProcGet = c.String(),
                        SqlProcSet = c.String(),
                        CacheKey = c.String(),
                        Serial = c.Int(nullable: false),
                        NoOfCharToEnter = c.Int(),
                        SqlParameter = c.String(),
                        IsCollapse = c.Boolean(nullable: false),
                        IsMandatory = c.Boolean(nullable: false),
                        PlaceHolder = c.String(),
                        ToolTip = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReportLineId)
                .ForeignKey("Web.ReportHeaders", t => t.ReportHeaderId)
                .Index(t => t.ReportHeaderId);
            
            CreateTable(
                "Web.SubReports",
                c => new
                    {
                        SubReportId = c.Int(nullable: false, identity: true),
                        SubReportName = c.String(),
                        SqlProc = c.String(),
                        ReportHeaderId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SubReportId)
                .ForeignKey("Web.ReportHeaders", t => t.ReportHeaderId)
                .Index(t => t.ReportHeaderId);
            
            CreateTable(
                "Web.ReportUIDValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UID = c.Guid(nullable: false),
                        Type = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Web.RequisitionCancelHeaders",
                c => new
                    {
                        RequisitionCancelHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        ReasonId = c.Int(nullable: false),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RequisitionCancelHeaderId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.People", t => t.PersonId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_RequisitionCancelHeader_DocID")
                .Index(t => t.PersonId)
                .Index(t => t.ReasonId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.RequisitionCancelLines",
                c => new
                    {
                        RequisitionCancelLineId = c.Int(nullable: false, identity: true),
                        RequisitionCancelHeaderId = c.Int(nullable: false),
                        RequisitionLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RequisitionCancelLineId)
                .ForeignKey("Web.RequisitionCancelHeaders", t => t.RequisitionCancelHeaderId)
                .ForeignKey("Web.RequisitionLines", t => t.RequisitionLineId)
                .Index(t => t.RequisitionCancelHeaderId)
                .Index(t => t.RequisitionLineId);
            
            CreateTable(
                "Web.RequisitionLineStatus",
                c => new
                    {
                        RequisitionLineId = c.Int(nullable: false),
                        CancelQty = c.Decimal(precision: 18, scale: 4),
                        CancelDate = c.DateTime(),
                        AmendmentQty = c.Decimal(precision: 18, scale: 4),
                        AmendmentDate = c.DateTime(),
                        IssueQty = c.Decimal(precision: 18, scale: 4),
                        ReceiveQty = c.Decimal(precision: 18, scale: 4),
                        IssueDate = c.DateTime(),
                        ReceiveDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RequisitionLineId)
                .ForeignKey("Web.RequisitionLines", t => t.RequisitionLineId)
                .Index(t => t.RequisitionLineId);
            
            CreateTable(
                "Web.RequisitionSettings",
                c => new
                    {
                        RequisitionSettingId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleCostCenter = c.Boolean(),
                        isMandatoryCostCenter = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        filterLedgerAccountGroups = c.String(),
                        filterLedgerAccounts = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        OnSubmitMenuId = c.Int(),
                        OnApproveMenuId = c.Int(),
                        DefaultReasonId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RequisitionSettingId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.OnApproveMenuId)
                .ForeignKey("Web.Menus", t => t.OnSubmitMenuId)
                .ForeignKey("Web.Reasons", t => t.DefaultReasonId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.OnSubmitMenuId)
                .Index(t => t.OnApproveMenuId)
                .Index(t => t.DefaultReasonId);
            
            CreateTable(
                "Web.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "Web.RolesControllerActions",
                c => new
                    {
                        RolesControllerActionId = c.Int(nullable: false, identity: true),
                        RoleId = c.String(maxLength: 128),
                        ControllerActionId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RolesControllerActionId)
                .ForeignKey("Web.ControllerActions", t => t.ControllerActionId)
                .ForeignKey("Web.AspNetRoles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.ControllerActionId);
            
            CreateTable(
                "Web.RolesDivisions",
                c => new
                    {
                        RolesDivisionId = c.Int(nullable: false, identity: true),
                        RoleId = c.String(maxLength: 128),
                        DivisionId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RolesDivisionId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.AspNetRoles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.RolesMenus",
                c => new
                    {
                        RolesMenuId = c.Int(nullable: false, identity: true),
                        RoleId = c.String(maxLength: 128),
                        MenuId = c.Int(nullable: false),
                        FullHeaderPermission = c.Boolean(nullable: false),
                        FullLinePermission = c.Boolean(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RolesMenuId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Menus", t => t.MenuId)
                .ForeignKey("Web.AspNetRoles", t => t.RoleId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.RoleId)
                .Index(t => t.MenuId)
                .Index(t => new { t.DivisionId, t.SiteId }, unique: true, name: "IX_PurchaseOrderHeader_DocID");
            
            CreateTable(
                "Web.RolesSites",
                c => new
                    {
                        RolesSiteId = c.Int(nullable: false, identity: true),
                        RoleId = c.String(maxLength: 128),
                        SiteId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RolesSiteId)
                .ForeignKey("Web.AspNetRoles", t => t.RoleId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.RoleId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "Web.RouteLines",
                c => new
                    {
                        RouteLineId = c.Int(nullable: false, identity: true),
                        RouteId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RouteLineId)
                .ForeignKey("Web.Cities", t => t.CityId)
                .ForeignKey("Web.Routes", t => t.RouteId)
                .Index(t => t.RouteId)
                .Index(t => t.CityId);
            
            CreateTable(
                "Web.Rug_RetentionPercentage",
                c => new
                    {
                        Rug_RetentionPercentageId = c.Int(nullable: false, identity: true),
                        ProductCategoryId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        RetentionPer = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Rug_RetentionPercentageId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Stocks", t => t.ProductCategoryId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.ProductCategoryId)
                .Index(t => new { t.DivisionId, t.SiteId }, unique: true, name: "IX_Stock_DocID");
            
            CreateTable(
                "Web.RugStencils",
                c => new
                    {
                        StencilId = c.Int(nullable: false),
                        ProductDesignId = c.Int(nullable: false),
                        ProductSizeId = c.Int(nullable: false),
                        FullHalf = c.String(maxLength: 10),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.StencilId)
                .ForeignKey("Web.Products", t => t.StencilId)
                .ForeignKey("Web.ProductDesigns", t => t.ProductDesignId)
                .ForeignKey("Web.ProductSizes", t => t.ProductSizeId)
                .Index(t => t.StencilId)
                .Index(t => t.ProductDesignId)
                .Index(t => t.ProductSizeId);
            
            CreateTable(
                "Web.SaleDeliveryOrderCancelHeaders",
                c => new
                    {
                        SaleDeliveryOrderCancelHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 10),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ReasonId = c.Int(nullable: false),
                        BuyerId = c.Int(nullable: false),
                        OrderById = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                    })
                .PrimaryKey(t => t.SaleDeliveryOrderCancelHeaderId)
                .ForeignKey("Web.Buyers", t => t.BuyerId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Employees", t => t.OrderById)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_DeliveryOrderHeader_Unique")
                .Index(t => t.ReasonId)
                .Index(t => t.BuyerId)
                .Index(t => t.OrderById);
            
            CreateTable(
                "Web.SaleDeliveryOrderCancelLines",
                c => new
                    {
                        SaleDeliveryOrderCancelLineId = c.Int(nullable: false, identity: true),
                        SaleDeliveryOrderCancelHeaderId = c.Int(nullable: false),
                        Sr = c.Int(),
                        ProductUidId = c.Int(),
                        SaleDeliveryOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        LockReason = c.String(),
                        OMSId = c.String(maxLength: 50),
                        ProductUidLastTransactionDocId = c.Int(),
                        ProductUidLastTransactionDocNo = c.String(),
                        ProductUidLastTransactionDocTypeId = c.Int(),
                        ProductUidLastTransactionDocDate = c.DateTime(),
                        ProductUidLastTransactionPersonId = c.Int(),
                        ProductUidCurrentGodownId = c.Int(),
                        ProductUidCurrentProcessId = c.Int(),
                        ProductUidStatus = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.SaleDeliveryOrderCancelLineId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.SaleDeliveryOrderCancelHeaders", t => t.SaleDeliveryOrderCancelHeaderId)
                .ForeignKey("Web.SaleDeliveryOrderLines", t => t.SaleDeliveryOrderLineId)
                .Index(t => t.SaleDeliveryOrderCancelHeaderId)
                .Index(t => t.ProductUidId)
                .Index(t => t.SaleDeliveryOrderLineId);
            
            CreateTable(
                "Web.SaleDeliveryOrderSettings",
                c => new
                    {
                        SaleDeliveryOrderSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        ImportMenuId = c.Int(),
                        isVisibleProductUID = c.Boolean(),
                        isMandatoryProductUID = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleDeliveryOrderSettingsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ImportMenuId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "Web.SaleDispatchReturnHeaders",
                c => new
                    {
                        SaleDispatchReturnHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        ReasonId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        BuyerId = c.Int(nullable: false),
                        Remark = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        StockHeaderId = c.Int(),
                        LockReason = c.String(),
                        LockReasonDelete = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        GodownId = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleDispatchReturnHeaderId)
                .ForeignKey("Web.Buyers", t => t.BuyerId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.StockHeaders", t => t.StockHeaderId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_SaleDispatchReturnHeader_DocID")
                .Index(t => t.ReasonId)
                .Index(t => t.BuyerId)
                .Index(t => t.StockHeaderId)
                .Index(t => t.GodownId);
            
            CreateTable(
                "Web.SaleDispatchReturnLines",
                c => new
                    {
                        SaleDispatchReturnLineId = c.Int(nullable: false, identity: true),
                        SaleDispatchReturnHeaderId = c.Int(nullable: false),
                        SaleDispatchLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(nullable: false, maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Weight = c.Decimal(precision: 18, scale: 4),
                        Remark = c.String(),
                        StockId = c.Int(),
                        Sr = c.Int(),
                        LockReason = c.String(),
                        LockReasonDelete = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleDispatchReturnLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.SaleDispatchLines", t => t.SaleDispatchLineId)
                .ForeignKey("Web.SaleDispatchReturnHeaders", t => t.SaleDispatchReturnHeaderId)
                .ForeignKey("Web.Stocks", t => t.StockId)
                .Index(t => t.SaleDispatchReturnHeaderId)
                .Index(t => t.SaleDispatchLineId)
                .Index(t => t.DealUnitId)
                .Index(t => t.StockId);
            
            CreateTable(
                "Web.SaleDispatchSettings",
                c => new
                    {
                        SaleDispatchSettingId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        filterLedgerAccountGroups = c.String(),
                        filterLedgerAccounts = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        DocTypePackingHeaderId = c.Int(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        SqlProcGatePass = c.String(maxLength: 100),
                        UnitConversionForId = c.Byte(),
                        DocTypeDispatchReturnId = c.Int(),
                        ImportMenuId = c.Int(),
                        isVisibleDeliveryTerms = c.Boolean(),
                        isVisibleDealUnit = c.Boolean(),
                        isVisibleShipMethod = c.Boolean(),
                        isVisibleSpecification = c.Boolean(),
                        isVisibleProductUid = c.Boolean(),
                        isVisibleProductCode = c.Boolean(),
                        isVisibleBaleNo = c.Boolean(),
                        isVisibleForSaleOrder = c.Boolean(),
                        isVisibleWeight = c.Boolean(),
                        ProcessId = c.Int(),
                        DeliveryTermsId = c.Int(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        GodownId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SaleDispatchSettingId)
                .ForeignKey("Web.DeliveryTerms", t => t.DeliveryTermsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeDispatchReturnId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypePackingHeaderId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.ShipMethods", t => t.ShipMethodId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.DocTypePackingHeaderId)
                .Index(t => t.UnitConversionForId)
                .Index(t => t.DocTypeDispatchReturnId)
                .Index(t => t.ImportMenuId)
                .Index(t => t.ProcessId)
                .Index(t => t.DeliveryTermsId)
                .Index(t => t.ShipMethodId)
                .Index(t => t.GodownId);
            
            CreateTable(
                "Web.SaleInvoiceHeaderCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        ProductChargeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.Charges", t => t.ProductChargeId)
                .ForeignKey("Web.SaleInvoiceHeaders", t => t.HeaderTableId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.ProductChargeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.SaleInvoiceLineCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineTableId = c.Int(nullable: false),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        DealQty = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.SaleInvoiceLines", t => t.LineTableId)
                .Index(t => t.LineTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.SaleInvoiceReturnHeaders",
                c => new
                    {
                        SaleInvoiceReturnHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        ReasonId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        LedgerHeaderId = c.Int(),
                        ExchangeRate = c.Decimal(precision: 18, scale: 4),
                        BuyerId = c.Int(nullable: false),
                        CalculateDiscountOnRate = c.Boolean(nullable: false),
                        SalesTaxGroupId = c.Int(),
                        SalesTaxGroupPartyId = c.Int(),
                        CurrencyId = c.Int(nullable: false),
                        SaleDispatchReturnHeaderId = c.Int(),
                        Remark = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        LockReason = c.String(),
                        LockReasonDelete = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleInvoiceReturnHeaderId)
                .ForeignKey("Web.Buyers", t => t.BuyerId)
                .ForeignKey("Web.Currencies", t => t.CurrencyId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.LedgerHeaders", t => t.LedgerHeaderId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.SaleDispatchReturnHeaders", t => t.SaleDispatchReturnHeaderId)
                .ForeignKey("Web.ChargeGroupPersons", t => t.SalesTaxGroupId)
                .ForeignKey("Web.SalesTaxGroupParties", t => t.SalesTaxGroupPartyId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DocTypeId, t.DocNo, t.DivisionId, t.SiteId }, unique: true, name: "IX_SaleInvoiceReturnHeader_DocID")
                .Index(t => t.ReasonId)
                .Index(t => t.LedgerHeaderId)
                .Index(t => t.BuyerId)
                .Index(t => t.SalesTaxGroupId)
                .Index(t => t.SalesTaxGroupPartyId)
                .Index(t => t.CurrencyId)
                .Index(t => t.SaleDispatchReturnHeaderId);
            
            CreateTable(
                "Web.SaleInvoiceReturnHeaderCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        ProductChargeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.Charges", t => t.ProductChargeId)
                .ForeignKey("Web.SaleInvoiceReturnHeaders", t => t.HeaderTableId)
                .Index(t => t.HeaderTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.ProductChargeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.SaleInvoiceReturnLines",
                c => new
                    {
                        SaleInvoiceReturnLineId = c.Int(nullable: false, identity: true),
                        SaleInvoiceReturnHeaderId = c.Int(nullable: false),
                        SaleInvoiceLineId = c.Int(nullable: false),
                        SaleDispatchReturnLineId = c.Int(),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitConversionMultiplier = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(nullable: false, maxLength: 3),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Weight = c.Decimal(precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DiscountPer = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        Sr = c.Int(),
                        LockReason = c.String(),
                        LockReasonDelete = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleInvoiceReturnLineId)
                .ForeignKey("Web.Units", t => t.DealUnitId)
                .ForeignKey("Web.SaleDispatchReturnLines", t => t.SaleDispatchReturnLineId)
                .ForeignKey("Web.SaleInvoiceLines", t => t.SaleInvoiceLineId)
                .ForeignKey("Web.SaleInvoiceReturnHeaders", t => t.SaleInvoiceReturnHeaderId)
                .Index(t => t.SaleInvoiceReturnHeaderId)
                .Index(t => t.SaleInvoiceLineId)
                .Index(t => t.SaleDispatchReturnLineId)
                .Index(t => t.DealUnitId);
            
            CreateTable(
                "Web.SaleInvoiceReturnLineCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineTableId = c.Int(nullable: false),
                        HeaderTableId = c.Int(nullable: false),
                        Sr = c.Int(nullable: false),
                        ChargeId = c.Int(nullable: false),
                        AddDeduct = c.Boolean(),
                        AffectCost = c.Boolean(nullable: false),
                        ChargeTypeId = c.Int(),
                        CalculateOnId = c.Int(),
                        PersonID = c.Int(),
                        LedgerAccountDrId = c.Int(),
                        LedgerAccountCrId = c.Int(),
                        ContraLedgerAccountId = c.Int(),
                        CostCenterId = c.Int(),
                        RateType = c.Byte(nullable: false),
                        IncludedInBase = c.Boolean(nullable: false),
                        ParentChargeId = c.Int(),
                        Rate = c.Decimal(precision: 18, scale: 4),
                        Amount = c.Decimal(precision: 18, scale: 4),
                        DealQty = c.Decimal(precision: 18, scale: 4),
                        IsVisible = c.Boolean(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Web.Charges", t => t.CalculateOnId)
                .ForeignKey("Web.Charges", t => t.ChargeId)
                .ForeignKey("Web.ChargeTypes", t => t.ChargeTypeId)
                .ForeignKey("Web.LedgerAccounts", t => t.ContraLedgerAccountId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountCrId)
                .ForeignKey("Web.LedgerAccounts", t => t.LedgerAccountDrId)
                .ForeignKey("Web.Charges", t => t.ParentChargeId)
                .ForeignKey("Web.People", t => t.PersonID)
                .ForeignKey("Web.SaleInvoiceReturnLines", t => t.LineTableId)
                .Index(t => t.LineTableId)
                .Index(t => t.ChargeId)
                .Index(t => t.ChargeTypeId)
                .Index(t => t.CalculateOnId)
                .Index(t => t.PersonID)
                .Index(t => t.LedgerAccountDrId)
                .Index(t => t.LedgerAccountCrId)
                .Index(t => t.ContraLedgerAccountId)
                .Index(t => t.CostCenterId)
                .Index(t => t.ParentChargeId);
            
            CreateTable(
                "Web.SaleInvoiceSettings",
                c => new
                    {
                        SaleInvoiceSettingId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisiblePromoCode = c.Boolean(),
                        isVisibleLotNo = c.Boolean(),
                        CalculateDiscountOnRate = c.Boolean(nullable: false),
                        filterLedgerAccountGroups = c.String(),
                        filterLedgerAccounts = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        filterContraDocTypes = c.String(),
                        filterContraSites = c.String(),
                        filterContraDivisions = c.String(),
                        DocTypePackingHeaderId = c.Int(),
                        SaleDispatchDocTypeId = c.Int(),
                        CalculationId = c.Int(nullable: false),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        SqlProcGatePass = c.String(maxLength: 100),
                        UnitConversionForId = c.Byte(),
                        DocTypeDispatchReturnId = c.Int(),
                        ImportMenuId = c.Int(),
                        isVisibleAgent = c.Boolean(),
                        isVisibleCurrency = c.Boolean(),
                        isVisibleDeliveryTerms = c.Boolean(),
                        isVisibleDealUnit = c.Boolean(),
                        isVisibleShipMethod = c.Boolean(),
                        isVisibleSpecification = c.Boolean(),
                        isVisibleSalesTaxGroup = c.Boolean(),
                        isVisibleProductUid = c.Boolean(),
                        isVisibleProductCode = c.Boolean(),
                        isVisibleBaleNo = c.Boolean(),
                        isVisibleDiscountPer = c.Boolean(),
                        isVisibleForSaleOrder = c.Boolean(),
                        isVisibleWeight = c.Boolean(),
                        CurrencyId = c.Int(nullable: false),
                        ProcessId = c.Int(),
                        DeliveryTermsId = c.Int(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        SalesTaxGroupId = c.Int(nullable: false),
                        GodownId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SaleInvoiceSettingId)
                .ForeignKey("Web.Calculations", t => t.CalculationId)
                .ForeignKey("Web.Currencies", t => t.CurrencyId)
                .ForeignKey("Web.DeliveryTerms", t => t.DeliveryTermsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeDispatchReturnId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypePackingHeaderId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.DocumentTypes", t => t.SaleDispatchDocTypeId)
                .ForeignKey("Web.SalesTaxGroups", t => t.SalesTaxGroupId)
                .ForeignKey("Web.ShipMethods", t => t.ShipMethodId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => t.DocTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.DocTypePackingHeaderId)
                .Index(t => t.SaleDispatchDocTypeId)
                .Index(t => t.CalculationId)
                .Index(t => t.UnitConversionForId)
                .Index(t => t.DocTypeDispatchReturnId)
                .Index(t => t.ImportMenuId)
                .Index(t => t.CurrencyId)
                .Index(t => t.ProcessId)
                .Index(t => t.DeliveryTermsId)
                .Index(t => t.ShipMethodId)
                .Index(t => t.SalesTaxGroupId)
                .Index(t => t.GodownId);
            
            CreateTable(
                "Web.SaleOrderAmendmentHeaders",
                c => new
                    {
                        SaleOrderAmendmentHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 10),
                        DivisionId = c.Int(),
                        SiteId = c.Int(nullable: false),
                        ReasonId = c.Int(nullable: false),
                        BuyerId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        LockReason = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleOrderAmendmentHeaderId)
                .ForeignKey("Web.Buyers", t => t.BuyerId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.ReasonId)
                .Index(t => t.BuyerId);
            
            CreateTable(
                "Web.SaleOrderCancelHeaders",
                c => new
                    {
                        SaleOrderCancelHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        ReasonId = c.Int(nullable: false),
                        DocNo = c.String(maxLength: 20),
                        DivisionId = c.Int(),
                        BuyerId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Remark = c.String(nullable: false),
                        LockReason = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleOrderCancelHeaderId)
                .ForeignKey("Web.Buyers", t => t.BuyerId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Reasons", t => t.ReasonId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ReasonId)
                .Index(t => t.DivisionId)
                .Index(t => t.BuyerId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "Web.SaleOrderCancelLines",
                c => new
                    {
                        SaleOrderCancelLineId = c.Int(nullable: false, identity: true),
                        SaleOrderLineId = c.Int(nullable: false),
                        SaleOrderCancelHeaderId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleOrderCancelLineId)
                .ForeignKey("Web.SaleOrderLines", t => t.SaleOrderLineId)
                .ForeignKey("Web.SaleOrderCancelHeaders", t => t.SaleOrderCancelHeaderId)
                .Index(t => t.SaleOrderLineId)
                .Index(t => t.SaleOrderCancelHeaderId);
            
            CreateTable(
                "Web.SaleOrderLineStatus",
                c => new
                    {
                        SaleOrderLineId = c.Int(nullable: false),
                        CancelQty = c.Decimal(precision: 18, scale: 4),
                        CancelDate = c.DateTime(),
                        AmendmentQty = c.Decimal(precision: 18, scale: 4),
                        AmendmentDate = c.DateTime(),
                        ShipQty = c.Decimal(precision: 18, scale: 4),
                        ShipDate = c.DateTime(),
                        ReturnQty = c.Decimal(precision: 18, scale: 4),
                        ReturnDate = c.DateTime(),
                        InvoiceQty = c.Decimal(precision: 18, scale: 4),
                        InvoiceDate = c.DateTime(),
                        PaymentQty = c.Decimal(precision: 18, scale: 4),
                        PaymentDate = c.DateTime(),
                        ExcessSaleDispatchReviewBy = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.SaleOrderLineId)
                .ForeignKey("Web.SaleOrderLines", t => t.SaleOrderLineId)
                .Index(t => t.SaleOrderLineId);
            
            CreateTable(
                "Web.SaleOrderQtyAmendmentLines",
                c => new
                    {
                        SaleOrderQtyAmendmentLineId = c.Int(nullable: false, identity: true),
                        SaleOrderAmendmentHeaderId = c.Int(nullable: false),
                        SaleOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleOrderQtyAmendmentLineId)
                .ForeignKey("Web.SaleOrderAmendmentHeaders", t => t.SaleOrderAmendmentHeaderId)
                .ForeignKey("Web.SaleOrderLines", t => t.SaleOrderLineId)
                .Index(t => t.SaleOrderAmendmentHeaderId)
                .Index(t => t.SaleOrderLineId);
            
            CreateTable(
                "Web.SaleOrderRateAmendmentLines",
                c => new
                    {
                        SaleOrderRateAmendmentLineId = c.Int(nullable: false, identity: true),
                        SaleOrderAmendmentHeaderId = c.Int(nullable: false),
                        SaleOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleOrderRateAmendmentLineId)
                .ForeignKey("Web.SaleOrderAmendmentHeaders", t => t.SaleOrderAmendmentHeaderId)
                .ForeignKey("Web.SaleOrderLines", t => t.SaleOrderLineId)
                .Index(t => t.SaleOrderAmendmentHeaderId)
                .Index(t => t.SaleOrderLineId);
            
            CreateTable(
                "Web.SaleOrderSettings",
                c => new
                    {
                        SaleOrderSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        CurrencyId = c.Int(nullable: false),
                        DeliveryTermsId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        UnitConversionForId = c.Byte(nullable: false),
                        isVisibleCurrency = c.Boolean(),
                        isVisibleShipMethod = c.Boolean(),
                        isVisibleDeliveryTerms = c.Boolean(),
                        isVisiblePriority = c.Boolean(),
                        isVisibleDimension1 = c.Boolean(),
                        isVisibleDimension2 = c.Boolean(),
                        isVisibleDealUnit = c.Boolean(),
                        isVisibleSpecification = c.Boolean(),
                        isVisibleProductCode = c.Boolean(),
                        isVisibleUnitConversionFor = c.Boolean(),
                        isVisibleAdvance = c.Boolean(),
                        filterLedgerAccountGroups = c.String(),
                        filterLedgerAccounts = c.String(),
                        filterProductTypes = c.String(),
                        filterProductGroups = c.String(),
                        filterProducts = c.String(),
                        SqlProcDocumentPrint = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterSubmit = c.String(maxLength: 100),
                        SqlProcDocumentPrint_AfterApprove = c.String(maxLength: 100),
                        ImportMenuId = c.Int(),
                        CalculationId = c.Int(),
                        ProcessId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SaleOrderSettingsId)
                .ForeignKey("Web.Calculations", t => t.CalculationId)
                .ForeignKey("Web.Currencies", t => t.CurrencyId)
                .ForeignKey("Web.DeliveryTerms", t => t.DeliveryTermsId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Menus", t => t.ImportMenuId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.ShipMethods", t => t.ShipMethodId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ShipMethodId)
                .Index(t => t.CurrencyId)
                .Index(t => t.DeliveryTermsId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.UnitConversionForId)
                .Index(t => t.ImportMenuId)
                .Index(t => t.CalculationId)
                .Index(t => t.ProcessId);
            
            CreateTable(
                "Web.SchemeDateDetails",
                c => new
                    {
                        SchemeDateDetailId = c.Int(nullable: false, identity: true),
                        SchemeId = c.Int(nullable: false),
                        ReceiveFromDate = c.DateTime(nullable: false),
                        ReceiveToDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SchemeDateDetailId)
                .ForeignKey("Web.SchemeHeaders", t => t.SchemeId)
                .Index(t => t.SchemeId);
            
            CreateTable(
                "Web.SchemeHeaders",
                c => new
                    {
                        SchemeId = c.Int(nullable: false, identity: true),
                        SchemeName = c.String(nullable: false, maxLength: 100),
                        ProcessId = c.Int(nullable: false),
                        OrderFromDate = c.DateTime(nullable: false),
                        OrderToDate = c.DateTime(nullable: false),
                        ApplicableOn = c.String(nullable: false, maxLength: 100),
                        ApplicableValues = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SchemeId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .Index(t => t.SchemeName, unique: true, name: "IX_Scheme_SchemeName")
                .Index(t => t.ProcessId);
            
            CreateTable(
                "Web.SchemeRateDetails",
                c => new
                    {
                        SchemeRateDetailId = c.Int(nullable: false, identity: true),
                        SchemeId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitId = c.String(nullable: false, maxLength: 3),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SchemeRateDetailId)
                .ForeignKey("Web.SchemeHeaders", t => t.SchemeId)
                .ForeignKey("Web.Units", t => t.UnitId)
                .Index(t => t.SchemeId)
                .Index(t => t.UnitId);
            
            CreateTable(
                "Web.StockAdjs",
                c => new
                    {
                        StockAdjId = c.Int(nullable: false, identity: true),
                        StockInId = c.Int(nullable: false),
                        StockOutId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        AdjustedQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.StockAdjId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .ForeignKey("Web.Stocks", t => t.StockInId)
                .ForeignKey("Web.Stocks", t => t.StockOutId)
                .Index(t => t.StockInId)
                .Index(t => t.StockOutId)
                .Index(t => new { t.DivisionId, t.SiteId }, unique: true, name: "IX_Stock_DocID");
            
            CreateTable(
                "Web.StockBalances",
                c => new
                    {
                        StockBalanceId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        ProcessId = c.Int(),
                        GodownId = c.Int(nullable: false),
                        CostCenterId = c.Int(),
                        LotNo = c.String(maxLength: 50),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.StockBalanceId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.ProcessId)
                .Index(t => t.GodownId)
                .Index(t => t.CostCenterId);
            
            CreateTable(
                "Web.StockInHandSettings",
                c => new
                    {
                        StockInHandSettingId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FromDate = c.DateTime(),
                        ToDate = c.DateTime(),
                        DivisionIds = c.String(),
                        SiteIds = c.String(),
                        GroupOn = c.String(),
                        ShowBalance = c.String(),
                    })
                .PrimaryKey(t => t.StockInHandSettingId);
            
            CreateTable(
                "Web.StockInOuts",
                c => new
                    {
                        StockInOutId = c.Int(nullable: false, identity: true),
                        StockOutId = c.Int(nullable: false),
                        StockInId = c.Int(nullable: false),
                        Qty_Adj = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StockInOutId)
                .ForeignKey("Web.Stocks", t => t.StockInId)
                .ForeignKey("Web.Stocks", t => t.StockOutId)
                .Index(t => t.StockOutId)
                .Index(t => t.StockInId);
            
            CreateTable(
                "Web.StockProcessBalances",
                c => new
                    {
                        StockProcessBalanceId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        ProcessId = c.Int(),
                        GodownId = c.Int(),
                        CostCenterId = c.Int(),
                        LotNo = c.String(maxLength: 10),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.StockProcessBalanceId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.ProcessId)
                .Index(t => t.GodownId)
                .Index(t => t.CostCenterId);
            
            CreateTable(
                "Web.StockUid",
                c => new
                    {
                        StockUidId = c.Int(nullable: false, identity: true),
                        DocHeaderId = c.Int(nullable: false),
                        DocLineId = c.Int(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        GodownId = c.Int(),
                        ProcessId = c.Int(),
                        ProductUidId = c.Int(nullable: false),
                        Qty_Iss = c.Int(nullable: false),
                        Qty_Rec = c.Int(nullable: false),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StockUidId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.Processes", t => t.ProcessId)
                .ForeignKey("Web.ProductUids", t => t.ProductUidId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.GodownId)
                .Index(t => t.ProcessId)
                .Index(t => t.ProductUidId);
            
            CreateTable(
                "Web.StockVirtual",
                c => new
                    {
                        DocHeaderId = c.Int(nullable: false, identity: true),
                        DocLineId = c.Int(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Qty_Iss = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Qty_Rec = c.Decimal(nullable: false, precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.DocHeaderId);
            
            CreateTable(
                "Web.TdsRates",
                c => new
                    {
                        TdsRateId = c.Int(nullable: false, identity: true),
                        TdsCategoryId = c.Int(nullable: false),
                        TdsGroupId = c.Int(nullable: false),
                        Percentage = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.TdsRateId)
                .ForeignKey("Web.TdsCategories", t => t.TdsCategoryId)
                .ForeignKey("Web.TdsGroups", t => t.TdsGroupId)
                .Index(t => t.TdsCategoryId)
                .Index(t => t.TdsGroupId);
            
            CreateTable(
                "Web.TempUserStores",
                c => new
                    {
                        TempUserStoreId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.TempUserStoreId);
            
            CreateTable(
                "Web.TrialBalanceSettings",
                c => new
                    {
                        TrialBalanceSettingId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FromDate = c.DateTime(),
                        ToDate = c.DateTime(),
                        DivisionIds = c.String(),
                        ShowZeroBalance = c.Boolean(nullable: false),
                        SiteIds = c.String(),
                        CostCenter = c.String(),
                        DisplayType = c.String(),
                        DrCr = c.String(),
                    })
                .PrimaryKey(t => t.TrialBalanceSettingId);
            
            CreateTable(
                "Web.UnitConversions",
                c => new
                    {
                        UnitConversionId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(),
                        UnitConversionForId = c.Byte(nullable: false),
                        FromQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        FromUnitId = c.String(maxLength: 3),
                        ToQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ToUnitId = c.String(maxLength: 3),
                        Description = c.String(maxLength: 100),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.UnitConversionId)
                .ForeignKey("Web.Units", t => t.FromUnitId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.Units", t => t.ToUnitId)
                .ForeignKey("Web.UnitConversionFors", t => t.UnitConversionForId)
                .Index(t => new { t.ProductId, t.UnitConversionForId, t.FromUnitId, t.ToUnitId }, unique: true, name: "IX_UnitConversion_UniqueKey");
            
            CreateTable(
                "Web.UrgentLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Qty = c.Decimal(precision: 18, scale: 4),
                        UnplannedQty = c.Decimal(precision: 18, scale: 4),
                        PlannedQty = c.Decimal(precision: 18, scale: 4),
                        InProcessQty = c.Decimal(precision: 18, scale: 4),
                        StockQty = c.Decimal(precision: 18, scale: 4),
                        PackedQty = c.Decimal(precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Web.UserBookMarks",
                c => new
                    {
                        UserBookMarkId = c.Int(nullable: false, identity: true),
                        ApplicationUserName = c.String(maxLength: 128),
                        MenuId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.UserBookMarkId)
                .ForeignKey("Web.Menus", t => t.MenuId)
                .Index(t => t.MenuId);
            
            CreateTable(
                "Web.UserInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Web.UserPersons",
                c => new
                    {
                        UserPersonId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        PersonId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserPersonId)
                .ForeignKey("Web.People", t => t.PersonId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "Web.UserRoles",
                c => new
                    {
                        UserRoleId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        RoleId = c.String(maxLength: 128),
                        ExpiryDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserRoleId)
                .ForeignKey("Web.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "Web.ViewJobInvoiceBalance",
                c => new
                    {
                        JobInvoiceLineId = c.Int(nullable: false, identity: true),
                        JobReceiveHeaderId = c.Int(nullable: false),
                        JobReceiveLineId = c.Int(nullable: false),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        JobInvoiceHeaderId = c.Int(nullable: false),
                        JobInvoiceNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        InvoiceDate = c.DateTime(nullable: false),
                        JobInvoiceDocTypeId = c.Int(nullable: false),
                        JobReceiveNo = c.String(),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobInvoiceLineId);
            
            CreateTable(
                "Web.ViewJobInvoiceBalanceForRateAmendment",
                c => new
                    {
                        JobInvoiceLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        JobInvoiceHeaderId = c.Int(nullable: false),
                        JobInvoiceNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        InvoiceDate = c.DateTime(nullable: false),
                        ProductUidId = c.Int(),
                        ProductUidName = c.String(),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        ProcessId = c.Int(nullable: false),
                        ProcessName = c.String(),
                    })
                .PrimaryKey(t => t.JobInvoiceLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.ViewJobOrderBalance",
                c => new
                    {
                        JobOrderLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        JobOrderHeaderId = c.Int(nullable: false),
                        JobOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        ProductUidId = c.Int(),
                    })
                .PrimaryKey(t => t.JobOrderLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.ProductId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.ViewJobOrderBalanceForInspection",
                c => new
                    {
                        JobOrderLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        JobOrderHeaderId = c.Int(nullable: false),
                        JobOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        ProductUidId = c.Int(),
                    })
                .PrimaryKey(t => t.JobOrderLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.ProductId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.ViewJobOrderBalanceForInspectionRequest",
                c => new
                    {
                        JobOrderLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        JobOrderHeaderId = c.Int(nullable: false),
                        JobOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        ProductUidId = c.Int(),
                    })
                .PrimaryKey(t => t.JobOrderLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.ProductId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.ViewJobOrderBalanceForInvoice",
                c => new
                    {
                        JobOrderLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        JobOrderHeaderId = c.Int(nullable: false),
                        JobOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                    })
                .PrimaryKey(t => t.JobOrderLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.JobWorkers", t => t.JobWorkerId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.JobWorkerId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.ViewJobOrderBalanceFromStatus",
                c => new
                    {
                        JobOrderLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        JobOrderHeaderId = c.Int(nullable: false),
                        JobOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        ProductUidId = c.Int(),
                    })
                .PrimaryKey(t => t.JobOrderLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.ProductId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.ViewJobOrderHeader",
                c => new
                    {
                        JobOrderHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        BillToPartyId = c.Int(nullable: false),
                        OrderById = c.Int(nullable: false),
                        GodownId = c.Int(nullable: false),
                        JobInstructionId = c.Int(nullable: false),
                        TermsAndConditions = c.String(),
                        ProcessId = c.Int(nullable: false),
                        ConstCenterId = c.Int(nullable: false),
                        MachineId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        TotalQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.JobOrderHeaderId);
            
            CreateTable(
                "Web.ViewJobOrderInspectionRequestBalance",
                c => new
                    {
                        JobOrderInspectionRequestLineId = c.Int(nullable: false, identity: true),
                        JobOrderLineId = c.Int(nullable: false),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        JobOrderInspectionRequestHeaderId = c.Int(nullable: false),
                        JobOrderInspectionRequestNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        ProductUidId = c.Int(),
                    })
                .PrimaryKey(t => t.JobOrderInspectionRequestLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.ProductId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.ViewJobOrderLine",
                c => new
                    {
                        JobOrderLineId = c.Int(nullable: false, identity: true),
                        JobOrderHeaderId = c.Int(nullable: false),
                        OrderQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OrderAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProductId = c.Int(nullable: false),
                        DeliveryUnitId = c.String(),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.JobOrderLineId);
            
            CreateTable(
                "Web.ViewJobReceiveBalance",
                c => new
                    {
                        JobReceiveLineId = c.Int(nullable: false, identity: true),
                        JobOrderLineId = c.Int(nullable: false),
                        JobOrderHeaderId = c.Int(nullable: false),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        JobReceiveHeaderId = c.Int(nullable: false),
                        JobReceiveNo = c.String(),
                        JobOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        JobWorkerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobReceiveLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.JobOrderLines", t => t.JobOrderLineId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.JobOrderLineId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.ViewJobReceiveBalanceForInvoice",
                c => new
                    {
                        JobReceiveLineId = c.Int(nullable: false, identity: true),
                        JobOrderLineId = c.Int(nullable: false),
                        JobOrderHeaderId = c.Int(nullable: false),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        JobReceiveHeaderId = c.Int(nullable: false),
                        JobReceiveNo = c.String(),
                        JobOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        JobWorkerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobReceiveLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.JobOrderLines", t => t.JobOrderLineId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.JobOrderLineId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id);
            
            CreateTable(
                "Web.ViewJobReceiveBalanceForQA",
                c => new
                    {
                        JobReceiveLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        JobReceiveHeaderId = c.Int(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        JobReceiveNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        JobWorkerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        ProductUidId = c.Int(),
                    })
                .PrimaryKey(t => t.JobReceiveLineId);
            
            CreateTable(
                "Web.ViewMaterialPlanBalance",
                c => new
                    {
                        MaterialPlanLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        MaterialPlanHeaderId = c.Int(nullable: false),
                        MaterialPlanNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        MaterialPlanDate = c.DateTime(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        DocTypeName = c.String(),
                    })
                .PrimaryKey(t => t.MaterialPlanLineId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .Index(t => t.DocTypeId);
            
            CreateTable(
                "Web.ViewMaterialRequestBalance",
                c => new
                    {
                        RequisitionLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        RequisitionHeaderId = c.Int(nullable: false),
                        MaterialRequestNo = c.String(),
                        PersonId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        MaterialRequestDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RequisitionLineId);
            
            CreateTable(
                "Web.ViewProdOrderBalance",
                c => new
                    {
                        ProdOrderLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProdOrderHeaderId = c.Int(nullable: false),
                        ProdOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        IndentDate = c.DateTime(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        ReferenceDocLineId = c.Int(),
                        ReferenceDocTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.ProdOrderLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.DocTypeId);
            
            CreateTable(
                "Web.ViewProdOrderBalanceForMPlan",
                c => new
                    {
                        ProdOrderLineId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProdOrderHeaderId = c.Int(nullable: false),
                        ProdOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        IndentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProdOrderLineId);
            
            CreateTable(
                "Web.ViewProdOrderHeader",
                c => new
                    {
                        ProdOrderHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        TotalQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.ProdOrderHeaderId);
            
            CreateTable(
                "Web.ViewProdOrderLine",
                c => new
                    {
                        ProdOrderLineId = c.Int(nullable: false, identity: true),
                        ProdOrderHeaderId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProductId = c.Int(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.ProdOrderLineId);
            
            CreateTable(
                "Web.ViewPurchaseGoodsReceiptBalance",
                c => new
                    {
                        PurchaseGoodsReceiptLineId = c.Int(nullable: false, identity: true),
                        PurchaseOrderHeaderId = c.Int(nullable: false),
                        PurchaseOrderLineId = c.Int(nullable: false),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceDealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceDocQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PurchaseGoodsReceiptHeaderId = c.Int(nullable: false),
                        PurchaseGoodsReceiptNo = c.String(),
                        PurchaseOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseGoodsReceiptLineId);
            
            CreateTable(
                "Web.ViewPurchaseGoodsReceiptLine",
                c => new
                    {
                        PurchaseGoodsReceiptLineId = c.Int(nullable: false, identity: true),
                        PurchaseGoodsReceiptHeaderId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProductId = c.Int(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.PurchaseGoodsReceiptLineId);
            
            CreateTable(
                "Web.ViewPurchaseIndentBalance",
                c => new
                    {
                        PurchaseIndentLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PurchaseIndentHeaderId = c.Int(nullable: false),
                        PurchaseIndentNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        IndentDate = c.DateTime(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        DocTypeName = c.String(),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseIndentLineId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .Index(t => t.DocTypeId);
            
            CreateTable(
                "Web.ViewPurchaseIndentHeader",
                c => new
                    {
                        PurchaseIndentHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        Status = c.String(),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        TotalQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.PurchaseIndentHeaderId);
            
            CreateTable(
                "Web.ViewPurchaseIndentLine",
                c => new
                    {
                        PurchaseIndentLineId = c.Int(nullable: false, identity: true),
                        PurchaseIndentHeaderId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProductId = c.Int(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.PurchaseIndentLineId);
            
            CreateTable(
                "Web.ViewPurchaseInvoiceBalance",
                c => new
                    {
                        PurchaseInvoiceLineId = c.Int(nullable: false, identity: true),
                        PurchaseGoodsReceiptHeaderId = c.Int(nullable: false),
                        PurchaseGoodsReceiptLineId = c.Int(nullable: false),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PurchaseInvoiceHeaderId = c.Int(nullable: false),
                        PurchaseInvoiceNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        PurchaseInvoiceDocTypeId = c.Int(nullable: false),
                        PurchaseGoodsReceiptNo = c.String(),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseInvoiceLineId);
            
            CreateTable(
                "Web.ViewPurchaseOrderBalance",
                c => new
                    {
                        PurchaseOrderLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PurchaseOrderHeaderId = c.Int(nullable: false),
                        PurchaseOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseOrderLineId);
            
            CreateTable(
                "Web.ViewPurchaseOrderBalanceForInvoice",
                c => new
                    {
                        PurchaseOrderLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PurchaseOrderHeaderId = c.Int(nullable: false),
                        PurchaseOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                    })
                .PrimaryKey(t => t.PurchaseOrderLineId);
            
            CreateTable(
                "Web.ViewPurchaseOrderHeader",
                c => new
                    {
                        PurchaseOrderHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        DeliveryTermsId = c.Int(nullable: false),
                        ShipAddressId = c.Int(nullable: false),
                        SupplierShipDate = c.DateTime(nullable: false),
                        SupplierRemark = c.String(),
                        CreditDays = c.Int(nullable: false),
                        ProgressPer = c.Int(nullable: false),
                        Status = c.String(),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        TotalQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.PurchaseOrderHeaderId);
            
            CreateTable(
                "Web.ViewPurchaseOrderLine",
                c => new
                    {
                        PurchaseOrderLineId = c.Int(nullable: false, identity: true),
                        PurchaseOrderHeaderId = c.Int(nullable: false),
                        OrderQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OrderDeliveryQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OrderAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProductId = c.Int(nullable: false),
                        DeliveryUnitId = c.String(),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.PurchaseOrderLineId);
            
            CreateTable(
                "Web.ViewRequisitionBalance",
                c => new
                    {
                        RequisitionLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        RequisitionHeaderId = c.Int(nullable: false),
                        RequisitionNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        CostCenterId = c.Int(),
                    })
                .PrimaryKey(t => t.RequisitionLineId)
                .ForeignKey("Web.CostCenters", t => t.CostCenterId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.People", t => t.PersonId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.ProductId)
                .Index(t => t.PersonId)
                .Index(t => t.SiteId)
                .Index(t => t.DivisionId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.CostCenterId);
            
            CreateTable(
                "Web._Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Web.ViewRugArea",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        SizeId = c.Int(nullable: false),
                        ProductShapeId = c.Int(nullable: false),
                        SizeName = c.String(),
                        Area = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitId = c.String(),
                        SqYardPerPcs = c.Decimal(nullable: false, precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.ProductId);
            
            CreateTable(
                "Web.ViewRugSize",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        StandardSizeID = c.Int(),
                        StandardSizeName = c.String(),
                        StandardSizeArea = c.Decimal(precision: 18, scale: 4),
                        ManufaturingSizeID = c.Int(),
                        ManufaturingSizeName = c.String(),
                        ManufaturingSizeArea = c.Decimal(precision: 18, scale: 4),
                        FinishingSizeID = c.Int(),
                        FinishingSizeName = c.String(),
                        FinishingSizeArea = c.Decimal(precision: 18, scale: 4),
                        StencilSizeId = c.Int(),
                        StencilSizeName = c.String(),
                        StencilSizeArea = c.Decimal(precision: 18, scale: 4),
                        MapSizeId = c.Int(),
                        MapSizeName = c.String(),
                        MapSizeArea = c.Decimal(precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.ProductId);
            
            CreateTable(
                "Web.ViewSaleDeliveryOrderBalance",
                c => new
                    {
                        SaleDeliveryOrderLineId = c.Int(nullable: false, identity: true),
                        OrderQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DispatchQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SaleOrderLineId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SaleDeliveryOrderHeaderId = c.Int(nullable: false),
                        SaleDeliveryOrderNo = c.String(),
                        BuyerId = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        Priority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SaleDeliveryOrderLineId);
            
            CreateTable(
                "Web.ViewSaleDispatchBalance",
                c => new
                    {
                        SaleDispatchLineId = c.Int(nullable: false, identity: true),
                        SaleOrderHeaderId = c.Int(nullable: false),
                        SaleOrderLineId = c.Int(nullable: false),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceDealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceDocQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SaleDispatchHeaderId = c.Int(nullable: false),
                        SaleDispatchNo = c.String(),
                        SaleOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        BuyerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        Rate = c.Decimal(precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.SaleDispatchLineId);
            
            CreateTable(
                "Web.ViewSaleInvoiceBalance",
                c => new
                    {
                        SaleInvoiceLineId = c.Int(nullable: false, identity: true),
                        SaleDispatchHeaderId = c.Int(nullable: false),
                        SaleDispatchLineId = c.Int(nullable: false),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SaleInvoiceHeaderId = c.Int(nullable: false),
                        SaleInvoiceNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        SaleToBuyerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        SaleInvoiceDocTypeId = c.Int(nullable: false),
                        SaleDispatchNo = c.String(),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SaleInvoiceLineId);
            
            CreateTable(
                "Web.ViewSaleInvoiceLine",
                c => new
                    {
                        SaleInvoiceHeaderId = c.Int(nullable: false, identity: true),
                        SaleInvoiceLineId = c.Int(nullable: false),
                        ProductUidId = c.Int(),
                        ProductID = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        Specification = c.String(),
                        SaleOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BaleNo = c.String(),
                        ProductInvoiceGroupId = c.Int(),
                        DealQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DealUnitId = c.String(),
                        GrossWeight = c.Decimal(nullable: false, precision: 18, scale: 4),
                        NetWeight = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Sr = c.Int(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.SaleInvoiceHeaderId);
            
            CreateTable(
                "Web.ViewSaleOrderBalance",
                c => new
                    {
                        SaleOrderLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SaleOrderHeaderId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        SaleOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        BuyerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SaleOrderLineId)
                .ForeignKey("Web.Dimension1", t => t.Dimension1Id)
                .ForeignKey("Web.Dimension2", t => t.Dimension2Id)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.Dimension1Id)
                .Index(t => t.Dimension2Id)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Web.ViewSaleOrderBalanceForCancellation",
                c => new
                    {
                        SaleOrderLineId = c.Int(nullable: false, identity: true),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BalanceAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SaleOrderHeaderId = c.Int(nullable: false),
                        SaleOrderNo = c.String(),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        BuyerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SaleOrderLineId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Web.ViewSaleOrderHeader",
                c => new
                    {
                        SaleOrderHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        SaleToBuyerId = c.Int(nullable: false),
                        BillToBuyerId = c.Int(nullable: false),
                        CurrencyId = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        ShipMethodId = c.Int(nullable: false),
                        ShipAddress = c.String(),
                        DeliveryTermsId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        BuyerOrderNo = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        TotalQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.SaleOrderHeaderId);
            
            CreateTable(
                "Web.ViewSaleOrderLine",
                c => new
                    {
                        SaleOrderLineId = c.Int(nullable: false, identity: true),
                        SaleOrderHeaderId = c.Int(nullable: false),
                        OrderQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProductId = c.Int(nullable: false),
                        Specification = c.String(),
                        DeliveryUnitId = c.Int(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.SaleOrderLineId);
            
            CreateTable(
                "Web.ViewStockInBalance",
                c => new
                    {
                        StockInId = c.Int(nullable: false, identity: true),
                        StockInNo = c.String(),
                        StockInDate = c.DateTime(nullable: false),
                        PersonId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        Dimension1Id = c.Int(),
                        Dimension2Id = c.Int(),
                        LotNo = c.String(),
                        BalanceQty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StockInId);
            
            CreateTable(
                "Web.WeavingRetensions",
                c => new
                    {
                        WeavingRetensionId = c.Int(nullable: false, identity: true),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProductCategoryId = c.Int(),
                        RetensionPer = c.Decimal(nullable: false, precision: 18, scale: 4),
                        MinimumAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.WeavingRetensionId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.ProductCategories", t => t.ProductCategoryId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => new { t.DivisionId, t.SiteId }, unique: true, name: "IX_WeavingRetension_DocID")
                .Index(t => t.ProductCategoryId);
            
            CreateTable(
                "Web.FinishedProduct",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        IsSample = c.Boolean(nullable: false),
                        ProductCategoryId = c.Int(),
                        ProductCollectionId = c.Int(),
                        ProductQualityId = c.Int(),
                        ProductDesignId = c.Int(),
                        ProductDesignPatternId = c.Int(),
                        ColourId = c.Int(),
                        FaceContentId = c.Int(),
                        ContentId = c.Int(),
                        ProductInvoiceGroupId = c.Int(),
                        ProcessSequenceHeaderId = c.Int(),
                        ProductManufacturerId = c.Int(),
                        ProductStyleId = c.Int(),
                        DescriptionOfGoodsId = c.Int(),
                        OriginCountryId = c.Int(),
                        ProductShapeId = c.Int(),
                        SampleId = c.Int(),
                        CounterNo = c.Int(),
                        DiscontinuedDate = c.DateTime(),
                        MapScale = c.Int(),
                        TraceType = c.String(),
                        MapType = c.String(),
                        ProductionRemark = c.String(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("Web.Products", t => t.ProductId)
                .ForeignKey("Web.ProductCategories", t => t.ProductCategoryId)
                .ForeignKey("Web.ProductCollections", t => t.ProductCollectionId)
                .ForeignKey("Web.ProductQualities", t => t.ProductQualityId)
                .ForeignKey("Web.ProductDesigns", t => t.ProductDesignId)
                .ForeignKey("Web.ProductDesignPatterns", t => t.ProductDesignPatternId)
                .ForeignKey("Web.Colours", t => t.ColourId)
                .ForeignKey("Web.ProductContentHeaders", t => t.FaceContentId)
                .ForeignKey("Web.ProductContentHeaders", t => t.ContentId)
                .ForeignKey("Web.ProductInvoiceGroups", t => t.ProductInvoiceGroupId)
                .ForeignKey("Web.ProcessSequenceHeaders", t => t.ProcessSequenceHeaderId)
                .ForeignKey("Web.ProductManufacturer", t => t.ProductManufacturerId)
                .ForeignKey("Web.ProductStyles", t => t.ProductStyleId)
                .ForeignKey("Web.DescriptionOfGoods", t => t.DescriptionOfGoodsId)
                .ForeignKey("Web.Countries", t => t.OriginCountryId)
                .ForeignKey("Web.ProductTypes", t => t.ProductShapeId)
                .ForeignKey("Web.Products", t => t.SampleId)
                .Index(t => t.ProductId)
                .Index(t => t.ProductCategoryId)
                .Index(t => t.ProductCollectionId)
                .Index(t => t.ProductQualityId)
                .Index(t => t.ProductDesignId)
                .Index(t => t.ProductDesignPatternId)
                .Index(t => t.ColourId)
                .Index(t => t.FaceContentId)
                .Index(t => t.ContentId)
                .Index(t => t.ProductInvoiceGroupId)
                .Index(t => t.ProcessSequenceHeaderId)
                .Index(t => t.ProductManufacturerId)
                .Index(t => t.ProductStyleId)
                .Index(t => t.DescriptionOfGoodsId)
                .Index(t => t.OriginCountryId)
                .Index(t => t.ProductShapeId)
                .Index(t => t.SampleId);
            
            CreateTable(
                "Web.SaleInvoiceHeaderDetail",
                c => new
                    {
                        SaleInvoiceHeaderId = c.Int(nullable: false),
                        BLNo = c.String(maxLength: 20),
                        BLDate = c.DateTime(nullable: false),
                        PrivateMark = c.String(maxLength: 20),
                        PortOfLoading = c.String(maxLength: 50),
                        DestinationPort = c.String(maxLength: 50),
                        FinalPlaceOfDelivery = c.String(maxLength: 50),
                        PreCarriageBy = c.String(maxLength: 50),
                        PlaceOfPreCarriage = c.String(maxLength: 50),
                        CircularNo = c.String(maxLength: 50),
                        CircularDate = c.DateTime(nullable: false),
                        OrderNo = c.String(maxLength: 50),
                        OrderDate = c.DateTime(nullable: false),
                        BaleNoSeries = c.String(),
                        DescriptionOfGoods = c.String(),
                        PackingMaterialDescription = c.String(),
                        KindsOfackages = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Compositions = c.String(),
                        OtherRefrence = c.String(),
                        TermsOfSale = c.String(),
                        NotifyParty = c.String(),
                        TransporterInformation = c.String(),
                        InvoiceAmount = c.Decimal(precision: 18, scale: 4),
                        Freight = c.Decimal(precision: 18, scale: 4),
                        VehicleNo = c.String(),
                    })
                .PrimaryKey(t => t.SaleInvoiceHeaderId)
                .ForeignKey("Web.SaleInvoiceHeaders", t => t.SaleInvoiceHeaderId)
                .Index(t => t.SaleInvoiceHeaderId);
            
            CreateTable(
                "Web.ProductManufacturer",
                c => new
                    {
                        PersonID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("Web.People", t => t.PersonID)
                .Index(t => t.PersonID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Web.ProductManufacturer", "PersonID", "Web.People");
            DropForeignKey("Web.SaleInvoiceHeaderDetail", "SaleInvoiceHeaderId", "Web.SaleInvoiceHeaders");
            DropForeignKey("Web.FinishedProduct", "SampleId", "Web.Products");
            DropForeignKey("Web.FinishedProduct", "ProductShapeId", "Web.ProductTypes");
            DropForeignKey("Web.FinishedProduct", "OriginCountryId", "Web.Countries");
            DropForeignKey("Web.FinishedProduct", "DescriptionOfGoodsId", "Web.DescriptionOfGoods");
            DropForeignKey("Web.FinishedProduct", "ProductStyleId", "Web.ProductStyles");
            DropForeignKey("Web.FinishedProduct", "ProductManufacturerId", "Web.ProductManufacturer");
            DropForeignKey("Web.FinishedProduct", "ProcessSequenceHeaderId", "Web.ProcessSequenceHeaders");
            DropForeignKey("Web.FinishedProduct", "ProductInvoiceGroupId", "Web.ProductInvoiceGroups");
            DropForeignKey("Web.FinishedProduct", "ContentId", "Web.ProductContentHeaders");
            DropForeignKey("Web.FinishedProduct", "FaceContentId", "Web.ProductContentHeaders");
            DropForeignKey("Web.FinishedProduct", "ColourId", "Web.Colours");
            DropForeignKey("Web.FinishedProduct", "ProductDesignPatternId", "Web.ProductDesignPatterns");
            DropForeignKey("Web.FinishedProduct", "ProductDesignId", "Web.ProductDesigns");
            DropForeignKey("Web.FinishedProduct", "ProductQualityId", "Web.ProductQualities");
            DropForeignKey("Web.FinishedProduct", "ProductCollectionId", "Web.ProductCollections");
            DropForeignKey("Web.FinishedProduct", "ProductCategoryId", "Web.ProductCategories");
            DropForeignKey("Web.FinishedProduct", "ProductId", "Web.Products");
            DropForeignKey("Web.WeavingRetensions", "SiteId", "Web.Sites");
            DropForeignKey("Web.WeavingRetensions", "ProductCategoryId", "Web.ProductCategories");
            DropForeignKey("Web.WeavingRetensions", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ViewSaleOrderBalanceForCancellation", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewSaleOrderBalance", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewSaleOrderBalance", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ViewSaleOrderBalance", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ViewRequisitionBalance", "SiteId", "Web.Sites");
            DropForeignKey("Web.ViewRequisitionBalance", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewRequisitionBalance", "PersonId", "Web.People");
            DropForeignKey("Web.ViewRequisitionBalance", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ViewRequisitionBalance", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ViewRequisitionBalance", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ViewRequisitionBalance", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.ViewPurchaseIndentBalance", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ViewProdOrderBalance", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewProdOrderBalance", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ViewProdOrderBalance", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ViewProdOrderBalance", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ViewMaterialPlanBalance", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ViewJobReceiveBalanceForInvoice", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewJobReceiveBalanceForInvoice", "JobOrderLineId", "Web.JobOrderLines");
            DropForeignKey("Web.ViewJobReceiveBalanceForInvoice", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ViewJobReceiveBalanceForInvoice", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ViewJobReceiveBalance", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewJobReceiveBalance", "JobOrderLineId", "Web.JobOrderLines");
            DropForeignKey("Web.ViewJobReceiveBalance", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ViewJobReceiveBalance", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ViewJobOrderInspectionRequestBalance", "SiteId", "Web.Sites");
            DropForeignKey("Web.ViewJobOrderInspectionRequestBalance", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewJobOrderInspectionRequestBalance", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ViewJobOrderInspectionRequestBalance", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ViewJobOrderInspectionRequestBalance", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ViewJobOrderBalanceFromStatus", "SiteId", "Web.Sites");
            DropForeignKey("Web.ViewJobOrderBalanceFromStatus", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewJobOrderBalanceFromStatus", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ViewJobOrderBalanceFromStatus", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ViewJobOrderBalanceFromStatus", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ViewJobOrderBalanceForInvoice", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewJobOrderBalanceForInvoice", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.ViewJobOrderBalanceForInvoice", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ViewJobOrderBalanceForInvoice", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ViewJobOrderBalanceForInspectionRequest", "SiteId", "Web.Sites");
            DropForeignKey("Web.ViewJobOrderBalanceForInspectionRequest", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewJobOrderBalanceForInspectionRequest", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ViewJobOrderBalanceForInspectionRequest", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ViewJobOrderBalanceForInspectionRequest", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ViewJobOrderBalanceForInspection", "SiteId", "Web.Sites");
            DropForeignKey("Web.ViewJobOrderBalanceForInspection", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewJobOrderBalanceForInspection", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ViewJobOrderBalanceForInspection", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ViewJobOrderBalanceForInspection", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ViewJobOrderBalance", "SiteId", "Web.Sites");
            DropForeignKey("Web.ViewJobOrderBalance", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewJobOrderBalance", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ViewJobOrderBalance", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ViewJobOrderBalance", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ViewJobInvoiceBalanceForRateAmendment", "ProductId", "Web.Products");
            DropForeignKey("Web.ViewJobInvoiceBalanceForRateAmendment", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.ViewJobInvoiceBalanceForRateAmendment", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ViewJobInvoiceBalanceForRateAmendment", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.AspNetUserRoles", "IdentityUser_Id", "Web.Users");
            DropForeignKey("Web.AspNetUserLogins", "IdentityUser_Id", "Web.Users");
            DropForeignKey("Web.AspNetUserClaims", "IdentityUser_Id", "Web.Users");
            DropForeignKey("Web.UserRoles", "UserId", "Web.Users");
            DropForeignKey("Web.UserPersons", "PersonId", "Web.People");
            DropForeignKey("Web.UserBookMarks", "MenuId", "Web.Menus");
            DropForeignKey("Web.UnitConversions", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.UnitConversions", "ToUnitId", "Web.Units");
            DropForeignKey("Web.UnitConversions", "ProductId", "Web.Products");
            DropForeignKey("Web.UnitConversions", "FromUnitId", "Web.Units");
            DropForeignKey("Web.TdsRates", "TdsGroupId", "Web.TdsGroups");
            DropForeignKey("Web.TdsRates", "TdsCategoryId", "Web.TdsCategories");
            DropForeignKey("Web.StockUid", "SiteId", "Web.Sites");
            DropForeignKey("Web.StockUid", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.StockUid", "ProcessId", "Web.Processes");
            DropForeignKey("Web.StockUid", "GodownId", "Web.Godowns");
            DropForeignKey("Web.StockUid", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.StockUid", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.StockProcessBalances", "ProductId", "Web.Products");
            DropForeignKey("Web.StockProcessBalances", "ProcessId", "Web.Processes");
            DropForeignKey("Web.StockProcessBalances", "GodownId", "Web.Godowns");
            DropForeignKey("Web.StockProcessBalances", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.StockProcessBalances", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.StockProcessBalances", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.StockInOuts", "StockOutId", "Web.Stocks");
            DropForeignKey("Web.StockInOuts", "StockInId", "Web.Stocks");
            DropForeignKey("Web.StockBalances", "ProductId", "Web.Products");
            DropForeignKey("Web.StockBalances", "ProcessId", "Web.Processes");
            DropForeignKey("Web.StockBalances", "GodownId", "Web.Godowns");
            DropForeignKey("Web.StockBalances", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.StockBalances", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.StockBalances", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.StockAdjs", "StockOutId", "Web.Stocks");
            DropForeignKey("Web.StockAdjs", "StockInId", "Web.Stocks");
            DropForeignKey("Web.StockAdjs", "SiteId", "Web.Sites");
            DropForeignKey("Web.StockAdjs", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SchemeRateDetails", "UnitId", "Web.Units");
            DropForeignKey("Web.SchemeRateDetails", "SchemeId", "Web.SchemeHeaders");
            DropForeignKey("Web.SchemeDateDetails", "SchemeId", "Web.SchemeHeaders");
            DropForeignKey("Web.SchemeHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.SaleOrderSettings", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.SaleOrderSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleOrderSettings", "ShipMethodId", "Web.ShipMethods");
            DropForeignKey("Web.SaleOrderSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.SaleOrderSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.SaleOrderSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleOrderSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleOrderSettings", "DeliveryTermsId", "Web.DeliveryTerms");
            DropForeignKey("Web.SaleOrderSettings", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.SaleOrderSettings", "CalculationId", "Web.Calculations");
            DropForeignKey("Web.SaleOrderRateAmendmentLines", "SaleOrderLineId", "Web.SaleOrderLines");
            DropForeignKey("Web.SaleOrderRateAmendmentLines", "SaleOrderAmendmentHeaderId", "Web.SaleOrderAmendmentHeaders");
            DropForeignKey("Web.SaleOrderQtyAmendmentLines", "SaleOrderLineId", "Web.SaleOrderLines");
            DropForeignKey("Web.SaleOrderQtyAmendmentLines", "SaleOrderAmendmentHeaderId", "Web.SaleOrderAmendmentHeaders");
            DropForeignKey("Web.SaleOrderLineStatus", "SaleOrderLineId", "Web.SaleOrderLines");
            DropForeignKey("Web.SaleOrderCancelHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleOrderCancelLines", "SaleOrderCancelHeaderId", "Web.SaleOrderCancelHeaders");
            DropForeignKey("Web.SaleOrderCancelLines", "SaleOrderLineId", "Web.SaleOrderLines");
            DropForeignKey("Web.SaleOrderCancelHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.SaleOrderCancelHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleOrderCancelHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleOrderCancelHeaders", "BuyerId", "Web.Buyers");
            DropForeignKey("Web.SaleOrderAmendmentHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleOrderAmendmentHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.SaleOrderAmendmentHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleOrderAmendmentHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleOrderAmendmentHeaders", "BuyerId", "Web.Buyers");
            DropForeignKey("Web.SaleInvoiceSettings", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.SaleInvoiceSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleInvoiceSettings", "ShipMethodId", "Web.ShipMethods");
            DropForeignKey("Web.SaleInvoiceSettings", "SalesTaxGroupId", "Web.SalesTaxGroups");
            DropForeignKey("Web.SaleInvoiceSettings", "SaleDispatchDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleInvoiceSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.SaleInvoiceSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.SaleInvoiceSettings", "GodownId", "Web.Godowns");
            DropForeignKey("Web.SaleInvoiceSettings", "DocTypePackingHeaderId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleInvoiceSettings", "DocTypeDispatchReturnId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleInvoiceSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleInvoiceSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleInvoiceSettings", "DeliveryTermsId", "Web.DeliveryTerms");
            DropForeignKey("Web.SaleInvoiceSettings", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.SaleInvoiceSettings", "CalculationId", "Web.Calculations");
            DropForeignKey("Web.SaleInvoiceReturnLineCharges", "LineTableId", "Web.SaleInvoiceReturnLines");
            DropForeignKey("Web.SaleInvoiceReturnLineCharges", "PersonID", "Web.People");
            DropForeignKey("Web.SaleInvoiceReturnLineCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.SaleInvoiceReturnLineCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.SaleInvoiceReturnLineCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.SaleInvoiceReturnLineCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.SaleInvoiceReturnLineCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.SaleInvoiceReturnLineCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.SaleInvoiceReturnLineCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.SaleInvoiceReturnLineCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.SaleInvoiceReturnLines", "SaleInvoiceReturnHeaderId", "Web.SaleInvoiceReturnHeaders");
            DropForeignKey("Web.SaleInvoiceReturnLines", "SaleInvoiceLineId", "Web.SaleInvoiceLines");
            DropForeignKey("Web.SaleInvoiceReturnLines", "SaleDispatchReturnLineId", "Web.SaleDispatchReturnLines");
            DropForeignKey("Web.SaleInvoiceReturnLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.SaleInvoiceReturnHeaderCharges", "HeaderTableId", "Web.SaleInvoiceReturnHeaders");
            DropForeignKey("Web.SaleInvoiceReturnHeaderCharges", "ProductChargeId", "Web.Charges");
            DropForeignKey("Web.SaleInvoiceReturnHeaderCharges", "PersonID", "Web.People");
            DropForeignKey("Web.SaleInvoiceReturnHeaderCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.SaleInvoiceReturnHeaderCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.SaleInvoiceReturnHeaderCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.SaleInvoiceReturnHeaderCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.SaleInvoiceReturnHeaderCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.SaleInvoiceReturnHeaderCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.SaleInvoiceReturnHeaderCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.SaleInvoiceReturnHeaderCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.SaleInvoiceReturnHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleInvoiceReturnHeaders", "SalesTaxGroupPartyId", "Web.SalesTaxGroupParties");
            DropForeignKey("Web.SaleInvoiceReturnHeaders", "SalesTaxGroupId", "Web.ChargeGroupPersons");
            DropForeignKey("Web.SaleInvoiceReturnHeaders", "SaleDispatchReturnHeaderId", "Web.SaleDispatchReturnHeaders");
            DropForeignKey("Web.SaleInvoiceReturnHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.SaleInvoiceReturnHeaders", "LedgerHeaderId", "Web.LedgerHeaders");
            DropForeignKey("Web.SaleInvoiceReturnHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleInvoiceReturnHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleInvoiceReturnHeaders", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.SaleInvoiceReturnHeaders", "BuyerId", "Web.Buyers");
            DropForeignKey("Web.SaleInvoiceLineCharges", "LineTableId", "Web.SaleInvoiceLines");
            DropForeignKey("Web.SaleInvoiceLineCharges", "PersonID", "Web.People");
            DropForeignKey("Web.SaleInvoiceLineCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.SaleInvoiceLineCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.SaleInvoiceLineCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.SaleInvoiceLineCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.SaleInvoiceLineCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.SaleInvoiceLineCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.SaleInvoiceLineCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.SaleInvoiceLineCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.SaleInvoiceHeaderCharges", "HeaderTableId", "Web.SaleInvoiceHeaders");
            DropForeignKey("Web.SaleInvoiceHeaderCharges", "ProductChargeId", "Web.Charges");
            DropForeignKey("Web.SaleInvoiceHeaderCharges", "PersonID", "Web.People");
            DropForeignKey("Web.SaleInvoiceHeaderCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.SaleInvoiceHeaderCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.SaleInvoiceHeaderCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.SaleInvoiceHeaderCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.SaleInvoiceHeaderCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.SaleInvoiceHeaderCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.SaleInvoiceHeaderCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.SaleInvoiceHeaderCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.SaleDispatchSettings", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.SaleDispatchSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleDispatchSettings", "ShipMethodId", "Web.ShipMethods");
            DropForeignKey("Web.SaleDispatchSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.SaleDispatchSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.SaleDispatchSettings", "GodownId", "Web.Godowns");
            DropForeignKey("Web.SaleDispatchSettings", "DocTypePackingHeaderId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleDispatchSettings", "DocTypeDispatchReturnId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleDispatchSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleDispatchSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleDispatchSettings", "DeliveryTermsId", "Web.DeliveryTerms");
            DropForeignKey("Web.SaleDispatchReturnLines", "StockId", "Web.Stocks");
            DropForeignKey("Web.SaleDispatchReturnLines", "SaleDispatchReturnHeaderId", "Web.SaleDispatchReturnHeaders");
            DropForeignKey("Web.SaleDispatchReturnLines", "SaleDispatchLineId", "Web.SaleDispatchLines");
            DropForeignKey("Web.SaleDispatchReturnLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.SaleDispatchReturnHeaders", "StockHeaderId", "Web.StockHeaders");
            DropForeignKey("Web.SaleDispatchReturnHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleDispatchReturnHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.SaleDispatchReturnHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.SaleDispatchReturnHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleDispatchReturnHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleDispatchReturnHeaders", "BuyerId", "Web.Buyers");
            DropForeignKey("Web.SaleDeliveryOrderSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleDeliveryOrderSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.SaleDeliveryOrderSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleDeliveryOrderSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleDeliveryOrderCancelLines", "SaleDeliveryOrderLineId", "Web.SaleDeliveryOrderLines");
            DropForeignKey("Web.SaleDeliveryOrderCancelLines", "SaleDeliveryOrderCancelHeaderId", "Web.SaleDeliveryOrderCancelHeaders");
            DropForeignKey("Web.SaleDeliveryOrderCancelLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.SaleDeliveryOrderCancelHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleDeliveryOrderCancelHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.SaleDeliveryOrderCancelHeaders", "OrderById", "Web.Employees");
            DropForeignKey("Web.SaleDeliveryOrderCancelHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleDeliveryOrderCancelHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleDeliveryOrderCancelHeaders", "BuyerId", "Web.Buyers");
            DropForeignKey("Web.RugStencils", "ProductSizeId", "Web.ProductSizes");
            DropForeignKey("Web.RugStencils", "ProductDesignId", "Web.ProductDesigns");
            DropForeignKey("Web.RugStencils", "StencilId", "Web.Products");
            DropForeignKey("Web.Rug_RetentionPercentage", "SiteId", "Web.Sites");
            DropForeignKey("Web.Rug_RetentionPercentage", "ProductCategoryId", "Web.Stocks");
            DropForeignKey("Web.Rug_RetentionPercentage", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.RouteLines", "RouteId", "Web.Routes");
            DropForeignKey("Web.RouteLines", "CityId", "Web.Cities");
            DropForeignKey("Web.RolesSites", "SiteId", "Web.Sites");
            DropForeignKey("Web.RolesSites", "RoleId", "Web.AspNetRoles");
            DropForeignKey("Web.RolesMenus", "SiteId", "Web.Sites");
            DropForeignKey("Web.RolesMenus", "RoleId", "Web.AspNetRoles");
            DropForeignKey("Web.RolesMenus", "MenuId", "Web.Menus");
            DropForeignKey("Web.RolesMenus", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.RolesDivisions", "RoleId", "Web.AspNetRoles");
            DropForeignKey("Web.RolesDivisions", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.RolesControllerActions", "RoleId", "Web.AspNetRoles");
            DropForeignKey("Web.RolesControllerActions", "ControllerActionId", "Web.ControllerActions");
            DropForeignKey("Web.AspNetUserRoles", "RoleId", "Web.AspNetRoles");
            DropForeignKey("Web.RequisitionSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.RequisitionSettings", "DefaultReasonId", "Web.Reasons");
            DropForeignKey("Web.RequisitionSettings", "OnSubmitMenuId", "Web.Menus");
            DropForeignKey("Web.RequisitionSettings", "OnApproveMenuId", "Web.Menus");
            DropForeignKey("Web.RequisitionSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.RequisitionSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.RequisitionLineStatus", "RequisitionLineId", "Web.RequisitionLines");
            DropForeignKey("Web.RequisitionCancelHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.RequisitionCancelLines", "RequisitionLineId", "Web.RequisitionLines");
            DropForeignKey("Web.RequisitionCancelLines", "RequisitionCancelHeaderId", "Web.RequisitionCancelHeaders");
            DropForeignKey("Web.RequisitionCancelHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.RequisitionCancelHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.RequisitionCancelHeaders", "PersonId", "Web.People");
            DropForeignKey("Web.RequisitionCancelHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.RequisitionCancelHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ReportColumns", "SubReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.ReportColumns", "SubReportId", "Web.SubReports");
            DropForeignKey("Web.SubReports", "ReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.ReportColumns", "ReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.ReportLines", "ReportHeaderId", "Web.ReportHeaders");
            DropForeignKey("Web.RateListProductRateGroups", "RateListHeaderId", "Web.RateListHeaders");
            DropForeignKey("Web.RateListProductRateGroups", "ProductRateGroupId", "Web.ProductRateGroups");
            DropForeignKey("Web.RateListPersonRateGroups", "RateListHeaderId", "Web.RateListHeaders");
            DropForeignKey("Web.RateListPersonRateGroups", "PersonRateGroupId", "Web.PersonRateGroups");
            DropForeignKey("Web.RateListLineHistories", "RateListHeaderId", "Web.RateListHeaders");
            DropForeignKey("Web.RateListLineHistories", "ProductRateGroupId", "Web.ProductRateGroups");
            DropForeignKey("Web.RateListLineHistories", "ProductId", "Web.Products");
            DropForeignKey("Web.RateListLineHistories", "PersonRateGroupId", "Web.PersonRateGroups");
            DropForeignKey("Web.RateListLineHistories", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.RateListLineHistories", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.RateListLines", "RateListHeaderId", "Web.RateListHeaders");
            DropForeignKey("Web.RateListLines", "ProductRateGroupId", "Web.ProductRateGroups");
            DropForeignKey("Web.RateListLines", "ProductId", "Web.Products");
            DropForeignKey("Web.RateListLines", "PersonRateGroupId", "Web.PersonRateGroups");
            DropForeignKey("Web.RateListLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.RateListLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.RateListHistories", "ProductId", "Web.Products");
            DropForeignKey("Web.RateListHistories", "ProcessId", "Web.Processes");
            DropForeignKey("Web.RateListHistories", "PersonRateGroupId", "Web.PersonRateGroups");
            DropForeignKey("Web.RateListHistories", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.RateListHistories", "DealUnitId", "Web.Units");
            DropForeignKey("Web.RateListHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.RateListHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.RateListHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.RateListHeaders", "DealUnitId", "Web.Units");
            DropForeignKey("Web.RateLists", "ProductId", "Web.Products");
            DropForeignKey("Web.RateLists", "ProcessId", "Web.Processes");
            DropForeignKey("Web.RateLists", "PersonRateGroupId", "Web.PersonRateGroups");
            DropForeignKey("Web.RateLists", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.RateLists", "DealUnitId", "Web.Units");
            DropForeignKey("Web.RateConversionSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.RateConversionSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.RateConversionSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.RateConversionSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseQuotationSettings", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.PurchaseQuotationSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseQuotationSettings", "OnSubmitMenuId", "Web.Menus");
            DropForeignKey("Web.PurchaseQuotationSettings", "OnApproveMenuId", "Web.Menus");
            DropForeignKey("Web.PurchaseQuotationSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseQuotationSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseQuotationSettings", "CalculationId", "Web.Calculations");
            DropForeignKey("Web.PurchaseQuotationLineCharges", "LineTableId", "Web.PurchaseQuotationLines");
            DropForeignKey("Web.PurchaseQuotationLineCharges", "PersonID", "Web.People");
            DropForeignKey("Web.PurchaseQuotationLineCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseQuotationLineCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseQuotationLineCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseQuotationLineCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.PurchaseQuotationLineCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseQuotationLineCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.PurchaseQuotationLineCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseQuotationLineCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.PurchaseQuotationHeaderCharges", "HeaderTableId", "Web.PurchaseQuotationHeaders");
            DropForeignKey("Web.PurchaseQuotationHeaderCharges", "ProductChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseQuotationHeaderCharges", "PersonID", "Web.People");
            DropForeignKey("Web.PurchaseQuotationHeaderCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseQuotationHeaderCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseQuotationHeaderCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseQuotationHeaderCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.PurchaseQuotationHeaderCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseQuotationHeaderCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.PurchaseQuotationHeaderCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseQuotationHeaderCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.PurchaseQuotationHeaders", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.PurchaseQuotationHeaders", "SupplierId", "Web.Suppliers");
            DropForeignKey("Web.PurchaseQuotationHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseQuotationHeaders", "ShipMethodId", "Web.ShipMethods");
            DropForeignKey("Web.PurchaseQuotationHeaders", "SalesTaxGroupId", "Web.ChargeGroupPersons");
            DropForeignKey("Web.PurchaseQuotationLines", "SalesTaxGroupId", "Web.SalesTaxGroups");
            DropForeignKey("Web.PurchaseQuotationLines", "PurchaseQuotationHeaderId", "Web.PurchaseQuotationHeaders");
            DropForeignKey("Web.PurchaseQuotationLines", "PurchaseIndentLineId", "Web.PurchaseIndentLines");
            DropForeignKey("Web.PurchaseQuotationLines", "ProductId", "Web.Products");
            DropForeignKey("Web.PurchaseQuotationLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.PurchaseQuotationLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.PurchaseQuotationLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.PurchaseQuotationHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseQuotationHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseQuotationHeaders", "DeliveryTermsId", "Web.DeliveryTerms");
            DropForeignKey("Web.PurchaseQuotationHeaders", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.PurchaseQuotationHeaders", "BillingAccountId", "Web.Suppliers");
            DropForeignKey("Web.PurchaseOrderSettings", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.PurchaseOrderSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseOrderSettings", "OnSubmitMenuId", "Web.Menus");
            DropForeignKey("Web.PurchaseOrderSettings", "OnApproveMenuId", "Web.Menus");
            DropForeignKey("Web.PurchaseOrderSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseOrderSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseOrderSettings", "CalculationId", "Web.Calculations");
            DropForeignKey("Web.PurchaseOrderRateAmendmentLineCharges", "LineTableId", "Web.PurchaseOrderRateAmendmentLines");
            DropForeignKey("Web.PurchaseOrderRateAmendmentLineCharges", "PersonID", "Web.People");
            DropForeignKey("Web.PurchaseOrderRateAmendmentLineCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseOrderRateAmendmentLineCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseOrderRateAmendmentLineCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseOrderRateAmendmentLineCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.PurchaseOrderRateAmendmentLineCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseOrderRateAmendmentLineCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.PurchaseOrderRateAmendmentLineCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseOrderRateAmendmentLineCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.PurchaseOrderRateAmendmentLines", "PurchaseOrderLineId", "Web.PurchaseOrderLines");
            DropForeignKey("Web.PurchaseOrderRateAmendmentLines", "PurchaseOrderAmendmentHeaderId", "Web.PurchaseOrderAmendmentHeaders");
            DropForeignKey("Web.PurchaseOrderQtyAmendmentLines", "PurchaseOrderLineId", "Web.PurchaseOrderLines");
            DropForeignKey("Web.PurchaseOrderQtyAmendmentLines", "PurchaseOrderAmendmentHeaderId", "Web.PurchaseOrderAmendmentHeaders");
            DropForeignKey("Web.PurchaseOrderLineStatus", "PurchaseOrderLineId", "Web.PurchaseOrderLines");
            DropForeignKey("Web.PurchaseOrderLineCharges", "LineTableId", "Web.PurchaseOrderLines");
            DropForeignKey("Web.PurchaseOrderLineCharges", "PersonID", "Web.People");
            DropForeignKey("Web.PurchaseOrderLineCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseOrderLineCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseOrderLineCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseOrderLineCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.PurchaseOrderLineCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseOrderLineCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.PurchaseOrderLineCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseOrderLineCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.PurchaseOrderInspectionSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseOrderInspectionSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.PurchaseOrderInspectionSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.PurchaseOrderInspectionSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseOrderInspectionSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseOrderInspectionRequestSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseOrderInspectionRequestSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.PurchaseOrderInspectionRequestSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.PurchaseOrderInspectionRequestSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseOrderInspectionRequestSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseOrderInspectionRequestCancelLines", "PurchaseOrderInspectionRequestLineId", "Web.PurchaseOrderInspectionRequestLines");
            DropForeignKey("Web.PurchaseOrderInspectionRequestCancelLines", "PurchaseOrderInspectionRequestCancelHeaderId", "Web.PurchaseOrderInspectionRequestCancelHeaders");
            DropForeignKey("Web.PurchaseOrderInspectionRequestCancelLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.PurchaseOrderInspectionRequestCancelHeaders", "SupplierId", "Web.Suppliers");
            DropForeignKey("Web.PurchaseOrderInspectionRequestCancelHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseOrderInspectionRequestCancelHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.PurchaseOrderInspectionRequestCancelHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.PurchaseOrderInspectionRequestCancelHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseOrderInspectionRequestCancelHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseOrderInspectionLines", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseOrderInspectionLines", "PurchaseOrderLineId", "Web.PurchaseOrderLines");
            DropForeignKey("Web.PurchaseOrderInspectionLines", "PurchaseOrderInspectionRequestLineId", "Web.PurchaseOrderInspectionRequestLines");
            DropForeignKey("Web.PurchaseOrderInspectionRequestLines", "PurchaseOrderLineId", "Web.PurchaseOrderLines");
            DropForeignKey("Web.PurchaseOrderInspectionRequestLines", "PurchaseOrderInspectionRequestHeaderId", "Web.PurchaseOrderInspectionRequestHeaders");
            DropForeignKey("Web.PurchaseOrderInspectionRequestHeaders", "SupplierId", "Web.Suppliers");
            DropForeignKey("Web.PurchaseOrderInspectionRequestHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseOrderInspectionRequestHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.PurchaseOrderInspectionRequestHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseOrderInspectionRequestHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseOrderInspectionRequestLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.PurchaseOrderInspectionLines", "PurchaseOrderInspectionHeaderId", "Web.PurchaseOrderInspectionHeaders");
            DropForeignKey("Web.PurchaseOrderInspectionLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.PurchaseOrderInspectionHeaders", "SupplierId", "Web.Suppliers");
            DropForeignKey("Web.PurchaseOrderInspectionHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseOrderInspectionHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseOrderInspectionHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.PurchaseOrderInspectionHeaders", "InspectionById", "Web.Employees");
            DropForeignKey("Web.PurchaseOrderInspectionHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseOrderInspectionHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseOrderHeaderStatus", "PurchaseOrderHeaderId", "Web.PurchaseOrderHeaders");
            DropForeignKey("Web.PurchaseOrderHeaderCharges", "HeaderTableId", "Web.PurchaseOrderHeaders");
            DropForeignKey("Web.PurchaseOrderHeaderCharges", "ProductChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseOrderHeaderCharges", "PersonID", "Web.People");
            DropForeignKey("Web.PurchaseOrderHeaderCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseOrderHeaderCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseOrderHeaderCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseOrderHeaderCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.PurchaseOrderHeaderCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseOrderHeaderCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.PurchaseOrderHeaderCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseOrderHeaderCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaderCharges", "HeaderTableId", "Web.PurchaseOrderAmendmentHeaders");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaderCharges", "ProductChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaderCharges", "PersonID", "Web.People");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaderCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaderCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaderCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaderCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaderCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaderCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaderCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaderCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaders", "SupplierId", "Web.Suppliers");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseOrderAmendmentHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseInvoiceSettings", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.PurchaseInvoiceSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseInvoiceSettings", "PurchaseGoodsReceiptDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseInvoiceSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.PurchaseInvoiceSettings", "DocTypeGoodsReturnId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseInvoiceSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseInvoiceSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseInvoiceSettings", "CalculationId", "Web.Calculations");
            DropForeignKey("Web.PurchaseInvoiceReturnLineCharges", "LineTableId", "Web.PurchaseInvoiceReturnLines");
            DropForeignKey("Web.PurchaseInvoiceReturnLineCharges", "PersonID", "Web.People");
            DropForeignKey("Web.PurchaseInvoiceReturnLineCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseInvoiceReturnLineCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseInvoiceReturnLineCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseInvoiceReturnLineCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.PurchaseInvoiceReturnLineCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseInvoiceReturnLineCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.PurchaseInvoiceReturnLineCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseInvoiceReturnLineCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.PurchaseInvoiceReturnLines", "PurchaseInvoiceReturnHeaderId", "Web.PurchaseInvoiceReturnHeaders");
            DropForeignKey("Web.PurchaseInvoiceReturnLines", "PurchaseInvoiceLineId", "Web.PurchaseInvoiceLines");
            DropForeignKey("Web.PurchaseInvoiceReturnLines", "PurchaseGoodsReturnLineId", "Web.PurchaseGoodsReturnLines");
            DropForeignKey("Web.PurchaseInvoiceReturnLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaderCharges", "HeaderTableId", "Web.PurchaseInvoiceReturnHeaders");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaderCharges", "ProductChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaderCharges", "PersonID", "Web.People");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaderCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaderCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaderCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaderCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaderCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaderCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaderCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaderCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaders", "SupplierId", "Web.Suppliers");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaders", "SalesTaxGroupPartyId", "Web.SalesTaxGroupParties");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaders", "SalesTaxGroupId", "Web.ChargeGroupPersons");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaders", "PurchaseGoodsReturnHeaderId", "Web.PurchaseGoodsReturnHeaders");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaders", "LedgerHeaderId", "Web.LedgerHeaders");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseInvoiceReturnHeaders", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.PurchaseInvoiceRateAmendmentLines", "PurchaseInvoiceLineId", "Web.PurchaseInvoiceLines");
            DropForeignKey("Web.PurchaseInvoiceRateAmendmentLines", "PurchaseInvoiceAmendmentHeaderId", "Web.PurchaseInvoiceAmendmentHeaders");
            DropForeignKey("Web.PurchaseInvoiceLineCharges", "LineTableId", "Web.PurchaseInvoiceLines");
            DropForeignKey("Web.PurchaseInvoiceLineCharges", "PersonID", "Web.People");
            DropForeignKey("Web.PurchaseInvoiceLineCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseInvoiceLineCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseInvoiceLineCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseInvoiceLineCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.PurchaseInvoiceLineCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseInvoiceLineCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.PurchaseInvoiceLineCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseInvoiceLineCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.PurchaseInvoiceHeaderCharges", "HeaderTableId", "Web.PurchaseInvoiceHeaders");
            DropForeignKey("Web.PurchaseInvoiceHeaderCharges", "ProductChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseInvoiceHeaderCharges", "PersonID", "Web.People");
            DropForeignKey("Web.PurchaseInvoiceHeaderCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseInvoiceHeaderCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseInvoiceHeaderCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseInvoiceHeaderCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.PurchaseInvoiceHeaderCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.PurchaseInvoiceHeaderCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.PurchaseInvoiceHeaderCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.PurchaseInvoiceHeaderCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.PurchaseInvoiceHeaders", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.PurchaseInvoiceHeaders", "SupplierId", "Web.Suppliers");
            DropForeignKey("Web.PurchaseInvoiceHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseInvoiceHeaders", "ShipMethodId", "Web.ShipMethods");
            DropForeignKey("Web.PurchaseInvoiceHeaders", "SalesTaxGroupPartyId", "Web.SalesTaxGroupParties");
            DropForeignKey("Web.PurchaseInvoiceHeaders", "SalesTaxGroupId", "Web.ChargeGroupPersons");
            DropForeignKey("Web.PurchaseInvoiceHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseInvoiceLines", "SalesTaxGroupId", "Web.SalesTaxGroups");
            DropForeignKey("Web.PurchaseInvoiceLines", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseInvoiceLines", "PurchaseInvoiceHeaderId", "Web.PurchaseInvoiceHeaders");
            DropForeignKey("Web.PurchaseInvoiceLines", "PurchaseGoodsReceiptLineId", "Web.PurchaseGoodsReceiptLines");
            DropForeignKey("Web.PurchaseInvoiceLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.PurchaseInvoiceHeaders", "PurchaseGoodsReceiptHeaderId", "Web.PurchaseGoodsReceiptHeaders");
            DropForeignKey("Web.PurchaseInvoiceHeaders", "LedgerHeaderId", "Web.LedgerHeaders");
            DropForeignKey("Web.PurchaseInvoiceHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseInvoiceHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseInvoiceHeaders", "DeliveryTermsId", "Web.DeliveryTerms");
            DropForeignKey("Web.PurchaseInvoiceHeaders", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.PurchaseInvoiceHeaders", "BillingAccountId", "Web.Suppliers");
            DropForeignKey("Web.PurchaseInvoiceAmendmentHeaders", "SupplierId", "Web.Suppliers");
            DropForeignKey("Web.PurchaseInvoiceAmendmentHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseInvoiceAmendmentHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseInvoiceAmendmentHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseIndentSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseIndentSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseIndentSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseIndentLineStatus", "PurchaseIndentLineId", "Web.PurchaseIndentLines");
            DropForeignKey("Web.PurchaseGoodsReturnLines", "StockId", "Web.Stocks");
            DropForeignKey("Web.PurchaseGoodsReturnLines", "PurchaseGoodsReturnHeaderId", "Web.PurchaseGoodsReturnHeaders");
            DropForeignKey("Web.PurchaseGoodsReturnLines", "PurchaseGoodsReceiptLineId", "Web.PurchaseGoodsReceiptLines");
            DropForeignKey("Web.PurchaseGoodsReturnLines", "ProductUidLastTransactionPersonId", "Web.People");
            DropForeignKey("Web.PurchaseGoodsReturnLines", "ProductUidLastTransactionDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseGoodsReturnLines", "ProductUidCurrentProcessId", "Web.Processes");
            DropForeignKey("Web.PurchaseGoodsReturnLines", "ProductUidCurrentGodownId", "Web.Godowns");
            DropForeignKey("Web.PurchaseGoodsReturnLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.PurchaseGoodsReturnHeaders", "SupplierId", "Web.Suppliers");
            DropForeignKey("Web.PurchaseGoodsReturnHeaders", "StockHeaderId", "Web.StockHeaders");
            DropForeignKey("Web.PurchaseGoodsReturnHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseGoodsReturnHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.PurchaseGoodsReturnHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.PurchaseGoodsReturnHeaders", "GatePassHeaderId", "Web.GatePassHeaders");
            DropForeignKey("Web.PurchaseGoodsReturnHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseGoodsReturnHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseGoodsReceiptSettings", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.PurchaseGoodsReceiptSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseGoodsReceiptSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.PurchaseGoodsReceiptSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseGoodsReceiptSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ProductUidSiteDetails", "ProductUidHeaderId", "Web.ProductUidHeaders");
            DropForeignKey("Web.ProductUidSiteDetails", "ProductId", "Web.Products");
            DropForeignKey("Web.ProductUidSiteDetails", "LastTransactionPersonId", "Web.People");
            DropForeignKey("Web.ProductUidSiteDetails", "LastTransactionDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProductUidSiteDetails", "GenPersonId", "Web.Buyers");
            DropForeignKey("Web.ProductUidSiteDetails", "GenDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProductUidSiteDetails", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ProductUidSiteDetails", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ProductUidSiteDetails", "CurrenctProcessId", "Web.Processes");
            DropForeignKey("Web.ProductUidSiteDetails", "CurrenctGodownId", "Web.Godowns");
            DropForeignKey("Web.ProductTags", "TagId", "Web.Tags");
            DropForeignKey("Web.ProductTags", "ProductId", "Web.Products");
            DropForeignKey("Web.ProductSiteDetails", "SiteId", "Web.Sites");
            DropForeignKey("Web.ProductSiteDetails", "ProductId", "Web.Products");
            DropForeignKey("Web.ProductSiteDetails", "GodownId", "Web.Godowns");
            DropForeignKey("Web.ProductSiteDetails", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.Sizes", "ProductShapeId", "Web.ProductShapes");
            DropForeignKey("Web.ProductProcesses", "ProductRateGroupId", "Web.ProductRateGroups");
            DropForeignKey("Web.ProductProcesses", "ProductId", "Web.Products");
            DropForeignKey("Web.ProductProcesses", "ProcessId", "Web.Processes");
            DropForeignKey("Web.ProductProcesses", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ProductProcesses", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ProductionOrderSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.ProductionOrderSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProductionOrderSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ProductCustomGroupLines", "ProductCustomGroupHeaderId", "Web.ProductCustomGroupHeaders");
            DropForeignKey("Web.ProductCustomGroupLines", "ProductId", "Web.Products");
            DropForeignKey("Web.ProductAlias", "ProductId", "Web.Products");
            DropForeignKey("Web.ProductAlias", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProdOrderSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.ProdOrderSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProdOrderSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ProdOrderLineStatus", "ProdOrderLineId", "Web.ProdOrderLines");
            DropForeignKey("Web.ProdOrderHeaderStatus", "ProdOrderHeaderId", "Web.ProdOrderHeaders");
            DropForeignKey("Web.ProdOrderCancelLines", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProdOrderCancelLines", "ProdOrderLineId", "Web.ProdOrderLines");
            DropForeignKey("Web.ProdOrderCancelLines", "ProdOrderCancelHeaderId", "Web.ProdOrderCancelHeaders");
            DropForeignKey("Web.ProdOrderCancelHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.ProdOrderCancelHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProdOrderCancelHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProdOrderCancelHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ProcessSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.ProcessSettings", "RateListMenuId", "Web.Menus");
            DropForeignKey("Web.ProcessSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.ProcessSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ProcessSequenceLines", "ProductRateGroupId", "Web.ProductRateGroups");
            DropForeignKey("Web.ProductRateGroups", "SiteId", "Web.Sites");
            DropForeignKey("Web.ProductRateGroups", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ProcessSequenceLines", "ProcessSequenceHeaderId", "Web.ProcessSequenceHeaders");
            DropForeignKey("Web.ProcessSequenceLines", "ProcessId", "Web.Processes");
            DropForeignKey("Web.PersonRegistrations", "PersonId", "Web.People");
            DropForeignKey("Web.PersonProcesses", "ProcessId", "Web.Processes");
            DropForeignKey("Web.PersonProcesses", "PersonRateGroupId", "Web.PersonRateGroups");
            DropForeignKey("Web.PersonProcesses", "PersonId", "Web.People");
            DropForeignKey("Web.PersonDocuments", "PersonId", "Web.People");
            DropForeignKey("Web.PersonCustomGroupLines", "PersonCustomGroupHeaderId", "Web.PersonCustomGroupHeaders");
            DropForeignKey("Web.PersonCustomGroupLines", "PersonId", "Web.People");
            DropForeignKey("Web.PersonBankAccounts", "PersonId", "Web.People");
            DropForeignKey("Web.PerkDocumentTypes", "SiteId", "Web.Sites");
            DropForeignKey("Web.PerkDocumentTypes", "RateDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PerkDocumentTypes", "PerkId", "Web.Perks");
            DropForeignKey("Web.PerkDocumentTypes", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PerkDocumentTypes", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PackingSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.PackingSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.PackingSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PackingSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.MaterialRequestSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.MaterialRequestSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.MaterialRequestSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.MaterialReceiveSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.MaterialReceiveSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.MaterialReceiveSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.MaterialReceiveSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.MaterialPlanSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.MaterialPlanSettings", "GodownId", "Web.Godowns");
            DropForeignKey("Web.MaterialPlanSettings", "DocTypePurchaseIndentId", "Web.DocumentTypes");
            DropForeignKey("Web.MaterialPlanSettings", "DocTypeProductionOrderId", "Web.DocumentTypes");
            DropForeignKey("Web.MaterialPlanSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.MaterialPlanSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.MaterialPlanForSaleOrders", "SaleOrderLineId", "Web.SaleOrderLines");
            DropForeignKey("Web.MaterialPlanForSaleOrders", "MaterialPlanLineId", "Web.MaterialPlanLines");
            DropForeignKey("Web.MaterialPlanForSaleOrders", "MaterialPlanHeaderId", "Web.MaterialPlanHeaders");
            DropForeignKey("Web.MaterialPlanForSaleOrderLines", "ProductId", "Web.Products");
            DropForeignKey("Web.MaterialPlanForSaleOrderLines", "MaterialPlanLineId", "Web.MaterialPlanLines");
            DropForeignKey("Web.MaterialPlanForSaleOrderLines", "MaterialPlanForSaleOrderId", "Web.MaterialPlanForSaleOrders");
            DropForeignKey("Web.MaterialPlanForSaleOrderLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.MaterialPlanForSaleOrderLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.MaterialPlanForProdOrders", "ProdOrderLineId", "Web.ProdOrderLines");
            DropForeignKey("Web.MaterialPlanForProdOrders", "MaterialPlanHeaderId", "Web.MaterialPlanHeaders");
            DropForeignKey("Web.MaterialPlanForProdOrderLines", "ProductId", "Web.Products");
            DropForeignKey("Web.MaterialPlanForProdOrderLines", "ProcessId", "Web.Processes");
            DropForeignKey("Web.MaterialPlanForProdOrderLines", "MaterialPlanLineId", "Web.MaterialPlanLines");
            DropForeignKey("Web.MaterialPlanForProdOrderLines", "MaterialPlanForProdOrderId", "Web.MaterialPlanForProdOrders");
            DropForeignKey("Web.MaterialPlanForProdOrderLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.MaterialPlanForProdOrderLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.MaterialPlanForJobOrders", "MaterialPlanLineId", "Web.MaterialPlanLines");
            DropForeignKey("Web.MaterialPlanForJobOrders", "MaterialPlanHeaderId", "Web.MaterialPlanHeaders");
            DropForeignKey("Web.MaterialPlanForJobOrderLines", "ProductId", "Web.Products");
            DropForeignKey("Web.MaterialPlanForJobOrderLines", "MaterialPlanLineId", "Web.MaterialPlanLines");
            DropForeignKey("Web.MaterialPlanForJobOrderLines", "MaterialPlanForJobOrderId", "Web.MaterialPlanForJobOrders");
            DropForeignKey("Web.MaterialPlanForJobOrderLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.MaterialPlanForJobOrderLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.MaterialPlanForJobOrders", "JobOrderLineId", "Web.JobOrderLines");
            DropForeignKey("Web.StockHeaderSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.StockHeaderSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.StockHeaderSettings", "OnSubmitMenuId", "Web.Menus");
            DropForeignKey("Web.StockHeaderSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.StockHeaderSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.StockHeaderSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.Manufacturers", "PersonID", "Web.People");
            DropForeignKey("Web.LedgerSettings", "WizardMenuId", "Web.Menus");
            DropForeignKey("Web.LedgerSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.LedgerSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.LedgerSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.LedgerSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.LedgerLineRefValues", "LedgerLineId", "Web.LedgerLines");
            DropForeignKey("Web.LedgerAdjs", "SiteId", "Web.Sites");
            DropForeignKey("Web.LedgerAdjs", "LedgerId", "Web.Ledgers");
            DropForeignKey("Web.LedgerAdjs", "DrLedgerId", "Web.Ledgers");
            DropForeignKey("Web.LedgerAdjs", "CrLedgerId", "Web.Ledgers");
            DropForeignKey("Web.Ledgers", "LedgerLineId", "Web.LedgerLines");
            DropForeignKey("Web.LedgerLines", "LedgerHeaderId", "Web.LedgerHeaders");
            DropForeignKey("Web.LedgerLines", "LedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.LedgerLines", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.Ledgers", "LedgerHeaderId", "Web.LedgerHeaders");
            DropForeignKey("Web.Ledgers", "LedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.Ledgers", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.Ledgers", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.LeaveTypes", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobReturnBoms", "StockProcessId", "Web.StockProcesses");
            DropForeignKey("Web.JobReturnBoms", "ProductId", "Web.Products");
            DropForeignKey("Web.JobReturnBoms", "JobReturnLineId", "Web.JobReturnLines");
            DropForeignKey("Web.JobReturnBoms", "JobReturnHeaderId", "Web.JobReturnHeaders");
            DropForeignKey("Web.JobReturnBoms", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.JobReturnBoms", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.JobReturnBoms", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.JobReceiveSettings", "WizardMenuId", "Web.Menus");
            DropForeignKey("Web.JobReceiveSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobReceiveSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobReceiveSettings", "OnSubmitMenuId", "Web.Menus");
            DropForeignKey("Web.JobReceiveSettings", "OnApproveMenuId", "Web.Menus");
            DropForeignKey("Web.JobReceiveSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.JobReceiveSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobReceiveSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobReceiveSettings", "CalculationId", "Web.Calculations");
            DropForeignKey("Web.JobReceiveQASettings", "WizardMenuId", "Web.Menus");
            DropForeignKey("Web.JobReceiveQASettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobReceiveQASettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobReceiveQASettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.JobReceiveQASettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobReceiveQASettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobReceiveQAHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobReceiveQAHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobReceiveQAHeaders", "QAById", "Web.Employees");
            DropForeignKey("Web.JobReceiveQAHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobReceiveQAHeaders", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobReceiveQALines", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobReceiveQALines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.JobReceiveQALines", "JobReceiveQAHeaderId", "Web.JobReceiveQAHeaders");
            DropForeignKey("Web.JobReceiveQALines", "JobReceiveLineId", "Web.JobReceiveLines");
            DropForeignKey("Web.JobReceiveQAHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobReceiveQAHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobReceiveLineStatus", "JobReceiveLineId", "Web.JobReceiveLines");
            DropForeignKey("Web.JobReceiveByProducts", "ProductId", "Web.Products");
            DropForeignKey("Web.JobReceiveByProducts", "JobReceiveHeaderId", "Web.JobReceiveHeaders");
            DropForeignKey("Web.JobReceiveByProducts", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.JobReceiveByProducts", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.JobReceiveBoms", "StockProcessId", "Web.StockProcesses");
            DropForeignKey("Web.JobReceiveBoms", "ProductId", "Web.Products");
            DropForeignKey("Web.JobReceiveBoms", "JobReceiveLineId", "Web.JobReceiveLines");
            DropForeignKey("Web.JobReceiveBoms", "JobReceiveHeaderId", "Web.JobReceiveHeaders");
            DropForeignKey("Web.JobReceiveBoms", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.JobReceiveBoms", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.JobReceiveBoms", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.JobOrderSettings", "WizardMenuId", "Web.Menus");
            DropForeignKey("Web.JobOrderSettings", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.JobOrderSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobOrderSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobOrderSettings", "OnSubmitMenuId", "Web.Menus");
            DropForeignKey("Web.JobOrderSettings", "OnApproveMenuId", "Web.Menus");
            DropForeignKey("Web.JobOrderSettings", "JobUnitId", "Web.Units");
            DropForeignKey("Web.JobOrderSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.JobOrderSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobOrderSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobOrderSettings", "DealUnitId", "Web.Units");
            DropForeignKey("Web.JobOrderSettings", "CalculationId", "Web.Calculations");
            DropForeignKey("Web.JobOrderRateAmendmentLines", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobOrderRateAmendmentLines", "JobOrderLineId", "Web.JobOrderLines");
            DropForeignKey("Web.JobOrderRateAmendmentLines", "JobOrderAmendmentHeaderId", "Web.JobOrderAmendmentHeaders");
            DropForeignKey("Web.JobOrderQtyAmendmentLines", "JobOrderLineId", "Web.JobOrderLines");
            DropForeignKey("Web.JobOrderQtyAmendmentLines", "JobOrderAmendmentHeaderId", "Web.JobOrderAmendmentHeaders");
            DropForeignKey("Web.JobOrderPerks", "PerkId", "Web.Perks");
            DropForeignKey("Web.JobOrderPerks", "JobOrderHeaderId", "Web.JobOrderHeaders");
            DropForeignKey("Web.JobOrderLineStatus", "JobOrderLineId", "Web.JobOrderLines");
            DropForeignKey("Web.JobOrderLineExtendeds", "OtherUnitId", "Web.Units");
            DropForeignKey("Web.JobOrderLineExtendeds", "JobOrderLineId", "Web.JobOrderLines");
            DropForeignKey("Web.JobOrderLineCharges", "PersonID", "Web.People");
            DropForeignKey("Web.JobOrderLineCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.JobOrderLineCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobOrderLineCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobOrderLineCharges", "LineTableId", "Web.JobOrderLines");
            DropForeignKey("Web.JobOrderLineCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.JobOrderLineCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobOrderLineCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.JobOrderLineCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.JobOrderLineCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.JobOrderJobOrders", "JobOrderHeaderId", "Web.JobOrderHeaders");
            DropForeignKey("Web.JobOrderJobOrders", "GenJobOrderHeaderId", "Web.JobOrderHeaders");
            DropForeignKey("Web.JobOrderInspectionSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobOrderInspectionSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobOrderInspectionSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.JobOrderInspectionSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobOrderInspectionSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobOrderInspectionRequestSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobOrderInspectionRequestSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobOrderInspectionRequestSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.JobOrderInspectionRequestSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobOrderInspectionRequestSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobOrderInspectionRequestCancelLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.JobOrderInspectionRequestCancelLines", "JobOrderInspectionRequestLineId", "Web.JobOrderInspectionRequestLines");
            DropForeignKey("Web.JobOrderInspectionRequestCancelLines", "JobOrderInspectionRequestCancelHeaderId", "Web.JobOrderInspectionRequestCancelHeaders");
            DropForeignKey("Web.JobOrderInspectionRequestCancelHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobOrderInspectionRequestCancelHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.JobOrderInspectionRequestCancelHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobOrderInspectionRequestCancelHeaders", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobOrderInspectionRequestCancelHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobOrderInspectionRequestCancelHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobOrderInspectionLines", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobOrderInspectionLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.JobOrderInspectionLines", "JobOrderLineId", "Web.JobOrderLines");
            DropForeignKey("Web.JobOrderInspectionLines", "JobOrderInspectionRequestLineId", "Web.JobOrderInspectionRequestLines");
            DropForeignKey("Web.JobOrderInspectionRequestLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.JobOrderInspectionRequestLines", "JobOrderLineId", "Web.JobOrderLines");
            DropForeignKey("Web.JobOrderInspectionRequestLines", "JobOrderInspectionRequestHeaderId", "Web.JobOrderInspectionRequestHeaders");
            DropForeignKey("Web.JobOrderInspectionRequestHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobOrderInspectionRequestHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobOrderInspectionRequestHeaders", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobOrderInspectionRequestHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobOrderInspectionRequestHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobOrderInspectionLines", "JobOrderInspectionHeaderId", "Web.JobOrderInspectionHeaders");
            DropForeignKey("Web.JobOrderInspectionHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobOrderInspectionHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobOrderInspectionHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobOrderInspectionHeaders", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobOrderInspectionHeaders", "InspectionById", "Web.Employees");
            DropForeignKey("Web.JobOrderInspectionHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobOrderInspectionHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobOrderHeaderStatus", "JobOrderHeaderId", "Web.JobOrderHeaders");
            DropForeignKey("Web.JobOrderHeaderCharges", "ProductChargeId", "Web.Charges");
            DropForeignKey("Web.JobOrderHeaderCharges", "PersonID", "Web.People");
            DropForeignKey("Web.JobOrderHeaderCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.JobOrderHeaderCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobOrderHeaderCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobOrderHeaderCharges", "HeaderTableId", "Web.JobOrderHeaders");
            DropForeignKey("Web.JobOrderHeaderCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.JobOrderHeaderCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobOrderHeaderCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.JobOrderHeaderCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.JobOrderHeaderCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.JobOrderCancelBoms", "ProductId", "Web.Products");
            DropForeignKey("Web.JobOrderCancelBoms", "JobOrderHeaderId", "Web.JobOrderHeaders");
            DropForeignKey("Web.JobOrderCancelBoms", "JobOrderCancelLineId", "Web.JobOrderCancelLines");
            DropForeignKey("Web.JobOrderCancelBoms", "JobOrderCancelHeaderId", "Web.JobOrderCancelHeaders");
            DropForeignKey("Web.JobOrderCancelHeaders", "StockHeaderId", "Web.StockHeaders");
            DropForeignKey("Web.JobOrderCancelHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobOrderCancelHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.JobOrderCancelHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobOrderCancelHeaders", "OrderById", "Web.Employees");
            DropForeignKey("Web.JobOrderCancelHeaders", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobOrderCancelLines", "JobOrderCancelHeaderId", "Web.JobOrderCancelHeaders");
            DropForeignKey("Web.JobOrderCancelLines", "StockProcessId", "Web.StockProcesses");
            DropForeignKey("Web.JobOrderCancelLines", "StockId", "Web.Stocks");
            DropForeignKey("Web.JobOrderCancelLines", "ProductUidLastTransactionPersonId", "Web.People");
            DropForeignKey("Web.JobOrderCancelLines", "ProductUidLastTransactionDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobOrderCancelLines", "ProductUidCurrentProcessId", "Web.Processes");
            DropForeignKey("Web.JobOrderCancelLines", "ProductUidCurrentGodownId", "Web.Godowns");
            DropForeignKey("Web.JobOrderCancelLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.JobOrderCancelLines", "JobOrderLineId", "Web.JobOrderLines");
            DropForeignKey("Web.JobOrderCancelHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.JobOrderCancelHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobOrderCancelHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobOrderCancelBoms", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.JobOrderCancelBoms", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.JobOrderByProducts", "ProductId", "Web.Products");
            DropForeignKey("Web.JobOrderByProducts", "JobOrderHeaderId", "Web.JobOrderHeaders");
            DropForeignKey("Web.JobOrderAmendmentHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobOrderAmendmentHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobOrderAmendmentHeaders", "OrderById", "Web.Employees");
            DropForeignKey("Web.JobOrderAmendmentHeaders", "LedgerHeaderId", "Web.LedgerHeaders");
            DropForeignKey("Web.JobOrderAmendmentHeaders", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobOrderAmendmentHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobOrderAmendmentHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobInvoiceSettings", "WizardMenuId", "Web.Menus");
            DropForeignKey("Web.JobInvoiceSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobInvoiceSettings", "JobReturnDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobInvoiceSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobInvoiceSettings", "JobReceiveDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobInvoiceSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.JobInvoiceSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobInvoiceSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobInvoiceSettings", "CalculationId", "Web.Calculations");
            DropForeignKey("Web.JobInvoiceReturnLineCharges", "PersonID", "Web.People");
            DropForeignKey("Web.JobInvoiceReturnLineCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceReturnLineCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceReturnLineCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceReturnLineCharges", "LineTableId", "Web.JobInvoiceReturnLines");
            DropForeignKey("Web.JobInvoiceReturnLineCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.JobInvoiceReturnLineCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceReturnLineCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.JobInvoiceReturnLineCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceReturnLineCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceReturnLines", "JobReturnLineId", "Web.JobReturnLines");
            DropForeignKey("Web.JobInvoiceReturnLines", "JobInvoiceReturnHeaderId", "Web.JobInvoiceReturnHeaders");
            DropForeignKey("Web.JobInvoiceReturnLines", "JobInvoiceLineId", "Web.JobInvoiceLines");
            DropForeignKey("Web.JobInvoiceReturnLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.JobInvoiceReturnHeaderCharges", "ProductChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceReturnHeaderCharges", "PersonID", "Web.People");
            DropForeignKey("Web.JobInvoiceReturnHeaderCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceReturnHeaderCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceReturnHeaderCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceReturnHeaderCharges", "HeaderTableId", "Web.JobInvoiceReturnHeaders");
            DropForeignKey("Web.JobInvoiceReturnHeaderCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.JobInvoiceReturnHeaderCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceReturnHeaderCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.JobInvoiceReturnHeaderCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceReturnHeaderCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceReturnHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobInvoiceReturnHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.JobInvoiceReturnHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobInvoiceReturnHeaders", "LedgerHeaderId", "Web.LedgerHeaders");
            DropForeignKey("Web.JobInvoiceReturnHeaders", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobInvoiceReturnHeaders", "JobReturnHeaderId", "Web.JobReturnHeaders");
            DropForeignKey("Web.JobReturnHeaders", "StockHeaderId", "Web.StockHeaders");
            DropForeignKey("Web.JobReturnHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobReturnHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobReturnHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.JobReturnHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobReturnHeaders", "OrderById", "Web.Employees");
            DropForeignKey("Web.JobReturnHeaders", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobReturnLines", "JobReturnHeaderId", "Web.JobReturnHeaders");
            DropForeignKey("Web.JobReturnLines", "StockProcessId", "Web.StockProcesses");
            DropForeignKey("Web.JobReturnLines", "StockId", "Web.Stocks");
            DropForeignKey("Web.JobReturnLines", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobReturnLines", "ProductUidLastTransactionPersonId", "Web.People");
            DropForeignKey("Web.JobReturnLines", "ProductUidLastTransactionDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobReturnLines", "ProductUidCurrentProcessId", "Web.Processes");
            DropForeignKey("Web.JobReturnLines", "ProductUidCurrentGodownId", "Web.Godowns");
            DropForeignKey("Web.JobReturnLines", "JobReceiveLineId", "Web.JobReceiveLines");
            DropForeignKey("Web.JobReturnLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.JobReturnHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.JobReturnHeaders", "GatePassHeaderId", "Web.GatePassHeaders");
            DropForeignKey("Web.JobReturnHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobReturnHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobInvoiceReturnHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobInvoiceReturnHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobInvoiceRateAmendmentLineCharges", "PersonID", "Web.People");
            DropForeignKey("Web.JobInvoiceRateAmendmentLineCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceRateAmendmentLineCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceRateAmendmentLineCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceRateAmendmentLineCharges", "LineTableId", "Web.JobInvoiceRateAmendmentLines");
            DropForeignKey("Web.JobInvoiceRateAmendmentLineCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.JobInvoiceRateAmendmentLineCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceRateAmendmentLineCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.JobInvoiceRateAmendmentLineCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceRateAmendmentLineCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceRateAmendmentLines", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobInvoiceRateAmendmentLines", "JobInvoiceLineId", "Web.JobInvoiceLines");
            DropForeignKey("Web.JobInvoiceRateAmendmentLines", "JobInvoiceAmendmentHeaderId", "Web.JobInvoiceAmendmentHeaders");
            DropForeignKey("Web.JobInvoiceLineStatus", "JobInvoiceLineId", "Web.JobInvoiceLines");
            DropForeignKey("Web.JobInvoiceLineCharges", "PersonID", "Web.People");
            DropForeignKey("Web.JobInvoiceLineCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceLineCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceLineCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceLineCharges", "LineTableId", "Web.JobInvoiceLines");
            DropForeignKey("Web.JobInvoiceLineCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.JobInvoiceLineCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceLineCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.JobInvoiceLineCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceLineCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceHeaderCharges", "ProductChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceHeaderCharges", "PersonID", "Web.People");
            DropForeignKey("Web.JobInvoiceHeaderCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceHeaderCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceHeaderCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceHeaderCharges", "HeaderTableId", "Web.JobInvoiceHeaders");
            DropForeignKey("Web.JobInvoiceHeaderCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.JobInvoiceHeaderCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceHeaderCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.JobInvoiceHeaderCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceHeaderCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobInvoiceHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobInvoiceHeaders", "LedgerHeaderId", "Web.LedgerHeaders");
            DropForeignKey("Web.JobInvoiceHeaders", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobInvoiceHeaders", "JobReceiveHeaderId", "Web.JobReceiveHeaders");
            DropForeignKey("Web.JobInvoiceLines", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobInvoiceLines", "JobReceiveLineId", "Web.JobReceiveLines");
            DropForeignKey("Web.JobReceiveLines", "StockProcessId", "Web.StockProcesses");
            DropForeignKey("Web.JobReceiveLines", "StockId", "Web.Stocks");
            DropForeignKey("Web.JobReceiveLines", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobReceiveLines", "ProductUidLastTransactionPersonId", "Web.People");
            DropForeignKey("Web.JobReceiveLines", "ProductUidLastTransactionDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobReceiveLines", "ProductUidCurrentProcessId", "Web.Processes");
            DropForeignKey("Web.JobReceiveLines", "ProductUidCurrentGodownId", "Web.Godowns");
            DropForeignKey("Web.JobReceiveLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.JobReceiveLines", "MachineId", "Web.Products");
            DropForeignKey("Web.JobReceiveLines", "JobReceiveHeaderId", "Web.JobReceiveHeaders");
            DropForeignKey("Web.JobReceiveHeaders", "StockHeaderId", "Web.StockHeaders");
            DropForeignKey("Web.JobReceiveHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobReceiveHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobReceiveHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobReceiveHeaders", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobReceiveHeaders", "JobReceiveById", "Web.Employees");
            DropForeignKey("Web.JobReceiveHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.JobReceiveHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobReceiveHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobReceiveLines", "JobOrderLineId", "Web.JobOrderLines");
            DropForeignKey("Web.JobReceiveLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.JobInvoiceLines", "JobInvoiceHeaderId", "Web.JobInvoiceHeaders");
            DropForeignKey("Web.JobInvoiceLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.JobInvoiceLines", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.JobInvoiceHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobInvoiceHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobInvoiceAmendmentHeaderCharges", "ProductChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceAmendmentHeaderCharges", "PersonID", "Web.People");
            DropForeignKey("Web.JobInvoiceAmendmentHeaderCharges", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceAmendmentHeaderCharges", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceAmendmentHeaderCharges", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceAmendmentHeaderCharges", "HeaderTableId", "Web.JobInvoiceAmendmentHeaders");
            DropForeignKey("Web.JobInvoiceAmendmentHeaderCharges", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.JobInvoiceAmendmentHeaderCharges", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.JobInvoiceAmendmentHeaderCharges", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.JobInvoiceAmendmentHeaderCharges", "ChargeId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceAmendmentHeaderCharges", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.JobInvoiceAmendmentHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobInvoiceAmendmentHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobInvoiceAmendmentHeaders", "OrderById", "Web.Employees");
            DropForeignKey("Web.JobInvoiceAmendmentHeaders", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobInvoiceAmendmentHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobInvoiceAmendmentHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobConsumptionSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobConsumptionSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobConsumptionSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobConsumptionSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.InspectionQaAttributes", "ProductTypeQaAttributeId", "Web.ProductTypeQaAttributes");
            DropForeignKey("Web.ProductTypeQaAttributes", "ProductTypeId", "Web.ProductTypes");
            DropForeignKey("Web.ExcessMaterialSettings", "SiteId", "Web.Sites");
            DropForeignKey("Web.ExcessMaterialSettings", "ProcessId", "Web.Processes");
            DropForeignKey("Web.ExcessMaterialSettings", "ImportMenuId", "Web.Menus");
            DropForeignKey("Web.ExcessMaterialSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ExcessMaterialSettings", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ExcessMaterialHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.ExcessMaterialHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.ExcessMaterialHeaders", "PersonId", "Web.People");
            DropForeignKey("Web.ExcessMaterialHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.ExcessMaterialLines", "ProductUidLastTransactionPersonId", "Web.People");
            DropForeignKey("Web.ExcessMaterialLines", "ProductUidLastTransactionDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ExcessMaterialLines", "ProductUidCurrentProcessId", "Web.Processes");
            DropForeignKey("Web.ExcessMaterialLines", "ProductUidCurrentGodownId", "Web.Godowns");
            DropForeignKey("Web.ExcessMaterialLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.ExcessMaterialLines", "ProductId", "Web.Products");
            DropForeignKey("Web.ExcessMaterialLines", "ProcessId", "Web.Processes");
            DropForeignKey("Web.ExcessMaterialLines", "ExcessMaterialHeaderId", "Web.ExcessMaterialHeaders");
            DropForeignKey("Web.ExcessMaterialLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ExcessMaterialLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ExcessMaterialHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ExcessMaterialHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ExcessMaterialHeaders", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.DocumentTypeTimePlans", "SiteId", "Web.Sites");
            DropForeignKey("Web.DocumentTypeTimePlans", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.DocumentTypeTimePlans", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.DocumentTypeTimeExtensions", "SiteId", "Web.Sites");
            DropForeignKey("Web.DocumentTypeTimeExtensions", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.DocumentTypeTimeExtensions", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.DocumentTypeSites", "SiteId", "Web.Sites");
            DropForeignKey("Web.DocumentTypeSites", "DocumentTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.DocumentTypeDivisions", "DocumentTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.DocumentTypeDivisions", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.DocSmsContents", "SiteId", "Web.Sites");
            DropForeignKey("Web.DocSmsContents", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.DocSmsContents", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.DocNotificationContents", "SiteId", "Web.Sites");
            DropForeignKey("Web.DocNotificationContents", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.DocNotificationContents", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.DocEmailContents", "SiteId", "Web.Sites");
            DropForeignKey("Web.DocEmailContents", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.DocEmailContents", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.DispatchWaybillLines", "DispatchWaybillHeaderId", "Web.DispatchWaybillHeaders");
            DropForeignKey("Web.DispatchWaybillLines", "CityId", "Web.Cities");
            DropForeignKey("Web.DispatchWaybillHeaders", "TransporterId", "Web.Transporters");
            DropForeignKey("Web.DispatchWaybillHeaders", "ToCityId", "Web.Cities");
            DropForeignKey("Web.DispatchWaybillHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.DispatchWaybillHeaders", "ShipMethodId", "Web.ShipMethods");
            DropForeignKey("Web.DispatchWaybillHeaders", "SaleInvoiceHeaderId", "Web.SaleInvoiceHeaders");
            DropForeignKey("Web.DispatchWaybillHeaders", "RouteId", "Web.Routes");
            DropForeignKey("Web.DispatchWaybillHeaders", "FromCityId", "Web.Cities");
            DropForeignKey("Web.DispatchWaybillHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.DispatchWaybillHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.DispatchWaybillHeaders", "ConsigneeId", "Web.Buyers");
            DropForeignKey("Web.CustomDetails", "TRCourierId", "Web.People");
            DropForeignKey("Web.CustomDetails", "SiteId", "Web.Sites");
            DropForeignKey("Web.CustomDetails", "SaleInvoiceHeaderId", "Web.SaleInvoiceHeaders");
            DropForeignKey("Web.SaleInvoiceHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleInvoiceHeaders", "SaleToBuyerId", "Web.People");
            DropForeignKey("Web.SaleInvoiceHeaders", "SaleDispatchHeaderId", "Web.SaleDispatchHeaders");
            DropForeignKey("Web.SaleDispatchHeaders", "StockHeaderId", "Web.StockHeaders");
            DropForeignKey("Web.SaleDispatchHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleDispatchHeaders", "ShipMethodId", "Web.ShipMethods");
            DropForeignKey("Web.SaleDispatchHeaders", "SaleToBuyerId", "Web.People");
            DropForeignKey("Web.SaleDispatchLines", "StockInId", "Web.Stocks");
            DropForeignKey("Web.SaleDispatchLines", "StockId", "Web.Stocks");
            DropForeignKey("Web.SaleInvoiceLines", "SalesTaxGroupId", "Web.SalesTaxGroups");
            DropForeignKey("Web.SaleInvoiceLines", "SaleOrderLineId", "Web.SaleOrderLines");
            DropForeignKey("Web.SaleInvoiceLines", "SaleInvoiceHeaderId", "Web.SaleInvoiceHeaders");
            DropForeignKey("Web.SaleInvoiceLines", "SaleDispatchLineId", "Web.SaleDispatchLines");
            DropForeignKey("Web.SaleInvoiceLines", "PromoCodeId", "Web.PromoCodes");
            DropForeignKey("Web.PromoCodes", "ProductTypeId", "Web.ProductTypes");
            DropForeignKey("Web.PromoCodes", "ProductGroupId", "Web.ProductGroups");
            DropForeignKey("Web.PromoCodes", "ProductCategoryId", "Web.ProductCategories");
            DropForeignKey("Web.PromoCodes", "ProductId", "Web.Products");
            DropForeignKey("Web.SaleInvoiceLines", "ProductInvoiceGroupId", "Web.ProductInvoiceGroups");
            DropForeignKey("Web.SaleInvoiceLines", "ProductId", "Web.Products");
            DropForeignKey("Web.SaleInvoiceLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.SaleInvoiceLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.SaleInvoiceLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.SaleDispatchLines", "SaleDispatchHeaderId", "Web.SaleDispatchHeaders");
            DropForeignKey("Web.SaleDispatchLines", "ProductUidLastTransactionPersonId", "Web.People");
            DropForeignKey("Web.SaleDispatchLines", "ProductUidLastTransactionDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleDispatchLines", "ProductUidCurrentProcessId", "Web.Processes");
            DropForeignKey("Web.SaleDispatchLines", "ProductUidCurrentGodownId", "Web.Godowns");
            DropForeignKey("Web.SaleDispatchLines", "PackingLineId", "Web.PackingLines");
            DropForeignKey("Web.SaleDispatchLines", "GodownId", "Web.Godowns");
            DropForeignKey("Web.SaleDispatchHeaders", "PackingHeaderId", "Web.PackingHeaders");
            DropForeignKey("Web.SaleDispatchHeaders", "GatePassHeaderId", "Web.GatePassHeaders");
            DropForeignKey("Web.SaleDispatchHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleDispatchHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleDispatchHeaders", "DeliveryTermsId", "Web.DeliveryTerms");
            DropForeignKey("Web.SaleInvoiceHeaders", "LedgerHeaderId", "Web.LedgerHeaders");
            DropForeignKey("Web.SaleInvoiceHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleInvoiceHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleInvoiceHeaders", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.SaleInvoiceHeaders", "BillToBuyerId", "Web.Buyers");
            DropForeignKey("Web.SaleInvoiceHeaders", "AgentId", "Web.People");
            DropForeignKey("Web.CustomDetails", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.CustomDetails", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.CurrencyConversions", "ToCurrencyId", "Web.Currencies");
            DropForeignKey("Web.CurrencyConversions", "FromCurrencyId", "Web.Currencies");
            DropForeignKey("Web.Couriers", "PersonID", "Web.People");
            DropForeignKey("Web.CostCenterStatusExtendeds", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.CostCenterStatus", "ProductId", "Web.Products");
            DropForeignKey("Web.CostCenterStatus", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.Companies", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.Companies", "CityId", "Web.Cities");
            DropForeignKey("Web.ChargeGroupProducts", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.CalculationLineLedgerAccounts", "SiteId", "Web.Sites");
            DropForeignKey("Web.CalculationLineLedgerAccounts", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.CalculationLineLedgerAccounts", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.CalculationLineLedgerAccounts", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.CalculationLineLedgerAccounts", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.CalculationLineLedgerAccounts", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.CalculationLineLedgerAccounts", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.CalculationLineLedgerAccounts", "CalculationProductId", "Web.CalculationProducts");
            DropForeignKey("Web.CalculationProducts", "PersonId", "Web.People");
            DropForeignKey("Web.CalculationProducts", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.CalculationProducts", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.CalculationProducts", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.CalculationProducts", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.CalculationProducts", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.CalculationProducts", "ChargeId", "Web.Charges");
            DropForeignKey("Web.CalculationProducts", "CalculationId", "Web.Calculations");
            DropForeignKey("Web.CalculationProducts", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.CalculationLineLedgerAccounts", "CalculationId", "Web.Calculations");
            DropForeignKey("Web.CalculationHeaderLedgerAccounts", "SiteId", "Web.Sites");
            DropForeignKey("Web.CalculationHeaderLedgerAccounts", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.CalculationHeaderLedgerAccounts", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.CalculationHeaderLedgerAccounts", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.CalculationHeaderLedgerAccounts", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.CalculationHeaderLedgerAccounts", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.CalculationHeaderLedgerAccounts", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.CalculationHeaderLedgerAccounts", "CalculationFooterId", "Web.CalculationFooters");
            DropForeignKey("Web.CalculationHeaderLedgerAccounts", "CalculationId", "Web.Calculations");
            DropForeignKey("Web.CalculationFooters", "ProductChargeId", "Web.Charges");
            DropForeignKey("Web.CalculationFooters", "PersonId", "Web.People");
            DropForeignKey("Web.CalculationFooters", "ParentChargeId", "Web.Charges");
            DropForeignKey("Web.CalculationFooters", "LedgerAccountDrId", "Web.LedgerAccounts");
            DropForeignKey("Web.CalculationFooters", "LedgerAccountCrId", "Web.LedgerAccounts");
            DropForeignKey("Web.CalculationFooters", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.CalculationFooters", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.CalculationFooters", "ChargeId", "Web.Charges");
            DropForeignKey("Web.CalculationFooters", "CalculationId", "Web.Calculations");
            DropForeignKey("Web.CalculationFooters", "CalculateOnId", "Web.Charges");
            DropForeignKey("Web.States", "Charge_ChargeId", "Web.Charges");
            DropForeignKey("Web.States", "Calculation_CalculationId", "Web.Calculations");
            DropForeignKey("Web.BusinessSessions", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.BusinessEntities", "TdsGroupId", "Web.TdsGroups");
            DropForeignKey("Web.BusinessEntities", "TdsCategoryId", "Web.TdsCategories");
            DropForeignKey("Web.BusinessEntities", "ServiceTaxCategoryId", "Web.ServiceTaxCategories");
            DropForeignKey("Web.BusinessEntities", "SalesTaxGroupPartyId", "Web.SalesTaxGroupParties");
            DropForeignKey("Web.BusinessEntities", "PersonRateGroupId", "Web.PersonRateGroups");
            DropForeignKey("Web.PersonRateGroups", "SiteId", "Web.Sites");
            DropForeignKey("Web.PersonRateGroups", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.BusinessEntities", "PersonID", "Web.People");
            DropForeignKey("Web.BusinessEntities", "ParentId", "Web.People");
            DropForeignKey("Web.BusinessEntities", "GuarantorId", "Web.People");
            DropForeignKey("Web.BusinessEntities", "Buyer_PersonID", "Web.Buyers");
            DropForeignKey("Web.BomDetails", "ProductId", "Web.Products");
            DropForeignKey("Web.BomDetails", "ProcessId", "Web.Processes");
            DropForeignKey("Web.BomDetails", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.BomDetails", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.BomDetails", "BaseProductId", "Web.Products");
            DropForeignKey("Web.ProductQualities", "ProductTypeId", "Web.ProductTypes");
            DropForeignKey("Web.ProductInvoiceGroups", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ProductInvoiceGroups", "DescriptionOfGoodsId", "Web.DescriptionOfGoods");
            DropForeignKey("Web.ProductDesignPatterns", "ProductTypeId", "Web.ProductTypes");
            DropForeignKey("Web.ProductCollections", "ProductTypeId", "Web.ProductTypes");
            DropForeignKey("Web.ProcessSequenceHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProductContentLines", "ProductGroupId", "Web.ProductGroups");
            DropForeignKey("Web.ProductContentLines", "ProductContentHeaderId", "Web.ProductContentHeaders");
            DropForeignKey("Web.Products", "UnitId", "Web.Units");
            DropForeignKey("Web.Products", "SalesTaxGroupProductId", "Web.SalesTaxGroupProducts");
            DropForeignKey("Web.PurchaseGoodsReceiptLines", "StockId", "Web.Stocks");
            DropForeignKey("Web.PurchaseGoodsReceiptLines", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseGoodsReceiptLines", "PurchaseOrderLineId", "Web.PurchaseOrderLines");
            DropForeignKey("Web.PurchaseGoodsReceiptLines", "PurchaseIndentLineId", "Web.PurchaseIndentLines");
            DropForeignKey("Web.PurchaseOrderLines", "SalesTaxGroupId", "Web.SalesTaxGroups");
            DropForeignKey("Web.SalesTaxGroups", "SalesTaxGroupProductId", "Web.SalesTaxGroupProducts");
            DropForeignKey("Web.SalesTaxGroups", "SalesTaxGroupPartyId", "Web.SalesTaxGroupParties");
            DropForeignKey("Web.PurchaseOrderLines", "PurchaseOrderHeaderId", "Web.PurchaseOrderHeaders");
            DropForeignKey("Web.PurchaseOrderHeaders", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.PurchaseOrderHeaders", "SupplierId", "Web.People");
            DropForeignKey("Web.PurchaseOrderHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseOrderHeaders", "ShipMethodId", "Web.ShipMethods");
            DropForeignKey("Web.PurchaseOrderHeaders", "SalesTaxGroupPersonId", "Web.ChargeGroupPersons");
            DropForeignKey("Web.ChargeGroupPersons", "ChargeTypeId", "Web.ChargeTypes");
            DropForeignKey("Web.PurchaseOrderHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseOrderHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseOrderHeaders", "DeliveryTermsId", "Web.DeliveryTerms");
            DropForeignKey("Web.PurchaseOrderHeaders", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.PurchaseOrderCancelLines", "PurchaseOrderLineId", "Web.PurchaseOrderLines");
            DropForeignKey("Web.PurchaseOrderCancelLines", "PurchaseOrderCancelHeaderId", "Web.PurchaseOrderCancelHeaders");
            DropForeignKey("Web.PurchaseOrderCancelHeaders", "SupplierId", "Web.Suppliers");
            DropForeignKey("Web.PurchaseOrderCancelHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseOrderCancelHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.PurchaseOrderCancelHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseOrderCancelHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseOrderLines", "PurchaseIndentLineId", "Web.PurchaseIndentLines");
            DropForeignKey("Web.PurchaseOrderLines", "ProductUidHeaderId", "Web.ProductUidHeaders");
            DropForeignKey("Web.PurchaseOrderLines", "ProductId", "Web.Products");
            DropForeignKey("Web.PurchaseOrderLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.PurchaseOrderLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.PurchaseOrderLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.PurchaseIndentLines", "PurchaseIndentHeaderId", "Web.PurchaseIndentHeaders");
            DropForeignKey("Web.PurchaseIndentHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseIndentHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.PurchaseIndentHeaders", "MaterialPlanHeaderId", "Web.MaterialPlanHeaders");
            DropForeignKey("Web.PurchaseIndentHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseIndentHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseIndentHeaders", "DepartmentId", "Web.Departments");
            DropForeignKey("Web.PurchaseIndentCancelLines", "PurchaseIndentLineId", "Web.PurchaseIndentLines");
            DropForeignKey("Web.PurchaseIndentCancelLines", "PurchaseIndentCancelHeaderId", "Web.PurchaseIndentCancelHeaders");
            DropForeignKey("Web.PurchaseIndentCancelHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseIndentCancelHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.PurchaseIndentCancelHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseIndentCancelHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseIndentLines", "ProductId", "Web.Products");
            DropForeignKey("Web.PurchaseIndentLines", "MaterialPlanLineId", "Web.MaterialPlanLines");
            DropForeignKey("Web.PurchaseIndentLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.PurchaseIndentLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.PurchaseGoodsReceiptLines", "PurchaseGoodsReceiptHeaderId", "Web.PurchaseGoodsReceiptHeaders");
            DropForeignKey("Web.PurchaseGoodsReceiptHeaders", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.PurchaseGoodsReceiptHeaders", "SupplierId", "Web.Suppliers");
            DropForeignKey("Web.PurchaseGoodsReceiptHeaders", "StockHeaderId", "Web.StockHeaders");
            DropForeignKey("Web.PurchaseGoodsReceiptHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseGoodsReceiptHeaders", "RoadPermitFormId", "Web.ProductUids");
            DropForeignKey("Web.PurchaseGoodsReceiptHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseGoodsReceiptHeaders", "PurchaseWaybillId", "Web.PurchaseWaybills");
            DropForeignKey("Web.PurchaseWaybills", "TransporterId", "Web.Transporters");
            DropForeignKey("Web.Transporters", "PersonID", "Web.People");
            DropForeignKey("Web.PurchaseWaybills", "ToCityId", "Web.Cities");
            DropForeignKey("Web.PurchaseWaybills", "SiteId", "Web.Sites");
            DropForeignKey("Web.PurchaseWaybills", "ShipMethodId", "Web.ShipMethods");
            DropForeignKey("Web.PurchaseWaybills", "FromCityId", "Web.Cities");
            DropForeignKey("Web.PurchaseWaybills", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseWaybills", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseWaybills", "ConsignerId", "Web.People");
            DropForeignKey("Web.PurchaseGoodsReceiptHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.PurchaseGoodsReceiptHeaders", "GateInId", "Web.GateIns");
            DropForeignKey("Web.GateIns", "SiteId", "Web.Sites");
            DropForeignKey("Web.GateIns", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.GateIns", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseGoodsReceiptHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseGoodsReceiptHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PurchaseGoodsReceiptLines", "ProductUidLastTransactionPersonId", "Web.People");
            DropForeignKey("Web.PurchaseGoodsReceiptLines", "ProductUidLastTransactionDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PurchaseGoodsReceiptLines", "ProductUidCurrentProcessId", "Web.Processes");
            DropForeignKey("Web.PurchaseGoodsReceiptLines", "ProductUidCurrentGodownId", "Web.Godowns");
            DropForeignKey("Web.PurchaseGoodsReceiptLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.PurchaseGoodsReceiptLines", "ProductId", "Web.Products");
            DropForeignKey("Web.PurchaseGoodsReceiptLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.PurchaseGoodsReceiptLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.PurchaseGoodsReceiptLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.ProductSuppliers", "SupplierId", "Web.Suppliers");
            DropForeignKey("Web.Suppliers", "SalesTaxGroupPartyId", "Web.SalesTaxGroupParties");
            DropForeignKey("Web.Suppliers", "PersonID", "Web.People");
            DropForeignKey("Web.ProductSuppliers", "ProductId", "Web.Products");
            DropForeignKey("Web.ProductSizes", "SizeId", "Web.Sizes");
            DropForeignKey("Web.Sizes", "UnitId", "Web.Units");
            DropForeignKey("Web.Sizes", "ProductShapeId", "Web.ProductTypes");
            DropForeignKey("Web.Sizes", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProductSizes", "ProductSizeTypeId", "Web.ProductSizeTypes");
            DropForeignKey("Web.ProductSizes", "ProductId", "Web.Products");
            DropForeignKey("Web.ProductRelatedAccessories", "Product_ProductId", "Web.Products");
            DropForeignKey("Web.ProductRelatedAccessories", "ProductId", "Web.Products");
            DropForeignKey("Web.ProductRelatedAccessories", "AccessoryId", "Web.Products");
            DropForeignKey("Web.ProductIncludedAccessories", "Product_ProductId", "Web.Products");
            DropForeignKey("Web.ProductIncludedAccessories", "ProductId", "Web.Products");
            DropForeignKey("Web.ProductIncludedAccessories", "AccessoryId", "Web.Products");
            DropForeignKey("Web.Products", "ProductGroupId", "Web.ProductGroups");
            DropForeignKey("Web.ProductBuyers", "ProductId", "Web.Products");
            DropForeignKey("Web.ProductBuyers", "BuyerId", "Web.Buyers");
            DropForeignKey("Web.ProductAttributes", "ProductId", "Web.Products");
            DropForeignKey("Web.JobOrderBoms", "ProductId", "Web.Products");
            DropForeignKey("Web.JobOrderBoms", "JobOrderLineId", "Web.JobOrderLines");
            DropForeignKey("Web.JobOrderBoms", "JobOrderHeaderId", "Web.JobOrderHeaders");
            DropForeignKey("Web.JobOrderHeaders", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.JobOrderHeaders", "StockHeaderId", "Web.StockHeaders");
            DropForeignKey("Web.JobOrderHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.JobOrderHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.JobOrderHeaders", "OrderById", "Web.Employees");
            DropForeignKey("Web.Employees", "PersonID", "Web.People");
            DropForeignKey("Web.Employees", "DesignationID", "Web.Designations");
            DropForeignKey("Web.Employees", "DepartmentID", "Web.Departments");
            DropForeignKey("Web.JobOrderHeaders", "MachineId", "Web.Machines");
            DropForeignKey("Web.JobOrderHeaders", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.JobOrderLines", "UnitId", "Web.Units");
            DropForeignKey("Web.JobOrderLines", "StockProcessId", "Web.StockProcesses");
            DropForeignKey("Web.JobOrderLines", "StockId", "Web.Stocks");
            DropForeignKey("Web.JobOrderLines", "ProductUidLastTransactionPersonId", "Web.People");
            DropForeignKey("Web.JobOrderLines", "ProductUidLastTransactionDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobOrderLines", "ProductUidHeaderId", "Web.ProductUidHeaders");
            DropForeignKey("Web.JobOrderLines", "ProductUidCurrentProcessId", "Web.Processes");
            DropForeignKey("Web.JobOrderLines", "ProductUidCurrentGodownId", "Web.Godowns");
            DropForeignKey("Web.JobOrderLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.ProductUids", "ProductUidHeaderId", "Web.ProductUidHeaders");
            DropForeignKey("Web.ProductUidHeaders", "ProductId", "Web.Products");
            DropForeignKey("Web.ProductUidHeaders", "GenPersonId", "Web.People");
            DropForeignKey("Web.ProductUidHeaders", "GenDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProductUidHeaders", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ProductUidHeaders", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ProductUids", "ProductId", "Web.Products");
            DropForeignKey("Web.PackingLines", "StockReceiveId", "Web.Stocks");
            DropForeignKey("Web.PackingLines", "StockIssueId", "Web.Stocks");
            DropForeignKey("Web.PackingLines", "SaleOrderLineId", "Web.SaleOrderLines");
            DropForeignKey("Web.PackingLines", "SaleDeliveryOrderLineId", "Web.SaleDeliveryOrderLines");
            DropForeignKey("Web.SaleDeliveryOrderLines", "SaleOrderLineId", "Web.SaleOrderLines");
            DropForeignKey("Web.SaleOrderLines", "SaleOrderHeaderId", "Web.SaleOrderHeaders");
            DropForeignKey("Web.SaleOrderHeaders", "UnitConversionForId", "Web.UnitConversionFors");
            DropForeignKey("Web.SaleOrderHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleOrderHeaders", "ShipMethodId", "Web.ShipMethods");
            DropForeignKey("Web.SaleOrderHeaders", "SaleToBuyerId", "Web.People");
            DropForeignKey("Web.SaleOrderHeaders", "LedgerHeaderId", "Web.LedgerHeaders");
            DropForeignKey("Web.SaleOrderHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleOrderHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleOrderHeaders", "DeliveryTermsId", "Web.DeliveryTerms");
            DropForeignKey("Web.SaleOrderHeaders", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.SaleOrderHeaders", "BillToBuyerId", "Web.Buyers");
            DropForeignKey("Web.SaleOrderLines", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleOrderLines", "ProductId", "Web.Products");
            DropForeignKey("Web.SaleOrderLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.SaleOrderLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.SaleOrderLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.SaleDeliveryOrderLines", "SaleDeliveryOrderHeaderId", "Web.SaleDeliveryOrderHeaders");
            DropForeignKey("Web.SaleDeliveryOrderHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.SaleDeliveryOrderHeaders", "ShipMethodId", "Web.ShipMethods");
            DropForeignKey("Web.SaleDeliveryOrderHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleDeliveryOrderHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.SaleDeliveryOrderHeaders", "BuyerId", "Web.Buyers");
            DropForeignKey("Web.PackingLines", "ProductUidLastTransactionPersonId", "Web.People");
            DropForeignKey("Web.PackingLines", "ProductUidLastTransactionDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PackingLines", "ProductUidCurrentProcessId", "Web.Processes");
            DropForeignKey("Web.PackingLines", "ProductUidCurrentGodownId", "Web.Godowns");
            DropForeignKey("Web.PackingLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.PackingLines", "ProductId", "Web.Products");
            DropForeignKey("Web.PackingLines", "PackingHeaderId", "Web.PackingHeaders");
            DropForeignKey("Web.PackingHeaders", "StockHeaderId", "Web.StockHeaders");
            DropForeignKey("Web.StockLines", "StockProcessId", "Web.StockProcesses");
            DropForeignKey("Web.StockLines", "StockHeaderId", "Web.StockHeaders");
            DropForeignKey("Web.StockLines", "StockId", "Web.Stocks");
            DropForeignKey("Web.StockLines", "RequisitionLineId", "Web.RequisitionLines");
            DropForeignKey("Web.RequisitionLines", "RequisitionHeaderId", "Web.RequisitionHeaders");
            DropForeignKey("Web.RequisitionHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.RequisitionHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.RequisitionHeaders", "ReasonId", "Web.Reasons");
            DropForeignKey("Web.Reasons", "DocumentCategoryId", "Web.DocumentCategories");
            DropForeignKey("Web.RequisitionHeaders", "PersonId", "Web.People");
            DropForeignKey("Web.RequisitionHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.RequisitionHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.RequisitionHeaders", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.RequisitionLines", "ProductId", "Web.Products");
            DropForeignKey("Web.RequisitionLines", "ProcessId", "Web.Processes");
            DropForeignKey("Web.RequisitionLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.RequisitionLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.StockLines", "ProductUidLastTransactionPersonId", "Web.People");
            DropForeignKey("Web.StockLines", "ProductUidLastTransactionDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.StockLines", "ProductUidCurrentProcessId", "Web.Processes");
            DropForeignKey("Web.StockLines", "ProductUidCurrentGodownId", "Web.Godowns");
            DropForeignKey("Web.StockLines", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.StockLines", "ProductId", "Web.Products");
            DropForeignKey("Web.StockLines", "FromStockProcessId", "Web.StockProcesses");
            DropForeignKey("Web.StockProcesses", "StockHeaderId", "Web.StockHeaders");
            DropForeignKey("Web.StockProcesses", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.StockProcesses", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.StockProcesses", "ProductId", "Web.Products");
            DropForeignKey("Web.StockProcesses", "ProcessId", "Web.Processes");
            DropForeignKey("Web.StockProcesses", "GodownId", "Web.Godowns");
            DropForeignKey("Web.StockProcesses", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.StockProcesses", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.StockProcesses", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.StockLines", "FromStockId", "Web.Stocks");
            DropForeignKey("Web.Stocks", "StockHeaderId", "Web.StockHeaders");
            DropForeignKey("Web.Stocks", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.Stocks", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.Stocks", "ProductId", "Web.Products");
            DropForeignKey("Web.Stocks", "ProcessId", "Web.Processes");
            DropForeignKey("Web.Stocks", "GodownId", "Web.Godowns");
            DropForeignKey("Web.Stocks", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.Stocks", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.Stocks", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.StockLines", "FromProcessId", "Web.Processes");
            DropForeignKey("Web.StockLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.StockLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.StockLines", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.StockHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.StockHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.StockHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.StockHeaders", "PersonId", "Web.People");
            DropForeignKey("Web.StockHeaders", "MachineId", "Web.Machines");
            DropForeignKey("Web.StockHeaders", "LedgerHeaderId", "Web.LedgerHeaders");
            DropForeignKey("Web.LedgerHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.LedgerHeaders", "ProcessId", "Web.Processes");
            DropForeignKey("Web.LedgerHeaders", "LedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.LedgerHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.LedgerHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.LedgerHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.LedgerHeaders", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.StockHeaders", "LedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.StockHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.StockHeaders", "GatePassHeaderId", "Web.GatePassHeaders");
            DropForeignKey("Web.StockHeaders", "FromGodownId", "Web.Godowns");
            DropForeignKey("Web.StockHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.StockHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.StockHeaders", "CurrencyId", "Web.Currencies");
            DropForeignKey("Web.StockHeaders", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.StockHeaders", "ContraLedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.PackingHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.PackingHeaders", "ShipMethodId", "Web.ShipMethods");
            DropForeignKey("Web.PackingHeaders", "JobWorkerId", "Web.JobWorkers");
            DropForeignKey("Web.PackingHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.PackingHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PackingHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.PackingHeaders", "DealUnitId", "Web.Units");
            DropForeignKey("Web.PackingHeaders", "BuyerId", "Web.People");
            DropForeignKey("Web.PackingLines", "FromProcessId", "Web.Processes");
            DropForeignKey("Web.PackingLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.PackingLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.PackingLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.ProductUids", "LastTransactionPersonId", "Web.People");
            DropForeignKey("Web.ProductUids", "LastTransactionDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProductUids", "GenPersonId", "Web.Buyers");
            DropForeignKey("Web.ProductUids", "GenDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProductUids", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ProductUids", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ProductUids", "CurrenctProcessId", "Web.Processes");
            DropForeignKey("Web.ProductUids", "CurrenctGodownId", "Web.Godowns");
            DropForeignKey("Web.JobOrderLines", "ProductId", "Web.Products");
            DropForeignKey("Web.JobOrderLines", "ProdOrderLineId", "Web.ProdOrderLines");
            DropForeignKey("Web.ProdOrderLines", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProdOrderLines", "ProductId", "Web.Products");
            DropForeignKey("Web.ProdOrderLines", "ProdOrderHeaderId", "Web.ProdOrderHeaders");
            DropForeignKey("Web.ProdOrderHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.ProdOrderHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProdOrderHeaders", "MaterialPlanHeaderId", "Web.MaterialPlanHeaders");
            DropForeignKey("Web.ProdOrderHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.ProdOrderHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.ProdOrderHeaders", "BuyerId", "Web.Buyers");
            DropForeignKey("Web.ProdOrderLines", "ProcessId", "Web.Processes");
            DropForeignKey("Web.ProdOrderLines", "MaterialPlanLineId", "Web.MaterialPlanLines");
            DropForeignKey("Web.MaterialPlanLines", "ProductId", "Web.Products");
            DropForeignKey("Web.MaterialPlanLines", "ProcessId", "Web.Processes");
            DropForeignKey("Web.MaterialPlanLines", "MaterialPlanHeaderId", "Web.MaterialPlanHeaders");
            DropForeignKey("Web.MaterialPlanHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.MaterialPlanHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.MaterialPlanHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.MaterialPlanHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.MaterialPlanHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.MaterialPlanHeaders", "BuyerId", "Web.Buyers");
            DropForeignKey("Web.Buyers", "PersonID", "Web.People");
            DropForeignKey("Web.MaterialPlanLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.MaterialPlanLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.ProdOrderLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.ProdOrderLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.JobOrderLines", "JobOrderHeaderId", "Web.JobOrderHeaders");
            DropForeignKey("Web.JobOrderLines", "FromProcessId", "Web.Processes");
            DropForeignKey("Web.JobOrderLines", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.JobOrderLines", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.JobOrderLines", "DealUnitId", "Web.Units");
            DropForeignKey("Web.JobOrderHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.JobOrderHeaders", "GatePassHeaderId", "Web.GatePassHeaders");
            DropForeignKey("Web.GatePassHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.GatePassHeaders", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.GatePassHeaders", "PersonId", "Web.People");
            DropForeignKey("Web.GatePassHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.GatePassLines", "UnitId", "Web.Units");
            DropForeignKey("Web.GatePassLines", "GatePassHeaderId", "Web.GatePassHeaders");
            DropForeignKey("Web.GatePassHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.GatePassHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobOrderHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.JobOrderHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobOrderHeaders", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.CostCenters", "SiteId", "Web.Sites");
            DropForeignKey("Web.Sites", "PersonId", "Web.People");
            DropForeignKey("Web.Godowns", "SiteId", "Web.Sites");
            DropForeignKey("Web.Sites", "DefaultGodownId", "Web.Godowns");
            DropForeignKey("Web.Godowns", "GateId", "Web.Gates");
            DropForeignKey("Web.Gates", "SiteId", "Web.Sites");
            DropForeignKey("Web.Sites", "DefaultDivisionId", "Web.Divisions");
            DropForeignKey("Web.Sites", "CityId", "Web.Cities");
            DropForeignKey("Web.CostCenters", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.Processes", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.CostCenters", "ProcessId", "Web.Processes");
            DropForeignKey("Web.Processes", "ParentProcessId", "Web.Processes");
            DropForeignKey("Web.Processes", "AccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.CostCenters", "ParentCostCenterId", "Web.CostCenters");
            DropForeignKey("Web.CostCenters", "LedgerAccountId", "Web.LedgerAccounts");
            DropForeignKey("Web.LedgerAccounts", "PersonId", "Web.People");
            DropForeignKey("Web.LedgerAccounts", "LedgerAccountGroupId", "Web.LedgerAccountGroups");
            DropForeignKey("Web.CostCenters", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.CostCenters", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.JobOrderHeaders", "BillToPartyId", "Web.JobWorkers");
            DropForeignKey("Web.JobWorkers", "PersonID", "Web.People");
            DropForeignKey("Web.JobOrderBoms", "Dimension2Id", "Web.Dimension2");
            DropForeignKey("Web.Dimension2", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.Dimension2", "ProductTypeId", "Web.ProductTypes");
            DropForeignKey("Web.JobOrderBoms", "Dimension1Id", "Web.Dimension1");
            DropForeignKey("Web.Dimension1", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.Dimension1", "ProductTypeId", "Web.ProductTypes");
            DropForeignKey("Web.ProductTypeAttributes", "ProductType_ProductTypeId", "Web.ProductTypes");
            DropForeignKey("Web.ProductTypes", "ProductNatureId", "Web.ProductNatures");
            DropForeignKey("Web.ProductGroups", "ProductTypeId", "Web.ProductTypes");
            DropForeignKey("Web.ProductDesigns", "ProductTypeId", "Web.ProductTypes");
            DropForeignKey("Web.ProductCategories", "ProductTypeId", "Web.ProductTypes");
            DropForeignKey("Web.ProductTypes", "Dimension2TypeId", "Web.Dimension2Types");
            DropForeignKey("Web.ProductTypes", "Dimension1TypeId", "Web.Dimension1Types");
            DropForeignKey("Web.Products", "DrawBackTariffHeadId", "Web.DrawBackTariffHeads");
            DropForeignKey("Web.Products", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.Products", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.AspNetUserRoles1", "RoleId", "Web.AspNetRoles1");
            DropForeignKey("Web.Agents", "PersonID", "Web.People");
            DropForeignKey("Web.PersonContacts", "Person_PersonID", "Web.People");
            DropForeignKey("Web.PersonContacts", "PersonContactTypeId", "Web.PersonContactTypes");
            DropForeignKey("Web.PersonContacts", "PersonId", "Web.People");
            DropForeignKey("Web.PersonContacts", "ContactId", "Web.People");
            DropForeignKey("Web.PersonAddresses", "PersonId", "Web.People");
            DropForeignKey("Web.PersonAddresses", "CityId", "Web.Cities");
            DropForeignKey("Web.States", "CountryId", "Web.Countries");
            DropForeignKey("Web.Cities", "StateId", "Web.States");
            DropForeignKey("Web.PersonAddresses", "AreaId", "Web.Areas");
            DropForeignKey("Web.People", "ApplicationUser_Id", "Web.Users");
            DropForeignKey("Web.ActivityLogs", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.DocumentTypes", "ReportMenuId", "Web.Menus");
            DropForeignKey("Web.Menus", "SubModuleId", "Web.MenuSubModules");
            DropForeignKey("Web.Menus", "ModuleId", "Web.MenuModules");
            DropForeignKey("Web.Menus", "ControllerActionId", "Web.ControllerActions");
            DropForeignKey("Web.DocumentTypes", "DocumentCategoryId", "Web.DocumentCategories");
            DropForeignKey("Web.DocumentTypes", "ControllerActionId", "Web.ControllerActions");
            DropForeignKey("Web.ControllerActions", "ControllerId", "Web.MvcControllers");
            DropForeignKey("Web.MvcControllers", "ParentControllerId", "Web.MvcControllers");
            DropForeignKey("Web.ActivityLogs", "ActivityType", "Web.ActivityTypes");
            DropIndex("Web.ProductManufacturer", new[] { "PersonID" });
            DropIndex("Web.SaleInvoiceHeaderDetail", new[] { "SaleInvoiceHeaderId" });
            DropIndex("Web.FinishedProduct", new[] { "SampleId" });
            DropIndex("Web.FinishedProduct", new[] { "ProductShapeId" });
            DropIndex("Web.FinishedProduct", new[] { "OriginCountryId" });
            DropIndex("Web.FinishedProduct", new[] { "DescriptionOfGoodsId" });
            DropIndex("Web.FinishedProduct", new[] { "ProductStyleId" });
            DropIndex("Web.FinishedProduct", new[] { "ProductManufacturerId" });
            DropIndex("Web.FinishedProduct", new[] { "ProcessSequenceHeaderId" });
            DropIndex("Web.FinishedProduct", new[] { "ProductInvoiceGroupId" });
            DropIndex("Web.FinishedProduct", new[] { "ContentId" });
            DropIndex("Web.FinishedProduct", new[] { "FaceContentId" });
            DropIndex("Web.FinishedProduct", new[] { "ColourId" });
            DropIndex("Web.FinishedProduct", new[] { "ProductDesignPatternId" });
            DropIndex("Web.FinishedProduct", new[] { "ProductDesignId" });
            DropIndex("Web.FinishedProduct", new[] { "ProductQualityId" });
            DropIndex("Web.FinishedProduct", new[] { "ProductCollectionId" });
            DropIndex("Web.FinishedProduct", new[] { "ProductCategoryId" });
            DropIndex("Web.FinishedProduct", new[] { "ProductId" });
            DropIndex("Web.WeavingRetensions", new[] { "ProductCategoryId" });
            DropIndex("Web.WeavingRetensions", "IX_WeavingRetension_DocID");
            DropIndex("Web.ViewSaleOrderBalanceForCancellation", new[] { "ProductId" });
            DropIndex("Web.ViewSaleOrderBalance", new[] { "ProductId" });
            DropIndex("Web.ViewSaleOrderBalance", new[] { "Dimension2Id" });
            DropIndex("Web.ViewSaleOrderBalance", new[] { "Dimension1Id" });
            DropIndex("Web.ViewRequisitionBalance", new[] { "CostCenterId" });
            DropIndex("Web.ViewRequisitionBalance", new[] { "Dimension2Id" });
            DropIndex("Web.ViewRequisitionBalance", new[] { "Dimension1Id" });
            DropIndex("Web.ViewRequisitionBalance", new[] { "DivisionId" });
            DropIndex("Web.ViewRequisitionBalance", new[] { "SiteId" });
            DropIndex("Web.ViewRequisitionBalance", new[] { "PersonId" });
            DropIndex("Web.ViewRequisitionBalance", new[] { "ProductId" });
            DropIndex("Web.ViewPurchaseIndentBalance", new[] { "DocTypeId" });
            DropIndex("Web.ViewProdOrderBalance", new[] { "DocTypeId" });
            DropIndex("Web.ViewProdOrderBalance", new[] { "Dimension2Id" });
            DropIndex("Web.ViewProdOrderBalance", new[] { "Dimension1Id" });
            DropIndex("Web.ViewProdOrderBalance", new[] { "ProductId" });
            DropIndex("Web.ViewMaterialPlanBalance", new[] { "DocTypeId" });
            DropIndex("Web.ViewJobReceiveBalanceForInvoice", new[] { "Dimension2Id" });
            DropIndex("Web.ViewJobReceiveBalanceForInvoice", new[] { "Dimension1Id" });
            DropIndex("Web.ViewJobReceiveBalanceForInvoice", new[] { "ProductId" });
            DropIndex("Web.ViewJobReceiveBalanceForInvoice", new[] { "JobOrderLineId" });
            DropIndex("Web.ViewJobReceiveBalance", new[] { "Dimension2Id" });
            DropIndex("Web.ViewJobReceiveBalance", new[] { "Dimension1Id" });
            DropIndex("Web.ViewJobReceiveBalance", new[] { "ProductId" });
            DropIndex("Web.ViewJobReceiveBalance", new[] { "JobOrderLineId" });
            DropIndex("Web.ViewJobOrderInspectionRequestBalance", new[] { "Dimension2Id" });
            DropIndex("Web.ViewJobOrderInspectionRequestBalance", new[] { "Dimension1Id" });
            DropIndex("Web.ViewJobOrderInspectionRequestBalance", new[] { "DivisionId" });
            DropIndex("Web.ViewJobOrderInspectionRequestBalance", new[] { "SiteId" });
            DropIndex("Web.ViewJobOrderInspectionRequestBalance", new[] { "ProductId" });
            DropIndex("Web.ViewJobOrderBalanceFromStatus", new[] { "Dimension2Id" });
            DropIndex("Web.ViewJobOrderBalanceFromStatus", new[] { "Dimension1Id" });
            DropIndex("Web.ViewJobOrderBalanceFromStatus", new[] { "DivisionId" });
            DropIndex("Web.ViewJobOrderBalanceFromStatus", new[] { "SiteId" });
            DropIndex("Web.ViewJobOrderBalanceFromStatus", new[] { "ProductId" });
            DropIndex("Web.ViewJobOrderBalanceForInvoice", new[] { "Dimension2Id" });
            DropIndex("Web.ViewJobOrderBalanceForInvoice", new[] { "Dimension1Id" });
            DropIndex("Web.ViewJobOrderBalanceForInvoice", new[] { "JobWorkerId" });
            DropIndex("Web.ViewJobOrderBalanceForInvoice", new[] { "ProductId" });
            DropIndex("Web.ViewJobOrderBalanceForInspectionRequest", new[] { "Dimension2Id" });
            DropIndex("Web.ViewJobOrderBalanceForInspectionRequest", new[] { "Dimension1Id" });
            DropIndex("Web.ViewJobOrderBalanceForInspectionRequest", new[] { "DivisionId" });
            DropIndex("Web.ViewJobOrderBalanceForInspectionRequest", new[] { "SiteId" });
            DropIndex("Web.ViewJobOrderBalanceForInspectionRequest", new[] { "ProductId" });
            DropIndex("Web.ViewJobOrderBalanceForInspection", new[] { "Dimension2Id" });
            DropIndex("Web.ViewJobOrderBalanceForInspection", new[] { "Dimension1Id" });
            DropIndex("Web.ViewJobOrderBalanceForInspection", new[] { "DivisionId" });
            DropIndex("Web.ViewJobOrderBalanceForInspection", new[] { "SiteId" });
            DropIndex("Web.ViewJobOrderBalanceForInspection", new[] { "ProductId" });
            DropIndex("Web.ViewJobOrderBalance", new[] { "Dimension2Id" });
            DropIndex("Web.ViewJobOrderBalance", new[] { "Dimension1Id" });
            DropIndex("Web.ViewJobOrderBalance", new[] { "DivisionId" });
            DropIndex("Web.ViewJobOrderBalance", new[] { "SiteId" });
            DropIndex("Web.ViewJobOrderBalance", new[] { "ProductId" });
            DropIndex("Web.ViewJobInvoiceBalanceForRateAmendment", new[] { "Dimension2Id" });
            DropIndex("Web.ViewJobInvoiceBalanceForRateAmendment", new[] { "Dimension1Id" });
            DropIndex("Web.ViewJobInvoiceBalanceForRateAmendment", new[] { "JobWorkerId" });
            DropIndex("Web.ViewJobInvoiceBalanceForRateAmendment", new[] { "ProductId" });
            DropIndex("Web.UserRoles", new[] { "UserId" });
            DropIndex("Web.UserPersons", new[] { "PersonId" });
            DropIndex("Web.UserBookMarks", new[] { "MenuId" });
            DropIndex("Web.UnitConversions", "IX_UnitConversion_UniqueKey");
            DropIndex("Web.TdsRates", new[] { "TdsGroupId" });
            DropIndex("Web.TdsRates", new[] { "TdsCategoryId" });
            DropIndex("Web.StockUid", new[] { "ProductUidId" });
            DropIndex("Web.StockUid", new[] { "ProcessId" });
            DropIndex("Web.StockUid", new[] { "GodownId" });
            DropIndex("Web.StockUid", new[] { "SiteId" });
            DropIndex("Web.StockUid", new[] { "DivisionId" });
            DropIndex("Web.StockUid", new[] { "DocTypeId" });
            DropIndex("Web.StockProcessBalances", new[] { "CostCenterId" });
            DropIndex("Web.StockProcessBalances", new[] { "GodownId" });
            DropIndex("Web.StockProcessBalances", new[] { "ProcessId" });
            DropIndex("Web.StockProcessBalances", new[] { "Dimension2Id" });
            DropIndex("Web.StockProcessBalances", new[] { "Dimension1Id" });
            DropIndex("Web.StockProcessBalances", new[] { "ProductId" });
            DropIndex("Web.StockInOuts", new[] { "StockInId" });
            DropIndex("Web.StockInOuts", new[] { "StockOutId" });
            DropIndex("Web.StockBalances", new[] { "CostCenterId" });
            DropIndex("Web.StockBalances", new[] { "GodownId" });
            DropIndex("Web.StockBalances", new[] { "ProcessId" });
            DropIndex("Web.StockBalances", new[] { "Dimension2Id" });
            DropIndex("Web.StockBalances", new[] { "Dimension1Id" });
            DropIndex("Web.StockBalances", new[] { "ProductId" });
            DropIndex("Web.StockAdjs", "IX_Stock_DocID");
            DropIndex("Web.StockAdjs", new[] { "StockOutId" });
            DropIndex("Web.StockAdjs", new[] { "StockInId" });
            DropIndex("Web.SchemeRateDetails", new[] { "UnitId" });
            DropIndex("Web.SchemeRateDetails", new[] { "SchemeId" });
            DropIndex("Web.SchemeHeaders", new[] { "ProcessId" });
            DropIndex("Web.SchemeHeaders", "IX_Scheme_SchemeName");
            DropIndex("Web.SchemeDateDetails", new[] { "SchemeId" });
            DropIndex("Web.SaleOrderSettings", new[] { "ProcessId" });
            DropIndex("Web.SaleOrderSettings", new[] { "CalculationId" });
            DropIndex("Web.SaleOrderSettings", new[] { "ImportMenuId" });
            DropIndex("Web.SaleOrderSettings", new[] { "UnitConversionForId" });
            DropIndex("Web.SaleOrderSettings", new[] { "DivisionId" });
            DropIndex("Web.SaleOrderSettings", new[] { "SiteId" });
            DropIndex("Web.SaleOrderSettings", new[] { "DeliveryTermsId" });
            DropIndex("Web.SaleOrderSettings", new[] { "CurrencyId" });
            DropIndex("Web.SaleOrderSettings", new[] { "ShipMethodId" });
            DropIndex("Web.SaleOrderSettings", new[] { "DocTypeId" });
            DropIndex("Web.SaleOrderRateAmendmentLines", new[] { "SaleOrderLineId" });
            DropIndex("Web.SaleOrderRateAmendmentLines", new[] { "SaleOrderAmendmentHeaderId" });
            DropIndex("Web.SaleOrderQtyAmendmentLines", new[] { "SaleOrderLineId" });
            DropIndex("Web.SaleOrderQtyAmendmentLines", new[] { "SaleOrderAmendmentHeaderId" });
            DropIndex("Web.SaleOrderLineStatus", new[] { "SaleOrderLineId" });
            DropIndex("Web.SaleOrderCancelLines", new[] { "SaleOrderCancelHeaderId" });
            DropIndex("Web.SaleOrderCancelLines", new[] { "SaleOrderLineId" });
            DropIndex("Web.SaleOrderCancelHeaders", new[] { "SiteId" });
            DropIndex("Web.SaleOrderCancelHeaders", new[] { "BuyerId" });
            DropIndex("Web.SaleOrderCancelHeaders", new[] { "DivisionId" });
            DropIndex("Web.SaleOrderCancelHeaders", new[] { "ReasonId" });
            DropIndex("Web.SaleOrderCancelHeaders", new[] { "DocTypeId" });
            DropIndex("Web.SaleOrderAmendmentHeaders", new[] { "BuyerId" });
            DropIndex("Web.SaleOrderAmendmentHeaders", new[] { "ReasonId" });
            DropIndex("Web.SaleOrderAmendmentHeaders", new[] { "SiteId" });
            DropIndex("Web.SaleOrderAmendmentHeaders", new[] { "DivisionId" });
            DropIndex("Web.SaleOrderAmendmentHeaders", new[] { "DocTypeId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "GodownId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "SalesTaxGroupId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "ShipMethodId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "DeliveryTermsId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "ProcessId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "CurrencyId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "ImportMenuId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "DocTypeDispatchReturnId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "UnitConversionForId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "CalculationId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "SaleDispatchDocTypeId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "DocTypePackingHeaderId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "DivisionId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "SiteId" });
            DropIndex("Web.SaleInvoiceSettings", new[] { "DocTypeId" });
            DropIndex("Web.SaleInvoiceReturnLineCharges", new[] { "ParentChargeId" });
            DropIndex("Web.SaleInvoiceReturnLineCharges", new[] { "CostCenterId" });
            DropIndex("Web.SaleInvoiceReturnLineCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.SaleInvoiceReturnLineCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.SaleInvoiceReturnLineCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.SaleInvoiceReturnLineCharges", new[] { "PersonID" });
            DropIndex("Web.SaleInvoiceReturnLineCharges", new[] { "CalculateOnId" });
            DropIndex("Web.SaleInvoiceReturnLineCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.SaleInvoiceReturnLineCharges", new[] { "ChargeId" });
            DropIndex("Web.SaleInvoiceReturnLineCharges", new[] { "LineTableId" });
            DropIndex("Web.SaleInvoiceReturnLines", new[] { "DealUnitId" });
            DropIndex("Web.SaleInvoiceReturnLines", new[] { "SaleDispatchReturnLineId" });
            DropIndex("Web.SaleInvoiceReturnLines", new[] { "SaleInvoiceLineId" });
            DropIndex("Web.SaleInvoiceReturnLines", new[] { "SaleInvoiceReturnHeaderId" });
            DropIndex("Web.SaleInvoiceReturnHeaderCharges", new[] { "ParentChargeId" });
            DropIndex("Web.SaleInvoiceReturnHeaderCharges", new[] { "CostCenterId" });
            DropIndex("Web.SaleInvoiceReturnHeaderCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.SaleInvoiceReturnHeaderCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.SaleInvoiceReturnHeaderCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.SaleInvoiceReturnHeaderCharges", new[] { "PersonID" });
            DropIndex("Web.SaleInvoiceReturnHeaderCharges", new[] { "CalculateOnId" });
            DropIndex("Web.SaleInvoiceReturnHeaderCharges", new[] { "ProductChargeId" });
            DropIndex("Web.SaleInvoiceReturnHeaderCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.SaleInvoiceReturnHeaderCharges", new[] { "ChargeId" });
            DropIndex("Web.SaleInvoiceReturnHeaderCharges", new[] { "HeaderTableId" });
            DropIndex("Web.SaleInvoiceReturnHeaders", new[] { "SaleDispatchReturnHeaderId" });
            DropIndex("Web.SaleInvoiceReturnHeaders", new[] { "CurrencyId" });
            DropIndex("Web.SaleInvoiceReturnHeaders", new[] { "SalesTaxGroupPartyId" });
            DropIndex("Web.SaleInvoiceReturnHeaders", new[] { "SalesTaxGroupId" });
            DropIndex("Web.SaleInvoiceReturnHeaders", new[] { "BuyerId" });
            DropIndex("Web.SaleInvoiceReturnHeaders", new[] { "LedgerHeaderId" });
            DropIndex("Web.SaleInvoiceReturnHeaders", new[] { "ReasonId" });
            DropIndex("Web.SaleInvoiceReturnHeaders", "IX_SaleInvoiceReturnHeader_DocID");
            DropIndex("Web.SaleInvoiceLineCharges", new[] { "ParentChargeId" });
            DropIndex("Web.SaleInvoiceLineCharges", new[] { "CostCenterId" });
            DropIndex("Web.SaleInvoiceLineCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.SaleInvoiceLineCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.SaleInvoiceLineCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.SaleInvoiceLineCharges", new[] { "PersonID" });
            DropIndex("Web.SaleInvoiceLineCharges", new[] { "CalculateOnId" });
            DropIndex("Web.SaleInvoiceLineCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.SaleInvoiceLineCharges", new[] { "ChargeId" });
            DropIndex("Web.SaleInvoiceLineCharges", new[] { "LineTableId" });
            DropIndex("Web.SaleInvoiceHeaderCharges", new[] { "ParentChargeId" });
            DropIndex("Web.SaleInvoiceHeaderCharges", new[] { "CostCenterId" });
            DropIndex("Web.SaleInvoiceHeaderCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.SaleInvoiceHeaderCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.SaleInvoiceHeaderCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.SaleInvoiceHeaderCharges", new[] { "PersonID" });
            DropIndex("Web.SaleInvoiceHeaderCharges", new[] { "CalculateOnId" });
            DropIndex("Web.SaleInvoiceHeaderCharges", new[] { "ProductChargeId" });
            DropIndex("Web.SaleInvoiceHeaderCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.SaleInvoiceHeaderCharges", new[] { "ChargeId" });
            DropIndex("Web.SaleInvoiceHeaderCharges", new[] { "HeaderTableId" });
            DropIndex("Web.SaleDispatchSettings", new[] { "GodownId" });
            DropIndex("Web.SaleDispatchSettings", new[] { "ShipMethodId" });
            DropIndex("Web.SaleDispatchSettings", new[] { "DeliveryTermsId" });
            DropIndex("Web.SaleDispatchSettings", new[] { "ProcessId" });
            DropIndex("Web.SaleDispatchSettings", new[] { "ImportMenuId" });
            DropIndex("Web.SaleDispatchSettings", new[] { "DocTypeDispatchReturnId" });
            DropIndex("Web.SaleDispatchSettings", new[] { "UnitConversionForId" });
            DropIndex("Web.SaleDispatchSettings", new[] { "DocTypePackingHeaderId" });
            DropIndex("Web.SaleDispatchSettings", new[] { "DivisionId" });
            DropIndex("Web.SaleDispatchSettings", new[] { "SiteId" });
            DropIndex("Web.SaleDispatchSettings", new[] { "DocTypeId" });
            DropIndex("Web.SaleDispatchReturnLines", new[] { "StockId" });
            DropIndex("Web.SaleDispatchReturnLines", new[] { "DealUnitId" });
            DropIndex("Web.SaleDispatchReturnLines", new[] { "SaleDispatchLineId" });
            DropIndex("Web.SaleDispatchReturnLines", new[] { "SaleDispatchReturnHeaderId" });
            DropIndex("Web.SaleDispatchReturnHeaders", new[] { "GodownId" });
            DropIndex("Web.SaleDispatchReturnHeaders", new[] { "StockHeaderId" });
            DropIndex("Web.SaleDispatchReturnHeaders", new[] { "BuyerId" });
            DropIndex("Web.SaleDispatchReturnHeaders", new[] { "ReasonId" });
            DropIndex("Web.SaleDispatchReturnHeaders", "IX_SaleDispatchReturnHeader_DocID");
            DropIndex("Web.SaleDeliveryOrderSettings", new[] { "DivisionId" });
            DropIndex("Web.SaleDeliveryOrderSettings", new[] { "SiteId" });
            DropIndex("Web.SaleDeliveryOrderSettings", new[] { "ImportMenuId" });
            DropIndex("Web.SaleDeliveryOrderSettings", new[] { "DocTypeId" });
            DropIndex("Web.SaleDeliveryOrderCancelLines", new[] { "SaleDeliveryOrderLineId" });
            DropIndex("Web.SaleDeliveryOrderCancelLines", new[] { "ProductUidId" });
            DropIndex("Web.SaleDeliveryOrderCancelLines", new[] { "SaleDeliveryOrderCancelHeaderId" });
            DropIndex("Web.SaleDeliveryOrderCancelHeaders", new[] { "OrderById" });
            DropIndex("Web.SaleDeliveryOrderCancelHeaders", new[] { "BuyerId" });
            DropIndex("Web.SaleDeliveryOrderCancelHeaders", new[] { "ReasonId" });
            DropIndex("Web.SaleDeliveryOrderCancelHeaders", "IX_DeliveryOrderHeader_Unique");
            DropIndex("Web.RugStencils", new[] { "ProductSizeId" });
            DropIndex("Web.RugStencils", new[] { "ProductDesignId" });
            DropIndex("Web.RugStencils", new[] { "StencilId" });
            DropIndex("Web.Rug_RetentionPercentage", "IX_Stock_DocID");
            DropIndex("Web.Rug_RetentionPercentage", new[] { "ProductCategoryId" });
            DropIndex("Web.RouteLines", new[] { "CityId" });
            DropIndex("Web.RouteLines", new[] { "RouteId" });
            DropIndex("Web.RolesSites", new[] { "SiteId" });
            DropIndex("Web.RolesSites", new[] { "RoleId" });
            DropIndex("Web.RolesMenus", "IX_PurchaseOrderHeader_DocID");
            DropIndex("Web.RolesMenus", new[] { "MenuId" });
            DropIndex("Web.RolesMenus", new[] { "RoleId" });
            DropIndex("Web.RolesDivisions", new[] { "DivisionId" });
            DropIndex("Web.RolesDivisions", new[] { "RoleId" });
            DropIndex("Web.RolesControllerActions", new[] { "ControllerActionId" });
            DropIndex("Web.RolesControllerActions", new[] { "RoleId" });
            DropIndex("Web.AspNetRoles", "RoleNameIndex");
            DropIndex("Web.RequisitionSettings", new[] { "DefaultReasonId" });
            DropIndex("Web.RequisitionSettings", new[] { "OnApproveMenuId" });
            DropIndex("Web.RequisitionSettings", new[] { "OnSubmitMenuId" });
            DropIndex("Web.RequisitionSettings", new[] { "DivisionId" });
            DropIndex("Web.RequisitionSettings", new[] { "SiteId" });
            DropIndex("Web.RequisitionSettings", new[] { "DocTypeId" });
            DropIndex("Web.RequisitionLineStatus", new[] { "RequisitionLineId" });
            DropIndex("Web.RequisitionCancelLines", new[] { "RequisitionLineId" });
            DropIndex("Web.RequisitionCancelLines", new[] { "RequisitionCancelHeaderId" });
            DropIndex("Web.RequisitionCancelHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.RequisitionCancelHeaders", new[] { "ReasonId" });
            DropIndex("Web.RequisitionCancelHeaders", new[] { "PersonId" });
            DropIndex("Web.RequisitionCancelHeaders", "IX_RequisitionCancelHeader_DocID");
            DropIndex("Web.SubReports", new[] { "ReportHeaderId" });
            DropIndex("Web.ReportLines", new[] { "ReportHeaderId" });
            DropIndex("Web.ReportColumns", new[] { "SubReportHeaderId" });
            DropIndex("Web.ReportColumns", new[] { "SubReportId" });
            DropIndex("Web.ReportColumns", new[] { "ReportHeaderId" });
            DropIndex("Web.RateListProductRateGroups", new[] { "ProductRateGroupId" });
            DropIndex("Web.RateListProductRateGroups", new[] { "RateListHeaderId" });
            DropIndex("Web.RateListPersonRateGroups", new[] { "PersonRateGroupId" });
            DropIndex("Web.RateListPersonRateGroups", new[] { "RateListHeaderId" });
            DropIndex("Web.RateListLineHistories", new[] { "Dimension2Id" });
            DropIndex("Web.RateListLineHistories", new[] { "Dimension1Id" });
            DropIndex("Web.RateListLineHistories", new[] { "ProductId" });
            DropIndex("Web.RateListLineHistories", new[] { "ProductRateGroupId" });
            DropIndex("Web.RateListLineHistories", new[] { "PersonRateGroupId" });
            DropIndex("Web.RateListLineHistories", new[] { "RateListHeaderId" });
            DropIndex("Web.RateListLines", new[] { "Dimension2Id" });
            DropIndex("Web.RateListLines", new[] { "Dimension1Id" });
            DropIndex("Web.RateListLines", new[] { "ProductId" });
            DropIndex("Web.RateListLines", new[] { "ProductRateGroupId" });
            DropIndex("Web.RateListLines", new[] { "PersonRateGroupId" });
            DropIndex("Web.RateListLines", new[] { "RateListHeaderId" });
            DropIndex("Web.RateListHistories", new[] { "DealUnitId" });
            DropIndex("Web.RateListHistories", new[] { "ProductId" });
            DropIndex("Web.RateListHistories", new[] { "DocTypeId" });
            DropIndex("Web.RateListHistories", new[] { "PersonRateGroupId" });
            DropIndex("Web.RateListHistories", new[] { "ProcessId" });
            DropIndex("Web.RateListHeaders", new[] { "DealUnitId" });
            DropIndex("Web.RateListHeaders", "IX_RateListHeader_Name");
            DropIndex("Web.RateListHeaders", "IX_RateListHeader_DocID");
            DropIndex("Web.RateLists", new[] { "DealUnitId" });
            DropIndex("Web.RateLists", new[] { "ProductId" });
            DropIndex("Web.RateLists", new[] { "DocTypeId" });
            DropIndex("Web.RateLists", new[] { "PersonRateGroupId" });
            DropIndex("Web.RateLists", new[] { "ProcessId" });
            DropIndex("Web.RateConversionSettings", new[] { "ProcessId" });
            DropIndex("Web.RateConversionSettings", new[] { "DivisionId" });
            DropIndex("Web.RateConversionSettings", new[] { "SiteId" });
            DropIndex("Web.RateConversionSettings", new[] { "DocTypeId" });
            DropIndex("Web.PurchaseQuotationSettings", new[] { "OnApproveMenuId" });
            DropIndex("Web.PurchaseQuotationSettings", new[] { "OnSubmitMenuId" });
            DropIndex("Web.PurchaseQuotationSettings", new[] { "UnitConversionForId" });
            DropIndex("Web.PurchaseQuotationSettings", new[] { "CalculationId" });
            DropIndex("Web.PurchaseQuotationSettings", new[] { "DivisionId" });
            DropIndex("Web.PurchaseQuotationSettings", new[] { "SiteId" });
            DropIndex("Web.PurchaseQuotationSettings", new[] { "DocTypeId" });
            DropIndex("Web.PurchaseQuotationLineCharges", new[] { "ParentChargeId" });
            DropIndex("Web.PurchaseQuotationLineCharges", new[] { "CostCenterId" });
            DropIndex("Web.PurchaseQuotationLineCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.PurchaseQuotationLineCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.PurchaseQuotationLineCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.PurchaseQuotationLineCharges", new[] { "PersonID" });
            DropIndex("Web.PurchaseQuotationLineCharges", new[] { "CalculateOnId" });
            DropIndex("Web.PurchaseQuotationLineCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.PurchaseQuotationLineCharges", new[] { "ChargeId" });
            DropIndex("Web.PurchaseQuotationLineCharges", new[] { "LineTableId" });
            DropIndex("Web.PurchaseQuotationHeaderCharges", new[] { "ParentChargeId" });
            DropIndex("Web.PurchaseQuotationHeaderCharges", new[] { "CostCenterId" });
            DropIndex("Web.PurchaseQuotationHeaderCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.PurchaseQuotationHeaderCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.PurchaseQuotationHeaderCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.PurchaseQuotationHeaderCharges", new[] { "PersonID" });
            DropIndex("Web.PurchaseQuotationHeaderCharges", new[] { "CalculateOnId" });
            DropIndex("Web.PurchaseQuotationHeaderCharges", new[] { "ProductChargeId" });
            DropIndex("Web.PurchaseQuotationHeaderCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.PurchaseQuotationHeaderCharges", new[] { "ChargeId" });
            DropIndex("Web.PurchaseQuotationHeaderCharges", new[] { "HeaderTableId" });
            DropIndex("Web.PurchaseQuotationLines", new[] { "DealUnitId" });
            DropIndex("Web.PurchaseQuotationLines", new[] { "SalesTaxGroupId" });
            DropIndex("Web.PurchaseQuotationLines", new[] { "Dimension2Id" });
            DropIndex("Web.PurchaseQuotationLines", new[] { "Dimension1Id" });
            DropIndex("Web.PurchaseQuotationLines", new[] { "PurchaseIndentLineId" });
            DropIndex("Web.PurchaseQuotationLines", new[] { "ProductId" });
            DropIndex("Web.PurchaseQuotationLines", new[] { "PurchaseQuotationHeaderId" });
            DropIndex("Web.PurchaseQuotationHeaders", new[] { "ShipMethodId" });
            DropIndex("Web.PurchaseQuotationHeaders", new[] { "DeliveryTermsId" });
            DropIndex("Web.PurchaseQuotationHeaders", new[] { "UnitConversionForId" });
            DropIndex("Web.PurchaseQuotationHeaders", new[] { "SalesTaxGroupId" });
            DropIndex("Web.PurchaseQuotationHeaders", new[] { "CurrencyId" });
            DropIndex("Web.PurchaseQuotationHeaders", new[] { "BillingAccountId" });
            DropIndex("Web.PurchaseQuotationHeaders", new[] { "SupplierId" });
            DropIndex("Web.PurchaseQuotationHeaders", "IX_PurchaseQuotationHeader_DocID");
            DropIndex("Web.PurchaseOrderSettings", new[] { "OnApproveMenuId" });
            DropIndex("Web.PurchaseOrderSettings", new[] { "OnSubmitMenuId" });
            DropIndex("Web.PurchaseOrderSettings", new[] { "UnitConversionForId" });
            DropIndex("Web.PurchaseOrderSettings", new[] { "CalculationId" });
            DropIndex("Web.PurchaseOrderSettings", new[] { "DivisionId" });
            DropIndex("Web.PurchaseOrderSettings", new[] { "SiteId" });
            DropIndex("Web.PurchaseOrderSettings", new[] { "DocTypeId" });
            DropIndex("Web.PurchaseOrderRateAmendmentLineCharges", new[] { "ParentChargeId" });
            DropIndex("Web.PurchaseOrderRateAmendmentLineCharges", new[] { "CostCenterId" });
            DropIndex("Web.PurchaseOrderRateAmendmentLineCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.PurchaseOrderRateAmendmentLineCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.PurchaseOrderRateAmendmentLineCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.PurchaseOrderRateAmendmentLineCharges", new[] { "PersonID" });
            DropIndex("Web.PurchaseOrderRateAmendmentLineCharges", new[] { "CalculateOnId" });
            DropIndex("Web.PurchaseOrderRateAmendmentLineCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.PurchaseOrderRateAmendmentLineCharges", new[] { "ChargeId" });
            DropIndex("Web.PurchaseOrderRateAmendmentLineCharges", new[] { "LineTableId" });
            DropIndex("Web.PurchaseOrderRateAmendmentLines", new[] { "PurchaseOrderLineId" });
            DropIndex("Web.PurchaseOrderRateAmendmentLines", new[] { "PurchaseOrderAmendmentHeaderId" });
            DropIndex("Web.PurchaseOrderQtyAmendmentLines", new[] { "PurchaseOrderLineId" });
            DropIndex("Web.PurchaseOrderQtyAmendmentLines", new[] { "PurchaseOrderAmendmentHeaderId" });
            DropIndex("Web.PurchaseOrderLineStatus", new[] { "PurchaseOrderLineId" });
            DropIndex("Web.PurchaseOrderLineCharges", new[] { "ParentChargeId" });
            DropIndex("Web.PurchaseOrderLineCharges", new[] { "CostCenterId" });
            DropIndex("Web.PurchaseOrderLineCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.PurchaseOrderLineCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.PurchaseOrderLineCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.PurchaseOrderLineCharges", new[] { "PersonID" });
            DropIndex("Web.PurchaseOrderLineCharges", new[] { "CalculateOnId" });
            DropIndex("Web.PurchaseOrderLineCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.PurchaseOrderLineCharges", new[] { "ChargeId" });
            DropIndex("Web.PurchaseOrderLineCharges", new[] { "LineTableId" });
            DropIndex("Web.PurchaseOrderInspectionSettings", new[] { "DivisionId" });
            DropIndex("Web.PurchaseOrderInspectionSettings", new[] { "SiteId" });
            DropIndex("Web.PurchaseOrderInspectionSettings", new[] { "ProcessId" });
            DropIndex("Web.PurchaseOrderInspectionSettings", new[] { "ImportMenuId" });
            DropIndex("Web.PurchaseOrderInspectionSettings", new[] { "DocTypeId" });
            DropIndex("Web.PurchaseOrderInspectionRequestSettings", new[] { "DivisionId" });
            DropIndex("Web.PurchaseOrderInspectionRequestSettings", new[] { "SiteId" });
            DropIndex("Web.PurchaseOrderInspectionRequestSettings", new[] { "ProcessId" });
            DropIndex("Web.PurchaseOrderInspectionRequestSettings", new[] { "ImportMenuId" });
            DropIndex("Web.PurchaseOrderInspectionRequestSettings", new[] { "DocTypeId" });
            DropIndex("Web.PurchaseOrderInspectionRequestCancelLines", new[] { "PurchaseOrderInspectionRequestLineId" });
            DropIndex("Web.PurchaseOrderInspectionRequestCancelLines", new[] { "ProductUidId" });
            DropIndex("Web.PurchaseOrderInspectionRequestCancelLines", new[] { "PurchaseOrderInspectionRequestCancelHeaderId" });
            DropIndex("Web.PurchaseOrderInspectionRequestCancelHeaders", new[] { "ReasonId" });
            DropIndex("Web.PurchaseOrderInspectionRequestCancelHeaders", new[] { "SupplierId" });
            DropIndex("Web.PurchaseOrderInspectionRequestCancelHeaders", new[] { "ProcessId" });
            DropIndex("Web.PurchaseOrderInspectionRequestCancelHeaders", new[] { "SiteId" });
            DropIndex("Web.PurchaseOrderInspectionRequestCancelHeaders", new[] { "DivisionId" });
            DropIndex("Web.PurchaseOrderInspectionRequestCancelHeaders", new[] { "DocTypeId" });
            DropIndex("Web.PurchaseOrderInspectionRequestHeaders", new[] { "SupplierId" });
            DropIndex("Web.PurchaseOrderInspectionRequestHeaders", new[] { "ProcessId" });
            DropIndex("Web.PurchaseOrderInspectionRequestHeaders", new[] { "SiteId" });
            DropIndex("Web.PurchaseOrderInspectionRequestHeaders", new[] { "DivisionId" });
            DropIndex("Web.PurchaseOrderInspectionRequestHeaders", new[] { "DocTypeId" });
            DropIndex("Web.PurchaseOrderInspectionRequestLines", new[] { "PurchaseOrderLineId" });
            DropIndex("Web.PurchaseOrderInspectionRequestLines", new[] { "ProductUidId" });
            DropIndex("Web.PurchaseOrderInspectionRequestLines", new[] { "PurchaseOrderInspectionRequestHeaderId" });
            DropIndex("Web.PurchaseOrderInspectionLines", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.PurchaseOrderInspectionLines", new[] { "PurchaseOrderLineId" });
            DropIndex("Web.PurchaseOrderInspectionLines", new[] { "PurchaseOrderInspectionRequestLineId" });
            DropIndex("Web.PurchaseOrderInspectionLines", new[] { "ProductUidId" });
            DropIndex("Web.PurchaseOrderInspectionLines", new[] { "PurchaseOrderInspectionHeaderId" });
            DropIndex("Web.PurchaseOrderInspectionHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.PurchaseOrderInspectionHeaders", new[] { "InspectionById" });
            DropIndex("Web.PurchaseOrderInspectionHeaders", new[] { "SupplierId" });
            DropIndex("Web.PurchaseOrderInspectionHeaders", new[] { "ProcessId" });
            DropIndex("Web.PurchaseOrderInspectionHeaders", "IX_PurchaseOrderInspectionHeader_DocID");
            DropIndex("Web.PurchaseOrderHeaderStatus", new[] { "PurchaseOrderHeaderId" });
            DropIndex("Web.PurchaseOrderHeaderCharges", new[] { "ParentChargeId" });
            DropIndex("Web.PurchaseOrderHeaderCharges", new[] { "CostCenterId" });
            DropIndex("Web.PurchaseOrderHeaderCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.PurchaseOrderHeaderCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.PurchaseOrderHeaderCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.PurchaseOrderHeaderCharges", new[] { "PersonID" });
            DropIndex("Web.PurchaseOrderHeaderCharges", new[] { "CalculateOnId" });
            DropIndex("Web.PurchaseOrderHeaderCharges", new[] { "ProductChargeId" });
            DropIndex("Web.PurchaseOrderHeaderCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.PurchaseOrderHeaderCharges", new[] { "ChargeId" });
            DropIndex("Web.PurchaseOrderHeaderCharges", new[] { "HeaderTableId" });
            DropIndex("Web.PurchaseOrderAmendmentHeaderCharges", new[] { "ParentChargeId" });
            DropIndex("Web.PurchaseOrderAmendmentHeaderCharges", new[] { "CostCenterId" });
            DropIndex("Web.PurchaseOrderAmendmentHeaderCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.PurchaseOrderAmendmentHeaderCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.PurchaseOrderAmendmentHeaderCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.PurchaseOrderAmendmentHeaderCharges", new[] { "PersonID" });
            DropIndex("Web.PurchaseOrderAmendmentHeaderCharges", new[] { "CalculateOnId" });
            DropIndex("Web.PurchaseOrderAmendmentHeaderCharges", new[] { "ProductChargeId" });
            DropIndex("Web.PurchaseOrderAmendmentHeaderCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.PurchaseOrderAmendmentHeaderCharges", new[] { "ChargeId" });
            DropIndex("Web.PurchaseOrderAmendmentHeaderCharges", new[] { "HeaderTableId" });
            DropIndex("Web.PurchaseOrderAmendmentHeaders", new[] { "SupplierId" });
            DropIndex("Web.PurchaseOrderAmendmentHeaders", new[] { "SiteId" });
            DropIndex("Web.PurchaseOrderAmendmentHeaders", new[] { "DivisionId" });
            DropIndex("Web.PurchaseOrderAmendmentHeaders", new[] { "DocTypeId" });
            DropIndex("Web.PurchaseInvoiceSettings", new[] { "ImportMenuId" });
            DropIndex("Web.PurchaseInvoiceSettings", new[] { "UnitConversionForId" });
            DropIndex("Web.PurchaseInvoiceSettings", new[] { "CalculationId" });
            DropIndex("Web.PurchaseInvoiceSettings", new[] { "PurchaseGoodsReceiptDocTypeId" });
            DropIndex("Web.PurchaseInvoiceSettings", new[] { "DocTypeGoodsReturnId" });
            DropIndex("Web.PurchaseInvoiceSettings", new[] { "DivisionId" });
            DropIndex("Web.PurchaseInvoiceSettings", new[] { "SiteId" });
            DropIndex("Web.PurchaseInvoiceSettings", new[] { "DocTypeId" });
            DropIndex("Web.PurchaseInvoiceReturnLineCharges", new[] { "ParentChargeId" });
            DropIndex("Web.PurchaseInvoiceReturnLineCharges", new[] { "CostCenterId" });
            DropIndex("Web.PurchaseInvoiceReturnLineCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.PurchaseInvoiceReturnLineCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.PurchaseInvoiceReturnLineCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.PurchaseInvoiceReturnLineCharges", new[] { "PersonID" });
            DropIndex("Web.PurchaseInvoiceReturnLineCharges", new[] { "CalculateOnId" });
            DropIndex("Web.PurchaseInvoiceReturnLineCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.PurchaseInvoiceReturnLineCharges", new[] { "ChargeId" });
            DropIndex("Web.PurchaseInvoiceReturnLineCharges", new[] { "LineTableId" });
            DropIndex("Web.PurchaseInvoiceReturnLines", new[] { "DealUnitId" });
            DropIndex("Web.PurchaseInvoiceReturnLines", new[] { "PurchaseGoodsReturnLineId" });
            DropIndex("Web.PurchaseInvoiceReturnLines", new[] { "PurchaseInvoiceLineId" });
            DropIndex("Web.PurchaseInvoiceReturnLines", new[] { "PurchaseInvoiceReturnHeaderId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaderCharges", new[] { "ParentChargeId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaderCharges", new[] { "CostCenterId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaderCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaderCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaderCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaderCharges", new[] { "PersonID" });
            DropIndex("Web.PurchaseInvoiceReturnHeaderCharges", new[] { "CalculateOnId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaderCharges", new[] { "ProductChargeId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaderCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaderCharges", new[] { "ChargeId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaderCharges", new[] { "HeaderTableId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaders", new[] { "PurchaseGoodsReturnHeaderId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaders", new[] { "CurrencyId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaders", new[] { "SalesTaxGroupPartyId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaders", new[] { "SalesTaxGroupId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaders", new[] { "SupplierId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaders", new[] { "LedgerHeaderId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaders", new[] { "ReasonId" });
            DropIndex("Web.PurchaseInvoiceReturnHeaders", "IX_PurchaseInvoiceReturnHeader_DocID");
            DropIndex("Web.PurchaseInvoiceRateAmendmentLines", new[] { "PurchaseInvoiceLineId" });
            DropIndex("Web.PurchaseInvoiceRateAmendmentLines", new[] { "PurchaseInvoiceAmendmentHeaderId" });
            DropIndex("Web.PurchaseInvoiceLineCharges", new[] { "ParentChargeId" });
            DropIndex("Web.PurchaseInvoiceLineCharges", new[] { "CostCenterId" });
            DropIndex("Web.PurchaseInvoiceLineCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.PurchaseInvoiceLineCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.PurchaseInvoiceLineCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.PurchaseInvoiceLineCharges", new[] { "PersonID" });
            DropIndex("Web.PurchaseInvoiceLineCharges", new[] { "CalculateOnId" });
            DropIndex("Web.PurchaseInvoiceLineCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.PurchaseInvoiceLineCharges", new[] { "ChargeId" });
            DropIndex("Web.PurchaseInvoiceLineCharges", new[] { "LineTableId" });
            DropIndex("Web.PurchaseInvoiceHeaderCharges", new[] { "ParentChargeId" });
            DropIndex("Web.PurchaseInvoiceHeaderCharges", new[] { "CostCenterId" });
            DropIndex("Web.PurchaseInvoiceHeaderCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.PurchaseInvoiceHeaderCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.PurchaseInvoiceHeaderCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.PurchaseInvoiceHeaderCharges", new[] { "PersonID" });
            DropIndex("Web.PurchaseInvoiceHeaderCharges", new[] { "CalculateOnId" });
            DropIndex("Web.PurchaseInvoiceHeaderCharges", new[] { "ProductChargeId" });
            DropIndex("Web.PurchaseInvoiceHeaderCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.PurchaseInvoiceHeaderCharges", new[] { "ChargeId" });
            DropIndex("Web.PurchaseInvoiceHeaderCharges", new[] { "HeaderTableId" });
            DropIndex("Web.PurchaseInvoiceLines", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.PurchaseInvoiceLines", new[] { "DealUnitId" });
            DropIndex("Web.PurchaseInvoiceLines", new[] { "SalesTaxGroupId" });
            DropIndex("Web.PurchaseInvoiceLines", new[] { "PurchaseGoodsReceiptLineId" });
            DropIndex("Web.PurchaseInvoiceLines", new[] { "PurchaseInvoiceHeaderId" });
            DropIndex("Web.PurchaseInvoiceHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.PurchaseInvoiceHeaders", new[] { "ShipMethodId" });
            DropIndex("Web.PurchaseInvoiceHeaders", new[] { "DeliveryTermsId" });
            DropIndex("Web.PurchaseInvoiceHeaders", new[] { "PurchaseGoodsReceiptHeaderId" });
            DropIndex("Web.PurchaseInvoiceHeaders", new[] { "UnitConversionForId" });
            DropIndex("Web.PurchaseInvoiceHeaders", new[] { "SalesTaxGroupPartyId" });
            DropIndex("Web.PurchaseInvoiceHeaders", new[] { "SalesTaxGroupId" });
            DropIndex("Web.PurchaseInvoiceHeaders", new[] { "BillingAccountId" });
            DropIndex("Web.PurchaseInvoiceHeaders", new[] { "SupplierId" });
            DropIndex("Web.PurchaseInvoiceHeaders", new[] { "CurrencyId" });
            DropIndex("Web.PurchaseInvoiceHeaders", new[] { "LedgerHeaderId" });
            DropIndex("Web.PurchaseInvoiceHeaders", "IX_PurchaseInvoiceHeader_DocID");
            DropIndex("Web.PurchaseInvoiceAmendmentHeaders", new[] { "SupplierId" });
            DropIndex("Web.PurchaseInvoiceAmendmentHeaders", new[] { "SiteId" });
            DropIndex("Web.PurchaseInvoiceAmendmentHeaders", new[] { "DivisionId" });
            DropIndex("Web.PurchaseInvoiceAmendmentHeaders", new[] { "DocTypeId" });
            DropIndex("Web.PurchaseIndentSettings", new[] { "DivisionId" });
            DropIndex("Web.PurchaseIndentSettings", new[] { "SiteId" });
            DropIndex("Web.PurchaseIndentSettings", new[] { "DocTypeId" });
            DropIndex("Web.PurchaseIndentLineStatus", new[] { "PurchaseIndentLineId" });
            DropIndex("Web.PurchaseGoodsReturnLines", new[] { "ProductUidCurrentProcessId" });
            DropIndex("Web.PurchaseGoodsReturnLines", new[] { "ProductUidCurrentGodownId" });
            DropIndex("Web.PurchaseGoodsReturnLines", new[] { "ProductUidLastTransactionPersonId" });
            DropIndex("Web.PurchaseGoodsReturnLines", new[] { "ProductUidLastTransactionDocTypeId" });
            DropIndex("Web.PurchaseGoodsReturnLines", new[] { "StockId" });
            DropIndex("Web.PurchaseGoodsReturnLines", new[] { "DealUnitId" });
            DropIndex("Web.PurchaseGoodsReturnLines", new[] { "PurchaseGoodsReceiptLineId" });
            DropIndex("Web.PurchaseGoodsReturnLines", new[] { "PurchaseGoodsReturnHeaderId" });
            DropIndex("Web.PurchaseGoodsReturnHeaders", new[] { "GodownId" });
            DropIndex("Web.PurchaseGoodsReturnHeaders", new[] { "StockHeaderId" });
            DropIndex("Web.PurchaseGoodsReturnHeaders", new[] { "GatePassHeaderId" });
            DropIndex("Web.PurchaseGoodsReturnHeaders", new[] { "SupplierId" });
            DropIndex("Web.PurchaseGoodsReturnHeaders", new[] { "ReasonId" });
            DropIndex("Web.PurchaseGoodsReturnHeaders", "IX_PurchaseGoodsReturnHeader_DocID");
            DropIndex("Web.PurchaseGoodsReceiptSettings", new[] { "ImportMenuId" });
            DropIndex("Web.PurchaseGoodsReceiptSettings", new[] { "UnitConversionForId" });
            DropIndex("Web.PurchaseGoodsReceiptSettings", new[] { "DivisionId" });
            DropIndex("Web.PurchaseGoodsReceiptSettings", new[] { "SiteId" });
            DropIndex("Web.PurchaseGoodsReceiptSettings", new[] { "DocTypeId" });
            DropIndex("Web.ProductUidSiteDetails", new[] { "CurrenctProcessId" });
            DropIndex("Web.ProductUidSiteDetails", new[] { "CurrenctGodownId" });
            DropIndex("Web.ProductUidSiteDetails", new[] { "LastTransactionPersonId" });
            DropIndex("Web.ProductUidSiteDetails", new[] { "LastTransactionDocTypeId" });
            DropIndex("Web.ProductUidSiteDetails", new[] { "GenPersonId" });
            DropIndex("Web.ProductUidSiteDetails", new[] { "GenDocTypeId" });
            DropIndex("Web.ProductUidSiteDetails", new[] { "Dimension2Id" });
            DropIndex("Web.ProductUidSiteDetails", new[] { "Dimension1Id" });
            DropIndex("Web.ProductUidSiteDetails", new[] { "ProductId" });
            DropIndex("Web.ProductUidSiteDetails", new[] { "ProductUidHeaderId" });
            DropIndex("Web.Tags", "IX_Tag_TagName");
            DropIndex("Web.ProductTags", new[] { "TagId" });
            DropIndex("Web.ProductTags", new[] { "ProductId" });
            DropIndex("Web.ProductSiteDetails", new[] { "GodownId" });
            DropIndex("Web.ProductSiteDetails", new[] { "SiteId" });
            DropIndex("Web.ProductSiteDetails", new[] { "DivisionId" });
            DropIndex("Web.ProductSiteDetails", new[] { "ProductId" });
            DropIndex("Web.ProductShapes", "IX_ProductShape_ProductShapeName");
            DropIndex("Web.ProductProcesses", new[] { "ProductRateGroupId" });
            DropIndex("Web.ProductProcesses", new[] { "Dimension2Id" });
            DropIndex("Web.ProductProcesses", new[] { "Dimension1Id" });
            DropIndex("Web.ProductProcesses", new[] { "ProcessId" });
            DropIndex("Web.ProductProcesses", new[] { "ProductId" });
            DropIndex("Web.ProductionOrderSettings", new[] { "DivisionId" });
            DropIndex("Web.ProductionOrderSettings", new[] { "SiteId" });
            DropIndex("Web.ProductionOrderSettings", new[] { "DocTypeId" });
            DropIndex("Web.ProductCustomGroupLines", new[] { "ProductId" });
            DropIndex("Web.ProductCustomGroupLines", new[] { "ProductCustomGroupHeaderId" });
            DropIndex("Web.ProductCustomGroupHeaders", "IX_ProductCustomGroup_ProductCustomGroupName");
            DropIndex("Web.ProductAlias", new[] { "ProductId" });
            DropIndex("Web.ProductAlias", "IX_ProductAlias_ProductAliasName");
            DropIndex("Web.ProdOrderSettings", new[] { "DivisionId" });
            DropIndex("Web.ProdOrderSettings", new[] { "SiteId" });
            DropIndex("Web.ProdOrderSettings", new[] { "DocTypeId" });
            DropIndex("Web.ProdOrderLineStatus", new[] { "ProdOrderLineId" });
            DropIndex("Web.ProdOrderHeaderStatus", new[] { "ProdOrderHeaderId" });
            DropIndex("Web.ProdOrderCancelLines", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.ProdOrderCancelLines", new[] { "ProdOrderLineId" });
            DropIndex("Web.ProdOrderCancelLines", new[] { "ProdOrderCancelHeaderId" });
            DropIndex("Web.ProdOrderCancelHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.ProdOrderCancelHeaders", "IX_ProdOrderCancelHeader_DocID");
            DropIndex("Web.ProcessSettings", new[] { "RateListMenuId" });
            DropIndex("Web.ProcessSettings", "IX_ProcessSetting_UniqueKey");
            DropIndex("Web.ProductRateGroups", "IX_ProductRateGroup_ProductRateGroupName");
            DropIndex("Web.ProcessSequenceLines", new[] { "ProductRateGroupId" });
            DropIndex("Web.ProcessSequenceLines", new[] { "ProcessId" });
            DropIndex("Web.ProcessSequenceLines", new[] { "ProcessSequenceHeaderId" });
            DropIndex("Web.PersonRegistrations", new[] { "PersonId" });
            DropIndex("Web.PersonProcesses", new[] { "PersonRateGroupId" });
            DropIndex("Web.PersonProcesses", new[] { "ProcessId" });
            DropIndex("Web.PersonProcesses", new[] { "PersonId" });
            DropIndex("Web.PersonDocuments", new[] { "PersonId" });
            DropIndex("Web.PersonCustomGroupLines", new[] { "PersonId" });
            DropIndex("Web.PersonCustomGroupLines", new[] { "PersonCustomGroupHeaderId" });
            DropIndex("Web.PersonCustomGroupHeaders", "IX_PersonCustomGroup_PersonCustomGroupName");
            DropIndex("Web.PersonBankAccounts", new[] { "PersonId" });
            DropIndex("Web.PerkDocumentTypes", new[] { "RateDocTypeId" });
            DropIndex("Web.PerkDocumentTypes", new[] { "PerkId" });
            DropIndex("Web.PerkDocumentTypes", new[] { "SiteId" });
            DropIndex("Web.PerkDocumentTypes", new[] { "DivisionId" });
            DropIndex("Web.PerkDocumentTypes", new[] { "DocTypeId" });
            DropIndex("Web.PackingSettings", new[] { "ImportMenuId" });
            DropIndex("Web.PackingSettings", new[] { "DivisionId" });
            DropIndex("Web.PackingSettings", new[] { "SiteId" });
            DropIndex("Web.PackingSettings", new[] { "DocTypeId" });
            DropIndex("Web.Narrations", "IX_Narration_NarrationName");
            DropIndex("Web.MaterialRequestSettings", new[] { "DivisionId" });
            DropIndex("Web.MaterialRequestSettings", new[] { "SiteId" });
            DropIndex("Web.MaterialRequestSettings", new[] { "DocTypeId" });
            DropIndex("Web.MaterialReceiveSettings", new[] { "ProcessId" });
            DropIndex("Web.MaterialReceiveSettings", new[] { "DivisionId" });
            DropIndex("Web.MaterialReceiveSettings", new[] { "SiteId" });
            DropIndex("Web.MaterialReceiveSettings", new[] { "DocTypeId" });
            DropIndex("Web.MaterialPlanSettings", new[] { "GodownId" });
            DropIndex("Web.MaterialPlanSettings", new[] { "DocTypeProductionOrderId" });
            DropIndex("Web.MaterialPlanSettings", new[] { "DocTypePurchaseIndentId" });
            DropIndex("Web.MaterialPlanSettings", new[] { "DivisionId" });
            DropIndex("Web.MaterialPlanSettings", new[] { "SiteId" });
            DropIndex("Web.MaterialPlanSettings", new[] { "DocTypeId" });
            DropIndex("Web.MaterialPlanForSaleOrderLines", new[] { "MaterialPlanLineId" });
            DropIndex("Web.MaterialPlanForSaleOrderLines", new[] { "Dimension2Id" });
            DropIndex("Web.MaterialPlanForSaleOrderLines", new[] { "Dimension1Id" });
            DropIndex("Web.MaterialPlanForSaleOrderLines", new[] { "ProductId" });
            DropIndex("Web.MaterialPlanForSaleOrderLines", new[] { "MaterialPlanForSaleOrderId" });
            DropIndex("Web.MaterialPlanForSaleOrders", new[] { "MaterialPlanLineId" });
            DropIndex("Web.MaterialPlanForSaleOrders", new[] { "SaleOrderLineId" });
            DropIndex("Web.MaterialPlanForSaleOrders", new[] { "MaterialPlanHeaderId" });
            DropIndex("Web.MaterialPlanForProdOrderLines", new[] { "MaterialPlanLineId" });
            DropIndex("Web.MaterialPlanForProdOrderLines", new[] { "ProcessId" });
            DropIndex("Web.MaterialPlanForProdOrderLines", new[] { "Dimension2Id" });
            DropIndex("Web.MaterialPlanForProdOrderLines", new[] { "Dimension1Id" });
            DropIndex("Web.MaterialPlanForProdOrderLines", new[] { "ProductId" });
            DropIndex("Web.MaterialPlanForProdOrderLines", new[] { "MaterialPlanForProdOrderId" });
            DropIndex("Web.MaterialPlanForProdOrders", new[] { "ProdOrderLineId" });
            DropIndex("Web.MaterialPlanForProdOrders", new[] { "MaterialPlanHeaderId" });
            DropIndex("Web.MaterialPlanForJobOrderLines", new[] { "MaterialPlanLineId" });
            DropIndex("Web.MaterialPlanForJobOrderLines", new[] { "Dimension2Id" });
            DropIndex("Web.MaterialPlanForJobOrderLines", new[] { "Dimension1Id" });
            DropIndex("Web.MaterialPlanForJobOrderLines", new[] { "ProductId" });
            DropIndex("Web.MaterialPlanForJobOrderLines", new[] { "MaterialPlanForJobOrderId" });
            DropIndex("Web.MaterialPlanForJobOrders", new[] { "MaterialPlanLineId" });
            DropIndex("Web.MaterialPlanForJobOrders", new[] { "JobOrderLineId" });
            DropIndex("Web.MaterialPlanForJobOrders", new[] { "MaterialPlanHeaderId" });
            DropIndex("Web.StockHeaderSettings", new[] { "ImportMenuId" });
            DropIndex("Web.StockHeaderSettings", new[] { "OnSubmitMenuId" });
            DropIndex("Web.StockHeaderSettings", new[] { "ProcessId" });
            DropIndex("Web.StockHeaderSettings", new[] { "DivisionId" });
            DropIndex("Web.StockHeaderSettings", new[] { "SiteId" });
            DropIndex("Web.StockHeaderSettings", new[] { "DocTypeId" });
            DropIndex("Web.Manufacturers", new[] { "PersonID" });
            DropIndex("Web.LedgerSettings", new[] { "WizardMenuId" });
            DropIndex("Web.LedgerSettings", new[] { "ProcessId" });
            DropIndex("Web.LedgerSettings", new[] { "DivisionId" });
            DropIndex("Web.LedgerSettings", new[] { "SiteId" });
            DropIndex("Web.LedgerSettings", new[] { "DocTypeId" });
            DropIndex("Web.LedgerLineRefValues", new[] { "LedgerLineId" });
            DropIndex("Web.LedgerAdjs", new[] { "SiteId" });
            DropIndex("Web.LedgerAdjs", new[] { "CrLedgerId" });
            DropIndex("Web.LedgerAdjs", new[] { "DrLedgerId" });
            DropIndex("Web.LedgerAdjs", new[] { "LedgerId" });
            DropIndex("Web.LedgerLines", new[] { "CostCenterId" });
            DropIndex("Web.LedgerLines", new[] { "LedgerAccountId" });
            DropIndex("Web.LedgerLines", new[] { "LedgerHeaderId" });
            DropIndex("Web.Ledgers", new[] { "CostCenterId" });
            DropIndex("Web.Ledgers", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.Ledgers", new[] { "LedgerAccountId" });
            DropIndex("Web.Ledgers", new[] { "LedgerLineId" });
            DropIndex("Web.Ledgers", new[] { "LedgerHeaderId" });
            DropIndex("Web.LeaveTypes", new[] { "SiteId" });
            DropIndex("Web.LeaveTypes", "IX_LeaveType_LeaveTypeName");
            DropIndex("Web.JobReturnBoms", new[] { "StockProcessId" });
            DropIndex("Web.JobReturnBoms", new[] { "CostCenterId" });
            DropIndex("Web.JobReturnBoms", new[] { "Dimension2Id" });
            DropIndex("Web.JobReturnBoms", new[] { "Dimension1Id" });
            DropIndex("Web.JobReturnBoms", new[] { "ProductId" });
            DropIndex("Web.JobReturnBoms", new[] { "JobReturnLineId" });
            DropIndex("Web.JobReturnBoms", new[] { "JobReturnHeaderId" });
            DropIndex("Web.JobReceiveSettings", new[] { "CalculationId" });
            DropIndex("Web.JobReceiveSettings", new[] { "OnApproveMenuId" });
            DropIndex("Web.JobReceiveSettings", new[] { "OnSubmitMenuId" });
            DropIndex("Web.JobReceiveSettings", new[] { "WizardMenuId" });
            DropIndex("Web.JobReceiveSettings", new[] { "ImportMenuId" });
            DropIndex("Web.JobReceiveSettings", new[] { "ProcessId" });
            DropIndex("Web.JobReceiveSettings", new[] { "DivisionId" });
            DropIndex("Web.JobReceiveSettings", new[] { "SiteId" });
            DropIndex("Web.JobReceiveSettings", new[] { "DocTypeId" });
            DropIndex("Web.JobReceiveQASettings", new[] { "DivisionId" });
            DropIndex("Web.JobReceiveQASettings", new[] { "SiteId" });
            DropIndex("Web.JobReceiveQASettings", new[] { "ProcessId" });
            DropIndex("Web.JobReceiveQASettings", new[] { "WizardMenuId" });
            DropIndex("Web.JobReceiveQASettings", new[] { "ImportMenuId" });
            DropIndex("Web.JobReceiveQASettings", new[] { "DocTypeId" });
            DropIndex("Web.JobReceiveQALines", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.JobReceiveQALines", new[] { "JobReceiveLineId" });
            DropIndex("Web.JobReceiveQALines", new[] { "ProductUidId" });
            DropIndex("Web.JobReceiveQALines", new[] { "JobReceiveQAHeaderId" });
            DropIndex("Web.JobReceiveQAHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.JobReceiveQAHeaders", new[] { "QAById" });
            DropIndex("Web.JobReceiveQAHeaders", new[] { "JobWorkerId" });
            DropIndex("Web.JobReceiveQAHeaders", new[] { "ProcessId" });
            DropIndex("Web.JobReceiveQAHeaders", "IX_JobReceiveQAHeader_DocID");
            DropIndex("Web.JobReceiveLineStatus", new[] { "JobReceiveLineId" });
            DropIndex("Web.JobReceiveByProducts", new[] { "Dimension2Id" });
            DropIndex("Web.JobReceiveByProducts", new[] { "Dimension1Id" });
            DropIndex("Web.JobReceiveByProducts", new[] { "ProductId" });
            DropIndex("Web.JobReceiveByProducts", new[] { "JobReceiveHeaderId" });
            DropIndex("Web.JobReceiveBoms", new[] { "StockProcessId" });
            DropIndex("Web.JobReceiveBoms", new[] { "CostCenterId" });
            DropIndex("Web.JobReceiveBoms", new[] { "Dimension2Id" });
            DropIndex("Web.JobReceiveBoms", new[] { "Dimension1Id" });
            DropIndex("Web.JobReceiveBoms", new[] { "ProductId" });
            DropIndex("Web.JobReceiveBoms", new[] { "JobReceiveLineId" });
            DropIndex("Web.JobReceiveBoms", new[] { "JobReceiveHeaderId" });
            DropIndex("Web.JobOrderSettings", new[] { "DealUnitId" });
            DropIndex("Web.JobOrderSettings", new[] { "OnApproveMenuId" });
            DropIndex("Web.JobOrderSettings", new[] { "OnSubmitMenuId" });
            DropIndex("Web.JobOrderSettings", new[] { "JobUnitId" });
            DropIndex("Web.JobOrderSettings", new[] { "WizardMenuId" });
            DropIndex("Web.JobOrderSettings", new[] { "ImportMenuId" });
            DropIndex("Web.JobOrderSettings", new[] { "CalculationId" });
            DropIndex("Web.JobOrderSettings", new[] { "ProcessId" });
            DropIndex("Web.JobOrderSettings", new[] { "UnitConversionForId" });
            DropIndex("Web.JobOrderSettings", new[] { "DivisionId" });
            DropIndex("Web.JobOrderSettings", new[] { "SiteId" });
            DropIndex("Web.JobOrderSettings", new[] { "DocTypeId" });
            DropIndex("Web.JobOrderRateAmendmentLines", new[] { "JobWorkerId" });
            DropIndex("Web.JobOrderRateAmendmentLines", new[] { "JobOrderLineId" });
            DropIndex("Web.JobOrderRateAmendmentLines", new[] { "JobOrderAmendmentHeaderId" });
            DropIndex("Web.JobOrderQtyAmendmentLines", new[] { "JobOrderLineId" });
            DropIndex("Web.JobOrderQtyAmendmentLines", new[] { "JobOrderAmendmentHeaderId" });
            DropIndex("Web.Perks", "IX_Perk_Perk");
            DropIndex("Web.JobOrderPerks", new[] { "PerkId" });
            DropIndex("Web.JobOrderPerks", new[] { "JobOrderHeaderId" });
            DropIndex("Web.JobOrderLineStatus", new[] { "JobOrderLineId" });
            DropIndex("Web.JobOrderLineExtendeds", new[] { "OtherUnitId" });
            DropIndex("Web.JobOrderLineExtendeds", new[] { "JobOrderLineId" });
            DropIndex("Web.JobOrderLineCharges", new[] { "ParentChargeId" });
            DropIndex("Web.JobOrderLineCharges", new[] { "CostCenterId" });
            DropIndex("Web.JobOrderLineCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.JobOrderLineCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.JobOrderLineCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.JobOrderLineCharges", new[] { "PersonID" });
            DropIndex("Web.JobOrderLineCharges", new[] { "CalculateOnId" });
            DropIndex("Web.JobOrderLineCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.JobOrderLineCharges", new[] { "ChargeId" });
            DropIndex("Web.JobOrderLineCharges", new[] { "LineTableId" });
            DropIndex("Web.JobOrderJobOrders", new[] { "GenJobOrderHeaderId" });
            DropIndex("Web.JobOrderJobOrders", new[] { "JobOrderHeaderId" });
            DropIndex("Web.JobOrderInspectionSettings", new[] { "DivisionId" });
            DropIndex("Web.JobOrderInspectionSettings", new[] { "SiteId" });
            DropIndex("Web.JobOrderInspectionSettings", new[] { "ProcessId" });
            DropIndex("Web.JobOrderInspectionSettings", new[] { "ImportMenuId" });
            DropIndex("Web.JobOrderInspectionSettings", new[] { "DocTypeId" });
            DropIndex("Web.JobOrderInspectionRequestSettings", new[] { "DivisionId" });
            DropIndex("Web.JobOrderInspectionRequestSettings", new[] { "SiteId" });
            DropIndex("Web.JobOrderInspectionRequestSettings", new[] { "ProcessId" });
            DropIndex("Web.JobOrderInspectionRequestSettings", new[] { "ImportMenuId" });
            DropIndex("Web.JobOrderInspectionRequestSettings", new[] { "DocTypeId" });
            DropIndex("Web.JobOrderInspectionRequestCancelLines", new[] { "JobOrderInspectionRequestLineId" });
            DropIndex("Web.JobOrderInspectionRequestCancelLines", new[] { "ProductUidId" });
            DropIndex("Web.JobOrderInspectionRequestCancelLines", new[] { "JobOrderInspectionRequestCancelHeaderId" });
            DropIndex("Web.JobOrderInspectionRequestCancelHeaders", new[] { "ReasonId" });
            DropIndex("Web.JobOrderInspectionRequestCancelHeaders", new[] { "JobWorkerId" });
            DropIndex("Web.JobOrderInspectionRequestCancelHeaders", new[] { "ProcessId" });
            DropIndex("Web.JobOrderInspectionRequestCancelHeaders", new[] { "SiteId" });
            DropIndex("Web.JobOrderInspectionRequestCancelHeaders", new[] { "DivisionId" });
            DropIndex("Web.JobOrderInspectionRequestCancelHeaders", new[] { "DocTypeId" });
            DropIndex("Web.JobOrderInspectionRequestHeaders", new[] { "JobWorkerId" });
            DropIndex("Web.JobOrderInspectionRequestHeaders", new[] { "ProcessId" });
            DropIndex("Web.JobOrderInspectionRequestHeaders", new[] { "SiteId" });
            DropIndex("Web.JobOrderInspectionRequestHeaders", new[] { "DivisionId" });
            DropIndex("Web.JobOrderInspectionRequestHeaders", new[] { "DocTypeId" });
            DropIndex("Web.JobOrderInspectionRequestLines", new[] { "JobOrderLineId" });
            DropIndex("Web.JobOrderInspectionRequestLines", new[] { "ProductUidId" });
            DropIndex("Web.JobOrderInspectionRequestLines", new[] { "JobOrderInspectionRequestHeaderId" });
            DropIndex("Web.JobOrderInspectionLines", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.JobOrderInspectionLines", new[] { "JobOrderLineId" });
            DropIndex("Web.JobOrderInspectionLines", new[] { "JobOrderInspectionRequestLineId" });
            DropIndex("Web.JobOrderInspectionLines", new[] { "ProductUidId" });
            DropIndex("Web.JobOrderInspectionLines", new[] { "JobOrderInspectionHeaderId" });
            DropIndex("Web.JobOrderInspectionHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.JobOrderInspectionHeaders", new[] { "InspectionById" });
            DropIndex("Web.JobOrderInspectionHeaders", new[] { "JobWorkerId" });
            DropIndex("Web.JobOrderInspectionHeaders", new[] { "ProcessId" });
            DropIndex("Web.JobOrderInspectionHeaders", "IX_JobOrderInspectionHeader_DocID");
            DropIndex("Web.JobOrderHeaderStatus", new[] { "JobOrderHeaderId" });
            DropIndex("Web.JobOrderHeaderCharges", new[] { "ParentChargeId" });
            DropIndex("Web.JobOrderHeaderCharges", new[] { "CostCenterId" });
            DropIndex("Web.JobOrderHeaderCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.JobOrderHeaderCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.JobOrderHeaderCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.JobOrderHeaderCharges", new[] { "PersonID" });
            DropIndex("Web.JobOrderHeaderCharges", new[] { "CalculateOnId" });
            DropIndex("Web.JobOrderHeaderCharges", new[] { "ProductChargeId" });
            DropIndex("Web.JobOrderHeaderCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.JobOrderHeaderCharges", new[] { "ChargeId" });
            DropIndex("Web.JobOrderHeaderCharges", new[] { "HeaderTableId" });
            DropIndex("Web.JobOrderCancelLines", new[] { "ProductUidCurrentProcessId" });
            DropIndex("Web.JobOrderCancelLines", new[] { "ProductUidCurrentGodownId" });
            DropIndex("Web.JobOrderCancelLines", new[] { "ProductUidLastTransactionPersonId" });
            DropIndex("Web.JobOrderCancelLines", new[] { "ProductUidLastTransactionDocTypeId" });
            DropIndex("Web.JobOrderCancelLines", new[] { "StockProcessId" });
            DropIndex("Web.JobOrderCancelLines", new[] { "StockId" });
            DropIndex("Web.JobOrderCancelLines", new[] { "JobOrderLineId" });
            DropIndex("Web.JobOrderCancelLines", new[] { "ProductUidId" });
            DropIndex("Web.JobOrderCancelLines", new[] { "JobOrderCancelHeaderId" });
            DropIndex("Web.JobOrderCancelHeaders", new[] { "StockHeaderId" });
            DropIndex("Web.JobOrderCancelHeaders", new[] { "GodownId" });
            DropIndex("Web.JobOrderCancelHeaders", new[] { "OrderById" });
            DropIndex("Web.JobOrderCancelHeaders", new[] { "JobWorkerId" });
            DropIndex("Web.JobOrderCancelHeaders", new[] { "ProcessId" });
            DropIndex("Web.JobOrderCancelHeaders", new[] { "SiteId" });
            DropIndex("Web.JobOrderCancelHeaders", new[] { "DivisionId" });
            DropIndex("Web.JobOrderCancelHeaders", new[] { "ReasonId" });
            DropIndex("Web.JobOrderCancelHeaders", new[] { "DocTypeId" });
            DropIndex("Web.JobOrderCancelBoms", new[] { "Dimension2Id" });
            DropIndex("Web.JobOrderCancelBoms", new[] { "Dimension1Id" });
            DropIndex("Web.JobOrderCancelBoms", new[] { "ProductId" });
            DropIndex("Web.JobOrderCancelBoms", new[] { "JobOrderHeaderId" });
            DropIndex("Web.JobOrderCancelBoms", new[] { "JobOrderCancelLineId" });
            DropIndex("Web.JobOrderCancelBoms", new[] { "JobOrderCancelHeaderId" });
            DropIndex("Web.JobOrderByProducts", new[] { "ProductId" });
            DropIndex("Web.JobOrderByProducts", new[] { "JobOrderHeaderId" });
            DropIndex("Web.JobOrderAmendmentHeaders", new[] { "LedgerHeaderId" });
            DropIndex("Web.JobOrderAmendmentHeaders", new[] { "ProcessId" });
            DropIndex("Web.JobOrderAmendmentHeaders", new[] { "OrderById" });
            DropIndex("Web.JobOrderAmendmentHeaders", new[] { "JobWorkerId" });
            DropIndex("Web.JobOrderAmendmentHeaders", new[] { "SiteId" });
            DropIndex("Web.JobOrderAmendmentHeaders", new[] { "DivisionId" });
            DropIndex("Web.JobOrderAmendmentHeaders", new[] { "DocTypeId" });
            DropIndex("Web.JobInvoiceSettings", new[] { "JobReturnDocTypeId" });
            DropIndex("Web.JobInvoiceSettings", new[] { "JobReceiveDocTypeId" });
            DropIndex("Web.JobInvoiceSettings", new[] { "CalculationId" });
            DropIndex("Web.JobInvoiceSettings", new[] { "WizardMenuId" });
            DropIndex("Web.JobInvoiceSettings", new[] { "ImportMenuId" });
            DropIndex("Web.JobInvoiceSettings", new[] { "ProcessId" });
            DropIndex("Web.JobInvoiceSettings", new[] { "DivisionId" });
            DropIndex("Web.JobInvoiceSettings", new[] { "SiteId" });
            DropIndex("Web.JobInvoiceSettings", new[] { "DocTypeId" });
            DropIndex("Web.JobInvoiceReturnLineCharges", new[] { "ParentChargeId" });
            DropIndex("Web.JobInvoiceReturnLineCharges", new[] { "CostCenterId" });
            DropIndex("Web.JobInvoiceReturnLineCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.JobInvoiceReturnLineCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.JobInvoiceReturnLineCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.JobInvoiceReturnLineCharges", new[] { "PersonID" });
            DropIndex("Web.JobInvoiceReturnLineCharges", new[] { "CalculateOnId" });
            DropIndex("Web.JobInvoiceReturnLineCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.JobInvoiceReturnLineCharges", new[] { "ChargeId" });
            DropIndex("Web.JobInvoiceReturnLineCharges", new[] { "LineTableId" });
            DropIndex("Web.JobInvoiceReturnLines", new[] { "DealUnitId" });
            DropIndex("Web.JobInvoiceReturnLines", new[] { "JobReturnLineId" });
            DropIndex("Web.JobInvoiceReturnLines", new[] { "JobInvoiceLineId" });
            DropIndex("Web.JobInvoiceReturnLines", new[] { "JobInvoiceReturnHeaderId" });
            DropIndex("Web.JobInvoiceReturnHeaderCharges", new[] { "ParentChargeId" });
            DropIndex("Web.JobInvoiceReturnHeaderCharges", new[] { "CostCenterId" });
            DropIndex("Web.JobInvoiceReturnHeaderCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.JobInvoiceReturnHeaderCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.JobInvoiceReturnHeaderCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.JobInvoiceReturnHeaderCharges", new[] { "PersonID" });
            DropIndex("Web.JobInvoiceReturnHeaderCharges", new[] { "CalculateOnId" });
            DropIndex("Web.JobInvoiceReturnHeaderCharges", new[] { "ProductChargeId" });
            DropIndex("Web.JobInvoiceReturnHeaderCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.JobInvoiceReturnHeaderCharges", new[] { "ChargeId" });
            DropIndex("Web.JobInvoiceReturnHeaderCharges", new[] { "HeaderTableId" });
            DropIndex("Web.JobReturnLines", new[] { "ProductUidCurrentProcessId" });
            DropIndex("Web.JobReturnLines", new[] { "ProductUidCurrentGodownId" });
            DropIndex("Web.JobReturnLines", new[] { "ProductUidLastTransactionPersonId" });
            DropIndex("Web.JobReturnLines", new[] { "ProductUidLastTransactionDocTypeId" });
            DropIndex("Web.JobReturnLines", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.JobReturnLines", new[] { "StockProcessId" });
            DropIndex("Web.JobReturnLines", new[] { "StockId" });
            DropIndex("Web.JobReturnLines", new[] { "DealUnitId" });
            DropIndex("Web.JobReturnLines", new[] { "JobReceiveLineId" });
            DropIndex("Web.JobReturnLines", new[] { "JobReturnHeaderId" });
            DropIndex("Web.JobReturnHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.JobReturnHeaders", new[] { "StockHeaderId" });
            DropIndex("Web.JobReturnHeaders", new[] { "GatePassHeaderId" });
            DropIndex("Web.JobReturnHeaders", new[] { "GodownId" });
            DropIndex("Web.JobReturnHeaders", new[] { "OrderById" });
            DropIndex("Web.JobReturnHeaders", new[] { "JobWorkerId" });
            DropIndex("Web.JobReturnHeaders", new[] { "ProcessId" });
            DropIndex("Web.JobReturnHeaders", new[] { "SiteId" });
            DropIndex("Web.JobReturnHeaders", new[] { "DivisionId" });
            DropIndex("Web.JobReturnHeaders", new[] { "ReasonId" });
            DropIndex("Web.JobReturnHeaders", new[] { "DocTypeId" });
            DropIndex("Web.JobInvoiceReturnHeaders", new[] { "JobReturnHeaderId" });
            DropIndex("Web.JobInvoiceReturnHeaders", new[] { "ProcessId" });
            DropIndex("Web.JobInvoiceReturnHeaders", new[] { "JobWorkerId" });
            DropIndex("Web.JobInvoiceReturnHeaders", new[] { "LedgerHeaderId" });
            DropIndex("Web.JobInvoiceReturnHeaders", new[] { "ReasonId" });
            DropIndex("Web.JobInvoiceReturnHeaders", "IX_JobInvoiceReturnHeader_DocID");
            DropIndex("Web.JobInvoiceRateAmendmentLineCharges", new[] { "ParentChargeId" });
            DropIndex("Web.JobInvoiceRateAmendmentLineCharges", new[] { "CostCenterId" });
            DropIndex("Web.JobInvoiceRateAmendmentLineCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.JobInvoiceRateAmendmentLineCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.JobInvoiceRateAmendmentLineCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.JobInvoiceRateAmendmentLineCharges", new[] { "PersonID" });
            DropIndex("Web.JobInvoiceRateAmendmentLineCharges", new[] { "CalculateOnId" });
            DropIndex("Web.JobInvoiceRateAmendmentLineCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.JobInvoiceRateAmendmentLineCharges", new[] { "ChargeId" });
            DropIndex("Web.JobInvoiceRateAmendmentLineCharges", new[] { "LineTableId" });
            DropIndex("Web.JobInvoiceRateAmendmentLines", new[] { "JobWorkerId" });
            DropIndex("Web.JobInvoiceRateAmendmentLines", new[] { "JobInvoiceLineId" });
            DropIndex("Web.JobInvoiceRateAmendmentLines", new[] { "JobInvoiceAmendmentHeaderId" });
            DropIndex("Web.JobInvoiceLineStatus", new[] { "JobInvoiceLineId" });
            DropIndex("Web.JobInvoiceLineCharges", new[] { "ParentChargeId" });
            DropIndex("Web.JobInvoiceLineCharges", new[] { "CostCenterId" });
            DropIndex("Web.JobInvoiceLineCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.JobInvoiceLineCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.JobInvoiceLineCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.JobInvoiceLineCharges", new[] { "PersonID" });
            DropIndex("Web.JobInvoiceLineCharges", new[] { "CalculateOnId" });
            DropIndex("Web.JobInvoiceLineCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.JobInvoiceLineCharges", new[] { "ChargeId" });
            DropIndex("Web.JobInvoiceLineCharges", new[] { "LineTableId" });
            DropIndex("Web.JobInvoiceHeaderCharges", new[] { "ParentChargeId" });
            DropIndex("Web.JobInvoiceHeaderCharges", new[] { "CostCenterId" });
            DropIndex("Web.JobInvoiceHeaderCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.JobInvoiceHeaderCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.JobInvoiceHeaderCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.JobInvoiceHeaderCharges", new[] { "PersonID" });
            DropIndex("Web.JobInvoiceHeaderCharges", new[] { "CalculateOnId" });
            DropIndex("Web.JobInvoiceHeaderCharges", new[] { "ProductChargeId" });
            DropIndex("Web.JobInvoiceHeaderCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.JobInvoiceHeaderCharges", new[] { "ChargeId" });
            DropIndex("Web.JobInvoiceHeaderCharges", new[] { "HeaderTableId" });
            DropIndex("Web.JobReceiveHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.JobReceiveHeaders", new[] { "StockHeaderId" });
            DropIndex("Web.JobReceiveHeaders", new[] { "GodownId" });
            DropIndex("Web.JobReceiveHeaders", new[] { "JobReceiveById" });
            DropIndex("Web.JobReceiveHeaders", new[] { "JobWorkerId" });
            DropIndex("Web.JobReceiveHeaders", new[] { "ProcessId" });
            DropIndex("Web.JobReceiveHeaders", "IX_JobReceiveHeader_DocID");
            DropIndex("Web.JobReceiveLines", new[] { "ProductUidCurrentProcessId" });
            DropIndex("Web.JobReceiveLines", new[] { "ProductUidCurrentGodownId" });
            DropIndex("Web.JobReceiveLines", new[] { "ProductUidLastTransactionPersonId" });
            DropIndex("Web.JobReceiveLines", new[] { "ProductUidLastTransactionDocTypeId" });
            DropIndex("Web.JobReceiveLines", new[] { "StockProcessId" });
            DropIndex("Web.JobReceiveLines", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.JobReceiveLines", new[] { "StockId" });
            DropIndex("Web.JobReceiveLines", new[] { "MachineId" });
            DropIndex("Web.JobReceiveLines", new[] { "DealUnitId" });
            DropIndex("Web.JobReceiveLines", new[] { "JobOrderLineId" });
            DropIndex("Web.JobReceiveLines", new[] { "ProductUidId" });
            DropIndex("Web.JobReceiveLines", new[] { "JobReceiveHeaderId" });
            DropIndex("Web.JobInvoiceLines", new[] { "CostCenterId" });
            DropIndex("Web.JobInvoiceLines", new[] { "DealUnitId" });
            DropIndex("Web.JobInvoiceLines", new[] { "JobReceiveLineId" });
            DropIndex("Web.JobInvoiceLines", new[] { "JobWorkerId" });
            DropIndex("Web.JobInvoiceLines", new[] { "JobInvoiceHeaderId" });
            DropIndex("Web.JobInvoiceHeaders", new[] { "JobReceiveHeaderId" });
            DropIndex("Web.JobInvoiceHeaders", new[] { "ProcessId" });
            DropIndex("Web.JobInvoiceHeaders", new[] { "JobWorkerId" });
            DropIndex("Web.JobInvoiceHeaders", new[] { "LedgerHeaderId" });
            DropIndex("Web.JobInvoiceHeaders", "IX_JobInvoiceHeader_DocID");
            DropIndex("Web.JobInvoiceAmendmentHeaderCharges", new[] { "ParentChargeId" });
            DropIndex("Web.JobInvoiceAmendmentHeaderCharges", new[] { "CostCenterId" });
            DropIndex("Web.JobInvoiceAmendmentHeaderCharges", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.JobInvoiceAmendmentHeaderCharges", new[] { "LedgerAccountCrId" });
            DropIndex("Web.JobInvoiceAmendmentHeaderCharges", new[] { "LedgerAccountDrId" });
            DropIndex("Web.JobInvoiceAmendmentHeaderCharges", new[] { "PersonID" });
            DropIndex("Web.JobInvoiceAmendmentHeaderCharges", new[] { "CalculateOnId" });
            DropIndex("Web.JobInvoiceAmendmentHeaderCharges", new[] { "ProductChargeId" });
            DropIndex("Web.JobInvoiceAmendmentHeaderCharges", new[] { "ChargeTypeId" });
            DropIndex("Web.JobInvoiceAmendmentHeaderCharges", new[] { "ChargeId" });
            DropIndex("Web.JobInvoiceAmendmentHeaderCharges", new[] { "HeaderTableId" });
            DropIndex("Web.JobInvoiceAmendmentHeaders", new[] { "ProcessId" });
            DropIndex("Web.JobInvoiceAmendmentHeaders", new[] { "OrderById" });
            DropIndex("Web.JobInvoiceAmendmentHeaders", new[] { "JobWorkerId" });
            DropIndex("Web.JobInvoiceAmendmentHeaders", new[] { "SiteId" });
            DropIndex("Web.JobInvoiceAmendmentHeaders", new[] { "DivisionId" });
            DropIndex("Web.JobInvoiceAmendmentHeaders", new[] { "DocTypeId" });
            DropIndex("Web.JobInstructions", "IX_JobInstruction_JobInstructionDescription");
            DropIndex("Web.JobConsumptionSettings", new[] { "ProcessId" });
            DropIndex("Web.JobConsumptionSettings", new[] { "DivisionId" });
            DropIndex("Web.JobConsumptionSettings", new[] { "SiteId" });
            DropIndex("Web.JobConsumptionSettings", new[] { "DocTypeId" });
            DropIndex("Web.ProductTypeQaAttributes", new[] { "ProductTypeId" });
            DropIndex("Web.InspectionQaAttributes", new[] { "ProductTypeQaAttributeId" });
            DropIndex("Web.ExcessMaterialSettings", new[] { "ImportMenuId" });
            DropIndex("Web.ExcessMaterialSettings", new[] { "ProcessId" });
            DropIndex("Web.ExcessMaterialSettings", new[] { "DivisionId" });
            DropIndex("Web.ExcessMaterialSettings", new[] { "SiteId" });
            DropIndex("Web.ExcessMaterialSettings", new[] { "DocTypeId" });
            DropIndex("Web.ExcessMaterialLines", new[] { "ProductUidCurrentProcessId" });
            DropIndex("Web.ExcessMaterialLines", new[] { "ProductUidCurrentGodownId" });
            DropIndex("Web.ExcessMaterialLines", new[] { "ProductUidLastTransactionPersonId" });
            DropIndex("Web.ExcessMaterialLines", new[] { "ProductUidLastTransactionDocTypeId" });
            DropIndex("Web.ExcessMaterialLines", new[] { "Dimension2Id" });
            DropIndex("Web.ExcessMaterialLines", new[] { "Dimension1Id" });
            DropIndex("Web.ExcessMaterialLines", new[] { "ProcessId" });
            DropIndex("Web.ExcessMaterialLines", new[] { "ProductId" });
            DropIndex("Web.ExcessMaterialLines", new[] { "ProductUidId" });
            DropIndex("Web.ExcessMaterialLines", new[] { "ExcessMaterialHeaderId" });
            DropIndex("Web.ExcessMaterialHeaders", new[] { "GodownId" });
            DropIndex("Web.ExcessMaterialHeaders", new[] { "ProcessId" });
            DropIndex("Web.ExcessMaterialHeaders", new[] { "PersonId" });
            DropIndex("Web.ExcessMaterialHeaders", new[] { "CurrencyId" });
            DropIndex("Web.ExcessMaterialHeaders", "IX_ExcessMaterialHeader_DocID");
            DropIndex("Web.DocumentTypeTimePlans", new[] { "SiteId" });
            DropIndex("Web.DocumentTypeTimePlans", new[] { "DivisionId" });
            DropIndex("Web.DocumentTypeTimePlans", new[] { "DocTypeId" });
            DropIndex("Web.DocumentTypeTimeExtensions", new[] { "SiteId" });
            DropIndex("Web.DocumentTypeTimeExtensions", new[] { "DivisionId" });
            DropIndex("Web.DocumentTypeTimeExtensions", new[] { "DocTypeId" });
            DropIndex("Web.DocumentTypeSites", new[] { "SiteId" });
            DropIndex("Web.DocumentTypeSites", new[] { "DocumentTypeId" });
            DropIndex("Web.DocumentTypeDivisions", new[] { "DivisionId" });
            DropIndex("Web.DocumentTypeDivisions", new[] { "DocumentTypeId" });
            DropIndex("Web.DocumentStatus", "IX_DocumentStatus_DocumentStatusName");
            DropIndex("Web.DocSmsContents", new[] { "DivisionId" });
            DropIndex("Web.DocSmsContents", new[] { "SiteId" });
            DropIndex("Web.DocSmsContents", new[] { "DocTypeId" });
            DropIndex("Web.DocNotificationContents", new[] { "DivisionId" });
            DropIndex("Web.DocNotificationContents", new[] { "SiteId" });
            DropIndex("Web.DocNotificationContents", new[] { "DocTypeId" });
            DropIndex("Web.DocEmailContents", new[] { "DivisionId" });
            DropIndex("Web.DocEmailContents", new[] { "SiteId" });
            DropIndex("Web.DocEmailContents", new[] { "DocTypeId" });
            DropIndex("Web.DispatchWaybillLines", new[] { "CityId" });
            DropIndex("Web.DispatchWaybillLines", new[] { "DispatchWaybillHeaderId" });
            DropIndex("Web.Routes", "IX_Route_RouteName");
            DropIndex("Web.DispatchWaybillHeaders", new[] { "RouteId" });
            DropIndex("Web.DispatchWaybillHeaders", new[] { "ToCityId" });
            DropIndex("Web.DispatchWaybillHeaders", new[] { "FromCityId" });
            DropIndex("Web.DispatchWaybillHeaders", new[] { "TransporterId" });
            DropIndex("Web.DispatchWaybillHeaders", new[] { "SaleInvoiceHeaderId" });
            DropIndex("Web.DispatchWaybillHeaders", new[] { "ShipMethodId" });
            DropIndex("Web.DispatchWaybillHeaders", new[] { "ConsigneeId" });
            DropIndex("Web.DispatchWaybillHeaders", "IX_DispatchWaybillHeader_DocID");
            DropIndex("Web.PromoCodes", new[] { "ProductTypeId" });
            DropIndex("Web.PromoCodes", new[] { "ProductCategoryId" });
            DropIndex("Web.PromoCodes", new[] { "ProductGroupId" });
            DropIndex("Web.PromoCodes", new[] { "ProductId" });
            DropIndex("Web.PromoCodes", "IX_PromoCode_PromoCode");
            DropIndex("Web.SaleInvoiceLines", new[] { "PromoCodeId" });
            DropIndex("Web.SaleInvoiceLines", new[] { "DealUnitId" });
            DropIndex("Web.SaleInvoiceLines", new[] { "ProductInvoiceGroupId" });
            DropIndex("Web.SaleInvoiceLines", new[] { "SalesTaxGroupId" });
            DropIndex("Web.SaleInvoiceLines", new[] { "Dimension2Id" });
            DropIndex("Web.SaleInvoiceLines", new[] { "Dimension1Id" });
            DropIndex("Web.SaleInvoiceLines", new[] { "ProductId" });
            DropIndex("Web.SaleInvoiceLines", new[] { "SaleOrderLineId" });
            DropIndex("Web.SaleInvoiceLines", new[] { "SaleDispatchLineId" });
            DropIndex("Web.SaleInvoiceLines", new[] { "SaleInvoiceHeaderId" });
            DropIndex("Web.SaleDispatchLines", new[] { "ProductUidCurrentProcessId" });
            DropIndex("Web.SaleDispatchLines", new[] { "ProductUidCurrentGodownId" });
            DropIndex("Web.SaleDispatchLines", new[] { "ProductUidLastTransactionPersonId" });
            DropIndex("Web.SaleDispatchLines", new[] { "ProductUidLastTransactionDocTypeId" });
            DropIndex("Web.SaleDispatchLines", new[] { "StockInId" });
            DropIndex("Web.SaleDispatchLines", new[] { "StockId" });
            DropIndex("Web.SaleDispatchLines", new[] { "GodownId" });
            DropIndex("Web.SaleDispatchLines", new[] { "PackingLineId" });
            DropIndex("Web.SaleDispatchLines", new[] { "SaleDispatchHeaderId" });
            DropIndex("Web.SaleDispatchHeaders", new[] { "PackingHeaderId" });
            DropIndex("Web.SaleDispatchHeaders", new[] { "StockHeaderId" });
            DropIndex("Web.SaleDispatchHeaders", new[] { "GatePassHeaderId" });
            DropIndex("Web.SaleDispatchHeaders", new[] { "ShipMethodId" });
            DropIndex("Web.SaleDispatchHeaders", new[] { "DeliveryTermsId" });
            DropIndex("Web.SaleDispatchHeaders", new[] { "SaleToBuyerId" });
            DropIndex("Web.SaleDispatchHeaders", "IX_SaleDispatchHeader_DocID");
            DropIndex("Web.SaleInvoiceHeaders", new[] { "SaleDispatchHeaderId" });
            DropIndex("Web.SaleInvoiceHeaders", new[] { "CurrencyId" });
            DropIndex("Web.SaleInvoiceHeaders", new[] { "AgentId" });
            DropIndex("Web.SaleInvoiceHeaders", new[] { "BillToBuyerId" });
            DropIndex("Web.SaleInvoiceHeaders", new[] { "SaleToBuyerId" });
            DropIndex("Web.SaleInvoiceHeaders", new[] { "LedgerHeaderId" });
            DropIndex("Web.SaleInvoiceHeaders", "IX_SaleInvoiceHeader_DocID");
            DropIndex("Web.CustomDetails", new[] { "TRCourierId" });
            DropIndex("Web.CustomDetails", new[] { "SaleInvoiceHeaderId" });
            DropIndex("Web.CustomDetails", "IX_CustomDetail_DocID");
            DropIndex("Web.CurrencyConversions", new[] { "ToCurrencyId" });
            DropIndex("Web.CurrencyConversions", new[] { "FromCurrencyId" });
            DropIndex("Web.Couriers", new[] { "PersonID" });
            DropIndex("Web.CostCenterStatusExtendeds", new[] { "CostCenterId" });
            DropIndex("Web.CostCenterStatus", new[] { "ProductId" });
            DropIndex("Web.CostCenterStatus", new[] { "CostCenterId" });
            DropIndex("Web.Companies", new[] { "CurrencyId" });
            DropIndex("Web.Companies", new[] { "CityId" });
            DropIndex("Web.Companies", "IX_Company_Company");
            DropIndex("Web.ChargeGroupProducts", new[] { "ChargeTypeId" });
            DropIndex("Web.ChargeGroupProducts", "IX_ChargeGroupProduct_ChargeGroupProductName");
            DropIndex("Web.CalculationProducts", new[] { "PersonId" });
            DropIndex("Web.CalculationProducts", new[] { "LedgerAccountCrId" });
            DropIndex("Web.CalculationProducts", new[] { "LedgerAccountDrId" });
            DropIndex("Web.CalculationProducts", new[] { "ParentChargeId" });
            DropIndex("Web.CalculationProducts", new[] { "CostCenterId" });
            DropIndex("Web.CalculationProducts", new[] { "CalculateOnId" });
            DropIndex("Web.CalculationProducts", new[] { "ChargeTypeId" });
            DropIndex("Web.CalculationProducts", "IX_CalculationProduct_CalculationProductName");
            DropIndex("Web.CalculationLineLedgerAccounts", new[] { "SiteId" });
            DropIndex("Web.CalculationLineLedgerAccounts", new[] { "DivisionId" });
            DropIndex("Web.CalculationLineLedgerAccounts", new[] { "CostCenterId" });
            DropIndex("Web.CalculationLineLedgerAccounts", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.CalculationLineLedgerAccounts", new[] { "LedgerAccountCrId" });
            DropIndex("Web.CalculationLineLedgerAccounts", new[] { "LedgerAccountDrId" });
            DropIndex("Web.CalculationLineLedgerAccounts", "IX_CalculationLineLedgerAccount_UniqueRow");
            DropIndex("Web.CalculationLineLedgerAccounts", new[] { "CalculationId" });
            DropIndex("Web.CalculationHeaderLedgerAccounts", new[] { "SiteId" });
            DropIndex("Web.CalculationHeaderLedgerAccounts", new[] { "DivisionId" });
            DropIndex("Web.CalculationHeaderLedgerAccounts", new[] { "CostCenterId" });
            DropIndex("Web.CalculationHeaderLedgerAccounts", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.CalculationHeaderLedgerAccounts", new[] { "LedgerAccountCrId" });
            DropIndex("Web.CalculationHeaderLedgerAccounts", new[] { "LedgerAccountDrId" });
            DropIndex("Web.CalculationHeaderLedgerAccounts", "IX_CalculationHeaderLedgerAccount_UniqueRow");
            DropIndex("Web.CalculationHeaderLedgerAccounts", new[] { "CalculationId" });
            DropIndex("Web.Charges", "IX_Charge_ChargeCode");
            DropIndex("Web.Charges", "IX_Charge_Charge");
            DropIndex("Web.CalculationFooters", new[] { "LedgerAccountCrId" });
            DropIndex("Web.CalculationFooters", new[] { "LedgerAccountDrId" });
            DropIndex("Web.CalculationFooters", new[] { "ParentChargeId" });
            DropIndex("Web.CalculationFooters", new[] { "PersonId" });
            DropIndex("Web.CalculationFooters", new[] { "CostCenterId" });
            DropIndex("Web.CalculationFooters", new[] { "ProductChargeId" });
            DropIndex("Web.CalculationFooters", new[] { "CalculateOnId" });
            DropIndex("Web.CalculationFooters", new[] { "ChargeTypeId" });
            DropIndex("Web.CalculationFooters", "IX_CalculationLine_CalculationLineName");
            DropIndex("Web.Calculations", "IX_Calculation_Calculation");
            DropIndex("Web.BusinessSessions", new[] { "DocTypeId" });
            DropIndex("Web.TdsGroups", "IX_TdsGroup_TdsGroupName");
            DropIndex("Web.TdsCategories", "IX_TdsCategory_TdsCategoryName");
            DropIndex("Web.ServiceTaxCategories", "IX_ServiceTaxCategory_ServiceTaxCategoryName");
            DropIndex("Web.PersonRateGroups", "IX_PersonRateGroup_PersonRateGroupName");
            DropIndex("Web.BusinessEntities", new[] { "Buyer_PersonID" });
            DropIndex("Web.BusinessEntities", new[] { "ServiceTaxCategoryId" });
            DropIndex("Web.BusinessEntities", new[] { "PersonRateGroupId" });
            DropIndex("Web.BusinessEntities", new[] { "SalesTaxGroupPartyId" });
            DropIndex("Web.BusinessEntities", new[] { "GuarantorId" });
            DropIndex("Web.BusinessEntities", new[] { "TdsGroupId" });
            DropIndex("Web.BusinessEntities", new[] { "TdsCategoryId" });
            DropIndex("Web.BusinessEntities", new[] { "ParentId" });
            DropIndex("Web.BusinessEntities", new[] { "PersonID" });
            DropIndex("Web.ProductStyles", "IX_ProductStyle_ProductStyleName");
            DropIndex("Web.ProductQualities", new[] { "ProductTypeId" });
            DropIndex("Web.ProductQualities", "IX_ProductQuality_ProductQualityName");
            DropIndex("Web.ProductInvoiceGroups", new[] { "DescriptionOfGoodsId" });
            DropIndex("Web.ProductInvoiceGroups", new[] { "DivisionId" });
            DropIndex("Web.ProductInvoiceGroups", "IX_ProductInvoiceGroup_ProductInvoiceGroupName");
            DropIndex("Web.ProductDesignPatterns", new[] { "ProductTypeId" });
            DropIndex("Web.ProductDesignPatterns", "IX_ProductDesignPattern_ProductDesignName");
            DropIndex("Web.ProductCollections", new[] { "ProductTypeId" });
            DropIndex("Web.ProductCollections", "IX_ProductCollection_ProductCollectionName");
            DropIndex("Web.ProcessSequenceHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.ProcessSequenceHeaders", "IX_ProcessSequence_ProcessSequenceHeaderName");
            DropIndex("Web.DescriptionOfGoods", "IX_DescriptionOfGoods_DescriptionOfGoodsName");
            DropIndex("Web.ProductContentLines", new[] { "ProductGroupId" });
            DropIndex("Web.ProductContentLines", new[] { "ProductContentHeaderId" });
            DropIndex("Web.ProductContentHeaders", "IX_ProductContent_ProductContentName");
            DropIndex("Web.Colours", "IX_Colour_ColourName");
            DropIndex("Web.SalesTaxGroupProducts", "IX_SalesTaxGroupProduct_SalesTaxGroupProductName");
            DropIndex("Web.SalesTaxGroups", new[] { "SalesTaxGroupPartyId" });
            DropIndex("Web.SalesTaxGroups", new[] { "SalesTaxGroupProductId" });
            DropIndex("Web.ChargeTypes", "IX_Charges_ChargesName");
            DropIndex("Web.ChargeGroupPersons", new[] { "ChargeTypeId" });
            DropIndex("Web.ChargeGroupPersons", "IX_ChargeGroupPerson_ChargeGroupPersonName");
            DropIndex("Web.PurchaseOrderHeaders", new[] { "UnitConversionForId" });
            DropIndex("Web.PurchaseOrderHeaders", new[] { "SalesTaxGroupPersonId" });
            DropIndex("Web.PurchaseOrderHeaders", new[] { "CurrencyId" });
            DropIndex("Web.PurchaseOrderHeaders", new[] { "DeliveryTermsId" });
            DropIndex("Web.PurchaseOrderHeaders", new[] { "ShipMethodId" });
            DropIndex("Web.PurchaseOrderHeaders", "IX_PurchaseOrderHeader_DocID");
            DropIndex("Web.PurchaseOrderCancelHeaders", new[] { "SupplierId" });
            DropIndex("Web.PurchaseOrderCancelHeaders", new[] { "ReasonId" });
            DropIndex("Web.PurchaseOrderCancelHeaders", "IX_PurchaseOrderCancelHeader_DocID");
            DropIndex("Web.PurchaseOrderCancelLines", new[] { "PurchaseOrderLineId" });
            DropIndex("Web.PurchaseOrderCancelLines", new[] { "PurchaseOrderCancelHeaderId" });
            DropIndex("Web.PurchaseOrderLines", new[] { "ProductUidHeaderId" });
            DropIndex("Web.PurchaseOrderLines", new[] { "DealUnitId" });
            DropIndex("Web.PurchaseOrderLines", new[] { "SalesTaxGroupId" });
            DropIndex("Web.PurchaseOrderLines", new[] { "Dimension2Id" });
            DropIndex("Web.PurchaseOrderLines", new[] { "Dimension1Id" });
            DropIndex("Web.PurchaseOrderLines", new[] { "ProductId" });
            DropIndex("Web.PurchaseOrderLines", new[] { "PurchaseIndentLineId" });
            DropIndex("Web.PurchaseOrderLines", new[] { "PurchaseOrderHeaderId" });
            DropIndex("Web.PurchaseIndentHeaders", new[] { "MaterialPlanHeaderId" });
            DropIndex("Web.PurchaseIndentHeaders", new[] { "ReasonId" });
            DropIndex("Web.PurchaseIndentHeaders", new[] { "DepartmentId" });
            DropIndex("Web.PurchaseIndentHeaders", "IX_PurchaseIndentHeader_DocID");
            DropIndex("Web.PurchaseIndentCancelHeaders", new[] { "ReasonId" });
            DropIndex("Web.PurchaseIndentCancelHeaders", "IX_PurchaseIndentCancelHeader_DocID");
            DropIndex("Web.PurchaseIndentCancelLines", new[] { "PurchaseIndentLineId" });
            DropIndex("Web.PurchaseIndentCancelLines", new[] { "PurchaseIndentCancelHeaderId" });
            DropIndex("Web.PurchaseIndentLines", new[] { "Dimension2Id" });
            DropIndex("Web.PurchaseIndentLines", new[] { "Dimension1Id" });
            DropIndex("Web.PurchaseIndentLines", new[] { "ProductId" });
            DropIndex("Web.PurchaseIndentLines", new[] { "MaterialPlanLineId" });
            DropIndex("Web.PurchaseIndentLines", new[] { "PurchaseIndentHeaderId" });
            DropIndex("Web.Transporters", new[] { "PersonID" });
            DropIndex("Web.PurchaseWaybills", new[] { "ToCityId" });
            DropIndex("Web.PurchaseWaybills", new[] { "FromCityId" });
            DropIndex("Web.PurchaseWaybills", new[] { "ShipMethodId" });
            DropIndex("Web.PurchaseWaybills", "IX_PurchaseTransportGR_DocID");
            DropIndex("Web.PurchaseWaybills", new[] { "ConsignerId" });
            DropIndex("Web.PurchaseWaybills", new[] { "TransporterId" });
            DropIndex("Web.PurchaseWaybills", "IX_PurchaseWaybill_EntryNo");
            DropIndex("Web.PurchaseWaybills", "IX_PurchaseWaybill_DocID");
            DropIndex("Web.GateIns", "IX_GateIn_DocID");
            DropIndex("Web.PurchaseGoodsReceiptHeaders", new[] { "UnitConversionForId" });
            DropIndex("Web.PurchaseGoodsReceiptHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.PurchaseGoodsReceiptHeaders", new[] { "StockHeaderId" });
            DropIndex("Web.PurchaseGoodsReceiptHeaders", new[] { "RoadPermitFormId" });
            DropIndex("Web.PurchaseGoodsReceiptHeaders", new[] { "GateInId" });
            DropIndex("Web.PurchaseGoodsReceiptHeaders", new[] { "PurchaseWaybillId" });
            DropIndex("Web.PurchaseGoodsReceiptHeaders", new[] { "GodownId" });
            DropIndex("Web.PurchaseGoodsReceiptHeaders", new[] { "SupplierId" });
            DropIndex("Web.PurchaseGoodsReceiptHeaders", "IX_PurchaseGoodsReceiptHeader_DocID");
            DropIndex("Web.PurchaseGoodsReceiptLines", new[] { "ProductUidCurrentProcessId" });
            DropIndex("Web.PurchaseGoodsReceiptLines", new[] { "ProductUidCurrentGodownId" });
            DropIndex("Web.PurchaseGoodsReceiptLines", new[] { "ProductUidLastTransactionPersonId" });
            DropIndex("Web.PurchaseGoodsReceiptLines", new[] { "ProductUidLastTransactionDocTypeId" });
            DropIndex("Web.PurchaseGoodsReceiptLines", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.PurchaseGoodsReceiptLines", new[] { "StockId" });
            DropIndex("Web.PurchaseGoodsReceiptLines", new[] { "DealUnitId" });
            DropIndex("Web.PurchaseGoodsReceiptLines", new[] { "Dimension2Id" });
            DropIndex("Web.PurchaseGoodsReceiptLines", new[] { "Dimension1Id" });
            DropIndex("Web.PurchaseGoodsReceiptLines", new[] { "ProductId" });
            DropIndex("Web.PurchaseGoodsReceiptLines", new[] { "ProductUidId" });
            DropIndex("Web.PurchaseGoodsReceiptLines", new[] { "PurchaseIndentLineId" });
            DropIndex("Web.PurchaseGoodsReceiptLines", new[] { "PurchaseOrderLineId" });
            DropIndex("Web.PurchaseGoodsReceiptLines", new[] { "PurchaseGoodsReceiptHeaderId" });
            DropIndex("Web.SalesTaxGroupParties", "IX_SalesTaxGroupParty_SalesTaxGroupPartyName");
            DropIndex("Web.Suppliers", new[] { "SalesTaxGroupPartyId" });
            DropIndex("Web.Suppliers", new[] { "PersonID" });
            DropIndex("Web.ProductSuppliers", new[] { "SupplierId" });
            DropIndex("Web.ProductSuppliers", new[] { "ProductId" });
            DropIndex("Web.Sizes", new[] { "UnitId" });
            DropIndex("Web.Sizes", new[] { "ProductShapeId" });
            DropIndex("Web.Sizes", "IX_Size_SizeName");
            DropIndex("Web.Sizes", new[] { "DocTypeId" });
            DropIndex("Web.ProductSizeTypes", "IX_ProductSizeType_ProductSizeTypeName");
            DropIndex("Web.ProductSizes", new[] { "SizeId" });
            DropIndex("Web.ProductSizes", new[] { "ProductId" });
            DropIndex("Web.ProductSizes", new[] { "ProductSizeTypeId" });
            DropIndex("Web.ProductRelatedAccessories", new[] { "Product_ProductId" });
            DropIndex("Web.ProductRelatedAccessories", "IX_ProductRelatedAccessories_AccessoryId");
            DropIndex("Web.ProductRelatedAccessories", "IX_ProductRelatedAccessories_ProductId");
            DropIndex("Web.ProductIncludedAccessories", new[] { "Product_ProductId" });
            DropIndex("Web.ProductIncludedAccessories", "IX_ProductIncludedAccessories_AccessoryId");
            DropIndex("Web.ProductIncludedAccessories", "IX_ProductIncludedAccessories_ProductId");
            DropIndex("Web.ProductBuyers", new[] { "BuyerId" });
            DropIndex("Web.ProductBuyers", new[] { "ProductId" });
            DropIndex("Web.ProductAttributes", new[] { "ProductId" });
            DropIndex("Web.Designations", "IX_Designation_DesignationName");
            DropIndex("Web.Departments", "IX_Department_DepartmentName");
            DropIndex("Web.Employees", new[] { "DepartmentID" });
            DropIndex("Web.Employees", new[] { "DesignationID" });
            DropIndex("Web.Employees", new[] { "PersonID" });
            DropIndex("Web.ProductUidHeaders", new[] { "GenPersonId" });
            DropIndex("Web.ProductUidHeaders", new[] { "GenDocTypeId" });
            DropIndex("Web.ProductUidHeaders", new[] { "Dimension2Id" });
            DropIndex("Web.ProductUidHeaders", new[] { "Dimension1Id" });
            DropIndex("Web.ProductUidHeaders", new[] { "ProductId" });
            DropIndex("Web.DeliveryTerms", "IX_DeliveryTerms_DeliveryTermsName");
            DropIndex("Web.SaleOrderHeaders", new[] { "UnitConversionForId" });
            DropIndex("Web.SaleOrderHeaders", new[] { "LedgerHeaderId" });
            DropIndex("Web.SaleOrderHeaders", new[] { "DeliveryTermsId" });
            DropIndex("Web.SaleOrderHeaders", new[] { "ShipMethodId" });
            DropIndex("Web.SaleOrderHeaders", new[] { "CurrencyId" });
            DropIndex("Web.SaleOrderHeaders", new[] { "BillToBuyerId" });
            DropIndex("Web.SaleOrderHeaders", new[] { "SaleToBuyerId" });
            DropIndex("Web.SaleOrderHeaders", new[] { "SiteId" });
            DropIndex("Web.SaleOrderHeaders", new[] { "DivisionId" });
            DropIndex("Web.SaleOrderHeaders", new[] { "DocTypeId" });
            DropIndex("Web.SaleOrderLines", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.SaleOrderLines", new[] { "DealUnitId" });
            DropIndex("Web.SaleOrderLines", new[] { "Dimension2Id" });
            DropIndex("Web.SaleOrderLines", new[] { "Dimension1Id" });
            DropIndex("Web.SaleOrderLines", "IX_SaleOrderLine_SaleOrdeHeaderProductDueDate");
            DropIndex("Web.SaleDeliveryOrderHeaders", new[] { "ShipMethodId" });
            DropIndex("Web.SaleDeliveryOrderHeaders", new[] { "BuyerId" });
            DropIndex("Web.SaleDeliveryOrderHeaders", new[] { "SiteId" });
            DropIndex("Web.SaleDeliveryOrderHeaders", new[] { "DivisionId" });
            DropIndex("Web.SaleDeliveryOrderHeaders", new[] { "DocTypeId" });
            DropIndex("Web.SaleDeliveryOrderLines", new[] { "SaleDeliveryOrderHeaderId" });
            DropIndex("Web.SaleDeliveryOrderLines", new[] { "SaleOrderLineId" });
            DropIndex("Web.Reasons", "IX_Reason_ReasonName");
            DropIndex("Web.RequisitionHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.RequisitionHeaders", new[] { "ReasonId" });
            DropIndex("Web.RequisitionHeaders", new[] { "CostCenterId" });
            DropIndex("Web.RequisitionHeaders", new[] { "PersonId" });
            DropIndex("Web.RequisitionHeaders", "IX_RequisitionHeader_DocID");
            DropIndex("Web.RequisitionLines", new[] { "ProcessId" });
            DropIndex("Web.RequisitionLines", new[] { "Dimension2Id" });
            DropIndex("Web.RequisitionLines", new[] { "Dimension1Id" });
            DropIndex("Web.RequisitionLines", new[] { "ProductId" });
            DropIndex("Web.RequisitionLines", new[] { "RequisitionHeaderId" });
            DropIndex("Web.StockProcesses", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.StockProcesses", new[] { "Dimension2Id" });
            DropIndex("Web.StockProcesses", new[] { "Dimension1Id" });
            DropIndex("Web.StockProcesses", new[] { "CostCenterId" });
            DropIndex("Web.StockProcesses", new[] { "GodownId" });
            DropIndex("Web.StockProcesses", new[] { "ProcessId" });
            DropIndex("Web.StockProcesses", new[] { "ProductUidId" });
            DropIndex("Web.StockProcesses", new[] { "ProductId" });
            DropIndex("Web.StockProcesses", new[] { "StockHeaderId" });
            DropIndex("Web.Stocks", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.Stocks", new[] { "Dimension2Id" });
            DropIndex("Web.Stocks", new[] { "Dimension1Id" });
            DropIndex("Web.Stocks", new[] { "CostCenterId" });
            DropIndex("Web.Stocks", new[] { "GodownId" });
            DropIndex("Web.Stocks", new[] { "ProcessId" });
            DropIndex("Web.Stocks", new[] { "ProductId" });
            DropIndex("Web.Stocks", new[] { "ProductUidId" });
            DropIndex("Web.Stocks", new[] { "StockHeaderId" });
            DropIndex("Web.StockLines", new[] { "ProductUidCurrentProcessId" });
            DropIndex("Web.StockLines", new[] { "ProductUidCurrentGodownId" });
            DropIndex("Web.StockLines", new[] { "ProductUidLastTransactionPersonId" });
            DropIndex("Web.StockLines", new[] { "ProductUidLastTransactionDocTypeId" });
            DropIndex("Web.StockLines", new[] { "CostCenterId" });
            DropIndex("Web.StockLines", new[] { "Dimension2Id" });
            DropIndex("Web.StockLines", new[] { "Dimension1Id" });
            DropIndex("Web.StockLines", new[] { "FromStockProcessId" });
            DropIndex("Web.StockLines", new[] { "StockProcessId" });
            DropIndex("Web.StockLines", new[] { "StockId" });
            DropIndex("Web.StockLines", new[] { "FromStockId" });
            DropIndex("Web.StockLines", new[] { "FromProcessId" });
            DropIndex("Web.StockLines", new[] { "ProductId" });
            DropIndex("Web.StockLines", new[] { "RequisitionLineId" });
            DropIndex("Web.StockLines", new[] { "ProductUidId" });
            DropIndex("Web.StockLines", new[] { "StockHeaderId" });
            DropIndex("Web.Machines", "IX_Machine_MachineName");
            DropIndex("Web.LedgerHeaders", new[] { "GodownId" });
            DropIndex("Web.LedgerHeaders", new[] { "CostCenterId" });
            DropIndex("Web.LedgerHeaders", new[] { "ProcessId" });
            DropIndex("Web.LedgerHeaders", new[] { "LedgerAccountId" });
            DropIndex("Web.LedgerHeaders", "IX_LedgerHeader_DocID");
            DropIndex("Web.Currencies", "IX_Currency_Name");
            DropIndex("Web.StockHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.StockHeaders", new[] { "LedgerAccountId" });
            DropIndex("Web.StockHeaders", new[] { "ContraLedgerAccountId" });
            DropIndex("Web.StockHeaders", new[] { "LedgerHeaderId" });
            DropIndex("Web.StockHeaders", new[] { "MachineId" });
            DropIndex("Web.StockHeaders", new[] { "CostCenterId" });
            DropIndex("Web.StockHeaders", new[] { "GatePassHeaderId" });
            DropIndex("Web.StockHeaders", new[] { "GodownId" });
            DropIndex("Web.StockHeaders", new[] { "FromGodownId" });
            DropIndex("Web.StockHeaders", new[] { "ProcessId" });
            DropIndex("Web.StockHeaders", new[] { "PersonId" });
            DropIndex("Web.StockHeaders", new[] { "CurrencyId" });
            DropIndex("Web.StockHeaders", "IX_StockHeader_DocID");
            DropIndex("Web.ShipMethods", "IX_ShipMethod_ShipMethodName");
            DropIndex("Web.PackingHeaders", new[] { "DealUnitId" });
            DropIndex("Web.PackingHeaders", new[] { "ShipMethodId" });
            DropIndex("Web.PackingHeaders", new[] { "GodownId" });
            DropIndex("Web.PackingHeaders", new[] { "JobWorkerId" });
            DropIndex("Web.PackingHeaders", new[] { "BuyerId" });
            DropIndex("Web.PackingHeaders", new[] { "StockHeaderId" });
            DropIndex("Web.PackingHeaders", "IX_PackingHeader_DocID");
            DropIndex("Web.PackingLines", new[] { "ProductUidCurrentProcessId" });
            DropIndex("Web.PackingLines", new[] { "ProductUidCurrentGodownId" });
            DropIndex("Web.PackingLines", new[] { "ProductUidLastTransactionPersonId" });
            DropIndex("Web.PackingLines", new[] { "ProductUidLastTransactionDocTypeId" });
            DropIndex("Web.PackingLines", new[] { "StockReceiveId" });
            DropIndex("Web.PackingLines", new[] { "StockIssueId" });
            DropIndex("Web.PackingLines", new[] { "DealUnitId" });
            DropIndex("Web.PackingLines", new[] { "FromProcessId" });
            DropIndex("Web.PackingLines", new[] { "SaleDeliveryOrderLineId" });
            DropIndex("Web.PackingLines", new[] { "SaleOrderLineId" });
            DropIndex("Web.PackingLines", new[] { "Dimension2Id" });
            DropIndex("Web.PackingLines", new[] { "Dimension1Id" });
            DropIndex("Web.PackingLines", new[] { "ProductId" });
            DropIndex("Web.PackingLines", new[] { "ProductUidId" });
            DropIndex("Web.PackingLines", new[] { "PackingHeaderId" });
            DropIndex("Web.ProductUids", new[] { "CurrenctProcessId" });
            DropIndex("Web.ProductUids", new[] { "CurrenctGodownId" });
            DropIndex("Web.ProductUids", new[] { "LastTransactionPersonId" });
            DropIndex("Web.ProductUids", new[] { "LastTransactionDocTypeId" });
            DropIndex("Web.ProductUids", new[] { "GenPersonId" });
            DropIndex("Web.ProductUids", new[] { "GenDocTypeId" });
            DropIndex("Web.ProductUids", new[] { "Dimension2Id" });
            DropIndex("Web.ProductUids", new[] { "Dimension1Id" });
            DropIndex("Web.ProductUids", new[] { "ProductId" });
            DropIndex("Web.ProductUids", new[] { "ProductUidHeaderId" });
            DropIndex("Web.ProductUids", "IX_ProductUid_ProductUidName");
            DropIndex("Web.ProdOrderHeaders", new[] { "MaterialPlanHeaderId" });
            DropIndex("Web.ProdOrderHeaders", new[] { "BuyerId" });
            DropIndex("Web.ProdOrderHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.ProdOrderHeaders", new[] { "SiteId" });
            DropIndex("Web.ProdOrderHeaders", new[] { "DivisionId" });
            DropIndex("Web.ProdOrderHeaders", new[] { "DocTypeId" });
            DropIndex("Web.Buyers", new[] { "PersonID" });
            DropIndex("Web.MaterialPlanHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.MaterialPlanHeaders", new[] { "GodownId" });
            DropIndex("Web.MaterialPlanHeaders", new[] { "SiteId" });
            DropIndex("Web.MaterialPlanHeaders", new[] { "DivisionId" });
            DropIndex("Web.MaterialPlanHeaders", new[] { "BuyerId" });
            DropIndex("Web.MaterialPlanHeaders", new[] { "DocTypeId" });
            DropIndex("Web.MaterialPlanLines", new[] { "Dimension2Id" });
            DropIndex("Web.MaterialPlanLines", new[] { "Dimension1Id" });
            DropIndex("Web.MaterialPlanLines", new[] { "ProcessId" });
            DropIndex("Web.MaterialPlanLines", new[] { "ProductId" });
            DropIndex("Web.MaterialPlanLines", new[] { "MaterialPlanHeaderId" });
            DropIndex("Web.ProdOrderLines", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.ProdOrderLines", new[] { "MaterialPlanLineId" });
            DropIndex("Web.ProdOrderLines", new[] { "ProcessId" });
            DropIndex("Web.ProdOrderLines", new[] { "Dimension2Id" });
            DropIndex("Web.ProdOrderLines", new[] { "Dimension1Id" });
            DropIndex("Web.ProdOrderLines", new[] { "ProductId" });
            DropIndex("Web.ProdOrderLines", new[] { "ProdOrderHeaderId" });
            DropIndex("Web.JobOrderLines", new[] { "ProductUidCurrentProcessId" });
            DropIndex("Web.JobOrderLines", new[] { "ProductUidCurrentGodownId" });
            DropIndex("Web.JobOrderLines", new[] { "ProductUidLastTransactionPersonId" });
            DropIndex("Web.JobOrderLines", new[] { "ProductUidLastTransactionDocTypeId" });
            DropIndex("Web.JobOrderLines", new[] { "ProductUidHeaderId" });
            DropIndex("Web.JobOrderLines", new[] { "StockProcessId" });
            DropIndex("Web.JobOrderLines", new[] { "StockId" });
            DropIndex("Web.JobOrderLines", new[] { "DealUnitId" });
            DropIndex("Web.JobOrderLines", new[] { "UnitId" });
            DropIndex("Web.JobOrderLines", new[] { "FromProcessId" });
            DropIndex("Web.JobOrderLines", new[] { "Dimension2Id" });
            DropIndex("Web.JobOrderLines", new[] { "Dimension1Id" });
            DropIndex("Web.JobOrderLines", new[] { "ProdOrderLineId" });
            DropIndex("Web.JobOrderLines", new[] { "ProductId" });
            DropIndex("Web.JobOrderLines", new[] { "ProductUidId" });
            DropIndex("Web.JobOrderLines", new[] { "JobOrderHeaderId" });
            DropIndex("Web.Units", "IX_Unit_UnitName");
            DropIndex("Web.GatePassLines", new[] { "UnitId" });
            DropIndex("Web.GatePassLines", new[] { "GatePassHeaderId" });
            DropIndex("Web.GatePassHeaders", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.GatePassHeaders", new[] { "GodownId" });
            DropIndex("Web.GatePassHeaders", new[] { "PersonId" });
            DropIndex("Web.GatePassHeaders", new[] { "SiteId" });
            DropIndex("Web.GatePassHeaders", "IX_GatePassHeader_DocID");
            DropIndex("Web.Gates", new[] { "SiteId" });
            DropIndex("Web.Gates", "IX_Gate_GateName");
            DropIndex("Web.Godowns", new[] { "GateId" });
            DropIndex("Web.Godowns", new[] { "SiteId" });
            DropIndex("Web.Godowns", "IX_Godown_GodownName");
            DropIndex("Web.Sites", new[] { "DefaultDivisionId" });
            DropIndex("Web.Sites", new[] { "DefaultGodownId" });
            DropIndex("Web.Sites", new[] { "PersonId" });
            DropIndex("Web.Sites", new[] { "CityId" });
            DropIndex("Web.Processes", new[] { "CostCenterId" });
            DropIndex("Web.Processes", new[] { "AccountId" });
            DropIndex("Web.Processes", new[] { "ParentProcessId" });
            DropIndex("Web.Processes", "IX_Process_ProcessName");
            DropIndex("Web.Processes", "IX_Process_ProcessCode");
            DropIndex("Web.LedgerAccountGroups", "IX_LedgerAccountGroup_LedgerAccountGroupName");
            DropIndex("Web.LedgerAccounts", new[] { "LedgerAccountGroupId" });
            DropIndex("Web.LedgerAccounts", new[] { "PersonId" });
            DropIndex("Web.LedgerAccounts", "IX_LedgerAccount_LedgerAccountName");
            DropIndex("Web.CostCenters", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.CostCenters", new[] { "ProcessId" });
            DropIndex("Web.CostCenters", new[] { "ParentCostCenterId" });
            DropIndex("Web.CostCenters", new[] { "LedgerAccountId" });
            DropIndex("Web.CostCenters", new[] { "SiteId" });
            DropIndex("Web.CostCenters", new[] { "DivisionId" });
            DropIndex("Web.CostCenters", new[] { "DocTypeId" });
            DropIndex("Web.CostCenters", "IX_CostCenter_CostCenterName");
            DropIndex("Web.JobWorkers", new[] { "PersonID" });
            DropIndex("Web.JobOrderHeaders", new[] { "UnitConversionForId" });
            DropIndex("Web.JobOrderHeaders", new[] { "StockHeaderId" });
            DropIndex("Web.JobOrderHeaders", new[] { "GatePassHeaderId" });
            DropIndex("Web.JobOrderHeaders", new[] { "MachineId" });
            DropIndex("Web.JobOrderHeaders", new[] { "CostCenterId" });
            DropIndex("Web.JobOrderHeaders", new[] { "ProcessId" });
            DropIndex("Web.JobOrderHeaders", new[] { "GodownId" });
            DropIndex("Web.JobOrderHeaders", new[] { "OrderById" });
            DropIndex("Web.JobOrderHeaders", new[] { "BillToPartyId" });
            DropIndex("Web.JobOrderHeaders", new[] { "JobWorkerId" });
            DropIndex("Web.JobOrderHeaders", new[] { "SiteId" });
            DropIndex("Web.JobOrderHeaders", new[] { "DivisionId" });
            DropIndex("Web.JobOrderHeaders", new[] { "DocTypeId" });
            DropIndex("Web.Dimension2", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.Dimension2", new[] { "ProductTypeId" });
            DropIndex("Web.Dimension2", "IX_Dimension2_Dimension2Name");
            DropIndex("Web.ProductTypeAttributes", new[] { "ProductType_ProductTypeId" });
            DropIndex("Web.ProductNatures", "IX_ProductNature_ProductNatureName");
            DropIndex("Web.ProductGroups", new[] { "ProductTypeId" });
            DropIndex("Web.ProductGroups", "IX_ProductGroup_ProductGroupName");
            DropIndex("Web.ProductDesigns", new[] { "ProductTypeId" });
            DropIndex("Web.ProductDesigns", "IX_ProductDesign_ProductDesignName");
            DropIndex("Web.ProductCategories", new[] { "ProductTypeId" });
            DropIndex("Web.ProductCategories", "IX_ProductCategory_ProductCategoryName");
            DropIndex("Web.Dimension2Types", "IX_Dimension2Type_Dimension2TypeName");
            DropIndex("Web.Dimension1Types", "IX_Dimension1Type_Dimension1TypeName");
            DropIndex("Web.ProductTypes", new[] { "Dimension2TypeId" });
            DropIndex("Web.ProductTypes", new[] { "Dimension1TypeId" });
            DropIndex("Web.ProductTypes", new[] { "ProductNatureId" });
            DropIndex("Web.ProductTypes", "IX_ProductType_ProductTypeName");
            DropIndex("Web.Dimension1", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.Dimension1", new[] { "ProductTypeId" });
            DropIndex("Web.Dimension1", "IX_Dimension1_Dimension1Name");
            DropIndex("Web.JobOrderBoms", new[] { "Dimension2Id" });
            DropIndex("Web.JobOrderBoms", new[] { "Dimension1Id" });
            DropIndex("Web.JobOrderBoms", new[] { "ProductId" });
            DropIndex("Web.JobOrderBoms", new[] { "JobOrderLineId" });
            DropIndex("Web.JobOrderBoms", new[] { "JobOrderHeaderId" });
            DropIndex("Web.DrawBackTariffHeads", "IX_DrawBackTariffHead_DrawBackTariffHeadName");
            DropIndex("Web.Divisions", "IX_Division_Division");
            DropIndex("Web.Products", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.Products", new[] { "DivisionId" });
            DropIndex("Web.Products", new[] { "UnitId" });
            DropIndex("Web.Products", new[] { "DrawBackTariffHeadId" });
            DropIndex("Web.Products", new[] { "SalesTaxGroupProductId" });
            DropIndex("Web.Products", new[] { "ProductGroupId" });
            DropIndex("Web.Products", "IX_Product_ProductName");
            DropIndex("Web.Products", "IX_Product_ProductCode");
            DropIndex("Web.BomDetails", new[] { "Dimension2Id" });
            DropIndex("Web.BomDetails", new[] { "Dimension1Id" });
            DropIndex("Web.BomDetails", new[] { "ProcessId" });
            DropIndex("Web.BomDetails", new[] { "ProductId" });
            DropIndex("Web.BomDetails", new[] { "BaseProductId" });
            DropIndex("Web.AspNetUserRoles1", new[] { "RoleId" });
            DropIndex("Web.PersonContactTypes", "IX_PersonContactType_PersonContactTypeName");
            DropIndex("Web.PersonContacts", new[] { "Person_PersonID" });
            DropIndex("Web.PersonContacts", new[] { "ContactId" });
            DropIndex("Web.PersonContacts", new[] { "PersonContactTypeId" });
            DropIndex("Web.PersonContacts", new[] { "PersonId" });
            DropIndex("Web.Countries", "IX_Country_Country");
            DropIndex("Web.States", new[] { "Charge_ChargeId" });
            DropIndex("Web.States", new[] { "Calculation_CalculationId" });
            DropIndex("Web.States", new[] { "CountryId" });
            DropIndex("Web.States", "IX_State_StateName");
            DropIndex("Web.Cities", new[] { "StateId" });
            DropIndex("Web.Cities", "IX_City_CityName");
            DropIndex("Web.Areas", "IX_Area_AreaName");
            DropIndex("Web.PersonAddresses", new[] { "AreaId" });
            DropIndex("Web.PersonAddresses", new[] { "CityId" });
            DropIndex("Web.PersonAddresses", new[] { "PersonId" });
            DropIndex("Web.AspNetUserRoles", new[] { "IdentityUser_Id" });
            DropIndex("Web.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("Web.AspNetUserLogins", new[] { "IdentityUser_Id" });
            DropIndex("Web.AspNetUserClaims", new[] { "IdentityUser_Id" });
            DropIndex("Web.Users", "UserNameIndex");
            DropIndex("Web.People", new[] { "ApplicationUser_Id" });
            DropIndex("Web.People", "IX_Person_Code");
            DropIndex("Web.Agents", new[] { "PersonID" });
            DropIndex("Web.MenuSubModules", "IX_SubModule_SubModuleName");
            DropIndex("Web.MenuModules", "IX_Module_ModuleName");
            DropIndex("Web.Menus", new[] { "ControllerActionId" });
            DropIndex("Web.Menus", "IX_Menu_MenuName");
            DropIndex("Web.DocumentCategories", "IX_DocumentCategory_DocumentCategoryName");
            DropIndex("Web.MvcControllers", new[] { "ParentControllerId" });
            DropIndex("Web.MvcControllers", "IX_Controller_ControllerName");
            DropIndex("Web.ControllerActions", "IX_ControllerAction_ControllerName");
            DropIndex("Web.ControllerActions", new[] { "ControllerId" });
            DropIndex("Web.DocumentTypes", new[] { "ReportMenuId" });
            DropIndex("Web.DocumentTypes", new[] { "ControllerActionId" });
            DropIndex("Web.DocumentTypes", new[] { "DocumentCategoryId" });
            DropIndex("Web.DocumentTypes", "IX_DocumentType_DocumentTypeName");
            DropIndex("Web.DocumentTypes", "IX_DocumentType_DocumentTypeShortName");
            DropIndex("Web.ActivityTypes", "IX_ActivityType_ActivityTypeName");
            DropIndex("Web.ActivityLogs", new[] { "ActivityType" });
            DropIndex("Web.ActivityLogs", new[] { "DocTypeId" });
            DropTable("Web.ProductManufacturer");
            DropTable("Web.SaleInvoiceHeaderDetail");
            DropTable("Web.FinishedProduct");
            DropTable("Web.WeavingRetensions");
            DropTable("Web.ViewStockInBalance");
            DropTable("Web.ViewSaleOrderLine");
            DropTable("Web.ViewSaleOrderHeader");
            DropTable("Web.ViewSaleOrderBalanceForCancellation");
            DropTable("Web.ViewSaleOrderBalance");
            DropTable("Web.ViewSaleInvoiceLine");
            DropTable("Web.ViewSaleInvoiceBalance");
            DropTable("Web.ViewSaleDispatchBalance");
            DropTable("Web.ViewSaleDeliveryOrderBalance");
            DropTable("Web.ViewRugSize");
            DropTable("Web.ViewRugArea");
            DropTable("Web._Roles");
            DropTable("Web.ViewRequisitionBalance");
            DropTable("Web.ViewPurchaseOrderLine");
            DropTable("Web.ViewPurchaseOrderHeader");
            DropTable("Web.ViewPurchaseOrderBalanceForInvoice");
            DropTable("Web.ViewPurchaseOrderBalance");
            DropTable("Web.ViewPurchaseInvoiceBalance");
            DropTable("Web.ViewPurchaseIndentLine");
            DropTable("Web.ViewPurchaseIndentHeader");
            DropTable("Web.ViewPurchaseIndentBalance");
            DropTable("Web.ViewPurchaseGoodsReceiptLine");
            DropTable("Web.ViewPurchaseGoodsReceiptBalance");
            DropTable("Web.ViewProdOrderLine");
            DropTable("Web.ViewProdOrderHeader");
            DropTable("Web.ViewProdOrderBalanceForMPlan");
            DropTable("Web.ViewProdOrderBalance");
            DropTable("Web.ViewMaterialRequestBalance");
            DropTable("Web.ViewMaterialPlanBalance");
            DropTable("Web.ViewJobReceiveBalanceForQA");
            DropTable("Web.ViewJobReceiveBalanceForInvoice");
            DropTable("Web.ViewJobReceiveBalance");
            DropTable("Web.ViewJobOrderLine");
            DropTable("Web.ViewJobOrderInspectionRequestBalance");
            DropTable("Web.ViewJobOrderHeader");
            DropTable("Web.ViewJobOrderBalanceFromStatus");
            DropTable("Web.ViewJobOrderBalanceForInvoice");
            DropTable("Web.ViewJobOrderBalanceForInspectionRequest");
            DropTable("Web.ViewJobOrderBalanceForInspection");
            DropTable("Web.ViewJobOrderBalance");
            DropTable("Web.ViewJobInvoiceBalanceForRateAmendment");
            DropTable("Web.ViewJobInvoiceBalance");
            DropTable("Web.UserRoles");
            DropTable("Web.UserPersons");
            DropTable("Web.UserInfoes");
            DropTable("Web.UserBookMarks");
            DropTable("Web.UrgentLists");
            DropTable("Web.UnitConversions");
            DropTable("Web.TrialBalanceSettings");
            DropTable("Web.TempUserStores");
            DropTable("Web.TdsRates");
            DropTable("Web.StockVirtual");
            DropTable("Web.StockUid");
            DropTable("Web.StockProcessBalances");
            DropTable("Web.StockInOuts");
            DropTable("Web.StockInHandSettings");
            DropTable("Web.StockBalances");
            DropTable("Web.StockAdjs");
            DropTable("Web.SchemeRateDetails");
            DropTable("Web.SchemeHeaders");
            DropTable("Web.SchemeDateDetails");
            DropTable("Web.SaleOrderSettings");
            DropTable("Web.SaleOrderRateAmendmentLines");
            DropTable("Web.SaleOrderQtyAmendmentLines");
            DropTable("Web.SaleOrderLineStatus");
            DropTable("Web.SaleOrderCancelLines");
            DropTable("Web.SaleOrderCancelHeaders");
            DropTable("Web.SaleOrderAmendmentHeaders");
            DropTable("Web.SaleInvoiceSettings");
            DropTable("Web.SaleInvoiceReturnLineCharges");
            DropTable("Web.SaleInvoiceReturnLines");
            DropTable("Web.SaleInvoiceReturnHeaderCharges");
            DropTable("Web.SaleInvoiceReturnHeaders");
            DropTable("Web.SaleInvoiceLineCharges");
            DropTable("Web.SaleInvoiceHeaderCharges");
            DropTable("Web.SaleDispatchSettings");
            DropTable("Web.SaleDispatchReturnLines");
            DropTable("Web.SaleDispatchReturnHeaders");
            DropTable("Web.SaleDeliveryOrderSettings");
            DropTable("Web.SaleDeliveryOrderCancelLines");
            DropTable("Web.SaleDeliveryOrderCancelHeaders");
            DropTable("Web.RugStencils");
            DropTable("Web.Rug_RetentionPercentage");
            DropTable("Web.RouteLines");
            DropTable("Web.RolesSites");
            DropTable("Web.RolesMenus");
            DropTable("Web.RolesDivisions");
            DropTable("Web.RolesControllerActions");
            DropTable("Web.AspNetRoles");
            DropTable("Web.RequisitionSettings");
            DropTable("Web.RequisitionLineStatus");
            DropTable("Web.RequisitionCancelLines");
            DropTable("Web.RequisitionCancelHeaders");
            DropTable("Web.ReportUIDValues");
            DropTable("Web.SubReports");
            DropTable("Web.ReportLines");
            DropTable("Web.ReportHeaders");
            DropTable("Web.ReportColumns");
            DropTable("Web.RateListProductRateGroups");
            DropTable("Web.RateListPersonRateGroups");
            DropTable("Web.RateListLineHistories");
            DropTable("Web.RateListLines");
            DropTable("Web.RateListHistories");
            DropTable("Web.RateListHeaders");
            DropTable("Web.RateLists");
            DropTable("Web.RateConversionSettings");
            DropTable("Web.PurchaseQuotationSettings");
            DropTable("Web.PurchaseQuotationLineCharges");
            DropTable("Web.PurchaseQuotationHeaderCharges");
            DropTable("Web.PurchaseQuotationLines");
            DropTable("Web.PurchaseQuotationHeaders");
            DropTable("Web.PurchaseOrderSettings");
            DropTable("Web.PurchaseOrderRateAmendmentLineCharges");
            DropTable("Web.PurchaseOrderRateAmendmentLines");
            DropTable("Web.PurchaseOrderQtyAmendmentLines");
            DropTable("Web.PurchaseOrderLineStatus");
            DropTable("Web.PurchaseOrderLineCharges");
            DropTable("Web.PurchaseOrderInspectionSettings");
            DropTable("Web.PurchaseOrderInspectionRequestSettings");
            DropTable("Web.PurchaseOrderInspectionRequestCancelLines");
            DropTable("Web.PurchaseOrderInspectionRequestCancelHeaders");
            DropTable("Web.PurchaseOrderInspectionRequestHeaders");
            DropTable("Web.PurchaseOrderInspectionRequestLines");
            DropTable("Web.PurchaseOrderInspectionLines");
            DropTable("Web.PurchaseOrderInspectionHeaders");
            DropTable("Web.PurchaseOrderHeaderStatus");
            DropTable("Web.PurchaseOrderHeaderCharges");
            DropTable("Web.PurchaseOrderAmendmentHeaderCharges");
            DropTable("Web.PurchaseOrderAmendmentHeaders");
            DropTable("Web.PurchaseInvoiceSettings");
            DropTable("Web.PurchaseInvoiceReturnLineCharges");
            DropTable("Web.PurchaseInvoiceReturnLines");
            DropTable("Web.PurchaseInvoiceReturnHeaderCharges");
            DropTable("Web.PurchaseInvoiceReturnHeaders");
            DropTable("Web.PurchaseInvoiceRateAmendmentLines");
            DropTable("Web.PurchaseInvoiceLineCharges");
            DropTable("Web.PurchaseInvoiceHeaderCharges");
            DropTable("Web.PurchaseInvoiceLines");
            DropTable("Web.PurchaseInvoiceHeaders");
            DropTable("Web.PurchaseInvoiceAmendmentHeaders");
            DropTable("Web.PurchaseIndentSettings");
            DropTable("Web.PurchaseIndentLineStatus");
            DropTable("Web.PurchaseGoodsReturnLines");
            DropTable("Web.PurchaseGoodsReturnHeaders");
            DropTable("Web.PurchaseGoodsReceiptSettings");
            DropTable("Web.ProductUidSiteDetails");
            DropTable("Web.Tags");
            DropTable("Web.ProductTags");
            DropTable("Web.ProductSiteDetails");
            DropTable("Web.ProductShapes");
            DropTable("Web.ProductProcesses");
            DropTable("Web.ProductionOrderSettings");
            DropTable("Web.ProductCustomGroupLines");
            DropTable("Web.ProductCustomGroupHeaders");
            DropTable("Web.ProductAlias");
            DropTable("Web.ProdOrderSettings");
            DropTable("Web.ProdOrderLineStatus");
            DropTable("Web.ProdOrderHeaderStatus");
            DropTable("Web.ProdOrderCancelLines");
            DropTable("Web.ProdOrderCancelHeaders");
            DropTable("Web.ProcessSettings");
            DropTable("Web.ProductRateGroups");
            DropTable("Web.ProcessSequenceLines");
            DropTable("Web.PersonRegistrations");
            DropTable("Web.PersonProcesses");
            DropTable("Web.PersonDocuments");
            DropTable("Web.PersonCustomGroupLines");
            DropTable("Web.PersonCustomGroupHeaders");
            DropTable("Web.PersonBankAccounts");
            DropTable("Web.PerkDocumentTypes");
            DropTable("Web.PackingSettings");
            DropTable("Web.Narrations");
            DropTable("Web.MaterialRequestSettings");
            DropTable("Web.MaterialReceiveSettings");
            DropTable("Web.MaterialPlanSettings");
            DropTable("Web.MaterialPlanForSaleOrderLines");
            DropTable("Web.MaterialPlanForSaleOrders");
            DropTable("Web.MaterialPlanForProdOrderLines");
            DropTable("Web.MaterialPlanForProdOrders");
            DropTable("Web.MaterialPlanForJobOrderLines");
            DropTable("Web.MaterialPlanForJobOrders");
            DropTable("Web.StockHeaderSettings");
            DropTable("Web.Manufacturers");
            DropTable("Web.LedgerSettings");
            DropTable("Web.LedgerLineRefValues");
            DropTable("Web.LedgerAdjs");
            DropTable("Web.LedgerLines");
            DropTable("Web.Ledgers");
            DropTable("Web.LeaveTypes");
            DropTable("Web.JobReturnBoms");
            DropTable("Web.JobReceiveSettings");
            DropTable("Web.JobReceiveQASettings");
            DropTable("Web.JobReceiveQALines");
            DropTable("Web.JobReceiveQAHeaders");
            DropTable("Web.JobReceiveLineStatus");
            DropTable("Web.JobReceiveByProducts");
            DropTable("Web.JobReceiveBoms");
            DropTable("Web.JobOrderSettings");
            DropTable("Web.JobOrderRateAmendmentLines");
            DropTable("Web.JobOrderQtyAmendmentLines");
            DropTable("Web.Perks");
            DropTable("Web.JobOrderPerks");
            DropTable("Web.JobOrderLineStatus");
            DropTable("Web.JobOrderLineExtendeds");
            DropTable("Web.JobOrderLineCharges");
            DropTable("Web.JobOrderJobOrders");
            DropTable("Web.JobOrderInspectionSettings");
            DropTable("Web.JobOrderInspectionRequestSettings");
            DropTable("Web.JobOrderInspectionRequestCancelLines");
            DropTable("Web.JobOrderInspectionRequestCancelHeaders");
            DropTable("Web.JobOrderInspectionRequestHeaders");
            DropTable("Web.JobOrderInspectionRequestLines");
            DropTable("Web.JobOrderInspectionLines");
            DropTable("Web.JobOrderInspectionHeaders");
            DropTable("Web.JobOrderHeaderStatus");
            DropTable("Web.JobOrderHeaderCharges");
            DropTable("Web.JobOrderCancelLines");
            DropTable("Web.JobOrderCancelHeaders");
            DropTable("Web.JobOrderCancelBoms");
            DropTable("Web.JobOrderByProducts");
            DropTable("Web.JobOrderAmendmentHeaders");
            DropTable("Web.JobInvoiceSettings");
            DropTable("Web.JobInvoiceReturnLineCharges");
            DropTable("Web.JobInvoiceReturnLines");
            DropTable("Web.JobInvoiceReturnHeaderCharges");
            DropTable("Web.JobReturnLines");
            DropTable("Web.JobReturnHeaders");
            DropTable("Web.JobInvoiceReturnHeaders");
            DropTable("Web.JobInvoiceRateAmendmentLineCharges");
            DropTable("Web.JobInvoiceRateAmendmentLines");
            DropTable("Web.JobInvoiceLineStatus");
            DropTable("Web.JobInvoiceLineCharges");
            DropTable("Web.JobInvoiceHeaderCharges");
            DropTable("Web.JobReceiveHeaders");
            DropTable("Web.JobReceiveLines");
            DropTable("Web.JobInvoiceLines");
            DropTable("Web.JobInvoiceHeaders");
            DropTable("Web.JobInvoiceAmendmentHeaderCharges");
            DropTable("Web.JobInvoiceAmendmentHeaders");
            DropTable("Web.JobInstructions");
            DropTable("Web.JobConsumptionSettings");
            DropTable("Web.ProductTypeQaAttributes");
            DropTable("Web.InspectionQaAttributes");
            DropTable("Web.ExcessMaterialSettings");
            DropTable("Web.ExcessMaterialLines");
            DropTable("Web.ExcessMaterialHeaders");
            DropTable("Web.DocumentTypeTimePlans");
            DropTable("Web.DocumentTypeTimeExtensions");
            DropTable("Web.DocumentTypeSites");
            DropTable("Web.DocumentTypeDivisions");
            DropTable("Web.DocumentStatus");
            DropTable("Web.DocumentAttachments");
            DropTable("Web.DocSmsContents");
            DropTable("Web.DocNotificationContents");
            DropTable("Web.DocEmailContents");
            DropTable("Web.DispatchWaybillLines");
            DropTable("Web.Routes");
            DropTable("Web.DispatchWaybillHeaders");
            DropTable("Web.PromoCodes");
            DropTable("Web.SaleInvoiceLines");
            DropTable("Web.SaleDispatchLines");
            DropTable("Web.SaleDispatchHeaders");
            DropTable("Web.SaleInvoiceHeaders");
            DropTable("Web.CustomDetails");
            DropTable("Web.CurrencyConversions");
            DropTable("Web.Couriers");
            DropTable("Web.Counters");
            DropTable("Web.CostCenterStatusExtendeds");
            DropTable("Web.CostCenterStatus");
            DropTable("Web.Companies");
            DropTable("Web.ChargeGroupProducts");
            DropTable("Web.CalculationProducts");
            DropTable("Web.CalculationLineLedgerAccounts");
            DropTable("Web.CalculationHeaderLedgerAccounts");
            DropTable("Web.Charges");
            DropTable("Web.CalculationFooters");
            DropTable("Web.Calculations");
            DropTable("Web.BusinessSessions");
            DropTable("Web.TdsGroups");
            DropTable("Web.TdsCategories");
            DropTable("Web.ServiceTaxCategories");
            DropTable("Web.PersonRateGroups");
            DropTable("Web.BusinessEntities");
            DropTable("Web.ProductStyles");
            DropTable("Web.ProductQualities");
            DropTable("Web.ProductInvoiceGroups");
            DropTable("Web.ProductDesignPatterns");
            DropTable("Web.ProductCollections");
            DropTable("Web.ProcessSequenceHeaders");
            DropTable("Web.DescriptionOfGoods");
            DropTable("Web.ProductContentLines");
            DropTable("Web.ProductContentHeaders");
            DropTable("Web.Colours");
            DropTable("Web.SalesTaxGroupProducts");
            DropTable("Web.SalesTaxGroups");
            DropTable("Web.ChargeTypes");
            DropTable("Web.ChargeGroupPersons");
            DropTable("Web.PurchaseOrderHeaders");
            DropTable("Web.PurchaseOrderCancelHeaders");
            DropTable("Web.PurchaseOrderCancelLines");
            DropTable("Web.PurchaseOrderLines");
            DropTable("Web.PurchaseIndentHeaders");
            DropTable("Web.PurchaseIndentCancelHeaders");
            DropTable("Web.PurchaseIndentCancelLines");
            DropTable("Web.PurchaseIndentLines");
            DropTable("Web.Transporters");
            DropTable("Web.PurchaseWaybills");
            DropTable("Web.GateIns");
            DropTable("Web.PurchaseGoodsReceiptHeaders");
            DropTable("Web.PurchaseGoodsReceiptLines");
            DropTable("Web.SalesTaxGroupParties");
            DropTable("Web.Suppliers");
            DropTable("Web.ProductSuppliers");
            DropTable("Web.Sizes");
            DropTable("Web.ProductSizeTypes");
            DropTable("Web.ProductSizes");
            DropTable("Web.ProductRelatedAccessories");
            DropTable("Web.ProductIncludedAccessories");
            DropTable("Web.ProductBuyers");
            DropTable("Web.ProductAttributes");
            DropTable("Web.Designations");
            DropTable("Web.Departments");
            DropTable("Web.Employees");
            DropTable("Web.ProductUidHeaders");
            DropTable("Web.UnitConversionFors");
            DropTable("Web.DeliveryTerms");
            DropTable("Web.SaleOrderHeaders");
            DropTable("Web.SaleOrderLines");
            DropTable("Web.SaleDeliveryOrderHeaders");
            DropTable("Web.SaleDeliveryOrderLines");
            DropTable("Web.Reasons");
            DropTable("Web.RequisitionHeaders");
            DropTable("Web.RequisitionLines");
            DropTable("Web.StockProcesses");
            DropTable("Web.Stocks");
            DropTable("Web.StockLines");
            DropTable("Web.Machines");
            DropTable("Web.LedgerHeaders");
            DropTable("Web.Currencies");
            DropTable("Web.StockHeaders");
            DropTable("Web.ShipMethods");
            DropTable("Web.PackingHeaders");
            DropTable("Web.PackingLines");
            DropTable("Web.ProductUids");
            DropTable("Web.ProdOrderHeaders");
            DropTable("Web.Buyers");
            DropTable("Web.MaterialPlanHeaders");
            DropTable("Web.MaterialPlanLines");
            DropTable("Web.ProdOrderLines");
            DropTable("Web.JobOrderLines");
            DropTable("Web.Units");
            DropTable("Web.GatePassLines");
            DropTable("Web.GatePassHeaders");
            DropTable("Web.Gates");
            DropTable("Web.Godowns");
            DropTable("Web.Sites");
            DropTable("Web.Processes");
            DropTable("Web.LedgerAccountGroups");
            DropTable("Web.LedgerAccounts");
            DropTable("Web.CostCenters");
            DropTable("Web.JobWorkers");
            DropTable("Web.JobOrderHeaders");
            DropTable("Web.Dimension2");
            DropTable("Web.ProductTypeAttributes");
            DropTable("Web.ProductNatures");
            DropTable("Web.ProductGroups");
            DropTable("Web.ProductDesigns");
            DropTable("Web.ProductCategories");
            DropTable("Web.Dimension2Types");
            DropTable("Web.Dimension1Types");
            DropTable("Web.ProductTypes");
            DropTable("Web.Dimension1");
            DropTable("Web.JobOrderBoms");
            DropTable("Web.DrawBackTariffHeads");
            DropTable("Web.Divisions");
            DropTable("Web.Products");
            DropTable("Web.BomDetails");
            DropTable("Web.AspNetUserRoles1");
            DropTable("Web.AspNetRoles1");
            DropTable("Web.PersonContactTypes");
            DropTable("Web.PersonContacts");
            DropTable("Web.Countries");
            DropTable("Web.States");
            DropTable("Web.Cities");
            DropTable("Web.Areas");
            DropTable("Web.PersonAddresses");
            DropTable("Web.AspNetUserRoles");
            DropTable("Web.AspNetUserLogins");
            DropTable("Web.AspNetUserClaims");
            DropTable("Web.Users");
            DropTable("Web.People");
            DropTable("Web.Agents");
            DropTable("Web.MenuSubModules");
            DropTable("Web.MenuModules");
            DropTable("Web.Menus");
            DropTable("Web.DocumentCategories");
            DropTable("Web.MvcControllers");
            DropTable("Web.ControllerActions");
            DropTable("Web.DocumentTypes");
            DropTable("Web.ActivityTypes");
            DropTable("Web.ActivityLogs");
            DropTable("Web._Users");
        }
    }
}
