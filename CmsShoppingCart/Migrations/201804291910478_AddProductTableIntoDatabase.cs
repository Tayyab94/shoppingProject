namespace CmsShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductTableIntoDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Slug = c.String(),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImgName = c.String(),
                        catagoryName = c.String(),
                        CatagoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblCatagories", t => t.CatagoryId, cascadeDelete: true)
                .Index(t => t.CatagoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblProduct", "CatagoryId", "dbo.tblCatagories");
            DropIndex("dbo.tblProduct", new[] { "CatagoryId" });
            DropTable("dbo.tblProduct");
        }
    }
}
