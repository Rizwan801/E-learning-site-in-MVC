namespace CUEL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notifications", "AppUserID", "dbo.AppUsers");
            DropIndex("dbo.Notifications", new[] { "AppUserID" });
            AddColumn("dbo.Notifications", "DriverID", c => c.Int());
            AddColumn("dbo.Notifications", "AppUser_AppUserID", c => c.Int());
            AddColumn("dbo.Notifications", "AppUser_AppUserID1", c => c.Int());
            CreateIndex("dbo.Notifications", "DriverID");
            CreateIndex("dbo.Notifications", "AppUser_AppUserID");
            CreateIndex("dbo.Notifications", "AppUser_AppUserID1");
            AddForeignKey("dbo.Notifications", "DriverID", "dbo.AppUsers", "AppUserID");
            AddForeignKey("dbo.Notifications", "AppUser_AppUserID1", "dbo.AppUsers", "AppUserID");
            AddForeignKey("dbo.Notifications", "AppUser_AppUserID", "dbo.AppUsers", "AppUserID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "AppUser_AppUserID", "dbo.AppUsers");
            DropForeignKey("dbo.Notifications", "AppUser_AppUserID1", "dbo.AppUsers");
            DropForeignKey("dbo.Notifications", "DriverID", "dbo.AppUsers");
            DropIndex("dbo.Notifications", new[] { "AppUser_AppUserID1" });
            DropIndex("dbo.Notifications", new[] { "AppUser_AppUserID" });
            DropIndex("dbo.Notifications", new[] { "DriverID" });
            DropColumn("dbo.Notifications", "AppUser_AppUserID1");
            DropColumn("dbo.Notifications", "AppUser_AppUserID");
            DropColumn("dbo.Notifications", "DriverID");
            CreateIndex("dbo.Notifications", "AppUserID");
            AddForeignKey("dbo.Notifications", "AppUserID", "dbo.AppUsers", "AppUserID");
        }
    }
}
