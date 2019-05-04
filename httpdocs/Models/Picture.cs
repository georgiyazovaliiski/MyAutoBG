using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string UrlPictureName { get; set; }
        public int ProductId { get; set; }
        public Picture()
        {

        }
        public Picture(string UrlPictureName, int ProductId)
        {
            this.UrlPictureName = UrlPictureName;
            this.ProductId = ProductId;
        }
    }
}