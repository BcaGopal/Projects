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
using Model.ViewModels;
using Presentation.Helper;

namespace Web
{
    [Authorize]
    public class SaleEnquiryProductMappingController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        ISaleEnquiryLineService _SaleEnquiryLineService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;
        public SaleEnquiryProductMappingController(ISaleEnquiryLineService SaleEnquiryLineService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _SaleEnquiryLineService = SaleEnquiryLineService;
            _unitOfWork = unitOfWork;
            _exception = exec;

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;
        }
        // GET: /SaleEnquiryProductMappingMaster/

        public ActionResult Index()
        {
            var SaleEnquiryProductMapping = _SaleEnquiryLineService.GetSaleEnquiryLineListForIndex().ToList();
            return View(SaleEnquiryProductMapping);
            //return RedirectToAction("Create");
        }

        // GET: /SaleEnquiryProductMappingMaster/Create

        

        // POST: /ProductMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(SaleEnquiryLineViewModel vm)
        {
            if (ModelState.IsValid)
            {
                List<LogTypeViewModel> LogList = new List<LogTypeViewModel>();

                SaleEnquiryLine temp = _SaleEnquiryLineService.Find(vm.SaleEnquiryLineId);

                SaleEnquiryLine ExRec = Mapper.Map<SaleEnquiryLine>(temp);

                temp.ProductId = vm.ProductId;
                temp.ModifiedDate = DateTime.Now;
                temp.ModifiedBy = User.Identity.Name;
                temp.ObjectState = Model.ObjectState.Modified;
                _SaleEnquiryLineService.Update(temp);

                ProductBuyer PB = new ProductBuyerService(_unitOfWork).Find((int)vm.SaleToBuyerId, (int)vm.ProductId);
                if (PB == null)
                {


                    string BuyerSku = vm.ProductGroup.Replace("-", "") + "-" + vm.Size + "-" + vm.Colour;

                    ProductBuyer ProdBuyer = new ProductBuyer();
                    ProdBuyer.BuyerId = (int)vm.SaleToBuyerId;
                    ProdBuyer.ProductId = (int)vm.ProductId;
                    ProdBuyer.BuyerSku = BuyerSku;
                    ProdBuyer.BuyerSpecification = vm.ProductGroup;
                    ProdBuyer.BuyerSpecification1 = vm.Size;
                    ProdBuyer.BuyerSpecification2 = vm.Colour;
                    ProdBuyer.BuyerSpecification3 = vm.ProductQuality;
                    ProdBuyer.CreatedDate = DateTime.Now;
                    ProdBuyer.CreatedBy = User.Identity.Name;
                    ProdBuyer.ModifiedDate = DateTime.Now;
                    ProdBuyer.ModifiedBy = User.Identity.Name;
                    ProdBuyer.ObjectState = Model.ObjectState.Added;
                    new ProductBuyerService(_unitOfWork).Create(ProdBuyer);
                }

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
                    return View("Create", vm);
                }

                LogActivity.LogActivityDetail(LogVm.Map(new ActiivtyLogViewModel
                {
                    DocTypeId = new DocumentTypeService(_unitOfWork).FindByName(TransactionDoctypeConstants.SaleEnquiryProductMapping).DocumentTypeId,
                    DocId = temp.SaleEnquiryLineId,
                    ActivityType = (int)ActivityTypeContants.Modified,
                    xEModifications = Modifications,
                }));

                return RedirectToAction("Index").Success("Data saved successfully");


            }
            return View("Create", vm);
        }


        // GET: /ProductMaster/Edit/5

        public ActionResult Edit(int id)
        {
            SaleEnquiryLine temp = _SaleEnquiryLineService.GetSaleEnquiryLine(id);
            SaleEnquiryLineExtended Extended = new SaleEnquiryLineExtendedService(_unitOfWork).Find(id);


            if (temp == null)
            {
                return HttpNotFound();
            }


            SaleEnquiryHeader H = new SaleEnquiryHeaderService(_unitOfWork).GetSaleEnquiryHeader(temp.SaleEnquiryHeaderId);
            ViewBag.DocNo = H.DocNo;
            SaleEnquiryLineViewModel s = Mapper.Map<SaleEnquiryLine, SaleEnquiryLineViewModel>(temp);

            s.ProductGroup = Extended.ProductGroup;
            s.Size = Extended.Size;
            s.ProductQuality = Extended.ProductQuality;
            s.Colour = Extended.Colour;
            s.SaleEnquiryDocNo = H.DocNo;
            s.SaleToBuyerId = H.SaleToBuyerId;
            s.SaleEnquiryHeaderId = H.SaleEnquiryHeaderId;



            var settings = new SaleEnquirySettingsService(_unitOfWork).GetSaleEnquirySettings(H.DocTypeId, H.DivisionId, H.SiteId);
            s.SaleEnquirySettings = Mapper.Map<SaleEnquirySettings, SaleEnquirySettingsViewModel>(settings);


            return View("Create", s);
        }     

        // GET: /ProductMaster/Delete/5




        [HttpGet]
        public ActionResult NextPage(int id)
        {
            var nextId = _SaleEnquiryLineService.NextId(id);
            return RedirectToAction("Edit", new { id = nextId });
        }
        [HttpGet]
        public ActionResult PrevPage(int id)
        {
            var nextId = _SaleEnquiryLineService.PrevId(id);
            return RedirectToAction("Edit", new { id = nextId });
        }


        public ActionResult GetCustomProducts(string searchTerm, int pageSize, int pageNum, int filter)//DocTypeId
        {
            var Query = GetCustomProductsHelpList(filter, searchTerm);
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

        public IQueryable<ComboBoxResult> GetCustomProductsHelpList(int Id, string term)
        {

            var SaleEnquiry = new SaleEnquiryHeaderService(_unitOfWork).Find(Id);

            var settings = new SaleEnquirySettingsService(_unitOfWork).GetSaleEnquirySettings(SaleEnquiry.DocTypeId, SaleEnquiry.DivisionId, SaleEnquiry.SiteId);

            string[] ProductTypes = null;
            if (!string.IsNullOrEmpty(settings.filterProductTypes)) { ProductTypes = settings.filterProductTypes.Split(",".ToCharArray()); }
            else { ProductTypes = new string[] { "NA" }; }

            string[] Products = null;
            if (!string.IsNullOrEmpty(settings.filterProducts)) { Products = settings.filterProducts.Split(",".ToCharArray()); }
            else { Products = new string[] { "NA" }; }

            string[] ProductGroups = null;
            if (!string.IsNullOrEmpty(settings.filterProductGroups)) { ProductGroups = settings.filterProductGroups.Split(",".ToCharArray()); }
            else { ProductGroups = new string[] { "NA" }; }

            return (from Pt in db.FinishedProduct
                    join Vrs in db.ViewRugSize on Pt.ProductId equals Vrs.ProductId into ViewRugSizeTable from ViewRugSizeTab in ViewRugSizeTable.DefaultIfEmpty()
                    where (string.IsNullOrEmpty(settings.filterProductTypes) ? 1 == 1 : ProductTypes.Contains(Pt.ProductGroup.ProductTypeId.ToString()))
                    && (string.IsNullOrEmpty(settings.filterProducts) ? 1 == 1 : Products.Contains(Pt.ProductId.ToString()))
                    && (string.IsNullOrEmpty(settings.filterProductGroups) ? 1 == 1 : ProductGroups.Contains(Pt.ProductGroupId.ToString()))
                    && (string.IsNullOrEmpty(term) ? 1 == 1 : Pt.ProductName.ToLower().Contains(term.ToLower()))
                    orderby Pt.ProductName
                    select new ComboBoxResult
                    {
                        id = Pt.ProductId.ToString(),
                        text = Pt.ProductName,
                        AProp1 = "Design : " + Pt.ProductGroup.ProductGroupName,
                        AProp2 = "Size : " + ViewRugSizeTab.StandardSizeName,
                        TextProp1 = "Colour : " + Pt.Colour.ColourName,
                        TextProp2 = "Quality : " + Pt.ProductQuality.ProductQualityName
                    });
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