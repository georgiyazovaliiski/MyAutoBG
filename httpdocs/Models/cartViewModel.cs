using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class cartViewModel
    {
        public Dictionary<Product, int> Products { get; set; }

        public double Whole { get; set; }
    }
}