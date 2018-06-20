using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using CmsShoppingCart.Models.Cart;
using CmsShoppingCart.Models.Data;

namespace CmsShoppingCart.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            //Init the Cart list
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();


            //check if the cart is empty
            if (cart.Count == 0 && Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View();

            }

            // Calculate the total and sve it to the ViewBag 


            decimal total = 0m;
            foreach (var item in cart)
            {
                total += item.Total;
            }
            ViewBag.GrandTotal = total;

            //return a cart_List
            return View(cart);
        }

        public ActionResult CartPartial()
        {
            //Init CartVM
            CartVM modalCart = new CartVM();

            //Init Qty
            int qty = 0;
            decimal price = 0m;


            //Check for the Cart Session

            if (Session["cart"] != null)
            {

                //Get the Total Qty and Price
                var list = (List<CartVM>)Session["cart"];

                foreach (var item in list)
                {
                    qty  += item.Quantity;
                    price  += item.Quantity * item.Price;

                }

                modalCart.Quantity = qty;
                modalCart.Price = price;
              

            }
            else
            {
                //or set the quantity And Price 0

                modalCart.Price = 0;
                modalCart.Quantity = 0;
            }

            //Return PartialView With Modal
            return PartialView(modalCart);
        }


        public ActionResult AddToCartPartial(int id)
        {

            //Init the Cart List

            List<CartVM> cart_List = Session["cart"] as List<CartVM> ?? new List<CartVM>();


            //Init the Cart_Vm

            CartVM model = new CartVM();

            using (Db _context = new Db())
            {
                //Get the Product

                ProductDTO objProduct = _context.Products.Find(id);

                //Check if the Product is Already in the Cart

                var ProductInCart = cart_List.FirstOrDefault(x => x.ProductID == id);

                //If Not add new Cart
                if (ProductInCart == null)
                {
                    cart_List.Add(new CartVM()
                    {
                        ProductID=objProduct.Id,
                        ProductName=objProduct.Name,
                        Quantity=1,
                        Price=objProduct.Price,
                        ProductImage=objProduct.ImgName

                    });
                }
                else
                {            //if it is , just incremnt the Product's Quantity
                    ProductInCart.Quantity++;
                }
            }

            //Get Total Quantity And the Price 
            int quntity = 0;
            decimal pric = 0m;

            //Loop THroug the Cart

            foreach (var item in cart_List)
            {
                quntity += item.Quantity;
                pric += item.Quantity * item.Price;
            }
            //Set the quantity in to the Mdoel
            model.Quantity = quntity;
            model.Price = pric;

            //Save back to Session

            Session["cart"] = cart_List;

            //return PartialView With the model 
            return PartialView("/Views/Cart/_addTocartPartial.cshtml", model);
        }

        // GET: Cart/IncrementProduct/id
        public JsonResult IncrementProduct(int id)
        {
            //Init the CartList
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db _context = new Db())
            {
                //Get the Cart_VM_data form the CarList
                CartVM model = cart.FirstOrDefault(x => x.ProductID == id);

                //Increment the Quantity
                model.Quantity++;

                // Store Needed Data

                var result = new { qty = model.Quantity, price = model.Price };

                //Return the Json with the Data
                return Json(result, JsonRequestBehavior.AllowGet);
            }
             
        }



        // GET: Cart/IncrementProduct/id
        public ActionResult DecrementProduct(int productID)
        {
            string NotificationMessage = null;
            //Init the CartList
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db _context = new Db())
            {
                //Get the Cart_VM_data form the CarList
                CartVM model = cart.FirstOrDefault(x => x.ProductID == productID);

                //if(model.Quantity<0)
                //{
                //    return RedirectToAction("Index", "Cart");
                //}

                //Increment the Quantity
                if(model.Quantity>1)
                {
                    model.Quantity--;

                    NotificationMessage = "you have decrease the Quantity of "+ model.ProductName +" product";
                }
                else
                {
                    model.Quantity=0;
                    cart.Remove(model);
                    NotificationMessage = "Your Item is Successfuly Remove form the cart";
                }
               

                // Store Needed Data

                var result = new { qty = model.Quantity, price = model.Price,msg= NotificationMessage };

                
                //Return the Json with the Data
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public void RemoveProduct(int id)
        {
            //Init the CartLisr
            List<CartVM> cart = Session["cart"] as List<CartVM>;
            using (Db _context =new  Db())
            {

                //Get the Model
                CartVM model = cart.FirstOrDefault(x => x.ProductID == id);
                cart.Remove(model);

            }
        }



        //Increment decrement with Paypal Account
        public ActionResult PaypalPartial()
        {
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            return PartialView(cart); //PArtial VIew with the Same name 

        }



        //Place and Order 

        [HttpPost]
        public void PlaceAnOrder()
        {
            //Get Cart List

            List<CartVM> cart = Session["cart"] as List<CartVM>;

            //Get the USer Name

            string userName = User.Identity.Name;

            //Declare the OrderId

            int OrderId = 0;


            using (Db _context = new Db())
            {
                //Init the OrderDTO

                OrderDTO objOrder = new OrderDTO();

                //Get the USerID

                var U_ID = _context.Users.FirstOrDefault(x => x.UserName == userName);
                int userID = U_ID.Id;

                objOrder.USerID = userID;
                objOrder.CreateAT = DateTime.Now;
                _context.orderDTOs.Add(objOrder);
                _context.SaveChanges();


                //Get the Inserted  ORder ID

                OrderId = objOrder.Id;


                //Init the orderDetailDTO

                OrderDetailDTO objOrderDetail = new OrderDetailDTO();

                foreach (var item in cart)
                {
                    objOrderDetail.OrderID = OrderId;
                    objOrderDetail.ProductID = item.ProductID;
                    objOrderDetail.Quantity = item.Quantity;

                    _context.orderDetailDTOs.Add(objOrderDetail);
                    _context.SaveChanges();
                }
                
            }

            //Send EMami

            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("5e1b682e5351ae", "7dd318409aa807"),
                EnableSsl = true
            };
            client.Send("tayyab.bhatti30@gmail.com", "tayyab.bhatti30@mail.com", "Hello world", "Your Order"+OrderId);
            

            //Reset the Session

            Session["cart"] = null;
        }
    }
}