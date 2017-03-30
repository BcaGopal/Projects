using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Core.Common;
using Model.Models;
using Data.Models;
using Service;
using Data.Infrastructure;
using System.Configuration;
using Model.ViewModel;
using System.Data.SqlClient;
using System.Xml.Linq;
using Model.DatabaseViews;
using System.Data;
using Reports.Controllers;
/*
'For Year End (Task And Check List)
'Step 1 - Print the pending weaving order report (order list) and save it to an excel file.
'Step 2 - Print the Material On Loom of all the Job Workers and save it to an excel file.
'Step 3 - Check the Pending  Weaving Orders Of This Year And Cancel Them.
'Step 4 - Create New Weaving Orders Of Same Qty As Cancelled.
'Step 5 - Transfer the material from old job order to new job order which have postive values.
'Step 6 - Transfer Bar Codes From Old Order To New Order
'Step 7 - Check Pending Weaving Orders In 31st March It Should Be 0.
'Step 8 - Check Pending Weaving Orders In 1st Apr It Should Have the same qty as the pending order qty was before year end process.
'Step 9 - Check Material Balance On Loom it should be same as before year end process.
'Transfer Incentive in New Orders


'The Records Were not inserted in stock virtual in 2014 year end but it should be.
*/

namespace Web
{
    [Authorize]
    public class YearClosingController : ReportController
    {

        public SqlConnection Con;
        public SqlCommand Cmd;
        public SqlDataAdapter SDA;
        public SqlTransaction ETrans;


        public string mQry = "";
        public int SiteId = (int)(System.Web.HttpContext.Current.Session["SiteId"]);
        public int DivisionId = (int)(System.Web.HttpContext.Current.Session["DivisionId"]);

        string StartDate = "";
        string EndDate = "";
        string PurjaPrefix = "";

        private ApplicationDbContext db = new ApplicationDbContext();

        IJobOrderHeaderService _JobOrderHeaderService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        public YearClosingController(IJobOrderHeaderService JobOrderHeaderService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _JobOrderHeaderService = JobOrderHeaderService;
            _exception = exec;
            _unitOfWork = unitOfWork;
        }


        private Boolean FPost(DateTime CloseDate)
        {

            Boolean IsSuccess = false;
            string mTrans = "";
            DataTable DtJobOrder = new DataTable();
            

            int I = 0, J = 0;

            //For Job Order Cancel

            int JobOrderCancelDocTypeId, JobOrderCancelReasonId, JobOrderCancelHeaderId;
            string JobOrderCancelDocDate = "", JobOrderCancelDocNo = "";


            // For Job Order
            string JobOrderDocNo = "";
            int CostCenterId, JobOrderHeaderId;
            string NewCostcenterNo = "";

            //Con = new SqlConnection(connectionString);
            //Con.Open();
            //SqlCommand Cmd = new SqlCommand();
            //Cmd = Con.CreateCommand();
            //SqlDataAdapter SDA;
            //SqlTransaction ETrans = Con.BeginTransaction(IsolationLevel.ReadCommitted);
            //Cmd.Transaction = ETrans;

            int WVORD = 458, WFORD = 455;
            int WVREC = 448;
            int WVCNL = 449;

            try
            {



                mTrans = "Begin";


                FRollBack(ETrans);

                // *****************  For Getting Pending Weaving Orders

                mQry = "SELECT JOH.JobOrderHeaderId, Max(convert(INT,JW.IsSisterConcern)) AS IsSisterConcern,  Max(C.CostcenterName) AS CostcenterName, Max(CE.ProductId) AS ProductId, Max(LA.LedgerAccountId) AS LedgerAccountId, Max(JOH.Remark) AS Remark, Max(JOH.UnitconversionForId) AS UnitconversionForId, Max(JOH.ActualDueDate) AS ActualDueDate, Max(JOH.DocTypeId) AS DocTypeId, Max(JOH.DocDate) AS DocDate, Max(JOH.DocNo) AS DocNo, Max(JOH.DivisionId) AS DivisionId, Max(JOH.SiteId) AS SiteId, Max(JOH.DueDate) AS DueDate, " +
                    " Max(JOH.JobWorkerId) AS JobWorkerId , Max(JOH.BillToPartyId) AS BillToPartyId, Max(JOH.OrderById) AS OrderById, Max(isnull(JOH.GodownId,0)) AS GodownId, Max(JOH.ProcessId) AS ProcessId, Max(JOH.CostCenterId) AS CostCenterId ,Max(JOH.TermsAndConditions) AS   TermsAndConditions " +
                    " FROM " +
                    " ( " +
                    " SELECT H.JobOrderHeaderId AS JobOrderHeaderId, L.JobOrderLineId,  " +
                    " L.Qty -  isnull(VRec.RecQty,0) - isnull(VCan.CanQty,0) AS BalQty   " +
                    " FROM " +
                    " ( " +
                    "       SELECT H.JobOrderHeaderId FROM Web.JobOrderHeaders H WITH (Nolock) WHERE H.DocTypeId IN ( " + WVORD + "," + WFORD + " )  " +
                    "       AND H.DocDate <=' " + CloseDate + "' AND H.SiteId = " + SiteId + " AND H.DivisionId =" + DivisionId + " " +
                    " ) H " +
                    " LEFT JOIN web.JobOrderLines L WITH (Nolock) ON L.JobOrderHeaderId = H.JobOrderHeaderId  " +
                    " LEFT JOIN  " +
                    " ( " +
                    "      SELECT RL.JobOrderLineId, sum(RL.Qty - isnull(RT.Qty,0)) AS RecQty   " +
                    "      FROM  " +
                    "      (  " +
                    "      SELECT JobReceiveHeaderId FROM Web.JobReceiveHeaders RH WITH (Nolock) WHERE RH.DocTypeId = " + WVREC + "  " +
                    "      AND RH.DocDate <=' " + CloseDate + "' AND RH.SiteId = " + SiteId + " AND RH.DivisionId =" + DivisionId + " " +
                    "      ) RH " +
                    "      LEFT JOIN web.JobReceiveLines RL WITH (Nolock) ON RL.JobReceiveHeaderId = RH.JobReceiveHeaderId  " +
                    "      LEFT JOIN web.JobReturnLines RT WITH (Nolock) ON RT.JobReceiveLineId = RL.JobReceiveLineId  " +
                    "      GROUP BY RL.JobOrderLineId  " +
                    " ) VRec ON VRec.JobOrderLineId = L.JobOrderLineId " +
                    " LEFT JOIN  " +
                    " ( " +
                    "     SELECT CL.JobOrderLineId, sum(CL.Qty) AS CanQty  " +
                    "     FROM Web.JobOrderCancelLines CL WITH (Nolock) GROUP BY CL.JobOrderLineId  " +
                    " ) VCan ON VCan.JobOrderLineId = L.JobOrderLineId " +
                    " WHERE L.Qty - isnull(VRec.RecQty,0) - isnull(VCan.CanQty,0) > 0 " +
                    " ) VMain " +
                    " LEFT JOIN WEb.JobOrderHeaders JOH WITH (Nolock) ON JOH.JobOrderHeaderId = VMain.JobOrderHeaderId " +
                    " LEFT JOIN web.CostCenters C WITH (Nolock) ON C.CostCenterId = JOH.CostCenterId " +
                    " LEFT JOIN web.CostCenterStatusExtendeds CE WITH (Nolock) ON CE.CostCenterId = JOH.CostCenterId " +
                    " LEFT JOIN web.LedgerAccounts LA WITH (Nolock) ON LA.PersonId = JOH.JobWorkerId " +
                    " LEFT JOIN web.People JW WITH (Nolock) ON JW.PersonID = JOH.JobWorkerId " +
                    " Where IsNull(JW.IsSisterConcern,0) = 0 " +
                    " GROUP BY JOH.JobOrderHeaderId " +
                    " Order by (Case When IsNumeric(Max(JOH.DocNo)) > 0 Then Convert(Numeric,Max(JOH.DocNo)) Else 0 End) ";


                SDA = new SqlDataAdapter(mQry, Con);
                SDA.SelectCommand.Transaction = ETrans;
                SDA.SelectCommand.CommandTimeout = 500;
                SDA.Fill(DtJobOrder);





                if (DtJobOrder.Rows.Count > 0)
                {

                    for (I = 0; I < DtJobOrder.Rows.Count; I++)
                    {

                        // *****************  For Insert in Web.JobOrderCancelHeaders

                        JobOrderCancelDocTypeId = WVCNL;
                        JobOrderCancelReasonId = 14;
                        JobOrderCancelDocDate = CloseDate.ToString();
                        JobOrderCancelDocNo = new DocumentTypeService(_unitOfWork).FGetNewDocNo("DocNo", "WEb.JobOrderCancelHeaders", JobOrderCancelDocTypeId, CloseDate, DivisionId, SiteId);


                        mQry = "INSERT INTO Web.JobOrderCancelHeaders (DocTypeId, DocDate, ReasonId, DocNo, DivisionId, SiteId, GodownId, Status, Remark, " +
                              " CreatedBy, ModifiedBy, CreatedDate, ModifiedDate, JobWorkerId, ProcessId, OrderById) " +
                              " VALUES ( " + JobOrderCancelDocTypeId + ", '" + JobOrderCancelDocDate + "' ," + JobOrderCancelReasonId + ", '" + JobOrderCancelDocNo + "', " +
                              " " + DtJobOrder.Rows[I]["DivisionId"].ToString() + ", " + DtJobOrder.Rows[I]["SiteId"].ToString() + ", " + FGetNullValue(DtJobOrder.Rows[I]["GodownId"].ToString()) + ", " +
                              " 1, Null,  'System', 'System', getdate(),getdate(), " + DtJobOrder.Rows[I]["JobWorkerId"].ToString() + " , " + DtJobOrder.Rows[I]["ProcessId"].ToString() + " , " + DtJobOrder.Rows[I]["OrderById"].ToString() + "  ) ";

                        Cmd = new SqlCommand(mQry, Con);
                        if (Cmd.Connection.State != ConnectionState.Open)
                            Cmd.Connection.Open();
                        Cmd.Transaction = ETrans;
                        Cmd.ExecuteNonQuery();


                        mQry = "SELECT JobOrderCancelHeaderId FROM web.JobOrderCancelHeaders C WHERE C.DocTypeId =" + JobOrderCancelDocTypeId + " AND C.SiteId = " + SiteId + " AND C.DivisionId =" + DivisionId + "  AND C.DocNo = '" + JobOrderCancelDocNo + "' ";
                        Cmd = new SqlCommand(mQry, Con);
                        if (Cmd.Connection.State != ConnectionState.Open)
                            Cmd.Connection.Open();
                        Cmd.Transaction = ETrans;
                        JobOrderCancelHeaderId = (int)Cmd.ExecuteScalar();








                        // *****************  For Insert in Web.CostCenters
                        int IsCostcenterExist = 0;
                        NewCostcenterNo = FGetNewCostcenterNo(DtJobOrder, I, ETrans);

                        mQry = "SELECT count(*) FROM web.CostCenters C WHERE C.ReferenceDocTypeId =" + DtJobOrder.Rows[I]["DocTypeId"].ToString() + " AND C.SiteId = " + SiteId + " AND C.DivisionId =" + DivisionId + " AND C.CostCenterName = '" + NewCostcenterNo + "' ";
                        SqlCommand Cmd2 = new SqlCommand(mQry, Con);
                        if (Cmd2.Connection.State != ConnectionState.Open)
                            Cmd2.Connection.Open();
                        Cmd2.Transaction = ETrans;
                        IsCostcenterExist = (int)Cmd2.ExecuteScalar();

                        if (IsCostcenterExist == 0)
                        {
                            mQry = "INSERT INTO Web.CostCenters (CostCenterName, IsActive, IsSystemDefine, CreatedBy, ModifiedBy, CreatedDate, ModifiedDate, " +
                                    " DivisionId, SiteId, LedgerAccountId, DocTypeId, StartDate, ReferenceDocTypeId, ProcessId, Status) " +
                                    " VALUES ('" + NewCostcenterNo + "', 1,1, 'System', 'System', getdate(),getdate(), " +
                                    " " + DivisionId + ", " + SiteId + ", " + DtJobOrder.Rows[I]["LedgerAccountId"].ToString() + ", " + DtJobOrder.Rows[I]["DocTypeId"].ToString() + ", dateadd(Day,1, '" + CloseDate + "'), " + DtJobOrder.Rows[I]["DocTypeId"].ToString() + ", " + DtJobOrder.Rows[I]["ProcessId"].ToString() + ", 1) ";

                            Cmd = new SqlCommand(mQry, Con);
                            if (Cmd.Connection.State != ConnectionState.Open)
                                Cmd.Connection.Open();
                            Cmd.Transaction = ETrans;
                            Cmd.ExecuteNonQuery();
                        }





                        // *****************  For Insert in Web.JobOrderHeaders
                        mQry = "SELECT CostcenterId FROM web.CostCenters C WHERE C.ReferenceDocTypeId =" + DtJobOrder.Rows[I]["DocTypeId"].ToString() + " AND C.SiteId = " + SiteId + " AND C.DivisionId =" + DivisionId + "  AND C.CostCenterName = '" + NewCostcenterNo + "' ";
                        Cmd = new SqlCommand(mQry, Con);
                        if (Cmd.Connection.State != ConnectionState.Open)
                            Cmd.Connection.Open();
                        Cmd.Transaction = ETrans;
                        CostCenterId = (int)Cmd.ExecuteScalar();
                        JobOrderDocNo = "OL" + DtJobOrder.Rows[I]["DocNo"].ToString();


                        mQry = "INSERT INTO Web.JobOrderHeaders (DocTypeId, DocDate, DocNo, DivisionId, SiteId, DueDate, JobWorkerId, BillToPartyId, OrderById, " +
                                " ActualDocDate, GodownId, ProcessId, CostCenterId, TermsAndConditions, Status, Remark, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, UnitconversionForId, ActualDueDate) " +
                                " VALUES ( " + DtJobOrder.Rows[I]["DocTypeId"].ToString() + ", dateadd(Day,1, '" + CloseDate + "'), '" + JobOrderDocNo + "', " + DivisionId + ", " + SiteId + ", " +
                                " '" + DtJobOrder.Rows[I]["DueDate"].ToString() + "', " + DtJobOrder.Rows[I]["JobWorkerId"].ToString() + ", " + DtJobOrder.Rows[I]["BillToPartyId"].ToString() + ", " + DtJobOrder.Rows[I]["OrderById"].ToString() + ", " +
                                " dateadd(Day,1, '" + CloseDate + "'), " + FGetNullValue(DtJobOrder.Rows[I]["GodownId"].ToString()) + ", " + DtJobOrder.Rows[I]["ProcessId"].ToString() + ", " + CostCenterId + ", ' " + DtJobOrder.Rows[I]["TermsAndConditions"].ToString() + "', 1, ' " + DtJobOrder.Rows[I]["Remark"].ToString() + "', " +
                                "  'System', getdate(),  'System', getdate(), " + DtJobOrder.Rows[I]["UnitconversionForId"].ToString() + ", '" + DtJobOrder.Rows[I]["ActualDueDate"].ToString() + "')";
                        Cmd = new SqlCommand(mQry, Con);
                        if (Cmd.Connection.State != ConnectionState.Open)
                            Cmd.Connection.Open();
                        Cmd.Transaction = ETrans;
                        Cmd.ExecuteNonQuery();


                        mQry = "SELECT JobOrderHeaderId FROM web.JobOrderHeaders C WHERE C.DocTypeId =" + DtJobOrder.Rows[I]["DocTypeId"].ToString() + " AND C.SiteId = " + SiteId + " AND C.DivisionId =" + DivisionId + "  AND C.DocNo = '" + JobOrderDocNo + "' ";
                        Cmd = new SqlCommand(mQry, Con);
                        if (Cmd.Connection.State != ConnectionState.Open)
                            Cmd.Connection.Open();
                        Cmd.Transaction = ETrans;
                        JobOrderHeaderId = (int)Cmd.ExecuteScalar();



                        mQry = " INSERT INTO Web.JobOrderJobOrders (JobOrderHeaderId, GenJobOrderHeaderId) " +
                                " Select " + JobOrderHeaderId + ", " + DtJobOrder.Rows[I]["JobOrderHeaderId"] + "";
                        Cmd = new SqlCommand(mQry, Con);
                        if (Cmd.Connection.State != ConnectionState.Open)
                            Cmd.Connection.Open();
                        Cmd.Transaction = ETrans;
                        Cmd.ExecuteNonQuery();


                        // *****************  For Getting Pending Job OrderLine
                        mQry = " SELECT H.JobOrderHeaderId, H.DocTypeId, H.DocNo, H.DocDate, H.JobWorkerId, H.ProcessId, " +
                                " L.JobOrderLineId, isnull(L.ProductUidId,0) AS ProductUidId, L.ProductId, isnull(L.ProdOrderLineId,0) ProdOrderLineId, L.DealUnitId, L.UnitId, L.Rate, L.Amount, L.Remark , " +
                                " L.NonCountedQty, L.LossQty, L.Specification, L.UnitConversionMultiplier, L.StockId AS StockId, L.ProductUidHeaderId AS ProductUidHeaderId, L.Sr, " +
                                " L.Qty -  isnull(VRec.RecQty,0) - isnull(VCan.CanQty,0) AS BalQty ,  (L.Qty -  isnull(VRec.RecQty,0) - isnull(VCan.CanQty,0)) * L.UnitConversionMultiplier AS BalDealQty   " +
                                " FROM " +
                                " ( SELECT H.* FROM Web.JobOrderHeaders H WITH (Nolock) WHERE H.JobOrderHeaderId = " + DtJobOrder.Rows[I]["JobOrderHeaderId"].ToString() + "  ) H " +
                                " LEFT JOIN web.JobOrderLines L WITH (Nolock) ON L.JobOrderHeaderId = H.JobOrderHeaderId  " +
                                " LEFT JOIN  " +
                                " ( " +
                                "      SELECT RL.JobOrderLineId, sum(RL.Qty - isnull(RT.Qty,0)) AS RecQty   " +
                                "      FROM  " +
                                "      (  " +
                                "      SELECT JobReceiveHeaderId FROM Web.JobReceiveHeaders RH WITH (Nolock) WHERE RH.DocTypeId = " + WVREC + "  " +
                                "      AND RH.DocDate <=' " + CloseDate + "' AND RH.SiteId = " + SiteId + " AND RH.DivisionId =" + DivisionId + " " +
                                "      ) RH " +
                                "      LEFT JOIN web.JobReceiveLines RL WITH (Nolock) ON RL.JobReceiveHeaderId = RH.JobReceiveHeaderId  " +
                                "      LEFT JOIN web.JobReturnLines RT WITH (Nolock) ON RT.JobReceiveLineId = RL.JobReceiveLineId  " +
                                "      LEFT JOIN web.JobOrderLines JOL WITH (Nolock) ON JOL.JobOrderLineId = RL.JobOrderLineId  " +
                                "      WHERE JOL.JobOrderHeaderId = " + DtJobOrder.Rows[I]["JobOrderHeaderId"].ToString() + "  " +
                                "      GROUP BY RL.JobOrderLineId  " +
                                " ) VRec ON VRec.JobOrderLineId = L.JobOrderLineId " +
                                " LEFT JOIN  " +
                                " ( " +
                                "     SELECT CL.JobOrderLineId, sum(CL.Qty) AS CanQty  " +
                                "     FROM Web.JobOrderCancelLines CL WITH (Nolock) " +
                                "     LEFT JOIN web.JobOrderLines JOL WITH (Nolock) ON JOL.JobOrderLineId = CL.JobOrderLineId  " +
                                "     WHERE JOL.JobOrderHeaderId = " + DtJobOrder.Rows[I]["JobOrderHeaderId"].ToString() + "  " +
                                "     GROUP BY CL.JobOrderLineId  " +
                                " ) VCan ON VCan.JobOrderLineId = L.JobOrderLineId " +
                                " WHERE L.Qty - isnull(VRec.RecQty,0) - isnull(VCan.CanQty,0) > 0 ";

                        DataTable DtJobOrderLine = new DataTable();
                        SDA = new SqlDataAdapter(mQry, Con);
                        SDA.SelectCommand.CommandTimeout = 500;
                        SDA.SelectCommand.Transaction = ETrans;
                        SDA.Fill(DtJobOrderLine);


                        if (DtJobOrderLine.Rows.Count > 0)
                        {

                            for (J = 0; J < DtJobOrderLine.Rows.Count; J++)
                            {
                                // *****************  For Insert in Web.JobOrderCancelLines
                                mQry = "INSERT INTO Web.JobOrderCancelLines (JobOrderCancelHeaderId, JobOrderLineId, Qty, CreatedBy, ModifiedBy, CreatedDate, ModifiedDate, " +
                                        " ProductUidId, Sr, ProductUidLastTransactionDocId, ProductUidLastTransactionDocNo, ProductUidLastTransactionDocTypeId, ProductUidLastTransactionDocDate, ProductUidLastTransactionPersonId, ProductUidCurrentProcessId, ProductUidStatus) " +
                                        " VALUES (" + JobOrderCancelHeaderId + ", " + DtJobOrderLine.Rows[J]["JobOrderLineId"].ToString() + ", " + DtJobOrderLine.Rows[J]["BalQty"].ToString() + ", 'System', 'System', getdate(),getdate(), " +
                                        "  " + FGetNullValue(DtJobOrderLine.Rows[J]["ProductUidId"].ToString()) + ", 1, " + DtJobOrderLine.Rows[J]["JobOrderHeaderId"].ToString() + ", '" + DtJobOrderLine.Rows[J]["DocNo"].ToString() + "', " + DtJobOrderLine.Rows[J]["DocTypeId"].ToString() + ", '" + DtJobOrderLine.Rows[J]["DocDate"].ToString() + "', '" + DtJobOrderLine.Rows[J]["JobWorkerId"].ToString() + "', '" + DtJobOrderLine.Rows[J]["ProcessId"].ToString() + "', 'Issue') ";
                                Cmd = new SqlCommand(mQry, Con);
                                if (Cmd.Connection.State != ConnectionState.Open)
                                    Cmd.Connection.Open();
                                Cmd.Transaction = ETrans;
                                Cmd.ExecuteNonQuery();

                                

                                // *****************  For Insert in Web.JobOrderLines
                                mQry = "INSERT INTO Web.JobOrderLines (JobOrderHeaderId, ProductUidId, ProductId, ProdOrderLineId, Qty, DealUnitId, DealQty, UnitId, Rate, Amount, Remark, " +
                                        " CreatedBy, ModifiedBy, CreatedDate, ModifiedDate, NonCountedQty, LossQty, Specification, UnitConversionMultiplier, StockId, ProductUidHeaderId, Sr,  " +
                                        " ProductUidLastTransactionDocId, ProductUidLastTransactionDocNo, ProductUidLastTransactionDocTypeId, ProductUidLastTransactionDocDate, ProductUidLastTransactionPersonId, ProductUidCurrentGodownId, ProductUidCurrentProcessId, ProductUidStatus) " +
                                        " VALUES (" + JobOrderHeaderId + ", " + FGetNullValue(DtJobOrderLine.Rows[J]["ProductUidId"].ToString()) + ", " + DtJobOrderLine.Rows[J]["ProductId"].ToString() + ", " + FGetNullValue(DtJobOrderLine.Rows[J]["ProdOrderLineId"].ToString()) + ", " + DtJobOrderLine.Rows[J]["BalQty"].ToString() + ", " +
                                        " '" + DtJobOrderLine.Rows[J]["DealUnitId"].ToString() + "', " + DtJobOrderLine.Rows[J]["BalDealQty"].ToString() + ", '" + DtJobOrderLine.Rows[J]["UnitId"].ToString() + "', " + DtJobOrderLine.Rows[J]["Rate"].ToString() + ", " + DtJobOrderLine.Rows[J]["Amount"].ToString() + ", '" + DtJobOrderLine.Rows[J]["Remark"].ToString() + "', " +
                                        " 'System', 'System', getdate(),getdate(), " + DtJobOrderLine.Rows[J]["NonCountedQty"].ToString() + ", " + DtJobOrderLine.Rows[J]["LossQty"].ToString() + ", '" + DtJobOrderLine.Rows[J]["Specification"].ToString() + "', " + DtJobOrderLine.Rows[J]["UnitConversionMultiplier"].ToString() + ", " +
                                        " NUll, Null, " + DtJobOrderLine.Rows[J]["Sr"].ToString() + ", NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL) ";
                                Cmd = new SqlCommand(mQry, Con);
                                if (Cmd.Connection.State != ConnectionState.Open)
                                    Cmd.Connection.Open();
                                Cmd.Transaction = ETrans;
                                Cmd.ExecuteNonQuery();

                                FTransferProductUids((int)DtJobOrderLine.Rows[J]["JobOrderLineId"], ETrans);
                            }

                            Cmd = new SqlCommand("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".sp_PostBomForWeavingOrderCancel");
                            Cmd.Connection = Con;
                            Cmd.CommandType = CommandType.StoredProcedure;
                            if (Cmd.Connection.State != ConnectionState.Open)
                                Cmd.Connection.Open();
                            Cmd.Parameters.AddWithValue("@JobOrderCancelHeaderId", JobOrderCancelHeaderId);
                            Cmd.Transaction = ETrans;
                            Cmd.ExecuteNonQuery();


                            if (SiteId == 17)
                            {
                                PostJobOrderConsumptionMainSite(JobOrderHeaderId, ETrans);
                            }
                            else
                            {
                                Cmd = new SqlCommand("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".sp_PostBomForWeavingOrder");
                                Cmd.Connection = Con;
                                Cmd.CommandType = CommandType.StoredProcedure;
                                if (Cmd.Connection.State != ConnectionState.Open)
                                    Cmd.Connection.Open();
                                Cmd.Parameters.AddWithValue("@JobOrderHeaderId", JobOrderHeaderId);
                                Cmd.Transaction = ETrans;
                                Cmd.ExecuteNonQuery();
                            }


                            Cmd = new SqlCommand("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".sp_ReGenerateRequisition");
                            Cmd.Connection = Con;
                            Cmd.CommandType = CommandType.StoredProcedure;
                            if (Cmd.Connection.State != ConnectionState.Open)
                                Cmd.Connection.Open();
                            Cmd.Parameters.AddWithValue("@OldJobOrderHeaderId", (int)DtJobOrder.Rows[I]["JobOrderHeaderId"]);
                            Cmd.Parameters.AddWithValue("@NewJobOrderHeaderId", JobOrderHeaderId);
                            Cmd.Parameters.AddWithValue("@JobOrderCancelHeaderId", JobOrderCancelHeaderId);
                            Cmd.Transaction = ETrans;
                            Cmd.ExecuteNonQuery();



                            if (SiteId == 17)
                            {
                                FTransferMaterial(JobOrderHeaderId, (int)DtJobOrder.Rows[I]["JobOrderHeaderId"], DivisionId, SiteId, CloseDate, ETrans);
                            }
                        }
                    }
                }

                mTrans = "Commit";
                ETrans.Commit();

                IsSuccess = true;
            }

            catch (Exception ex)
            {
                if (mTrans == "Begin")
                {
                    ETrans.Rollback();
                }
                //MsgBox(ex.Message);
            }

            return IsSuccess;
        }

        public ActionResult YearClosing()
        {
            return View("YearClosing");
        }



        public ActionResult PostYearClosing(DateTime CloseDate)
        {
            Boolean IsSuccess = false;

            if (CloseDate.Month >= 1 && CloseDate.Month <= 3)
            {
                StartDate = "01/Apr/" + (CloseDate.Year - 1).ToString();
                EndDate = "31/Mar/" + CloseDate.Year.ToString();
                PurjaPrefix = CloseDate.Year.ToString().Substring(CloseDate.Year.ToString().Length - 2);
            }
            else
            {
                StartDate = "01/Apr/" + (CloseDate.Year).ToString();
                EndDate = "31/Mar/" + (CloseDate.Year + 1).ToString();
                PurjaPrefix = (CloseDate.Year + 1).ToString().Substring((CloseDate.Year + 1).ToString().Length - 2);
            }

            InitializeConnection();


            CloseDate = Convert.ToDateTime("31/Mar/2017");
            if (CloseDate != null)
            {
                IsSuccess = FPost(CloseDate);
            }

            if (IsSuccess)
                return View("Sucess");
            else
                return View("Error");
        }


        private string FGetNullValue(String Value)
        {
            string strFGetNullValue = "";
            if (Value == "0")
                strFGetNullValue = "null";
            else
                strFGetNullValue = Value;
            return strFGetNullValue;
        }


        private string FGetNewCostcenterNo(DataTable DtJobOrder, int Row, SqlTransaction ETrans)
        {
            string NewPurja = "";

            //Con = new SqlConnection(connectionString);
            //SqlCommand Cmd;
            string bConstruction = "";

            object ObjVar = "";
            long bStartFrom = 1;
            long MaxPurjaNo = 0;
            string RunningPurja = "";
            string strCondConstruction = "";


            mQry = "SELECT C.CostCenterName " +
                " FROM web.JobOrderJobOrders JJ WITH (Nolock) " +
                " LEFT JOIN web.JobOrderHeaders JO WITH (Nolock) ON JO.JobOrderHeaderId = JJ.JobOrderHeaderId  " +
                " LEFT JOIN web.JobOrderHeaders GJO WITH (Nolock) ON GJO.JobOrderHeaderId = JJ.GenJobOrderHeaderId   " +
                " LEFT JOIN web.CostCenters C ON C.CostCenterId = JO.JobOrderHeaderId  " +
                " WHERE JO.SiteId =" + DtJobOrder.Rows[Row]["SiteId"].ToString() + " AND JO.DivisionId =" + DtJobOrder.Rows[Row]["DivisionId"].ToString() + " " +
                " AND JO.DocDate  BETWEEN DateAdd(Year,1,'" + StartDate + "') AND  DateAdd(Year,1,'" + EndDate + "') " +
                " AND C.CostCenterName = '" + DtJobOrder.Rows[Row]["CostCenterName"].ToString() + "' ";

            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            ObjVar = Cmd.ExecuteScalar();

            if (ObjVar != null)
                RunningPurja = (string)ObjVar;
            else
                RunningPurja = "";


            if (RunningPurja == "")
            {

                mQry = "SELECT PC.ProductCategoryName FROM web.FinishedProduct FP WITH (Nolock) " +
                        " LEFT JOIN web.ProductCategories PC WITH (Nolock) ON PC.ProductCategoryId = FP.ProductCategoryId " +
                        " WHERE FP.ProductId = " + DtJobOrder.Rows[Row]["ProductId"].ToString() + " ";

                Cmd = new SqlCommand(mQry, Con);
                if (Cmd.Connection.State != ConnectionState.Open)
                    Cmd.Connection.Open();
                Cmd.Transaction = ETrans;
                ObjVar = Cmd.ExecuteScalar();

                if (ObjVar != null)
                    bConstruction = (string)ObjVar;
                else
                    bConstruction = "";



                if (DtJobOrder.Rows[Row]["DivisionId"].ToString() == "6" && DtJobOrder.Rows[Row]["SiteId"].ToString() == "17")
                {
                    if (DtJobOrder.Rows[Row]["IsSisterConcern"].ToString() == "1")
                    {
                        bStartFrom = 5501;
                        strCondConstruction = " AND IsNull(JW.IsSisterConcern,0) <> 0 ";
                    }
                    else
                    {
                        if (bConstruction == "TUFTED")
                        {
                            bStartFrom = 2001;
                            strCondConstruction = " AND PC.ProductCategoryName =  '" + bConstruction + "' AND IsNull(JW.IsSisterConcern,0) = 0 ";
                        }
                        else if (bConstruction == "HANDLOOMS" && DtJobOrder.Rows[Row]["DivisionId"].ToString() == "6" && DtJobOrder.Rows[Row]["SiteId"].ToString() == "17")
                        {
                            bStartFrom = 501;
                            strCondConstruction = " AND PC.ProductCategoryName =  '" + bConstruction + "'  AND IsNull(JW.IsSisterConcern,0) = 0 ";
                        }
                        else
                        {
                            strCondConstruction = " AND PC.ProductCategoryName Not In ('TUFTED','HANDLOOMS')  AND IsNull(JW.IsSisterConcern,0) = 0 ";
                            bStartFrom = 1;
                        }

                    }

                }
                else if (DtJobOrder.Rows[Row]["DivisionId"].ToString() == "1" && DtJobOrder.Rows[Row]["SiteId"].ToString() == "17")
                {

                    if (DtJobOrder.Rows[Row]["IsSisterConcern"].ToString() == "1")
                    {
                        bStartFrom = 8001;
                        strCondConstruction = " AND IsNull(JW.IsSisterConcern,0) <> 0 ";
                    }
                    else
                    {
                        if (bConstruction == "KELIM")
                        {
                            bStartFrom = 1;
                            strCondConstruction = " AND PC.ProductCategoryName =  '" + bConstruction + "' AND IsNull(JW.IsSisterConcern,0) = 0 ";
                        }
                        else
                        {
                            strCondConstruction = " AND PC.ProductCategoryName Not In ('TUFTED','HANDLOOMS')  AND IsNull(JW.IsSisterConcern,0) = 0 ";
                            bStartFrom = 5001;
                        }

                    }
                }
                else
                {
                    bStartFrom = 1;
                }


                string Prefix = "OL" + PurjaPrefix + "-";
                mQry = "SELECT IsNull(Max(Convert(BIGINT,replace(C.CostCenterName,'" + Prefix + "',''))),0) " +
                         " FROM   " +
                         " (  " +
                         "      SELECT H.*  " +
                         "      FROM web.JobOrderHeaders H WITH (Nolock)   " +
                         "      LEFT JOIN web.DocumentTypes DT ON DT.DocumentTypeId = H.DocTypeId  " +
                         "      WHERE DT.DocumentCategoryId  =343 AND  H.SiteId =" + DtJobOrder.Rows[Row]["SiteId"].ToString() + " AND H.DivisionId =" + DtJobOrder.Rows[Row]["DivisionId"].ToString() + " " +
                         "      AND H.DocDate  BETWEEN DateAdd(Year,1,'" + StartDate + "') AND  DateAdd(Year,1,'" + EndDate + "') " +
                         " ) H " +
                         " LEFT JOIN web.CostCenters C WITH (Nolock) ON C.CostCenterId = H.CostCenterId  " +
                         " LEFT JOIN web.People JW WITH (Nolock) ON JW.PersonID = H.JobWorkerId  " +
                         " LEFT JOIN web.JobOrderLines L WITH (Nolock) ON L.JobOrderHeaderId = H.JobOrderHeaderId  " +
                         " LEFT JOIN web.FinishedProduct  FP WITH (Nolock) ON FP.ProductId = L.ProductId  " +
                         " LEFT JOIN web.ProductCategories PC WITH (Nolock) ON PC.ProductCategoryId = FP.ProductCategoryId " +
                         " WHERE 1=1 AND  IsNumeric(replace(replace(C.CostCenterName,'-',''),'OL','')) = 1 " + strCondConstruction;

                Cmd = new SqlCommand(mQry, Con);
                if (Cmd.Connection.State != ConnectionState.Open)
                    Cmd.Connection.Open();
                Cmd.Transaction = ETrans;
                ObjVar = Cmd.ExecuteScalar();

                if (ObjVar != null)
                    MaxPurjaNo = (long)ObjVar;
                else
                    MaxPurjaNo = 0;



                if (MaxPurjaNo == 0)
                    NewPurja = "OL" + PurjaPrefix + "-" + bStartFrom.ToString().PadLeft(4, '0').ToString();
                else
                    NewPurja = "OL" + PurjaPrefix + "-" + (MaxPurjaNo + 1).ToString().PadLeft(4, '0').ToString();

            }
            else
                NewPurja = RunningPurja;

            return NewPurja;
        }

        public void FTransferMaterial(int NewJobOrderId, int OldJobOrderId, int DivisionId, int SiteId, DateTime CloseDate, SqlTransaction ETrans)
        {
            int PurjaTransferDocTypeId = 288;
            DateTime PurjaTransferDocDate;
            string PurjaTransferDocNo = "";
            int NewCostCenterId;
            int OldCostCenterId;
            object ObjVar = "";
            SqlDataAdapter StockPeocessDA;
            DataTable DtStockProcess = new DataTable();
            SqlDataAdapter StockOrderHeaderDA;
            DataTable DtStockHeader = new DataTable();

            mQry = " Select CostCenterId From Web.JobOrderHeaders H With (NoLock) Where H.JobOrderHeaderId = " + NewJobOrderId.ToString() + "";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            NewCostCenterId = (int)Cmd.ExecuteScalar();


            mQry = " Select CostCenterId From Web.JobOrderHeaders H With (NoLock) Where H.JobOrderHeaderId = " + OldJobOrderId.ToString() + "";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            OldCostCenterId = (int)Cmd.ExecuteScalar();

            mQry = " SELECT IsNull(Sum(Qty_Rec),0) - IsNull(Sum(Qty_Iss),0) FROM Web.StockProcesses L With (NoLock) WHERE L.CostCenterId = " + OldCostCenterId.ToString() + " HAVING IsNull(Sum(L.Qty_Rec),0) - IsNull(Sum(L.Qty_Iss),0) > 0 ";
            StockPeocessDA = new SqlDataAdapter(mQry, Con);
            StockPeocessDA.SelectCommand.CommandTimeout = 500;
            StockPeocessDA.SelectCommand.Transaction = ETrans;
            StockPeocessDA.Fill(DtStockProcess);



            if (DtStockProcess.Rows.Count > 0)
            {
                PurjaTransferDocDate = CloseDate.AddDays(1);
                PurjaTransferDocNo = new DocumentTypeService(_unitOfWork).FGetNewDocNo("DocNo", "Web.StockHeaders", PurjaTransferDocTypeId, PurjaTransferDocDate, DivisionId, SiteId);


                mQry = "INSERT INTO Web.StockHeaders (DocHeaderId, DocTypeId, DocDate, DocNo, DivisionId, SiteId, CurrencyId, PersonId, ProcessId, FromGodownId, GodownId, " +
                        " Remark, Status, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, OMSId, CostCenterId, MachineId, GatePassHeaderId, LedgerHeaderId, ContraLedgerAccountId, " +
                        " ReferenceDocTypeId, ReferenceDocId, LockReason, LedgerAccountId, ReviewCount, ReviewBy, IsGatePassPrinted) " +
                        " Select Null As DocHeaderId, " + PurjaTransferDocTypeId + " As DocTypeId,  '" + PurjaTransferDocDate + "' As  DocDate,  '" + PurjaTransferDocNo + "' As DocNo, " +
                        " H.DivisionId, H.SiteId, Null As CurrencyId, H.JobWorkerId As PersonId, H.ProcessId, Null As FromGodownId, Null As GodownId, " +
                        " 'Purja Transfer For Closing' As Remark, 1 As Status,  'System' As CreatedBy, getdate() As CreatedDate, 'System' As ModifiedBy, getdate() As ModifiedDate, " +
                        " Null As OMSId, H.CostCenterId, Null As MachineId, Null As GatePassHeaderId, Null As LedgerHeaderId, Null As ContraLedgerAccountId, " +
                        " Null As ReferenceDocTypeId, Null As ReferenceDocId, Null As LockReason, Null As LedgerAccountId, Null As ReviewCount, Null As ReviewBy, Null As IsGatePassPrinted " +
                        " From Web.JobOrderHeaders H Where H.JobOrderHeaderId = " + OldJobOrderId + "";
                Cmd = new SqlCommand(mQry, Con);
                if (Cmd.Connection.State != ConnectionState.Open)
                    Cmd.Connection.Open();
                Cmd.Transaction = ETrans;
                Cmd.ExecuteNonQuery();

                mQry = "SELECT * FROM Web.StockHeaders C WHERE C.DocTypeId =" + PurjaTransferDocTypeId + " AND C.SiteId = " + SiteId + " AND C.DivisionId =" + DivisionId + "  AND C.DocNo = '" + PurjaTransferDocNo + "' ";
                StockOrderHeaderDA = new SqlDataAdapter(mQry, Con);
                StockOrderHeaderDA.SelectCommand.CommandTimeout = 500;
                StockOrderHeaderDA.SelectCommand.Transaction = ETrans;
                StockOrderHeaderDA.Fill(DtStockHeader);


                mQry = " INSERT INTO Web.StockLines (StockHeaderId, ProductUidId, RequisitionLineId, ProductId, FromProcessId, LotNo, Qty, Remark, Dimension1Id, Dimension2Id, " +
                        " Specification, Rate, Amount, CreatedBy, ModifiedBy, CreatedDate, ModifiedDate, OMSId, DocProductId, DocDimension1, DocDimension2, FromStockId, " +
                        " StockId, StockProcessHeader_StockProcessHeaderId, StockProcessId, CostCenterId, DocNature, ReferenceDocTypeId, ReferenceDocId, ReferenceDocLineId, " +
                        " LockReason, FromStockProcessid, Sr, ProductUidLastTransactionDocId, ProductUidLastTransactionDocNo, ProductUidLastTransactionDocTypeId, " +
                        " ProductUidLastTransactionDocDate, ProductUidLastTransactionPersonId, ProductUidCurrentGodownId, ProductUidCurrentProcessId, ProductUidStatus, " +
                        " Weight, Dimension3Id, Dimension4Id, StockInId) " +
                        " Select " + DtStockHeader.Rows[0]["StockHeaderId"] + " As StockHeaderId, Null As ProductUidId, Null As RequisitionLineId, L.ProductId, 43 As FromProcessId, Null As LotNo, " +
                        " IsNull(Sum(L.Qty_Rec),0) - IsNull(Sum(L.Qty_Iss),0) As Qty, 'Purja Transfer For Closing' As Remark, " +
                        " Null As Dimension1Id, Null As Dimension2Id, " +
                        " Null As Specification, 0 As Rate, 0 As Amount, '" + DtStockHeader.Rows[0]["CreatedBy"] + "'  As CreatedBy,  " +
                        " '" + DtStockHeader.Rows[0]["ModifiedBy"] + "'  As ModifiedBy, '" + DtStockHeader.Rows[0]["CreatedDate"] + "'  As CreatedDate, " +
                        " '" + DtStockHeader.Rows[0]["ModifiedDate"] + "'  As ModifiedDate, Null As OMSId, L.ProductId As DocProductId, " +
                        " Null As DocDimension1, Null As DocDimension2, Null As FromStockId, " +
                        " Null As StockId, Null As StockProcessHeader_StockProcessHeaderId, Null As StockProcessId, " + NewCostCenterId + " As CostCenterId, 'T' As DocNature, " +
                        " Null As ReferenceDocTypeId, Null As ReferenceDocId, Null As ReferenceDocLineId, " +
                        " Null As LockReason, Null As FromStockProcessid, 1 As Sr, Null As ProductUidLastTransactionDocId, Null As ProductUidLastTransactionDocNo, Null As ProductUidLastTransactionDocTypeId, " +
                        " Null As ProductUidLastTransactionDocDate, Null As ProductUidLastTransactionPersonId, Null As ProductUidCurrentGodownId, Null As ProductUidCurrentProcessId, Null As ProductUidStatus, " +
                        " Null As Weight, Null As Dimension3Id, Null As Dimension4Id, Null As StockInId " +
                        " From Web.StockProcesses L " +
                        " Where L.CostCenterId = " + OldCostCenterId + "" +
                        " Group By L.ProductId " +
                        " HAVING IsNull(Sum(L.Qty_Rec),0) - IsNull(Sum(L.Qty_Iss),0) > 0 ";
                Cmd = new SqlCommand(mQry, Con);
                if (Cmd.Connection.State != ConnectionState.Open)
                    Cmd.Connection.Open();
                Cmd.Transaction = ETrans;
                Cmd.ExecuteNonQuery();


                mQry = " INSERT INTO Web.StockProcesses (StockHeaderId, ProductId, ProcessId, LotNo, CostCenterId, Qty_Iss, Qty_Rec, " +
                        " Rate, ExpiryDate, Specification, Dimension1Id, Dimension2Id, OMSId, GodownId, CreatedBy, ModifiedBy, CreatedDate, ModifiedDate,  " +
                        " DocDate, DocLineId, Remark, ProductUidId, ReferenceDocTypeId, ReferenceDocId, ProductGroupId,  " +
                        " Weight_Rec, Weight_Iss, Dimension3Id, Dimension4Id) " +
                        " SELECT H.StockHeaderId, L.ProductId, H.ProcessId, L.LotNo, " +
                        " H.CostCenterId, L.Qty AS Qty_Iss, 0 AS Qty_Rec,  " +
                        " 0 AS Rate, NULL AS ExpiryDate, L.Specification, L.Dimension1Id, L.Dimension2Id, NULL AS OMSId,  " +
                        " NULL AS GodownId, L.CreatedBy, L.ModifiedBy, L.CreatedDate, L.ModifiedDate,  " +
                        " H.DocDate, L.StockLineId AS DocLineId, L.Remark, L.ProductUidId, NULL AS ReferenceDocTypeId, NULL AS ReferenceDocId,  " +
                        " NULL AS ProductGroupId, NULL AS Weight_Rec, NULL AS Weight_Iss, NULL AS Dimension3Id, NULL AS Dimension4Id " +
                        " FROM Web.StockHeaders H  " +
                        " LEFT JOIN Web.StockLines L ON H.StockHeaderId = L.StockHeaderId " +
                        " WHERE H.StockHeaderId = " + DtStockHeader.Rows[0]["StockHeaderId"] + " " +
                        " UNION ALL  " +
                        " SELECT H.StockHeaderId, L.ProductId, H.ProcessId, L.LotNo, " +
                        " L.CostCenterId, 0 AS Qty_Iss, L.Qty AS Qty_Rec,  " +
                        " 0 AS Rate, NULL AS ExpiryDate, L.Specification, L.Dimension1Id, L.Dimension2Id, NULL AS OMSId,  " +
                        " NULL AS GodownId, L.CreatedBy, L.ModifiedBy, L.CreatedDate, L.ModifiedDate,  " +
                        " H.DocDate, L.StockLineId AS DocLineId, L.Remark, L.ProductUidId, NULL AS ReferenceDocTypeId, NULL AS ReferenceDocId,  " +
                        " NULL AS ProductGroupId, NULL AS Weight_Rec, NULL AS Weight_Iss, NULL AS Dimension3Id, NULL AS Dimension4Id " +
                        " FROM Web.StockHeaders H  " +
                        " LEFT JOIN Web.StockLines L ON H.StockHeaderId = L.StockHeaderId " +
                        " WHERE H.StockHeaderId = " + DtStockHeader.Rows[0]["StockHeaderId"] + " ";
                Cmd = new SqlCommand(mQry, Con);
                if (Cmd.Connection.State != ConnectionState.Open)
                    Cmd.Connection.Open();
                Cmd.Transaction = ETrans;
                Cmd.ExecuteNonQuery();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void InitializeConnection()
        {
            Con = new SqlConnection(connectionString);
            Con.Open();
            Cmd = new SqlCommand();
            Cmd = Con.CreateCommand();
            ETrans = Con.BeginTransaction(IsolationLevel.ReadCommitted);
            Cmd.Transaction = ETrans;
        }

        public void FTransferProductUids(int OldJobOrderLineId, SqlTransaction ETrans)
        {
            if (SiteId == 17)
            {
                mQry = " INSERT INTO Web.ProductUidHeaders (ProductId, Dimension1Id, Dimension2Id, LotNo, " +
                        " GenDocId, GenDocNo, GenDocTypeId, GenDocDate, GenPersonId, CreatedBy, ModifiedBy, CreatedDate, ModifiedDate,  " +
                        " OMSId, GenRemark, ReferenceDocTypeId, ReferenceDocId, Dimension3Id, Dimension4Id) " +
                        " SELECT L.ProductId, NULL AS Dimension1Id, NULL AS Dimension2Id, NULL AS LotNo,  " +
                        " Nh.JobOrderHeaderId AS GenDocId, Nh.DocNo AS GenDocNo, Nh.DocTypeId AS GenDocTypeId,  " +
                        " Nh.DocDate AS GenDocDate, Nh.JobWorkerId AS GenPersonId, 'System' AS CreatedBy, 'System' AS ModifiedBy,  " +
                        " getdate() AS CreatedDate, getdate() AS ModifiedDate,  " +
                        " NULL AS OMSId, 'Product Uid Transfer On Closing' AS GenRemark,  " +
                        " 706 AS ReferenceDocTypeId, Puh.ProductUIDHeaderId AS ReferenceDocId, NULL AS Dimension3Id, NULL AS Dimension4Id " +
                        " FROM Web.JobOrderLines L  " +
                        " LEFT JOIN Web.ProductUidHeaders Puh ON L.ProductUidHeaderId = Puh.ProductUIDHeaderId " +
                        " LEFT JOIN ( " +
                        " 	SELECT Pu.ProductUidHeaderId, Count(*) AS Cnt " +
                        " 	FROM Web.ProductUids Pu " +
                        " 	WHERE Pu.GenDocId = Pu.LastTransactionDocId " +
                        " 	GROUP BY Pu.ProductUidHeaderId " +
                        " ) AS V1 ON Puh.ProductUidHeaderId = V1.ProductUidHeaderId " +
                        " LEFT JOIN Web.JobOrderHeaders H ON L.JobOrderHeaderId = H.JobOrderHeaderId " +
                        " LEFT JOIN Web.JobOrderJobOrders Jj ON H.JobOrderHeaderId = Jj.GenJobOrderHeaderId " +
                        " LEFT JOIN Web.JobOrderHeaders Nh ON JJ.JobOrderHeaderId = Nh.JobOrderHeaderId " +
                        " WHERE V1.Cnt > 0 " +
                        " AND L.JobOrderLineId = " + OldJobOrderLineId + " ";
                Cmd = new SqlCommand(mQry, Con);
                if (Cmd.Connection.State != ConnectionState.Open)
                    Cmd.Connection.Open();
                Cmd.Transaction = ETrans;
                Cmd.ExecuteNonQuery();


                mQry = "UPDATE Web.ProductUids " +
                        " SET Web.ProductUids.ProductUidHeaderId = V1.ProductUIDHeaderId " +
                        " FROM ( " +
                        " 	SELECT Pu.ProductUIDId, Puh1.ProductUIDHeaderId " +
                        " 	FROM Web.JobOrderLines L  " +
                        " 	LEFT JOIN Web.ProductUidHeaders Puh ON L.ProductUidHeaderId = Puh.ProductUIDHeaderId " +
                        " 	LEFT JOIN Web.ProductUidHeaders Puh1 ON Puh.ProductUIDHeaderId = Puh1.ReferenceDocId " +
                        " 	LEFT JOIN Web.ProductUids Pu ON Puh.ProductUIDHeaderId = Pu.ProductUidHeaderId " +
                        " 	WHERE Pu.GenDocId = Pu.LastTransactionDocId " +
                        " 	AND L.JobOrderLineId = " + OldJobOrderLineId + "  " +
                        " ) AS V1 WHERE Web.ProductUids.ProductUidId = V1.ProductUidId ";
                Cmd = new SqlCommand(mQry, Con);
                if (Cmd.Connection.State != ConnectionState.Open)
                    Cmd.Connection.Open();
                Cmd.Transaction = ETrans;
                Cmd.ExecuteNonQuery();


                mQry = " UPDATE Web.JobOrderLines " +
                        " SET Web.JobOrderLines.ProductUidHeaderId = V1.ProductUidHeaderID " +
                        " FROM ( " +
                        " 	SELECT Jol.JobOrderLineId, Puh1.ProductUIDHeaderId " +
                        " 	FROM Web.JobOrderLines L    " +
                        " 	LEFT JOIN Web.JobOrderHeaders H ON L.JobOrderHeaderId = H.JobOrderHeaderId " +
                        " 	LEFT JOIN Web.JobOrderJobOrders Jj ON H.JobOrderHeaderId = JJ.GenJobOrderHeaderId " +
                        " 	LEFT JOIN Web.JobOrderLines Jol ON Jj.JobOrderHeaderId = Jol.JobOrderHeaderId   " +
                        " 		AND L.ProductId = Jol.ProductId  " +
                        " 		AND IsNull(L.ProdOrderLineId,0) = IsNull(Jol.ProdOrderLineId,0) " +
                        " 	LEFT JOIN Web.ProductUidHeaders Puh ON L.ProductUidHeaderId = Puh.ProductUIDHeaderId   " +
                        " 	LEFT JOIN Web.ProductUidHeaders Puh1 ON Puh.ProductUIDHeaderId = Puh1.ReferenceDocId   " +
                        " 	WHERE L.JobOrderLineId =   " + OldJobOrderLineId + "   " +
                        " ) AS V1 WHERE Web.JobOrderLines.JobOrderLineId = V1.JobOrderLineId";
                Cmd = new SqlCommand(mQry, Con);
                if (Cmd.Connection.State != ConnectionState.Open)
                    Cmd.Connection.Open();
                Cmd.Transaction = ETrans;
                Cmd.ExecuteNonQuery();


                mQry = "UPDATE Web.ProductUids " +
                        " SET Web.ProductUids.GenDocId = V1.GenDocId, " +
                        " Web.ProductUids.GenDocNo = V1.GenDocNo, " +
                        " Web.ProductUids.GenDocTypeId = V1.GenDocTypeId, " +
                        " Web.ProductUids.GenDocDate = V1.GenDocDate, " +
                        " Web.ProductUids.GenPersonId = V1.GenPersonId, " +
                        " Web.ProductUids.LastTransactionDocId = V1.GenDocId, " +
                        " Web.ProductUids.LastTransactionDocNo = V1.GenDocNo, " +
                        " Web.ProductUids.LastTransactionDocTypeId = V1.GenDocTypeId, " +
                        " Web.ProductUids.LastTransactionDocDate = V1.GenDocDate, " +
                        " Web.ProductUids.LastTransactionPersonId = V1.GenPersonId " +
                        " FROM ( " +
                        " 	SELECT Pu.ProductUIDId, Puh1.GenDocId, Puh1.GenDocNo, Puh1.GenDocTypeId, Puh1.GenDocDate, Puh1.GenPersonId " +
                        " 	FROM Web.JobOrderLines L  " +
                        " 	LEFT JOIN Web.ProductUidHeaders Puh ON L.ProductUidHeaderId = Puh.ProductUIDHeaderId " +
                        " 	LEFT JOIN Web.ProductUidHeaders Puh1 ON Puh.ProductUIDHeaderId = Puh1.ReferenceDocId " +
                        " 	LEFT JOIN Web.ProductUids Pu ON Puh1.ProductUIDHeaderId = Pu.ProductUidHeaderId " +
                        " 	WHERE L.JobOrderLineId = " + OldJobOrderLineId + "   " +
                        " ) AS V1 WHERE Web.ProductUids.ProductUidId = V1.ProductUidId ";
                Cmd = new SqlCommand(mQry, Con);
                if (Cmd.Connection.State != ConnectionState.Open)
                    Cmd.Connection.Open();
                Cmd.Transaction = ETrans;
                Cmd.ExecuteNonQuery();
            }
            else
            {
                mQry = "UPDATE Web.ProductUids " +
                        " SET Web.ProductUids.LastTransactionDocId = V1.LastTransactionDocId, " +
                        " Web.ProductUids.LastTransactionDocNo = V1.LastTransactionDocNo, " +
                        " Web.ProductUids.LastTransactionDocTypeId = V1.LastTransactionDocTypeId, " +
                        " Web.ProductUids.LastTransactionDocDate = V1.LastTransactionDocDate, " +
                        " Web.ProductUids.LastTransactionPersonId = V1.LastTransactionPersonId " +
                        " FROM ( " +
                        " 	SELECT L.ProductUidId, Jh.JobOrderHeaderId AS LastTransactionDocId, Jh.DocNo AS LastTransactionDocNo,  " +
                        " 	Jh.DocTypeId AS LastTransactionDocTypeId, Jh.DocDate AS LastTransactionDocDate, Jh.JobWorkerId AS LastTransactionPersonId " +
                        " 	FROM Web.JobOrderLines L  " +
                        " 	LEFT JOIN Web.JobOrderHeaders H ON L.JobOrderHeaderId = H.JobOrderHeaderId "+
                        "   LEFT JOIN Web.JobOrderJobOrders JJ ON H.JobOrderHeaderId = JJ.GenJobOrderHeaderId "+
	                    "   LEFT JOIN Web.JobOrderHeaders Jh ON Jj.JobOrderHeaderId = Jh.JobOrderHeaderId "+
                        " 	WHERE L.JobOrderLineId = " + OldJobOrderLineId + "   " +
                        " ) AS V1 WHERE Web.ProductUids.ProductUidId = V1.ProductUidId ";
                Cmd = new SqlCommand(mQry, Con);
                if (Cmd.Connection.State != ConnectionState.Open)
                    Cmd.Connection.Open();
                Cmd.Transaction = ETrans;
                Cmd.ExecuteNonQuery();
            }
        }


        public void FRollBack(SqlTransaction ETrans)
        {
            mQry = " DELETE FROM Web.StockLines	WHERE StockHeaderId IN (SELECT StockHeaderId FROM Web.StockHeaders WHERE DocTypeId = 288 AND DocDate = '01/Apr/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();

            mQry = " DELETE FROM Web.StockProcesses	WHERE StockHeaderId IN (SELECT StockHeaderId FROM Web.StockHeaders WHERE DocTypeId = 288 AND DocDate = '01/Apr/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();

            mQry = " DELETE FROM Web.StockHeaders	WHERE StockHeaderId IN (SELECT StockHeaderId FROM Web.StockHeaders WHERE DocTypeId = 288 AND DocDate = '01/Apr/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();

            mQry = " DELETE FROM Web.RequisitionCancelLines	WHERE RequisitionCancelHeaderId IN (SELECT RequisitionCancelHeaderId FROM Web.RequisitionCancelHeaders WHERE DocTypeId = 633 AND DocDate = '31/Mar/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();

            mQry = " DELETE FROM Web.RequisitionCancelHeaders WHERE RequisitionCancelHeaderId IN (SELECT RequisitionCancelHeaderId FROM Web.RequisitionCancelHeaders WHERE DocTypeId = 633 AND DocDate = '31/Mar/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();


            mQry = " DELETE FROM Web.RequisitionLineStatus	WHERE RequisitionLineId IN (SELECT L.RequisitionLineId FROM Web.RequisitionHeaders H LEFT JOIN Web.RequisitionLines L ON H.RequisitionHeaderId = L.RequisitionHeaderId WHERE H.DocTypeId = 361 AND H.DocDate = '01/Apr/2017' AND H.CreatedBy = 'System' And H.DivisionId = " + DivisionId + " And H.SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();


            mQry = " DELETE FROM Web.RequisitionLines	WHERE RequisitionHeaderId IN (SELECT RequisitionHeaderId FROM Web.RequisitionHeaders WHERE DocTypeId = 361 AND DocDate = '01/Apr/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();

            mQry = " DELETE FROM Web.RequisitionHeaders WHERE RequisitionHeaderId IN (SELECT RequisitionHeaderId FROM Web.RequisitionHeaders WHERE DocTypeId = 361 AND DocDate = '01/Apr/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();




            mQry = " DELETE FROM Web.JobOrderCancelBoms 	WHERE JobOrderCancelHeaderId IN (SELECT JobOrderCancelHeaderId FROM Web.JobOrderCancelHeaders WHERE DocTypeId = 449 AND DocDate = '31/Mar/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();

            mQry = " DELETE FROM Web.JobOrderCancelLines	WHERE JobOrderCancelHeaderId IN (SELECT JobOrderCancelHeaderId FROM Web.JobOrderCancelHeaders WHERE DocTypeId = 449 AND DocDate = '31/Mar/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();

            mQry = " DELETE FROM Web.JobOrderCancelHeaders	WHERE JobOrderCancelHeaderId IN (SELECT JobOrderCancelHeaderId FROM Web.JobOrderCancelHeaders WHERE DocTypeId = 449 AND DocDate = '31/Mar/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();


            mQry = " DELETE FROM Web.JobOrderBoms 	WHERE JobOrderHeaderId IN (SELECT JobOrderHeaderId FROM Web.JobOrderHeaders WHERE DocTypeId = 458 AND DocDate = '01/Apr/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();


            if (SiteId == 17)
            {
                mQry = " UPDATE Web.ProductUids " +
                        " SET Web.ProductUids.ProductUidHeaderId = V1.ProductUidHeaderId " +
                        " FROM ( " +
                        " 	SELECT Pu.ProductUIDId, Puh.ProductUIDHeaderId " +
                        " 	FROM Web.JobOrderHeaders Joh  " +
                        " 	LEFT JOIN Web.JobOrderLines Jol ON Joh.JobOrderHeaderId = Jol.JobOrderHeaderId " +
                        " 	LEFT JOIN Web.ProductUidHeaders H ON Jol.ProductUidHeaderId = H.ProductUIDHeaderId " +
                        " 	LEFT JOIN Web.ProductUidHeaders Puh ON H.ReferenceDocId =Puh.ProductUIDHeaderId " +
                        " 	LEFT JOIN Web.ProductUids Pu ON H.ProductUIDHeaderId = Pu.ProductUidHeaderId " +
                        " 	WHERE Joh.DocDate = '01/Apr/2017'  " +
                        " 	AND Joh.CreatedBy = 'System' " +
                        " 	AND Joh.SiteId = " + SiteId + " AND Joh.DivisionId = " + DivisionId + " " +
                        "   AND Pu.ProductUIDId IS NOT NULL AND Puh.ProductUIDHeaderId IS NOT NULL " +
                        " ) AS V1 WHERE Web.ProductUids.ProductUidId = V1.ProductUidId ";
                Cmd = new SqlCommand(mQry, Con);
                if (Cmd.Connection.State != ConnectionState.Open)
                    Cmd.Connection.Open();
                Cmd.Transaction = ETrans;
                Cmd.ExecuteNonQuery();


                mQry = " UPDATE Web.JobOrderLines " +
                        " SET Web.JobOrderLines.ProductUidHeaderId = V1.ProductUidHeaderID " +
                        " FROM ( " +
                        " 	SELECT Jol.JobOrderLineId, Puh1.ProductUIDHeaderId " +
                        " 	FROM Web.JobOrderLines L    " +
                        " 	LEFT JOIN Web.JobOrderHeaders H ON L.JobOrderHeaderId = H.JobOrderHeaderId " +
                        " 	LEFT JOIN Web.JobOrderJobOrders Jj ON H.JobOrderHeaderId = JJ.GenJobOrderHeaderId " +
                        " 	LEFT JOIN Web.JobOrderLines Jol ON Jj.JobOrderHeaderId = Jol.JobOrderHeaderId   " +
                        " 		AND L.ProductId = Jol.ProductId  " +
                        " 		AND IsNull(L.ProdOrderLineId,0) = IsNull(Jol.ProdOrderLineId,0) " +
                        " 	LEFT JOIN Web.ProductUidHeaders Puh ON L.ProductUidHeaderId = Puh.ProductUIDHeaderId   " +
                        " 	LEFT JOIN Web.ProductUidHeaders Puh1 ON Puh.ReferenceDocId = Puh1.ProductUIDHeaderId  " +
                        " 	WHERE H.DocDate = '01/Apr/2017' " +
                        " 	AND H.CreatedBy = 'System' " +
                        " 	AND H.SiteId = " + SiteId + "  AND H.DivisionId = " + DivisionId + " " +
                        " ) AS V1 WHERE Web.JobOrderLines.JobOrderLineId = V1.JobOrderLineId ";
                Cmd = new SqlCommand(mQry, Con);
                if (Cmd.Connection.State != ConnectionState.Open)
                    Cmd.Connection.Open();
                Cmd.Transaction = ETrans;
                Cmd.ExecuteNonQuery();
            }
            else
            {
                mQry = "UPDATE Web.ProductUids " +
                        " SET Web.ProductUids.LastTransactionDocId = V1.LastTransactionDocId, " +
                        " Web.ProductUids.LastTransactionDocNo = V1.LastTransactionDocNo, " +
                        " Web.ProductUids.LastTransactionDocTypeId = V1.LastTransactionDocId, " +
                        " Web.ProductUids.LastTransactionDocDate = V1.LastTransactionDocDate, " +
                        " Web.ProductUids.LastTransactionPersonId = V1.LastTransactionPersonId " +
                        " FROM ( " +
                        " 	SELECT L.ProductUidId, Joh.JobOrderHeaderId AS LastTransactionDocId, Joh.DocNo AS LastTransactionDocNo,  " +
                        "   Joh.DocTypeId AS LastTransactionDocTypeId, Joh.DocDate AS LastTransactionDocDate, Joh.JobWorkerId AS LastTransactionPersonId " +
                        "   FROM Web.JobOrderLines L  " +
                        "   LEFT JOIN Web.JobOrderHeaders H ON L.JobOrderHeaderId = H.JobOrderHeaderId " +
                        "   LEFT JOIN Web.JobOrderJobOrders Jj ON H.JobOrderHeaderId = Jj.JobOrderHeaderId " +
                        "   LEFT JOIN Web.JobOrderHeaders Joh ON Jj.GenJobOrderHeaderId = Joh.JobOrderHeaderId " +
                        " 	WHERE H.DocDate = '01/Apr/2017' " +
                        " 	AND H.CreatedBy = 'System' " +
                        " 	AND H.SiteId = " + SiteId + "  AND H.DivisionId = " + DivisionId + " " +
                        " ) AS V1 WHERE Web.ProductUids.ProductUidId = V1.ProductUidId ";
                Cmd = new SqlCommand(mQry, Con);
                if (Cmd.Connection.State != ConnectionState.Open)
                    Cmd.Connection.Open();
                Cmd.Transaction = ETrans;
                Cmd.ExecuteNonQuery();
            }

            SqlDataAdapter ProductUidHeaderDA;
            DataTable DtProductUidHeader = new DataTable();
            mQry = " SELECT H.ProductUIDHeaderId " +
                    " 	FROM Web.JobOrderHeaders Joh  " +
                    " 	LEFT JOIN Web.JobOrderLines Jol ON Joh.JobOrderHeaderId = Jol.JobOrderHeaderId " +
                    " 	LEFT JOIN Web.ProductUidHeaders H ON Jol.ProductUidHeaderId = H.ProductUIDHeaderId " +
                    " 	WHERE Joh.DocDate = '01/Apr/2017'  " +
                    " 	AND Joh.CreatedBy = 'System' " +
                    " 	AND Joh.SiteId = " + SiteId + " AND Joh.DivisionId = " + DivisionId + " ";
            ProductUidHeaderDA = new SqlDataAdapter(mQry, Con);
            ProductUidHeaderDA.SelectCommand.CommandTimeout = 500;
            ProductUidHeaderDA.SelectCommand.Transaction = ETrans;
            ProductUidHeaderDA.Fill(DtProductUidHeader);



            mQry = " DELETE FROM Web.JobOrderLines	WHERE JobOrderHeaderId IN (SELECT JobOrderHeaderId FROM Web.JobOrderHeaders WHERE DocTypeId = 458 AND DocDate = '01/Apr/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();


            if (SiteId == 17)
            {
                for (int I = 0; I < DtProductUidHeader.Rows.Count; I++)
                {
                    mQry = " DELETE FROM Web.ProductUidHeaders WHERE ProductUidHeaderId = " + DtProductUidHeader.Rows[I]["ProductUIDHeaderId"] + " ";
                    Cmd = new SqlCommand(mQry, Con);
                    if (Cmd.Connection.State != ConnectionState.Open)
                        Cmd.Connection.Open();
                    Cmd.Transaction = ETrans;
                    Cmd.ExecuteNonQuery();
                }
            }



            mQry = " DELETE FROM Web.JobOrderJobOrders	WHERE JobOrderHeaderId IN (SELECT JobOrderHeaderId FROM Web.JobOrderHeaders WHERE DocTypeId = 458 AND DocDate = '01/Apr/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();

            mQry = " DELETE FROM Web.JobOrderHeaders	WHERE JobOrderHeaderId IN (SELECT JobOrderHeaderId FROM Web.JobOrderHeaders WHERE DocTypeId = 458 AND DocDate = '01/Apr/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + ") ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();





            mQry = " DELETE FROM Web.CostCenters  WHERE DocTypeId = 458 AND StartDate = '01/Apr/2017' AND CreatedBy = 'System' And DivisionId = " + DivisionId + " And SiteId = " + SiteId + " ";
            Cmd = new SqlCommand(mQry, Con);
            if (Cmd.Connection.State != ConnectionState.Open)
                Cmd.Connection.Open();
            Cmd.Transaction = ETrans;
            Cmd.ExecuteNonQuery();


        }

        public void PostJobOrderConsumptionMainSite(int JobOrderHeaderId, SqlTransaction ETrans)
        {
            SqlDataAdapter JobOrderLineDA;
            DataTable DtJobOrderLine = new DataTable();

            SqlDataAdapter JobOrderHeaderDA;
            DataTable DtJobOrderHeader = new DataTable();

            mQry = " SELECT * FROM Web.JobOrderHeaders H With (NoLock) WHERE H.JobOrderHeaderId = " + JobOrderHeaderId.ToString() + "  ";
            JobOrderHeaderDA = new SqlDataAdapter(mQry, Con);
            JobOrderHeaderDA.SelectCommand.CommandTimeout = 500;
            JobOrderHeaderDA.SelectCommand.Transaction = ETrans;
            JobOrderHeaderDA.Fill(DtJobOrderHeader);


            mQry = " SELECT * FROM Web.JobOrderLines L With (NoLock) WHERE L.JobOrderHeaderId = " + JobOrderHeaderId.ToString() + "  ";
            JobOrderLineDA = new SqlDataAdapter(mQry, Con);
            JobOrderLineDA.SelectCommand.CommandTimeout = 500;
            JobOrderLineDA.SelectCommand.Transaction = ETrans;
            JobOrderLineDA.Fill(DtJobOrderLine);


            for (int I = 0; I < DtJobOrderLine.Rows.Count; I++)
            {
                SqlParameter SQLDocTypeID = new SqlParameter("@DocTypeId", DtJobOrderHeader.Rows[0]["DocTypeId"]);
                SqlParameter SQLProductID = new SqlParameter("@ProductId", DtJobOrderLine.Rows[I]["ProductId"]);
                SqlParameter SQLProcessId = new SqlParameter("@ProcessId", DtJobOrderHeader.Rows[0]["ProcessId"]);
                SqlParameter SQLQty = new SqlParameter("@Qty",  DtJobOrderLine.Rows[I]["Qty"]);

                List<ProcGetBomForWeavingViewModel> BomPostList = new List<ProcGetBomForWeavingViewModel>();
                BomPostList = db.Database.SqlQuery<ProcGetBomForWeavingViewModel>("Web.ProcGetBomForWeaving @DocTypeId, @ProductId, @ProcessId,@Qty", SQLDocTypeID, SQLProductID, SQLProcessId, SQLQty).ToList();

                foreach (var item in BomPostList)
                {
                    if (item.Dimension1Id != null)
                    {
                        mQry = " INSERT INTO Web.JobOrderBoms (JobOrderHeaderId, JobOrderLineId, ProductId, Qty, CreatedBy, ModifiedBy, CreatedDate, ModifiedDate, OMSId, " +
                            " Dimension1Id, Dimension2Id, CreatedMode, BOMQty, Dimension3Id, Dimension4Id) " +
                            " Select " + JobOrderHeaderId + ", " + DtJobOrderLine.Rows[I]["JobOrderLineId"].ToString() + ", " + item.ProductId + ", " + Convert.ToDecimal(item.Qty) + ", " +
                            " 'System' As CreatedBy, 'System' As ModifiedBy, getdate() As CreatedDate, getdate() As modifieddate, NUll As omsid, " +
                            " " + item.Dimension1Id + " As dimension1id, Null As dimension2id, Null As createdmode, Null As bomqty, " +
                            " Null As dimension3id, Null As dimension4id ";
                    }
                    else
                    {
                        mQry = " INSERT INTO Web.JobOrderBoms (JobOrderHeaderId, JobOrderLineId, ProductId, Qty, CreatedBy, ModifiedBy, CreatedDate, ModifiedDate, OMSId, " +
                            " Dimension1Id, Dimension2Id, CreatedMode, BOMQty, Dimension3Id, Dimension4Id) " +
                            " Select " + JobOrderHeaderId + ", " + DtJobOrderLine.Rows[I]["JobOrderLineId"].ToString() + ", " + item.ProductId + ", " + Convert.ToDecimal(item.Qty) + ", " +
                            " 'System' As CreatedBy, 'System' As ModifiedBy, getdate() As CreatedDate, getdate() As modifieddate, NUll As omsid, " +
                            " Null As dimension1id, Null As dimension2id, Null As createdmode, Null As bomqty, " +
                            " Null As dimension3id, Null As dimension4id ";
                    }


                    Cmd = new SqlCommand(mQry, Con);
                    if (Cmd.Connection.State != ConnectionState.Open)
                        Cmd.Connection.Open();
                    Cmd.Transaction = ETrans;
                    Cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
