using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Areas.Admin.Models.Shop
{
    public class OrderForAdminVM
    {
        public int OrderNumber { get; set; }

       public string UserName { get; set; }

        public decimal Total { get; set; }

        public Dictionary<string,int> ProductsAndQty { get; set; }


        public DateTime CreateAT { get; set; }
    }
}