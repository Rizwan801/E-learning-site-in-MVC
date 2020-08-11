using CUEL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace CUEL.Controllers
{
    public class HomeController : Controller
    {
        AppDb db = new AppDb();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccountRequests()
        {
            var users = db.AppUsers.Include(u=>u.Department).Include(u=>u.Batch).Where(u => !u.Active).ToList();
            return View(users);
        }
        public ActionResult ActiveAccount(int Id)
        {
            var user = db.AppUsers.Find(Id);
            user.Active = true;
            db.SaveChanges();
            return RedirectToAction("AccountRequests");
        }
        public ActionResult DeActiveAccount(int Id)
        {
            var user = db.AppUsers.Find(Id);
            user.Active = false;
            db.SaveChanges();
            return RedirectToAction("AccountRequests");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Message()
        {
            return View();
        }
    }
}