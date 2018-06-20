namespace CmsShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IcollectionUserDTOAdd_intoUserRoleTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tblRoles", "UserDTO_Id", "dbo.tblUser");
            DropIndex("dbo.tblRoles", new[] { "UserDTO_Id" });
            CreateTable(
                "dbo.UserDTORoleDTOes",
                c => new
                    {
                        UserDTO_Id = c.Int(nullable: false),
                        RoleDTO_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserDTO_Id, t.RoleDTO_Id })
                .ForeignKey("dbo.tblUser", t => t.UserDTO_Id, cascadeDelete: true)
                .ForeignKey("dbo.tblRoles", t => t.RoleDTO_Id, cascadeDelete: true)
                .Index(t => t.UserDTO_Id)
                .Index(t => t.RoleDTO_Id);
            
            DropColumn("dbo.tblRoles", "UserDTO_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblRoles", "UserDTO_Id", c => c.Int());
            DropForeignKey("dbo.UserDTORoleDTOes", "RoleDTO_Id", "dbo.tblRoles");
            DropForeignKey("dbo.UserDTORoleDTOes", "UserDTO_Id", "dbo.tblUser");
            DropIndex("dbo.UserDTORoleDTOes", new[] { "RoleDTO_Id" });
            DropIndex("dbo.UserDTORoleDTOes", new[] { "UserDTO_Id" });
            DropTable("dbo.UserDTORoleDTOes");
            CreateIndex("dbo.tblRoles", "UserDTO_Id");
            AddForeignKey("dbo.tblRoles", "UserDTO_Id", "dbo.tblUser", "Id");
        }
    }
}
