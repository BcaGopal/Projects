using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Tasks.Models;
using Models.Login.Models;

namespace Models.Login.Infrastructure
{
    public interface IStandardLoginModelDbSets
    {
        DbSet<Model.Tasks.Models.Tasks> Tasks { get; set; }
        DbSet<DAR> DAR { get; set; }
        DbSet<Project> Project { get; set; }
        DbSet<UserTeam> UserTeam { get; set; }
        //Notification Models
        DbSet<Notification> Notification { get; set; }
        DbSet<NotificationSubject> NotificationSubject { get; set; }
    }
}
