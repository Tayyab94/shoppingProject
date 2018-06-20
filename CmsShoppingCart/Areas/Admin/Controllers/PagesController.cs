using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModel.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
  //  [Authorize(Roles ="Admin")]
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {

            //Declare the list of PageVM 
            List<PagesVM> pageList;

            using (Db db = new Db())
            {

                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PagesVM(x)).ToList();
            }
            Db _context = new Db();

            //Return the PAgelist to View
                return View(pageList);
        }


        [HttpGet]
        // GET: Admin/Dashboard/AddPage
        public ActionResult AddPage()
        {
            return View();
        }
        [HttpPost]
        // Post: Admin/Dashboard/AddPage
        public ActionResult AddPage(PagesVM model)
        {
            //Check ModlState
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db _Context = new Db())
            {
                //declare Slug
                string slug;

                //Init PageDTO
                PagesDTO objPageDTO = new PagesDTO();

                //DTO Tile 
                objPageDTO.Title = model.Title;
                
                //Check for and Set if slug is Need be
                if(string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //Make suer that Tilte and slug are Unique
                if(_Context.Pages.Any(x=>x.Title==model.Title)|| _Context.Pages.Any(x=>x.Slug==slug))
                {
                    ModelState.AddModelError("", "The Title or the Slug already exist..");
                    return View(model);
                }

                //DTO the Rest
                objPageDTO.Slug = slug;
                objPageDTO.Body = model.Body;
                objPageDTO.HasSidebar = model.HasSidebar;
                objPageDTO.Sorting = 100;

                //DTO Save

                _Context.Pages.Add(objPageDTO);
                _Context.SaveChanges();
            }

            //Set TempData Message

            TempData["SM"] = "YOu have Added New Page Successfuly..";

            //Redirect
            return RedirectToAction("AddPage");
                
        }

        [HttpGet]
        // Post: Admin/Dashboard/EditPage/id
        public ActionResult EditPage(int id)
        {
            //Declare PageVM
            PagesVM model;
            using (Db _Context = new Db())
            {
                //Get The PAge
                PagesDTO pagesDTO = _Context.Pages.Find(id);

                //Confirm page Exist
                if(pagesDTO==null)
                {
                    return Content("Sorry.. This Page does Not Exist..");
                }
                model = new PagesVM(pagesDTO);
            }

                return View(model);
        }

        [HttpPost]
        // Post: Admin/Dashboard/EditPage/id
        public ActionResult EditPage(PagesVM model)
        {
            //Check model Sate
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db _Context = new Db())
            {
                //GEt page Id
                int id = model.ID;
                //DeCleare SLug
                string slug="";
                //Get the Page
                PagesDTO obj = _Context.Pages.Find(id);

                //DTO Title 
                obj.Title = model.Title;

                //Check slug and set if need be
                if(model.Slug.ToLower()!="home")
                {
                    if(string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }
                if(_Context.Pages.Where(x=>x.ID!=id).Any(x=>x.Slug==slug)||_Context.Pages.Where(x=>x.ID!=id).Any(x=>x.Title==model.Title))
                {
                    ModelState.AddModelError("", "This Title or slug have Already Exist..");
                    return View(model);
                }

                //DTo the Rest
                //DTO the Rest
                obj.Slug = slug;
                obj.Body = model.Body;
                obj.HasSidebar = model.HasSidebar;
         

                //DTO Save
                
                _Context.SaveChanges();
            }

            TempData["SM"] = "You have Successfuly Edit Page..";
            return RedirectToAction("EditPage");
        }

        // Post: Admin/Dashboard/PageDetails/id
        public ActionResult PageDetails(int id)
        {
            //Declare Model
            PagesVM model;
            using (Db _context = new Db())
            {
                PagesDTO obj = _context.Pages.Find(id);
                if(obj==null)
                {
                    return Content("","The Page does not Exist..");
                }

                model = new PagesVM(obj);
            }
                return View(model);
        }


     
        // Post: Admin/Dashboard/DeletePage/id
        public ActionResult DeletePage(int id)
        {
            using (Db _context = new Db())
            {
                //GEt the PAge
                PagesDTO obj = _context.Pages.Find(id);

                //Remove the Page
                _context.Pages.Remove(obj);
                //Save_Changes

                _context.SaveChanges();

            }
                return RedirectToAction("Index");
        }

        [HttpPost]
        // Post: Admin/Dashboard/ReorderPages
        public void ReorderPages(int [] id)
        {
            using (Db _context = new Db())
            {
                //Init veriable Count with 1
                int cout = 1;

                PagesDTO obj;


                //Set Sorting for each page

                foreach (var pageId in id)
                {
                    obj = _context.Pages.Find(pageId);
                    obj.Sorting = cout;
                    _context.SaveChanges();
                    cout++;
                }
            }
        }

        // GET: Admin/Dashboard/AddPage
        public ActionResult EditSideBar()
        {

            //Declare Model
            SidebarVM model;

            using (Db _context = new Db())
            {
                Sidebar obj = _context.Sidebar.Find(1);
                model = new SidebarVM(obj);
            }

            return View(model);
        }
        [HttpPost]
        // post: Admin/Dashboard/AddPage
        public ActionResult EditSideBar(SidebarVM obj)
        {

            //Declare Model

            using (Db _context = new Db())
            {
                Sidebar obj1 = _context.Sidebar.Find(1);

                obj1.Body = obj.Body;
                _context.SaveChanges();
            }

            //Set Temp Message
            TempData["SM"] = "You have Edited the Sidebar Successfuly....";

            return RedirectToAction("EditSideBar");
        }

        
        public ActionResult PageMenuPartial()
        {
            //Declare the list of pageVM
            List<PagesVM> listPage;
            //Get all the Pages expect Home
            using (Db _context = new Db())
            {
                listPage = _context.Pages.ToArray().OrderBy(x => x.Sorting).Where(x => x.Slug != "home").Select(s => new PagesVM(s)).ToList();
            }

            return PartialView(listPage);
        }
    }
}