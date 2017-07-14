using System.Collections.Generic;
using System.Web.Mvc;
using Service;
using AdminSetup.Models.Models;
using ProjLib.ViewModels;
using System.Data.SqlClient;
using System.Data;
using System;

namespace Module
{
    [Authorize]
    public class UpdateTableStructureController : Controller
    {
        string mQry = "";
        IModuleService _ModuleService;

        public UpdateTableStructureController(IModuleService mService)
        {
            _ModuleService = mService;
        }

        public ActionResult UpdateTables()
        {
            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'BinLocations'") == 0)
                {
                    mQry = @"CREATE TABLE Web.BinLocations 
	                            (
	                            BinLocationId   INT IDENTITY NOT NULL,
	                            BinLocationCode NVARCHAR (10),
	                            BinLocationName NVARCHAR (50) NOT NULL,
	                            IsActive        BIT DEFAULT ((1)) NOT NULL,
	                            CreatedBy       NVARCHAR (max),
	                            ModifiedBy      NVARCHAR (max),
	                            CreatedDate     DATETIME NOT NULL,
	                            ModifiedDate    DATETIME NOT NULL,
	                            OMSId           NVARCHAR (50),
	                            GodownId        INT,
	                            CONSTRAINT [PK_Web.BinLocations] PRIMARY KEY (BinLocationId)
	                            WITH (FILLFACTOR = 90),
	                            CONSTRAINT [FK_Web.BinLocations_Web.Godowns_GodownId] FOREIGN KEY (GodownId) REFERENCES Web.Godowns (GodownId)
	                            )
                            

                            CREATE UNIQUE INDEX IX_BinLocation_BinLocationName
	                            ON Web.BinLocations (BinLocationName)
	                            WITH (FILLFACTOR = 90)
                            ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }



            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Columns WHERE COLUMN_NAME =  'BinLocation' AND TABLE_NAME = 'ProductSiteDetails'") == 1)
                {
                    mQry = "ALTER TABLE Web.ProductSiteDetails DROP COLUMN BinLocation";
                    ExecuteQuery(mQry);
                    
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }

            AddFields("LedgerAccountGroups", "ParentLedgerAccountGroupId", "INT", "LedgerAccountGroups");
            AddFields("LedgerAccountGroups", "BSNature", "NVARCHAR (20)");
            AddFields("LedgerAccountGroups", "BSSr", "INT");
            AddFields("LedgerAccountGroups", "TradingNature", "NVARCHAR (20)");
            AddFields("LedgerAccountGroups", "TradingSr", "INT");
            AddFields("LedgerAccountGroups", "PLNature", "NVARCHAR (20)");
            AddFields("LedgerAccountGroups", "PLSr", "INT");


            AddFields("TdsRates", "LedgerAccountId", "INT", "LedgerAccounts");
            AddFields("ProductSiteDetails", "BinLocationId", "INT", "BinLocations");

            AddFields("Products", "ProductCategoryId", "INT", "ProductCategories");
            AddFields("Products", "SaleRate", " Decimal(18,4)");

            AddFields("ProductTypeSettings", "isVisibleProductDescription", "BIT");
            AddFields("ProductTypeSettings", "isVisibleProductSpecification", "BIT");
            AddFields("ProductTypeSettings", "isVisibleProductCategory", "BIT");
            AddFields("ProductTypeSettings", "isVisibleSalesTaxGroup", "BIT");
            AddFields("ProductTypeSettings", "isVisibleSaleRate", "BIT");
            AddFields("ProductTypeSettings", "isVisibleStandardCost", "BIT");
            AddFields("ProductTypeSettings", "isVisibleTags", "BIT");
            AddFields("ProductTypeSettings", "isVisibleMinimumOrderQty", "BIT");
            AddFields("ProductTypeSettings", "isVisibleReOrderLevel", "BIT");
            AddFields("ProductTypeSettings", "isVisibleGodownId", "BIT");
            AddFields("ProductTypeSettings", "isVisibleBinLocationId", "BIT");
            AddFields("ProductTypeSettings", "isVisibleProfitMargin", "BIT");
            AddFields("ProductTypeSettings", "isVisibleCarryingCost", "BIT");
            AddFields("ProductTypeSettings", "isVisibleLotManagement", "BIT");
            AddFields("ProductTypeSettings", "isVisibleConsumptionDetail", "BIT");
            AddFields("ProductTypeSettings", "isVisibleProductProcessDetail", "BIT");

            AddFields("ProductTypeSettings", "ProductNameCaption", "NVARCHAR(Max)");
            AddFields("ProductTypeSettings", "ProductCodeCaption", "NVARCHAR(Max)");
            AddFields("ProductTypeSettings", "ProductDescriptionCaption", "NVARCHAR(Max)");

            AddFields("ProductTypeSettings", "ProductSpecificationCaption", "NVARCHAR(Max)");
            AddFields("ProductTypeSettings", "ProductGroupCaption", "NVARCHAR(Max)");
            AddFields("ProductTypeSettings", "ProductCategoryCaption", "NVARCHAR(Max)");


            AddFields("ProductUids", "ProductUidSpecification", "NVARCHAR(Max)");



            AddFields("JobInvoiceHeaders", "FinancierId", "INT");

            AddFields("JobInvoiceLines", "RateDiscountPer", "Decimal(18,4)");
            AddFields("JobReceiveLines", "MfgDate", "DATETIME");

            AddFields("DocumentTypeSettings", "CostCenterCaption", "NVARCHAR (50)");
            AddFields("DocumentTypeSettings", "SpecificationCaption", "NVARCHAR (50)");

            AddFields("JobInvoiceSettings", "isGenerateProductUid", "BIT");
            AddFields("JobInvoiceSettings", "isVisiblePenalty", "BIT");
            AddFields("JobInvoiceSettings", "isVisibleIncentive", "BIT");
            AddFields("JobInvoiceSettings", "isVisibleMfgDate", "BIT");
            AddFields("JobInvoiceSettings", "isVisibleFinancier", "BIT");
            AddFields("JobInvoiceSettings", "isVisibleRateDiscountPer", "BIT");

            AddFields("JobOrderSettings", "isVisibleFinancier", "BIT");
            AddFields("JobOrderSettings", "isVisibleSalesExecutive", "BIT");
            AddFields("JobOrderHeaders", "FinancierId", "Int", "People");
            AddFields("JobOrderHeaders", "SalesExecutiveId", "Int", "People");


            AddFields("SaleOrderSettings", "isVisibleFinancier", "BIT");
            AddFields("SaleOrderSettings", "isVisibleSalesExecutive", "BIT");
            AddFields("SaleOrderHeaders", "FinancierId", "Int", "People");
            AddFields("SaleOrderHeaders", "SalesExecutiveId", "Int", "People");

            AddFields("SaleInvoiceSettings", "isVisibleFinancier", "BIT");
            AddFields("SaleInvoiceSettings", "isVisibleSalesExecutive", "BIT");
            AddFields("SaleInvoiceHeaders", "FinancierId", "Int", "People");
            AddFields("SaleInvoiceHeaders", "SalesExecutiveId", "Int", "People");

            AddFields("JobOrderSettings", "isVisibleProcessHeader", "BIT");


            AddFields("SaleInvoiceLines", "RateRemark", "nvarchar(Max)");
            AddFields("SaleInvoiceHeaderDetail", "Insurance", "Decimal(18,4)");

            AddFields("PackingLines", "FreeQty", "Decimal(18,4)");
            AddFields("SaleOrderLines", "FreeQty", "Decimal(18,4)");

            AddFields("CarpetSkuSettings", "NameBaseOnSize", "nvarchar(50)");

            AddFields("WeavingRetensions", "ProductQualityId", "INT", "ProductQualities");

            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'SaleQuotationHeaders'") == 0)
                {
                    mQry = @"CREATE TABLE Web.SaleQuotationHeaders
	                        (
	                        SaleQuotationHeaderId       INT IDENTITY NOT NULL,
	                        DocTypeId                   INT NOT NULL,
	                        DocDate                     DATETIME NOT NULL,
	                        DocNo                       NVARCHAR (20),
	                        DivisionId                  INT NOT NULL,
	                        SiteId                      INT NOT NULL,
	                        ParentSaleQuotationHeaderId INT,
	                        DueDate                     DATETIME NOT NULL,
	                        ExpiryDate                  DATETIME NOT NULL,
	                        ProcessId                   INT NOT NULL,
	                        CostCenterId                INT,
	                        SaleToBuyerId               INT NOT NULL,
	                        CurrencyId                  INT NOT NULL,
	                        TermsAndConditions          NVARCHAR (max),
	                        Status                      INT NOT NULL,
	                        UnitConversionForId         TINYINT NOT NULL,
	                        SalesTaxGroupPersonId       INT,
	                        Remark                      NVARCHAR (max),
	                        LockReason                  NVARCHAR (max),
	                        ReviewCount                 INT,
	                        ReviewBy                    NVARCHAR (max),
	                        CreatedBy                   NVARCHAR (max),
	                        CreatedDate                 DATETIME NOT NULL,
	                        ModifiedBy                  NVARCHAR (max),
	                        ModifiedDate                DATETIME NOT NULL,
	                        ReferenceDocTypeId          INT,
	                        ReferenceDocId              INT,
	                        OMSId                       NVARCHAR (50),
	                        CONSTRAINT [PK_Web.SaleQuotationHeaders] PRIMARY KEY (SaleQuotationHeaderId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaders_Web.CostCenters_CostCenterId] FOREIGN KEY (CostCenterId) REFERENCES Web.CostCenters (CostCenterId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaders_Web.Currencies_CurrencyId] FOREIGN KEY (CurrencyId) REFERENCES Web.Currencies (ID),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaders_Web.Divisions_DivisionId] FOREIGN KEY (DivisionId) REFERENCES Web.Divisions (DivisionId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaders_Web.DocumentTypes_DocTypeId] FOREIGN KEY (DocTypeId) REFERENCES Web.DocumentTypes (DocumentTypeId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaders_Web.SaleQuotationHeaders_ParentSaleQuotationHeaderId] FOREIGN KEY (ParentSaleQuotationHeaderId) REFERENCES Web.SaleQuotationHeaders (SaleQuotationHeaderId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaders_Web.Processes_ProcessId] FOREIGN KEY (ProcessId) REFERENCES Web.Processes (ProcessId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaders_Web.DocumentTypes_ReferenceDocTypeId] FOREIGN KEY (ReferenceDocTypeId) REFERENCES Web.DocumentTypes (DocumentTypeId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaders_Web.ChargeGroupPersons_SalesTaxGroupPersonId] FOREIGN KEY (SalesTaxGroupPersonId) REFERENCES Web.ChargeGroupPersons (ChargeGroupPersonId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaders_Web.People_SaleToBuyerId] FOREIGN KEY (SaleToBuyerId) REFERENCES Web.People (PersonID),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaders_Web.Sites_SiteId] FOREIGN KEY (SiteId) REFERENCES Web.Sites (SiteId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaders_Web.UnitConversionFors_UnitConversionForId] FOREIGN KEY (UnitConversionForId) REFERENCES Web.UnitConversionFors (UnitconversionForId)
	                        )
                        

                        CREATE INDEX IX_DocTypeId
	                        ON Web.SaleQuotationHeaders (DocTypeId)
                        

                        CREATE INDEX IX_DivisionId
	                        ON Web.SaleQuotationHeaders (DivisionId)
                        

                        CREATE INDEX IX_SiteId
	                        ON Web.SaleQuotationHeaders (SiteId)
                        

                        CREATE INDEX IX_ParentSaleQuotationHeaderId
	                        ON Web.SaleQuotationHeaders (ParentSaleQuotationHeaderId)
                        

                        CREATE INDEX IX_ProcessId
	                        ON Web.SaleQuotationHeaders (ProcessId)
                        

                        CREATE INDEX IX_CostCenterId
	                        ON Web.SaleQuotationHeaders (CostCenterId)
                        

                        CREATE INDEX IX_SaleToBuyerId
	                        ON Web.SaleQuotationHeaders (SaleToBuyerId)
                        

                        CREATE INDEX IX_CurrencyId
	                        ON Web.SaleQuotationHeaders (CurrencyId)
                        

                        CREATE INDEX IX_UnitConversionForId
	                        ON Web.SaleQuotationHeaders (UnitConversionForId)
                        

                        CREATE INDEX IX_SalesTaxGroupPersonId
	                        ON Web.SaleQuotationHeaders (SalesTaxGroupPersonId)
                        

                        CREATE INDEX IX_ReferenceDocTypeId
	                        ON Web.SaleQuotationHeaders (ReferenceDocTypeId)
                        ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }



            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'SaleQuotationHeaderDetails'") == 0)
                {
                    mQry = @"CREATE TABLE Web.SaleQuotationHeaderDetails
	                        (
	                        SaleQuotationHeaderId INT NOT NULL,
	                        Priority              INT NOT NULL,
	                        ShipMethodId          INT NOT NULL,
	                        IsDoorDelivery        BIT,
	                        DeliveryTermsId       INT NOT NULL,
	                        CreditDays            INT NOT NULL,
	                        FinancierId           INT,
	                        SalesExecutiveId      INT,
	                        AgentId               INT,
	                        PayTermAdvancePer     DECIMAL (18, 4),
	                        PayTermOnDeliveryPer  DECIMAL (18, 4),
	                        PayTermOnDueDatePer   DECIMAL (18, 4),
	                        PayTermCashPer        DECIMAL (18, 4),
	                        PayTermBankPer        DECIMAL (18, 4),
	                        PayTermDescription    NVARCHAR (max),
	                        CONSTRAINT [PK_Web.SaleQuotationHeaderDetails] PRIMARY KEY (SaleQuotationHeaderId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderDetails_Web.SaleQuotationHeaders_SaleQuotationHeaderId] FOREIGN KEY (SaleQuotationHeaderId) REFERENCES Web.SaleQuotationHeaders (SaleQuotationHeaderId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderDetails_Web.People_AgentId] FOREIGN KEY (AgentId) REFERENCES Web.People (PersonID),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderDetails_Web.DeliveryTerms_DeliveryTermsId] FOREIGN KEY (DeliveryTermsId) REFERENCES Web.DeliveryTerms (DeliveryTermsId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderDetails_Web.People_FinancierId] FOREIGN KEY (FinancierId) REFERENCES Web.People (PersonID),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderDetails_Web.People_SalesExecutiveId] FOREIGN KEY (SalesExecutiveId) REFERENCES Web.People (PersonID),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderDetails_Web.ShipMethods_ShipMethodId] FOREIGN KEY (ShipMethodId) REFERENCES Web.ShipMethods (ShipMethodId)
	                        )
                        

                        CREATE INDEX IX_ShipMethodId
	                        ON Web.SaleQuotationHeaderDetails (ShipMethodId)
                        

                        CREATE INDEX IX_DeliveryTermsId
	                        ON Web.SaleQuotationHeaderDetails (DeliveryTermsId)
                        

                        CREATE INDEX IX_FinancierId
	                        ON Web.SaleQuotationHeaderDetails (FinancierId)
                        

                        CREATE INDEX IX_SalesExecutiveId
	                        ON Web.SaleQuotationHeaderDetails (SalesExecutiveId)
                        

                        CREATE INDEX IX_AgentId
	                        ON Web.SaleQuotationHeaderDetails (AgentId)
                         ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }



            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'SaleQuotationHeaderCharges'") == 0)
                {
                    mQry = @"CREATE TABLE Web.SaleQuotationHeaderCharges
	                        (
	                        Id                         INT IDENTITY NOT NULL,
	                        HeaderTableId              INT NOT NULL,
	                        Sr                         INT NOT NULL,
	                        ChargeId                   INT NOT NULL,
	                        AddDeduct                  TINYINT,
	                        AffectCost                 BIT NOT NULL,
	                        ChargeTypeId               INT,
	                        ProductChargeId            INT,
	                        CalculateOnId              INT,
	                        PersonID                   INT,
	                        LedgerAccountDrId          INT,
	                        LedgerAccountCrId          INT,
	                        ContraLedgerAccountId      INT,
	                        CostCenterId               INT,
	                        RateType                   TINYINT NOT NULL,
	                        IncludedInBase             BIT NOT NULL,
	                        ParentChargeId             INT,
	                        Rate                       DECIMAL (18, 4),
	                        Amount                     DECIMAL (18, 4),
	                        IsVisible                  BIT NOT NULL,
	                        IncludedCharges            NVARCHAR (max),
	                        IncludedChargesCalculation NVARCHAR (max),
	                        OMSId                      NVARCHAR (50),
	                        CONSTRAINT [PK_Web.SaleQuotationHeaderCharges] PRIMARY KEY (Id),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderCharges_Web.Charges_CalculateOnId] FOREIGN KEY (CalculateOnId) REFERENCES Web.Charges (ChargeId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderCharges_Web.Charges_ChargeId] FOREIGN KEY (ChargeId) REFERENCES Web.Charges (ChargeId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderCharges_Web.ChargeTypes_ChargeTypeId] FOREIGN KEY (ChargeTypeId) REFERENCES Web.ChargeTypes (ChargeTypeId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderCharges_Web.LedgerAccounts_ContraLedgerAccountId] FOREIGN KEY (ContraLedgerAccountId) REFERENCES Web.LedgerAccounts (LedgerAccountId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderCharges_Web.CostCenters_CostCenterId] FOREIGN KEY (CostCenterId) REFERENCES Web.CostCenters (CostCenterId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderCharges_Web.LedgerAccounts_LedgerAccountCrId] FOREIGN KEY (LedgerAccountCrId) REFERENCES Web.LedgerAccounts (LedgerAccountId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderCharges_Web.LedgerAccounts_LedgerAccountDrId] FOREIGN KEY (LedgerAccountDrId) REFERENCES Web.LedgerAccounts (LedgerAccountId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderCharges_Web.Charges_ParentChargeId] FOREIGN KEY (ParentChargeId) REFERENCES Web.Charges (ChargeId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderCharges_Web.People_PersonID] FOREIGN KEY (PersonID) REFERENCES Web.People (PersonID),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderCharges_Web.Charges_ProductChargeId] FOREIGN KEY (ProductChargeId) REFERENCES Web.Charges (ChargeId),
	                        CONSTRAINT [FK_Web.SaleQuotationHeaderCharges_Web.SaleQuotationHeaders_HeaderTableId] FOREIGN KEY (HeaderTableId) REFERENCES Web.SaleQuotationHeaders (SaleQuotationHeaderId)
	                        )
                        

                        CREATE INDEX IX_HeaderTableId
	                        ON Web.SaleQuotationHeaderCharges (HeaderTableId)
                        

                        CREATE INDEX IX_ChargeId
	                        ON Web.SaleQuotationHeaderCharges (ChargeId)
                        

                        CREATE INDEX IX_ChargeTypeId
	                        ON Web.SaleQuotationHeaderCharges (ChargeTypeId)
                        

                        CREATE INDEX IX_ProductChargeId
	                        ON Web.SaleQuotationHeaderCharges (ProductChargeId)
                        

                        CREATE INDEX IX_CalculateOnId
	                        ON Web.SaleQuotationHeaderCharges (CalculateOnId)
                        

                        CREATE INDEX IX_PersonID
	                        ON Web.SaleQuotationHeaderCharges (PersonID)
                        

                        CREATE INDEX IX_LedgerAccountDrId
	                        ON Web.SaleQuotationHeaderCharges (LedgerAccountDrId)
                        

                        CREATE INDEX IX_LedgerAccountCrId
	                        ON Web.SaleQuotationHeaderCharges (LedgerAccountCrId)
                        

                        CREATE INDEX IX_ContraLedgerAccountId
	                        ON Web.SaleQuotationHeaderCharges (ContraLedgerAccountId)
                        

                        CREATE INDEX IX_CostCenterId
	                        ON Web.SaleQuotationHeaderCharges (CostCenterId)
                        

                        CREATE INDEX IX_ParentChargeId
	                        ON Web.SaleQuotationHeaderCharges (ParentChargeId)
                        ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }




            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'SaleQuotationLines'") == 0)
                {
                    mQry = @"CREATE TABLE Web.SaleQuotationLines
	                        (
	                        SaleQuotationLineId      INT IDENTITY NOT NULL,
	                        SaleQuotationHeaderId    INT NOT NULL,
	                        SaleEnquiryLineId        INT,
	                        ProductId                INT NOT NULL,
	                        Dimension1Id             INT,
	                        Dimension2Id             INT,
	                        Dimension3Id             INT,
	                        Dimension4Id             INT,
	                        Specification            NVARCHAR (50),
	                        Qty                      DECIMAL (18, 4) NOT NULL,
	                        DealUnitId               NVARCHAR (3),
	                        DealQty                  DECIMAL (18, 4) NOT NULL,
	                        UnitConversionMultiplier DECIMAL (18, 4),
	                        Rate                     DECIMAL (18, 4) NOT NULL,
	                        Amount                   DECIMAL (18, 4) NOT NULL,
	                        DiscountPer              DECIMAL (18, 4),
	                        ReferenceDocTypeId       INT,
	                        ReferenceDocLineId       INT,
	                        Sr                       INT NOT NULL,
	                        Remark                   NVARCHAR (max),
	                        LockReason               NVARCHAR (max),
	                        CreatedBy                NVARCHAR (max),
	                        ModifiedBy               NVARCHAR (max),
	                        CreatedDate              DATETIME NOT NULL,
	                        ModifiedDate             DATETIME NOT NULL,
	                        OMSId                    NVARCHAR (50),
	                        CONSTRAINT [PK_Web.SaleQuotationLines] PRIMARY KEY (SaleQuotationLineId),
	                        CONSTRAINT [FK_Web.SaleQuotationLines_Web.Units_DealUnitId] FOREIGN KEY (DealUnitId) REFERENCES Web.Units (UnitId),
	                        CONSTRAINT [FK_Web.SaleQuotationLines_Web.Dimension1_Dimension1Id] FOREIGN KEY (Dimension1Id) REFERENCES Web.Dimension1 (Dimension1Id),
	                        CONSTRAINT [FK_Web.SaleQuotationLines_Web.Dimension2_Dimension2Id] FOREIGN KEY (Dimension2Id) REFERENCES Web.Dimension2 (Dimension2Id),
	                        CONSTRAINT [FK_Web.SaleQuotationLines_Web.Dimension3_Dimension3Id] FOREIGN KEY (Dimension3Id) REFERENCES Web.Dimension3 (Dimension3Id),
	                        CONSTRAINT [FK_Web.SaleQuotationLines_Web.Dimension4_Dimension4Id] FOREIGN KEY (Dimension4Id) REFERENCES Web.Dimension4 (Dimension4Id),
	                        CONSTRAINT [FK_Web.SaleQuotationLines_Web.Products_ProductId] FOREIGN KEY (ProductId) REFERENCES Web.Products (ProductId),
	                        CONSTRAINT [FK_Web.SaleQuotationLines_Web.DocumentTypes_ReferenceDocTypeId] FOREIGN KEY (ReferenceDocTypeId) REFERENCES Web.DocumentTypes (DocumentTypeId),
	                        CONSTRAINT [FK_Web.SaleQuotationLines_Web.SaleEnquiryLines_SaleEnquiryLineId] FOREIGN KEY (SaleEnquiryLineId) REFERENCES Web.SaleEnquiryLines (SaleEnquiryLineId),
	                        CONSTRAINT [FK_Web.SaleQuotationLines_Web.SaleQuotationHeaders_SaleQuotationHeaderId] FOREIGN KEY (SaleQuotationHeaderId) REFERENCES Web.SaleQuotationHeaders (SaleQuotationHeaderId)
	                        )
                        

                        CREATE UNIQUE INDEX IX_SaleQuotationLine_SaleQuotationHeaderProductDueDate
	                        ON Web.SaleQuotationLines (SaleQuotationHeaderId, ProductId, Dimension1Id, Dimension2Id, Dimension3Id, Dimension4Id)
                        

                        CREATE INDEX IX_SaleEnquiryLineId
	                        ON Web.SaleQuotationLines (SaleEnquiryLineId)
                        

                        CREATE INDEX IX_Dimension1Id
	                        ON Web.SaleQuotationLines (Dimension1Id)
                        

                        CREATE INDEX IX_Dimension2Id
	                        ON Web.SaleQuotationLines (Dimension2Id)
                        

                        CREATE INDEX IX_Dimension3Id
	                        ON Web.SaleQuotationLines (Dimension3Id)
                        

                        CREATE INDEX IX_Dimension4Id
	                        ON Web.SaleQuotationLines (Dimension4Id)
                        

                        CREATE INDEX IX_DealUnitId
	                        ON Web.SaleQuotationLines (DealUnitId)
                        

                        CREATE INDEX IX_ReferenceDocTypeId
	                        ON Web.SaleQuotationLines (ReferenceDocTypeId)
                        ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }




            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'SaleQuotationLineCharges'") == 0)
                {
                    mQry = @"CREATE TABLE Web.SaleQuotationLineCharges
	                        (
	                        Id                         INT IDENTITY NOT NULL,
	                        LineTableId                INT NOT NULL,
	                        HeaderTableId              INT NOT NULL,
	                        Sr                         INT NOT NULL,
	                        ChargeId                   INT NOT NULL,
	                        AddDeduct                  TINYINT,
	                        AffectCost                 BIT NOT NULL,
	                        ChargeTypeId               INT,
	                        CalculateOnId              INT,
	                        PersonID                   INT,
	                        LedgerAccountDrId          INT,
	                        LedgerAccountCrId          INT,
	                        ContraLedgerAccountId      INT,
	                        CostCenterId               INT,
	                        RateType                   TINYINT NOT NULL,
	                        IncludedInBase             BIT NOT NULL,
	                        ParentChargeId             INT,
	                        Rate                       DECIMAL (18, 4),
	                        Amount                     DECIMAL (18, 4),
	                        DealQty                    DECIMAL (18, 4),
	                        IsVisible                  BIT NOT NULL,
	                        IncludedCharges            NVARCHAR (max),
	                        IncludedChargesCalculation NVARCHAR (max),
	                        OMSId                      NVARCHAR (50),
	                        CONSTRAINT [PK_Web.SaleQuotationLineCharges] PRIMARY KEY (Id),
	                        CONSTRAINT [FK_Web.SaleQuotationLineCharges_Web.Charges_CalculateOnId] FOREIGN KEY (CalculateOnId) REFERENCES Web.Charges (ChargeId),
	                        CONSTRAINT [FK_Web.SaleQuotationLineCharges_Web.Charges_ChargeId] FOREIGN KEY (ChargeId) REFERENCES Web.Charges (ChargeId),
	                        CONSTRAINT [FK_Web.SaleQuotationLineCharges_Web.ChargeTypes_ChargeTypeId] FOREIGN KEY (ChargeTypeId) REFERENCES Web.ChargeTypes (ChargeTypeId),
	                        CONSTRAINT [FK_Web.SaleQuotationLineCharges_Web.LedgerAccounts_ContraLedgerAccountId] FOREIGN KEY (ContraLedgerAccountId) REFERENCES Web.LedgerAccounts (LedgerAccountId),
	                        CONSTRAINT [FK_Web.SaleQuotationLineCharges_Web.CostCenters_CostCenterId] FOREIGN KEY (CostCenterId) REFERENCES Web.CostCenters (CostCenterId),
	                        CONSTRAINT [FK_Web.SaleQuotationLineCharges_Web.LedgerAccounts_LedgerAccountCrId] FOREIGN KEY (LedgerAccountCrId) REFERENCES Web.LedgerAccounts (LedgerAccountId),
	                        CONSTRAINT [FK_Web.SaleQuotationLineCharges_Web.LedgerAccounts_LedgerAccountDrId] FOREIGN KEY (LedgerAccountDrId) REFERENCES Web.LedgerAccounts (LedgerAccountId),
	                        CONSTRAINT [FK_Web.SaleQuotationLineCharges_Web.Charges_ParentChargeId] FOREIGN KEY (ParentChargeId) REFERENCES Web.Charges (ChargeId),
	                        CONSTRAINT [FK_Web.SaleQuotationLineCharges_Web.People_PersonID] FOREIGN KEY (PersonID) REFERENCES Web.People (PersonID),
	                        CONSTRAINT [FK_Web.SaleQuotationLineCharges_Web.SaleQuotationLines_LineTableId] FOREIGN KEY (LineTableId) REFERENCES Web.SaleQuotationLines (SaleQuotationLineId)
	                        )
                        

                        CREATE INDEX IX_LineTableId
	                        ON Web.SaleQuotationLineCharges (LineTableId)
                        

                        CREATE INDEX IX_ChargeId
	                        ON Web.SaleQuotationLineCharges (ChargeId)
                        

                        CREATE INDEX IX_ChargeTypeId
	                        ON Web.SaleQuotationLineCharges (ChargeTypeId)
                        

                        CREATE INDEX IX_CalculateOnId
	                        ON Web.SaleQuotationLineCharges (CalculateOnId)
                        

                        CREATE INDEX IX_PersonID
	                        ON Web.SaleQuotationLineCharges (PersonID)
                        

                        CREATE INDEX IX_LedgerAccountDrId
	                        ON Web.SaleQuotationLineCharges (LedgerAccountDrId)
                        

                        CREATE INDEX IX_LedgerAccountCrId
	                        ON Web.SaleQuotationLineCharges (LedgerAccountCrId)
                        

                        CREATE INDEX IX_ContraLedgerAccountId
	                        ON Web.SaleQuotationLineCharges (ContraLedgerAccountId)
                        

                        CREATE INDEX IX_CostCenterId
	                        ON Web.SaleQuotationLineCharges (CostCenterId)
                        

                        CREATE INDEX IX_ParentChargeId
	                        ON Web.SaleQuotationLineCharges (ParentChargeId)
                        ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }




            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'SaleQuotationSettings'") == 0)
                {
                    mQry = @"CREATE TABLE Web.SaleQuotationSettings
	                        (
	                        SaleQuotationSettingsId           INT IDENTITY NOT NULL,
	                        DocTypeId                         INT NOT NULL,
	                        Priority                          INT NOT NULL,
	                        ShipMethodId                      INT NOT NULL,
	                        CurrencyId                        INT NOT NULL,
	                        DeliveryTermsId                   INT NOT NULL,
	                        SiteId                            INT NOT NULL,
	                        DivisionId                        INT NOT NULL,
	                        UnitConversionForId               TINYINT NOT NULL,
	                        ProcessId                         INT NOT NULL,
	                        TermsAndConditions                NVARCHAR (max),
	                        isVisibleCurrency                 BIT,
	                        isVisibleShipMethod               BIT,
	                        isVisibleSalesTaxGroupPerson      BIT,
	                        isVisibleDoorDelivery             BIT,
	                        isVisibleCreditDays               BIT,
	                        isVisibleCostCenter               BIT,
	                        isVisibleDeliveryTerms            BIT,
	                        isVisiblePriority                 BIT,
	                        isVisibleDimension1               BIT,
	                        isVisibleDimension2               BIT,
	                        isVisibleDimension3               BIT,
	                        isVisibleDimension4               BIT,
	                        isVisibleDealUnit                 BIT,
	                        isVisibleSpecification            BIT,
	                        isVisibleProductCode              BIT,
	                        isVisibleUnitConversionFor        BIT,
	                        isVisibleFinancier                BIT,
	                        isVisibleSalesExecutive           BIT,
	                        isVisiblePaymentTerms             BIT,
	                        isUniqueCostCenter                BIT,
	                        IsPersonWiseCostCenter            BIT,
	                        isVisibleFromSaleEnquiry          BIT NOT NULL,
	                        isVisibleAgent                    BIT NOT NULL,
	                        filterLedgerAccountGroups         NVARCHAR (max),
	                        filterLedgerAccounts              NVARCHAR (max),
	                        filterProductTypes                NVARCHAR (max),
	                        filterProductGroups               NVARCHAR (max),
	                        filterProducts                    NVARCHAR (max),
	                        filterPersonRoles                 NVARCHAR (max),
	                        filterContraDocTypes              NVARCHAR (max),
	                        filterContraSites                 NVARCHAR (max),
	                        filterContraDivisions             NVARCHAR (max),
	                        FilterProductDivision             NVARCHAR (max),
	                        filterProductCategories           NVARCHAR (max),
	                        DocumentPrint                     NVARCHAR (100),
	                        NoOfPrintCopies                   INT,
	                        DealUnitId                        NVARCHAR (3),
	                        SqlProcDocumentPrint              NVARCHAR (100),
	                        SqlProcDocumentPrint_AfterSubmit  NVARCHAR (100),
	                        SqlProcDocumentPrint_AfterApprove NVARCHAR (100),
	                        ImportMenuId                      INT,
	                        WizardMenuId                      INT,
	                        ExportMenuId                      INT,
	                        CalculationId                     INT,
	                        CreatedBy                         NVARCHAR (max),
	                        ModifiedBy                        NVARCHAR (max),
	                        CreatedDate                       DATETIME NOT NULL,
	                        ModifiedDate                      DATETIME NOT NULL,
	                        OMSId                             NVARCHAR (50),
	                        CONSTRAINT [PK_Web.SaleQuotationSettings] PRIMARY KEY (SaleQuotationSettingsId),
	                        CONSTRAINT [FK_Web.SaleQuotationSettings_Web.Calculations_CalculationId] FOREIGN KEY (CalculationId) REFERENCES Web.Calculations (CalculationId),
	                        CONSTRAINT [FK_Web.SaleQuotationSettings_Web.Currencies_CurrencyId] FOREIGN KEY (CurrencyId) REFERENCES Web.Currencies (ID),
	                        CONSTRAINT [FK_Web.SaleQuotationSettings_Web.Units_DealUnitId] FOREIGN KEY (DealUnitId) REFERENCES Web.Units (UnitId),
	                        CONSTRAINT [FK_Web.SaleQuotationSettings_Web.DeliveryTerms_DeliveryTermsId] FOREIGN KEY (DeliveryTermsId) REFERENCES Web.DeliveryTerms (DeliveryTermsId),
	                        CONSTRAINT [FK_Web.SaleQuotationSettings_Web.Divisions_DivisionId] FOREIGN KEY (DivisionId) REFERENCES Web.Divisions (DivisionId),
	                        CONSTRAINT [FK_Web.SaleQuotationSettings_Web.DocumentTypes_DocTypeId] FOREIGN KEY (DocTypeId) REFERENCES Web.DocumentTypes (DocumentTypeId),
	                        CONSTRAINT [FK_Web.SaleQuotationSettings_Web.Menus_ExportMenuId] FOREIGN KEY (ExportMenuId) REFERENCES Web.Menus (MenuId),
	                        CONSTRAINT [FK_Web.SaleQuotationSettings_Web.Menus_ImportMenuId] FOREIGN KEY (ImportMenuId) REFERENCES Web.Menus (MenuId),
	                        CONSTRAINT [FK_Web.SaleQuotationSettings_Web.Processes_ProcessId] FOREIGN KEY (ProcessId) REFERENCES Web.Processes (ProcessId),
	                        CONSTRAINT [FK_Web.SaleQuotationSettings_Web.ShipMethods_ShipMethodId] FOREIGN KEY (ShipMethodId) REFERENCES Web.ShipMethods (ShipMethodId),
	                        CONSTRAINT [FK_Web.SaleQuotationSettings_Web.Sites_SiteId] FOREIGN KEY (SiteId) REFERENCES Web.Sites (SiteId),
	                        CONSTRAINT [FK_Web.SaleQuotationSettings_Web.UnitConversionFors_UnitConversionForId] FOREIGN KEY (UnitConversionForId) REFERENCES Web.UnitConversionFors (UnitconversionForId),
	                        CONSTRAINT [FK_Web.SaleQuotationSettings_Web.Menus_WizardMenuId] FOREIGN KEY (WizardMenuId) REFERENCES Web.Menus (MenuId)
	                        )
                        

                        CREATE INDEX IX_DocTypeId
	                        ON Web.SaleQuotationSettings (DocTypeId)
                        

                        CREATE INDEX IX_ShipMethodId
	                        ON Web.SaleQuotationSettings (ShipMethodId)
                        

                        CREATE INDEX IX_CurrencyId
	                        ON Web.SaleQuotationSettings (CurrencyId)
                        

                        CREATE INDEX IX_DeliveryTermsId
	                        ON Web.SaleQuotationSettings (DeliveryTermsId)
                        

                        CREATE INDEX IX_SiteId
	                        ON Web.SaleQuotationSettings (SiteId)
                        

                        CREATE INDEX IX_DivisionId
	                        ON Web.SaleQuotationSettings (DivisionId)
                        

                        CREATE INDEX IX_UnitConversionForId
	                        ON Web.SaleQuotationSettings (UnitConversionForId)
                        

                        CREATE INDEX IX_ProcessId
	                        ON Web.SaleQuotationSettings (ProcessId)
                        

                        CREATE INDEX IX_DealUnitId
	                        ON Web.SaleQuotationSettings (DealUnitId)
                        

                        CREATE INDEX IX_ImportMenuId
	                        ON Web.SaleQuotationSettings (ImportMenuId)
                        

                        CREATE INDEX IX_WizardMenuId
	                        ON Web.SaleQuotationSettings (WizardMenuId)
                        

                        CREATE INDEX IX_ExportMenuId
	                        ON Web.SaleQuotationSettings (ExportMenuId)
                        

                        CREATE INDEX IX_CalculationId
	                        ON Web.SaleQuotationSettings (CalculationId)
                        ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }


            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'SaleInvoiceLineDetails'") == 0)
                {
                    mQry = @"CREATE TABLE Web.SaleInvoiceLineDetails
	                        (
	                        SaleInvoiceLineId INT NOT NULL,
	                        RewardPoints      DECIMAL (18, 4),
	                        CONSTRAINT [PK_Web.SaleInvoiceLineDetails] PRIMARY KEY (SaleInvoiceLineId),
	                        CONSTRAINT [FK_Web.SaleInvoiceLineDetails_Web.SaleInvoiceLines_SaleInvoiceLineId] FOREIGN KEY (SaleInvoiceLineId) REFERENCES Web.SaleInvoiceLines (SaleInvoiceLineId)
	                        )

                        CREATE INDEX IX_SaleInvoiceLineId
	                        ON Web.SaleInvoiceLineDetails (SaleInvoiceLineId)";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }


            AddFields("SaleInvoiceSettings", "isVisibleFreeQty", "BIT");
            AddFields("SaleInvoiceSettings", "isVisibleRewardPoints", "BIT");

            AddFields("SaleDispatchSettings", "isVisibleFreeQty", "BIT");

            AddFields("PackingLines", "SealNo", "nvarchar(Max)");
            AddFields("PackingLines", "RateRemark", "nvarchar(Max)");
            
            AddFields("PackingLines", "FreeQty", "Decimal(18,4)");

            AddFields("SaleOrderLines", "FreeQty", "Decimal(18,4)");

            AddFields("SaleDispatchSettings", "isVisibleFreeQty", "BIT");

            AddFields("StockLines", "StockInId", "Int","Stocks");


            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'CompanySettings'") == 0)
                {
                    mQry = @"CREATE TABLE Web.CompanySettings
	                        (
	                        CompanySettingsId     INT IDENTITY NOT NULL,
	                        CompanyId             INT NOT NULL,
	                        IsVisibleMessage      BIT,
	                        IsVisibleTask         BIT,
	                        IsVisibleNotification BIT,
	                        SiteCaption           NVARCHAR (50),
	                        DivisionCaption       NVARCHAR (50),
	                        GodownCaption         NVARCHAR (50),
	                        CreatedBy             NVARCHAR (max),
	                        ModifiedBy            NVARCHAR (max),
	                        CreatedDate           DATETIME NOT NULL,
	                        ModifiedDate          DATETIME NOT NULL,
	                        OMSId                 NVARCHAR (50),
	                        CONSTRAINT [PK_Web.CompanySettings] PRIMARY KEY (CompanySettingsId),
	                        CONSTRAINT [FK_Web.CompanySettings_Web.Companies_CompanyId] FOREIGN KEY (CompanyId) REFERENCES Web.Companies (CompanyId)
	                        )


                        CREATE INDEX IX_CompanyId
	                        ON Web.CompanySettings (CompanyId)
                        ";
                    ExecuteQuery(mQry);


                    mQry = @" INSERT INTO Web.CompanySettings (CompanyId, CreatedBy, ModifiedBy, CreatedDate, ModifiedDate)
                                SELECT C.CompanyId, 'admin' AS CreatedBy, 'admin' AS ModifiedBy, getdate() AS CreatedDate, 
                                getdate() AS ModifiedDate
                                FROM Web.Companies C
                                LEFT JOIN Web.CompanySettings Cs ON C.CompanyId = Cs.CompanyId
                                WHERE Cs.CompanySettingsId IS NULL";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }


            AddFields("ProductTypeSettings", "ImportMenuId", "Int","Menus");
            AddFields("SaleInvoiceSettings", "IsVisibleCreditDays", "Bit");

            AddFields("JobReceiveLines", "ProductId", "Int","Products");
            AddFields("JobReceiveLines", "Dimension1Id", "Int", "Dimension1");
            AddFields("JobReceiveLines", "Dimension2Id", "Int", "Dimension2");
            AddFields("JobReceiveLines", "Dimension3Id", "Int", "Dimension3");
            AddFields("JobReceiveLines", "Dimension4Id", "Int", "Dimension4");
            AddFields("JobInvoiceLines", "RateDiscountAmt", "Decimal(18,4)");

            AddFields("LedgerHeaders", "DrCr", "nvarchar(2)");
            AddFields("LedgerSettings", "isVisibleDrCr", "BIT");



            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'ChargeGroupSettings'") == 0)
                {
                    mQry = @"CREATE TABLE Web.ChargeGroupSettings
	                        (
	                        ChargeGroupSettingsId    INT IDENTITY NOT NULL,
	                        ChargeTypeId             INT NOT NULL,
	                        ChargeGroupPersonId      INT NOT NULL,
	                        ChargeGroupProductId     INT NOT NULL,
	                        ChargableLedgerAccountId INT NOT NULL,
	                        ChargePer                DECIMAL (18, 4) NOT NULL,
	                        ChargeLedgerAccountId    INT NOT NULL,
	                        CreatedBy                NVARCHAR (max),
	                        ModifiedBy               NVARCHAR (max),
	                        CreatedDate              DATETIME NOT NULL,
	                        ModifiedDate             DATETIME NOT NULL,
	                        OMSId                    NVARCHAR (50),
	                        CONSTRAINT [PK_Web.ChargeGroupSettings] PRIMARY KEY (ChargeGroupSettingsId),
	                        CONSTRAINT [FK_Web.ChargeGroupSettings_Web.LedgerAccounts_ChargableLedgerAccountId] FOREIGN KEY (ChargableLedgerAccountId) REFERENCES Web.LedgerAccounts (LedgerAccountId),
	                        CONSTRAINT [FK_Web.ChargeGroupSettings_Web.ChargeGroupPersons_ChargeGroupPersonId] FOREIGN KEY (ChargeGroupPersonId) REFERENCES Web.ChargeGroupPersons (ChargeGroupPersonId),
	                        CONSTRAINT [FK_Web.ChargeGroupSettings_Web.ChargeGroupProducts_ChargeGroupProductId] FOREIGN KEY (ChargeGroupProductId) REFERENCES Web.ChargeGroupProducts (ChargeGroupProductId),
	                        CONSTRAINT [FK_Web.ChargeGroupSettings_Web.LedgerAccounts_ChargeLedgerAccountId] FOREIGN KEY (ChargeLedgerAccountId) REFERENCES Web.LedgerAccounts (LedgerAccountId),
	                        CONSTRAINT [FK_Web.ChargeGroupSettings_Web.ChargeTypes_ChargeTypeId] FOREIGN KEY (ChargeTypeId) REFERENCES Web.ChargeTypes (ChargeTypeId)
	                        )


                        CREATE INDEX IX_ChargeTypeId
	                        ON Web.ChargeGroupSettings (ChargeTypeId)


                        CREATE INDEX IX_ChargeGroupPersonId
	                        ON Web.ChargeGroupSettings (ChargeGroupPersonId)


                        CREATE INDEX IX_ChargeGroupProductId
	                        ON Web.ChargeGroupSettings (ChargeGroupProductId)


                        CREATE INDEX IX_ChargableLedgerAccountId
	                        ON Web.ChargeGroupSettings (ChargableLedgerAccountId)


                        CREATE INDEX IX_ChargeLedgerAccountId
	                        ON Web.ChargeGroupSettings (ChargeLedgerAccountId)
                        ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }


            AddFields("StockHeaderSettings", "IsVisibleReferenceDocId", "Int");
            AddFields("StockHeaderSettings", "SqlProcHelpListReferenceDocId", "nvarchar(100)");

            AddFields("StockLines", "ReferenceDocTypeId", "Int","DocumentTypes");
            AddFields("StockLines", "ReferenceDocId", "Int");
            AddFields("StockLines", "ReferenceDocLineId", "Int");


            AddFields("StockHeaderSettings", "isVisibleProcessHeader", "BIT");
            AddFields("StockHeaderSettings", "isMandatoryProductUID", "BIT");

            AddFields("DocumentTypeSettings", "ReferenceDocTypeCaption", "nvarchar(50)");
            AddFields("DocumentTypeSettings", "ReferenceDocIdCaption", "nvarchar(50)");

            AddFields("LedgerSettings", "isVisibleReferenceDocId", "BIT");
            AddFields("LedgerSettings", "isVisibleReferenceDocTypeId", "BIT");
            AddFields("LedgerSettings", "filterReferenceDocTypes", "nvarchar(Max)");


            AddFields("CompanySettings", "isVisibleCompanyName", "BIT");



            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'DocumentTypeAttributes'") == 0)
                {
                    mQry = @"CREATE TABLE Web.DocumentTypeAttributes
	                        (
	                        DocumentTypeAttributeId INT IDENTITY NOT NULL,
	                        Name                    NVARCHAR (max) NOT NULL,
	                        IsMandatory             BIT NOT NULL,
	                        DataType                NVARCHAR (max),
	                        ListItem                NVARCHAR (max),
	                        DefaultValue            NVARCHAR (max),
	                        IsActive                BIT NOT NULL,
	                        DocumentTypeId          INT NOT NULL,
	                        CreatedBy               NVARCHAR (max),
	                        ModifiedBy              NVARCHAR (max),
	                        CreatedDate             DATETIME NOT NULL,
	                        ModifiedDate            DATETIME NOT NULL,
	                        OMSId                   NVARCHAR (50),
	                        CONSTRAINT [PK_Web.DocumentTypeAttributes] PRIMARY KEY (DocumentTypeAttributeId),
	                        CONSTRAINT [FK_Web.DocumentTypeAttributes_Web.DocumentTypes_DocumentTypeId] FOREIGN KEY (DocumentTypeId) REFERENCES Web.DocumentTypes (DocumentTypeId)
	                        )


                        CREATE INDEX IX_DocumentTypeId
	                        ON Web.DocumentTypeAttributes (DocumentTypeId)                        ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }



            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'SaleInvoiceHeaderAttributes'") == 0)
                {
                    mQry = @"CREATE TABLE Web.SaleInvoiceHeaderAttributes
	                        (
	                        SaleInvoiceHeaderAttributeId    INT IDENTITY NOT NULL,
	                        SaleInvoiceHeaderId             INT NOT NULL,
	                        DocumentTypeAttributeId         INT NOT NULL,
	                        SaleInvoiceHeaderAttributeValue NVARCHAR (max),
	                        CreatedBy                       NVARCHAR (max),
	                        ModifiedBy                      NVARCHAR (max),
	                        CreatedDate                     DATETIME NOT NULL,
	                        ModifiedDate                    DATETIME NOT NULL,
	                        OMSId                           NVARCHAR (50),
	                        CONSTRAINT [PK_Web.SaleInvoiceHeaderAttributes] PRIMARY KEY (SaleInvoiceHeaderAttributeId),
	                        CONSTRAINT [FK_Web.SaleInvoiceHeaderAttributes_Web.SaleInvoiceHeaders_SaleInvoiceHeaderId] FOREIGN KEY (SaleInvoiceHeaderId) REFERENCES Web.SaleInvoiceHeaders (SaleInvoiceHeaderId),
	                        CONSTRAINT [FK_Web.SaleInvoiceHeaderAttributes_Web.DocumentTypeAttributes_DocumentTypeAttributeId] FOREIGN KEY (DocumentTypeAttributeId) REFERENCES Web.DocumentTypeAttributes (DocumentTypeAttributeId)
	                        )


                        CREATE INDEX IX_SaleInvoiceHeaderId
	                        ON Web.SaleInvoiceHeaderAttributes (SaleInvoiceHeaderId)


                        CREATE INDEX IX_DocumentTypeAttributeId
	                        ON Web.SaleInvoiceHeaderAttributes (DocumentTypeAttributeId) ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }


            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'SaleDeliveryHeaders'") == 0)
                {
                    mQry = @"CREATE TABLE Web.SaleDeliveryHeaders
	                        (
	                        SaleDeliveryHeaderId     INT IDENTITY NOT NULL,
	                        DocTypeId                INT NOT NULL,
	                        DocDate                  DATETIME NOT NULL,
	                        DocNo                    NVARCHAR (20) NOT NULL,
	                        DivisionId               INT NOT NULL,
	                        SiteId                   INT NOT NULL,
	                        SaleToBuyerId            INT NOT NULL,
	                        DeliverToPerson          NVARCHAR (100),
	                        DeliverToPersonReference NVARCHAR (20),
	                        ShipToPartyAddress       NVARCHAR (250),
	                        GatePassHeaderId         INT,
	                        Remark                   NVARCHAR (max),
	                        Status                   INT NOT NULL,
	                        ReviewCount              INT,
	                        ReviewBy                 NVARCHAR (max),
	                        CreatedBy                NVARCHAR (max),
	                        ModifiedBy               NVARCHAR (max),
	                        CreatedDate              DATETIME NOT NULL,
	                        ModifiedDate             DATETIME NOT NULL,
	                        OMSId                    NVARCHAR (50),
	                        CONSTRAINT [PK_Web..SaleDeliveryHeaders] PRIMARY KEY (SaleDeliveryHeaderId),
	                        CONSTRAINT [FK_Web.SaleDeliveryHeaders_Web.Divisions_DivisionId] FOREIGN KEY (DivisionId) REFERENCES Web.Divisions (DivisionId),
	                        CONSTRAINT [FK_Web.SaleDeliveryHeaders_Web.DocumentTypes_DocTypeId] FOREIGN KEY (DocTypeId) REFERENCES Web.DocumentTypes (DocumentTypeId),
	                        CONSTRAINT [FK_Web.SaleDeliveryHeaders_Web.GatePassHeaders_GatePassHeaderId] FOREIGN KEY (GatePassHeaderId) REFERENCES Web.GatePassHeaders (GatePassHeaderId),
	                        CONSTRAINT [FK_Web.SaleDeliveryHeaders_Web.People_SaleToBuyerId] FOREIGN KEY (SaleToBuyerId) REFERENCES Web.People (PersonID),
	                        CONSTRAINT [FK_Web.SaleDeliveryHeaders_Web.Sites_SiteId] FOREIGN KEY (SiteId) REFERENCES Web.Sites (SiteId)
	                        )


                        CREATE UNIQUE INDEX IX_SaleDeliveryHeader_DocID
	                        ON Web.SaleDeliveryHeaders (DocTypeId, DocNo, DivisionId, SiteId)


                        CREATE INDEX IX_SaleToBuyerId
	                        ON Web.SaleDeliveryHeaders (SaleToBuyerId)


                        CREATE INDEX IX_GatePassHeaderId
	                        ON Web.SaleDeliveryHeaders (GatePassHeaderId) ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }



            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'SaleDeliveryLines'") == 0)
                {
                    mQry = @"CREATE TABLE Web.SaleDeliveryLines
	                        (
	                        SaleDeliveryLineId       INT IDENTITY NOT NULL,
	                        SaleDeliveryHeaderId     INT NOT NULL,
	                        SaleInvoiceLineId        INT NOT NULL,
	                        Qty                      DECIMAL (18, 4) NOT NULL,
	                        DealUnitId               NVARCHAR (3) NOT NULL,
	                        UnitConversionMultiplier DECIMAL (18, 4) NOT NULL,
	                        DealQty                  DECIMAL (18, 4) NOT NULL,
	                        Remark                   NVARCHAR (max),
	                        CreatedBy                NVARCHAR (max),
	                        ModifiedBy               NVARCHAR (max),
	                        CreatedDate              DATETIME NOT NULL,
	                        ModifiedDate             DATETIME NOT NULL,
	                        OMSId                    NVARCHAR (50),
	                        LockReason               NVARCHAR (max),
	                        CONSTRAINT [PK_Web..SaleDeliveryLines] PRIMARY KEY (SaleDeliveryLineId),
	                        CONSTRAINT [FK_Web.SaleDeliveryLines_Web.Units_DealUnitId] FOREIGN KEY (DealUnitId) REFERENCES Web.Units (UnitId),
	                        CONSTRAINT [FK_Web.SaleDeliveryLines_Web.SaleDeliveryHeaders_SaleDeliveryHeaderId] FOREIGN KEY (SaleDeliveryHeaderId) REFERENCES Web.SaleDeliveryHeaders (SaleDeliveryHeaderId),
	                        CONSTRAINT [FK_Web.SaleDeliveryLines_Web.SaleInvoiceLines_SaleInvoiceLineId] FOREIGN KEY (SaleInvoiceLineId) REFERENCES Web.SaleInvoiceLines (SaleInvoiceLineId)
	                        )


                        CREATE INDEX IX_SaleDeliveryHeaderId
	                        ON Web.SaleDeliveryLines (SaleDeliveryHeaderId)


                        CREATE INDEX IX_SaleInvoiceLineId
	                        ON Web.SaleDeliveryLines (SaleInvoiceLineId)


                        CREATE INDEX IX_DealUnitId
	                        ON Web.SaleDeliveryLines (DealUnitId) ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }



            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'SaleDeliverySettings'") == 0)
                {
                    mQry = @"CREATE TABLE Web.SaleDeliverySettings
	                        (
	                        SaleDeliverySettingId             INT IDENTITY NOT NULL,
	                        DocTypeId                         INT NOT NULL,
	                        SiteId                            INT NOT NULL,
	                        DivisionId                        INT NOT NULL,
	                        isVisibleDimension1               BIT,
	                        isVisibleDimension2               BIT,
	                        isVisibleDimension3               BIT,
	                        isVisibleDimension4               BIT,
	                        filterLedgerAccountGroups         NVARCHAR (max),
	                        filterLedgerAccounts              NVARCHAR (max),
	                        filterProductTypes                NVARCHAR (max),
	                        filterProductGroups               NVARCHAR (max),
	                        filterProducts                    NVARCHAR (max),
	                        filterContraDocTypes              NVARCHAR (max),
	                        filterContraSites                 NVARCHAR (max),
	                        filterContraDivisions             NVARCHAR (max),
	                        filterPersonRoles                 NVARCHAR (max),
	                        SqlProcDocumentPrint              NVARCHAR (100),
	                        SqlProcDocumentPrint_AfterSubmit  NVARCHAR (100),
	                        SqlProcDocumentPrint_AfterApprove NVARCHAR (100),
	                        SqlProcGatePass                   NVARCHAR (100),
	                        UnitConversionForId               TINYINT,
	                        ImportMenuId                      INT,
	                        ExportMenuId                      INT,
	                        isVisibleDealUnit                 BIT,
	                        isVisibleProductUid               BIT,
	                        ProcessId                         INT,
	                        CreatedBy                         NVARCHAR (max),
	                        ModifiedBy                        NVARCHAR (max),
	                        CreatedDate                       DATETIME NOT NULL,
	                        ModifiedDate                      DATETIME NOT NULL,
	                        CONSTRAINT [PK_Web..SaleDeliverySettings] PRIMARY KEY (SaleDeliverySettingId),
	                        CONSTRAINT [FK_Web.SaleDeliverySettings_Web.Divisions_DivisionId] FOREIGN KEY (DivisionId) REFERENCES Web.Divisions (DivisionId),
	                        CONSTRAINT [FK_Web.SaleDeliverySettings_Web.DocumentTypes_DocTypeId] FOREIGN KEY (DocTypeId) REFERENCES Web.DocumentTypes (DocumentTypeId),
	                        CONSTRAINT [FK_Web.SaleDeliverySettings_Web.Menus_ExportMenuId] FOREIGN KEY (ExportMenuId) REFERENCES Web.Menus (MenuId),
	                        CONSTRAINT [FK_Web.SaleDeliverySettings_Web.Menus_ImportMenuId] FOREIGN KEY (ImportMenuId) REFERENCES Web.Menus (MenuId),
	                        CONSTRAINT [FK_Web.SaleDeliverySettings_Web.Processes_ProcessId] FOREIGN KEY (ProcessId) REFERENCES Web.Processes (ProcessId),
	                        CONSTRAINT [FK_Web.SaleDeliverySettings_Web.Sites_SiteId] FOREIGN KEY (SiteId) REFERENCES Web.Sites (SiteId),
	                        CONSTRAINT [FK_Web.SaleDeliverySettings_Web.UnitConversionFors_UnitConversionForId] FOREIGN KEY (UnitConversionForId) REFERENCES Web.UnitConversionFors (UnitconversionForId)
	                        )


                        CREATE INDEX IX_DocTypeId
	                        ON Web.SaleDeliverySettings (DocTypeId)


                        CREATE INDEX IX_SiteId
	                        ON Web.SaleDeliverySettings (SiteId)


                        CREATE INDEX IX_DivisionId
	                        ON Web.SaleDeliverySettings (DivisionId)


                        CREATE INDEX IX_UnitConversionForId
	                        ON Web.SaleDeliverySettings (UnitConversionForId)


                        CREATE INDEX IX_ImportMenuId
	                        ON Web.SaleDeliverySettings (ImportMenuId)


                        CREATE INDEX IX_ExportMenuId
	                        ON Web.SaleDeliverySettings (ExportMenuId)


                        CREATE INDEX IX_ProcessId
	                        ON Web.SaleDeliverySettings (ProcessId) ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }


            AddFields("LedgerSettings", "PartyDocNoCaption", "nvarchar(50)");
            AddFields("LedgerSettings", "PartyDocDateCaption", "nvarchar(50)");

            AddFields("JobReceiveLines", "Specification", "nvarchar(50)");

            AddFields("LedgerSettings", "isVisibleAdjustmentType", "BIT");
            AddFields("LedgerSettings", "isVisiblePaymentFor", "BIT");
            AddFields("LedgerSettings", "isVisiblePartyDocNo", "BIT");
            AddFields("LedgerSettings", "isVisiblePartyDocDate", "BIT");

            AddFields("LedgerHeaders", "PartyDocNo", "nvarchar(50)");
            AddFields("LedgerHeaders", "PartyDocDate", "DateTime");
            AddFields("LedgerSettings", "isVisibleLedgerAdj", "BIT");

            AddFields("PersonSettings", "isVisibleAadharNo", "BIT");
            AddFields("PersonSettings", "isMandatoryAadharNo", "BIT");
            AddFields("PersonSettings", "isVisiblePersonAddressDetail", "BIT");
            AddFields("PersonSettings", "isVisiblePersonOpeningDetail", "BIT");


            AddFields("JobInvoiceSettings", "isVisibleSalesTaxGroupProduct", "BIT");
            AddFields("JobInvoiceSettings", "isVisibleSalesTaxGroupPerson", "BIT");
            AddFields("JobInvoiceLines", "SalesTaxGroupProductId", "Int", "ChargeGroupProducts");
            AddFields("JobInvoiceHeaders", "SalesTaxGroupPersonId", "Int", "ChargeGroupPerson");


            AddFields("SaleInvoiceSettings", "SqlProcProductUidHelpList", "nvarchar(100)");

            AddFields("LedgerSettings", "IsAutoDocNo", "BIT NOT NULL DEFAULT(0)");

            AddFields("LedgerSettings", "filterExcludeLedgerAccountGroupHeaders", "nvarchar(Max)");
            AddFields("LedgerSettings", "filterExcludeLedgerAccountGroupLines", "nvarchar(Max)");
            AddFields("PersonSettings", "isVisibleLedgerAccountGroup", "BIT");



            AddFields("ProductTypeSettings", "isVisibleDefaultDimension1", "BIT");
            AddFields("ProductTypeSettings", "isVisibleDefaultDimension2", "BIT");
            AddFields("ProductTypeSettings", "isVisibleDefaultDimension3", "BIT");
            AddFields("ProductTypeSettings", "isVisibleDefaultDimension4", "BIT");


            AddFields("Products", "DefaultDimension1Id", "Int", "Dimension1");
            AddFields("Products", "DefaultDimension2Id", "Int", "Dimension2");
            AddFields("Products", "DefaultDimension3Id", "Int", "Dimension3");
            AddFields("Products", "DefaultDimension4Id", "Int", "Dimension4");


            AddFields("SaleInvoiceHeaders", "TermsAndConditions", "nvarchar(Max)");
            AddFields("SaleInvoiceSettings", "isVisibleTermsAndConditions", "BIT");

            AddFields("ProductTypeSettings", "isVisibleDiscontinueDate", "BIT");
            AddFields("Products", "DiscontinueDate", "DateTime");
            AddFields("Products", "DiscontinueReason", "nvarchar(Max)");


            AddFields("CostCenters", "ProductUidId", "Int","ProductUids");
            AddFields("SaleDispatchLines", "CostCenterId", "Int", "CostCenters");

            AddFields("ProductTypeSettings", "IndexFilterParameter", "nvarchar(20)");

            AddFields("LedgerAccountGroups", "Weightage", "TINYINT");
            AddFields("Ledgers", "Priority", "Int");


            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'DiscountTypes'") == 0)
                {
                    mQry = @" CREATE TABLE Web.DiscountTypes
	                        (
	                        DiscountTypeId   INT IDENTITY NOT NULL,
	                        DocTypeId        INT NOT NULL,
	                        DiscountTypeName NVARCHAR (50) NOT NULL,
	                        Rate             DECIMAL (18, 4) NOT NULL,
	                        IsActive         BIT DEFAULT ((1)) NOT NULL,
	                        CreatedBy        NVARCHAR (max),
	                        ModifiedBy       NVARCHAR (max),
	                        CreatedDate      DATETIME NOT NULL,
	                        ModifiedDate     DATETIME NOT NULL,
	                        OMSId            NVARCHAR (50),
	                        CONSTRAINT [PK_Web.DiscountTypes] PRIMARY KEY (DiscountTypeId)
	                        CONSTRAINT [FK_Web.DiscountTypes_Web.DocumentTypes_DocTypeId] FOREIGN KEY (DocTypeId) REFERENCES Web.DocumentTypes (DocumentTypeId)
	                        )

                        CREATE UNIQUE INDEX IX_DiscountType_DiscountTypeName
	                        ON Web.DiscountTypes (DiscountTypeName)
	                        WITH (FILLFACTOR = 90)
                        ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }

            AddFields("ProductTypeSettings", "isPostedInStock", "BIT Default(1)");
            AddFields("PackingLines", "Sr", "Int");
            AddFields("SaleDispatchLines", "Sr", "Int");
            AddFields("SaleDispatchLines", "DiscountTypeId", "Int", "DiscountTypes");


            AddFields("ChargeGroupSettings", "ProcessId", "Int", "Processes");

            AddFields("ProductTypeSettings", "SqlProcProductCode", "nvarchar(100)");
            AddFields("PersonSettings", "SqlProcPersonCode", "nvarchar(100)");


            AddFields("Sites", "ReportHeaderTextLeft", "nvarchar(Max)");
            AddFields("Sites", "ReportHeaderTextRight", "nvarchar(Max)");

            AddFields("Ledgers", "ChqDate", "DATETIME");
            AddFields("Ledgers", "BankDate", "DATETIME");


            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'ImportMessages'") == 0)
                {
                    mQry = @"CREATE TABLE Web.ImportMessages
	                        (
	                            ImportMessageId INT IDENTITY NOT NULL,
	                            ImportKey       NVARCHAR (50),
	                            ImportHeaderId  INT NOT NULL,
                                SqlProcedure    NVARCHAR (100),
	                            RecordId        NVARCHAR (100),
	                            Head            NVARCHAR (max),
	                            Value           NVARCHAR (max),
	                            ValueType       NVARCHAR (max),
	                            Remark          NVARCHAR (max),
	                            IsActive        BIT NOT NULL,
	                            CreatedBy       NVARCHAR (max),
	                            CreatedDate     DATETIME NOT NULL,
	                            CONSTRAINT [PK_Web.ImportMessages] PRIMARY KEY (ImportMessageId)
	                        )";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }


            AddFields("JobOrderSettings", "SqlProcProductUidHelpList", "nvarchar(100)");
            AddFields("JobReceiveSettings", "SqlProcProductUidHelpList", "nvarchar(100)");
            AddFields("JobInvoiceSettings", "SqlProcProductUidHelpList", "nvarchar(100)");
            AddFields("StockHeaderSettings", "SqlProcProductUidHelpList", "nvarchar(100)");
            AddFields("SaleDispatchReturnLines", "GodownId", "Int","Godowns");
            AddFields("SaleInvoiceSettings", "SaleInvoiceReturnDocTypeId", "Int", "DocumentTypes");

            AddFields("LedgerSettings", "isVisibleLineDrCr", "Bit");

            AddFields("LedgerLines", "DrCr", "nvarchar(2)");

            AddFields("SaleDeliveryLines", "Sr", "int");
            AddFields("SaleDeliverySettings", "WizardMenuId", "int","Menus");


            AddFields("TrialBalanceSettings", "ShowContraAccount", "Bit");


            DropFields("JobInvoiceLines", "MfgDate");

            AddFields("Stocks", "MfgDate", "DateTime");
            AddFields("ProductUids", "MfgDate", "DateTime");



            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Tables WHERE TABLE_NAME = 'SalesTaxProductCodes'") == 0)
                {
                    mQry = @"CREATE TABLE Web.SalesTaxProductCodes
	                            (
	                            SalesTaxProductCodeId INT IDENTITY NOT NULL,
	                            Code                  NVARCHAR (50) NOT NULL,
	                            Description           NVARCHAR (max),
	                            IsActive              BIT NOT NULL,
	                            CreatedBy             NVARCHAR (max),
	                            ModifiedBy            NVARCHAR (max),
	                            CreatedDate           DATETIME NOT NULL,
	                            ModifiedDate          DATETIME NOT NULL,
	                            OMSId                 NVARCHAR (50),
	                            CONSTRAINT [PK_Web.SalesTaxProductCodes] PRIMARY KEY (SalesTaxProductCodeId)
	                            )

                            CREATE UNIQUE INDEX IX_SalesTaxProduct_SalesTaxProductCode
	                            ON Web.SalesTaxProductCodes (Code)
                            ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }




            AddFields("Products", "SalesTaxProductCodeId", "Int", "SalesTaxProductCodes");
            AddFields("ProductGroups", "DefaultSalesTaxProductCodeId", "Int", "SalesTaxProductCodes");
            AddFields("ProductTypeSettings", "isVisibleSalesTaxProductCode", "Bit");
            AddFields("ProductTypeSettings", "SalesTaxProductCodeCaption", "nvarchar(Max)");

            AddFields("SaleInvoiceSettings", "isVisibleShipToPartyAddress", "Bit");


            return RedirectToAction("Module", "Menu");
        }

        public void AddFields(string TableName, string FieldName, string DataType, string ForeignKeyTable = null)
        {
            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Columns WHERE COLUMN_NAME =  '" + FieldName + "' AND TABLE_NAME = '" + TableName + "'") == 0)
                {
                    mQry = "ALTER TABLE Web." + TableName + " ADD " + FieldName + " " + DataType;
                    ExecuteQuery(mQry);

                    if (ForeignKeyTable != "" && ForeignKeyTable != null)
                    {
                        string ForeignKeyTablePrimaryField = "";
                        mQry = " SELECT column_name " +
                                " FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC " +
                                " INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME " +
                                " and ku.table_name= '"+ ForeignKeyTable +"' " +
                                " ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION ";
                        ForeignKeyTablePrimaryField = (string)ExecuteScaler(mQry);

                        mQry = " ALTER TABLE Web." + TableName + "  ADD CONSTRAINT [FK_Web." + TableName + "_Web." + ForeignKeyTable + "_" + FieldName + "] FOREIGN KEY (" + FieldName + ") REFERENCES Web." + ForeignKeyTable + "(" + ForeignKeyTablePrimaryField + ")";
                        ExecuteQuery(mQry);
                    }
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }
        }


        public void DropFields(string TableName, string FieldName)
        {
            try
            {
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Columns WHERE COLUMN_NAME =  '" + FieldName + "' AND TABLE_NAME = '" + TableName + "'") != 0)
                {
                    mQry = "ALTER TABLE Web." + TableName + " DROP COLUMN " + FieldName + " ";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }
        }
       

        public void RecordError(Exception ex)
        {
            int CurrentSiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            int CurrentDivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];


            mQry = @"INSERT INTO Web.ActivityLogs (DocId, ActivityType, Narration, UserRemark, CreatedBy, CreatedDate, DocStatus, SiteId, DivisionId)
                    SELECT 0 AS DocId, 1 AS ActivityType, 'Update Table Structure' AS Narration, '" + ex.Message.Replace("'", "") + "' AS UserRemark, 'System' AS CreatedBy, getdate() AS CreatedDate, 0 As DocStatus, " + CurrentSiteId + " As SiteId, " + CurrentDivisionId + " As DivisionId ";
            ExecuteQuery(mQry);
        }

        public void ExecuteQuery(string Qry)
        {
            string ConnectionString = (string)System.Web.HttpContext.Current.Session["DefaultConnectionString"];

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(Qry);
                cmd.Connection = sqlConnection;
                cmd.ExecuteNonQuery();
            }
        }

        public object ExecuteScaler(string Qry)
        {
            object val = null;
            string ConnectionString = (string)System.Web.HttpContext.Current.Session["DefaultConnectionString"];

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(Qry);
                cmd.Connection = sqlConnection;
                val = cmd.ExecuteScalar();
            }

            return val;
        }
    }


}