namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesTaxProductCode : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Web.LeaveHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.LeaveHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.LeaveHeaders", "EmployeeId", "Web.People");
            DropForeignKey("Web.LeaveHeaders", "PersonId", "Web.People");
            DropForeignKey("Web.LeaveHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.LeaveLines", "LeaveHeaderId", "Web.LeaveHeaders");
            DropForeignKey("Web.LeaveLines", "LeaveSlotId", "Web.LeaveSlots");
            DropForeignKey("Web.LeaveLines", "LeaveTypeId", "Web.LeaveTypes");
            DropForeignKey("Web.Loans", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.Loans", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.LoanApplications", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.LoanApplications", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.LoanApplications", "EmployeeId", "Web.People");
            DropForeignKey("Web.LoanApplications", "SiteId", "Web.Sites");
            DropForeignKey("Web.Loans", "LoanApplicationId", "Web.LoanApplications");
            DropForeignKey("Web.Loans", "PersonId", "Web.People");
            DropForeignKey("Web.Loans", "SiteId", "Web.Sites");
            DropIndex("Web.LeaveHeaders", new[] { "DocTypeId" });
            DropIndex("Web.LeaveHeaders", new[] { "SiteId" });
            DropIndex("Web.LeaveHeaders", new[] { "DivisionId" });
            DropIndex("Web.LeaveHeaders", new[] { "EmployeeId" });
            DropIndex("Web.LeaveHeaders", new[] { "PersonId" });
            DropIndex("Web.LeaveLines", new[] { "LeaveHeaderId" });
            DropIndex("Web.LeaveLines", new[] { "LeaveSlotId" });
            DropIndex("Web.LeaveLines", new[] { "LeaveTypeId" });
            DropIndex("Web.Loans", new[] { "LoanApplicationId" });
            DropIndex("Web.Loans", "IX_Loan_DocID");
            DropIndex("Web.Loans", new[] { "PersonId" });
            DropIndex("Web.Loans", new[] { "SiteId" });
            DropIndex("Web.Loans", new[] { "DivisionId" });
            DropIndex("Web.LoanApplications", "IX_LoanApplication_DocID");
            DropIndex("Web.LoanApplications", new[] { "EmployeeId" });
            DropIndex("Web.LoanApplications", new[] { "SiteId" });
            DropIndex("Web.LoanApplications", new[] { "DivisionId" });
            CreateTable(
                "Web.SalesTaxProductCodes",
                c => new
                    {
                        SalesTaxProductCodeId = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SalesTaxProductCodeId)
                .Index(t => t.Code, unique: true, name: "IX_SalesTaxProduct_SalesTaxProductCode");
            
            CreateTable(
                "Web.ImportMessages",
                c => new
                    {
                        ImportMessageId = c.Int(nullable: false, identity: true),
                        ImportHeaderId = c.Int(nullable: false),
                        SqlProcedure = c.String(maxLength: 100),
                        RecordId = c.String(maxLength: 100),
                        Head = c.String(),
                        Value = c.String(),
                        ValueType = c.String(),
                        Remark = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ImportMessageId)
                .ForeignKey("Web.ImportHeaders", t => t.ImportHeaderId)
                .Index(t => t.ImportHeaderId);
            
            AddColumn("Web.Sites", "ReportHeaderTextLeft", c => c.String());
            AddColumn("Web.Sites", "ReportHeaderTextRight", c => c.String());
            AddColumn("Web.Products", "SalesTaxProductCodeId", c => c.Int());
            AddColumn("Web.ProductGroups", "DefaultSalesTaxProductCodeId", c => c.Int());
            AddColumn("Web.JobReceiveLines", "MfgDate", c => c.DateTime());
            AddColumn("Web.JobInvoiceSettings", "SqlProcProductUidHelpList", c => c.String(maxLength: 100));
            AddColumn("Web.JobOrderSettings", "SqlProcProductUidHelpList", c => c.String(maxLength: 100));
            AddColumn("Web.JobReceiveSettings", "SqlProcProductUidHelpList", c => c.String(maxLength: 100));
            AddColumn("Web.Ledgers", "BankDate", c => c.DateTime());
            AddColumn("Web.Ledgers", "ChqDate", c => c.DateTime());
            AddColumn("Web.LedgerLines", "DrCr", c => c.String(maxLength: 2));
            AddColumn("Web.LedgerSettings", "isVisibleLineDrCr", c => c.Boolean());
            AddColumn("Web.StockHeaderSettings", "SqlProcProductUidHelpList", c => c.String(maxLength: 100));
            AddColumn("Web.PersonSettings", "SqlProcPersonCode", c => c.String(maxLength: 100));
            AddColumn("Web.ProductTypeSettings", "isVisibleSalesTaxProductCode", c => c.Boolean());
            AddColumn("Web.ProductTypeSettings", "SalesTaxProductCodeCaption", c => c.String());
            AddColumn("Web.ProductTypeSettings", "SqlProcProductCode", c => c.String(maxLength: 100));
            AddColumn("Web.SaleDeliveryLines", "Sr", c => c.Int(nullable: false));
            AddColumn("Web.SaleDeliverySettings", "WizardMenuId", c => c.Int());
            AddColumn("Web.SaleDispatchReturnLines", "GodownId", c => c.Int());
            AddColumn("Web.SaleInvoiceSettings", "SaleInvoiceReturnDocTypeId", c => c.Int());
            AddColumn("Web.SaleInvoiceSettings", "isVisibleShipToPartyAddress", c => c.Boolean());
            AddColumn("Web.TrialBalanceSettings", "ShowContraAccount", c => c.Boolean(nullable: false));
            AlterColumn("Web.JobInvoiceHeaders", "JobWorkerDocDate", c => c.DateTime());
            CreateIndex("Web.Products", "SalesTaxProductCodeId");
            CreateIndex("Web.ProductGroups", "DefaultSalesTaxProductCodeId");
            CreateIndex("Web.SaleDeliverySettings", "WizardMenuId");
            CreateIndex("Web.SaleDispatchReturnLines", "GodownId");
            CreateIndex("Web.SaleInvoiceSettings", "SaleInvoiceReturnDocTypeId");
            AddForeignKey("Web.ProductGroups", "DefaultSalesTaxProductCodeId", "Web.SalesTaxProductCodes", "SalesTaxProductCodeId");
            AddForeignKey("Web.Products", "SalesTaxProductCodeId", "Web.SalesTaxProductCodes", "SalesTaxProductCodeId");
            AddForeignKey("Web.SaleDeliverySettings", "WizardMenuId", "Web.Menus", "MenuId");
            AddForeignKey("Web.SaleDispatchReturnLines", "GodownId", "Web.Godowns", "GodownId");
            AddForeignKey("Web.SaleInvoiceSettings", "SaleInvoiceReturnDocTypeId", "Web.DocumentTypes", "DocumentTypeId");
            DropColumn("Web.JobInvoiceLines", "MfgDate");
            DropTable("Web.LeaveHeaders");
            DropTable("Web.LeaveLines");
            DropTable("Web.LeaveSlots");
            DropTable("Web.Loans");
            DropTable("Web.LoanApplications");
        }
        
        public override void Down()
        {
            CreateTable(
                "Web.LoanApplications",
                c => new
                    {
                        LoanApplicationId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        InstallMentAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        EmployeeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        Reason = c.String(),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                    })
                .PrimaryKey(t => t.LoanApplicationId);
            
            CreateTable(
                "Web.Loans",
                c => new
                    {
                        LoanId = c.Int(nullable: false, identity: true),
                        LoanApplicationId = c.Int(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        InstallMentAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        PersonId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                    })
                .PrimaryKey(t => t.LoanId);
            
            CreateTable(
                "Web.LeaveSlots",
                c => new
                    {
                        LeaveSlotId = c.Int(nullable: false, identity: true),
                        SlotName = c.String(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 4),
                        IsActive = c.Boolean(nullable: false),
                        Q1 = c.Boolean(nullable: false),
                        Q2 = c.Boolean(nullable: false),
                        Q3 = c.Boolean(nullable: false),
                        Q4 = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(),
                    })
                .PrimaryKey(t => t.LeaveSlotId);
            
            CreateTable(
                "Web.LeaveLines",
                c => new
                    {
                        LeaveLineId = c.Int(nullable: false, identity: true),
                        LeaveHeaderId = c.Int(nullable: false),
                        LeaveDate = c.DateTime(nullable: false),
                        LeaveSlotId = c.Int(nullable: false),
                        LeaveTypeId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.LeaveLineId);
            
            CreateTable(
                "Web.LeaveHeaders",
                c => new
                    {
                        LeaveHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(nullable: false, maxLength: 20),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                    })
                .PrimaryKey(t => t.LeaveHeaderId);
            
            AddColumn("Web.JobInvoiceLines", "MfgDate", c => c.DateTime());
            DropForeignKey("Web.SaleInvoiceSettings", "SaleInvoiceReturnDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.SaleDispatchReturnLines", "GodownId", "Web.Godowns");
            DropForeignKey("Web.SaleDeliverySettings", "WizardMenuId", "Web.Menus");
            DropForeignKey("Web.ImportMessages", "ImportHeaderId", "Web.ImportHeaders");
            DropForeignKey("Web.Products", "SalesTaxProductCodeId", "Web.SalesTaxProductCodes");
            DropForeignKey("Web.ProductGroups", "DefaultSalesTaxProductCodeId", "Web.SalesTaxProductCodes");
            DropIndex("Web.SaleInvoiceSettings", new[] { "SaleInvoiceReturnDocTypeId" });
            DropIndex("Web.SaleDispatchReturnLines", new[] { "GodownId" });
            DropIndex("Web.SaleDeliverySettings", new[] { "WizardMenuId" });
            DropIndex("Web.ImportMessages", new[] { "ImportHeaderId" });
            DropIndex("Web.SalesTaxProductCodes", "IX_SalesTaxProduct_SalesTaxProductCode");
            DropIndex("Web.ProductGroups", new[] { "DefaultSalesTaxProductCodeId" });
            DropIndex("Web.Products", new[] { "SalesTaxProductCodeId" });
            AlterColumn("Web.JobInvoiceHeaders", "JobWorkerDocDate", c => c.DateTime(nullable: false));
            DropColumn("Web.TrialBalanceSettings", "ShowContraAccount");
            DropColumn("Web.SaleInvoiceSettings", "isVisibleShipToPartyAddress");
            DropColumn("Web.SaleInvoiceSettings", "SaleInvoiceReturnDocTypeId");
            DropColumn("Web.SaleDispatchReturnLines", "GodownId");
            DropColumn("Web.SaleDeliverySettings", "WizardMenuId");
            DropColumn("Web.SaleDeliveryLines", "Sr");
            DropColumn("Web.ProductTypeSettings", "SqlProcProductCode");
            DropColumn("Web.ProductTypeSettings", "SalesTaxProductCodeCaption");
            DropColumn("Web.ProductTypeSettings", "isVisibleSalesTaxProductCode");
            DropColumn("Web.PersonSettings", "SqlProcPersonCode");
            DropColumn("Web.StockHeaderSettings", "SqlProcProductUidHelpList");
            DropColumn("Web.LedgerSettings", "isVisibleLineDrCr");
            DropColumn("Web.LedgerLines", "DrCr");
            DropColumn("Web.Ledgers", "ChqDate");
            DropColumn("Web.Ledgers", "BankDate");
            DropColumn("Web.JobReceiveSettings", "SqlProcProductUidHelpList");
            DropColumn("Web.JobOrderSettings", "SqlProcProductUidHelpList");
            DropColumn("Web.JobInvoiceSettings", "SqlProcProductUidHelpList");
            DropColumn("Web.JobReceiveLines", "MfgDate");
            DropColumn("Web.ProductGroups", "DefaultSalesTaxProductCodeId");
            DropColumn("Web.Products", "SalesTaxProductCodeId");
            DropColumn("Web.Sites", "ReportHeaderTextRight");
            DropColumn("Web.Sites", "ReportHeaderTextLeft");
            DropTable("Web.ImportMessages");
            DropTable("Web.SalesTaxProductCodes");
            CreateIndex("Web.LoanApplications", "DivisionId");
            CreateIndex("Web.LoanApplications", "SiteId");
            CreateIndex("Web.LoanApplications", "EmployeeId");
            CreateIndex("Web.LoanApplications", new[] { "DocTypeId", "DocNo" }, unique: true, name: "IX_LoanApplication_DocID");
            CreateIndex("Web.Loans", "DivisionId");
            CreateIndex("Web.Loans", "SiteId");
            CreateIndex("Web.Loans", "PersonId");
            CreateIndex("Web.Loans", new[] { "DocTypeId", "DocNo" }, unique: true, name: "IX_Loan_DocID");
            CreateIndex("Web.Loans", "LoanApplicationId");
            CreateIndex("Web.LeaveLines", "LeaveTypeId");
            CreateIndex("Web.LeaveLines", "LeaveSlotId");
            CreateIndex("Web.LeaveLines", "LeaveHeaderId");
            CreateIndex("Web.LeaveHeaders", "PersonId");
            CreateIndex("Web.LeaveHeaders", "EmployeeId");
            CreateIndex("Web.LeaveHeaders", "DivisionId");
            CreateIndex("Web.LeaveHeaders", "SiteId");
            CreateIndex("Web.LeaveHeaders", "DocTypeId");
            AddForeignKey("Web.Loans", "SiteId", "Web.Sites", "SiteId");
            AddForeignKey("Web.Loans", "PersonId", "Web.People", "PersonID");
            AddForeignKey("Web.Loans", "LoanApplicationId", "Web.LoanApplications", "LoanApplicationId");
            AddForeignKey("Web.LoanApplications", "SiteId", "Web.Sites", "SiteId");
            AddForeignKey("Web.LoanApplications", "EmployeeId", "Web.People", "PersonID");
            AddForeignKey("Web.LoanApplications", "DocTypeId", "Web.DocumentTypes", "DocumentTypeId");
            AddForeignKey("Web.LoanApplications", "DivisionId", "Web.Divisions", "DivisionId");
            AddForeignKey("Web.Loans", "DocTypeId", "Web.DocumentTypes", "DocumentTypeId");
            AddForeignKey("Web.Loans", "DivisionId", "Web.Divisions", "DivisionId");
            AddForeignKey("Web.LeaveLines", "LeaveTypeId", "Web.LeaveTypes", "LeaveTypeId");
            AddForeignKey("Web.LeaveLines", "LeaveSlotId", "Web.LeaveSlots", "LeaveSlotId");
            AddForeignKey("Web.LeaveLines", "LeaveHeaderId", "Web.LeaveHeaders", "LeaveHeaderId");
            AddForeignKey("Web.LeaveHeaders", "SiteId", "Web.Sites", "SiteId");
            AddForeignKey("Web.LeaveHeaders", "PersonId", "Web.People", "PersonID");
            AddForeignKey("Web.LeaveHeaders", "EmployeeId", "Web.People", "PersonID");
            AddForeignKey("Web.LeaveHeaders", "DocTypeId", "Web.DocumentTypes", "DocumentTypeId");
            AddForeignKey("Web.LeaveHeaders", "DivisionId", "Web.Divisions", "DivisionId");
        }
    }
}
