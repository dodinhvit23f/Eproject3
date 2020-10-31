using Eproject3.Models;
using Eproject3.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Eproject3.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseEntities db = new DatabaseEntities();
        Repository r = new Repository();
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Session["isAdmin"] == null)
            {
                return RedirectToAction("Login");
            }
            Users user = (Users)Session["uses"];
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name", user.Pack_id);
            ViewBag.Roll_id = new SelectList(db.Roles, "id", "name", user.Roll_id);
            return View(user);
        }
        public ActionResult Login()
        {
            if (Session["isAdmin"] != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(string usn,string pwd)
        {
            string hashed = r.HashPwd(pwd);
            var isValid = db.Users.Where(p=>p.UPhone==usn && p.UPass== hashed).FirstOrDefault();
            if (isValid == null)
            {
                ViewBag.err = "Wrong credential";
                return View();
            }else if (isValid.Roll_id != 1 )
            {
                ViewBag.err = "You are not permited here";
                return View();
            }
            Session["uses"] = isValid;
            Session["isAdmin"] = true;
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([Bind(Include = "id,UPhone,UPass,UAdress,Img,Roll_id,Pack_id,Exp_Date,AccNum")] Users users)
        {
            if (ModelState.IsValid)
            {
                users.UPass = r.HashPwd(users.UPass);
                db.Entry(users).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name", users.Pack_id);
            ViewBag.Roll_id = new SelectList(db.Roles, "id", "name", users.Roll_id);
            return View(users);
        }
    }
}