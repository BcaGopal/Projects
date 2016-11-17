﻿using System.Collections.Generic;
using System.Linq;
using Data.Infrastructure;
using Model.Models;
using System;
using Model;
using Data.Models;

namespace Service
{
    public interface IUserReferralService : IDisposable
    {
        UserReferral Create(UserReferral pt);
        UserReferral Create(string UserName, string ToUser, string RoleId);
        void Delete(UserReferral pt);
        UserReferral Find(string Id);
        void Update(UserReferral pt);
        IEnumerable<string> GetUserTypes();
        IEnumerable<UserReferral> GetUserReferralList();
        string GetUserEmail(string UserId);
        Dictionary<string, string> GetUserRolesList();
    }

    public class UserReferralService : IUserReferralService
    {
        private readonly LoginApplicationDbContext db;
        public UserReferralService(IUnitOfWork unitOfWork)
        {
            db = new LoginApplicationDbContext();
        }

        public UserReferral Find(string Id)
        {
            return db.UserReferral.Find(Id);
        }

        public UserReferral Create(UserReferral pt)
        {
            pt.ObjectState = ObjectState.Added;
            db.UserReferral.Add(pt);
            db.SaveChanges();
            return pt;
        }

        public UserReferral Create(string UserName, string ToUser, string RoleId)
        {
            UserReferral uref = new UserReferral();
            uref.CreatedBy = UserName;
            uref.CreatedDate = DateTime.Now;
            uref.IsActive = true;
            uref.ToUser = ToUser;
            uref.RoleId = RoleId;
            uref.ModifiedBy = UserName;
            uref.ModifiedDate = DateTime.Now;
            uref.ObjectState = ObjectState.Added;
            db.UserReferral.Add(uref);
            db.SaveChanges();
            return uref;
        }

        public void Delete(UserReferral pt)
        {
            db.UserReferral.Remove(pt);
        }

        public void Update(UserReferral pt)
        {
            pt.ObjectState = ObjectState.Modified;
            db.UserReferral.Add(pt);
        }

        public IEnumerable<UserReferral> GetUserReferralList()
        {
            var pt = (from p in db.UserReferral
                      orderby p.CreatedDate descending
                      select p
                          );
            return pt;
        }

        public IEnumerable<string> GetUserTypes()
        {
            return db.UserType.Select(m => m.Name).ToList();
        }

        public string GetUserEmail(string UserId)
        {
            string UserEmail = "";
            var User = db.Users.Find(UserId);
            if (User != null)
                UserEmail = User.Email;
            return UserEmail;
        }

        public Dictionary<string, string> GetUserRolesList()
        {

            Dictionary<string, string> RolesList = new Dictionary<string, string>();

            using (ApplicationDbContext con = new ApplicationDbContext())
            {
                RolesList = (from p in con.Roles
                             select new
                             {
                                 p.Id,
                                 p.Name
                             }).ToDictionary(m => m.Id, m => m.Name);
            }

            return RolesList;

        }

        public void Dispose()
        {
            db.Dispose();
        }

    }
}