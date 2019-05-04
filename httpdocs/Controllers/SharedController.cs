using AutoPartsMVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoPartsMVC.Controllers
{
    public class SharedController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public PartialViewResult CatalogueMenu()
        {
            var CatalogueList = db.Categories.Where(a => a.SubCategoryId == 0).ToList();
            CatalogueList = changeCategoriesByCulture(CatalogueList);
            return PartialView("~/Views/Shared/_CatalogueMenu.cshtml", CatalogueList);
        }
        public List<Category> changeCategoriesByCulture(List<Category> CatalogueList)
        {
            if (CultureInfo.CurrentCulture.ToString().Equals("en"))
                for (int i = 0; i < CatalogueList.Count; i++)
                {
                    CatalogueList[i].Name = CatalogueList[i].NameEN;
                }
            return CatalogueList;
        }
        public PartialViewResult SubCategories(int upperCategory)
        {
            if (upperCategory == 0)
            {
                return PartialView();
            }
            var CatalogueList = db.Categories.Where(a => a.SubCategoryId == upperCategory).ToList();
            CatalogueList = changeCategoriesByCulture(CatalogueList);
            return PartialView("~/Views/Shared/_upperCategories.cshtml", CatalogueList.OrderByDescending(a => a.Name));
        }
        public ActionResult SubSubCategories(int id)
        {
            if (id == 0)
            {
                return PartialView();
            }
            var CatalogueList = db.Categories.Where(a => a.SubCategoryId == id).ToList();
            CatalogueList = changeCategoriesByCulture(CatalogueList);
            return Json(CatalogueList, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult Promoes()
        {
            var promoIds = db.Promos.Select(a => a.ProductId).ToList();
            var promoProds = new List<Product>();
            foreach (var p in promoIds)
            {
                var product = db.Products.Where(a => a.Id == p).Select(a => a).FirstOrDefault();
                promoProds.Add(product);
            }
            return PartialView("~/Views/Shared/_Promoes.cshtml", translateProducts(promoProds));
        }
        public PartialViewResult SpecialOffers()
        {
            var promoProds = new List<Product>();
            promoProds.AddRange(db.ProductsFromClients.Where(a => a.CategoryId == 5 && a.Approved == true).OrderByDescending(a=>a.Id).Take(3).ToList());
            return PartialView("~/Views/Shared/_SpecialOffers.cshtml", promoProds);
        }
        public ActionResult DeleteProductInCart(int productId)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                userId = HttpContext.Request.Cookies["_Tokenche"].Value;
            }
            var items = db.ProductsInCarts.Where(a => a.ProductId == productId && a.UserId == userId).ToList();
            foreach (var i in items)
            {
                db.ProductsInCarts.Remove(i);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult EditProduct(int productId, int val)
        {

            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                userId = HttpContext.Request.Cookies["_Tokenche"].Value;
            }
            var item = db.ProductsInCarts.Where(a => a.ProductId == productId && a.UserId == userId).FirstOrDefault();
            item.Quantity = val;
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Edit Successfull",JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult SharedBottomMenu()
        {
            var mainCategories = db.Categories.Where(a => a.SubCategoryId == 0).ToList();
            mainCategories = changeCategoriesByCulture(mainCategories);
            var subCategories = new Dictionary<int,List<Category>>();
            var subsubCategories = new Dictionary<int, List<Category>>();
            foreach (var c in mainCategories)
            {
                var partialSubs = db.Categories.Where(a => a.SubCategoryId == c.Id).OrderByDescending(a => a.Name).ToList();
                partialSubs = changeCategoriesByCulture(partialSubs);
                subCategories.Add(c.Id, partialSubs);
                foreach(var p in partialSubs)
                {
                    var partialSubSubs = db.Categories.Where(a => a.SubCategoryId == p.Id).ToList();
                    partialSubSubs = changeCategoriesByCulture(partialSubSubs);
                    subsubCategories.Add(p.Id,partialSubSubs);
                }
            }

            var catalogueWhole = new BottomMenuModel();
            catalogueWhole.mainCategories = mainCategories;
            catalogueWhole.subCategories = subCategories;
            catalogueWhole.subsubCategories = subsubCategories;
            return PartialView("~/Views/Shared/_SharedBottomMenu.cshtml", catalogueWhole);
        }

        private List<Product> translateProducts(List<Product> products)
        {
            if (CultureInfo.CurrentCulture.ToString().Equals("en"))
                for (int i = 0; i < products.Count; i++)
                {
                    products[i].Name = products[i].NameEN;
                    products[i].Description = products[i].DescriptionEN;
                    products[i].Price = products[i].PriceEN;
                }
            return products;
        }
        private Product translateProducts(Product product)
        {
            if (CultureInfo.CurrentCulture.ToString().Equals("en"))
            {
                product.Name = product.NameEN;
                product.Description = product.DescriptionEN;
                product.Price = product.PriceEN;
            }
            return product;
        }
    }
}