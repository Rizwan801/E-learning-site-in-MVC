namespace CUEL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m7 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.FormPostComments", name: "FormPost_FormPostID", newName: "FormPostID");
            RenameIndex(table: "dbo.FormPostComments", name: "IX_FormPost_FormPostID", newName: "IX_FormPostID");
            DropColumn("dbo.FormPostComments", "PostID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FormPostComments", "PostID", c => c.Int());
            RenameIndex(table: "dbo.FormPostComments", name: "IX_FormPostID", newName: "IX_FormPost_FormPostID");
            RenameColumn(table: "dbo.FormPostComments", name: "FormPostID", newName: "FormPost_FormPostID");
        }
    }
}
