using AutoPartsMVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoPartsMVC.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Title = "Начало";
            return View();
        }
        [HttpPost]
        public ActionResult Index([Bind(Include = "Price,Name,Description,Email,PhoneNumber,FirstName,LastName,Country,City")] ProductFromClient pfc, HttpPostedFileBase ProductImage)
        {
            pfc.ProductImageName = SaveImage(ProductImage);
            pfc.TimesBought = 0;
            pfc.Name = pfc.Name.Trim();

            pfc.urlName = pfc.Name.Replace(".", " ");
            pfc.urlName = pfc.urlName.Replace("/", " ");
            pfc.urlName = pfc.urlName.Replace("\"", " ");
            pfc.urlName = pfc.urlName.Replace("'", " ");
            pfc.urlName = pfc.urlName.Replace("\\", " ");
            pfc.urlName = pfc.urlName.Replace("%", " ");
            var words = pfc.urlName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            pfc.urlName = string.Join("-", words);
            pfc.CategoryId = 5;
            db.ProductsFromClients.Add(pfc);
            db.SaveChanges();
            ViewBag.Title = CultureInfo.CurrentCulture.ToString();
            return View();
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
    }
}