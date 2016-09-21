using System.Data.Entity;
using Data.Infrastructure;
using Model.Tasks.Models;
using Models.Login.Infrastructure;
using Models.Login.Models;
using Model.DatabaseViews;

namespace Data.Models
{
    public partial class LoginApplicationDbContext : DataContext, IStandardLoginModelDbSets
    {       
        public string strSchemaName = "Web";

        public LoginApplicationDbContext()
            : base("LoginDB", false)
        {
            ;
            Configuration.ProxyCreationEnabled = false;
        }        
        public DbSet<NotificationSubject> NotificationSubject { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<NotificationUser> NotificationUser { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<DAR> DAR { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<UserTeam> UserTeam { get; set; }
        public DbSet<_Users> _Users { get; set; }
    }
}