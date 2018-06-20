using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Models.Data
{
    [Table("tblCatagories")]
    public class CatagoryDTO
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName ="nvarchar")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar")]
        public string Slug { get; set; }
        public int Sorting { get; set; }
    }
}