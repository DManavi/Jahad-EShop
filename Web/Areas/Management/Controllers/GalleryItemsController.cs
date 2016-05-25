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
    public class GalleryItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Management/GalleryItems
        public async Task<ActionResult> Index(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Category = db.Galleries.FirstOrDefault(_ => _.Id == id);

            return View(await db.GalleryItems.Where(_ => _.Gallery.Id == id).ToListAsync());
        }

        // GET: Management/GalleryItems/Create
        public ActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.CategoryId = id;

            return View();
        }

        // POST: Management/GalleryItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid? id, [Bind(Include = "Title,Content")] GalleryItem galleryItem)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var img = Request.Files["img"];

            if (img.ContentLength < 1) { ModelState.AddModelError("img", "فایل تصویر انتخاب نشده است"); }

            if (ModelState.IsValid)
            {
                galleryItem.Id = Guid.NewGuid();

                galleryItem.LastUpdate = DateTime.Now;

                var category = db.Galleries.FirstOrDefault(_ => _.Id == id);

                galleryItem.Gallery = category;

                db.Entry(category).State = EntityState.Unchanged;

                db.GalleryItems.Add(galleryItem);

                await db.SaveChangesAsync();

                img.Save("Gallery", Server, galleryItem.Id);

                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.CategoryId = id;

            return View(galleryItem);
        }

        // GET: Management/GalleryItems/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GalleryItem galleryItem = await db.GalleryItems.FindAsync(id);

            ViewBag.Categoryid = galleryItem.Gallery.Id;

            if (galleryItem == null)
            {
                return HttpNotFound();
            }

            return View(galleryItem);
        }

        // POST: Management/GalleryItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Content")] GalleryItem galleryItem)
        {
            if (ModelState.IsValid)
            {
                var model = db.GalleryItems.FirstOrDefault(_ => _.Id == galleryItem.Id);

                var img = Request.Files["img"];

                if (img.ContentLength > 0) // new/updated image uploaded
                {
                    img.Save("Gallery", Server, model.Id);
                }

                model.Title = galleryItem.Title;

                model.Content = galleryItem.Content;

                model.LastUpdate = DateTime.Now;

                db.Entry(model).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { id = model.Gallery.Id });
            }

            ViewBag.CategoryId = db.GalleryItems.FirstOrDefault(_ => _.Id == galleryItem.Id).Gallery.Id;

            return View(galleryItem);
        }

        // GET: Management/GalleryItems/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GalleryItem galleryItem = await db.GalleryItems.FindAsync(id);

            ViewBag.CategoryId = galleryItem.Gallery.Id;

            if (galleryItem == null)
            {
                return HttpNotFound();
            }
            return View(galleryItem);
        }

        // POST: Management/GalleryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            GalleryItem galleryItem = await db.GalleryItems.FindAsync(id);

            var categoryId = galleryItem.Gallery.Id;

            db.GalleryItems.Remove(galleryItem);

            await db.SaveChangesAsync();

            FileMethods.Delete("Gallery", Server, id);

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
