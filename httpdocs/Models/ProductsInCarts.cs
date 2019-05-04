using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class ProductsInCarts
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public bool CheckedOut { get; set; }
        public int OrderId { get; set; }
    }
}