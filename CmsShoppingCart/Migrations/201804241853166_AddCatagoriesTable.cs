namespace CmsShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCatagoriesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblCatagories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        Slug = c.String(maxLength: 4000),
                        Sorting = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblCatagories");
        }
    }
}
