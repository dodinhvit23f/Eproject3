﻿using System;
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
    public class ContestersController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Contesters
        public async Task<ActionResult> Index()
        {
            var contester = db.Contester.Include(c => c.Users);
            return View(await contester.ToListAsync());
        }

        // GET: Contesters/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contester contester = await db.Contester.FindAsync(id);
            if (contester == null)
            {
                return HttpNotFound();
            }
            return View(contester);
        }

        // GET: Contesters/Create
        public ActionResult Create()
        {
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone");
            return View();
        }

        // POST: Contesters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Use_id,Name,Phone")] Contester contester)
        {
            if (ModelState.IsValid)
            {
                db.Contester.Add(contester);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", contester.Use_id);
            return View(contester);
        }

        // GET: Contesters/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contester contester = await db.Contester.FindAsync(id);
            if (contester == null)
            {
                return HttpNotFound();
            }
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", contester.Use_id);
            return View(contester);
        }

        // POST: Contesters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Use_id,Name,Phone")] Contester contester)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contester).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", contester.Use_id);
            return View(contester);
        }

        // GET: Contesters/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contester contester = await db.Contester.FindAsync(id);
            if (contester == null)
            {
                return HttpNotFound();
            }
            return View(contester);
        }

        // POST: Contesters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Contester contester = await db.Contester.FindAsync(id);
            db.Contester.Remove(contester);
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