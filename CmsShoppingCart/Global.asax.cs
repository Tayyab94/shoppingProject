using CmsShoppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CmsShoppingCart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticationReques()
        {
            //Check User if Login
            if (User == null) { return; }

            //Get The User Name 
            string username = Context.User.Identity.Name;

            //Declare the Arrayof the Roles
            string[] roles = null;

            using (Db _context = new Db())
            {
                //Populate the USer

                UserDTO objUSer = _context.Users.Include(a=>a.userRoles).FirstOrDefault(x => x.UserName == username);
                foreach (var item in objUSer.userRoles)
                {
                    roles = _context.UserRoles.Where(x => x.UserId == objUSer.Id).Select(x => item.Role.RoleName).ToArray();
                }
              //  roles = _context.UserRoles.Where(x => x.UserId == objUSer.Id).Select(x => x.Role.RoleName).ToArray();
                //  UserDTO d = _context.Users.Include(a => a.RoleDTOs.Select(x => x.RoleName).ToArray()).FirstOrDefault(x => x.UserName == username);
                
            }


            //Built the IPrinciple Object

            IIdentity userIdentity = new GenericIdentity(username);
            IPrincipal newUserObj = new GenericPrincipal(userIdentity,roles);
            

            //Update Context.User
            Context.User = newUserObj;
        }
    }
}
