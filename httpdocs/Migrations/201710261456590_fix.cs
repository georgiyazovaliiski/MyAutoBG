namespace AutoPartsMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "DateOfOrderStart", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Orders", "DateOfOrderFinish", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "DateOfOrderFinish", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "DateOfOrderStart", c => c.DateTime(nullable: false));
        }
    }
}
