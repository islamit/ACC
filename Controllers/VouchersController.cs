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
    public class VouchersController : Controller
    {
        private ACCEntities db = new ACCEntities();

        // GET: Vouchers
        public async Task<ActionResult> Index(Boolean? Is_Delete = false, string searchString = null, int max_results = 30)
        {
            var vouchers =
               (from s in db.Vouchers
                where s.Is_Delete == Is_Delete
                orderby s.Update_Date descending
                select s).Take(max_results).Include(v => v.Account).Include(v => v.Voucher_Types);
            if (!String.IsNullOrEmpty(searchString))
            {
                vouchers =
                (from s in db.Vouchers 
                 where s.Is_Delete == Is_Delete && s.Account.Account_Name.Contains(searchString)
                 orderby s.Update_Date descending
                 select s).Take(max_results).Include(v => v.Account).Include(v => v.Voucher_Types);
            }

            return View(await vouchers.ToListAsync());
        }

        // GET: Vouchers/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = await db.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        // GET: Vouchers/Create
        public ActionResult Create()
        {
            ViewBag.FK_Account = new SelectList(db.Accounts.Where(i => i.Is_Delete == false).OrderByDescending(a => a.Update_Date), "PK_Account", "Account_Name");
            ViewBag.FK_Voucher_Type = new SelectList(db.Voucher_Types, "PK_Voucher_Type", "Voucher_Type");
            return View();
        }

        // POST: Vouchers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = 
            "PK_Voucher,FK_Sales_Man,FK_Voucher_Type,Voucher_Num,Voucher_Date," +
            "FK_Account,Voucher_Name,Amount," +
            "FK_Insert_User,FK_Transaction,FK_Transaction_Type,FK_Voucher_Status,Insert_Date,Update_Date," +
            "FK_Update_User,Is_Delete,Delete_Date,FK_Delete_User")] Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                voucher.FK_Insert_User = User.Identity.GetUserId();
                db.Vouchers.Add(voucher);
                await db.SaveChangesAsync();
                TempData["msg"] = "Create";
                return RedirectToAction("Index");
            }

            ViewBag.FK_Account = new SelectList(db.Accounts.Where(i => i.Is_Delete == false), "PK_Account", "Account_Name", voucher.FK_Account);
            ViewBag.FK_Voucher_Type = new SelectList(db.Voucher_Types, "PK_Voucher_Type", "Voucher_Type", voucher.FK_Voucher_Type);
            TempData["msg"] = "Error";
            return View(voucher);
        }

        // GET: Vouchers/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = await db.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_Account = new SelectList(db.Accounts.Where(i => i.Is_Delete == false), "PK_Account", "Account_Name", voucher.FK_Account);
            ViewBag.FK_Voucher_Type = new SelectList(db.Voucher_Types, "PK_Voucher_Type", "Voucher_Type", voucher.FK_Voucher_Type);
            return View(voucher);
        }

        // POST: Vouchers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PK_Voucher,FK_Voucher_Type,Voucher_Num,Voucher_Date,FK_Account,Voucher_Name,Amount,FK_Insert_User,FK_Transaction,FK_Transaction_Type,FK_Voucher_Status,Insert_Date,Update_Date,FK_Update_User,Is_Delete,Delete_Date,FK_Delete_User")] Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                voucher.FK_Insert_User  = User.Identity.GetUserId();
                db.Entry(voucher).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["msg"] = "Edit";

                return RedirectToAction("Index");
            }
            ViewBag.FK_Account = new SelectList(db.Accounts, "PK_Account", "Account_Name", voucher.FK_Account);
            
            ViewBag.FK_Voucher_Type = new SelectList(db.Voucher_Types, "PK_Voucher_Type", "Voucher_Type", voucher.FK_Voucher_Type);
            TempData["msg"] = "Error";
            return View(voucher);
        }

        // GET: Vouchers/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = await db.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        // POST: Vouchers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Voucher voucher = await db.Vouchers.FindAsync(id);
            if (ModelState.IsValid)
            {
                voucher.FK_Insert_User = User.Identity.GetUserId();
                db.Vouchers.Remove(voucher);
                await db.SaveChangesAsync();
                TempData["msg"] = "Delete";
                return RedirectToAction("Index");
            }
            ViewBag.FK_Account = new SelectList(db.Accounts, "PK_Account", "Account_Name", voucher.FK_Account);
            ViewBag.FK_Voucher_Type = new SelectList(db.Voucher_Types, "PK_Voucher_Type", "Voucher_Type", voucher.FK_Voucher_Type);
            TempData["msg"] = "Error";
            return View(voucher);
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
