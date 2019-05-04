namespace AutoPartsMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class yeah : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "urlName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "urlName");
        }
    }
}
