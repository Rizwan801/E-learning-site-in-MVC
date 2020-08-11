namespace CUEL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m10 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notifications", "AppUser_AppUserID1", "dbo.AppUsers");
            DropIndex("dbo.Notifications", new[] { "AppUser_AppUserID1" });
            DropColumn("dbo.Notifications", "AppUserID");
            RenameColumn(table: "dbo.Notifications", name: "AppUser_AppUserID", newName: "AppUserID");
            RenameIndex(table: "dbo.Notifications", name: "IX_AppUser_AppUserID", newName: "IX_AppUserID");
            DropColumn("dbo.Notifications", "AppUser_AppUserID1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "AppUser_AppUserID1", c => c.Int());
            RenameIndex(table: "dbo.Notifications", name: "IX_AppUserID", newName: "IX_AppUser_AppUserID");
            RenameColumn(table: "dbo.Notifications", name: "AppUserID", newName: "AppUser_AppUserID");
            AddColumn("dbo.Notifications", "AppUserID", c => c.Int());
            CreateIndex("dbo.Notifications", "AppUser_AppUserID1");
            AddForeignKey("dbo.Notifications", "AppUser_AppUserID1", "dbo.AppUsers", "AppUserID");
        }
    }
}
