using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoPartsMVC.Models;

namespace AutoPartsMVC.Controllers
{
    public class PromoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var result = new List<Product>();
            foreach (var item in db.Promos.ToList())
            {
                result.Add(db.Products.Where(a => a.Id == item.ProductId).FirstOrDefault());
            }
            return View(result);
        }

        public ActionResult Details(int? id)
        {
            if (!User.Identity.IsAuthenticated||!User.IsInRole("AppAdmin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promo promo = db.Promos.Find(id);
            if (promo == null)
            {
                return HttpNotFound();
            }
            return View(promo);
        }

        public ActionResult Create()
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId")] Promo promo)
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Promos.Add(promo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(promo);
        }
        

        public ActionResult Delete(int? id)
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Promo promo = db.Promos.Where(a => a.ProductId == id).FirstOrDefault();
            if (promo == null)
            {
                return HttpNotFound();
            }
            return View(promo);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Promo promo = db.Promos.Where(a=>a.ProductId == id).FirstOrDefault();
            db.Promos.Remove(promo);
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
