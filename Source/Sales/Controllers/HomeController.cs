using System;
using System.Web.Mvc;

namespace Sales.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["MenuDomain"] == null)
                throw new Exception("Menu domain not configured in Sales Project");
            return Redirect((string)System.Configuration.ConfigurationManager.AppSettings["MenuDomain"] + "SiteSelection/SiteSelection");
        }

      
    }
}