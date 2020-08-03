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
    public class Sales_ManController : Controller
    {
        private ACCEntities db = new ACCEntities();

        // GET: Sales_Man
        public async Task<ActionResult> Index()
        {
            return View(await db.Sales_Man.ToListAsync());
        }

        // GET: Sales_Man/Details/5
        public async Task<ActionResult> Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sales_Man sales_Man = await db.Sales_Man.FindAsync(id);
            if (sales_Man == null)
            {
                return HttpNotFound();
            }
            return View(sales_Man);
        }

        // GET: Sales_Man/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sales_Man/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PK_Sales_Man,Sales_Man_Name")] Sales_Man sales_Man)
        {
            if (ModelState.IsValid)
            {
                db.Sales_Man.Add(sales_Man);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sales_Man);
        }

        // GET: Sales_Man/Edit/5
        public async Task<ActionResult> Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sales_Man sales_Man = await db.Sales_Man.FindAsync(id);
            if (sales_Man == null)
            {
                return HttpNotFound();
            }
            return View(sales_Man);
        }

        // POST: Sales_Man/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PK_Sales_Man,Sales_Man_Name")] Sales_Man sales_Man)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sales_Man).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sales_Man);
        }

        // GET: Sales_Man/Delete/5
        public async Task<ActionResult> Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sales_Man sales_Man = await db.Sales_Man.FindAsync(id);
            if (sales_Man == null)
            {
                return HttpNotFound();
            }
            return View(sales_Man);
        }

        // POST: Sales_Man/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(byte id)
        {
            Sales_Man sales_Man = await db.Sales_Man.FindAsync(id);
            db.Sales_Man.Remove(sales_Man);
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
