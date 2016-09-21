using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planning.Controllers
{
    [Authorize]
    public class HomeController : System.Web.Mvc.Controller
    {

        public ActionResult Index()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["MenuDomain"] == null)
                throw new Exception("Menu domain not configured in purchase Project");
            return Redirect((string)System.Configuration.ConfigurationManager.AppSettings["MenuDomain"] + "SiteSelection/SiteSelection");           
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}