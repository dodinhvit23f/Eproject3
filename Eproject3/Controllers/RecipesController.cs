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
        /// <summary>
        /// phan cu hai
        /// </summary>
        /// <returns></returns>
        // GET: Recipes/Create
        public ActionResult Create()
        {
           // ViewBag.Contester_id = new SelectList(db.Contester, "id", "Name");
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Title,Content,Img,Contester_id,R_Status")] Recipes recipes, HttpPostedFileBase[] Url, string[] txtText, string[] txtIgredent, int Contester_id)
        {
            string Cont = "";
            string url_img = "";
            string ingre = "";
            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };
            if (ModelState.IsValid)
            {
              
                    try
                    {
                        foreach (HttpPostedFileBase img in Url)
                        {
                        if (img != null)
                        {
                            string path = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(img.FileName));
                            img.SaveAs(path);
                            string ex = Path.GetExtension(img.FileName);
                            if (!check(ex, formats))
                            {
                                ViewBag.FileStatus = "Sai";
                                return View(recipes);
                            }
                            url_img += Path.GetFileName(img.FileName) + ",";
                        }
                        else
                        {
                            ViewBag.FileStatus = "Quen Them Anh ha Ban";
                            return View(recipes);
                        }
                    }
                            
                        


                    }
                    catch (Exception e)
                    {
                        ViewBag.FileStatus = "Error while file uploading.";
                    }

                    recipes.Img = url_img.Substring(0, url_img.Length - 1);

                    foreach (var text in txtText)
                    {
                        if (text != "")
                        {

                            Cont += text + ",";
                        }
                    }
                    foreach (var ingredent in txtIgredent)
                    {
                        if (ingredent != "")
                        {

                            ingre += ingredent + ",";
                        }
                    }
                    Cont = Cont.Substring(0, Cont.Length - 1);
                    recipes.Content = Cont;
                    recipes.ingredent = ingre.Substring(0, ingre.Length - 1);
                    recipes.Contester_id = Contester_id;
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
        public async Task<ActionResult> Edit([Bind(Include = "id,Title,Content,Img,Contester_id,R_Status")] Recipes recipes, HttpPostedFileBase[] Url, string[] txtText,string[] txtIgredent)
        { 
            string Cont = "";
            string url_img = "";
            string ingre = "";
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (HttpPostedFileBase img in Url)
                    {
                        if (img != null)
                        {
                            string path = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(img.FileName));
                            img.SaveAs(path);
                            url_img += Path.GetFileName(img.FileName) + ",";
                        }
                    }


                }
                catch (Exception e)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }

                recipes.Img = url_img.Substring(0, url_img.Length - 1);

                foreach (var text in txtText)
                {
                    if (text != "")
                    {

                        Cont += text + ",";
                    }
                }
                foreach (var ingredent in txtIgredent)
                {
                    if (ingredent != "")
                    {

                        ingre += ingredent + ",";
                    }
                }
                Cont = Cont.Substring(0, Cont.Length - 1);
                recipes.Content = Cont;
                recipes.ingredent = ingre.Substring(0, ingre.Length - 1);
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
        public bool check(string extension, string[] format) {
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
