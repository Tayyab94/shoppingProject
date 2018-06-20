namespace CmsShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderTableInToDataBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        USerID = c.Int(nullable: false),
                        CreateAT = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblUser", t => t.USerID, cascadeDelete: true)
                .Index(t => t.USerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblOrder", "USerID", "dbo.tblUser");
            DropIndex("dbo.tblOrder", new[] { "USerID" });
            DropTable("dbo.tblOrder");
        }
    }
}
