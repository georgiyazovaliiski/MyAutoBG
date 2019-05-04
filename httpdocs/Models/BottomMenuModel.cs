using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class BottomMenuModel
    {
        public List<Category> mainCategories { get; set; }
        public Dictionary<int, List<Category>> subCategories { get; set; }
        public Dictionary<int,List<Category>> subsubCategories { get; set; }
    }
}