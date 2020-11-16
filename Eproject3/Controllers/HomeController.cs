using Eproject3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Eproject3.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();
        public async Task<ActionResult> Index()
        {
            if (TempData["done"] != null)
            {
                ViewBag.done = "Done,you have submit your exams successfully";
            }
            var contests = db.Contest;
            DateTime next7days = DateTime.Today.AddDays(7);
            ViewBag.contests = db.Contest.Where(p=>p.exp_time>DateTime.Now||p.C_Time== next7days);
            var user = (Users)Session["user"];
            if (user == null || user.Pack_id==3 || user.Roll_id !=1)
            {
                ViewBag.Tips = db.Tips.Where(p=>p.isFree.Value);
                var isvalid = db.Recipes.Where(p => p.R_Status == 0);
                return View(await isvalid.ToListAsync());
            }
            else
            {
                ViewBag.Tips = db.Tips;
                var isvalid = db.Recipes;
                return View(await isvalid.ToListAsync());
            }
        }
        [HttpGet]
        public ActionResult Search(string kw)
        {
            var tips = db.Tips.Where(p=>p.Title.Contains(kw));
            var recipes = db.Recipes.Where(p=>p.Title.Contains(kw));
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