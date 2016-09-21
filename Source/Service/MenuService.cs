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

namespace Service
{
    public interface IMenuService : IDisposable
    {
        Menu Create(Menu pt);
        void Delete(int id);
        void Delete(Menu pt);
        Menu Find(string Name);
        Menu Find(int id);      
        void Update(Menu pt);
        Menu Add(Menu pt);
        IEnumerable<Menu> GetMenuList();
        Menu GetMenuByName(string terms);
        MenuViewModel GetMenu(int MenuId);
    }

    public class MenuService : IMenuService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<Menu> _MenuRepository;
        RepositoryQuery<Menu> MenuRepository;
        public MenuService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _MenuRepository = new Repository<Menu>(db);
            MenuRepository = new RepositoryQuery<Menu>(_MenuRepository);
        }
        public Menu GetMenuByName(string terms)
        {
            return (from p in db.Menu
                    where p.MenuName == terms
                    select p).FirstOrDefault();
        }



        public Menu Find(string Name)
        {
            return MenuRepository.Get().Where(i => i.MenuName == Name).FirstOrDefault();
        }

        //public Menu GetMenuByRoles()
        //{

           

        //}


        public Menu Find(int id)
        {
            return _unitOfWork.Repository<Menu>().Find(id);
        }

        public Menu Create(Menu pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<Menu>().Insert(pt);
            return pt;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<Menu>().Delete(id);
        }

        public void Delete(Menu pt)
        {
            _unitOfWork.Repository<Menu>().Delete(pt);
        }

        public void Update(Menu pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<Menu>().Update(pt);
        }

        public IEnumerable<Menu> GetMenuList()
        {
            var pt = (from p in db.Menu
                      orderby p.MenuName
                      select p
                          );

            return pt;
        }

        public Menu Add(Menu pt)
        {
            _unitOfWork.Repository<Menu>().Insert(pt);
            return pt;
        }

        public MenuViewModel GetMenu(int MenuId)
        {
            MenuViewModel menuviewmodel = (from M in db.Menu
                                           join C in db.ControllerAction on M.ControllerActionId equals C.ControllerActionId into ControllerActionTable
                                           from ControllerActionTab in ControllerActionTable.DefaultIfEmpty()
                                           where M.MenuId == MenuId
                                           select new MenuViewModel
                                           {
                                               MenuId = M.MenuId,
                                               ControllerName = ControllerActionTab.ControllerName,
                                               ActionName = ControllerActionTab.ActionName,
                                               RouteId = M.RouteId,
                                               URL=M.URL,
                                               ModuleId=M.ModuleId,
                                               SubModuleId=M.SubModuleId,
                                           }).FirstOrDefault();
            return menuviewmodel;
        }

        public void Dispose()
        {
        }

    }
}
