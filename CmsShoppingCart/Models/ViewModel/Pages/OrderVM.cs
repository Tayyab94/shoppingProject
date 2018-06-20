using CmsShoppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Views.Shop1
{
    public class OrderVM
    {
        public OrderVM()
        {

        }

        public OrderVM(OrderDTO obj)
        {
            this.Id = obj.Id;
            this.USerID = obj.USerID;
            this.CreateAT = obj.CreateAT;
        }
        public int Id { get; set; }

        public int USerID { get; set; }

        public DateTime CreateAT { get; set; }
    }
}