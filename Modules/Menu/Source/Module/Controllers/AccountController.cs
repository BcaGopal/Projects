using System;
using System.Web;
// New namespace imports:
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Web.Security;
using ProjLib.Constants;
namespace AdminSetup.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            string HomeUrl = (string)System.Web.HttpContext.Current.Session[SessionNameConstants.LoginDomain];

            if (string.IsNullOrEmpty(HomeUrl))
            {
                throw new Exception("Login Domain is not set in Modules Project");
            }

            AuthenticationManager.SignOut();
            FormsAuthentication.SignOut();

            Session.Abandon();
            return Redirect(HomeUrl);
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return System.Web.HttpContext.Current.GetOwinContext().Authentication;
            }
        }
        #endregion
    }

}
