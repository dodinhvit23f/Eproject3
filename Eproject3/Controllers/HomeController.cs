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
            var user = (Users)Session["user"];
            if (user == null || user.Pack_id==3)
            {
                ViewBag.Tips = db.Tips.Where(p=>p.isFree.Value);
                var isvalid = db.Recipes.Where(p => p.R_Status == 0);
                return View(await isvalid.ToListAsync());
            }
            else
            {
                ViewBag.Tips = db.Tips.Where(p => !p.isFree.Value);
                var isvalid = db.Recipes.Where(p => p.R_Status == 1);
                return View(await isvalid.ToListAsync());
            }
        }
        [Route("Admin")]
        public ActionResult AdminIndex()
        {
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
    }
}