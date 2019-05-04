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
    public class SpecialOffersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.SpecialOffers.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialOffer specialOffer = db.SpecialOffers.Find(id);
            if (specialOffer == null)
            {
                return HttpNotFound();
            }
            return View(specialOffer);
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
        public ActionResult Create([Bind(Include = "Id,ProductId")] SpecialOffer specialOffer)
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.SpecialOffers.Add(specialOffer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(specialOffer);
        }

        public ActionResult Edit(int? id)
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialOffer specialOffer = db.SpecialOffers.Find(id);
            if (specialOffer == null)
            {
                return HttpNotFound();
            }
            return View(specialOffer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId")] SpecialOffer specialOffer)
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(specialOffer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(specialOffer);
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
            SpecialOffer specialOffer = db.SpecialOffers.Find(id);
            if (specialOffer == null)
            {
                return HttpNotFound();
            }
            return View(specialOffer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            SpecialOffer specialOffer = db.SpecialOffers.Find(id);
            db.SpecialOffers.Remove(specialOffer);
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
