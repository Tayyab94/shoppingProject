namespace CmsShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MAnipulationUSerROleEtc : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UsersRole", "UserID", "dbo.tblUser");
            DropForeignKey("dbo.UsersRole", "RoleID", "dbo.tblRoles");
            DropIndex("dbo.UsersRole", new[] { "UserID" });
            DropIndex("dbo.UsersRole", new[] { "RoleID" });
            CreateTable(
                "dbo.tblUsersRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.tblRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.tblUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            DropTable("dbo.UsersRole");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UsersRole",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        RoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.RoleID });
            
            DropForeignKey("dbo.tblUsersRoles", "UserId", "dbo.tblUser");
            DropForeignKey("dbo.tblUsersRoles", "RoleId", "dbo.tblRoles");
            DropIndex("dbo.tblUsersRoles", new[] { "RoleId" });
            DropIndex("dbo.tblUsersRoles", new[] { "UserId" });
            DropTable("dbo.tblUsersRoles");
            CreateIndex("dbo.UsersRole", "RoleID");
            CreateIndex("dbo.UsersRole", "UserID");
            AddForeignKey("dbo.UsersRole", "RoleID", "dbo.tblRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UsersRole", "UserID", "dbo.tblUser", "Id", cascadeDelete: true);
        }
    }
}
