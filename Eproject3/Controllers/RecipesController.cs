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
    public class RecipesController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Recipes
        public async Task<ActionResult> Index()
        {
            var recipes = db.Recipes.Include(r => r.Contester);
            return View(await recipes.ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Lay feedbacks
            ViewBag.FeedBack = db.FeedBack.Where(m => m.Recipes_id == id).Include(i => i.Users).ToList();
            Recipes recipes = await db.Recipes.FindAsync(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
            return View(recipes);
        }

        // GET: Recipes/Create
        public ActionResult Create()
        {
            ViewBag.Contester_id = new SelectList(db.Contester, "id", "Name");
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Title,Content,Img,Contester_id,R_Status")] Recipes recipes, HttpPostedFileBase Url, HttpPostedFileBase Url1, HttpPostedFileBase Url2)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Url != null)
                    {
                        //Luu anh vao folder images
                        string path1 = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(Url.FileName));
                        string path2 = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(Url1.FileName));
                        string path3 = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(Url2.FileName));
                        Url.SaveAs(path1);
                        Url1.SaveAs(path2);
                        Url2.SaveAs(path3);
                    }
                }
                catch (Exception e)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }
                string img_url = Path.GetFileName(Url.FileName) +","+ Path.GetFileName(Url1.FileName)+"," + Path.GetFileName(Url2.FileName);
                recipes.Img = img_url;
                db.Recipes.Add(recipes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Contester_id = new SelectList(db.Contester, "id", "Name", recipes.Contester_id);
            return View(recipes);
        }

        // GET: Recipes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipes recipes = await db.Recipes.FindAsync(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
            ViewBag.Contester_id = new SelectList(db.Contester, "id", "Name", recipes.Contester_id);
            return View(recipes);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Title,Content,Img,Contester_id,R_Status")] Recipes recipes, HttpPostedFileBase Url)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Url != null)
                    {
                        //Luu anh vao folder images
                        string path = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(Url.FileName));
                        Url.SaveAs(path);
                    }
                }
                catch (Exception e)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }

                recipes.Img = Path.GetFileName(Url.FileName);
                db.Entry(recipes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Contester_id = new SelectList(db.Contester, "id", "Name", recipes.Contester_id);
            return View(recipes);
        }

        // GET: Recipes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipes recipes = await db.Recipes.FindAsync(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
            return View(recipes);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Recipes recipes = await db.Recipes.FindAsync(id);
            db.Recipes.Remove(recipes);
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
