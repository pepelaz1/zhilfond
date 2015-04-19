using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Interfaces;
using DAL.Repositories;

namespace UI.Controllers
{
    public class MainController : Controller
    {
        static readonly IUsersRepository _repository = new UsersRepository();

        //
        // GET: /Main/

        public ActionResult Index()
        {
            var o = HttpContext.Session["token"];
            if (o != null)
            {
                return View();
            }
            return RedirectToAction("Index", "AccessDenied");
        }

        public ActionResult Constructor()
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

        public ActionResult Dicts()
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

        public ActionResult Settings()
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
    }
}
