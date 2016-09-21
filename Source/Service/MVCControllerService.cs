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
    public interface IMvcControllerService : IDisposable
    {
        MvcController Create(MvcController pt);
        void Delete(int id);
        void Delete(MvcController pt);
        MvcController Find(string Name);
        MvcController Find(int id);
        IEnumerable<MvcController> GetPagedList(int pageNumber, int pageSize, out int totalRecords);
        void Update(MvcController pt);
        MvcController Add(MvcController pt);
        IEnumerable<MvcController> GetMvcControllerList();

        // IEnumerable<MvcController> GetMvcControllerList(int buyerId);
        Task<IEquatable<MvcController>> GetAsync();
        Task<MvcController> FindAsync(int id);
        MvcController GetMvcControllerByName(string terms);
        int NextId(int id);
        int PrevId(int id);
    }

    public class MvcControllerService : IMvcControllerService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<MvcController> _MvcControllerRepository;
        RepositoryQuery<MvcController> MvcControllerRepository;
        public MvcControllerService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _MvcControllerRepository = new Repository<MvcController>(db);
            MvcControllerRepository = new RepositoryQuery<MvcController>(_MvcControllerRepository);
        }
        public MvcController GetMvcControllerByName(string terms)
        {
            return (from p in db.MvcController
                    where p.ControllerName == terms
                    select p).FirstOrDefault();
        }

        public MvcController Find(string Name)
        {
            return MvcControllerRepository.Get().Where(i => i.ControllerName == Name).FirstOrDefault();
        }


        public MvcController Find(int id)
        {
            return _unitOfWork.Repository<MvcController>().Find(id);
        }

        public MvcController Create(MvcController pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<MvcController>().Insert(pt);
            return pt;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<MvcController>().Delete(id);
        }

        public void Delete(MvcController pt)
        {
            _unitOfWork.Repository<MvcController>().Delete(pt);
        }

        public void Update(MvcController pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<MvcController>().Update(pt);
        }

        public IEnumerable<MvcController> GetPagedList(int pageNumber, int pageSize, out int totalRecords)
        {
            var so = _unitOfWork.Repository<MvcController>()
                .Query()
                .OrderBy(q => q.OrderBy(c => c.ControllerName))                
                .GetPage(pageNumber, pageSize, out totalRecords);

            return so;
        }

        public IEnumerable<MvcController> GetMvcControllerList()
        {
            var pt = _unitOfWork.Repository<MvcController>().Query().Get().OrderBy(m=>m.ControllerName);

            return pt;
        }

        public MvcController Add(MvcController pt)
        {
            _unitOfWork.Repository<MvcController>().Insert(pt);
            return pt;
        }

        public int NextId(int id)
        {
            int temp = 0;
            if (id != 0)
            {
                temp = (from p in db.MvcController
                        orderby p.ControllerName
                        select p.ControllerId).AsEnumerable().SkipWhile(p => p != id).Skip(1).FirstOrDefault();
            }
            else
            {
                temp = (from p in db.MvcController
                        orderby p.ControllerName
                        select p.ControllerId).FirstOrDefault();
            }
            if (temp != 0)
                return temp;
            else
                return id;
        }

        public int PrevId(int id)
        {

            int temp = 0;
            if (id != 0)
            {

                temp = (from p in db.MvcController
                        orderby p.ControllerName
                        select p.ControllerId).AsEnumerable().TakeWhile(p => p != id).LastOrDefault();
            }
            else
            {
                temp = (from p in db.MvcController
                        orderby p.ControllerName
                        select p.ControllerId).AsEnumerable().LastOrDefault();
            }
            if (temp != 0)
                return temp;
            else
                return id;
        }

        public void Dispose()
        {
        }


        public Task<IEquatable<MvcController>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MvcController> FindAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
