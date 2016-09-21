using System.Web;
using System.Web.Mvc;
using Notifier.Hubs;
using System.Linq;
using System;
using Data.Models;
using Core.Common;

namespace Web
{
    [Authorize]
    public class NotificationController : System.Web.Mvc.Controller
    { 
    
        public ActionResult GetAllNotifications()
        {

            string UserName = User.Identity.Name;

            var temp = RegisterChanges.GetAllNotifications (UserName);

            return View("~/Views/Shared/Notifications.cshtml",temp);
        }

        public ActionResult NotificationRequest(int id)//NotificationId
        {

            var DefaultUrl = HttpContext.Request.UrlReferrer.ToString();

            string RetUrl = RegisterChanges.SetReadDate(id);

            if (string.IsNullOrEmpty(RetUrl))
            { return Redirect(DefaultUrl); }

            Uri rU = new Uri(RetUrl);

            string[] QueryParameters = HttpUtility.ParseQueryString(rU.Query).AllKeys;

            if (QueryParameters.Contains("SiteId") && QueryParameters.Contains("DivisionId"))
            {
                int SiteId = Int32.Parse(HttpUtility.ParseQueryString(rU.Query).Get("SiteId"));
                int DivisionId = Int32.Parse(HttpUtility.ParseQueryString(rU.Query).Get("DivisionId"));


                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    if (SiteId != 0)
                    {
                        System.Web.HttpContext.Current.Session["SiteId"] = SiteId;
                        System.Web.HttpContext.Current.Session[SessionNameConstants.SiteShortName] = db.Site.Find(SiteId).SiteCode;
                    }
                    if (DivisionId != 0)
                    {
                        System.Web.HttpContext.Current.Session["DivisionId"] = DivisionId;
                        System.Web.HttpContext.Current.Session[SessionNameConstants.DivisionName] = db.Divisions.Find(DivisionId).DivisionName;
                    }

                }

                var ParamCollection = HttpUtility.ParseQueryString(rU.Query);
                ParamCollection.Remove("SiteId");
                ParamCollection.Remove("DivisionId");

                UriBuilder r = new UriBuilder(RetUrl);
                r.Query = ParamCollection.ToString();

                RetUrl = r.ToString();
            }

            return Redirect(RetUrl);

        }

        public ActionResult UpdateNotificationSessionCount(int Count)
        {
            System.Web.HttpContext.Current.Session[SessionNameConstants.UserNotificationCount] = Count;

            return Json(true);
        }

    }
}
