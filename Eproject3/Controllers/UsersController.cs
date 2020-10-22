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

namespace Eproject3.Controllers
{
    public class UsersController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();
        Repo.Repository r = new Repo.Repository();
        // GET: Users
        public async Task<ActionResult> Index()
        {
            var user = (Users)Session["user"];
            if (user == null || user.Roll_id != 1 )
            {
                TempData["AuErr"] = true;
                return RedirectToAction("LoginView");
            }
            var users = db.Users.Include(u => u.Packs).Include(u => u.Roles);
            return View(await users.ToListAsync());
        }
       

        public async Task<ActionResult> LoginView()
        {
            if (TempData["AuErr"] !=null)
            {
                ViewBag.AuErr = true;
            }
            else
            {
                ViewBag.AuErr = false;
            }
            if (TempData["lErr"] != null)
            {
                ViewBag.lErr = true;
            }
            else
            {
                ViewBag.lErr = false;
            }
            if (TempData["EpErr"] != null)
            {
                ViewBag.EpErr = true;
            }
            else
            {
                ViewBag.EpErr = false;
            }
            return View();
        }
        public async Task<ActionResult> LogOut()
        {
            Session["user"] = null;
            return RedirectToAction("index", "Home");
        }
        public async Task<ActionResult> Login(string phones, string pwd)
        {
            string hashed = r.HashPwd(pwd);
            var isValid = db.Users.Where(p => p.UPhone == phones && p.UPass.Equals(hashed)).FirstOrDefault();
            if (isValid != null)
            {
                if (isValid.Roll_id.Value == 1 || DateTime.Compare(isValid.Exp_Date.Value, DateTime.Now) >= 0 || isValid.Pack_id.Value == 3)
                {
                    Session["user"] = isValid;
                    return RedirectToAction("index", "Home");
                }
                else if (DateTime.Compare(isValid.Exp_Date.Value, DateTime.Now) < 0)
                {
                    TempData["EpErr"] = true;
                    return RedirectToAction("LoginView");
                }
                return RedirectToAction("LoginView");
            }
            else
            {
                TempData["lErr"] = true;
                return RedirectToAction("LoginView");
            }
        }




        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = await db.Users.FindAsync(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            var user = (Users)Session["user"];
            if (user == null || user.Roll_id != 1)
            {
                TempData["AuErr"] = true;
                return RedirectToAction("LoginView");
            }
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name");
            ViewBag.Roll_id = new SelectList(db.Roles, "id", "name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,UPhone,UPass,UAdress,Img,Roll_id,Pack_id,Exp_Date,AccNum")] Users users)
        {
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name", users.Pack_id);
            if (db.Users.Where(p => p.UPhone == users.UPhone).FirstOrDefault() != null)
            {
                ViewBag.ExErr = "This phone number has been registered before";
                return View(users);
            }
            if (ModelState.IsValid)
            {
                if (users.Pack_id == 1)
                {
                    users.Exp_Date = DateTime.Now.AddMonths(1);
                }
                else if (users.Pack_id == 2)
                {
                    users.Exp_Date = DateTime.Now.AddYears(1);
                }
                else
                {
                    users.Exp_Date = DateTime.Now;

                }
                string hashed = r.HashPwd(users.UPass);
                users.UPass = hashed;
                users.Roll_id = 2;
                db.Users.Add(users);
                await db.SaveChangesAsync();
                return RedirectToAction("LoginView");
            }

            //ViewBag.Roll_id = new SelectList(db.Roles, "id", "name", users.Roll_id);
            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var user = (Users)Session["user"];
            if (user == null || user.Roll_id != 1)
            {
                TempData["AuErr"] = true;
                return RedirectToAction("LoginView");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = await db.Users.FindAsync(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name", users.Pack_id);
            ViewBag.Roll_id = new SelectList(db.Roles, "id", "name", users.Roll_id);
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,UPhone,UPass,UAdress,Img,Roll_id,Pack_id,Exp_Date,AccNum")] Users users)
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

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            var user = (Users)Session["user"];
            if (user == null || user.Roll_id != 1 )
            {
                TempData["AuErr"] = true;
                return RedirectToAction("LoginView");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = await db.Users.FindAsync(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Users users = await db.Users.FindAsync(id);
            db.Users.Remove(users);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
