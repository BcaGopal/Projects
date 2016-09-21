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
using Microsoft.AspNet.Identity.EntityFramework;

namespace Service
{
    public interface IUserService : IDisposable
    {
        IEnumerable<IdentityUser> GetUserList(string[] RoleNameStr);
    }

    public class UserService : IUserService
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<IdentityUser> GetUserList(string[] RoleNameStr)
        {
            IEnumerable<IdentityUser> temp = from H in db.Users
                                             join L in db.AspNetUserRole on H.Id equals L.UserId into UserRoleTable
                                             from UserRoleTab in UserRoleTable.DefaultIfEmpty()
                                             join R in db.AspNetRole on UserRoleTab.RoleId equals R.Id into RoleTable
                                             from RoleTab in RoleTable.DefaultIfEmpty()
                                             where RoleNameStr.Contains(RoleTab.Name)
                                             select H;
            if (temp != null)
            {
                return temp.ToList();
            }
            else
            {
                return null;
            }
        }


        public void Dispose()
        {
        }
    }
}
