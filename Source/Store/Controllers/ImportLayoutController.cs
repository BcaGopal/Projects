using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Service;
using Model.Models;
using Model.ViewModel;
using Model.ViewModels;

namespace Web
{
    [Authorize]
    public class ImportLayoutController : System.Web.Mvc.Controller
    {
        IImportLineService _ImportLineService;
        IImportHeaderService _ImportHeaderservice;
        public ImportLayoutController(IImportLineService line, IImportHeaderService ImportHeaderServ)
        {
            _ImportLineService = line;
            _ImportHeaderservice = ImportHeaderServ;
        }

        [HttpGet]
        public ActionResult ImportLayout(string name)
        {
            ImportHeader header = _ImportHeaderservice.GetImportHeaderByName(name);
            List<ImportLine> lines = _ImportLineService.GetImportLineList(header.ImportHeaderId).ToList();

            Dictionary<int, string> DefaultValues = TempData["ImportLayoutDefaultValues"] as Dictionary<int, string>;
            TempData["ImportLayoutDefaultValues"] = DefaultValues;
            foreach (var item in lines)
            {
                if (DefaultValues != null && DefaultValues.ContainsKey(item.ImportLineId))
                {
                    item.DefaultValue = DefaultValues[item.ImportLineId];
                }
            }

            ImportMasterViewModel vm = new ImportMasterViewModel();

            if (TempData["closeOnSelectOption"] != null)
                vm.closeOnSelect = (bool)TempData["closeOnSelectOption"];

            vm.ImportHeader = header;
            vm.ImportLine = lines;
            vm.ImportHeaderId = header.ImportHeaderId;

            return View(vm);
        }

        public JsonResult SetSelectOption(bool Checked)
        {
            TempData["closeOnSelectOption"] = Checked;
            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ImportHeaderservice.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
