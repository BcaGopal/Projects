using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Infrastructure;
using Model.Models;
using Model.ViewModels;
using Core.Common;
using System;
using Model;
using System.Threading.Tasks;
using Data.Models;
using Model.ViewModel;

namespace Service
{
    public interface IRolesMenuService : IDisposable
    {
        RolesMenu Create(RolesMenu pt);
        void Delete(int id);
        void Delete(RolesMenu pt);
        RolesMenu Find(int ptId);
        void Update(RolesMenu pt);
        RolesMenu Add(RolesMenu pt);
        IEnumerable<RolesMenu> GetRolesMenuList();
        RolesMenu Find(int MenuId, string RoleId);
        IEnumerable<RolesMenuViewModel> GetRolesMenuList(string RoleId);
        int GetPermittedActionsCountForMenuId(int MenuId, string RoleId);
    }

    public class RolesMenuService : IRolesMenuService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<RolesMenu> _RolesMenuRepository;
        RepositoryQuery<RolesMenu> RolesMenuRepository;
        public RolesMenuService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _RolesMenuRepository = new Repository<RolesMenu>(db);
            RolesMenuRepository = new RepositoryQuery<RolesMenu>(_RolesMenuRepository);
        }

        public RolesMenu Find(int pt)
        {
            return _unitOfWork.Repository<RolesMenu>().Find(pt);
        }

        public RolesMenu Create(RolesMenu pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<RolesMenu>().Insert(pt);
            return pt;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<RolesMenu>().Delete(id);
        }

        public void Delete(RolesMenu pt)
        {
            _unitOfWork.Repository<RolesMenu>().Delete(pt);
        }

        public void Update(RolesMenu pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<RolesMenu>().Update(pt);
        }

        public IEnumerable<RolesMenu> GetRolesMenuList()
        {
            var pt = _unitOfWork.Repository<RolesMenu>().Query().Get();

            return pt;
        }


        public RolesMenu Add(RolesMenu pt)
        {
            _unitOfWork.Repository<RolesMenu>().Insert(pt);
            return pt;
        }

        public RolesMenu Find(int MenuId, string RoleId)
        {
            return _unitOfWork.Repository<RolesMenu>().Query().Get().Where(m=>m.RoleId==RoleId && m.MenuId==MenuId).FirstOrDefault();
        }

        public IEnumerable<RolesMenuViewModel> GetRolesMenuList(string RoleId)
        {
            return (from p in db.RolesMenu
                    where p.RoleId == RoleId
                    select new RolesMenuViewModel
                    {
                        MenuId = p.MenuId,
                        RoleId = p.RoleId,
                        RolesMenuId = p.RolesMenuId,
                        MenuName = p.Menu.MenuName,
                        RoleName = p.Role.Name,
                        FullHeaderPermission=p.FullHeaderPermission,
                        FullLinePermission=p.FullLinePermission
                    });
        }
        public RolesMenu GetRoleMenuForRoleId(string RoleId,int MenuId)
        {

            return (from p in db.RolesMenu
                    where p.RoleId == RoleId && p.MenuId == MenuId
                    select p).FirstOrDefault();

        }
        public int GetPermittedActionsCountForMenuId(int MenuId, string RoleId)
        {

            int ControllerId = (from p in db.Menu
                                join t in db.ControllerAction on p.ControllerActionId equals t.ControllerActionId
                                join t2 in db.MvcController on t.ControllerId equals t2.ControllerId
                                where p.MenuId == MenuId
                                select
                                    t2.ControllerId
            ).FirstOrDefault();

            int list = (from p in db.ControllerAction
                        join t in db.RolesControllerAction.Where(m => m.RoleId == RoleId) on p.ControllerActionId equals t.ControllerActionId
                        where p.ControllerId == ControllerId
                        select t).Count();
            return list;
        }

        public int GetChildPermittedActionsCountForMenuId(int MenuId, string RoleId)
        {
            string ControllerId= string.Join(",",(from p in db.Menu
                                join t in db.MvcController on p.ControllerAction.ControllerId equals t.ParentControllerId
                                join t2 in db.MvcController on t.ControllerId equals t2.ControllerId
                                where p.MenuId == MenuId
                                select
                                    t2.ControllerId
            ).ToList());            

            int list = (from p in db.ControllerAction
                        join t in db.RolesControllerAction.Where(m => m.RoleId == RoleId) on p.ControllerActionId equals t.ControllerActionId
                        where ControllerId.Contains(p.ControllerId.ToString())
                        select t).Count();
            return list;
        }

        public void Dispose()
        {
        }
    }
}
