using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Infrastructure;
using Model.Models;

using Core.Common;
using System;
using Model;
using System.Threading.Tasks;
using Data.Models;
using Model.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.SqlServer;

namespace Service
{
    public interface ISubModuleService : IDisposable
    {
        MenuSubModule Create(MenuSubModule pt);
        void Delete(int id);
        void Delete(MenuSubModule pt);
        MenuSubModule Find(string Name);
        MenuSubModule Find(int id);
        void Update(MenuSubModule pt);
        MenuSubModule Add(MenuSubModule pt);
        IEnumerable<MenuSubModule> GetSubModuleList();
        MenuSubModule GetSubModuleByName(string terms);
        IEnumerable<SubModuleViewModel> GetSubModuleFromModule(int id, string appuserid);//Module Id,Application User Id
        IEnumerable<SubModuleViewModel> GetSubModuleFromModuleForUsers(int id, string appuserid, List<string> RoleIds, int SiteId, int DivisionId);//Module Id,Application User Id
        IEnumerable<SubModuleViewModel> GetSubModuleFromModuleForPermissions(int id, string RoleId, int SiteId, int DivisionId);//Module Id,Application User Id
    }

    public class SubModuleService : ISubModuleService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<MenuSubModule> _SubModuleRepository;
        RepositoryQuery<MenuSubModule> SubModuleRepository;
        public SubModuleService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _SubModuleRepository = new Repository<MenuSubModule>(db);
            SubModuleRepository = new RepositoryQuery<MenuSubModule>(_SubModuleRepository);
        }
        public MenuSubModule GetSubModuleByName(string terms)
        {
            return (from p in db.MenuSubModule
                    where p.SubModuleName == terms
                    select p).FirstOrDefault();
        }

        public MenuSubModule Find(string Name)
        {
            return SubModuleRepository.Get().Where(i => i.SubModuleName == Name).FirstOrDefault();
        }

        public IEnumerable<SubModuleViewModel> GetSubModuleFromModule(int id, string appuserid)
        {
            //orderby SqlFunctions.StringConvert(double.Parse(temp.Srl))  






            var temp2 = (from p in db.Menu
                         where (p.IsVisible.HasValue ? p.IsVisible == true : 1 == 1) && p.ModuleId == id
                         group p by new { p.SubModuleId, p.SubModule.SubModuleName } into res
                         join t in db.MenuSubModule on res.Key.SubModuleId equals t.SubModuleId into table
                         from tab in table.DefaultIfEmpty()
                         select new SubModuleViewModel
                         {
                             ModuleId = id,
                             SubModuleIconName = tab.IconName,
                             SubModuleId = res.Key.SubModuleId,
                             SubModuleName = res.Key.SubModuleName,
                             Srl = res.Max(m => m.Srl),
                             MenuViewModel = (from temp in res
                                              orderby temp.Srl
                                              select new MenuViewModel
                                              {
                                                  MenuId = temp.MenuId,
                                                  MenuName = temp.MenuName,
                                                  ModuleId = temp.ModuleId,
                                                  SubModuleId = temp.SubModuleId,
                                                  ControllerActionId = temp.ControllerActionId,
                                                  Description = temp.Description,
                                                  IconName = temp.IconName,
                                                  Srl = temp.Srl,
                                                  URL = temp.URL,
                                                  BookMarked = ((from tek in db.UserBookMark
                                                                 where tek.ApplicationUserName == appuserid && tek.MenuId == temp.MenuId
                                                                 select tek).Any()
                                                                               ),

                                              }
                                         ).ToList()

                         }).ToList();



            double x = 0;
            var SubModuleList = temp2.OrderBy(m => m.SubModuleId).ThenBy(sx => double.TryParse(sx.Srl, out x) ? x : 0);



            return SubModuleList;

        }

        public IEnumerable<SubModuleViewModel> GetSubModuleFromModuleForUsers(int id, string appuserid, List<string> RoleIds, int SiteId, int DivisionId)
        {

            //Testing Block

            var Roles = (from p in db.Roles
                         select p).ToList();

            var RoleId = string.Join(",", from p in Roles
                                          where RoleIds.Contains(p.Name)
                                          select p.Id.ToString());
            //End


            var temp2 = (from p in db.Menu
                         join t in db.RolesMenu on p.MenuId equals t.MenuId
                         where (p.IsVisible.HasValue ? p.IsVisible == true : 1 == 1) && p.ModuleId == id && t.SiteId == SiteId && t.DivisionId == DivisionId && RoleId.Contains(t.RoleId)
                         group p by new { p.SubModuleId, p.SubModule.SubModuleName } into res
                         join t in db.MenuSubModule on res.Key.SubModuleId equals t.SubModuleId into table
                         from tab in table.DefaultIfEmpty()
                         select new SubModuleViewModel
                         {
                             ModuleId = id,
                             SubModuleIconName = tab.IconName,
                             SubModuleId = res.Key.SubModuleId,
                             SubModuleName = res.Key.SubModuleName,
                             Srl = res.Max(m => m.Srl),
                             MenuViewModel = (from temp in res
                                              group temp by temp.MenuId into g
                                              orderby g.Max(m => m.Srl)
                                              select new MenuViewModel
                                              {
                                                  MenuId = g.Key,
                                                  MenuName = g.FirstOrDefault().MenuName,
                                                  ModuleId = g.FirstOrDefault().ModuleId,
                                                  SubModuleId = g.FirstOrDefault().SubModuleId,
                                                  ControllerActionId = g.FirstOrDefault().ControllerActionId,
                                                  Description = g.FirstOrDefault().Description,
                                                  IconName = g.FirstOrDefault().IconName,
                                                  Srl = g.FirstOrDefault().Srl,
                                                  URL = g.FirstOrDefault().URL,
                                                  BookMarked = ((from tek in db.UserBookMark
                                                                 where tek.ApplicationUserName == appuserid && tek.MenuId == g.FirstOrDefault().MenuId
                                                                 select tek).Any()
                                                                               ),

                                              }
                                         ).ToList()

                         }).ToList();



            double x = 0;
            var SubModuleList = temp2.OrderBy(m => m.SubModuleId).ThenBy(sx => double.TryParse(sx.Srl, out x) ? x : 0);

            var tempSubModList = from p in SubModuleList
                                 group p by p.SubModuleId into g
                                 select g.FirstOrDefault();

            return tempSubModList;

        }



        public IEnumerable<SubModuleViewModel> GetSubModuleFromModuleForPermissions(int id, string RoleId, int SiteId, int DivisionId)
        {
            var temp2 = (from p in db.Menu
                         where (p.IsVisible.HasValue ? p.IsVisible == true : 1 == 1) && p.ModuleId == id
                         group p by new { p.SubModuleId, p.SubModule.SubModuleName } into res
                         join t in db.MenuSubModule on res.Key.SubModuleId equals t.SubModuleId into table
                         from tab in table.DefaultIfEmpty()
                         select new SubModuleViewModel
                         {
                             SubModuleIconName = tab.IconName,
                             ModuleId = id,
                             SubModuleId = res.Key.SubModuleId,
                             SubModuleName = res.Key.SubModuleName,
                             Srl = res.Max(m => m.Srl),
                             MenuViewModel = (from temp in res
                                              orderby temp.Srl
                                              select new MenuViewModel
                                              {
                                                  MenuId = temp.MenuId,
                                                  MenuName = temp.MenuName,
                                                  ModuleId = temp.ModuleId,
                                                  SubModuleId = temp.SubModuleId,
                                                  ControllerActionId = temp.ControllerActionId,
                                                  Description = temp.Description,
                                                  IconName = temp.IconName,
                                                  Srl = temp.Srl,
                                                  URL = temp.URL,
                                                  PermissionAssigned = ((from tek in db.RolesMenu
                                                                         where tek.RoleId == RoleId && tek.MenuId == temp.MenuId && tek.SiteId == SiteId && tek.DivisionId == DivisionId
                                                                         select tek).Any()
                                                                               ),

                                              }
                                         ).ToList()

                         }).ToList();



            double x = 0;
            var SubModuleList = temp2.OrderBy(m => m.SubModuleId).ThenBy(sx => double.TryParse(sx.Srl, out x) ? x : 0);



            return SubModuleList;
        }


        public MenuSubModule Find(int id)
        {
            return _unitOfWork.Repository<MenuSubModule>().Find(id);
        }

        public MenuSubModule Create(MenuSubModule pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<MenuSubModule>().Insert(pt);
            return pt;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<MenuSubModule>().Delete(id);
        }

        public void Delete(MenuSubModule pt)
        {
            _unitOfWork.Repository<MenuSubModule>().Delete(pt);
        }

        public void Update(MenuSubModule pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<MenuSubModule>().Update(pt);
        }

        public IEnumerable<MenuSubModule> GetSubModuleList()
        {
            var pt = (from p in db.MenuSubModule
                      orderby p.SubModuleName
                      select p
                          );

            return pt;
        }

        public MenuSubModule Add(MenuSubModule pt)
        {
            _unitOfWork.Repository<MenuSubModule>().Insert(pt);
            return pt;
        }

        public void Dispose()
        {
        }

    }
}
