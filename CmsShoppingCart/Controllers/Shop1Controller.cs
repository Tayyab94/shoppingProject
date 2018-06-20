using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsShoppingCart.Models.ViewModel.Pages;
using CmsShoppingCart.Models.Data;
using System.IO;

namespace CmsShoppingCart.Controllers
{
    public class Shop1Controller : Controller
    {
        // GET: Shop1
        public ActionResult Index()
        {
            return RedirectToAction("Index","Page");
        }


        public ActionResult CatagoryMenuPartial()
        {
            //Declare the CatagoryVm list

            List<CataogryVM> LstCatagory;

            //Init theList
            using (Db _context = new Db())
            {
                LstCatagory = _context.Catagories.ToArray().OrderBy(x => x.Sorting).Select(x => new CataogryVM(x)).ToList();
            }

            //Return PArtial With the Lsit

                return PartialView(LstCatagory);
        }



        public ActionResult Catagory(string name)
        {

            //Delare the List of ProductVM
            List<ProductVM> ListProd;
            using (Db _context = new Db())
            {
                CatagoryDTO catagoryDTO = _context.Catagories.Where(x => x.Slug == name).FirstOrDefault();
                int catID = catagoryDTO.Id;


                //Init the prodList
                // we can also use the ToList() Method insted of ToArray()
                ListProd = _context.Products.ToArray().Where(x => x.CatagoryId == catID).Select(x => new ProductVM(x)).ToList();

                //Get the CatagoryNAme
                var ProductcatName = _context.Products.Where(x => x.CatagoryId == catID).FirstOrDefault();

                ViewBag.CatagoryNAME = ProductcatName.catagoryName;


            }
                return View(ListProd);
        }


        // GET: Shop1/product-detail/name
        [ActionName("product-detail")]
        public ActionResult ProductDetails(string name)
        {

            //Declare the VM and DTO
            ProductDTO prodto;
            ProductVM provm;

            //Init the product id
            int id = 0;
            using (Db _context = new Db())
            {
                //check if the Product exist
                if(!_context.Products.Any(x=>x.Slug==name))
                {
                    // this will direct the Index method the current Controller and his index method will be direct to the PAges controller
                    return RedirectToAction("Index", "Shop1");
                }


                //Init the DOT
                prodto = _context.Products.Where(x => x.Slug.Equals(name)).FirstOrDefault();
                id = prodto.Id;


                //Init the model

                provm = new ProductVM(prodto);
            }

            //Get All the Gallery Imagess...
            provm.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallary/Thumbs"))
                                .Select(fb => Path.GetFileName(fb));
            return View("ProductDetails",provm);
        }

    }

}