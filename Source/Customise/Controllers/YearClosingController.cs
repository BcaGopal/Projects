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
        public string mQry = "";
        public int SiteId = (int)(System.Web.HttpContext.Current.Session["SiteId"]);
        public int DivisionId = (int)(System.Web.HttpContext.Current.Session["DivisionId"]);

        private ApplicationDbContext db = new ApplicationDbContext();

        IJobOrderHeaderService _JobOrderHeaderService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        public YearClosingController(IJobOrderHeaderService PurchaseOrderHeaderService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _JobOrderHeaderService = PurchaseOrderHeaderService;
            _exception = exec;
            _unitOfWork = unitOfWork;
        }


        private Boolean FPostJobOrderCancel(DateTime CloseDate)
        {

            Boolean IsSuccess = false;
            string mTrans = "";
            DataTable DtJobOrder = new DataTable();
            DataTable DtJobOrderLine = new DataTable();

            int I = 0, J = 0;

            //For Job Order Cancel

            int JobOrderCancelDocTypeId, JobOrderCancelReasonId, JobOrderCancelHeaderId;
            string JobOrderCancelDocDate = "", JobOrderCancelDocNo = "";


            // For Job Order
            string JobOrderDocNo = "";
            int CostCenterId, JobOrderHeaderId;
            string NewCostcenterNo = "";




            int WVORD = 458, WFORD = 455, WVORH = 637, WVORB = 638;
            int WVREC = 448;
            int WVCNL = 449;

            try
            {

                Con = new SqlConnection(connectionString);
                SqlCommand cmd;
                SqlDataAdapter SDA;
                mTrans = "Begin";



                // *****************  For Getting Pending Weaving Orders

                mQry = "SELECT Top 10 JOH.JobOrderHeaderId, Max(convert(INT,JW.IsSisterConcern)) AS IsSisterConcern,  Max(C.CostcenterName) AS CostcenterName, Max(CE.ProductId) AS ProductId, Max(LA.LedgerAccountId) AS LedgerAccountId, Max(JOH.Remark) AS Remark, Max(JOH.UnitconversionForId) AS UnitconversionForId, Max(JOH.ActualDueDate) AS ActualDueDate, Max(JOH.DocTypeId) AS DocTypeId, Max(JOH.DocDate) AS DocDate, Max(JOH.DocNo) AS DocNo, Max(JOH.DivisionId) AS DivisionId, Max(JOH.SiteId) AS SiteId, Max(JOH.DueDate) AS DueDate, " +
                    " Max(JOH.JobWorkerId) AS JobWorkerId , Max(JOH.BillToPartyId) AS BillToPartyId, Max(JOH.OrderById) AS OrderById, Max(isnull(JOH.GodownId,0)) AS GodownId, Max(JOH.ProcessId) AS ProcessId, Max(JOH.CostCenterId) AS CostCenterId ,Max(JOH.TermsAndConditions) AS   TermsAndConditions " +
                    " FROM " +
                    " ( " +
                    " SELECT H.JobOrderHeaderId AS JobOrderHeaderId, L.JobOrderLineId,  " +
                    " L.Qty -  isnull(VRec.RecQty,0) - isnull(VCan.CanQty,0) AS BalQty   " +
                    " FROM " +
                    " ( " +
                    " SELECT H.JobOrderHeaderId FROM Web.JobOrderHeaders H WITH (Nolock) WHERE H.DocTypeId IN ( " + WVORD + "," + WFORD + " )  " +
                    " AND H.DocDate <=' " + CloseDate + "' AND H.SiteId = " + SiteId + " AND H.DivisionId =" + DivisionId + " " +
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
                    " GROUP BY JOH.JobOrderHeaderId " +
                    " Order by (Case When IsNumeric(Max(JOH.DocNo)) > 0 Then Convert(Numeric,Max(JOH.DocNo)) Else 0 End) ";


                SDA = new SqlDataAdapter(mQry, Con);
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
                              " 1, Null,  '" + User.Identity.Name + "', '" + User.Identity.Name + "', getdate(),getdate(), " + DtJobOrder.Rows[I]["JobWorkerId"].ToString() + " , " + DtJobOrder.Rows[I]["ProcessId"].ToString() + " , " + DtJobOrder.Rows[I]["OrderById"].ToString() + "  ) ";

                        cmd = new SqlCommand(mQry, Con);
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();


                        mQry = "SELECT JobOrderCancelHeaderId FROM web.JobOrderCancelHeaders C WHERE C.DocTypeId =" + JobOrderCancelDocTypeId + " AND C.SiteId = " + SiteId + " AND C.DivisionId =" + DivisionId + "  AND C.DocNo = '" + JobOrderCancelDocNo + "' ";
                        cmd = new SqlCommand(mQry, Con);
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        JobOrderCancelHeaderId = (int)cmd.ExecuteScalar();








                        // *****************  For Insert in Web.CostCenters
                        int IsCostcenterExist = 0;
                        NewCostcenterNo = FGetNewCostcenterNo(DtJobOrder, I);

                        mQry = "SELECT count(*) FROM web.CostCenters C WHERE C.ReferenceDocTypeId =" + DtJobOrder.Rows[I]["DocTypeId"].ToString() + " AND C.SiteId = " + SiteId + " AND C.DivisionId =" + DivisionId + " AND C.CostCenterName = '" + NewCostcenterNo + "' ";
                        SqlCommand cmd2 = new SqlCommand(mQry, Con);
                        if (cmd2.Connection.State != ConnectionState.Open)
                            cmd2.Connection.Open();
                        IsCostcenterExist = (int)cmd2.ExecuteScalar();

                        if (IsCostcenterExist == 0)
                        {
                            mQry = "INSERT INTO Web.CostCenters (CostCenterName, IsActive, IsSystemDefine, CreatedBy, ModifiedBy, CreatedDate, ModifiedDate, " +
                                    " DivisionId, SiteId, LedgerAccountId, DocTypeId, StartDate, ReferenceDocTypeId, ProcessId, Status) " +
                                    " VALUES ('" + NewCostcenterNo + "', 1,1, '" + User.Identity.Name + "', '" + User.Identity.Name + "', getdate(),getdate(), " +
                                    " " + DivisionId + ", " + SiteId + ", " + DtJobOrder.Rows[I]["LedgerAccountId"].ToString() + ", " + DtJobOrder.Rows[I]["DocTypeId"].ToString() + ", dateadd(Day,1, '" + CloseDate + "'), " + DtJobOrder.Rows[I]["DocTypeId"].ToString() + ", " + DtJobOrder.Rows[I]["ProcessId"].ToString() + ", 1) ";

                            cmd = new SqlCommand(mQry, Con);
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            cmd.ExecuteNonQuery();
                        }





                        // *****************  For Insert in Web.JobOrderHeaders
                        mQry = "SELECT CostcenterId FROM web.CostCenters C WHERE C.ReferenceDocTypeId =" + DtJobOrder.Rows[I]["DocTypeId"].ToString() + " AND C.SiteId = " + SiteId + " AND C.DivisionId =" + DivisionId + "  AND C.CostCenterName = '" + NewCostcenterNo + "' ";
                        cmd = new SqlCommand(mQry, Con);
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        CostCenterId = (int)cmd.ExecuteScalar();
                        JobOrderDocNo = "OL" + DtJobOrder.Rows[I]["DocNo"].ToString();


                        mQry = "INSERT INTO Web.JobOrderHeaders (DocTypeId, DocDate, DocNo, DivisionId, SiteId, DueDate, JobWorkerId, BillToPartyId, OrderById, " +
                                " GodownId, ProcessId, CostCenterId, TermsAndConditions, Status, Remark, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, UnitconversionForId, ActualDueDate) " +
                                " VALUES ( " + DtJobOrder.Rows[I]["DocTypeId"].ToString() + ", dateadd(Day,1, '" + CloseDate + "'), '" + JobOrderDocNo + "', " + DivisionId + ", " + SiteId + ", " +
                                " '" + DtJobOrder.Rows[I]["DueDate"].ToString() + "', " + DtJobOrder.Rows[I]["JobWorkerId"].ToString() + ", " + DtJobOrder.Rows[I]["BillToPartyId"].ToString() + ", " + DtJobOrder.Rows[I]["OrderById"].ToString() + ", " +
                                " " + FGetNullValue(DtJobOrder.Rows[I]["GodownId"].ToString()) + ", " + DtJobOrder.Rows[I]["ProcessId"].ToString() + ", " + CostCenterId + ", ' " + DtJobOrder.Rows[I]["TermsAndConditions"].ToString() + "', 1, ' " + DtJobOrder.Rows[I]["Remark"].ToString() + "', " +
                                "  '" + User.Identity.Name + "', getdate(),  '" + User.Identity.Name + "', getdate(), " + DtJobOrder.Rows[I]["UnitconversionForId"].ToString() + ", '" + DtJobOrder.Rows[I]["ActualDueDate"].ToString() + "')";
                        cmd = new SqlCommand(mQry, Con);
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        cmd.ExecuteNonQuery();


                        mQry = "SELECT JobOrderHeaderId FROM web.JobOrderHeaders C WHERE C.DocTypeId =" + DtJobOrder.Rows[I]["DocTypeId"].ToString() + " AND C.SiteId = " + SiteId + " AND C.DivisionId =" + DivisionId + "  AND C.DocNo = '" + JobOrderDocNo + "' ";
                        cmd = new SqlCommand(mQry, Con);
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        JobOrderHeaderId = (int)cmd.ExecuteScalar();




                        // *****************  For Getting Pending Job OrderLine
                        mQry = " SELECT H.JobOrderHeaderId, H.DocTypeId, H.DocNo, H.DocDate, H.JobWorkerId, H.ProcessId, " +
                                " L.JobOrderLineId, isnull(L.ProductUidId,0) AS ProductUidId, L.ProductId, isnull(L.ProdOrderLineId,0) ProdOrderLineId, L.DealUnitId, L.Rate, L.Amount, L.Remark , " +
                                " L.NonCountedQty, L.LossQty, L.Specification, L.UnitConversionMultiplier, isnull(L.StockId,0) AS StockId, isnull(L.ProductUidHeaderId,0) AS ProductUidHeaderId, L.Sr, " +
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


                        SDA = new SqlDataAdapter(mQry, Con);
                        SDA.SelectCommand.CommandTimeout = 500;
                        SDA.Fill(DtJobOrderLine);


                        if (DtJobOrderLine.Rows.Count > 0)
                        {

                            for (J = 0; J < DtJobOrderLine.Rows.Count; J++)
                            {
                                // *****************  For Insert in Web.JobOrderCancelLines
                                mQry = "INSERT INTO Web.JobOrderCancelLines (JobOrderCancelHeaderId, JobOrderLineId, Qty, CreatedBy, ModifiedBy, CreatedDate, ModifiedDate, " +
                                        " ProductUidId, Sr, ProductUidLastTransactionDocId, ProductUidLastTransactionDocNo, ProductUidLastTransactionDocTypeId, ProductUidLastTransactionDocDate, ProductUidLastTransactionPersonId, ProductUidCurrentProcessId, ProductUidStatus) " +
                                        " VALUES (" + JobOrderCancelHeaderId + ", " + DtJobOrderLine.Rows[J]["JobOrderLineId"].ToString() + ", " + DtJobOrderLine.Rows[J]["BalQty"].ToString() + ", '" + User.Identity.Name + "', '" + User.Identity.Name + "', getdate(),getdate(), " +
                                        "  " + FGetNullValue(DtJobOrderLine.Rows[J]["ProductUidId"].ToString()) + ", 1, " + DtJobOrderLine.Rows[J]["JobOrderHeaderId"].ToString() + ", '" + DtJobOrderLine.Rows[J]["DocNo"].ToString() + "', " + DtJobOrderLine.Rows[J]["DocTypeId"].ToString() + ", '" + DtJobOrderLine.Rows[J]["DocDate"].ToString() + "', '" + DtJobOrderLine.Rows[J]["JobWorkerId"].ToString() + "', '" + DtJobOrderLine.Rows[J]["ProcessId"].ToString() + "', 'Issue') ";
                                cmd = new SqlCommand(mQry, Con);
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();
                                cmd.ExecuteNonQuery();



                                // *****************  For Insert in Web.JobOrderLines
                                mQry = "INSERT INTO Web.JobOrderLines (JobOrderHeaderId, ProductUidId, ProductId, ProdOrderLineId, Qty, DealUnitId, DealQty, Rate, Amount, Remark, " +
                                        " CreatedBy, ModifiedBy, CreatedDate, ModifiedDate, NonCountedQty, LossQty, Specification, UnitConversionMultiplier, StockId, ProductUidHeaderId, Sr,  " +
                                        " ProductUidLastTransactionDocId, ProductUidLastTransactionDocNo, ProductUidLastTransactionDocTypeId, ProductUidLastTransactionDocDate, ProductUidLastTransactionPersonId, ProductUidCurrentGodownId, ProductUidCurrentProcessId, ProductUidStatus) " +
                                        " VALUES (" + JobOrderHeaderId + ", " + FGetNullValue(DtJobOrderLine.Rows[J]["ProductUidId"].ToString()) + ", " + DtJobOrderLine.Rows[J]["ProductId"].ToString() + ", " + FGetNullValue(DtJobOrderLine.Rows[J]["ProdOrderLineId"].ToString()) + ", " + DtJobOrderLine.Rows[J]["BalQty"].ToString() + ", " +
                                        " '" + DtJobOrderLine.Rows[J]["DealUnitId"].ToString() + "', " + DtJobOrderLine.Rows[J]["BalDealQty"].ToString() + ", " + DtJobOrderLine.Rows[J]["Rate"].ToString() + ", " + DtJobOrderLine.Rows[J]["Amount"].ToString() + ", '" + DtJobOrderLine.Rows[J]["Remark"].ToString() + "', " +
                                        " '" + User.Identity.Name + "', '" + User.Identity.Name + "', getdate(),getdate(), " + DtJobOrderLine.Rows[J]["NonCountedQty"].ToString() + ", " + DtJobOrderLine.Rows[J]["LossQty"].ToString() + ", '" + DtJobOrderLine.Rows[J]["Specification"].ToString() + "', " + DtJobOrderLine.Rows[J]["UnitConversionMultiplier"].ToString() + ", " +
                                        " " + FGetNullValue(DtJobOrderLine.Rows[J]["StockId"].ToString()) + ", " + (DtJobOrderLine.Rows[J]["ProductUidHeaderId"].ToString()) + ", " + DtJobOrderLine.Rows[J]["Sr"].ToString() + ", NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL) ";
                                cmd = new SqlCommand(mQry, Con);
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();
                                cmd.ExecuteNonQuery();

                            }
                        }



                        //If DtJobOrderDetail.Rows.Count > 0 Then
                        //    For J = 0 To DtJobOrderDetail.Rows.Count - 1
                        //        mQry = "  INSERT INTO JobOrderDetail(DocId, Sr, " +
                        //                  " Item, JobOrder, JobOrderSr, ProdOrder, ProdOrderSr, Qty, Unit, MeasurePerPcs, TotalMeasure, " +
                        //                  " MeasureUnit, PerimeterPerPcs, TotalPerimeter, " +
                        //                  " Rate, Amount, IncentivePer, Remark) " +
                        //                  " Values(" & AgL.Chk_Text(JobOrderCancelDocId) & ", " +
                        //                  " " & Val(AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr"))) & ", " +
                        //                  " " & AgL.Chk_Text(AgL.XNull(DtJobOrderDetail.Rows(J)("Item"))) & ", " +
                        //                  " " & AgL.Chk_Text(AgL.XNull(DtJobOrderDetail.Rows(J)("JobOrder"))) & ", " +
                        //                  " " & Val(AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr"))) & ", " +
                        //                  " " & AgL.Chk_Text(AgL.XNull(DtJobOrderDetail.Rows(J)("ProdOrder"))) & ", " +
                        //                  " " & Val(AgL.VNull(DtJobOrderDetail.Rows(J)("ProdOrderSr"))) & ", " +
                        //                  " " & -Val(AgL.XNull(DtJobOrderDetail.Rows(J)("Qty"))) & ", " +
                        //                  " " & AgL.Chk_Text(AgL.XNull(DtJobOrderDetail.Rows(J)("Unit"))) & ", " +
                        //                  " " & Val(AgL.XNull(DtJobOrderDetail.Rows(J)("MeasurePerPcs"))) & ", " +
                        //                  " " & -Val(AgL.XNull(DtJobOrderDetail.Rows(J)("TotalMeasure"))) & ", " +
                        //                  " " & AgL.Chk_Text(AgL.XNull(DtJobOrderDetail.Rows(J)("MeasureUnit"))) & ", " +
                        //                  " " & Val(AgL.XNull(DtJobOrderDetail.Rows(J)("PerimeterPerPcs"))) & ", " +
                        //                  " " & -Val(AgL.XNull(DtJobOrderDetail.Rows(J)("TotalPerimeter"))) & ", " +
                        //                  " " & Val(AgL.XNull(DtJobOrderDetail.Rows(J)("Rate"))) & ", " +
                        //                  " " & -Val(AgL.XNull(DtJobOrderDetail.Rows(J)("Amount"))) & ", " +
                        //                  " " & -Val(AgL.VNull(DtJobOrderDetail.Rows(J)("IncentivePer"))) & ", " +
                        //                  " " & AgL.Chk_Text(AgL.XNull(DtJobOrderDetail.Rows(J)("Remark"))) & ") "
                        //        AgL.Dman_ExecuteNonQry(mQry, AgL.GCn, AgL.ECmd)

                        //        FPostConsumption(JobOrderCancelDocId, AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr")), _
                        //                         AgL.XNull(DtJobOrderDetail.Rows(J)("JobOrder")), _
                        //                         AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr")), _
                        //                         DtJobOrderDetail, J, True, AgL.GCn, AgL.ECmd)

                        //        mQry = "  INSERT INTO JobOrderDetail(DocId, Sr, " +
                        //                " JobWorkerRateGroup, Item, FromProcess, CurrStock, Qty, Unit, MeasurePerPcs, TotalMeasure, MeasureUnit, " +
                        //                " ProdPlan, ProdOrder, ProdOrderSr, LossPer, Loss, Rate, Amount, IncentivePer, " +
                        //                " Bom, Quality, JobOrder, JobOrderSr) " +
                        //                " VALUES (" & AgL.Chk_Text(JobOrderDocId) & ", 	" +
                        //                " " & Val(AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr"))) & ", " +
                        //                " " & AgL.Chk_Text(DtJobOrderDetail.Rows(J)("JobWorkerRateGroup")) & ", " +
                        //                " " & AgL.Chk_Text(DtJobOrderDetail.Rows(J)("Item")) & ", " +
                        //                " " & AgL.Chk_Text(DtJobOrderDetail.Rows(J)("FromProcess")) & ", " +
                        //                " " & Val(DtJobOrderDetail.Rows(J)("CurrStock")) & ", " +
                        //                " " & Val(DtJobOrderDetail.Rows(J)("Qty")) & ", " +
                        //                " " & AgL.Chk_Text(DtJobOrderDetail.Rows(J)("Unit")) & ", " +
                        //                " " & Val(DtJobOrderDetail.Rows(J)("MeasurePerPcs")) & ", " +
                        //                " " & Val(DtJobOrderDetail.Rows(J)("TotalMeasure")) & ", " +
                        //                " " & AgL.Chk_Text(DtJobOrderDetail.Rows(J)("MeasureUnit")) & ", " +
                        //                " " & AgL.Chk_Text(DtJobOrderDetail.Rows(J)("ProdPlan")) & ", " +
                        //                " " & AgL.Chk_Text(DtJobOrderDetail.Rows(J)("ProdOrder")) & ", " +
                        //                " " & Val(AgL.VNull(DtJobOrderDetail.Rows(J)("ProdOrderSr"))) & ", " +
                        //                " " & Val(DtJobOrderDetail.Rows(J)("LossPer")) & ", " +
                        //                " " & Val(DtJobOrderDetail.Rows(J)("Loss")) & ", " +
                        //                " " & Val(DtJobOrderDetail.Rows(J)("Rate")) & ", " +
                        //                " " & Val(DtJobOrderDetail.Rows(J)("Amount")) & ", " +
                        //                " " & AgL.VNull(DtJobOrderDetail.Rows(J)("IncentivePer")) & ", " +
                        //                " " & AgL.Chk_Text(DtJobOrderDetail.Rows(J)("Bom")) & ", " +
                        //                " " & AgL.Chk_Text(DtJobOrderDetail.Rows(J)("Quality")) & ", " +
                        //                " " & AgL.Chk_Text(JobOrderDocId) & ", " +
                        //                " " & Val(AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr"))) & ") "
                        //        AgL.Dman_ExecuteNonQry(mQry, AgL.GCn, AgL.ECmd)

                        //        FPostConsumption(JobOrderDocId, AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr")), _
                        //                         JobOrderDocId, AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr")), _
                        //                         DtJobOrderDetail, J, False, AgL.GCn, AgL.ECmd)


                        //If Process is running for main branch
                        //then for transfering bar codes to non sister concern job workers
                        //Update item_uid genDOcId and GenSr with new order DocId.
                        //If Job Worker is sister concern site then BarCodes will cancel from that order
                        //and will transfer to new created job orders.

                        //If AgL.PubSiteCode = "1" Then
                        //Dim bSisterConcernSite$ = ""
                        //If AgL.Dman_Execute("SELECT IsNull(S.SisterConcernYn,0) FROM SubGroup S With (NoLock) WHERE S.SubCode =//" & AgL.XNull(DtJobOrder.Rows(I)("JobWorker")) & "//", AgL.GcnRead).ExecuteScalar Then
                        //    bSisterConcernSite = AgL.FillData("SELECT IsNull(S.SisterConcernSite,////)  FROM SubGroup S With (NoLock) WHERE S.SubCode =//" & AgL.XNull(DtJobOrder.Rows(I)("JobWorker")) & "//", AgL.GcnRead).tables(0).rows(0)(0)

                        //    mQry = " INSERT INTO JobIssRecUID(DocID, TSr, Sr, IssRec, Process, Item, Item_UID, Site_Code, " +
                        //             " V_Date, V_Type, SubCode, Div_Code, RecId, EntryDate, JobRecDocId) " +
                        //             " Select " & AgL.Chk_Text(JobOrderCancelDocId) & ", I.GenSr, I.Sr, //R//, " +
                        //             " " & AgL.Chk_Text(ClsMain.Temp_NCat.WeavingOrder) & ", " +
                        //             " I.Item, I.Code, " & AgL.Chk_Text(bSisterConcernSite) & ", " +
                        //             " " & AgL.Chk_Text(JobOrderCancelV_Date) & ", " & AgL.Chk_Text(JobOrderCancelV_Type) & ", " +
                        //             " " & AgL.Chk_Text(AgL.XNull(DtJobOrder.Rows(I)("JobWorker"))) & ", " +
                        //             " " & AgL.Chk_Text(AgL.PubDivCode) & ", " +
                        //             " " & AgL.Chk_Text(JobOrderCancelManualRefNo) & ", GetDate(), " +
                        //             " " & AgL.Chk_Text(AgL.XNull(DtJobOrderDetail.Rows(J)("JobOrder"))) & " " +
                        //             " From Item_Uid I With (NoLock) " +
                        //             " Where I.GenDocId = //" & AgL.XNull(DtJobOrderDetail.Rows(J)("JobOrder")) & "// " +
                        //             " And I.GenSr = " & AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr")) & " " +
                        //             " And I.RecDocID Is Null " +
                        //             " And I.CancelDocId IS NULL "
                        //    AgL.Dman_ExecuteNonQry(mQry, AgL.GCn, AgL.ECmd)

                        //    mQry = " Update JobIssRecUID " +
                        //             " SET JobRecDocId = " & AgL.Chk_Text(JobOrderCancelDocId) & " " +
                        //             " Where DocId = //" & AgL.XNull(DtJobOrderDetail.Rows(J)("JobOrder")) & "// " +
                        //             " And Sr = " & AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr")) & " " +
                        //             " And JobRecDocId Is Null "
                        //    AgL.Dman_ExecuteNonQry(mQry, AgL.GCn, AgL.ECmd)


                        ////    mQry = " INSERT INTO JobIssRecUID(DocID, TSr, Sr, IssRec, Process, Item, Item_UID, Site_Code, " +
                        ////             " V_Date, V_Type, SubCode, Div_Code, RecId, EntryDate) " +
                        ////             " Select " & AgL.Chk_Text(JobOrderDocId) & ", I.GenSr, I.Sr, //I//, " +
                        ////             " " & AgL.Chk_Text(ClsMain.Temp_NCat.WeavingOrder) & ", " +
                        ////             " I.Item, I.Code, " & AgL.Chk_Text(bSisterConcernSite) & ", " +
                        ////             " " & AgL.Chk_Text(JobOrderCancelV_Date) & ", " & AgL.Chk_Text(JobOrderCancelV_Type) & ", " +
                        ////             " " & AgL.Chk_Text(AgL.XNull(DtJobOrder.Rows(I)("JobWorker"))) & ", " +
                        ////             " " & AgL.Chk_Text(AgL.PubDivCode) & ", " +
                        ////             " " & AgL.Chk_Text(JobOrderCancelManualRefNo) & ", GetDate() " +
                        ////             " From Item_Uid I With (NoLock) " +
                        ////             " Where I.GenDocId = //" & AgL.XNull(DtJobOrderDetail.Rows(J)("JobOrder")) & "// " +
                        ////             " And I.GenSr = " & AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr")) & " " +
                        ////             " And I.RecDocID Is Null " +
                        ////             " And I.CancelDocId IS NULL "
                        ////    AgL.Dman_ExecuteNonQry(mQry, AgL.GCn, AgL.ECmd)
                        ////End If

                        //mQry = " UPDATE Item_Uid " +
                        //       " Set " +
                        //       " GenDocId = //" & JobOrderDocId & "//, " +
                        //       " GenSr = //" & AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr")) & "//, " +
                        //       " TransferedFrom = //" & AgL.XNull(DtJobOrder.Rows(I)("DocId")) & "//  " +
                        //       " Where GenDocId = //" & AgL.XNull(DtJobOrderDetail.Rows(J)("JobOrder")) & "// " +
                        //       " And GenSr = " & AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr")) & " " +
                        //       " And RecDocID Is Null " +
                        //       " And CancelDocId IS NULL "
                        //AgL.Dman_ExecuteNonQry(mQry, AgL.GCn, AgL.ECmd)

                        // Else
                        //If process is running for branch
                        //it will cancel barcodes from old orders and trasnfer barcodes to new orders

                        //mQry = " INSERT INTO JobIssRecUID(DocID, TSr, Sr, IssRec, Process, Item, Item_UID, Site_Code, " +
                        //         " V_Date, V_Type, SubCode, Div_Code, RecId, EntryDate, JobRecDocId) " +
                        //         " Select " & AgL.Chk_Text(JobOrderCancelDocId) & ", J.TSr, J.Sr, //R//, " +
                        //         " " & AgL.Chk_Text(ClsMain.Temp_NCat.WeavingOrder) & ", " +
                        //         " J.Item, J.Item_UID, " & AgL.Chk_Text(AgL.PubSiteCode) & ", " +
                        //         " " & AgL.Chk_Text(JobOrderCancelV_Date) & ", " & AgL.Chk_Text(JobOrderCancelV_Type) & ", " +
                        //         " " & AgL.Chk_Text(AgL.XNull(DtJobOrder.Rows(I)("JobWorker"))) & ", " +
                        //         " " & AgL.Chk_Text(AgL.PubDivCode) & ", " +
                        //         " " & AgL.Chk_Text(JobOrderCancelManualRefNo) & ", GetDate(), " +
                        //         " " & AgL.Chk_Text(AgL.XNull(DtJobOrderDetail.Rows(J)("JobOrder"))) & " " +
                        //         " From JobIssRecUid J " +
                        //         " Where J.DocId = //" & AgL.XNull(DtJobOrderDetail.Rows(J)("JobOrder")) & "// " +
                        //         " And J.TSr = " & AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr")) & " " +
                        //         " And J.JobRecDocId Is Null "
                        //AgL.Dman_ExecuteNonQry(mQry, AgL.GCn, AgL.ECmd)

                        //mQry = " INSERT INTO JobIssRecUID(DocID, TSr, Sr, IssRec, Process, Item, Item_UID, Site_Code, " +
                        //      " V_Date, V_Type, SubCode, Div_Code, RecId, EntryDate) " +
                        //      " Select " & AgL.Chk_Text(JobOrderDocId) & ", J.TSr, J.Sr, //I//, " +
                        //      " " & AgL.Chk_Text(ClsMain.Temp_NCat.WeavingOrder) & ", " +
                        //      " J.Item, J.Item_UID, " & AgL.Chk_Text(AgL.PubSiteCode) & ", " +
                        //      " " & AgL.Chk_Text(JobOrderV_Date) & ", " & AgL.Chk_Text(JobOrderV_Type) & ", " +
                        //      " " & AgL.Chk_Text(AgL.XNull(DtJobOrder.Rows(I)("JobWorker"))) & ", " +
                        //      " " & AgL.Chk_Text(AgL.PubDivCode) & ", " +
                        //      " " & AgL.Chk_Text(JobOrderManualRefNo) & ", GetDate() " +
                        //      " From JobIssRecUid J " +
                        //      " Where J.DocId = //" & AgL.XNull(DtJobOrderDetail.Rows(J)("JobOrder")) & "// " +
                        //      " And J.TSr  = " & AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr")) & "" +
                        //      " And J.JobRecDocId Is Null "
                        //AgL.Dman_ExecuteNonQry(mQry, AgL.GCn, AgL.ECmd)

                        //mQry = " Update JobIssRecUID " +
                        //         " SET JobRecDocId = " & AgL.Chk_Text(JobOrderCancelDocId) & " " +
                        //         " Where DocId = //" & AgL.XNull(DtJobOrderDetail.Rows(J)("JobOrder")) & "// " +
                        //         " And TSr  = " & AgL.VNull(DtJobOrderDetail.Rows(J)("JobOrderSr")) & " " +
                        //         " And JobRecDocId Is Null "
                        //AgL.Dman_ExecuteNonQry(mQry, AgL.GCn, AgL.ECmd)
                    }
                }
                //}
                //FSetHeaderTotals(JobOrderCancelDocId)
                //FSetHeaderTotals(JobOrderDocId)

                //mQry = " INSERT INTO JobOrderQCInstruction (DocId, Sr, Parameter, StdValue) " +
                //       " Select //" & JobOrderDocId & "//, Sr, Parameter, StdValue From JobOrderQCInstruction Where DocId = //" & AgL.XNull(DtJobOrder.Rows(I)("DocId")) & "// "
                //AgL.Dman_ExecuteNonQry(mQry, AgL.GCn, AgL.ECmd)

                //If AgL.PubSiteCode = "1" Then
                //    FTransferMaterial(JobOrderDocId, AgL.XNull(DtJobOrder.Rows(I)("DocId")), AgL.XNull(DtJobOrder.Rows(I)("Div_Code")), AgL.XNull(DtJobOrder.Rows(I)("Site_Code")))
                //    mQry = " Select IsNull(Sg.SisterConcernYn,0) As SisterConcernYn From SubGroup Sg Where Sg.SubCode = //" & AgL.XNull(DtJobOrder.Rows(I)("JobWorker")) & "// "
                //    If AgL.VNull(AgL.Dman_Execute(mQry, AgL.GcnRead).ExecuteScalar) <> 0 Then
                //        FCreateProdOrderCancelAtSisterConcernSite(JobOrderCancelDocId, AgL.XNull(DtJobOrder.Rows(I)("JobWorker")), _
                //                                                   AgL.GCn, AgL.ECmd)

                //        FCreateProdOrderAtSisterConcernSite(JobOrderDocId, AgL.XNull(DtJobOrder.Rows(I)("JobWorker")), _
                //                                JobOrderManualRefNo, JobOrderV_No, JobOrderV_Type, _
                //                                AgL.GCn, AgL.ECmd)

                //    End If
                //End If

                //DtJobOrderDetail.Dispose()


                //}

                //mQry = "INSERT INTO StockVirtual (DocID, Sr, V_Type, V_Prefix, V_Date, V_No, Div_Code, Site_Code, SubCode, Item, Qty_Iss, Qty_Rec, Unit, MeasurePerPcs, Measure_Iss, Measure_Rec, MeasureUnit, Rate, Amount) " +
                //       "SELECT L.DocID, Sr, V_Type, V_Prefix, V_Date, V_No, Div_Code, Site_Code, H.JobWorker SubCode, Item, 0 Qty_Iss, L.Qty Qty_Rec, Unit, 0 MeasurePerPcs, 0 Measure_Iss, L.TotalMeasure Measure_Rec, MeasureUnit, L.Rate Rate, Amount " +
                //       "FROM dbo.JobOrderDetail L With (NoLock) " +
                //       "LEFT JOIN JobOrder H  With (NoLock) ON H.DocID = L.DocId  " +
                //       "WHERE L.DocId LIKE //%YE15%// And H.Div_Code = //" & AgL.PubDivCode & "// and H.Site_Code =//" & AgL.PubSiteCode & "// "
                //AgL.Dman_ExecuteNonQry(mQry, AgL.GCn, AgL.ECmd)
                //}

                //AgL.ETrans.Commit()
                mTrans = "Commit";

                IsSuccess = true;
            }

            catch (Exception ex)
            {
                if (mTrans == "Begin")
                {
                    //AgL.ETrans.Rollback()
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
            if (CloseDate != null)
            {
                IsSuccess = FPostJobOrderCancel(CloseDate);
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


        private string FGetNewCostcenterNo(DataTable DtJobOrder, int Row)
        {
            string NewPurja = "";

            Con = new SqlConnection(connectionString);
            SqlCommand cmd;
            string bConstruction = "";

            object ObjVar = "";
            long bStartFrom = 1;
            long MaxPurjaNo = 0;
            string RunningPurja = "";
            string strCondConstruction = "";


            //mQry = " SELECT TOP 1 IsNull(Convert(BIGINT,replace(C.CostCenterName,'OL16-','')),0)  " +
            //        " FROM web.CostCenters C WITH (Nolock)  " +
            //        " WHERE C.ReferenceDocTypeId =" + DtJobOrder.Rows[Row]["DocTypeId"].ToString() + " AND C.SiteId =" + DtJobOrder.Rows[Row]["SiteId"].ToString() + " AND C.DivisionId = " + DtJobOrder.Rows[Row]["DivisionId"].ToString() + " AND  C.CostCenterName LIKE '%OL16-%' " +
            //        " AND IsNumeric(replace(C.CostCenterName,'OL16-','')) = 1  " +
            //        " ORDER BY Convert(BIGINT,replace(C.CostCenterName,'OL16-','')) DESC ";
            // cmd= new SqlCommand(mQry, Con);
            //cmd.Connection.Open();
            //ObjMaxPurjaNo = cmd.ExecuteScalar();
            //if (ObjMaxPurjaNo != null)
            //    MaxPurjaNo = (long)ObjMaxPurjaNo;
            //else
            //    MaxPurjaNo = 0;


            //if (MaxPurjaNo == 0)
            //    NewPurja = "OL16-" + bStartFrom.ToString().PadLeft(4, '0').ToString();
            //else
            //    NewPurja = "OL16-" + (MaxPurjaNo + 1).ToString().PadLeft(4, '0').ToString();


            mQry = "SELECT C.CostCenterName " +
                " FROM web.JobOrderJobOrders JJ WITH (Nolock) " +
                " LEFT JOIN web.JobOrderHeaders JO WITH (Nolock) ON JO.JobOrderHeaderId = JJ.JobOrderHeaderId  " +
                " LEFT JOIN web.JobOrderHeaders GJO WITH (Nolock) ON GJO.JobOrderHeaderId = JJ.GenJobOrderHeaderId   " +
                " LEFT JOIN web.CostCenters C ON C.CostCenterId = JO.JobOrderHeaderId  " +
                " WHERE JO.SiteId =" + DtJobOrder.Rows[Row]["SiteId"].ToString() + " AND JO.DivisionId =" + DtJobOrder.Rows[Row]["DivisionId"].ToString() + " " +
                " AND JO.DocDate  BETWEEN DateAdd(Year,1,'01/Apr/2015') AND  DateAdd(Year,1,'31/Mar/2016') " +
                " AND C.CostCenterName = '" + DtJobOrder.Rows[Row]["CostCenterName"].ToString() + "' ";

            cmd = new SqlCommand(mQry, Con);
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
            ObjVar = cmd.ExecuteScalar();

            if (ObjVar != null)
                RunningPurja = (string)ObjVar;
            else
                RunningPurja = "";






            if (RunningPurja == "")
            {

                mQry = "SELECT PC.ProductCategoryName FROM web.FinishedProduct FP WITH (Nolock) " +
                        " LEFT JOIN web.ProductCategories PC WITH (Nolock) ON PC.ProductCategoryId = FP.ProductCategoryId " +
                        " WHERE FP.ProductId = " + DtJobOrder.Rows[Row]["ProductId"].ToString() + " ";

                cmd = new SqlCommand(mQry, Con);
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                ObjVar = cmd.ExecuteScalar();

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
                    


                mQry = "SELECT IsNull(Max(Convert(BIGINT,replace(C.CostCenterName,'OL16-',''))),0) " +
                         " FROM   " +
                         " (  " +
                         "      SELECT H.*  " +
                         "      FROM web.JobOrderHeaders H WITH (Nolock)   " +
                         "      LEFT JOIN web.DocumentTypes DT ON DT.DocumentTypeId = H.DocTypeId  " +
                         "      WHERE DT.DocumentCategoryId  =343 AND  H.SiteId =" + DtJobOrder.Rows[Row]["SiteId"].ToString() + " AND H.DivisionId =" + DtJobOrder.Rows[Row]["DivisionId"].ToString() + " " +
                         "      AND H.DocDate  BETWEEN DateAdd(Year,1,'01/Apr/2015') AND  DateAdd(Year,1,'31/Mar/2016') " +
                         " ) H " +
                         " LEFT JOIN web.CostCenters C WITH (Nolock) ON C.CostCenterId = H.CostCenterId  " +
                         " LEFT JOIN web.People JW WITH (Nolock) ON JW.PersonID = H.JobWorkerId  " +
                         " LEFT JOIN web.JobOrderLines L WITH (Nolock) ON L.JobOrderHeaderId = H.JobOrderHeaderId  " +
                         " LEFT JOIN web.FinishedProduct  FP WITH (Nolock) ON FP.ProductId = L.ProductId  " +
                         " LEFT JOIN web.ProductCategories PC WITH (Nolock) ON PC.ProductCategoryId = FP.ProductCategoryId " +
                         " WHERE 1=1 AND  IsNumeric(replace(replace(C.CostCenterName,'-',''),'OL','')) = 1 " + strCondConstruction;

                cmd = new SqlCommand(mQry, Con);
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                ObjVar = cmd.ExecuteScalar();

                if (ObjVar != null)
                    MaxPurjaNo = (long)ObjVar;
                else
                    MaxPurjaNo = 0;



                if (MaxPurjaNo == 0)
                    NewPurja = "OL16-" + bStartFrom.ToString().PadLeft(4, '0').ToString();
                else
                    NewPurja = "OL16-" + (MaxPurjaNo + 1).ToString().PadLeft(4, '0').ToString();

            }
            else
                NewPurja = RunningPurja;

            return NewPurja;
        }

        private void FPostConsumption(SqlCommand Cmd)
        {
            //Dim J As Integer = 0
            //Dim bQry$ = ""
            //Dim DsTemp As DataSet = Nothing
            //Dim bTempTable$ = ""
            //Dim DtTemp As DataTable = Nothing
            //Dim BomCode$ = ""
            //Dim bTotalMeasure As Double = 0

            //mQry = "Select Code From BOM With (Nolock) Where Design = (Select Design From Rug_CarpetSKU Where Code = '" & AgL.XNull(DtJobOrderDetail.Rows(mRow)("Item")) & "')"
            //DtTemp = AgL.FillData(mQry, AgL.GcnRead).Tables(0)

            //If DtTemp.Rows.Count > 0 Then
            //    BomCode = DtTemp.Rows(0)("Code")
            //Else
            //    Err.Raise(1, , "Consumption Detail not found for Carpet SKU #" & DtJobOrderDetail.Rows(mRow)("Item"))
            //End If

            //If OrderCancel = True Then
            //    bTotalMeasure = -AgL.VNull(DtJobOrderDetail.Rows(mRow)("TotalMeasure"))
            //Else
            //    bTotalMeasure = AgL.VNull(DtJobOrderDetail.Rows(mRow)("TotalMeasure"))
            //End If

            //mQry = "INSERT INTO JobOrderBOM(DocId, TSr, Sr, Item, Qty, Unit, JobOrder, JobOrderSr) " & _
            //        " SELECT '" & SearchCode & "', " & TSr & ", row_number() Over (Order By Bd.Item),  Bd.Item, " & _
            //        " Bd.Qty * " & Val(bTotalMeasure) & " As BomQty, I.Unit, " & _
            //        " '" & JobOrder & "', " & JobOrderSr & " " & _
            //        " FROM Bom B  " & _
            //        " LEFT JOIN BomDetail Bd ON B.Code = Bd.Code " & _
            //        " LEFT JOIN Item I ON Bd.Item = I.Code " & _
            //        " WHERE B.Code = '" & BomCode & "'"
            //AgL.Dman_ExecuteNonQry(mQry, Conn, Cmd)
        }


        



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
