using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Owin;
using Model.Models;
using Data.Models;
using Service;
using Data.Infrastructure;
using Presentation.Helper;
using System.Configuration;
using Model.ViewModel;

namespace Presentation.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        IModuleService _ModuleService;
        ISubModuleService _SubModuleService;
        IUnitOfWork _unitOfWork;
        public MenuController(IModuleService mService, IUnitOfWork unitOfWork, ISubModuleService serv)
        {
            _ModuleService = mService;
            _unitOfWork = unitOfWork;
            _SubModuleService = serv;
        }

        
        //public ActionResult Module()
        //{
        //    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        //    List<string> UserRoles = UserManager.GetRoles(User.Identity.GetUserId()).ToList();
        //    MenuModuleViewModel Vm = new MenuModuleViewModel();
        //    List<MenuModule> ma = new List<MenuModule>();

            
        //    if (User.IsInRole("Admin"))
        //    {
        //        ma = _ModuleService.GetModuleList().ToList();
        //    }
        //    else
        //    {
        //        ma = _ModuleService.GetModuleListForUser(UserRoles).ToList();
        //    }
            
        //    Vm.MenuModule = ma;

        //    if(ma.Count()==1)
        //    {
        //        return RedirectToAction("SubModule", new { id = ma.FirstOrDefault().ModuleId });
        //    }
        //    else
        //    return View("Module", Vm);
        //}

        
        //public ActionResult UserPermissions(string RoleId)
        //{
        //    MenuModuleViewModel Vm = new MenuModuleViewModel();
        //    System.Web.HttpContext.Current.Session["RoleUId"] = RoleId;
        //    List<MenuModule> ma = new List<MenuModule>();
        //    if (User.IsInRole("Admin"))
        //    {
        //        ma = _ModuleService.GetModuleList().ToList();
        //    }
            
        //    if (!string.IsNullOrEmpty(RoleId) && (string)TempData["Validation"] == "Valid")
        //    { Vm.RoleId = RoleId; Vm.RoleModification = true; }
        //    Vm.MenuModule = ma;
        //    return View("Module", Vm);
        //}

        
        public ActionResult SubModule(int id,bool ? RolePerm)//ModuleId
        {

            //return Redirect("http://localhost:7120/PurchaseOrderHeader/DocumentTypeIndex/218?MenuId=30");            
           
            List<SubModuleViewModel> vm = new List<SubModuleViewModel>();

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            string appuserid = UserManager.FindByName(User.Identity.Name).Id;

            List<string> UserRoles = (List<string>)(System.Web.HttpContext.Current.Session["Roles"]);
            int DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            
           
            string RoleId = (string)System.Web.HttpContext.Current.Session["RoleUId"];

                      
            if(UserRoles.Contains("Admin"))
            {
                vm = _SubModuleService.GetSubModuleFromModule(id, null).ToList();
            }
            else
            {
                vm = _SubModuleService.GetSubModuleFromModuleForUsers(id, appuserid, UserRoles,SiteId,DivisionId).ToList();
            }
            MenuModule tem = _ModuleService.Find(id);
            ViewBag.MName = tem.ModuleName;
            ViewBag.IconName = tem.IconName;
            ViewBag.RolePermissions = RolePerm??false;
            
            return View("SubModule", vm);
        }

        
        public ActionResult MenuSelection(int id)//Controller ActionId
        {
            //ControllerAction ca = new ControllerActionService(_unitOfWork).Find(id);

            MenuViewModel menuviewmodel = new MenuService(_unitOfWork).GetMenu(id);


            if (menuviewmodel == null)
            {
                return View("~/Views/Shared/UnderImplementation.cshtml");
            }
            else
            {
                return RedirectToAction(menuviewmodel.ActionName, menuviewmodel.ControllerName, new { MenuId = menuviewmodel.MenuId, id = menuviewmodel.RouteId });
            }

        }

        
        public ActionResult DropDown(int id)//Menu Id
        {

            //int controlleractionid = new MenuService(_unitOfWork).Find(id).ControllerActionId;

            //ControllerAction ca = new ControllerActionService(_unitOfWork).Find(controlleractionid);

            //if (ca == null)
            //{
            //    return View("~/Views/Shared/UnderImplementation.cshtml");
            //}
            //else
            //{
            //    return RedirectToAction(ca.ActionName, ca.ControllerName);
            //}

            MenuViewModel menuviewmodel = new MenuService(_unitOfWork).GetMenu(id);


            if (menuviewmodel == null)
            {
                return View("~/Views/Shared/UnderImplementation.cshtml");
            }
            else
            {
                return RedirectToAction(menuviewmodel.ActionName, menuviewmodel.ControllerName, new { MenuId = menuviewmodel.MenuId, id = menuviewmodel.RouteId });
            }

        }


    }
}