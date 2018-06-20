using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Models.Data
{
    public class Db:DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<UserDTO>()
            //    .HasMany<RoleDTO>(x => x.RoleDTOs)
            //    .WithMany(x => x.UserDTOs)
            //    .Map(c =>
            //    {
            //        c.MapLeftKey("UserID");
            //        c.MapRightKey("RoleID");
            //        c.ToTable("UsersRole");


            //    });
        }

        public DbSet<PagesDTO> Pages { get; set; }
        public DbSet<Sidebar> Sidebar { get; set; }

        public DbSet<CatagoryDTO> Catagories { get; set; }
        public DbSet<ProductDTO> Products { get; set; }

        public DbSet<UserDTO> Users { get; set; }

        public DbSet<RoleDTO> Roles { get; set; }

        //public DbSet<UserRoles> UserRoles { get; set; }


        public DbSet<OrderDTO> orderDTOs { get; set; }

        public DbSet<OrderDetailDTO> orderDetailDTOs { get; set; }

        public DbSet<UserRoles> UserRoles { get; set; }
    }
}