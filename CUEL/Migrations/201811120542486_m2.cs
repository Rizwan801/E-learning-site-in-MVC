namespace CUEL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostLikeDislikes",
                c => new
                    {
                        PostLikeDislikeID = c.Int(nullable: false, identity: true),
                        PostID = c.Int(nullable: false),
                        AppUserID = c.Int(nullable: false),
                        LikeDislike = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostLikeDislikeID)
                .ForeignKey("dbo.AppUsers", t => t.AppUserID, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .Index(t => t.PostID)
                .Index(t => t.AppUserID);
            
            AddColumn("dbo.PostComments", "DateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Posts", "Added", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostLikeDislikes", "PostID", "dbo.Posts");
            DropForeignKey("dbo.PostLikeDislikes", "AppUserID", "dbo.AppUsers");
            DropIndex("dbo.PostLikeDislikes", new[] { "AppUserID" });
            DropIndex("dbo.PostLikeDislikes", new[] { "PostID" });
            DropColumn("dbo.Posts", "Added");
            DropColumn("dbo.PostComments", "DateTime");
            DropTable("dbo.PostLikeDislikes");
        }
    }
}
