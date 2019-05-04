namespace AutoPartsMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newfix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderProductQuantities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrderProductQuantities");
        }
    }
}
