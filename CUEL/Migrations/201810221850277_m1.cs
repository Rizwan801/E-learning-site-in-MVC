namespace CUEL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        AppUserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        FullName = c.String(nullable: false),
                        FatherName = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        DepartmentID = c.Int(),
                        RegNo = c.String(maxLength: 20),
                        BatchID = c.Int(),
                        UserType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppUserID)
                .ForeignKey("dbo.Batches", t => t.BatchID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID)
                .Index(t => t.DepartmentID)
                .Index(t => t.BatchID);
            
            CreateTable(
                "dbo.Batches",
                c => new
                    {
                        BatchID = c.Int(nullable: false, identity: true),
                        BatchNo = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BatchID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
            CreateTable(
                "dbo.FormPosts",
                c => new
                    {
                        FormPostID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        File = c.Binary(),
                        FileType = c.String(),
                        AppUserID = c.Int(),
                        DiscussionFormID = c.Int(),
                    })
                .PrimaryKey(t => t.FormPostID)
                .ForeignKey("dbo.AppUsers", t => t.AppUserID)
                .ForeignKey("dbo.DiscussionForms", t => t.DiscussionFormID)
                .Index(t => t.AppUserID)
                .Index(t => t.DiscussionFormID);
            
            CreateTable(
                "dbo.DiscussionForms",
                c => new
                    {
                        DiscussionFormID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.DiscussionFormID);
            
            CreateTable(
                "dbo.UserDiscussions",
                c => new
                    {
                        UserDiscussionID = c.Int(nullable: false, identity: true),
                        AppUserID = c.Int(),
                        DiscussionFormID = c.Int(),
                    })
                .PrimaryKey(t => t.UserDiscussionID)
                .ForeignKey("dbo.AppUsers", t => t.AppUserID)
                .ForeignKey("dbo.DiscussionForms", t => t.DiscussionFormID)
                .Index(t => t.AppUserID)
                .Index(t => t.DiscussionFormID);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        MessageBody = c.String(),
                        AppUserID = c.Int(),
                        ChatID = c.Int(),
                    })
                .PrimaryKey(t => t.MessageID)
                .ForeignKey("dbo.AppUsers", t => t.AppUserID)
                .ForeignKey("dbo.Chats", t => t.ChatID)
                .Index(t => t.AppUserID)
                .Index(t => t.ChatID);
            
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        ChatID = c.Int(nullable: false, identity: true),
                        AppUser1ID = c.Int(nullable: false),
                        AppUser2ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChatID);
            
            CreateTable(
                "dbo.PostComments",
                c => new
                    {
                        PostCommentID = c.Int(nullable: false, identity: true),
                        CommentBody = c.String(),
                        PostID = c.Int(),
                        AppUserID = c.Int(),
                    })
                .PrimaryKey(t => t.PostCommentID)
                .ForeignKey("dbo.AppUsers", t => t.AppUserID)
                .ForeignKey("dbo.Posts", t => t.PostID)
                .Index(t => t.PostID)
                .Index(t => t.AppUserID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        File = c.Binary(),
                        FileType = c.String(),
                        AppUserID = c.Int(),
                    })
                .PrimaryKey(t => t.PostID)
                .ForeignKey("dbo.AppUsers", t => t.AppUserID)
                .Index(t => t.AppUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostComments", "PostID", "dbo.Posts");
            DropForeignKey("dbo.Posts", "AppUserID", "dbo.AppUsers");
            DropForeignKey("dbo.PostComments", "AppUserID", "dbo.AppUsers");
            DropForeignKey("dbo.Messages", "ChatID", "dbo.Chats");
            DropForeignKey("dbo.Messages", "AppUserID", "dbo.AppUsers");
            DropForeignKey("dbo.UserDiscussions", "DiscussionFormID", "dbo.DiscussionForms");
            DropForeignKey("dbo.UserDiscussions", "AppUserID", "dbo.AppUsers");
            DropForeignKey("dbo.FormPosts", "DiscussionFormID", "dbo.DiscussionForms");
            DropForeignKey("dbo.FormPosts", "AppUserID", "dbo.AppUsers");
            DropForeignKey("dbo.AppUsers", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.AppUsers", "BatchID", "dbo.Batches");
            DropIndex("dbo.Posts", new[] { "AppUserID" });
            DropIndex("dbo.PostComments", new[] { "AppUserID" });
            DropIndex("dbo.PostComments", new[] { "PostID" });
            DropIndex("dbo.Messages", new[] { "ChatID" });
            DropIndex("dbo.Messages", new[] { "AppUserID" });
            DropIndex("dbo.UserDiscussions", new[] { "DiscussionFormID" });
            DropIndex("dbo.UserDiscussions", new[] { "AppUserID" });
            DropIndex("dbo.FormPosts", new[] { "DiscussionFormID" });
            DropIndex("dbo.FormPosts", new[] { "AppUserID" });
            DropIndex("dbo.AppUsers", new[] { "BatchID" });
            DropIndex("dbo.AppUsers", new[] { "DepartmentID" });
            DropTable("dbo.Posts");
            DropTable("dbo.PostComments");
            DropTable("dbo.Chats");
            DropTable("dbo.Messages");
            DropTable("dbo.UserDiscussions");
            DropTable("dbo.DiscussionForms");
            DropTable("dbo.FormPosts");
            DropTable("dbo.Departments");
            DropTable("dbo.Batches");
            DropTable("dbo.AppUsers");
        }
    }
}
