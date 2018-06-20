using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Models.Data
{
    [Table("tblUser")]
    public class UserDTO
    {
        public UserDTO()
        {
            //RoleDTOs =new  List<RoleDTO>();

            userRoles = new List<UserRoles>();
        }
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public string UserName { get; set; }

        public ICollection<UserRoles> userRoles { get; set; }
        //public virtual ICollection<RoleDTO> RoleDTOs { get; set; }
    }
}