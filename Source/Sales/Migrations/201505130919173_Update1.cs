namespace Sales.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Web.AspNetUserClaims", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.AspNetUserClaims", name: "IX_User_Id", newName: "IX_UserId");
            DropPrimaryKey("dbo.AspNetUserLogins");            
            AddColumn("Web.User", "EmailConfirmed", c => c.Boolean(nullable: false));
            AddColumn("Web.User", "PhoneNumber", c => c.String());
            AddColumn("Web.User", "PhoneNumberConfirmed", c => c.Boolean(nullable: false));
            AddColumn("Web.User", "TwoFactorEnabled", c => c.Boolean(nullable: false));
            AddColumn("Web.User", "LockoutEndDateUtc", c => c.DateTime());
            AddColumn("Web.User", "LockoutEnabled", c => c.Boolean(nullable: false));
            AddColumn("Web.User", "AccessFailedCount", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetRoles", "Name", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false, maxLength: 256));
            AddPrimaryKey("dbo.AspNetUserLogins", new[] { "LoginProvider", "ProviderKey", "UserId" });
            CreateIndex("dbo.AspNetRoles", "Name", unique: true, name: "RoleNameIndex");
            CreateIndex("dbo.AspNetUsers", "UserName", unique: true, name: "UserNameIndex");
            DropColumn("dbo.AspNetUsers", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropPrimaryKey("dbo.AspNetUserLogins");
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String());
            AlterColumn("dbo.AspNetRoles", "Name", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "AccessFailedCount");
            DropColumn("dbo.AspNetUsers", "LockoutEnabled");
            DropColumn("dbo.AspNetUsers", "LockoutEndDateUtc");
            DropColumn("dbo.AspNetUsers", "TwoFactorEnabled");
            DropColumn("dbo.AspNetUsers", "PhoneNumberConfirmed");
            DropColumn("dbo.AspNetUsers", "PhoneNumber");
            DropColumn("dbo.AspNetUsers", "EmailConfirmed");
            DropColumn("dbo.AspNetUsers", "Email");
            AddPrimaryKey("dbo.AspNetUserLogins", new[] { "UserId", "LoginProvider", "ProviderKey" });
            RenameIndex(table: "dbo.AspNetUserClaims", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.AspNetUserClaims", name: "UserId", newName: "User_Id");
        }
    }
}
