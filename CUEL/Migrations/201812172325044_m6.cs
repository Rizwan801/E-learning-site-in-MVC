namespace CUEL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FormPostComments",
                c => new
                    {
                        FormPostCommentID = c.Int(nullable: false, identity: true),
                        CommentBody = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        PostID = c.Int(),
                        AppUserID = c.Int(),
                        FormPost_FormPostID = c.Int(),
                    })
                .PrimaryKey(t => t.FormPostCommentID)
                .ForeignKey("dbo.AppUsers", t => t.AppUserID)
                .ForeignKey("dbo.FormPosts", t => t.FormPost_FormPostID)
                .Index(t => t.AppUserID)
                .Index(t => t.FormPost_FormPostID);
            
            CreateTable(
                "dbo.FormPostLikeDislikes",
                c => new
                    {
                        FormPostLikeDislikeID = c.Int(nullable: false, identity: true),
                        FormPostID = c.Int(nullable: false),
                        AppUserID = c.Int(nullable: false),
                        LikeDislike = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FormPostLikeDislikeID)
                .ForeignKey("dbo.AppUsers", t => t.AppUserID, cascadeDelete: true)
                .ForeignKey("dbo.FormPosts", t => t.FormPostID, cascadeDelete: true)
                .Index(t => t.FormPostID)
                .Index(t => t.AppUserID);
            
            AddColumn("dbo.FormPosts", "Added", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FormPostLikeDislikes", "FormPostID", "dbo.FormPosts");
            DropForeignKey("dbo.FormPostLikeDislikes", "AppUserID", "dbo.AppUsers");
            DropForeignKey("dbo.FormPostComments", "FormPost_FormPostID", "dbo.FormPosts");
            DropForeignKey("dbo.FormPostComments", "AppUserID", "dbo.AppUsers");
            DropIndex("dbo.FormPostLikeDislikes", new[] { "AppUserID" });
            DropIndex("dbo.FormPostLikeDislikes", new[] { "FormPostID" });
            DropIndex("dbo.FormPostComments", new[] { "FormPost_FormPostID" });
            DropIndex("dbo.FormPostComments", new[] { "AppUserID" });
            DropColumn("dbo.FormPosts", "Added");
            DropTable("dbo.FormPostLikeDislikes");
            DropTable("dbo.FormPostComments");
        }
    }
}
