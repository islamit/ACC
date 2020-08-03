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
    public class CustomerController : Controller
    {
        private ACCEntities db = new ACCEntities();
        // GET: Customer
        public async Task<ActionResult> Index(Boolean? Is_Delete=false, string searchString=null,int max_results=30)
        {
            var customer =
                 (from s in db.Customers
                  where s.Is_Delete == Is_Delete
                  orderby s.Update_Date descending
                  select s).Take(max_results);
            if (!String.IsNullOrEmpty(searchString))
            {
                customer =
                (from s in db.Customers
                 where s.Is_Delete == Is_Delete   &&   s.Customer_Name.Contains(searchString)
                  orderby s.Update_Date descending
                  select s).Take(max_results);
            }
            return View(await customer.ToListAsync());
        }
        public async Task<ActionResult> Index2(Boolean? Is_Delete = false, int fromDay=30,int ToDay=60)
            {
            //int daysDiff = ((TimeSpan)(date2 - date1)).Days;
            var customer =
                 (from s in db.Customers
                  join a in db.Accounts on s.FK_Account equals a.PK_Account
                  join t in db.Transactions on s.FK_Account equals t.FK_Account
                  where s.Is_Delete == Is_Delete
                  && DbFunctions.DiffDays(t.Transaction_Date, DateTime.Now) >= fromDay
                  && DbFunctions.DiffDays(t.Transaction_Date, DateTime.Now) < ToDay
                  orderby t.Transaction_Date descending
                  select s);
            
            return View(await customer.ToListAsync());
        }
        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            List<Customer> ObjList = new List<Customer>();
            var customers = (from N in ObjList
                             where N.Customer_Name.StartsWith(prefix)
                             select new { N.Customer_Name });
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        // GET: Customer/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerTransaction = db.Transactions.AsNoTracking()
                .Where(t => t.FK_Account == customer.FK_Account && t.Is_Delete != true).OrderByDescending(t => t.Update_Date).ToList();

            ViewBag.CustomerOrder = db.Orders.AsNoTracking()
                 .Where(t => t.FK_Account == customer.FK_Account && t.Is_Delete != true).OrderByDescending(t => t.Update_Date).ToList();

            ViewBag.CustomerOrderItem = db.OrderItems.AsNoTracking().ToList();


            return View(customer);
        }
        public async Task<ActionResult> Details_Ver2(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerTransaction = db.Transactions.AsNoTracking()
                 .Where(t => t.FK_Account == customer.FK_Account && t.Is_Delete!=true).OrderByDescending(t => t.Update_Date).ToList();

            ViewBag.CustomerOrder = db.Orders.AsNoTracking()
                 .Where(t => t.FK_Account == customer.FK_Account && t.Is_Delete != true).OrderByDescending(t => t.Update_Date).ToList();

            ViewBag.CustomerOrderItem = db.OrderItems.AsNoTracking().ToList();

            return View(customer);
        }
        // GET: Customer/Create
        public ActionResult Create()
        {
            ViewBag.FK_Customer_Type = new SelectList(db.Customers_Type, "PK_Customer_Type", "Customer_Type");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FileNum,PK_Customer,Customer_Name," +
            "IDCard,Mobile1,Mobile2,Address,Notes,FK_Account,Balance," +
            "FK_Customer_Type,FK_Insert_User,Insert_Date,Update_Date,FK_Update_User," +
            "Is_Delete,Delete_Date,FK_Delete_User")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.FK_Insert_User = User.Identity.GetUserId();
                db.Customers.Add(customer);
                TempData["msg"] = "Create";
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            TempData["msg"] = "Error";
           return View(customer);
           
        }
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_Customer_Type = new SelectList(db.Customers_Type, "PK_Customer_Type", "Customer_Type", customer.FK_Customer_Type);
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FileNum,PK_Customer,Balance,Customer_Name,IDCard,Mobile1,Mobile2,Address,Notes,FK_Account,FK_Customer_Type,FK_Insert_User,Insert_Date,Update_Date,FK_Update_User,Is_Delete,Delete_Date,FK_Delete_User")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.FK_Insert_User  = User.Identity.GetUserId();
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["msg"] = "Edit";
                return RedirectToAction("Index");
            }
            TempData["msg"] = "Error";
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Restore([Bind(Include = "FileNum,PK_Customer,Balance,Customer_Name,IDCard,Mobile1,Mobile2,Address,Notes,FK_Account,FK_Customer_Type,FK_Insert_User,Insert_Date,Update_Date,FK_Update_User,Is_Delete,Delete_Date,FK_Delete_User")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.FK_Insert_User = User.Identity.GetUserId();
                customer.Is_Delete=false;
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["msg"] = "Edit";
                return RedirectToAction("Index");
            }
            TempData["msg"] = "Error";
            return View(customer);
        }

        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Customer customer = await db.Customers.FindAsync(id);
            if (ModelState.IsValid)
            {
                customer.FK_Insert_User = User.Identity.GetUserId();
                db.Customers.Remove(customer);
                await db.SaveChangesAsync();
                TempData["msg"] = "Delete";
                return RedirectToAction("Index");
            }
              TempData["msg"] = "Error";
            return View(customer);
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
