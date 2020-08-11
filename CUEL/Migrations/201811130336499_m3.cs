namespace CUEL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "Picture", c => c.Binary());
            AddColumn("dbo.AppUsers", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUsers", "Content");
            DropColumn("dbo.AppUsers", "Picture");
        }
    }
}
