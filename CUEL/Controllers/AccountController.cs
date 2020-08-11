using CUEL.Models;
using CUEL.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CUEL.Controllers
{
    public class AccountController : Controller
    {
        AppDb db = new AppDb();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult AdminLogin()
        {
            return View();
        }
        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM login)
        {
            using (AppDb sdb = new AppDb())
            {
                var result = db.AppUsers.Where(u => (u.Email == login.Email || u.UserName == login.Email) && u.Password == login.Password).Include(u => u.Department).Include(u => u.Batch).FirstOrDefault();
                if (result != null)
                {
                    Session["AppUser"] = result;
                    if (result.UserType == UserType.Admin)
                    {
//                        return RedirectToAction("Index", "Home");
                        ViewBag.Error = "Kindly Use Admin Login Page!";
                        return View(login);
                    }
                    else if (result.Active)
                    {
                        ViewBag.Error = "Your Acount is not Active!";
                        return RedirectToAction("Index", "Posts");
                    }
                    else
                    {
                        ViewBag.Error = "Your Acount is not Active!";
                        return View(login);
                    }
                }
                else
                {
                    ViewBag.Error = "Authentications are Incorrect!";
                    return View(login);
                }
            }
           // return View(login);
        }
        [HttpPost]
        public ActionResult AdminLogin(LoginVM login)
        {
            using (AppDb sdb = new AppDb())
            {
                var result = db.AppUsers.Where(u => (u.Email == login.Email || u.UserName == login.Email) && u.Password == login.Password && u.UserType == UserType.Admin).Include(u => u.Department).Include(u => u.Batch).FirstOrDefault();
                if (result != null)
                {
                    Session["AppUser"] = result;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Authentications are Incorrect!";
                    return View(login);
                }
            }
        }


        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session["AppUser"] = null;
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
        public FileContentResult GetPicture(int id)
        {
            AppUser user = db.AppUsers.Find(id);
            if (user.Picture != null && user.Picture.Length > 0)
            {
                return File(user.Picture, user.Content);
            }
            return null;
        }
    }
}