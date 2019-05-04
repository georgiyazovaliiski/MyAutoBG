namespace AutoPartsMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductsInCarts", "CheckedOut", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProductsInCarts", "OrderId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductsInCarts", "OrderId");
            DropColumn("dbo.ProductsInCarts", "CheckedOut");
        }
    }
}
