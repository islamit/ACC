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
    public class TransactionsController : Controller
    {
        private ACCEntities db = new ACCEntities();

        // GET: Transactions
        public async Task<ActionResult> Index(Boolean? Is_Delete=false, int max_results = 30)
        {
            //var transactions = db.Transactions.Where(i => i.Is_Delete == Is_Delete).OrderByDescending(t=>t.Update_Date).Include(t => t.Account).Include(t => t.Transaction_Types);
            var transactions =
              (from s in db.Transactions
               where s.Is_Delete == Is_Delete
               orderby s.Update_Date descending
               select s).Take(max_results).Include(t => t.Account).Include(t => t.Transaction_Types);
            return View(await transactions.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = await db.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.FK_Account = new SelectList(db.Accounts, "PK_Account", "Account_Name");
            ViewBag.FK_Transaction_Type = new SelectList(db.Transaction_Types, "PK_Transaction_Type", "Transaction_Type");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PK_Transaction,FK_Transaction_Type,Transaction_Date,FK_Account,Transaction_Name,Amount,FK_Insert_User,Insert_Date,FK_Transaction_Status,Update_Date,FK_Update_User,Is_Delete,Delete_Date,FK_Delete_User")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.FK_Account = new SelectList(db.Accounts, "PK_Account", "Account_Name", transaction.FK_Account);
            ViewBag.FK_Transaction_Type = new SelectList(db.Transaction_Types, "PK_Transaction_Type", "Transaction_Type", transaction.FK_Transaction_Type);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = await db.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_Account = new SelectList(db.Accounts, "PK_Account", "Account_Name", transaction.FK_Account);
            ViewBag.FK_Transaction_Type = new SelectList(db.Transaction_Types, "PK_Transaction_Type", "Transaction_Type", transaction.FK_Transaction_Type);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PK_Transaction,FK_Transaction_Type,Transaction_Date,FK_Account,Transaction_Name,Amount,FK_Insert_User,Insert_Date,FK_Transaction_Status,Update_Date,FK_Update_User,Is_Delete,Delete_Date,FK_Delete_User")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.FK_Account = new SelectList(db.Accounts, "PK_Account", "Account_Name", transaction.FK_Account);
            ViewBag.FK_Transaction_Type = new SelectList(db.Transaction_Types, "PK_Transaction_Type", "Transaction_Type", transaction.FK_Transaction_Type);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = await db.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Transaction transaction = await db.Transactions.FindAsync(id);
            db.Transactions.Remove(transaction);
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
