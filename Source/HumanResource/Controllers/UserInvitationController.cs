using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Model.Models;
using Service;
using Microsoft.AspNet.Identity;
using Presentation;
using Core.Common;
using Model.ViewModel;
using EmailContents;
using System.Threading.Tasks;

namespace Web
{
    [Authorize]
    public class UserInvitationController : System.Web.Mvc.Controller
    {
        ActiivtyLogViewModel LogVm = new ActiivtyLogViewModel();

        IUserReferralService _UserReferralService;
        IEmployeeService _employeeService;
        IExceptionHandlingService _exception;
        public UserInvitationController(IUserReferralService UserRefService, IExceptionHandlingService exec, IEmployeeService empService)
        {
            _UserReferralService = UserRefService;
            _employeeService = empService;
            _exception = exec;

            //Log Initialization
            LogVm.SessionId = 0;
            LogVm.ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            LogVm.ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            LogVm.User = System.Web.HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.Name;
        }

        [HttpGet]
        public ActionResult SendInvites()
        {
            List<string> UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];
            var EmpId = _employeeService.GetEmloyeeForUser(User.Identity.GetUserId());

            if (UserRoles.Contains("Admin") || (EmpId.HasValue && EmpId.Value > 0))
            {
                ViewBag.UserTypeList = _UserReferralService.GetUserTypes();
                ViewBag.UserRoleList = _UserReferralService.GetUserRolesList();
                return View();
            }

            return Redirect((string)System.Web.HttpContext.Current.Session[SessionNameConstants.MenuDomain] + "/Menu/Module/").Warning("Must be an employee to send User Invites.");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendInvites(string UserEmails, string UserType, string UserRole)
        {

            if (string.IsNullOrEmpty(UserEmails))
            {
                ModelState.AddModelError("UserEmails", "The User email field is required");
                return View();
            }

            int AppId = (int)System.Web.HttpContext.Current.Session["ApplicationId"];
            string RefreeId = User.Identity.GetUserId();

            string USerEmail = _UserReferralService.GetUserEmail(RefreeId);

            bool error = false;
            string ErrorMsg = "";
            foreach (var user in UserEmails.Split(','))
            {

                UserReferral uref = _UserReferralService.Create(User.Identity.Name, user, UserRole);

                
                try
                {
                    await SendRegisterInvitation(uref, AppId, RefreeId, user, UserType, USerEmail);
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                    error = true;
                }
            }

            if (error)
                return Redirect((string)System.Web.HttpContext.Current.Session[SessionNameConstants.MenuDomain] + "/Menu/Module/").Success(ErrorMsg);

            return Redirect((string)System.Web.HttpContext.Current.Session[SessionNameConstants.MenuDomain] + "/Menu/Module/").Success("Invite sent successfully");
        }

        private async Task SendRegisterInvitation(UserReferral uref, int AppId, string RefreeId, string user, string UserType, string CC)
        {
            RegistrationInvitaionEmail riemail = new RegistrationInvitaionEmail();
            await riemail.SendUserRegistrationInvitation(user, AppId, RefreeId, uref.UserReferralId.ToString(), UserType, CC);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _UserReferralService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
