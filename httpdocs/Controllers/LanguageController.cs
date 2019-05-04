using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AutoPartsMVC.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Change(string LA)
        {
            if (LA != null)
            {
                if (LA.Equals("bg"))
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("bg-BG");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("bg-BG");
                }

                if (LA.Equals("en"))
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                }
            }
            HttpCookie cookie = new HttpCookie("LNG");
            cookie.Value = LA;
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index","Home");
        }
    }
}