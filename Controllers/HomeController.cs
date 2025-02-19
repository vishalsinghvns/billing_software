using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Preora.dbcon;

namespace SofttrixAccounts.Controllers
{
    public class HomeController : Controller
    {
        Preora_DBEntities db = new Preora_DBEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var log = db.Mst_Users.Where(x => x.username == username && x.password == password).FirstOrDefault();
            if (log != null && log.is_deleted == false)
            {
                Session["uid"] = log.id;
                Session["uname"] = log.username;
                Session["name"] = log.Uname;
                Session["role"] = log.rolename;
                List<int?> list = db.tbl_UserHasMenus.Where(x => x.userid == log.id).Select(x=>x.m_id).ToList();
                int?[] arr = list.ToArray();
                Session["menu"] = arr;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ModelState.AddModelError("Error", "The username or password provided is incorrect.");
                return View();
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
    }
}