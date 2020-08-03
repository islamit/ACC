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
using Microsoft.AspNet.Identity;

namespace ACC.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private ACCEntities db = new ACCEntities();

        // GET: Accounts
        public async Task<ActionResult> Index(int max_results = 30)
        {
            var accounts =
            (from s in db.Accounts
             orderby s.Update_Date descending
             select s).Take(max_results).Include(a => a.Account_Categories).Include(a => a.Account_Types).OrderByDescending(i => i.Update_Date);
            return View(await accounts.ToListAsync());
        }
        [HttpPost]
        public JsonResult AccountList()
        {
            List<SelectListItem> Accounts = new List<SelectListItem>();
            for (int i = 0; i < 10; i++)
            {
                Accounts.Add(new SelectListItem
                {
                    Value = db.Accounts.ToList()[i].PK_Account,
                    Text = db.Accounts.ToList()[i].Account_Name
                });
            }
            return Json(Accounts);
        }

        // GET: Accounts/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountTransaction = db.Transactions.AsNoTracking()
                 .Where(t => t.FK_Account == account.PK_Account).OrderByDescending(t => t.Update_Date).ToList();

            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.FK_Account_Category = new SelectList(db.Account_Categories, "PK_Account_Category", "Account_Category");
            ViewBag.FK_Account_Type = new SelectList(db.Account_Types, "PK_Account_Type", "Account_Type");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PK_Account,Account_Name,Balance,CR_Sum,DP_Sum,FK_Account_Type,FK_Account_Category,Insert_Date,FK_Insert_User,Update_Date,FK_Update_User,Is_Delete,Delete_Date,FK_Delete_User")] Account account)
        {
            if (ModelState.IsValid)
            {
                account.PK_Account = Guid.NewGuid().ToString();
                account.FK_Insert_User = User.Identity.GetUserId();
                account.Is_Delete = false;
                db.Accounts.Add(account);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.FK_Account_Category = new SelectList(db.Account_Categories, "PK_Account_Category", "Account_Category", account.FK_Account_Category);
            ViewBag.FK_Account_Type = new SelectList(db.Account_Types, "PK_Account_Type", "Account_Type", account.FK_Account_Type);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_Account_Category = new SelectList(db.Account_Categories, "PK_Account_Category", "Account_Category", account.FK_Account_Category);
            ViewBag.FK_Account_Type = new SelectList(db.Account_Types, "PK_Account_Type", "Account_Type", account.FK_Account_Type);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PK_Account,Account_Name,Balance,CR_Sum,DP_Sum,FK_Account_Type,FK_Account_Category,Insert_Date,FK_Insert_User,Update_Date,FK_Update_User,Is_Delete,Delete_Date,FK_Delete_User")] Account account)
        {
            if (ModelState.IsValid)
            {
                account.FK_Insert_User = User.Identity.GetUserId();
                db.Entry(account).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.FK_Account_Category = new SelectList(db.Account_Categories, "PK_Account_Category", "Account_Category", account.FK_Account_Category);
            ViewBag.FK_Account_Type = new SelectList(db.Account_Types, "PK_Account_Type", "Account_Type", account.FK_Account_Type);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Account account = await db.Accounts.FindAsync(id);
            db.Accounts.Remove(account);
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
