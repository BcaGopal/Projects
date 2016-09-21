using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Model.Models;
using Data.Models;
using Service;
using Data.Infrastructure;
using Presentation.ViewModels;
using Presentation;
using Core.Common;
using Model.ViewModel;
using AutoMapper;
using System.Xml.Linq;

namespace Web
{
    [Authorize]
    public class ProductCategoryController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        //IGenericService<ProductCategory> _GenericService<ProductCategory>;
        IProductCategoryService _ProductCategoryService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;
        public ProductCategoryController(IProductCategoryService ProductCategoryService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _ProductCategoryService = ProductCategoryService;
            _unitOfWork = unitOfWork;
            _exception = exec;

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;
        }
        // GET: /ProductMaster/

        public ActionResult Index(int id)
        {
            var p = _ProductCategoryService.GetProductCategoryList(id);
            ViewBag.id = id;
            ViewBag.Name = new ProductTypeService(_unitOfWork).Find(id).ProductTypeName;
            return View(p);
        }
        public ActionResult ProductTypeIndex()
        {
            var producttype = new ProductTypeService(_unitOfWork).GetProductTypeListForCategory().ToList();
            return View("ProductTypeIndex", producttype);
        }


        // GET: /ProductMaster/Create

        public ActionResult Create(int id)
        {
            ProductCategory vm = new ProductCategory();
            vm.IsActive = true;
            vm.ProductTypeId = id;
            ViewBag.id = id;
            ViewBag.Name = new ProductTypeService(_unitOfWork).Find(vm.ProductTypeId).ProductTypeName;
            return View("Create", vm);

        }

        // POST: /ProductMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(ProductCategory vm)
        {
            ProductCategory pt = vm;
            if (ModelState.IsValid)
            {


                if (vm.ProductCategoryId <= 0)
                {
                    pt.CreatedDate = DateTime.Now;
                    pt.ModifiedDate = DateTime.Now;
                    pt.CreatedBy = User.Identity.Name;
                    pt.ModifiedBy = User.Identity.Name;
                    pt.ObjectState = Model.ObjectState.Added;
                    _ProductCategoryService.Create(pt);
                
                    try
                    {
                        _unitOfWork.Save();
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        ModelState.AddModelError("", message);
                        ViewBag.id = vm.ProductTypeId;
                        ViewBag.Name = new ProductTypeService(_unitOfWork).Find(vm.ProductTypeId).ProductTypeName;
                        return View("Create", vm);
                    }

                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.ProductCategry).DocumentTypeId,
                        DocId = pt.ProductCategoryId,
                        ActivityType = (int)ActivityTypeContants.Added,
                    }));
                    
                    return RedirectToAction("Create", new { id = pt.ProductTypeId }).Success("Data saved successfully");
                }
                else
                {
                    List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();
                    ProductCategory temp = _ProductCategoryService.Find(pt.ProductCategoryId);

                    ProductCategory ExRec = Mapper.Map<ProductCategory>(temp);

                    temp.ProductCategoryName = pt.ProductCategoryName;
                    temp.ProductTypeId = pt.ProductTypeId;
                    temp.IsActive = pt.IsActive;
                    temp.ModifiedDate = DateTime.Now;
                    temp.ModifiedBy = User.Identity.Name;
                    temp.ObjectState = Model.ObjectState.Modified;
                    _ProductCategoryService.Update(temp);


                    LogList.Add(new LogTypeViewModel
                    {
                        ExObj = ExRec,
                        Obj = temp,
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
                        ViewBag.id = vm.ProductTypeId;
                        ViewBag.Name = new ProductTypeService(_unitOfWork).Find(vm.ProductTypeId).ProductTypeName;
                        return View("Create", pt);
                    }


                    LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                    {
                        DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.ProductCategry).DocumentTypeId,
                        DocId = temp.ProductCategoryId,
                        ActivityType = (int)ActivityTypeContants.Modified,
                        xEModifications = Modifications,
                    }));
            
                    return RedirectToAction("Index", new { id = pt.ProductTypeId }).Success("Data saved successfully");
                }
            }

            ViewBag.id = vm.ProductTypeId;
            ViewBag.Name = new ProductTypeService(_unitOfWork).Find(vm.ProductTypeId).ProductTypeName;
            return View("Create", vm);

        }


        // GET: /ProductMaster/Edit/5

        public ActionResult Edit(int id)
        {
            ProductCategory pt = _ProductCategoryService.Find(id);
            if (pt == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = pt.ProductTypeId;
            ViewBag.Name = new ProductTypeService(_unitOfWork).Find(pt.ProductTypeId).ProductTypeName;
            return View("Create", pt);
        }


        // GET: /ProductMaster/Delete/5

        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory ProductCategory = _ProductCategoryService.Find(id);
            if (ProductCategory == null)
            {
                return HttpNotFound();
            }

            ReasonViewModel vm = new ReasonViewModel()
            {
                id = id,
            };

            return PartialView("_Reason", vm);
        }

        // POST: /ProductCategoryMaster/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(ReasonViewModel vm)
        {
            List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();
            if (ModelState.IsValid)
            {
                var temp = _ProductCategoryService.Find(vm.id);

                LogList.Add(new LogTypeViewModel
                {
                    ExObj = temp,
                });

                _ProductCategoryService.Delete(vm.id);
                XElement Modifications = new ModificationsCheckService().CheckChanges(LogList);
                try
                {
                    _unitOfWork.Save();
                }

                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    ModelState.AddModelError("", message);
                    return PartialView("_Reason", vm);
                }

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.ProductCategry).DocumentTypeId,
                    DocId = vm.id,
                    ActivityType = (int)ActivityTypeContants.Deleted,
                    UserRemark = vm.Reason,
                    xEModifications = Modifications,
                }));

                return Json(new { success = true });

            }
            return PartialView("_Reason", vm);
        }



        [HttpGet]
        public ActionResult NextPage(int id, int ptypeid)//CurrentHeaderId
        {
            var nextId = _ProductCategoryService.NextId(id, ptypeid);
            return RedirectToAction("Edit", new { id = nextId });
        }
        [HttpGet]
        public ActionResult PrevPage(int id, int ptypeid)//CurrentHeaderId
        {
            var nextId = _ProductCategoryService.PrevId(id, ptypeid);
            return RedirectToAction("Edit", new { id = nextId });
        }

        [HttpGet]
        public ActionResult History()
        {
            //To Be Implemented
            return View("~/Views/Shared/UnderImplementation.cshtml");
        }
        [HttpGet]
        public ActionResult Print()
        {
            //To Be Implemented
            return View("~/Views/Shared/UnderImplementation.cshtml");
        }
        [HttpGet]
        public ActionResult Email()
        {
            //To Be Implemented
            return View("~/Views/Shared/UnderImplementation.cshtml");
        }

        [HttpGet]
        public ActionResult Report()
        {

            DocumentType Dt = new DocumentType();
            Dt = new DocumentTypeService(_unitOfWork).FindByName(MasterDocTypeConstants.ProductCategry);

            return Redirect((string)System.Configuration.ConfigurationManager.AppSettings["CustomizeDomain"] + "/Report_ReportPrint/ReportPrint/?MenuId=" + Dt.ReportMenuId);

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
