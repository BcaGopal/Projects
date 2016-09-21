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
    public interface IRolesDivisionService : IDisposable
    {
        RolesDivision Create(RolesDivision pt);
        void Delete(int id);
        void Delete(RolesDivision pt);
        RolesDivision Find(int ptId);
        void Update(RolesDivision pt);
        RolesDivision Add(RolesDivision pt);
        IEnumerable<RolesDivision> GetRolesDivisionList();
        RolesDivision Find(int DivisionId, string RoleId);
        IEnumerable<RolesDivisionViewModel> GetRolesDivisionList(string RoleId); 
    }

    public class RolesDivisionService : IRolesDivisionService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<RolesDivision> _RolesDivisionRepository;
        RepositoryQuery<RolesDivision> RolesDivisionRepository;
        public RolesDivisionService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _RolesDivisionRepository = new Repository<RolesDivision>(db);
            RolesDivisionRepository = new RepositoryQuery<RolesDivision>(_RolesDivisionRepository);
        }

        public RolesDivision Find(int pt)
        {
            return _unitOfWork.Repository<RolesDivision>().Find(pt);
        }

        public RolesDivision Create(RolesDivision pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<RolesDivision>().Insert(pt);
            return pt;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<RolesDivision>().Delete(id);
        }

        public void Delete(RolesDivision pt)
        {
            _unitOfWork.Repository<RolesDivision>().Delete(pt);
        }

        public void Update(RolesDivision pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<RolesDivision>().Update(pt);
        }

        public IEnumerable<RolesDivision> GetRolesDivisionList()
        {
            var pt = _unitOfWork.Repository<RolesDivision>().Query().Get();

            return pt;
        }


        public RolesDivision Add(RolesDivision pt)
        {
            _unitOfWork.Repository<RolesDivision>().Insert(pt);
            return pt;
        }

        public RolesDivision Find(int DivisionId, string RoleId)
        {
            return _unitOfWork.Repository<RolesDivision>().Query().Get().Where(m=>m.RoleId==RoleId && m.DivisionId==DivisionId).FirstOrDefault();
        }

        public IEnumerable<RolesDivisionViewModel> GetRolesDivisionList(string RoleId)
        {
            return (from p in db.RolesDivision
                    where p.RoleId == RoleId
                    select new RolesDivisionViewModel
                    {
                        DivisionId = p.DivisionId,
                        RoleId = p.RoleId,
                        RoleName = p.Role.Name,
                        RolesDivisionId = p.RolesDivisionId
                    });
        }

        public void Dispose()
        {
        }
    }
}
