﻿using System.Web;
using System.Web.Mvc;

namespace Calculations
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequestLogFilter());
            filters.Add(new AuthenticationFilter());
        }
    }
}
