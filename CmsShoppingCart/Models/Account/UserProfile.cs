using CmsShoppingCart.Models.Account;
using CmsShoppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Views.Account
{
    public class UserProfile
    {
        public UserProfile()
        {

        }

        public UserProfile(UserDTO row)
        {
            this.Id = row.Id;
            this.FirstName = row.FirstName;
            this.LastName = row.LastName;
            this.Password = row.Password;
            this.UserName = row.UserName;
            this.Email = row.Email;
     
        }

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}