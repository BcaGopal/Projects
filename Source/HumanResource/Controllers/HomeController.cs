﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanResource.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["MenuDomain"] == null)
                throw new Exception("Menu domain not configured in HR Project");
            return Redirect((string)System.Configuration.ConfigurationManager.AppSettings["MenuDomain"] + "SiteSelection/SiteSelection");
        }

  
    }
}