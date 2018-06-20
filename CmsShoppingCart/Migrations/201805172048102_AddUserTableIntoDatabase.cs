namespace CmsShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserTableIntoDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblUser");
        }
    }
}
