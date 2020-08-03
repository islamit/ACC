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
    public class OrdersController : Controller
    {
        private ACCEntities db = new ACCEntities();
        public async Task<ActionResult> Index(Boolean? Is_Delete = false, string searchString = null, int max_results = 30)
        {
            ViewBag.FK_Item = new SelectList(db.Items, "PK_Item", "Item_Name");
            ViewBag.FK_Sales_Man = new SelectList(db.Sales_Man, "PK_Sales_Man", "Sales_Man_Name");
            ViewBag.FK_Account = new SelectList(db.Accounts.Where(i => i.Is_Delete == false).OrderByDescending(a => a.Update_Date), "PK_Account", "Account_Name");
            var orders =
               (from s in db.Orders
                where s.Is_Delete == Is_Delete
                orderby s.Update_Date descending
                select s).Take(max_results).Include(o => o.Account).Include(o => o.Sales_Man);
            if (!String.IsNullOrEmpty(searchString))
            {
                orders =
                (from s in db.Orders
                 where s.Is_Delete == Is_Delete && s.Account.Account_Name.Contains(searchString)
                 orderby s.Update_Date descending
                 select s).Take(max_results).Include(o => o.Account).Include(o => o.Sales_Man);
            }
            return View(await orders.ToListAsync());
        }
       
        [HttpPost]
        public ActionResult SaveOrder(byte FK_Sales_Man,
                String OrderNo,
                String FK_Account,
                DateTime Order_Date,
                String Sponsor,
                String Sponsor_Info,
                Decimal Payment,
                String Notes,
                int Voucher_Num,
                OrderItem[] order_item)
        {

            Int64 PK_Order = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
            string result = "Error! Order Is Not Complete!";
            if (OrderNo != null)
            {

                Order model = new Order();
                model.PK_Order = PK_Order;
                model.OrderNo = OrderNo;
                model.FK_Sales_Man = FK_Sales_Man;
                model.FK_Account = FK_Account;
                model.Order_Date = Order_Date;
                model.Sponsor_Info = Sponsor_Info;
                model.FK_Insert_User = User.Identity.GetUserId();
                model.Is_Delete = false;
                model.Sponsor = Sponsor;
                model.Payment = Payment;
                model.Notes = Notes;
                model.Voucher_Num = Voucher_Num;
                


                db.Orders.Add(model);
                db.SaveChanges();
                foreach (var item in order_item)
                {
                    //var orderId = Guid.NewGuid();
                    Random r = new Random();
                    var x = r.Next(1000000, 2000000);
                    string s = x.ToString("000000");

                    OrderItem O = new OrderItem();
                    Int64 PK_OrderItem = long.Parse(s);
                    O.PK_OrderItem = PK_OrderItem;
                    O.FK_Order = PK_Order;
                    O.FK_Item = item.FK_Item;
                    O.Quantity = item.Quantity;
                    O.Price = item.Price;
                    db.OrderItems.Add(O);
                    db.SaveChanges();

                }

                TempData["msg"] = "Create";
                result = "Success! Order Is Complete!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    
    public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_Account = new SelectList(db.Accounts, "PK_Account", "Account_Name", order.FK_Account);
            ViewBag.FK_Sales_Man = new SelectList(db.Sales_Man, "PK_Sales_Man", "Sales_Man_Name", order.FK_Sales_Man);
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PK_Order,FK_Sales_Man,OrderNo,FK_Account,Order_Date,Sponsor,Sponsor_Info,GTotal,FK_Insert_User,FK_Transaction,Insert_Date,Update_Date,FK_Update_User,Is_Delete,Delete_Date,FK_Delete_User,Payment,Notes,Voucher_Num")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.FK_Insert_User = User.Identity.GetUserId();
                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["msg"] = "Edit";
                return RedirectToAction("Index");
            }
            ViewBag.FK_Account = new SelectList(db.Accounts, "PK_Account", "Account_Name", order.FK_Account);
            ViewBag.FK_Sales_Man = new SelectList(db.Sales_Man, "PK_Sales_Man", "Sales_Man_Name", order.FK_Sales_Man);
            TempData["msg"] = "Error";
            return View(order);
        }
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.orderItem = db.OrderItems.AsNoTracking()
               .Where(i => i.FK_Order == id).OrderByDescending(i => i.FK_Item).ToList();

            return View(order);
             
        }
        public async Task<ActionResult> Order(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.orderItem = db.OrderItems.AsNoTracking()
               .Where(i => i.FK_Order == id).OrderByDescending(i => i.FK_Item).ToList();

            return View(order);

        }
        public ActionResult Create()
        {
            ViewBag.FK_Sales_Man = new SelectList(db.Sales_Man, "PK_Sales_Man", "Sales_Man_Name");
            ViewBag.FK_Account = new SelectList(db.Accounts.Where(i=>i.Is_Delete==false ).OrderByDescending(a=>a.Update_Date), "PK_Account", "Account_Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PK_Order,FK_Sales_Man,OrderNo,FK_Account,Order_Date,Sponsor,Sponsor_Info,GTotal,FK_Insert_User,FK_Transaction,Insert_Date,Update_Date,FK_Update_User,Is_Delete,Delete_Date,FK_Delete_User")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.FK_Insert_User = User.Identity.GetUserId();

                db.Orders.Add(order);
                await db.SaveChangesAsync();
                TempData["msg"] = "Create";
                return RedirectToAction("Index");
            }
            ViewBag.FK_Account = new SelectList(db.Accounts, "PK_Account", "Account_Name", order.FK_Account);
            ViewBag.FK_Sales_Man = new SelectList(db.Sales_Man, "PK_Sales_Man", "Sales_Man_Name", order.FK_Sales_Man);
            TempData["msg"] = "Error";
            return View(order);
        }
        public async Task<ActionResult> Update(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = await db.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_Item = new SelectList(db.Items, "PK_Item", "Item_Name", orderItem.FK_Item);
            ViewBag.FK_Order = new SelectList(db.Orders, "PK_Order", "OrderNo", orderItem.FK_Order);
            return View(orderItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update([Bind(Include = "PK_OrderItem,FK_Order,FK_Item,Quantity,Price")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {   

                db.Entry(orderItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["msg"] = "Edit";
                return RedirectToActionPermanent("Details", "Orders", new { id = orderItem.FK_Order });

            }
            TempData["msg"] = "Error";
            return RedirectToActionPermanent("Details", "Orders", new { id = orderItem.FK_Order });
        }
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {

            Order order = await db.Orders.FindAsync(id);
            order.FK_Insert_User = User.Identity.GetUserId();

            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            TempData["msg"] = "Delete";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Add(long FK_Order)
        {
            ViewBag.FK_Item = new SelectList(db.Items, "PK_Item", "Item_Name");
            OrderItem oi = new OrderItem
            { FK_Order = FK_Order };
            return View(oi);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add([Bind(Include = "PK_OrderItem,FK_Order,FK_Item,Quantity,Price")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                //var orderId = Guid.NewGuid();
                Random r = new Random();
                var x = r.Next(1000000, 2000000);
                string s = x.ToString("000000");
                Int64 PK_OrderItem = long.Parse(s);
                orderItem.PK_OrderItem = PK_OrderItem;

                db.OrderItems.Add(orderItem);
                await db.SaveChangesAsync();
                TempData["msg"] = "Create";
                return RedirectToActionPermanent("Details", "Orders", new { id = orderItem.FK_Order });
            }
            ViewBag.ItemID = new SelectList(db.Items, "PK_Item", "Item_Name", orderItem.FK_Item);
            TempData["msg"] = "Error";
            return View(orderItem);
        }
        public async Task<ActionResult> Remove(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = await db.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Remove(long id)
        {
            OrderItem orderItem = await db.OrderItems.FindAsync(id);
            ViewBag.PK = orderItem.FK_Order;
            db.OrderItems.Remove(orderItem);
            await db.SaveChangesAsync();
            TempData["msg"] = "Delete";
            return RedirectToActionPermanent("Details", "Orders", new { id = ViewBag.PK });
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
