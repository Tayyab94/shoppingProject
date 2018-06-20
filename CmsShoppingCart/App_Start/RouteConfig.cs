using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CmsShoppingCart
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
            routes.MapRoute("Cart", "Cart/{action}/{id}", new { controller = "Cart", action = "Index", id = UrlParameter.Optional }, new[] { "CmsShoppingCart.Controllers" });


            routes.MapRoute("Account", "Account/{action}/{id}", new { controller = "Account", action = "Index", id = UrlParameter.Optional }, new[] { "CmsShoppingCart.Controllers" });


            // THis name (Shop1) has given the main MVC controller Name . Not a Area's Controller Name and we declare this 
            // Route for the CatagoryList  show on the Client side Home Page
            routes.MapRoute("shop1", "shop1/{action}/{name}", new { controller = "shop1", action = "Index" , name = UrlParameter.Optional }, new[] { "CmsShoppingCart.Controllers" });

            

            routes.MapRoute("PageMenuPartial", "Pages/PageMenuPartial", new { controller = "Pages", action = "PageMenuPartial" }, new[] { "CmsShoppingCart.Controllers" });

            routes.MapRoute("Default", "", new { controller = "Pages", action = "Index" }, new[] { "CmsShoppingCart.Controllers" });

            routes.MapRoute("Pages", "{page}", new { controller = "Pages", action = "Index" }, new[] { "CmsShoppingCart.Controllers" });



            
        }   
    }
}
