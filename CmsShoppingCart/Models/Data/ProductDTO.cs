using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Models.Data
{
    [Table("tblProduct")]
    public class ProductDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
        public string ImgName { get; set; }

        public string catagoryName { get; set; }

        public int CatagoryId { get; set; }
        [ForeignKey("CatagoryId")]
        public virtual CatagoryDTO Catagory { get; set; }
    }
}