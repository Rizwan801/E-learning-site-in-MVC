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
    public class PostsController : Controller
    {
        private AppDb db = new AppDb();

        // GET: Posts
        public ActionResult Index()
        {
            if (Session["AppUser"] != null)
            {
                AppUser user = Session["AppUser"] as AppUser;
                List<Post> posts = new List<Post>();
                if (user.UserType == UserType.Admin)
                {
                    posts = db.Posts.Include(p => p.AppUser).Include(p => p.PostComments).Include(p => p.PostComments.Select(c => c.AppUser)).Include(p => p.PostLikes).ToList();
                }
                else
                {
                    posts = db.Posts.Where(p => p.AppUser.DepartmentID == user.DepartmentID).Include(p => p.AppUser).Include(p => p.PostComments).Include(p => p.PostComments.Select(c => c.AppUser)).Include(p => p.PostLikes).ToList();
                }
                posts = posts.OrderByDescending(p => p.Added).ToList();
                return View(posts.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        public ActionResult UserProfile(int? id)
        {
            if (Session["AppUser"] != null)
            {
                if (id == null || id == 0)
                {
                    AppUser user = Session["AppUser"] as AppUser;
                    var posts = db.Posts.Where(p => p.AppUser.AppUserID == user.AppUserID).Include(p => p.AppUser).Include(p => p.PostComments).Include(p => p.PostComments.Select(c => c.AppUser)).Include(p => p.PostLikes);
                    posts = posts.OrderByDescending(p => p.Added);
                    return View("Index",posts.ToList());
                }
                else
                {
                    AppUser user = db.AppUsers.Find(id);
                    var posts = db.Posts.Where(p => p.AppUser.AppUserID == user.AppUserID).Include(p => p.AppUser).Include(p => p.PostComments).Include(p => p.PostComments.Select(c => c.AppUser)).Include(p => p.PostLikes);
                    posts = posts.OrderByDescending(p => p.Added);
                    return View("Index",posts.ToList());
                }

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Include(p => p.AppUser).Include(p => p.PostComments)
                .Include(p => p.PostComments.Select(c => c.AppUser))
                .Include(p => p.PostLikes).FirstOrDefault(p => p.PostID == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostID,Title,Description")] Post post, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    byte[] img = new byte[Image.ContentLength];
                    Image.InputStream.Read(img, 0, Image.ContentLength);
                    post.FileType = Image.ContentType;
                    post.File = img;
                }
                var u = Session["AppUser"] as AppUser;
                post.AppUserID = u.AppUserID;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", post.AppUserID);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostID,Title,Description,File,FileType,AppUserID")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", post.AppUserID);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Include(p => p.AppUser).Include(p => p.PostComments).Include(p => p.PostComments.Select(c => c.AppUser)).Include(p => p.PostLikes).FirstOrDefault(p=>p.PostID == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
//            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public FileContentResult GetFile(int id)
        {
            Post post = db.Posts.Find(id);
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
