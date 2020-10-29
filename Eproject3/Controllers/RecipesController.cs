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
            Recipes recipes = await db.Recipes.FindAsync(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
            return View(recipes);
        }
        /// <summary>
        /// phan cu hai
        /// </summary>
        /// <returns></returns>
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
        public async Task<ActionResult> Create([Bind(Include = "id,Title,Content,Img,Contester_id,R_Status")] Recipes recipes)
        {
            if (ModelState.IsValid)
            {
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
        public async Task<ActionResult> Edit([Bind(Include = "id,Title,Content,Img,Contester_id,R_Status")] Recipes recipes)
        {
            if (ModelState.IsValid)
            {
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
