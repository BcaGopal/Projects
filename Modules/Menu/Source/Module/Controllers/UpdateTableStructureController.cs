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
                            GO

                            CREATE UNIQUE INDEX IX_BinLocation_BinLocationName
	                            ON Web.BinLocations (BinLocationName)
	                            WITH (FILLFACTOR = 90)
                            GO";
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

                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Columns WHERE COLUMN_NAME =  'ParentLedgerAccountGroupId' AND TABLE_NAME = 'LedgerAccountGroups'") == 0)
                {
                    mQry = "ALTER TABLE Web.LedgerAccountGroups ADD ParentLedgerAccountGroupId INT";
                    ExecuteQuery(mQry);
                }

                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Columns WHERE COLUMN_NAME =  'BSNature' AND TABLE_NAME = 'LedgerAccountGroups'") == 0)
                {
                    mQry = "ALTER TABLE Web.LedgerAccountGroups ADD BSNature NVARCHAR (20)";
                    ExecuteQuery(mQry);
                }

                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Columns WHERE COLUMN_NAME =  'BSSr' AND TABLE_NAME = 'LedgerAccountGroups'") == 0)
                {
                    mQry = "ALTER TABLE Web.LedgerAccountGroups ADD BSSr INT";
                    ExecuteQuery(mQry);
                }

                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Columns WHERE COLUMN_NAME =  'TradingNature' AND TABLE_NAME = 'LedgerAccountGroups'") == 0)
                {
                    mQry = "ALTER TABLE Web.LedgerAccountGroups ADD TradingNature NVARCHAR (20)";
                    ExecuteQuery(mQry);
                }

                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Columns WHERE COLUMN_NAME =  'TradingSr' AND TABLE_NAME = 'LedgerAccountGroups'") == 0)
                {
                    mQry = "ALTER TABLE Web.LedgerAccountGroups ADD TradingSr INT";
                    ExecuteQuery(mQry);
                }

                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Columns WHERE COLUMN_NAME =  'PLNature' AND TABLE_NAME = 'LedgerAccountGroups'") == 0)
                {
                    mQry = "ALTER TABLE Web.LedgerAccountGroups ADD PLNature NVARCHAR (20)";
                    ExecuteQuery(mQry);
                }

                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Columns WHERE COLUMN_NAME =  'PLSr' AND TABLE_NAME = 'LedgerAccountGroups'") == 0)
                {
                    mQry = "ALTER TABLE Web.LedgerAccountGroups ADD PLSr INT";
                    ExecuteQuery(mQry);
                }

            }
            catch (Exception ex)
            {
                RecordError(ex);
            }



            AddFields("TdsRates", "LedgerAccountId", "INT", "LedgerAccounts");
            AddFields("ProductSiteDetails", "BinLocationId", "INT", "BinLocations");

            AddFields("Products", "ProductCategoryId", "INT", "ProductCategories");
            AddFields("Products", "SaleRate", " Decimal(18,4))");

            AddFields("ProductTypeSettings", "isVisibleProductDescription", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleProductSpecification", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleProductCategory", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleSalesTaxGroup", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleSaleRate", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleStandardCost", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleTags", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleMinimumOrderQty", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleReOrderLevel", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleGodownId", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleBinLocationId", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleProfitMargin", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleCarryingCost", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleLotManagement", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleConsumptionDetail", "BIT)");
            AddFields("ProductTypeSettings", "isVisibleProductProcessDetail", "BIT)");

            AddFields("ProductTypeSettings", "ProductNameCaption", "NVARCHAR(Max)");
            AddFields("ProductTypeSettings", "ProductCodeCaption", "NVARCHAR(Max)");
            AddFields("ProductTypeSettings", "ProductDescriptionCaption", "NVARCHAR(Max)");

            AddFields("ProductTypeSettings", "ProductSpecificationCaption", "NVARCHAR(Max)");
            AddFields("ProductTypeSettings", "ProductGroupCaption", "NVARCHAR(Max)");
            AddFields("ProductTypeSettings", "ProductCategoryCaption", "NVARCHAR(Max)");


            AddFields("ProductUids", "ProductUidSpecification", "NVARCHAR(Max)");



            AddFields("JobInvoiceHeaders", "FinancierId", "INT");

            AddFields("JobInvoiceLines", "RateDiscountPer", "Decimal(18,4)");
            AddFields("JobInvoiceLines", "MfgDate", "DATETIME");

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
       

        public void RecordError(Exception ex)
        {
            int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

            mQry = @"INSERT INTO Web.ActivityLogs (DocId, ActivityType, Narration, UserRemark, CreatedBy, CreatedDate, DocStatus, SiteId, DivisionId)
                    SELECT 0 AS DocId, 1 AS ActivityType, 'Update Table Structure' AS Narration, '" + ex.Message.Replace("'","") + "' AS UserRemark, 'System' AS CreatedBy, getdate() AS CreatedDate, 0 as DocStatus, " + SiteId + "," + DivisionId + " ";
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