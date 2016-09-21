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
    public interface IGatePassHeaderService : IDisposable
    {
        GatePassHeader Create(GatePassHeader pt);
        void Delete(int id);
        void Delete(GatePassHeader pt);
        GatePassHeader Find(string Name);
        GatePassHeader Find(int id);
        IEnumerable<GatePassHeader> GetPagedList(int pageNumber, int pageSize, out int totalRecords);
        void Update(GatePassHeader pt);
        GatePassHeader Add(GatePassHeader pt);                
        Task<IEquatable<GatePassHeader>> GetAsync();
        Task<GatePassHeader> FindAsync(int id);        
    }

    public class GatePassHeaderService : IGatePassHeaderService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<GatePassHeader> _GatePassHeaderRepository;
        RepositoryQuery<GatePassHeader> GatePassHeaderRepository;
        public GatePassHeaderService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _GatePassHeaderRepository = new Repository<GatePassHeader>(db);
            GatePassHeaderRepository = new RepositoryQuery<GatePassHeader>(_GatePassHeaderRepository);
        }

        public GatePassHeader Find(string Name)
        {
            return GatePassHeaderRepository.Get().Where(i => i.DocNo == Name).FirstOrDefault();
        }


        public GatePassHeader Find(int id)
        {
            return _unitOfWork.Repository<GatePassHeader>().Find(id);
        }

        public GatePassHeader Create(GatePassHeader pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<GatePassHeader>().Insert(pt);
            return pt;
        }     

        public void Delete(int id)
        {
            _unitOfWork.Repository<GatePassHeader>().Delete(id);
        }

        public void Delete(GatePassHeader pt)
        {
            _unitOfWork.Repository<GatePassHeader>().Delete(pt);
        }

        public void Update(GatePassHeader pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<GatePassHeader>().Update(pt);
        }

        public IEnumerable<GatePassHeader> GetPagedList(int pageNumber, int pageSize, out int totalRecords)
        {
            var so = _unitOfWork.Repository<GatePassHeader>()
                .Query()
                .OrderBy(q => q.OrderBy(c => c.DocNo))                
                .GetPage(pageNumber, pageSize, out totalRecords);

            return so;
        }

        public GatePassHeader Add(GatePassHeader pt)
        {
            _unitOfWork.Repository<GatePassHeader>().Insert(pt);
            return pt;
        }

        public void Dispose()
        {
        }


        public Task<IEquatable<GatePassHeader>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GatePassHeader> FindAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
