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

namespace Eproject3.Areas.Admin.Controllers
{
    public class TipsController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Tips
        public async Task<ActionResult> Index()
        {
            var tips = db.Tips.Include(t => t.Users);
            return View(await tips.ToListAsync());
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
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone");
            return View();
        }

        // POST: Tips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Use_id,Content,Img")] Tips tips)
        {
            if (ModelState.IsValid)
            {
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
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", tips.Use_id);
            return View(tips);
        }

        // POST: Tips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Use_id,Content,Img")] Tips tips)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tips).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", tips.Use_id);
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
