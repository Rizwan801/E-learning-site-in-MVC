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
    public class StudentsController : Controller
    {
        private AppDb db = new AppDb();

        // GET: Students
        public ActionResult Index()
        {
            var appUsers = db.AppUsers.Include(a => a.Batch).Include(a => a.Department).Where(u=>u.UserType == UserType.Student && u.Active);
            return View(appUsers.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.AppUsers.Include(a => a.Batch).Include(a => a.Department).FirstOrDefault(u=>u.AppUserID == id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchNo");
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppUserID,UserName,Password,FullName,FatherName,DOB,Gender,Email,DepartmentID,RegNo,BatchID")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                appUser.UserType = UserType.Student;
                db.AppUsers.Add(appUser);
                db.SaveChanges();
                return RedirectToAction("Login","Account");
            }

            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchNo", appUser.BatchID);
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", appUser.DepartmentID);
            return View(appUser);
        }

        // GET: Students/Edit/5
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

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppUserID,UserName,Password,FullName,FatherName,DOB,Gender,Email,DepartmentID,RegNo,BatchID")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                appUser.UserType = UserType.Student; 
                db.Entry(appUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchNo", appUser.BatchID);
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", appUser.DepartmentID);
            return View(appUser);
        }

        // GET: Students/Delete/5
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

        // POST: Students/Delete/5
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
