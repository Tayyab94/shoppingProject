using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModel.Pages;

namespace CmsShoppingCart.Controllers
{
    public class PagesController : Controller
    {
        // GET: Index{Pages}
        public ActionResult Index(string page="")
        {

            if (page =="")
                page = "home";

            //Declare the model and DTO
            PagesDTO objPageDto;
            PagesVM objPageVm;
            //Check if the Page is Exist
            using (Db _context = new Db())
            {
                if(!_context.Pages.Any())
                {
                    return RedirectToAction("Index", new { page = "" });
                }
            }

            //Gae PageDTo
            using (Db _context = new Db())
            {
                objPageDto = _context.Pages.Where(x => x.Slug == page).FirstOrDefault();
            }

            ////Set the Page Title
            if (objPageDto.Title != null)
            {

                ViewBag.pagetitle = objPageDto.Title;
            }
            else
            {
                ViewBag.pagetitle = "home";
            }



             // check the sidebar

            if (objPageDto.HasSidebar == true)
            {
                ViewBag.sidebar = "yes";
            }
            else
            {
                ViewBag.sidebar = "no";
            }


            //Init the model

            objPageVm = new PagesVM(objPageDto);
                return View(objPageVm);
        }
    }
}