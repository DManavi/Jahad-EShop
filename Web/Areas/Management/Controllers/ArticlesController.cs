using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Areas.Management.Controllers
{
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Management/Articles
        public async Task<ActionResult> Index(Guid id)
        {
            ViewBag.Category = db.ArticleCategories.FirstOrDefault(_ => _.Id == id);

            return View(await db.Articles.Where(_ => _.Category.Id == id).ToListAsync());
        }

        // GET: Management/Articles/Create
        public ActionResult Create(Guid id)
        {
            ViewBag.CategoryId = id;

            return View();
        }

        // POST: Management/Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid id, [Bind(Include = "Title,Abstract,Content,Tags")] Article article)
        {
            if (ModelState.IsValid)
            {
                var img = Request.Files["img"];

                article.Id = Guid.NewGuid();

                article.LastUpdate = DateTime.Now;

                article.HasImage = img.ContentLength > 0;

                var category = db.ArticleCategories.FirstOrDefault(_ => _.Id == id);

                article.Category = category;

                db.Entry(category).State = EntityState.Unchanged;

                db.Articles.Add(article);

                await db.SaveChangesAsync();

                if (article.HasImage)
                {
                    img.Save("Articles", Server, article.Id);
                }

                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.CategoryId = id;

            return View(article);
        }

        // GET: Management/Articles/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Article article = await db.Articles.FindAsync(id);

            ViewBag.CategoryId = article.Category.Id;

            if (article == null)
            {
                return HttpNotFound();
            }

            return View(article);
        }

        // POST: Management/Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Abstract,Content,Tags,HasImage")] Article article)
        {
            if (ModelState.IsValid)
            {
                var model = db.Articles.FirstOrDefault(_ => _.Id == article.Id);

                var img = Request.Files["img"];

                if (model.HasImage && !article.HasImage) // delete article image
                {
                    model.HasImage = false;

                    FileMethods.Delete("Articles", Server, model.Id);
                }
                else if (article.HasImage && img.ContentLength > 0) // new/updated image uploaded
                {
                    img.Save("Articles", Server, model.Id);

                    model.HasImage = true;
                }

                model.Title = article.Title;

                model.Abstract = article.Abstract;

                model.Content = article.Content;

                model.Tags = article.Tags;

                model.LastUpdate = DateTime.Now;

                db.Entry(model).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { id = model.Category.Id });
            }

            ViewBag.CategoryId = db.Articles.FirstOrDefault(_ => _.Id == article.Id).Category.Id;

            return View(article);
        }

        // GET: Management/Articles/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Article article = await db.Articles.FindAsync(id);

            ViewBag.CategoryId = article.Category.Id;

            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Management/Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Article article = await db.Articles.FindAsync(id);

            var categoryId = article.Category.Id;

            db.Articles.Remove(article);

            await db.SaveChangesAsync();

            if (article.HasImage)
            {
                FileMethods.Delete("Articles", Server, id);
            }

            return RedirectToAction("Index", new { id = categoryId });
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
