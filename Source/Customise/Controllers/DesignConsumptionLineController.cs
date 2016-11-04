using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Model.Models;
using Data.Models;
using Service;
using Data.Infrastructure;
using Core.Common;
using Model.ViewModels;
using AutoMapper;
using Model.ViewModel;
using System.Xml.Linq;
using System
.Linq;

namespace Web
{

    [Authorize]
    public class DesignConsumptionLineController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        IBomDetailService _BomDetailService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;

        public DesignConsumptionLineController(IBomDetailService BomDetail, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _BomDetailService = BomDetail;
            _unitOfWork = unitOfWork;
            _exception = exec;

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;

        }

        [HttpGet]
        public JsonResult IndexForFaceContent(int id)
        {
            var p = _BomDetailService.GetDesignConsumptionFaceContentForIndex(id);
            return Json(p, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult IndexForOtherContent(int id)
        {
            var p = _BomDetailService.GetDesignConsumptionOtherContentForIndex(id);
            return Json(p, JsonRequestBehavior.AllowGet);

        }


        private void PrepareViewBag(DesignConsumptionLineViewModel svm)
        {
            var ProductFaceContentGroups = from p in db.Product
                                           join pg in db.ProductGroups on p.ReferenceDocId equals pg.ProductGroupId into ProductGroupTable
                                           from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                           join fp in db.FinishedProduct on ProductGroupTab.ProductGroupId equals fp.ProductGroupId into FinishedProductTable
                                           from FinishedProductTab in FinishedProductTable.DefaultIfEmpty()
                                           join pcl in db.ProductContentLine on FinishedProductTab.FaceContentId equals pcl.ProductContentHeaderId into ProductContentLineTable
                                           from ProductContentLineTab in ProductContentLineTable.DefaultIfEmpty()
                                           where p.ProductId == svm.BaseProductId && ((int?)ProductContentLineTab.ProductGroupId ?? 0) != 0
                                           group new { ProductContentLineTab } by new { ProductContentLineTab.ProductGroupId } into Result
                                           select new
                                           {
                                               ProductGroupId = Result.Key.ProductGroupId
                                           };

            var LastTrRec = (from L in db.BomDetail
                             join P in db.Product on L.ProductId equals P.ProductId into ProductTable
                             from ProductTab in ProductTable.DefaultIfEmpty()
                             join pcon in ProductFaceContentGroups on ProductTab.ProductGroupId equals pcon.ProductGroupId into ProductFaceContentTable
                             from ProductFaceContentTab in ProductFaceContentTable.DefaultIfEmpty()
                             where L.BaseProductId == svm.BaseProductId && ((int?)ProductFaceContentTab.ProductGroupId ?? 0) != 0
                             group new { L } by new { L.BaseProductId } into Result
                             select new
                             {
                                 TotalPercentage = Result.Sum(i => i.L.ConsumptionPer)
                             }).FirstOrDefault();

            if (LastTrRec != null)
                ViewBag.LastTransaction = LastTrRec.TotalPercentage + "% Consumption filled, " + (100 - LastTrRec.TotalPercentage) + " remaining.";
        }

        public ActionResult _Create(int Id) //Id ==>Sale Order Header Id
        {
            DesignConsumptionLineViewModel s = new DesignConsumptionLineViewModel();
            s.BaseProductId = Id;

            DesignConsumptionLineViewModel temp = _BomDetailService.GetBaseProductDetail(Id);

            s.DesignName = temp.DesignName;
            s.QualityName = temp.QualityName;
            s.Weight = temp.Weight;




            PrepareViewBag(s);
            return PartialView("_Create", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreatePost(DesignConsumptionLineViewModel svm)
        {
            if (ModelState.IsValid)
            {
                if (svm.BomDetailId == 0)
                {
                    BomDetail bomdetail = new BomDetail();

                    bomdetail.BaseProductId = svm.BaseProductId;
                    bomdetail.BatchQty = 1;
                    bomdetail.ConsumptionPer = svm.ConsumptionPer;
                    bomdetail.Dimension1Id = svm.Dimension1Id;
                    bomdetail.ProcessId = new ProcessService(_unitOfWork).Find(ProcessConstants.Weaving).ProcessId;
                    bomdetail.ProductId = svm.ProductId;
                    bomdetail.Qty = svm.Qty;

                    bomdetail.CreatedDate = DateTime.Now;
                    bomdetail.ModifiedDate = DateTime.Now;
                    bomdetail.CreatedBy = User.Identity.Name;
                    bomdetail.ModifiedBy = User.Identity.Name;
                    bomdetail.ObjectState = Model.ObjectState.Added;
                    _BomDetailService.Create(bomdetail);


                    if (bomdetail.BaseProductId == bomdetail.ProductId)
                    {
                        PrepareViewBag(svm);
                        //return View(svm).Danger(DataValidationMsg);
                        ModelState.AddModelError("", "Invalid Product is Selected!");
                        return PartialView("_Create", svm);
                    }
                    try
                    {
                        _unitOfWork.Save();
                    }
                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        ModelState.AddModelError("", message);
                        return PartialView("_Create", svm);
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.DesignConsumption).DocumentTypeId,
                        DocId = bomdetail.BomDetailId,
                        ActivityType = (int)ActivityTypeContants.Added,
                    }));

                    return RedirectToAction("_Create", new { id = svm.BaseProductId });
                }
                else
                {
                    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

                    BomDetail bomdetail = _BomDetailService.Find(svm.BomDetailId);

                    BomDetail ExRec = Mapper.Map<BomDetail>(bomdetail);

                    bomdetail.BaseProductId = svm.BaseProductId;
                    bomdetail.BatchQty = 1;
                    bomdetail.ConsumptionPer = svm.ConsumptionPer;
                    bomdetail.Dimension1Id = svm.Dimension1Id;
                    bomdetail.ProductId = svm.ProductId;
                    bomdetail.Qty = svm.Qty;
                   

                    bomdetail.ModifiedDate = DateTime.Now;
                    bomdetail.ModifiedBy = User.Identity.Name;
                    bomdetail.ObjectState = Model.ObjectState.Modified;
                    _BomDetailService.Update(bomdetail);

                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = ExRec,
                        Obj = bomdetail,
                    });
                    XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);
                    try
                    {
                        _unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        ModelState.AddModelError("", message);
                        return PartialView("_Create", svm);
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.DesignConsumption).DocumentTypeId,
                        DocId = bomdetail.BomDetailId,
                        ActivityType = (int)ActivityTypeContants.Modified,
                        xEModifications = Modifications,
                    }));

                    return Json(new { success = true });
                }
            }

            PrepareViewBag(svm);
            return PartialView("_Create", svm);
        }


        public ActionResult _Edit(int id)
        {
            DesignConsumptionLineViewModel s = _BomDetailService.GetDesignConsumptionLineForEdit(id);

            DesignConsumptionLineViewModel temp = _BomDetailService.GetBaseProductDetail(s.BaseProductId);

            s.DesignName = temp.DesignName;
            s.QualityName = temp.QualityName;
            s.Weight = temp.Weight;

            PrepareViewBag(s);

            if (s == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Create", s);
        }

        [HttpGet]
        public ActionResult _EditWithSKU(int id)
        {
            DesignConsumptionLineViewModel s = _BomDetailService.GetDesignConsumptionLineForEdit(id);

            DesignConsumptionLineViewModel temp = _BomDetailService.GetBaseProductDetail(s.BaseProductId);


            var product = db.Product.Find(s.BaseProductId);

            var pendingSKUConsumptionToUpdate = (from p in db.Product
                                                 join pg in db.ProductGroups on p.ProductGroupId equals pg.ProductGroupId
                                                 join pt in db.ProductTypes on pg.ProductTypeId equals pt.ProductTypeId
                                                 join bd1 in db.BomDetail on p.ProductId equals bd1.BaseProductId
                                                 join bd2 in db.BomDetail on bd1.ProductId equals bd2.BaseProductId
                                                 join prod in db.Product on bd2.ProductId equals prod.ProductId
                                                 join d1 in db.Dimension1 on bd2.Dimension1Id equals d1.Dimension1Id into table
                                                 from dtab in table.DefaultIfEmpty()
                                                 where pg.ProductGroupName == product.ProductName && bd2.Dimension1Id == s.Dimension1Id
                                                 && bd2.ProductId == s.ProductId
                                                 && bd2.BomDetailId != s.BomDetailId
                                                 group new { p, bd2, dtab, prod } by bd2.BomDetailId into g
                                                 orderby g.Max(m => m.p.ProductName)
                                                 select g.Select(m => new SKUBomViewModel { BaseProductName = m.p.ProductName, Shade = m.dtab.Dimension1Name, Qty = m.bd2.Qty, ProductName = m.prod.ProductName }).FirstOrDefault()).ToList();

            s.DesignName = temp.DesignName;
            s.QualityName = temp.QualityName;
            s.Weight = temp.Weight;
            s.SKUs = pendingSKUConsumptionToUpdate;
            PrepareViewBag(s);

            if (s == null)
            {
                return HttpNotFound();
            }
            return PartialView("_EditWithSKU", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _EditWithSKU(DesignConsumptionLineViewModel svm)
        {
            if (ModelState.IsValid)
            {

                List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

                BomDetail bomdetail = _BomDetailService.Find(svm.BomDetailId);

                int? ExistingDim1Id = bomdetail.Dimension1Id;
                int ExistingProductId = bomdetail.ProductId;

                BomDetail ExRec = Mapper.Map<BomDetail>(bomdetail);

                bomdetail.BaseProductId = svm.BaseProductId;
                bomdetail.Dimension1Id = svm.Dimension1Id;
                bomdetail.ProductId = svm.ProductId;

                bomdetail.ModifiedDate = DateTime.Now;
                bomdetail.ModifiedBy = User.Identity.Name;
                bomdetail.ObjectState = Model.ObjectState.Modified;
                _BomDetailService.Update(bomdetail);

                var product = db.Product.Find(bomdetail.BaseProductId);

                var pendingSKUConsumptionToUpdate = (from p in db.Product
                                                     join pg in db.ProductGroups on p.ProductGroupId equals pg.ProductGroupId
                                                     join pt in db.ProductTypes on pg.ProductTypeId equals pt.ProductTypeId
                                                     join bd1 in db.BomDetail on p.ProductId equals bd1.BaseProductId
                                                     join bd2 in db.BomDetail on bd1.ProductId equals bd2.BaseProductId
                                                     where pg.ProductGroupName == product.ProductName && bd2.Dimension1Id == ExistingDim1Id
                                                     && bd2.ProductId == ExistingProductId
                                                     && bd2.BomDetailId != bomdetail.BomDetailId
                                                     group bd2 by bd2.BomDetailId into g
                                                     orderby g.Key
                                                     select g.FirstOrDefault()).ToList();

                foreach (var item in pendingSKUConsumptionToUpdate)
                {
                    item.ProductId = bomdetail.ProductId;
                    item.Dimension1Id = bomdetail.Dimension1Id;
                    item.ObjectState = Model.ObjectState.Modified;

                    _BomDetailService.Update(item);
                }

                LogList.Add(new LogTypeViewModel
                {
                    ExObj = ExRec,
                    Obj = bomdetail,
                });
                XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);

                Modifications.Add("Updated in All SKU and Shades.");

                try
                {
                    _unitOfWork.Save();
                }

                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    ModelState.AddModelError("", message);
                    return PartialView("_EditWithSKU", svm);
                }

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.DesignConsumption).DocumentTypeId,
                    DocId = bomdetail.BomDetailId,
                    ActivityType = (int)ActivityTypeContants.Modified,
                    xEModifications = Modifications,
                }));

                return Json(new { success = true });

            }

            PrepareViewBag(svm);
            return PartialView("_EditWithSKU", svm);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BomDetail BomDetail = _BomDetailService.Find(id);
            if (BomDetail == null)
            {
                return HttpNotFound();
            }
            return View(BomDetail);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(DesignConsumptionLineViewModel vm)
        {
            List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

            BomDetail BomDetail = _BomDetailService.Find(vm.BomDetailId);
            LogList.Add(new LogTypeViewModel
            {
                ExObj = BomDetail,
            });

            _BomDetailService.Delete(vm.BomDetailId);
            XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);
            try
            {
                _unitOfWork.Save();
            }

            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                ModelState.AddModelError("", message);
                return PartialView("EditSize", vm);
            }

            LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
            {
                DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.DesignConsumption).DocumentTypeId,
                DocId = BomDetail.BomDetailId,
                ActivityType = (int)ActivityTypeContants.Deleted,
                xEModifications = Modifications,
            }));

            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetProductDetailJson(int ProductId)
        {
            ProductWithGroupAndUnit productgroupandunit = _BomDetailService.GetProductGroupAndUnit(ProductId);
            List<ProductWithGroupAndUnit> productgroupandunitJson = new List<ProductWithGroupAndUnit>();

            productgroupandunitJson.Add(new ProductWithGroupAndUnit()
            {
                ProductGroupId = productgroupandunit.ProductGroupId,
                ProductGroupName = productgroupandunit.ProductGroupName,
                UnitName = productgroupandunit.UnitName
            });

            return Json(productgroupandunitJson);
        }


        public JsonResult CheckForValidationinEdit(int ProductId, int? Dimension1Id, int BaseProductId, int BomDetailId)
        {
            var temp = (_BomDetailService.CheckForProductShadeExists(ProductId, Dimension1Id, BaseProductId, BomDetailId));
            return Json(new { returnvalue = temp });
        }

        public JsonResult CheckForValidation(int ProductId, int? Dimension1Id, int BaseProductId)
        {
            var temp = (_BomDetailService.CheckForProductShadeExists(ProductId, Dimension1Id, BaseProductId));
            return Json(new { returnvalue = temp });
        }

        public JsonResult GetConsumptionTotalQty(int BaseProductId, Decimal TotalWeight, Decimal BomQty, int BomDetailId)
        {
            var ProductFaceContentGroups = from p in db.Product
                                           join pg in db.ProductGroups on p.ReferenceDocId equals pg.ProductGroupId into ProductGroupTable
                                           from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                           join fp in db.FinishedProduct on ProductGroupTab.ProductGroupId equals fp.ProductGroupId into FinishedProductTable
                                           from FinishedProductTab in FinishedProductTable.DefaultIfEmpty()
                                           join pcl in db.ProductContentLine on FinishedProductTab.FaceContentId equals pcl.ProductContentHeaderId into ProductContentLineTable
                                           from ProductContentLineTab in ProductContentLineTable.DefaultIfEmpty()
                                           where p.ProductId == BaseProductId && ((int?)ProductContentLineTab.ProductGroupId ?? 0) != 0
                                           group new { ProductContentLineTab } by new { ProductContentLineTab.ProductGroupId } into Result
                                           select new
                                           {
                                               ProductGroupId = Result.Key.ProductGroupId
                                           };


            Decimal TotalFillQty = 0;
            var temp = (from L in db.BomDetail
                        join p in db.Product on L.ProductId equals p.ProductId into ProductTable
                        from ProductTab in ProductTable.DefaultIfEmpty()
                        join pcon in ProductFaceContentGroups on ProductTab.ProductGroupId equals pcon.ProductGroupId into ProductFaceContentTable
                        from ProductFaceContentTab in ProductFaceContentTable.DefaultIfEmpty()
                        where L.BaseProductId == BaseProductId && L.BomDetailId != BomDetailId && ((int?)ProductFaceContentTab.ProductGroupId ?? 0) != 0
                                    group (L) by (L.BaseProductId) into Result
                                    select new
                                    {
                                        TotalQty = Result.Sum(i => i.Qty)
                                    }).FirstOrDefault();

            if (temp != null)
            {
                TotalFillQty  = temp.TotalQty;
            }

            return Json(TotalFillQty);
        }

        public JsonResult IsProductContent(int BaseProductId, int ProductId)
        {
            bool IsContent = true;
            var ProductFaceContentGroups = from p in db.Product
                                           join pg in db.ProductGroups on p.ReferenceDocId equals pg.ProductGroupId into ProductGroupTable
                                           from ProductGroupTab in ProductGroupTable.DefaultIfEmpty()
                                           join fp in db.FinishedProduct on ProductGroupTab.ProductGroupId equals fp.ProductGroupId into FinishedProductTable
                                           from FinishedProductTab in FinishedProductTable.DefaultIfEmpty()
                                           join pcl in db.ProductContentLine on FinishedProductTab.FaceContentId equals pcl.ProductContentHeaderId into ProductContentLineTable
                                           from ProductContentLineTab in ProductContentLineTable.DefaultIfEmpty()
                                           where p.ProductId == BaseProductId && ((int?)ProductContentLineTab.ProductGroupId ?? 0) != 0
                                           group new { ProductContentLineTab } by new { ProductContentLineTab.ProductGroupId } into Result
                                           select new
                                           {
                                               ProductGroupId = Result.Key.ProductGroupId
                                           };


            var temp = (from p in db.Product 
                        join pcon in ProductFaceContentGroups on p.ProductGroupId equals pcon.ProductGroupId into ProductFaceContentTable
                        from ProductFaceContentTab in ProductFaceContentTable.DefaultIfEmpty()
                        where p.ProductId == ProductId && ((int?)ProductFaceContentTab.ProductGroupId ?? 0) != 0
                        select new
                        {
                            ProductId = p.ProductId
                        }).FirstOrDefault();

            if (temp != null)
            {
                IsContent = true;
            }
            else
            {
                IsContent = false;
            }

            return Json(IsContent);
        }

        
    }
}
