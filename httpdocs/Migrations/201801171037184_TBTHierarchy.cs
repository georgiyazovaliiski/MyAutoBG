namespace AutoPartsMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TBTHierarchy : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductsFromClient",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Approved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Id)
                .Index(t => t.Id);
            
            DropColumn("dbo.Products", "Email");
            DropColumn("dbo.Products", "PhoneNumber");
            DropColumn("dbo.Products", "FirstName");
            DropColumn("dbo.Products", "LastName");
            DropColumn("dbo.Products", "Approved");
            DropColumn("dbo.Products", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Products", "Approved", c => c.Boolean());
            AddColumn("dbo.Products", "LastName", c => c.String());
            AddColumn("dbo.Products", "FirstName", c => c.String());
            AddColumn("dbo.Products", "PhoneNumber", c => c.String());
            AddColumn("dbo.Products", "Email", c => c.String());
            DropForeignKey("dbo.ProductsFromClient", "Id", "dbo.Products");
            DropIndex("dbo.ProductsFromClient", new[] { "Id" });
            DropTable("dbo.ProductsFromClient");
        }
    }
}
