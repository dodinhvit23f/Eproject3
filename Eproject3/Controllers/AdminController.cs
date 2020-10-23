using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eproject3.Models;
using System.IO;

namespace Eproject3.Controllers
{
    public class AdminController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();
        // GET: Admin
        public async Task<ActionResult> Index()
        {
            if (Session["isAdmin"] == null)
            {
                return RedirectToAction("LoginView","Users");
            }
            var isValid = (Users)Session["user"];
            if (isValid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = await db.Users.FindAsync(isValid.id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name", users.Pack_id);
            ViewBag.Roll_id = new SelectList(db.Roles, "id", "name", users.Roll_id);
            return View(users);
        }
    }
}