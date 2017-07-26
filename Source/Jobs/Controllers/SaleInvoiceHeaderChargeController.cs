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
using System.Data;
using System.Xml.Linq;
using Model.DatabaseViews;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Presentation.Helper;
using System.Text;
using Model.ViewModels;
using System.Web.Script.Serialization;

namespace Web
{
    [Authorize]
    public class SaleInvoiceHeaderChargeController : System.Web.Mvc.Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        protected string connectionString = (string)System.Web.HttpContext.Current.Session["DefaultConnectionString"];
        ISaleInvoiceHeaderChargeService _SaleInvoiceHeaderChargeService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        public SaleInvoiceHeaderChargeController(ISaleInvoiceHeaderChargeService SaleInvoiceHeaderChargeService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _SaleInvoiceHeaderChargeService = SaleInvoiceHeaderChargeService;
            _exception = exec;
            _unitOfWork = unitOfWork;
        }

        //public ActionResult SaleInvoiceCharge(FormCollection form)
        //{
        //    var SubReportDataList = new List<DataTable>();
        //    var SubReportNameList = new List<string>();
        //    DataTable ReportData = new DataTable();
        //    Dictionary<string, string> ReportFilters = new Dictionary<string, string>();
        //    StringBuilder queryString = new StringBuilder();

        //    string ReportHeaderId = (form["ReportHeaderId"].ToString());

        //    ReportHeader header = new ReportHeaderService(_unitOfWork).GetReportHeader(Convert.ToInt32(ReportHeaderId));
        //    List<ReportLine> lines = new ReportLineService(_unitOfWork).GetReportLineList(header.ReportHeaderId).ToList();

        //    //List<string> SubReportProcList = new ReportHeaderService(_unitOfWork).GetSubReportProcList (Convert.ToInt32(ReportHeaderId));
        //    List<string> SubReportProcList = new List<string>();

        //    ApplicationDbContext Db = new ApplicationDbContext();
        //    queryString.Append(db.strSchemaName + "." + header.SqlProc);


        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand(queryString.ToString(), sqlConnection);
        //        int DocumenttypeId = 634;
        //        foreach (var item in lines)
        //        {


        //            if (item.SqlParameter != "" && item.SqlParameter != null)
        //            {
        //                if (item.SqlParameter == "@LoginSite" || item.SqlParameter == "@LoginDivision")
        //                {
        //                    if (item.SqlParameter == "@LoginSite")
        //                        cmd.Parameters.AddWithValue(item.SqlParameter, (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginSiteId]);
        //                    //cmd.Parameters.AddWithValue(item.SqlParameter, 17);
        //                    else if (item.SqlParameter == "@LoginDivision")
        //                        cmd.Parameters.AddWithValue(item.SqlParameter, (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginDivisionId]);

        //                }
        //                else if (item.FieldName == "Site" && form[item.FieldName].ToString() == "")
        //                {
        //                    cmd.Parameters.AddWithValue(item.SqlParameter, (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginSiteId]);
        //                }

        //                else if (item.FieldName == "Division" && form[item.FieldName].ToString() == "")
        //                {

        //                    cmd.Parameters.AddWithValue(item.SqlParameter, (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginDivisionId]);
        //                }

        //                else
        //                {
        //                    if (form[item.FieldName].ToString() != "")
        //                    {
        //                        if (item.DataType == "Date")
        //                        {
        //                            cmd.Parameters.AddWithValue(item.SqlParameter, (form[item.FieldName].ToString() != "" ? String.Format("{0:MMMM dd yyyy}", form[item.FieldName].ToString()) : "Null"));

        //                        }
        //                        else
        //                        {
        //                            cmd.Parameters.AddWithValue(item.SqlParameter, (form[item.FieldName].ToString() != "" ? form[item.FieldName].ToString() : "Null"));

        //                        }
        //                    }
        //                }

        //            }
        //        }
        //        cmd.Parameters.AddWithValue("@SiteId", (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginSiteId]);
        //        cmd.Parameters.AddWithValue("@DivisionId", (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginDivisionId]);

        //        cmd.Parameters.AddWithValue("@DocumenttypeId", DocumenttypeId);
        //        cmd.CommandTimeout = 200;
        //        SqlDataAdapter sqlDataAapter = new SqlDataAdapter(cmd);
        //        sqlDataAapter.SelectCommand.CommandType = CommandType.StoredProcedure;
        //        // dsRep.EnforceConstraints = false;
        //        sqlDataAapter.Fill(ReportData);
        //    }

        //    if (ReportData.Rows.Count > 0)
        //    {   
        //        var list = new List<dynamic>();

        //        foreach (DataRow row in ReportData.Rows)
        //        {
        //            dynamic dyn = new ExpandoObject();
        //            list.Add(dyn);
        //            foreach (DataColumn column in ReportData.Columns)
        //            {
        //                var dic = (IDictionary<string, object>)dyn;
        //                dic[column.ColumnName] = row[column];
        //            }
        //        }
        //       return Json(new { Success = true, Data = list.ToList() }, JsonRequestBehavior.AllowGet);
        //        //TempData["TempCustomer"] = list.ToList();
        //      // return View(list.ToList());
        //        //return View("SaleInvoiceHeaderCharges");
        //    }

        //    else
        //    {
        //        ViewBag.Message = "No Record to Print.";
        //        return View("Close");
        //    }

        //   // return View("SaleInvoiceHeaderCharges");
        //}

        //[HttpPost]
        //[MultipleButton(Name = "Print", Argument = "PDF")]
        //public ActionResult PrintToPDF(FormCollection form)
        //{
        //    return SaleInvoiceCharge(form);
        //}

        //public ActionResult ReportPrint(FormCollection form)
        //{
        //    var SubReportDataList = new List<DataTable>();
        //    var SubReportNameList = new List<string>();
        //    DataTable ReportData = new DataTable();
        //    Dictionary<string, string> ReportFilters = new Dictionary<string, string>();
        //    StringBuilder queryString = new StringBuilder();

        //    string ReportHeaderId = (form["ReportHeaderId"].ToString());

        //    ReportHeader header = new ReportHeaderService(_unitOfWork).GetReportHeader(Convert.ToInt32(ReportHeaderId));
        //    List<ReportLine> lines =new ReportLineService(_unitOfWork).GetReportLineList(header.ReportHeaderId).ToList();

        //        //List<string> SubReportProcList = new ReportHeaderService(_unitOfWork).GetSubReportProcList (Convert.ToInt32(ReportHeaderId));
        //        List<string> SubReportProcList = new List<string>();

        //        ApplicationDbContext Db = new ApplicationDbContext();
        //        queryString.Append(db.strSchemaName + "." + header.SqlProc);


        //        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //        {
        //            SqlCommand cmd = new SqlCommand(queryString.ToString(), sqlConnection);
        //        int DocumenttypeId = 634;
        //        foreach (var item in lines)
        //            {


        //                if (item.SqlParameter != "" && item.SqlParameter != null)
        //                {
        //                    if (item.SqlParameter == "@LoginSite" || item.SqlParameter == "@LoginDivision")
        //                    {
        //                        if (item.SqlParameter == "@LoginSite")
        //                            cmd.Parameters.AddWithValue(item.SqlParameter, (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginSiteId]);
        //                        //cmd.Parameters.AddWithValue(item.SqlParameter, 17);
        //                        else if (item.SqlParameter == "@LoginDivision")
        //                            cmd.Parameters.AddWithValue(item.SqlParameter, (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginDivisionId]);

        //                    }
        //                    else if (item.FieldName == "Site" && form[item.FieldName].ToString() == "")
        //                    {
        //                        cmd.Parameters.AddWithValue(item.SqlParameter, (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginSiteId]);

        //                    }

        //                    else if (item.FieldName == "Division" && form[item.FieldName].ToString() == "")
        //                    {

        //                        cmd.Parameters.AddWithValue(item.SqlParameter, (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginDivisionId]);
        //                    }

        //                    else
        //                    {
        //                        if (form[item.FieldName].ToString() != "")
        //                        {
        //                            if (item.DataType == "Date")
        //                            {
        //                                cmd.Parameters.AddWithValue(item.SqlParameter, (form[item.FieldName].ToString() != "" ? String.Format("{0:MMMM dd yyyy}", form[item.FieldName].ToString()) : "Null"));

        //                            }
        //                            else
        //                            {
        //                                cmd.Parameters.AddWithValue(item.SqlParameter, (form[item.FieldName].ToString() != "" ? form[item.FieldName].ToString() : "Null"));

        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //        cmd.Parameters.AddWithValue("@SiteId", (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginSiteId]);
        //        cmd.Parameters.AddWithValue("@DivisionId", (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginDivisionId]);
               
        //        cmd.Parameters.AddWithValue("@DocumenttypeId", DocumenttypeId);
        //        cmd.CommandTimeout = 200;
        //            SqlDataAdapter sqlDataAapter = new SqlDataAdapter(cmd);
        //            sqlDataAapter.SelectCommand.CommandType = CommandType.StoredProcedure;
        //           // dsRep.EnforceConstraints = false;
        //            sqlDataAapter.Fill(ReportData);
        //        }

        //        if (ReportData.Rows.Count > 0)
        //        {

        //        //    var Paralist = new List<string>();

        //        //foreach (var item in lines)
        //        //{
        //        //    if (item.SqlParameter == "@LoginSite" || item.SqlParameter == "@LoginDivision")
        //        //    {
        //        //    }
        //        //    else
        //        //    {
        //        //        if (item.SqlParameter != "" && item.SqlParameter != null && form[item.FieldName].ToString() != "")
        //        //        {
        //        //            if (item.DataType == "Date")
        //        //            {
        //        //                if (!string.IsNullOrEmpty(form[item.FieldName].ToString())) { Paralist.Add(item.DisplayName + " : " + form[item.FieldName].ToString()); ReportFilters.Add(item.DisplayName, form[item.FieldName].ToString()); }
        //        //            }
        //        //            else if (item.DataType == "Single Select")
        //        //            {
        //        //                if (!string.IsNullOrEmpty(item.ListItem))
        //        //                { Paralist.Add(item.DisplayName + " : " + form[item.FieldName].ToString()); }
        //        //                else if (!string.IsNullOrEmpty(form[item.FieldName].ToString())) { Paralist.Add(item.DisplayName + " : " + form[item.FieldName + "Names"].ToString()); ReportFilters.Add(item.DisplayName, form[item.FieldName + "Names"].ToString()); }

        //        //            }
        //        //            //else if (item.DataType == "Constant Value")
        //        //            //{

        //        //            //    //if (!string.IsNullOrEmpty(form[item.FieldName].ToString())) { Paralist.Add(item.DisplayName + " : " + form[item.FieldName + "Names"].ToString()); }
        //        //            //}

        //        //            else if (item.DataType == "Multi Select")
        //        //            {
        //        //                if (form[item.FieldName].ToString() != "") { Paralist.Add(item.DisplayName + " : " + form[item.FieldName + "Names"].ToString()); ReportFilters.Add(item.DisplayName, form[item.FieldName + "Names"].ToString()); }
        //        //            }
        //        //            else
        //        //            {
        //        //                if (form[item.FieldName].ToString() != "") { Paralist.Add(item.DisplayName + " : " + form[item.FieldName].ToString()); ReportFilters.Add(item.DisplayName, form[item.FieldName].ToString()); }
        //        //            }
        //        //        }
        //        //    }
        //        //}

        //        //    string mimtype;
        //        //    ReportGenerateService c = new ReportGenerateService();
        //        //    byte[] BAR;

        //        //    BAR = c.ReportGenerate(ReportData, out mimtype, ReportFileType, Paralist, SubReportDataList, null, SubReportNameList, User.Identity.Name);

        //        //    XElement s = new XElement(CustomStringOp.CleanCode(header.ReportName));
        //        //    XElement Name = new XElement("Filters");
        //        //    foreach (var Rec in ReportFilters)
        //        //    {
        //        //        Name.Add(new XElement(CustomStringOp.CleanCode(Rec.Key), Rec.Value));
        //        //    }
        //        //    s.Add(Name);

        //        //    //LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
        //        //    //{
        //        //    //    DocTypeId = new DocumentTypeService(_unitOfWork).Find(TransactionDoctypeConstants.Report).DocumentTypeId,
        //        //    //    DocId = header.ReportHeaderId,
        //        //    //    ActivityType = (int)ActivityTypeContants.Report,
        //        //    //    xEModifications = s,
        //        //    //}));

        //        //    if (BAR.Length == 1)
        //        //    {
        //        //        ViewBag.Message = "Report Name is not define.";
        //        //        return View("Close");
        //        //    }
        //        //    else if (BAR.Length == 2)
        //        //    {
        //        //        ViewBag.Message = "Report Title is not define.";
        //        //        return View("Close");
        //        //    }
        //        //    else
        //        //    {

        //        //        //if (mimtype != "application/pdf")
        //        //        if (mimtype == "application/vnd.ms-excel")
        //        //            return File(BAR, mimtype, header.ReportName + ".xls");
        //        //        else
        //        //            return File(BAR, mimtype);
        //        //    }
        //        var list = new List<dynamic>();

        //        foreach (DataRow row in ReportData.Rows)
        //        {
        //            dynamic dyn = new ExpandoObject();
        //            list.Add(dyn);
        //            foreach (DataColumn column in ReportData.Columns)
        //            {
        //                var dic = (IDictionary<string, object>)dyn;
        //                dic[column.ColumnName] = row[column];
        //            }
        //        }
        //        return Json(new { Success = true, Data = list.ToList() }, JsonRequestBehavior.AllowGet);
        //        //return View("SaleInvoiceHeaderCharges");

        //    }

        //        else
        //        {
        //            ViewBag.Message = "No Record to Print.";
        //            return View("Close");
        //        }

          
            

        //}

        //public JsonResult SaleInvoiceHeaderChargeList(DateTime FromDate, DateTime ToDate,string MenuName)
        //{
        //    StringBuilder queryString = new StringBuilder();
        //    int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
        //    int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];       


        //    DataSet ds = new DataSet();
        //    ReportHeader header = new ReportHeaderService(_unitOfWork).GetReportHeaderByName(MenuName);
        //    List<ReportLine> lines = new ReportLineService(_unitOfWork).GetReportLineList(header.ReportHeaderId).ToList();
        //    ApplicationDbContext Db = new ApplicationDbContext();
        //    queryString.Append(db.strSchemaName + "." + header.SqlProc);
        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand(queryString.ToString(), sqlConnection);
        //        cmd.Parameters.AddWithValue("@FromDate", FromDate);
        //        cmd.Parameters.AddWithValue("@ToDate", ToDate);
        //        cmd.Parameters.AddWithValue("@SiteId", (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginSiteId]);
        //        cmd.Parameters.AddWithValue("@DivisionId", (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginDivisionId]);
        //        //cmd.Parameters.AddWithValue("@DocumenttypeId", DocumenttypeId);
        //        cmd.CommandTimeout = 200;
        //        SqlDataAdapter sqlDataAapter = new SqlDataAdapter(cmd);
        //        sqlDataAapter.SelectCommand.CommandType = CommandType.StoredProcedure;              
        //        sqlDataAapter.Fill(ds);
        //    }








        //    //SqlParameter SqlParameterSiteId = new SqlParameter("@SiteId", SiteId);
        //    //SqlParameter SqlParameterDivisionId = new SqlParameter("@DivisionId", DivisionId);
        //    //SqlParameter SqlParameterFromDate = new SqlParameter("@FromDate", FromDate);
        //    //SqlParameter SqlParameterToDate = new SqlParameter("@ToDate", ToDate);
        //    //SqlParameter SqlParameterDocumenttypeId = new SqlParameter("@DocumenttypeId", DocumenttypeId);



        //    //IEnumerable<SaleInvoiceHeaderCharges> Tep = db.Database.SqlQuery<SaleInvoiceHeaderCharges>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".spRep_SaleInvoiceHeaderChargeReport @SiteId, @DivisionId, @FromDate, @ToDate,@DocumenttypeId", SqlParameterSiteId, SqlParameterDivisionId, SqlParameterFromDate, SqlParameterToDate, SqlParameterDocumenttypeId).ToList();




        //    //var FormattedDate = Tep.AsEnumerable().Select(m => new
        //    //{
        //    //    DocDate = m.DocDate.ToString("dd/MMM/yyyy"),
        //    //    DocNo = m.DocNo,
        //    //    BillToBuyerName = m.BillToBuyer,
        //    //    Qty = m.Qty,
        //    //    UnitName = m.UnitName,
        //    //    DeliveryQty = m.DeliveryQty,
        //    //    DeliveryUnit = m.DeliveryUnit,
        //    //    NetAmount = m.NetAmount,
        //    //    Remark = m.Remark,
        //    //}).ToList();

        //    //////////////////////


        //    var list = new List<dynamic>();
        //    //DataSet ds = new DataSet();
        //    //string ConnectionString = (string)System.Web.HttpContext.Current.Session["DefaultConnectionString"];
        //    //using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
        //    //{
        //    //    sqlConnection.Open();
        //    //    SqlCommand cmd = new SqlCommand("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".spRep_SaleInvoiceHeaderChargeReport", sqlConnection);
        //    //    cmd.Parameters.AddWithValue("@SiteId", SiteId);
        //    //    cmd.Parameters.AddWithValue("@DivisionId", DivisionId);
        //    //    cmd.Parameters.AddWithValue("@FromDate", FromDate);
        //    //    cmd.Parameters.AddWithValue("@ToDate", ToDate);
        //    //    cmd.Parameters.AddWithValue("@DocumenttypeId", DocumenttypeId);
        //    //    SqlDataAdapter sqlDataAapter = new SqlDataAdapter(cmd);
        //    //    sqlDataAapter.SelectCommand.CommandType = CommandType.StoredProcedure;
        //    //    sqlDataAapter.Fill(ds);
        //        //Tepm = JsonConvert.SerializeObject(ds.Tables[0]);

        //        //var test = ds.Tables[0].AsEnumerable();
        //        //var Tepm = JsonConvert.SerializeObject(ds.Tables[0]);
        //        // var t = test.ToArray();

                
        //       // JSONString = JsonConvert.SerializeObject(ds.Tables[0]);

               
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            dynamic dyn = new ExpandoObject();
        //            list.Add(dyn);
        //            foreach (DataColumn column in ds.Tables[0].Columns)
        //            {
        //                var dic = (IDictionary<string, object>)dyn;
        //                dic[column.ColumnName] = row[column];
        //            }
        //        }

        //   // }

        //    //var t1 = list.ToList();
        //    //var to = JSONString.ToList();

        //    return Json(new { Success = true, Data = list.ToList() }, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult SaleInvoiceChargelist(FormCollection form)
        {
            var SubReportDataList = new List<DataTable>();
            var SubReportNameList = new List<string>();
            DataTable ReportData = new DataTable();
            var list = new List<dynamic>();
            Dictionary<string, string> ReportFilters = new Dictionary<string, string>();
            StringBuilder queryString = new StringBuilder();

            string ReportHeaderId =(form["ReportHeaderId"].ToString());

            ReportHeader header = new ReportHeaderService(_unitOfWork).GetReportHeader(Convert.ToInt32(ReportHeaderId));
            List<ReportLine> lines = new ReportLineService(_unitOfWork).GetReportLineList(header.ReportHeaderId).ToList();

            //List<string> SubReportProcList = new ReportHeaderService(_unitOfWork).GetSubReportProcList (Convert.ToInt32(ReportHeaderId));
            List<string> SubReportProcList = new List<string>();

            ApplicationDbContext Db = new ApplicationDbContext();
            queryString.Append(db.strSchemaName + "." + header.SqlProc);


            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString.ToString(), sqlConnection);                
                foreach (var item in lines)
                {


                    if (item.SqlParameter != "" && item.SqlParameter != null)
                    {
                        if (item.SqlParameter == "@LoginSite" || item.SqlParameter == "@LoginDivision")
                        {
                            if (item.SqlParameter == "@LoginSite")
                                cmd.Parameters.AddWithValue(item.SqlParameter, (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginSiteId]);
                            //cmd.Parameters.AddWithValue(item.SqlParameter, 17);
                            else if (item.SqlParameter == "@LoginDivision")
                                cmd.Parameters.AddWithValue(item.SqlParameter, (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginDivisionId]);

                        }
                        else if (item.FieldName == "Site" && form[item.FieldName].ToString() == "")
                        {
                            cmd.Parameters.AddWithValue(item.SqlParameter, (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginSiteId]);
                        }

                        else if (item.FieldName == "Division" && form[item.FieldName].ToString() == "")
                        {

                            cmd.Parameters.AddWithValue(item.SqlParameter, (int)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginDivisionId]);
                        }

                        else
                        {
                            if (form[item.FieldName].ToString() != "")
                            {
                                if (item.DataType == "Date")
                                {
                                    cmd.Parameters.AddWithValue(item.SqlParameter, (form[item.FieldName].ToString() != "" ? String.Format("{0:MMMM dd yyyy}", form[item.FieldName].ToString()) : "Null"));

                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue(item.SqlParameter, (form[item.FieldName].ToString() != "" ? form[item.FieldName].ToString() : "Null"));

                                }
                            }
                        }

                    }
                }
                cmd.CommandTimeout = 200;
                SqlDataAdapter sqlDataAapter = new SqlDataAdapter(cmd);
                sqlDataAapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                // dsRep.EnforceConstraints = false;
                sqlDataAapter.Fill(ReportData);
            }

            if (ReportData.Rows.Count > 0)
            {
                //var list = new List<dynamic>();

                foreach (DataRow row in ReportData.Rows)
                {
                    dynamic dyn = new ExpandoObject();
                    list.Add(dyn);
                    foreach (DataColumn column in ReportData.Columns)
                    {
                        var dic = (IDictionary<string, object>)dyn;
                        dic[column.ColumnName] = row[column];
                    }
                }
                
               //return Json(new { Success = true, Data = list.ToList(), MaxJsonLength = int.MaxValue }, JsonRequestBehavior.AllowGet);
              // return Json(new { Success = true, Data = list.ToList()}, JsonRequestBehavior.AllowGet);


                JsonResult json = Json(new { Success = true, Data = list.ToList() }, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;


                // JavaScriptSerializer serializer = new JavaScriptSerializer() { MaxJsonLength = 1024000, RecursionLimit = 100 };
                //return Json(serializer.Serialize(list.ToList()));


                //var outputJsonResult = Json(list.ToList());
                //outputJsonResult.MaxJsonLength = 10 * 1024 * 1024; // 10 MB
                //return outputJsonResult;

                //var p = Json(list.ToList(), JsonRequestBehavior.AllowGet);
                //p.MaxJsonLength = int.MaxValue;
                //return p;

                //TempData["TempCustomer"] = list.ToList();
                // return View(list.ToList());
                //return View("SaleInvoiceHeaderCharges");
            }

            else
            {
                ViewBag.Message = "No Record to Print.";
                return Json(new { Success = true, Data = list.ToList() }, JsonRequestBehavior.AllowGet);
            }

            // return View("SaleInvoiceHeaderCharges");
        }

        public ActionResult DataTablePrint(int MenuId)
        {
            Menu menu = new MenuService(_unitOfWork).Find(MenuId);
            //ViewBag.MenuName = menu.MenuName;
            //Session["MenuName"] = menu.MenuName;
            //return View("SaleInvoiceHeaderCharges");
           return RedirectToAction("ReportLayout", "SaleInvoiceHeaderCharge", new { name = menu.MenuName });
        }

        [HttpGet]
        public ActionResult ReportLayout(string name)
        {
            ReportHeader header = new ReportHeaderService(_unitOfWork).GetReportHeaderByName(name);
            List<ReportLine> lines = new ReportLineService(_unitOfWork).GetReportLineList(header.ReportHeaderId).ToList();

            Dictionary<int, string> DefaultValues = TempData["ReportLayoutDefaultValues"] as Dictionary<int, string>;
            TempData["ReportLayoutDefaultValues"] = DefaultValues;
            foreach (var item in lines)
            {
                if (DefaultValues != null && DefaultValues.ContainsKey(item.ReportLineId))
                {
                    item.DefaultValue = DefaultValues[item.ReportLineId];
                }
            }

            ReportMasterViewModel vm = new ReportMasterViewModel();

            if (TempData["closeOnSelectOption"] != null)
                vm.closeOnSelect = (bool)TempData["closeOnSelectOption"];

            vm.ReportHeader = header;
            vm.ReportLine = lines;
            vm.ReportHeaderId = header.ReportHeaderId;

            return View(vm);
        }
        //public ActionResult Filters(string name)
        // {
        //    //string name = (string)System.Web.HttpContext.Current.Session["MenuName"];
        //    ReportHeader header = new ReportHeaderService(_unitOfWork).GetReportHeaderByName(name);
        //    List<ReportLine> lines = new ReportLineService(_unitOfWork).GetReportLineList(header.ReportHeaderId).ToList();
           

        //    Dictionary<int, string> DefaultValues = TempData["ReportLayoutDefaultValues"] as Dictionary<int, string>;
        //    TempData["ReportLayoutDefaultValues"] = DefaultValues;
        //    foreach (var item in lines)
        //    {
        //        if (DefaultValues != null && DefaultValues.ContainsKey(item.ReportLineId))
        //        {
        //            item.DefaultValue = DefaultValues[item.ReportLineId];
        //        }
        //    }

        //    ReportMasterViewModel vm = new ReportMasterViewModel();

        //    if (TempData["closeOnSelectOption"] != null)
        //        vm.closeOnSelect = (bool)TempData["closeOnSelectOption"];

        //    vm.ReportHeader = header;
        //    vm.ReportLine = lines;
        //    vm.ReportHeaderId = header.ReportHeaderId;
        
        //    return PartialView("_Filters", vm);
        //}

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
