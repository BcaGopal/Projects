using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Service;
using System.Net;
using System.Web.Security;
using AdminSetup.Models.ViewModels;
using ProjLib.Constants;
using ProjLib.ViewModels;
using Models.Company.Models;
using Models.BasicSetup.ViewModels;

namespace Module
{
    [Authorize]
    public class SiteSelectionController : Controller
    {
        private readonly ISiteSelectionService _siteSelectionService;
        private readonly IUserRolesService _userRolesService;
        private readonly IModuleService _moduleService;
        private readonly IUserBookMarkService _userBookMarkService;
        private readonly IRolesControllerActionService _rolesControllerAcitonService;
        public SiteSelectionController(ISiteSelectionService SiteSelectionServ, IUserRolesService userRolesService, IModuleService moduleServ,
            IUserBookMarkService userBookmarkServ, IRolesControllerActionService rolesControllerActServ)
        {
            _siteSelectionService = SiteSelectionServ;
            _userRolesService = userRolesService;
            _moduleService = moduleServ;
            _userBookMarkService = userBookmarkServ;
            _rolesControllerAcitonService = rolesControllerActServ;
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult SiteSelection()
        {
            //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            string UserId = User.Identity.GetUserId();

            var userInRoles = _userRolesService.GetUserRolesList(UserId);           

            if (userInRoles.Count() <= 0)
            {
                AuthenticationManager.SignOut();
                FormsAuthentication.SignOut();
                Session.Abandon();
                return View("NoRoles");
            }

            SiteSelectionViewModel vm = new SiteSelectionViewModel();

            AssignSession();

            IEnumerable<string> UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];

            //Testing Block

            var temp = _userRolesService.GetRolesList().ToList();

            var RoleIds = string.Join(",", from p in temp
                                           where UserRoles.Contains(p.Name)
                                           select p.Id.ToString());
            //End

            if (UserRoles.Contains("Admin"))
            {

                ViewBag.SiteList = _siteSelectionService.GetSiteList().ToList();
                ViewBag.DivisionList = _siteSelectionService.GetDivisionList().ToList();
            }
            else
            {
                var SiteList = _siteSelectionService.GetSiteList(RoleIds).ToList();
                ViewBag.SiteList = SiteList;
                var DivList = _siteSelectionService.GetDivisionList(RoleIds).ToList();
                ViewBag.DivisionList = DivList;
                if (SiteList.Count == 0 || DivList.Count == 0)
                {
                    return RedirectToAction("AccessDenied", "Account");
                }
            }
            if (System.Web.HttpContext.Current.Session["DivisionId"] != null && System.Web.HttpContext.Current.Session["SiteId"] != null)
            {
                vm.DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
                vm.SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            }

            return View(vm);
        }


        private void AssignSession()
        {
            var UserId = User.Identity.GetUserId();

            #region Roles
            IEnumerable<string> UserRoles = _userRolesService.GetUserRolesForSession(UserId);

            System.Web.HttpContext.Current.Session["Roles"] = UserRoles;

            #endregion

            #region CompanyDetails
            System.Web.HttpContext.Current.Session["CompanyId"] = 1;
            System.Web.HttpContext.Current.Session["CompanyName"] = "SURYA CARPET PVT. LTD.";
            #endregion

            #region BookMarks
            Dictionary<int, string> bookmarks = new Dictionary<int, string>();
            var temp = _userBookMarkService.GetUserBookMarkListForUser(User.Identity.Name);
            foreach (var item in temp)
            {
                bookmarks.Add(item.MenuId, item.MenuName);
            }


            List<UserBookMarkViewModel> vm = new List<UserBookMarkViewModel>();
            foreach (var item in temp)
            {
                vm.Add(new UserBookMarkViewModel()
                {
                    IconName = item.IconName,
                    MenuId = item.MenuId,
                    MenuName = item.MenuName,
                });
            }

            System.Web.HttpContext.Current.Session["BookMarks"] = vm;
            #endregion

            #region Permissions
            if (!UserRoles.Contains("Admin"))
            {
                List<RolesControllerActionViewModel> Temp = _rolesControllerAcitonService.GetRolesControllerActionsForRoles(UserRoles.ToList()).ToList();

                System.Web.HttpContext.Current.Session["CAPermissionsCacheKeyHint"] = Temp.ToList();
            }

            #endregion

            #region NotificationCount

            System.Web.HttpContext.Current.Session[SessionNameConstants.UserNotificationCount] = _siteSelectionService.GetNotificationCount(User.Identity.Name);

            #endregion

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SiteSelection(SiteSelectionViewModel vm)
        {
            System.Web.HttpContext.Current.Session["DivisionId"] = vm.DivisionId;
            System.Web.HttpContext.Current.Session["SiteId"] = vm.SiteId;

            Site S = _siteSelectionService.GetSite(vm.SiteId);
            Division D = _siteSelectionService.GetDivision(vm.DivisionId);

            System.Web.HttpContext.Current.Session[SessionNameConstants.LoginDivisionId] = vm.DivisionId;
            System.Web.HttpContext.Current.Session[SessionNameConstants.LoginSiteId] = vm.SiteId;
            System.Web.HttpContext.Current.Session[SessionNameConstants.CompanyName] = S.SiteName;
            System.Web.HttpContext.Current.Session[SessionNameConstants.SiteName] = S.SiteName;
            System.Web.HttpContext.Current.Session[SessionNameConstants.SiteShortName] = S.SiteCode;
            System.Web.HttpContext.Current.Session[SessionNameConstants.SiteAddress] = S.Address;
            System.Web.HttpContext.Current.Session[SessionNameConstants.SiteCityName] = S.City.CityName;
            System.Web.HttpContext.Current.Session[SessionNameConstants.DivisionName] = D.DivisionName;

            List<string> UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];

            if (UserRoles.Contains("Admin"))
            {
                System.Web.HttpContext.Current.Session["UserModuleList"] = _moduleService.GetModuleList().ToList();
            }
            else
            {
                System.Web.HttpContext.Current.Session["UserModuleList"] = _moduleService.GetModuleListForUser(UserRoles.ToList(), vm.SiteId, vm.DivisionId).ToList();
            }


            return RedirectToAction("DefaultGodownSelection");
        }


        public ActionResult DefaultGodownSelection()
        {

            int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

            ViewBag.GodownList = _siteSelectionService.GetGodownList(SiteId);
            ViewBag.GodownId = (int?)System.Web.HttpContext.Current.Session["DefaultGodownId"];

            return View();
        }

        [HttpPost]
        public ActionResult DefaultGodownSelection(int? DefaultGodownId)
        {
            if (DefaultGodownId.HasValue && DefaultGodownId.Value > 0)
                System.Web.HttpContext.Current.Session["DefaultGodownId"] = DefaultGodownId;
            return RedirectToAction("Module", "Menu");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _siteSelectionService.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}