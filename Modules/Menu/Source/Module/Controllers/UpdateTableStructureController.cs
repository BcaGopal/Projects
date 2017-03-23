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
                if ((int)ExecuteScaler("SELECT Count(*) AS Cnt FROM INFORMATION_SCHEMA.Columns WHERE COLUMN_NAME =  'LedgerAccountId' AND TABLE_NAME = 'TdsRates'") == 0)
                {
                    mQry = "ALTER TABLE Web.TdsRates ADD LedgerAccountId INT";
                    ExecuteQuery(mQry);

                    mQry = "ALTER TABLE Web.TdsRates ADD CONSTRAINT [FK_Web.TdsRates_Web.LedgerAccounts_LedgerAccountId] FOREIGN KEY (LedgerAccountId) REFERENCES Web.LedgerAccounts(LedgerAccountId)";
                    ExecuteQuery(mQry);
                }
            }
            catch (Exception ex)
            { }


            var Moduleslist = _ModuleService.GetModules();
            return View("Module", Moduleslist);
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