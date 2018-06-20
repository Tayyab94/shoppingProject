using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Models.Data
{
    [Table("tblSidebar")]
    public class Sidebar
    {
        [Key]
        public int ID { get; set; }

        public string Body { get; set; }
    }
}