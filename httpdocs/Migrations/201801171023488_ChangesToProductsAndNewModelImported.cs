namespace AutoPartsMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToProductsAndNewModelImported : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Email", c => c.String());
            AddColumn("dbo.Products", "PhoneNumber", c => c.String());
            AddColumn("dbo.Products", "FirstName", c => c.String());
            AddColumn("dbo.Products", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "LastName");
            DropColumn("dbo.Products", "FirstName");
            DropColumn("dbo.Products", "PhoneNumber");
            DropColumn("dbo.Products", "Email");
        }
    }
}
