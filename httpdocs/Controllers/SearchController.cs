using AutoPartsMVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoPartsMVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Products(string searchTerm)
        {
            var result = db.Products.Where(a => a.Name.Contains(searchTerm) || a.Description.Contains(searchTerm) || a.NameEN.Contains(searchTerm) || a.DescriptionEN.Contains(searchTerm)).ToList();
            return View(translateProducts(result));
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
    }
}