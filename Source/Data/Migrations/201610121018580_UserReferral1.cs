namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserReferral1 : DbMigration
    {
        public override void Up()
        {
            DropTable("Web.UserReferrals");
        }
        
        public override void Down()
        {
            CreateTable(
                "Web.UserReferrals",
                c => new
                    {
                        UserReferralId = c.Guid(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserReferralId);
            
        }
    }
}
