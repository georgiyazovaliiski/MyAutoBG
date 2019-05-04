using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class ProductAndPictures
    {
        public String Product { get; set; }
        public List<Picture> Pictures { get; set; }

        public ProductAndPictures()
        {
            this.Pictures = new List<Picture>();
        }
    }
}