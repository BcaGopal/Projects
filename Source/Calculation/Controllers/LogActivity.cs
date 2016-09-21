using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.Models;
using Data.Models;
using Service;
using Data.Infrastructure;
using Presentation.ViewModels;
using Presentation;
using Core.Common;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using Model.ViewModel;
using System.Xml.Linq;

namespace Web
{
    [Authorize]
    public class LogActivity
    {
        public static void LogActivityDetail(int? DocTypeId, int DocId, int? DocLineId, int ActivityType, string UserRemark, string User, string DocName, XElement Mods)
        {
            ActivityLog log = new ActivityLog()
            {
                DocTypeId = DocTypeId,
                DocLineId = DocLineId,
                ActivityType = ActivityType,
                CreatedBy = User,
                Modifications = Mods != null ? Mods.ToString() : "",
                CreatedDate = DateTime.Now,
                DocId = DocId,
            };
            if (ActivityType == (int)ActivityTypeContants.Added)
            {
                log.Narration = "DocNo: " + DocName;
            }
            else if (ActivityType == (int)ActivityTypeContants.Modified)
            {
                log.Narration = "DocNo: " + DocName;
                log.UserRemark = UserRemark;
            }
            else if (ActivityType == (int)ActivityTypeContants.Submitted)
            {
                log.UserRemark = UserRemark;
                log.Narration = "DocNo: " + DocName;
            }
            else if (ActivityType == (int)ActivityTypeContants.Approved)
            {
                log.Narration = "DocNo: " + DocName;
                log.UserRemark = UserRemark;
            }
            else if (ActivityType == (int)ActivityTypeContants.ModificationSumbitted)
            {
                log.Narration = "DocNo: " + DocName;
                log.UserRemark = UserRemark;
            }
            else if (ActivityType == (int)ActivityTypeContants.Deleted)
            {
                log.Narration = "DocNo: " + DocName;
                log.UserRemark = UserRemark;
            }

            log.ObjectState = Model.ObjectState.Added;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.ActivityLog.Add(log);
                context.SaveChanges();
            }
            

        }    
      
    }

    public class ActivityLogController : System.Web.Mvc.Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        IActivityLogService _ActivityLogService;
        IUnitOfWork _unitOfWork;
        IExceptionHandlingService _exception;
        public ActivityLogController(IActivityLogService ActivityLogService, IUnitOfWork unitOfWork, IExceptionHandlingService exec)
        {
            _ActivityLogService = ActivityLogService;
            _unitOfWork = unitOfWork;
            _exception = exec;
        }

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
