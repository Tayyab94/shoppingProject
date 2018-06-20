namespace CmsShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTblUSerRoleTableFromDataBAse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tblUsersRoles", "RoleId", "dbo.tblRoles");
            DropForeignKey("dbo.tblUsersRoles", "UserId", "dbo.tblUser");
            DropIndex("dbo.tblUsersRoles", new[] { "UserId" });
            DropIndex("dbo.tblUsersRoles", new[] { "RoleId" });
            DropTable("dbo.tblUsersRoles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tblUsersRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId });
            
            CreateIndex("dbo.tblUsersRoles", "RoleId");
            CreateIndex("dbo.tblUsersRoles", "UserId");
            AddForeignKey("dbo.tblUsersRoles", "UserId", "dbo.tblUser", "Id", cascadeDelete: true);
            AddForeignKey("dbo.tblUsersRoles", "RoleId", "dbo.tblRoles", "Id", cascadeDelete: true);
        }
    }
}
