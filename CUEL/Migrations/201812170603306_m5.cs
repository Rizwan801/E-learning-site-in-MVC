namespace CUEL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Link = c.String(),
                        AppUserID = c.Int(),
                    })
                .PrimaryKey(t => t.NotificationID)
                .ForeignKey("dbo.AppUsers", t => t.AppUserID)
                .Index(t => t.AppUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "AppUserID", "dbo.AppUsers");
            DropIndex("dbo.Notifications", new[] { "AppUserID" });
            DropTable("dbo.Notifications");
        }
    }
}
