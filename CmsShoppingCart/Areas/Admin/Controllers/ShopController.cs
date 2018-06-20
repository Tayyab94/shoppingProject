using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModel.Pages;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using CmsShoppingCart.Models;
using CmsShoppingCart.Areas.Admin.Models.Shop;
using CmsShoppingCart.Views.Shop1;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Catagory
        public ActionResult Catagory()
        {

            List<CataogryVM> CatagoryList;
            using (Db _context = new Db())
            {
                CatagoryList = _context.Catagories.ToArray().OrderBy(c => c.Name).Select(c => new CataogryVM(c)).ToList();
            }
            return View(CatagoryList);
        }

        public string AddNewCatagory(string catName)
        {
            //Declare ID
            string Id;
            using (Db _context = new Db())
            {
                //Check that the catagory name is unique
                if (_context.Catagories.Any(x => x.Name == catName))
                {
                    return "titletaken";
                }
                //Initi CatagoryDTO

                CatagoryDTO objCatagory = new CatagoryDTO();
                //Add to DTo
                objCatagory.Name = catName;
                objCatagory.Slug = catName.Replace(" ", "-").ToLower();
                objCatagory.Sorting = 100;

                //Save to DTO

                _context.Catagories.Add(objCatagory);
                _context.SaveChanges();

                Id = objCatagory.Name;
            }
            return Id;
        }

        public ActionResult sample(string obj)
        {
            string Id;
            string ms;
            using (Db _context = new Db())
            {
                if (obj == null)
                {
                    return Json(new { Msg1 = "Please Enter The Catargory Name..." }, JsonRequestBehavior.AllowGet);
                }
                //Check that the catagory name is unique
                if (_context.Catagories.Any(x => x.Name == obj) || obj == null)
                {
                    return Json(new { Msg = "Already Exist" }, JsonRequestBehavior.AllowGet);
                }
                //Initi CatagoryDTO

                CatagoryDTO objCatagory = new CatagoryDTO();
                //Add to DTo
                objCatagory.Name = obj;
                objCatagory.Slug = obj.Replace(" ", "-").ToLower();
                objCatagory.Sorting = 100;

                //Save to DTO

                _context.Catagories.Add(objCatagory);
                _context.SaveChanges();
                Id = objCatagory.Id.ToString();
                ms = "Data Save ho gya";
            }


            return Json(new { SampleName = obj, ID = Id, Ms = ms }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // Post: Admin/shop/ReorderCatagories
        public void ReorderCatagories(int[] id)
        {
            using (Db _context = new Db())
            {
                //Init veriable Count with 1
                int cout = 1;

                CatagoryDTO obj;


                //Set Sorting for each page

                foreach (var cataId in id)
                {
                    obj = _context.Catagories.Find(cataId);
                    obj.Sorting = cout;
                    _context.SaveChanges();
                    cout++;
                }
            }
        }

        // Post: Admin/shop/DeleteCatagory/id
        public ActionResult DeleteCatagory(int id)
        {
            using (Db _context = new Db())
            {
                //GEt the PAge
                CatagoryDTO obj1 = _context.Catagories.Find(id);

                //Remove the Page
                _context.Catagories.Remove(obj1);
                //Save_Changes

                _context.SaveChanges();

            }
            return RedirectToAction("Catagory");
        }

        // Post: Admin/shop/EditCatagory/id
        [HttpPost]
        public string EditCatagory(string newCataName, int id)
        {
            using (Db _context = new Db())
            {
                if (_context.Catagories.Any(a => a.Name == newCataName))
                {
                    return "titletaken";
                }
                CatagoryDTO obj = _context.Catagories.Find(id);
                obj.Name = newCataName;
                obj.Slug = newCataName.Replace(" ", "-").ToLower();

                _context.SaveChanges();
            }

            return "ok";
        }

        // Admin/shop/AddProduct
        public ActionResult AddProduct()
        {
            //Init Model
            ProductVM model = new ProductVM();

            //Add Select List of Catagort to model
            using (Db _context = new Db())
            {
                model.Catagories = new SelectList(_context.Catagories.ToList(), "Id", "Name");
            }

            return View(model);

        }

        [HttpPost]
        public ActionResult AddProduct(ProductVM obj, HttpPostedFileBase file)
        {
            //Check ModelState is Valid
            if (!ModelState.IsValid)
            {
                using (Db _context = new Db())
                {
                    obj.Catagories = new SelectList(_context.Catagories.ToList(), "Id", "Name");
                    return View(obj);
                }
            }

            //MakeSure the Product Name is unique

            using (Db _context = new Db())
            {
                if (_context.Products.Any(a => a.Name == obj.Name))
                {

                    obj.Catagories = new SelectList(_context.Catagories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "The Product Name is Taken..!");
                    return View(obj);
                }
            }

            //Declare Product ID
            int Id;
            //Init And save ProductDTO
            using (Db _context = new Db())
            {
                ProductDTO objPro = new ProductDTO();
                objPro.Name = obj.Name;
                objPro.Description = obj.Description;
                objPro.Slug = obj.Name.Replace(" ", "-").ToLower();
                objPro.Price = obj.Price;
                objPro.CatagoryId = obj.CatagoryId;

                CatagoryDTO objCat = _context.Catagories.FirstOrDefault(x => x.Id == obj.CatagoryId);
                objPro.catagoryName = objCat.Name;

                _context.Products.Add(objPro);
                _context.SaveChanges();

                //Get the ID

                Id = objPro.Id;
            }


            //Set tempData Message
            TempData["SM"] = "You Have Added the product Succesfuly";


            #region Upload_Image

            //Create The Necessary Directories
            var OriginalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            string PathString1 = Path.Combine(OriginalDirectory.ToString(), "Products");
            string PathString2 = Path.Combine(OriginalDirectory.ToString(), "Products\\" + Id.ToString());
            string PathString3 = Path.Combine(OriginalDirectory.ToString(), "Products\\" + Id.ToString() + "\\Thumbs");
            string PathString4 = Path.Combine(OriginalDirectory.ToString(), "Products\\" + Id.ToString() + "\\Gallary");
            string PathString5 = Path.Combine(OriginalDirectory.ToString(), "Products\\" + Id.ToString() + "\\Gallary\\Thumbs");

            if (!Directory.Exists(PathString1))
            {
                Directory.CreateDirectory(PathString1);
            }
            if (!Directory.Exists(PathString2))
            {
                Directory.CreateDirectory(PathString2);
            }
            if (!Directory.Exists(PathString3))
            {
                Directory.CreateDirectory(PathString3);

            }
            if (!Directory.Exists(PathString4))
            {
                Directory.CreateDirectory(PathString4);
            }
            if (!Directory.Exists(PathString5))
            {
                Directory.CreateDirectory(PathString5);
            }

            //Check If the File was Upload
            if (file != null && file.ContentLength > 0)
            {
                //Get the File Extension 
                string ex = file.ContentType.ToLower();

                //Verify the file Extension
                if (ex != "image/jpg" &&
                    ex != "image/jpeg" &&
                    ex != "image/pjepg" &&
                    ex != "image/gif" &&
                    ex != "image/x-png" &&
                    ex != "image/png")
                {
                    using (Db _context = new Db())
                    {
                        obj.Catagories = new SelectList(_context.Catagories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "The Image was not Uploaded- Wrong Image Extension..!");
                        return View(obj);
                    }
                }

                //Init the Image Name
                string ImagName = file.FileName;

                //Save ImageName to DTO
                using (Db _context = new Db())
                {
                    ProductDTO objP = _context.Products.Find(Id);
                    objP.ImgName = ImagName;
                    _context.SaveChanges();
                }

                //Set Original And Thumb  Images Paths


                var path1 = string.Format("{0}\\{1}", PathString2, ImagName);
                var path2 = string.Format("{0}\\{1}", PathString3, ImagName);

                //Save  Original
                file.SaveAs(path1);

                //Create and Save Thumb 
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }
            #endregion

            return RedirectToAction("AddProduct");
        }


        // GET: Admin/Shop/CatagoryshowProducts
        public ActionResult showProducts(int? page, int? CatId)
        {
            //Declare the list of ProductVM 
            List<ProductVM> ListOfProductVM;

            //Set the PageNumber
            var PageNumber = page ?? 1;
            using (Db _context = new Db())
            {
                ListOfProductVM = _context.Products.ToArray()
                                         .Where(x => CatId == null || CatId == 0 || x.CatagoryId == CatId)
                                         .Select(x => new ProductVM(x)).ToList();

                //Populate the Catagory Select list

                ViewBag.l_Catagories = new SelectList(_context.Catagories.ToList(), "Id", "Name");

                //Set the Selected Catagories

                ViewBag.SelectedCatagories = CatId.ToString();
            }

            //Set the pagination   (For this we must Install the PagedList.Mvc form Nuget Packages.. and import the Libraty)

            var onePageOfProduct = ListOfProductVM.ToPagedList(PageNumber, 3);

            ViewBag.OnePageOfProduct = onePageOfProduct;

            return View(ListOfProductVM);
        }

        // Admin/shop/EditProduct/id
        [HttpGet]
        public ActionResult EditProduct(int id)
        {

            //Delcare model        
            ProductVM model;

            using (Db _context = new Db())
            {

                //Get Products
                ProductDTO obj = _context.Products.Find(id);

                //Make Sure that the product exist...
                if (obj == null)
                {
                    return Content("That product does not Exist...");
                }

                //Init Model ProductSVM
                model = new ProductVM(obj);

                //Make catagory select list.
                model.Catagories = new SelectList(_context.Catagories.ToList(), "Id", "Name");

                //Get All the Gallery Imagess...
                model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallary/Thumbs"))
                                    .Select(fb => Path.GetFileName(fb));


            }
            return View(model);
        }

        //[HttpPost] Admin/shop/EditProduct/id
        [HttpPost]
        public ActionResult EditProduct(ProductVM model, HttpPostedFileBase file)
        {
            //get product Id

            int id = model.Id;

            //populate the catagory select list amd Image Gallery
            using (Db _context = new Db())
            {
                model.Catagories = new SelectList(_context.Catagories.ToList(), "Id", "Name");
            }
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallary/Thumbs"))
                .Select(fn => Path.GetFileName(fn));

            //Check the Model State
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Make sure the product name is unique..
            using (Db _context = new Db())
            {
                if (_context.Products.Where(x => x.Id != id).Any(x => x.Name == model.Name))
                {
                    ModelState.AddModelError("", "The name of the Products is exist...!");
                    return View(model);
                }
            }

            //Update the Product...
            using (Db _context = new Db())
            {
                ProductDTO obj = _context.Products.Find(id);
                obj.Name = model.Name;
                obj.Slug = model.Name.Replace(" ", "-").ToLower();
                obj.Price = model.Price;
                obj.Description
                    = model.Description;
                obj.CatagoryId = model.CatagoryId;
                if (model.ImgName != null)
                {
                    obj.ImgName = model.ImgName;
                }


                CatagoryDTO objcat = _context.Catagories.FirstOrDefault(x => x.Id == model.CatagoryId);
                obj.catagoryName = objcat.Name;

                _context.SaveChanges();
            }

            //Set tempdate message
            TempData["SM"] = "You have Edit the product successfuly..";

            #region Image Upload
            //Check the File Upload
            if(file!=null && file.ContentLength>0)
            {

                //Get the Extension
                string exe = file.ContentType.ToLower();


                //verify the extension

                if (exe != "image/jpg" &&
                       exe != "image/jpeg" &&
                       exe != "image/pjepg" &&
                       exe != "image/gif" &&
                       exe != "image/x-png" &&
                       exe != "image/png")
                {
                    using (Db _context = new Db())
                    {
                       
                        ModelState.AddModelError("", "The Image was not Uploaded- Wrong Image Extension..!");
                        return View(model);
                    }
                }


                //Set Upload Directory Path
                var OriginalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                
                string PathString1 = Path.Combine(OriginalDirectory.ToString(), "Products\\" + id.ToString());
                string PathString2 = Path.Combine(OriginalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");


                //Delete Files From Directorr

                DirectoryInfo dI1 = new DirectoryInfo(PathString1);
                DirectoryInfo dI2 = new DirectoryInfo(PathString2);

                foreach (FileInfo item in dI1.GetFiles())
                {
                    item.Delete();
                }

                foreach (FileInfo item1 in dI2.GetFiles())
                {
                    item1.Delete();
                }


                //Save Image Name

                string ImName = file.FileName;

                using (Db _context = new Db())
                {
                    ProductDTO objP = _context.Products.Find(id);
                    objP.ImgName = ImName;
                    _context.SaveChanges();
                }

                //Set the Original and Thumbs Imagess
                var path1 = string.Format("{0}\\{1}", PathString1, ImName);
                var path2 = string.Format("{0}\\{1}", PathString2, ImName);

                //Save  Original
                file.SaveAs(path1);

                //Create and Save Thumb 
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);



            }




            #endregion

            return RedirectToAction("EditProduct");
        }


        // Admin/shop/DeleteProduct/id
        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {

            //Delete the product from the Db
            using (Db _context = new Db())
            {
                ProductDTO objProduct = _context.Products.Find(id);
                if(objProduct!=null)
                {
                    _context.Products.Remove(objProduct);
                    _context.SaveChanges();
                }else
                {
                    return Content("Sory...we can't Delete this product due to unexistence...");
                }
               
            }

            //Delete Profuct folder

            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            string pathString = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());

            if (Directory.Exists(pathString))
                Directory.Delete(pathString,true);

                //Retrun 
                return RedirectToAction("showProducts");
        }

        // Admin/shop/DeleteProduct/id/id

            [HttpPost]
        public ActionResult SaveGalleryImages(int id)
        {
            //Loop through The Files

            foreach (string fileName in Request.Files)
            {
                //Init the File
                HttpPostedFileBase file = Request.Files[fileName];

                //Check it is not null
                if(file!=null && file.ContentLength>0)
                {

                    //Set the Directory File.

                    var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                    string PathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallary");
                    string PathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallary\\Thumbs");


                    //Set the Image Path



                    var Path1 = string.Format("{0}\\{1}", PathString1, fileName);
                    //var Path2 = string.Format("{0}\\{1}", PathString2, fileName);
                    var path2 = string.Format($"{PathString2}\\{file.FileName}");


                    //Save Original And Thembs

                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);
                }


            }
            return View();
        }


        // Admin/shop/DeleteImage/id

        [HttpPost]
        public void DeleteImage(int id,string ImageName)
        {
            string FullPath1= Request.MapPath("~/Images/Uploads/Products/" + id + "/Gallary/" + ImageName);

            string FullPath2= Request.MapPath("~/Images/Uploads/Products/" + id + "/Gallary/Thumbs/" + ImageName);

            if (System.IO.File.Exists(FullPath1))
                System.IO.File.Delete(FullPath1);

            if (System.IO.File.Exists(FullPath2))
                System.IO.File.Delete(FullPath2);

         
        }


        // Admin/shop/Orders
        public ActionResult Orders()
        {

            //Initi the List Of ORderforAdmin
            List<OrderForAdminVM> orderForAdmin = new List<OrderForAdminVM>();
            using (Db _context = new Db())
            {
                //List of OrderVM
                List<OrderVM> ListorderVMs = _context.orderDTOs.ToArray().Select(x => new OrderVM(x)).ToList();

                foreach (var order in ListorderVMs)
                {
                    //Init the Product Dictionary 
                    Dictionary<string, int> ProductsAndQty = new Dictionary<string, int>();

                    //Declare the total

                    decimal total = 0m;

                    //Init the list of OrderDetailDTO
                    List<OrderDetailDTO> ListOrderDetails = _context.orderDetailDTOs.Where(x => x.OrderID == order.Id).ToList();


                    //Get UserNAme

                    UserDTO objUSerDOT = _context.Users.Where(x => x.Id == order.USerID).FirstOrDefault();

                    string userName = objUSerDOT.UserName;

                    //loop through the list of OrderDetailDTO

                    foreach (var OrderDetail in ListOrderDetails)
                    {
                        //Get Product

                        ProductDTO objPRoductDTO = _context.Products.Where(x => x.Id == OrderDetail.ProductID).FirstOrDefault();


                        //Get Product Price
                        decimal productPrice = objPRoductDTO.Price;

                        //Get ProdcutNAme
                        string productName = objPRoductDTO.Name;

                        //Add tO ProcutstList 
                        ProductsAndQty.Add(productName, OrderDetail.Quantity);

                        //Get the Total

                        total += OrderDetail.Quantity * productPrice;

                    }

                    //Add To OrderForAdminVm_List

                    orderForAdmin.Add(new OrderForAdminVM()
                    {
                        OrderNumber = order.Id,
                        UserName = userName,
                        Total = total,
                        ProductsAndQty = ProductsAndQty,
                        CreateAT = order.CreateAT


                    });

                }
                
            }
                return View(orderForAdmin);
        }
    }
}