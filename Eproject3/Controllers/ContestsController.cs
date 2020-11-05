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
    public class ContestsController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Contests
        public async Task<ActionResult> Index()
        {
            return View(await db.Contest.ToListAsync());
        }

        // GET: Contests/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            ViewBag.exams = db.Exams.Where(p=>p.Contest_id==id);
            if (TempData["over"] != null)
            {
                ViewBag.over = TempData["over"];
            }
            if (TempData["early"] != null)
            {
                ViewBag.early = TempData["early"];
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = await db.Contest.FindAsync(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            return View(contest);
        }
        public ActionResult Join(int id)
        {
            var isvalid = db.Contest.Find(id);
            if (isvalid.exp_time<DateTime.Now)
            {
                TempData["over"] = "This contest is over";
                return RedirectToAction("Details/" + id);
            }else if (isvalid.C_Time>DateTime.Now)
            {
                TempData["early"] = "This contest has not begun yet";
                return RedirectToAction("Details/" + id);
            }
            TempData["ctId"] = id;           
            return RedirectToAction("Create","Contesters");
        }

        // GET: Contests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,C_Time,exp_time,C_Description")] Contest contest)
        {
            if (ModelState.IsValid)
            {
               
                if (DateTime.Compare(contest.C_Time.Value, contest.exp_time.Value) > 0)
                {
                    ViewBag.DateError = "Exp date must be later than start date";
                    return View(contest);
                }
                db.Contest.Add(contest);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contest);
        }

        // GET: Contests/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = await db.Contest.FindAsync(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            return View(contest);
        }

        // POST: Contests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,C_Time,exp_time,C_Description")] Contest contest)
        {
            if (ModelState.IsValid)
            {

                if (DateTime.Compare(contest.C_Time.Value, contest.exp_time.Value) < 0) {
                    ViewBag.DateError = "Exp date must be later than start date";
                    return View(contest);
                }
                db.Entry(contest).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contest);
        }

        // GET: Contests/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = await db.Contest.FindAsync(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            return View(contest);
        }

        // POST: Contests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Contest contest = await db.Contest.FindAsync(id);
            db.Contest.Remove(contest);
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
