using Eproject3.Models;
using Eproject3.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Eproject3.Areas.Admin.Models
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
                return RedirectToAction("LoginView", "Users");
            }
            Users user = (Users)Session["uses"];
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name", user.Pack_id);
            ViewBag.Roll_id = new SelectList(db.Roles, "id", "name", user.Roll_id);
            return View(user);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Index([Bind(Include = "id,UPhone,UPass,UAdress,Img,Roll_id,Pack_id,Exp_Date,AccNum")] Users users)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        users.UPass = r.HashPwd(users.UPass);
        //        db.Entry(users).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Pack_id = new SelectList(db.Packs, "id", "name", users.Pack_id);
        //    ViewBag.Roll_id = new SelectList(db.Roles, "id", "name", users.Roll_id);
        //    return View(users);
        //}
    }
}