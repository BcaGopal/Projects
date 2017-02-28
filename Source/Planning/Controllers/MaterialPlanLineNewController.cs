﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model.Models;
using Data.Models;
using Service;
using Data.Infrastructure;
using Core.Common;
using AutoMapper;
using Model.ViewModel;
using System.Data.SqlClient;
using System.Configuration;
using Model.ViewModels;
using Planning;
using System.Data;

namespace Presentation
{

    [Authorize]
    public class MaterialPlanLineNewController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        IMaterialPlanForSaleOrderLineService _MaterialPlanForSaleOrderLineService;
        IMaterialPlanForProdOrderService _MaterialPlanForProdOrderLineService;
        IMaterialPlanLineService _MaterialPlanLineService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        bool TimePlanValidation = true;
        string ExceptionMsg = "";
        bool Continue = true;

        public MaterialPlanLineNewController(IMaterialPlanForSaleOrderLineService SaleOrder, IUnitOfWork unitOfWork, IExceptionHandlingService exec, IMaterialPlanForProdOrderService prodorder, IMaterialPlanLineService line)
        {
            _MaterialPlanForSaleOrderLineService = SaleOrder;
            _MaterialPlanForProdOrderLineService = prodorder;
            _MaterialPlanLineService = line;
            _unitOfWork = unitOfWork;
            _exception = exec;
        }

        [HttpGet]
        public JsonResult Index(int id)
        {
            var p = new MaterialPlanLineService(_unitOfWork).GetMaterialPlanLineList(id).ToList();
            return Json(p, JsonRequestBehavior.AllowGet);
        }
        //For SaleOrder::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        public ActionResult _ForSaleOrder(int id)
        {
            MaterialPlanForLineFilterViewModel vm = new MaterialPlanForLineFilterViewModel();
            vm.DocTypeId = new MaterialPlanHeaderService(_unitOfWork).Find(id).DocTypeId;
            MaterialPlanHeader Header = new MaterialPlanHeaderService(_unitOfWork).Find(id);
            MaterialPlanSettings Settings = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(Header.DocTypeId, Header.DivisionId, Header.SiteId);
            vm.MaterialPlanSettings = Mapper.Map<MaterialPlanSettings, MaterialPlanSettingsViewModel>(Settings);

            vm.DocumentTypeSettings = new DocumentTypeSettingsService(_unitOfWork).GetDocumentTypeSettingsForDocument(Header.DocTypeId);
            vm.MaterialPlanHeaderId = id;
            return PartialView("_Filters", vm);
        }


        public string Validate_Product(MaterialPlanForLineFilterViewModel vm)
        {
            string MsgValidate_Product = "";

            return MsgValidate_Product;
        }

        public string Validate_SaleOrder(string SaleOrderHeaderList)
        {
            string MsgValidate_SaleOrder = "";

            IEnumerable<SaleOrderHeader> temp = new SaleOrderHeaderService(_unitOfWork).GetSaleOrderListFromIds(SaleOrderHeaderList);

            int j = 0;
            foreach (var item in temp)
            {
                if (string.IsNullOrEmpty(item.ReviewBy) || item.ReviewCount < 1)
                {
                    MsgValidate_SaleOrder = MsgValidate_SaleOrder + item.DocNo + ",";
                }

            }

            if (MsgValidate_SaleOrder.Length > 0)
            {
                MsgValidate_SaleOrder = "Sale Order " + MsgValidate_SaleOrder.Substring(0, MsgValidate_SaleOrder.Length - 1) + " Not Review";
            }
            return MsgValidate_SaleOrder;
        }


        public string Validate_Model(MaterialPlanForLineFilterViewModel vm)
        {
            string MsgValidate_Model = "";
            if (!string.IsNullOrEmpty(vm.SaleOrderHeaderId))
                MsgValidate_Model = Validate_SaleOrder(vm.SaleOrderHeaderId);

            if (!string.IsNullOrEmpty(vm.ProductId))
                MsgValidate_Model = MsgValidate_Model + Validate_Product(vm);

            return MsgValidate_Model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _FilterPost(MaterialPlanForLineFilterViewModel vm)
        {

            string ValidationMsg = "";

            ValidationMsg = Validate_Model(vm);

            // if (vm.SaleOrderHeaderId  != null)            {

            //     string[] ArSaleOrderHeaderId = vm.SaleOrderHeaderId.Split(',');

            //     for(int i = 0; i < ArSaleOrderHeaderId.Length; i++)
            //        {
            //         int SaleOrderHeaderId = Convert.ToInt32(ArSaleOrderHeaderId[i]);
            //         SaleOrderHeader SOH = new SaleOrderHeaderService(_unitOfWork).Find(SaleOrderHeaderId);

            //         if ( SOH.Status != (int) StatusConstants.Approved )
            //         {
            //             ValidationMsg = ValidationMsg + SOH.DocNo + ",";
            //         }

            //        }
            //}



            if (ValidationMsg != "")
            {
                ModelState.AddModelError("", ValidationMsg);
                return PartialView("_Filters", vm);
            }


            List<MaterialPlanForSaleOrderViewModel> temp = _MaterialPlanForSaleOrderLineService.GetSaleOrdersForFilters(vm).ToList();
            MaterialPlanLineListViewModel svm = new MaterialPlanLineListViewModel();

            var DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            var SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            //svm.MaterialPlanSettings = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(vm.DocTypeId, DivisionId, SiteId);
            var settings = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(vm.DocTypeId, DivisionId, SiteId);
            svm.MaterialPlanSettings = Mapper.Map<MaterialPlanSettings, MaterialPlanSettingsViewModel>(settings);

            svm.MaterialPlanLineViewModel = temp;
            return PartialView("_Results", svm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ResultsPost(MaterialPlanLineListViewModel vm)
        {
            MaterialPlanHeader header = new MaterialPlanHeaderService(_unitOfWork).Find(vm.MaterialPlanLineViewModel.FirstOrDefault().MaterialPlanHeaderId);

            string sessionmaterialplanline = header.DocTypeId + "_" + header.MaterialPlanHeaderId + "_materialplanline";

            System.Web.HttpContext.Current.Session[sessionmaterialplanline] = vm.MaterialPlanLineViewModel.ToList();
            //MaterialPlanSettings Setting = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(header.DocTypeId, header.DivisionId, header.SiteId);

            var settings = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(header.DocTypeId, header.DivisionId, header.SiteId);
            

            if (ModelState.IsValid)
            {

                var ProductIds = vm.MaterialPlanLineViewModel.Select(m => m.ProductId).ToArray();

                List<MaterialPlanLineViewModel> Line = new List<MaterialPlanLineViewModel>();
                if (settings.SqlProcConsumptionSummary != null)
                {
                    Line = new MaterialPlanForSaleOrderLineService(_unitOfWork).GetMaterialPlanSummaryFromProcedure(vm, (string)System.Web.HttpContext.Current.Session["DefaultConnectionString"], settings.SqlProcConsumptionSummary).ToList();
                }
                else
                {
                    var summary = (from p in db.Product.Where(p => ProductIds.Contains(p.ProductId)).AsEnumerable()
                                   join t in vm.MaterialPlanLineViewModel on p.ProductId equals t.ProductId
                                   where t.Qty > 0
                                   group t by new { t.ProductId, p.ProductName, t.Dimension1Id, t.Dimension2Id, t.Dimension3Id, t.Dimension4Id, t.Specification } into g
                                   join p1 in db.Product.Where(p => ProductIds.Contains(p.ProductId)).AsEnumerable() on g.Key.ProductId equals p1.ProductId
                                   join u1 in db.Units on p1.UnitId equals u1.UnitId
                                   select new
                                   {
                                       id = g.Key.ProductId,
                                       QtySum = g.Sum(m => m.Qty),
                                       GroupedItems = g,
                                       name = g.Key.ProductName,
                                       unitname = u1.UnitName,
                                       Dimension1Id = g.Key.Dimension1Id,
                                       Dimension1Name = g.Max(i => i.Dimension1Name),
                                       Dimension2Id = g.Key.Dimension2Id,
                                       Dimension2Name = g.Max(i => i.Dimension2Name),
                                       Dimension3Id = g.Key.Dimension3Id,
                                       Dimension3Name = g.Max(i => i.Dimension3Name),
                                       Dimension4Id = g.Key.Dimension4Id,
                                       Dimension4Name = g.Max(i => i.Dimension4Name),
                                       Specification = g.Key.Specification,
                                       Fractionunits = u1.DecimalPlaces
                                   }).ToList();

                    int j = 0;
                    foreach (var item in summary)
                    {
                        MaterialPlanLineViewModel planline = new MaterialPlanLineViewModel();
                        planline.ProductName = item.name;
                        planline.RequiredQty = item.QtySum;
                        planline.Dimension1Name = item.Dimension1Name;
                        planline.Dimension2Name = item.Dimension2Name;
                        planline.Dimension3Name = item.Dimension3Name;
                        planline.Dimension4Name = item.Dimension4Name;

                            //using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString()))
                            using (SqlConnection sqlConnection = new SqlConnection((string)System.Web.HttpContext.Current.Session["DefaultConnectionString"]))
                            {
                                sqlConnection.Open();

                                int ProductCode = item.id;

                                SqlCommand Totalf = new SqlCommand("SELECT Web.FGetExcessStock( " + ProductCode + ", " + header.DocTypeId + ")", sqlConnection);

                                planline.ExcessStockQty = Convert.ToDecimal(Totalf.ExecuteScalar() == DBNull.Value ? 0 : Totalf.ExecuteScalar());
                            }

                        
                        planline.Specification = item.Specification;
                        planline.MaterialPlanHeaderId = vm.MaterialPlanLineViewModel.FirstOrDefault().MaterialPlanHeaderId;
                        planline.ProductId = item.id;
                        planline.Dimension1Id = item.Dimension1Id;
                        planline.Dimension2Id = item.Dimension2Id;
                        planline.Dimension3Id = item.Dimension3Id;
                        planline.Dimension4Id = item.Dimension4Id;
                        planline.ProcessId = new ProcessService(_unitOfWork).Find(ProcessConstants.Manufacturing).ProcessId;
                        planline.ProcessName = new ProcessService(_unitOfWork).Find(ProcessConstants.Manufacturing).ProcessName;
                        planline.ProdPlanQty = item.QtySum;
                        planline.UnitName = item.unitname;
                        planline.unitDecimalPlaces = item.Fractionunits;
                        planline.GeneratedFor = MaterialPlanConstants.SaleOrder;
                        Line.Add(planline);

                        j++;
                    }
                }

                MaterialPlanSummaryViewModel Summary = new MaterialPlanSummaryViewModel();

                var DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
                var SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
                Summary.MaterialPlanSettings = Mapper.Map<MaterialPlanSettings, MaterialPlanSettingsViewModel>(settings); ;

                Summary.PlanLine = Line.OrderBy(m => m.ProductName).ThenBy(m => m.Dimension1Name).ThenBy(m => m.Dimension2Name).ThenBy(m => m.Dimension3Name).ThenBy(m => m.Dimension4Name).ToList();

                return PartialView("_SummarySaleOrder", Summary);

            }
            return PartialView("_Results", vm);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _SummaryPostSaleOrder(MaterialPlanSummaryViewModel vm)
        {

            MaterialPlanHeader header = new MaterialPlanHeaderService(_unitOfWork).Find(vm.PlanLine.FirstOrDefault().MaterialPlanHeaderId);

            int Serial = new MaterialPlanLineService(_unitOfWork).GetMaxSr(header.MaterialPlanHeaderId);

            string sessionMaterialPlanLine = header.DocTypeId + "_" + header.MaterialPlanHeaderId + "_materialplanline";


            MaterialPlanLineListViewModel svm = new MaterialPlanLineListViewModel();
            svm.MaterialPlanLineViewModel = (List<MaterialPlanForSaleOrderViewModel>)System.Web.HttpContext.Current.Session[sessionMaterialPlanLine];

            //MaterialPlanSettings Settings = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(header.DocTypeId, header.DivisionId, header.SiteId);


            //Getting Settings
            var settings = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(header.DocTypeId, header.DivisionId, header.SiteId);
            vm.MaterialPlanSettings = Mapper.Map<MaterialPlanSettings, MaterialPlanSettingsViewModel>(settings);

            using (var context = new ApplicationDbContext())
            {
                bool isPr = false;
                bool isPP = false;
                int j = 0;
                foreach (var item in vm.PlanLine)
                {
                    {
                        MaterialPlanLine planLine = new MaterialPlanLine();
                        planLine.RequiredQty = item.RequiredQty;
                        planLine.ExcessStockQty = item.ExcessStockQty;
                        planLine.MaterialPlanHeaderId = item.MaterialPlanHeaderId;
                        planLine.ProductId = item.ProductId;
                        planLine.Dimension1Id = item.Dimension1Id;
                        planLine.Dimension2Id = item.Dimension2Id;
                        planLine.Dimension3Id = item.Dimension3Id;
                        planLine.Dimension4Id = item.Dimension4Id;

                        planLine.ProdPlanQty = item.ProdPlanQty;
                        planLine.CreatedBy = User.Identity.Name;
                        planLine.CreatedDate = DateTime.Now;
                        planLine.Sr = Serial++;
                        planLine.Specification = item.Specification;
                        planLine.ModifiedBy = User.Identity.Name;
                        planLine.MaterialPlanLineId = j;
                        planLine.ModifiedDate = DateTime.Now;
                        planLine.ProcessId = item.ProcessId;
                        planLine.Remark = item.Remark;
                        planLine.PurchPlanQty = item.PurchPlanQty;
                        planLine.StockPlanQty = item.StockPlanQty;
                        planLine.GeneratedFor = MaterialPlanConstants.SaleOrder;
                        planLine.ObjectState = Model.ObjectState.Added;
                        context.MaterialPlanLine.Add(planLine);
                        if (!isPr)
                        {
                            if (item.ProdPlanQty > 0)
                                isPr = true;
                        }
                        if (!isPP)
                        {
                            if (item.PurchPlanQty > 0)
                                isPP = true;
                        }
                    }
                    j++;
                }


                int prodorderheaderid = 0;
                int prodorderlineid = 0;
                if (isPr)
                {
                    
                    //ProdOrderHeader ExistingProdOrder = new ProdOrderHeaderService(_unitOfWork).GetProdOrderForMaterialPlan(header.MaterialPlanHeaderId);
                    ProdOrderHeader ExistingProdOrder = new ProdOrderHeaderService(_unitOfWork).GetPurchOrProdForMaterialPlan(header.MaterialPlanHeaderId, settings.DocTypeProductionOrderId.Value);
                    int ProdORderSerial = 1;
                    if (ExistingProdOrder == null)
                    {
                        ProdOrderHeader prodOrderHeader = new ProdOrderHeader();
                        prodOrderHeader.ProdOrderHeaderId = prodorderheaderid-- ;
                        prodOrderHeader.CreatedBy = User.Identity.Name;
                        prodOrderHeader.CreatedDate = DateTime.Now;
                        prodOrderHeader.DivisionId = header.DivisionId;
                        prodOrderHeader.DocDate = header.DocDate;
                        prodOrderHeader.DocNo = header.DocNo;
                        prodOrderHeader.DocTypeId = settings.DocTypeProductionOrderId.Value;
                        prodOrderHeader.DueDate = header.DueDate;
                        prodOrderHeader.MaterialPlanHeaderId = header.MaterialPlanHeaderId;
                        prodOrderHeader.ModifiedBy = User.Identity.Name;
                        prodOrderHeader.ModifiedDate = DateTime.Now;
                        prodOrderHeader.Remark = header.Remark;
                        prodOrderHeader.BuyerId = header.BuyerId;
                        prodOrderHeader.SiteId = header.SiteId;
                        //prodOrderHeader.Status = header.Status;
                        prodOrderHeader.Status = header.Status;
                        prodOrderHeader.LockReason = "Production Order automatically generated from planning.";
                        prodOrderHeader.ObjectState = Model.ObjectState.Added;
                        context.ProdOrderHeader.Add(prodOrderHeader);

                        //ForCreating ProdOrderStatus
                        ProdOrderHeaderStatus pt = new ProdOrderHeaderStatus();
                        pt.ProdOrderHeaderId = prodOrderHeader.ProdOrderHeaderId;
                        pt.ObjectState = Model.ObjectState.Added;
                        context.ProdOrderHeaderStatus.Add(pt);


                        //int ProdOrderLineKey = 0;
                        foreach (var item in context.MaterialPlanLine.Local.Where(m => m.ProdPlanQty > 0))
                        {
                            ProdOrderLine prodOrderLine = new ProdOrderLine();
                            prodOrderLine.CreatedBy = User.Identity.Name;
                            prodOrderLine.CreatedDate = DateTime.Now;
                            prodOrderLine.MaterialPlanLineId = item.MaterialPlanLineId;
                            prodOrderLine.ModifiedBy = User.Identity.Name;
                            prodOrderLine.ModifiedDate = DateTime.Now;
                            prodOrderLine.ProdOrderHeaderId = prodOrderHeader.ProdOrderHeaderId;
                            prodOrderLine.Specification = item.Specification;
                            prodOrderLine.ProductId = item.ProductId;
                            prodOrderLine.Dimension1Id = item.Dimension1Id;
                            prodOrderLine.Dimension2Id = item.Dimension2Id;
                            prodOrderLine.Dimension3Id = item.Dimension3Id;
                            prodOrderLine.Dimension4Id = item.Dimension4Id;
                            prodOrderLine.ProcessId = item.ProcessId;
                            prodOrderLine.Sr = ProdORderSerial++;
                            prodOrderLine.Qty = item.ProdPlanQty;
                            prodOrderLine.Remark = item.Remark;
                            prodOrderLine.LockReason = "Production Order automatically generated from planning.";
                            prodOrderLine.ProdOrderLineId = prodorderlineid--;
                            prodOrderLine.ObjectState = Model.ObjectState.Added;
                            context.ProdOrderLine.Add(prodOrderLine);

                            //ForAdding ProdrderLinestatus
                            ProdOrderLineStatus ptl = new ProdOrderLineStatus();
                            ptl.ProdOrderLineId = prodOrderLine.ProdOrderLineId;
                            ptl.ObjectState = Model.ObjectState.Added;
                            context.ProdOrderLineStatus.Add(ptl);

                        }

                    }
                    else
                    {
                        ProdORderSerial = new ProdOrderLineService(_unitOfWork).GetMaxSr(ExistingProdOrder.ProdOrderHeaderId);


                        //int ProdOrderLineKey = 0;
                        foreach (var item in context.MaterialPlanLine.Local.Where(m => m.ProdPlanQty > 0))
                        {
                            ProdOrderLine prodOrderLine = new ProdOrderLine();
                            prodOrderLine.CreatedBy = User.Identity.Name;
                            prodOrderLine.CreatedDate = DateTime.Now;
                            prodOrderLine.MaterialPlanLineId = item.MaterialPlanLineId;
                            prodOrderLine.ModifiedBy = User.Identity.Name;
                            prodOrderLine.ModifiedDate = DateTime.Now;
                            prodOrderLine.ProdOrderHeaderId = ExistingProdOrder.ProdOrderHeaderId;
                            prodOrderLine.ProductId = item.ProductId;
                            prodOrderLine.Dimension1Id = item.Dimension1Id;
                            prodOrderLine.Dimension2Id = item.Dimension2Id;
                            prodOrderLine.Dimension3Id = item.Dimension3Id;
                            prodOrderLine.Dimension4Id = item.Dimension4Id;
                            prodOrderLine.ProcessId = item.ProcessId;
                            prodOrderLine.Specification = item.Specification;
                            prodOrderLine.Qty = item.ProdPlanQty;
                            prodOrderLine.Sr = ProdORderSerial++;
                            prodOrderLine.Remark = item.Remark;
                            prodOrderLine.LockReason = "Production Order automatically generated from planning.";
                            prodOrderLine.ProdOrderLineId = prodorderlineid--;
                            prodOrderLine.ObjectState = Model.ObjectState.Added;
                            context.ProdOrderLine.Add(prodOrderLine);

                            //ForAdding ProdrderLinestatus
                            ProdOrderLineStatus ptl = new ProdOrderLineStatus();
                            ptl.ProdOrderLineId = prodOrderLine.ProdOrderLineId;
                            ptl.ObjectState = Model.ObjectState.Added;
                            context.ProdOrderLineStatus.Add(ptl);

                        }
                    }



                }
                if (isPP)
                {


                    //PurchaseIndentHeader ExistingIndent = new PurchaseIndentHeaderService(_unitOfWork).GetPurchaseIndentForMaterialPlan(header.MaterialPlanHeaderId);
                    ProdOrderHeader ExistingIndent = new ProdOrderHeaderService(_unitOfWork).GetPurchOrProdForMaterialPlan(header.MaterialPlanHeaderId, settings.DocTypePurchaseIndentId.Value);
                    int PurchaseIndentSr = 1;
                    if (ExistingIndent == null)
                    {

                        //PurchaseIndentHeader indentHeader = new PurchaseIndentHeader();
                        ProdOrderHeader indentHeader = new ProdOrderHeader();
                        indentHeader.ProdOrderHeaderId = prodorderheaderid--;
                        indentHeader.CreatedBy = User.Identity.Name;
                        indentHeader.CreatedDate = DateTime.Now;
                        indentHeader.DivisionId = header.DivisionId;
                        indentHeader.DocDate = header.DocDate;
                        indentHeader.DocNo = header.DocNo;
                        indentHeader.DocTypeId = settings.DocTypePurchaseIndentId.Value;
                        indentHeader.DueDate = header.DueDate;
                        indentHeader.MaterialPlanHeaderId = header.MaterialPlanHeaderId;
                        indentHeader.ModifiedBy = User.Identity.Name;
                        indentHeader.ModifiedDate = DateTime.Now;
                        indentHeader.Remark = header.Remark;
                        indentHeader.BuyerId = header.BuyerId;
                        indentHeader.SiteId = header.SiteId;
                        //indentHeader.Status = header.Status;
                        indentHeader.Status = header.Status;
                        indentHeader.LockReason = "Purchase Indent automatically generated from planning.";
                        //indentHeader.DepartmentId = (int)DepartmentConstants.Production;
                        indentHeader.ObjectState = Model.ObjectState.Added;
                        //context.PurchaseIndentHeader.Add(indentHeader);
                        context.ProdOrderHeader.Add(indentHeader);



                        //ForCreating ProdOrderStatus
                        ProdOrderHeaderStatus pt = new ProdOrderHeaderStatus();
                        pt.ProdOrderHeaderId = indentHeader.ProdOrderHeaderId;
                        pt.ObjectState = Model.ObjectState.Added;
                        context.ProdOrderHeaderStatus.Add(pt);



                        foreach (var item in context.MaterialPlanLine.Local.Where(m => m.PurchPlanQty > 0))
                        {
                            //PurchaseIndentLine indentLine = new PurchaseIndentLine();
                            ProdOrderLine indentLine = new ProdOrderLine();
                            indentLine.CreatedBy = User.Identity.Name;
                            indentLine.CreatedDate = DateTime.Now;
                            indentLine.MaterialPlanLineId = item.MaterialPlanLineId;
                            indentLine.ModifiedBy = User.Identity.Name;
                            indentLine.ModifiedDate = DateTime.Now;
                            indentLine.ProdOrderHeaderId = indentHeader.ProdOrderHeaderId;
                            indentLine.Specification = item.Specification;
                            indentLine.ProductId = item.ProductId;
                            indentLine.Dimension1Id = item.Dimension1Id;
                            indentLine.Dimension2Id = item.Dimension2Id;
                            indentLine.Dimension3Id = item.Dimension3Id;
                            indentLine.Dimension4Id = item.Dimension4Id;
                            indentLine.ProcessId = new ProcessService(_unitOfWork).Find(ProcessConstants.Purchase).ProcessId;
                            indentLine.Sr = PurchaseIndentSr++;
                            indentLine.Qty = item.PurchPlanQty;
                            indentLine.Remark = item.Remark;
                            indentLine.LockReason = "Purchase Indent automatically generated from planning.";
                            indentLine.ProdOrderLineId = prodorderlineid--;
                            //indentLine.PurchaseIndentHeaderId = indentHeader.PurchaseIndentHeaderId;
                            indentLine.ObjectState = Model.ObjectState.Added;
                            //context.PurchaseIndentLine.Add(indentLine);
                            context.ProdOrderLine.Add(indentLine);

                            //ForAdding PurchaseIndentLinestatus
                            ProdOrderLineStatus ptl = new ProdOrderLineStatus();
                            ptl.ProdOrderLineId = indentLine.ProdOrderLineId;
                            ptl.ObjectState = Model.ObjectState.Added;
                            context.ProdOrderLineStatus.Add(ptl);
                        }

                    }
                    else
                    {
                        //PurchaseIndentSr = new PurchaseIndentLineService(_unitOfWork).GetMaxSr(ExistingIndent.PurchaseIndentHeaderId);
                        PurchaseIndentSr = new ProdOrderLineService(_unitOfWork).GetMaxSr(ExistingIndent.ProdOrderHeaderId);
                        foreach (var item in context.MaterialPlanLine.Local.Where(m => m.PurchPlanQty > 0))
                        {
                            //PurchaseIndentLine indentLine = new PurchaseIndentLine();
                            ProdOrderLine indentLine = new ProdOrderLine();
                            indentLine.CreatedBy = User.Identity.Name;
                            indentLine.CreatedDate = DateTime.Now;
                            indentLine.MaterialPlanLineId = item.MaterialPlanLineId;
                            indentLine.ModifiedBy = User.Identity.Name;
                            indentLine.ModifiedDate = DateTime.Now;
                            indentLine.ProdOrderHeaderId = ExistingIndent.ProdOrderHeaderId;
                            indentLine.Specification = item.Specification;
                            indentLine.ProductId = item.ProductId;
                            indentLine.Dimension1Id = item.Dimension1Id;
                            indentLine.Dimension2Id = item.Dimension2Id;
                            indentLine.Dimension3Id = item.Dimension3Id;
                            indentLine.Dimension4Id = item.Dimension4Id;
                            indentLine.Qty = item.PurchPlanQty;
                            indentLine.ProcessId = new ProcessService(_unitOfWork).Find(ProcessConstants.Purchase).ProcessId;
                            indentLine.Sr = PurchaseIndentSr++;
                            indentLine.Remark = item.Remark;
                            indentLine.LockReason = "Purchase Indent automatically generated from planning.";
                            //indentLine.PurchaseIndentHeaderId = ExistingIndent.PurchaseIndentHeaderId;
                            indentLine.ProdOrderLineId = prodorderlineid--;
                            indentLine.ObjectState = Model.ObjectState.Added;
                            //context.PurchaseIndentLine.Add(indentLine);
                            context.ProdOrderLine.Add(indentLine);

                            //ForAdding ProdrderLinestatus
                            ProdOrderLineStatus ptl = new ProdOrderLineStatus();
                            ptl.ProdOrderLineId = indentLine.ProdOrderLineId;
                            ptl.ObjectState = Model.ObjectState.Added;
                            context.ProdOrderLineStatus.Add(ptl);
                        }
                    }




                }


                int i = 0;
                int MaterialPlanForSaleOrderSr = new MaterialPlanForSaleOrderService(_unitOfWork).GetMaxSr(svm.MaterialPlanLineViewModel.FirstOrDefault().MaterialPlanHeaderId);
                foreach (var item in svm.MaterialPlanLineViewModel)
                {

                    if (item.Qty > 0)
                    {
                        MaterialPlanForSaleOrder order = new MaterialPlanForSaleOrder();
                        order.MaterialPlanHeaderId = item.MaterialPlanHeaderId;
                        order.Qty = item.Qty;
                        order.SaleOrderLineId = item.SaleOrderLineId;
                        order.MaterialPlanForSaleOrderId = i;
                        order.Sr = MaterialPlanForSaleOrderSr++;
                        order.CreatedBy = User.Identity.Name;
                        order.CreatedDate = DateTime.Now;
                        order.ModifiedBy = User.Identity.Name;
                        order.ModifiedDate = DateTime.Now;
                        
                        var temp = context.MaterialPlanLine.Local.Where(m => m.ProductId == item.ProductId).FirstOrDefault();
                        if (temp != null)
                        {
                            order.MaterialPlanLineId = context.MaterialPlanLine.Local.Where(m => m.ProductId == item.ProductId).FirstOrDefault().MaterialPlanLineId;
                        }

                        order.ObjectState = Model.ObjectState.Added;
                        context.MaterialPlanForSaleOrder.Add(order);

                        i++;

                    }
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    ModelState.AddModelError("", message);
                    var DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
                    var SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
                    //vm.MaterialPlanSettings = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(header.DocTypeId, DivisionId, SiteId);
                    vm.MaterialPlanSettings = Mapper.Map<MaterialPlanSettings, MaterialPlanSettingsViewModel>(settings);
                    return PartialView("_SummarySaleOrder", vm);
                }

                System.Web.HttpContext.Current.Session.Remove(sessionMaterialPlanLine);
                return Json(new { success = true });
            }

        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreatePost(MaterialPlanLineViewModel vm)
        {

            MaterialPlanLine s = Mapper.Map<MaterialPlanLineViewModel, MaterialPlanLine>(vm);
            MaterialPlanHeader temp = new MaterialPlanHeaderService(_unitOfWork).Find(s.MaterialPlanHeaderId);


            if (ModelState.IsValid)
            {
                if (vm.MaterialPlanLineId <= 0)
                {
                    s.CreatedDate = DateTime.Now;
                    s.ModifiedDate = DateTime.Now;
                    s.CreatedBy = User.Identity.Name;
                    s.ModifiedBy = User.Identity.Name;
                    s.Sr = new MaterialPlanLineService(_unitOfWork).GetMaxSr(s.MaterialPlanHeaderId);
                    s.ObjectState = Model.ObjectState.Added;
                    _MaterialPlanLineService.Create(s);

                    MaterialPlanHeader header = new MaterialPlanHeaderService(_unitOfWork).Find(s.MaterialPlanHeaderId);
                    if (header.Status != (int)StatusConstants.Drafted)
                    {
                        header.Status = (int)StatusConstants.Modified;
                        new MaterialPlanHeaderService(_unitOfWork).Update(header);
                    }


                    try
                    {
                        _unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        ModelState.AddModelError("", message);
                        return PartialView("_Create", vm);

                    }

                    return RedirectToAction("_Create", new { id = s.MaterialPlanHeaderId });
                }
                else
                {

                    MaterialPlanHeader header = new MaterialPlanHeaderService(_unitOfWork).Find(vm.MaterialPlanHeaderId);

                    int status = header.Status;
                    MaterialPlanLine temp1 = _MaterialPlanLineService.Find(vm.MaterialPlanLineId);

                    temp1.DueDate = vm.DueDate;
                    temp1.StockPlanQty = vm.StockPlanQty;
                    temp1.PurchPlanQty = vm.PurchPlanQty;
                    temp1.ProdPlanQty = vm.ProdPlanQty;
                    temp1.Remark = vm.Remark;
                    temp1.ModifiedDate = DateTime.Now;
                    temp1.ModifiedBy = User.Identity.Name;
                    _MaterialPlanLineService.Update(temp1);


                    header.Status = (int)StatusConstants.Modified;
                    new MaterialPlanHeaderService(_unitOfWork).Update(header);

                    try
                    {
                        _unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        ModelState.AddModelError("", message);
                        return PartialView("_Create", vm);
                    }

                    return Json(new { success = true });
                }
            }
            return PartialView("_Create", vm);
        }

        public ActionResult _Edit(int id)//Materialplanlineid
        {
            MaterialPlanLine m = _MaterialPlanLineService.Find(id);
            MaterialPlanHeader Header = new MaterialPlanHeaderService(_unitOfWork).Find(m.MaterialPlanHeaderId);
            MaterialPlanLineViewModel vm = Mapper.Map<MaterialPlanLine, MaterialPlanLineViewModel>(m);

            //Getting Settings
            var settings = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(Header.DocTypeId, Header.DivisionId, Header.SiteId);
            vm.MaterialPlanSettings = Mapper.Map<MaterialPlanSettings, MaterialPlanSettingsViewModel>(settings);


            vm.DocumentTypeSettings = new DocumentTypeSettingsService(_unitOfWork).GetDocumentTypeSettingsForDocument(Header.DocTypeId);

            return PartialView("_Create", vm);

        }

        public ActionResult SaleOrderDetail(int id)//Materialplanlineid
        {
            var m = _MaterialPlanLineService.GetSaleOrderDetail(id);
            return PartialView("_Detail", m);
        }

        public ActionResult ProdOrderDetail(int id)//Materialplanlineid
        {
            var m = _MaterialPlanLineService.GetProdOrderDetail(id);
            return PartialView("_Detail", m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(MaterialPlanLineViewModel vm)
        {
            MaterialPlanLine MaterialPlanLine = _MaterialPlanLineService.Find(vm.MaterialPlanLineId);
            _MaterialPlanLineService.Delete(vm.MaterialPlanLineId);
            MaterialPlanHeader header = new MaterialPlanHeaderService(_unitOfWork).Find(MaterialPlanLine.MaterialPlanHeaderId);
            if (header.Status != (int)StatusConstants.Drafted)
            {
                header.Status = (int)StatusConstants.Modified;
                new MaterialPlanHeaderService(_unitOfWork).Update(header);
            }

            IEnumerable<ProdOrderLine> ProdOrderLineListAutoGenerated = new ProdOrderLineService(_unitOfWork).GetProdOrderLineForMaterialPlan(vm.MaterialPlanLineId);

            foreach (ProdOrderLine item in ProdOrderLineListAutoGenerated)
            {
                new ProdOrderLineStatusService(_unitOfWork).Delete(item.ProdOrderLineId);
                new ProdOrderLineService(_unitOfWork).Delete(item.ProdOrderLineId);
            }


            if (MaterialPlanLine.GeneratedFor == MaterialPlanConstants.SaleOrder)
            {
                var saleOrderLine = new MaterialPlanForSaleOrderService(_unitOfWork).GetPlanSaleOrderForMaterialPlanLine(vm.MaterialPlanLineId);
                foreach (var item in saleOrderLine)
                {
                    new MaterialPlanForSaleOrderService(_unitOfWork).Delete(item.MaterialPlanForSaleOrderId);
                    //new MaterialPlanForSaleOrderService(_unitOfWork).Delete(item.MaterialPlanForSaleOrderId);
                }
            }
            else if (MaterialPlanLine.GeneratedFor == MaterialPlanConstants.ProdOrder)
            {
                var ProdOrderLine = new MaterialPlanForProdOrderService(_unitOfWork).GetPlanProdOrderForMaterialPlanLine(vm.MaterialPlanLineId);
                foreach (var item in ProdOrderLine)
                {
                    new MaterialPlanForProdOrderService(_unitOfWork).Delete(item.MaterialPlanForProdOrderId);
                    //new MaterialPlanForSaleOrderService(_unitOfWork).Delete(item.MaterialPlanForSaleOrderId);
                }
            }

            try
            {
                _unitOfWork.Save();
            }

            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                ModelState.AddModelError("", message);
                return PartialView("_Create", vm);
            }
            return Json(new { success = true });
        }






        //ForProduction Order::::::::::::::::::::::::::::::::::::::::::::::::

        public void PrepareViewBag()
        {
            ViewBag.ProductTypeList = new ProductTypeService(_unitOfWork).GetProductTypeList().ToList();
        }

        public ActionResult _ForProdOrder(int id)
        {
            MaterialPlanLineForProductionFilterViewModel vm = new MaterialPlanLineForProductionFilterViewModel();
            vm.DocTypeId = new MaterialPlanHeaderService(_unitOfWork).Find(id).DocTypeId;
            vm.DocumentTypeSettings = new DocumentTypeSettingsService(_unitOfWork).GetDocumentTypeSettingsForDocument(vm.DocTypeId);
            vm.MaterialPlanHeaderId = id;
            PrepareViewBag();
            return PartialView("_FiltersProduction", vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _FilterPostProduction(MaterialPlanLineForProductionFilterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                List<MaterialPlanForSaleOrderViewModel> temp = _MaterialPlanForSaleOrderLineService.GetProdOrdersForFilters(vm).ToList();
                MaterialPlanLineListViewModel svm = new MaterialPlanLineListViewModel();

                svm.MaterialPlanLineViewModel = temp;

                var DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
                var SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
                //svm.MaterialPlanSettings = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(vm.DocTypeId, DivisionId, SiteId);
                var settings = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(vm.DocTypeId, DivisionId, SiteId);
                svm.MaterialPlanSettings = Mapper.Map<MaterialPlanSettings, MaterialPlanSettingsViewModel>(settings);

                return PartialView("_ResultsProduction", svm);
            }
            PrepareViewBag();
            return PartialView("_FiltersProduction", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ResultsPostProduction(MaterialPlanLineListViewModel vm)
        {
            if (ModelState.IsValid)
            {
                MaterialPlanHeader header = new MaterialPlanHeaderService(_unitOfWork).Find(vm.MaterialPlanLineViewModel.FirstOrDefault().MaterialPlanHeaderId);

                string ProcName = new MaterialPlanSettingsService(_unitOfWork).GetBomProcedureForDocType(header.DocTypeId);


                List<MaterialPlanForProcedureViewModel> prodorderlinefromprocedure = new MaterialPlanForProdOrderLineService(_unitOfWork).GetMaterialPlanLineFromProcedure(vm, (string)System.Web.HttpContext.Current.Session["DefaultConnectionString"], ProcName).ToList();

                var DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
                var SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
                MaterialPlanSummaryViewModel MPSummary = new MaterialPlanSummaryViewModel();

                //MaterialPlanSettings Setting = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(header.DocTypeId, DivisionId, SiteId);

                var settings = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(header.DocTypeId, header.DivisionId, header.SiteId);
                vm.MaterialPlanSettings = Mapper.Map<MaterialPlanSettings, MaterialPlanSettingsViewModel>(settings);

                if (settings.SqlProcConsumptionSummary != null)
                {
                    string sessionmaterialplanline = header.DocTypeId + "_" + header.MaterialPlanHeaderId + "_materialplanline";
                    string sessionprodorderbom = header.DocTypeId + "_" + header.MaterialPlanHeaderId + "_prodorderbom";




                    List<MaterialPlanLineViewModel> MSummary = GetMaterialPlanSummaryFromProcedure(prodorderlinefromprocedure, (string)System.Web.HttpContext.Current.Session["DefaultConnectionString"], settings.SqlProcConsumptionSummary, header.MaterialPlanHeaderId).ToList();

                    // Only For Weaving Plan
                    foreach (var item in prodorderlinefromprocedure)
                    {
                        item.Dimension1Id = null;
                        item.Dimension2Id = null;
                        item.Dimension3Id = null;
                        item.Dimension4Id = null;
                    }

                    System.Web.HttpContext.Current.Session[sessionmaterialplanline] = vm.MaterialPlanLineViewModel.ToList();
                    System.Web.HttpContext.Current.Session[sessionprodorderbom] = prodorderlinefromprocedure;

                    MPSummary.MaterialPlanSettings = Mapper.Map<MaterialPlanSettings, MaterialPlanSettingsViewModel>(settings);
                    MPSummary.PlanLine = MSummary.OrderBy(m => m.ProductName).ThenBy(m => m.Dimension1Name).ThenByDescending(m => m.Dimension2Name).ThenByDescending(m => m.Dimension3Name).ThenByDescending(m => m.Dimension4Name).ToList();

                }
                else
                {
                    string sessionmaterialplanline = header.DocTypeId + "_" + header.MaterialPlanHeaderId + "_materialplanline";
                    string sessionprodorderbom = header.DocTypeId + "_" + header.MaterialPlanHeaderId + "_prodorderbom";

                    System.Web.HttpContext.Current.Session[sessionmaterialplanline] = vm.MaterialPlanLineViewModel.ToList();
                    System.Web.HttpContext.Current.Session[sessionprodorderbom] = prodorderlinefromprocedure;



                    var ProductIds = prodorderlinefromprocedure.Select(m => m.ProductId).ToArray();
                    var Dimension1Ids = prodorderlinefromprocedure.Select(m => m.Dimension1Id).ToArray();
                    var Dimension2Ids = prodorderlinefromprocedure.Select(m => m.Dimension2Id).ToArray();
                    var Dimension3Ids = prodorderlinefromprocedure.Select(m => m.Dimension3Id).ToArray();
                    var Dimension4Ids = prodorderlinefromprocedure.Select(m => m.Dimension4Id).ToArray();
                    var processIds = prodorderlinefromprocedure.Select(m => m.ProcessId).ToArray();


                    var Products = (from p in db.Product.AsNoTracking()
                                    where ProductIds.Contains(p.ProductId)
                                    select p).ToList();

                    var UnitIds = Products.Select(m => m.UnitId).ToArray();

                    var Units = (from p in db.Units.AsNoTracking()
                                 where UnitIds.Contains(p.UnitId)
                                 select p).ToList();

                    var Dimension1s = (from p in db.Dimension1.AsNoTracking()
                                       where Dimension1Ids.Contains(p.Dimension1Id)
                                       select p).ToList();

                    var Dimension2s = (from p in db.Dimension2.AsNoTracking()
                                       where Dimension2Ids.Contains(p.Dimension2Id)
                                       select p).ToList();

                    var Dimension3s = (from p in db.Dimension3.AsNoTracking()
                                       where Dimension3Ids.Contains(p.Dimension3Id)
                                       select p).ToList();

                    var Dimension4s = (from p in db.Dimension4.AsNoTracking()
                                       where Dimension4Ids.Contains(p.Dimension4Id)
                                       select p).ToList();

                    var Processes = (from p in db.Process.AsNoTracking()
                                     where processIds.Contains(p.ProcessId)
                                     select p).ToList();

                    var ProductProcess = (from p in db.ProductProcess.AsNoTracking()
                                          where processIds.Contains(p.ProcessId) && Dimension1Ids.Contains(p.Dimension1Id)
                                          && Dimension2Ids.Contains(p.Dimension2Id) && Dimension3Ids.Contains(p.Dimension3Id)
                                          && Dimension4Ids.Contains(p.Dimension4Id) 
                                          && ProductIds.Contains(p.ProductId)
                                          select p).ToList();

                    var summary = (from t2 in prodorderlinefromprocedure
                                   where t2.Qty > 0
                                   group t2 by new { t2.ProductId, t2.Dimension1Id, t2.Dimension2Id, t2.Dimension3Id, t2.Dimension4Id, t2.ProcessId } into g
                                   join p1 in Products on g.Key.ProductId equals p1.ProductId
                                   join u1 in Units on p1.UnitId equals u1.UnitId
                                   join d1 in Dimension1s on g.Key.Dimension1Id equals d1.Dimension1Id into Dim1Table
                                   from DT1 in Dim1Table.DefaultIfEmpty()
                                   join d2 in Dimension2s on g.Key.Dimension2Id equals d2.Dimension2Id into Dim2Table
                                   from DT2 in Dim2Table.DefaultIfEmpty()
                                   join d3 in Dimension3s on g.Key.Dimension3Id equals d3.Dimension3Id into Dim3Table
                                   from DT3 in Dim3Table.DefaultIfEmpty()
                                   join d4 in Dimension4s on g.Key.Dimension4Id equals d4.Dimension4Id into Dim4Table
                                   from DT4 in Dim4Table.DefaultIfEmpty()
                                   join proc in Processes on g.Key.ProcessId equals proc.ProcessId into ProcTable
                                   from PT in ProcTable.DefaultIfEmpty()
                                   join PP in ProductProcess on new { A = g.Key.ProcessId ?? 0, B = g.Key.Dimension1Id ?? 0, D = g.Key.Dimension2Id ?? 0, E = g.Key.Dimension3Id ?? 0, F = g.Key.Dimension4Id ?? 0, g.Key.ProductId } equals new { A = PP.ProcessId ?? 0, B = PP.Dimension1Id ?? 0, D = PP.Dimension2Id ?? 0, E = PP.Dimension3Id ?? 0, F = PP.Dimension4Id ?? 0, PP.ProductId } into ProductProcessTable
                                   from ProductProcessTab in ProductProcessTable.DefaultIfEmpty()
                                   select new
                                   {
                                       id = g.Key.ProductId,
                                       name = p1.ProductName,
                                       PurchaseProduction = ProductProcessTab == null ? "Production" : (ProductProcessTab.PurchProd),
                                       dim1Id = g.Key.Dimension1Id,
                                       dim1Name = DT1 == null ? "" : (DT1.Dimension1Name),
                                       dim2Id = g.Key.Dimension2Id,
                                       dim2Name = DT2 == null ? "" : (DT2.Dimension2Name),
                                       dim3Id = g.Key.Dimension3Id,
                                       dim3Name = DT3 == null ? "" : (DT3.Dimension3Name),
                                       dim4Id = g.Key.Dimension4Id,
                                       dim4Name = DT4 == null ? "" : (DT4.Dimension4Name),
                                       procid = g.Key.ProcessId,
                                       procName = PT == null ? "" : (PT.ProcessName),
                                       //QtySum = Math.Ceiling(g.Sum(m => m.Qty)),
                                       QtySum = g.Sum(m => m.Qty),
                                       UnitName = u1.UnitName,
                                       DecimalPlaces = u1.DecimalPlaces,
                                       GroupedItems = g,
                                   }).ToList();






                    int j = 0;

                    List<MaterialPlanLineViewModel> Line = new List<MaterialPlanLineViewModel>();

                    foreach (var item in summary)
                    {
                        MaterialPlanLineViewModel planline = new MaterialPlanLineViewModel();
                        planline.ProductName = item.name;
                        planline.UnitName = item.UnitName;
                        planline.unitDecimalPlaces = item.DecimalPlaces;
                        planline.RequiredQty = item.QtySum;
                        planline.Dimension1Id = item.dim1Id;
                        planline.Dimension2Id = item.dim2Id;
                        planline.Dimension3Id = item.dim3Id;
                        planline.Dimension4Id = item.dim4Id;
                        planline.Dimension1Name = item.dim1Name;
                        planline.Dimension2Name = item.dim2Name;
                        planline.Dimension3Name = item.dim3Name;
                        planline.Dimension4Name = item.dim4Name;
                        planline.ProcessName = item.procName;

                        //using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString()))
                        using (SqlConnection sqlConnection = new SqlConnection((string)System.Web.HttpContext.Current.Session["DefaultConnectionString"]))
                        {
                            sqlConnection.Open();

                            int ProductCode = item.id;

                            string CommandText = "";
                            if (item.dim1Id != null && item.dim1Id != 0)
                            {
                                CommandText = "SELECT Web.FGetExcessStock_WithDimension( " + ProductCode + ", " + header.DocTypeId + ", " + item.dim1Id + ") ";
                            }
                            else{
                                CommandText = "SELECT Web.FGetExcessStock( " + ProductCode + ", " + header.DocTypeId + ")";
                            }
                            

                           //SqlCommand Totalf = new SqlCommand("SELECT Web.FGetExcessStock_WithDimension( " + ProductCode + ", " + header.DocTypeId + ", " + item.dim1Id + ") ", sqlConnection);
                            SqlCommand Totalf = new SqlCommand(CommandText, sqlConnection);
                            planline.ExcessStockQty = Convert.ToDecimal(Totalf.ExecuteScalar() == DBNull.Value ? 0 : Totalf.ExecuteScalar()); 
                        }
                       // planline.ExcessStockQty = 10;
                        planline.MaterialPlanHeaderId = vm.MaterialPlanLineViewModel.FirstOrDefault().MaterialPlanHeaderId;
                        planline.ProductId = item.id;
                        //planline.ProdPlanQty = item.QtySum;
                        planline.ProdPlanQty = (item.PurchaseProduction == "Purchase") ? 0 : item.QtySum;
                        planline.PurchPlanQty = (item.PurchaseProduction == "Purchase") ? item.QtySum : 0;
                        planline.GeneratedFor = MaterialPlanConstants.ProdOrder;
                        planline.ProcessId = item.procid;
                        Line.Add(planline);

                    }



                    //MaterialPlanSummaryViewModel Summary = new MaterialPlanSummaryViewModel();
                    MPSummary.MaterialPlanSettings = Mapper.Map<MaterialPlanSettings, MaterialPlanSettingsViewModel>(settings);
                    MPSummary.PlanLine = Line.OrderBy(m => m.ProductName).ThenBy(m => m.Dimension1Name).ThenByDescending(m => m.Dimension2Name).ThenByDescending(m => m.Dimension3Name).ThenByDescending(m => m.Dimension4Name).ToList();
                }


                return PartialView("_Summary", MPSummary);
            }
            return PartialView("_Results", vm);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _SummaryPost(MaterialPlanSummaryViewModel vm)
        {
            //db.Configuration.AutoDetectChangesEnabled = false;

            MaterialPlanHeader header = new MaterialPlanHeaderService(_unitOfWork).Find(vm.PlanLine.FirstOrDefault().MaterialPlanHeaderId);

            int Serial = new MaterialPlanLineService(_unitOfWork).GetMaxSr(header.MaterialPlanHeaderId);

            MaterialPlanSettings Settings = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(header.DocTypeId, header.DivisionId, header.SiteId);

            string sessionMaterialPlanLine = header.DocTypeId + "_" + header.MaterialPlanHeaderId + "_materialplanline";
            string sessionProdOrderBom = header.DocTypeId + "_" + header.MaterialPlanHeaderId + "_prodorderbom";


            MaterialPlanLineListViewModel svm = new MaterialPlanLineListViewModel();
            svm.MaterialPlanLineViewModel = (List<MaterialPlanForSaleOrderViewModel>)System.Web.HttpContext.Current.Session[sessionMaterialPlanLine];
            List<MaterialPlanForProcedureViewModel> prodOrderBom = (List<MaterialPlanForProcedureViewModel>)System.Web.HttpContext.Current.Session[sessionProdOrderBom];


            using (var context = new ApplicationDbContext())
            {
                bool isPr = false;
                bool isPP = false;
                int j = 0;
                foreach (var item in vm.PlanLine)
                {
                    {
                        MaterialPlanLine planLine = new MaterialPlanLine();
                        planLine.RequiredQty = item.RequiredQty;
                        planLine.ExcessStockQty = item.ExcessStockQty;
                        planLine.MaterialPlanHeaderId = item.MaterialPlanHeaderId;
                        planLine.ProductId = item.ProductId;
                        planLine.ProdPlanQty = item.ProdPlanQty;
                        planLine.CreatedBy = User.Identity.Name;
                        planLine.Dimension1Id = item.Dimension1Id;
                        planLine.Dimension2Id = item.Dimension2Id;
                        planLine.Dimension3Id = item.Dimension3Id;
                        planLine.Dimension4Id = item.Dimension4Id;
                        planLine.Sr = Serial++;
                        planLine.CreatedDate = DateTime.Now;
                        planLine.ModifiedBy = User.Identity.Name;
                        planLine.MaterialPlanLineId = j;
                        planLine.ModifiedDate = DateTime.Now;
                        planLine.ProcessId = item.ProcessId;
                        planLine.Remark = item.Remark;
                        planLine.PurchPlanQty = item.PurchPlanQty;
                        planLine.StockPlanQty = item.StockPlanQty;
                        planLine.GeneratedFor = MaterialPlanConstants.ProdOrder;
                        planLine.ObjectState = Model.ObjectState.Added;
                        context.MaterialPlanLine.Add(planLine);
                        if (!isPr)
                        {
                            if (item.ProdPlanQty > 0)
                                isPr = true;
                        }
                        if (!isPP)
                        {
                            if (item.PurchPlanQty > 0)
                                isPP = true;
                        }


                        ProductProcess PP = new ProductProcessService(_unitOfWork).FindByProductProcess(item.ProductId, item.ProcessId, item.Dimension1Id, item.Dimension2Id, item.Dimension3Id, item.Dimension4Id);
                        if (PP == null)
                        {
                            ProductProcess NewPP = new ProductProcess();
                            NewPP.ProcessId = (int?)item.ProcessId;
                            NewPP.ProductId = item.ProductId;
                            NewPP.Dimension1Id = (int?)item.Dimension1Id;
                            NewPP.Dimension2Id = (int?)item.Dimension2Id;
                            NewPP.Dimension3Id = (int?)item.Dimension3Id;
                            NewPP.Dimension4Id = (int?)item.Dimension4Id;
                            NewPP.PurchProd = (item.PurchPlanQty > 0 ? "Purchase" : "Production");
                            NewPP.CreatedBy = User.Identity.Name;
                            NewPP.CreatedDate = DateTime.Now;
                            NewPP.ModifiedBy = User.Identity.Name;
                            NewPP.ModifiedDate = DateTime.Now;
                            NewPP.ObjectState = Model.ObjectState.Added;
                            context.ProductProcess.Add(NewPP);
                        }

                    }
                    j++;
                }
                MaterialPlanHeader temp = new MaterialPlanHeaderService(_unitOfWork).Find(vm.PlanLine.FirstOrDefault().MaterialPlanHeaderId);


                int prodorderheaderid = 0;
                int prodorderlineid = 0;
                if (isPr)
                {



                    //ProdOrderHeader ExistingProdOrder = new ProdOrderHeaderService(_unitOfWork).GetProdOrderForMaterialPlan(header.MaterialPlanHeaderId);
                    ProdOrderHeader ExistingProdOrder = new ProdOrderHeaderService(_unitOfWork).GetPurchOrProdForMaterialPlan(header.MaterialPlanHeaderId, Settings.DocTypeProductionOrderId.Value);
                    int ProdOrderSr = 1;
                    if (ExistingProdOrder == null)
                    {


                        ProdOrderHeader prodOrderHeader = new ProdOrderHeader();
                        prodOrderHeader.ProdOrderHeaderId = prodorderheaderid--;
                        prodOrderHeader.CreatedBy = User.Identity.Name;
                        prodOrderHeader.CreatedDate = DateTime.Now;
                        prodOrderHeader.DivisionId = temp.DivisionId;
                        prodOrderHeader.DocDate = temp.DocDate;
                        prodOrderHeader.DocNo = temp.DocNo;
                        prodOrderHeader.DocTypeId = Settings.DocTypeProductionOrderId.Value;
                        prodOrderHeader.DueDate = temp.DueDate;
                        prodOrderHeader.MaterialPlanHeaderId = temp.MaterialPlanHeaderId;
                        prodOrderHeader.ModifiedBy = User.Identity.Name;
                        prodOrderHeader.ModifiedDate = DateTime.Now;
                        prodOrderHeader.Remark = temp.Remark;
                        prodOrderHeader.SiteId = temp.SiteId;
                        prodOrderHeader.BuyerId = temp.BuyerId;
                        //prodOrderHeader.Status = temp.Status;
                        prodOrderHeader.LockReason = "Prod order automatically generated from planning.";
                        prodOrderHeader.Status = header.Status;
                        prodOrderHeader.ObjectState = Model.ObjectState.Added;
                        context.ProdOrderHeader.Add(prodOrderHeader);

                        //ForCreating ProdOrderStatus
                        ProdOrderHeaderStatus pt = new ProdOrderHeaderStatus();
                        pt.ProdOrderHeaderId = prodOrderHeader.ProdOrderHeaderId;
                        pt.ObjectState = Model.ObjectState.Added;
                        context.ProdOrderHeaderStatus.Add(pt);

                        //int ProdOrderLineKey = 0;

                        foreach (var item in context.MaterialPlanLine.Local.Where(m => m.ProdPlanQty > 0))
                        {
                            ProdOrderLine prodOrderLine = new ProdOrderLine();
                            prodOrderLine.CreatedBy = User.Identity.Name;
                            prodOrderLine.CreatedDate = DateTime.Now;
                            prodOrderLine.MaterialPlanLineId = item.MaterialPlanLineId;
                            prodOrderLine.ModifiedBy = User.Identity.Name;
                            prodOrderLine.Dimension1Id = item.Dimension1Id;
                            prodOrderLine.Dimension2Id = item.Dimension2Id;
                            prodOrderLine.Dimension3Id = item.Dimension3Id;
                            prodOrderLine.Dimension4Id = item.Dimension4Id;
                            prodOrderLine.ProcessId = item.ProcessId;
                            prodOrderLine.Sr = ProdOrderSr++;
                            prodOrderLine.ModifiedDate = DateTime.Now;
                            prodOrderLine.ProdOrderHeaderId = prodOrderHeader.ProdOrderHeaderId;
                            prodOrderLine.ProductId = item.ProductId;
                            prodOrderLine.Qty = item.ProdPlanQty;
                            prodOrderLine.Remark = item.Remark;
                            prodOrderLine.LockReason = "Prod order automatically generated from planning.";
                            prodOrderLine.ProdOrderLineId = prodorderlineid--;
                            prodOrderLine.ObjectState = Model.ObjectState.Added;
                            context.ProdOrderLine.Add(prodOrderLine);

                            //ForAdding ProdrderLinestatus
                            ProdOrderLineStatus ptl = new ProdOrderLineStatus();
                            ptl.ProdOrderLineId = prodOrderLine.ProdOrderLineId;
                            ptl.ObjectState = Model.ObjectState.Added;
                            context.ProdOrderLineStatus.Add(ptl);

                        }

                    }
                    else
                    {
                        ProdOrderSr = new ProdOrderLineService(_unitOfWork).GetMaxSr(ExistingProdOrder.ProdOrderHeaderId);

                        //int ProdOrderLineKey = 0;

                        foreach (var item in context.MaterialPlanLine.Local.Where(m => m.ProdPlanQty > 0))
                        {
                            ProdOrderLine prodOrderLine = new ProdOrderLine();
                            prodOrderLine.CreatedBy = User.Identity.Name;
                            prodOrderLine.CreatedDate = DateTime.Now;
                            prodOrderLine.MaterialPlanLineId = item.MaterialPlanLineId;
                            prodOrderLine.Dimension1Id = item.Dimension1Id;
                            prodOrderLine.Dimension2Id = item.Dimension2Id;
                            prodOrderLine.Dimension3Id = item.Dimension3Id;
                            prodOrderLine.Dimension4Id = item.Dimension4Id;
                            prodOrderLine.ProcessId = item.ProcessId;
                            prodOrderLine.ModifiedBy = User.Identity.Name;
                            prodOrderLine.ModifiedDate = DateTime.Now;
                            prodOrderLine.Sr = ProdOrderSr++;
                            prodOrderLine.ProdOrderHeaderId = ExistingProdOrder.ProdOrderHeaderId;
                            prodOrderLine.ProductId = item.ProductId;
                            prodOrderLine.Qty = item.ProdPlanQty;
                            prodOrderLine.Remark = item.Remark;
                            prodOrderLine.LockReason = "Prod order automatically generated from planning.";
                            prodOrderLine.ProdOrderLineId = prodorderlineid--;
                            prodOrderLine.ObjectState = Model.ObjectState.Added;
                            context.ProdOrderLine.Add(prodOrderLine);

                            //ForAdding ProdrderLinestatus
                            ProdOrderLineStatus ptl = new ProdOrderLineStatus();
                            ptl.ProdOrderLineId = prodOrderLine.ProdOrderLineId;
                            ptl.ObjectState = Model.ObjectState.Added;
                            context.ProdOrderLineStatus.Add(ptl);

                        }
                    }
                }
                if (isPP)
                {

                    //PurchaseIndentHeader ExistingIndent = new PurchaseIndentHeaderService(_unitOfWork).GetPurchaseIndentForMaterialPlan(header.MaterialPlanHeaderId);
                    //ProdOrderHeader ExistingIndent = new ProdOrderHeaderService(_unitOfWork).GetPurchOrProdForMaterialPlan(header.MaterialPlanHeaderId, vm.MaterialPlanSettings.DocTypePurchaseIndentId ?? 0);
                    ProdOrderHeader ExistingIndent = new ProdOrderHeaderService(_unitOfWork).GetPurchOrProdForMaterialPlan(header.MaterialPlanHeaderId, Settings.DocTypePurchaseIndentId.Value);
                    int PurchaseIndentSr = 1;
                    if (ExistingIndent == null)
                    {

                        //PurchaseIndentHeader indentHeader = new PurchaseIndentHeader();
                        ProdOrderHeader indentHeader = new ProdOrderHeader();
                        indentHeader.ProdOrderHeaderId = prodorderheaderid--;
                        indentHeader.CreatedBy = User.Identity.Name;
                        indentHeader.CreatedDate = DateTime.Now;
                        indentHeader.DivisionId = temp.DivisionId;
                        indentHeader.DocDate = temp.DocDate;
                        indentHeader.DocNo = temp.DocNo;
                        indentHeader.DocTypeId = Settings.DocTypePurchaseIndentId.Value;
                        indentHeader.DueDate = header.DueDate;
                        indentHeader.MaterialPlanHeaderId = header.MaterialPlanHeaderId;
                        indentHeader.ModifiedBy = User.Identity.Name;
                        indentHeader.ModifiedDate = DateTime.Now;
                        indentHeader.Remark = temp.Remark;
                        indentHeader.SiteId = temp.SiteId;
                        //indentHeader.Status = temp.Status;
                        indentHeader.LockReason = "Purchase Indent automatically generated from planning.";
                        indentHeader.Status = header.Status;
                        //indentHeader.DepartmentId = (int)DepartmentConstants.Production;
                        indentHeader.ObjectState = Model.ObjectState.Added;
                        //context.PurchaseIndentHeader.Add(indentHeader);
                        context.ProdOrderHeader.Add(indentHeader);


                        //ForCreating ProdOrderStatus
                        ProdOrderHeaderStatus pt = new ProdOrderHeaderStatus();
                        pt.ProdOrderHeaderId = indentHeader.ProdOrderHeaderId;
                        pt.ObjectState = Model.ObjectState.Added;
                        context.ProdOrderHeaderStatus.Add(pt);

                        foreach (var item in context.MaterialPlanLine.Local.Where(m => m.PurchPlanQty > 0))
                        {
                            //PurchaseIndentLine indentLine = new PurchaseIndentLine();
                            ProdOrderLine indentLine = new ProdOrderLine();
                            indentLine.CreatedBy = User.Identity.Name;
                            indentLine.CreatedDate = DateTime.Now;
                            indentLine.MaterialPlanLineId = item.MaterialPlanLineId;
                            indentLine.ModifiedBy = User.Identity.Name;
                            indentLine.ModifiedDate = DateTime.Now;
                            indentLine.ProdOrderHeaderId = indentHeader.ProdOrderHeaderId;
                            indentLine.Specification = item.Specification;
                            indentLine.ProductId = item.ProductId;
                            indentLine.Dimension1Id = item.Dimension1Id;
                            indentLine.Dimension2Id = item.Dimension2Id;
                            indentLine.Dimension3Id = item.Dimension3Id;
                            indentLine.Dimension4Id = item.Dimension4Id;
                            //indentLine.ProcessId = item.ProcessId;
                            indentLine.ProcessId = new ProcessService(_unitOfWork).Find(ProcessConstants.Purchase).ProcessId;
                            indentLine.Sr = PurchaseIndentSr++;
                            indentLine.Qty = item.PurchPlanQty;
                            indentLine.Remark = item.Remark;
                            indentLine.LockReason = "Purchase Indent automatically generated from planning.";
                            indentLine.ProdOrderLineId = prodorderlineid--;
                            //indentLine.PurchaseIndentHeaderId = indentHeader.PurchaseIndentHeaderId;
                            indentLine.ObjectState = Model.ObjectState.Added;
                            //context.PurchaseIndentLine.Add(indentLine);
                            context.ProdOrderLine.Add(indentLine);

                            //ForAdding PurchaseIndentLinestatus
                            ProdOrderLineStatus ptl = new ProdOrderLineStatus();
                            ptl.ProdOrderLineId = indentLine.ProdOrderLineId;
                            ptl.ObjectState = Model.ObjectState.Added;
                            context.ProdOrderLineStatus.Add(ptl);
                        }
                    }
                    else
                    {
                        //PurchaseIndentSr = new PurchaseIndentLineService(_unitOfWork).GetMaxSr(ExistingIndent.PurchaseIndentHeaderId);
                        PurchaseIndentSr = new ProdOrderLineService(_unitOfWork).GetMaxSr(ExistingIndent.ProdOrderHeaderId);
                        foreach (var item in context.MaterialPlanLine.Local.Where(m => m.PurchPlanQty > 0))
                        {
                            //PurchaseIndentLine indentLine = new PurchaseIndentLine();
                            ProdOrderLine indentLine = new ProdOrderLine();
                            indentLine.CreatedBy = User.Identity.Name;
                            indentLine.CreatedDate = DateTime.Now;
                            indentLine.MaterialPlanLineId = item.MaterialPlanLineId;
                            indentLine.ModifiedBy = User.Identity.Name;
                            indentLine.ModifiedDate = DateTime.Now;
                            indentLine.ProdOrderHeaderId = ExistingIndent.ProdOrderHeaderId;
                            indentLine.Specification = item.Specification;
                            indentLine.ProductId = item.ProductId;
                            indentLine.Dimension1Id = item.Dimension1Id;
                            indentLine.Dimension2Id = item.Dimension2Id;
                            indentLine.Dimension3Id = item.Dimension3Id;
                            indentLine.Dimension4Id = item.Dimension4Id;
                            //indentLine.ProcessId = item.ProcessId;
                            indentLine.ProcessId = new ProcessService(_unitOfWork).Find(ProcessConstants.Purchase).ProcessId;
                            indentLine.Qty = item.PurchPlanQty;
                            indentLine.Remark = item.Remark;
                            indentLine.LockReason = "Purchase Indent automatically generated from planning.";
                            indentLine.Sr = PurchaseIndentSr++;
                            //indentLine.PurchaseIndentHeaderId = ExistingIndent.PurchaseIndentHeaderId;
                            indentLine.ProdOrderLineId = prodorderlineid--;
                            indentLine.ObjectState = Model.ObjectState.Added;
                            //context.PurchaseIndentLine.Add(indentLine);
                            context.ProdOrderLine.Add(indentLine);

                            //ForAdding ProdrderLinestatus
                            ProdOrderLineStatus ptl = new ProdOrderLineStatus();
                            ptl.ProdOrderLineId = indentLine.ProdOrderLineId;
                            ptl.ObjectState = Model.ObjectState.Added;
                            context.ProdOrderLineStatus.Add(ptl);
                        }
                    }
                }


                int i = 0;
                int MPFPOSr = new MaterialPlanForProdOrderService(_unitOfWork).GetMaxSr(header.MaterialPlanHeaderId);
                foreach (var item in svm.MaterialPlanLineViewModel)
                {
                    if (item.Qty > 0)
                    {
                        MaterialPlanForProdOrder order = new MaterialPlanForProdOrder();
                        order.MaterialPlanHeaderId = item.MaterialPlanHeaderId;
                        order.Qty = item.Qty;
                        order.ProdOrderLineId = item.ProdOrderLineId;
                        order.MaterialPlanForProdOrderId = i;
                        order.CreatedBy = User.Identity.Name;
                        order.CreatedDate = DateTime.Now;
                        order.ModifiedBy = User.Identity.Name;
                        order.ModifiedDate = DateTime.Now;
                        order.Sr = MPFPOSr++;
                        order.ObjectState = Model.ObjectState.Added;
                        context.MaterialPlanForProdOrder.Add(order);


                        var prodorderline = (from p in prodOrderBom
                                             where p.ProdOrderLineId == item.ProdOrderLineId
                                             select p);
                        int MPFPOLSr = 1;
                        foreach (var item2 in prodorderline)
                        {
                            MaterialPlanForProdOrderLine line = new MaterialPlanForProdOrderLine();
                            line.Qty = item2.Qty;
                            line.Dimension1Id = item2.Dimension1Id;
                            line.Dimension2Id = item2.Dimension2Id;
                            line.Dimension3Id = item2.Dimension3Id;
                            line.Dimension4Id = item2.Dimension4Id;
                            line.ProcessId = item2.ProcessId;
                            line.MaterialPlanForProdOrderId = order.MaterialPlanForProdOrderId;
                            line.ProductId = item2.ProductId;
                            line.Sr = MPFPOLSr++;
                            line.ObjectState = Model.ObjectState.Added;
                            //line.MaterialPlanLineId = context.MaterialPlanLine.Local.Where(m => m.ProductId == line.ProductId && m.Dimension1Id == line.Dimension1Id && m.Dimension2Id == line.Dimension2Id && m.Dimension3Id == line.Dimension3Id && m.Dimension4Id == line.Dimension4Id && m.ProcessId == line.ProcessId).FirstOrDefault().MaterialPlanLineId;
                            context.MaterialPlanForProdOrderLine.Add(line);
                        }

                        i++;

                    }
                }

                try
                {
                    //context.SaveChanges();
                    context.SaveChanges();
                    context.Configuration.AutoDetectChangesEnabled = true;
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    ModelState.AddModelError("", message);
                    var DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
                    var SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
                    //vm.MaterialPlanSettings = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(header.DocTypeId, DivisionId, SiteId);
                    var settings = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(header.DocTypeId, header.DivisionId, header.SiteId);
                    vm.MaterialPlanSettings = Mapper.Map<MaterialPlanSettings, MaterialPlanSettingsViewModel>(settings);
                    return PartialView("_Summary", vm);
                }


                System.Web.HttpContext.Current.Session.Remove(sessionProdOrderBom);
                System.Web.HttpContext.Current.Session.Remove(sessionMaterialPlanLine);
                return Json(new { success = true });
            }

        }


        public JsonResult GetProdOrders(int id, string term)//DocTypeId-->Changed To DocHeaderId
        {
            var DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            var SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

            MaterialPlanHeader Header = new MaterialPlanHeaderService(_unitOfWork).Find(id);

            var temp = new MaterialPlanSettingsService(_unitOfWork).GetMaterialPlanSettingsForDocument(Header.DocTypeId, DivisionId, SiteId);

            string ProcName = temp.PendingProdOrderList;

            return Json(new ProdOrderHeaderService(_unitOfWork).GetProdOrdersForDocumentType(term, id, ProcName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSaleOrders(int id, string term)//DocTypeId
        {
            return Json(new SaleOrderHeaderService(_unitOfWork).GetSaleOrdersForDocumentType(id, term), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSaleOrderStatus(int SaleOrderHeaderId)
        {
            Boolean Result = false;
            SaleOrderHeader SOH = new SaleOrderHeaderService(_unitOfWork).Find(SaleOrderHeaderId);
            if (SOH.Status == (int)StatusConstants.Approved)
            {
                Result = true;

            }

            else
            {
                Result = false;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ValidateSaleOrderHeaderId(List<string> SOHeaderId)
        {


            string SaleOrderHeaderList = "";

            if (SOHeaderId.Count > 0)
            {
                for (int i = 0; i < SOHeaderId.Count; i++)
                {
                    if (SaleOrderHeaderList == "")
                        SaleOrderHeaderList = SaleOrderHeaderList + SOHeaderId[i];
                    else
                        SaleOrderHeaderList = SaleOrderHeaderList + "," + SOHeaderId[i];
                }
            }

            string ValidationMsg = "";

            if (SaleOrderHeaderList != "")
                ValidationMsg = Validate_SaleOrder(SaleOrderHeaderList);


            if (ValidationMsg != "")
            {
                return Json(new { Error = ValidationMsg, Success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Error = ValidationMsg, Success = true }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetCustomProducts(string searchTerm, int pageSize, int pageNum, int filter)//DocTypeId
        {
            var Query = _MaterialPlanLineService.GetCustomProducts(filter, searchTerm);
            var temp = Query.Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();

            var count = Query.Count();

            ComboBoxPagedResult Data = new ComboBoxPagedResult();
            Data.Results = temp;
            Data.Total = count;

            return new JsonpResult
            {
                Data = Data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult GetCustomProductGroups(string searchTerm, int pageSize, int pageNum, int filter)//DocTypeId
        {
            var Query = _MaterialPlanLineService.GetCustomProductGroups(filter, searchTerm);
            var temp = Query.Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();

            var count = Query.Count();

            ComboBoxPagedResult Data = new ComboBoxPagedResult();
            Data.Results = temp;
            Data.Total = count;

            return new JsonpResult
            {
                Data = Data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public IEnumerable<MaterialPlanLineViewModel> GetMaterialPlanSummaryFromProcedure(List<MaterialPlanForProcedureViewModel> vm, string ConnectionString, string ProcName, int MaterialPlanHeaderId)
        {


            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ProductId");
            dataTable.Columns.Add("ProdOrderLineId");
            dataTable.Columns.Add("Dimension1Id");
            dataTable.Columns.Add("Dimension2Id");
            dataTable.Columns.Add("Dimension3Id");
            dataTable.Columns.Add("Dimension4Id");
            dataTable.Columns.Add("ProcessId");
            dataTable.Columns.Add("Qty");


            foreach (var item in vm)
            {
                var dr = dataTable.NewRow();
                dr["ProductId"] = item.ProductId;
                dr["ProdOrderLineId"] = item.ProdOrderLineId;
                dr["Dimension1Id"] = item.Dimension1Id;
                dr["Dimension2Id"] = item.Dimension2Id;
                dr["Dimension3Id"] = item.Dimension3Id;
                dr["Dimension4Id"] = item.Dimension4Id;
                dr["ProcessId"] = item.ProcessId;
                dr["Qty"] = item.Qty;
                dataTable.Rows.Add(dr);

            }
            DataSet ds = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                using (SqlCommand cmd = new SqlCommand(ProcName))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    cmd.Parameters.AddWithValue("@T", dataTable);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                }
            }

            DataTable dt2 = ds.Tables[0];


            List<MaterialPlanLineViewModel> temp = new List<MaterialPlanLineViewModel>();

            foreach (DataRow dr in dt2.Rows)
            {

                MaterialPlanLineViewModel line = new MaterialPlanLineViewModel();

                line.ProductId = dr["ProductId"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["ProductId"].ToString());
                line.ProcessId = dr["ProcessId"] == System.DBNull.Value ? null : (int?)Convert.ToInt32(dr["ProcessId"].ToString());
                line.Dimension1Id = dr["Dimension1Id"] == System.DBNull.Value ? null : (int?)Convert.ToInt32(dr["Dimension1Id"]);
                line.Dimension2Id = dr["Dimension2Id"] == System.DBNull.Value ? null : (int?)Convert.ToInt32(dr["Dimension2Id"]);
                line.Dimension3Id = dr["Dimension3Id"] == System.DBNull.Value ? null : (int?)Convert.ToInt32(dr["Dimension3Id"]);
                line.Dimension4Id = dr["Dimension4Id"] == System.DBNull.Value ? null : (int?)Convert.ToInt32(dr["Dimension4Id"]);


                line.ProductName = dr["ProductName"] == System.DBNull.Value ? null : dr["ProductName"].ToString();
                line.UnitName = dr["UnitName"] == System.DBNull.Value ? null : dr["UnitName"].ToString();
                line.unitDecimalPlaces = dr["DecimalPlaces"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["DecimalPlaces"].ToString());
                line.RequiredQty = dr["Qty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(dr["Qty"].ToString());
                line.Dimension1Name = dr["Dimension1Name"] == System.DBNull.Value ? null : dr["Dimension1Name"].ToString();
                line.Dimension2Name = dr["Dimension2Name"] == System.DBNull.Value ? null : dr["Dimension2Name"].ToString();
                line.Dimension3Name = dr["Dimension3Name"] == System.DBNull.Value ? null : dr["Dimension3Name"].ToString();
                line.Dimension4Name = dr["Dimension4Name"] == System.DBNull.Value ? null : dr["Dimension4Name"].ToString();
                line.ProcessName = dr["ProcessName"] == System.DBNull.Value ? null : dr["ProcessName"].ToString();


                line.ExcessStockQty = 0;
                line.MaterialPlanHeaderId = MaterialPlanHeaderId;


                line.ProdPlanQty = (dr["PurchProd"].ToString() == "Purchase") ? 0 : Convert.ToDecimal(dr["Qty"].ToString());
                line.PurchPlanQty = (dr["PurchProd"].ToString() == "Purchase") ? Convert.ToDecimal(dr["Qty"].ToString()) : 0;

                line.GeneratedFor = MaterialPlanConstants.ProdOrder;



                temp.Add(line);
            }

            return temp;

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
