using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class Category
    {
        public Category(string Name, int SubCategoryId, string Type, string NameEN)
        {
            this.Name = Name;
            this.SubCategoryId = SubCategoryId;
            this.Type = Type;

            this.NameEN = NameEN;
        }
        public Category()
        {

        }
        public int Id { get; set; }
        

        [StringLength(450)]
        public string Name { get; set; }
        public int SubCategoryId { get; set; }
        public string Type { get; set; }
        
        [StringLength(450)]
        public string NameEN { get; set; }
        public string UrlName { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}