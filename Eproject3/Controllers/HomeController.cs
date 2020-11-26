using Eproject3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Eproject3.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();
        public ActionResult Index(int? page,int? pagetips)
        {
            var isvalid = db.Recipes.ToList();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            int pagetipsNumber;
            if (pagetips == null)
            {
                pagetipsNumber = 1;
            }
            else {
                pagetipsNumber =(int)pagetips;
            }

            if (TempData["done"] != null)
            {
                ViewBag.done = "Done,you have submit your exams successfully";
            }
            var contests = db.Contest;
            ViewBag.Cate = db.Categories;
            var user = (Users)Session["user"];
            if (user == null || user.Pack_id == 3 || user.Roll_id != 1)
            {
                ViewBag.Tips = db.Tips.Where(p => p.isFree.Value);
                isvalid = db.Recipes.Where(p => p.R_Status == 0).ToList();

            }
            else
            {
                ViewBag.pagetipnumber = pagetipsNumber;
                ViewBag.pagesize = 6;
                ViewBag.Tips = db.Tips;
                //isvalid = db.Recipes.ToList();

            }
            return View(isvalid.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Search(string kw)
        {
            var tips = db.Tips.Where(p=>p.Title.Contains(kw) || p.Categories.Cate_Name.Contains(kw) );
            var recipes = db.Recipes.Where(p=>p.Title.Contains(kw) || p.Categories.Cate_Name.Contains(kw));
            ViewBag.tips = tips;
            ViewBag.recipes = recipes;
            ViewBag.kw = kw;
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult FAQ()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}