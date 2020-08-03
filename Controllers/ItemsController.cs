using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ACC.Models;

namespace ACC.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private ACCEntities db = new ACCEntities();
        public async Task<ActionResult> Index(int max_results = 30)
        {
            var Items =
                (from s in db.Items
                 select s).Take(max_results);
            return View(await Items.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        public ActionResult Create()
        {
            return View();
        }
       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PK_Item,Item_Name,Item_Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                await db.SaveChangesAsync();
                TempData["msg"] = "Create";
                return RedirectToAction("Index");
            }
            TempData["msg"] = "Error";
            return View(item);
        }
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PK_Item,Item_Name,Item_Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["msg"] = "Edit";
                return RedirectToAction("Index");
            }
            TempData["msg"] = "Error";
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Item item = await db.Items.FindAsync(id);
            db.Items.Remove(item);
            TempData["msg"] = "Delete";
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
