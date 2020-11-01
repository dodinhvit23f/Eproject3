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
    public class FeedBacksController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: FeedBacks
        public async Task<ActionResult> Index()
        {
            var feedBack = db.FeedBack.Include(f => f.Recipes).Include(f => f.Tips).Include(f => f.Users);
            return View(await feedBack.ToListAsync());
        }

        // GET: FeedBacks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedBack feedBack = await db.FeedBack.FindAsync(id);
            if (feedBack == null)
            {
                return HttpNotFound();
            }
            return View(feedBack);
        }

        // GET: FeedBacks/Create
        public ActionResult Create()
        {
            ViewBag.Recipes_id = new SelectList(db.Recipes, "id", "Title");
            ViewBag.Tip_id = new SelectList(db.Tips, "id", "Content");
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone");
            return View();
        }

        // POST: FeedBacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int? Use_id,int Recipes_id,string Content,int?Tip_id)
        {
            if (ModelState.IsValid)
            {
                if (Use_id == null)
                {
                    return Redirect("~/Users/LoginView");
                }
                FeedBack feedBack = new FeedBack();
                feedBack.Use_id = Use_id;
                feedBack.Recipes_id = Recipes_id;
                feedBack.Content = Content;
                feedBack.Tip_id = Tip_id;
                db.FeedBack.Add(feedBack);
                await db.SaveChangesAsync();
                return Redirect("~/Recipes/Details/id=" + Recipes_id);
            }
            return Redirect("~/Recipes/Details/id=" + Recipes_id);


            //ViewBag.Recipes_id = new SelectList(db.Recipes, "id", "Title", feedBack.Recipes_id);
            //ViewBag.Tip_id = new SelectList(db.Tips, "id", "Content", feedBack.Tip_id);
            //ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", feedBack.Use_id);
            //return View(feedBack);
        }

        // GET: FeedBacks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedBack feedBack = await db.FeedBack.FindAsync(id);
            if (feedBack == null)
            {
                return HttpNotFound();
            }
            ViewBag.Recipes_id = new SelectList(db.Recipes, "id", "Title", feedBack.Recipes_id);
            ViewBag.Tip_id = new SelectList(db.Tips, "id", "Content", feedBack.Tip_id);
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", feedBack.Use_id);
            return View(feedBack);
        }

        // POST: FeedBacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Use_id,Recipes_id,Content,Tip_id")] FeedBack feedBack)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedBack).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Recipes_id = new SelectList(db.Recipes, "id", "Title", feedBack.Recipes_id);
            ViewBag.Tip_id = new SelectList(db.Tips, "id", "Content", feedBack.Tip_id);
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", feedBack.Use_id);
            return View(feedBack);
        }

        // GET: FeedBacks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedBack feedBack = await db.FeedBack.FindAsync(id);
            if (feedBack == null)
            {
                return HttpNotFound();
            }
            return View(feedBack);
        }

        // POST: FeedBacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FeedBack feedBack = await db.FeedBack.FindAsync(id);
            db.FeedBack.Remove(feedBack);
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
