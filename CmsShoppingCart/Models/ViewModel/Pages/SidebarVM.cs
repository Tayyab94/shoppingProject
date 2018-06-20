using CmsShoppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Models.ViewModel.Pages
{
    public class SidebarVM
    {
        public SidebarVM()
        {

        }
        public SidebarVM(Sidebar row)
        {
            ID = row.ID;
            Body = row.Body;
        }
        public int ID { get; set; }

        [AllowHtml]
        public string Body { get; set; }
    }
}