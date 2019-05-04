using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public double Price { get; set; }

        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int TimesBought { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        
        public string ProductImageName { get; set; }
        public double PriceEN { get; set; }

        [Index(IsUnique = true)]
        [StringLength(450)]
        public string NameEN { get; set; }

        [DataType(DataType.MultilineText)]
        public string DescriptionEN { get; set; }

        public Category Category { get; set; }
 
        public string urlName { get; set; }

        public Product()
        {

        }

        public Product(
                double Price,
                string Name,
                string Description,
                int TimesBought,
                int CategoryId,
                string ProductImageName,
                double PriceEN,
                string NameEN,
                string DescriptionEN,
                string urlName
            )
        {
            this.Price = Price;
            this.Name = Name;
            this.Description = Description;
            this.TimesBought = TimesBought;
            this.CategoryId = CategoryId;
            this.ProductImageName = ProductImageName;
            this.PriceEN = PriceEN;
            this.NameEN = NameEN;
            this.DescriptionEN = DescriptionEN;
            this.urlName = urlName;
        }
    }
}