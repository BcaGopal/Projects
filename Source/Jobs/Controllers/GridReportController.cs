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
    public class GridReportController : System.Web.Mvc.Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        protected string connectionString = (string)System.Web.HttpContext.Current.Session["DefaultConnectionString"];
        //IGridReportService _GridReportService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;


        public GridReportController(IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _exception = exec;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult GridReportLayout(int MenuId)
        {
            Menu menu = new MenuService(_unitOfWork).Find(MenuId);

            ReportHeader header = new ReportHeaderService(_unitOfWork).GetReportHeaderByName(menu.MenuName);
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

        public JsonResult GridReportFill(FormCollection form)
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

                ReportData.Columns.Add("SysParamType");
                
                
                DataRow DRowAggregate = ReportData.NewRow();
                DRowAggregate["SysParamType"] = "Aggregate";



                foreach (DataColumn column in ReportData.Columns)
                {
                    switch (column.DataType.ToString())
                    {
                        case "System.Decimal":
                            {
                                DRowAggregate[column.ColumnName] = 2;
                                break;
                            }
                    }
                }

                ReportData.Rows.Add(DRowAggregate);
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


                JsonResult json = Json(new { Success = true, Data = list.ToList() }, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }

            else
            {
                ViewBag.Message = "No Record to Print.";
                return Json(new { Success = true, Data = list.ToList() }, JsonRequestBehavior.AllowGet);
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
    }
}
