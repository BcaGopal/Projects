using System;
using System.Web.Mvc;
using Model.Models;
using Service;
using Core.Common;
using Model.ViewModel;
using System.Xml.Linq;

namespace Web
{
 

    public class ActivityLogController : System.Web.Mvc.Controller
    {
        public ActionResult LogEditReason()
        {            
            return PartialView("~/Views/Shared/_Reason.cshtml");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostLogReason(ActivityLogForEditViewModel vm)
        {
            if(ModelState.IsValid)
            {            
                return Json(new { success = true,UserRemark=vm.UserRemark });
            }
            return PartialView("~/Views/Shared/_Reason.cshtml", vm);
        }
    }
}
