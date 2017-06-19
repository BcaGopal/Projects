namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImportMessages : DbMigration
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
            DropIndex("Web.LedgerAccounts", "IX_LedgerAccount_LedgerAccountName");
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
                "Web.ImportMessages",
                c => new
                    {
                        ImportMessageId = c.Int(nullable: false, identity: true),
                        ImportKey = c.String(maxLength: 50),
                        ImportName = c.String(maxLength: 100),
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
                .PrimaryKey(t => t.ImportMessageId);
            
            CreateTable(
                "Web.ViewLedgerBalance",
                c => new
                    {
                        LedgerId = c.Int(nullable: false, identity: true),
                        LedgerHeaderId = c.Int(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        Priority = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        LedgerAccountId = c.Int(nullable: false),
                        Nature = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.LedgerId);
            
            AddColumn("Web.Sites", "ReportHeaderTextLeft", c => c.String());
            AddColumn("Web.Sites", "ReportHeaderTextRight", c => c.String());
            AddColumn("Web.Products", "DefaultDimension1Id", c => c.Int());
            AddColumn("Web.Products", "DefaultDimension2Id", c => c.Int());
            AddColumn("Web.Products", "DefaultDimension3Id", c => c.Int());
            AddColumn("Web.Products", "DefaultDimension4Id", c => c.Int());
            AddColumn("Web.Products", "DiscontinueDate", c => c.DateTime());
            AddColumn("Web.Products", "DiscontinueReason", c => c.String());
            AddColumn("Web.CostCenters", "ProductUidId", c => c.Int());
            AddColumn("Web.LedgerAccountGroups", "Weightage", c => c.Byte());
            AddColumn("Web.LedgerAccountGroups", "ParentLedgerAccountGroupId", c => c.Int());
            AddColumn("Web.LedgerHeaders", "PartyDocNo", c => c.String(maxLength: 50));
            AddColumn("Web.LedgerHeaders", "PartyDocDate", c => c.DateTime());
            AddColumn("Web.CarpetSkuSettings", "NameBaseOnSize", c => c.String());
            AddColumn("Web.SaleInvoiceHeaders", "TermsAndConditions", c => c.String());
            AddColumn("Web.SaleDispatchLines", "CostCenterId", c => c.Int());
            AddColumn("Web.ImportLines", "FileNo", c => c.Int(nullable: false));
            AddColumn("Web.JobInvoiceHeaders", "JobWorkerDocDate", c => c.DateTime());
            AddColumn("Web.JobInvoiceHeaders", "SalesTaxGroupPersonId", c => c.Int());
            AddColumn("Web.JobInvoiceLines", "SalesTaxGroupProductId", c => c.Int());
            AddColumn("Web.JobInvoiceSettings", "isVisibleSalesTaxGroupPerson", c => c.Boolean());
            AddColumn("Web.JobInvoiceSettings", "isVisibleSalesTaxGroupProduct", c => c.Boolean());
            AddColumn("Web.Ledgers", "BankDate", c => c.DateTime());
            AddColumn("Web.Ledgers", "ChqDate", c => c.DateTime());
            AddColumn("Web.LedgerSettings", "isVisibleAdjustmentType", c => c.Boolean());
            AddColumn("Web.LedgerSettings", "isVisiblePaymentFor", c => c.Boolean());
            AddColumn("Web.LedgerSettings", "isVisiblePartyDocNo", c => c.Boolean());
            AddColumn("Web.LedgerSettings", "isVisiblePartyDocDate", c => c.Boolean());
            AddColumn("Web.LedgerSettings", "isVisibleLedgerAdj", c => c.Boolean());
            AddColumn("Web.LedgerSettings", "filterExcludeLedgerAccountGroupHeaders", c => c.String());
            AddColumn("Web.LedgerSettings", "filterExcludeLedgerAccountGroupLines", c => c.String());
            AddColumn("Web.LedgerSettings", "IsAutoDocNo", c => c.Boolean(nullable: false));
            AddColumn("Web.LedgerSettings", "PartyDocNoCaption", c => c.String(maxLength: 50));
            AddColumn("Web.LedgerSettings", "PartyDocDateCaption", c => c.String(maxLength: 50));
            AddColumn("Web.PersonSettings", "isVisibleAadharNo", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isVisiblePersonAddressDetail", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isVisiblePersonOpeningDetail", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isVisibleLedgerAccountGroup", c => c.Boolean());
            AddColumn("Web.PersonSettings", "isMandatoryAadharNo", c => c.Boolean());
            AddColumn("Web.PersonSettings", "SqlProcPersonCode", c => c.String(maxLength: 100));
            AddColumn("Web.ProductTypeSettings", "isVisibleDefaultDimension1", c => c.Boolean());
            AddColumn("Web.ProductTypeSettings", "isVisibleDefaultDimension2", c => c.Boolean());
            AddColumn("Web.ProductTypeSettings", "isVisibleDefaultDimension3", c => c.Boolean());
            AddColumn("Web.ProductTypeSettings", "isVisibleDefaultDimension4", c => c.Boolean());
            AddColumn("Web.ProductTypeSettings", "isVisibleDiscontinueDate", c => c.Boolean());
            AddColumn("Web.ProductTypeSettings", "IndexFilterParameter", c => c.String(maxLength: 20));
            AddColumn("Web.ProductTypeSettings", "SqlProcProductCode", c => c.String(maxLength: 100));
            AddColumn("Web.SaleInvoiceSettings", "isVisibleTermsAndConditions", c => c.Boolean());
            AddColumn("Web.SaleInvoiceSettings", "SqlProcProductUidHelpList", c => c.String(maxLength: 100));
            AlterColumn("Web.People", "Suffix", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("Web.LedgerAccounts", "LedgerAccountSuffix", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("Web.Products", "DefaultDimension1Id");
            CreateIndex("Web.Products", "DefaultDimension2Id");
            CreateIndex("Web.Products", "DefaultDimension3Id");
            CreateIndex("Web.Products", "DefaultDimension4Id");
            CreateIndex("Web.CostCenters", "ProductUidId");
            CreateIndex("Web.LedgerAccounts", new[] { "LedgerAccountName", "LedgerAccountSuffix" }, unique: true, name: "IX_LedgerAccount_LedgerAccountName");
            CreateIndex("Web.SaleDispatchLines", "CostCenterId");
            CreateIndex("Web.JobInvoiceHeaders", "SalesTaxGroupPersonId");
            CreateIndex("Web.JobInvoiceLines", "SalesTaxGroupProductId");
            AddForeignKey("Web.Products", "DefaultDimension1Id", "Web.Dimension1", "Dimension1Id");
            AddForeignKey("Web.Products", "DefaultDimension2Id", "Web.Dimension2", "Dimension2Id");
            AddForeignKey("Web.Products", "DefaultDimension3Id", "Web.Dimension3", "Dimension3Id");
            AddForeignKey("Web.Products", "DefaultDimension4Id", "Web.Dimension4", "Dimension4Id");
            AddForeignKey("Web.CostCenters", "ProductUidId", "Web.ProductUids", "ProductUIDId");
            AddForeignKey("Web.SaleDispatchLines", "CostCenterId", "Web.CostCenters", "CostCenterId");
            AddForeignKey("Web.JobInvoiceLines", "SalesTaxGroupProductId", "Web.ChargeGroupProducts", "ChargeGroupProductId");
            AddForeignKey("Web.JobInvoiceHeaders", "SalesTaxGroupPersonId", "Web.ChargeGroupPersons", "ChargeGroupPersonId");
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
                        DueDate = c.DateTime(nullable: false),
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
            
            DropForeignKey("Web.JobInvoiceHeaders", "SalesTaxGroupPersonId", "Web.ChargeGroupPersons");
            DropForeignKey("Web.JobInvoiceLines", "SalesTaxGroupProductId", "Web.ChargeGroupProducts");
            DropForeignKey("Web.SaleDispatchLines", "CostCenterId", "Web.CostCenters");
            DropForeignKey("Web.CostCenters", "ProductUidId", "Web.ProductUids");
            DropForeignKey("Web.Products", "DefaultDimension4Id", "Web.Dimension4");
            DropForeignKey("Web.Products", "DefaultDimension3Id", "Web.Dimension3");
            DropForeignKey("Web.Products", "DefaultDimension2Id", "Web.Dimension2");
            DropForeignKey("Web.Products", "DefaultDimension1Id", "Web.Dimension1");
            DropIndex("Web.JobInvoiceLines", new[] { "SalesTaxGroupProductId" });
            DropIndex("Web.JobInvoiceHeaders", new[] { "SalesTaxGroupPersonId" });
            DropIndex("Web.SaleDispatchLines", new[] { "CostCenterId" });
            DropIndex("Web.LedgerAccounts", "IX_LedgerAccount_LedgerAccountName");
            DropIndex("Web.CostCenters", new[] { "ProductUidId" });
            DropIndex("Web.Products", new[] { "DefaultDimension4Id" });
            DropIndex("Web.Products", new[] { "DefaultDimension3Id" });
            DropIndex("Web.Products", new[] { "DefaultDimension2Id" });
            DropIndex("Web.Products", new[] { "DefaultDimension1Id" });
            AlterColumn("Web.LedgerAccounts", "LedgerAccountSuffix", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("Web.People", "Suffix", c => c.String(nullable: false, maxLength: 20));
            DropColumn("Web.SaleInvoiceSettings", "SqlProcProductUidHelpList");
            DropColumn("Web.SaleInvoiceSettings", "isVisibleTermsAndConditions");
            DropColumn("Web.ProductTypeSettings", "SqlProcProductCode");
            DropColumn("Web.ProductTypeSettings", "IndexFilterParameter");
            DropColumn("Web.ProductTypeSettings", "isVisibleDiscontinueDate");
            DropColumn("Web.ProductTypeSettings", "isVisibleDefaultDimension4");
            DropColumn("Web.ProductTypeSettings", "isVisibleDefaultDimension3");
            DropColumn("Web.ProductTypeSettings", "isVisibleDefaultDimension2");
            DropColumn("Web.ProductTypeSettings", "isVisibleDefaultDimension1");
            DropColumn("Web.PersonSettings", "SqlProcPersonCode");
            DropColumn("Web.PersonSettings", "isMandatoryAadharNo");
            DropColumn("Web.PersonSettings", "isVisibleLedgerAccountGroup");
            DropColumn("Web.PersonSettings", "isVisiblePersonOpeningDetail");
            DropColumn("Web.PersonSettings", "isVisiblePersonAddressDetail");
            DropColumn("Web.PersonSettings", "isVisibleAadharNo");
            DropColumn("Web.LedgerSettings", "PartyDocDateCaption");
            DropColumn("Web.LedgerSettings", "PartyDocNoCaption");
            DropColumn("Web.LedgerSettings", "IsAutoDocNo");
            DropColumn("Web.LedgerSettings", "filterExcludeLedgerAccountGroupLines");
            DropColumn("Web.LedgerSettings", "filterExcludeLedgerAccountGroupHeaders");
            DropColumn("Web.LedgerSettings", "isVisibleLedgerAdj");
            DropColumn("Web.LedgerSettings", "isVisiblePartyDocDate");
            DropColumn("Web.LedgerSettings", "isVisiblePartyDocNo");
            DropColumn("Web.LedgerSettings", "isVisiblePaymentFor");
            DropColumn("Web.LedgerSettings", "isVisibleAdjustmentType");
            DropColumn("Web.Ledgers", "ChqDate");
            DropColumn("Web.Ledgers", "BankDate");
            DropColumn("Web.JobInvoiceSettings", "isVisibleSalesTaxGroupProduct");
            DropColumn("Web.JobInvoiceSettings", "isVisibleSalesTaxGroupPerson");
            DropColumn("Web.JobInvoiceLines", "SalesTaxGroupProductId");
            DropColumn("Web.JobInvoiceHeaders", "SalesTaxGroupPersonId");
            DropColumn("Web.JobInvoiceHeaders", "JobWorkerDocDate");
            DropColumn("Web.ImportLines", "FileNo");
            DropColumn("Web.SaleDispatchLines", "CostCenterId");
            DropColumn("Web.SaleInvoiceHeaders", "TermsAndConditions");
            DropColumn("Web.CarpetSkuSettings", "NameBaseOnSize");
            DropColumn("Web.LedgerHeaders", "PartyDocDate");
            DropColumn("Web.LedgerHeaders", "PartyDocNo");
            DropColumn("Web.LedgerAccountGroups", "ParentLedgerAccountGroupId");
            DropColumn("Web.LedgerAccountGroups", "Weightage");
            DropColumn("Web.CostCenters", "ProductUidId");
            DropColumn("Web.Products", "DiscontinueReason");
            DropColumn("Web.Products", "DiscontinueDate");
            DropColumn("Web.Products", "DefaultDimension4Id");
            DropColumn("Web.Products", "DefaultDimension3Id");
            DropColumn("Web.Products", "DefaultDimension2Id");
            DropColumn("Web.Products", "DefaultDimension1Id");
            DropColumn("Web.Sites", "ReportHeaderTextRight");
            DropColumn("Web.Sites", "ReportHeaderTextLeft");
            DropTable("Web.ViewLedgerBalance");
            DropTable("Web.ImportMessages");
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
            CreateIndex("Web.LedgerAccounts", new[] { "LedgerAccountName", "LedgerAccountSuffix" }, unique: true, name: "IX_LedgerAccount_LedgerAccountName");
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
