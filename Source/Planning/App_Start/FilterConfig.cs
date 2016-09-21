using Planning.Controller;
using System.Web;
using System.Web.Mvc;

namespace Planning
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomHandleErrorAttribute());
            filters.Add(new AuthenticationFilter());
        }
    }
}
