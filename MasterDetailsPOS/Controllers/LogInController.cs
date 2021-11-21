using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MasterDetailsPOS.Models;

namespace MasterDetailsPOS.Controllers
{
    public class LogInController : Controller
    {
        [HttpGet]
        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserLogin(Login login)
        {
            using (MasterDetailsPOSEntities db = new MasterDetailsPOSEntities())
            {
                var valid = db.Logins.Any(a => a.UserName == login.UserName && a.Password == login.Password);
                if (valid)
                {
                    FormsAuthentication.SetAuthCookie(login.UserName,false);
                    return Redirect("/Dashboard/Index");
                }
                return View(login);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/LogIn/UserLogin");
        }
    }
}