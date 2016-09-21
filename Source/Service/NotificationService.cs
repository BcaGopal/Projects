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
using Models.Login.Models;

namespace Service
{
    public interface INotificationService : IDisposable
    {
        Notification Create(Notification pt);
        void Delete(int id);
        void Delete(Notification pt);
        Notification Find(int id);
        IEnumerable<Notification> GetPagedList(int pageNumber, int pageSize, out int totalRecords);
        void Update(Notification pt);
        Notification Add(Notification pt);
        IEnumerable<Notification> GetNotificationList();

        // IEnumerable<Notification> GetNotificationList(int buyerId);
        Task<IEquatable<Notification>> GetAsync();
        Task<Notification> FindAsync(int id);
        int NextId(int id);
        int PrevId(int id);
    }

    public class NotificationService : INotificationService
    {
        NotificationApplicationDbContext db = new NotificationApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<Notification> _NotificationRepository;
        RepositoryQuery<Notification> NotificationRepository;
        public NotificationService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _NotificationRepository = new Repository<Notification>(db);
            NotificationRepository = new RepositoryQuery<Notification>(_NotificationRepository);
        }

        public Notification Find(int id)
        {
            return _unitOfWork.Repository<Notification>().Find(id);

        }

        public Notification Create(Notification pt)
        {
            //pt.ObjectState = ObjectState.Added;
            //_unitOfWork.Repository<Notification>().Insert(pt);
            pt.ObjectState = ObjectState.Added;
            db.Notification.Add(pt);
            db.SaveChanges();
            return pt;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<Notification>().Delete(id);
        }

        public void Delete(Notification pt)
        {
            _unitOfWork.Repository<Notification>().Delete(pt);
        }

        public void Update(Notification pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<Notification>().Update(pt);
        }

        public IEnumerable<Notification> GetPagedList(int pageNumber, int pageSize, out int totalRecords)
        {
            var so = _unitOfWork.Repository<Notification>()
                .Query()
                .GetPage(pageNumber, pageSize, out totalRecords);

            return so;
        }

        public IEnumerable<Notification> GetNotificationList()
        {
            var pt = _unitOfWork.Repository<Notification>().Query().Get();

            return pt;
        }

        public Notification Add(Notification pt)
        {
            _unitOfWork.Repository<Notification>().Insert(pt);
            return pt;
        }

        public int NextId(int id)
        {
            int temp = 0;
            if (id != 0)
            {
                temp = (from p in db.Notification
                        select p.NotificationId).AsEnumerable().SkipWhile(p => p != id).Skip(1).FirstOrDefault();
            }
            else
            {
                temp = (from p in db.Notification
                        select p.NotificationId).FirstOrDefault();
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

                temp = (from p in db.Notification
                        select p.NotificationId).AsEnumerable().TakeWhile(p => p != id).LastOrDefault();
            }
            else
            {
                temp = (from p in db.Notification
                        select p.NotificationId).AsEnumerable().LastOrDefault();
            }
            if (temp != 0)
                return temp;
            else
                return id;
        }

        public void Dispose()
        {
        }


        public Task<IEquatable<Notification>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Notification> FindAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
