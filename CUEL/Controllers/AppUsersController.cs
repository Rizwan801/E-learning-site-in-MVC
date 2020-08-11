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
    public class AppUsersController : Controller
    {
        private AppDb db = new AppDb();

        // GET: AppUsers
        public ActionResult Index()
        {
            var appUsers = db.AppUsers.Include(a => a.Batch).Include(a => a.Department);
            return View(appUsers.ToList());
        }

        // GET: AppUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        // GET: AppUsers/Create
        public ActionResult Create()
        {
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchNo");
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName");
            return View();
        }

        // POST: AppUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppUserID,UserName,Password,FullName,FatherName,DOB,Gender,Email,DepartmentID,RegNo,BatchID,UserType")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                db.AppUsers.Add(appUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchNo", appUser.BatchID);
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", appUser.DepartmentID);
            return View(appUser);
        }

        // GET: AppUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchNo", appUser.BatchID);
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", appUser.DepartmentID);
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppUserID,UserName,Password,FullName,FatherName,DOB,Gender,Email,DepartmentID,RegNo,BatchID,UserType")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchNo", appUser.BatchID);
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", appUser.DepartmentID);
            return View(appUser);
        }
        public ActionResult UserProfile(int? id)
        {
            AppUser user = db.AppUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile([Bind(Include = "AppUserId,Password")] AppUser user, HttpPostedFileBase Image1)
        {
            if (Image1 != null && Image1.ContentLength > 0)
            {
                byte[] img1 = new byte[Image1.ContentLength];
                Image1.InputStream.Read(img1, 0, Image1.ContentLength);
                user.Content = Image1.ContentType;
                user.Picture = img1;                
            }
            else
            {
                using (AppDb app = new AppDb())
                {
                    AppUser au = app.AppUsers.Find(user.AppUserID);
                    user.Picture = au.Picture;
                    user.Content = au.Content;
                }
            }
            using (AppDb appdb = new AppDb())
            {
                AppUser appUser = appdb.AppUsers.Find(user.AppUserID);
                appUser.Password = user.Password;
                appUser.Picture = user.Picture;
                appUser.Content = user.Content;
                appdb.Entry(appUser).State = EntityState.Modified;
                appdb.SaveChanges();
                return RedirectToAction("UserProfile", new { id = user.AppUserID });

            }
            return View(user);
        }
        // GET: AppUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        // POST: AppUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppUser appUser = db.AppUsers.Find(id);
            db.AppUsers.Remove(appUser);
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
