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
    public class MessagesController : Controller
    {
        private AppDb db = new AppDb();

        // GET: Messages
        public ActionResult Index()
        {
            var user = Session["AppUser"] as AppUser;
            if (user != null)
            {
                var chats = new List<Chat>();
                chats = db.Chats.Include(c => c.Messages).Include(c => c.Messages.Select(m => m.AppUser)).Where(c => c.AppUser1ID == user.AppUserID || c.AppUser2ID == user.AppUserID).ToList();
                foreach (var chat in chats)
                {
                    chat.AppUser1 = db.AppUsers.Find(chat.AppUser1ID);
                    chat.AppUser2 = db.AppUsers.Find(chat.AppUser2ID);
                }
                return View(chats);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        [HttpGet]
        public ActionResult ComposeMessage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ComposeMessage(string email, string msg)
        {
            var reciever = db.AppUsers.Where(u => u.Email == email || u.UserName == email).FirstOrDefault();
            if (reciever != null)
            {
                var sender = Session["AppUser"] as AppUser;
                var chat = db.Chats.Where(c => (c.AppUser1ID == sender.AppUserID && c.AppUser2ID == reciever.AppUserID)
                || (c.AppUser1ID == reciever.AppUserID && c.AppUser2ID == sender.AppUserID)).FirstOrDefault();
                if (chat != null)
                {
                    Message m = new Message()
                    {
                        AppUserID = sender.AppUserID,
                        ChatID = chat.ChatID,
                        MessageBody = msg
                    };
                    db.Messages.Add(m);
                    db.SaveChanges();
                }
                else
                {
                    Chat c = new Chat()
                    {
                        AppUser1ID = sender.AppUserID,
                        AppUser2ID = reciever.AppUserID,
                        Messages = new List<Message>(){
                            new Message(){
                                AppUserID = sender.AppUserID,
                         MessageBody = msg}
                        }
                    };
                    db.Chats.Add(c);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            else
            {
                ViewBag.Error = $"{email} not found!";
                return View();
            }
        }
        [HttpPost]
        public ActionResult SendMessage(int cid, string msg)
        {
            var sender = Session["AppUser"] as AppUser;
            var chat = db.Chats.Find(cid);
            if (chat != null)
            {
                Message m = new Message()
                {
                    AppUserID = sender.AppUserID,
                    ChatID = chat.ChatID,
                    MessageBody = msg
                };
                db.Messages.Add(m);
                db.SaveChanges();
            }            
            return RedirectToAction("Index");
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName");
            ViewBag.ChatID = new SelectList(db.Chats, "ChatID", "ChatID");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageID,MessageBody,AppUserID,ChatID")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", message.AppUserID);
            ViewBag.ChatID = new SelectList(db.Chats, "ChatID", "ChatID", message.ChatID);
            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", message.AppUserID);
            ViewBag.ChatID = new SelectList(db.Chats, "ChatID", "ChatID", message.ChatID);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageID,MessageBody,AppUserID,ChatID")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", message.AppUserID);
            ViewBag.ChatID = new SelectList(db.Chats, "ChatID", "ChatID", message.ChatID);
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
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
