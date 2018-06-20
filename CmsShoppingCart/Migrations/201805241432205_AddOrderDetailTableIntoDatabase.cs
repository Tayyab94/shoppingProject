namespace CmsShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderDetailTableIntoDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblOrderDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblOrder", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.tblProduct", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblOrderDetail", "ProductID", "dbo.tblProduct");
            DropForeignKey("dbo.tblOrderDetail", "OrderID", "dbo.tblOrder");
            DropIndex("dbo.tblOrderDetail", new[] { "ProductID" });
            DropIndex("dbo.tblOrderDetail", new[] { "OrderID" });
            DropTable("dbo.tblOrderDetail");
        }
    }
}
