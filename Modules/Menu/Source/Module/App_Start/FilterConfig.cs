﻿using Modules;
using System.Web;
using System.Web.Mvc;

namespace Module
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthenticationFilter());
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}
