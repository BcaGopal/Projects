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

namespace Service
{
    public interface IControllerActionService : IDisposable
    {
        ControllerAction Create(ControllerAction pt);
        void Delete(int id);
        void Delete(ControllerAction pt);
        ControllerAction Find(int id);
        ControllerAction Find(string Name);
        ControllerAction Find(string Name,int ? ControllerId);
        void Update(ControllerAction pt);
        ControllerAction Add(ControllerAction pt);
        IEnumerable<ControllerAction> GetControllerActionList();        
    }

    public class ControllerActionService : IControllerActionService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<ControllerAction> _ControllerActionRepository;
        RepositoryQuery<ControllerAction> ControllerActionRepository;
        public ControllerActionService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _ControllerActionRepository = new Repository<ControllerAction>(db);
            ControllerActionRepository = new RepositoryQuery<ControllerAction>(_ControllerActionRepository);
        }
 


        public ControllerAction Find(int id)
        {
            return _unitOfWork.Repository<ControllerAction>().Find(id);
        }

        public ControllerAction Find(string Name)
        {
            return _unitOfWork.Repository<ControllerAction>().Query().Get().Where(m=>m.ActionName==Name).FirstOrDefault();
        }

        public ControllerAction Find(string Name, int? ControllerId)
        {
            return _unitOfWork.Repository<ControllerAction>().Query().Get().Where(m => m.ActionName == Name&& m.ControllerId==ControllerId).FirstOrDefault();
        }
        public ControllerAction Create(ControllerAction pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<ControllerAction>().Insert(pt);
            return pt;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<ControllerAction>().Delete(id);
        }

        public void Delete(ControllerAction pt)
        {
            _unitOfWork.Repository<ControllerAction>().Delete(pt);
        }

        public void Update(ControllerAction pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<ControllerAction>().Update(pt);
        }

        public IEnumerable<ControllerAction> GetControllerActionList()
        {
            var pt = (from p in db.ControllerAction                      
                      select p
                          );

            return pt;
        }

        public ControllerAction Add(ControllerAction pt)
        {
            _unitOfWork.Repository<ControllerAction>().Insert(pt);
            return pt;
        }

        public void Dispose()
        {
        }

    }
}
