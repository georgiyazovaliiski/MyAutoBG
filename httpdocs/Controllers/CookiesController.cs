using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AutoPartsMVC.Controllers
{
    public class CookiesController : Controller
    {
        public ActionResult Check()
        {
            HttpCookie cookie = new HttpCookie("Cookie");
            cookie.Value = GenerateToken();
            cookie.Secure = true;
            cookie.HttpOnly = true;
            cookie.Domain = "localhost:49924";
            cookie.Expires = DateTime.Now.AddDays(30);
            Request.Cookies.Add(cookie);
            var a = this.ControllerContext.HttpContext.Request.Cookies;
            ArrayList result = new ArrayList();
            for (int i = 0; i < a.Count; i++)
            {
                result.Add(a[i]);
            }
            return View(result);
        }
        public PartialViewResult Index()
        {
            string a = "";
            if (HttpContext.Request.Cookies["_Tokenche"] != null)
            {
                a = HttpContext.Request.Cookies["_Tokenche"].Value;/*false;*/
            }
            return PartialView("~/Views/Cookies/_Index.cshtml", a);
        }
        public bool CheckForCookies()
        {
            bool result = false;
            string cookie = "There is no cookie!";
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("Cookie"))
            {
                cookie = "Yeah - Cookie: " + this.ControllerContext.HttpContext.Request.Cookies["Cookie"].Value;
                result = true;
            }
            else
            {
                result = false;
            }
            ViewData["Cookie"] = cookie;
            return result;
        }
        public ActionResult Create()
        {
            if (Request.Cookies.AllKeys.Contains("Cookie"))
            {
                Request.Cookies["Cookie"].Expires = DateTime.Now.AddDays(30);
                return RedirectToAction("Index", "Catalogue");
            }
            else
            {
                HttpCookie cookie = new HttpCookie("Cookie");
                cookie.Value = GenerateToken();
                cookie.Secure = true;
                cookie.HttpOnly = true;
                cookie.Expires = DateTime.Now.AddDays(30);
                Request.Cookies.Add(cookie);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Remove()
        {
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("Cookie"))
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["Cookie"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Catalogue");
            }
            return RedirectToAction("Index", "Home");
        }
        public string GenerateToken()
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            DateTime dateTime = DateTime.UtcNow;
            dateTime = dateTime.AddSeconds(-dateTime.Second);
            if (dateTime.Minute % 2 != 0)
                dateTime = dateTime.AddMinutes(1);

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(dateTime.ToString());
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public void CheckLanguage()
        {
            if (HttpContext.Response.Cookies["lng"] == null)
            {
                HttpContext.Response.Cookies.Add(new HttpCookie("lng"));
                HttpContext.Response.Cookies["lng"].Value = "BG";
            }
        }
    }
}