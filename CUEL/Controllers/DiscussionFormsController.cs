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
    public class DiscussionFormsController : Controller
    {
        private AppDb db = new AppDb();

        // GET: DiscussionForms
        public ActionResult Index()
        {
            return View(db.DiscussionForms.ToList());
        }
        public ActionResult Subscribe(int id)
        {
            var user = Session["AppUser"] as AppUser;
            if (user != null)
            {
                db.UserDiscussions.Add(new UserDiscussion()
                {
                    AppUserID = user.AppUserID,
                    DiscussionFormID = id
                });
                db.SaveChanges();
                return RedirectToAction("Forms");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult UnSubscribe(int id)
        {
            var user = Session["AppUser"] as AppUser;
            if (user != null)
            {
                var res = db.UserDiscussions.FirstOrDefault(ud => ud.AppUserID == user.AppUserID && ud.DiscussionFormID == id);
                if (res != null)
                {
                    db.Entry(res).State = EntityState.Deleted;
                    db.UserDiscussions.Remove(res);
                    db.SaveChanges();
                }
                return RedirectToAction("Forms");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Forms()
        {
            var user = Session["AppUser"] as AppUser;
            if (user != null)
            {
                FormsVM forms = new FormsVM();
                var sforms = db.UserDiscussions.Where(ud => ud.AppUserID == user.AppUserID).ToList();
                var tforms = db.DiscussionForms.ToList();
                tforms.ForEach(t => {
                    if (sforms.Any(s=>s.DiscussionFormID == t.DiscussionFormID))
                    {
                        forms.SubscribedFroms.Add(t);
                    }
                    else
                    {
                        forms.UnSubscribedFroms.Add(t);
                    }
                });
                return View(forms);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult Discussions(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscussionForm discussionForm = db.DiscussionForms.Find(id);
            if (discussionForm == null)
            {
                return HttpNotFound();
            }
            var posts = db.FormPosts.Where(p => p.DiscussionFormID == id).Include(p => p.AppUser).Include(p => p.FormPostComments).Include(p => p.FormPostComments.Select(c => c.AppUser)).Include(p => p.FormPostLikes);
            posts = posts.OrderByDescending(p => p.Added);
            //return View(posts.ToList());
            discussionForm.FormPosts = posts.ToList();
            return View(discussionForm);
        }

        // GET: DiscussionForms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscussionForm discussionForm = db.DiscussionForms.Find(id);
            if (discussionForm == null)
            {
                return HttpNotFound();
            }
            return View(discussionForm);
        }

        // GET: DiscussionForms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DiscussionForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DiscussionFormID,Title")] DiscussionForm discussionForm)
        {
            if (ModelState.IsValid)
            {
                db.DiscussionForms.Add(discussionForm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(discussionForm);
        }

        // GET: DiscussionForms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscussionForm discussionForm = db.DiscussionForms.Find(id);
            if (discussionForm == null)
            {
                return HttpNotFound();
            }
            return View(discussionForm);
        }

        // POST: DiscussionForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DiscussionFormID,Title")] DiscussionForm discussionForm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discussionForm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(discussionForm);
        }

        // GET: DiscussionForms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscussionForm discussionForm = db.DiscussionForms.Find(id);
            if (discussionForm == null)
            {
                return HttpNotFound();
            }
            return View(discussionForm);
        }

        // POST: DiscussionForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiscussionForm discussionForm = db.DiscussionForms.Find(id);
            db.DiscussionForms.Remove(discussionForm);
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
    public class FormsVM
    {
        public List<DiscussionForm> SubscribedFroms { get; set; } = new List<DiscussionForm>();
        public List<DiscussionForm> UnSubscribedFroms { get; set; } = new List<DiscussionForm>();
    }
}
