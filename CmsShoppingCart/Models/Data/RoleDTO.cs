using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Models.Data
{
    [Table("tblRoles")]

    public class RoleDTO
    {
        public RoleDTO()
        {
            //UserDTOs = new HashSet<UserDTO>();

            UserRoles = new List<UserRoles>();
        }
        [Key]
        public int Id { get; set; }

        public string RoleName { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }

       //public virtual ICollection<UserDTO> UserDTOs { get; set; }
    }
}