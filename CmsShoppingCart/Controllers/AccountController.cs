using CmsShoppingCart.Models.Account;
using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Views.Account;
using CmsShoppingCart.Views.Shop1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CmsShoppingCart.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return Redirect("~/account/Login");
        }

        // GET: Account/Login
        [HttpGet]       //for login to the user goto the webconfig file and add <authentication>
        public ActionResult Login()
        {
            //Confirm USer is not login 

            string username = User.Identity.Name;
            if (!string.IsNullOrEmpty(username))
                return RedirectToAction("User-profle");
            return View();
        }

        // GET: Account/login
        [HttpPost]
        public ActionResult Login(LoginUser obj)
        {
            //Check the model state
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //check if the user name is Valid

            bool isVailUser = false;
            using (Db _context = new Db())
            {
                if (_context.Users.Any(x => x.UserName.Equals(obj.UserName) && x.Password.Equals(obj.Password)))
                    isVailUser = true;


            }
            if (isVailUser == false)
            {
                ModelState.AddModelError("", "User Name or Password is Invalid");
                return View(obj);
            }
            else
            { 
                if(obj.RememberMe==true)
                {
                    FormsAuthentication.SetAuthCookie(obj.UserName, obj.RememberMe);
                    return Redirect(FormsAuthentication.GetRedirectUrl(obj.UserName, obj.RememberMe));
                }
               else
                {
                    return Redirect(FormsAuthentication.GetRedirectUrl(obj.UserName, obj.RememberMe));
                }
       

            }

        }


        // GET: CreateAccount
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }


        // GET: CreateAccount
        [ActionName("create-account")]

        [HttpPost]
        public ActionResult CreateAccount(UserVM obj)
        {


            //Check the Model State
            if (!ModelState.IsValid)
            {
                return View("CreateAccount", obj);
            }

            //Check if the password is Match
            if (!obj.Password.Equals(obj.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password and Confirm Password don't match");
                return View("CreateAccount", obj);
            }


            using (Db _context = new Db())
            {
                //Makesure the Username is unique
                if (_context.Users.Any(x => x.UserName.Equals(obj.UserName)))
                {
                    ModelState.AddModelError("", "User Name " + obj.UserName + " is Taken");
                    obj.UserName = "";
                    return View("CreateAccount", obj);
                }

                //Create UserDTo
                UserDTO objUserD = new UserDTO()
                {
                    FirstName = obj.FirstName,
                    LastName = obj.LastName,
                    UserName = obj.UserName,
                    Password = obj.Password,
                    Email = obj.Email,
                    //RoleDTOs = new List<RoleDTO>() { new RoleDTO { Id = 2 } }

                };
                //_context.Entry(objUserD.RoleDTOs).State = System.Data.Entity.EntityState.Unchanged;
                //foreach (var item in objUserD.RoleDTOs)
                //{
                //    _context.Entry(item).State = System.Data.Entity.EntityState.Unchanged;
                //}
                //Add the Data into the database

                _context.Users.Add(objUserD);

                //Save
                _context.SaveChanges();


                //Add user Role

                int id = objUserD.Id;

                UserRoles userRoles = new UserRoles()
                {
                    UserId = id,
                    RoleId = 2
                };

                _context.UserRoles.Add(userRoles);
                _context.SaveChanges();

            }
            //Create TempData Message
            TempData["SM"] = "You are now Registered and Can login";


            return Redirect("~/account/Login");
        }

      //  [Authorize]
        public ActionResult logedout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Account/Login");
        }

      //  [Authorize]

        public ActionResult UserNavPart()
        {
            string username = User.Identity.Name;

            //Declare Model

            UserNavPartial model;

            using (Db _context = new Db())
            {
                //Get theUser Name
                UserDTO obj = _context.Users.FirstOrDefault(x => x.UserName == username);

                //Built the model

                model = new UserNavPartial()
                {
                    FirstName = obj.FirstName,
                    LastName = obj.LastName
                };
            }
            return PartialView(model);
        }


        [ActionName("user-profile")]
        [HttpGet]
        //[Authorize]
        public ActionResult UserProf()
        {
            //Get user Name
            string name = User.Identity.Name;

            UserProfile model;

            using (Db _context = new Db())
            {
                UserDTO obj = _context.Users.FirstOrDefault(x => x.UserName == name);

                model = new UserProfile(obj);
            }

            //retru  View
            return View("UserProf", model);
        }


        [ActionName("user-profile")]
        [HttpPost]
        //[Authorize]
        public ActionResult UserProf(UserProfile model)
        {
            //check the model Status
            if (!ModelState.IsValid)
            {
                return View("UserProf", model);

            }

            //Check if the password is not match

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Password don't matcg");
                    return View("UserProf", model);
                }
            }

            using (Db _context = new Db())
            {
                string username = User.Identity.Name;


                //Makesure the user name is Unique

                if (_context.Users.Where(x => x.Id != model.Id).Any(x => x.UserName.Equals(username)))
                {
                    ModelState.AddModelError("", "User Name " + model.UserName + " already exist");
                    model.UserName = "";
                    return View("UserProf", model);
                }


                UserDTO obj = _context.Users.Find(model.Id);
                obj.FirstName = model.FirstName;
                obj.Email = model.Email;
                obj.LastName = model.LastName;
         
                obj.UserName = model.UserName;

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    obj.Password = model.Password;
                }


                //_context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                
            }
            TempData["SM"] = "You have Edit your Profile";
          
            //retru  View

            //return Redirect("~/account/Login");
            //return RedirectToAction("Login", "Account");
            //return RedirectToAction("Index", "Pages",new { area=""});

            //return RedirectToAction("", "Default");

            return Redirect("~/account/user-profile");

            
        }

        //Get/ Accounts/ORders
        //[Authorize(Roles="User")]
        public ActionResult Orders()
        {

            //Init the list of OrderForUserVM

            List<OrderForUserVM> ListOrderForUser = new List<OrderForUserVM>();

            using (Db _context = new Db())
            {
                //Get USer ID
                UserDTO objUser = _context.Users.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();
                int U_ID = objUser.Id;

                //List for Ordervm

                List<OrderVM> ListOrderVM = _context.orderDTOs.Where(x => x.UserDTO.Id == U_ID).ToArray().Select(x => new OrderVM(x)).ToList();

                //Loop through the lsit of OrderVM
                foreach (var order in ListOrderVM)
                {
                    //Init the product Dictionary

                    Dictionary<string, int> ProductsAndQty = new Dictionary<string, int>();

                    //Declare the total

                decimal total= 0m;

                    //Init the list of OrderDetailDTos

                    List<OrderDetailDTO> listOrderDetails = _context.orderDetailDTOs.Where(c => c.OrderDTO.Id == order.Id).ToList();

                    //Loopthrough the list of OrderDetal

                    foreach (var OdrDetail in listOrderDetails)
                    {
                        ProductDTO objProduct = _context.Products.Where(x => x.Id == OdrDetail.ProductID).FirstOrDefault();

                        //Get the Product PRice

                        decimal price = objProduct.Price;

                        //Get the Product Name
                        string ProName = objProduct.Name;

                        //Add to ProductDictionary
                        ProductsAndQty.Add(ProName, OdrDetail.Quantity);


                        //Get the Total

                        total += price * OdrDetail.Quantity;

                    }
                    //Add To ORderForUSerVM List
                    ListOrderForUser.Add(new OrderForUserVM()
                    {
                        OrderNumber = order.Id,
                        Total = total,
                        CreateAT = order.CreateAT,
                        ProductsAndQty = ProductsAndQty


                    });
                }

              


            }
                return View(ListOrderForUser);
        }
    }
}