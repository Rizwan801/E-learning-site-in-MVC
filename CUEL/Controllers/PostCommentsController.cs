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
    public class PostCommentsController : Controller
    {
        private AppDb db = new AppDb();

        // GET: PostComments
        public ActionResult Index()
        {
            var postComments = db.PostComments.Include(p => p.AppUser).Include(p => p.Post);
            return View(postComments.ToList());
        }

        // GET: PostComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostComment postComment = db.PostComments.Find(id);
            if (postComment == null)
            {
                return HttpNotFound();
            }
            return View(postComment);
        }

        // GET: PostComments/Create
        public ActionResult Create()
        {
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName");
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Title");
            return View();
        }

        // POST: PostComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostCommentID,CommentBody,PostID")] PostComment postComment, string ru)
        {
            if (ModelState.IsValid)
            {
                var u = Session["AppUser"] as AppUser;
                postComment.AppUserID = u.AppUserID;
                db.PostComments.Add(postComment);
                db.SaveChanges();
                var pu = db.Posts.Find(postComment.PostID);
                if (pu.AppUserID != u.AppUserID)
                {
                    db.Notifications.Add(new Notification()
                    {
                        DriverID = u.AppUserID,
                        AppUserID = pu.AppUserID,
                        Text = u.FullName + " commented on your post.",
                        Link = "/Posts/Details?id=" + postComment.PostID
                    });
                    db.SaveChanges();
                }
                //                return RedirectToAction("Index","Posts");
                return Redirect(ru);
            }

            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", postComment.AppUserID);
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Title", postComment.PostID);
            return View(postComment);
        }

        // GET: PostComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostComment postComment = db.PostComments.Find(id);
            if (postComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", postComment.AppUserID);
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Title", postComment.PostID);
            return View(postComment);
        }

        // POST: PostComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostCommentID,CommentBody,DateTime,PostID,AppUserID")] PostComment postComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", postComment.AppUserID);
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Title", postComment.PostID);
            return View(postComment);
        }

        // GET: PostComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostComment postComment = db.PostComments.Find(id);
            if (postComment == null)
            {
                return HttpNotFound();
            }
            return View(postComment);
        }

        // POST: PostComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostComment postComment = db.PostComments.Find(id);
            db.PostComments.Remove(postComment);
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
