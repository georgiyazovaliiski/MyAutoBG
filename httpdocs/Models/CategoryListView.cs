using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class CategoryListView
    {
        public CategoryListView()
        {

        }
        public CategoryListView(Category category, string highcategory, string higherCategoryName)
        {
            this.category = category;
            this.highcategoryName = highcategory;
            this.highercategoryName = higherCategoryName;
        }
        public Category category { get; set; }
        public string highcategoryName { get; set; }
        public string highercategoryName { get; set; }
    }
}