using AutoPartsMVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoPartsMVC.Controllers
{
    [HandleError(View="~/Views/Errors/Index")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CheckOut()
        {
            string userId = "";
            if(User.Identity.IsAuthenticated)
                userId = User.Identity.GetUserId();
            else if(Request.Cookies["_Tokenche"] != null)
            {
                userId = Request.Cookies["_Tokenche"].Value;
            }
            else
            {
                return RedirectToAction("Index", "Cart");
            }
            var products = db.ProductsInCarts.Where(a => a.UserId.Equals(userId)).Select(a => a.Id).ToList();
            if (products.Count < 1)
            {
                return RedirectToAction("Index", "Cart");
            }
            return View();
        }
        [HttpPost]
        public ActionResult CheckOut(string name,
            string lastName,
            string phone,
            int addressType,
            bool termsAndConditionsAgreement,
            string contactAddress,
            string address = "")
        {
            if (termsAndConditionsAgreement != true)
            {
                throw new Exception("Не сте се съгласили с общите условия!");
            }
            if (name == null || phone == null || address == null)
            {
                throw new Exception("Не сте попълнили всички полета!");
            }
                var order = AddOrderToDb(name, lastName, phone, addressType, termsAndConditionsAgreement, contactAddress, address);
                AddProductsToOrder(order);

                return RedirectToAction("Success", "Orders");
        }

        private void AddProductsToOrder(int order)
        {
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
            }
            else
            {
                userId = HttpContext.Request.Cookies["_Tokenche"].Value;
            }
            var col = db.ProductsInCarts.Where(a => a.UserId == userId).ToList();
            if(col.Count == 0)
            {
                throw new HttpUnhandledException("Няма въведени продукти");
            }
            foreach(var c in col)
            {
                c.CheckedOut = true;
                c.OrderId = order;
                db.ProductsInCarts.Remove(c);
                var opq = new OrderProductQuantity(c.OrderId,c.ProductId,c.Quantity);
                db.OrdersProductsQuantities.Add(opq);
                db.SaveChanges();
            }
        }

        public ActionResult Success()
        {
            List<string> model = new List<string>() { "Поздравления! Вие изпратихте своята поръчка! Очаквайте скорошно обаждане от нас!" };
            return View(model);
        }

        public ActionResult YourOrders()
        {
            if (User.Identity.IsAuthenticated)
            {
                Dictionary<Order, List<Product>> result = new Dictionary<Order, List<Product>>();
                var a = GetOrdersById();
                foreach (var order in a)
                {
                    var b = getOrderProducts(order);
                    result.Add(order, b);
                }
                return View(result);
            }
            else return RedirectToAction("Index", "Home");
        }

        public ActionResult WaitingApprovalOrders()
        {
            if (isAdmin())
            {
                List<Order> a = db.Orders.Where(b => b.approved == false).ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult WaitingOrders()
        {
            if (isAdmin())
            {
                List<Order> a = db.Orders.Where(b => b.approved == true && b.DateOfOrderFinish==null).ToList();
            return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult DoneOrders()
            {
                if (isAdmin())
            {
                List<Order> a = db.Orders.Where(b => b.approved == true && b.DateOfOrderFinish != null).ToList();
                    return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }

        public ActionResult Approve(int id)
        {
            var approvingOrder = db.Orders.Where(a => a.Id == id).Select(a => a).FirstOrDefault();
            approvingOrder.approved = true;
            db.Entry(approvingOrder).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("WaitingApprovalOrders", "Orders");
        }
        public ActionResult DeleteOrder(int id)
        {
            var deleting = db.Orders.Where(a => a.Id == id).FirstOrDefault();
            db.Orders.Remove(deleting);
            db.SaveChanges();
            return RedirectToAction("WaitingApprovalOrders", "Orders");
        }
        public ActionResult FinishOrder(int id)
        {
            var approvingOrder = db.Orders.Where(a => a.Id == id).Select(a => a).FirstOrDefault();
            var productsFromOrder = db.OrdersProductsQuantities.Where(a => a.OrderId == id).ToList();
            foreach (var productAndQuantity in productsFromOrder)
            {
                var product = db.Products.Where(a => a.Id == productAndQuantity.ProductId).FirstOrDefault();
                product.TimesBought += productAndQuantity.Quantity;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
            }
            approvingOrder.DateOfOrderFinish = DateTime.Now;
            db.Entry(approvingOrder).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("WaitingOrders", "Orders");
        }
        public ActionResult DeleteWaitingOrder(int id)
        {
            var deleting = db.Orders.Where(a => a.Id == id).FirstOrDefault();
            db.Orders.Remove(deleting);
            db.SaveChanges();
            return RedirectToAction("WaitingOrders", "Orders");
        }/*
        public ActionResult Details()
        {
            return View();
        }*/
        
        public ActionResult Details(int id)
        {
            var realOrder = db.Orders.Find(id);
            var order = db.OrdersProductsQuantities.Where(a => a.OrderId == id).Select(a => a).ToList();
            var productsInOrder = new Dictionary<Product,int>();
            foreach (var o in order)
            {
                var addingProduct = db.Products.Find(o.ProductId);
                if(!productsInOrder.Keys.Contains(addingProduct))
                productsInOrder.Add(addingProduct,o.Quantity);
                else
                {
                    productsInOrder[addingProduct] += o.Quantity;
                }
            }
            OrderWithProducts owp = new OrderWithProducts(realOrder, productsInOrder);
            return View(owp);
        }
        
        private bool isAdmin()
        {
            if (User.IsInRole("AppAdmin")) return true;
            else return false;
        }

        private List<Order> GetOrdersById()
        {
            var userId = User.Identity.GetUserId();
            return db.Orders.Where(a => a.UserId == userId).ToList();
        }

        private List<Product> getOrderProducts(Order order)
        {
            var productsFromOrder = db.OrdersProductsQuantities.Where(a => a.OrderId == order.Id).Select(a=>a.ProductId).ToList();
            var result = new List<Product>();
            foreach(var p in productsFromOrder)
            {
                result.Add(db.Products.Find(p));
            }
            return result;
        }

        private int AddOrderToDb(string name,
            string lastName,
            string phone,
            int addressType,
            bool termsAndConditionsAgreement,
            string contactAddress,
            string address)
        {
            var userId = "";
            if (User.Identity.IsAuthenticated) { 
                userId = User.Identity.GetUserId();
            }
            else
            {
                userId = HttpContext.Request.Cookies["_Tokenche"].Value;
            }
            var col = db.ProductsInCarts.Where(a => a.UserId == userId).ToList();
            if (col.Count == 0)
            {
                throw new HttpUnhandledException("Няма въведени продукти");
            }
            var order = new Order(userId, name, lastName, phone,addressType, DateTime.Now,termsAndConditionsAgreement, contactAddress, address);
                db.Orders.Add(order);
                db.SaveChanges();
                return db.Orders.Find(order.Id).Id;
        }

        private void validate(string paymentMethod)
        {
            if (paymentMethod.Equals("cash") || paymentMethod.Equals("econt"))
            {

            }
            else
            {
                RedirectToAction("Index", "Cart");
            }
        }
    }
}