using AutoPartsMVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AutoPartsMVC.Controllers
{
    public class CartController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        // GET: Cart
        public ActionResult Index()
        {
            var model = new cartViewModel();
            model.Whole = 0;
            var cart = new Dictionary<Product, int>();
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
            }
            if(userId.Equals(""))
            {
                if (HttpContext.Request.Cookies["_Tokenche"] != null)
                userId = HttpContext.Request.Cookies["_Tokenche"].Value;
            }
            var productsInCart = db.ProductsInCarts.Where(a => a.UserId.Equals(userId)).Select(a => a).ToList();

            foreach (var p in productsInCart)
            {
                var product = db.Products.Where(a => a.Id == p.ProductId).Select(a => a).FirstOrDefault();
                product = translateProducts(product);
                if (!cart.ContainsKey(product))
                    cart.Add(product, p.Quantity);
                else
                {
                    cart[product] += p.Quantity;
                }
                model.Whole += product.Price * p.Quantity;
            }
            model.Products = cart;
            return View(model);
        }
        public string GenerateToken(DateTime input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            DateTime dateTime = input;
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
        // GET: Cart
        public ActionResult AddProduct(int id, int quantity)
        {

            if (db.Products.Find(id) != null && quantity >= 1 && User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var product = db.ProductsInCarts.Where(a => a.ProductId == id && a.UserId == userId).FirstOrDefault();
                if (product != null)
                {
                    product.Quantity += quantity;
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    ProductsInCarts productInCart = new ProductsInCarts();
                    productInCart.ProductId = id;
                    productInCart.Quantity = quantity;
                    productInCart.UserId = User.Identity.GetUserId();
                    productInCart.CheckedOut = false;
                    db.ProductsInCarts.Add(productInCart);
                    db.SaveChanges();
                }
                return Json("Successfull added product in cart!", JsonRequestBehavior.AllowGet);
            }
            else if(!User.Identity.IsAuthenticated)
            {
                HttpCookie cookie = HttpContext.Request.Cookies["_Tokenche"];
                if (cookie == null)
                {
                    cookie = new HttpCookie("_Tokenche");
                    cookie.Value = GenerateToken(DateTime.Now);
                    HttpContext.Response.Cookies.Add(cookie);
                } 

                var product = db.ProductsInCarts.Where(a => a.ProductId == id && a.UserId == cookie.Value).FirstOrDefault();
                if (product != null)
                {
                    product.Quantity += quantity;
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    ProductsInCarts productInCart = new ProductsInCarts();
                    productInCart.ProductId = id;
                    productInCart.Quantity = quantity;
                    productInCart.UserId = cookie.Value;
                    productInCart.CheckedOut = false;
                    db.ProductsInCarts.Add(productInCart);
                    db.SaveChanges();
                }
                return Json("Successfull added product in cart!", JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Error, check quantity and product!", JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetTotalCount()
        {
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
            }
            else if(HttpContext.Request.Cookies["_Tokenche"]!=null)
            {
                userId = HttpContext.Request.Cookies["_Tokenche"].Value;
            }
            var t = db.ProductsInCarts
                .Where(a => a.UserId == userId)
                .Select(a => a.ProductId)
                .Count();
            double sum = 0;
            if (CultureInfo.CurrentCulture.ToString().Equals("bg"))
            {
                var t2 = db.ProductsInCarts
                    .Join(db.Products, a => a.ProductId, p => p.Id, (a, p) => new { a, p })
                    .Where(a => a.a.UserId == userId)
                    .Select(a => new { Price = a.a.Quantity * a.p.Price })
                    .ToList();
                sum = t2.Sum(b => b.Price);
            }
            else
            {
                var t2 = db.ProductsInCarts
                    .Join(db.Products, a => a.ProductId, p => p.Id, (a, p) => new { a, p })
                    .Where(a => a.a.UserId == userId)
                    .Select(a => new { Price = a.a.Quantity * a.p.PriceEN })
                    .ToList();
                sum = t2.Sum(b => b.Price);
            }
            return PartialView("~/Views/Shared/_CartProductCount.cshtml", new CartInfoView(t,sum));
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
