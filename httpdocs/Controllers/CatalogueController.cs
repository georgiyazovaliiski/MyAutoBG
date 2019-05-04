using AutoPartsMVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoPartsMVC.Controllers
{
    public class CatalogueController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Catalogue
        public ActionResult Index()
        {
            var products = new List<Product>();
            
                products = translateProducts(db.Products.OrderByDescending(a=>a.Id).Where(a=>a.CategoryId!=5).Take(9).ToList());

            return View(products);
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

        public ActionResult Product(string id)
        {
            var represent = new RepresentProduct();
            var product = db.Products.Where(a => a.urlName.Equals(id)).FirstOrDefault();
            var pictures = db.Pictures.Where(a => a.ProductId == product.Id).Select(a=>a.UrlPictureName).ToList();
            /*if(product == null)
            {
                product = db.Products.Where(a => a.NameEN == id).FirstOrDefault();
            }*/
            product = translateProducts(product);
            represent.Product = product;
            represent.Pictures = pictures;
            return View(represent);
        }
        public ActionResult Category(string id)
        {
            var catId = db.Categories.Where(a => a.UrlName.Trim().Equals(id)).Select(a => a.Id).FirstOrDefault();
            var products = new List<Product>();
            products = db.Products.Where(a => a.CategoryId == catId).OrderBy(a => a.Id).ToList();
            products = translateProducts(products);

            if (products.Count == 0)
            {
                RedirectToAction("Index", "Home");
            }
            return View(products);
        }
    }
}