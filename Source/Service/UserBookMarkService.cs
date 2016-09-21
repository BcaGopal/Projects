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
using ProjLib.ViewModels;

namespace Service
{
    public interface IUserBookMarkService : IDisposable
    {
        UserBookMark Create(UserBookMark pt);
        void Delete(int id);
        void Delete(UserBookMark pt);
        UserBookMark Find(int id);      
        void Update(UserBookMark pt);
        IEnumerable<UserBookMark> GetUserBookMarkList();
        UserBookMark FindUserBookMark(string userid, int menuid);
        IEnumerable<UserBookMarkViewModel> GetUserBookMarkListForUser(string AppuserId);
        bool CheckBookMarkExists(string UserId, int MenuId);
    }

    public class UserBookMarkService : IUserBookMarkService
    {
        ApplicationDbContext db;
       
        public UserBookMarkService(ApplicationDbContext db)
        {
            this.db = db;
        }


        public UserBookMark Find(int id)
        {
            return db.UserBookMark.Find(id);
        }
        public UserBookMark FindUserBookMark(string userid, int menuid)
        {
            return (from p in db.UserBookMark
                    where p.MenuId == menuid && p.ApplicationUserName == userid
                    select p
                        ).FirstOrDefault();

        }


        public IEnumerable<UserBookMarkViewModel> GetUserBookMarkListForUser(string AppuserId)
        {
            return (from p in db.UserBookMark
                    join t in db.Menu on p.MenuId equals t.MenuId into table from tab in table.DefaultIfEmpty()
                    where p.ApplicationUserName == AppuserId
                    select new UserBookMarkViewModel
                    {
                        MenuId=p.MenuId,
                        MenuName=tab.MenuName,
                        IconName=tab.IconName,
                    }
                        ).ToList();

        }

        public UserBookMark Create(UserBookMark pt)
        {
            pt.ObjectState = ObjectState.Added;
            db.UserBookMark.Add(pt);
            return pt;
        }

        public void Delete(int id)
        {
            var UserBookMark = db.UserBookMark.Find(id);
            UserBookMark.ObjectState = Model.ObjectState.Deleted;
            db.UserBookMark.Remove(UserBookMark);
        }

        public void Delete(UserBookMark pt)
        {
            pt.ObjectState = Model.ObjectState.Deleted;
            db.UserBookMark.Remove(pt);
        }

        public void Update(UserBookMark pt)
        {
            pt.ObjectState = ObjectState.Modified;
            db.UserBookMark.Add(pt);
        }

        public IEnumerable<UserBookMark> GetUserBookMarkList()
        {
            var pt = (from p in db.UserBookMark                      
                      select p
                          );

            return pt;
        }

        public bool CheckBookMarkExists(string UserId, int MenuId)
        {
            return (from p in db.UserBookMark
                    where p.MenuId == MenuId && p.ApplicationUserName == UserId
                    select p).Any();
        }
        public void Dispose()
        {
        }

    }
}
