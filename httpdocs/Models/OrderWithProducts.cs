using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class OrderWithProducts
    {
        public Order order { get; set; }
        public Dictionary<Product,int> products { get; set; }

        public OrderWithProducts()
        {

        }

        public OrderWithProducts(Order order, Dictionary<Product,int> products)
        {
            this.order = order;
            this.products = products;
        }
    }
}