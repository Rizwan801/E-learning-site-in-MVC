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
    public class FormPostCommentsController : Controller
    {
        private AppDb db = new AppDb();

        // GET: FormPostComments
        public ActionResult Index()
        {
            var formPostComments = db.FormPostComments.Include(f => f.AppUser);
            return View(formPostComments.ToList());
        }

        // GET: FormPostComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormPostComment formPostComment = db.FormPostComments.Find(id);
            if (formPostComment == null)
            {
                return HttpNotFound();
            }
            return View(formPostComment);
        }

        // GET: FormPostComments/Create
        public ActionResult Create()
        {
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName");
            return View();
        }

        // POST: FormPostComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FormPostCommentID,CommentBody,FormPostID")] FormPostComment formPostComment,int DiscussionFormID, string ru)
        {
            if (ModelState.IsValid)
            {
                var u = Session["AppUser"] as AppUser;
                formPostComment.AppUserID = u.AppUserID;
                db.FormPostComments.Add(formPostComment);
                db.SaveChanges();
                var pu = db.FormPosts.Find(formPostComment.FormPostID);
                if (pu.AppUserID != u.AppUserID)
                {
                    db.Notifications.Add(new Notification()
                    {
                        DriverID = u.AppUserID,
                        AppUserID = pu.AppUserID,
                        Text = u.FullName + " commented on your post in "+db.DiscussionForms.Find(pu.DiscussionFormID).Title+".",
                        Link = "/FormPosts/Details?id=" + formPostComment.FormPostID
                    });
                    db.SaveChanges();
                }
                //return RedirectToAction("Discussions", "DiscussionForms", new { id = DiscussionFormID });
                return Redirect(ru);
            }

            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", formPostComment.AppUserID);
            return View(formPostComment);
        }

        // GET: FormPostComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormPostComment formPostComment = db.FormPostComments.Find(id);
            if (formPostComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", formPostComment.AppUserID);
            return View(formPostComment);
        }

        // POST: FormPostComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FormPostCommentID,CommentBody,DateTime,PostID,AppUserID")] FormPostComment formPostComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formPostComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", formPostComment.AppUserID);
            return View(formPostComment);
        }

        // GET: FormPostComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormPostComment formPostComment = db.FormPostComments.Find(id);
            if (formPostComment == null)
            {
                return HttpNotFound();
            }
            return View(formPostComment);
        }

        // POST: FormPostComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FormPostComment formPostComment = db.FormPostComments.Find(id);
            db.FormPostComments.Remove(formPostComment);
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
