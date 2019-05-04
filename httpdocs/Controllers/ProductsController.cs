using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoPartsMVC.Models;
using System.IO;

namespace AutoPartsMVC.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Pictures(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ProductAndPictures pap = new ProductAndPictures();
            var picturesOfProduct = db.Pictures.Where(a => a.ProductId == id).ToList();
            if(picturesOfProduct.Count != 0)
            {
                pap.Pictures = picturesOfProduct;
            }
            pap.Product = db.Products.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();
            return View(pap);
        }
        
        public ActionResult RemovePictureFromProduct(int id)
        {
            Picture p = db.Pictures.Where(a => a.Id == id).FirstOrDefault();
            db.Pictures.Remove(p);
            var directoryPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/img/");

            if (System.IO.File.Exists(directoryPath + p.UrlPictureName))
            {
                try
                {
                    System.IO.File.Delete(directoryPath + p.UrlPictureName);
                }
                catch (System.IO.IOException e)
                {
                    throw new Exception(e.ToString());
                }
            }
            db.SaveChanges();
            return RedirectToAction("Pictures",new { id = p.ProductId });
        }
        [HttpGet]
        public ActionResult addPictureToProduct()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        [HttpPost]
        public ActionResult addPictureToProduct(int ProductId, HttpPostedFileBase PictureName)
        {
            var name = SaveImage(PictureName);
            Picture adding = new Picture(name.ToString(), ProductId);
            db.Pictures.Add(adding);
            db.SaveChanges();
            return RedirectToAction("addPictureToProduct");
        }

        public ActionResult ChangeUrlNames()
        {
            var allProducts = db.Products.Where(a => a.urlName == null).ToList();

            foreach (var product in allProducts)
            {
                product.urlName = product.NameEN.Replace(".", " ");
                product.urlName = product.urlName.Replace("/", " ");
                product.urlName = product.urlName.Replace("\"", " ");
                product.urlName = product.urlName.Replace("'", " ");
                product.urlName = product.urlName.Replace("\\", " ");
                product.urlName = product.urlName.Replace("%", " ");
                var words = product.urlName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                product.urlName = string.Join("-", words);

                db.Entry(product).State = EntityState.Modified;
                
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Products");
        }

        public ActionResult Index()
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(db.Products.ToList().OrderBy(a => a.Name));
            }
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
            List<Product> products = db.Products.Where(a=>a.CategoryId == id).ToList();
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        public ActionResult Category(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var products = db.Products.Where(a => a.CategoryId == id).ToList();
            if(products == null)
            {
                return HttpNotFound();
            }
            return View(products);
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
        public ActionResult Create([Bind(Include = "Id,Price,Name,Description,CategoryId,PriceEN,NameEN,DescriptionEN")] Product product, HttpPostedFileBase imageModel = null)
        {
            if (imageModel == null)
            {
                return RedirectToAction("Create");
            }
            if (product.Name.EndsWith(".") || product.Name.StartsWith("."))
            {
                ModelState.AddModelError("Name", "Името не трябва да започва или свършва с точка (.)");
            }
            if (product.NameEN.EndsWith(".") || product.NameEN.StartsWith("."))
            {
                ModelState.AddModelError("NameEN", "Името не трябва да започва или свършва с точка (.)");
            }
            if (product.Description.Length < 30)
            {
                ModelState.AddModelError("Description", "Описанието трябва да бъде над 30 символа!");
            }
            if (product.DescriptionEN.Length < 30)
            {
                ModelState.AddModelError("DescriptionEN", "Описанието трябва да бъде над 30 символа!");
            }

            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            product.ProductImageName = SaveImage(imageModel);
            product.TimesBought = 0;
            product.Name = product.Name.Trim();
            product.NameEN = product.NameEN.Trim();

            product.urlName = product.NameEN.Replace(".", " ");
            product.urlName = product.urlName.Replace("/", " ");
            product.urlName = product.urlName.Replace("\"", " ");
            product.urlName = product.urlName.Replace("'", " ");
            product.urlName = product.urlName.Replace("\\", " ");
            product.urlName = product.urlName.Replace("%", " ");
            var words = product.urlName.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries).ToArray();
            product.urlName = string.Join("-",words);

            if (ModelState.IsValid && db.Products.Where(a=>a.Name == product.Name || a.NameEN == product.NameEN || a.Name == product.NameEN || a.NameEN == product.Name).FirstOrDefault() == null)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        private String SaveImage(HttpPostedFileBase imageModel)
        {
            var fileName = CreateFileName(imageModel.FileName);

            var path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
            imageModel.SaveAs(path);
            return fileName;
        }
        private String EditImage(HttpPostedFileBase imageModel, string ProductImageName)
        {

            var fileName = ProductImageName;

            var path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
            imageModel.SaveAs(path);
            return fileName;
        }

        private string CreateFileName(string entryFileName)
        {
            DateTime imageDateUpload = DateTime.Now;
            String imageDateName =
                imageDateUpload.Year.ToString() +
                imageDateUpload.Month.ToString() +
                imageDateUpload.Day.ToString() +
                imageDateUpload.Hour.ToString() +
                imageDateUpload.Minute.ToString() +
                imageDateUpload.Second.ToString();

            var fileName = Path.GetFileName(entryFileName.Trim());
            fileName = fileName.Replace(" ", String.Empty);
            fileName = imageDateName + "-" + fileName;
            return fileName;
        }

        public ActionResult showCategories()
        {
            List<CategoryListView> clv = new List<CategoryListView>();

            List<Category> categories = db.Categories
                .Where(a => a.SubCategoryId != 0 
                            && (a.SubCategoryId != 1 
                            && a.SubCategoryId != 2 
                            && a.SubCategoryId != 3 
                            && a.SubCategoryId != 4 
                            && a.SubCategoryId != 5))
                .OrderBy(a=>a.SubCategoryId)
                .ToList();
            foreach (var category in categories)
            {
                var highcategory = db.Categories
                    .Where(a => a.Id == category.SubCategoryId)
                    .Select(a=> new { Name = a.Name, highcategoryId = a.SubCategoryId })
                    .FirstOrDefault();

                var higherCategory = db.Categories
                    .Where(a => a.Id == highcategory.highcategoryId)
                    .Select(a => a.Name)
                    .FirstOrDefault();

                clv.Add(new CategoryListView(category,highcategory.Name, higherCategory.ToString()));
            }

            return Json(clv, JsonRequestBehavior.AllowGet);
        }
        public ActionResult showAllCategories()
        {
            List<CategoryListView> clv = new List<CategoryListView>();

            List<Category> categories = db.Categories
                .Where(a => a.SubCategoryId == 0 || 
                            (a.SubCategoryId == 1 || a.SubCategoryId == 2 ||
                            a.SubCategoryId == 3 || a.SubCategoryId == 4 || 
                            a.SubCategoryId == 5))
                .Select(a => a)
                .ToList();
            foreach (var category in categories)
            {
                var highcategory = db.Categories
                    .Where(a => a.Id == category.SubCategoryId)
                    .Select(a => a.Name)
                    .FirstOrDefault();
                if(highcategory == null)
                 highcategory = "";

                clv.Add(new CategoryListView(category, highcategory , ""));
            }

            return Json(clv, JsonRequestBehavior.AllowGet);
        }
        public ActionResult showNonPromoProductList()
        {
            var firstly = db.Products.ToList();
            var promos = db.Promos.ToList();
            foreach (var p in promos)
            {
                firstly.Remove(firstly.Where(a => a.Id == p.ProductId).FirstOrDefault());
            }
            return Json(firstly, JsonRequestBehavior.AllowGet);
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
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Price,Name,Description,TimesBought,CategoryId,ProductImageName,PriceEN,NameEN,DescriptionEN")] Product product, HttpPostedFileBase image = null)
        {
            if (product.Name.EndsWith(".") || product.Name.StartsWith("."))
            {
                ModelState.AddModelError("Name", "Името не трябва да започва или свършва с точка (.)");
            }
            if (product.NameEN.EndsWith(".") || product.NameEN.StartsWith("."))
            {
                ModelState.AddModelError("NameEN", "Името не трябва да започва или свършва с точка (.)");
            }
            if (image != null)
            product.ProductImageName = EditImage(image, product.ProductImageName);

            product.Name = product.Name.Trim();
            product.NameEN = product.NameEN.Trim();
            product.DescriptionEN = product.DescriptionEN.Trim();
            product.Description = product.Description.Trim();

            product.urlName = product.NameEN.Replace(".", " ");
            product.urlName = product.urlName.Replace("/", " ");
            product.urlName = product.urlName.Replace("\"", " ");
            product.urlName = product.urlName.Replace("'", " ");
            product.urlName = product.urlName.Replace("\\", " ");
            product.urlName = product.urlName.Replace("%", " ");
            var words = product.urlName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            product.urlName = string.Join("-", words);

            if (product.Description.Length < 30)
            {
                ModelState.AddModelError("Description", "Описанието трябва да бъде над 30 символа!");
            }
            if (product.DescriptionEN.Length < 30)
            {
                ModelState.AddModelError("DescriptionEN", "Описанието трябва да бъде над 30 символа!");
            }

            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
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
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();

            if (db.Promos.Where(a => a.ProductId == product.Id).FirstOrDefault() != null)
            {
                db.Promos.Remove(db.Promos.Where(a => a.ProductId == product.Id).FirstOrDefault());
                db.SaveChanges();
            }

            if (db.SpecialOffers.Where(a => a.ProductId == product.Id).FirstOrDefault() != null)
            {
                db.SpecialOffers.Remove(db.SpecialOffers.Where(a => a.ProductId == product.Id).FirstOrDefault());
                db.SaveChanges();
            }

            if (db.ProductsInCarts.Where(a => a.ProductId == product.Id).FirstOrDefault() != null)
            {
                db.ProductsInCarts.Remove(db.ProductsInCarts.Where(a => a.ProductId == product.Id).FirstOrDefault());
                db.SaveChanges();
            }

            if (db.OrdersProductsQuantities.Where(a => a.ProductId == product.Id).FirstOrDefault() != null)
            {
                db.OrdersProductsQuantities.Remove(db.OrdersProductsQuantities.Where(a => a.ProductId == product.Id).FirstOrDefault());
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        
        public ActionResult FromUsers(String Type)
        {
            if (!User.IsInRole("AppAdmin") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Type.Equals("WaitingApproval"))
                {
                    ViewBag.ApprovedOrNot = "Чакащи одобрение";
                    return View(db.ProductsFromClients.Where(a => a.Approved == false).ToList().OrderBy(a => a.Name));
                }
                else
                {
                    ViewBag.ApprovedOrNot = "Одобрени";
                    return View(db.ProductsFromClients.Where(a => a.Approved == true).ToList().OrderBy(a => a.Name));
                }
            }
        }
        public ActionResult Approve(int Id)
        {
            var product = db.ProductsFromClients.Where(a => a.Id == Id).FirstOrDefault();
            product.Approved = true;
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("FromUsers", "Products", new { Type = "Approved" });
            }
            return RedirectToAction("FromUsers", "Products", new { Type = "WaitingApproval" });
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
