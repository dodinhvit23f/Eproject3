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
    public class TipsController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Tips
        public async Task<ActionResult> Index()
        {
            var isValid = (Users)Session["user"];
            if (isValid != null)
            {
                var tips = db.Tips.Where(p=>p.Use_id==isValid.id).Include(t => t.Users);
                return View(await tips.ToListAsync());
            }
            else
            {
                return RedirectToAction("LoginView", "Users");
            }

        }

        // GET: Tips/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tips tips = await db.Tips.FindAsync(id);
            if (tips == null)
            {
                return HttpNotFound();
            }
            return View(tips);
        }

        // GET: Tips/Create
        public ActionResult Create()
        {
            Users u = (Users)Session["User"];
            if (u != null)
            {
                ViewBag.Cate_id = new SelectList(db.Categories.ToList(), "id", "Cate_name");
                return View();
            }
            return Redirect("~/Users/LoginView");
        }

        // POST: Tips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Use_id,Content,Img,Title,Levels,Cate_id,isFree")] Tips tips, HttpPostedFileBase[] Url, string[] txtText,int isFree, string rate)
        {
            int flag = 0;
            string Cont = "";
            string url_img = "";
            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (HttpPostedFileBase img in Url)
                    {
                        if (img != null)
                        {
                           
                            string ex = Path.GetExtension(img.FileName);
                            if (!check(ex, formats))
                            {
                                flag = 1;
                                ViewBag.FileStatus = ex + " is not an image";
                                return View(tips);
                            }
                            url_img += Path.GetFileName(img.FileName) + ",";
                        }
                        else
                        {
                            flag = 1;
                            ViewBag.FileStatus = "Content must have image !!!!";
                            return View(tips);
                        }
                        if (flag != 1)
                        {
                            string path = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(img.FileName));
                            img.SaveAs(path);
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }

                tips.Img = url_img.Substring(0, url_img.Length - 1);

                foreach (var text in txtText)
                {
                    if (text != "")
                    {

                        Cont += text + ",";
                    }
                }
                Cont = Cont.Substring(0, Cont.Length - 1);
                tips.Content = Cont;
                var isvalid = (Users)Session["user"];
                tips.Use_id = isvalid.id;
                if (isFree == 0)
                {
                    tips.isFree = true;
                }
                else {
                    tips.isFree = false;
                }
                tips.Levels = rate;
                db.Tips.Add(tips);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", tips.Use_id);
            return View(tips);
        }

        // GET: Tips/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tips tips = await db.Tips.FindAsync(id);
            if (tips == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cate_id = new SelectList(db.Categories.ToList(), "id", "Cate_name");
            return View(tips);
        }

        // POST: Tips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "iid,Use_id,Content,Img,Title,Levels,Cate_id,isFree")] Tips tips, HttpPostedFileBase[] Url, string[] txtText, int isFree, string rate)
        {
            int flag = 0;
            string Cont = "";
            string url_img = "";
            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (HttpPostedFileBase img in Url)
                    {
                        if (img != null)
                        {

                            string ex = Path.GetExtension(img.FileName);
                            if (!check(ex, formats))
                            {
                                flag = 1;
                                ViewBag.FileStatus = ex + " is not an image";
                                return View(tips);
                            }
                            url_img += Path.GetFileName(img.FileName) + ",";
                        }
                        else
                        {
                            flag = 1;
                            ViewBag.FileStatus = "Content must have image !!!!";
                            return View(tips);
                        }
                        if (flag != 1)
                        {
                            string path = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(img.FileName));
                            img.SaveAs(path);
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }

                tips.Img = url_img.Substring(0, url_img.Length - 1);

                foreach (var text in txtText)
                {
                    if (text != "")
                    {

                        Cont += text + ",";
                    }
                }
                Cont = Cont.Substring(0, Cont.Length - 1);
                tips.Content = Cont;
                var isvalid = (Users)Session["user"];
                tips.Use_id = isvalid.id;
                if (isFree == 0)
                {
                    tips.isFree = true;
                }
                else
                {
                    tips.isFree = false;
                }
                tips.Levels = rate;
                db.Entry(tips).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", tips.Use_id);
            return View(tips);
        }

        // GET: Tips/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tips tips = await db.Tips.FindAsync(id);
            if (tips == null)
            {
                return HttpNotFound();
            }
            return View(tips);
        }

        // POST: Tips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tips tips = await db.Tips.FindAsync(id);
            db.Tips.Remove(tips);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public bool check(string extension, string[] format)
        {
            foreach (string exten in format)
            {
                if (extension.Contains(exten))
                {
                    return true;
                }
            }
            return false;
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
