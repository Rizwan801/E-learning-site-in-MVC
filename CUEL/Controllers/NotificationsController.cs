using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CUEL.Models;

namespace CUEL.Controllers
{
    public class NotificationsController : Controller
    {
        private AppDb db = new AppDb();

        // GET: Notifications
        public ActionResult Index()
        {
            if (Session["AppUser"] != null)
            {
                AppUser user = Session["AppUser"] as AppUser;
                var notifications = db.Notifications.Include(n => n.AppUser).Where(n=>n.AppUserID == user.AppUserID);
                notifications = notifications.OrderByDescending(n => n.DateTime);
                return View(notifications.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public PartialViewResult Latest()
        {
            if (Session["AppUser"] != null)
            {
                AppUser user = Session["AppUser"] as AppUser;
                var notifications = db.Notifications.Include(n => n.AppUser).Where(n=>n.AppUserID == user.AppUserID);
                notifications = notifications.OrderByDescending(n => n.DateTime);
                if (notifications.Count() > 5)
                {
                    notifications = notifications.Take(5);
                }
                return PartialView(notifications.ToList());
            }
            else
            {
                return PartialView(new List<Notification>());
            }
        }

        // GET: Notifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // GET: Notifications/Create
        public ActionResult Create()
        {
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName");
            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NotificationID,Text,Link,AppUserID")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                db.Notifications.Add(notification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", notification.AppUserID);
            return View(notification);
        }

        // GET: Notifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", notification.AppUserID);
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NotificationID,Text,Link,AppUserID")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", notification.AppUserID);
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notification notification = db.Notifications.Find(id);
            db.Notifications.Remove(notification);
            db.SaveChanges();
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
