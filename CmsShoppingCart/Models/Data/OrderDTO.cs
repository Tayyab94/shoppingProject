using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Models.Data
{
    [Table("tblOrder")]
    public class OrderDTO
    {

        [Key]
        public int Id { get; set; }

        public int USerID { get; set; }

        public DateTime CreateAT { get; set; }


       [ForeignKey("USerID")]
        public virtual UserDTO UserDTO { get; set; }
    }
}