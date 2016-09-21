using System.Collections.Generic;
using System.Linq;
using Data.Infrastructure;
using Model.Models;
using System;
using Model;
using Data.Models;
using ProjLib.ViewModels;

namespace Service
{
    public interface IRolesControllerActionService : IDisposable
    {
        RolesControllerAction Create(RolesControllerAction pt);
        void Delete(int id);
        void Delete(RolesControllerAction pt);
        RolesControllerAction Find(int ptId);
        void Update(RolesControllerAction pt);
        RolesControllerAction Add(RolesControllerAction pt);
        IEnumerable<RolesControllerAction> GetRolesControllerActionList();
        RolesControllerAction Find(int MenuId, string RoleId);
        IEnumerable<RolesControllerActionViewModel> GetRolesControllerActionList(string RoleId);
        IEnumerable<RolesControllerActionViewModel> GetRolesControllerActionsForRoles(List<String> Roles);
    }

    public class RolesControllerActionService : IRolesControllerActionService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<RolesControllerAction> _RolesControllerActionRepository;
        RepositoryQuery<RolesControllerAction> RolesControllerActionRepository;
        public RolesControllerActionService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _RolesControllerActionRepository = new Repository<RolesControllerAction>(db);
            RolesControllerActionRepository = new RepositoryQuery<RolesControllerAction>(_RolesControllerActionRepository);
        }

        public RolesControllerAction Find(int pt)
        {
            return _unitOfWork.Repository<RolesControllerAction>().Find(pt);
        }

        public RolesControllerAction Create(RolesControllerAction pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<RolesControllerAction>().Insert(pt);
            return pt;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<RolesControllerAction>().Delete(id);
        }

        public void Delete(RolesControllerAction pt)
        {
            _unitOfWork.Repository<RolesControllerAction>().Delete(pt);
        }

        public void Update(RolesControllerAction pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<RolesControllerAction>().Update(pt);
        }

        public IEnumerable<RolesControllerAction> GetRolesControllerActionList()
        {
            var pt = _unitOfWork.Repository<RolesControllerAction>().Query().Get();

            return pt;
        }

        public RolesControllerAction Add(RolesControllerAction pt)
        {
            _unitOfWork.Repository<RolesControllerAction>().Insert(pt);
            return pt;
        }

        public RolesControllerAction Find(int ControllerActionId, string RoleId)
        {
            return _unitOfWork.Repository<RolesControllerAction>().Query().Get().Where(m=>m.RoleId==RoleId && m.ControllerActionId==ControllerActionId).FirstOrDefault();
        }

        public IEnumerable<RolesControllerActionViewModel> GetRolesControllerActionList(string RoleId)
        {
            return (from p in db.RolesControllerAction
                    where p.RoleId == RoleId
                    select new RolesControllerActionViewModel
                    {
                        ControllerActionId = p.ControllerActionId,
                        RoleId = p.RoleId,
                        RolesControllerActionId = p.RolesControllerActionId,
                        ControllerActionName=p.ControllerAction.ActionName,
                        RoleName = p.Role.Name,
                    });
        }
        public RolesControllerAction GetControllerActionForRoleId(string RoleId,int ControllerActionId)
        {

            return (from p in db.RolesControllerAction
                    where p.RoleId == RoleId && p.ControllerActionId==ControllerActionId
                    select p).FirstOrDefault();

        }

        public IEnumerable<RolesControllerActionViewModel> GetRolesControllerActionsForRoles(List<String> Roles)
        {
            var temp = (from p in db.Roles
                        select p).ToList();

            var RoleIds = string.Join(",", from p in temp
                                           where Roles.Contains(p.Name)
                                           select p.Id.ToString());

            var Temp = (from p in db.RolesControllerAction
                        where RoleIds.Contains(p.RoleId)
                        select new RolesControllerActionViewModel { 
                        ControllerActionId=p.ControllerActionId,
                        ControllerActionName=p.ControllerAction.ActionName,
                        ControllerName=p.ControllerAction.Controller.ControllerName,
                        RoleId=p.RoleId,
                        RoleName=p.Role.Name,
                        RolesControllerActionId=p.RolesControllerActionId
                        });

            return Temp;
        }
        public void Dispose()
        {
        }
    }
}
