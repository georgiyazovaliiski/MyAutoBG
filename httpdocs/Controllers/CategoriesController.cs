using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoPartsMVC.Models;
using System.Text.RegularExpressions;

namespace AutoPartsMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.Categories.Where(a=>a.SubCategoryId != 0).ToList());
        }

        // GET: Categories/Details/5
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
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
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
        public ActionResult Create([Bind(Include = "Id,Name,SubCategoryId,NameEN")] Category category)
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            category.Type = "Primary";
            if (category.SubCategoryId != 0)
            {
                var upperCatId = category.SubCategoryId;
                var upperCategory = db.Categories.Where(a => a.Id == upperCatId).FirstOrDefault();
                if (upperCategory.Type.Equals("Primary"))
                {
                    upperCategory.Type = "Secondary";
                    db.Entry(upperCategory).State = EntityState.Modified;
                }
            }
            category.Name = category.Name.Trim();
            category.NameEN = category.NameEN.Trim();
            category.UrlName = category.NameEN.Trim();
            category.UrlName = Regex.Replace(category.UrlName, @"[\s]", string.Empty);
            category.UrlName = category.UrlName.ToLower();


            var higherCat = db.Categories
                .Where(a => a.Id == category.SubCategoryId)
                .Select(a => new { Name = a.UrlName, HighId = a.SubCategoryId })
                .FirstOrDefault();
            category.UrlName += "-" + higherCat.Name;
            string highestCat = db.Categories
                .Where(a => a.Id == higherCat.HighId)
                .Select(a => a.UrlName)
                .FirstOrDefault();

            if (highestCat != null)
            {
                category.UrlName += "-" + highestCat;
            }

            if(db.Categories.Where(a=>a.UrlName == category.UrlName).ToList().Count > 0)
            {
                category.UrlName += (db.Categories.Where(a => a.UrlName.StartsWith(category.UrlName)).ToList().Count + 1).ToString();
            }

            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();


                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
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
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,SubCategoryId,Type,NameEN,UrlName")] Category category)
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
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
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List<Category> RemainingCategories = db.Categories.Where(a => a.SubCategoryId == id).ToList();
            foreach (var item in RemainingCategories)
            {
                var middleCategories = db.Categories.Where(a=>a.SubCategoryId == item.Id).ToList();
                foreach (var item2 in middleCategories)
                {
                    var deletingProds = db.Products.Where(a => a.CategoryId == item2.Id).ToList();
                    foreach (var p in deletingProds)
                    {
                        db.Products.Remove(p);
                        db.SaveChanges();
                    }
                    db.Categories.Remove(item2);
                    db.SaveChanges();
                }
                db.Categories.Remove(item);
                db.SaveChanges();
            }
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Category category = db.Categories.Find(id);
            var upperCategory = category.SubCategoryId;
            db.Categories.Remove(category);
            db.SaveChanges();
            var upperCat = db.Categories.Where(a => a.SubCategoryId == upperCategory).ToList();
            if (upperCat.Count == 0)
            {
                var removingCat = db.Categories.Where(a => a.Id == upperCategory).FirstOrDefault();
                if (removingCat != null)
                {
                    removingCat.Type = "Primary";
                    db.Entry(removingCat).State = EntityState.Modified;
                }
            }
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
