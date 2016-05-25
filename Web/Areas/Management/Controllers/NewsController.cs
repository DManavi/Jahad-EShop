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
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Management/News
        public async Task<ActionResult> Index(Guid id)
        {
            ViewBag.Category = db.NewsCategories.FirstOrDefault(_ => _.Id == id);

            return View(await db.News.Where(_ => _.Category.Id == id).ToListAsync());
        }

        // GET: Management/News/Create
        public ActionResult Create(Guid id)
        {
            ViewBag.CategoryId = id;

            return View();
        }

        // POST: Management/News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid id, [Bind(Include = "Title,Abstract,Content")] News news)
        {
            if (ModelState.IsValid)
            {
                var img = Request.Files["img"];

                news.HasImage = img.ContentLength > 0;

                var category = db.NewsCategories.FirstOrDefault(_ => _.Id == id);

                news.Category = category;

                db.Entry(category).State = EntityState.Unchanged;

                news.Id = Guid.NewGuid();

                news.LastUpdate = DateTime.Now;

                db.News.Add(news);

                await db.SaveChangesAsync();

                if (news.HasImage)
                {
                    img.Save("News", Server, news.Id);
                }

                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.CategoryId = id;

            return View(news);
        }

        // GET: Management/News/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.News.FindAsync(id);

            ViewBag.CategoryId = news.Category.Id;

            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: Management/News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Abstract,Content,HasImage")] News news)
        {
            if (ModelState.IsValid)
            {
                var model = db.News.FirstOrDefault(_ => _.Id == news.Id);

                var img = Request.Files["img"];

                if (model.HasImage && !news.HasImage) // delete article image
                {
                    model.HasImage = false;

                    FileMethods.Delete("News", Server, model.Id);
                }
                else if (news.HasImage && img.ContentLength > 0) // new/updated image uploaded
                {
                    img.Save("News", Server, model.Id);

                    model.HasImage = true;
                }

                model.Title = news.Title;

                model.Abstract = news.Abstract;

                model.Content = news.Content;

                model.LastUpdate = DateTime.Now;

                db.Entry(model).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { id = model.Category.Id });
            }

            ViewBag.CategoryId = db.Articles.FirstOrDefault(_ => _.Id == news.Id).Category.Id;

            return View(news);
        }

        // GET: Management/News/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            News news = await db.News.FindAsync(id);

            ViewBag.CategoryId = news.Category.Id;

            if (news == null)
            {
                return HttpNotFound();
            }

            return View(news);
        }

        // POST: Management/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            News news = await db.News.FindAsync(id);

            var categoryId = news.Category.Id;

            db.News.Remove(news);

            await db.SaveChangesAsync();

            if (news.HasImage)
            {
                FileMethods.Delete("News", Server, id);
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
