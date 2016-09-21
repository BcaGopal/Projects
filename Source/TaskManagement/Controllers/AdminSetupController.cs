using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjLib.ViewModels;

namespace Web
{
    [Authorize]
    public class AdminSetupController : System.Web.Mvc.Controller
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }


        public static class ValidateData
        {
            public static bool ValidateUserPermission(string ActionName, string ControllerName)
            {
                bool Temp = false;
                if (System.Web.HttpContext.Current.Session["CAPermissionsCacheKeyHint"] != null)
                {
                    var CacheData = (List<RolesControllerActionViewModel>)System.Web.HttpContext.Current.Session["CAPermissionsCacheKeyHint"];

                    Temp = (from p in CacheData
                                 where p.ControllerName == ControllerName && p.ControllerActionName==ActionName
                                 select p).Any();
                }
                return Temp;
            }
        }

    }


}
