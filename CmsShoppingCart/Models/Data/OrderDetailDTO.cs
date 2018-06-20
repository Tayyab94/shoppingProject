using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Models.Data
{
    [Table("tblOrderDetail")]
    public class OrderDetailDTO
    {
        [Key]
        public int Id { get; set; }

        public int OrderID { get; set; }

        //public int UserID { get; set; }

        public int ProductID { get; set; }


        public int Quantity { get; set; }

        [ForeignKey("OrderID")]
        public virtual OrderDTO OrderDTO{ get; set; }

        //[ForeignKey("UserID")]
        //public virtual UserDTO UserDTO { get; set; }

        [ForeignKey("ProductID")]

        public virtual ProductDTO ProductDTO { get; set; }






    }
}