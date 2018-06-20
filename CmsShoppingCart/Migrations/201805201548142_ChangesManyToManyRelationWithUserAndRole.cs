namespace CmsShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesManyToManyRelationWithUserAndRole : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserDTORoleDTOes", newName: "UsersRole");
            DropForeignKey("dbo.tblUserRoles", "RoleId", "dbo.tblRoles");
            DropForeignKey("dbo.tblUserRoles", "UserId", "dbo.tblUser");
            DropIndex("dbo.tblUserRoles", new[] { "UserId" });
            DropIndex("dbo.tblUserRoles", new[] { "RoleId" });
            RenameColumn(table: "dbo.UsersRole", name: "UserDTO_Id", newName: "UserID");
            RenameColumn(table: "dbo.UsersRole", name: "RoleDTO_Id", newName: "RoleID");
            RenameIndex(table: "dbo.UsersRole", name: "IX_UserDTO_Id", newName: "IX_UserID");
            RenameIndex(table: "dbo.UsersRole", name: "IX_RoleDTO_Id", newName: "IX_RoleID");
            DropTable("dbo.tblUserRoles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tblUserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId });
            
            RenameIndex(table: "dbo.UsersRole", name: "IX_RoleID", newName: "IX_RoleDTO_Id");
            RenameIndex(table: "dbo.UsersRole", name: "IX_UserID", newName: "IX_UserDTO_Id");
            RenameColumn(table: "dbo.UsersRole", name: "RoleID", newName: "RoleDTO_Id");
            RenameColumn(table: "dbo.UsersRole", name: "UserID", newName: "UserDTO_Id");
            CreateIndex("dbo.tblUserRoles", "RoleId");
            CreateIndex("dbo.tblUserRoles", "UserId");
            AddForeignKey("dbo.tblUserRoles", "UserId", "dbo.tblUser", "Id", cascadeDelete: true);
            AddForeignKey("dbo.tblUserRoles", "RoleId", "dbo.tblRoles", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.UsersRole", newName: "UserDTORoleDTOes");
        }
    }
}
