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
            return View(await users.Where(p=>p.id==user.id).ToListAsync());
        }
        public async Task<ActionResult> Recipes(int userID)
        {
            return View(await db.Recipes.Where(p=>p.Contester_id==userID).ToListAsync());
        }
        public async Task<ActionResult> Tips(int userID)
        {
            return View(await db.Tips.Where(p => p.Use_id == userID).ToListAsync());
        }
        public ActionResult ChangePwd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePwd(string oldp,string newp)
        {
            var user = (Users)Session["user"];
            string hashed = r.HashPwd(oldp);
            var isvalid = db.Users.Where(p=>p.UPhone==user.UPhone && p.UPass== hashed).FirstOrDefault();
            ViewBag.old = oldp;
            ViewBag.newp = newp;
            if (user != null && isvalid != null)
            {
                if (newp.Length < 8 || newp.Length >50)
                {
                    ViewBag.err = "Password must be a 8-50 characters string ";
                    return View();
                }
                isvalid.UPass = r.HashPwd(newp);
                db.SaveChanges();
                return RedirectToAction("index","Home");
            }          
            ViewBag.err = "Wrong credential";
            return View();
            
        }
        public ActionResult ForgetPwd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPwd(string phone)
        {
            ViewBag.phone = phone;
            var isvalid = db.Users.Where(p => p.UPhone == phone).FirstOrDefault();
            if (isvalid != null)
            {
                Guid newPass = new Guid();
                isvalid.UPass = r.HashPwd(newPass.ToString());
                //Send sms to User to send new password
                db.SaveChanges();
                TempData["done"] = "Check your inbox in your phone to receive new password ";
                return RedirectToAction("LoginView");
            }
            else
            {
                ViewBag.err = "No phone number found";
                return View();
            }
        }
            public async Task<ActionResult> LoginView()
        {
            if (TempData["done"] != null)
            {
                ViewBag.done = TempData["done"];
            }
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
            Session["isAdmin"] = null;
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
                    if (isValid.Roll_id.Value == 1)
                    {
                        Session["isAdmin"] = true;
                    }
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
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name");
            ViewBag.Roll_id = new SelectList(db.Roles, "id", "name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,UPhone,UPass,UAdress,Img,Roll_id,Pack_id,Exp_Date,AccNum")] Users users, HttpPostedFileBase Url)
        {
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name", users.Pack_id);
            string url_img = "";
            if (db.Users.Where(p => p.UPhone == users.UPhone).FirstOrDefault() != null)
            {
                ViewBag.ExErr = "This phone number has been registered before";
                return View(users);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(Url.FileName));
                    Url.SaveAs(path);
                    url_img += Path.GetFileName(Url.FileName) + ","; 
                }
                catch (Exception e)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }
                users.Img = url_img.Substring(0, url_img.Length - 1);
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
            var isValid = (Users)Session["user"];
            if (isValid ==null || isValid.id != id)
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
            var isValid = (Users)Session["user"];
            if (ModelState.IsValid)
            {
                users.Exp_Date = isValid.Exp_Date;
                users.Pack_id = isValid.Pack_id;
                users.Roll_id = isValid.Roll_id;
                //users.UPass = r.HashPwd(users.UPass);
                db.Entry(users).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name", users.Pack_id);
            ViewBag.Roll_id = new SelectList(db.Roles, "id", "name", users.Roll_id);
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["isAdmin"] == null)
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
