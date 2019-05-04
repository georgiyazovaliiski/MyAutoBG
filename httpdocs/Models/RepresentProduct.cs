using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class RepresentProduct
    {
        public Product Product { get; set; }
        public List<string> Pictures { get; set; }
    }
}