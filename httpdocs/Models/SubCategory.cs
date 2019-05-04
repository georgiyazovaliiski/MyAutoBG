using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UpperCategoryId { get; set; }
        public string CategoryImageName { get; set; }
    }
}