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
    public class FormPostsController : Controller
    {
        private AppDb db = new AppDb();

        // GET: FormPosts
        public ActionResult Index()
        {
            var formPosts = db.FormPosts.Include(f => f.AppUser).Include(f => f.DiscussionForm);
            return View(formPosts.ToList());
        }

        // GET: FormPosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormPost post = db.FormPosts.Include(p => p.AppUser).Include(p => p.FormPostComments)
                .Include(p => p.FormPostComments.Select(c => c.AppUser))
                .Include(p => p.FormPostLikes).FirstOrDefault(p => p.FormPostID == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: FormPosts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FormPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FormPostID,Title,Description,DiscussionFormID")] FormPost formPost, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    byte[] img = new byte[Image.ContentLength];
                    Image.InputStream.Read(img, 0, Image.ContentLength);
                    formPost.FileType = Image.ContentType;
                    formPost.File = img;
                }
                var u = Session["AppUser"] as AppUser;
                formPost.AppUserID = u.AppUserID;
                db.FormPosts.Add(formPost);
                db.SaveChanges();
                return RedirectToAction("Discussions", "DiscussionForms",new { id = formPost.DiscussionFormID });
            }
            return View(formPost);
        }

        // GET: FormPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormPost formPost = db.FormPosts.Find(id);
            if (formPost == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", formPost.AppUserID);
            ViewBag.DiscussionFormID = new SelectList(db.DiscussionForms, "DiscussionFormID", "Title", formPost.DiscussionFormID);
            return View(formPost);
        }

        // POST: FormPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FormPostID,Title,Description,File,FileType,Added,AppUserID,DiscussionFormID")] FormPost formPost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", formPost.AppUserID);
            ViewBag.DiscussionFormID = new SelectList(db.DiscussionForms, "DiscussionFormID", "Title", formPost.DiscussionFormID);
            return View(formPost);
        }

        // GET: FormPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormPost formPost = db.FormPosts.Include(p => p.AppUser).Include(p => p.FormPostComments).Include(p => p.FormPostComments.Select(c => c.AppUser)).Include(p => p.FormPostLikes).FirstOrDefault(f=>f.FormPostID == id); 
            if (formPost == null)
            {
                return HttpNotFound();
            }
            int fid = formPost.DiscussionFormID.Value;
            db.FormPosts.Remove(formPost);
            db.SaveChanges();
            return RedirectToAction("Discussions", "DiscussionForms",new { id = fid});
         //   return View(formPost);
        }

        // POST: FormPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FormPost formPost = db.FormPosts.Find(id);
            db.FormPosts.Remove(formPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public FileContentResult GetFile(int id)
        {
            FormPost post = db.FormPosts.Find(id);
            if (post.File != null && post.File.Length > 0)
            {
                return File(post.File, post.FileType);
            }
            return null;
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
