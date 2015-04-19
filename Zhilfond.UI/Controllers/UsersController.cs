using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using UI.Filters;
using DAL.Interfaces;
using DAL.Repositories;

namespace UI.Controllers
{
    public class UsersController : Controller
    {
        static readonly IUsersRepository _repository = new UsersRepository();

        public ActionResult Index()
        {
            var o = HttpContext.Session["token"];
            if (o != null)
            {
                DAL.Models.UserV u = _repository.GetVByToken(o.ToString());
                if (u.Role == "Администраторы")
                    return View();
            }
            return RedirectToAction("Index", "AccessDenied");
        }

        public ActionResult ChangePassword()
        {
            var o = HttpContext.Session["token"];
            if (o != null)
            {
               return View();
            }
            return RedirectToAction("Index", "AccessDenied");
        }

        public ActionResult Logoff()
        {
            System.Web.HttpContext.Current.Session["token"] = null;
            return this.RedirectToAction("Index","Home");
        }
    }
}
