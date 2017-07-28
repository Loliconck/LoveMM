using LoveMM.DAL;
using LoveMM.Filters;
using LoveMM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoveMM.Controllers
{
    [AdminAuthorization]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        LoveMMContext dbContext = new LoveMMContext();
        private static string AuthSaveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Admin admin = dbContext.Admins.FirstOrDefault(t => t.UserName == model.UserName && t.Password.ToLower() == model.Password.ToLower() && t.IsDelete == false);
                if (admin != null && admin.Id > 0)
                {
                    admin.LastLoginTime = DateTime.Now;
                    dbContext.SaveChanges();
                    Session[AuthSaveKey] = admin.Id;
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError("LoginError", "用户名或密码不正确。");
            return View(model);
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Admin admin)
        {
            if (ModelState.IsValid)
            {
                admin.CTime = DateTime.Now;
                admin.IsDelete = false;
                admin.LastLoginTime = DateTime.Now;
                dbContext.Admins.Add(admin);
                dbContext.SaveChanges();
                return RedirectToAction("Login");
            }
            ModelState.AddModelError("CreateError", "创建管理员出错。");
            return View(admin);
        }
    }
}
