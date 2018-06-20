namespace CmsShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SidearTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblSidebar",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblSidebar");
        }
    }
}
