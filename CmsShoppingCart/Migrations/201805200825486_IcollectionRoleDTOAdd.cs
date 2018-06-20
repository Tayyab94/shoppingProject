namespace CmsShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IcollectionRoleDTOAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRoles", "UserDTO_Id", c => c.Int());
            CreateIndex("dbo.tblRoles", "UserDTO_Id");
            AddForeignKey("dbo.tblRoles", "UserDTO_Id", "dbo.tblUser", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblRoles", "UserDTO_Id", "dbo.tblUser");
            DropIndex("dbo.tblRoles", new[] { "UserDTO_Id" });
            DropColumn("dbo.tblRoles", "UserDTO_Id");
        }
    }
}
